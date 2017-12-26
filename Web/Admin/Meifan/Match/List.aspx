<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Match.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;竞赛&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>竞赛管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           <%if (PmsAdd)
              {%>
            <a href="Add.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">添加</a>
            <% } %>

            <%if (PmsDelete)
              {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>

             <% } %>
            <%if (PmsEnable)
              {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateEnable(1)">启用</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateEnable(0)">禁用</a>
            <% } %>
            <div>
                状态:
                    <select id="ddlIsPublish" onchange="Search()">
                        <option value="">全部</option>
                        <option value="1">启用</option>
                        <option value="0">禁用</option>
                    </select>

                <input type="text" id="txtKeyWord" placeholder="名称" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/meifan/match/";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "list.ashx",
                       queryParams: {},
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       pageSize: 50,
                       rownumbers: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   {
                                       field: 'activity_img', title: '图片', width: 10, align: 'center', formatter: function (value) {
                                           if (value == '' || value == null)
                                               return "";
                                           var str = new StringBuilder();
                                           str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                           return str.ToString();
                                       }
                                   },
                                   { field: 'activity_name', title: '名称', width: 20, align: 'left' },
                                   { field: 'summary', title: '概要', width: 20, align: 'left' },
                                   //{
                                   //    field: 'signup_count', title: '报名人数', width: 20, align: 'center', formatter: function (value, rowData) {
                                   //        var str = new StringBuilder();
                                   //        //if (value == 0) {
                                   //        //    str.AppendFormat("{0}", value);
                                   //        //} else {
                                   //        str.AppendFormat('<a class="listClickNum" href="SignUp/List.aspx?activity_id={0}"  title="点击查看报名详情">{1}</a>', rowData.activity_id, value);

                                   //        // }
                                   //        return str.ToString();
                                   //    }
                                   //},
                                   {
                                       field: 'is_publish', title: '状态', width: 10, align: 'left', formatter: function (value, rowData) {

                                           var str = new StringBuilder();
                                           switch (value) {
                                               case 1:
                                                   str.AppendFormat('<font color="green">启用</font>');
                                                   break;
                                               case 0:
                                                   str.AppendFormat('<font color="red">禁用</font>');
                                                   break;

                                                   break;
                                               default:

                                           }

                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'is_need_pay', title: '是否收费', width: 10, align: 'left', formatter: function (value, rowData) {

                                           var str = new StringBuilder();
                                           switch (value) {
                                               case 1:
                                                   str.AppendFormat('付费');
                                                   break;
                                               case 0:
                                                   str.AppendFormat('免费');
                                                   break;

                                                   break;
                                               default:

                                           }

                                           return str.ToString();
                                       }
                                   }
                                 <%if (PmsUpdate){%>
                                   ,
                                   {
                                       field: 'Op', title: '操作', width: 20, align: 'center', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="Update.aspx?activity_id={0}">编辑</a>', rowData['activity_id']);
                                           return str.ToString();
                                       }
                                   }
                                   <% } %>


                       ]]
                   }
            );

            $("#btnSearch").click(function () {

                Search();


            })


        })




        function Search() {

            $('#grvData').datagrid({ url: handlerUrl + "list.ashx", queryParams: { is_publish: $("#ddlIsPublish").val(), keyword: $("#txtKeyWord").val() } });



        }
        function UpdateEnable(isEnable) {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                var msg = "禁用";
                if (isEnable == 0) {
                    msg = "启用";
                }
                $.messager.confirm("系统提示", "确认" + msg + "选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].activity_id);
                        }

                        var dataModel = {
                            activity_ids: ids.join(','),
                            is_enable: isEnable
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "updateenable.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status == true) {
                                    Alert("操作成功");
                                } else {
                                    Alert("操作失败");
                                }

                                $('#grvData').datagrid('reload');

                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].activity_id);
                        }

                        var dataModel = {

                            activity_ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "delete.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status == true) {
                                    Alert("删除成功");
                                } else {
                                    Alert("删除失败");
                                }

                                $('#grvData').datagrid('reload');

                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }



    </script>
</asp:Content>
