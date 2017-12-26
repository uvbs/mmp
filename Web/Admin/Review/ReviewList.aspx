<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ReviewList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Review.ReviewList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectList {
            width: 150px;
            height: 30px;
            line-height: 30px;
            padding: 3px;
            display: inline;
            border-radius: 0px;
        }

        .textKeyword {
            width: 260px;
            display: inline;
            border-radius: 0px;
        }
        .colorBlue{
            color: #337ab7;
            text-decoration: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=ReviewName %>管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<%=ReviewName %>列表

    <% if (!string.IsNullOrWhiteSpace(pFolder))
       {%>
    <a href="javascript:;" style="float: right; margin-right: 20px; color: Black;" title="返回上级列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" onclick="ToBack()">返回</a>
    <% }%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

            <% if (string.IsNullOrWhiteSpace(pFolder))
               {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" id="A1" onclick="AddItem()">发布话题</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" id="btnDelItem" onclick="DelItem()">删除</a>
            <a href="ReviewConfig.aspx" class="easyui-linkbutton" iconcls="icon-edit" plain="true">话题配置</a>
            <a href="#" onclick="ShowQRcode()" style="color: blue;">获取手机端话题列表链接</a>
            <% }%>




            <% if (!string.IsNullOrWhiteSpace(pFolder))
               {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnPassItem" onclick="PassItem()">审核通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnNoPassItem" onclick="NoPassItem()">审核不通过</a>
            <br />
            审核状态:<select id="ddlStatus" class="form-control selectList">
                <option value="">全部</option>
                <option value="0">待审核</option>
                <option value="1">审核通过</option>
                <option value="2">审核不通过</option>
            </select>
                
           <%-- <%
                
                   if (pFolder == "ArticleManage")
                   {
                       %>
                             类型:<select id="ddlType" class="form-control selectList">
                            <% if (string.IsNullOrWhiteSpace(Request["foreignkeyId"]))
                               {%>
                            <option value="" selected="selected">全部</option>
                            <option value="ArticleComment">评论</option>
                            <option value="CommentReply">回复</option>
                            <% }
                               else
                               {
                                   %>
                                 <option value="CommentReply">回复</option>
                            <%
                               }
                              %>
                            </select>
                         <%
                   }
            %>--%>
            <% }%>

        关键字:<input id="txtKeyword" class="form-control textKeyword" placeholder="内容" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>


    <table id="grvData" fitcolumns="true">
    </table>



    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/ReviewHandler.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';

        var ActId = '<%=Request["ActId"] %>';

        var Pfolder = '<%=Request["Pfolder"] %>';

        var foreignkeyId = '<%=Request["foreignkeyId"] %>';

        var cateId = '<%=Request["cateId"] %>';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "GetReviewList", ActId: ActId, Pfolder: Pfolder, foreignkeyId: foreignkeyId, status: "" },
                height: document.documentElement.clientHeight - 115,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
    <% if (string.IsNullOrWhiteSpace(pFolder))
       {%>

                        { field: 'title', title: '标题', width: 50, align: 'left', formatter: FormatterTitle },
                    
                        {
                            field: 'reply_count', title: '回复', width: 30, align: 'left', formatter: function (value, row) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="ReplyList.aspx?ReviewId={0}" title="点击查看统计详情">查看回复/{1}</a>', row.review_id, row.reply_count);

                                return str.ToString();

                            }
                        },
    <% }
       else if (pFolder == "OrderComment")
       {%>

                        { field: 'title', title: '标题', width: 50, align: 'left', formatter: FormatterTitle },
                        { field: 'content', title: '评论内容', width: 100, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'comment_img', title: '晒图', width: 100, align: 'left', formatter: function (value, row) {
                                var str = new StringBuilder();
                                if (value != '' && value != null) {
                                    var values = value.split(',');
                                    for (var i = 0; i < values.length; i++) {
                                        str.AppendFormat('<img src="{0}" style="width:50px;height:50px;margin-left:5px;"/>', values[i]);
                                    }
                                }
                                return str.ToString();
                            }
                        },
                        {
                            field: 'toUrl', title: '商品', width: 40, align: 'left', formatter: function (value,row) {
                                var str = new StringBuilder();
                                    str.AppendFormat("<a href='/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/{0}#/productDetail/{0}' class='colorBlue' onclick='goProductDetailUrl()'>链接</a>", row.ex1);
                                return str.ToString();
                            }
                        },
                        { field: 'name', title: '用户', width: 40, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'status', title: '状态', width: 40, align: 'left', formatter: function (value, row) {
                                if (value == 1) return '<span style="color:green;">审核通过<span>';
                                if (value == 2) return '<span style="color:red;">审核不通过<span>';
                                return "待审核";
                            }
                        },
            <% }
       else if (pFolder == "ArticleManage")
       {
            %>
                        { field: 'content', title: '评论内容', width: 150, align: 'left', formatter: FormatterTitle },


                      


                        {
                            field: 'rpyNum', title: '回复数', width: 30, align: 'left', formatter: function (value, row) {
                                var str = new StringBuilder();
                                <% if (!string.IsNullOrWhiteSpace(Request["foreignkeyId"]))
                           {%>
                                str.AppendFormat('{0}', value);
                                <% }
                           else
                           {%>
                                <% }%>
                                if (row.type == "ArticleComment") {
                                    str.AppendFormat('<a style="color:blue;" href="/Admin/Review/ReviewList.aspx?ActId={1}&Pfolder={2}&foreignkeyId={3}" title="点击查看">{0}</a>', value, ActId, "ArticleManage", row.id);
                                }
                                else {
                                    str.AppendFormat('{0}', value);
                                }
                                return str.ToString();
                            }
                        },
                        { field: 'name', title: '用户', width: 40, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'type', title: '类型', width: 40, align: 'left', formatter: function (value, row) {
                                if (value == "ArticleComment") return '<span style="color:blue;">评论<span>';
                                if (value == "CommentReply") return '<span>回复<span>';
                                return "";
                            }
                        },
                        {
                            field: 'status', title: '状态', width: 40, align: 'left', formatter: function (value, row) {
                                if (value == 1) return '<span style="color:green;">审核通过<span>';
                                if (value == 2) return '<span style="color:red;">审核不通过<span>';
                                return "待审核";
                            }
                        },
            <%
       
       }%>

                ]]
            }
            );
        });

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            return ids;
        }
        //获取选中行ID集合
        function GetRowsAutoIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].review_id);
            }
            return ids;
        }
        function DelItem() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确认删除选中？", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Delete", ids: GetRowsAutoIds(rows).join(',') },
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

        function PassItem() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确认审核通过选中？", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Pass", ids: GetRowsAutoIds(rows).join(',') },
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
        function NoPassItem() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确认审核不通过选中？", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "NoPass", ids: GetRowsAutoIds(rows).join(',') },
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
        function Search() {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "GetReviewList", ActId: ActId, Pfolder: Pfolder, foreignkeyId: foreignkeyId, status: $("#ddlStatus").val(), keyword: $("#txtKeyword").val() }
            });
        }
        function ToBack() {
            switch (Pfolder) {
                case "ArticleManage":
                    window.location.href = "/App/Cation/ArticleManage.aspx";
                    break;
                case "Review":
                    window.location.href = "/Admin/Review/ReviewList.aspx?ActId=" + ActId + "&Pfolder=ArticleManage";
                    break;
                case "Ask":
                    window.location.href = "/Admin/Ask/AskList.aspx";
                    break;
                case "Case":
                    window.location.href = "/Admin/Case/CaseList.aspx";
                    break;
                case "News":
                    window.location.href = "/Admin/News/NewsList.aspx";
                    break;
                case "Open":
                    window.location.href = "/Admin/Open/OpenList.aspx";
                    break;
                case "Regulations":
                    window.location.href = "/Admin/Regulations/RegulationsList.aspx";
                    break;
                case "Statuses":
                    window.location.href = "/Admin/Statuses/StatusesArticleList.aspx?cateId=" + cateId;
                    break;

                default:
                    window.history.go(-1);
                    break;
            }
        }


        function AddItem() {
            //添加话题
            location.href = "ReviewCompile.aspx";

        }

        function ShowQRcode() {
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Review/M/List.aspx');
            $('#dlgSHowQRCode').dialog('open');
            var linkUrl = "http://" + domain + "/App/Review/M/List.aspx";
            $("#alinkurl").html(linkUrl).attr("href", linkUrl);
        }
    </script>
</asp:Content>
