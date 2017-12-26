<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.jikuwifi.Index" %>
<%
    string redirectUrl = Request["redirectUrl"];
    if (!string.IsNullOrEmpty(redirectUrl))
    {
        Response.Redirect(redirectUrl);
    }
    try
    {
        string indexString = System.IO.File.ReadAllText(this.Server.MapPath("/customize/jikuwifi/index.html"));
        string mallConfigString = new ZentCloud.BLLJIMP.BLLKeyValueData().GetMallConfigList();
        indexString = indexString.Replace("var MALLCONFIG={mallconfignew:{}};", string.Format("var MALLCONFIG={0};", mallConfigString));
        //indexString = indexString.Replace("src=\"scripts/", "src=\"/customize/jikuwifi/scripts/");
        //indexString = indexString.Replace("href=\"styles/", "href=\"/customize/jikuwifi/styles/");
        Response.Write(indexString);
    }
    catch (Exception)
    {
        Response.WriteFile("index.html");

    }
    
%>

