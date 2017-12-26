using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperWebSocket;
using System.Configuration;
using ZentCloud.BLLJIMP.Model;


namespace WebSocketConsole
{

    class Program
    {

        static SuperWebSocket.WebSocketServer appServer = new WebSocketServer();
        static void Main(string[] args)
        {

            Console.WriteLine("按任意键启动 WebSocketServer!");
            Console.ReadKey();
            Console.WriteLine();

            //Setup the appServer
            if (!appServer.Setup(2012)) //Setup with listening port
            {
                Console.WriteLine("设置端口失败");
                Console.ReadKey();
                return;
            }
            appServer.NewSessionConnected += NewSessionConnected;
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(NewMessageReceived);
            appServer.SessionClosed += SessionClosed;
            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("启动失败");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("服务已经成功启动, 输入'q' 停止服务");
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            //Stop the appServer
            appServer.Stop();
            Console.WriteLine();
            Console.WriteLine("服务已经停止");
            Console.ReadKey();
        }

        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="session"></param>
        static void NewSessionConnected(WebSocketSession session)
        {
            try
            {


                ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
                ZentCloud.BLLJIMP.BLLLiveChat bllLive = new ZentCloud.BLLJIMP.BLLLiveChat();
                ZentCloud.BLLJIMP.BLLWebSite bllWebsite = new ZentCloud.BLLJIMP.BLLWebSite();
                #region 用户接入
                if (session.Path.Contains("client"))
                {
                    // ws = new window[support]('ws://localhost:2012/client/userautoid');
                    string idString = session.Path.Split('/')[2];
                    Console.WriteLine(string.Format("用户接入:用户ID:{0} {1}", idString, DateTime.Now.ToString()));
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    LiveChatRoom room = bllUser.Get<LiveChatRoom>(string.Format("RoomId={0}", userInfo.AutoID));
                    #region 第一次连接
                    if (room == null)
                    {

                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                        try
                        {

                            //创建聊天室
                            room = new LiveChatRoom();
                            room.RoomId = userInfo.AutoID.ToString();
                            room.CreateUserAutoId = userInfo.AutoID.ToString();
                            room.InsertDate = DateTime.Now;
                            room.WebsiteOwner = userInfo.WebsiteOwner;
                            room.UpdateTime = DateTime.Now;
                            room.UserIsOnLine = 1;
                            room.UserShowName = bllUser.GetUserDispalyName(userInfo);
                            if (string.IsNullOrEmpty(room.UserShowName))
                            {
                                room.UserShowName = "用户" + userInfo.AutoID.ToString();
                            }
                            room.UserHeadImg = bllUser.GetUserDispalyAvatar(userInfo);
                            if (bllUser.Add(room, tran))
                            {
                                LiveChatRoomUser roomUser = new LiveChatRoomUser();
                                roomUser.RoomId = room.RoomId;
                                roomUser.InsertDate = DateTime.Now;
                                roomUser.UserAutoId = userInfo.AutoID.ToString();
                                roomUser.UserType = "0";
                                roomUser.SocketSessionId = session.SessionID;
                                if (bllUser.Add(roomUser, tran))
                                {
                                    tran.Commit();
                                }

                            }



                        }
                        catch (Exception ex)
                        {

                            tran.Rollback();
                            Console.WriteLine(ex.ToString());

                        }

                    }
                    #endregion

                    else
                    {
                        room.UserIsOnLine = 1;
                        room.UserShowName = bllUser.GetUserDispalyName(userInfo);
                        room.UserHeadImg = bllUser.GetUserDispalyAvatar(userInfo);
                        if (string.IsNullOrEmpty(room.UserShowName))
                        {
                            room.UserShowName = "用户" + userInfo.AutoID.ToString();
                        }
                        bllUser.Update(room);
                        bllUser.Update(new LiveChatRoomUser(), string.Format("SocketSessionId='{0}'", session.SessionID), string.Format("RoomId={0} And UserAutoId='{1}' And UserType=0", userInfo.AutoID, userInfo.AutoID));


                    }
                    //bllUser.Update(userInfo, string.Format("IsOnLine=1"), string.Format("AutoId={0}", userInfo.AutoID));

                    List<LiveChatRoomUser> roomUserList = bllUser.GetList<LiveChatRoomUser>(string.Format("RoomId={0} And UserType=0", idString));

                    foreach (var item in roomUserList)
                    {
                        var sess = appServer.GetSessionByID(item.SocketSessionId);
                        #region 客服在线自动回复
                        if (sess != null)
                        {
                            if (bllLive.IsKefuOnLine(room.WebsiteOwner) && item.UserType == "0")//客服在线,自动回复
                            {
                                var companyConfig = bllWebsite.Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", room.WebsiteOwner));
                                if (companyConfig != null && (!string.IsNullOrEmpty(companyConfig.KefuOnLineReply)))
                                {
                                    Message respAutoReply = new Message();
                                    respAutoReply.message = companyConfig.KefuOnLineReply;
                                    respAutoReply.message_type = "text";
                                    respAutoReply.send_user_head_img = "/img/icons/kefu.png";
                                    respAutoReply.send_user_name = "";
                                    respAutoReply.send_user_type = "1";
                                    respAutoReply.send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    string respAutoReplyJson = ZentCloud.Common.JSONHelper.ObjectToJson(respAutoReply);
                                    sess.TrySend(respAutoReplyJson);


                                }
                            }






                        }
                        #endregion
                    }






                }


                #endregion

                #region 客服接入
                if (session.Path.Contains("server"))
                {
                    // ws = new window[support]('ws://localhost:2012/server/userautoid/roomid');
                    string idString = session.Path.Split('/')[2];
                    string roomId = session.Path.Split('/')[3];
                    LiveChatRoom room = bllUser.Get<LiveChatRoom>(string.Format("RoomId={0}", roomId));
                    Console.WriteLine(string.Format("客服接入:客服ID:{0}房间号:{1} {2}", idString, roomId, DateTime.Now.ToString()));
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    if (bllUser.GetCount<LiveChatRoomUser>(string.Format("RoomId={0} And UserAutoId='{1}' And UserType=1", roomId, userInfo.AutoID)) == 0)
                    {
                        LiveChatRoomUser roomUser = new LiveChatRoomUser();
                        roomUser.RoomId = roomId;
                        roomUser.InsertDate = DateTime.Now;
                        roomUser.UserAutoId = userInfo.AutoID.ToString();
                        roomUser.UserType = "1";
                        roomUser.SocketSessionId = session.SessionID;
                        bllUser.Add(roomUser);
                    }
                    else
                    {
                        bllUser.Update(new LiveChatRoomUser(), string.Format("SocketSessionId='{0}'", session.SessionID), string.Format("RoomId={0} And UserAutoId='{1}' And UserType=1", roomId, userInfo.AutoID));
                    }
                    room.IsKefuJoin = 1;
                    bllUser.Update(room);






                }
                #endregion

                #region 主页接入
                if (session.Path.Contains("index"))
                {
                    // ws = new window[support]('ws://localhost:2012/index/userautoid');
                    string idString = session.Path.Split('/')[2];
                    var userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    //string roomId = session.Path.Split('/')[3];
                    Console.WriteLine(string.Format("客服接入首页:客服ID:{0} {1}", idString, DateTime.Now.ToString()));
                    if (bllUser.GetCount<LiveChatAdminRoomUser>(string.Format("UserAutoId={0} And WebsiteOwner='{1}'", userInfo.AutoID, userInfo.WebsiteOwner)) == 0)
                    {
                        LiveChatAdminRoomUser model = new LiveChatAdminRoomUser();
                        model.InsertDate = DateTime.Now;
                        model.UserAutoId = userInfo.AutoID.ToString();
                        model.SocketSessionId = session.SessionID;
                        model.WebsiteOwner = userInfo.WebsiteOwner;
                        bllUser.Add(model);
                    }
                    else
                    {
                        bllUser.Update(new LiveChatAdminRoomUser(), string.Format("SocketSessionId='{0}'", session.SessionID), string.Format("UserAutoId={0}",userInfo.AutoID));
                    }

                    bllUser.Update(userInfo, string.Format(" IsOnline=1"), string.Format(" AutoId={0}",userInfo.AutoID));


                }
                #endregion
            }
            catch (Exception ex)
            {

                Console.WriteLine("连接异常:" + ex.ToString() + DateTime.Now.ToString());
            }

        }

