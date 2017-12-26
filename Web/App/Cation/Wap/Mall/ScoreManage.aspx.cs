﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ScoreManage : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo currentUserInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllMall.IsLogin)
            {
                currentUserInfo = DataLoadTool.GetCurrUserModel();
            }
            else
            {
                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));

            }
        }
    }
}