using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class TestSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string key = TextBox1.Text.Trim();
            string val = TextBox2.Text.Trim();
            if (key != "" && val != "")
            {
                Session[key] = val;
            }
        }
    }
}