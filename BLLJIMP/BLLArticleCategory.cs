using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 分类业务逻辑
    /// </summary>
    public class BLLArticleCategory : BLL
    {

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <param name="type">分类类型</param>
        /// <param name="preId">父ID</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public List<Model.ArticleCategory> GetCateList(out int totalCount, string type, int? preId, string websiteOwner, int pageSize = int.MaxValue, int pageIndex = 1,string keyword="")
        {
            List<Model.ArticleCategory> result = new List<Model.ArticleCategory>();

            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(type)) strWhere.AppendFormat(" AND CategoryType = '{0}'", type);

            if (preId.HasValue && preId.Value > 0)
            {
                var ids = GetCateAndChildIds(preId.Value);
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    strWhere.AppendFormat(" AND AutoID in ({0}) ", ids);
                }
                else
                {
                    strWhere.AppendFormat(" AND AutoID in (-1) ");
                }
            }
                
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat(" AND CategoryName = '{0}' ", keyword);
            }

            result = GetLit<Model.ArticleCategory>(pageSize, pageIndex, strWhere.ToString(), " Sort ASC ");

            totalCount = GetCount<Model.ArticleCategory>(strWhere.ToString());

            return result;
        }

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <param name="type">分类类型</param>
        /// <param name="preId">父ID</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public List<Model.ArticleCategory> GetCateChildList(out int totalCount, string type, string websiteOwner, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            List<Model.ArticleCategory> result = new List<Model.ArticleCategory>();

            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(type)) strWhere.AppendFormat(" AND CategoryType = '{0}'", type);
            strWhere.AppendFormat(" AND PreID != 0 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            }

            result = GetLit<Model.ArticleCategory>(pageSize, pageIndex, strWhere.ToString(), " Sort ASC ");

            totalCount = GetCount<Model.ArticleCategory>(strWhere.ToString());

            return result;
        }

        /// <summary>
        /// 获取分类和所有子分类id
        /// </summary>
        /// <param name="preId"></param>
        /// <returns></returns>
        public string GetCateAndChildIds(int preId)
        {
            string result = string.Empty;
            string sql = string.Format("with a as(select AutoID from ZCJ_ArticleCategory where AutoID={0} union all select x.AutoID from ZCJ_ArticleCategory x,a where x.PreID=a.AutoID) select * from a", preId);
            var list = ZentCloud.ZCBLLEngine.BLLBase.Query<ZentCloud.BLLJIMP.BLLWebSite.CategoryIDModel>(sql);
            if (list.Count > 0)
            {
                result = string.Join(",", list.SelectMany(p => new List<int>() { (int)p.AutoID }));
            }
            return result;
        }

        /// <summary>
        /// 提交类型
        /// </summary>
        /// <param name="cate"></param
        /// <returns></returns>
        public bool PutArticleCategory(Model.ArticleCategory cate)
        {
            if (cate.AutoID !=0 )
            {
                Model.ArticleCategory oldCate = GetArticleCategory(cate.AutoID);
                oldCate.CategoryName = cate.CategoryName;
                oldCate.Summary = cate.Summary;
                oldCate.ImgSrc = cate.ImgSrc;
                oldCate.CreateTime = cate.CreateTime;
                oldCate.Sort = cate.Sort;

                return Update(oldCate);
            }
            else
            {
                return Add(cate);
            }
        }

        public Model.ArticleCategory GetArticleCategory(int autoId)
        {
            return Get<Model.ArticleCategory>(string.Format(" AutoID={0} ", autoId));
        }

        public ArticleCategoryTypeConfig GetArticleCategoryTypeConfig(string websiteOwner,string categoryType)
        {
            List<string> websiteOwnerList = new List<string>() { "Common" };
            if (!string.IsNullOrWhiteSpace(websiteOwner)) websiteOwnerList.Add(websiteOwner);
            List<ArticleCategoryTypeConfig> list = GetArticleCategoryTypeConfigList(Common.MyStringHelper.ListToStr(websiteOwnerList, "'", ","), categoryType);
            ArticleCategoryTypeConfig temp = list.FirstOrDefault(p => !p.WebsiteOwner.Equals("Common"));
            if (temp != null) return temp;
            ArticleCategoryTypeConfig result = list.FirstOrDefault(p => p.WebsiteOwner.Equals("Common"));
            if (result != null) return result;
            return new ArticleCategoryTypeConfig() { CategoryType = categoryType };
        }

        public List<Model.ArticleCategoryTypeConfig> GetArticleCategoryTypeConfigList(string inWebsiteOwner, string categoryType)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("WebsiteOwner In ({0}) ", inWebsiteOwner);
            if (!string.IsNullOrWhiteSpace(categoryType))
            {
                strWhere.AppendFormat(" AND CategoryType='{0}' ", categoryType);
            }
            return GetList<Model.ArticleCategoryTypeConfig>(strWhere.ToString());
        }

        public ArticeCategoryTypeResponse GetTypeConfig(string websiteOwner, string categoryType)
        {
            ArticleCategoryTypeConfig nCategoryTypeConfig = GetArticleCategoryTypeConfig(websiteOwner, categoryType);

            CompanyWebsite_Config nWebsiteConfig = Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", websiteOwner));
            var apply_url = "/App/Member/Wap/PhoneVerify.aspx?referrer=" + System.Web.HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
            if (nWebsiteConfig != null && (nWebsiteConfig.MemberStandard == 2 || nWebsiteConfig.MemberStandard == 3))
            {
                apply_url = "/App/Member/Wap/CompleteUserInfo.aspx?referrer=" + System.Web.HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString());
            }
            UserInfo curUser = GetCurrentUserInfo();

            return new ArticeCategoryTypeResponse
            {
                time_set_method = nCategoryTypeConfig.TimeSetMethod,
                time_set_style = nCategoryTypeConfig.TimeSetStyle,
                spend_method = nCategoryTypeConfig.SpendMethod,
                title = nCategoryTypeConfig.CategoryTypeTitle,
                home_title = nCategoryTypeConfig.CategoryTypeHomeTitle,
                order_list_title = nCategoryTypeConfig.CategoryTypeOrderListTitle,
                order_detail_title = nCategoryTypeConfig.CategoryTypeOrderDetailTitle,
                category_type = nCategoryTypeConfig.CategoryType,
                category_name = nCategoryTypeConfig.CategoryTypeDispalyName,
                stock_name = nCategoryTypeConfig.CategoryTypeStockName,
                slide_width = nCategoryTypeConfig.SlideWidth,
                slide_height = nCategoryTypeConfig.SlideHeight,
                is_login = curUser != null ? true : false,
                is_member = IsMember(),
                truename = curUser != null ? curUser.TrueName : "",
                phone = curUser != null ? curUser.Phone : "",
                access_level = curUser != null ? curUser.AccessLevel : 0,
                apply_url = apply_url,
                nopms_url = "/Error/NoPmsMobile.htm",
                share_title = nCategoryTypeConfig.ShareTitle,
                share_img = nCategoryTypeConfig.ShareImg,
                share_desc = nCategoryTypeConfig.ShareDesc,
                share_link = nCategoryTypeConfig.ShareLink
            };
        }
    }
}
