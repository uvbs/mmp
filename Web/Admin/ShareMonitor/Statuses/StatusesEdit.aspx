<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="StatusesEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statuses.StatusesEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="StatusesList.aspx">社区管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=Request["id"]=="0"?"新增":"编辑" %>社区</span>
    <a href="StatusesList.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回社区列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCategoryName" class="commonTxt" placeholder="名称(必填)"  value="<%=model.CategoryName %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" class="commonTxt"  value="<%=model.Summary %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=model.ImgSrc %>" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*750。
                        <input type="file" id="txtThumbnailsPath" name="file1" class="hidden"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        创建时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datebox" data-options="editable:false" style="width: 150px;" id="txtCreateTime" value="<% = model.CreateTime < DateTime.Parse("1900-01-01") ? DateTime.Now.ToString("yyyy-MM-dd") :model.CreateTime.ToString("yyyy-MM-dd") %>" />
                        必填 点击左侧图标选择时间
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        排序:
                    </td>
                    <td>
                        <input id="txtSort" type="text" style="width:250px;" value="<%=model.Sort %>" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>
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
        var handlerUrl = "Handler/StatusesHandler.ashx",
            $document = $(document),
            currAutoID = <% =model.AutoID %>;

        init();

        function init() {
            if(currAutoID != 0){
                $('#btnReset').hide();
            }
            bindEvent();
            
        }
        function bindEvent() {

            $document.on('click', '#btnSave', function () {
                saveData();
            });

            $document.on('click', '#btnReset', function () { ResetCurr(); });
            
            $document.on('change', '#txtThumbnailsPath', function () {
                try {
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

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var model = {
                Action:'EditStatuses',
                AutoID: <% =model.AutoID %>,
                CategoryName: $.trim($('#txtCategoryName').val()),
                preId:'0',
                Summary: $("#txtSummary").val(),
                ImgSrc: $("#imgThumbnailsPath").attr("src"),
                CreateTime: $("#txtCreateTime").val(),
                Sort:$("#txtSort").val()
            };
            setTimeout(function () {
                if (model.CategoryName == '') {
                    $('#txtCategoryName').focus();
                    alert('标题不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                if(model.ImgSrc == ''){
                    alert('请上传缩略图', 3);
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
                            if (model.AutoID == '0')
                            { 
                                ResetCurr();
                            }
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            }, 400);

        }

        function ResetCurr() {
            ClearAll();
        }

    </script>
</asp:Content>
