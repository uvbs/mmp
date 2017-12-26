<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="TheVoteInfoChart.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TheVote.TheVoteInfoChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;所有投票&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>查看结果图</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="container" style="min-width: 700px; height: 400px">
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/highcharts.js" type="text/javascript"></script>
    <script src="/Scripts/exporting.js" type="text/javascript"></script>
    <script src="/Scripts/highcharts-3d.js" type="text/javascript"></script>
    <script>
 ﻿$(function () {
   var autoId='<%=autoId %>';
   var chart;
   var handlerUrl = "/Handler/App/WXTheVoteInfoHandler.ashx";
    function GetData(){
      $.ajax({
         type: 'post',
         url: handlerUrl,
         data: {Action:"GetDiInfos",autoId:autoId},
         dataType: "json",
         success: function (resp) {
             if (resp.Status == 0) {
                    InitChart(resp.ExObj);
                    showValues();
              }
            } 
          });
    }
    function InitChart(data){
        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'container',
                type: 'column',
                margin: 75,
                options3d: {
                    enabled: true,
                    alpha: 15,
                    beta: 15,
                    depth: 50,
                    viewDistance: 25
                }
            },
             xAxis: {
                categories:data.categories
            },
            title: {
                text: '投票信息结果图'
            },
            plotOptions: {
                column: {
                    depth: 20
                }
            },
            series: data.series
         });
    }
    

    function showValues() {
      chart.options.chart.options3d.alpha=0;
      chart.options.chart.options3d.beta=0;
      chart.redraw(false);
    }
  GetData();
});				
</script>
</asp:Content>
