using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    public partial class SignUp : VoteCommBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断是否已经报名，报名则跳到我的报名页

            var model = bllVote.GetVoteObjectInfo(currVote.AutoID,currentUserInfo.UserID);
            if (model != null)
            {
                Response.Redirect("MySignUp.aspx?vid=" + currVote.AutoID);
            }

        }
    }
}