<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="OpenConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Open.OpenConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="OpenList.aspx">公开课管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>公开课设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">课件创建人：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCreater" class="commonTxt" placeholder=""  value="<%=Creater %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">公开课公告：
                    </td>
                    <td width="*" align="left">
                        <textarea  id="txtOpenClassNotice" class="commonTxt" placeholder=""  rows="4"><%=OpenClassNotice %></textarea>
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
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/OpenHandler.ashx",
            currUserId = '<%=UserId %>';
        init();

        function init() {
            //处理初始化tags
            if (currUserId == '') {
                window.parent.location.href = "/login";
                return;
            }
            bindEvent();
        }

        
        function bindEvent() {
            $("#btnSave").on('click', function () {
                saveData();
            });
        }

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var model = {
                Action:'EditOpenConfig',
                Creater: $.trim($('#txtCreater').val()),
                OpenClassNotice: $.trim($('#txtOpenClassNotice').val())
            };
            setTimeout(function () {
                if (model.Creater == '') {
                    $('#txtCreater').focus();
                    alert('课件创建人不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }

                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnReset.removeClass('disabled');
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

    </script>
</asp:Content>
