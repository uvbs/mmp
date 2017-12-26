using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Partner
{
    public partial class WXPartnerList : System.Web.UI.Page
    {
        public string PartnerStr;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string websiteOwner;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCType();
            }
        }
        private void GetCType()
        {
            this.websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;

            List<BLLJIMP.Model.ArticleCategory> acategorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='Partner'AND WebsiteOwner='{0}' order by sort asc, AutoId asc", this.websiteOwner));
            if (acategorys.Count>0)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in acategorys)
                {
                    PartnerStr += "<li class=\"catli\" v=\"" + item.AutoID + "\"><a >" + item.CategoryName + "</a></li>";
                }
            }

        }
    }
}