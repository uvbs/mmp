<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WebAnalytics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WebAnalytics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   


   <style type="text/css">
    #tableMain
    {
        width:100%;
        text-align:center;
        
    }
    #tableMain th 
    {
        height: 16px;
        line-height: 16px;
        padding: 7px 10px;
        color: #999;
        white-space: nowrap;
        font-weight: normal;
    }
    #tableMain td 
    {
        height:25px; border-bottom:#F0f0f0 solid 1px;
        
    }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;" >网站统计</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>网站统计</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
	<script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        var weekChangeValue = 0;
        $(function () {
            //            $("input[name='ChartItem']").click(function () {
            //                drawChart();
            //            });
            $("#btnThisWeek").addClass("disabled");
            $("#btnNextWeek").addClass("disabled");

            $('#btnNextWeek').click(function () {
                if (weekChangeValue == 0)
                    return;
                weekChangeValue += 7;
                drawChart();
            });
            $('#btnPreWeek').click(function () {
                weekChangeValue -= 7;
                drawChart();
            });
            $('#btnThisWeek').click(function () {
                if (weekChangeValue == 0)
                    return;
                weekChangeValue = 0;
                drawChart();
            });

            $('#btnReloadChart').click(function () {
                drawChart();
            });
        });


        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {

            //上下周按钮处理
            if (weekChangeValue == 0) {
                $("#btnThisWeek").addClass("disabled");
                $("#btnNextWeek").addClass("disabled");
            }
            else {
                $("#btnThisWeek").removeClass("disabled");
                $("#btnNextWeek").removeClass("disabled");
            }

            var chartItem = GetCheckGroupVal("ChartItem", "value");
            $.messager.progress({ text: '正在处理。。。' });
            $.ajax({
                type: 'post',
                url: '/Handler/CommHandler.ashx',
                data: { Action: 'GetLineChartData', ChartItem: chartItem, weekChangeValue: weekChangeValue },
                success: function (result) {
                    $.messager.progress('close');
                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {

                        var data = google.visualization.arrayToDataTable(
                        //[["date", "user", "test","ex"], ["2013-12-30", 1, 2, 3], ["2013-12-31", 4, 5, 6]]
                        //eval(resp.ExObj)
                        resp.ExObj
                        //                        [
                        //                          ['Year', 'Sales', 'Expenses'],
                        //                          ['2004', 1000, 400],
                        //                          ['2005', 1170, 460],
                        //                          ['2006', 660, 1120],
                        //                          ['2007', 1030, 540]
                        //                        ]
                        );

                        var options = {
                            title: '一周数据曲线',
                            fontSize: 10
                        };

                        var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
                        chart.draw(data, options);
                    }
                    else {
                        Alert('数据加载失败');
                    }
                }
            });


        }



    </script>
    <div class="ActivityBox">
		<table cellspacing="0" cellpadding="0" id="tableMain">
        <tr style="background-color: #fae1dc;">
				<th></th>
				<th>新用户</th>
                <th>PC主页(IP/UV/PV)</th>
				<th>手机主页(IP/UV/PV)</th>
				<th>文章活动(IP/UV/PV)</th>
                <th>文章数</th>
                <th>活动数</th>
                <th>分享数</th>
                <th>报名人数</th>
			</tr>
			<tr>
				<td style=" width:50px;">今日</td>
				<td><%=alyModel.NewUserToday %></td>
				<td><%=string.Format("{0}/{1}/{2}",alyModel.PCIndexViewIPToday,alyModel.PCIndexViewUVToday,alyModel.PCIndexViewPVToday) %></td>
				<td><%=string.Format("{0}/{1}/{2}", alyModel.MobileIndexViewIPToday, alyModel.MobileIndexViewUVToday, alyModel.MobileIndexViewPVToday)%></td>
                <td><%=string.Format("{0}/{1}/{2}",alyModel.ArticleViewIPToday,alyModel.ArticleViewUVToday,alyModel.ArticleViewPVToday) %></td>
                <td><%=alyModel.ArticlePubToday %></td>
				<td><%=alyModel.ActivityPubToday %></td>
				<td><%=alyModel.ShareToday %></td>
                <td><%=alyModel.SignUpToday %></td>
			</tr>	
			<tr>
				<td>昨日</td>
                <td><%=alyModel.NewUserYesterday %></td>
				<td><%=string.Format("{0}/{1}/{2}",alyModel.PCIndexViewIPYesterday,alyModel.PCIndexViewUVYesterday,alyModel.PCIndexViewPVYesterday) %></td>
				<td><%=string.Format("{0}/{1}/{2}", alyModel.MobileIndexViewIPYesterday, alyModel.MobileIndexViewUVYesterday, alyModel.MobileIndexViewPVYesterday)%></td>
                <td><%=string.Format("{0}/{1}/{2}",alyModel.ArticleViewIPYesterday,alyModel.ArticleViewUVYesterday,alyModel.ArticleViewPVYesterday) %></td>
                <td><%=alyModel.ArticlePubYesterday %></td>
				<td><%=alyModel.ActivityPubYesterday %></td>
				<td><%=alyModel.ShareYesterday %></td>
                <td><%=alyModel.SignUpYesterday %></td>
			</tr>			
			<tr>
				<td>本周</td>
                <td><%=alyModel.NewUserThisWeek %></td>
				<td><%=string.Format("{0}/{1}/{2}",alyModel.PCIndexViewIPThisWeek,alyModel.PCIndexViewUVThisWeek,alyModel.PCIndexViewPVThisWeek) %></td>
				<td><%=string.Format("{0}/{1}/{2}", alyModel.MobileIndexViewIPThisWeek, alyModel.MobileIndexViewUVThisWeek, alyModel.MobileIndexViewPVThisWeek)%></td>
                <td><%=string.Format("{0}/{1}/{2}",alyModel.ArticleViewIPThisWeek,alyModel.ArticleViewUVThisWeek,alyModel.ArticleViewPVThisWeek) %></td>
                <td><%=alyModel.ArticlePubThisWeek %></td>
				<td><%=alyModel.ActivityPubThisWeek %></td>
				<td><%=alyModel.ShareThisWeek %></td>
                <td><%=alyModel.SignUpThisWeek %></td>
			</tr>			
			<tr>
				<td>本月</td>
                <td><%=alyModel.NewUserThisMonth %></td>
				<td><%=string.Format("{0}/{1}/{2}",alyModel.PCIndexViewIPThisMonth,alyModel.PCIndexViewUVThisMonth,alyModel.PCIndexViewPVThisMonth) %></td>
				<td><%=string.Format("{0}/{1}/{2}", alyModel.MobileIndexViewIPThisMonth, alyModel.MobileIndexViewUVThisMonth, alyModel.MobileIndexViewPVThisMonth)%></td>
                <td><%=string.Format("{0}/{1}/{2}",alyModel.ArticleViewIPThisMonth,alyModel.ArticleViewUVThisMonth,alyModel.ArticleViewPVThisMonth) %></td>
                <td><%=alyModel.ArticlePubThisMonth %></td>
				<td><%=alyModel.ActivityPubThisMonth %></td>
				<td><%=alyModel.ShareThisMonth %></td>
                <td><%=alyModel.SignUpThisMonth %></td>
			</tr>			
			<tr>
				<td>全部</td>
                <td><%=alyModel.NewUserAll %></td>
				<td><%=string.Format("{0}/{1}/{2}",alyModel.PCIndexViewIPAll,alyModel.PCIndexViewUVAll,alyModel.PCIndexViewPVAll) %></td>
				<td><%=string.Format("{0}/{1}/{2}", alyModel.MobileIndexViewIPAll, alyModel.MobileIndexViewUVAll, alyModel.MobileIndexViewPVAll)%></td>
                <td><%=string.Format("{0}/{1}/{2}",alyModel.ArticleViewIPAll,alyModel.ArticleViewUVAll,alyModel.ArticleViewPVAll) %></td>
                <td><%=alyModel.ArticlePubAll %></td>
				<td><%=alyModel.ActivityPubAll %></td>
				<td><%=alyModel.ShareAll %></td>
                <td><%=alyModel.SignUpAll %></td>
			</tr>																				
		</table>
		</div>
        <fieldset>
            <legend>图表分析</legend>

            <table width="100%">
                <tr>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="新增用户" id="chkNewUser" checked="checked" /> <label for="chkNewUser">新增用户</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="新发文章" id="chkArticlePub" checked="checked" /> <label for="chkArticlePub">新发文章</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="新发活动" id="chkActivityPub" checked="checked" /> <label for="chkActivityPub">新发活动</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="分享数" id="chkShare"/> <label for="chkShare">分享数</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="报名人数" id="chkSignUp"/> <label for="chkSignUp">报名人数</label></td>
                </tr>
                <tr>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="PC主页访问(IP)" id="chkPCIndexViewIP" /> <label for="chkPCIndexViewIP">PC主页访问(IP)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="PC主页访问(UV)" id="chkPCIndexViewUV" /> <label for="chkPCIndexViewUV">PC主页访问(UV)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="PC主页访问(PV)" id="chkPCIndexViewPV" /> <label for="chkPCIndexViewPV">PC主页访问(PV)</label></td>
                    <td style="width:20%" align="left"></td>
                    <td style="width:20%" align="left"></td>
                </tr>
                <tr>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="手机主页访问(IP)" id="chkMobileIndexViewIP" /> <label for="chkMobileIndexViewIP">手机主页访问(IP)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="手机主页访问(UV)" id="chkMobileIndexViewUV" /> <label for="chkMobileIndexViewUV">手机主页访问(UV)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="手机主页访问(PV)" id="chkMobileIndexViewPV" /> <label for="chkMobileIndexViewPV">手机主页访问(PV)</label></td>
                    <td style="width:20%" align="left"></td>
                    <td style="width:20%" align="left"></td>
                </tr>
                <tr>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="文章活动访问(IP)" id="chkArticleViewIP" /> <label for="chkArticleViewIP">文章活动访问(IP)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="文章活动访问(UV)" id="chkArticleViewUV" /> <label for="chkArticleViewUV">文章活动访问(UV)</label></td>
                    <td style="width:20%" align="left"><input type="checkbox" name="ChartItem" value="文章活动访问(PV)" id="chkArticleViewPV" /> <label for="chkArticleViewPV">文章活动访问(PV)</label></td>
                    <td style="width:20%" align="left"></td>
                    <td style="width:20%" align="left"></td>
                </tr>
                <tr>
                    <td colspan="5">
                      <a href="javascript:;" class="button button-rounded button-flat-highlight" id="btnReloadChart">重新生成图表</a>
                    </td>
                </tr>
            </table>
            <div id="chart_div" style="width: 100%; height: 500px; font-size:12px"></div>
            <div align="center">
                    <a href="javascript:;" class="button button-rounded button-flat-primary" id="btnPreWeek">上一周</a>
                    <a href="javascript:;" class="button button-rounded button-flat-primary" id="btnThisWeek">本周</a>
                    <a href="javascript:;" class="button button-rounded button-flat-primary" id="btnNextWeek">下一周</a>
            </div>
        </fieldset>
     
</asp:Content>
