﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CompanyWebsite
{
    public partial class ProjectorCompile : System.Web.UI.Page
    {
        public string aid = "0";
        public string webAction = "add";
        public string actionStr = "";
        BLLWebSite bll = new BLLWebSite();
        public CompanyWebsite_Projector model = new CompanyWebsite_Projector();
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bll.GetCompanyWebsiteProjectorById(aid);

                if (model == null)
                {
                    Response.End();
                }
                else
                {

                }
            }




        }

    }
}