using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.Jiepai
{
    public partial class SignUp : JiePaiBase
    {
        /// <summary>
        /// 投票业务逻辑基类
        /// </summary>
        ZentCloud.BLLJIMP.BLLVote bllVote = new ZentCloud.BLLJIMP.BLLVote();
        /// <summary>
        /// 选手信息
        /// </summary>
        public ZentCloud.BLLJIMP.Model.VoteObjectInfo model;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
        /// <summary>
        /// 世界街拍投票ID
        /// </summary>
        int jiePaiVoteId;
        protected void Page_Load(object sender, EventArgs e)
        {
            jiePaiVoteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["JiePaiVoteId"]);
            if (bllVote.IsLogin)
            {
                currentUserInfo = bllVote.GetCurrentUserInfo();
            }
          
            model = bllVote.GetVoteObjectInfo(jiePaiVoteId, currentUserInfo.UserID);
        }
    }
}