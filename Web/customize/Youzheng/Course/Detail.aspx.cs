using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.Youzheng.Course
{
    public partial class Detail : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 通用关系BLL
        /// </summary>
        BLLCommRelation bllCommRela = new BLLCommRelation();
        /// <summary>
        /// 
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 商品实体
        /// </summary>
        public WXMallProductInfo productInfo = new WXMallProductInfo();
        /// <summary>
        /// 证书列表
        /// </summary>
        public List<ProductSku> skuList = new List<ProductSku>();
        /// <summary>
        /// Tab宽度百分比
        /// </summary>
        public double tabWidth = 100;
        /// <summary>
        /// tab 数量
        /// </summary>
        public int tabCount = 0;
        /// <summary>
        ///  推荐课程
        /// </summary>
        public List<WXMallProductInfo> recommendProductList = new List<WXMallProductInfo>();
        /// <summary>
        /// 全局配置
        /// </summary>
        public CompanyWebsite_Config config = new CompanyWebsite_Config();
        /// <summary>
        /// 逻辑层 BLLWebSite
        /// </summary>
        BLLWebSite bllWebsite = new BLLWebSite();
        /// <summary>
        /// 是否已经收藏
        /// </summary>
        public int IsCollect = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string productId = Request["id"];
            productInfo = bllMall.GetProduct(productId);

            config = bllWebsite.GetCompanyWebsiteConfig();

            #region 分类名称
            if (!string.IsNullOrEmpty(productInfo.CategoryId))
            {
                var categoryInfo = bllMall.Get<BLLJIMP.Model.WXMallCategory>(string.Format("AutoId={0}", productInfo.CategoryId));
                if (categoryInfo != null)
                {
                    productInfo.CategoryName = categoryInfo.CategoryName;
                }

            }
            #endregion

            #region tab宽度
           
            if (!string.IsNullOrEmpty(productInfo.TabExTitle1))
            {
                tabCount += 1;

            }
            if (!string.IsNullOrEmpty(productInfo.TabExTitle2))
            {
                tabCount += 1;

            }
            if (!string.IsNullOrEmpty(productInfo.TabExTitle3))
            {
                tabCount += 1;

            }
            if (!string.IsNullOrEmpty(productInfo.TabExTitle4))
            {
                tabCount += 1;

            }
            if (!string.IsNullOrEmpty(productInfo.TabExTitle5))
            {
                tabCount += 1;

            }
            if (tabCount > 0)
            {
                tabWidth = 100 / tabCount;
            }
            #endregion

            #region 证书列表
            skuList = bllMall.GetProductSkuList(int.Parse(productInfo.PID));
            foreach (var item in skuList)
            {
                if (!string.IsNullOrEmpty(item.ShowProps))
                {
                    if (item.ShowProps.Split(':').Length == 2)
                    {
                        item.ShowProps = item.ShowProps.Split(':')[1];

                    }

                }


            }
            #endregion

            #region 推荐教材
            if (!string.IsNullOrEmpty(productInfo.RelationProductId))
            {
                foreach (var pId in productInfo.RelationProductId.Split(','))
                {
                    if (!string.IsNullOrEmpty(pId))
                    {
                        WXMallProductInfo product = bllMall.GetProduct(int.Parse(pId));

                        if (product != null)
                        {
                            if (product.PName.Length>=10)
                            {
                                product.PName = product.PName.Substring(0,10)+"...";
                            }

                            recommendProductList.Add(product);
                        }


                    }


                }


            } 
            #endregion


            #region 是否已经收藏s
            if (bllCommRela.ExistRelation(BLLJIMP.Enums.CommRelationType.ProductCollect, bllUser.GetCurrUserID(), productInfo.PID))
            {
                IsCollect = 1;
            } 
            #endregion

        }
    }
}