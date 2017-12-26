using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.test.efast
{
    public partial class GetShippingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Open.EfastSDK.Client client = new Open.EfastSDK.Client();

            //GridView1.DataSource = client.GetShippingList();

            //this.DataBind();
            
            Response.Write(Request.Url);
            //Response.Write("<br>");
            Response.Write(Request["aaa"]);
        }
    }
}