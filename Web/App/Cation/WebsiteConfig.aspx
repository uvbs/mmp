<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WebsiteConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WebsiteConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <style type="text/css">
        /*fieldset*/
        .sort {
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
    当前位置：&nbsp;<span>网站配置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <p>
            <a href="javascript:;" id="btnSave" onclick="Save();" class="button button-rounded button-primary">保存</a>
        </p>
        <% ZentCloud.BLLJIMP.Model.WebsiteInfo currWebsiteInfo = ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel(); %>
        <hr style="border: 1px dotted #036;" />
        <fieldset>
            <legend>基本配置</legend>
            <div class="input-group">
                <span class="input-group-addon">站点名</span>
                <input id="txtWebsiteName" type="text" class="form-control" placeholder="站点名" />
            </div>
            <div class="input-group">
                <span class="input-group-addon">文章活动顶部代码</span>
                <textarea rows="2" id="txtArticleHeadCode" class="form-control" placeholder="文章活动顶部代码"></textarea>
            </div>
            <div class="input-group">
                <span class="input-group-addon">文章活动底部代码</span>
                <textarea rows="2" id="txtArticleBottomCode" class="form-control" placeholder="文章活动顶部代码"></textarea>
            </div>

        </fieldset>
        <fieldset>
            <legend>菜单配置</legend>
            <div class="input-group">
                <span class="input-group-addon">菜单一</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.CourseManageMenuRName))
                   { %>
                <input id="txtCourseManageMenuRName" type="text" class="form-control" placeholder="课程管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtCourseManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"课程管理 (如果留空则不会显示菜单\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单二</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ActivityManageMenuRName))
                   { %>
                <input id="txtActivityManageMenuRName" type="text" class="form-control" placeholder="活动管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtActivityManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"活动管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单三</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleManageMenuRName))
                   { %>
                <input id="txtArticleManageMenuRName" type="text" class="form-control" placeholder="文章管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"文章管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单四</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.MasterManageMenuRName))
                   { %>
                <input id="txtMasterManageMenuRName" type="text" class="form-control" placeholder="专家库管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtMasterManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"专家库管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单五</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.QuestionManageMenuRName))
                   { %>
                <input id="txtQuestionManageMenuRName" type="text" class="form-control" placeholder="问答管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtQuestionManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"问答管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单六</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.UserManageMenuRName))
                   { %>
                <input id="txtUserManageMenuRName" type="text" class="form-control" placeholder="用户管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtUserManageMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"用户管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">菜单七</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.SignUpCourseMenuRName))
                   { %>
                <input id="txtSignUpCourseMenuRName" type="text" class="form-control" placeholder="报名管理 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtSignUpCourseMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"报名管理 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>

            <div class="input-group">
                <span class="input-group-addon">菜单八</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.MallMenuRName))
                   { %>
                <input id="txtMallMenuRName" type="text" class="form-control" placeholder="在线商城 (如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtMallMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"在线商城 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>

            <div class="input-group">
                <span class="input-group-addon">菜单九</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.WebSiteStatisticsMenuRName))
                   { %>
                <input id="txtWebSiteStatisticsMenuRName" type="text" class="form-control" placeholder="网站统计(如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtWebSiteStatisticsMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"在线商城 (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>


            <div class="input-group">
                <span class="input-group-addon">菜单十</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.AddVMenuRName))
                   { %>
                <input id="txtAddVMenuRName" type="text" class="form-control" placeholder="微信加V(如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtAddVMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"微信加V (如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>

            <div class="input-group">
                <span class="input-group-addon">菜单十一</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.MonitorMenuRName))
                   { %>
                <input id="txtMonitorMenuRName" type="text" class="form-control" placeholder="监测平台(如果留空则不会显示菜单)" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtMonitorMenuRName\" type=\"text\" class=\"form-control\" placeholder=\"监测平台(如果留空则不会显示菜单)\" />"));
                   }
                %>
            </div>



        </fieldset>
        <br />
        <fieldset>
            <legend>课程分类配置</legend>

            <div class="input-group">
                <span class="input-group-addon">分类一</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.CourseCate1))
                   { %>
                <input id="txtCourseCate1" type="text" class="form-control" placeholder="精彩课程回放" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtCourseCate1\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.CourseCate1));
                   }
                %>
            </div>

            <div class="input-group">
                <span class="input-group-addon">分类二</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.CourseCate2))
                   { %>
                <input id="txtCourseCate2" type="text" class="form-control" placeholder="课程预告" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtCourseCate2\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.CourseCate2));
                   }
                %>
            </div>



        </fieldset>
        <br />
        <fieldset>
            <legend>文章分类配置</legend>
            <div class="input-group">
                <span class="input-group-addon">分类一</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate1))
                   { %>
                <input id="txtArticleCate1" type="text" class="form-control" placeholder="月度成果分享" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleCate1\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.ArticleCate1));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类二</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate2))
                   { %>
                <input id="txtArticleCate2" type="text" class="form-control" placeholder="老板点评分享" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleCate2\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.ArticleCate2));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类三</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate3))
                   { %>
                <input id="txtArticleCate3" type="text" class="form-control" placeholder="实用资源分享" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleCate3\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.ArticleCate3));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类四</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate4))
                   { %>
                <input id="txtArticleCate4" type="text" class="form-control" placeholder="成功案例分享" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleCate4\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.ArticleCate4));
                   }
                %>
            </div>
            <div class="input-group">
                <span class="input-group-addon">分类五</span>
                <% if (string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate5))
                   { %>
                <input id="txtArticleCate5" type="text" class="form-control" placeholder="每周感悟分享" />
                <%}
                   else
                   {
                       Response.Write(string.Format("<input id=\"txtArticleCate5\" type=\"text\" class=\"form-control\" placeholder=\"{0}\" />", currWebsiteInfo.ArticleCate5));
                   }
                %>
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
                    <h4 class="modal-title" id="myModalLabel">系统提示</h4>
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
        $(function () {
            ShowConfigData();
        });

        function Save() {
            var dataModel = {
                Action: 'UpdateWebSiteInfo',
                CourseManageMenuRName: $.trim($(txtCourseManageMenuRName).val()),
                ArticleManageMenuRName: $.trim($(txtArticleManageMenuRName).val()),
                MasterManageMenuRName: $.trim($(txtMasterManageMenuRName).val()),
                QuestionManageMenuRName: $.trim($(txtQuestionManageMenuRName).val()),
                UserManageMenuRName: $.trim($(txtUserManageMenuRName).val()),
                SignUpCourseMenuRName: $.trim($(txtSignUpCourseMenuRName).val()), //txtActivityManageMenuRName
                ActivityManageMenuRName: $.trim($(txtActivityManageMenuRName).val()),
                CourseCate1: $.trim($(txtCourseCate1).val()),
                CourseCate2: $.trim($(txtCourseCate2).val()),
                ArticleCate1: $.trim($(txtArticleCate1).val()),
                ArticleCate2: $.trim($(txtArticleCate2).val()),
                ArticleCate3: $.trim($(txtArticleCate3).val()),
                ArticleCate4: $.trim($(txtArticleCate4).val()),
                ArticleCate5: $.trim($(txtArticleCate5).val()),
                WebsiteName: $.trim($(txtWebsiteName).val()),
                MallMenuRName: $.trim($(txtMallMenuRName).val()),
                WebSiteStatisticsMenuRName: $.trim($(txtWebSiteStatisticsMenuRName).val()),
                AddVMenuRName: $.trim($(txtAddVMenuRName).val()),
                MonitorMenuRName: $.trim($(txtMonitorMenuRName).val()),
                ArticleHeadCode: $.trim($(txtArticleHeadCode).val()),
                ArticleBottomCode: $.trim($(txtArticleBottomCode).val())

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
                    try {

                        $('#txtArticleCate1').val(resp.ArticleCate1);
                        $('#txtArticleCate2').val(resp.ArticleCate2);
                        $('#txtArticleCate3').val(resp.ArticleCate3);
                        $('#txtArticleCate4').val(resp.ArticleCate4);
                        $('#txtArticleCate5').val(resp.ArticleCate5);

                        $('#txtCourseCate1').val(resp.CourseCate1);
                        $('#txtCourseCate2').val(resp.CourseCate2);

                        $('#txtArticleManageMenuRName').val(resp.ArticleManageMenuRName);
                        $('#txtCourseManageMenuRName').val(resp.CourseManageMenuRName);
                        $('#txtMasterManageMenuRName').val(resp.MasterManageMenuRName);
                        $('#txtQuestionManageMenuRName').val(resp.QuestionManageMenuRName);
                        $('#txtSignUpCourseMenuRName').val(resp.SignUpCourseMenuRName);
                        $('#txtUserManageMenuRName').val(resp.UserManageMenuRName);
                        $('#txtActivityManageMenuRName').val(resp.ActivityManageMenuRName);
                        $('#txtWebsiteName').val(resp.WebsiteName);
                        $('#txtMallMenuRName').val(resp.MallMenuRName);
                        $('#txtWebSiteStatisticsMenuRName').val(resp.WebSiteStatisticsMenuRName);
                        $("#txtAddVMenuRName").val(resp.AddVMenuRName);
                        $("#txtMonitorMenuRName").val(resp.MonitorMenuRName);
                        $("#txtArticleHeadCode").val(resp.ArticleHeadCode);
                        $("#txtArticleBottomCode").val(resp.ArticleBottomCode);
                    } catch (e) {
                        alert(e);
                    }
                }
            });
        }

        function ShowMsg(msg) {
            Alert(msg);
            //$('#myModal').modal('show');
            //$('#lbMsg').html(msg);
        }

    </script>
</asp:Content>
