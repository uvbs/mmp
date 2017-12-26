<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PageList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.WebPC.PageList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;PC页面管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="Add.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                 id="btnAdd">新增页面</a>
           
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <div>
                <span style="font-size: 12px; font-weight: normal">名称：</span>
                <input type="text" style="width: 200px" id="txtKeyWord" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
        
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
      
        var handlerUrl = "/Serv/Api/Admin/WebPc/";

        $(function () {

             //加载数据
                $('#grvData').datagrid(
                      {
                          method: "Post",
                          url: handlerUrl + "List.ashx",
                          queryParams: { KeyWord: $("#txtKeyWord").val() },
                          height: document.documentElement.clientHeight - 112,
                          pagination: true,
                          striped: true,
                          pageSize: 50,
                          rownumbers: true,
                          rowStyler: function () { return 'height:25px'; },
                          columns: [[
                                      { title: 'ck', width: 5, checkbox: true },
                                      { field: 'PageName', title: '页面名称', width: 80, align: 'left' },
                                       {
                                           field: 'op', title: '操作', width: 25, align: 'center', formatter: function (value, rowData) {
                                               var str = new StringBuilder();
                                               str.AppendFormat('<a title="编辑" href="Edit.aspx?pageId={0}" >编辑</a>|', rowData.PageId);
                                               str.AppendFormat('<a title="获取链接" href="javascript:" onclick="ShowLink(\'{0}\')">获取页面链接</a>', rowData.PageId);
                                               return str.ToString();
                                           }
                                       }
                                                                     


                          ]]
                      }
                  );



              

                //搜索
                $("#btnSearch").click(function () {
                   
                    $('#grvData').datagrid({ url: handlerUrl+"List.ashx", queryParams: {KeyWord: $("#txtKeyWord").val()} });
                });



            });


            //删除     
            function Delete() {
                try {
                    var rows = $('#grvData').datagrid('getSelections');
                    if (!EGCheckIsSelect(rows))
                        return;

                    $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                        if (r) {
                            var ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].PageId);

                            }
                            var dataModel = {
                               
                                ids: ids.join(',')
                            }
                           

                            $.ajax({
                                type: 'post',
                                url: handlerUrl+"Delete.ashx",
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == 1) {
                                        Show("删除完成");
                                        $('#grvData').datagrid('reload');
                                    } else {
                                        Show("删除失败");
                                    }
                                }
                            });
                        }
                    });

                } catch (e) {
                    Alert(e);
                }
            }

            //页面链接
            function ShowLink(id) {
                var link = "http://"+window.location.host+"/WebPc/Page.aspx?pageId="+id;
                layer.open({
                    title: '页面链接',
                    type: 1,
                    skin: 'layui-layer-rim', //加上边框
                    area: ['300px', '100px'], //宽高
                    content: '<div style="text-align: center;padding: 10px;"><a target="_blank" href="' + link + '">'+link+'</a></div>'

                   
                });
            }

    </script>
</asp:Content>
