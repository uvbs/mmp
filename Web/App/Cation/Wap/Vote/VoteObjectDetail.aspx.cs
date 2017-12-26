using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote
{
    public partial class VoteObjectDetail : System.Web.UI.Page
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 投票对象信息
        /// </summary>
        public VoteObjectInfo model = new VoteObjectInfo();
        public VoteInfo VoteInfoModel = new VoteInfo();
        /// <summary>
        /// 剩余票数
        /// </summary>
        public string CanUseVoteCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(Request["id"]);
                //检查登录
                if (!bllVote.IsLogin)
                {
                    

                    Response.Write("请用微信打开");
                    Response.End();
                    return;
                   

                   

                }
                else
                {
                    //if (id == 121)
                    //{
                    //    if (!Request.Url.Host.Equals("comeoncloud.comeoncloud.net"))
                    //    {
                    //        Response.Write("请用微信打开");
                    //        Response.End();
                    //    }
                    //}
                }
                //
                model = bllVote.GetVoteObjectInfo(id);
                if (model == null)
                {
                    Response.Write("选手不存在");
                    Response.End();
                    return;
                }
                VoteInfoModel = bllVote.GetVoteInfo(model.VoteID);
                //if ((!VoteInfoModel.WebsiteOwner.Equals(bllVote.WebsiteOwner)) && (VoteInfoModel.IsFree.Equals(0)))
                //{
                //    Response.Write("拒绝访问");
                //    Response.End();
                //    return;

                //}
                if (VoteInfoModel.VoteStatus.Equals(0))
                {
                    CanUseVoteCount = "投票已结束";
                    return;
                }
                if (!string.IsNullOrEmpty(VoteInfoModel.StopDate))
                {
                    if (DateTime.Now >= (Convert.ToDateTime(VoteInfoModel.StopDate)))
                    {
                        CanUseVoteCount = "投票已结束";
                        return;
                    }
                }
                //if (VoteInfoModel.IsFree.Equals(1) && VoteInfoModel.VoteType.Equals(1) && VoteInfoModel.VoteCountAutoUpdate.Equals(1))
                //{
                //    //检查是否更新票数
                //    bllVote.UpdateUserVoteCount(VoteInfoModel.AutoID, DataLoadTool.GetCurrUserModel());

                //}
                CanUseVoteCount = bllVote.GetCanUseVoteCount(model.VoteID, DataLoadTool.GetCurrUserID()).ToString();

            }
            catch (Exception)
            {

                Response.End();
            }

        }


    }
}
