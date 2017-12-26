using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class OrderCode : System.Web.UI.Page
    {
        /// <summary>
        /// 二维码
        /// </summary>
        public System.Text.StringBuilder QCode = new System.Text.StringBuilder();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (bllMall.IsLogin)
            {
                UserInfo userInfo = DataLoadTool.GetCurrUserModel();
                string orderid=Request["oid"];
                if (string.IsNullOrEmpty(orderid))
                {
                    Response.End();
                }
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderid);
                if (orderInfo==null)
                {
                    Response.End();
                }
                if (!orderInfo.OrderUserID.Equals(userInfo.UserID))
                {
                    Response.End();
                }
                QCode.Append(string.Format("订单编号:{0}; ",orderInfo.OrderID));
                //QCode.AppendFormat("商品数量:{0}", orderInfo.ProductCount);
                //QCode.AppendFormat("总金额:{0}", orderInfo.TotalAmount);
                //QCode.AppendFormat("收货人:{0}", orderInfo.Consignee);
                //QCode.AppendFormat("手机号:{0}", orderInfo.Phone);
                //QCode.AppendFormat("手机号:{0}", orderInfo.Phone);
                List<WXMallOrderDetailsInfo> orderdetails = bllMall.GetOrderDetailsList(orderid);
                foreach (var item in orderdetails)
                {
                    WXMallProductInfo productInfo = bllMall.GetProduct(item.PID);
                    if (productInfo!=null)
                    {
                          QCode.AppendFormat("[{0} X {1}];",productInfo.PName,item.TotalCount);
                    }
                    
                }


            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.Url.PathAndQuery));
            }


        }

    }
}