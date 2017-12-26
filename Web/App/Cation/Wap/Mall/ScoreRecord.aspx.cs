using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ScoreRecord : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 总收入
        /// </summary>
        public double ScoreTotalIn = 0;
        /// <summary>
        /// 总支出
        /// </summary>
        public double ScoreTotalOut= 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllMall.IsLogin)
            {
                ScoreTotalIn=bllMall.GetScoreRecordTotalInOut(DataLoadTool.GetCurrUserID(),1);
                ScoreTotalOut = bllMall.GetScoreRecordTotalInOut(DataLoadTool.GetCurrUserID(), 0);
            }
            else
            {
                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));

            }
        }
    }
}