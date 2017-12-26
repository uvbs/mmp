<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WBHOpinionInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.WBHOpinionInfoMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiPartnerHandler.ashx";
        var domain = '<%=Request.Url.Host %>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetOpinionInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserName', title: '用户名', width: 10, align: 'left' },
                                { field: 'Otype', title: '类型', width: 10, align: 'left' },
                                { field: 'OContext', title: '内容', width: 10, align: 'left' },
                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate }
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
                        data: { Action: "DelOpinionInfo", ids: GetRowsIds(rows).join(',') },
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
            window.location.href = "WXWBHBannaImgInfo.aspx?AutoId=" + GetRowsIds(rows).join(',');
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
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>投诉意见</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" onclick="Delete()"
                plain="true">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
