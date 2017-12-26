using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.web
{
    public partial class list : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            string templateName = bll.GetWebsiteInfoModelFromDataBase().CompanyWebSiteTemplateName;
            if (string.IsNullOrEmpty(templateName))
            {
                templateName = "template1";
            }

            string url = string.Format("http://{0}{1}", Request.Url.Host, string.Format("/web/template/{0}/list.html?cateid={1}",templateName,Request["cateid"]));
            string html= Common.MySpider.GetPageSourceForUTF8(url);
            var websitConfig = bll.GetCompanyWebsiteConfig();
            if (websitConfig!=null)
            {
                string title = string.Format("<title>{0}</title>", websitConfig.WebsiteTitle);
                html = html.Replace("<title></title>", title);

            }
            else
            {
                websitConfig = new BLLJIMP.Model.CompanyWebsite_Config();
            }
            if (html.Contains("$SHARETITLE$"))
            {
                html = html.Replace("$SHARETITLE$", websitConfig.WebsiteTitle);
            }
            if (html.Contains("$SHAREDESC$"))
            {
                html = html.Replace("$SHAREDESC$", websitConfig.WebsiteDescription);
            }
            if (html.Contains("$SHAREIMGURL$"))
            {
                html = html.Replace("$SHAREIMGURL$", string.Format("http://{0}{1}", Request.Url.Host, websitConfig.WebsiteImage));
            }

            Response.Write(html);
        }
    }
}