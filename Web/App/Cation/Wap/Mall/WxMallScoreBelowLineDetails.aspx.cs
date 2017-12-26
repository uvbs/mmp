﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class WxMallScoreBelowLineDetails : System.Web.UI.Page
    {

        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public WXMallScoreProductInfo model;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo currentUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                int Pid = int.Parse(Request["pid"]);
                model = bllMall.GetScoreProduct(Pid);
                if (model == null)
                {
                    Response.End();
                }
                if (bllMall.IsLogin)
                {
                    currentUserInfo = DataLoadTool.GetCurrUserModel();
                }
                else
                {
                    Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath + "?pid=" + Request["pid"]));

                }

            }
            catch (Exception)
            {

                Response.End();
            }


        }
    }
}