<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeixinMenu.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var handlerUrl = '/Handler/WeiXin/WeixinHandler.ashx';
        var grid;
        var currSelectID = 0;

        $(function () {


            $(window).resize(function () {
                $(list_data).datagrid('resize',
	            {
	                width: document.body.clientWidth,
	                height: document.documentElement.clientHeight
	            });
            });
            //-----------------加载gridview
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight,
                toolbar: '#divToolbar',
                idField: "MenuID", 
                view: fileview,
                fitCloumns: true,
                pagination: false,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryWeixinMenu" }
            });
            //------------加载gridview

            //窗体关闭按钮---------------------
            $("#dlgMenuInfo").find("#btnExit").bind("click", function () {
                $("#dlgMenuInfo").window("close");
            });

            //窗体保存按钮---------------------
            $("#btnSave").bind("click", function () {

                var tag = jQuery.trim(jQuery(this).attr("tag"));

                if (tag == "add") {
                    //添加
                    Add();
                    return;
                }
                else {
                    //修改
                    Edit();
                    return;
                }
            });

//            //查询按钮点击绑定
//            $("#btnSearch").click(function () {
//                var searchReq = $.trim($(txtSearchUserID).val());
//                grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryWeixinMenu", SearchReq: searchReq} });

//            });

            //加载菜单
            LoadMenuSelectList();

        });

        function LoadData() {
            grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryWeixinMenu"} });
        }


        function ShowAdd() {
            ClearWinDataByTag('input', dlgMenuInfo);
            //加载菜单
            LoadMenuSelectList();
            $('#dlgMenuInfo').window(
            {
                title: '添加菜单'
            }
            );

            $('#dlgMenuInfo').dialog('open');
            $(btnSave).attr('tag', 'add');

        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            ClearWinDataByTag('input', dlgMenuInfo);

            $('#dlgMenuInfo').window(
            {
                title: '编辑菜单'
            }
            );

            $('#dlgMenuInfo').dialog('open');
            $(btnSave).attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].MenuID;
            $('#ddlPreMenu').val(rows[0].PreID);
            $(txtNodeName).val($.trim(rows[0].NodeName).replace('└', ''));
            $(txtUrlOrKey).val(rows[0].KeyOrUrl);
          
//            $(txtMenuSort).val(rows[0].MenuSort);
//            var ishide = rows[0].IsHide;
//            if (ishide == "1") {
//                $("#rdshow").attr("checked", true);
//            } else {
//                $("#rdhide").attr("checked", true);
//            }


            var menutype =$.trim(rows[0].Type);
            if (menutype == "view") {
                $("#rdview").attr("checked", true);
                         
                
            } else if (menutype == "click") {
                $("#rdclick").attr("checked", true);
                               
               
            }
                           

        }

        function Add() {
            try {
                var model = GetDlgModel();
                if (!CheckDlgInput(model)) {
                    return false;
                }

                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "AddWeixinMenu", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '添加成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgMenuInfo").window("close");
                            LoadMenuSelectList();
                        } else {
                            $.messager.alert("系统提示", "添加失败：" + result);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }
        }

        function Edit() {
            try {
                var model = GetDlgModel();
                if (!CheckDlgInput(model)) {
                    return false;
                }
                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "EditWeixinMenu", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '编辑成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgMenuInfo").window("close");
                            LoadMenuSelectList();
                        } else {
                            $.messager.alert("系统提示", "编辑失败：" + result);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }

        }

        //批量删除
        function Delete() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确定删除选中菜单？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].MenuID);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWeixinMenu", ids: ids.join(',') },
                        success: function (result) {
                            $.messager.show({
                                title: '系统提示',
                                msg: '已删除数据' + result + '条'
                            });
                            grid.datagrid('reload');
                        }
                    });
                }
            });
        }

        //获取对话框数据实体
        function GetDlgModel() {
            var model =
            {
                "MenuID": currSelectID,
                "NodeName": $.trim($(txtNodeName).val()),
                "PreID": $('#ddlPreMenu').val(),
                "Type": rdview.checked ? "view" : "click",
                "KeyOrUrl": $.trim($(txtUrlOrKey).val())
        
//                "IsHide": rdhide.checked ? 0 : 1
            }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['NodeName'] == '') {
                $(txtNodeName).val("");
                $(txtNodeName).focus();
                return false;
            }
            if (model['KeyOrUrl'] == '') {
                $("#txtUrlOrKey").val("");
                $("#txtUrlOrKey").focus();
                return false;
            }
            return true;
        }

        //加载菜单选择列表
        function LoadMenuSelectList() {
            $.post(handlerUrl, { Action: "GetMenuSelectList" }, function (data) {
                $("#sp_menu").html(data);
            });
        }


        function formartmenutype(value) {
            if ($.trim(value) == "view") {
                return "链接";
            }
            else {
                return "点击";
            }


        }

        //生成微信客户端菜单
        function CreateWeixinClientMenu() {

            $.messager.confirm('系统提示', '确定发布？', function (o) {
                if (o) {

                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "CreateWeixinClientMenu"},
                        success: function (result) {
                            $.messager.alert("系统提示",result)
                           
                        }
                    });
                }
            });


        }

        function MoveStep(dir) {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                $.messager.alert("系统提示", "请选择一个菜单进行操作！", "info");
                return;
            }
