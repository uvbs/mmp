using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.SuperTeam
{
    public partial class Detail : SuperTeamBase
    {
        /// <summary>
        /// 投票业务逻辑基类
        /// </summary>
        ZentCloud.BLLJIMP.BLLVote bllVote = new ZentCloud.BLLJIMP.BLLVote();
        /// <summary>
        /// 选手信息
        /// </summary>
        public ZentCloud.BLLJIMP.Model.VoteObjectInfo model;
        protected void Page_Load(object sender, EventArgs e)
        {

            model = bllVote.GetVoteObjectInfo(int.Parse(Request["id"]));
            if (model == null)
            {
                Response.End();
            }
            if (model.Status != 1)
            {
                Response.Write("审核未通过");
                Response.End();
            }
        }
    }
}