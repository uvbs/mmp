using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.score
{
    public partial class Score : UserPage
    {
        public UserInfo uinfo;
        public int UserLevel = 1;
        BLLJIMP.BLLUserScore bllUserScore;

        protected void Page_Load(object sender, EventArgs e)
        {
            uinfo = DataLoadTool.GetCurrUserModel();
           // txtScore.Text = uinfo.TotalScore.ToString();
            bllUserScore = new BLLJIMP.BLLUserScore(uinfo.UserID);
            UserLevel = bllUserScore.GetUserLevelByTotalScore(uinfo.HistoryTotalScore);
           
        }
    }
}