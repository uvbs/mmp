<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Shop.Index" %>
<%
    string redirectUrl = Request["redirectUrl"];
    if (!string.IsNullOrEmpty(redirectUrl))
    {
        Response.Redirect(redirectUrl);

    }
    try
    {
        string indexStr = System.IO.File.ReadAllText(this.Server.MapPath("index.html"));
        ZentCloud.BLLJIMP.BLLWebSite bllWebSite = new ZentCloud.BLLJIMP.BLLWebSite();
        ZentCloud.BLLJIMP.Model.CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
        indexStr = indexStr.Replace("'$$$MallConfig$$$'",new ZentCloud.BLLJIMP.BLLKeyValueData().GetMallConfigList());
        indexStr = indexStr.Replace("<title></title>", "<title>" + nWebsiteConfig.WebsiteTitle + "</title>");
        
        //this.Title = nWebsiteConfig.WebsiteTitle;

        if (!string.IsNullOrWhiteSpace(webSiteInfo.ThemeColor))
        {
            var i = indexStr.IndexOf("</head>");
            indexStr = indexStr.Replace("</head>", 
                string.Format("<style>.themeColor{1}color: {0}!important;{2}.themeBackground{1}  background:  {0}!important;{2}.themeBackgroundColor{1}  background-color:  {0}!important;{2}.themeBorderColor{1}  border-color:   {0}!important;{2}</style></head>",
                    webSiteInfo.ThemeColor,"{","}"
                ));
        }

        if (!string.IsNullOrEmpty(webSiteInfo.LoginPageConfig))
        {
            string loginPlaceholderStyle = string.Empty;
            string regPlaceholderStyle = string.Empty;
            ZCJson.Linq.JObject jo = ZCJson.Linq.JObject.Parse(webSiteInfo.LoginPageConfig);
            ZCJson.Linq.JToken toKenLogin = jo["pages"][0];
            ZCJson.Linq.JToken toKenReg = jo["pages"][1];
            if (toKenLogin != null)
            {
                loginPlaceholderStyle = toKenLogin["login_text_tip_fontcolor"].ToString();
            }
            if (toKenReg != null)
            {
                regPlaceholderStyle = toKenReg["reg_text_tip_fontcolor"].ToString();
            }
            var i = indexStr.IndexOf("</head>");
            indexStr = indexStr.Replace("</head>",
                string.Format("<style>.loginStyle ::-webkit-input-placeholder{1}color: {0}!important;{2}.regStyle ::-webkit-input-placeholder{1}  color:  {3}!important;{2}</style></head>",
                    loginPlaceholderStyle, "{", "}", regPlaceholderStyle
                ));
        }
        
        
        
        Response.Write(indexStr);

        //图标文件
        string icoScript = bllWebSite.GetIcoScript();
        if (!string.IsNullOrWhiteSpace(icoScript))
        {
            Response.Write(icoScript);
        }
    }
    catch (Exception ex)
    {
        //Response.Write(ex.Message);
        Response.WriteFile("index.html");
    }

%>

<script>
    (function () {
        'use strict';
        angular
.module('shop')
.config(config);
        function config(shopInfo) {
            shopInfo.title = "<%=webSiteInfo.WXMallName%>";
            shopInfo.desc = "<%=webSiteInfo.WXMallName%>";
            shopInfo.imgUrl = "<%=webSiteInfo.WXMallBannerImage%>";
            
        }
    })();
</script>
<script>
    setTimeout(function () {
        $.get('/serv/pubapi.ashx?action=VistWxpay', {}, function (data) {
            $.get('/serv/pubapi.ashx?action=VistWxpay', {}, function (data) {
            }, 'json')
        }, 'json')
    }, 1000);
</script>