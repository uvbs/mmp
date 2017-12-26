<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WxFansUserInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.WxFansUserInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>所有粉丝信息</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           <%-- <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'"
                id="btnExportToFile" onclick="DownLoadData()">导出到文件</a>--%>
                 <a  href="javascript:history.go(-1);" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
        </div>
    </div>
     <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXForwardHandler.ashx";
        var ActivityId = '<%=ActivitId %>';
        var userId = '<%=userId%>';
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "GetFansUserInfo", article_id: ActivityId, user_id: userId },
                       height: document.documentElement.clientHeight - 150,
                       pagination: true,
                       striped: true,
                       singleSelect: true,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                    {
                                        field: 'head_img_url', title: '头像', width: 5, align: 'center', formatter: function (value) {
                                            if (value == '' || value == null)
                                                return "";
                                            var str = new StringBuilder();
                                            str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                            return str.ToString();
                                        }
                                    },
                                   { field: 'wx_nick_name', title: '微信昵称', width: 10, align: 'left' },
                                   { field: 'name', title: '姓名', width: 10, align: 'left' },
                                   { field: 'phone', title: '手机号码', width: 10, align: 'left' },
                                   { field: 'email', title: '邮箱', width: 10, align: 'left' },
                                   { field: 'company', title: '公司', width: 10, align: 'left' },
                                   { field: 'postion', title: '职位', width: 10, align: 'left' }
                       ]]
                   }
               );
        });


        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteForwar", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 0) {
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
                ids.push(rows[i].ActivityId
                    );
            }
            return ids;
        }


        function Search() {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "GetForwarList", ActivityName: $("#txtName").val() }
                   });
        }

        function DownLoadData() {
            $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                if (o) {
                    window.open(handlerUrl + '?Action=DownLoadForwardData&Mid=' + Mid + "&activityID=" + ActivityId + "&uid=" + uid);
                }
            });
        }
    </script>
</asp:Content>
