using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public class DistributionBase : System.Web.UI.Page
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo CurrentUserInfo = new UserInfo();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected override void OnInit(EventArgs e)
        {

            var rawUrl = Request.RawUrl.ToLower();

            if (bllUser.IsLogin)
            {
                CurrentUserInfo = bllUser.GetCurrentUserInfo();
                CurrentUserInfo.WXHeadimgurlLocal = string.IsNullOrEmpty(CurrentUserInfo.WXHeadimgurlLocal) ? "/App/Cation/Wap/Mall/Distribution/images/person.png" : CurrentUserInfo.WXHeadimgurlLocal;

            }
            else if(rawUrl.IndexOf("/app/cation/wap/mall/distribution/mydistributionqcode.aspx") == -1)
            {

                this.Response.Write("<span style=\"font-size:40px;\">请在微信客户端中打开！</span>");
                this.Response.End();


            }
            base.OnInit(e);
        }
    }
}