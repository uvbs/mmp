<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MonitorPlanManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Monitor.MonitorPlanManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">

     var handlerUrl = '/Handler/Monitor/MonitorHandler.ashx';
     var grid;
     var currSelectID = 0;
     var planId = "<%=planId %>"
     $(function () {



         //-----------------加载gridview
         grid = jQuery("#list_data").datagrid({
             method: "Post",
             url: handlerUrl,
             height: 570,
             toolbar: '#divToolbar',
             fitCloumns: true,
             pagination: true,
             rownumbers: true,
             singleSelect: false,
             queryParams: { Action: "QueryPlan", PlanId: planId }
         });
         //------------加载gridview

         //窗体关闭按钮---------------------
         $("#dlgPlanInfo").find("#btnExit").bind("click", function () {
             $("#dlgPlanInfo").window("close");
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

         //查询按钮点击绑定
         $("#btnSearch").click(function () {
             var planname = $.trim($("#txtPlanNames").val());
             grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryPlan", PlanName: planname, PlanId: planId} });

         });




     });



     function ShowAdd() {
         ClearWinDataByTag('input', dlgPlanInfo);
         $("#btnSave").attr('tag', 'add');
         $('#dlgPlanInfo').window(
            {
                title: '新建任务'
            }
            );

         $('#dlgPlanInfo').dialog('open');

     }

     function ShowEdit() {
         var rows = grid.datagrid('getSelections');
         if (!EGCheckNoSelectMultiRow(rows)) {
             return;
         }
         //            ClearWinDataByTag('input', dlgPlanInfo);

         //            $('#dlgPlanInfo').window(
         //            {
         //                title: '编辑'
         //            }
         //            );

         $('#dlgPlanInfo').dialog('open');
         $("#btnSave").attr('tag', 'edit');
         try {


             //加载编辑数据
             currSelectID = rows[0].MonitorPlanID;
             $('#txtPlanName').val(rows[0].PlanName);
             $("#txtRemark").val(rows[0].Remark);
             var status = rows[0].PlanStatus;
             if (status == "1") {
                 $("#rd1").attr("checked", true);
             } else {
                 $("#rd0").attr("checked", true);
             }
             //----
         } catch (e) {
             alert(e);
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
                 data: { Action: "AddPlan", JsonData: JSON.stringify(model).toString() },
                 success: function (result) {
                     if (result == "true") {
                         $.messager.show({
                             title: '系统提示',
                             msg: '添加成功.'
                         });
                         grid.datagrid('reload');
                         $("#dlgPlanInfo").window("close");
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
                 data: { Action: "EditPlan", JsonData: JSON.stringify(model).toString() },
                 success: function (result) {
                     if (result == "true") {
                         $.messager.show({
                             title: '系统提示',
                             msg: '编辑成功.'
                         });
                         grid.datagrid('reload');
                         $("#dlgPlanInfo").window("close");
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
         $.messager.confirm('系统提示', '确定删除选中数据？', function (o) {
             if (o) {
                 var ids = new Array();
                 for (var i = 0; i < rows.length; i++) {
                     ids.push(rows[i].MonitorPlanID);
                 }
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeletePlan", ids: ids.join(',') },
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
                "MonitorPlanID": currSelectID,
                "PlanName": $.trim($("#txtPlanName").val()),
                "PlanStatus": rd0.checked ? "0" : "1",
                "Remark": $("#txtRemark").val()
            }
         return model;
     }

     //检查输入框输入
     function CheckDlgInput(model) {
         if (model['PlanName'] == '') {
             $("#txtPlanName").val("");
             $("#txtPlanName").focus();
             return false;
         }
         return true;
     }


     //格式化状态
     function FormatStatus(value) {

         if (value == "0") {
             return "<font color='red'>已停止</font>";
         }
         if (value == "1") {
             return "<font color='green'>正在运行</font>";
         }

     }


     //管理
     function Opreate(value, row) {
         var result = new StringBuilder();
         result.AppendFormat('<a title="链接管理" href="javascript:;" onclick="GotoLinkPage({0})">链接管理</a>', row.MonitorPlanID);
         return result.ToString();
     }

     //管理跳转
     function GotoLinkPage(id) {
         var url = "/Monitor/MonitorLink.aspx?id=" + id;
         window.location = url;

     }
     //管理跳转

     //点击人次
     function FormatClickCount(value, row) {

         return "<a title=\"点击查看详细列表\" href=\"/Monitor/MonitorEventDetails.aspx?planId=" + row.MonitorPlanID + " &eventType=1\">" + value + "</a>";


     }
     //打开人次
     function FormatOpenCount(value, row) {

         return "<a title=\"点击查看详细列表\" href=\"/Monitor/MonitorEventDetails.aspx?planId=" + row.MonitorPlanID + " &eventType=0\">" + value + "</a>";


     }




     //格式化链接数
     function FormatLinkCount(value, row) {

         return "<a title=\"点击管理链接\" href=\"/Monitor/MonitorLink.aspx?id=" + row.MonitorPlanID + "\">" + value + "</a>";


     }

     //管理链接
     function LinkManage() {
         var rows = grid.datagrid('getSelections');
         if (!EGCheckNoSelectMultiRow(rows)) {
             return;
         }

         GotoLinkPage(rows[0].MonitorPlanID);


     }


     //批量更改启动停止状态
     function BatChangState(v) {

         var rows = grid.datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         var ids = GetRowsIds(rows).join(','); //id集合字符串

         var msg = v == 1 ? "确定开始选中任务?" : "确定停止选中任务?";

         $.messager.confirm("系统提示", msg, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "BatChangPlanStatus", ids: ids, status: v },
                     success: function (result) {
                         if (result == "true") {
                             messager('系统提示', "操作成功！");
                             grid.datagrid('reload');
                             return;
                         }
                         $.messager.alert("操作失败", result);
                     }

                 });
             }
         });


     }



     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].MonitorPlanID);
         }
         return ids;
     }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：监测平台&nbsp;<span>任务管理</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
                <table id="list_data" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        <%if (ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel().UserType.Equals(1))
                          { %>
                            <th field="UserID" width="10">
                            用户名
                            </th>
                              
                          <%} %>
                        <th field="MonitorPlanID" width="10">
                            任务ID
                        </th>
                        <th field="PlanName" width="20">
                            任务名称
                        </th>
                        <th field="PlanStatus" formatter="FormatStatus" width="10">
                            状态
                        </th>
                         <th field="LinkCount" formatter="FormatLinkCount" width="10" align="center">
                            链接数
                        </th>
                        

                        <th field="OpenCount" align="center"  width="10">
                            浏览量（PV）
                        </th> 

                         <th field="DistinctOpenCount" formatter="FormatLinkCount" width="10" align="center">
                            IP数
                        </th>
                        <th field="InsertDate" formatter="FormatDate" width="20">
                            建立日期
                        </th>
                       <%-- <th field="ClickCount"  formatter="FormatClickCount"  width="10">
                            点击人次
                        </th>--%>
                       <%-- <th field="DistinctClickCount"  width="10">
                            点击人数
                        </th>--%>
                       <%-- <th field="OpenCode"   width="10">
                            打开代码
                        </th>--%>
                        
                        <th field="action" formatter="Opreate" width="10">
                           操作
                        </th>
                       
                    </tr>
                </thead>
            </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
      
        
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="新建任务" onclick="ShowAdd()" id="btnAdd" runat="server">新建任务</a> 

           

                <a href="javascript:;"
                    class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑任务" onclick="ShowEdit()"
                    id="btnEdit" runat="server">编辑任务 </a>
                    
                    <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-delete" plain="true" title="删除任务" onclick="Delete()" id="btnDelete"
                        runat="server">删除任务 </a>
                                               <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-start" plain="true" title="开始" onclick="BatChangState(1)" id="A1"
                        runat="server">开始 </a>

                         <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-stop" plain="true" title="停止" onclick="BatChangState(0)" id="A2"
                        runat="server">停止 </a>
                <br />
                <div>
                    <span style="font-size: 12px; font-weight: normal">任务名称:</span>
                    <input id="txtPlanNames" style="width: 200px" />
                    <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                </div>

        </div>
    </div>
    <div id="dlgPlanInfo" class="easyui-dialog" closed="true" modal="true" title="任务" style="width: 360px;
        height: 210px; padding: 10px">
        <div style="margin-left: 20px">
            <table>
   
                <tr>
                    <td height="25" width="30%" align="left">
                        任务名称：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtPlanName" style="width: 200px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入任务名称"  />
                    </td>
                </tr>
               <tr>
                    <td height="25" width="30%" align="left">
                        备注：
                    </td>
                    <td height="25" width="*" align="left">
                    <textarea id="txtRemark" style="width:200px;height:50px"></textarea>
                      
                    </td>
                </tr>

                <tr>
                    <td height="25" width="30%" align="left">
                        是否立刻开始：
                    </td>
                    <td height="25" width="*" align="left">
                       <input type="radio" id="rd1" checked="checked" name="rdo"/>
                                <label for='rd1'>
                                    是</label>
                                <input type="radio" id="rd0" name="rdo" />
                                <label for='rd0'>
                                    否</label>
                    </td>
                </tr>
              
            </table>

            <div style="float:right;margin-top:5px;">
           <a href="#" class="easyui-linkbutton" id="btnSave">确定</a> 
           <a href="#" class="easyui-linkbutton" id="btnExit">取消</a>
           </div>
            
        </div>
    </div>

    
     <div id="dlgReadCode" class="easyui-dialog" closed="true" modal="true" title="打开标签" style="width: 400px;
        height: 120px; padding: 0px">
       
          
           <textarea id="txtCode"  style="width:366px;height:70px;"></textarea>
            
       
    </div>
</asp:Content>


