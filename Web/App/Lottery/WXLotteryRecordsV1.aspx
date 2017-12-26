<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXLotteryRecordsV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryRecordsV1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&gt;&nbsp;<span>中奖结果</span> <a href="javascript:void(0);"
        class="easyui-linkbutton" iconcls="icon-back" plain="true" style="float: right;
        margin-right: 50px;" onclick="goback()">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
            onclick="UpdateIsGetPrize()">标记为已领奖</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
            onclick="ExportData()">导出中奖数据</a>
        <div id="data" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        
        <div style="margin-bottom: 5px">
            奖品&nbsp;<select id="ddlawards" onchange="Search()">
                <option value="">全部</option>
                <%foreach (var item in AwardList)
                  {
                      Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.AutoID, item.PrizeName));
                  } %>
            </select>
            <input id="txtUserId" style="width: 200px; display: none;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();" style="display: none;">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var lotteryid = '<%=Request["id"]%>';
        var backUrl = '<%=Request["backUrl"]%>';
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLotteryRecordV1", LotteryId: lotteryid },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'WXAwardName', title: '奖品', width: 20, align: 'left' },
                                { field: 'HeadImg', title: '头像', width: 10, align: 'left', formatter: function (value, rowData) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();

                                }
                                },
                                { field: 'WXNickName', title: '昵称', width: 20, align: 'left' },
                                { field: 'InsertDate', title: '中奖时间', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'op', title: '是否已领奖', width: 20, align: 'left', formatter: function (value, rowData) {
                                    if (rowData.IsGetPrize == 1) {
                                        return "<font color='grean'>已领奖</font>";
                                    }
                                    else {
                                        return "<font color='red'>未领奖</font>";
                                    }

                                }
                                },
                                {
                                    field: 'Name', title: '姓名', width: 20, align: 'left'
                                },
                               {
                                   field: 'Phone', title: '手机', width: 20, align: 'left'
                               }


                               // { field: 'Name', title: '姓名', width: 20, align: 'left',
                               //     formatter: function (value, rowData) {

                               //         if (value == ""||value==null) {
                               //             return rowData.UserInfo.TrueName;
                               //         }
                               //         return rowData.Name;

                               //     }
                               // },
                               //{ field: 'Phone', title: '手机', width: 20, align: 'left',
                               //    formatter: function (value, rowData) {
                               //        if (value == ""||value==null) {
                               //            return rowData.UserInfo.Phone;
                               //        }
                               //        return rowData.Phone;

                               //    }
                               //}


                             ]]
	            }
            );


        });


        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLotteryRecordV1", LotteryId: lotteryid, UserId: $("#txtUserId").val(), AwardId: $("#ddlawards").val() }
	            });
        }

        function UpdateIsGetPrize() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateIsGetPrize", ids: GetRowsIds(rows).join(',') },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        $('#grvData').datagrid('reload');
                    }
                    else {
                        Alert(resp.Msg);
                    }
                }
            });
        }

        //导出中奖数据
        function ExportData() {
            $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                if (o) {
                    window.open('/Serv/API/Admin/Lottery/WinningData/Export.ashx?LotteryID=' + lotteryid);
                }
            });


        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID
                 );
            }
            return ids;
        }

        function goback() {
            if(!backUrl){
                window.location.href = '/App/Lottery/WXLotteryMgrV1.aspx';
            } else {
                window.location.href = backUrl;
            }
        }


    </script>
</asp:Content>
