<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXLotteryMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=Request.Url.Host %>';
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLottery" },
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
                    singleSelect:true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ThumbnailsPath', title: '缩略图', width: 10, align: 'center', formatter: function (value,rowData) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img onclick="ShowQRcode(\'{0}\')" class="imgAlign" src="{1}" title="缩略图" height="50" width="50" />', rowData.AutoID, rowData.ThumbnailsPath);
                                    return str.ToString();
                                }
                                },
                                { field: 'LotteryName', title: '刮奖活动名称', width: 40, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a onclick="ShowQRcode(\'{0}\')" title="{1}">{1}</a>', rowData.AutoID, rowData.LotteryName);
                   return str.ToString();          
                                
                                
                 } },
                                { field: 'Status', title: '状态', width: 10, align: 'left', formatter: function (value) {
                                    if (value == 1) {
                                        return '<span style="color:green">进行中</span>';
                                    }
                                    else {
                                        return '<span style="color:red">已停止</span>';
                                    }
                                } 
                                },
                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'op', title: '操作', width: 25, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">二维码</a>|&nbsp;<a href="/App/Lottery/WXLotteryCompile.aspx?Action=edit&id={0}" title="点击编辑" >编辑</a>|&nbsp;<a href="/App/Lottery/WXLotteryRecords.aspx?id={0}" title="点击查看中奖名单">查看中奖名单</a>', rowData.AutoID);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );


        });






        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWXLottery", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            //
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $('#grvData').datagrid('reload');
                                Show(resp.Msg);

                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }

                    });
                }
            });


        }

        //重置刮奖
        function Reset() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定重置所选刮奖?刮奖记录及中奖记录将会被清除", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "ResetWXLottery", ids: GetRowsIds(rows).join(',') },
                        success: function (resp) {
                            Alert(resp);
                        }

                    });
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


        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLottery", LotteryName: $("#txtName").val()}
	            });
	        }

	    

          function ShowQRcode(id) {
            //dlgSHowQRCode
              $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Lottery/wap/Scratch.aspx?id='+id);
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/Lottery/wap/Scratch.aspx?id="+id;
            $("#alinkurl").html(linkurl).attr("href", linkurl);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>所有刮奖活动</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="/App/Lottery/WXLotteryCompile.aspx?Action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
               id="btnAdd">新建刮奖活动</a>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="Reset()">重置刮奖</a>
                   
            <br />
            
                        活动名称:<input id="txtName" style="width:200px;" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
         <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>