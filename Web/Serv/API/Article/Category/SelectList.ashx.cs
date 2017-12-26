using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.Common;
using ZentCloud.Common.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article.Category
{
    /// <summary>
    /// SelectList 的摘要说明
    /// </summary>
    public class SelectList : BaseHandlerNoAction
    {

        BLLArticleCategory bll = new BLLArticleCategory();
        MyCategoriesV2 myCategories = new MyCategoriesV2();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            if (string.IsNullOrWhiteSpace(type)) type = "Article";
            string websiteowner = context.Request["websiteowner"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bll.WebsiteOwner;
            int totalCount = 0;
            List<BLLJIMP.Model.ArticleCategory> categoryData = bll.GetCateList(out totalCount, type, 0, websiteowner, int.MaxValue, 1, null);
            
            List<MyCategoryV2Model> myCategoryModel = myCategories.GetCommCateModelList("AutoID","PreID","CategoryName",categoryData);
            List<ListItem> list = myCategories.GetCateListItem(myCategoryModel, "0");
            apiResp.result = from p in list
                             select new
                             {
                                 value = p.Value,
                                 text = p.Text
                             };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}