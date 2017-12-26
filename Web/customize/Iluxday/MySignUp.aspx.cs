using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.Iluxday
{
    public partial class MySignUp : IluxdayBase
    {
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 选手信息
        /// </summary>
        public BLLJIMP.Model.VoteObjectInfo model = new BLLJIMP.Model.VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            int voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IluxdayVoteId"]);
             model = bllVote.GetVoteObjectInfo(voteId, CurrentUserInfo.UserID);
            if (model == null)
            {
                Response.Redirect("SignUp.aspx");
               
            }
            

        }
    }
}