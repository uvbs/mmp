using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Config
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : BaseHandlerNoAction
    {

        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            var companyConfig = bllWebsite.GetCompanyWebsiteConfig();
            if (!string.IsNullOrEmpty(companyConfig.LoginConfigJson))
            {
                JToken JToken = JToken.Parse(companyConfig.LoginConfigJson);
                apiResp.status = true;
                apiResp.msg = "ok";
                apiResp.result = new
                {
                    logo=companyConfig.WebsiteImage,
                    system_name=JToken["system_name"]!=null?JToken["system_name"].ToString():"",
                    background_image = JToken["background_image"] != null ? JToken["background_image"].ToString() : "",
                    login_btn_background_color = JToken["login_btn_background_color"] != null ? JToken["login_btn_background_color"].ToString() : "",


                        

                };

            }


            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        
    }
}