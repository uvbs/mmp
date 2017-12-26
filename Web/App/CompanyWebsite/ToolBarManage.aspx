<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ToolBarManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.ToolBarManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .icon {
           width: 1em; height: 1em;
           vertical-align: -0.15em;
           fill: currentColor;
           overflow: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微网站&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=new ZentCloud.BLLJIMP.BLLCompanyWebSite().GetToolBarUseTypeName(this.Request["use_type"]) %>管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/CheckIconFont.js"></script>
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" onclick="ToEditPage('add','0','<% = this.Request["use_type"] %>','')" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">添加</a> 
            <%--<a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除</a>--%>
            <br />
            <select id="ddlGroup"></select>
<%--            是否电脑端
            <select id="ddlIsPc">
                <option value="">全部</option>
                <option value="1">电脑端</option>
            </select>--%>
            <label style="margin-left: 8px;">名称</label>
            <input type="text" id="txtName" style="width: 200px;" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
     <div id="dlgInfo" class="easyui-dialog" closed="true" title="修改分组名称" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    分组名称：
                </td>
                <td>
                    <input id="txtGroupName" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var groups =<% = groups%>;
        var use_type ='<% =this.Request["use_type"]%>';
        var is_system ='<% =this.Request["is_system"]%>';
        var group_ids=0;
        $(function () {
            loadGroups();
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryCompanyWebsiteToolBar",UseType:use_type,KeyType:$("#ddlGroup").val(),IsSystem:"<%=this.Request["is_system"] =="1"?1:0  %>",IsPc:"<%=Request["isPc"]%>"},
                height: document.documentElement.clientHeight - 112,
                //pagination: true,
                striped: true,
                //pageSize: 50,
                //rownumbers: true,
                //checkOnSelect:false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                         { field: 'KeyType', title: '分组', width: 100, align: 'center', formatter: FormatterTitle },
                         { field: 'ToolBarImage', title: '图标或图片', width: 70, align: 'center', formatter: function (value,rowData) {
                             var str = new StringBuilder();
                             if (value == '' || value == null){
                                 if(rowData.ImageUrl  != null && rowData.ImageUrl !=''){
                                     str.AppendFormat('<img src="{0}" style="max-height:50px;max-width:50px;" />', rowData.ImageUrl);
                                 }
                             }
                             else{
                                 value = value.replace('iconfont ','');
                                 str.AppendFormat('<div style="height:50px;font-size:50px;">');
                                 if(rowData.IcoColor!=null && rowData.IcoColor!=""){
                                     str.AppendFormat('<svg class="icon" style="color:{0};" aria-hidden="true">', rowData.IcoColor);
                                     str.AppendFormat('<use xlink:href="#{0}"></use>', value);
                                     str.AppendFormat('</svg>');

                                 }
                                 else{
                                     str.AppendFormat('<svg class="icon" aria-hidden="true">');
                                     str.AppendFormat('<use xlink:href="#{0}"></use>', value);
                                     str.AppendFormat('</svg>');
                                 }
                                 str.AppendFormat('</div>');
                             }
                             return str.ToString();
                         }},
                        { field: 'ToolBarName', title: '名称', width: 160, align: 'left',formatter:function(value,rowData){
                            if (rowData.IsSystem == 1) {
                                return "<font color='red'>"+value+"</font>";
                            }
                            else {
                                return value;
                            }
                        } },
                        { field: 'PlayIndex', title: '播放顺序', width: 160, align: 'center',formatter:function(value,rowData){
                            return "<input type='text' id='txtIndex"+rowData["AutoID"]+"' value="+value+" style='width:50px;' maxlength='5'/> <a title='点击保存排序号'  onclick='UpdateSortIndex("+rowData["AutoID"]+")' href='javascript:void(0);'>保存</a>";
                        }},
                        { field: 'ToolBarType', title: '类型', width: 100, align: 'center', formatter: FormatterTitle },
                        { field: 'IsShow', title: '是否显示', width: 100, align: 'center', formatter: function (value, rowData) {
                            if (value == "1") {
                                return "<font color='green'>显示</font>";
                            }
                            else {
                                return "<font color='red'>不显示</font>";
                            }

                        }
                        },
                        { field: 'EditCloum', title: '行操作', width: 50, align: 'center', 
                        formatter: function (value, rowData,rowIndex) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="javascript:void(0)" class="l-btn" onclick="ToEditPage(\'edit\',\'{0}\',\'{1}\',\'{2}\')"><span class="l-btn-left"><span class="l-btn-text">编辑</span></span></a>', rowData.AutoID,use_type,rowData.ToolBarName);
                                if(is_system !=1 && rowData.IsSystem != 1) str.AppendFormat('<br /><a href="javascript:void(0)" class="l-btn" onclick="ToDeletePage({0})"><span class="l-btn-left"><span class="l-btn-text" style="color:#EC4A23;">删除</span></span></a>',rowIndex);
                                return str.ToString();
                            }
                        },
                        { field: 'EditGroup', title: '组操作', width: 50, align: 'center', 
                            formatter: function (value, rowData, rowIndex) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="ToEditGroup({0});"><span class="l-btn-left"><span class="l-btn-text">编辑组</span></span></a><br />', rowIndex);
                                str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="ToDeleteGroup({0});"><span class="l-btn-left"><span class="l-btn-text" style="color:#EC4A23;">删除组</span></span></a>', rowIndex);
                                return str.ToString();
                            }
                        }
                ]],
                onClickRow: function(index, data) {
                    $(this).datagrid('unselectRow', index);
                },
                onLoadSuccess:function(data){
                    SetMergeCells(data, 'KeyType','KeyType');
                    SetMergeCells(data, 'KeyType','EditGroup');
                }
            });
            //批量设置访问级别对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var dataModel = {
                            ids: group_ids,
                            GroupName: $.trim($('#txtGroupName').val())
                        };
                        if(dataModel.GroupName == ""){
                            $.messager.alert('系统提示', '请输入分组名称');
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: '/Serv/API/Admin/CompanyWebsite/ToolBar/UpdateGroupName.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    group_ids = 0;
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    $.messager.show({
                                        title: '系统提示',
                                        msg: resp.msg
                                    });
                                    window.location.href=window.location.href;
                                }
                                else {
                                    $.messager.alert('系统提示', resp.msg);
                                }
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

            $("#ddlGroup").change(function(){
            
                Search();
            
            }
                )
            $("#ddlIsPc").change(function(){
            
                Search();
            
            }
    )

        });

        function SetMergeCells(data, colField, mergeCell){
            var nValue = null;
            var startIndex = 0;
            for (var i = 0; i < data.rows.length; i++) {
                if(i ==0){
                    nValue = data.rows[i][colField];
                    startIndex = i;
                }
                if(nValue != data.rows[i][colField] &&  i>0 ){
                    $(grvData).datagrid('mergeCells',{
                        index: startIndex,
                        field: mergeCell,
                        rowspan: i - startIndex
                    });
                    nValue = data.rows[i][colField];
                    startIndex = i;
                }
            }
            $(grvData).datagrid('mergeCells',{
                index: startIndex,
                field: mergeCell,
                rowspan: data.rows.length - startIndex
            });
        }
        
        //删除
        function ToDeletePage(rowIndex) {
            var rows = $('#grvData').datagrid('getRows'); //获取选中的行
            var row = rows[rowIndex];
            var id = 0;
            if(is_system !=1 && row.IsSystem != 1) id = row.AutoID;
            if (id == 0) {
                Alert("基础导航不能删除");
                return;
            }
            $.messager.confirm("系统提示", "确定删除导航["+row.ToolBarName+"]选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteCompanyWebsiteToolBar", ids: id },
                        success: function (result) {
                            Alert(result);
                            $('#grvData').datagrid('reload');
                        }
                    });
                }
            });
        }
        function ToEditGroup(rowIndex){
            var rows = $('#grvData').datagrid('getRows'); //获取选中的行
            var row = rows[rowIndex];
            var ids = [];
            for (var i = rowIndex; i < rows.length; i++) {
                if(row.KeyType != rows[i].KeyType) break;
                ids.push(rows[i].AutoID);
            }
            group_ids = ids.join(',');
            $('#txtGroupName').val(row.KeyType);
            $('#dlgInfo').dialog('open');
            
        }
        //删除
        function ToDeleteGroup(rowIndex) {
            var rows = $('#grvData').datagrid('getRows'); //获取选中的行
            var row = rows[rowIndex];
            var ids = [];
            var str = "确定删除分组["+row.KeyType+"]的非基础导航?";
            for (var i = rowIndex; i < rows.length; i++) {
                if(is_system !=1 && rows[i].IsSystem == 1) continue;
                if(row.KeyType != rows[i].KeyType) break;
                ids.push(rows[i].AutoID);
            }
            <%if (userType != 1){%>
            str = "确定删除分组["+row.KeyType+"]的非基础导航导航?";
            <%}%>

            if(ids.length ==0){
                Alert("仅能删除非基础导航");
                return;
            }
            $.messager.confirm("系统提示", str, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteCompanyWebsiteToolBar", ids:  ids.join(',') },
                        success: function (result) {
                            Alert(result);
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });
        }
        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = [];
            var str = "确定删除选中导航?";
            for (var i = 0; i < rows.length; i++) {
                if(is_system !=1 && rows[i].IsSystem == 1) continue;
                ids.push(rows[i].AutoID);
            }
            <%if (userType != 1)
        {%>
            str = "确定删除选中(非基础)导航?";
            <%}%>
            if(ids.length ==0){
                Alert("请选择导航");
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteCompanyWebsiteToolBar", ids: ids.join(',') },
                        success: function (result) {
                            Alert(result);
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });
        }

        function ToEditPage(Action,aid,use_type,name){
            var key_type = $.trim($("#ddlGroup").val());
            key_type = encodeURIComponent(key_type);
            var str = new StringBuilder();
            str.AppendFormat("/App/CompanyWebsite/ToolBarCompile.aspx?Action={0}&aid={1}&use_type={2}&key_type={3}&is_system={4}&isPc={5}",Action,aid,use_type,key_type,<%=this.Request["is_system"] =="1"?1:0  %>,'<%=this.Request["ispc"] %>');
            
            var title = Action == 'add'? '添加':'编辑-' + name;

            top.addTab('导航管理-' + title,str.ToString());
            
            //document.location.href=str.ToString();


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


        function Search() {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "QueryCompanyWebsiteToolBar",UseType:use_type,KeyType:$("#ddlGroup").val(), ToolBarName: $("#txtName").val(),IsSystem:<%=this.Request["is_system"] =="1"?1:0  %>,IsPc:"<%=Request["isPc"]%>" }
                   });
               }; 
               function loadGroups(){
                   var appendhtml = new StringBuilder();
                   appendhtml.AppendFormat('<option value="">全部</option>');
                   for (var i = 0; i < groups.length; i++) {
                       appendhtml.AppendFormat('<option value="{0}">{0}</option>',groups[i]);
                   }
                   if(groups.length == 0) appendhtml.AppendFormat('<option value="商品导航">商品导航</option>');
                   $("#ddlGroup").append(appendhtml.ToString());
               }
               //排序保存
               function UpdateSortIndex(id){
            
                   if(id==null||id==0){
                       return false;
                   }
                   var re = /^[1-9]+[0-9]*]*$/;
                   if (!re.test($("#txtIndex"+id+"").val())) {
                       if($("#txtIndex"+id+"").val()==0){
                       }else{
                           alert("请输入正整数");
                           $("#txtIndex" + id).val("");
                           $("#txtIndex" + id).focus();
                           return false;
                       }
                   }


                   $.ajax({
                       type: 'post',
                       url: handlerUrl,
                       data: { Action: "UpdateSortIndex", ToolBarID: id, SortIndex: $("#txtIndex"+id+"").val() },
                       dataType: "json",
                       success: function (resp) {
                           if (resp.Status == 1) {
                               $('#grvData').datagrid("reload");
                           }
                           else {
                               Alert(resp.Msg);
                           }
                 

                       }
                   });
               }
    </script>
    <% = icoScript %>
</asp:Content>
