using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string trueName=context.Request["true_name"];
            string phone=context.Request["phone"];

            UserInfo model = new UserInfo();
            model.UserID = string.Format("PCUser{0}", Guid.NewGuid().ToString());//Guid
            model.Password = ZentCloud.Common.Rand.Str_char(12);
            model.UserType = 2;
            model.WebsiteOwner = bllUser.WebsiteOwner;
            model.Regtime = DateTime.Now;
            model.WXOpenId = "";
            model.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            model.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            model.LastLoginDate = DateTime.Now;
            model.LoginTotalCount = 1;
            model.TrueName = trueName;
            model.Phone = phone;
            if (bllUser.GetCount<UserInfo>(string.Format("Websiteowner='{0}' And Phone='{1}'",bllUser.WebsiteOwner,phone))>0)
            {
                apiResp.msg = "手机号重复";
                bllUser.ContextResponse(context,apiResp);
                return;
            }
            if (bllUser.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {

            }
            bllUser.ContextResponse(context,apiResp);

        }

        
    }
}