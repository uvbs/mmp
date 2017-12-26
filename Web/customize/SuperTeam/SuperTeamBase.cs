using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.SuperTeam
{
    /// <summary>
    /// 基类
    /// </summary>
    public class SuperTeamBase : System.Web.UI.Page
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo CurrentUserInfo = new UserInfo();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 我要报名或我的报名
        /// </summary>
        public string signUpText = "团队报名";
        protected override void OnInit(EventArgs e)
        {
            if (!bllUser.IsLogin)
            {
                Response.Write("请在微信客户端微信打开");
                Response.End();

            }
            else
            {
                CurrentUserInfo = bllUser.GetCurrentUserInfo();
                int voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SuperTeamVoteId"]);
               var model = bllVote.GetVoteObjectInfo(voteId, CurrentUserInfo.UserID);
                if (model!= null)
                {
                    signUpText = "我的报名";

                }
            }
           
        }
    }
}