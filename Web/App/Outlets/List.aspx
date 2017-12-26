<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.List" %>

<% string indexStr = System.IO.File.ReadAllText(this.Server.MapPath("List.html"));
    indexStr = indexStr.Replace("Info.html", "Info.aspx");
    indexStr = indexStr.Replace("List.html", "List.aspx");
    indexStr = indexStr.Replace("ShowMap.html", "ShowMap.aspx");
    this.Response.Write(indexStr); %>