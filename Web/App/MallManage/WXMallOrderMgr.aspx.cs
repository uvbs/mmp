using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallOrderMgr : System.Web.UI.Page
    {
       /// <summary>
       /// BLL
       /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 门店列表
        /// </summary>
        //public System.Text.StringBuilder sbStores=new System.Text.StringBuilder();
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public System.Text.StringBuilder sbOrderStatuList = new System.Text.StringBuilder();
        /// <summary>
        /// 商品分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        /// <summary>
        /// 是否是分销商城
        /// </summary>
        public bool IsDistributionMall;
        /// <summary>
        /// 订单状态列表
        /// </summary>
        public List<WXMallOrderStatusInfo> OrderStatusList;
        protected void Page_Load(object sender, EventArgs e)
        {
            //foreach (var item in bll.GetWXMallStoreListByWebSite())
            //{
            //    sbStores.AppendFormat("<option  value=\"{0}\" >{1}</option>",item.AutoID,item.StoreName);
                
            //}
            OrderStatusList=bllMall.GetOrderStatuList();
            foreach (var item in OrderStatusList)
            {
                sbOrderStatuList.AppendFormat("<option  value=\"{0}\" >{1}</option>", item.OrderStatu, item.OrderStatu);

            }
            foreach (var item in bllMall.GetCategoryList())
            {
                sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            }
            if (bllMall.GetWebsiteInfoModel().IsDistributionMall==1)
            {
                IsDistributionMall = true;
            }



        }
    }
}