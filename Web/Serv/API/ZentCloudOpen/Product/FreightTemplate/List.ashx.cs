using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product.FreightTemplate
{
    /// <summary>
    /// 运费模板
    /// </summary>
    public class List : BaseHanderOpen
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            var sourceData = bllMall.GetFreightTemplateList();
            var list = from p in sourceData
                       select new
                       {
                           freight_template_id = p.TemplateId,
                           freight_template_name = p.TemplateName,
                           //is_enable = p.IsEnable,
                           //valuation_rules = GetFreightTemplateAreaObj(bllMall.GetFreightTemplateRuleList(p.TemplateId)),
                           //last_modiffty_date = p.LastModifyDate != null ? bllMall.GetTimeStamp((DateTime)p.LastModifyDate) : 0,
                           //calc_type = p.CalcType
                       };
            resp.status = true;
            resp.msg = "ok";
            resp.result = new
            {
                totalcount = list.Count(),
                list = list,//列表

            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            
        }

        
    }
}