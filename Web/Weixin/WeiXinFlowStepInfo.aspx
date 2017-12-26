<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeiXinFlowStepInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeiXinFlowStepInfo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

    <script type="text/javascript">


         var grid;
         
         //处理文件路径
         var url = "/Handler/WeiXin/WeiXinFlowStepInfoManage.ashx";
         
         //传入的流程ID
         var FlowID = <%=FlowID %>;


         //加载文档
         jQuery().ready(function () {
             
                $(window).resize(function () {
                $(list_data).datagrid('resize',
	            {
	                width: document.body.clientWidth - 10,
	                height: document.documentElement.clientHeight - 120
	            });
            });
             grid = jQuery("#list_data").datagrid({
                 method: "Post",
                idField: "AutoID", 
                view: fileview ,
                 url: url,
                  height: document.documentElement.clientHeight - 120,
                 pageSize: 10,
                 pagination: true,
                 rownumbers: true,
                 singleSelect: false,
                 queryParams: { Action: "Query",FlowID:FlowID ,SearchTitle: "" }
          
             });
             
             //取消---------------------
             $("#win").find("#btnExit").bind("click", function () {
                 $("#win").window("close");
             });
             //取消---------------------



             //搜索------------------------
             $("#btnSearch").click(function () {
                 var SearchTitle = $("#txtName").val();
                 grid.datagrid({ url: url, queryParams: {Action: "Query",FlowID:FlowID ,SearchTitle: SearchTitle} });
             });
             //搜索------------------------

             //验证下拉框改变
              $("#ddlAuthFunc").change(function () {
              if ($(this).val()=="phone") {
                    $("#divIsVerifyCode").show();
                    }

                    else {
                     $("#divIsVerifyCode").hide();
    
                    }
             });
             //验证下拉框改变


            //保存---------------------
            $("#btnSave").bind("click", function () {

//                var tag = jQuery.trim(jQuery(this).attr("tag"));

//                if (tag == "add") {
//                    //添加
//                    Add();
//                    return;
//                }
//                //修改
               Save();
            });
        });
        //保存---------------------
        
        //弹出添加或编辑框开始
        function ShowAddOrEdit(addoredit) {
            var title = ""; //弹出框标题
            var titleicon = "icon-" + addoredit; //弹出框标题图标
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                Clear("txtFlowField|txtFieldDescription|txtSendMsg|txtErrorMsg|ddlAuthFunc");
                $("#divIsVerifyCode").hide();
                //设置弹出框标题
                title = "添加";

            }
            //弹出添加框结束

            //弹出编辑框开始
            else if (addoredit == "edit") {
                // 只能选择一条记录操作
                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager('系统提示', "请选择一条记录进行操作！");
                    return;
                }
                if (num > 1) {
                    $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                    return;
                }
                // 只能选择一条记录操作

             //加载信息开始
            $("#txtFlowField").val(rows[0].FlowField);
            $("#txtFieldDescription").val(rows[0].FieldDescription);
            $("#txtSendMsg").val(rows[0].SendMsg);
            $("#txtErrorMsg").val(rows[0].ErrorMsg);
            var authfunc=rows[0].AuthFunc;
            $("#ddlAuthFunc").val(authfunc);
            if (authfunc=="phone")
             {
                if (rows[0].IsVerifyCode=="1") {
                 $("#rd1").attr("checked",true)
                    }
                    else if (rows[0].IsVerifyCode=="0")  {
                         $("#rd0").attr("checked",true)
                    }
                $("#divIsVerifyCode").show();
               
             }
             else {
                     $("#divIsVerifyCode").hide();
                }                //加载信息结束

                //设置弹出框标题
                title = "编辑";


            }
            //弹出编辑框结束


            //弹出对话框
            $("#win").window({
                title: title,
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: titleicon,
                resizable: false,
                width: 350,
                height: 250,
                top: ($(window).height() - 250) * 0.5,
                left: ($(window).width() - 350) * 0.5

            });
            //弹出对话框

            //设置保存按钮属性 add为添加，edit为编辑
            $("#btnSave").attr("tag", addoredit);


        }
        //展示添加或编辑框结束


        //添加或编辑操作开始---------
        function Save() {
            var FlowField = $("#txtFlowField").val();
            var FieldDescription = $("#txtFieldDescription").val();
            var SendMsg = $("#txtSendMsg").val();
            var ErrorMsg = $("#txtErrorMsg").val();
            var AuthFunc = $("#ddlAuthFunc").val();
            if (FlowField == "") {
                $("#txtFlowField").focus();
                return false;

            }
            if (SendMsg == "") {
                $("#txtSendMsg").focus();
                return false;

            }
            if (AuthFunc!="") {
               if (ErrorMsg == "") {
                $("#txtErrorMsg").focus();
                return false;

            }
             }
            var IsVerifyCode = "";
            if ($("#rd1").attr("checked")) {
                IsVerifyCode = 1;
            }
            else if ($("#rd0").attr("checked")) {
                IsVerifyCode =0;
            }
            var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
            //----------执行添加操作开始
            if (action == "add") {
            //------------添加
            jQuery.ajax({
                type: "Post",
                url: url,
                data: { Action: "Add",FlowID:FlowID,FlowField: FlowField, FieldDescription: FieldDescription, SendMsg: SendMsg,ErrorMsg:ErrorMsg,AuthFunc:AuthFunc,IsVerifyCode:IsVerifyCode },
                success: function (result) {
                    if (result == "true") {
                        messager("系统提示", "添加成功");
                        $("#win").window("close");
                        grid.datagrid('reload');
                    } else {
                        messager("系统提示", "添加失败");
                    }
                }
            });
                //添加---------------
            }
            //-----------执行添加操作结束
            //-----------执行编辑操作开始
            else if (action == "edit") {
            //-----------修改
            var rows = grid.datagrid('getSelections');
            var StepID=rows[0].StepID;
            jQuery.ajax({
                type: "Post",
                url: url,
                data: { Action: "Edit",FlowID:FlowID,StepID:StepID, FlowField: FlowField, FieldDescription: FieldDescription, SendMsg: SendMsg,ErrorMsg:ErrorMsg,AuthFunc:AuthFunc,IsVerifyCode:IsVerifyCode },
                success: function (result) {
                    if (result == "true") {
                        messager("系统提示", "修改成功");
                        $("#win").window("close");
                        grid.datagrid('reload');
                    }
                    else {
                        messager("系统提示", "修改失败");
                    }
                }
            });
                //修改
            }
            //--------------执行编辑操作结束

        }
        //添加或编辑操作结束---------


