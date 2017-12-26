using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BeachHoney
{
    public partial class MyPrize :BeachHoneyBase
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 选手信息
        /// </summary>
        public VoteObjectInfo model = new VoteObjectInfo();
        public bool IsGetPrize = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllVote.GetVoteObjectInfo(bllVote.BeachHoneyVoteID, CurrentUserInfo.UserID);
            if (model == null)
            {
                Response.Write("您还没有报名，请先报名");
                Response.End();
            }
            else if (!model.Status.Equals(1))
            {
                Response.Write("您还未通过审核");
                Response.End();
            }
            else if(model.Rank>1000)
            {
                Response.Write("您没有获奖");
                Response.End();
            }
            int TotalCount = 0;
            bllCardCoupon.GetMyCardCoupons(BLLJIMP.Enums.EnumCardCouponType.EntranceTicket, CurrentUserInfo.UserID, 1, 1, out TotalCount);

            if (TotalCount > 0)
            {
                IsGetPrize = true;
            }


        }
    }
}