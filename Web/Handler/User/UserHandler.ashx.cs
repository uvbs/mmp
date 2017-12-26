using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.User
{
    /// <summary>
    /// UserHandler 的摘要说明
    /// </summary>
    public class UserHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser userBll=new BLLJIMP.BLLUser();
        /// <summary>
        /// 站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = string.Empty;

            try
            {
                this.currentUserInfo = userBll.GetCurrentUserInfo();
                switch (action)
                {
                    //case "GetCodeListInfo":
                    //    result = GetCodeListInfo(context);
                    //    break;

                    //case "GetUserInfoByOpenId":
                    //    result = GetUserInfov(context);
                    //    break;
                    case "GetUserInfo":
                        result = GetUserInfo(context);
                        break;
                    case "EditUserInfoByOpenId":
                        result = EditUserInfoV1(context);
                        break;
                    case "EditUserInfo":
                        result = EditUserInfo(context);
                        break;
                    case "EditUserInfoV1":
                        result = EditUserInfoV1(context);
                        break;
                    //case "SendPhoneVerifyCode":
                    //    result = SendPhoneVerifyCode(context);
                       // break;
                    case "VerifyPhone":
                        result = VerifyPhone(context);
                        break;
                    //IsAllUserBaseInfo
                    case "IsAllUserBaseInfo":
                        result = IsAllUserBaseInfo(context);
                        break;
                    default:
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

        /// <summary>
        /// 检查用户是否补足信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string IsAllUserBaseInfo(HttpContext context)
        {
            if (this.userBll.IsAllUserBaseInfo(this.currentUserInfo.UserID))
                resp.Status = 1;
            else
                resp.Status = 0;
            return ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);
        }

        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string VerifyPhone(HttpContext context)
        {
            string phone = context.Request["phone"];
            string phoneCheckCode = context.Request["phoneCheckCode"];
            string checkCode = context.Request["checkCode"];
            string sessionCheckPhone = context.Session["ChcekPhone"] == null ? "" : context.Session["ChcekPhone"].ToString();
            string sessionPhoneCheckCode = context.Session["PhoneChcekCode"] == null ? "" : context.Session["PhoneChcekCode"].ToString();
            string sessionCheckCode = context.Session["CheckCode"] == null ? "" : context.Session["CheckCode"].ToString();

            #region 验证判断
            if (string.IsNullOrWhiteSpace(sessionPhoneCheckCode))
            {
                resp.Status = 0;
                resp.Msg = "验证码已过期，请重新发送手机校验码!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrWhiteSpace(sessionCheckCode))
            {
                resp.Status = 0;
                resp.Msg = "验证码已过期，请重新刷新验证码!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            //检查格式
            if (!Common.ValidatorHelper.PhoneNumLogicJudge(phone))
            {
                resp.Status = 0;
                resp.Msg = "手机号码不正确!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (sessionCheckPhone != phone)
            {
                resp.Status = 0;
                resp.Msg = "手机号码有变化，请重新发送校验码!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrWhiteSpace(phoneCheckCode))
            {
                resp.Status = 0;
                resp.Msg = "手机校验码不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (string.IsNullOrWhiteSpace(checkCode))
            {
                resp.Status = 0;
                resp.Msg = "验证码不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (checkCode != sessionCheckCode)
            {
                resp.Status = 0;
                resp.Msg = "验证码有误!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (phoneCheckCode != sessionPhoneCheckCode)
            {
                resp.Status = 0;
                resp.Msg = "手机校验码有误!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            #endregion


            //更新手机验证信息
            this.currentUserInfo.Phone = phone;
            this.currentUserInfo.IsPhoneVerify = 1;
            if (this.userBll.Update(this.currentUserInfo, string.Format(" Phone='{0}',IsPhoneVerify=1",currentUserInfo.Phone),string.Format(" AutoID={0}",currentUserInfo.AutoID))>0)
            {
                resp.Status = 1;
                resp.Msg = "验证成功!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "用户信息更新失败!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }

        ///// <summary>
        ///// 发送手机验证码
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string SendPhoneVerifyCode(HttpContext context)
        //{
        //    string phone = context.Request["phone"];
        //    string code = Common.Rand.Number(6);

        //    //检查格式
        //    if (!Common.ValidatorHelper.PhoneNumLogicJudge(phone))
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "手机号码不正确!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    //检查发送时间
        //    if (context.Session["PhoneChcekTime"] != null)
        //    {
        //        DateTime preSendTime = (DateTime)context.Session["PhoneChcekTime"];
        //        if ((DateTime.Now - preSendTime).TotalSeconds < 59)
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "验证码发送时间过短!";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //    }

        //    //检查号码是否已重复验证
        //    if (this.userBll.CheckPhoneIsVerify(phone))
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "该手机号码已被验证过,如有问题请联系客服工作人员!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    context.Session["PhoneChcekCode"] = code;

        //    context.Session["ChcekPhone"] = phone;

        //    //发送系统短信
        //    string cnt = string.Format("验证码为{0}(客服工作人员绝不会索取此验证码，切勿告知他人),请在页面中输入以完成验证", code);

        //    if (userBll.SendSysSms(phone, cnt))
        //    {
        //        context.Session["PhoneChcekTime"] = DateTime.Now;
        //        resp.Status = 1;
        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "短信发送失败，请联系客服人员!";
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        private string EditUserInfo(HttpContext context)
        {
            string name = context.Request["Name"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string company = context.Request["Company"];
            string position = context.Request["Postion"];

            //格式判断
            if (!string.IsNullOrWhiteSpace(phone) && !Common.ValidatorHelper.PhoneNumLogicJudge(phone))
            {
                resp.Status = 0;
                resp.Msg = "手机号码格式错误!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrWhiteSpace(email) && !Common.ValidatorHelper.EmailLogicJudge(email))
            {
                resp.Status = 0;
                resp.Msg = "邮箱地址错误!";
                return Common.JSONHelper.ObjectToJson(resp);
            }




            if (currentUserInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "用户不存在!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            currentUserInfo.TrueName = name;

            //修改了手机号则验证失效，需要重新验证
            if (currentUserInfo.Phone != phone)
            {
                currentUserInfo.Phone = phone;
                currentUserInfo.IsPhoneVerify = 0;
            }
            //editUser.Phone = Phone;
            currentUserInfo.Email = email;
            currentUserInfo.Company = company;
            currentUserInfo.Postion = position;
            currentUserInfo.LastLoginDate = DateTime.Now;
            if (this.userBll.Update(currentUserInfo, string.Format(" TrueName='{0}',IsPhoneVerify={1},Email='{2}',Company='{3}',Postion='{4}'", currentUserInfo.TrueName, currentUserInfo.IsPhoneVerify, currentUserInfo.Email, currentUserInfo.Company, currentUserInfo.Postion), string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "成功更新!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }
        /// <summary>
        /// 商城编辑个人资料
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditUserInfoV1(HttpContext context)
        {

            string name = context.Request["Name"];
            string phone = context.Request["Phone"];
            string email = context.Request["Email"];
            string gender = context.Request["Gender"];
            string addressArea = context.Request["AddressArea"];
            string company = context.Request["Company"];
            string position = context.Request["Position"];
            string recommendCode= context.Request["RecommendCode"];//推荐码
            //格式判断
            //if (!string.IsNullOrWhiteSpace(Phone) && !Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
            //{
            //    resp.Status = 0;
            //    resp.Msg = "手机号码格式错误!";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            if (!string.IsNullOrWhiteSpace(email) && !Common.ValidatorHelper.EmailLogicJudge(email))
            {
                resp.Status = 0;
                resp.Msg = "邮箱地址错误!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            
            WebsiteInfo webSitemodel=bllWebsite.GetWebsiteInfo(bllWebsite.WebsiteOwner);
            if (webSitemodel.IsNeedDistributionRecommendCode == 1)
            {
                if (string.IsNullOrEmpty(recommendCode))
                {
                    resp.Status = 0;
                    resp.Msg = "请输入推荐码!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
                UserInfo recommendUserInfo = userBll.GetUserInfoByAutoID(int.Parse(recommendCode));
                if (recommendUserInfo == null)
                {
                    resp.Status = 0;
                    resp.Msg = "推荐码错误!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }


                
                currentUserInfo.DistributionOwner = recommendUserInfo.UserID;



            }


            currentUserInfo.TrueName = name;
            currentUserInfo.Phone = phone;
            currentUserInfo.Email = email;
            currentUserInfo.Gender = gender;
            currentUserInfo.AddressArea = addressArea;
            currentUserInfo.LastLoginDate = DateTime.Now;
            currentUserInfo.Company = company;
            currentUserInfo.Postion = position;

            //UserInfo recommendUserInfo=null;

            //if (!string.IsNullOrEmpty(recommendCode))
            //{
            //    recommendUserInfo = userBll.GetUserInfoByAutoID(int.Parse(recommendCode));
            //} 
            


            string sql=string.Format(" TrueName='{0}',Phone='{1}',Email='{2}',Gender='{3}',AddressArea='{4}'", currentUserInfo.TrueName, currentUserInfo.Phone, currentUserInfo.Email, currentUserInfo.Gender, currentUserInfo.AddressArea);
            if (webSitemodel.IsNeedDistributionRecommendCode == 1)
            {
                sql += string.Format(",DistributionOwner='{0}' ",currentUserInfo.DistributionOwner);
            }
            if (this.userBll.Update(currentUserInfo,sql , string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "保存成功!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

        }

        //private string EditUserInfoByOpenId(HttpContext context)
        //{
        //    //获取编辑数据
        //    string openId = context.Request["openId"];
        //    string Name = context.Request["Name"];
        //    string Phone = context.Request["Phone"];
        //    string Email = context.Request["Email"];
        //    string Company = context.Request["Company"];
        //    string Postion = context.Request["Postion"];
        //    string ArticleSourceType = context.Request["ArticleSourceType"];
        //    string ArticleSourceWXHao = context.Request["ArticleSourceWXHao"];
        //    string ArticleSourceWebSite = context.Request["ArticleSourceWebSite"];
        //    string ArticleSourceName = context.Request["ArticleSourceName"];

        //    //格式判断
        //    if (!string.IsNullOrWhiteSpace(Phone) && !Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "手机号码格式错误!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    if (!string.IsNullOrWhiteSpace(Email) && !Common.ValidatorHelper.EmailLogicJudge(Email))
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "邮箱地址错误!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    if (this.userInfo.UserType != 1)
        //    {
        //        if (openId != this.userInfo.WXOpenId)
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "无权更改他人的信息!";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //    }

        //    BLLJIMP.Model.UserInfo editUser = this.userBll.GetUserInfoByOpenId(openId);

        //    if (editUser == null)
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "用户不存在!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    editUser.TrueName = Name;

        //    //修改了手机号则验证失效，需要重新验证
        //    if (editUser.Phone != Phone)
        //    {
        //        editUser.Phone = Phone;
        //        editUser.IsPhoneVerify = 0;
        //    }

        //    editUser.Email = Email;
        //    editUser.Company = Company;
        //    editUser.Postion = Postion;
        //    editUser.ArticleSourceType = ArticleSourceType;
        //    editUser.ArticleSourceWebSite = ArticleSourceWebSite;
        //    editUser.ArticleSourceWXHao = ArticleSourceWXHao;
        //    editUser.ArticleSourceName = ArticleSourceName;

        //    if (this.userBll.Update(editUser))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "成功更新!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }
        //    else
        //    {
        //        resp.Status = 0;
        //        resp.Msg = "更新失败!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //}

        ///// <summary>
        ///// 获取当前用户
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string GetUserInfov(HttpContext context)
        //{
        //    //BLLJIMP.Model.UserInfo result = this.userBll.GetUserInfoByOpenId(context.Request[SessionKey.systemset.WXCurrOpenerOpenIDKey]);
        //    resp.Status = 1;
        //    resp.ExObj = currentUserInfo;
        //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        //}


        private string GetUserInfoByOpenId(HttpContext context)
        {
            BLLJIMP.Model.UserInfo userInfo = this.userBll.GetUserInfoByOpenId(context.Request[SessionKey.systemset.WXCurrOpenerOpenIDKey]);
            resp.Status = 1;
            resp.ExObj = userInfo;
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        private string GetUserInfo(HttpContext context)
        {
            resp.Status = 1;
            resp.ExObj = this.currentUserInfo;
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
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