//         //添加步骤弹出框---------------------
//         function ShowAdd() {
//             $("#win").window({
//                 title:"添加" ,
//                 closed: false,
//                 collapsible: false,
//                 minimizable: false,
//                 maximizable: false,
//                 iconCls: "icon-add",
//                 resizable: false,
//                 width: 350,
//                 height: 250,
//                top:($(window).height() - 350) * 0.5,   
//                left:($(window).width() - 250) * 0.5

//             });
//             //设置保存按钮目标为添加
//             $("#btnSave").attr("tag", "add");
//            Clear();
//          


//         }
//        //添加题目弹出框---------------------
         


//  //保存添加的信息---------------------
//        function Add() {
//        
//            var FlowField = $("#txtFlowField").val();
//            var FieldDescription = $("#txtFieldDescription").val();
//            var SendMsg = $("#txtSendMsg").val();
//            var ErrorMsg = $("#txtErrorMsg").val();
//            var AuthFunc = $("#ddlAuthFunc").val();
//            if (FlowField == "") {
//                $("#txtFlowField").focus();
//                return false;

//            }
//            if (SendMsg == "") {
//                $("#txtSendMsg").focus();
//                return false;

//            }
//            if (ErrorMsg == "") {
//                $("#txtErrorMsg").focus();
//                return false;

//            }
//            var IsVerifyCode = "";
//            if ($("#rd1").attr("checked")) {
//                IsVerifyCode = 1;
//            }
//            else if ($("#rd0").attr("checked")) {
//                IsVerifyCode =0;
//            }
//            jQuery.ajax({
//                type: "Post",
//                url: url,
//                data: { Action: "Add",FlowID:FlowID,FlowField: FlowField, FieldDescription: FieldDescription, SendMsg: SendMsg,ErrorMsg:ErrorMsg,AuthFunc:AuthFunc,IsVerifyCode:IsVerifyCode },
//                success: function (result) {
//                    if (result == "true") {
//                        messager("系统提示", "添加成功");
//                        $("#win").window("close");
//                        grid.datagrid('reload');
//                    } else {
//                        messager("系统提示", "添加失败");
//                    }
//                }
//            });
//        };
//        //保存添加的信息---------------------


//        // 修改信息---------------------
//        function Save() {
//       
//            var rows = grid.datagrid('getSelections');
//            var FlowID=rows[0].FlowID;
//            var StepID=rows[0].StepID;
//            var FlowField = $("#txtFlowField").val();
//            var FieldDescription = $("#txtFieldDescription").val();
//            var SendMsg = $("#txtSendMsg").val();
//            var ErrorMsg = $("#txtErrorMsg").val();
//            var AuthFunc = $("#ddlAuthFunc").val();
//            if (FlowField == "") {
//                $("#txtFlowField").focus();
//                return false;

