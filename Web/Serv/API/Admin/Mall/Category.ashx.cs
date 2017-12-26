using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Category : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
            var sourceData= bllMall.GetList<WXMallCategory>(sbWhere.ToString());
            var list = from p in sourceData
                       select new
                       {
                           category_id = p.AutoID,
                           category_name = p.CategoryName,
                           parent_id=p.PreID
                       };

            var data = new
            {
                totalcount = list.Count(),
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }


    }
}