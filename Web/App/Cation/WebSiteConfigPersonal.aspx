<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WebSiteConfigPersonal.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WebSiteConfigPersonal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <style type="text/css">
        /*fieldset*/
       .sort
        {
             /*width: 780px;*/
        }
        /*.input-group
        {
            width: 100%;
        }
        .input-group-addon
        {
            width: 150px;
        }*/
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;系统管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>系统设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <p>
            <a href="javascript:void(0)" id="btnSave" onclick="Save();" class="button button-rounded button-primary">
                保存</a>
        </p>
        <% ZentCloud.BLLJIMP.Model.WebsiteInfo currWebsiteInfo = ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel(); %>
        <hr style="border: 1px dotted #036;" />
        <fieldset>
            <legend>基本配置</legend>
            <div class="input-group">
                <span class="input-group-addon">站点名称</span>
                <input id="txtWebsiteName" type="text" class="form-control" placeholder="站点名称" />
            </div>
           <div class="input-group">
                <span class="input-group-addon">站点Logo</span>
                <img src="/img/index/logo.png" id="imglogo" style="width:180px;height:60px;"/>
                <a id="auploadThumbnails" title="点击上传图片" href="javascript:;" class="easyui-linkbutton" plain="true" onclick="txtThumbnailsPath.click()">上传Logo</a>(建议图片尺寸为180px*60px)

                 <input type="file" id="txtThumbnailsPath" name="file1" style="display:none;" />
            </div>


            <div class="input-group">
                <span class="input-group-addon">文章活动详细页顶部代码</span>
                <textarea rows="2" id="txtArticleHeadCode" class="form-control" placeholder="文章活动详细页顶部代码"></textarea>
            </div>
           <div class="input-group">
               <span class="input-group-addon">文章活动详细页底部代码</span>
               <textarea rows="2" id="txtArticleBottomCode" class="form-control" placeholder="文章活动详细页底部代码"></textarea>
            </div>
            <br />
            <div class="input-group">
                <label>
                <input id="chkIsHideAdminLogoAndTop" type="checkbox" />
                    隐藏后台 logo及 顶部
                </label>
            </div>

        </fieldset>
       
       
       
      
       
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">
                        系统提示</h4>
                </div>
                <div class="modal-body">
                    <label id="lbMsg">
                    </label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        关闭</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    $(function () {

        if ($.browser.msie) { //ie 下
            //缩略图
            $("#auploadThumbnails").hide();
            $("#txtThumbnailsPath").show();

        }
        else {
            $("#txtThumbnailsPath").hide(); //缩略图
        }

        $("#txtThumbnailsPath").live('change', function () {
            try {
                $.messager.progress({ text: '正在上传图片...' });
                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 imglogo.src = resp.ExStr;

                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });


    });
    
    </script>
    <script type="text/javascript" src="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {
            ShowConfigData();
        });

        function Save() {
            var dataModel = {
                Action: 'UpdateWebSiteInfoPersonal',
                WebsiteName: $.trim($(txtWebsiteName).val()),
                ArticleHeadCode: $.trim($(txtArticleHeadCode).val()),
                ArticleBottomCode: $.trim($(txtArticleBottomCode).val()),
                WebsiteLogo: $("#imglogo").attr("src"),
                IsHideAdminLogoAndTop:$('#chkIsHideAdminLogoAndTop').get(0).checked? 1:0
            }

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType: "json",
                success: function (resp) {
                    ShowMsg(resp.Msg);
                }
            });

            //ShowMsg("添加成功!");
        }

        function ShowConfigData() {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'QueryWebSiteInfo' },
                dataType: "json",
                success: function (resp) {
                    $('#chkIsHideAdminLogoAndTop').attr("checked", resp.IsHideAdminLogoAndTop == 1);
                    $('#txtWebsiteName').val(resp.WebsiteName);
                    $("#txtArticleHeadCode").val(resp.ArticleHeadCode);
                    $("#txtArticleBottomCode").val(resp.ArticleBottomCode);
                    $("#imglogo").attr("src", resp.WebsiteLogo);

                }
            });
        }

        function ShowMsg(msg) {
            //            $('#myModal').modal('show');
            //            $('#lbMsg').html(msg);
            Alert(msg);
        }

    </script>
</asp:Content>

