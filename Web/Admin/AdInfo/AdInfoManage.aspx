<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AdInfoManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.AdInfo.AdInfoManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;图片管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>手机首页图片管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>

            <%
                if (!isHide)
                {
                    %>
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                            onclick="Delete()">删除</a>
                    <%
                }    
             %>
            
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 420px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>标题:
                </td>
                <td>
                    <input id="txtTitle" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>图片:
                </td>
                <td>
                    <img alt="缩略图" src="" width="210px" height="100px" id="imgThumbnailsPath" class="rounded" />
                    <br />
                    <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                    <br />
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式420*200图片。
                    <input type="file" id="txtThumbnailsPath" name="file1" class="hidden" />
                </td>
            </tr>
            <tr>
                <td>地址链接:
                </td>
                <td>
                    <input id="txtSiteUrl" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>排序:
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 250px;" class="easyui-numberbox" data-options="min:0,precision:0" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/AdInfoHandler.ashx",
            currSelectID = 0,
            currAction = '',
            $document = $(document),
            currTagType = '<%=Request["type"]%>';

            $(function () {
                $('#grvData').datagrid(
                      {
                          method: "Post",
                          url: handlerUrl,
                          queryParams: { Action: "QueryList", Type: currTagType },
                          height: document.documentElement.clientHeight - 112,
                          pagination: true,
                          striped: true,
                          pageSize: 50,
                          rownumbers: true,
                          rowStyler: function () { return 'height:25px'; },
                          columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            {
                                field: 'ImgUrl', title: '缩略图', width: 80, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                            },
                            { field: 'Title', title: '标题', width: 100, align: 'left' },
                            { field: 'SiteUrl', title: '链接', width: 120, align: 'left' },
                            { field: 'Sort', title: '排序', width: 20, align: 'left' }
                          ]]
                      }
                  );
                $('#dlgInput').dialog({
                    buttons: [{
                        text: '保存',
                        handler: function () {
                            try {
                                var dataModel = {
                                    Action: currAction,
                                    AutoId: currSelectID,
                                    Title: $.trim($('#txtTitle').val()),
                                    Type: currTagType,
                                    ImgUrl: $.trim($('#imgThumbnailsPath').attr("src")),
                                    SiteUrl: $.trim($('#txtSiteUrl').val()),
                                    Sort: $.trim($("#txtSort").numberbox("getValue"))
                                }
                                if (dataModel.Title == '') {
                                    Alert('请输入标题');
                                    return;
                                }
                                if (dataModel.ImgUrl == '') {
                                    Alert('请上传图片');
                                    return;
                                }
                                $.ajax({
                                    type: 'post',
                                    url: handlerUrl,
                                    data: dataModel,
                                    dataType: "json",
                                    success: function (resp) {
                                        if (resp.Status == 1) {
                                            Show(resp.Msg);
                                            $('#dlgInput').dialog('close');
                                            $('#grvData').datagrid('reload');
                                        } else if (resp.Status == 3) {
                                            Show(resp.Msg);
                                        } else {
                                            Alert(resp.Msg);

                                        }
                                    }
                                });

                            } catch (e) {
                                Alert(e);
                            }
                        }
                    }, {
                        text: '取消',
                        handler: function () {
                            $('#dlgInput').dialog('close');
                        }
                    }]
                });
                bindEvent();
            });

            function bindEvent() {
                $document.on('change', '#txtThumbnailsPath', function () {
                    try {
                        var ofileExtension = $("#txtThumbnailsPath").val();
                        ofileExtension = ofileExtension.substr(ofileExtension.lastIndexOf(".")).toLowerCase();
                        if (ofileExtension != ".jpg" && ofileExtension != ".png" && ofileExtension != ".gif") {
                            alert("请上传JPG、PNG和GIF格式图片");
                            return;
                        }
                        $.messager.progress({
                            text: '正在上传图片...'
                        });
                        $.ajaxFileUpload({
                            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                            secureuri: false,
                            fileElementId: 'txtThumbnailsPath',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    imgThumbnailsPath.src = resp.ExStr;

                                } else {
                                    alert(resp.Msg);
                                }
                            }
                        });

                    } catch (e) {
                        alert(e);
                    }
                });
            }

            function ShowAdd() {
                currAction = 'AddAdInfo';
                currSelectID = 0;
                $('#dlgInput').dialog({ title: '添加' });
                $('#dlgInput').dialog('open');
                $("#dlgInput input").val("");
                $('#txtSort').numberbox("setValue", 1);
                $('#imgThumbnailsPath').attr("src", "");
            }

            function Delete() {
                try {
                    var rows = $('#grvData').datagrid('getSelections');
                    if (!EGCheckIsSelect(rows))
                        return;

                    $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                        if (r) {
                            var ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].AutoId);
                            }
                            var dataModel = {
                                Action: 'DeleteAdInfo',
                                ids: ids.join(',')
                            }
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show("删除完成");
                                        $('#grvData').datagrid('reload');
                                    } else {
                                        Show("删除失败");
                                    }
                                }
                            });
                        }
                    });

                } catch (e) {
                    Alert(e);
                }
            }

            function ShowEdit() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckNoSelectMultiRow(rows))
                    return;

                currAction = 'UpdateAdInfo';
                currSelectID = rows[0].AutoId;

                $('#txtTitle').val(rows[0].Title);
                $('#imgThumbnailsPath').attr("src", rows[0].ImgUrl)
                $('#txtSiteUrl').val(rows[0].SiteUrl);
                $('#txtSort').numberbox("setValue", rows[0].Sort);

                $('#dlgInput').dialog({ title: '修改标签' });
                $('#dlgInput').dialog('open');
            }

    </script>
</asp:Content>

