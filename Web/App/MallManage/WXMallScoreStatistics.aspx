<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallScoreStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallScoreStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body > div:nth-child(1) {
            border: 0;
        }

        .warpTitle {
            text-align: center;
            font-size: 20px;
            padding: 10px;
        }

        .wrapGrvData {
            margin: 0 16px;
            border-right: 1px solid #ddd;
            border-radius: 2px;
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
            margin-top: 10px;
        }

        .truename {
            font-size: 20px;
            color: #FF1515;
            margin-right: 5px;
        }

        .warp-data {
            margin: 0px 16px;
            background: #E0E0E0;
            margin-top: 20px;
        }

        .tr td {
            font-size: 18px;
            max-width: 100%;
            margin-bottom: 5px;
            font-weight: 700;
            color: #5C5566;
        }

        .tb {
            height: 100px;
            border-collapse: collapse;
            text-align: center;
            border-color: #fff;
            width: 100%;
        }

        .Text {
            width: 200px;
            display: inline;
        }
        .blue {
            color:blue;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div class="warpTitle"><span class="">积分统计</span></div>

    <div class="pageSubTitle"><span class="mainText">详细数据</span></div>

    <div class="warp-data">
        <table class="tb" border="1">
            <tr>
                <%
                    if (Request.Url.Host.IndexOf("songhebao") < 0)
                    {
                %>
                <td>
                    <label>总共获得积分</label></td>
                <td>
                    <label>总共消耗积分</label></td>
                <%
                    }     
                %>

                <td>
                    <label>发展会员总共获得积分</label></td>
                <td>
                    <label>分享商品获得积分</label></td>
                <td>
                    <label>签到获得积分</label></td>
                <td>
                    <label>下单交易成功获得积分</label></td>
                <td>
                    <label>补签消耗积分</label></td>
                <td>
                    <label>下单消耗积分</label></td>
                
            </tr>
            <tr class="tr">
                <%
                    if (Request.Url.Host.IndexOf("songhebao") < 0)
                    {
                %>
                <td class="sum_total_score"></td>
                <td class="delete_total_score"></td>
                <%
                    }     
                %>
                <td class="member_total_score"></td>
                <td class="share_total_score"></td>
                <td class="signin_total_score"></td>
                <td class="order_success_total_score"></td>
                <td class="retroactive_total_score"></td>
                <td class="order_total_score"></td>
            </tr>
        </table>

    </div>

    <div id="toolbar" class="pageTopBtnBg" style="padding: 10px 20px; height: auto;">
        <div style="margin-bottom: 5px">
            <span style="font-size: 12px; font-weight: normal">时间段：</span>
            <input class="easyui-datetimebox" id="start" />&nbsp;至
                <input class="easyui-datetimebox" id="stop" />
            关键字：<input type="text" id="txtKey" class="form-control Text" placeholder="请选择会员" />
            <a href="javascript:;" style="color: blue;" onclick="ClearKey()">清除</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">查询</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="Export();" id="btnEdit">加入导出任务</a>

            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-excel" plain="true"  id="btnDownLoad">下载</a>
        </div>
    </div>

    <div class="wrapGrvData">
        <table id="grvData" fitcolumns="true"></table>
    </div>

    <div id="userDialog" class="easyui-dialog" closed="true" modal="true" title="会员列表" style="width: 500px; padding: 15px;">
        <p style="padding: 10px;">
            关键字：<input type="text" id="txtTrueNameValue" class="form-control Text" placeholder="微信昵称、姓名">
            <a class="easyui-linkbutton" iconcls="icon-search" onclick="SearchUser()">搜索</a>
        </p>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
        <div id="dlgTask" class="easyui-dialog" closed="true" modal="true" title="任务列表" style="width: 700px; padding: 15px;">
        
        <table id="grvTask" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Serv/API/admin/User/Score/List.ashx";
        var userListUrl = "";

        var userNme = '';
        var userId = '';
        $(function () {
            $('#grvData').datagrid(
                {
                    method: "Post",
                    url: handlerUrl,
                    queryParams: { user_id: '', type: '' },
                    rowStyler: function () { return 'height:25px'; },
                    pagination: true,
                    striped: true,
                    pageSize: 10,
                    height: document.documentElement.clientHeight - 140,
                    rownumbers: true,
                    singleSelect: true,
                    columns: [[

                                { field: 'create_time', title: '时间', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'true_name', title: '用户', width: 20, align: 'left' },
                                { field: 'score', title: '数值', width: 20, align: 'left' },
                                { field: 'add_note', title: '说明', width: 20, align: 'left' }
                    ]],
                    onLoadSuccess: function (data) {
                        var result = data.score_info;
                        if (result) {
                            $('.sum_total_score').text(result.sum_total_score);
                            $('.delete_total_score').text(result.delete_total_score);
                            $('.signin_total_score').text(result.signin_total_score);
                            $('.retroactive_total_score').text(result.retroactive_total_score);
                            $('.order_total_score').text(result.order_total_score);
                            $('.share_total_score').text(result.share_total_score);
                            $('.member_total_score').text(result.member_total_score);
                            $('.order_success_total_score').text(result.order_success_total_score);
                        }
                    }

                });

            //单击选择会员文本框
            $('#txtKey').click(function () {
                $('#userDialog').dialog('open');
                $("#userDialog").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });
                $('#grvUserInfo').datagrid(
                  {
                      loadMsg: "正在加载数据",
                      method: "Post",
                      url: '/serv/api/admin/member/list.ashx',
                      pagination: true,
                      loadFilter: pagerFilter,
                      striped: true,
                      pageSize: 10,
                      rownumbers: true,
                      singleSelect: true,
                      height: 380,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[

                                  { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                      ]]

                  }
              );

            });

            //单击选择会员文本框
            $('#btnDownLoad').click(function () {
                $('#dlgTask').dialog('open');
                $("#dlgTask").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });
                $('#grvTask').datagrid(
                  {
                      loadMsg: "正在加载数据",
                      method: "Post",
                      url: '/handler/app/cationhandler.ashx',
                      queryParams: { Action: 'GetTimingTasks', task_type: '15' },
                      pagination: true,
                      striped: true,
                      pageSize: 10,
                      rownumbers: true,
                      singleSelect: true,
                      height: 380,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[

                                  { field: 'InsertDateString', title: '任务时间', width: 20, align: 'left', formatter: FormatterTitle },
                                  { field: 'StatusString', title: '任务状态', width: 10, align: 'left', formatter: FormatterTitle },
                                  { field: 'FromDate', title: '统计开始时间', width: 20, align: 'left', formatter: FormatDate },
                                  { field: 'ToDate', title: '统计结束时间', width: 20, align: 'left', formatter: FormatDate },
                                  { field: 'Title', title: '用户', width: 20, align: 'left' },
                                  {
                                      field: 'Url', title: '文件', width: 10, align: 'left', formatter: function (value) {

                                          if (value==""||value==null) {

                                              return "";
                                          }
                                          var str = new StringBuilder();
                                          str.AppendFormat('<a href="{0}" target="_blank" class="blue">下载</a>', value);
                                          return str.ToString();

                                      }
                                  },
                      ]]

                  }
              );

            });

            //会员列表dialog
            $('#userDialog').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvUserInfo').datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;                        if (!EGCheckNoSelectMultiRow(rows))
                            return;                        $('#txtKey').val(rows[0].WXNickname);
                        userId = rows[0].UserID;
                        if (userId) {
                            $("#userDialog").dialog('close');
                        }


                    }
                },
                {
                    text: '取消',
                    handler: function () {
                        $("#userDialog").dialog('close');
                    }
                }]
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
                    type: ''
                }
            });
        }


        ///查询会员
        function SearchUser() {
            var txtTrueName = $('#txtTrueNameValue').val();
            $('#grvUserInfo').datagrid(                {
                    url: "/serv/api/admin/member/list.ashx",                    queryParams: {
                        KeyWord: txtTrueName,
                        mapping_type: 1,
                        isOrAnd: 1
                    }
                });
        }

        //清除筛选条件
        function ClearKey() {
            $('#txtKey').val('');
            $('#start').datetimebox('setValue', '');
            $('#stop').datetimebox('setValue', '');
        }

        //导出
        function Export() {

            var startTime = $("#start").datebox('getValue');
            var endTime = $("#stop").datebox('getValue');
            if (startTime == "" && endTime==""&&userId=="") {
                Alert("请选择时间或会员");
                return false;
            }
            $.messager.confirm('系统提示', '确认将当前条件加入到任务? ', function (o) {
                if (o) {
                    
                    
                    $.ajax({
                        type: 'post',
                        url: "/handler/app/cationhandler.ashx",
                        data: { action: "AddScoreStatisticsTask", start_time: startTime, end_time: endTime, user_id: userId },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.IsSuccess) {
                                Alert("已加入任务,结果可在稍后下载");
                            }
                            else {
                                Alert(resp.Msg);
                            }


                        }
                    });
                    //var start = $("#start").datebox('getValue');
                    //var end = $("#stop").datebox('getValue');
                    //var zurl = "/serv/api/admin/user/score/export.ashx?start_time=" + start + "&stop_time=" + end + "&user_id=" + userId + "&type=";
                    //window.open(zurl);


                }
            });
        }

    </script>


</asp:Content>
