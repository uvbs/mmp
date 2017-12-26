using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product.Category
{
    /// <summary>
    /// 分类列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
            var sourceData = bllMall.GetList<WXMallCategory>(sbWhere.ToString());
            var list = from p in sourceData
                       select new
                       {
                           category_id = p.AutoID,
                           category_name = p.CategoryName,
                           parent_id = p.PreID
                       };

            resp.status = true;
            resp.msg = "ok";
            resp.result = new
            {
                totalcount = list.Count(),
                list = list,//列表

            };

            bllMall.ContextResponse(context, resp);
        }

       
    }
}