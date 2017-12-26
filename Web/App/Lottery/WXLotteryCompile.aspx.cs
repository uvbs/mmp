using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class WXLotteryCompile : System.Web.UI.Page
    {
        public int id = 0;
        public string webAction = "add";
        public string actionStr = "";
        BLL bll = new BLL("");
        public WXLottery model = new WXLottery();
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Convert.ToInt32(Request["id"]);
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bll.Get< WXLottery>(string.Format("AutoID='{0}'",id));

                if (model == null)
                {
                    Response.End();
                }
                else
                {

                }
            }

          




        }

    }
}