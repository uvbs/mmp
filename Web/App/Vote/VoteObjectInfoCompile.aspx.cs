using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class VoteObjectInfoCompile : System.Web.UI.Page
    {
        public string aid = "0";
        public string webAction = "add";
        public string actionStr = "";
        BLLVote bll = new BLLVote();
        public VoteObjectInfo model = new VoteObjectInfo();
        public VoteInfo votemodel = new VoteInfo();
        public UserInfo ObjUserInfo = new UserInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bll.GetVoteObjectInfo(int.Parse(aid));
                if (model == null)
                {
                    Response.End();
                }
                else
                {

                }
            }
            votemodel = bll.GetVoteInfo(int.Parse(Request["vid"]));
            if (!string.IsNullOrEmpty(model.CreateUserId))
            {
                ObjUserInfo = new BLLUser().GetUserInfo(model.CreateUserId);

            }


        }

    }
}