//            if (num > 1) {
//                $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
//                return;
//            }
            var direction = "";
            if (dir == 0) {
                direction = "up";

            }
            else if (dir == 1) {
                direction = "down";
            }
            jQuery.ajax({
                type: "Post",
                url: handlerUrl,
                data: { Action: "MoveMenu", MenuID: rows[0].MenuID,Direction: direction },
                success: function (result) {
                    if (result == "true") {
                        messager('系统提示', "操作成功！");
                        grid.datagrid('reload');

                        return;
                    }
                    $.messager.alert("系统提示", result, "info");
                }
            });


        }

        var fileview = $.extend({}, $.fn.datagrid.defaults.view, { onAfterRender: function (target) { ischeckItem(); } });

        var checkedItems = [];
        function ischeckItem() {

            for (var i = 0; i < checkedItems.length; i++) {
                grid.datagrid('selectRecord', checkedItems[i]); //根据id选中行 

            }



        }

        function findCheckedItem(ID) {
            for (var i = 0; i < checkedItems.length; i++) {
                if (checkedItems[i] == ID) return i;
            }
            return -1;
        }

        function addcheckItem() {
            var row = grid.datagrid('getChecked');
            for (var i = 0; i < row.length; i++) {
                if (findCheckedItem(row[i].MenuID) == -1) {
                    checkedItems.push(row[i].MenuID);
                }
            }
        }
        function removeAllItem(rows) {

            for (var i = 0; i < rows.length; i++) {
                var k = findCheckedItem(rows[i].MenuID);
                if (k != -1) {
                    checkedItems.splice(i, 1);
                }
            }
        }
        function removeSingleItem(rowIndex, rowData) {
            var k = findCheckedItem(rowData.MenuID);
            if (k != -1) {
                checkedItems.splice(k, 1);
            }
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


                <table id="list_data" fitcolumns="true">
                <thead>
                    <tr>
                        
                        <th field="NodeName" width="30">
                            菜单名称
                        </th>
                        <th field="Type" formatter="formartmenutype" width="10">
                            菜单类型
                        </th>
                       
                        <th field="KeyOrUrl" width="50">
                            Key值或链接地址
                        </th>
                       
                    </tr>
                </thead>
            </table>
    <div id="divToolbar"  style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
        
   <a href="#" title="上移" class="easyui-linkbutton"  plain="true" onclick="MoveStep(0)">
                             <img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a> 
                            <a href="#" title="下移" class="easyui-linkbutton"  plain="true" onclick="MoveStep(1)">
                             <img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a>
                             
       
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                title="添加菜单" onclick="ShowAdd()" id="btnAdd" runat="server">添加菜单</a> 

           

        
                <a href="javascript:;"
                    class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑菜单" onclick="ShowEdit()"
                    id="btnEdit" runat="server">编辑菜单 </a>
                   

       
                       
                    <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-remove" plain="true" title="删除菜单" onclick="Delete()" id="btnDelete"
                        runat="server">删除菜单 </a>
                       
                        <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-edit" plain="true" title="发布"  id="A1" onclick="CreateWeixinClientMenu()"
                        runat="server">发布 </a>


                        <br />
                        
                        <span>提示：1.可以配置2-3个一级菜单，每个一级菜单下可以配置2-5个子菜单。<br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.编辑中的菜单不能直接在用户手机上生效，你需要进行发布,
                                    发布后24小时内所有的用户都将更新到新的菜单。<br/>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.发布自定义菜单后，由于微信客户端缓存，需要24小时微信客户端才会展现出来。建议测试时可以尝试取消关注公众账号后，再次关注，则可以看到创建后的效果。
                                  
                                   
                        </span>
                        <br />
        </div>
    </div>
    <div id="dlgMenuInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 380px;
        height: 250px; padding: 10px">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td height="25" align="left" class="style1">
                        上级菜单：
                    </td>
                    <td height="25" width="*" align="left">
                        <span id="sp_menu"></span>
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        菜单名称：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtNodeName" style="width: 200px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入菜单名称"  />
                    </td>
                </tr>

                
                <tr>
                    <td height="25" align="left" class="style1">
                        菜单类型：
                    </td>
                    <td height="25" width="*" align="left">
                       <input type="radio" id="rdview" checked="checked" name="rdomenutype"/>
                                <label for='rdview'>
                                    链接</label>
                                <input type="radio" id="rdclick" name="rdomenutype" />
                                <label for='rdclick'>
                                    点击</label>
                    </td>
                </tr>
                                <tr>
                    <td height="25" align="left" class="style1">
                        链接地址或Key：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtUrlOrKey" style="width:200px;" class="easyui-validatebox"
                            required="true" missingmessage="不能为空" />
                    </td>
                </tr>
               

              
            </table>
            <div style="margin-top: 15px; margin-left: 90px;">
                <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                    确定</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                        关 闭</a>
            </div>
        </div>
    </div>
</asp:Content>