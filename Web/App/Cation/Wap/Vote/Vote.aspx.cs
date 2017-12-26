using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote
{
    public partial class Vote : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 投票信息
        /// </summary>
        public VoteInfo VoteInfo = new VoteInfo();
        /// <summary>
        /// 剩余票数
        /// </summary>
        public string CanUseVoteCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int voteID = int.Parse(Request["vid"]);
                //检查登录
                if (!bllVote.IsLogin)
                {
                    //if (VoteID == 121)
                    //{

                        Response.Write("请用微信打开");
                        Response.End();
                        return;
                    //}

                  //Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}?vid={1}", Request.FilePath,VoteID));

                }
                else
                {
                    //if (VoteID==121)
                    //{
                    //    if (!Request.Url.Host.Equals("comeoncloud.comeoncloud.net"))
                    //    {
                    //        Response.Write("请用微信打开");
                    //        Response.End();
                    //    }
                    //}
                }
                //

                VoteInfo = bllVote.GetVoteInfo(voteID);

                if (VoteInfo==null)
                {
                    Response.Write("投票不存在");
                    Response.End();
                    return;
                }
                //if ((!VoteInfo.WebsiteOwner.Equals(bllVote.WebsiteOwner))&&(VoteInfo.IsFree.Equals(0)))
                //{
                //    Response.Write("拒绝访问");
                //    Response.End();
                //    return;
                //}
                if (VoteInfo.VoteStatus.Equals(0))
                {
                    CanUseVoteCount = "投票已结束";
                    return;
                }
                if (!string.IsNullOrEmpty(VoteInfo.StopDate))
                {
                    if (DateTime.Now>=(Convert.ToDateTime(VoteInfo.StopDate)))
                    {
                        CanUseVoteCount = "投票已结束";
                        return;
                    }
                }

                //if (VoteInfo.IsFree.Equals(1)&&VoteInfo.VoteType.Equals(1)&&VoteInfo.VoteCountAutoUpdate.Equals(1))
                //{
                //    //检查是否更新票数
                //    bllVote.UpdateUserVoteCount(VoteInfo.AutoID, DataLoadTool.GetCurrUserModel());

                //}
                CanUseVoteCount = bllVote.GetCanUseVoteCount(voteID, DataLoadTool.GetCurrUserID()).ToString();

            }
            catch (Exception)
            {
                
                Response.End();
            }

        }
    }
}