using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Stock
{
    public partial class StockList : System.Web.UI.Page
    {
        protected string sendNoticePrice;
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
            sendNoticePrice = bllKeyValueData.GetDataVaule("SendNoticePrice", "1", bllKeyValueData.WebsiteOwner);
            if (string.IsNullOrWhiteSpace(sendNoticePrice)) sendNoticePrice = "-1";
        }
    }
}