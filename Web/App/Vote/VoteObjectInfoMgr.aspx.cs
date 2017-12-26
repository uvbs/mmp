using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class VoteObjectInfoMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public VoteInfo VoteInfo = new VoteInfo();
        public string DoMain = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            VoteInfo = bllVote.GetVoteInfo(int.Parse(Request["vid"]));
            DoMain = Request.Url.Host;
            if (!new BLLJIMP.BLLUser("").GetUserInfo(bllVote.WebsiteOwner).WeixinIsAdvancedAuthenticate.Equals(1))
            {
                DoMain = "comeoncloud.comeoncloud.net";
            }
        }
    }
}