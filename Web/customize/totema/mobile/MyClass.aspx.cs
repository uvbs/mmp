using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.totema.mobile
{
    public partial class MyClass : TotemaBase
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public VoteObjectInfo model = new VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {

            model = bllVote.GetVoteObjectInfo(bllVote.TotemaVoteID, currentUserInfo.UserID);
            if (model==null)
            {
                Response.Write("您还没有报名，请先报名");
                Response.End();
            }
        }
    }
}