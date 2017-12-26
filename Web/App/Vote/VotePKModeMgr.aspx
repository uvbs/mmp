<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VotePKModeMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VotePKModeMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;<%=VoteInfo.VoteName%>
    &gt;&nbsp;&nbsp;<span>PK组管理 </span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" onclick="ShowAdd()"
                iconcls="icon-add2" plain="true">新增PK组</a> 
           <%-- <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>--%>
             <a href="/App/Vote/VoteInfoMgr.aspx"
                        class="easyui-linkbutton" iconcls="icon-back" plain="true" style="float: right; margin-right: 20px;">返回</a>
            <br />
            <span style="font-size: 12px;margin-left:20px; font-weight: normal">组名</span>
            <input type="text" style="width: 200px" id="txtGroupName" placeholder="请输入关键字搜索" />
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="Search();" iconcls="icon-search" id="btnSearch">
                查询</a>
            <br />
        </div>
    </div>

    <table id="grvData" fitcolumns="true">
    </table>

    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>组名:</td>
                <td>
                    <input id="txtVoteGroupName" placeholder="请输入组名" class="form-control" type="text" style="width: 250px;" />
                </td>
            </tr>
             <tr  id="trName">
                <td>PK者:</td>
                <td>
                    <input id="txtVoteGroupMembers" class="form-control" onclick="ShowObjectInfo()" style="width: 250px;position: inherit; display: inline-block;" readonly="readonly" type="text" />
                </td>
            </tr>
            <tr>
                <td>排序:</td>
                <td>
                    <input id="txtSort" class="form-control"  placeholder="请输入排序" type="number" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    
     <div id="dlgObject" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
         <div>
             编号和姓名:<input type="text" id="number" />
             <a href="javascript:void(0)" class="easyui-linkbutton" onclick="SearchKey();" iconcls="icon-search">
                查询</a>
             <br />
         </div>
        <table id="grvInfo" fitcolumns="true">
        </table>
    </div>

    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var voteId = '<%=Request["vid"]%>';
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var OrderBy = " Status ASC,AutoID ASC";
        var action = "";
        var GroupId = 0;
        var ids = "";
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryVoteGroupInfo", VoteId: voteId, Sort: " Sort Desc,VoteGroupId ASC "},
                height: document.documentElement.clientHeight - 112,
                ////pagination: true,
                //singleSelect:true,
                striped: true,
                //pageSize: 50,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                        { field: 'group_name', title: '组名', width: 100, align: 'center', formatter: FormatterTitle },
                        { field: 'vote_number', title: '选手编号', width: 100, align: 'center', formatter: FormatterTitle },
                        { field: 'vote_object_name', title: '姓名', width: 100, align: 'center', formatter: FormatterTitle},
                        { field: 'vote_count', title: '票数', width: 100, align: 'center', formatter:FormatterTitle},
                        { field: 'EditCloum', title: '行操作', width: 60, align: 'center', 
                            formatter: function (value, rowData,rowIndex) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="javascript:void(0)" class="l-btn"  onclick="ToDeletePage({0});"><span class="l-btn-left"><span class="l-btn-text" style="color:#EC4A23;">删除</span></span></a><br />', rowIndex);
                               
                                return str.ToString();
                            }
                        },
                        { field: 'EditGroup', title: '组操作', width: 60, align: 'center', 
                            formatter: function (value, rowData, rowIndex) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="ToAddGroup({0});"><span class="l-btn-left"><span class="l-btn-text">加入组</span></span></a><br />', rowIndex);
                                str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="ToEditGroup({0});"><span class="l-btn-left"><span class="l-btn-text">编辑组</span></span></a><br />', rowIndex);
                                str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="ToDeleteGroup({0});"><span class="l-btn-left"><span class="l-btn-text">删除组</span></span></a>',rowIndex);
                                return str.ToString();
                            }
                        }
                ]],
                onClickRow: function (index, data) {
                    $(this).datagrid('unselectRow', index);
                },
                onLoadSuccess: function (data) {
                    SetMergeCells(data, 'group_name', 'group_name');
                    SetMergeCells(data, 'group_name', 'EditGroup');
                }
            });


            //搜索
            $("#btnSearch").click(function () {
               

            });



            //添加PK组对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $("#grvInfo").datagrid("getSelections");
                        var autoids = [];
                        for (var i = 0; i < rows.length; i++) {
                            autoids.push(rows[i].AutoID);
                        }
                        var dataModel = {
                            Action: action,
                            VoteId: voteId,
                            VoteGroupId:GroupId,
                            VoteGroupName: $.trim($("#txtVoteGroupName").val()),
                            VoteGroupMembers: action == "EditVoteGroupInfo" ? $("#txtVoteGroupMembers").val() : autoids.join(','),
                            Sort: $("#txtSort").val()
                        };
                        $.messager.progress({ text: '正在提交...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status > 0) {
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                alert(resp.Msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });

            //参与者列表
            $('#grvInfo').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteObjectInfo", VoteID: voteId, OrderBy: OrderBy,Status:1 },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                 columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Number', title: '编号', width: 20, align: 'left' },
                                { field: 'VoteObjectName', title: '参与者名称', width: 20, align: 'left' },
                                { field: 'VoteCount', title: '票数', width: 20, align: 'left' }

	                ]]
	            }
            );

            //显示参与者列表  单击文本框
            $('#dlgObject').dialog({

                buttons: [{
                    text: '保存',
                    handler: function () {
                        var rows = $('#grvInfo').datagrid('getSelections');
                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }
                        if (action != '') {
                            
                            var names = [];
                            ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                names.push(rows[i].VoteObjectName);
                                ids.push(rows[i].VoteGroupId);
                            }
                            $("#txtVoteGroupMembers").val(names);
                            $('#dlgObject').dialog('close');
                        } else {
                            var objects = [];
                            for (var j = 0; j < rows.length; j++) {
                                objects.push(rows[j].AutoID);
                            }
                            var dataModel =
                                {
                                    Action: "AddVoteGroupByMember",
                                    VoteId: voteId,
                                    VoteGroupId: GroupId,
                                    VoteGroupMembers: objects.join(',')
                                };
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status>0) {
                                        $('#dlgObject').dialog('close');
                                        $('#grvData').datagrid('reload');
                                        $.messager.show({
                                            title: '系统提示',
                                            msg: resp.Msg
                                        });
                                    }
                                    else {
                                        $.messager.alert('系统提示', resp.Msg);
                                    }
                                }
                            });




                        }
                       
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgObject').dialog('close');
                    }
                }]
            });

            
        })

        function Search() {
            var groupName = $("#txtGroupName").val();
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteGroupInfo", VoteId: voteId, Sort: " Sort Desc,VoteGroupId ASC ",keyWord:groupName }
	            });
        }

        function SearchKey() {
            var keyWord = $("#number").val();
            $('#grvInfo').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { Action: "QueryVoteObjectInfo", VoteID: voteId, OrderBy: OrderBy, Status: 1, VoteObjectName: keyWord },
                   height: 300,
                   pagination: true,
                   striped: true,
                   pageSize: 50,
                   rownumbers: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[
                              { title: 'ck', width: 5, checkbox: true },
                              { field: 'Number', title: '编号', width: 20, align: 'left' },
                              { field: 'VoteObjectName', title: '参与者名称', width: 20, align: 'left' },
                              { field: 'VoteCount', title: '票数', width: 20, align: 'left' }

                   ]]
               }
           );
        }

        

        //添加pk组 单击新增按钮
        function ShowAdd() {
            action = "AddVoteGroupInfo";
            $("#txtVoteGroupName").val("");
            $("#txtVoteGroupMembers").val("");
            $("#txtSort").val("");
            $("#trName").show();
            $("#dlgInfo").dialog({ title: "添加PK组" });
            $("#dlgInfo").dialog("open");
        }
        //显示参与者列表  单击文本框
        function ShowObjectInfo() {
            $('#dlgObject').dialog({ title: '参与者信息' });
            $('#dlgObject').dialog('open');
            $("#grvInfo").datagrid("reload");
        }
        //跨行跨列
        function SetMergeCells(data, colField, mergeCell) {
            var nValue = null;
            var startIndex = 0;
            for (var i = 0; i < data.rows.length; i++) {
                if (i == 0) {
                    nValue = data.rows[i][colField];
                    startIndex = i;
                }
                if (nValue != data.rows[i][colField] && i > 0) {
                    $(grvData).datagrid('mergeCells', {
                        index: startIndex,
                        field: mergeCell,
                        rowspan: i - startIndex
                    });
                    nValue = data.rows[i][colField];
                    startIndex = i;
                }
            }
            $(grvData).datagrid('mergeCells', {
                index: startIndex,
                field: mergeCell,
                rowspan: data.rows.length - startIndex
            });
        }
        //删除组
        function ToDeleteGroup(rowIndex) {
            var rows = $('#grvData').datagrid('getRows');//获取选中的行
            var row = rows[rowIndex];
            var str = "确定要删除[" + row.group_name + "]分组?";
            $.messager.confirm("系统提示", str, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteVoteGroupInfo", id: row.group_id },
                        dataType:'json',
                        success: function (resp) {
                            alert(resp.Msg);
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });

        }
        //编辑组
        function ToEditGroup(rowIndex) {
            action = "EditVoteGroupInfo";
            var rows = $('#grvData').datagrid('getRows');//获取选中的行
            var row = rows[rowIndex];
            GroupId = row.group_id;
            $("#txtVoteGroupName").val(row.group_name);
            $("#trName").hide();
            $("#txtSort").val(row.group_sort);
            $("#dlgInfo").dialog({ title: "编辑分组" });
            $("#dlgInfo").dialog("open");
        }
        //删除行
        function ToDeletePage(rowIndex) {
            var rows = $('#grvData').datagrid('getRows');//获取选中的行
            var row = rows[rowIndex];
            var str = "确定要删除[编号=" + row.vote_number + "]的选手?";
            $.messager.confirm("系统提示", str, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteVoteGroupInfoByMembers", objectId: row.object_id, groupId: row.group_id },
                        dataType:'json',
                        success: function (resp) {
                            Alert(resp.Msg);
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });
        }
        //加入组
        function ToAddGroup(rowIndex) {
            var rows = $('#grvData').datagrid('getRows');//获取选中的行
            var row = rows[rowIndex];
            GroupId = row.group_id;
            action = '';
            $("#grvInfo").datagrid("reload");
            $('#dlgObject').dialog({ title: '参与者信息' });
            $('#dlgObject').dialog('open');
        }

    </script>
</asp:Content>
