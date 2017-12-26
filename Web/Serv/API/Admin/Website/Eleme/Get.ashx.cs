using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TakeOutSDK.Eleme;
using TakeOutSDK.Eleme.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Website.Eleme
{
    /// <summary>
    /// Get 的摘要说明 获取饿了么token
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();

        public void ProcessRequest(HttpContext context)
        {

            var website = bllWebsite.GetWebsiteInfoModelFromDataBase();
            int num = 30;
            string url = string.Empty;
            if (HttpContext.Current.Request.Url.Host.ToString().Contains("localhost"))
            {
                num = 1;
                url = "https://open-api-sandbox.shop.ele.me/token";
            }
            else
            {
                url = "https://open-api.shop.ele.me/token";
            }
            if (!string.IsNullOrWhiteSpace(website.ElemeAccessToken) && website.ElemeTokenLastUpdateDate.Value.AddDays(num) > DateTime.Now)
            {
                apiResp.result = website.ElemeAccessToken;
            }
            else
            {
                Authorize model = new Authorize(website.ElemeAppKey, website.ElemeAppSecret);
                AuthorizeResponse toKenModel = model.GetToken(url);
                website.ElemeAccessToken = toKenModel.access_token;
                website.ElemeTokenLastUpdateDate = DateTime.Now;
                bllWebsite.Update(website);
                apiResp.result = website.ElemeAccessToken;
            }
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

    }
}