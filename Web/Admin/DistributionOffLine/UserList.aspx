<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="UserList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;业务分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>业务员管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowSetPreUser();">修改分销上级</a>

                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="ShowAddHistoryAmount();">增加累计财富</a>
            <br />
            姓名:
            <input type="text" id="txtKeyWord" style="width: 200px;" 
placeholder="姓名,用户ID" />
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">
                查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgDistribution" class="easyui-dialog" closed="true" title="设置上级" style="width: 600px;
        padding: 15px;">
        上级姓名:
        <input type="text" id="txtKeyWord1" style="width: 200px;" placeholder="姓名" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchPre()">
            查询</a>
        <table id="grvPreUserData" fitcolumns="true">
        </table>
    </div>

        <div id="dlgAddHistoryAmount" class="easyui-dialog" closed="true" title="增加累计财富" style="width: 400px;
        padding: 15px;">

                <table width="100%">
                 <tr>
                <td style="width:70px;">
                    增加累计财富金额:
                </td>
                <td>
                     <input id="txtAddHistoryAmount" onkeyup="this.value=this.value.replace(/\D/g,'')"/>
                </td>
            </tr>
            </table>
        
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/User/List.ashx";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                //queryParams: { Action: "QueryWebsiteUser", HaveTrueName: 1 },
	                height: document.documentElement.clientHeight - 185,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '用户ID', width: 50, align: 'left' },
                                {
                                    field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'Email', title: '邮箱', width: 50, align: 'left', formatter: FormatterTitle },

                                { field: 'HistoryDistributionOffLineTotalAmount', title: '累计财富', width: 50, align: 'center', formatter: FormatterTitle },
                                 { field: 'DistributionOffLineTotalAmount', title: '可提现财富', width: 50, align: 'center', formatter: function (value, row) {

                                     return row.DistributionOffLineTotalAmount - row.DistributionOffLineFrozenAmount;

                                 } 
                                 }

                                , { field: 'RecommendTrueName', title: '推荐人', width: 50, align: 'center', formatter: function (value, row) {

                                    return row.DistributionOffLineRecomendUserInfo.TrueName;

                                }
                                }




	                ]]
	            });

            $('#grvPreUserData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[

                                {
                                    field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle }




	                ]]
	            });

            //设置分销上级对话框
            $('#dlgDistribution').dialog({
                buttons: [{
                    text: '修改分销上级',
                    handler: function () {

                        var auIds = GetRowsIds($('#grvData').datagrid('getSelections')).toString();
                        var rows = $("#grvPreUserData").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;
                        var preUserId = $('#grvPreUserData').datagrid('getSelections')[0].UserID;
                        var preAutoId = $('#grvPreUserData').datagrid('getSelections')[0].AutoID;
                        if ($.inArray(preAutoId, GetRowsIds($('#grvData').datagrid('getSelections'))) >= 0) {
                            Alert("上级用户不能跟选择的用户相同，请检查");
                            return false;
                        }



                        $.ajax({
                            type: 'post',
                            url: "Handler/User/UpdatePreUserId.ashx",
                            data: { autoIds: auIds, preUserId: preUserId },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    Alert("操作成功");
                                    $('#dlgDistribution').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistribution').dialog('close');
                    }
                }]
            });

            //增加累计财富
            $('#dlgAddHistoryAmount').dialog({
                buttons: [{
                    text: '增加累计财富',
                    handler: function () {
                        var autoIds = GetRowsIds($('#grvData').datagrid('getSelections')).toString();
                        var amount = $(txtAddHistoryAmount).val();
                        if (amount == "") {
                            Alert("请输入金额");
                            return false;
                        }
                        if (isNaN(amount)) {
                            Alert("请输入数字");
                            return false;
                        }
                        $.ajax({
                            type: 'post',
                            url: "Handler/User/AddHistoryTotalAmount.ashx",
                            data: { autoIds: autoIds, amount: amount },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    Alert("操作成功");
                                    $('#dlgAddHistoryAmount').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAddHistoryAmount').dialog('close');
                    }
                }]
            });

        });

        //搜索
        function Search() {

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { keyword: $(txtKeyWord).val() }
                    });
                }

        //搜索上级
        function SearchPre() {
            $('#grvPreUserData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { keyword: $(txtKeyWord1).val() }
                    });
        }

        //显示设置分销上级对话框
        function ShowSetPreUser() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgDistribution').dialog({ title: '请选择上级用户' });
            $('#dlgDistribution').dialog('open');

        }


        //显示设置分销上级对话框
        function ShowAddHistoryAmount() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgAddHistoryAmount').dialog({ title: '增加累计财富' });
            $('#dlgAddHistoryAmount').dialog('open');

        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);

            }
            return ids;
        }


    </script>
</asp:Content>
