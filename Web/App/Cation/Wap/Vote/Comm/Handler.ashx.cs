using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    /// <summary>
    /// 沙滩宝贝处理文件
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// SMS
        /// </summary>
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        /// <summary>
        /// 卡券业务逻辑
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 
        /// </summary>
        VoteInfo currVote = new VoteInfo();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                if (bllVote.IsLogin)
                {

                    currentUserInfo = bllVote.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }

                currVote = bllVote.GetVoteInfo(Convert.ToInt32(context.Request["vid"]));
                if (currVote == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "活动未找到";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }


                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action不存在";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
        outoff:
            context.Response.Write(result);
        }

        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteObjectInfo(HttpContext context)
        {

            try
            {

           

            string voteObjectName = context.Request["VoteObjectName"];
            string area = context.Request["Area"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(currVote.AutoID) + 1).ToString();//TODO:生成 number的算法可能会并发产生重复
            string phone = context.Request["Phone"];
            string height = context.Request["height"];

            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];

            string schoolName = context.Request["SchoolName"];
            string conatact = context.Request["Conatact"];
            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string ex3 = context.Request["Ex3"];
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];


            if (bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID) != null)
            {
                resp.errcode = 1;
                resp.errmsg = "你已经报过名了";
                goto outoff;
            }
            //if (string.IsNullOrEmpty(introduction))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入" + (string.IsNullOrWhiteSpace(currVote.SignUpDeclarationRename)? "参赛宣言": currVote.SignUpDeclarationRename);
            //    goto outoff;
            //}
            //if (string.IsNullOrEmpty(voteObjectName))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入姓名";
            //    goto outoff;
            //}

            //if (string.IsNullOrEmpty(area))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入城市";
            //    goto outoff;
            //}


            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            }
            if (bllVote.IsExitsVoteObjectNumber(currVote.AutoID, number))
            {
                resp.errcode = 1;
                resp.errmsg = "编号已经存在";
                goto outoff;
            }

            CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            if (!string.IsNullOrWhiteSpace(showImage1))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
                if (imgAngle1 != 0)
                {
                    imgAngle1 = RevertAngle(imgAngle1);

                    //获取源图片地址
                    string imgPath1 = context.Server.MapPath(showImage1);

                    var imgResult = imgHelper.RotateImg(imgPath1, imgAngle1, imgPath1);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage2))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle2 = Convert.ToInt32(context.Request["imgAngle2"]);
                if (imgAngle2 != 0)
                {
                    imgAngle2 = RevertAngle(imgAngle2);
                    //获取源图片地址
                    string imgPath2 = context.Server.MapPath(showImage2);
                    var imgResult = imgHelper.RotateImg(imgPath2, imgAngle2, imgPath2);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage3))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle3 = Convert.ToInt32(context.Request["imgAngle3"]);

                if (imgAngle3 != 0)
                {
                    imgAngle3 = RevertAngle(imgAngle3);
                    //获取源图片地址
                    string imgPath3 = context.Server.MapPath(showImage3);
                    var imgResult = imgHelper.RotateImg(imgPath3, imgAngle3, imgPath3);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage4))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle4 = Convert.ToInt32(context.Request["imgAngle4"]);

                if (imgAngle4 != 0)
                {
                    imgAngle4 = RevertAngle(imgAngle4);
                    //获取源图片地址
                    string imgPath4 = context.Server.MapPath(showImage4);
                    var imgResult = imgHelper.RotateImg(imgPath4, imgAngle4, imgPath4);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage5))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle5 = Convert.ToInt32(context.Request["imgAngle5"]);

                if (imgAngle5 != 0)
                {
                    imgAngle5 = RevertAngle(imgAngle5);
                    //获取源图片地址
                    string imgPath5 = context.Server.MapPath(showImage5);
                    var imgResult = imgHelper.RotateImg(imgPath5, imgAngle5, imgPath5);

                }
            }




            VoteObjectInfo model = new VoteObjectInfo();
            model.VoteID = currVote.AutoID;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Area = area;
            model.Introduction = introduction;
            model.Phone = phone;
            model.Status = 0;
            model.CreateUserId = currentUserInfo.UserID;
            model.Rank = 0;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
            model.Height = height;
            model.SchoolName = schoolName;
            model.Contact = conatact;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.Ex3 = ex3;
            if (!string.IsNullOrEmpty(voteObjectHeadImage))
            {
                model.VoteObjectHeadImage = voteObjectHeadImage;
            }
            if (bllVote.AddVoteObjectInfo(model))
            {
                resp.errmsg = "报名成功";
                bllVote.UpdateVoteObjectRank(currVote.AutoID);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "报名失败";
            }

            }
            catch (Exception ex)
            {
                bllVote.ToLog("投票报名异常"+ex.ToString());
                resp.errcode = 1;
                resp.errmsg = "报名失败,请重试";

            }
            outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteObjectInfo(HttpContext context)
        {

            string voteObjectName = context.Request["VoteObjectName"];
            string area = context.Request["Area"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(currVote.AutoID) + 1).ToString();
            string phone = context.Request["Phone"];
            string height = context.Request["height"];

            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];

            string schoolName = context.Request["SchoolName"];
            string conatact = context.Request["Conatact"];
            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string ex3 = context.Request["Ex3"];
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];

            VoteObjectInfo model = bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID);
            if (model == null)
            {
                resp.errcode = 1;
                resp.errmsg = "您还没有报名";
                goto outoff;
            }
            if (model.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "您已通过审核,不能修改个人资料";
                goto outoff;
            }
            //if (string.IsNullOrEmpty(introduction))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入参赛口号";
            //    goto outoff;
            //}
            //if (string.IsNullOrEmpty(voteObjectName))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入姓名";
            //    goto outoff;
            //}

            //if (string.IsNullOrEmpty(area))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入城市";
            //    goto outoff;
            //}


            //if (string.IsNullOrEmpty(phone))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入联系人电话";
            //    goto outoff;
            //}
            CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            if (!string.IsNullOrWhiteSpace(showImage1))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
                //imgAngle = 90;
                if (imgAngle1 != 0)
                {
                    imgAngle1 = RevertAngle(imgAngle1);
                    if (showImage1.StartsWith("http"))//远程图片
                    {
                        showImage1 = bllJuactivity.DownLoadRemoteImage(showImage1);

                    }
                    //获取源图片地址
                    string imgPath1 = context.Server.MapPath(showImage1);


                    var imgResult = imgHelper.RotateImg(imgPath1, imgAngle1, imgPath1);




                }
            }
            if (!string.IsNullOrWhiteSpace(showImage2))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle2 = Convert.ToInt32(context.Request["imgAngle2"]);

                if (imgAngle2 != 0)
                {
                    imgAngle2 = RevertAngle(imgAngle2);
                    if (showImage2.StartsWith("http"))//远程图片
                    {
                        showImage2 = bllJuactivity.DownLoadRemoteImage(showImage2);

                    }
                    //获取源图片地址
                    string imgPath2 = context.Server.MapPath(showImage2);
                    var imgResult = imgHelper.RotateImg(imgPath2, imgAngle2, imgPath2);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage3))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle3 = Convert.ToInt32(context.Request["imgAngle3"]);

                if (imgAngle3 != 0)
                {
                    imgAngle3 = RevertAngle(imgAngle3);
                    if (showImage3.StartsWith("http"))//远程图片
                    {
                        showImage3 = bllJuactivity.DownLoadRemoteImage(showImage3);

                    }
                    //获取源图片地址
                    string imgPath3 = context.Server.MapPath(showImage3);
                    var imgResult = imgHelper.RotateImg(imgPath3, imgAngle3, imgPath3);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage4))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle4 = Convert.ToInt32(context.Request["imgAngle4"]);

                if (imgAngle4 != 0)
                {
                    imgAngle4 = RevertAngle(imgAngle4);
                    if (showImage4.StartsWith("http"))//远程图片
                    {
                        showImage4 = bllJuactivity.DownLoadRemoteImage(showImage4);

                    }
                    //获取源图片地址
                    string imgPath4 = context.Server.MapPath(showImage4);
                    var imgResult = imgHelper.RotateImg(imgPath4, imgAngle4, imgPath4);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage5))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle5 = Convert.ToInt32(context.Request["imgAngle5"]);

                if (imgAngle5 != 0)
                {
                    imgAngle5 = RevertAngle(imgAngle5);
                    if (showImage5.StartsWith("http"))//远程图片
                    {
                        showImage5 = bllJuactivity.DownLoadRemoteImage(showImage5);

                    }
                    //获取源图片地址
                    string imgPath5 = context.Server.MapPath(showImage5);
                    var imgResult = imgHelper.RotateImg(imgPath5, imgAngle5, imgPath5);

                }
            }

            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Area = area;
            model.Introduction = introduction;
            model.Phone = phone;
            model.Height = height;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
            model.SchoolName = schoolName;
            model.Contact = conatact;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.Ex3 = ex3;
            if (!string.IsNullOrEmpty(voteObjectHeadImage))
            {
                model.VoteObjectHeadImage = voteObjectHeadImage;
            }
            if (bllVote.UpdateVoteObjectInfo(model))
            {
                resp.errmsg = "修改成功";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "修改失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectVoteCount(HttpContext context)
        {

            int voteObjectId = int.Parse(context.Request["id"]);
            string msg = "";
            if (bllVote.UpdateVoteObjectVoteCount(currVote.AutoID, voteObjectId, currentUserInfo.UserID, 1, out msg))
            {

                if (!bllVote.UpdateVoteObjectRank(currVote.AutoID, "1"))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新排名失败";
                    goto outoff;
                }
                resp.errmsg = "投票成功!";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取选手列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteObjectVoteList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["Pageindex"]);
            int pageSize = int.Parse(context.Request["Pagesize"]);
            string keyWord = context.Request["KeyWord"];
            string sort = context.Request["Sort"];
            int vid = Convert.ToInt32(context.Request["vid"]);

            int totalCount = 0;//总数量
            VoteObjectModelApi apiResult = new VoteObjectModelApi();
            apiResult.list = new List<VoteObjectModel>();
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(vid, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort);
            foreach (var item in sourceList)
            {
                VoteObjectModel model = new VoteObjectModel();
                model.id = item.AutoID;
                model.headimg = item.VoteObjectHeadImage;
                model.name = item.VoteObjectName;
                model.number = item.Number;
                model.rank = item.Rank;
                model.votecount = item.VoteCount;
                model.area = item.Area;
                model.conatact = item.Contact;
                apiResult.list.Add(model);
            }
            apiResult.totalcount = totalCount;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 获取所有组
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteGroupList(HttpContext context)
        {

            int vid = Convert.ToInt32(context.Request["vid"]);

            //获取所有组
            resp.result = bllVote.GetVoteAllGroup(vid);
            resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;

            return Common.JSONHelper.ObjectToJson(resp); ;
        }

        /// <summary>
        /// 系统添加票数 随机投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SysAddvoteCountRand(HttpContext context)
        {

            if (currentUserInfo.UserType.Equals(1))//超级管理员才可以操作
            {
                int addCountFrom = int.Parse(context.Request["AddCountFrom"]);
                int addCountTo = int.Parse(context.Request["AddCountTo"]);
                int voteCountFrom = int.Parse(context.Request["VoteCountFrom"]);
                int voteCountTo = int.Parse(context.Request["VoteCountTo"]);
                int voteId = int.Parse(context.Request["VoteId"]);
                //
                List<VoteObjectInfo> voteObjList = bllVote.GetList<VoteObjectInfo>(string.Format(" VoteCount>={0} And VoteCount<={1} And VoteID={2} And Status=1 ", voteCountFrom, voteCountTo, voteId));
                if (voteObjList.Count > 0)
                {

                    foreach (var item in voteObjList)
                    {
                        Random rand = new Random();
                        int addVoteCount = rand.Next(1, addCountTo + 1);
                        for (int i = 0; i < addVoteCount; i++)
                        {
                            //注册用户
                            DateTime regTime = DateTime.Now.AddMinutes(new Random().Next(1, 10));

                            #region 注册用户
                            //注册用户
                            UserInfo userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                            userInfo.UserID = string.Format("WXUser{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
                            userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                            userInfo.UserType = 2;
                            //UserInfo.WebsiteOwner = bllVote.WebsiteOwner;
                            userInfo.WebsiteOwner = "haima";
                            userInfo.Regtime = regTime;
                            userInfo.WXOpenId = "zc" + ZentCloud.Common.Rand.Str_char(12) + "cl";
                            userInfo.WXScope = "snsapi_base";
                            userInfo.RegIP = bllVote.GetLit<UserInfo>(1, new Random().Next(1, 10000), "RegIP IS NOT NULL")[0].RegIP;
                            userInfo.LastLoginIP = userInfo.RegIP;
                            userInfo.LastLoginDate = regTime;
                            userInfo.LoginTotalCount = 1;
                            if (bllVote.Add(userInfo))
                            {
                                VoteLogInfo log = new VoteLogInfo();
                                log.UserID = userInfo.UserID;
                                log.VoteID = voteId;
                                log.VoteObjectID = item.AutoID;
                                log.InsertDate = ((DateTime)(userInfo.Regtime)).AddSeconds(rand.Next(2, 10));
                                log.CreateUserID = bllVote.WebsiteOwner;
                                //Log.WebsiteOwner = bllVote.WebsiteOwner;
                                log.WebsiteOwner = "haima";
                                log.VoteCount = 1;
                                log.IP = userInfo.RegIP;
                                log.IPLocation = Common.MySpider.GetIPLocation(log.IP);
                                if (bllVote.Add(log))
                                {
                                    if (bllVote.Update(new VoteObjectInfo(), "VoteCount+=1", string.Format(" AutoID={0}", item.AutoID)) > 0)
                                    {

                                        System.Threading.Thread.Sleep(100);

                                    }

                                }
                            }

                            #endregion
                            //




                        }






                    }
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "在所选选手票数范围内，查询不到数据";
                }

                //




            }
            else
            {
                resp.errcode = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 系统添加票数 指定选手编号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SysAddvoteCount(HttpContext context)
        {

            if (currentUserInfo.UserType.Equals(1))//超级管理员才可以操作
            {
                int number = int.Parse(context.Request["Number"]);
                int addCount = int.Parse(context.Request["AddCount"]);
                int voteId = int.Parse(context.Request["VoteId"]);
                //
                VoteObjectInfo voteObj = bllVote.Get<VoteObjectInfo>(string.Format(" Number={0} And VoteID={1} And Status=1 ", number, voteId));
                if (voteObj != null)
                {



                    for (int i = 0; i < addCount; i++)
                    {
                        Random rand = new Random();
                        //注册用户
                        DateTime regTime = DateTime.Now.AddMinutes(new Random().Next(1, 10));

                        #region 注册用户
                        //注册用户
                        UserInfo userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                        userInfo.UserID = string.Format("WXUser{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
                        userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                        userInfo.UserType = 2;
                        userInfo.WebsiteOwner = bllVote.WebsiteOwner;
                        userInfo.Regtime = regTime;
                        userInfo.WXOpenId = "zc" + ZentCloud.Common.Rand.Str_char(12) + "cl";
                        userInfo.WXScope = "snsapi_base";
                        userInfo.RegIP = bllVote.GetLit<UserInfo>(1, new Random().Next(1, 10000), "RegIP IS NOT NULL")[0].RegIP;
                        userInfo.LastLoginIP = userInfo.RegIP;
                        userInfo.LastLoginDate = regTime;
                        userInfo.LoginTotalCount = 1;
                        if (bllVote.Add(userInfo))
                        {
                            VoteLogInfo log = new VoteLogInfo();
                            log.UserID = userInfo.UserID;
                            log.VoteID = voteId;
                            log.VoteObjectID = voteObj.AutoID;
                            log.InsertDate = ((DateTime)(userInfo.Regtime)).AddSeconds(rand.Next(2, 10));
                            log.CreateUserID = bllVote.WebsiteOwner;
                            log.WebsiteOwner = bllVote.WebsiteOwner;
                            log.VoteCount = 1;
                            log.IP = userInfo.RegIP;
                            log.IPLocation = Common.MySpider.GetIPLocation(log.IP);
                            if (bllVote.Add(log))
                            {
                                if (bllVote.Update(new VoteObjectInfo(), "VoteCount+=1", string.Format(" AutoID={0}", voteObj.AutoID)) > 0)
                                {

                                    System.Threading.Thread.Sleep(100);

                                }

                            }
                        }

                        #endregion
                        //











                    }
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "选手编号不存在";
                }

                //


            }
            else
            {
                resp.errcode = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 顺逆时针旋转角度对应
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int RevertAngle(int value)
        {

            switch (value)
            {
                case 90:
                    return 270;
                case 270:
                    return 90;
                default:
                    return value;

            }
        }


        /// <summary>
        /// 获取手机领奖验证码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSmsVercode(HttpContext context)
        {
            //检查是否已经领过了
            int totalCount = 0;
            bllCardCoupon.GetMyCardCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, currentUserInfo.UserID, 1, 1, out totalCount);

            if (totalCount > 0)
            {
                resp.errcode = 3;
                resp.errmsg = "您已经领取过热带风暴电子门票了";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            var model = bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID);
            if (model == null)
            {
                resp.errcode = 3;
                resp.errmsg = "您还没有报名";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (!model.Status.Equals(1))
            {
                resp.errcode = 3;
                resp.errmsg = "您还未通过审核";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (model.Rank > 1000)
            {
                resp.errcode = 3;
                resp.errmsg = "您没有获奖";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string phone = model.Phone;//手机号
            var lastVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastVerCode != null)
            {
                if ((DateTime.Now - lastVerCode.InsertDate).TotalSeconds < 60)
                {
                    resp.errcode = 2;
                    resp.errmsg = "验证码限制每60秒发送一次";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

                resp.errcode = 0;
                resp.errmsg = "验证码已发送";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            else
            {

            }

            bool isSuccess = false;
            string msg = "";
            string verCode = new Random().Next(111111, 999999).ToString();
            bllSms.SendSmsVerificationCode(phone, string.Format("您的领奖验证码{0}", verCode), "沙滩宝贝", verCode, out isSuccess, out msg);
            if (isSuccess)
            {
                resp.errmsg = "验证码发送成功";
            }
            else
            {
                resp.errcode = 3;
                resp.errmsg = msg;
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 更新我的收货地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateMyAddress(HttpContext context)
        {
            var model = bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID);
            if (model == null)
            {
                resp.errcode = 3;
                resp.errmsg = "您还没有报名";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (!model.Status.Equals(1))
            {
                resp.errcode = 3;
                resp.errmsg = "您还未通过审核";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (model.Rank > 1000)
            {
                resp.errcode = 3;
                resp.errmsg = "您没有获奖";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllVote.Update(model, string.Format(" Address='{0}'", context.Request["address"]), string.Format(" AutoID={0}", model.AutoID)) > 0)
            {
                resp.errmsg = "成功提交了收货地址";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.errcode = 3;
                resp.errmsg = "更新收货地址失败";
                return Common.JSONHelper.ObjectToJson(resp);
            }





        }

        /// <summary>
        /// 获取热带风暴门票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCardCoupon(HttpContext context)
        {

            string smsVerCode = context.Request["SmsVerCode"];
            var model = bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID);
            if (model == null)
            {
                resp.errcode = 3;
                resp.errmsg = "您还没有报名";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (!model.Status.Equals(1))
            {
                resp.errcode = 3;
                resp.errmsg = "您还未通过审核";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else if (model.Rank > 1000)
            {
                resp.errcode = 3;
                resp.errmsg = "您没有获奖";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经领过了
            int totalCount = 0;
            bllCardCoupon.GetMyCardCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, currentUserInfo.UserID, 1, 1, out totalCount);

            if (totalCount > 0)
            {
                resp.errcode = 3;
                resp.errmsg = "您已经领取过热带风暴电子门票了";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(smsVerCode))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入手机验证码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            var lastVerCode = bllSms.GetLastSmsVerificationCode(model.Phone);
            if (lastVerCode == null)
            {
                resp.errcode = 3;
                resp.errmsg = "请先获取手机验证码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (smsVerCode != lastVerCode.VerificationCode)
            {
                resp.errcode = 3;
                resp.errmsg = "手机验证码错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int count = 0;
            if (model.Rank >= 1 && model.Rank <= 10)
            {
                count = 4;//1-到10名 发4张
            }
            if (model.Rank >= 11 && model.Rank <= 100)
            {
                count = 2;//11-到100名 发2张
            }
            if (model.Rank >= 101 && model.Rank <= 1000)
            {
                count = 1;//101-到1000名 发1张
            }
            for (int i = 1; i <= count; i++)
            {
                string Msg = "";
                if (bllCardCoupon.ReciveCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, 1, currentUserInfo.UserID, out Msg))
                {
                    System.Threading.Thread.Sleep(1000);

                }
                else
                {
                    resp.errcode = 3;
                    resp.errmsg = Msg;
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }



            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitInfoDali(HttpContext context)
        {
            string voteId = context.Request["vid"];
            string phone = context.Request["phone"];
            var recordCount = bllVote.GetCount<SMSDetails>(string.Format("PlanID={0} And Receiver='{1}'", voteId, currentUserInfo.UserID));
            if (recordCount==1)
            {
                resp.errcode = -1;
                resp.errmsg = "不能再继续抽奖";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string token = "";
            DateTime dtSendTime = DateTime.Now;
            if (dtSendTime.Hour>=19)
            {
                dtSendTime=Convert.ToDateTime( dtSendTime.ToString("yyyy-MM-dd"));
                dtSendTime = dtSendTime.AddDays(1).AddHours(9);
            }
            Random rand = new Random();
            int index = rand.Next(1, 3);
            switch (index)
            {
                case 1:
                    token = "我的时尚";
                    break;
                case 2:
                    token = "加油";
                    break;
                default:
                    break;
            }
            string sendMsg = string.Format("感谢参与，您的密令为 “{0}”，点击 c.tb.cn/c.ZIW7y 立刻前往官方店联系客服领取！【The.Me】", token);


            UserInfo websiteOwnerUserInfo = bllVote.GetCurrWebSiteUserInfo();
            Common.HttpInterFace webRequest = new Common.HttpInterFace();
            string parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membermission&attime={4}", websiteOwnerUserInfo.UserID, websiteOwnerUserInfo.Password, phone, sendMsg, dtSendTime.ToString());
            string returnCode = webRequest.PostWebRequest(parm, "http://sms.comeoncloud.net/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(returnCode) && (returnCode.ToString().Equals("0")))
            {
                resp.errmsg = "ok";
                SMSDetails model = new SMSDetails();
                model.PlanID = voteId;
                model.Receiver =currentUserInfo.UserID;
                bllSms.Add(model);

            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "code:"+returnCode;
                //if (returnCode=="1002")//余额不足
                //{
                //    resp.errcode = 0;
                //}
                return Common.JSONHelper.ObjectToJson(resp);

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 默认响应模型
        /// </summary>
        private class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

            /// <summary>
            /// 结果
            /// </summary>
            public dynamic result { get; set; }
        }

        /// <summary>
        /// 选手模型
        /// </summary>
        private class VoteObjectModel
        {
            /// <summary>
            /// 系统编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 编号
            /// </summary>
            public string number { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string headimg { get; set; }
            /// <summary>
            /// 得票数
            /// </summary>
            public int votecount { get; set; }
            /// <summary>
            /// 区域
            /// </summary>
            public string area { get; set; }
            /// <summary>
            /// 排名
            /// </summary>
            public int rank { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string conatact { get; set; }

        }

        private class VoteObjectModelApi
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 选手列表
            /// </summary>
            public List<VoteObjectModel> list { get; set; }


        }




        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}