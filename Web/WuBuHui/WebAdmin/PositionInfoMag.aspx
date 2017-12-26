<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="PositionInfoMag.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.PositionInfoMag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiPosintionHandler.ashx";
        var domain = '<%=Request.Url.Host %>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetPostitionInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Title', title: '名称', width: 10, align: 'left' },
                                { field: 'IocnImg', title: '图片', width: 10, align: 'left', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'Address', title: '地址', width: 10, align: 'left' },
                                { field: 'SalaryRange', title: '薪资范围', width: 10, align: 'left' },
                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'OP', title: '申请记录', width: 20, align: 'left', formatter: function (value,rowData) {

                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="ApplyPostionRecord.aspx?id={0}">查看申请记录</a>', rowData.AutoId);
                                    return str.ToString();

                                
                                
                                } }
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
                        data: { Action: "DelPositionInfo", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 0) {
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
        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoId);
            }
            return ids;
        }

        function ConfigBarCodeInfoEdit() {
            $('#dlgPmsInfo').window(
            {
                title: '管理配置返回结果'
            }
            );

            $.post(handlerUrl, { Action: "GetConfigureConfigInfo" }, function (data) {
                var resp2 = $.parseJSON(data);
                if (resp2.Status == 0) {
                    $("#txtQueryNum").val(resp2.ExObj.QueryNum);
                    $("#txtPopupInfoon").val(resp2.ExObj.PopupInfo);
                }
                $('#dlgPmsInfo').dialog('open');
            });
        }

        function Search() {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTheVoteInfos", VoteName: $("#txtName").val() }
	            });
        }

        //窗体关闭按钮---------------------
        $("#btnExit").live("click", function () {
            $("#dlgPmsInfo").window("close");
        });

        $("#btnSave").live("click", function () {
            var QueryNum = $("#txtQueryNum").val();
            var PopupInfoon = $("#txtPopupInfoon").val();
            $.post(handlerUrl, { Action: "ConfigureConfigInfo", QueryNum: QueryNum, PopupInfo: PopupInfoon }, function (data) {
                var resp3 = $.parseJSON(data);
                if (resp3.Status = 0) {
                    Show(resp3.Msg);
                } else {
                    Alert(resp3.Msg);
                }
            });
        });
        function OnUpload() {
            $('#UploadDiv').window(
            {
                title: '上传文件'
            }
            );
            $('#UploadDiv').dialog('open');
        }

        $("#btnClose").live("click", function () {
            $("#UploadDiv").window("close");
        });

        $("#BtnUpload").live("click", function () {
            $.ajaxFileUpload(
                     {
                         url: handlerUrl + '?action=UploadCodeInfoData',
                         secureuri: false,
                         fileElementId: 'uploadify',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             $("#UploadDiv").window("close");
                             $('#grvData').datagrid('reload');
                             show(resp.Msg)
                         }
                     }
                    );
        });
        function OnEdit() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            if (rows.length > 1) {
                Alert("只能选择一行数据");
            }
            //            alert(GetRowsIds(rows).join(','));
            window.location.href = "PositionInfoAddU.aspx?AutoId=" + GetRowsIds(rows).join(',');
        }
        function ShowQRcode(id) {
            //dlgSHowQRCode
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=' + id);
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=" + id;
            $("#alinkurl").html(linkurl).attr("href", linkurl);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>职位管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="PositionInfoAddU.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">
                添加</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit"
                    plain="true" onclick="OnEdit()">修改</a><a href="javascript:void(0)" class="easyui-linkbutton"
                        iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
