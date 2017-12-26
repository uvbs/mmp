<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Scorehistory.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Scorehistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
        .wrapGrvData {
            margin: 0 16px;
            border-right: 1px solid #ddd;
        }

        .pageSubTitle {
            padding: 10px 16px;
            background-color: #f9f8f8;
            margin: 0 16px;
        }

            .pageSubTitle .mainText {
                display: inline-block;
                margin: 0 12px 0 0;
                padding: 0 0 0 10px;
                border-left: 4px solid #f70;
                font-size: 14px;
                font-weight: bold;
                line-height: 20px;
            }

        .pageTopBtnBg {
            background-color: #ffffff;
        }

        .truename {
            font-size: 20px;
            color: #FF1515;
            margin-right: 5px;
        }

        .warp-data {
            margin: 0px 16px;
            margin-top: 20px;
            background: #E0E0E0;
        }

        .tr td {
            font-size: 18px;
            max-width: 100%;
            margin-bottom: 5px;
            font-weight: 700;
            color: #5C5566;
        }

        .tdBack {
            background: #999395;
        }

        .tb {
            height: 100px;
            border-collapse: collapse;
            text-align: center;
            border-color: #fff;
            width: 100%;
        }
    </style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <a href="javascript:history.go(-1);" style="float: right; margin-right: 20px;" title="返回会员列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

    <div style="text-align: center; font-size: 20px; padding: 10px;">
        <span class="">会员积分统计</span>
    </div>

    <div class="pageSubTitle"><span class="mainText">详细数据</span></div>


    <%
        if (string.IsNullOrEmpty(scoreType))
        {
    %>
    <div class="warp-data">
        <table class="tb" border="1">
            <tr>
                <td>
                    <label>当前剩余积分</label></td>
                <td>
                    <label>总共获得积分</label></td>
                <td>
                    <label>总共消耗积分</label></td>
                <td>
                    <label>签到获得积分</label></td>
                <td>
                    <label>分享商品获得积分</label></td>
                <td>
                    <label>发展会员总共获得积分</label></td>
                <td>
                    <label>补签消耗积分</label></td>
                <td>
                    <label>下单消耗积分</label></td>
            </tr>
            <tr class="tr">
                <td class="curr_total_score"></td>
                <td class="sum_total_score"></td>
                <td class="delete_total_score"></td>
                <td class="signin_total_score"></td>
                <td class="share_total_score"></td>
                <td class="member_total_score"></td>
                <td class="retroactive_total_score"></td>
                <td class="order_total_score"></td>
            </tr>
        </table>

    </div>
    <%
        } 
    %>


    <div id="toolbar" class="pageTopBtnBg" style="padding: 10px 48px; height: auto;">
        <div style="margin-bottom: 5px">
            <span style="font-size: 12px; font-weight: normal">时间段：</span>
            <input class="easyui-datebox" id="start" />&nbsp;至
                <input class="easyui-datebox" id="stop" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">查询</a>
        </div>
    </div>

    <div class="wrapGrvData">
        <table id="grvData" fitcolumns="true"></table>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Serv/API/admin/User/Score/List.ashx";
        var userId = '<%=model.UserID%>';
        var type = '<%=Request["type"]%>';
        var col = "积分";
      
        $(function () {
            if (type != '') {
                col = "余额";
            }

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { user_id: userId, type: type },
	                rowStyler: function () { return 'height:25px'; },
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                height: document.documentElement.clientHeight - 140,
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[

                                { field: 'create_time', title: '时间', width: 50, align: 'left', formatter: FormatDate },
                                { field: 'score', title: '数值', width: 50, align: 'left' },
                                //{ field: 'curr_total_score', title:'当前'+col, width: 50, align: 'left' },
                                { field: 'add_note', title: '说明', width: 100, align: 'left' }
	                ]],
	                onLoadSuccess: function (data) {
	                    var result = data.score_info;
	                    if (result) {
	                        $('.curr_total_score').text(result.curr_total_score);
	                        $('.sum_total_score').text(result.sum_total_score);
	                        $('.delete_total_score').text(result.delete_total_score);
	                        $('.signin_total_score').text(result.signin_total_score);
	                        $('.retroactive_total_score').text(result.retroactive_total_score);
	                        $('.order_total_score').text(result.order_total_score);
	                        $('.share_total_score').text(result.share_total_score);
	                        $('.member_total_score').text(result.member_total_score);
	                    }
	                }

	            });





        });


        function Search() {

            var start = $('#start').datebox('getValue');
            var stop = $('#stop').datebox('getValue');
            $('#grvData').datagrid({
                url: handlerUrl,
                queryParams: {
                    start_time: start,
                    stop_time: stop,
                    user_id: userId,
                    type: type
                }
            });
        }
    </script>
</asp:Content>
