using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.totema
{
    public partial class ClassDetail : System.Web.UI.Page
    {
        public string aid = "0";
        BLLVote bll = new BLLVote();
        public VoteObjectInfo model = new VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            model = this.bll.GetVoteObjectInfo(int.Parse(aid));
            if (model == null)
            {
                Response.End();
            }

        }
    }
}