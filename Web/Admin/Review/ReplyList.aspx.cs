using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Admin.Review
{
    public partial class ReplyList : System.Web.UI.Page
    {
        /// <summary>
        /// 话题模型
        /// </summary>
        protected ReviewInfo Review;
        BLLReview bllReview = new BLLJIMP.BLLReview();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["ReviewId"]))
            {
                Response.Write("ReviewId 参数必传");
                Response.End();

            }
            Review = bllReview.Get<ReviewInfo>(string.Format(" AutoId={0}",Request["ReviewId"]));
            if (Review==null)
            {
                Response.Write("ReviewId 参数错误");
                Response.End();
            }
        }
    }
}