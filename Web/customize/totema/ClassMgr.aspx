<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ClassMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.ClassMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;班级管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a> <a href="javascript:history.go(-1);" class="easyui-linkbutton"
                    iconcls="icon-back" plain="true" style="float: right; margin-right: 20px;">返回</a>
            <br />
            审核状态:
            <select id="ddlStatus">
                <option value="">全部</option>
                <option value="0">等待审核</option>
                <option value="1">审核通过</option>
            </select>
            <span style="font-size: 12px; font-weight: normal">班级名称：</span>
            <input type="text" style="width: 200px" id="txtClassName" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">
                查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 420px; height: 330px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var domain = '<%=Request.Url.Host%>';
        var voteId = '116';
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var OrderBy = " Order by Status ASC,VoteCount DESC";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteObjectInfo", VoteID: voteId, OrderBy: OrderBy },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Status', title: '审核状态', width: 10, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    switch (value) {
                                        case 1:
                                            return "<font color='green'>审核通过</font>";
                                            break;
                                        default:
                                            return "<font color='red'>等待审核</font>";
                                            break;

                                    }
                                }
                                },
                                { field: 'Number', title: '编号', width: 5, align: 'left' },
                                { field: 'VoteObjectHeadImage', title: '班级图片', width: 10, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="80" width="80" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'VoteObjectName', title: '班级名称', width: 20, align: 'left' },
                                { field: 'VoteCount', title: '票数', width: 10, align: 'left' },
                                { field: 'Op', title: '操作', width: 20, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="ClassDetail.aspx?aid={0}" >[查看]</a>', rowData['AutoID'], rowData['VoteID']);
                                    //str.AppendFormat('&nbsp;&nbsp;<a href="javascript:void(0)" onclick="ShowQRcode({0})">[二维码]</a>', rowData['AutoID']);
                                    return str.ToString();
                                }
                                }

                             ]]
	            }
            );

            $("#btnSearch").click(function () {
                var VoteObjectName = $("#txtClassName").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryVoteObjectInfo", VoteID: voteId, VoteObjectName: VoteObjectName, OrderBy: OrderBy, Status: $("#ddlStatus").val()} });
            });

        })


        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoID);
                        }

                        var dataModel = {
                            Action: 'DeleteVoteObjectInfo',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                Alert(result);
                                $('#grvData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowQRcode(aid) {
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Cation/Wap/Vote/VoteObjectDetail.aspx?id=' + aid);
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/Cation/Wap/Vote/VoteObjectDetail.aspx?id=" + aid;
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl);
        }
    </script>
</asp:Content>
