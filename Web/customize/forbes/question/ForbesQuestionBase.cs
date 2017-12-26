using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    /// <summary>
    /// 基类
    /// </summary>
    public class ForbesQuestionBase : System.Web.UI.Page
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo CurrentUserInfo = new UserInfo();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected override void OnInit(EventArgs e)
        {
            if (!bllUser.IsLogin)
            {
                Response.Write("请用微信打开");
                Response.End();

            }
            else
            {
                CurrentUserInfo = bllUser.GetCurrentUserInfo();
                //int count = bllUser.GetCount<ForbesQuestionResult>(string.Format("UserId='{0}'", CurrentUserInfo.UserID));
                //if (count >= 2)
                //{

                //    Response.Write("您已经答过两题");
                //    Response.End();

                //}
            }
           
        }
    }
}