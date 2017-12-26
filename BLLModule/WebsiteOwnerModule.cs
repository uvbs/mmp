using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.IO;

namespace ZentCloud.BLLModule
{
    public class WebsiteOwnerModule : IHttpModule
    {

        public void Dispose()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        ///Bll Redis
        /// </summary>
        BLLJIMP.BLLRedis bllRedis = new BLLJIMP.BLLRedis();
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }
        void context_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                /*
                * 获取当前域名
                * 
                * 判断是否是已经存在站点配置，如果存在，判断域名相同则不再处理，不相同则进行重新获取配置处理
                * 不存在站点配置则立即进行获取配置处理
                * 
                * 根据域名取得站点配置
                * 如果取得站点配置着保存当前配置
                * 否则跳到域名未注册页面
                * 
                */

                //站点配置信息 WebsiteInfoModel , 站点配置域名 WebsiteDomain

                // 获取应用程序
                HttpContext context = ((HttpApplication)sender).Context;
                string currAbsolutePath = context.Request.Url.AbsolutePath == null ? "" : context.Request.Url.AbsolutePath.ToLower();
                //ToLog(currAbsolutePath);

                //禁用V2后台html访问
                if (currAbsolutePath == "/customize/mmpadmin/index.html")
                {
                    context.Response.Redirect("/Error/e404.htm");
                    return;
                }

                ToLog(string.Format("currAbsolutePath:{0}", currAbsolutePath));
                string pageExtraName = System.IO.Path.GetExtension(currAbsolutePath);//ZentCloud.Common.IOHelper.GetExtraName(currAbsolutePath);

                //List<string> pageExtraNameFilterList = new List<string>()
                //{
                //    ".aspx",
                //    ".ashx",
                //    ".cn",
                //    ".com",
                //    ".net",
                //    ".chtml"
                //};
                //List<string> pageV2List = new List<string>(){
                //    "/",
                //    "/login",
                //    "/index",
                //    "/index2",
                //    "/adminlogin"
                //};

                ////只处理aspx、chtml、ashx、htm、html页面
                //if (!pageExtraNameFilterList.Contains(pageExtraName) && !pageV2List.Exists(p => p.Equals(currAbsolutePath)))
                //    return;

                if (context.Session == null) return;
                if (context.Session["WebsiteOwner"] != null &&
                    context.Session["WebsiteInfoModel"] != null)
                {
                    return;
                }

                string domain = context.Request.Url.Host.ToLower();
                //ToLog(string.Format("domain:{0}", domain));
                //WebsiteDomainInfo websiteDomainModel = bll.Get<WebsiteDomainInfo>(string.Format(" WebsiteDomain = '{0}' ", domain));
                WebsiteDomainInfo websiteDomainModel = bllRedis.GetWebsiteDomainInfo(domain);
                //ToLog(string.Format("websiteDomainModel:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(websiteDomainModel)));
                if (websiteDomainModel == null)
                {
                    context.Response.Redirect("/Error/DomainNoReg.htm");
                    //GotoNoReg(context);
                    return;
                }
                else
                {
                    //ToLog(string.Format("WebsiteOwner:{0}", websiteDomainModel.WebsiteOwner));
                    WebsiteInfo webSiteModel = bllRedis.GetWebsiteInfo(websiteDomainModel.WebsiteOwner);
                    //ToLog(string.Format("webSiteModel:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(webSiteModel)));
                    context.Session["WebsiteOwner"] = webSiteModel.WebsiteOwner;
                    context.Session["WebsiteInfoModel"] = webSiteModel;
                }
            }
            catch (Exception ex)
            {
                //ToLog(ex.Message);
                //context.Response.Redirect("/error/syserror.html?aspxerrorpath=" + currAbsolutePath);
                //return;
                throw ex;
            }

        }

        //private void GotoNoReg(HttpContext context)
        //{
        //    context.Response.Redirect("/Error/DomainNoReg.htm");
        //}

        private void ToLog(string msg)
        {
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(@"D:\WebsiteOwnerModule错误.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
            //    }
            //}
            //catch { }
        }
    }
}
