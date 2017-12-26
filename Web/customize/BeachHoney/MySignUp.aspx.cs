using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BeachHoney
{
    public partial class MySignUp : BeachHoneyBase
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 选手信息
        /// </summary>
        public VoteObjectInfo model = new VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {

            model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
            if (model == null)
            {
                Response.Write("您还没有报名，请先报名");
                Response.End();
            }
            else
            {
                
            }
        }
    }
}