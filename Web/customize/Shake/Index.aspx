﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Shake.Index" %>
<%

    string redirectUrl = Request["redirectUrl"];
    if (!string.IsNullOrEmpty(redirectUrl))
    {
        Response.Redirect(redirectUrl);
    }
    Response.WriteFile("index.html");
%>

