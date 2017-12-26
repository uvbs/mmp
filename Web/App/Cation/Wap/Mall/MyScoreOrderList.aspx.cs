using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyScoreOrderList : System.Web.UI.Page
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前用户所有订单
        /// </summary>
        public List<WXMallScoreOrderInfo> orderList;
        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (bll.IsLogin)
            {
                orderList = bll.GetList<WXMallScoreOrderInfo>(string.Format("OrderUserID='{0}' And WebsiteOwner='{1}' And IsDelete=0 Order By InsertDate DESC", bll.GetCurrUserID(), bll.WebsiteOwner));
            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }
        }

    }
}