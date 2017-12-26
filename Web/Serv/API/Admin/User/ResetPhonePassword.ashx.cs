using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// ResetPassword 的摘要说明
    /// </summary>
    public class ResetPhonePassword : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLSMS bllSms = new BLLSMS("");
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bllUser.WebsiteOwner;
            UserInfo ouser = bllUser.GetUserInfoByAutoID(Convert.ToInt32(id), websiteOwner);
            if (ouser == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "会员未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            ouser.Password = ZentCloud.Common.Rand.Number(6);
            bool smsBool = false;
            string smsMsg = "";
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            bllSms.SendSmsMisson(ouser.Phone, "您的天下华商月供宝密码已被重置，初始密码为：" + ouser.Password, "", website.SmsSignature, out smsBool, out smsMsg);
            if (!smsBool)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "发送短信密码失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(ouser, string.Format("Password='{0}',PayPassword=null", ouser.Password),
                string.Format("AutoID={0}", ouser.AutoID)) <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "重置失败";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "重置完成，新密码将发送到会员的手机";
            bllUser.ContextResponse(context, apiResp);
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