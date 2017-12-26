<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="IndustryTemplateCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.IndustryTemplateCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <style type="text/css">
        /*fieldset*/
        .sort
        {
            width: 780px;
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
    当前位置：&nbsp;<span><%=actionStr%>模板</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <p>
            <a href="javascript:;" id="btnSave"  class="button button-rounded button-primary">
                保存</a>
                <a href="IndustryTemplateManage.aspx" style="float:right;" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
            
        </p>
       
        <hr style="border: 1px dotted #036;" />
        <fieldset>
            <legend>基本配置</legend>
            <div class="input-group">
                <span class="input-group-addon">模板名称</span>
                <input id="txtTemplateName" type="text" value="<%=CurrTemplateModel.IndustryTemplateName%>" class="form-control" placeholder="模板名称" />
            </div>
        </fieldset>
        <fieldset>
            <legend>菜单配置</legend>
            <div class="input-group">
                <span class="input-group-addon">菜单一</span>
                
                <input id="txtCourseManageMenuRName" value="<%=CurrTemplateModel.CourseManageMenuRName%>" type="text" class="form-control" placeholder="课程管理 (如果留空则不会显示菜单)" />
                 
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单二</span>
                
                <input id="txtActivityManageMenuRName" value="<%=CurrTemplateModel.ActivityManageMenuRName%>" type="text" class="form-control" placeholder="活动管理 (如果留空则不会显示菜单)" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单三</span>
               
                <input id="txtArticleManageMenuRName" value="<%=CurrTemplateModel.ArticleManageMenuRName%>" type="text" class="form-control" placeholder="文章管理 (如果留空则不会显示菜单)" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单四</span>
               
                <input id="txtMasterManageMenuRName" value="<%=CurrTemplateModel.MasterManageMenuRName%>" type="text" class="form-control" placeholder="专家库管理 (如果留空则不会显示菜单)" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单五</span>
               
                <input id="txtQuestionManageMenuRName" value="<%=CurrTemplateModel.QuestionManageMenuRName%>" type="text" class="form-control" placeholder="问答管理 (如果留空则不会显示菜单)" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单六</span>
                
                <input id="txtUserManageMenuRName" value="<%=CurrTemplateModel.UserManageMenuRName%>" type="text" class="form-control" placeholder="用户管理 (如果留空则不会显示菜单)" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单七</span>
               
                <input id="txtSignUpCourseMenuRName" value="<%=CurrTemplateModel.SignUpCourseMenuRName%>" type="text" class="form-control" placeholder="报名管理 (如果留空则不会显示菜单)" />
                
            </div>

              <div class="input-group">
                <span class="input-group-addon">菜单八</span>
               
                <input id="txtMallMenuRName" value="<%=CurrTemplateModel.MallMenuRName%>" type="text" class="form-control" placeholder="在线商城 (如果留空则不会显示菜单)" />
                
            </div>

            <div class="input-group">
                <span class="input-group-addon">菜单九</span>
               
                <input id="txtWebSiteStatisticsMenuRName" value="<%=CurrTemplateModel.WebSiteStatisticsMenuRName%>" type="text" class="form-control" placeholder="网站统计 (如果留空则不会显示菜单)" />
                
            </div>
                        <div class="input-group">
                <span class="input-group-addon">菜单十</span>
               
                <input id="txtAddVMenuRName" value="<%=CurrTemplateModel.AddVMenuRName%>" type="text" class="form-control" placeholder="微信加V(如果留空则不会显示菜单)" />
                
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend>课程分类配置</legend>

             <div class="input-group">
                <span class="input-group-addon">分类一</span>
               
                <input id="txtCourseCate1" value="<%=CurrTemplateModel.CourseCate1%>" type="text" class="form-control" placeholder="精彩课程回放" />
                
            </div>

            <div class="input-group">
                <span class="input-group-addon">分类二</span>
               
                <input id="txtCourseCate2" value="<%=CurrTemplateModel.CourseCate2%>" type="text" class="form-control" placeholder="课程预告" />
                
            </div>

           

        </fieldset>
        <br />
        <fieldset>
            <legend>文章分类配置</legend>
            <div class="input-group">
                <span class="input-group-addon">分类一</span>
                
                <input id="txtArticleCate1" value="<%=CurrTemplateModel.ArticleCate1%>" type="text" class="form-control" placeholder="月度成果分享" />
               
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类二</span>
               
                <input id="txtArticleCate2" value="<%=CurrTemplateModel.ArticleCate2%>" type="text" class="form-control" placeholder="老板点评分享" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类三</span>
               
                <input id="txtArticleCate3" value="<%=CurrTemplateModel.ArticleCate3%>" type="text" class="form-control" placeholder="实用资源分享" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类四</span>
               
                <input id="txtArticleCate4" value="<%=CurrTemplateModel.ArticleCate4%>" type="text" class="form-control" placeholder="成功案例分享" />
                
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类五</span>
               
                <input id="txtArticleCate5" value="<%=CurrTemplateModel.ArticleCate5%>" type="text" class="form-control" placeholder="每周感悟分享" />
               
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
    <script type="text/javascript" src="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        $(function () {

            $('#btnSave').click(function () {
                try {
                    var model = {
                        Action: currAction == 'add' ? 'AddIndustryTemplate' : 'EditIndustryTemplate',
                        AutoID: '<%=CurrTemplateModel.AutoID %>',
                        IndustryTemplateName: $.trim($(txtTemplateName).val()),
                        CourseManageMenuRName: $.trim($(txtCourseManageMenuRName).val()),
                        ArticleManageMenuRName: $.trim($(txtArticleManageMenuRName).val()),
                        MasterManageMenuRName: $.trim($(txtMasterManageMenuRName).val()),
                        QuestionManageMenuRName: $.trim($(txtQuestionManageMenuRName).val()),
                        UserManageMenuRName: $.trim($(txtUserManageMenuRName).val()),
                        SignUpCourseMenuRName: $.trim($(txtSignUpCourseMenuRName).val()), //txtActivityManageMenuRName
                        ActivityManageMenuRName: $.trim($(txtActivityManageMenuRName).val()),
                        WebSiteStatisticsMenuRName: $.trim($(txtWebSiteStatisticsMenuRName).val()),
                        MallMenuRName: $.trim($(txtMallMenuRName).val()),
                        CourseCate1: $.trim($(txtCourseCate1).val()),
                        CourseCate2: $.trim($(txtCourseCate2).val()),
                        ArticleCate1: $.trim($(txtArticleCate1).val()),
                        ArticleCate2: $.trim($(txtArticleCate2).val()),
                        ArticleCate3: $.trim($(txtArticleCate3).val()),
                        ArticleCate4: $.trim($(txtArticleCate4).val()),
                        ArticleCate5: $.trim($(txtArticleCate5).val()),
                        AddVMenuRName: $.trim($(txtAddVMenuRName).val())
                    }

                    if (model.IndustryTemplateName == '') {
                        $('#txtTemplateName').focus();
                        ShowMsg("请输入模板名称");
                        return;
                    }

                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            $.messager.progress('close');
                            var resp = $.parseJSON(result);
                            ShowMsg(resp.Msg);
                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });


        });



       


        function ShowMsg(msg) {
            $('#myModal').modal('show');
            $('#lbMsg').html(msg);
        }

    </script>
</asp:Content>
