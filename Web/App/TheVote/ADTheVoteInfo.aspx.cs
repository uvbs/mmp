using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.TheVote
{
    public partial class ADTheVoteInfo : System.Web.UI.Page
    {
        public string Tag = "添加";
        public string AutoId = "0";
        public string currAction = "add";
        public BLLJIMP.Model.TheVoteInfo model;
        public BLLJIMP.BLLJuActivity jubll;

        public BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["id"];
                jubll = new BLLJIMP.BLLJuActivity("");
                if (!string.IsNullOrEmpty(AutoId))
                {
                    Tag = "编辑";
                    currAction = "Edit";
                }
            }
        }
    }
}