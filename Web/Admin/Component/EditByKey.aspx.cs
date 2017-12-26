using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Admin.Component
{
    public partial class EditByKey : System.Web.UI.Page
    {
        BLLJIMP.BLLComponent bll = new BLLJIMP.BLLComponent();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        BLLPermission.BLLMenuPermission bllPms = new BLLPermission.BLLMenuPermission("");
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        MyCategories bllMyCategories = new MyCategories();
        protected string slides;
        protected string component;
        protected string component_model;
        protected string common_components;
        protected string toolbars;
        protected string mall_cates;
        protected string mall_tags;
        protected string keyname;
        protected string webpms_groups;
        protected string art_cates;
        protected string act_cates;
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<long> groupids = bllPms.GetPmsGroupIDByUser(bll.WebsiteOwner);
            if (groupids.Count > 0) webpms_groups = Common.MyStringHelper.ListToStr(groupids, "", ",");
            string key = this.Request["key"];
            BLLJIMP.Model.Component nComponent = bll.GetComponentByKey(key, bll.WebsiteOwner);
            if (nComponent == null)
            {
                Common.WebMessageBox.Show(this, "组件未找到");
                return;
            }
            keyname = bll.GetComponentNameByKey(key);
            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", nComponent.ComponentModelId.ToString());
            if (componentModel == null)
            {
                Common.WebMessageBox.Show(this, "模板未找到");
                return;
            }
            List<ComponentModelField> componentModelFields = bll.GetListByKey<ComponentModelField>("ComponentModelKey", componentModel.ComponentModelKey)
                .OrderBy(p => p.ComponentFieldSort).ToList();
            dynamic cModel = new
            {
                component_model_id = componentModel.AutoId,
                component_model_name = componentModel.ComponentModelName,
                component_model_type = componentModel.ComponentModelType,
                component_model_link_url = componentModel.ComponentModelLinkUrl,
                component_model_html_url = componentModel.ComponentModelHtmlUrl,
                component_model_fields = (from p in componentModelFields
                                          select new
                                          {
                                              component_field_id = p.AutoId,
                                              component_field = p.ComponentField,
                                              component_field_name = p.ComponentFieldName,
                                              component_field_type = p.ComponentFieldType,
                                              component_field_data_type = p.ComponentFieldType,
                                              component_field_data_value = p.ComponentFieldDataValue
                                          })
            };

            component = JsonConvert.SerializeObject(nComponent);
            component_model = JsonConvert.SerializeObject(cModel);


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
            //读取商品分类
            List<BLLJIMP.Model.WXMallCategory> mallCateList = bllMall.GetCategoryList(1, int.MaxValue, null, out total);
            result = (from p in mallCateList
                              select new
                              {
                                  cate_id = p.AutoID,
                                  cate_name = p.CategoryName
                              }).Distinct();
            mall_cates = JsonConvert.SerializeObject(result);

            //读取商品标签
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


            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}