﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.Net;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// ActivityApi 的摘要说明
    /// </summary>
    public class ActivityApi : IHttpHandler, IRequiresSessionState
    {
        BLLActivity bll;
        HttpContext contentthis;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            try
            {
                this.contentthis = context;
                bll = new BLLActivity("");
                var CallBackUrl = GetPostParm("CallBackUrl");
                CallBackUrl = CallBackUrl.Contains("?") ? CallBackUrl + "&" : CallBackUrl + "?";
                var LoginName = GetPostParm("LoginName");
                var LoginPwd = GetPostParm("LoginPwd");
                var ActivityID = GetPostParm("ActivityID");
                var Name = GetPostParm("Name");
                var Phone = GetPostParm("Phone");
                var K1 = GetPostParm("K1");
                var K2 = GetPostParm("K2");
                var K3 = GetPostParm("K3");
                var K4 = GetPostParm("K4");
                var K5 = GetPostParm("K5");
                var K6 = GetPostParm("K6");
                var K7 = GetPostParm("K7");
                var K8 = GetPostParm("K8");
                var K9 = GetPostParm("K9");
                var K10 = GetPostParm("K10");
                var K11 = GetPostParm("K11");
                var K12 = GetPostParm("K12");
                var K13 = GetPostParm("K13");
                var K14 = GetPostParm("K14");
                var K15 = GetPostParm("K15");
                var K16 = GetPostParm("K16");
                var K17 = GetPostParm("K17");
                var K18 = GetPostParm("K18");
                var K19 = GetPostParm("K19");
                var K20 = GetPostParm("K20");

                var StrDistinctKeys = GetPostParm("DistinctKeys");//检查重复的字段，多个字段用,分隔， 没有此参数默认用手机检查
                var weixinOpenID = GetPostParm(SessionKey.WXCurrOpenerOpenIDKey);
                var SpreadUserID = Common.Base64Change.DecodeBase64ByUTF8(GetPostParm("SpreadUserID"));

                List<KeyValue> LstKeyValue = new List<KeyValue>();
                LstKeyValue.Add(new KeyValue("LoginName", LoginName));
                LstKeyValue.Add(new KeyValue("LoginPwd", LoginPwd));
                LstKeyValue.Add(new KeyValue("ActivityID", ActivityID));
                LstKeyValue.Add(new KeyValue("Name", Name));
                LstKeyValue.Add(new KeyValue("Phone", Phone));
                LstKeyValue.Add(new KeyValue("K1", K1));
                LstKeyValue.Add(new KeyValue("K2", K2));
                LstKeyValue.Add(new KeyValue("K3", K3));
                LstKeyValue.Add(new KeyValue("K4", K4));
                LstKeyValue.Add(new KeyValue("K5", K5));
                LstKeyValue.Add(new KeyValue("K6", K6));
                LstKeyValue.Add(new KeyValue("K7", K7));
                LstKeyValue.Add(new KeyValue("K8", K8));
                LstKeyValue.Add(new KeyValue("K9", K9));
                LstKeyValue.Add(new KeyValue("K10", K10));
                LstKeyValue.Add(new KeyValue("K11", K11));
                LstKeyValue.Add(new KeyValue("K12", K12));
                LstKeyValue.Add(new KeyValue("K13", K13));
                LstKeyValue.Add(new KeyValue("K14", K14));
                LstKeyValue.Add(new KeyValue("K15", K15));
                LstKeyValue.Add(new KeyValue("K16", K16));
                LstKeyValue.Add(new KeyValue("K17", K17));
                LstKeyValue.Add(new KeyValue("K18", K18));
                LstKeyValue.Add(new KeyValue("K19", K19));
                LstKeyValue.Add(new KeyValue("K20", K20));
                LstKeyValue.Add(new KeyValue("WeixinOpenID", weixinOpenID));

                ///活动信息
                var Activity = bll.Get<ActivityInfo>(string.Format("ActivityID='{0}'", ActivityID));
                #region 升级注释

                //#region IP限制
                ////获取用户IP;
                //string UserHostAddress = context.Request.UserHostAddress;
                //var count = DataCache.GetCache(UserHostAddress);
                //if (count != null)
                //{
                //    int newcount = int.Parse(count.ToString()) + 1;
                //    DataCache.SetCache(UserHostAddress, newcount);
                //    int LimitCount = 100;
                //    if (Activity.LimitCount != null)
                //    {
                //        LimitCount = Activity.LimitCount;
                //    }
                //    if (newcount >= LimitCount)
                //    {
                //        context.ApplicationInstance.CompleteRequest();
                //        return;
                //    }
                //}
                //else
                //{
                //    DataCache.SetCache(UserHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
                //}


                //#endregion

                //#region 判断必填项
                //if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
                //{
                //    context.Response.Redirect(string.Format("{0}status=3&message=姓名和手机不能为空!", CallBackUrl), false);
                //    return;
                //}

                //if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
                //{
                //    context.Response.Redirect(string.Format("{0}status=5&message=手机号码无效!", CallBackUrl), false);


                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(ActivityID))
                //{
                //    context.Response.Redirect(string.Format("{0}status=6&message=活动编号不能为空", CallBackUrl), false);
                //    return;
                //}
                //#endregion

                //#region 登录验证
                //bool loginStatus = false;
                //ZentCloud.BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser("");
                //string userID = Common.Base64Change.DecodeBase64ByUTF8(LoginName);
                //loginStatus = userBll.LoginZCEncrypt(userID, LoginPwd);
                //if (!loginStatus)
                //{
                //    context.Response.Redirect(string.Format("{0}status=4&message=登录失败", CallBackUrl), false);
                //    return;
                //}

                //#endregion

                //#region 活动权限验证


                //if (Activity == null)
                //{
                //    context.Response.Redirect(string.Format("{0}status=7&message=活动编号不存在", CallBackUrl), false);
                //    return;
                //}
                //if (Activity.ActivityStatus.Equals(0))
                //{
                //    context.Response.Redirect(string.Format("{0}status=8&message=活动已关闭", CallBackUrl), false);
                //    return;

                //}
                //if (Activity.IsDelete.Equals(1))
                //{
                //    context.Response.Redirect(string.Format("{0}status=-1&message=活动已删除", CallBackUrl), false);
                //    return;

                //}
                //if (!(Activity.UserID.Equals(userID)))
                //{
                //    context.Response.Redirect(string.Format("{0}status=9&message=无权进行此操作", CallBackUrl), false);
                //    return;

                //}
                //#endregion


                //#region 检查是否已经报名

                //if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}'  and IsDelete = 0  ", ActivityID, Phone)) > 0)
                //{
                //    context.Response.Redirect(string.Format("{0}status=1&message=该用户已经报过名", CallBackUrl), false);
                //    return;

                //}

                //#endregion 
                #endregion


                #region IP限制
                //获取用户IP;
                string UserHostAddress = context.Request.UserHostAddress;
                var count = DataCache.GetCache(UserHostAddress);
                if (count != null)
                {
                    int newcount = int.Parse(count.ToString()) + 1;
                    DataCache.SetCache(UserHostAddress, newcount);
                    int LimitCount = 100;
                    if (Activity.LimitCount != null)
                    {
                        LimitCount = Activity.LimitCount;
                    }
                    if (newcount >= LimitCount)
                    {
                        context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
                else
                {
                    DataCache.SetCache(UserHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
                }


                #endregion

                #region 判断必填项
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
                {
                    //resp.Status = 3;
                    //resp.Msg = "姓名和手机不能为空!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    context.Response.Redirect(string.Format("{0}status=3&message=姓名和手机不能为空!", CallBackUrl), false);
                    return;
                }

                if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
                {
                    //resp.Status = 5;
                    //resp.Msg = "手机号码无效!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    context.Response.Redirect(string.Format("{0}status=5&message=手机号码无效!", CallBackUrl), false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ActivityID))
                {
                    //resp.Status = 6;
                    //resp.Msg = "活动编号不能为空!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    context.Response.Redirect(string.Format("{0}status=6&message=活动编号不能为空", CallBackUrl), false);
                    return;
                }

                //检查自定义必填字段

                #endregion

                #region 登录验证
                bool loginStatus = false;
                ZentCloud.BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser("");
                string userID = Common.Base64Change.DecodeBase64ByUTF8(LoginName);
                loginStatus = userBll.LoginZCEncrypt(userID, LoginPwd);
                if (!loginStatus)
                {
                    //resp.Status = 4;
                    //resp.Msg = "登录失败!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    context.Response.Redirect(string.Format("{0}status=4&message=登录失败", CallBackUrl), false);
                    return;
                }

                #endregion

                #region 活动权限验证


                if (Activity == null)
                {
                    //resp.Status = 7;
                    //resp.Msg = "活动编号不存在!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                    //return;

                    context.Response.Redirect(string.Format("{0}status=7&message=活动编号不存在", CallBackUrl), false);
                    return;
                }
                if (Activity.ActivityStatus.Equals(0))
                {
                    //resp.Status = 8;
                    //resp.Msg = "活动已关闭!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));

                    context.Response.Redirect(string.Format("{0}status=8&message=活动已关闭", CallBackUrl), false);
                    return;

                }
                if (!(Activity.UserID.Equals(userID)))
                {
                    //resp.Status = 9;
                    //resp.Msg = "无权进行此操作!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));

                    context.Response.Redirect(string.Format("{0}status=9&message=无权进行此操作", CallBackUrl), false);
                    return;

                }
                if (Activity.IsDelete.Equals(1))
                {
                    //resp.Status = -1;
                    //resp.Msg = "活动已删除!";
                    //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));

                    context.Response.Redirect(string.Format("{0}status=8&message=活动已关闭", CallBackUrl), false);
                    return;

                }
                #endregion

                #region 检查自定义必填项
                List<ActivityFieldMappingInfo> ListRequiredField = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And FieldIsNull=1", Activity.ActivityID));
                if (ListRequiredField.Count > 0)
                {
                    foreach (var RequiredField in ListRequiredField)
                    {
                        if (string.IsNullOrEmpty(LstKeyValue.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", RequiredField.ExFieldIndex))).Value))
                        {
                            //resp.Status = 11;
                            //resp.Msg = string.Format("请输入 {0}", RequiredField.MappingName);
                            //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                            //return;

                            context.Response.Redirect(string.Format("{0}status=8&message=请输入 {1}", CallBackUrl, RequiredField.MappingName), false);
                            return;
                        }
                    }
                }
                #endregion

                #region 检查数据格式
                //检查数据格式
                List<ActivityFieldMappingInfo> activityFieldMapping = bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", Activity.ActivityID));
                foreach (var item in activityFieldMapping)
                {

                    string value = LstKeyValue.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value;

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    //检查数据格式
                    if (item.FormatValiFunc == "email")//email检查
                    {
                        if (!Common.ValidatorHelper.EmailLogicJudge(value))
                        {
                            //resp.Status = 12;
                            //resp.Msg = string.Format("{0}格式不正确", item.MappingName);
                            //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                            //return;
                            context.Response.Redirect(string.Format("{0}status=8&message={1}格式不正确", CallBackUrl, item.MappingName), false);
                            return;
                        }
                    }
                    if (item.FormatValiFunc == "url")//url检查
                    {
                        System.Text.RegularExpressions.Regex RegUrl = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");//网址
                        System.Text.RegularExpressions.Match m = RegUrl.Match(value);
                        if (!m.Success)
                        {
                            //resp.Status = 13;
                            //resp.Msg = string.Format("{0}格式不正确", item.MappingName);
                            //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                            //return;
                            context.Response.Redirect(string.Format("{0}status=8&message={1}格式不正确", CallBackUrl, item.MappingName), false);
                            return;
                        }
                    }
                }
                #endregion

                #region 检查是否已经报名
                if (!string.IsNullOrEmpty(StrDistinctKeys))
                {

                    if (!StrDistinctKeys.Equals("none"))//自定义检查重复
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder("1=1 ");
                        string[] DistinctKeys = StrDistinctKeys.Split(',');
                        foreach (var item in DistinctKeys)
                        {
                            sb.AppendFormat("And {0}='{1}' ", item, LstKeyValue.Single(p => p.Key.Equals(item)).Value);
                        }
                        sb.Append("  and IsDelete = 0  ");
                        if (bll.GetCount<ActivityDataInfo>(sb.ToString()) > 0)
                        {

                            //resp.Status = 1;
                            //resp.Msg = "重复的报名!";
                            //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                            //return;
                            context.Response.Redirect(string.Format("{0}status=8&message=重复的报名", CallBackUrl), false);
                            return;
                        }

                    }
                    else//不检查重复
                    {

                    }



                }
                else//默认检查
                {
                    if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And Phone='{1}' and IsDelete = 0 ", ActivityID, Phone)) > 0)
                    {
                        //resp.Status = 1;
                        //resp.Msg = "已经报过名了!";
                        //context.Response.Write(Common.JSONHelper.ObjectToJson(resp));

                        context.Response.Redirect(string.Format("{0}status=1&message=该用户已经报过名", CallBackUrl), false);
                        return;

                    }
                }



                #endregion


                var NewActivityUID = 1001;
                var LastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", ActivityID));
                if (LastActivityDataInfo != null)
                {
                    NewActivityUID = LastActivityDataInfo.UID + 1;
                }

                ActivityDataInfo model = new ActivityDataInfo();
                model.ActivityID = ActivityID;
                model.UID = NewActivityUID;
                model.Name = Name;
                model.Phone = Phone;
                model.K1 = K1;
                model.K2 = K2;
                model.K3 = K3;
                model.K4 = K4;
                model.K5 = K5;
                model.K6 = K6;
                model.K7 = K7;
                model.K8 = K8;
                model.K9 = K9;
                model.K10 = K10;
                model.K11 = K11;
                model.K12 = K12;
                model.K13 = K13;
                model.K14 = K14;
                model.K15 = K15;
                model.K16 = K16;
                model.K17 = K17;
                model.K18 = K18;
                model.K19 = K19;
                model.K20 = K20;

                model.WeixinOpenID = weixinOpenID;
                model.SpreadUserID = SpreadUserID;

                if (bll.Add(model))
                {

                    var userInfo = bll.Get<UserInfo>(string.Format("UserID='{0}'", userID));//活动发起者信息
                    //Common.HttpInterFace WebRequest = new Common.HttpInterFace();//

                    ////报名成功，检查是否向报名者发送短信
                    //#region 检查是否向报名者需要发送短信
                    //if (!string.IsNullOrWhiteSpace(Activity.ConfirmSMSContent))
                    //{


                    //    //短信内容不为空，向用户发送短信

                    //    string Parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", userInfo.UserID, userInfo.Password, Phone, Activity.ConfirmSMSContent);


                    //    var result = WebRequest.PostWebRequest(Parm, "http://www.jubit.org/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));

                    //    // context.Response.Redirect(redirecturl, false); 

                    //}
                    //#endregion

                    ////检查是否向管理员发送通知短信
                    //#region 检查是否向管理员发送能通知短信

                    //if (!string.IsNullOrWhiteSpace(Activity.AdminPhone) && (!string.IsNullOrWhiteSpace(Activity.AdminSMSContent)))
                    //{

                    //    foreach (var item in Activity.AdminPhone.Split(','))
                    //    {
                    //        //短信内容不为空，向用户发送短信
                    //        string Parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", userInfo.UserID, userInfo.Password, item, Activity.AdminSMSContent);

                    //        var result = WebRequest.PostWebRequest(Parm, "http://www.jubit.org/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
                    //    }



                    //    // context.Response.Redirect(redirecturl, false); 

                    //}
                    //#endregion


                    //检查是否向微信客服号发送通知信息
                    #region 检查是否向微信客服号发送通知信息

                    #region 构造要发送的微信消息
                    //构造要发送的消息
                    System.Text.StringBuilder msg = new System.Text.StringBuilder();
                    msg.AppendFormat("{0}\n有新的报名!\n", Activity.ActivityName);

                    msg.AppendFormat("姓名:{0}\n", model.Name);
                    msg.AppendFormat("手机:{0}\n", model.Phone);
                    foreach (var item in activityFieldMapping)
                    {
                        msg.AppendFormat("{0}:{1}\n", item.MappingName, LstKeyValue.SingleOrDefault(p => p.Key.Equals(string.Format("K{0}", item.ExFieldIndex))).Value);
                    }
                    msg.AppendFormat("{0:f}\n", DateTime.Now.ToString());
                    //构造要发送的消息 
                    #endregion

                    if (!string.IsNullOrEmpty(Activity.ActivityNoticeKeFuId))
                    {
                        WXKeFu keFuInfo = bll.Get<WXKeFu>(string.Format("AutoID={0}", Activity.ActivityNoticeKeFuId));
                        if (keFuInfo != null)
                        {

                            UserInfo currWebSiteUserInfo = bll.Get<UserInfo>(string.Format(" UserID = '{0}' ", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));
                            if (currWebSiteUserInfo != null)
                            {


                                BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
                                var accesstoken = bllWeixin.GetAccessToken(currWebSiteUserInfo.UserID);
                                if (accesstoken != string.Empty)
                                {
                                    string result = bllWeixin.SendKeFuMessageText(accesstoken, keFuInfo.WeiXinOpenID, msg.ToString());
                                    if (result.Contains("ok"))
                                    {
                                        //发送微信消息成功


                                    }



                                }





                            }



                        }
                    }





                    #endregion


                    //try
                    //{
                    //    #region 报名成功后客户信息处理
                    //    //*TODO:一:检查手机号，若手机号不存在，则直接把客户信息插入客户表
                    //    ///二：若手机号已经存在 则分以下步骤:
                    //    ///1.原有的姓名为空，报名的数据有姓名，则更新客户姓名
                    //    ///2.原有的性别为空，报名的数据有性别，则更新客户性别
                    //    ///3.原有的生日为空，报名的数据有生日，则更新客户生日
                    //    ///4.原有的QQ为空，报名的数据有QQ，则更新客户QQ
                    //    ///5.原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
                    //    ///6.原有的网址为空，报名的数据有网址，则更新客户网址
                    //    ///7.原有的公司为空，报名的数据有公司，则更新客户公司
                    //    ///8.原有的职位为空，报名的数据有职位，则更新客户职位
                    //    ///9.原有的地址为空，报名的数据有地址，则更新客户地址
                    //    ///10.原有的备注为空，报名的数据有备注，则更新客户备注
                    //    ///11.报名的数据有邮箱，则依次检查第一个邮箱，第二个邮箱，第三个邮箱，第四个邮箱是否有数据，没有数据就更新，如果四个邮箱都有数据了则放弃报名邮箱
                    //    ///12.报名的数据有微博ID，则依次检查第一个微博ID，第二个微博ID，第三个微博ID，第四个微博ID是否有数据，没有数据就更新，如果四个微博ID都有数据了则放弃报名微博ID
                    //    ///13.报名的数据有微信OpenID，则依次检查第一个微信OpenID，第二个微信OpenID，第三个微信OpenID，第四个微信OpenID是否有数据，没有数据就更新，如果四个微信OpenID都有数据了则放弃报名微信OpenID


                    //    var MemberInfo = bll.Get<MemberInfo>(string.Format("UserID='{0}' And Mobile='{1}'", userID, model.Phone));//属于此活动发起者 的手机号的客户

                    //    var RequestMemberInfo = ConvertToMemberInfoModel(model);//报名数据转换成的客户实体

                    //    #region 手机号不存在，则直接把客户信息插入客户表
                    //    if (MemberInfo == null)//手机号不存在，则直接把客户信息插入客户表
                    //    {
                    //        RequestMemberInfo.MemberID = bll.GetGUID(BLLJIMP.TransacType.MemberAdd);
                    //        RequestMemberInfo.UserID = userID;
                    //        RequestMemberInfo.MemberType = 0;
                    //        bll.Add(RequestMemberInfo);

                    //    }
                    //    #endregion

                    //    #region 手机号已经存在，检查是否需要更新客户表
                    //    else
                    //    {
                    //        #region 原有的姓名为空，报名的数据有姓名，则更新客户姓名
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Name))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Name))
                    //            {
                    //                //原有的姓名为空，报名的数据有姓名，则更新客户姓名
                    //                bll.Update(MemberInfo, string.Format("Name='{0}'", RequestMemberInfo.Name), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的性别为空，报名的数据有性别，则更新客户性别
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Sex))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Sex))
                    //            {
                    //                //原有的性别为空，报名的数据有性别，则更新客户性别
                    //                bll.Update(MemberInfo, string.Format("Sex='{0}'", RequestMemberInfo.Sex), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的生日为空，报名的数据有生日，则更新客户生日
                    //        if (RequestMemberInfo.Birthday != null)
                    //        {
                    //            if (MemberInfo.Birthday == null)
                    //            {
                    //                //原有的生日为空，报名的数据有生日，则更新客户生日
                    //                bll.Update(MemberInfo, string.Format("Birthday='{0}'", RequestMemberInfo.Birthday), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的QQ为空，报名的数据有QQ，则更新客户QQ
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.QQ))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.QQ))
                    //            {
                    //                //原有的QQ为空，报名的数据有QQ，则更新客户QQ
                    //                bll.Update(MemberInfo, string.Format("QQ='{0}'", RequestMemberInfo.QQ), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Tel))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Tel))
                    //            {
                    //                //原有的固定电话为空，报名的数据有固定电话，则更新客户固定电话
                    //                bll.Update(MemberInfo, string.Format("Tel='{0}'", RequestMemberInfo.Tel), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }

                    //        #endregion

                    //        #region 原有的网址为空，报名的数据有网址，则更新客户网址
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Website))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Website))
                    //            {
                    //                //原有的网址为空，报名的数据有网址，则更新客户网址
                    //                bll.Update(MemberInfo, string.Format("Website='{0}'", RequestMemberInfo.Website), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的公司为空，报名的数据有公司，则更新客户公司
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Company))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Company))
                    //            {
                    //                //原有的公司为空，报名的数据有公司，则更新客户公司
                    //                bll.Update(MemberInfo, string.Format("Company='{0}'", RequestMemberInfo.Company), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的职位为空，报名的数据有职位，则更新客户职位
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Title))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Title))
                    //            {
                    //                //原有的职位为空，报名的数据有职位，则更新客户职位
                    //                bll.Update(MemberInfo, string.Format("Title='{0}'", RequestMemberInfo.Title), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的地址为空，报名的数据有地址，则更新客户地址
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Address))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Address))
                    //            {
                    //                //原有的地址为空，报名的数据有地址，则更新客户地址
                    //                bll.Update(MemberInfo, string.Format("Address='{0}'", RequestMemberInfo.Address), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 原有的备注为空，报名的数据有备注，则更新客户备注
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Remark))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Remark))
                    //            {
                    //                //原有的备注为空，报名的数据有备注，则更新客户备注
                    //                bll.Update(MemberInfo, string.Format("Remark='{0}'", RequestMemberInfo.Remark), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }
                    //        }
                    //        #endregion

                    //        #region 检查邮箱更新
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.Email))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.Email))//原有的第一个邮箱为空,直接把报名邮箱更新到第一个邮箱
                    //            {
                    //                //更新第一个邮箱
                    //                bll.Update(MemberInfo, string.Format("Email='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }

                    //            else//原有的第一个邮箱不为空
                    //            {

                    //                if (!MemberInfo.Email.Equals(RequestMemberInfo.Email) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email2)) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email3)) && (!RequestMemberInfo.Email.Equals(MemberInfo.Email4)))//报名的邮箱与原来的四个邮箱不相同，则检查能否更新到拓展邮箱里
                    //                {
                    //                    if (string.IsNullOrEmpty(MemberInfo.Email2))//第二个邮箱为空，则把报名邮箱更新到第二个邮箱
                    //                    {
                    //                        //更新第二个邮箱
                    //                        bll.Update(MemberInfo, string.Format("Email2='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                    }
                    //                    else//第二个邮箱不为空，检查第三个邮箱
                    //                    {
                    //                        if (string.IsNullOrEmpty(MemberInfo.Email3))//第三个邮箱为空，则报名邮箱更新到第三个邮箱
                    //                        {
                    //                            //更新第三个邮箱
                    //                            bll.Update(MemberInfo, string.Format("Email3='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                        }
                    //                        else//第三个邮箱也有数据了，检查第四个邮箱
                    //                        {
                    //                            if (string.IsNullOrEmpty(MemberInfo.Email4))//第四个邮箱为空，则报名邮箱更新到第四个邮箱
                    //                            {
                    //                                //更新第四个邮箱
                    //                                bll.Update(MemberInfo, string.Format("Email4='{0}'", RequestMemberInfo.Email), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //                            }
                    //                            else//第四个邮箱有数据，则放弃报名邮箱
                    //                            {


                    //                            }

                    //                        }
                    //                    }


                    //                }
                    //                else//报名的邮箱与原来第一个邮箱相同，不做操作
                    //                {

                    //                }


                    //            }



                    //        }
                    //        #endregion

                    //        #region 检查微博ID更新
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.WeiboID))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.WeiboID))//原有的第一个微博ID为空
                    //            {
                    //                //更新第一个微博ID
                    //                bll.Update(MemberInfo, string.Format("WeiboID='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }

                    //            else//原有的第一个微博ID不为空
                    //            {

                    //                if (!MemberInfo.WeiboID.Equals(RequestMemberInfo.WeiboID) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID2)) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID3)) && (!RequestMemberInfo.WeiboID.Equals(MemberInfo.WeiboID4)))//报名的微博ID与原来的四个微博ID都不相同，则检查能否更新到拓展微博ID里
                    //                {
                    //                    if (string.IsNullOrEmpty(MemberInfo.WeiboID2))//第二个微博ID为空，则把报名微博ID更新到第二个微博ID
                    //                    {
                    //                        //更新第二个微博ID
                    //                        bll.Update(MemberInfo, string.Format("WeiboID2='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                    }
                    //                    else//第二个微博ID不为空，检查第三个微博ID
                    //                    {
                    //                        if (string.IsNullOrEmpty(MemberInfo.WeiboID3))//第三个微博ID为空，则报名微博ID更新到第三个微博ID
                    //                        {
                    //                            //更新第三个微博ID
                    //                            bll.Update(MemberInfo, string.Format("WeiboID3='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                        }
                    //                        else//第三个微博ID也有数据了，检查第四个微博ID
                    //                        {
                    //                            if (string.IsNullOrEmpty(MemberInfo.WeiboID4))//第四个微博ID为空，则报名微博ID更新到第四个微博ID
                    //                            {
                    //                                //更新第四个微博ID
                    //                                bll.Update(MemberInfo, string.Format("WeiboID4='{0}'", RequestMemberInfo.WeiboID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //                            }
                    //                            else
                    //                            {
                    //                                //第四个微博ID有数据了，放弃报名微博ID
                    //                            }

                    //                        }
                    //                    }


                    //                }
                    //                else//报名的微博ID与原来第一个微博ID相同，不做操作
                    //                {

                    //                }


                    //            }



                    //        }
                    //        #endregion

                    //        #region 检查微信OPenID更新
                    //        if (!string.IsNullOrEmpty(RequestMemberInfo.WeixinOpenID))
                    //        {
                    //            if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID))//原有的第一个微信OPenID为空,直接把报名微信OPenID更新到第一个微信OPenID
                    //            {
                    //                //更新第一个微信OPenID
                    //                bll.Update(MemberInfo, string.Format("WeixinOpenID='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //            }

                    //            else//原有的第一个微信OPenID不为空
                    //            {

                    //                if (!MemberInfo.WeixinOpenID.Equals(RequestMemberInfo.WeixinOpenID) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID2)) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID3)) && (!RequestMemberInfo.WeixinOpenID.Equals(MemberInfo.WeixinOpenID4)))//报名的微信OPenID与原来的四个微信OPenID不相同，则检查能否更新到拓展微信OPenID里
                    //                {
                    //                    if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID2))//第二个微信OPenID为空，则把报名微信OPenID更新到第二个微信OPenID
                    //                    {
                    //                        //更新第二个微信OPenID
                    //                        bll.Update(MemberInfo, string.Format("WeixinOpenID2='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                    }
                    //                    else//第二个微信OPenID不为空，检查第三个微信OPenID
                    //                    {
                    //                        if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID3))//第三个微信OPenID为空，则报名微信OPenID更新到第三个微信OPenID
                    //                        {
                    //                            //更新第三个微信OPenID
                    //                            bll.Update(MemberInfo, string.Format("WeixinOpenID3='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));

                    //                        }
                    //                        else//第三个微信OPenID也有数据了，检查第四个微信OPenID
                    //                        {
                    //                            if (string.IsNullOrEmpty(MemberInfo.WeixinOpenID4))//第四个微信OPenID为空，则报名微信OPenID更新到第四个微信OPenID
                    //                            {
                    //                                //更新第四个微信OPenID
                    //                                bll.Update(MemberInfo, string.Format("WeixinOpenID4='{0}'", RequestMemberInfo.WeixinOpenID), string.Format("MemberID='{0}'", MemberInfo.MemberID));
                    //                            }
                    //                            else//第四个微信OPenID有数据，则放弃报名微信OPenID
                    //                            {


                    //                            }

                    //                        }
                    //                    }


                    //                }
                    //                else//报名的微信OPenID与原来第一个微信OPenID相同，不做操作
                    //                {

                    //                }


                    //            }



                    //        }
                    //        #endregion

                    //    }
                    //    #endregion


                    //    #endregion
                    //}
                    //catch (Exception ex)
                    //{
                    //    //ToDo:客户信息处理异常
                    //    //日志记录
                    //}
                    //回调
                    context.Response.Redirect(string.Format("{0}status=0&message=报名成功！", CallBackUrl), false);


                    return;
                }
                else
                {
                    context.Response.Redirect(string.Format("{0}status=2&message=报名失败，请重试或联系管理员！", CallBackUrl), false);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Redirect(string.Format("{0}?status=10&message={1}", GetPostParm("CallBackUrl"), "报名失败，请重试或联系管理员"), false);
                //日志记录


            }


        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private string GetPostParm(string parm)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parm))
                    return this.contentthis.Request[parm];
            }
            catch { return ""; }
            return "";
        }


        /// <summary>
        /// 根据提交的报名数据转换成客户信息实体
        /// </summary>
        /// <param name="activitydatamodel">报名数据实体</param>
        /// <returns></returns>
        private MemberInfo ConvertToMemberInfoModel(ActivityDataInfo activitydatamodel)
        {
            MemberInfo model = new MemberInfo();
            model.Name = activitydatamodel.Name;
            model.Mobile = activitydatamodel.Phone;
            if (!string.IsNullOrEmpty(activitydatamodel.K1))
            {
                var K1MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 1);
                model = SetMemberInfoModelByMappingName(K1MappingName, activitydatamodel.K1, model);

            }
            if (!string.IsNullOrEmpty(activitydatamodel.K2))
            {
                var K2MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 2);
                model = SetMemberInfoModelByMappingName(K2MappingName, activitydatamodel.K2, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K3))
            {
                var K3MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 3);
                model = SetMemberInfoModelByMappingName(K3MappingName, activitydatamodel.K3, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K4))
            {
                var K4MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 4);
                model = SetMemberInfoModelByMappingName(K4MappingName, activitydatamodel.K4, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K5))
            {
                var K5MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 5);
                model = SetMemberInfoModelByMappingName(K5MappingName, activitydatamodel.K5, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K6))
            {
                var K6MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 6);
                model = SetMemberInfoModelByMappingName(K6MappingName, activitydatamodel.K6, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K7))
            {
                var K7MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 7);
                model = SetMemberInfoModelByMappingName(K7MappingName, activitydatamodel.K7, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K8))
            {
                var K8MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 8);
                model = SetMemberInfoModelByMappingName(K8MappingName, activitydatamodel.K8, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K9))
            {
                var K9MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 9);
                model = SetMemberInfoModelByMappingName(K9MappingName, activitydatamodel.K9, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K10))
            {
                var K10MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 10);
                model = SetMemberInfoModelByMappingName(K10MappingName, activitydatamodel.K10, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K11))
            {
                var K11MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 11);
                model = SetMemberInfoModelByMappingName(K11MappingName, activitydatamodel.K11, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K12))
            {
                var K12MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 12);
                model = SetMemberInfoModelByMappingName(K12MappingName, activitydatamodel.K12, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K13))
            {
                var K13MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 13);
                model = SetMemberInfoModelByMappingName(K13MappingName, activitydatamodel.K13, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K14))
            {
                var K14MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 14);
                model = SetMemberInfoModelByMappingName(K14MappingName, activitydatamodel.K14, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K15))
            {
                var K15MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 15);
                model = SetMemberInfoModelByMappingName(K15MappingName, activitydatamodel.K15, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K16))
            {
                var K16MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 16);
                model = SetMemberInfoModelByMappingName(K16MappingName, activitydatamodel.K16, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K17))
            {
                var K17MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 17);
                model = SetMemberInfoModelByMappingName(K17MappingName, activitydatamodel.K17, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K18))
            {
                var K18MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 18);
                model = SetMemberInfoModelByMappingName(K18MappingName, activitydatamodel.K18, model);
            }

            if (!string.IsNullOrEmpty(activitydatamodel.K19))
            {
                var K19MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 19);
                model = SetMemberInfoModelByMappingName(K19MappingName, activitydatamodel.K19, model);
            }
            if (!string.IsNullOrEmpty(activitydatamodel.K20))
            {
                var K20MappingName = GetMappingNameByActivityIDAndExFieldIndex(activitydatamodel.ActivityID, 20);
                model = SetMemberInfoModelByMappingName(K20MappingName, activitydatamodel.K20, model);
            }


            return model;


        }

        /// <summary>
        /// 根据活动ID跟扩展字段索引获取扩展字段映射名称
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <param name="ExFieldIndex"></param>
        /// <returns></returns>
        private string GetMappingNameByActivityIDAndExFieldIndex(string ActivityID, int ExFieldIndex)
        {
            var MappingInfo = bll.Get<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' And ExFieldIndex={1}", ActivityID, ExFieldIndex));
            if (MappingInfo != null)
            {
                return MappingInfo.MappingName;
            }
            return "";

        }

        /// <summary>
        /// 根据映射字段名称设置客户Model信息
        /// </summary>
        /// <param name="MappingName">映射名称</param>
        /// <param name="Value">值</param>
        /// <param name="model">客户实体</param>
        /// <returns></returns>
        private MemberInfo SetMemberInfoModelByMappingName(string MappingName, string Value, MemberInfo model)
        {

            if (MappingName.Equals("微博ID"))
            {
                model.WeiboID = Value;
            }
            if (MappingName.Equals("微博昵称"))
            {
                model.WeiboScreenName = Value;
            }
            if (MappingName.Equals("性别"))
            {
                model.Sex = Value;
            }
            if (MappingName.Equals("生日"))
            {
                DateTime Birthday;
                if (DateTime.TryParse(Value, out Birthday))
                {
                    model.Birthday = Birthday;
                }

            }
            if (MappingName.Equals("电子邮箱"))
            {
                model.Email = Value;
            }
            if (MappingName.Equals("QQ"))
            {
                model.QQ = Value;
            }
            if (MappingName.Equals("固定电话"))
            {
                model.Tel = Value;
            }
            if (MappingName.Equals("网址"))
            {
                model.Website = Value;
            }
            if (MappingName.Equals("公司"))
            {
                model.Company = Value;
            }
            if (MappingName.Equals("职位"))
            {
                model.Title = Value;
            }
            if (MappingName.Equals("地址"))
            {
                model.Address = Value;
            }
            if (MappingName.Equals("备注"))
            {
                model.Remark = Value;
            }
            if (MappingName.Equals("微信OPenID"))
            {
                model.WeixinOpenID = Value;
            }

            return model;


        }

        private class KeyValue
        {
            public KeyValue(string key, string value)
            {
                Key = key;
                Value = value;
            }
            public string Key { get; set; }
            public string Value { get; set; }


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