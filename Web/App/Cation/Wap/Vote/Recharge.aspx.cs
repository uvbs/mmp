using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote
{
    public partial class Recharge : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public VoteInfo VoteInfo;
        /// <summary>
        /// 剩余票数
        /// </summary>
        public int CanUseVoteCount;
        public List<VoteRecharge> RechargeList;
        /// <summary>
        ///当前用户信息
        /// </summary>
        public UserInfo UserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserInfo = DataLoadTool.GetCurrUserModel();
                VoteInfo = bllVote.GetVoteInfo(int.Parse(Request["vid"]));
                CanUseVoteCount = bllVote.GetCanUseVoteCount(VoteInfo.AutoID, DataLoadTool.GetCurrUserID());
                RechargeList = bllVote.GetVoteRechargeList(VoteInfo.AutoID);

            }
            catch (Exception)
            {

                Response.End();
                
            }

        }
    }
}