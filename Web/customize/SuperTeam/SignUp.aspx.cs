using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.SuperTeam
{
    public partial class SignUp : SuperTeamBase
    {
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        protected void Page_Load(object sender, EventArgs e)
        {
            int voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SuperTeamVoteId"]);
            var voteObjInfo=bllVote.GetVoteObjectInfo(voteId, CurrentUserInfo.UserID);
            if (voteObjInfo != null)
            {
                Response.Redirect("MySignUp.aspx");
            }

        }
    }
}