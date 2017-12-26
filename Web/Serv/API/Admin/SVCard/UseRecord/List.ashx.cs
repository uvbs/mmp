using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard.UseRecord
{

    /// <summary>
    /// 储值卡使用记录
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            var list = bll.GetList<StoredValueCardUseRecord>(string.Format("WebsiteOwner='{0}' And MyCardId={1}", bll.WebsiteOwner, id));
            var resp = new
            {
                total = list.Count,
                rows = from p in list
                       select new {
                       use_date=p.UseDate.ToString(),
                       remark=p.Remark
                       
                       }
            };
            bll.ContextResponse(context, resp);
        }
    }


}