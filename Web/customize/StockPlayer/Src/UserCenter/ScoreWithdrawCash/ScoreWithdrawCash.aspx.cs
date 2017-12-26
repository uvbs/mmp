using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreWithdrawCash
{
    public partial class ScoreWithdrawCash : System.Web.UI.Page
    {
        protected string Recharge;
        protected string MinScore;
        protected string MinWithdrawCashScore;
        protected BLLJIMP.Model.UserInfo curUser = new BLLJIMP.Model.UserInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
            BLLUser bllUser = new BLLUser();
            curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/customize/StockPlayer/SrcUserCenter/UserCenter.aspx", true);
                return;
            }
            string websiteOwner = bllKeyValueData.WebsiteOwner;
            Recharge = bllKeyValueData.GetDataVaule("Recharge", "100", websiteOwner);
            MinScore = bllKeyValueData.GetDataVaule("MinScore", "1", websiteOwner);
            MinWithdrawCashScore = bllKeyValueData.GetDataVaule("MinWithdrawCashScore", "1", websiteOwner);
        }
    }
}