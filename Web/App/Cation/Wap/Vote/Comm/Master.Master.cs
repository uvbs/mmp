using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    public partial class Master : System.Web.UI.MasterPage
    {

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();

        public VoteInfo currVote = new VoteInfo();
        public UserInfo currentUserInfo = new UserInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUserInfo = bllUser.GetCurrentUserInfo();
            currVote = bllVote.GetVoteInfo(Convert.ToInt32(Request["vid"]));

        }

    }
}