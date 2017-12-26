using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class VotePKModeMgr : System.Web.UI.Page
    {
        /// <summary>
        /// BLL 投票
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 投票
        /// </summary>
        public VoteInfo VoteInfo = new VoteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["vid"]))
            {
                VoteInfo = bllVote.GetVoteInfo(int.Parse(Request["vid"]));
            }
            
        }
    }
}