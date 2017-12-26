<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PubMenuManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.PubMenuManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
   
    当前位置：&nbsp;公众号设置&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>公众号菜单设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div >
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="NodeName" width="30">
                    菜单名称
                </th>
                <th field="Type" formatter="FormartMenuType" width="10">
                    菜单类型
                </th>
                <th field="KeyOrUrl" width="50">
                   链接或关键字
                </th>
            </tr>
        </thead>
    </table>
    </div>
    <div id="divToolbar" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0);" title="上移" class="easyui-linkbutton" plain="true" onclick="MoveStep('up')">
                <img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a> <a href="javascript:void(0);" title="下移"
                    class="easyui-linkbutton" plain="true" onclick="MoveStep('down')">
                    <img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a> <a href="javascript:;"
                        class="easyui-linkbutton" iconcls="icon-add2" plain="true" title="添加菜单" onclick="ShowAdd()"
                        id="btnAdd" runat="server">添加菜单</a> <a href="javascript:;" class="easyui-linkbutton"
                            iconcls="icon-edit" plain="true" title="编辑菜单" onclick="ShowEdit()" id="btnEdit"
                            runat="server">编辑菜单 </a><a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete"
                                plain="true" title="删除菜单" onclick="Delete()" id="btnDelete" >删除菜单
                            </a><a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                                title="发布" id="A1" onclick="CreateWeixinClientMenu()" runat="server">发布
            </a>
            <br />
            <span>提示：1.可以配置2-3个一级菜单，每个一级菜单下可以配置2-5个子菜单。<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2.编辑中的菜单不能直接在用户手机上生效，你需要进行发布, 发布后24小时内所有的用户都将更新到新的菜单。<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3.发布自定义菜单后，由于微信客户端缓存，需要24小时微信客户端才会展现出来。建议测试时可以尝试取消关注公众账号后，<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;再次关注，则可以看到创建后的效果。
            </span>
            <br />
        </div>
    </div>
    <div id="dlgMenuInfo" class="easyui-dialog" title="菜单" modal="true" closed="true" style="width: 380px;
       padding: 10px">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td height="25" align="left" class="style1">
                        上级菜单：
                    </td>
                    <td height="25" width="*" align="left">
                        <span id="spmenu"></span>
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        菜单名称：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtNodeName" style="width: 200px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入菜单名称" />
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        菜单类型：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="radio" id="rdview" checked="checked" name="rdomenutype" />
                        <label for='rdview'>
                            链接</label>
                        <input type="radio" id="rdclick" name="rdomenutype" />
                        <label for='rdclick'>
                            关键字回复</label>
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        链接或关键字：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtUrlOrKey" style="width: 200px;" class="easyui-validatebox"
                            required="true" missingmessage="不能为空" />
                    </td>
                </tr>
                   <tr>
                    <td height="25" align="left" class="style1">
                        排序：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" onkeyup="value=value.replace(/[^\d]/g,'') " id="txtMenuSort" style="width: 50px;" class="easyui-validatebox"
                            required="true" missingmessage="从小到大排序" />
                    </td>
                </tr>

            </table>
            
        </div>
    </div>

   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var grid;
     var currSelectID = 0;
     var currentAction = "";
     $(function () {

       






         //-----------------加载gridview
         grid = $("#grvData").datagrid({
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




         //加载菜单
         LoadMenuSelectList();

         $('#dlgMenuInfo').dialog({
             buttons: [{
                 text: "确定",
                 handler: function () {
                     Save();
                     //                     if (currentAction == "add") {
                     //                         Add();
                     //                     }
                     //                     else if (currentAction == "edit") {
                     //                         Edit();


                     //                     }
                 }
             }, {
                 text: "取消",
                 handler: function () {
                     $('#dlgMenuInfo').dialog('close');
                 }
             }]
         });

           <%
            ZentCloud.BLLJIMP.BLLWeixin bllWeixin = new ZentCloud.BLLJIMP.BLLWeixin();
            if (string.IsNullOrEmpty(bllWeixin.GetAccessToken(bllWeixin.WebsiteOwner)))
            {
               %>
         $.messager.confirm('系统提示', '公众号未接入,是否前去接入?', function (o) {
                             if (o) {
                                 window.location.href = "/App/Cation/PubConfig.aspx";
                             }
                     })
                <%
            }
            
         %>

     });

//     function LoadData() {
//         grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryWeixinMenu"} });
//     }


     function ShowAdd() {
         ClearWinDataByTag('input', dlgMenuInfo);
         //加载菜单
         LoadMenuSelectList();
         $('#dlgMenuInfo').dialog('open');
         currentAction = "AddWeixinMenu";

     }

     function ShowEdit() {
         var rows = grid.datagrid('getSelections');
         if (!EGCheckNoSelectMultiRow(rows)) {
             return;
         }
         ClearWinDataByTag('input', dlgMenuInfo);


         $('#dlgMenuInfo').dialog('open');
         currentAction = "EditWeixinMenu";
         //加载编辑数据
         currSelectID = rows[0].MenuID;
         $('#ddlPreMenu').val(rows[0].PreID);
         $(txtNodeName).val($.trim(rows[0].NodeName).replace('└', ''));
         $(txtUrlOrKey).val(rows[0].KeyOrUrl);
         $(txtMenuSort).val(rows[0].MenuSort);
         //            var ishide = rows[0].IsHide;
         //            if (ishide == "1") {
         //                $("#rdshow").attr("checked", true);
         //            } else {
         //                $("#rdhide").attr("checked", true);
         //            }


         var menuType = $.trim(rows[0].Type);
         if (menuType == "view") {
             $("#rdview").attr("checked", true);


         } else if (menuType == "click") {
             $("#rdclick").attr("checked", true);


         }


     }

//     function Add() {
//         try {
//             var model = GetDlgModel();
//             if (!CheckDlgInput(model)) {
//                 return false;
//             }

//             $.ajax({
//                 type: "Post",
//                 url: handlerUrl,
//                 data: { Action: "AddWeixinMenu", JsonData: JSON.stringify(model).toString() },
//                 success: function (result) {
//                     if (result == "true") {
//                         Alert("添加成功");
//                         grid.datagrid('reload');
//                         $("#dlgMenuInfo").dialog("close");
//                         LoadMenuSelectList();
//                     } else {

//                         Alert(result);
//                     }
//                 }
//             });

//         } catch (e) {
//             alert(e);
//         }
//     }

//     function Edit() {
//         try {
//             var model = GetDlgModel();
//             if (!CheckDlgInput(model)) {
//                 return false;
//             }
//             $.ajax({
//                 type: "Post",
//                 url: handlerUrl,
//                 data: { Action: "EditWeixinMenu", JsonData: JSON.stringify(model).toString() },
//                 success: function (result) {
//                     if (result == "true") {
//                         //Alert("保存成功");
//                         grid.datagrid('reload');
//                         $("#dlgMenuInfo").window("close");
//                         LoadMenuSelectList();
//                     } else {

//                         Alert(result);
//                     }
//                 }
//             });

//         } catch (e) {
//             alert(e);
//         }

//     }
        
     //保存
     function Save() {
         try {
             var model = GetDlgModel();
             if (!CheckDlgInput(model)) {
                 return false;
             }
             $.ajax({
                 type: "Post",
                 url: handlerUrl,
                 data: { Action: currentAction, JsonData: JSON.stringify(model).toString() },
                 success: function (result) {
                     if (result == "true") {
                         grid.datagrid('reload');
                         $("#dlgMenuInfo").window("close");
                         LoadMenuSelectList();
                     } else {

                         Alert(result);
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
                         Alert('已删除数据' + result + '条');
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
                "KeyOrUrl": $.trim($(txtUrlOrKey).val()),
                "MenuSort": $.trim($(txtMenuSort).val())

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
         if (model['MenuSort'] == '') {
             $(txtMenuSort).val("");
             $(txtMenuSort).focus();
             return false;
         }
         //if (model["Type"] == 'view') {
         //    if (!IsURL(model["KeyOrUrl"])) {
         //        $(txtUrlOrKey).val("");
         //        $(txtUrlOrKey).focus();
         //        $.messager.alert("系统提示", '链接格式不正确')
         //        return false;
         //    }
         //}
         
         return true;
     }

     //加载上级菜单
     function LoadMenuSelectList() {
         $.post(handlerUrl, { Action: "GetMenuSelectList" }, function (data) {
             $("#spmenu").html(data);
             
         });
     }

     //格式化菜单类型
     function FormartMenuType(value) {
         if ($.trim(value) == "view") {
             return "链接";
         }
         else {
             return "关键字回复";
         }


     }

     //生成微信客户端菜单
     function CreateWeixinClientMenu() {
         $.messager.confirm('系统提示', '确定发布？', function (o) {
             if (o) {

                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "CreateWeixinClientMenu" },
                     success: function (result) {
                         $.messager.alert("系统提示", result)

                     }
                 });
             }
         });


     }

     //上移下移菜单
     function MoveStep(direction) {
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
//         var direction = "";
//         if (dir == 0) {
//             direction = "up";

//         }
//         else if (dir == 1) {
//             direction = "down";
//         }
         $.ajax({
             type: "Post",
             url: handlerUrl,
             data: { Action: "MoveMenu", MenuID: rows[0].MenuID, Direction: direction },
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

     //扩展GridView
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

</asp:Content>
