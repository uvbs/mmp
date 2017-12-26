using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.Text;
using ZentCloud.BLLJIMP.Model.Weixin;
using ZentCloud.Common;
using System.IO;
namespace ZentCloud.JubitIMP.Web.Handler.WeiXin
{
    /// <summary>
    /// WXRegistration 的摘要说明
    /// </summary>
    public class WXRegistration : IHttpHandler, IRequiresSessionState
    {

        AshxResponse resp = new AshxResponse();
        BLLWeixin weixinBll = new BLLWeixin("");
        BLLUser userBll;
        ZentCloud.BLLJIMP.Model.UserInfo userModel;
        SystemSet systemSet;
        public void ProcessRequest(HttpContext context)
        {
            string result = "false";
            try
            {
                context.Response.ContentType = "text/plain";
                context.Response.Expires = 0;
                string action = context.Request["Action"];
                systemSet = this.weixinBll.GetSysSet();
                userBll = new BLLUser("");
                weixinBll = new BLLWeixin("");
                switch (action)
                {
                    case "WXReg":
                        result = WXReg(context);//注册微信会员
                        break;
                    //case "CheckIsWXMember":
                    //    result = CheckIsWXMember(context);
                    //    break;
                    //CheckIsWXMemberWithWeixinVerify
                    case "CheckIsWXMemberWithWeixinVerify":
                        result = CheckIsWXMemberWithWeixinVerify(context);
                        break;
                    case "GetWeixinMemberInfo"://获取微信会员信息
                        result = GetWeixinMemberInfo(context);
                        break;

                    case "EditWeixinMemberInfo"://编辑微信会员信息
                        result = EditWeixinMemberInfo(context);
                        break;
                    //WXRegWithWeixinVerify
                    case "WXRegWithWeixinVerify"://编辑微信会员信息
                        result = WXRegWithWeixinVerify(context);
                        break;
                }

            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);

        }


