<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Info.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.Info" %>
<% string indexStr = System.IO.File.ReadAllText(this.Server.MapPath("Info.html"));
    indexStr = indexStr.Replace("Info.html", "Info.aspx");
    indexStr = indexStr.Replace("List.html", "List.aspx");
    indexStr = indexStr.Replace("ShowMap.html", "ShowMap.aspx");
    this.Response.Write(indexStr); %>
