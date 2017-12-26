<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ArticleManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ArticleManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=moduleName %>管理&nbsp;&gt&nbsp;<span>所有<%=moduleName %></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <%if (isHideAdd == 0)
              { %>
            <a href="javascript:top.addTab( '新建<%=moduleName %>','/App/Cation/ArticleCompile.aspx?Action=add&cateRootId=<%=cateRootId %>&type=<%=type %>&isHideTag=<%=isHideTag %>&isHideCate=<%=isHideCate %>&isHideLevel=<%=isHideLevel %>&isHideWeixin=<%=isHideWeixin %>&isHideArea=<%=isHideArea %>&isHideTemplate=<%=isHideTemplate %>&isHideRelationArticle=<%=isHideRelationArticle %>&isHideUpSort=<%=isHideUpSort %>&nickReplaceId=<%=nickReplaceId %>&summaryReplaceTitle=<%=summaryReplaceTitle %>&isHideSummary=<%=isHideSummary %>&isHideImg=<%=isHideImg %>&isHideFile=<%=isHideFile %>&isHideUrl=<%=isHideUrl %>&isShowPraise=<%=isShowPraise %>&isShowFavorite=<%=isShowFavorite %>&isShowReward=<%=isShowReward %>&moduleName=<%=moduleName %>&isHideDelete=<%=isHideDelete %>&isHideAdd=<%=isHideAdd %>' )" class="easyui-linkbutton" iconcls="icon-add2"
                plain="true" id="btnAdd">新建<%=moduleName %></a>
            
            <%
                }
            %>
            <%
                if (isHideDelete ==0 && !isHide&&(WebsiteOwner!="meifan"))
                {   
            %>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-delete" plain="true" onclick="Delete()">批量删除<%=moduleName %></a>
            <%
                }
            %>
            <%if (isHideCate == 0 && (WebsiteOwner != "meifan"))
              { %>
            批量设置分类:
              <%=sbCategory1.ToString()%>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="BatchSetArticleCategory()">批量设置分类</a>
            <%}%>
            <%--<a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="DeployTutor()">配置理财师</a>--%>

            <%if (isHideLevel == 0 && (WebsiteOwner != "meifan"))
              { %>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="BatchSetAccessLevel()">设置访问等级</a>
            <%}%>
            <%if (isHideWeixin == 0 && (WebsiteOwner != "meifan"))
              { %>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="AddXiFen()">加入微吸粉</a>
            <%}%>
            <br />
            <%if (isHideCate == 0 && (WebsiteOwner != "meifan"))
              { %>
             &nbsp;分类：
                <%=sbCategory.ToString()%>
            <%}%>
            <%if (summaryReplaceTitle == 1)
              { %>
            内容：
            <%}
              else
              {%>
            标题：
            <%}%>
            <input id="txtArticleName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px; height: 185px; padding: 10px">
        <table>
            <tr>
                <td height="25" align="left">导师名称：
                </td>
                <td height="25" width="*" align="left">
                    <select id="Tutor">
                        <%=TutorStr%>
                    </select>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)"
                        id="btnExit" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>访问等级:
                </td>
                <td>
                    <input id="txtAccessLevel" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        var cateRootId = '<%=cateRootId%>';
        var type = '<%=type%>';
        var summaryReplaceTitle = "<%=summaryReplaceTitle %>";
        $(function () {

            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "QueryJuActivityForWeb", ArticleType: type, ArticleTypeEx1: "", CategoryId: cateRootId, isShowSummary: summaryReplaceTitle },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       singleSelect: false,

                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'JuActivityID', title: '编号', width: 20, align: 'left', formatter: FormatterTitle },
                                   
            <%if (isHideImg == 0)
              { %>
                                   {
                                       field: 'ThumbnailsPath', title: '缩略图', width: 25, align: 'center', formatter: function (value) {
                                           if (value == '' || value == null)
                                               return "";
                                           var str = new StringBuilder();
                                           str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                           return str.ToString();
                                       }
                                   },
            <%}%>
            <%if (summaryReplaceTitle == 1)
              { %>
                                {
                                    field: 'Summary', title: '内容', width: 100, align: 'left', formatter: FormatterTitle
                                },
            <%}
              else
              {%>
                                {
                                    field: 'ActivityName', title: '标题', width: 100, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\',\'{2}\')" title="{0}">{0}</a>', value, rowData.JuActivityIDHex, rowData.JuActivityID);
                                        return str.ToString();
                                    }
                                },
            <%}%>
            <%if (isHideCate == 0 && (WebsiteOwner != "meifan"))
              { %>
                                { field: 'CategoryName', title: '分类', width: 30, align: 'left', formatter: FormatterTitle },
            <%}%>
            <%if (isHideLevel == 0 && (WebsiteOwner != "meifan"))
              { %>
                                 { field: 'AccessLevel', title: '访问等级', width: 20, align: 'left', formatter: FormatterTitle },
            <%}%>
                                { field: 'CreateDate', title: '创建时间', width: 35, align: 'left', formatter: FormatDate },
                                {
                                    field: 'IsHide', title: '状态', width: 15, align: 'center', formatter: function (value) {
                                        if (value == 1) {
                                            return '<span style="color:red">隐藏</span>';
                                        }
                                        else {
                                            return '<span style="color:green">显示</span>';
                                        }
                                    }
                                },
                                <%
        if (isArticlePv)
        {
                                        %>
                                             {
                                                 field: 'IP', title: 'IP/PV', width: 30, align: 'center', formatter: function (value, rowData) {
                                                     var str = new StringBuilder();
                                                     if (rowData["PV"] == 0) {
                                                         str.AppendFormat("{0}", rowData["PV"]);
                                                     } else {
                                                         str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                                         //str.AppendFormat('<a class="listClickNum" href="javascript:top.addTab(\'IP/PV统计详情-{3}\',\'/App/Monitor/EventDetails.aspx?aid={0}\')" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV, rowData.ActivityName);
                                                     }
                                                     return str.ToString();
                                                 }
                                             },
                                        <%
        }
                                    %>
            <%if (isHideWeixin == 0)
              { %>
                                {
                                    field: 'UV', title: '微信阅读人数', width: 30, align: 'center', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData["UV"] == 0) {
                                            str.AppendFormat("{0}", rowData["UV"]);
                                        } else {
                                            //str.AppendFormat('<a class="listClickNum"  href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>', rowData.JuActivityID, rowData.UV);
                                            str.AppendFormat('<a class="listClickNum" href="javascript:top.addTab(\'微信阅读-{2}\',\'/App/Monitor/EventDetails.aspx?aid={0}&uv=1\')" title="点击查看统计详情">{1}</a>', rowData.JuActivityID, rowData.UV, rowData.ActivityName);
                                        }

                                        return str.ToString();
                                    }
                                },
            <%}%>
                        <%
        if (isHideCommentCount == 0 && (WebsiteOwner != "meifan"))
                        {
                        %>
                             {
                                 field: 'CommentCount', title: '评论数', width: 15, align: 'left', formatter: function (value, rowData) {
                                     if (value > 0) {
                                         var str = new StringBuilder();
                                         str.AppendFormat('<a class="listClickNum" href="/Admin/Review/ReviewList.aspx?ActId={0}&Pfolder=ArticleManage">' + value + '</a>', rowData['JuActivityID']);
                                         return str.ToString();
                                     }
                                     else {
                                         return value;
                                     }
                                 }
                             },
                         <%
                        }
                        %>
                               
            <%if (isShowPraise == 1)
              { %>
                                {
                                    field: 'PraiseCount', title: '点赞数', width: 15, align: 'left', formatter: FormatterTitle
                                },
            <%}%>
            <%if (isShowReward == 1)
              { %>
                                {
                                    field: 'RewardTotal', title: '赠送总额', width: 15, align: 'left', formatter: FormatterTitle
                                },
            <%}%>
                                <%for (var i = 1; i <= 10; i++)
                                  {%>

                                <%
                                      var fieldMap = this.tableFieldList.FirstOrDefault(p => p.Field == "K" + i.ToString());
                                      var isShow = fieldMap != null;
                                      if (isShow)
                                          isShow = fieldMap.IsShowInList == 1;
                                %>

                                <%   if (isShow)
                                     {   %>

                                    {
                                        field: 'K<%=i %>', title: '<%=fieldMap.MappingName%>', width: 40, align: 'left', formatter: function (value) {
                                            return '<span title="' + value + '">' + value + '</span>';
                                        }
                                    },

                                <% } %>

                                <%}%>
                                <%
                                if (isHideShareMonitorId == 0 && (WebsiteOwner != "meifan"))
                                {
                                    %>
                                {
                                    field: 'ShareMonitorId', title: '传播路径', width: 30, align: 'center', formatter: function (value, rowData) {

                                        var str = new StringBuilder();
                                        if (value == 0) {
                                            str.Append("<span>-</span>");
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/Admin/ShareMonitor/ShareTree/tree.html?mid={0}"  title="点击查看分享路径">查看</a>', value);
                                        }

                                        return str.ToString();
                                    }
                                },
                                    <%
                                }
                                %>
                                 

                                 //{ field: 'ShareTotalCount', title: '分享统计', width: 40, align: 'center',
                                 //    formatter: function (value, rowData) {
                                 //        var str = new StringBuilder();
                                 //        str.AppendFormat('<a href="/App/Cation/ArticleStatistics.aspx?articleId={0}"  title="点击查看统计详情" >{1}</a>', rowData.JuActivityID, value);
                                 //        return str.ToString();
                                 //    }
                                 //},
                                <%if (nickReplaceId == 1)
                                  { %>
                                { field: 'UserNickname', title: '发布人', width: 30, align: 'left' },
                                <%}
                                  else
                                  {%>
                                { field: 'UserID', title: '发布人', width: 30, align: 'left' },
                                <%}%>
                                //{ field: 'TrueName', title: '姓名', width: 40, align: 'left' },
                                <%if (isHideUpSort == 0)
                                  { %>
                                {
                                    field: 'Sort', title: '排序', width: 25, align: 'left', formatter: function (value, rowData) {
                                        var newvalue = "";
                                        if (value != null) {
                                            newvalue = value;
                                        }
                                        var str = new StringBuilder();
                                        str.AppendFormat('<input type="text" value="{0}" id="txtArticle{1}" style="width:20px;" maxlength="5"> <a title="点击保存排序号"  onclick="UpdateSortIndex({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.JuActivityID);
                                        return str.ToString();
                                    }
                                },

                                <%}%>
                                {
                                    field: 'IsSignUpJubit', title: '操作', width: 15, align: 'center', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        var cateRootId = "<%=cateRootId%>";
                                        var isHideTag = "<%=isHideTag %>";
                                        var isHideCate = "<%=isHideCate %>";
                                        var isHideLevel = "<%=isHideLevel %>";
                                        var isHideWeixin = "<%=isHideWeixin %>";
                                        var isHideArea = "<%=isHideArea %>";
                                        var isHideTemplate = "<%=isHideTemplate %>";
                                        var isHideRelationArticle = "<%=isHideRelationArticle %>";
                                        var isHideUpSort = "<%=isHideUpSort %>";
                                        var nickReplaceId = "<%=nickReplaceId %>";
                                        var isHideSummary = "<%=isHideSummary %>";
                                        var isHideImg = "<%=isHideImg %>";
                                        var isHideFile = "<%=isHideFile %>";
                                        var isHideUrl = "<%=isHideUrl %>";
                                        var isShowPraise = "<%=isShowPraise %>";
                                        var isShowFavorite = "<%=isShowFavorite %>";
                                        var isShowReward = "<%=isShowReward %>";
                                        var isHideDelete = "<%=isHideDelete %>";
                                        var isHideAdd = "<%=isHideAdd %>";
                                        var moduleName = "<%=moduleName%>";
                                        var aType = "<%=type%>";
                                        str.AppendFormat('<a href="javascript:top.addTab(\'编辑<%=moduleName %>-{4}\',\'/App/Cation/ArticleCompile.aspx?Action=edit&aid={0}&cateRootId={1}&isHideTag={2}&isHideCate={5}&isHideLevel={6}&isHideWeixin={7}&isHideArea={8}&isHideTemplate={9}&isHideRelationArticle={10}&moduleName={3}&type={11}&isHideUpSort={12}&nickReplaceId={13}&summaryReplaceTitle={14}&isHideSummary={15}&isHideImg={16}&isHideFile={17}&isHideUrl={18}&isShowPraise={19}&isShowFavorite={20}&isShowReward={21}&isHideDelete={22}&isHideAdd={23}\')"><img alt="编辑该{3}" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该{3}" /></a>'
                                            , rowData['JuActivityID'], cateRootId, isHideTag, moduleName, rowData.ActivityName, isHideCate, isHideLevel, isHideWeixin, isHideArea, isHideTemplate, isHideRelationArticle, aType, isHideUpSort, nickReplaceId, summaryReplaceTitle, isHideSummary, isHideImg, isHideFile, isHideUrl, isShowPraise, isShowFavorite, isShowReward, isHideDelete, isHideAdd);
                                        return str.ToString();
                                    }
                                }
                       ]]
                   }
            );





            //批量设置访问级别对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var JuActivityIDs = [];
                        for (var i = 0; i < rows.length; i++) {
                            JuActivityIDs.push(rows[i].JuActivityID);
                        }

                        var dataModel = {
                            Action: "UpdateAccessLevel",
                            AccessLevel: $.trim($('#txtAccessLevel').val()),
                            JuActivityID: JuActivityIDs.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status > 0) {
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                }
                                Alert(resp.Msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });


        });

                        //更新排序号
                        function UpdateSortIndex(articleid) {

                            var sortindex = $("#txtArticle" + articleid).val();
                            if ($.trim(sortindex) == "") {
                                $("#txtArticle" + articleid).focus();
                                return false;
                            }



                            var re = /^[1-9]+[0-9]*]*$/;
                            if (!re.test(sortindex)) {
                                alert("请输入正整数");
                                $("#txtArticle" + articleid).val("");
                                $("#txtArticle" + articleid).focus();
                                return false;
                            }


                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: { Action: "UpdateArticleSortIndex", ArticleID: articleid, SortIndex: sortindex },
                                dataType: "json",
                                success: function (resp) {
                                    try {
                                        if (resp.Status == 1) {
                                            Show(resp.Msg);
                                            $('#grvData').datagrid("reload");
                                        }
                                        else {
                                            Alert(resp.Msg);
                                        }
                                    } catch (e) {
                                        alert(e);
                                    }

                                }
                            });


                        }

                        //        function ShowEdit(aid) {
                        //            window.location.href = '/App/Cation/ArticleCompile.aspx?Action=edit&aid=' + aid;
                        //        }

                        //        function ShowAdd() {
                        //            window.location.href = 'ArticleCompile.aspx?Action=add'
                        //            return;
                        //            $.messager.progress({ text: '正在处理。。。' });
                        //            //检查是否已补足资料
                        //            $.ajax({
                        //                type: 'post',
                        //                url: '/Handler/User/UserHandler.ashx',
                        //                data: { Action: 'IsAllUserBaseInfo' },
                        //                success: function (result) {
                        //                    $.messager.progress('close');
                        //                    var resp = $.parseJSON(result);
                        //                    if (resp.Status == 1) {
                        //                        window.location.href = 'ArticleCompile.aspx?Action=add'
                        //                    }
                        //                    else {
                        //                        $.messager.confirm('系统提示', '您的资料未填写完整，填写完整后才能发表分享，现在去填写？', function (r) {
                        //                            if (r) {
                        //                                window.location.href = '/App/Cation/user/InfoCenter.aspx';
                        //                            }
                        //                        });
                        //                    }
                        //                }
                        //            });
                        //        }

                        //     function ShowQRcode(aid) {
                        //         //dlgSHowQRCode
                        //         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/' + aid + '/' + 'details.chtml');
                        //         $('#dlgSHowQRCode').dialog('open');
                        //         var linkurl = "http://" + domain + "/" + aid + "/" + "details.chtml";
                        //         $("#alinkurl").html(linkurl);
                        //         $("#alinkurl").attr("href", linkurl);

                        //     }
                        function ShowQRcode(aid, juid) {
                            var linkurl = "http://" + domain + "/" + aid + "/" + "details.chtml";
                            if (domain == "forbes.comeoncloud.net") {
                                linkurl = "http://forbes.comeoncloud.net/customize/forbes/index.html#/article_single/" + juid;
                            }
                            $.ajax({
                                type: 'post',
                                url: "/Handler/QCode.ashx",
                                data: { code: linkurl },
                                success: function (result) {
                                    $("#imgQrcode").attr("src", result);
                                }
                            });
                            $('#dlgSHowQRCode').dialog('open');
                            $("#alinkurl").html(linkurl);
                            $("#alinkurl").attr("href", linkurl);
                        }


                        //删除
                        function Delete() {

                            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            $.messager.confirm("系统提示", "确定删除选中<%=moduleName %>?", function (o) {
                            if (o) {
                                $.ajax({
                                    type: "Post",
                                    url: handlerUrl,
                                    data: { Action: "DeleteJuActivity", ids: GetRowsIds(rows).join(','), type: type },
                                    dataType: "json",
                                    success: function (resp) {
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
                        $('#grvData').datagrid(
                               {
                                   method: "Post",
                                   url: handlerUrl,
                                   queryParams: { Action: "QueryJuActivityForWeb", ArticleType: type, ArticleTypeEx1: "", CategoryId: $("#ddlcategory").val() == 0 ? cateRootId : $("#ddlcategory").val(), ActivityName: $("#txtArticleName").val(), isShowSummary: summaryReplaceTitle }
                               });
                    }
                    //配置导师<%=moduleName %>
        function DeployTutor() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }

            $('#dlgPmsInfo').window(
               {
                   title: '配置<%=moduleName %>导师'
               }
            );
            $('#dlgPmsInfo').dialog('open');
        }
        //加入微吸粉
        function AddXiFen() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) return;
            if (confirm("是否加入微吸粉")) {
                $.ajax({
                    type: "post",
                    url: handlerUrl,
                    data: { Action: "ActivityForwardInfo", ids: GetRowsIds(rows).join(','), forward_type: "fans" },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 0) {
                            Alert(resp.Msg)
                        } else {
                            Alert(resp.Msg)
                        }
                    }
                });
            }
        }


        function BatchSetArticleCategory() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            if ($("#ddlsetcategory").val() == "") {

                Alert("请选择要设置的分类");
                return;
            }
            var categoryname = $("#ddlsetcategory").find("option:selected").text().replace('└', '');
            //
            $.messager.confirm("系统提示", "确定将所选<%=moduleName %>的分类修改为 " + categoryname, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "BatchSetArticleCategory", ids: GetRowsIds(rows).join(','), CategroyId: $("#ddlsetcategory").val() == 0 ? cateRootId : $("#ddlsetcategory").val() },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                             Alert(resp.Msg);

                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });
     }

     $("#btnSave").live("click", function () {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         var TutorId = $('#Tutor').val();
         $.ajax({
             type: "Post",
             url: handlerUrl,
             data: { Action: "SavaArticleTutor", ids: GetRowsIds(rows).join(','), userId: TutorId },
             dataType: "json",
             success: function (resp) {
                 if (resp.Status == 1) {
                     $('#grvData').datagrid('reload');
                     Alert(resp.Msg);

                 }
                 else {
                     Alert(resp.Msg);
                 }
             }

         });
     });

     //设置访问等级
     function BatchSetAccessLevel() {
         var rows = $("#grvData").datagrid('getSelections');//获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $("#dlgInfo").dialog({ title: "设置访问级别" });
         $("#dlgInfo").dialog("open");
     }

     //窗体关闭按钮---------------------
     $("#btnExit").live("click", function () {
         $("#dlgPmsInfo").window("close");
     });

    </script>
</asp:Content>
