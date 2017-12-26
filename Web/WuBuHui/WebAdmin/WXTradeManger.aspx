<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXTradeManger.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.WXTradeManger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
        var domain = '<%=Request.Url.Host %>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTradeInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '编号', width: 10, align: 'left' },
                                { field: 'CategoryName', title: '名称', width: 10, align: 'left' },
                                { field: 'CategoryType', title: '类别', width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (value == "trade") {
                                        return "行业";
                                    } else if (value == "word") {
                                        return "话题标签";
                                    } else if (value == "Partner") {
                                        return "五伴会分类";
                                    } else {
                                        return "专业";
                                    }
                                }
                                },
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
                        data: { Action: "DelTradeInfo", ids: GetRowsIds(rows).join(',') },
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
                ids.push(rows[i].AutoID);
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

        var aID = 0;
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
            var QueryNum = $("#txtName").val();
            var type = $("#strade").val();
            $.post(handlerUrl, { Action: "UTradeInfo", cName: QueryNum, AutoId: aID, CategoryType: type }, function (data) {
                var resp3 = $.parseJSON(data);
                if (resp3.Status == 0) {
                    $("#dlgPmsInfo").window("close");
                    $('#grvData').datagrid('reload');
                    Show(resp3.Msg);
                } else {
                    Alert(resp3.Msg);
                }
            });
        });
        function OnSave() {
            aID = 0;
            $("#txtName").val("");
            $('#dlgPmsInfo').window(
            {
                title: '添加行业名称'
            }
            );
            $('#dlgPmsInfo').dialog('open');
        }

        $("#btnClose").live("click", function () {
            $("#dlgPmsInfo").window("close");
        });

        $("#BtnUpload").live("click", function () {
            $.ajaxFileUpload(
                     {
                         url: handlerUrl + '?action=UploadCodeInfoData',
                         secureuri: false,
                         fileElementId: 'uploadify',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             $("#dlgPmsInfo").window("close");
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
            $("#txtName").val(rows[0].CategoryName);
            aID = rows[0].AutoID;
            $('#dlgPmsInfo').window(
            {
                title: '添加行业名称'
            }
            );
            $('#dlgPmsInfo').dialog('open');
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
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>行业列表</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="OnSave()">添加</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="OnEdit()">修改</a><a href="javascript:void(0)"
                        class="easyui-linkbutton" iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px;
        height: 185px; padding: 10px">
        <table>
            <tr>
                <td height="25" align="left">
                    名称：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtName" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">
                    专业名称：
                </td>
                <td height="25" width="*" align="left">
                    <select id="strade">
                        <option value="trade">行业</option>
                        <option value="Professional">专业</option>
                        <option value="word">话题标签</option>
                        <option value="Partner">公司行业</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)"
                        id="btnExit" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
