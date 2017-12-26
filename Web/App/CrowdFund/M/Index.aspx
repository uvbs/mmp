<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.M.Index" %>
<%
    string redirectUrl = Request["redirectUrl"];
    if (!string.IsNullOrEmpty(redirectUrl))
    {
        Response.Redirect(redirectUrl);

    }
    Response.WriteFile("index.html");

%>
