using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.web
{
    public partial class detail : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        BLLJIMP.BLLJuActivity bllArticle = new BLLJIMP.BLLJuActivity("");
        protected void Page_Load(object sender, EventArgs e)
        {
            string templateName = bll.GetWebsiteInfoModelFromDataBase().CompanyWebSiteTemplateName;
            if (string.IsNullOrEmpty(templateName))
            {
                templateName = "template1";
            }
            string url = string.Format("http://{0}{1}", Request.Url.Host,string.Format("/web/template/{0}/article.html?articleid={1}", templateName, Request["articleid"]));
            string html = Common.MySpider.GetPageSourceForUTF8(url);
            int articleid = Convert.ToInt32(Request["articleid"], 16);
            JuActivityInfo article = new BLLJIMP.BLLJuActivity("").GetJuActivity(articleid);
            string title = string.Format("<title>{0}</title>", article.ActivityName);
            html = html.Replace("<title></title>", title);

            if (html.Contains("$SHARETITLE$"))
            {
                html = html.Replace("$SHARETITLE$", article.ActivityName);
            }
            if (html.Contains("$SHAREDESC$"))
            {
                html = html.Replace("$SHAREDESC$", article.Summary);
            }
            if (html.Contains("$SHAREIMGURL$"))
            {
                html = html.Replace("$SHAREIMGURL$", string.Format("http://{0}{1}", Request.Url.Host, article.ThumbnailsPath));
            }

            Response.Write(html);
            bllArticle.UpdateIPCount(articleid);
            bllArticle.UpdatePVCount(articleid);
        }
    }
}