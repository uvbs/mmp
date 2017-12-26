using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Activity
{
    /// <summary>
    /// 活动处理文件
    /// </summary>
    public class ActivityHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllActivity.GetCurrentUserInfo();
                string action = context.Request["Action"];

                switch (action)
                {
                    case "GetActivityInfo":
                        result = GetActivityInfo(context);
                        break;
                    case "GetActivityDataInfo":
                        result = GetActivityDataInfo(context);
                        break;
                    case "GroupSendSms":
                        result = GroupSendSms(context);
                        break;
                    case "GetActivityItemList":
                        result = GetJuActivityItemList(context);
                        break;
                    case "GetMyActivityList":
                        result = GetMyActivityList(context);
                        break;
                    case "ReceiveActivity"://接收活动转赠
                        result = ReceiveActivity(context);
                        break;

                }
            }
            catch (Exception ex)
            {

                this.resp.Status = -1;
                this.resp.Msg = "异常" + ex.Message;

                result = ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
            }

            context.Response.Write(result);
        }

        private string GetActivityDataInfo(HttpContext context)
        {


            throw new NotImplementedException();
        }

        /// <summary>
        /// 群发短信
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GroupSendSms(HttpContext context)
        {
            if (this.currentUserInfo.IsPhoneVerify == 0)
            {
                resp.Status = 0;
                resp.Msg = "手机号码验证后才能进行改操作!";
                return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
            }

            string activityId = context.Request["ActivityID"];
            string smsContent = context.Request["SmsContent"];

            if (string.IsNullOrWhiteSpace(activityId) || string.IsNullOrWhiteSpace(smsContent))
            {
                resp.Status = 0;
                resp.Msg = "活动ID短信内容都不能为空!";
                return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
            }

            ActivityInfo activityModel = this.bllActivity.Get<ActivityInfo>(string.Format(" ActivityID = '{0}' and UserID = '{1}' ",
                    activityId,
                    this.currentUserInfo.UserID
                ));

            if (activityModel != null)
            {

                if (activityModel.GroupSendSmsCount >= 2)
                {
                    resp.Status = 0;
                    resp.Msg = "该活动两次免费群发资格已用完!";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
                }

                //取得短信发送列表
                List<ActivityDataInfo> dataList = this.bllActivity.GetList<ActivityDataInfo>(string.Format(" ActivityID = '{0}' and Phone is not null ", activityId));

                if (dataList.Count == 0)
                {
                    resp.Status = 0;
                    resp.Msg = "该活动没有可用号码!";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
                }

                List<BLLJIMP.Model.SMSTriggerDetails> smsList = new List<BLLJIMP.Model.SMSTriggerDetails>();
                foreach (var item in dataList)
                {
                    //构造发送明细数据
                    BLLJIMP.Model.SMSTriggerDetails sms = new BLLJIMP.Model.SMSTriggerDetails()
                    {
                        UserID = this.currentUserInfo.UserID,
                        Receiver = item.Phone,
                        SendContent = smsContent.Replace("{姓名}", item.Name),
                        SubmitDate = DateTime.Now,
                        SubmitStatus = 0
                    };
                    smsList.Add(sms);
                }

                for (int i = 0; i < smsList.Count; i++)
                {
                    try
                    {
                        //发送短信

                        //构造发送url
                        string url = string.Format("{0}?userName={1}&userPwd={2}&mobile={3}&content={4}&pipeID=membertrigger",
                                SessionKey.systemset.SysSmsApi,
                                SessionKey.systemset.SysSmsUserID,
                                SessionKey.systemset.SysSmsUserPwd,
                                smsList[i].Receiver,
                                smsList[i].SendContent
                            );

                        //提交短信
                        smsList[i].SubmitStatusORG = Common.MySpider.GetPageSourceForUTF8(url);

                        if (smsList[i].SubmitStatusORG == "0")
                            smsList[i].SubmitStatus = 1;
                        else
                            smsList[i].SubmitStatus = -1;

                    }
                    catch (Exception ex)
                    {
                        smsList[i].OtherDescription = ex.Message;
                        smsList[i].SubmitStatus = -1;
                    }
                }

                //发送完成更新短信状态并入库
                this.bllActivity.AddList(smsList);
                activityModel.GroupSendSmsCount++;
                this.bllActivity.Update(activityModel);
                resp.Status = 1;
                resp.Msg = "短信群发成功!";

                return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "活动不存在或者活动不属于当前用户!";
                return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
            }

        }

        /// <summary>
        /// 获取活动信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityInfo(HttpContext context)
        {
            string activityId = context.Request["ActivityID"];

            ActivityInfo activityModel = this.bllActivity.Get<ActivityInfo>(string.Format(" ActivityID = '{0}' and UserID = '{1}' ",
                    activityId,
                    this.currentUserInfo.UserID
                ));

            if (activityModel != null)
            {
                resp.Status = 1;
                resp.ExObj = activityModel;
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "活动不存在或者活动不属于当前用户!";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);

        }

        /// <summary>
        /// 获取活动信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetJuActivityItemList(HttpContext context)
        {
            string juActivityId = context.Request["JuActivityId"];

            resp.ExObj = bllActivity.GetList<CrowdFundItem>(string.Format(" CrowdFundID='{0}' And ItemType='Activity'", juActivityId));
            return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);

        }

        /// <summary>
        /// 接收活动转赠
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReceiveActivity(HttpContext context)
        {
            if (currentUserInfo==null)
            {
                resp.Msg = "请在微信中打开";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string activityId = context.Request["ActivityId"];//活动ID
            string fromUserAutoId = context.Request["FromUserAutoId"];//赠送用户ID
            JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivityByActivityID(activityId);

            //检查
            if (juActivityInfo == null)
            {
                resp.Msg = "转赠活动不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            UserInfo fromUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(fromUserAutoId));
            if (fromUserInfo == null)
            {
                resp.Msg = "转赠用户不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            WXSignInInfo signInfo = bllActivity.Get<WXSignInInfo>(string.Format(" JuActivityID='{0}' And SignInUserID='{1}'", juActivityInfo.JuActivityID, fromUserInfo.UserID));
            if (signInfo!=null)
            {
                resp.Msg = "不能接受此转赠";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            ActivityDataInfo dataInfo = bllActivity.Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And UserId='{1}' And IsDelete=0 And OrderId!=''  And PaymentStatus=1", activityId, fromUserInfo.UserID));

            if (dataInfo == null)
            {
                resp.Msg = " 不能接受此转赠";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (fromUserInfo.UserID==currentUserInfo.UserID)
            {
                resp.Msg = "不能接收自己的转赠";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(dataInfo.ToUserId))
            {
                if (dataInfo.ToUserId == currentUserInfo.UserID)
                {
                    resp.Msg = " 您已经接收过转赠";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                else
                {
                    resp.Msg = " 此活动已经转赠过了";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            ActivityDataInfo dataCurrentInfo = bllActivity.Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And UserId='{1}' And IsDelete=0", activityId,currentUserInfo.UserID));
            if (dataCurrentInfo!=null)
            {
                    resp.Msg = " 您已经报名过此活动,不能再接受转赠";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            WXMallOrderInfo fromUserOrderInfo = bllMall.GetOrderInfo(dataInfo.OrderId);
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                //订单
                WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
                orderInfo.Consignee = bllUser.GetUserDispalyName(currentUserInfo);
                orderInfo.InsertDate = DateTime.Now;
                orderInfo.OrderUserID = currentUserInfo.UserID;
                orderInfo.Phone = currentUserInfo.Phone;
                orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
                orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                orderInfo.MyCouponCardId = fromUserOrderInfo.MyCouponCardId;
                orderInfo.UseScore = fromUserOrderInfo.UseScore;
                orderInfo.Status = "待发货";
                orderInfo.ArticleCategoryType = "Mall";
                orderInfo.OrderType = 4;
                orderInfo.Ex1 = juActivityInfo.ActivityName;
                orderInfo.Ex2 = orderInfo.Ex2;
                orderInfo.Ex3 = orderInfo.Ex3;
                orderInfo.OrderMemo = orderInfo.OrderMemo;
                orderInfo.TotalAmount = fromUserOrderInfo.TotalAmount;
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;

                //订单
                if (!bllMall.Add(orderInfo,tran))
                {
                    tran.Rollback();
                    resp.Msg = " 插入订单表失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                ActivityDataInfo newData = new ActivityDataInfo();
                newData.ActivityID = dataInfo.ActivityID;
                newData.UserId = currentUserInfo.UserID;
                newData.WebsiteOwner = bllUser.WebsiteOwner;
                newData.OrderId =orderInfo.OrderID;
                newData.PaymentStatus = 1;
                newData.Name = bllUser.GetUserDispalyName(currentUserInfo);
                newData.Phone = currentUserInfo.Phone;
                newData.FromUserId = fromUserInfo.UserID;
                newData.InsertDate = DateTime.Now;
                newData.UID = bllJuactivity.Get<ActivityDataInfo>(string.Format(" ActivityID='{0}'  Order By UID DESC",activityId)).UID + 1;
                if (!bllJuactivity.Add(newData, tran))
                {
                    tran.Rollback();
                    resp.Msg = " 插入报名表失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                dataInfo.ToUserId = currentUserInfo.UserID;
                if (!bllJuactivity.Update(dataInfo,tran))
                {
                    tran.Rollback();
                    resp.Msg = " 转赠失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }


                string showName = "活动";
                var config = bllActivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllActivity.WebsiteOwner));
                if (config!=null)
                {
                    if (!string.IsNullOrEmpty(config.ShowName))
                    {
                        showName = config.ShowName;
                    }
                }

                bllWeixin.SendTemplateMessageNotifyComm(fromUserInfo, string.Format("{0}转赠通知",showName), string.Format(" {0}已接收你转赠的{1}{2}", bllUser.GetUserDispalyName(currentUserInfo),showName,juActivityInfo.ActivityName));

                bllWeixin.SendTemplateMessageNotifyComm(currentUserInfo, string.Format("{0}接收通知",showName), string.Format(" 您已接收了{0}转赠的{1}{2}", bllUser.GetUserDispalyName(fromUserInfo), showName,juActivityInfo.ActivityName));
               

                tran.Commit();
                resp.Status = 1;
                resp.Msg = "ok";
            }
            catch (Exception ex)
            {
                resp.Msg = ex.Message;
                tran.Rollback();

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取我的活动
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyActivityList(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string activityStatus = context.Request["activityStatus"];//活动状态 即将开始 已签到 已结束 待支付

            List<ActivityDataInfo> signUpDataList = bllActivity.GetList<ActivityDataInfo>(string.Format(" (UserId='{0}' Or WeixinOpenID='{1}') And IsDelete=0", currentUserInfo.UserID, currentUserInfo.WXOpenId));
            string signUpActivityIds = "0";
            if (signUpDataList.Count > 0)
            {
                foreach (var item in signUpDataList)
                {
                    signUpActivityIds += item.ActivityID + ",";
                }

            }
            signUpActivityIds = signUpActivityIds.TrimEnd(',');
            List<JuActivityInfo> myJuActivityList = bllActivity.GetList<JuActivityInfo>(string.Format(" SignUpActivityId in({0})", signUpActivityIds));
            myJuActivityList = myJuActivityList.OrderByDescending(p=>p.ActivityStartDate).ToList();
            List<MyActivity> list = new List<MyActivity>();
            switch (activityStatus)
            {
                case "即将开始"://进行中的活动也包括在内
                    foreach (var item in myJuActivityList)
                    {
                        if (item.IsHide == 0 || (item.ActivityStartDate != null && ((DateTime)item.ActivityStartDate) > DateTime.Now))
                        {
                            MyActivity model = new MyActivity();
                            model.ActivityName = item.ActivityName;
                            model.ActivityImage = item.ThumbnailsPath;
                            model.Address = item.ActivityAddress;
                            model.IsFee = item.IsFee;
                            if (model.IsFee == 1)
                            {
                                model.PaymentStatus = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).PaymentStatus;
                                model.OrderId = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).OrderId;
                                if (!string.IsNullOrEmpty(model.OrderId))
                                {
                                    model.Amount = bllMall.GetOrderInfo(model.OrderId).TotalAmount;
                                }
                            }
                            model.StartTime = item.ActivityStartDate != null ? (item.ActivityStartDate.ToString()) : "";
                            model.StopTime = item.ActivityEndDate != null ? (item.ActivityEndDate.ToString()) : "";
                            model.JuactivityIdHex = item.JuActivityIDHex;
                            model.SignUpActivityID = item.SignUpActivityID;
                            model.ActivityStatus = item.ActivityStatus;
                            model.JuactivityId = item.JuActivityID;
                            list.Add(model);


                        }

                    }
                    break;
                case "已签到":
                    List<JuActivityInfo> mySignInJuactivityList = new List<JuActivityInfo>();
                    List<WXSignInInfo> signList = bllActivity.GetList<WXSignInInfo>(string.Format(" SignInUserID='{0}'", currentUserInfo.UserID));
                    foreach (var item in signList)
                    {
                        var signModel = myJuActivityList.SingleOrDefault(p => p.JuActivityID == item.JuActivityID);
                        if (signModel != null)
                        {
                            mySignInJuactivityList.Add(signModel);
                        }

                    }

                    foreach (var item in mySignInJuactivityList)
                    {

                        MyActivity model = new MyActivity();
                        model.ActivityName = item.ActivityName;
                        model.ActivityImage = item.ThumbnailsPath;
                        model.Address = item.ActivityAddress;
                        model.IsFee = item.IsFee;
                        if (model.IsFee == 1)
                        {
                            model.PaymentStatus = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).PaymentStatus;
                            model.OrderId = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).OrderId;
                            if (!string.IsNullOrEmpty(model.OrderId))
                            {
                                model.Amount = bllMall.GetOrderInfo(model.OrderId).TotalAmount;
                            }
                        }
                        model.StartTime = item.ActivityStartDate != null ? (item.ActivityStartDate.ToString()) : "";
                        model.StopTime = item.ActivityEndDate != null ? (item.ActivityEndDate.ToString()) : "";
                        model.JuactivityIdHex = item.JuActivityIDHex;
                        model.SignUpActivityID = item.SignUpActivityID;
                        model.ActivityStatus = item.ActivityStatus;
                        model.JuactivityId = item.JuActivityID;
                        list.Add(model);

                    }


                    break;
                case "已结束":
                    foreach (var item in myJuActivityList)
                    {
                        if ((item.ActivityEndDate != null && ((DateTime)item.ActivityEndDate) <= DateTime.Now) || item.IsHide == 1)
                        {
                            MyActivity model = new MyActivity();
                            model.ActivityName = item.ActivityName;
                            model.ActivityImage = item.ThumbnailsPath;
                            model.Address = item.ActivityAddress;
                            model.IsFee = item.IsFee;
                            if (model.IsFee == 1)
                            {
                                model.PaymentStatus = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).PaymentStatus;
                                model.OrderId = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).OrderId;
                                if (!string.IsNullOrEmpty(model.OrderId))
                                {
                                    model.Amount = bllMall.GetOrderInfo(model.OrderId).TotalAmount;
                                }
                            }
                            model.StartTime = item.ActivityStartDate != null ? (item.ActivityStartDate.ToString()) : "";
                            model.StopTime = item.ActivityEndDate != null ? (item.ActivityEndDate.ToString()) : "";
                            model.JuactivityIdHex = item.JuActivityIDHex;
                            model.SignUpActivityID = item.SignUpActivityID;
                            model.ActivityStatus = item.ActivityStatus;
                            model.JuactivityId = item.JuActivityID;
                            list.Add(model);


                        }

                    }

                    break;
                case "待支付":
                    signUpDataList = signUpDataList.Where(p => p.OrderId != "").Where(p => p.PaymentStatus == 0).ToList();
                    signUpActivityIds = "0";
                    if (signUpDataList.Count > 0)
                    {
                        foreach (var item in signUpDataList)
                        {
                            signUpActivityIds += item.ActivityID + ",";
                        }

                    }
                    signUpActivityIds = signUpActivityIds.TrimEnd(',');
                    myJuActivityList = bllActivity.GetList<JuActivityInfo>(string.Format(" SignUpActivityId in({0})", signUpActivityIds)).Where(p => p.IsFee == 1).ToList();
                    foreach (var item in myJuActivityList)
                    {

                        MyActivity model = new MyActivity();
                        model.ActivityName = item.ActivityName;
                        model.ActivityImage = item.ThumbnailsPath;
                        model.Address = item.ActivityAddress;
                        model.IsFee = item.IsFee;
                        if (model.IsFee == 1)
                        {
                            model.PaymentStatus = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).PaymentStatus;
                            model.OrderId = signUpDataList.First(p => p.ActivityID == item.SignUpActivityID).OrderId;
                            if (!string.IsNullOrEmpty(model.OrderId))
                            {
                                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(model.OrderId);
                                model.Amount = orderInfo.TotalAmount;
                                model.ItemName = orderInfo.Ex2;
                                model.ItemAmount = orderInfo.Ex3;
                            }

                        }
                        model.StartTime = item.ActivityStartDate != null ? (item.ActivityStartDate.ToString()) : "";
                        model.StopTime = item.ActivityEndDate != null ? (item.ActivityEndDate.ToString()) : "";
                        model.JuactivityIdHex = item.JuActivityIDHex;
                        model.SignUpActivityID = item.SignUpActivityID;
                        model.ActivityStatus = item.ActivityStatus;
                        model.JuactivityId = item.JuActivityID;
                        list.Add(model);


                    }
                    break;
                default:
                    break;
            }
            list = list.OrderBy(p=>p.ActivityStatus).ToList();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            resp.Result = list;
            resp.IsSuccess = true;
            resp.Msg = "ok";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 我的活动模型
        /// </summary>
        public class MyActivity
        {
            /// <summary>
            /// 活动名称
            /// </summary>
            public string ActivityName { get; set; }
            /// <summary>
            /// 活动图片
            /// </summary>
            public string ActivityImage { get; set; }
            /// <summary>
            /// 是否收费 0免费 1收费
            /// </summary>
            public int IsFee { get; set; }
            /// <summary>
            /// 支付状态
            /// 0 未支付
            /// 1 已支付
            /// </summary>
            public int PaymentStatus { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string StopTime { get; set; }
            /// <summary>
            /// 活动状态
            /// </summary>
            public int ActivityStatus { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderId { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// 活动十六进制
            /// </summary>
            public string JuactivityIdHex { get; set; }
            public int JuactivityId { get; set; }
            /// <summary>
            /// 选项名称
            /// </summary>
            public string ItemName { get; set; }
            /// <summary>
            /// 选项金额
            /// </summary>
            public string ItemAmount { get; set; }
            /// <summary>
            /// 报名活动ID
            /// </summary>
            public string SignUpActivityID { get; set; }


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