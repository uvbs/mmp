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
    public partial class EditPage : System.Web.UI.Page
    {
        BLLComponent bll = new BLLComponent();
        BLLPermission.BLLMenuPermission bllPms = new BLLPermission.BLLMenuPermission("");
        BLLMall bllMall = new BLLMall();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLTag bllTag = new BLLTag();
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWeisite = new BLLWebSite();
        MyCategories bllMyCategories = new MyCategories();
        protected UserInfo curUser;
        protected string backlist;
        protected string icoScript;
        protected string iconclasses;
        protected string slides;
        protected string toolbars;
        protected string course_cates;
        protected string mall_cates;
        protected string art_cates;
        protected string act_cates;
        protected string mall_tags;
        protected string template_types;
        protected string component;
        protected string template = "null";
        protected string login_info;
        protected string mallConfig;
        protected string component_model = "{}";
        protected string edit_template = "0";
        List<string> limitControls = BLLComponent.limitControls;
        protected bool canSaveTemp = false;
        protected string loginUserInfo;
        //微信绑定域名
        protected string strDomain = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetUserInfo();
            GetIcons();
            GetInitSelect();
            GetInitComponent();
            GetLoginInfo();
            GetMallConfig();
            GetWebsiteConfig();
        }
        private void GetWebsiteConfig()
        {
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
            else
            {
                strDomain = Request.Url.Authority;
            }
        }
        private void GetUserInfo()
        {
            curUser = bll.GetCurrentUserInfo();
            if (curUser.UserType == 1 || bllPms.CheckUserAndPmsKey(curUser.UserID,BLLPermission.Enums.PermissionSysKey.SaveComponentTemplate)) {
                canSaveTemp = true;
            }
            dynamic userObject = new { islogin = false };
            if (curUser != null)
            {
                userObject = new
                {
                    islogin = true,
                    user_id = curUser.AutoID,
                    user_name = curUser.UserID,
                    nick_name = bllUser.GetUserDispalyName(curUser),
                    avatar = bllUser.GetUserDispalyAvatar(curUser),
                    level = ""
                };
            }
            loginUserInfo = JsonConvert.SerializeObject(userObject);
        }
        private void GetInitComponent()
        {
            //组件
            string key = this.Request["key"];
            string component_id = this.Request["component_id"];
            backlist = this.Request["backlist"];
            BLLJIMP.Model.Component nComponent = new BLLJIMP.Model.Component();
            if (!string.IsNullOrWhiteSpace(key))
            {
                nComponent = bll.GetComponentByKey(key, bll.WebsiteOwner);
                if (nComponent == null)
                {
                    nComponent = new BLLJIMP.Model.Component() { ComponentKey = key };
                    nComponent.IsInitData = 1;
                    component = JsonConvert.SerializeObject(nComponent);
                    return;
                }
            }
            else if (!string.IsNullOrWhiteSpace(component_id)) {
                if (component_id == "0") {
                    nComponent.IsInitData = 1;
                    component = JsonConvert.SerializeObject(nComponent);
                    return;
                }
                nComponent = bll.GetByKey<BLLJIMP.Model.Component>("AutoId", component_id, true);
                if (nComponent == null){
                    this.Response.Write("页面未找到");
                    this.Response.End();
                    return;
                }
            }
            component = JsonConvert.SerializeObject(nComponent);
            if (nComponent.ComponentModelId == 0) return;
            //模板
            if(nComponent.ComponentTemplateId > 0){
                ComponentTemplate componentTemplate = bll.GetByKey<ComponentTemplate>("AutoId", nComponent.ComponentTemplateId.ToString());
                if (componentTemplate != null) template = JsonConvert.SerializeObject(componentTemplate);
            }

            //组件库
            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", nComponent.ComponentModelId.ToString());
            if (componentModel == null) return;

            List<ComponentModelField> componentModelFields = bll.GetListByKey<ComponentModelField>("ComponentModelKey", componentModel.ComponentModelKey)
                .OrderBy(p => p.ComponentFieldSort).ToList();
            dynamic cModel = new
            {
                component_model_id = componentModel.AutoId,
                component_model_key = componentModel.ComponentModelKey,
                component_model_name = componentModel.ComponentModelName,
                component_model_fields = (from p in componentModelFields.Where(p => p.ComponentFieldType >= 4 && p.ComponentFieldType != 8 && limitControls.Contains(p.ComponentField))
                                          select new
                                          {
                                              component_field_id = p.AutoId,
                                              component_field = p.ComponentField,
                                              component_field_name = p.ComponentFieldName,
                                              component_field_type = p.ComponentFieldType,
                                              component_field_data_value = p.ComponentFieldDataValue,
                                              disabled = false
                                          })
            };
            component_model = JsonConvert.SerializeObject(cModel);
        }
        private void GetInitSelect()
        {
            //读取广告
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            var slideData = bll.GetColList<Slide>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,Type");
            dynamic result = slideData.Select(p => p.Type).Distinct().OrderBy(p => p);
            slides = JsonConvert.SerializeObject(result);

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
            List<WXMallCategory> mallCateList = bllMall.GetCategoryList(1, int.MaxValue, null, out total);
            List<ListItem> mallCateItemList = new List<ListItem>();
            if (mallCateList.Count > 0)
            {
                mallCateItemList = bllMyCategories.GetCateListItem(bllMyCategories.GetCommCateModelList("AutoID", "PreID", "CategoryName", mallCateList), 0);
            }
            result = (from p in mallCateItemList
                      select new
                      {
                          cate_id = p.Value,
                          cate_name = p.Text,
                          pre_id = p.Attributes["PreID"].ToString()
                      }).Distinct();
            mall_cates = JsonConvert.SerializeObject(result);
            
            //读取课程分类
            List<WXMallCategory> courseCateList = bllMall.GetCategoryList(1, int.MaxValue, null, out total,type:"Course");
            List<ListItem> courseCateItemList = new List<ListItem>();
            if (courseCateList.Count > 0)
            {
                courseCateItemList = bllMyCategories.GetCateListItem(bllMyCategories.GetCommCateModelList("AutoID", "PreID", "CategoryName", courseCateList), 0);
            }
            result = (from p in courseCateItemList
                      select new
                      {
                          cate_id = p.Value,
                          cate_name = p.Text,
                          pre_id = p.Attributes["PreID"].ToString()
                      }).Distinct();
            course_cates = JsonConvert.SerializeObject(result);

            //读取商品标签
            List<MemberTag> tagList = bllTag.GetTags(bllTag.WebsiteOwner, null, 1, int.MaxValue, out total, "Mall");
            result = tagList.OrderBy(p => p.TagName).Select(p => p.TagName).Distinct();
            mall_tags = JsonConvert.SerializeObject(result);

            //读取文章分类
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

            //读取活动分类
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


            List<ArticleCategory> tempCateList = bllArticleCategory.GetCateList(out total, "CompTempType", 0, "Common", int.MaxValue, 1, null);
            result = (from p in tempCateList
                      select new
                      {
                          value = p.AutoID,
                          text = p.CategoryName
                      }).Distinct();
            template_types = JsonConvert.SerializeObject(result);

        }
        private void GetLoginInfo()
        {
            UserInfo curUser = bll.GetCurrentUserInfo();
            dynamic userObject =  new{islogin = false};
            if (curUser != null)
            {
                var levelItem = bllUser.GetUserLevelByHistoryTotalScore(curUser.HistoryTotalScore, "CommonScore");
                userObject = new
                {
                    islogin = true,
                    user_id = curUser.AutoID,
                    user_name = curUser.UserID,
                    nick_name = bllUser.GetUserDispalyName(curUser),
                    avatar = bllUser.GetUserDispalyAvatar(curUser),
                    level = levelItem == null ? "" : levelItem.LevelString
                };
            }
            login_info = JsonConvert.SerializeObject(userObject);
        }
        private void GetMallConfig()
        {
            mallConfig = new ZentCloud.BLLJIMP.BLLKeyValueData().GetMallConfigList();
        }

        private void GetIcons()
        {
            //头部图标引用
            iconclasses = bll.GetIcoClassArray();
            //图标js文件
            icoScript = bll.GetIcoScript();
        }

    }
}