﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BeachHoney
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BeachHoneyBase : System.Web.UI.Page
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
            }
           
        }
    }
}