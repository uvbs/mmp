using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions
{
    public partial class WXDiscussList : System.Web.UI.Page
    {

        public string DisussStr;
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

            List<BLLJIMP.Model.ArticleCategory> acategorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='word'AND WebsiteOwner='{0}'", this.websiteOwner));
            if (acategorys != null)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in acategorys)
                {
                    DisussStr += "<li class=\"catli\" v=\"" + item.AutoID + "\"><a >" + item.CategoryName + "</a></li>";
                }
            }

        }
    }
}