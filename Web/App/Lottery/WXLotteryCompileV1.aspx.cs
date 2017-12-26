using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class WXLotteryCompileV1 : System.Web.UI.Page
    {
        //public int id = 0;
        //public string webAction = "add";
        //public string actionStr = "";
        //BLL bll = new BLL("");
        //public WXLotteryV1 model = new WXLotteryV1(); 
        public string backUrl = "WXLotteryMgrV1.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {

            //webAction = Request["Action"];
            //actionStr = webAction == "add" ? "添加" : "编辑";
            //if (webAction == "edit")
            //{
            //    id = Convert.ToInt32(Request["id"]);
            //    model = this.bll.Get<WXLotteryV1>(string.Format("AutoID='{0}'", id));
            //    if (model == null)
            //    {
            //        Response.End();
            //    }
            //    else
            //    {

            //    }
            //}
            string reqBackUrl = Request["BackUrl"];
            if (!string.IsNullOrWhiteSpace(reqBackUrl))
            {
                backUrl = HttpUtility.UrlDecode(reqBackUrl);
            }





        }

    }
}