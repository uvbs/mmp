using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.Iluxday
{
    public partial class SignUp : IluxdayBase
    {
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        protected void Page_Load(object sender, EventArgs e)
        {
            int voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IluxdayVoteId"]);
            var voteObjInfo=bllVote.GetVoteObjectInfo(voteId, CurrentUserInfo.UserID);
            if (voteObjInfo != null)
            {
                //
                //Response.Redirect(string.Format("Detail.aspx?id={0}", voteObjInfo.AutoID));
                Response.Redirect("MySignUp.aspx");
            }

        }
    }
}