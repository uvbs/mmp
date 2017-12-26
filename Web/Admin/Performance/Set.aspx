<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Set.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Performance.Set" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .leveNumber {
            width: 150px;
            color: red;
            font-size: 14px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"管理奖比例设置" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/performance/SetList.ashx',pagination:true,striped:true,
        loadFilter: pagerFilter,rownumbers:true,queryParams:{field_count:'<% = baseSets.Count %>'}">
        <thead>
            <tr>
                <th field="user_phone" width="50" formatter="FormatterTitle">会员手机</th>
                <th field="user_name" width="80" styler="StylerSys" formatter="FormatterTitle">会员姓名</th>
                <%foreach (var item in baseSets)
                  {%>
                    <th field="<% = "p"+ Convert.ToInt64(item.Performance) %>" width="50" formatter="FormatterTitle"><% = item.Name %></th>
                  <%}%>
                <th field="action" width="50" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a id="btnAdd" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" onclick="ShowAdd();">新增</a>
            会员手机：<input id="txtMember" class="easyui-textbox" style="width: 90px;" placeholder="手机" value="<% = Request["member"] %>" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <div  id="dlgSet" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'修改规则比例',width:500,height:450,modal:true,buttons:'#dlgSetButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0" width="100%">
            <tr class="trMemberPhone">
                <td style="text-align:right;">
                    会员手机：
                </td>
                <td>
                    <input class="easyui-searchbox" data-options="prompt:'会员手机',searcher:SearchSpread" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    会员姓名：
                </td>
                <td>
                    <span class="memberID hidden"></span>
                    <span class="memberTrueName"></span>
                </td>
            </tr>
            <%foreach (var item in baseSets)
                  {%>
            <tr>
                <td style="text-align:right;">
                    <% = item.Name %>：
                </td>
                <td>
                    <input id="txt<% = "p"+ Convert.ToInt64(item.Performance) %>" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
                <%}%>
        </table>
    </div>
    <div id="dlgSetButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="Set()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgSet').dialog('close');">取消</a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/performance/';
        var keys = [
            'start'
            <%foreach (var item in baseSets)
                  {%>
            ,'p'+ '<% = Convert.ToInt64(item.Performance) %>'
                <%}%>
        ];
        var keynames = [
            'start'
            <%foreach (var item in baseSets)
                  {%>
            , '<% = item.Name %>'
                <%}%>
        ];
        keys.RemoveIndexOf(0);
        keynames.RemoveIndexOf(0);

        var field_count = '<% = baseSets.Count %>';
        $(function () { });
        function StylerSys(value, rowData, index) {
            if (rowData.is_sys == 1) return 'color:red;';
        }
        function FormatterAction(value, rowData, index) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:ShowEdit({0})"><img alt="修改" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/list.png" title="修改" /></a>', index);
            if (rowData.is_sys != 1) str.AppendFormat('<a href="javascript:DeleteSet({0})" style="margin-left:10px;"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/cancel.png" title="删除" /></a>', index);
            return str.ToString();
        }
        function SearchSpread(value) {
            var spreadid = $.trim(value);
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetSpreadUser.ashx',
                data: { spreadid: spreadid },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        $('#dlgSet .memberID').text(resp.result.id);
                        $('#dlgSet .memberTrueName').html('<span style="color:green">' + resp.result.name + '</span>');
                    } else {
                        $('#dlgSet .memberID').text(0);
                        $('#dlgSet .memberTrueName').html('<span style="color:red;">' + resp.msg + '</span>');
                    }
                }
            });
        }
        function ShowAdd() {
            $('#dlgSet input[type="text"]').val('');
            $('#dlgSet .memberID').text(0);
            $('#dlgSet .memberTrueName').text('');
            $('#dlgSet .trMemberPhone').show();
            $('#dlgSet').dialog({ title: '新增规则' });
            $('#dlgSet').dialog('open');
        }
        function ShowEdit(index) {
            var data = $('#grvData').datagrid('getData');
            var row = data.rows[index];
            $('#dlgSet .memberID').text(row.user_auto_id);
            $('#dlgSet .memberTrueName').text(row.user_name);
            $('#dlgSet .trMemberPhone').hide();
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var v = row[key];
                if (v) v = v.replace('%', '');
                if ($('#dlgSet #txt' + key).length > 0) $('#dlgSet #txt' + key).val(v);
            }
            $('#dlgSet').dialog({ title: '修改规则' });
            $('#dlgSet').dialog('open');
        }
        function Set() {
            var postData = {
                user_auto_id: $('#dlgSet .memberID').text()
            };
            if (keys.length == 0) {
                alert('请初始化设置');
                return;
            }
            if (postData.user_auto_id == 0) {
                alert('会员未找到');
                return;
            }
            postData.keys = keys.join(',');
            postData.keynames = keynames.join(',');
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var kv = $('#dlgSet #txt' + key).val();
                if (!kv) {
                    alert('请填完所有设置');
                    return;
                }
                postData[key] = kv;
            }
            $.ajax({
                type: 'post',
                url: handlerUrl+'Set.ashx',
                data: postData,
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        Show(resp.msg);
                        $('#dlgSet').dialog('close');
                        $('#grvData').datagrid('reload');
                    }
                    else {
                        Alert(resp.msg);
                    }
                }
            });
        }

        function DeleteSet(index) {
            var data = $('#grvData').datagrid('getData');
            var row = data.rows[index];
            
            $.messager.confirm('系统提示', '确认删除[' + row.user_name + ']的设置?', function (r) {
                if (r) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl+'DeleteSet.ashx',
                        data: { user_id: row.user_id },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.status) {
                                Show(resp.msg);
                                $('#dlgSet').dialog('close');
                                $('#grvData').datagrid('reload');
                            } else {
                                Alert(resp.msg);
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
