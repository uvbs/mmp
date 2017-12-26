using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Position
{
    public partial class WXPositionList : System.Web.UI.Page
    {
        public System.Text.StringBuilder sbTrade=new System.Text.StringBuilder();
        public System.Text.StringBuilder sbProfessional=new System.Text.StringBuilder();
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {

            List<BLLJIMP.Model.ArticleCategory> trades = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType ='trade' AND WebsiteOwner='{0}'", bll.WebsiteOwner));
            List<BLLJIMP.Model.ArticleCategory> Professionals = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType ='Professional' AND WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (trades.Count>0)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in trades)
                {
                   sbTrade.AppendFormat("<input type=\"checkbox\" class=\"checkinput\" value=\"" + item.AutoID + "\" id=\"trade" + item.AutoID + "\" name=\"cbtrade\" >");
                   sbTrade.AppendFormat("<label class=\"checklabel wbtn wbtn_gary\" for=\"trade" + item.AutoID + "\"> ");
                   sbTrade.AppendFormat("<span class=\"title\">" + item.CategoryName + "</span><span class=\"wbtn wbtn_orange checkmark\"><span class=\"iconfont icon-yes2\">");
                   sbTrade.AppendFormat( "</span></span></label>");
                }
            }

            if (Professionals.Count>0)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in Professionals)
                {
                   sbProfessional.AppendFormat("<input type=\"checkbox\" class=\"checkinput\" value=\"" + item.AutoID + "\"  id=\"professionals" + item.AutoID + "\" name=\"cbprofessionals\">");
                   sbProfessional.AppendFormat("<label class=\"checklabel wbtn wbtn_gary\" for=\"professionals" + item.AutoID + "\"> ");
                   sbProfessional.AppendFormat("<span class=\"title\">" + item.CategoryName + "</span><span class=\"wbtn wbtn_orange checkmark\"><span class=\"iconfont icon-yes2\">");
                   sbProfessional.AppendFormat("</span></span></label>");
                }
            }
        }

    }
}