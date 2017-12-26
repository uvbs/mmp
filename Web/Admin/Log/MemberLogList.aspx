<%@ Page Title="日志记录" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MemberLogList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Log.MemberLogList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .datagrid-cell-c1-remark{
            white-space:normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"日志记录" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/log/MemberLogList.ashx',pagination:true,striped:true,loadFilter: thisPagerFilter,rownumbers:true,showFooter:true,
        onLoadSuccess:thisOnLoadSuccess,
        queryParams:{type:'<%= Request["module"] %>',target_id:'<%= Request["target_id"] %>' }">
        <thead>
            <tr>
                <th field="cuser" width="35" formatter="FormatterCaoZuoUser">操作人</th>
                <th field="time" width="35" formatter="FormatterTitle">操作时间</th>
                <th field="remark" width="100" formatter="FormatterContent">内容</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add2"
            plain="true" id="btnAdd" onclick="OpenAddShow()">新增日志</a>
        <span>当前会员：<span id="spMember"></span></span>
    </div>
    <div  id="dlgAdd" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'新增日志',width:450,height:320,modal:true,buttons:'#dlgAddButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" style="width:95%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:70px;">日志：</td>
                <td>
                    <div class="txtContent" 
                        contenteditable="true" 
                        style="width: 100%;line-height:21px;min-height:212px;padding:5px;border:solid #d3d3d3 1px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAddButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="AddLog()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgAdd').dialog('close');">取消</a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var target_id = '<%= Request["target_id"]%>'
        function thisPagerFilter(result) {
            var data = result.result;
            if (data == null) {
                return {
                    total: 0,
                    rows: []
                }
            }
            return {
                total: data.totalcount,
                rows: data.list,
                member: data.member
            };
        }
        function thisOnLoadSuccess(data) {
            console.log(data)
            if (data.member) {
                $('#spMember').text(data.member.name);
            }
        }
        function FormatterCaoZuoUser(value, rowData) {
            if (!value) return "";
            return value.name;
        }
        function FormatterContent(value, rowData) {
            var content = value;
            content = FormatterContentImg(content, '执照[', ']');
            content = FormatterContentImg(content, '凭证[', ']');
            return content;
        }
        function FormatterContentImg(content, startKey, endKey) {
            var startIndex = content.indexOf(startKey);
            if (startIndex >= 0) {
                var endIndex = content.indexOf(endKey, startIndex);
                var replaceString = content.substring(startIndex + 3, endIndex);
                var replaceNewString = replaceString;
                var spt = replaceNewString.split('改为');
                for (var i = 0; i < spt.length; i++) {
                    replaceNewString = FormatterSpImg(replaceNewString, spt[i], ',');
                }
                replaceNewString = replaceNewString.replace('改为', '<br />改为');
                content = content.replace(replaceString, replaceNewString);
                content = content.replace(startKey, '<br />' + startKey);
            } else {

            }
            return content;
        }
        function FormatterSpImg(replaceString,spString,spKey){
            if (!spString) return replaceString;
            if ($.trim(spString) == '') return replaceString;
            var imgs = spString.split(spKey);
            var links = [];
            for (var i = 0; i < imgs.length; i++) {
                if(!imgs[i]) continue;
                links.push('<a href="'+imgs[i]+'" style="margin:3px;" target="_blank"><img src="'+imgs[i]+'" width="80" height="80" /></a>');
            }
            console.log(links);
            if(links.length>0) {
                var linkString = links.join('');
                replaceString = replaceString.replace(spString, linkString);
            }
            return replaceString;
        }
        function OpenAddShow() {
            $('#dlgAdd .txtContent').html('');
            $('#dlgAdd').dialog('open');
        }
        function AddLog() {
            var content = $('#dlgAdd .txtContent').html();
            if (content.length > 500) {
                alert("备注最多能输入500个字");
                return;
            }
            $.messager.confirm('友情提示', '确定添加日志？',
                 function (o) {
                     if (o) {
                         $.ajax({
                             type: "Post",
                             url: '/Serv/API/Admin/Log/AddMemberLog.ashx',
                             data: {target_id:target_id, content: content },
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.status) {
                                     $('#grvData').datagrid('reload');
                                     $('#dlgAdd').dialog('close');
                                 } else {
                                     alert(resp.msg);
                                 }
                             }
                         });
                     }
                 });
        }
    </script>
</asp:Content>
