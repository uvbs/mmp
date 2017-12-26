using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 网站 BLL
    /// </summary>
    public class BLLWebSite : BLL
    {
        BLLPermission.BLLMenuPermission bllUserPmsGroupRelation = new BLLPermission.BLLMenuPermission("");
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public BLLWebSite()
            : base()
        {

        }
        public string GetCopyright()
        {
            if(string.IsNullOrWhiteSpace(WebsiteOwner)) return "";
            WebsiteInfo website = GetColByKey<WebsiteInfo>("WebsiteOwner", WebsiteOwner, "WebsiteOwner,TotalAmountShowName");
            if (website == null) return "";
            return website.ArticleBottomCode;
        }
        /// <summary>
        /// 获取幻灯片列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyWebsite_Projector> GetProjectorList()
        {
            return GetList<CompanyWebsite_Projector>(string.Format(" WebsiteOwner='{0}' Order By PlayIndex ASC", WebsiteOwner));
        }
        /// <summary>
        /// 获取中部导航列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyWebsite_Navigate> GetNavigateList()
        {
            return GetList<CompanyWebsite_Navigate>(string.Format(" WebsiteOwner='{0}' Order By PlayIndex ASC", WebsiteOwner));
        }
        /// <summary>
        /// 获取工具列表
        /// </summary>
        /// <returns></returns>
        public List<CompanyWebsite_ToolBar> GetToolBarList()
        {
            return GetList<CompanyWebsite_ToolBar>(string.Format(" WebsiteOwner='{0}' Order By PlayIndex ASC", WebsiteOwner));
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        public List<JuActivityInfo> GetArticleList(string categoryid = "", string name = "")
        {
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
            if (!string.IsNullOrEmpty(categoryid))
            {
                string sql = string.Format("with a as(select AutoID from ZCJ_ArticleCategory where AutoID={0} union all select x.AutoID from ZCJ_ArticleCategory x,a where x.PreID=a.AutoID) select * from a", categoryid);
                var list = ZentCloud.ZCBLLEngine.BLLBase.Query<CategoryIDModel>(sql);
                if (list.Count > 0)
                {
                    categoryid = string.Join(",", list.SelectMany(p => new List<int>() { (int)p.AutoID }));
                }
                sbWhere.AppendFormat(" And CategoryId in({0})", categoryid);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sbWhere.AppendFormat(" And (ActivityName like '%{0}%' Or Summary like '%{0}%')", name);

            }
            sbWhere.Append(" ORDER BY ISNULL(Sort,99999) ASC,LastUpdateDate DESC,CreateDate DESC");
            return GetList<JuActivityInfo>(sbWhere.ToString());


        }
        /// <summary>
        /// 获取文章列表V1
        /// </summary>
        /// <returns></returns>
        public List<JuActivityInfo> GetArticleListV1(string categoryId = "", string name = "")
        {
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId ={0}", categoryId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sbWhere.AppendFormat(" And (ActivityName like '%{0}%' Or Summary like '%{0}%')", name);

            }
            sbWhere.Append(" ORDER BY ISNULL(Sort,99999) ASC,LastUpdateDate DESC,CreateDate DESC");
            return GetList<JuActivityInfo>(sbWhere.ToString());


        }


        /// <summary>
        /// 文章活动分类模型
        /// </summary>
        public class CategoryIDModel : ZCBLLEngine.ModelTable
        {
            public int AutoID { get; set; }

        }
        /// <summary>
        /// 获取所有微网站模板
        /// </summary>
        /// <returns></returns>
        public List<CompanyWebsiteTemplate> GetCompanyWebsiteTemplateList()
        {

            return GetList<CompanyWebsiteTemplate>();

        }
        /// <summary>
        /// 获取网站配置信息
        /// </summary>
        /// <returns></returns>
        public CompanyWebsite_Config GetConfig()
        {
            
            var result = Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));

            if (result != null) FilterCompanyWebsiteConfig(ref result);

            return result;

        }
        /// <summary>
        /// 获取当前站点配置信息
        /// </summary>
        /// <returns></returns>
        public CompanyWebsite_Config GetCompanyWebsiteConfig()
        {
            var data = Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));

            if (data == null) data = new CompanyWebsite_Config();

            FilterCompanyWebsiteConfig(ref data);

            return data;
        }
       
        public CompanyWebsite_Config GetCompanyWebsiteConfig(string websiteOwner)
        {
            var data = Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", websiteOwner));

            if (data == null) data = new CompanyWebsite_Config();

            FilterCompanyWebsiteConfig(ref data, websiteOwner);

            return data;
        }
        /// <summary>
        /// 填充默认配置，当配置项为空的时候不让它为空
        /// </summary>
        /// <param name="config"></param>
        public void FilterCompanyWebsiteConfig(ref CompanyWebsite_Config config,string websiteOwner = "")
        {
            var currentWebsiteInfo = new WebsiteInfo();
            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            }
            else
            {
                currentWebsiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);
            }
            if (string.IsNullOrWhiteSpace(config.WebsiteTitle))
            {
                config.WebsiteTitle = currentWebsiteInfo.WebsiteName;
            }
            if (string.IsNullOrWhiteSpace(config.WebsiteDescription))
            {
                config.WebsiteDescription = "欢迎访问" + currentWebsiteInfo.WebsiteName;
            }
            if (string.IsNullOrWhiteSpace(config.Copyright))
            {
                config.Copyright = currentWebsiteInfo.WebsiteName + "@" + DateTime.Now.Year;
            }
            if (string.IsNullOrWhiteSpace(config.DistributionQRCodeIcon))
            {
                //config.DistributionQRCodeIcon = "http://open-files.comeoncloud.net/www/fuqijiaoyu/jubit/image/20160526/399F479B65E04BAA9B4AB8B35793F1C6.png";
                
            }
            else
            {
                //处理已经把图片变为至云之家的站点数据
                if (currentWebsiteInfo.WebsiteOwner != "comeoncloud" && config.DistributionQRCodeIcon == "http://open-files.comeoncloud.net/www/fuqijiaoyu/jubit/image/20160526/399F479B65E04BAA9B4AB8B35793F1C6.png")
                {
                    config.DistributionQRCodeIcon = "";
                }
            }
            if (string.IsNullOrWhiteSpace(config.WeixinAccountNickName))
            {
                config.WeixinAccountNickName = currentWebsiteInfo.WebsiteName;
            }
            
        }

        /// <summary>
        /// 更新网站配置
        /// </summary>
        /// <param name="websiteTitle"></param>
        /// <param name="copyright"></param>
        /// <returns></returns>
        public bool UpdateCompanyWebsiteConfig(string websiteTitle, string copyright, string websiteimg, string websitedescription, string groupName, string shopAdType, string buttomtoolbar, string memberStandard
            , string haveComment, string memberStandardDescription, string myCardCouponsTitle, string weixinAccountNickName, string distributionQRCodeIcon, string articleToolBarGrous, string activityToolBarGrous, string groupBuyIndexUrl, string noPermissionsPage, string personalCenterLink, decimal lowestAmount, string tel, string qq, int isDisableKefu, string kefuUrl, string kefuImage, string kefuOnLineReply, string kefuOffLineReply, string isEnableCustomizeLoginPage, string loginConfigJson, string outletsSearchRange)
        {
            CompanyWebsite_Config model = new CompanyWebsite_Config();

            if (GetCompanyWebsiteConfig().AutoID == 0)//还没有配置
            {
                model.WebsiteTitle = websiteTitle;
                model.Copyright = copyright;
                model.WebsiteOwner = WebsiteOwner;
                model.WebsiteImage = websiteimg;
                model.WebsiteDescription = websitedescription;
                model.ShopNavGroupName = groupName;
                model.ShopAdType = shopAdType;
                model.BottomToolbars = buttomtoolbar;
                model.MemberStandard = Convert.ToInt32(memberStandard);
                model.HaveComment = Convert.ToInt32(haveComment);
                model.MemberStandardDescription = memberStandardDescription;
                model.MyCardCouponsTitle = myCardCouponsTitle;
                model.WeixinAccountNickName = weixinAccountNickName;
                model.DistributionQRCodeIcon = distributionQRCodeIcon;
                model.ArticleToolBarGrous = articleToolBarGrous;
                model.ActivityToolBarGrous = activityToolBarGrous;
                model.GroupBuyIndexUrl = groupBuyIndexUrl;
                model.NoPermissionsPage =int.Parse(noPermissionsPage);
                model.PersonalCenterLink = personalCenterLink;
                model.LowestAmount = lowestAmount;
                model.Tel = tel;
                model.QQ = qq;
                model.IsDisableKefu = isDisableKefu;
                model.KefuImage = kefuImage;
                model.KefuUrl = kefuUrl;
                model.KefuOnLineReply = kefuOnLineReply;
                model.KefuOffLineReply = kefuOffLineReply;
                model.IsEnableCustomizeLoginPage = Convert.ToInt32(isEnableCustomizeLoginPage);
                model.LoginConfigJson = loginConfigJson;
                model.OutletsSearchRange = outletsSearchRange;

                //model.StockType = stockType;
                //model.IsAutoAssignOrder = isAutoAssignOrder;
                //model.AutoAssignOrderRange = autoAssignOrderRange;
                //model.ShopCartAlongSettlement = shopCartAlongSettlement;
                //model.IsStoreSince = isStoreSince;
                //model.StoreSinceTimeJson = storeSinceTimeJson;
                //model.IsHomeDelivery = isHomeDelivery;
                //model.EarliestDeliveryTime = earliestDeliveryTime;
                //model.HomeDeliveryTimeJson = homeDeliveryTimeJson;

                return Add(model);

            }
            else
            {
                model = GetCompanyWebsiteConfig();
                model.WebsiteTitle = websiteTitle;
                model.Copyright = copyright;
                model.WebsiteImage = websiteimg;
                model.WebsiteDescription = websitedescription;
                model.ShopNavGroupName = groupName;
                model.ShopAdType = shopAdType;
                model.BottomToolbars = buttomtoolbar;
                model.MemberStandard = Convert.ToInt32(memberStandard);
                model.HaveComment = Convert.ToInt32(haveComment);
                model.MemberStandardDescription = memberStandardDescription;
                model.MyCardCouponsTitle = myCardCouponsTitle;
                model.WeixinAccountNickName = weixinAccountNickName;
                model.DistributionQRCodeIcon = distributionQRCodeIcon;
                model.ArticleToolBarGrous = articleToolBarGrous;
                model.ActivityToolBarGrous = activityToolBarGrous;
                model.GroupBuyIndexUrl = groupBuyIndexUrl;
                model.NoPermissionsPage = int.Parse(noPermissionsPage);
                model.PersonalCenterLink = personalCenterLink;
                model.LowestAmount = lowestAmount;
                model.Tel = tel;
                model.QQ = qq;
                model.IsDisableKefu = isDisableKefu;
                model.KefuImage = kefuImage;
                model.KefuUrl = kefuUrl;
                model.KefuOnLineReply = kefuOnLineReply;
                model.KefuOffLineReply = kefuOffLineReply;
                model.IsEnableCustomizeLoginPage = Convert.ToInt32(isEnableCustomizeLoginPage);
                model.LoginConfigJson = loginConfigJson;
                model.OutletsSearchRange = outletsSearchRange;
                //model.StockType = stockType;
                //model.IsAutoAssignOrder = isAutoAssignOrder;
                //model.AutoAssignOrderRange = autoAssignOrderRange;
                //model.ShopCartAlongSettlement = shopCartAlongSettlement;
                //model.IsStoreSince = isStoreSince;
                //model.StoreSinceTimeJson = storeSinceTimeJson;
                //model.IsHomeDelivery = isHomeDelivery;
                //model.EarliestDeliveryTime = earliestDeliveryTime;
                //model.HomeDeliveryTimeJson = homeDeliveryTimeJson;
                return Update(model);
            }


        }

        /// <summary>
        /// 获取单个幻灯片信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyWebsite_Projector GetCompanyWebsiteProjectorById(string id)
        {

            return Get<CompanyWebsite_Projector>(string.Format("AutoID={0}", id));
        }

        /// <summary>
        /// 获取单个 导航 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyWebsite_Navigate GetCompanyWebsiteNavigateById(string id)
        {
            return Get<CompanyWebsite_Navigate>(string.Format("AutoID={0}", id));
        }
        /// <summary>
        /// 获取单个企业微网站模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyWebsiteTemplate GetCompanyWebsiteTemplateById(string id)
        {
            return Get<CompanyWebsiteTemplate>(string.Format("AutoID={0}", id));
        }
        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="?"></param>
        /// <param name="keyWord"></param>
        /// <param name="version"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WebsiteInfo> GetWebsiteInfoList(int pageIndex,int pageSize,string keyWord,string version,string sort,out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder(" 1=1");
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (WebsiteName like '%{0}%' or WebsiteOwner like '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(version))
            {
                sbWhere.AppendFormat("AND WebsiteOwner In ( SELECT [UserID] FROM [ZCJ_UserPmsGroupRelationInfo] WHERE GroupID={0})", version);
            }
            totalCount = GetCount<WebsiteInfo>(sbWhere.ToString());
            return GetLit<WebsiteInfo>(pageSize,pageIndex,sbWhere.ToString(),sort);
        }


        /// <summary>
        /// 获取站点版本
        /// </summary>
        /// <returns></returns>
        public string GetWebsiteVersion(string websiteOwner)
        {

            List<long> listRelation = bllUserPmsGroupRelation.BaseCacheGetUserPmsGroupRelationList(websiteOwner)
                .Select(p => p.GroupID).ToList();
            if (listRelation.Count == 0)
            {
                return "未购买";
            }
            string groupIDs = ZentCloud.Common.MyStringHelper.ListToStr(listRelation, "", ",");

            List<ZentCloud.BLLPermission.Model.PermissionGroupInfo> list = bllPermission.GetGroupList(1, 1, null, null, groupIDs, 1);

            if (list.Count == 0)
            {
                return "未购买";
            }
            else
            {
                return list[0].GroupName;
            }
        }

        public WebsiteInfo GetWebsiteInfo(string websiteOwner)
        {
            return GetWebsiteInfoModelFromDataBase(websiteOwner);
            //WebsiteInfo website = Get<WebsiteInfo>(string.Format(" WebsiteOwner = '{0}' ", websiteOwner));
            //return website;
        }

        public WebsiteInfo GetWebsiteInfo()
        {
            WebsiteInfo website = GetWebsiteInfo(WebsiteOwner);
            if (website != null) HttpContext.Current.Session["WebsiteInfoModel"] = website;
            return website;
        }

        public string GetDistributionQRCodeIcon(string websiteOwner)
        {
            CompanyWebsite_Config config = GetColByKey<CompanyWebsite_Config>("WebsiteOwner", websiteOwner, "AutoID,DistributionQRCodeIcon");
            if (config == null) return "";
            return config.DistributionQRCodeIcon;
        }
        
        /// <summary>
        /// 获取站点微信用户注册方式：
        /// 0自动注册
        /// 1手动注册(任何操作前跳转注册页：参考复启做的注册方式)
        /// 2手动注册(不跳转注册页，在操作具体步骤的时候才跳转到手机号码绑定页)
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Enums.WebsiteWXUserRegType GetWebsiteWXUserRegType(string websiteOwner)
        {
            return (Enums.WebsiteWXUserRegType)new BLLCommRelation().GetIsNotAutoRegNewWxUser(websiteOwner);
        }


        public string GetWebsiteWXAppIdKey(string websiteOwner)
        {
            BLLUser bllUser = new BLLUser();

            string result = "";

            var websiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);
            var websiteOwnerInfo = bllUser.GetUserInfo(websiteInfo.WebsiteOwner, websiteOwner);
            if (!string.IsNullOrWhiteSpace(websiteInfo.AuthorizerAppId))
            {
                result += websiteInfo.AuthorizerAppId;
            }

            if (!string.IsNullOrWhiteSpace(websiteOwnerInfo.WeixinAppId))
            {
                result += ":" + websiteOwnerInfo.WeixinAppId;
            }

            return result;
        }

        #region 系统导航 添加

        /// <summary>
        /// 添加站点系统组件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public void AddSysToolBars(string websiteOwner, JObject sysJson)
        {
            foreach (JProperty item in sysJson.Properties())
            {
                if (sysJson[item.Name] == null) continue;
                AddSysToolBar(item.Name, websiteOwner, sysJson[item.Name]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentKey"></param>
        /// <param name="websiteOwner"></param>
        private void AddSysToolBar(string toolBarKey, string websiteOwner, JToken sysJson)
        {
            JArray nJArray = JArray.FromObject(sysJson);
            if (nJArray.Count == 0) return;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND KeyType='{0}' ", toolBarKey);
            CompanyWebsite_ToolBar oToolBar = Get<CompanyWebsite_ToolBar>(sbSql.ToString());
            if (oToolBar != null) return;
            int sort = 1;
            foreach (JToken item in nJArray)
            {
                CompanyWebsite_ToolBar nToolBar = new CompanyWebsite_ToolBar();
                if (item["ToolBarName"] != null) nToolBar.ToolBarName = item["ToolBarName"].ToString();
                if (item["ToolBarImage"] != null) nToolBar.ToolBarImage = item["ToolBarImage"].ToString();
                if (item["ToolBarType"] != null) nToolBar.ToolBarType = item["ToolBarType"].ToString();
                if (item["ToolBarTypeValue"] != null) nToolBar.ToolBarTypeValue = item["ToolBarTypeValue"].ToString();
                nToolBar.KeyType = toolBarKey;
                if (item["UseType"] != null) nToolBar.UseType = item["UseType"].ToString();
                if (item["ActBgColor"] != null) nToolBar.ActBgColor = item["ActBgColor"].ToString();
                if (item["BgColor"] != null) nToolBar.BgColor = item["BgColor"].ToString();
                if (item["ActColor"] != null) nToolBar.ActColor = item["ActColor"].ToString();
                if (item["Color"] != null) nToolBar.Color = item["Color"].ToString();
                if (item["PreID"] != null) nToolBar.PreID = Convert.ToInt32(item["PreID"]);
                if (item["IcoColor"] != null) nToolBar.IcoColor = item["IcoColor"].ToString();
                if (item["PermissionGroup"] != null) nToolBar.PermissionGroup = item["PermissionGroup"].ToString();
                nToolBar.WebsiteOwner = websiteOwner;
                nToolBar.PlayIndex = sort;
                nToolBar.IsShow = "1";
                Add(nToolBar);
                sort++;
            }
        }
        #endregion
    }
}
