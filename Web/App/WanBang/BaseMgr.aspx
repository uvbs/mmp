<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="BaseMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.BaseMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;基地管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="BaseCompile.aspx?Action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true">添加基地</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" >编辑基地信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除基地</a>
            <a href="/App/WanBang/Wap/BaseList.aspx" target="_blank" class="easyui-linkbutton" iconcls="icon-list" plain="true" >查看手机端基地列表</a>
            <div>
               区县:
               <select id="ddlArea">
               <option value="">全部</option>
               <option value="黄浦区">黄浦区</option>
               <option value="长宁区">长宁区</option>
               <option value="徐汇区">徐汇区</option>
               <option value="静安区">静安区</option>
               <option value="杨浦区">杨浦区</option>
               <option value="虹口区">虹口区</option>
               <option value="闸北区">闸北区</option>
               <option value="普陀区">普陀区</option>
               <option value="浦东新区">浦东新区</option>
               <option value="宝山区">宝山区</option>
               <option value="闵行区">闵行区</option>
               <option value="金山区">金山区</option>
               <option value="嘉定区">嘉定区</option>
               <option value="青浦区">青浦区</option>
               <option value="松江区">松江区</option>
               <option value="奉贤区">奉贤区</option>
               <option value="崇明县">崇明县</option>
               </select>

                <span style="font-size: 12px; font-weight: normal">基地名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
   
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    var handlerUrl = "/Handler/WanBang/PC.ashx";
    $(function () {
        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryBaseInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Thumbnails', title: '缩略图', width: 8, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'Area', title: '区/县', width: 10, align: 'left' },
                                { field: 'BaseName', title: '基地名称', width: 30, align: 'left' },
                                { field: 'Contacts', title: '联系人', width: 10, align: 'left' },
                                { field: 'InsertDate', title: '添加日期', width: 15, align: 'left', formatter: FormatDate }


                             ]]
	            }
            );






        $("#btnSearch").click(function () {

            $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryBaseInfo", BaseName: $("#txtName").val(), Area: $("#ddlArea").val()} });
        });



    });

    function Delete() {
        try {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            $.messager.confirm("系统提示", "确认删除选中基地?", function (r) {
                if (r) {
                    var ids = [];

                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].AutoID);
                    }

                    var dataModel = {
                        Action: 'DeleteBaseInfo',
                        ids: ids.join(',')
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: dataModel,
                        success: function (result) {
                            Alert(result);
                            $('#grvData').datagrid('reload');
                        }
                    });
                }
            });

        } catch (e) {
            Alert(e);
        }
    }

    function ShowEdit() {
        var rows = $('#grvData').datagrid('getSelections');

        if (!EGCheckIsSelect(rows))
            return;

        if (!EGCheckNoSelectMultiRow(rows))
            return;

        window.location.href = "BaseCompile.aspx?Action=edit&Id=" + rows[0].AutoID;

    }


    </script>
</asp:Content>
