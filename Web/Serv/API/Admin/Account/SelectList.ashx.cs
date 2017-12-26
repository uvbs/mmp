using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Account
{
    /// <summary>
    /// 账户列表下拉
    /// </summary>
    public class SelectList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string showAll = context.Request["show_all"];
            string showWebsiteOwner = context.Request["show_website_owner"];
            List<dynamic> resultList = new List<dynamic>();
            string blankStr ="";
            if (showAll == "1") {
                resultList.Add(new { value = "", text = "所有帐号" });
                blankStr = "\u3000";
            }
            if (showWebsiteOwner == "1") {
                resultList.Add(new { value = bllUser.WebsiteOwner, text = bllUser.WebsiteOwner });
                blankStr = "\u3000"+ "└";
            }
            
            List<UserInfo> list =  bllUser.GetSubAccountList(bllUser.WebsiteOwner,null,null);
            if (list.Count > 0)
            {
                List<string> userIdList = list.Select(p => p.UserID).ToList();
                var rList= from p in userIdList
                           select new {
                               value = p,
                               text = blankStr + p
                           };
                resultList.AddRange(rList);
            }
            apiResp.result = resultList;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}