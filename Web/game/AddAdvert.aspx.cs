using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.game
{
    public partial class AddAdvert : System.Web.UI.Page
    {
        public string GameId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GameId=Request["gid"];
        }
    }
}