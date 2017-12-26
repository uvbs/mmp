<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Recycle.Article" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp回收站&nbsp;&gt&nbsp;<span><%=moduleName %></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="SetRestoreArticle()">批量还原文章</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        var cateRootId =0;

        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryJuActivityByDelete", ArticleType: "Article", ArticleTypeEx1: "", CategoryId: cateRootId },
                height: document.documentElement.clientHeight - 112,
                pagination: true,
                striped: true,
                
                singleSelect: false,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'JuActivityID', title: '编号', width: 30, align: 'left', formatter: FormatterTitle },
                           {
                               field: 'ThumbnailsPath', title: '缩略图', width: 30, align: 'center', formatter: function (value) {
                                   if (value == '' || value == null)
                                       return "";
                                  
                                   var str = new StringBuilder();
                                   str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                   return str.ToString();
                               }
                           },
                            {
                                field: 'ActivityName', title: '标题', width: 30, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\',\'{2}\')" title="{0}">{0}</a>', value, rowData.JuActivityIDHex, rowData.JuActivityID);
                                    return str.ToString();
                                }
                            },
                            { field: 'CategoryName', title: '分类', width: 40, align: 'left', formatter: FormatterTitle },
                            { field: 'CreateDate', title: '创建时间', width: 40, align: 'left', formatter: FormatDate },
                            {
                                field: 'IP', title: 'IP/PV', width: 30, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (rowData["PV"] == 0) {
                                        str.AppendFormat("{0}", rowData["PV"]);
                                    } else {
                                        str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                    }
                                    return str.ToString();
                                }
                            },


                         { field: 'UserID', title: '发布人', width: 40, align: 'left' }
                        
                ]]
            });
        });
        
        //删除
        function SetRestoreArticle() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定还原选中文章?", function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: handlerUrl,
                            data: { Action: "RecoverJuActivity", ids: GetRowsIds(rows).join(',') },
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

               
                  


          
    </script>
</asp:Content>
