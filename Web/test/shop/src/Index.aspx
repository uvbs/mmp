<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Test.shop.src.Index" %>
<%
    string redirectUrl = Request["redirectUrl"];
    ZentCloud.BLLJIMP.Model.WebsiteInfo webSiteInfo = new ZentCloud.BLLJIMP.BLLMall().GetWebsiteInfoModelFromDataBase();
    
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