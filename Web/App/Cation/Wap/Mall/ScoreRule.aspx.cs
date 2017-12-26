using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ScoreRule : System.Web.UI.Page
    {
        public ZentCloud.BLLJIMP.Model.ScoreConfig model = new BLLJIMP.Model.ScoreConfig();
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllScore.GetScoreConfig();

        }

    }
}