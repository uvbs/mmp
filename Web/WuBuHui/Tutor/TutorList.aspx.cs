using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Tutor
{
    public partial class TutorList : System.Web.UI.Page
    {
        public string TutorStr;
        public string ProfessionalStr;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string websiteOwner;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTutorCtype();
            }
        }
        private void GetTutorCtype()
        {
            this.websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;

            List<BLLJIMP.Model.ArticleCategory> trades = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType ='trade' AND WebsiteOwner='{0}'", this.websiteOwner));
            List<BLLJIMP.Model.ArticleCategory> Professionals = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType ='Professional' AND WebsiteOwner='{0}'", this.websiteOwner));

            if (trades != null)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in trades)
                {
                    TutorStr += "<input type=\"checkbox\" class=\"checkinput\" value=\"" + item.AutoID + "\" id=\"Tutor" + item.AutoID + "\" name=\"cbtrade\">";
                    TutorStr += "<label class=\"checklabel wbtn wbtn_gary\" for=\"Tutor" + item.AutoID + "\"> ";
                    TutorStr += "<span class=\"title\">" + item.CategoryName + "</span><span class=\"wbtn wbtn_orange checkmark\"><span class=\"iconfont icon-yes2\">";
                    TutorStr += "</span></span></label>";
                }
            }

            if (Professionals != null)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in Professionals)
                {
                    ProfessionalStr += "<input type=\"checkbox\" class=\"checkinput\" value=\"" + item.AutoID + "\"  id=\"Tutor" + item.AutoID + "\" name=\"cbprofessionals\">";
                    ProfessionalStr += "<label class=\"checklabel wbtn wbtn_gary\" for=\"Tutor" + item.AutoID + "\"> ";
                    ProfessionalStr += "<span class=\"title\">" + item.CategoryName + "</span><span class=\"wbtn wbtn_orange checkmark\"><span class=\"iconfont icon-yes2\">";
                    ProfessionalStr += "</span></span></label>";
                }
            }

        }
    }
}