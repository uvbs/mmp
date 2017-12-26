using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// 账户余额变动记录
    /// </summary>
    public class AccountAmountRecordList : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户Bll
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string userId = context.Request["userId"];
            int totalCount = 0;
            var data = bllUser.GetUserCreditAcountDetailsList(pageIndex, pageSize, userId,out totalCount);
            var list = from p in data
                       select new
                       {
                           remark=p.AddNote,
                           time=p.AddTime.ToString()
                           
                       };

            apiResp.result = new
                {
                    totalcount = totalCount,
                    list = list//列表
                };
            apiResp.status = true;
            apiResp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }



    }
}