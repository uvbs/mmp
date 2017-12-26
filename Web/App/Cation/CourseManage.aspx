<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CourseManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.CourseManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        $(function () {
            var myMenu;
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[0];
            myMenu.expandMenu(firstSubmenu);
        });
    </script>--%>
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host %>';

        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "Article", ArticleTypeEx1: "hf_course" },
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ThumbnailsPath', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" /></a>', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'ActivityName', title: '课程名称', width: 160, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\');" title="{0}">{0}</a>', value, rowData.JuActivityIDHex);
                                    return str.ToString();
                                }
                                },
                            { field: 'RecommendCate', title: '分类', width: 100, align: 'left', formatter: FormatterTitle },
                                 { field: 'ActivityLecturer', title: '讲师', width: 50, align: 'left' },
                                { field: 'ActivityStartDate', title: '开课时间', width: 100, align: 'left', formatter: FormatDate },
                                { field: 'IsHide', title: '状态', width: 50, align: 'center', formatter: function (value) {
                                    if (value == 1) {
                                        return '<span style="color:red">隐藏</span>';
                                    }
                                    else {
                                        return '<span style="color:green">显示</span>';
                                    }
                                }
                                },
                                { field: 'IP', title: 'IP/PV', width: 40, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" title="点击查看统计详情" onclick="window.location.href=\'/App/Monitor/EventDetails.aspx?aid={0}\'">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                    return str.ToString();
                                }
                                },
	                //{ field: 'ShareTotalCount', title: '分享次数', width: 40, align: 'center' },
                                {field: 'UserID', title: '发布人', width: 40, align: 'left' },
                                { field: 'IsSignUpJubit', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" onclick="ShowEdit(\'{0}\');"><img alt="编辑该课程" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该课程" /></a>', rowData['JuActivityID']);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );


        });

        function ShowEdit(aid) {
            window.location.href = 'CourseCompile.aspx?Action=edit&aid=' + aid;
        }

        function ShowAdd() {
            window.location.href = 'CourseCompile.aspx?Action=add'
            return;
            $.messager.progress({ text: '正在处理。。。' });
            //检查是否已补足资料
            $.ajax({
                type: 'post',
                url: '/Handler/User/UserHandler.ashx',
                data: { Action: 'IsAllUserBaseInfo' },
                success: function (result) {
                    $.messager.progress('close');
                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {
                        window.location.href = '/FShare/ArticleCompile.aspx?Action=add'
                    }
                    else {
                        $.messager.confirm('系统提示', '您的资料未填写完整，填写完整后才能发表课程，现在去填写？', function (r) {
                            if (r) {
                                window.location.href = '/fshare/user/InfoCenter.aspx';
                            }
                        });
                    }
                }
            });
        }

        function ShowQRcode(aid) {
            //dlgSHowQRCode
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/' + aid + '/' + 'details.chtml');

            var linkurl = "http://" + domain + "/" + aid + "/" + "details.chtml";
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl); 

            $('#dlgSHowQRCode').dialog('open');
        }

        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中活动?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteJuActivity", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            //
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
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
                ids.push(rows[i].JuActivityID
                 );
            }
            return ids;
        }


        function Search() {
            var v = GetCheckGroupVal('RecommendCate', 'v'); //$('#selectCate').combobox('getText');
            //alert(v);
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "Article", RecommendCate: v, ArticleTypeEx1: "hf_course" }
	            });
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>课程管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">发布新课程</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除课程</a>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;所属分类：
            <%--<select class="easyui-combobox" name="state" id="selectCate" style="width: 200px;"
                data-options="multiple:true,panelHeight:'auto'">
                <%
                    //读取hf分类
                    List<ZentCloud.BLLJIMP.Model.UserPersonalizeDataInfo> cateList = new ZentCloud.BLLJIMP.BLLUserPersonalize("").QueryUserPList("hf").Where(p => p.PersonalizeType == 2).ToList();

                    StringBuilder strCateHtml = new StringBuilder();
                    foreach (var item in cateList)
                    {
                        strCateHtml.AppendFormat("<option value=\"{0}\">{0}</option>",
                                item.Val1
                            );
                    }

                    Response.Write(strCateHtml.ToString());
                %>
            </select>--%>
            <%
               
                //ZentCloud.BLLJIMP.Model.WebsiteInfo websiteInfo = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetWebsiteInfoModel();
                //StringBuilder strCateHtml = new StringBuilder();
                //strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{0}\" id=\"chkArticleCate1\" /><label for=\"chkArticleCate1\">{0}</label> &nbsp;", websiteInfo.CourseCate1);

                //strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{0}\" id=\"chkArticleCate2\" /><label for=\"chkArticleCate2\">{0}</label> &nbsp;", websiteInfo.CourseCate2);
                //Response.Write(strCateHtml.ToString());           

                //读取hf分类
                List<ZentCloud.BLLJIMP.Model.UserPersonalizeDataInfo> cateList = new ZentCloud.BLLJIMP.BLLUserPersonalize("").QueryUserPList("hf").Where(p => p.PersonalizeType == 2).ToList(); ;

                StringBuilder strCateHtml = new StringBuilder();
                foreach (var item in cateList)
                {
                    strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{1}\" id=\"chkArticleCate{0}\" /><label for=\"chkArticleCate{0}\">{1}</label> &nbsp;",
                            item.PersonalizeID,
                            ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetJuActiviCateRName(item.PersonalizeID)
                        );
                }

                Response.Write(strCateHtml.ToString());
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码即可打开本页面进行分享" modal="true" style="width: 320px; height: 320px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
         <br />
         <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
