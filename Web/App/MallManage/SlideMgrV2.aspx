<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SlideMgrV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.SlideMgrV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;首页<span>广告管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="SlideAddEditV2.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">添加</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete"
                    plain="true" onclick="Delete()">批量删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QuerySlide" },
	                height: document.documentElement.clientHeight - 160,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Type', title: '类型', width: 10, align: 'center', formatter: function (value) {

                                    if (value == "slide1") {
                                        return "滑动图片1";
                                    }
                                    if (value == "slide2") {
                                        return "滑动图片2";
                                    }
                                    if (value == "slide3") {
                                        return "滑动图片3";
                                    }
                                    if (value == "slide4") {
                                        return "滑动图片4";
                                    }
                                    if (value == "promotion") {
                                        return "限时特卖";
                                    }
                                    if (value == "pagetop1") {
                                        return "顶部广告1";
                                    }
                                    return value;
                                }
                                },
                                { field: 'ImageUrl', title: '图片', width: 20, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" width="200" height="100" />', value);
                                    return str.ToString();
                                }
                                },
                            { field: 'LinkText', title: '文字', width: 10, align: 'center' },
                            { field: 'Link', title: '跳转链接', width: 20, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="{0}" target="_blank">{0}</>', value);
                                return str.ToString();
                            }
                            },
                            { field: 'Sort', title: '排序', width: 10, align: 'center' },
                            { field: 'EditCloum', title: '操作', width: 10, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改" href="SlideAddEditV2.aspx?id={0}">编辑</a>', rowData.AutoID);
                                return str.ToString();
                            }
                            }




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
                        data: { Action: "DeleteSlide", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                Show(resp.Msg);
                                $('#grvData').datagrid('reload');
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
                ids.push(rows[i].AutoID
                 );
            }
            return ids;
        }



    </script>
</asp:Content>