using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.customize.BeachHoney
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
        UserInfo CurrentUserInfo = new UserInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                if (bllVote.IsLogin)
                {

                    CurrentUserInfo = bllVote.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
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

            string voteObjectName = context.Request["VoteObjectName"];
            string area = context.Request["Area"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(bllVote.BeachHoneyVoteID) + 1).ToString();
            string phone = context.Request["Phone"];


            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];
            if (!context.Request.Url.Host.Equals("beachhoney.comeoncloud.net"))
	        {
                resp.errcode = 1;
                resp.errmsg = "请通过沙滩宝贝公众号进行报名";
                goto outoff;
	        }
            if (bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID) != null)
            {
                resp.errcode = 1;
                resp.errmsg = "你已经报过名了";
                goto outoff;
            }
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛口号";
                goto outoff;
            }
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                goto outoff;
            }

            if (string.IsNullOrEmpty(area))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入城市";
                goto outoff;
            }


            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            }
            if (bllVote.IsExitsVoteObjectNumber(bllVote.BeachHoneyVoteID, number))
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
            model.VoteID = bllVote.BeachHoneyVoteID;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Area = area;
            model.Introduction = introduction;
            model.Phone = phone;
            model.Status = 0;
            model.CreateUserId = CurrentUserInfo.UserID;
            model.Rank = 0;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
            if (bllVote.AddVoteObjectInfo(model))
            {
                resp.errmsg = "报名成功";
                bllVote.UpdateVoteObjectRank(bllVote.BeachHoneyVoteID);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "报名失败";
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
            string number = (bllVote.GetVoteObjectMaxNumber(bllVote.BeachHoneyVoteID) + 1).ToString();
            string phone = context.Request["Phone"];
            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            string showImage5 = context.Request["ShowImage5"];

            VoteObjectInfo model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
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
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛口号";
                goto outoff;
            }
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                goto outoff;
            }

            if (string.IsNullOrEmpty(area))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入城市";
                goto outoff;
            }


            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            }
            CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            if (!string.IsNullOrWhiteSpace(showImage1))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
                //imgAngle = 90;
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

            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Area = area;
            model.Introduction = introduction;
            model.Phone = phone;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;
            model.ShowImage3 = showImage3;
            model.ShowImage4 = showImage4;
            model.ShowImage5 = showImage5;
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
            if (!bllVote.IsWeiXinBrowser)
            {
                resp.errcode = 2;
                resp.errmsg = "";
                goto outoff;
            }


            int voteObjectId = int.Parse(context.Request["id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(bllVote.BeachHoneyVoteID);
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.errcode = 1;
                resp.errmsg = "投票停止";
                goto outoff;

            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > (DateTime.Parse(voteInfo.StopDate)))
                {
                    resp.errcode = 1;
                    resp.errmsg = "投票结束";
                    goto outoff;

                }
            }
            VoteObjectInfo model = bllVote.GetVoteObjectInfo(voteObjectId);
            if (!model.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能投票";
                goto outoff;
            }
            //检查是否可以投票
            if (bllVote.GetCount<VoteLogInfo>(string.Format("VoteObjectID={0} And UserID='{1}'", voteObjectId, CurrentUserInfo.UserID)) > 0)
            {
                resp.errcode = 1;
                resp.errmsg = "您已经投过票了";
                goto outoff;
            }
            //

            //
            if (bllVote.UpdateVoteObjectVoteCount(bllVote.BeachHoneyVoteID, voteObjectId, CurrentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(bllVote.BeachHoneyVoteID, "1"))
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
                resp.errmsg = "投票失败";
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
            int totalCount = 0;//总数量
            VoteObjectModelApi apiResult = new VoteObjectModelApi();
            apiResult.list = new List<VoteObjectModel>();
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(bllVote.BeachHoneyVoteID, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort);
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
                apiResult.list.Add(model);
            }
            apiResult.totalcount = totalCount;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 系统添加票数 随机投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SysAddvoteCountRand(HttpContext context)
        {

            if (CurrentUserInfo.UserType.Equals(1))//超级管理员才可以操作
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
                            userInfo.RegIP = bllVote.GetLit<UserInfo>(1,new Random().Next(1,10000),"RegIP IS NOT NULL")[0].RegIP;
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

            if (CurrentUserInfo.UserType.Equals(1))//超级管理员才可以操作
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
            bllCardCoupon.GetMyCardCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, CurrentUserInfo.UserID, 1, 1, out totalCount);

            if (totalCount > 0)
            {
                resp.errcode = 3;
                resp.errmsg = "您已经领取过热带风暴电子门票了";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            var model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
            if (model == null)
            {
                resp.errcode = 3;
                resp.errmsg ="您还没有报名";
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
            var model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
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

            if (bllVote.Update(model,string.Format(" Address='{0}'",context.Request["address"]),string.Format(" AutoID={0}",model.AutoID))>0)
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
            var model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
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
            bllCardCoupon.GetMyCardCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, CurrentUserInfo.UserID, 1, 1, out totalCount);

            if (totalCount>0)
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
            if (lastVerCode== null)
            {
                resp.errcode = 3;
                resp.errmsg = "请先获取手机验证码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (smsVerCode!=lastVerCode.VerificationCode)
            {
                resp.errcode = 3;
                resp.errmsg = "手机验证码错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            int count = 0;
            if (model.Rank>=1&&model.Rank<=10)
            {
                count = 4;//1-到10名 发4张
            }
            if (model.Rank >= 11 && model.Rank <= 100)
            {
                count =2;//11-到100名 发2张
            }
            if (model.Rank >= 101 && model.Rank <= 1000)
            {
                count =1;//101-到1000名 发1张
            }
            for (int i =1; i <=count; i++)
            {
                string Msg="";
                if (bllCardCoupon.ReciveCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, 1, CurrentUserInfo.UserID, out Msg))
                {
                    System.Threading.Thread.Sleep(1000);

                }
                else
                {
                    resp.errcode = 3;
                    resp.errmsg =Msg;
                    return Common.JSONHelper.ObjectToJson(resp);
                }

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