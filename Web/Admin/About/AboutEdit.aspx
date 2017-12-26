<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AboutEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.About.AboutEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span><%=model.ActivityName %></span>
    <a href="NewsList.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回法规列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                                <%=model.ActivityDescription%>
                            </div>
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td  style="width: 100px;" align="right" class="tdTitle">
                        阅读数:
                    </td>
                    <td>
                        <%=model.PV %>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
    <script src="/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="/static-modules/lib/layer/layer.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "Handler/AboutHandler.ashx",
            currAcvityID = '<%=model.JuActivityID %>',
            editor,
            $document = $(document);

        init();

        function init() {
            bindEvent();
        }

        function bindEvent() {

            $document.on('click', '#btnSave', function () {
                saveData();
            });
        }

        function saveData() {
            var $btnSave = $('#btnSave');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');

            var model = {
                Action:'EditAbout',
                JuActivityID: <% =model.JuActivityID %>,
                ActivityDescription: editor.html()
            };
            setTimeout(function () {
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnSave.removeClass('disabled').text('保存');
                        if (resp.Status == 1) {
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            }, 400);

        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'
                ],
                filterMode: false
            });
        });

    </script>
</asp:Content>
