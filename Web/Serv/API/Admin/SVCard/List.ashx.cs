using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard
{
    /// <summary>
    /// 储值卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string status = context.Request["status"];
            string keyword = context.Request["keyword"];
            string websiteOwner = bll.WebsiteOwner;
            int total = bll.GetCount(websiteOwner, status, keyword);
            List<StoredValueCard> list = new List<StoredValueCard>();
            if (total > 0) list = bll.GetList(rows, page, websiteOwner, status, keyword);

            apiResp.msg = "查询储值卡列表";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = total,
                list = from p in list 
                       select new{
                           id = p.AutoId,
                           amount = p.Amount,
                           name = p.Name,
                           bg_img = p.BgImage,
                           valid_to = !p.ValidTo.HasValue?"":p.ValidTo.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                           max_count = p.MaxCount,
                           send_count = p.SendCount,
                           status = p.Status
                       }
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}