using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.Common.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Component
{
    public partial class Edit : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        BLLPermission.BLLMenuPermission bllPms = new BLLPermission.BLLMenuPermission("");
        MyCategories bllMyCategories = new MyCategories();
        protected string slides;
        protected string common_components;
        protected string toolbars;
        protected string mall_cates;
        protected string art_cates;
        protected string act_cates;
        protected string mall_tags;
        protected string webpms_groups;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<long> groupids  = bllPms.GetPmsGroupIDByUser(bll.WebsiteOwner);
            if (groupids.Count > 0) webpms_groups = Common.MyStringHelper.ListToStr(groupids, "", ",");

            //读取广告
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            var slideData = bll.GetColList<BLLJIMP.Model.Slide>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,Type");
            dynamic result = slideData.Select(p => p.Type).Distinct().OrderBy(p => p);
            slides = JsonConvert.SerializeObject(result);

            //读取组件
            sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", bll.WebsiteOwner);
            sbWhere.AppendFormat(" AND ComponentType != 'page' ");
            var componentList = bll.GetColList<BLLJIMP.Model.Component>(int.MaxValue, 1, sbWhere.ToString(), "AutoId,ComponentName,ComponentType");

            result = from p in componentList
                             select new
                             {
                                 component_id = p.AutoId,
                                 component_name = p.ComponentName,
                                 component_type = p.ComponentType
                             };
            common_components = JsonConvert.SerializeObject(result);

            //读取底部工具栏
            sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' OR WebsiteOwner Is Null ", bll.WebsiteOwner);
            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,UseType");
            result = (from p in dataList
                              select new
                              {
                                  key_type = p.KeyType,
                                  use_type = p.UseType
                              }).Distinct();
            toolbars = JsonConvert.SerializeObject(result);
            int total = 0;
            List<BLLJIMP.Model.WXMallCategory> mallCateList = bllMall.GetCategoryList(1, int.MaxValue, null, out total);
            result = (from p in mallCateList
                              select new
                              {
                                  cate_id = p.AutoID,
                                  cate_name = p.CategoryName
                              }).Distinct();
            mall_cates = JsonConvert.SerializeObject(result);

            List<BLLJIMP.Model.MemberTag> tagList = bllTag.GetTags(bllTag.WebsiteOwner, null, 1, int.MaxValue, out total, "Mall");
            result = tagList.OrderBy(p=>p.TagName).Select(p=>p.TagName).Distinct();
            mall_tags = JsonConvert.SerializeObject(result);

            List<ArticleCategory> artCateList = bllArticleCategory.GetCateList(out total, "Article", null, bllArticleCategory.WebsiteOwner, int.MaxValue, 1);
            List<ListItem> artCateItemList = new List<ListItem>();
            if (artCateList.Count > 0)
            {
                artCateItemList = bllMyCategories.GetCateListItem(bllMyCategories.GetCommCateModelList("AutoID", "PreID", "CategoryName", artCateList), 0);
            }
            result = (from p in artCateItemList
                      select new
                      {
                          cate_id = p.Value,
                          cate_name = p.Text
                      }).Distinct();
            art_cates = JsonConvert.SerializeObject(result);

            List<ArticleCategory> actCateList = bllArticleCategory.GetCateList(out total, "Activity", null, bllArticleCategory.WebsiteOwner, int.MaxValue, 1);
            List<ListItem> actCateItemList = new List<ListItem>();
            if (actCateList.Count > 0)
            {
                actCateItemList = bllMyCategories.GetCateListItem(bllMyCategories.GetCommCateModelList("AutoID", "PreID", "CategoryName", actCateList), 0);
            }
            result = (from p in actCateItemList
                      select new
                      {
                          cate_id = p.Value,
                          cate_name = p.Text
                      }).Distinct();
            act_cates = JsonConvert.SerializeObject(result);

        }
    }
}