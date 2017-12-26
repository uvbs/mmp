<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ShowMap.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.ShowMap" %>
<% string indexStr = System.IO.File.ReadAllText(this.Server.MapPath("ShowMap.html"));
    indexStr = indexStr.Replace("Info.html", "Info.aspx");
    indexStr = indexStr.Replace("List.html", "List.aspx");
    indexStr = indexStr.Replace("ShowMap.html", "ShowMap.aspx");
    this.Response.Write(indexStr); %>