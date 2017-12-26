using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.customize.jikuwifi
{
    public partial class Index : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();



        protected void Page_Load(object sender, EventArgs e)
        {
            Tolog("进入jikuwifi页面");

            //try
            //{
            //    var jikuReadHistory = Session["jikuReadHistory"];
            //    if (jikuReadHistory == null)
            //    {
            //        Session["jikuReadHistory"] = "jikuReadHistory";
            //        Response.Redirect(Request.Url.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Tolog( "重跳一次异常" + ex.Message);
            //}

            Tolog(" currLoginStatus: " + bll.IsLogin);

            Tolog(" currUserInfo: " +  JsonConvert.SerializeObject(bll.GetCurrentUserInfo()));

        }


        public void Tolog(string msg)
        {
            bll.ToLog(msg, "D:\\wifilog.txt");
        }

    }
}