        /// <summary>
        /// 发送消息事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        static void NewMessageReceived(WebSocketSession session, string message)
        {
            try
            {


                ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
                ZentCloud.BLLJIMP.BLLWeixin bllWeixin = new ZentCloud.BLLJIMP.BLLWeixin();
                ZentCloud.BLLJIMP.BLLLiveChat bllLive = new ZentCloud.BLLJIMP.BLLLiveChat();

                ZentCloud.BLLJIMP.BLLWebSite bllWebsite = new ZentCloud.BLLJIMP.BLLWebSite();

                //ZentCloud.BLLJIMP.BLLWebsiteDomainInfo bllWebsiteDomain = new ZentCloud.BLLJIMP.BLLWebsiteDomainInfo();
                Message resp = new Message();
                #region 用户发送
                if (session.Path.Contains("client"))
                {
                    // ws = new window[support]('ws://localhost:2012/client/userautoid');
                    string idString = session.Path.Split('/')[2];
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    LiveChatRoom room = bllUser.Get<LiveChatRoom>(string.Format("RoomId={0}", userInfo.AutoID));
                    Console.WriteLine(string.Format("用户发送消息:用户ID:{0},房间号:{1},消息:{2} {3}", idString, room.RoomId, message, DateTime.Now.ToString()));
                    List<LiveChatRoomUser> roomUserList = bllUser.GetList<LiveChatRoomUser>(string.Format("RoomId={0}", room.RoomId));

                    resp.message = message;
                    resp.message_type = "text";
                    resp.send_user_head_img = bllUser.GetUserDispalyAvatar(userInfo);
                    resp.send_user_name = "";
                    resp.send_user_type = "0";
                    resp.send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    string respJson = ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    room.UnReadCount += 1;
                    room.UpdateTime = DateTime.Now;
                    bllLive.Update(room);
                    foreach (var item in roomUserList)
                    {

                        var sess = appServer.GetSessionByID(item.SocketSessionId);
                        if (sess != null)
                        {
                            sess.TrySend(respJson);

                            #region 客服离线自动回复
                            if (!bllLive.IsKefuOnLine(room.WebsiteOwner) && item.UserType == "0")//客服不在线,自动 回复
                            {

                                var companyConfig = bllWebsite.Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", room.WebsiteOwner));
                                if (companyConfig != null && (!string.IsNullOrEmpty(companyConfig.KefuOffLineReply)))
                                {
                                    Message respAutoReply = new Message();
                                    respAutoReply.message = companyConfig.KefuOffLineReply;
                                    respAutoReply.message_type = "text";
                                    respAutoReply.send_user_head_img = "/img/icons/kefu.png";
                                    respAutoReply.send_user_name = "";
                                    respAutoReply.send_user_type = "1";
                                    respAutoReply.send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                    string respAutoReplyJson = ZentCloud.Common.JSONHelper.ObjectToJson(respAutoReply);
                                    sess.TrySend(respAutoReplyJson);


                                }






                            }
                            #endregion
                        }
                        if (item.UserType == "1")
                        {
                            //var kefuUser = bllUser.GetUserInfoByAutoID(int.Parse(item.UserAutoId));
                            //if (kefuUser.IsOnLine == 1)
                            //{
                            room.UnReadCount = 0;
                            bllLive.Update(room);
                            //}
                        }



                    }




                    LiveChatDetail detail = new LiveChatDetail();
                    detail.InsertDate = DateTime.Now;
                    detail.Message = message;
                    detail.MessageType = "text";
                    detail.RoomId = room.RoomId;
                    detail.UserAutoId = userInfo.AutoID.ToString();
                    detail.UserType = "0";
                    detail.UserHeadImg = bllUser.GetUserDispalyAvatar(userInfo);

                    bllUser.Add(detail);

                    #region 即时发送提醒消息
                    foreach (var item in bllUser.GetList<LiveChatAdminRoomUser>(string.Format(" WebsiteOwner='{0}'", userInfo.WebsiteOwner)))
                    {
                        var sess = appServer.GetSessionByID(item.SocketSessionId);
                        if (sess != null)
                        {
                            sess.Send(respJson);
                        }

                    } 
                    #endregion
                    

                    //#region 客服全部不在线,给客服发送模板消息
                    //if (bllLive.IsAllKefuOffLine(userInfo.WebsiteOwner))
                    //{
                    //    List<WXKeFu> kefuList = bllUser.GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", userInfo.WebsiteOwner));

                    //    foreach (var item in kefuList)
                    //    {

                    //        UserInfo kefuInfo = bllUser.GetUserInfoByOpenId(item.WeiXinOpenID, item.WebsiteOwner);
                    //        if (kefuInfo != null)
                    //        {
                    //            bool sendResult= bllWeixin.SendTemplateMessageNotifyComm(kefuInfo, string.Format("{0}发送了一条消息，请登录电脑回复", bllUser.GetUserDispalyName(userInfo)), string.Format("{0}", message));
                    //            Console.WriteLine("客服不在线,给客服发送消息:{0}发送结果:{1} {2}", message, sendResult, DateTime.Now.ToString());
                    //        }
                    //    }

                    //} 
                    //#endregion



                }
                #endregion

                #region 客服发送
                if (session.Path.Contains("server"))
                {
                    // ws = new window[support]('ws://localhost:2012/server/userautoid/roomid');
                    string idString = session.Path.Split('/')[2];
                    string roomId = session.Path.Split('/')[3];
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    Console.WriteLine(string.Format("客服发送消息:客服ID:{0},房间号:{1},消息:{2} {3}", idString, roomId, message, DateTime.Now.ToString()));
                    List<LiveChatRoomUser> roomUserList = bllUser.GetList<LiveChatRoomUser>(string.Format("RoomId={0}", roomId));
                    resp.message = message;
                    resp.message_type = "text";
                    resp.send_user_head_img = "/img/icons/kefu.png";
                    resp.send_user_name = "";
                    resp.send_user_type = "1";
                    resp.send_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    string respJson = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    foreach (var item in roomUserList)
                    {

                        var sess = appServer.GetSessionByID(item.SocketSessionId);
                        if (sess != null)
                        {
                            sess.Send(respJson);
                        }
                        //#region 用户不在线,客服回复
                        //if (item.UserType == "0")
                        //{
                        //    UserInfo uInfo = bllUser.GetUserInfoByAutoID(int.Parse(item.UserAutoId));
                        //    if (uInfo.IsOnLine == 0)
                        //    {
                        //        string url = string.Format("http://{0}/App/LiveChat/LiveChat.aspx", bllWebsiteDomain.GetWebsiteDoMain(userInfo.WebsiteOwner));

                        //        var sendResult = bllWeixin.SendTemplateMessageNotifyComm(uInfo, string.Format("客服给您发送了一条消息"), string.Format("{0}", message), url);

                        //        Console.WriteLine("用户不在线,客服发送消息:{0}发送结果:{1} {2}", message, sendResult, DateTime.Now.ToString());



                        //    }
                        //} 
                        //#endregion


                    }
                    LiveChatDetail detail = new LiveChatDetail();
                    detail.InsertDate = DateTime.Now;
                    detail.Message = message;
                    detail.MessageType = "text";
                    detail.RoomId = roomId;
                    detail.UserAutoId = userInfo.AutoID.ToString();
                    detail.UserType = "1";
                    bllUser.Add(detail);


                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine("发送消息异常:" + ex.ToString() + DateTime.Now.ToString());

            }


        }


        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        static void SessionClosed(WebSocketSession session, CloseReason reason)
        {
            try
            {


                ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
                #region 用户关闭
                if (session.Path.Contains("client"))
                {
                    // ws = new window[support]('ws://localhost:2012/client/userautoid');
                    string idString = session.Path.Split('/')[2];
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));

                    LiveChatRoom room = bllUser.Get<LiveChatRoom>(string.Format("RoomId={0}", userInfo.AutoID));

                    //给客服发送消息,通知会话结束
                    List<LiveChatRoomUser> roomUserList = bllUser.GetList<LiveChatRoomUser>(string.Format("RoomId={0}", userInfo.AutoID));
                    Message resp = new Message();
                    resp.message = "用户已经关闭会话";
                    resp.message_type = "system";
                    resp.send_user_head_img = "";
                    resp.send_user_name = "";
                    resp.send_user_type = "-1";
                    string respJson = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    foreach (var item in roomUserList.Where(p => p.UserType == "1"))
                    {
                        var sess = appServer.GetSessionByID(item.SocketSessionId);
                        if (sess != null)
                        {
                            sess.Send(respJson);
                        }


                    }
                    room.UserIsOnLine = 0;
                    bllUser.Update(room);
                    //bllUser.Update(userInfo, string.Format("IsOnLine=0"), string.Format("AutoId={0}", userInfo.AutoID));
                    Console.WriteLine(string.Format("用户关闭会话:用户ID：{0},房间号{1} {2}", idString, userInfo.AutoID, DateTime.Now.ToString()));
                }
                #endregion

                #region 客服关闭
                if (session.Path.Contains("server"))
                {
                    // ws = new window[support]('ws://localhost:2012/server/userautoid/roomid');
                    string idString = session.Path.Split('/')[2];
                    string roomId = session.Path.Split('/')[3];
                    LiveChatRoom room = bllUser.Get<LiveChatRoom>(string.Format("RoomId={0}", roomId));
                    room.IsKefuJoin = 0;
                    bllUser.Update(room);
                    bllUser.Delete(new LiveChatRoomUser(), string.Format("RoomId={0} And UserAutoId={1} And UserType=1", roomId, idString));
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    bllUser.Update(userInfo, string.Format("IsOnLine=0"), string.Format("AutoId={0}", userInfo.AutoID));
                    Console.WriteLine(string.Format("客服关闭会话:客服ID：{0},房间号{1} {2}", idString, roomId, DateTime.Now.ToString()));
                    bllUser.Update(userInfo, string.Format(" IsOnline=0"), string.Format(" AutoId={0}", userInfo.AutoID));
                }
                #endregion

                #region 客服首页关闭
                if (session.Path.Contains("index"))
                {
                    // ws = new window[support]('ws://localhost:2012/index/userautoid');
                    string idString = session.Path.Split('/')[2];
                    
                    UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(idString));
                    bllUser.Update(userInfo, string.Format("IsOnLine=0"), string.Format("AutoId={0}", userInfo.AutoID));
                    Console.WriteLine(string.Format("客服关闭首页:客服ID：{0},房间号{1}", idString, DateTime.Now.ToString()));
                    bllUser.Update(userInfo, string.Format(" IsOnline=0"), string.Format(" AutoId={0}", userInfo.AutoID));
                }
                #endregion

                Console.WriteLine("CloseReason:" + bllUser.EnumToString(reason));
            }
            catch (Exception ex)
            {
                Console.WriteLine("关闭异常:" + ex.ToString() + DateTime.Now.ToString());

            }
        }

        /// <summary>
        /// 消息模型
        /// </summary>
        public class Message
        {
            /// <summary>
            /// 消息主体
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 消息类型
            /// system 系统提示
            /// text   文本
            /// </summary>
            public string message_type { get; set; }
            /// <summary>
            /// 发送用户类型
            /// -1系统
            /// 0 用户
            /// 1 客服
            /// </summary>
            public string send_user_type { get; set; }
            /// <summary>
            /// 发送用户显示名称
            /// </summary>
            public string send_user_name { get; set; }
            /// <summary>
            /// 发送用户显示头像
            /// </summary>
            public string send_user_head_img { get; set; }
            /// <summary>
            /// 发送时间
            /// </summary>
            public string send_time { get; set; }

        }

    }

}
