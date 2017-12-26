using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    public partial class MySignUp : VoteCommBase
    {
        /// <summary>
        /// 选手信息
        /// </summary>
        public VoteObjectInfo model = new VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {

            model = bllVote.GetVoteObjectInfo(currVote.AutoID, currentUserInfo.UserID);
            if (model == null)
            {
                Response.Redirect("SignUp.aspx?vid=" + currVote.AutoID);
                Response.Write("您还没有报名，请先报名");
                Response.End();
            }
            else
            {
                
            }
        }
    }
}