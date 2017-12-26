using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class VoteLogInfoMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public System.Text.StringBuilder sbVoteList = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo userInfo = DataLoadTool.GetCurrUserModel();
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("WebsiteOwner='{0}'", bllVote.WebsiteOwner));
            if ((!userInfo.UserID.Equals(bllVote.WebsiteOwner)) && (!userInfo.UserType.Equals(1)))
            {
                sbWhere.AppendFormat(" And CreateUserID='{0}'", userInfo.UserID);
            }
            foreach (var item in bllVote.GetLit<VoteInfo>(100, 1, sbWhere.ToString(), "AutoID DESC"))
            {
                sbVoteList.AppendLine(string.Format("<option value=\"{0}\">{1}</option>", item.AutoID, item.VoteName));

            }
        }
    }
}