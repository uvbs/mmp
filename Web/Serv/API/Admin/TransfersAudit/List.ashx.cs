using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TransfersAudit
{
    /// <summary>
    /// 审核列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLTransfersAudit bll = new BLLJIMP.BLLTransfersAudit();
        public void ProcessRequest(HttpContext context)
        {
            if (!bll.IsTransfersAuditPer(CurrentUserInfo) && (CurrentUserInfo.WebsiteOwner != bll.WebsiteOwner) && (CurrentUserInfo.UserType != 1))
            {
                apiResp.status = false;
                apiResp.msg = "您没有审核权限";
                bll.ContextResponse(context, apiResp);
                return;
            }
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string status = context.Request["status"];
            string keyWord=context.Request["keyword"];
            string fromDate=context.Request["from_date"];
            string toDate=context.Request["to_date"];
            string type=context.Request["type"];
            int totalCount = 0;
            var sourceData = bll.List(pageIndex, pageSize, bll.WebsiteOwner, status,fromDate,toDate,type, keyWord, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           id=p.AutoId,
                           tran_info=p.TranInfo,
                           type=p.Type,
                           amount=p.Amount,
                           status=p.Status,
                           insert_date=p.InsertDate.ToString()


                       };

            var resp = new
            {
                total = totalCount,
                rows = list
            };
            bll.ContextResponse(context, resp);

           
        }


    }
}