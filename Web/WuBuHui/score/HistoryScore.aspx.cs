using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.score
{
    public partial class HistoryScore : System.Web.UI.Page
    {
        public UserInfo uinfo;
        public int UserLevel = 1;
        BLLJIMP.BLLUserScore bllUserScore;
        public UserLevelConfig NextUserLevel;
        public int Percent = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            uinfo = DataLoadTool.GetCurrUserModel();
            bllUserScore = new BLLJIMP.BLLUserScore(uinfo.UserID);
            UserLevel = bllUserScore.GetUserLevelByTotalScore(uinfo.HistoryTotalScore);

            NextUserLevel = bllUserScore.Get<UserLevelConfig>(string.Format("LevelNumber={0}", UserLevel+1));

            if (NextUserLevel==null)
            {
                NextUserLevel = new UserLevelConfig();
                NextUserLevel.LevelNumber = 1;
                NextUserLevel.FromHistoryScore = 0;
            }
            Percent =(int)((uinfo.HistoryTotalScore/NextUserLevel.FromHistoryScore)*100);
            if (Percent<0)
            {
                Percent = 0;
            }
            if (Percent>100)
            {
                Percent = 100;
            }


        }
    }
}