//            }
//            if (SendMsg == "") {
//                $("#txtSendMsg").focus();
//                return false;

//            }
//            if (ErrorMsg == "") {
//                $("#txtErrorMsg").focus();
//                return false;

//            }
//           var IsVerifyCode = "";
//            if ($("#rd1").attr("checked")) {
//                IsVerifyCode = 1;
//            }
//            else if ($("#rd0").attr("checked")) {
//                IsVerifyCode =0;
//            }


//            jQuery.ajax({
//                type: "Post",
//                url: url,
//                data: { Action: "Edit",FlowID:FlowID,StepID:StepID, FlowField: FlowField, FieldDescription: FieldDescription, SendMsg: SendMsg,ErrorMsg:ErrorMsg,AuthFunc:AuthFunc,IsVerifyCode:IsVerifyCode },
//                success: function (result) {
//                    if (result == "true") {
//                        messager("系统提示", "修改成功");
//                        $("#win").window("close");
//                        grid.datagrid('reload');
//                    }
//                    else {
//                        messager("系统提示", "修改失败");
//                    }
//                }
//            });
//        }
//        // 修改信息---------------------





         // 删除---------------------
         function Delete() {
             var rows = grid.datagrid('getSelections');
             var num = rows.length;
             if (num == 0) {
                 messager("系统提示", "请选择您要删除的记录");
                 return;
             }
             var ids = [];

             for (var i = 0; i < rows.length; i++) {
                 ids.push(rows[i].StepID);
             }

             $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                 if (r) {
                     jQuery.ajax({
                         type: "Post",
                         url: url,
                         data: { Action: "Delete", StepID: ids.join(','),FlowID:FlowID },
                         success: function (result) {
                             if (result=="true") {
                                 messager('系统提示', "删除成功！");
                                 grid.datagrid('reload');
                              
                                 return;
                             }
                                messager('系统提示', "删除失败！");
                         }
                     });
                 }
             });
         };
      //删除---------------------



//        //展示编辑框---------------------
//        function Edit() {
//            var rows = grid.datagrid('getSelections');
//            var num = rows.length;
//            if (num == 0) {
//                messager('系统提示', "请选择一条记录进行操作！");
//                return;
//            }
//            if (num > 1) {
//                $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
//                return;
//            }
//            $("#win").window({
//                title: "修改",
//                closed: false,
//                collapsible: false,
//                minimizable: false,
//                maximizable: false,
//                iconCls: "icon-edit",
//                resizable: false,
//                width: 350,
//                height: 250,
//                top:($(window).height() - 350) * 0.5,   
//                left:($(window).width() - 250) * 0.5

//            });
//            //加载信息
//            $("#txtFlowField").val(rows[0].FlowField);
//            $("#txtFieldDescription").val(rows[0].FieldDescription);
//            $("#txtSendMsg").val(rows[0].SendMsg);
//            $("#txtErrorMsg").val(rows[0].ErrorMsg);
//            var authfunc=rows[0].AuthFunc;
//            $("#ddlAuthFunc").val(authfunc);
//            if (authfunc=="phone")
//             {
//                if (rows[0].IsVerifyCode=="1") {
//                 $("#rd1").attr("checked",true)
//                    }
//                    else if (rows[0].IsVerifyCode=="0")  {
//                         $("#rd0").attr("checked",true)
//                    }
//                $("#divIsVerifyCode").show();
//               
//             }
//             else {
//                     $("#divIsVerifyCode").hide();
//                }

//                   

//            $("#btnSave").attr("tag", "Edit");
//        }
//        //展示编辑框--------------


//        //清除数据-----------
//        function Clear() {
//            $("#txtFlowField").val("");
//            $("#txtFieldDescription").val("");
//            $("#txtSendMsg").val("");
//            $("#txtErrorMsg").val("验证失败，请重新输入");
//            $("#ddlAuthFunc").val("");
//             $("#divIsVerifyCode").hide();
//        }

