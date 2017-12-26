using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// 商品分类列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {    
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string type=context.Request["type"];
            string preIds = context.Request["pre_ids"];
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bllMall.WebsiteOwner));
            int totalCount = 0;
            var mallCategoryData = bllMall.GetCategoryList(pageIndex, pageSize, "", out totalCount,type);//bllMall.GetLit<ZentCloud.BLLJIMP.Model.WXMallCategory>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in mallCategoryData
                       select new
                       {
                           category_id=p.AutoID,
                           category_name=p.CategoryName,
                           category_description=p.Description,
                           category_img_url=p.CategoryImg,
                           category_parent_id=p.PreID,
                           sort = p.Sort,
                           type=p.Type
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
        }

    }
}