using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.comeoncloudV2
{
    public partial class Index : System.Web.UI.Page
    {
        BLLComponent bll = new BLLComponent();
        BLLWebSite bllWebSite = new BLLWebSite();
        string cgid;
        string cateid;
        string mallcate;
        string keyword;
        string ngroute;
        string key;
        protected void Page_Load(object sender, EventArgs e)
        {

            //#region 读取组件 模板
            //if (Request["cgid"] == null && Request["cateid"] == null && Request["ngroute"] == null && Request["key"] == null)
            //{
            //    this.Response.Write("参数错误");
            //    return;
            //}
            ////读取配置
            //cgid = Request["cgid"];
            //cateid = Request["cateid"] == null ? "" : Request["cateid"];
            //mallcate = Request["mallcate"] == null ? "" : Request["mallcate"];
            //keyword = Request["keyword"] == null ? "" : Request["keyword"];
            //ngroute = Request["ngroute"];
            //key = Request["key"];
            //if (ngroute != null)
            //{
            //    var routes = ngroute.Split('/');
            //    if (string.IsNullOrWhiteSpace(cgid) && routes.Length > 2) cgid = routes[2];
            //    if (string.IsNullOrWhiteSpace(cateid) && routes.Length > 3) cgid = routes[3];
            //}

            ////替换配置
            //Component model = new Component();
            //if (!string.IsNullOrWhiteSpace(key))
            //{
            //    model = bll.GetComponentByKey(key, bll.WebsiteOwner);
            //}
            //else
            //{
            //    model = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, cgid));
            //}
            //if (model == null)
            //{
            //    this.Response.Write("组件不存在");
            //    return;
            //}
            //UserInfo curUser = bllUser.GetCurrentUserInfo();
            //CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            //#region 检查页面访问权限
            //if (model.AccessLevel > 0)
            //{
            //    string noPmsUrl = "/Error/NoPmsMobile.htm";
            //    if (curUser == null)
            //    {
            //        this.Response.Redirect(noPmsUrl);
            //        return;
            //    }
            //    if (curUser.AccessLevel < model.AccessLevel)
            //    {
            //        if (bllUser.IsMember())
            //        {
            //            this.Response.Redirect(noPmsUrl);
            //            return;
            //        }
            //        else
            //        {
            //            if (nWebsiteConfig == null || nWebsiteConfig.MemberStandard == 0)
            //            {
            //                this.Response.Redirect(noPmsUrl);
            //                return;
            //            }
            //            if (nWebsiteConfig.MemberStandard == 1)
            //            {
            //                this.Response.Redirect("/App/Member/Wap/PhoneVerify.aspx?referrer=" + HttpUtility.UrlEncode(this.Request.Url.ToString()));
            //            }
            //            else
            //            {
            //                this.Response.Redirect("/App/Member/Wap/CompleteUserInfo.aspx?referrer=" + HttpUtility.UrlEncode(this.Request.Url.ToString()));
            //            }
            //            return;
            //        }
            //    }
            //}
            //#endregion
            //ComponentModel cmodel = bll.GetByKey<ComponentModel>("AutoId", model.ComponentModelId.ToString());
            //if (cmodel == null || string.IsNullOrWhiteSpace(cmodel.ComponentModelHtmlUrl))
            //{
            //    this.Response.Write("页面不存在");
            //    return;
            //}
            //string cmodelPath = this.Server.MapPath(cmodel.ComponentModelHtmlUrl);
            //if (!File.Exists(cmodelPath))
            //{
            //    this.Response.Write("页面不存在");
            //    return;
            //}
            //string indexStr = File.ReadAllText(cmodelPath);

            //#endregion

        }
        private void LoadParams()
        {
            //读取配置
            cgid = Request["cgid"];
            cateid = Request["cateid"] == null ? "" : Request["cateid"];
            mallcate = Request["mallcate"] == null ? "" : Request["mallcate"];
            keyword = Request["keyword"] == null ? "" : Request["keyword"];
            ngroute = Request["ngroute"];
            key = Request["key"];
            if (ngroute != null)
            {
                var routes = ngroute.Split('/');
                if (string.IsNullOrWhiteSpace(cgid) && routes.Length > 2) cgid = routes[2];
                if (string.IsNullOrWhiteSpace(cateid) && routes.Length > 3) cgid = routes[3];
            }

        }
    }
}