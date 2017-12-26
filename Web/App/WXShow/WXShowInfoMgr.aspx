<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXShowInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WXShow.WXShowInfoMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理&nbsp;&gt&nbsp;<span>微秀</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="ADWXShowInfo.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">
                新建</a>
                 <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit"
                    plain="true" onclick="OnEdit()">编辑</a>
                     <a href="javascript:void(0)" class="easyui-linkbutton"
                        iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
            <br />
            名称:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
           
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXShowInfoHandler.ashx";
     var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetWxShowInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ShowImg', title: '图片', width: 10, align: 'left', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'ShowName', title: '名称', width: 15, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">{1}&nbsp;[二维码]</a>', rowData.AutoId, value);
                                    return str.ToString();
                                }
                                },
                                { field: 'ShowUrl', title: '链接', width: 20, align: 'left' },
                                {
                                    field: 'ippv', title: 'PV/IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.PV == 0) {
                                            str.AppendFormat('{0}',0);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.AutoId, rowData.PV, rowData.IP);
                                        }
                                        return str.ToString();
                                    }
                                },
                                //{
                                //    field: 'IP', title: 'IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                //        var str = new StringBuilder();
                                //        if (value == 0) {
                                //            str.AppendFormat('{0}', 0);
                                //        } else {
                                //            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}</a>', rowData.AotoId, rowData.IP);
                                //        }
                                //        return str.ToString();
                                //    }
                                //},
                                {
                                    field: 'UV', title: '微信阅读人数', width: 10, align: 'left', formatter: function (value,rowData) {
                                        var str = new StringBuilder();
                                        if (value == 0) {
                                            str.AppendFormat('{0}', 0);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>',rowData.AutoId,value);
                                        }
                                        return str.ToString();
                                    }
                                },
                                { field: 'SHARECOUNT', title: '分享', width: 10, align: 'left', formatter: function (value,rowData) {
                                    return rowData.ShareAppMessageCount + rowData.ShareTimelineCount
                                } 
                                },
                                { field: 'InsertDate', title: '创建时间', width: 10, align: 'left', formatter: FormatDate },
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
                     data: { Action: "DeletetWxShowInfos", ids: GetRowsIds(rows).join(',') },
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
             ids.push(rows[i].AutoId);
         }
         return ids;
     }



     function Search() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetWxShowInfos", Name: $("#txtName").val() }
	            });
     }






     function OnEdit() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         if (rows.length > 1) {
             Alert("只能选择一行数据");
         }
         //            alert(GetRowsIds(rows).join(','));
         window.location.href = "ADWXShowInfo.aspx?AutoId=" + GetRowsIds(rows).join(',');
     }
     function ShowQRcode(id) {
         //dlgSHowQRCode
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=' + id);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=" + id;
         $("#alinkurl").html(linkurl).attr("href", linkurl);
     }
    </script>
</asp:Content>