        /// <summary>
        /// 编辑微信会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditWeixinMemberInfo(HttpContext context)
        {
            try
            {
                WXMemberInfo memberModel = new WXMemberInfo();
                string msg = "";
                if (this.GetWeixinMemberInfo(context, out memberModel, out userModel, out msg))
                {
                    //获取编辑数据
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

                    memberModel.Name = name;
                    memberModel.Phone = phone;
                    memberModel.Email = email;
                    memberModel.Company = company;
                    memberModel.Postion = position;

                    if (context.Session[systemSet.WXCurrOpenerUserInfoKey] != null)
                    {
                        //存储拉取到的用户信息
                        BLLJIMP.Model.Weixin.WeixinUserInfo usModel = (BLLJIMP.Model.Weixin.WeixinUserInfo)context.Session[systemSet.WXCurrOpenerUserInfoKey];
                        memberModel.WXCity = usModel.City;
                        memberModel.WXCountry = usModel.Country;
                        memberModel.WXHeadimgurl = usModel.HeadImgUrl;
                        memberModel.WXNickname = usModel.NickName;
                        memberModel.WXPrivilege = usModel.Privilege == null ? "" : Common.JSONHelper.ObjectToJson(usModel.Privilege);
                        memberModel.WXProvince = usModel.Province;
                        memberModel.WXSex = usModel.Sex;

                    }

                    if (context.Session[systemSet.WXOAuthAccessTokenEntityKey] != null)
                    {
                        //存储授权信息
                        BLLWXOAuthModule.WXOAuthAccessTokenEntity acModel = (BLLWXOAuthModule.WXOAuthAccessTokenEntity)context.Session[systemSet.WXOAuthAccessTokenEntityKey];
                        memberModel.AccessToken = acModel.AccessToken;
                        memberModel.RefreshToken = acModel.RefreshToken;
                        memberModel.Scope = acModel.Scope;

                    }

                    if (this.weixinBll.Update(memberModel))
                    {
                        resp.Status = 1;
                        resp.Msg = "更新成功!";
                    }
                    else
                    {
                        resp.Status = 0;
                        resp.Msg = "更新会员信息失败!";
                    }

                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = msg;
                }

            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取微信会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWeixinMemberInfo(HttpContext context)
        {
            try
            {
                WXMemberInfo memberModel = new WXMemberInfo();
                string msg = "";
                if (this.GetWeixinMemberInfo(context, out memberModel, out userModel, out msg))
                {
                    resp.Status = 1;
                    resp.ExObj = memberModel;
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = msg;
                }

            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.Msg = ex.Message;
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取传入的用户信息和微信会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="memberModel"></param>
        /// <param name="userInfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool GetWeixinMemberInfo(HttpContext context, out WXMemberInfo memberModel, out ZentCloud.BLLJIMP.Model.UserInfo userInfo, out string msg)
        {
            //取得OpenID支持两种模式：传入模式，授权Session模式
            string openId = context.Request[systemSet.WXCurrOpenerOpenIDKey];
            string userAutoIDHex = context.Request[systemSet.UserAutoIDHexKey];
            memberModel = new WXMemberInfo();
            msg = "";

            //判断用户微信是否已认证，认证则根据Session取openId，否则根据Url取得
            userInfo = userBll.GetUserInfoByAutoIDHex(userAutoIDHex);

            if (userInfo == null)
            {
                msg = "用户不存在!";
                return false;
            }

            if (userModel.IsWeixinVerify == 1 && context.Session[systemSet.WXCurrOpenerOpenIDKey] != null)
            {
                //如果是微信认证用户而且认证取到的openID也存在，则会忽略浏览器url上的openid
                openId = context.Session[systemSet.WXCurrOpenerOpenIDKey].ToString();
            }

            memberModel = weixinBll.GetWXMemberInfo(userModel.UserID, openId);

            if (memberModel == null)
            {
                msg = "会员不存在!";
                return false;
            }

            return true;
        }


        ///// <summary>
        ///// 检查是否是微信会员
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string CheckIsWXMember(HttpContext context)
        //{
        //    string userId = context.Request["userId"];
        //    string wxOpenId = context.Request["wxOpenId"];
        //    return this.weixinBll.CheckIsWXMember(userId, wxOpenId).ToString().ToLower();
        //}

        /// <summary>
        /// 检查是否是微信会员(添加了微信认证机制的检查)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CheckIsWXMemberWithWeixinVerify(HttpContext context)
        {
            string result = "";

            try
            {
                WXMemberInfo memberModel = new WXMemberInfo();
                string msg = "";
                result = this.GetWeixinMemberInfo(context, out memberModel, out userModel, out msg).ToString().ToLower();
            }
            catch (Exception ex)
            {
                result = "false";
            }

            return result;

        }


        /// <summary>
        ///注册微信会员(添加了微信认证机制的检查)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string WXRegWithWeixinVerify(HttpContext context)
        {

            ZentCloud.BLLJIMP.Model.UserInfo userInfo;
            WXMemberInfo model = new WXMemberInfo();
            string userIDHex = context.Request[systemSet.UserAutoIDHexKey];

            userInfo = this.userBll.GetUserInfoByAutoIDHex(userIDHex);

            if (userInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "用户名不存在!";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            model.UserID = userInfo.UserID;

            model.Name = context.Request["Name"];
            model.Company = context.Request["Company"];
            model.Postion = context.Request["Postion"];
            model.WeixinNumber = context.Request["WeixinNumber"];
            model.WeixinOpenID = context.Request[systemSet.WXCurrOpenerOpenIDKey];

            //判断是否是认证的微信，并且是否有微信号存在授权的Session里
            if (userInfo.IsWeixinVerify == 1 && context.Session[systemSet.WXCurrOpenerOpenIDKey] != null)
            {
                //如果是微信认证用户而且认证取到的openID也存在，则会忽略浏览器url上的openid
                model.WeixinOpenID = context.Session[systemSet.WXCurrOpenerOpenIDKey].ToString();
            }

            //判断是否有拉去到的微信信息，如有则存储下来
            if (context.Session[systemSet.WXOAuthAccessTokenEntityKey] != null)
            {
                //存储授权信息
                BLLWXOAuthModule.WXOAuthAccessTokenEntity acModel = (BLLWXOAuthModule.WXOAuthAccessTokenEntity)context.Session[systemSet.WXOAuthAccessTokenEntityKey];
                model.AccessToken = acModel.AccessToken;
                model.RefreshToken = acModel.RefreshToken;
                model.Scope = acModel.Scope;
            }
            if (context.Session[systemSet.WXCurrOpenerUserInfoKey] != null)
            {
                //存储拉取到的用户信息
                BLLJIMP.Model.Weixin.WeixinUserInfo usModel = (BLLJIMP.Model.Weixin.WeixinUserInfo)context.Session[systemSet.WXCurrOpenerUserInfoKey];
                model.WXCity = usModel.City;
                model.WXCountry = usModel.Country;
                model.WXHeadimgurl = usModel.HeadImgUrl;
                model.WXNickname = usModel.NickName;
                model.WXPrivilege = usModel.Privilege == null ? "" : Common.JSONHelper.ObjectToJson(usModel.Privilege);
                model.WXProvince = usModel.Province;
                model.WXSex = usModel.Sex;
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                resp.Status = 0;
                resp.Msg = "请输入姓名";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.Phone = context.Request["Phone"];
            if (string.IsNullOrEmpty(model.Phone))
            {
                resp.Status = 0;
                resp.Msg = "请输入手机号码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!Common.ValidatorHelper.PhoneNumLogicJudge(model.Phone))
            {
                resp.Status = 0;
                resp.Msg = "手机号码格式不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.Email = context.Request["Email"];
            if (!string.IsNullOrEmpty(model.Email))
            {
                if (!Common.ValidatorHelper.EmailLogicJudge(model.Email))
                {
                    resp.Status = 0;
                    resp.Msg = "Email格式不正确";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            if (weixinBll.GetCount<WXMemberInfo>(string.Format("UserID='{0}' and WeixinOpenID='{1}'", userInfo.UserID, model.WeixinOpenID)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "您已经注册过了!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            model.MemberID = long.Parse(weixinBll.GetGUID(ZentCloud.BLLJIMP.TransacType.WXMemberInfoAdd));
            if (weixinBll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "注册成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "注册失败！请重试或联系管理员";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        ///注册微信会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string WXReg(HttpContext context)
        {

            ZentCloud.BLLJIMP.Model.UserInfo userInfo;
            WXMemberInfo model = new WXMemberInfo();
            string userId = context.Request["userId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                int AutoId = Convert.ToInt32(context.Request["Aid"], 16); //ZCJ_UserInfo AutoID 
                userInfo = weixinBll.Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("AutoID={0}", AutoId));
            }
            else
            {
                userInfo = new BLLUser(userId).GetUserInfo(userId);
            }
            if (userInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "用户名不存在!";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            model.UserID = userInfo.UserID;

            model.Name = context.Request["Name"];
            model.Company = context.Request["Company"];
            model.Postion = context.Request["Postion"];
            model.WeixinNumber = context.Request["WeixinNumber"];
            model.WeixinOpenID = context.Request["WeixinOpenID"];

            //判断是否有拉去到的微信信息，如有则存储下来
            if (context.Session[systemSet.WXOAuthAccessTokenEntityKey] != null)
            {
                try
                {
                    //存储授权信息
                    BLLWXOAuthModule.WXOAuthAccessTokenEntity acModel = (BLLWXOAuthModule.WXOAuthAccessTokenEntity)context.Session[systemSet.WXOAuthAccessTokenEntityKey];
                    model.AccessToken = acModel.AccessToken;
                    model.RefreshToken = acModel.RefreshToken;
                    model.Scope = acModel.Scope;
                    //using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
                    //{
                    //    sw.WriteLine(string.Format("{0} acModelEX：{1}", DateTime.Now.ToString(), JSONHelper.ObjectToJson(acModel)));
                    //}
                }
                catch (Exception ex)
                {
                    //using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
                    //{
                    //    sw.WriteLine(string.Format("{0} 存储授权信息EX：{1}", DateTime.Now.ToString(), ex.Message));
                    //}
                }
            }
            if (context.Session[systemSet.WXCurrOpenerUserInfoKey] != null)
            {
                try
                {
                    //存储拉取到的用户信息
                    BLLJIMP.Model.Weixin.WeixinUserInfo usModel = (BLLJIMP.Model.Weixin.WeixinUserInfo)context.Session[systemSet.WXCurrOpenerUserInfoKey];
                    model.WXCity = usModel.City;
                    model.WXCountry = usModel.Country;
                    model.WXHeadimgurl = usModel.HeadImgUrl;
                    model.WXNickname = usModel.NickName;
                    model.WXPrivilege = usModel.Privilege == null ? "" : Common.JSONHelper.ObjectToJson(usModel.Privilege);
                    model.WXProvince = usModel.Province;
                    model.WXSex = usModel.Sex;

                    //using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
                    //{
                    //    sw.WriteLine(string.Format("{0} usModel：{1}", DateTime.Now.ToString(), JSONHelper.ObjectToJson(usModel)));
                    //}
                }
                catch (Exception ex)
                {
                    //using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
                    //{
                    //    sw.WriteLine(string.Format("{0} 存储拉取到的用户信息EX：{1}", DateTime.Now.ToString(), ex.Message));
                    //}
                }
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                resp.Status = 0;
                resp.Msg = "请输入姓名";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.Phone = context.Request["Phone"];
            if (string.IsNullOrEmpty(model.Phone))
            {
                resp.Status = 0;
                resp.Msg = "请输入手机号码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!Common.ValidatorHelper.PhoneNumLogicJudge(model.Phone))
            {
                resp.Status = 0;
                resp.Msg = "手机号码格式不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            model.Email = context.Request["Email"];
            if (!string.IsNullOrEmpty(model.Email))
            {
                if (!Common.ValidatorHelper.EmailLogicJudge(model.Email))
                {
                    resp.Status = 0;
                    resp.Msg = "Email格式不正确";
                    return Common.JSONHelper.ObjectToJson(resp);

                }


            }
            if (weixinBll.GetCount<WXMemberInfo>(string.Format("UserID='{0}' and WeixinOpenID='{1}'", userInfo.UserID, model.WeixinOpenID)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "您已经注册过了!";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            model.MemberID = long.Parse(weixinBll.GetGUID(ZentCloud.BLLJIMP.TransacType.WXMemberInfoAdd));
            if (weixinBll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "注册成功!";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "注册失败！请重试或联系管理员";


            }
            return Common.JSONHelper.ObjectToJson(resp);
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