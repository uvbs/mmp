using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class OrderMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public System.Text.StringBuilder sbVoteList = new System.Text.StringBuilder();
        public System.Text.StringBuilder sbSubAccountList = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            foreach (var item in bllVote.GetLit<VoteInfo>(100, 1, string.Format("WebsiteOwner='{0}'", bllVote.WebsiteOwner), " AutoID DESC"))
            {
                sbVoteList.AppendLine(string.Format("<option value=\"{0}\">{1}</option>",item.AutoID,item.VoteName));

            }
            foreach (var item in bllVote.GetList<UserInfo>(string.Format("IsSubAccount='1' And WebsiteOwner='{0}'", bllVote.WebsiteOwner)))
            {
                sbSubAccountList.AppendLine(string.Format("<option value=\"{0}\">{1}</option>", item.UserID, item.UserID));

            }
            

        }
    }
}