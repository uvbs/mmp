using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 
    /// 绑定微信手机号：
    /// 
    /// 如果手机号已被注册，账户已经有openid，提示手机号已经被绑定注册
    /// 如果手机号已被注册，账户没有openid，绑定openid到手机上
    /// 
    /// 
    /// 如果当前openid还未注册用户
    /// --如果手机号没注册过用户，绑定成为一个新用户，并设置登陆状态为成功
    /// 
    /// 如果当前openid已经注册过用户    
    /// --如果手机号没注册过用户，把手机号填入openid的用户中
    /// 
    /// 
    /// </summary>
    public class BindOpenIdPhone : BaseHandlerRequiresSessionNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            BLLUser userBll = new BLLUser();
            BLLSMS smsBll = new BLLSMS("");
            BLLDistribution bllDist = new BLLDistribution();

            try
            {
                
                var phone = context.Request["phone"];
                var code = context.Request["code"];
                var password = context.Request["password"];

                string openId = "";
                if (context.Session["currWXOpenId"] != null)
                {
                    openId = context.Session["currWXOpenId"].ToString();
                }

                //if (string.IsNullOrWhiteSpace(openId))
                //{
                //    apiResp.status = false;
                //    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                //    apiResp.msg = "找不到微信OpenId";
                //    userBll.ContextResponse(context, apiResp);
                //    return;
                //}

                phone = phone.Trim();
                code = code.Trim();
                password = password.Trim();

                if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code))
                {
                    apiResp.status = false;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    apiResp.msg = "手机以及验证码必传";
                    userBll.ContextResponse(context, apiResp);
                    return;
                }


                if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(phone))
                {
                    apiResp.status = false;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    apiResp.msg = "手机号码格式不正确";
                    userBll.ContextResponse(context, apiResp);
                    return;
                }

                var vCode = smsBll.GetLastSmsVerificationCode(phone);

                if (vCode == null)
                {
                    apiResp.status = false;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    apiResp.msg = "无效验证码";
                    userBll.ContextResponse(context, apiResp);
                    return;
                }

                if (vCode.VerificationCode != code)
                {
                    apiResp.status = false;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    apiResp.msg = "无效验证码";
                    userBll.ContextResponse(context, apiResp);
                    return;
                }

                UserInfo phoneUser = userBll.GetUserInfoByPhone(phone);
                UserInfo openIdUser = null;
                if (!string.IsNullOrWhiteSpace(openId)) openIdUser = userBll.GetUserInfoByOpenId(openId);

                #region phoneUser != null
                if (phoneUser != null)
                {
                    if (openIdUser == null && string.IsNullOrWhiteSpace(phoneUser.WXOpenId) && !string.IsNullOrWhiteSpace(openId))
                    {
                        StringBuilder sbSQL = new StringBuilder();
                        sbSQL.AppendFormat(" WXOpenId = '{0}' ", openId);
                        if (!string.IsNullOrWhiteSpace(password)) sbSQL.AppendFormat(",Password = '{0}' ", password);
                        //绑定openid到手机号账户上
                        if (userBll.Update(phoneUser, sbSQL.ToString(), string.Format(" AutoID = {0} ", phoneUser.AutoID)) > 0)
                        {

                            //设置登陆状态成功
                            //设置用户会话ID
                            context.Session[ZentCloud.Common.SessionKey.UserID] = phoneUser.UserID;
                            context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态

                            apiResp.status = true;
                            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                            apiResp.msg = "绑定成功";
                            userBll.ContextResponse(context, apiResp);

                            bllDist.SetUserDistributionOwnerByTemp(phoneUser.UserID, phoneUser.WebsiteOwner);

                            return;
                        }
                        else
                        {
                            apiResp.status = false;
                            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            apiResp.msg = "绑定失败";
                            userBll.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(password))
                    {
                        if (userBll.Update(phoneUser, string.Format(" Password = {0} ", password), string.Format(" AutoID = {0} ", phoneUser.AutoID)) > 0)
                        {
                            //设置登陆状态成功
                            //设置用户会话ID
                            context.Session[ZentCloud.Common.SessionKey.UserID] = phoneUser.UserID;
                            context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态

                            apiResp.status = true;
                            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                            apiResp.msg = "登录成功";
                            userBll.ContextResponse(context, apiResp);
                            return;
                        }
                        else
                        {
                            apiResp.status = false;
                            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                            apiResp.msg = "注册失败";
                            userBll.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    else
                    {
                        //设置登陆状态成功
                        //设置用户会话ID
                        context.Session[ZentCloud.Common.SessionKey.UserID] = phoneUser.UserID;
                        context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态

                        apiResp.status = true;
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                        apiResp.msg = "登录成功";
                        userBll.ContextResponse(context, apiResp);
                        return;
                    }
                } 
                #endregion

                if (openIdUser == null)
                {
                    //构造新用户
                    var currentUserInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                    currentUserInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        currentUserInfo.Password = password;
                    }
                    else
                    {
                        currentUserInfo.Password = ZentCloud.Common.Rand.Str_char(6);
                    }
                    currentUserInfo.UserType = 2;
                    currentUserInfo.WebsiteOwner = userBll.WebsiteOwner;
                    currentUserInfo.Regtime = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(openId)) currentUserInfo.WXOpenId = openId;
                    currentUserInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                    currentUserInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                    currentUserInfo.LastLoginDate = DateTime.Now;
                    currentUserInfo.LoginTotalCount = 1;
                    currentUserInfo.Phone = phone;

                    if (userBll.Add(currentUserInfo))
                    {
                        //设置登陆状态成功
                        //设置用户会话ID
                        context.Session[ZentCloud.Common.SessionKey.UserID] = currentUserInfo.UserID;
                        context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态

                        apiResp.status = true;
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                        apiResp.msg = "注册成功";
                        userBll.ContextResponse(context, apiResp);

                        bllDist.SetUserDistributionOwnerByTemp(currentUserInfo.UserID, currentUserInfo.WebsiteOwner);

                        return;
                    }
                    else
                    {
                        apiResp.status = false;
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        apiResp.msg = "注册失败";
                        userBll.ContextResponse(context, apiResp);
                        return;
                    }
                }
                else
                {

                    apiResp.status = false;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    apiResp.msg = "该微信已绑定其他账号";
                    userBll.ContextResponse(context, apiResp);
                    return;

                    StringBuilder sbSQL1 = new StringBuilder();
                    sbSQL1.AppendFormat(" Phone = '{0}' ", phone);
                    if (!string.IsNullOrWhiteSpace(password)) sbSQL1.AppendFormat(",Password = '{0}' ", password);
                    openIdUser.Phone = phone;
                    if (userBll.Update(openIdUser, sbSQL1.ToString(), string.Format(" AutoID = {0} ", openIdUser.AutoID)) > 0)
                    {
                        //设置登陆状态成功
                        //设置用户会话ID
                        context.Session[ZentCloud.Common.SessionKey.UserID] = openIdUser.UserID;
                        context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态

                        apiResp.status = true;
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                        apiResp.msg = "绑定成功";
                        userBll.ContextResponse(context, apiResp);

                        bllDist.SetUserDistributionOwnerByTemp(openIdUser.UserID, openIdUser.WebsiteOwner);
                        return;
                    }
                    else
                    {
                        apiResp.status = false;
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        apiResp.msg = "绑定失败";
                        userBll.ContextResponse(context, apiResp);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                apiResp.status = false;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "绑定失败:" + ex.Message;
                userBll.ContextResponse(context, apiResp);
                return;
            }

        }
        
    }
}