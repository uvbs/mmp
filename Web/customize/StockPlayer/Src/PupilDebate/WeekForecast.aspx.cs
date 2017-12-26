using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.PupilDebate
{
    public partial class WeekForecast : System.Web.UI.Page
    {
        protected string rootId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            rootId = Request["rootid"];
        }
    }
}