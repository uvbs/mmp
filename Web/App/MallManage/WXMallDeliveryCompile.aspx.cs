using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallDeliveryCompile : System.Web.UI.Page
    {
        public int autoid = 0;
        public string webAction = "add";
        BLLMall bll = new BLLMall();
        public WXMallDelivery model = new WXMallDelivery();
        public string HeadTitle = "添加配送方式";
        protected void Page_Load(object sender, EventArgs e)
        {

            webAction = Request["Action"];

            if (webAction == "edit")
            {
                autoid = Convert.ToInt32(Request["id"]);
                model = bll.GetDelivery(autoid);
                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    HeadTitle = string.Format("{0}", model.DeliveryName);

                }
            }



        }
    }
}