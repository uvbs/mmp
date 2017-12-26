using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using System.Data;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// ExportProduct 导出商品
    /// </summary>
    public class ExportProduct : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        public void ProcessRequest(HttpContext context)
        {
            string keyWord = context.Request["keyword"];//关键字
            string categoryId = context.Request["category_id"];//分类id
            string sort = context.Request["sort"];//排序号
            string isAppointment = context.Request["is_appointment"];//是否是预购商品
            string isGroupBuyProduct = context.Request["is_group_buy"];//是否参与团购
            string iSthreshold = context.Request["is_threshold"];//库存紧急商品
            string isOnSale = context.Request["is_onsale"];//上下架
            string isPromotionProduct = context.Request["is_promotion_product"];//是否特卖商品


            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' AND IsDelete=0 ", bllMall.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (PName like '%{0}%' Or ProductCode like '%{0}%' or Tags like '%{0}%' Or PID='{0}')", keyWord);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                sbWhere.AppendFormat(" And CategoryId='{0}'", categoryId);
            }
            if (!string.IsNullOrEmpty(isOnSale))
            {
                sbWhere.AppendFormat(" And IsOnSale={0} ", isOnSale);
            }
            else
            {
                sbWhere.AppendFormat(" And IsOnSale=1 ");
            }
            if (!string.IsNullOrEmpty(isAppointment))
            {
                sbWhere.AppendFormat(" And IsAppointment={0}", isAppointment);
            }
            if (!string.IsNullOrEmpty(isPromotionProduct))
            {
                sbWhere.AppendFormat(" And IsPromotionProduct={0} ", isPromotionProduct);
            }

            if (!string.IsNullOrEmpty(isGroupBuyProduct) && isGroupBuyProduct=="1")
            {
                sbWhere.AppendFormat(" And GroupBuyRuleIds <>'' ");
                //是团购商品的时候，过滤掉可以用积分购买的商品
                sbWhere.AppendFormat(" And ( Score = 0 or Score is null ) ");
            }

            string orderBy = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                sbWhere.AppendFormat(GetSortList(sort));
            }

            List<WXMallProductInfo> productList = bllMall.GetList<WXMallProductInfo>(sbWhere.ToString());
            List<ProductModel> pList = new List<ProductModel>();
            WebsiteInfo website = bllMall.GetWebsiteInfoModelFromDataBase();
            var list = from p in productList
                       select new
                       {
                           product_id = p.PID,
                           product_code = p.ProductCode,
                           category_name = bllMall.GetWXMallCategoryName(p.CategoryId),
                           product_title = p.PName,
                           base_price=p.BasePrice,
                           quote_price=p.PreviousPrice,
                           sale_count = p.SaleCount,
                           totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                           supplier_name =bllMall.GetSuppLierByUserId(p.SupplierUserId, p.WebsiteOwner)!=null?bllMall.GetSuppLierByUserId(p.SupplierUserId, p.WebsiteOwner).Company:"",
                           freight_template =p.FreightTemplateId <= 0 ? "包邮" : bllMall.GetFreightTemplate(p.FreightTemplateId).TemplateName
                       };

            if (!string.IsNullOrEmpty(iSthreshold))
            {
                list = list.Where(p => p.totalcount <= 0).ToList();
            }
            foreach (var p in list)
            {
                foreach (var item in bllMall.GetProductSkuList(int.Parse(p.product_id)))
                {
                    ProductModel prodduct = new ProductModel();
                    prodduct.PID = item.ProductId;
                    prodduct.PName = p.product_title;
                    prodduct.TotalCount = p.totalcount;
                    prodduct.ProductCode = p.product_code;
                    prodduct.CategoryName = p.category_name;
                    prodduct.SupplierName = p.supplier_name;
                    prodduct.ProductStock = item.Stock;
                    prodduct.Price = item.Price;
                    prodduct.FreightTemplateName = p.freight_template;
                    prodduct.ShowProps = item.ShowProps;
                    prodduct.BasePrice = item.BasePrice;
                    prodduct.pBasePrice = p.base_price;
                    prodduct.QuotePrice = p.quote_price;
                    pList.Add(prodduct);
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("商品ID");
            dt.Columns.Add("商品名称");
            dt.Columns.Add("商品编码");
            dt.Columns.Add("商品规格");
            dt.Columns.Add("总库存");
            dt.Columns.Add("基础价");
            dt.Columns.Add("售价");
            dt.Columns.Add("原价");
            dt.Columns.Add("库存");
            dt.Columns.Add("物流");
            dt.Columns.Add("商户");

            for (int i = 0; i < pList.Count; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["商品ID"] = pList[i].PID;
                newRow["商品名称"] = pList[i].PName;
                newRow["商品编码"] = pList[i].ProductCode;
                newRow["商品规格"] = pList[i].ShowProps;
                newRow["总库存"] = pList[i].TotalCount;
                newRow["基础价"] = pList[i].BasePrice;
                if (pList[i].BasePrice == 0) newRow["基础价"] = pList[i].pBasePrice;
                newRow["售价"] = pList[i].Price;
                newRow["原价"] = pList[i].QuotePrice;
                newRow["库存"] = pList[i].ProductStock;
                newRow["物流"] = pList[i].FreightTemplateName;
                newRow["商户"] = pList[i].SupplierName;
                dt.Rows.Add(newRow);
            }
            dt.TableName = "商品列表";
            DataTable[] dt2 = { dt };
            DataLoadTool.ExportDataTable(dt2, string.Format("{0}_data.xls", DateTime.Now.ToString()));
        }

        public class ProductModel
        {
            public int PID { get;set;}
            public string PName { get; set; }
            public string ProductCode { get; set; }
            public string CategoryName { get; set; }
            public string SupplierName { get; set; }
            public int ProductStock { get; set; }
            public decimal Price { get; set; }
            public int TotalCount { get; set; }
            public string FreightTemplateName { get; set; }
            public string ShowProps { get; set; }
            public decimal BasePrice { get; set; }
            public decimal pBasePrice { get; set; }
            /// <summary>
            /// 原价
            /// </summary>
            public decimal QuotePrice { get; set; }
        }



        private string GetSortList(string sort)
        {
            string orderBy = string.Empty;
            switch (sort)
            {
                case "price_asc":
                    orderBy = " Price ASC,Sort DESC,InsertDate DESC";
                    break;
                case "price_desc":
                    orderBy = " Price DESC,Sort DESC,InsertDate DESC";
                    break;
                case "pv":
                    orderBy = " PV DESC,Sort DESC,InsertDate DESC";
                    break;
                case "pv_asc":
                    orderBy = " PV ASC,Sort DESC,InsertDate DESC";
                    break;
                case "time_asc":
                    orderBy = " InsertDate ASC,Sort DESC";
                    break;
                case "time_desc":
                    orderBy = " InsertDate DESC,Sort DESC";
                    break;
                case "sales_volume":
                    orderBy = " SaleCount DESC,Sort DESC,InsertDate DESC";
                    break;
                case "sales_asc":
                    orderBy = " SaleCount ASC,Sort DESC,InsertDate DESC";
                    break;
                case "sales_volume_onemonth":
                    orderBy = " SaleCountOneMonth DESC,Sort DESC,InsertDate DESC";
                    break;
                case "sales_volume_threemonth":
                    orderBy = " SaleCountThreeMonth DESC,Sort DESC,InsertDate DESC";
                    break;
                case "sales_volume_halfyear":
                    orderBy = " SaleCountHalfYear DESC,Sort DESC,InsertDate DESC";
                    break;
                case "sales_volume_oneyear":
                    orderBy = " SaleCountOneYear DESC,Sort DESC,InsertDate DESC";
                    break;
                case "uv_desc":
                    orderBy = " UV DESC,Sort DESC,InsertDate DESC";
                    break;
                case "uv_asc":
                    orderBy = " UV ASC,Sort DESC,InsertDate DESC";
                    break;
                default:
                    break;
            }
            return orderBy;
        }
    }
}