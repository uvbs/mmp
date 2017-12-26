using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.SupplierChannel
{
    /// <summary>
    /// SettlementList 的摘要说明
    /// </summary>
    public class SettlementList : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string status = context.Request["status"];//状态
            string date = context.Request["date"];//日期
            string settlementId = context.Request["settlement_id"];//结算单号
            string supplierUserId = context.Request["supplier_user_id"];//供应商账号
            if (bllUser.IsSupplier(currentUserInfo))
            {
                supplierUserId = currentUserInfo.UserID;
            }
            int totalCount = 0;
            var sourceData = bllMall.SupplierChannelSettlementList(bllMall.WebsiteOwner, supplierUserId, pageIndex, pageSize, status, settlementId, date, out totalCount);
            var data = from p in sourceData
                       select new
                       {
                           id = p.AutoId,
                           settlement_id = p.SettlementId,
                           user_id=p.UserId,
                           channel_name = p.ChannelName,
                           from_date = p.FromDate.ToString(),
                           to_date = p.ToDate.ToString(),
                           status = p.Status,
                           settlement_total_amount = p.SettlementTotalAmount,
                           insert_date = p.InsertDate,
                           remark = p.Remark,
                           img_url = p.ImgUrl
                       };

            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = totalCount,
                list = data

            };
            bllMall.ContextResponse(context, apiResp);

        }
    }
}