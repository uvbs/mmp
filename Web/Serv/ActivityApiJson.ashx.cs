using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.Common;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.IO;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 提交报名数据接口
    /// </summary>
    public class ActivityApiJson : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 当前访问的用户信息
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 聚活动信息
        /// </summary>
        JuActivityInfo juActivityInfo;
        /// <summary>
        /// 真实活动信息
        /// </summary>
        //ActivityInfo activity;
        /// <summary>
        /// 当前请求参数键值对
        /// </summary>
        Dictionary<string, string> dicPar;
        /// <summary>
        /// 逻辑
        /// </summary>
        BLLActivity bll = new BLLActivity("");
        /// <summary>
        /// 微信逻辑
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 用户逻辑
        /// </summary>
        ZentCloud.BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        ///// <summary>
        ///// 线下分销
        ///// </summary>
        //ZentCloud.BLLJIMP.BLLDistributionOffLine bllDis = new BLLDistributionOffLine();
        /// <summary>
        /// 线上分销
        /// </summary>
        ZentCloud.BLLJIMP.BLLDistribution bllDisOnLine = new BLLDistribution();
        /// <summary>
        /// 
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {
                // UserInfo distributionOffLineCommendUser = null;//分销推荐人
                //bool isDistributionOffLineApply = false;//是否线下分销申请
                dicPar = bll.GetRequestParameter();
                string weixinOpenID = null;//微信openid
                string activityId = null;//活动编号
                dicPar.TryGetValue("ActivityID", out activityId);
                string spreadUserId = null;
                dicPar.TryGetValue("SpreadUserID", out spreadUserId);//推广用户的用户名
                string shareUserId = string.Empty;
                dicPar.TryGetValue("ShareUserID", out shareUserId);
                string shareId = string.Empty;
                dicPar.TryGetValue("ShareID", out shareId);
                string strDistinctKeys = null;//检查重复的字段，多个字段用,分隔， //没有此参数默认用手机检查  
                dicPar.TryGetValue("DistinctKeys", out strDistinctKeys);
                string monitorPlanID = null;//监测任务ID
                dicPar.TryGetValue("MonitorPlanID", out monitorPlanID);
                string name = null;//姓名
                dicPar.TryGetValue("Name", out name);
                string phone = null;//手机
                dicPar.TryGetValue("Phone", out phone);
                string isSaveUserInfo = string.Empty;
                dicPar.TryGetValue("IsSaveUserInfo", out isSaveUserInfo);

                //activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", activityId));

                #region IP限制
                //获取用户IP;
                string userHostAddress = context.Request.UserHostAddress;
                var count = DataCache.GetCache(userHostAddress);
                if (count != null)
                {
                    int newCount = int.Parse(count.ToString()) + 1;
                    DataCache.SetCache(userHostAddress, newCount);
                    int limitCount = 1000;
                    //if (activity != null)
                    //{

                    //    //limitCount = activity.LimitCount;

                    //}
                    if (newCount >= limitCount)
                    {

                        resp.Status = 14;
                        resp.Msg = "您的提交过于频繁，请稍后再试";
                        goto outoff;



                    }
                }
                else
                {
                    DataCache.SetCache(userHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
                }


                #endregion

                #region 判断必填项
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
                {
                    resp.Status = 3;
                    resp.Msg = "姓名和手机不能为空!";
                    goto outoff;

                }
                //if ((!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))&&(!Phone.Equals("systemdefault")))//系统缺省手机号不检查
                //{
                //    resp.Status = 5;
                //    resp.Msg = "手机号码无效!";
                //    goto outoff;
                //}
                if (((!phone.StartsWith("1")) || (!phone.Length.Equals(11))) && (!phone.Equals("systemdefault")))//系统缺省手机号不检查
                {
                    resp.Status = 5;
                    resp.Msg = "手机号码无效!";
                    goto outoff;
                }
                if (string.IsNullOrEmpty(activityId))
                {
                    resp.Status = 6;
                    resp.Msg = "活动编号不能为空!";
                    goto outoff;

                }

                //手机注册的重复数据
                UserInfo dicUser = bllUser.Get<UserInfo>(string.Format(" WebsiteOwner='{0}' AND Phone='{1}' AND WXOpenId is null ", bllUser.WebsiteOwner, phone));
                if (dicUser != null)
                {
                    dicUser.Phone3 = phone;
                    dicUser.Phone = "";
                    bllUser.Update(dicUser);
                }



                #endregion

                #region 活动权限验证
                //if (activity == null)
                //{
                //    resp.Status = 7;
                //    resp.Msg = "活动编号不存在!";
                //    goto outoff;
                //}
                //if (activity.ActivityStatus.Equals(0))
                //{
                //    resp.Status = 8;
                //    resp.Msg = "活动已关闭!";
                //    goto outoff;
                //}

                //if (activity.IsDelete.Equals(1))
                //{
                //    resp.Status = -1;
                //    resp.Msg = "活动已删除!";
                //    goto outoff;
                //}
                #endregion

                #region 是否可以报名
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                    if (!string.IsNullOrEmpty(currentUserInfo.WXOpenId))
                    {
                        weixinOpenID = currentUserInfo.WXOpenId;
                    }
                }
                juActivityInfo = bll.Get<JuActivityInfo>(string.Format("SignUpActivityID={0}", dicPar["ActivityID"]));
                if (juActivityInfo != null)
                {
                    #region 活动已经停止
                    if (juActivityInfo.IsHide.Equals(1))
                    {
                        resp.Status = 14;
                        resp.Msg = "活动已停止";
                        goto outoff;

                    }
                    #endregion

                    #region 检查报名人数
                    if (juActivityInfo.MaxSignUpTotalCount > 0)//检查报名人数
                    {
                        if (juActivityInfo.SignUpTotalCount > (juActivityInfo.MaxSignUpTotalCount - 1))
                        {
                            resp.Status = 15;
                            resp.Msg = "报名人数已满";
                            goto outoff;

                        }

                    }
                    #endregion

                    #region 检查积分
                    if (juActivityInfo.ActivityIntegral > 0)
                    {
                        if (!bll.IsLogin)
                        {
                            resp.Status = 12;
                            resp.Msg = "此活动必须登录才能报名";
                            goto outoff;

                        }
                        else
                        {

                            if (currentUserInfo.TotalScore < juActivityInfo.ActivityIntegral)
                            {
                                resp.Status = 13;
                                resp.Msg = "您的积分不足,无法报名";
                                goto outoff;

                            }
                        }
                    }
                    #endregion

                    #region 指定标签的用户可以报名
                    if (!string.IsNullOrEmpty(juActivityInfo.Tags))
                    {
                        if (!bll.IsLogin)
                        {
                            resp.Status = 12;
                            resp.Msg = "此活动必须登录才能报名";
                            goto outoff;
                        }
                        if (string.IsNullOrEmpty(currentUserInfo.TagName))
                        {
                            resp.Status = 12;
                            resp.Msg = "此活动只接受特定用户的报名";
                            goto outoff;
                        }
                        bool checkResult = false;
                        foreach (string item in currentUserInfo.TagName.Split(','))
                        {
                            if (juActivityInfo.Tags.Contains(item))
                            {
                                checkResult = true;
                                break;
                            }
                        }
                        if (!checkResult)
                        {
                            resp.Status = 12;
                            resp.Msg = "此活动只接受特定用户的报名";
                            goto outoff;
                        }


                    }
                    #endregion

                }
                #endregion

                #region 检查自定义必填项
                List<ActivityFieldMappingInfo> requiredFieldList = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And FieldIsNull=1", juActivityInfo.SignUpActivityID));
                if (requiredFieldList.Count > 0)
                {
                    foreach (var requiredField in requiredFieldList)
                    {
                        if (string.IsNullOrEmpty(dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", requiredField.ExFieldIndex))).Value))
                        {
                            resp.Status = 11;
                            resp.Msg = string.Format(" {0} 必填", requiredField.MappingName);
                            goto outoff;

                        }
                    }
                }
                #endregion

                #region 检查数据格式
                //检查数据格式
                List<ActivityFieldMappingInfo> activityFieldMappingList = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", juActivityInfo.SignUpActivityID));
                foreach (var item in activityFieldMappingList)
                {

                    string value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    //检查数据格式
                    if (item.FormatValiFunc == "email")//email检查
                    {
                        if (!Common.ValidatorHelper.EmailLogicJudge(value))
                        {
                            resp.Status = 12;
                            resp.Msg = string.Format("{0}格式不正确", item.MappingName);
                            goto outoff;

                        }
                    }
                    if (item.FormatValiFunc == "url")//url检查
                    {
                        System.Text.RegularExpressions.Regex regUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                        System.Text.RegularExpressions.Match match = regUrl.Match(value);
                        if (!match.Success)
                        {
                            resp.Status = 13;
                            resp.Msg = string.Format("{0}格式不正确", item.MappingName);
                            goto outoff;

                        }
                    }
                }
                #endregion

                #region 检查是否已经报名

                //if (activityId != bllDis.GetDistributionOffLineApplyActivityID())//线下分销不检查
                //{
                if (!string.IsNullOrEmpty(strDistinctKeys))
                {

                    if (!strDistinctKeys.Equals("none"))//自定义检查重复
                    {
                        System.Text.StringBuilder sbWhere = new System.Text.StringBuilder("1=1 ");
                        string[] distinctKeys = strDistinctKeys.Split(',');
                        foreach (var item in distinctKeys)
                        {
                            sbWhere.AppendFormat("And {0}='{1}' ", item, dicPar.Single(p => p.Key.Equals(item)).Value);
                        }
                        sbWhere.Append("  and IsDelete = 0  ");
                        if (bll.GetCount<ActivityDataInfo>(sbWhere.ToString()) > 0)
                        {

                            resp.Status = 1;
                            resp.Msg = "重复的报名!";
                            goto outoff;


                        }

                    }
                    else//不检查重复
                    {

                    }



                }
                else//默认检查
                {
                    if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}' and IsDelete = 0 ", activityId, phone)) > 0)
                    {
                        resp.Status = 1;
                        resp.Msg = "已经报过名了!";
                        goto outoff;


                    }
                }
                //}





                #endregion

                //if (activityId == bllDis.GetDistributionOffLineApplyActivityID())//如果当前活动是分销
                //{
                //    if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And UserId='{1}' and IsDelete = 0 ", activityId, currentUserInfo.UserID)) > 0)
                //    {
                //        bll.Update(new ActivityDataInfo(), string.Format("IsDelete=1"), string.Format(" ActivityID='{0}' And UserId='{1}'", activityId, currentUserInfo.UserID));
                //    }
                //}

                //#region 检查手机验证码 特殊化
                ////if (ActivityID.Equals("186135"))
                ////{
                ////    string VerCode = HttpContext.Current.Session["SmsVerificationCode"] == null ? "" : HttpContext.Current.Session["SmsVerificationCode"].ToString();
                ////    if (string.IsNullOrEmpty(context.Request["SmsVerificationCode"]))
                ////    {
                ////        resp.Status = 12;
                ////        resp.Msg = "请输入手机验证码";
                ////        goto outoff;


                ////    }
                ////    if (!context.Request["SmsVerificationCode"].ToString().Equals(VerCode))
                ////    {
                ////            resp.Status = 13;
                ////            resp.Msg = "手机验证码不正确";
                ////            goto outoff;
                ////    }
                ////} 
                //#endregion

                var newActivityUID = 1001;
                var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", activityId));
                if (lastActivityDataInfo != null)
                {
                    newActivityUID = lastActivityDataInfo.UID + 1;
                }
                ActivityDataInfo model = bll.ConvertRequestToModel<ActivityDataInfo>(new ActivityDataInfo());
                model.UID = newActivityUID;
                model.WeixinOpenID = weixinOpenID;
                model.SpreadUserID = spreadUserId;
                if (spreadUserId == currentUserInfo.UserID)
                {
                    model.SpreadUserID = bll.WebsiteOwner;
                }
                if (!string.IsNullOrEmpty(monitorPlanID))
                {
                    model.MonitorPlanID = int.Parse(monitorPlanID);
                }
                model.ShareUserID = string.IsNullOrWhiteSpace(shareUserId) ? "" : shareUserId;
                model.ShareID = string.IsNullOrWhiteSpace(shareId) ? "" : shareId;
                model.WebsiteOwner = bll.WebsiteOwner;
                if (bll.IsLogin)
                {
                    model.UserId = bll.GetCurrUserID();
                }

                //保存分销推荐码
                // model.DistributionOffLineRecommendCode = context.Request["DistributionOffLineRecommendCode"];


                //if (!string.IsNullOrWhiteSpace(model.DistributionOffLineRecommendCode))
                //{
                //    bool isTrueCode = false;
                //    int userAutoId = 0;
                //    if (int.TryParse(model.DistributionOffLineRecommendCode, out userAutoId))
                //    {
                //        distributionOffLineCommendUser = bllUser.GetUserInfoByAutoID(userAutoId);

                //        if (distributionOffLineCommendUser != null)
                //        {
                //            //是站点用户或者是当前站点的分销员才有效
                //            if (bll.WebsiteOwner.ToLower() == distributionOffLineCommendUser.UserID.ToLower())
                //            {
                //                isTrueCode = true;
                //            }
                //            else if (distributionOffLineCommendUser.WebsiteOwner.ToLower() == bll.WebsiteOwner.ToLower() && !string.IsNullOrWhiteSpace(distributionOffLineCommendUser.DistributionOffLinePreUserId))
                //            {
                //                isTrueCode = true;
                //            }
                //        }
                //    }

                //    if (!isTrueCode)
                //    {
                //        resp.Status = 1;
                //        resp.Msg = "不是有效的推荐码";
                //        goto outoff;
                //    }
                //    else
                //    {
                //        isDistributionOffLineApply = true;
                //    }

                //    ToLog("isDistributionOffLineApply:" + isDistributionOffLineApply);

                //}


                if (bll.Add(model))
                {
                    try
                    {
                        //#region 业务分销员申请消息
                        ////发送模板消息跟系统微客服和他的上级
                        //ToLog("isDistributionOffLineApply:" + isDistributionOffLineApply);
                        //if (isDistributionOffLineApply)
                        //{
                        //    ToLog("申请成为财富会员,发送消息给系统管理员");

                        //    //发送消息给他上级
                        //    //bllWeixin.SendTemplateMessageNotifyComm(distributionOffLineCommendUser.WXOpenId, "财富伙伴申请", string.Format("姓名:{0}\\n手机号:{1}", model.Name, model.Phone));

                        //    //发送消息给系统管理员
                        //    // WXKeFu kefuInfo = bllWeixin.GetDefaultKefu();
                        //    //ToLog("客服实体:" + JSONHelper.ObjectToJson(kefuInfo));
                        //    bllWeixin.SendTemplateMessageToKefu(string.Format("用户“{0}”申请成为财富会员", model.Name), string.Format("姓名:{0}\\n手机号:{1}", model.Name, model.Phone));

                        //}
                        //#endregion

                        #region 个人资料补足
                        if (bll.IsLogin)
                        {
                            //个人资料补足
                            bool isEdit = false;
                            isSaveUserInfo = "1";
                            if (string.IsNullOrWhiteSpace(currentUserInfo.TrueName))
                            {
                                isEdit = true;
                                currentUserInfo.TrueName = model.Name;
                            }
                            if (string.IsNullOrWhiteSpace(currentUserInfo.Phone))
                            {
                                isEdit = true;
                                currentUserInfo.Phone = model.Phone;
                            }
                            List<BLLJIMP.Model.ActivityFieldMappingInfo> mapingList = this.bll.GetList<BLLJIMP.Model.ActivityFieldMappingInfo>(string.Format(" ActivityID = '{0}' ", model.ActivityID));

                            foreach (var item in mapingList)
                            {
                                string value = "";
                                if (item.MappingName.IndexOf("邮箱") > -1 || item.MappingName.IndexOf("邮件") > -1 || item.MappingName.ToLower().IndexOf("email") > -1)
                                {
                                    isEdit = true;

                                    value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;
                                    if (string.IsNullOrWhiteSpace(currentUserInfo.Email))
                                        currentUserInfo.Email = value;
                                    continue;
                                }

                                if (item.MappingName.IndexOf("职位") > -1)
                                {
                                    isEdit = true;
                                    value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;
                                    if (string.IsNullOrWhiteSpace(currentUserInfo.Postion))
                                        currentUserInfo.Postion = value;
                                    continue;
                                }

                                if (item.MappingName.IndexOf("公司") > -1)
                                {
                                    isEdit = true;
                                    value = dicPar.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;
                                    if (value.Length >= 255)
                                    {
                                        resp.Status = 3;
                                        resp.Msg = "公司名称过长!";
                                        goto outoff;
                                    }
                                    if (string.IsNullOrWhiteSpace(currentUserInfo.Company))
                                        currentUserInfo.Company = value;
                                    continue;
                                }
                            }


                            if (isEdit)
                            {
                                this.bll.Update(new UserInfo(),
                                        string.Format(" TrueName='{0}',Phone='{1}',Email='{2}',Postion='{3}',Company='{4}'  ",
                                                currentUserInfo.TrueName,
                                                currentUserInfo.Phone,
                                                currentUserInfo.Email,
                                                currentUserInfo.Postion,
                                                currentUserInfo.Company
                                            ),
                                        string.Format(" AutoID = {0} ", this.currentUserInfo.AutoID)
                                    );
                            }

                            //if (isEdit && !string.IsNullOrWhiteSpace(isSaveUserInfo))
                            //{
                            //    if (isSaveUserInfo == "1")
                            //    {
                            //        this.bll.Update(new UserInfo(),
                            //                string.Format(" TrueName='{0}',Phone='{1}',Email='{2}',Postion='{3}',Company='{4}'  ",
                            //                        currentUserInfo.TrueName,
                            //                        currentUserInfo.Phone,
                            //                        currentUserInfo.Email,
                            //                        currentUserInfo.Postion,
                            //                        currentUserInfo.Company
                            //                    ),
                            //                string.Format(" AutoID = {0} ", this.currentUserInfo.AutoID)
                            //            );
                            //    }
                            //}
                            //if (bll.IsLogin)
                            //{
                            //    var websiteInfo = bll.GetWebsiteInfoModelFromDataBase();
                            //    if (websiteInfo.IsSynchronizationData != null)
                            //    {
                            //        if (websiteInfo.IsSynchronizationData.Value == 1)
                            //        {

                            //            bllUser.Update(currentUserInfo);
                            //        }
                            //    }
                            //}


                        }
                        #endregion

                        #region 检查是否向微信客服号发送通知信息

                        if (juActivityInfo != null)
                        {

                            //if (!string.IsNullOrEmpty(juActivityInfo.ActivityNoticeKeFuId))
                            //{

                            //    WXKeFu keFuInfo = bll.Get<WXKeFu>(string.Format("AutoID={0}", juActivityInfo.ActivityNoticeKeFuId));
                            //    if (keFuInfo != null)
                            //    {

                            //        #region 发送模板消息给客服
                            //        if (juActivityInfo.ArticleType != "greetingcard")
                            //        {
                            //            bllWeixin.SendTemplateMessageNotifyComm(keFuInfo.WeiXinOpenID, string.Format("{0}有新的报名", juActivityInfo.ActivityName), string.Format("姓名:{0}\\n手机号:{1}", model.Name, model.Phone));
                            //        }
                            //        #endregion


                            //    }
                            //}

                            #region 发送模板消息给客服
                            if (juActivityInfo.ArticleType != "greetingcard")
                            {
                                bllWeixin.SendTemplateMessageToKefu(string.Format("{0}有新的报名", juActivityInfo.ActivityName), string.Format("姓名:{0}\\n手机号:{1}", model.Name, model.Phone));
                            }
                            #endregion

                            juActivityInfo.SignUpCount = juActivityInfo.SignUpTotalCount + 1;
                            bll.Update(juActivityInfo);


                        }




                        #endregion

                        #region 更新转发表报名数量
                        MonitorLinkInfo linkModel = bll.Get<MonitorLinkInfo>(string.Format(" MonitorPlanID={0} AND LinkName='{1}' ", monitorPlanID, spreadUserId));
                        int signupCount = bll.GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", int.Parse(monitorPlanID), spreadUserId));
                        if (linkModel != null)
                        {
                            linkModel.ActivitySignUpCount = signupCount;
                            bll.Update(linkModel);
                        }
                        #endregion

                        //#region 向报名人发送公众号模板消息-仅五步会有效

                        //if (bll.WebsiteOwner == "wubuhui")
                        //{
                        //    if (bllWeixin.IsLogin)
                        //    {
                        //        string accessToken = bllWeixin.GetAccessToken();
                        //        if (accessToken != string.Empty)
                        //        {
                        //            BLLWeixin.TMSignupNotification notificaiton = new BLLWeixin.TMSignupNotification();

                        //            notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/Index.aspx", System.Web.HttpContext.Current.Request.Url.Host);

                        //            notificaiton.TemplateId = "-eJq0w8PEFQRLnWXwqKn73zWvAah6nJxYHgEK3L4Pek";
                        //            notificaiton.First = "恭喜您成功报名我们的活动";
                        //            notificaiton.Keynote1 = juActivityInfo.ActivityName;
                        //            notificaiton.Keynote2 = juActivityInfo.ActivityStartDate.ToString();
                        //            notificaiton.Keynote3 = juActivityInfo.ActivityAddress;
                        //            notificaiton.Remark = "欢迎您届时参加！";
                        //            bllWeixin.SendTemplateMessage(accessToken, currentUserInfo.WXOpenId, notificaiton);

                        //        }
                        //    }
                        //}



                        //#endregion

                        #region 发送模板消息给用户
                        if ((currentUserInfo != null))
                        {
                            if (juActivityInfo.ArticleType != "greetingcard" && juActivityInfo.IsFee == 0)
                            {
                                bllWeixin.SendTemplateMessageNotifyComm(currentUserInfo, string.Format("您已成功报名{0}", juActivityInfo.ActivityName), string.Format("姓名:{0}\\n手机号:{1}", model.Name, model.Phone));
                            }
                            if (juActivityInfo.IsFee == 1)//收费活动
                            {
                                if (model.Amount>0)
                                {
                                  bllWeixin.SendTemplateMessageNotifyComm(currentUserInfo, string.Format("您已报名{0}，该活动收费，请尽快完成支付。", juActivityInfo.ActivityName), "");

                                }
                            }
                        }
                        #endregion

                        #region 微转发自动成为线上分销员
                        if (!string.IsNullOrEmpty(model.SpreadUserID))
                        {
                            if (bll.IsLogin)
                            {
                                try
                                {
                                    WebsiteInfo websiteInfo = bll.GetWebsiteInfoModelFromDataBase();

                                    if (websiteInfo.DistributionRelationBuildSpreadActivity == 1)
                                    {
                                        UserInfo recommentUserInfo = bllUser.GetUserInfo
                                            (spreadUserId);
                                        //ActivityInfo disActivityInfo = bll.GetActivityInfoByActivityID(bllDis.GetDistributionOffLineApplyActivityID());
                                        //bllDis.AutoApply(currentUserInfo, activity, disActivityInfo, recommentUserInfo, model);
                                        if (bllUser.IsDistributionMember(recommentUserInfo) || (recommentUserInfo.UserID == websiteInfo.WebsiteOwner))
                                        {
                                            bllDisOnLine.SetUserDistributionOwner(currentUserInfo.UserID, recommentUserInfo.UserID, currentUserInfo.WebsiteOwner);
                                        }
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                    ToLog(ex.ToString());

                                }



                            }
                        }
                        #endregion

                        juActivityInfo = bllJuactivity.GetJuActivityByActivityID(activityId);

                    }
                    catch (Exception)
                    {


                    }
                    resp.Msg = "提交成功!";
                    //Dictionary<string, object> exObj = new Dictionary<string, object>();
                    //exObj.Add("Activity", Activity);
                    //exObj.Add("ActivityData", model);
                    //exObj.Add("ActivityFieldMapping", activityFieldMapping);
                    //resp.ExObj = exObj;
                    resp.ExInt = model.UID;
                    if (juActivityInfo != null)
                    {
                        resp.ExStr = juActivityInfo.ActivitySignuptUrl;
                    }

                    // 推荐报名活动加积分
                    if (!string.IsNullOrEmpty(spreadUserId))
                    {
                        if (spreadUserId != currentUserInfo.UserID)
                        {
                            UserInfo recommentUserInfo = bllUser.GetUserInfo(spreadUserId);
                            bllUser.AddUserScoreDetail(recommentUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.RecommendSignUpActivityAddScore), recommentUserInfo.WebsiteOwner, null, null);

                        }

                    }

                    #region MyRegion
                    if (!string.IsNullOrEmpty(context.Request["AutoSignIn"]))
                    {

                        if (context.Request["AutoSignIn"].ToString() == "1")
                        {

                            try
                            {


                                WXSignInInfo signInInfo = new WXSignInInfo();
                                signInInfo.SignInUserID = currentUserInfo.UserID;
                                signInInfo.Name = model.Name;
                                signInInfo.Phone = model.Phone;
                                signInInfo.JuActivityID = juActivityInfo.JuActivityID;
                                signInInfo.SignInOpenID = currentUserInfo.WXOpenId;
                                signInInfo.SignInTime = DateTime.Now;
                                //判断是否已经签到过
                                if (!bllJuactivity.Exists(signInInfo, new List<string>() { "SignInUserID", "JuActivityID" }))
                                {

                                    if (bllJuactivity.Add(signInInfo))
                                    {
                                        bllUser.AddUserScoreDetail(currentUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.SignIn), currentUserInfo.WebsiteOwner, null, null);


                                    }



                                }
                            }
                            catch (Exception)
                            {


                            }


                        }


                    } 
                    #endregion


                    goto outoff;

                }
                else
                {
                    resp.Status = 2;
                    resp.Msg = "报名失败，请重试或联系管理员!";
                    goto outoff;

                }
            }
            catch (Exception ex)
            {
                //using (StreamWriter sw = new StreamWriter(@"C:\log.txt", true, System.Text.Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(ex.Data);
                //}
                resp.Status = 10;
                resp.Msg = "报名失败：" + ex.ToString();
                goto outoff;
                //日志记录


            }
        outoff:
            context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
        }


        private void ToLog(string msg)
        {
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
            //    }
            //}
            //catch { }
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
