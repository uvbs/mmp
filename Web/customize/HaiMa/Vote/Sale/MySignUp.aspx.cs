using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale
{
    public partial class MySignUp : HaiMaBase
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 选手信息
        /// </summary>
        public VoteObjectInfo model = new VoteObjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllVote.GetVoteObjectInfo(int.Parse(System.Configuration.ConfigurationManager.AppSettings["HaiMaVoteIDSale"]), CurrentUserInfo.UserID);
            if (model == null)
            {
                Response.Redirect("nosignup.htm");
            }
            else
            {

            }
        }
    }
}