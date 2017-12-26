using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CompanyWebsite
{
    public partial class WebsiteConfig : System.Web.UI.Page
    {

        /// <summary>
        /// 
        /// </summary>
        public CompanyWebsite_Config config;
        /// <summary>
        /// 
        /// </summary>
        public List<string> toolBars = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> slides = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> disabledFields = new List<string>() { "TrueName", "Phone" };
        /// <summary>
        /// 
        /// </summary>
        public List<ListItem> fieldList = new List<ListItem>();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 
        /// </summary>
        BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
        /// <summary>
        /// 
        /// </summary>
        BLLSlide bllSlide = new BLLSlide();
        /// <summary>
        /// 
        /// </summary>
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        /// <summary>
        /// 
        /// </summary>
        BllScore bllScore = new BllScore();
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo currentWebsiteInfo;
        /// <summary>
        /// 文章活动底部导航
        /// </summary>
        public List<String> ArticleGroups;
        /// <summary>
        /// 登录配置
        /// </summary>
        protected JToken JTokenLogin;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            config = bll.GetCompanyWebsiteConfig();
            
            toolBars = bllCompanyWebSite.GetToolBarList(int.MaxValue, 1, bllCompanyWebSite.WebsiteOwner, null, null, false)
                .OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();
            slides = bllSlide.GetCurrWebsiteAllTypeList();

            #region 会员标准字段
            List<TableFieldMapping> baseFieldList = bllTableFieldMap.GetTableFieldMap(null, "ZCJ_UserInfo");
            List<TableFieldMapping> webFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, true);
            foreach (var item in baseFieldList)
            {
                if (fieldList.Exists(p => p.Value == item.Field)) continue;
                TableFieldMapping nWebFieldItem = webFieldList.FirstOrDefault(p => p.Field == item.Field);
                fieldList.Add(new ListItem
                {
                    Value = item.Field,
                    Text = nWebFieldItem == null ? item.MappingName : nWebFieldItem.MappingName,
                    Selected = nWebFieldItem !=null && nWebFieldItem.IsDelete==0 ? true:false
                });
            }
            List<string> baseFieldStringList = baseFieldList.Select(p => p.Field).ToList();
            foreach (var item in webFieldList.Where(p=>!baseFieldStringList.Contains( p.Field)))
            {
                if (fieldList.Exists(p => p.Value == item.Field)) continue;
                fieldList.Add(new ListItem
                {
                    Value = item.Field,
                    Text = item.MappingName,
                    Selected = item.IsDelete == 0 ? true : false
                });
            }
            #endregion 会员标准字段

            currentWebsiteInfo = bll.GetWebsiteInfoModelFromDataBase();

            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, string.Format(" WebsiteOwner = '{0}'", bll.WebsiteOwner), "AutoID,KeyType,BaseID");
            ArticleGroups = dataList.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();

            if (!string.IsNullOrEmpty(config.LoginConfigJson))
            {
                JTokenLogin = JToken.Parse(config.LoginConfigJson);
            }
                    
        }
    }
}