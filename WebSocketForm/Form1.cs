using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using SuperWebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Socket;

namespace WebSocketForm
{
    public partial class Form1 : Form
    {
        private List<User> users;
        private List<User> uList;
        private IBootstrap bootstrap;
        private WebSocketServer appServer;
        private BLLUser bllUser = new BLLUser();
        private BLLSocketLog bllSocketLog = new BLLSocketLog();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            users = new List<User>();
            uList = new List<User>();

            bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                MessageBox.Show("CreateBootstrap 出错");
                return;
            }
            var socketServer = bootstrap.AppServers.FirstOrDefault(s => s.Name.Equals("ZentCloudWebSocket")) as WebSocketServer;

            socketServer.NewSessionConnected += socketServer_NewSessionConnected;
            socketServer.SessionClosed += socketServer_SessionClosed;

            socketServer.NewMessageReceived += socketServer_NewMessageReceived;
            appServer = socketServer;
            bootstrap.Start();
        }
        //连接时
        private void socketServer_NewSessionConnected(WebSocketSession session)
        {
            string idString = session.Path.TrimStart('/');
            string sessionID = session.SessionID;
            DateTime curTime = DateTime.Now;
            int id =0;
            int.TryParse(idString,out id);
            if (id > 0) //正常登录用户连接id
            {
                editUsers(id, sessionID, curTime);
                noticeOnlineFriends(id, "FriendLogin");
            }
            else
            {
                string[] sps = idString.Split('/');
                string action = sps[0];

                #region 建立连接,返回rediskey用于生成二维码(前台生成二维码)
                if (action == "QRCode"){
                    createQRCodeLoginRedisKey(session, sessionID);
                }
                #endregion 建立连接,返回rediskey用于生成二维码
                #region 手机端建立连接，检查redis中是否有用户(手机端，点击连接)
                else if (action == "QRCodeLogin"){
                    string redisKey = sps[1];
                    checkQRCodeLoginRedis(session, redisKey);
                #endregion  手机端建立连接，检查redis中是否有用户(手机端，点击连接)
                }
            }
        }
        //记录redis，返回redisKey，用于生成二维码
        private void createQRCodeLoginRedisKey(WebSocketSession session, string sessionID)
        {
            string redisKey = sessionID.Replace("-", "").ToUpper();
            try
            {
                RedisHelper.RedisHelper.StringSetSerialize(redisKey, new QRCodeLoginRedis
                {
                    sessionID = sessionID
                }, TimeSpan.FromHours(1));

                session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                {
                    redisKey = redisKey,
                    status = 0,
                    msg = "等待登录"
                }));
            }
            catch (Exception ex)
            {
                session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                {
                    redisKey = redisKey,
                    status = 9,
                    msg = "redis服务错误"
                }));
            }
        }

        //手机端点击连接，检查redis中记录的用户
        private void checkQRCodeLoginRedis(WebSocketSession session, string redisKey)
        {
            QRCodeLoginRedis qrReids = new QRCodeLoginRedis();
            try
            {
                qrReids = RedisHelper.RedisHelper.StringGet<QRCodeLoginRedis>(redisKey);
            }
            catch (Exception ex)
            {
                session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                {
                    redisKey = redisKey,
                    status = 9,
                    msg = "redis服务错误"
                }));
                return;
            }
            WebSocketSession qrSession = appServer.GetSessionByID(qrReids.sessionID);
            if (qrSession != null)
            {
                if (string.IsNullOrWhiteSpace(qrReids.userID))
                {
                    qrSession.Send(JsonConvert.SerializeObject(new QRCodeLogin
                    {
                        redisKey = redisKey,
                        status = 2,
                        msg = "登录失败"
                    }));
                    session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                    {
                        redisKey = redisKey,
                        status = 2,
                        msg = "登录失败"
                    }));
                }
                else
                {
                    qrSession.Send(JsonConvert.SerializeObject(new QRCodeLogin
                    {
                        redisKey = redisKey,
                        status = 1,
                        msg = "登录成功"
                    }));

                    session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                    {
                        redisKey = redisKey,
                        status = 1,
                        msg = "登录成功"
                    }));
                }
            }
            else
            {
                session.Send(JsonConvert.SerializeObject(new QRCodeLogin
                {
                    redisKey = redisKey,
                    status = 9,
                    msg = "连接申请已过期"
                }));
            }
        }

        //登录用户连接时
        private void editUsers(int id, string sessionID, DateTime curTime)
        {
            User user = users.FirstOrDefault(p => p.id == id);
            if (user != null)
            {
                user.connlogs.Add(new Conn() { SessionID = sessionID, conntime = curTime, online = true });
                //RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.SocketOnline, users);
            }
            else
            {
                User u = uList.FirstOrDefault(p => p.id == id);
                if (u == null)
                {
                    u = new User();
                    u.id = id;

                    UserInfo curUser = bllUser.GetUserInfoByAutoID(id);
                    u.name = bllUser.GetUserDispalyName(curUser);
                    u.ico = bllUser.GetUserDispalyAvatar(curUser);
                    u.userid = curUser.UserID;
                    uList.Add(u);
                }
                user = new User();
                user.id = id;
                user.name = u.name;
                user.userid = u.userid;
                user.ico = u.ico;
                user.logintime = curTime;
                user.connlogs = new List<Conn>(){
                    new Conn(){ SessionID=sessionID, conntime=curTime, online=true }
                };
                users.Add(user);
                //RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.SocketOnline, users);
                AddRow(user);
            }
        }

        //接收客户端消息
        private void socketServer_NewMessageReceived(WebSocketSession session, string message)
        {
            string idString = session.Path.TrimStart('/');
            int id = 0;
            int.TryParse(idString, out id);
            if (id > 0) //正常登录用户连接id
            {
                string[] msgsp = message.Split('/');
                string action = msgsp[0];
                if (action == "GetOnlineFriends")
                {
                    getOnlineFriends(session, id, msgsp[1]);
                }
            }
        }
        //获得在线好友
        private void getOnlineFriends(WebSocketSession session, int id, string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                User user = users.FirstOrDefault(p => p.id == id);
                List<int> friends = param.Split(',').Select(p => Convert.ToInt32(p)).ToList();
                user.friends = friends;
                List<int> onlineFriends = new List<int>();
                foreach (var item in users.Where(p=>friends.Contains(p.id) && p.connlogs.Where(pi=>pi.online).Count()>0))
                {
                    onlineFriends.Add(item.id);
                }
                if (onlineFriends.Count>0) sendOnlineFriends(session, onlineFriends);
            }
        }
        //返回在线好友
        private void sendOnlineFriends(WebSocketSession session, List<int> friends)
        {
            string message = friends.Count == 0 ? "" : ZentCloud.Common.MyStringHelper.ListToStr(friends, "", ",");
            session.Send(JsonConvert.SerializeObject(new SendToClient
            {
                action = "GetOnlineFriends",
                message = message
            }));
        }
        //登录通知相关在线好友
        private void noticeOnlineFriends(int id, string action)
        {
            foreach (var item in users.Where(p => p.friends.Contains(id) && p.connlogs.Where(pi => pi.online).Count() > 0))
	        {
                foreach (var connlog in item.connlogs.Where(pi => pi.online))
                {
                    WebSocketSession rSession = appServer.GetSessionByID(connlog.SessionID);
                    sendFriendOnlineStatus(rSession, id, action);
                }
	        }
        }
        //返回上线通知
        private void sendFriendOnlineStatus(WebSocketSession session, int id, string action)
        {
            session.Send(JsonConvert.SerializeObject(new SendToClient
            {
                action = action,
                message = id.ToString()
            }));
        }
        //断开连接时
        private void socketServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason reason)
        {
            string closeReason = CommonPlatform.Helper.EnumStringHelper.ToString(reason);
            DateTime curTime = DateTime.Now;
            #region 程序关闭时记录日志
            if (reason == SuperSocket.SocketBase.CloseReason.ServerShutdown)
            {
                delAllUsers(curTime, closeReason);
                //RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.SocketOnline, users);
            }
            #endregion 程序关闭时记录日志
            #region 断开连接时判断
            else
            {
                string idString = session.Path.TrimStart('/');
                string sessionID = session.SessionID;
                int id = 0;
                int.TryParse(idString, out id);
                #region 正常登录用户断开连接
                if (id > 0)  //正常登录用户断开连接id
                {
                    if (reason == SuperSocket.SocketBase.CloseReason.ClientClosing)
                    {
                        Thread.Sleep(5000);
                    }
                    delUsers(id, sessionID, curTime, closeReason);
                }
                #endregion 正常登录用户断开连接
            }
            #endregion 断开连接时判断
        }
        /// <summary>
        /// 清空所有用户连接状态，记录日志
        /// </summary>
        /// <param name="curTime"></param>
        /// <param name="closeReason"></param>
        private void delAllUsers(DateTime curTime, string closeReason)
        {
            foreach (User user in users)
            {
                SocketLog log = new SocketLog();
                log.UserAutoID = user.id;
                log.UserID = user.userid;
                log.UserNickname = user.name;
                log.StartTime = user.logintime;
                log.StartTimestamp = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(log.StartTime);
                log.EndTime = curTime;
                log.UserAvatar = user.ico;
                log.Minutes = Convert.ToInt32((log.EndTime - log.StartTime).TotalMinutes);
                log.CloseReason = closeReason;
                if (!bllSocketLog.Exists(log, new List<string>() { "UserAutoID", "StartTimestamp" }))
                {
                    bllSocketLog.Add(log);
                    bllSocketLog.UpdateUserOnlineTimes(user.id);
                }
                DelRow(user);
                users.Remove(user);
            }
        }
        //清理用户连接状态，记录日志
        private void delUsers(int id, string sessionID, DateTime curTime, string closeReason)
        {
            User user = users.FirstOrDefault(p => p.id == id);
            if (user != null)
            {
                Conn connlog = user.connlogs.FirstOrDefault(p => p.SessionID == sessionID);
                if (connlog!=null) connlog.online = false;
                foreach (var item in user.connlogs.Where(p => p.online))
                {
                    WebSocketSession rSession = appServer.GetSessionByID(item.SessionID);
                    if (rSession == null) item.online = false;
                }
                if (user.connlogs.Where(p => p.online).Count() == 0)
                {
                    SocketLog log = new SocketLog();
                    log.UserAutoID = user.id;
                    log.UserID = user.userid;
                    log.UserNickname = user.name;
                    log.StartTime = user.logintime;
                    log.StartTimestamp = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(log.StartTime);
                    log.EndTime = curTime;
                    log.UserAvatar = user.ico;
                    log.Minutes = Convert.ToInt32((log.EndTime - log.StartTime).TotalMinutes);
                    log.CloseReason = closeReason;
                    if (!bllSocketLog.Exists(log, new List<string>() { "UserAutoID", "StartTimestamp" }))
                    {
                        bllUser.Add(log);
                        bllSocketLog.UpdateUserOnlineTimes(user.id);
                    }
                    DelRow(user);
                    users.Remove(user);
                    noticeOnlineFriends(id, "FriendLogout");
                }
                //RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.SocketOnline, users);
            }
        }
        //清除Session不存在的用户
        private void delNoSessionUsers()
        {

        }

        delegate void Delegate(object obj);
        //添加行
        private void AddRow(object row)
        {
            if (listView1.InvokeRequired)
            {
                Delegate d = new Delegate(AddRow);
                listView1.Invoke(d, row);
            }
            else
            {
                User user = (User)row;
                ListViewItem item = new ListViewItem();
                item.Text = user.id.ToString();
                item.SubItems.Add(user.name);
                item.SubItems.Add(user.logintime.ToString("yyyy/MM/dd HH:mm:ss"));
                listView1.Items.Add(item);
            }
        }
        //删除行
        private void DelRow(object row)
        {
            if (listView1.InvokeRequired)
            {
                Delegate d = new Delegate(DelRow);
                listView1.Invoke(d, row);
            }
            else
            {
                User user = (User)row;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Text == user.id.ToString())
                    {
                        listView1.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                bootstrap.Stop();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