//        //清除数据-----------

        function MoveStep(dir){
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager('系统提示', "请选择一条记录进行操作！");
                return;
            }
            if (num > 1) {
                $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                return;
            }
            var direction="";
            if (dir==0) {
            direction="up";
    
            }
            else if (dir==1) {
            direction="down";
        }
                         jQuery.ajax({
                         type: "Post",
                         url: url,
                         data: { Action: "MoveStep", StepID: rows[0].StepID,FlowID:FlowID,Direction:direction },
                         success: function (result) {
                             if (result=="true") {
                                 messager('系统提示', "操作成功！");
                                 grid.datagrid('reload');
                             
                                 return;
                             }
                                messager('系统提示', result);
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
               if (findCheckedItem(row[i].AutoID) == -1) {
                   checkedItems.push(row[i].AutoID);
               }
           }
       }
       function removeAllItem(rows) {

           for (var i = 0; i < rows.length; i++) {
               var k = findCheckedItem(rows[i].AutoID);
               if (k != -1) {
                   checkedItems.splice(i, 1);
               }
           }
       }
       function removeSingleItem(rowIndex, rowData) {
           var k = findCheckedItem(rowData.AutoID);
           if (k != -1) {
               checkedItems.splice(k, 1);
           }
       }
        function changestate(value,row) {

            if (row.AuthFunc =="phone") {
            if (value=="1") {
              return "<font color='green'>是</font>";
    
                }
               else{
                return "<font color='red'>否</font>";
               
               }
              

        }
        return "";
           
        
        
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="center" style="margin: 5px;">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
               
                    <div>
                        <h5>
                           流程名称：<%=FlowName%></h5>
                            <a style="float:right" href="/Weixin/WeiXinFlowInfoManage.aspx" class="easyui-linkbutton"
                                 iconcls="icon-back" plain="true">返回流程管理</a>
                    </div>              
                       
                      <div>
                        <span style="font-size: 12px; font-weight: normal">字段名称:</span>
                        <input type="text" style="width: 200px" id="txtName" />
                        <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                    </div>
                 
                      <div>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAddOrEdit('add')">
                            添加步骤</a>
                             <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">
                            编辑步骤</a> 
                             <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                                onclick="Delete()">删除步骤</a>
                            <a href="#" title="上移" class="easyui-linkbutton"  plain="true" onclick="MoveStep(0)">
                             <img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a> 
                            <a href="#" title="下移" class="easyui-linkbutton"  plain="true" onclick="MoveStep(1)">
                             <img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a>
                             
                              
                             
                              </div>
                                   
                    

              
            </div>
            <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="5" checkbox="true">
                        </th>
                            <th field="StepID" width="10">
                            步骤ID
                        </th>

                        <th field="FlowField" width="20">
                            流程字段
                        </th>
                        <th field="SendMsg" width="30">
                            下发信息
                        </th>
                        <th field="ErrorMsg" width="30">
                            验证失败信息
                        </th>
                        <th field="AuthFunc" width="10">
                            验证方法
                        </th>
                             <th field="IsVerifyCode" formatter="changestate" width="10">
                            验证码
                        </th>

                    </tr>
                </thead>
            </table>
          
           <div id="win" class="easyui-window" modal="true" closed="true" style="padding: 10px;
                text-align: center;">
                <table style="margin: auto;">
           
                    <tr>
                        <td style="width: 20%;" align="right">
                            字段名称:
                        </td>
                        <td style="text-align:left">
                          
                            <input type="text" id="txtFlowField" style="width: 150px;" class="easyui-validatebox" required="true" missingmessage="请输入字段名称" />
                        </td>
                    </tr>
              
                                        <tr>
                        <td style="width: 20%;" align="right">
                            字段说明:
                        </td>
                        <td style="text-align:left">
                          
                            <input type="text" id="txtFieldDescription" style="width: 150px;"  />
                        </td>
                    </tr>
                                                            <tr>
                        <td style="width: 20%;" align="right">
                            下发信息:
                        </td>
                        <td style="text-align:left">
                          
                            <input type="text" id="txtSendMsg" style="width: 150px;" class="easyui-validatebox" required="true" missingmessage="请输入下发信息"   />

                        </td>
                    </tr>
                                                                                <tr>
                        <td style="width: 20%;" align="right">
                            错误信息:
                        </td>
                        <td style="text-align:left">
                          
                            <input type="text" id="txtErrorMsg" style="width: 150px;" />

                        </td>
                    </tr>
                     <tr>
                        <td style="width: 20%;" align="right">
                            验证类型:
                        </td>
                        <td style="text-align:left">
                          <select id="ddlAuthFunc">
                          <option value="">无</option>
                          <option value="phone">手机</option>
                          <option value="email">电子邮箱</option>
                          </select>
                           
                           <span id="divIsVerifyCode" style="display:none">
                           验证码：
                            <input type="radio" id="rd1" name="rdo" checked="checked" /> <label for='rd1'>是</label>
	                        <input type="radio" id="rd0" name="rdo"  /> <label for='rd0'>否</label>

                           </span>
                        </td>
                    </tr>


                    <tr>
                        <td>
                        </td>
                        <td align="left">
                           
                            <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>
                        </td>
                    </tr>
                </table>

            </div>
          
        </div>
    </div>
</asp:Content>