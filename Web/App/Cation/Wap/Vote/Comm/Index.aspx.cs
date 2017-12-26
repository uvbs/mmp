using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    public partial class Index : VoteCommBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = currVote.VoteName;
        }
    }
}