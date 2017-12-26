<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeixinSpread.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinSpread" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var handlerUrl = '/Handler/Monitor/MonitorHandler.ashx';
        var grid;
        var currSelectID = 0;
        var planid = 0;
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
                fitCloumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "QueryWeixinSpread" }
            });
            //------------加载gridview

            //窗体关闭按钮---------------------
            $("#dlgWeixinSpreadInfo").find("#btnExit").bind("click", function () {
                $("#dlgWeixinSpreadInfo").window("close");
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
                var spreadname = $.trim($("#txtWeixinSpreadNames").val());
                grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryWeixinSpread", SpreadName: spreadname} });

            });




        });



        function ShowAdd() {
            ClearWinDataByTag('input', dlgWeixinSpreadInfo);
            $("#btnSave").attr('tag', 'add');
            $('#dlgWeixinSpreadInfo').window(
            {
                title: '新建微信推广'
            }
            );

            $('#dlgWeixinSpreadInfo').dialog('open');

        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            //            ClearWinDataByTag('input', dlgWeixinSpreadInfo);

            //            $('#dlgWeixinSpreadInfo').window(
            //            {
            //                title: '编辑'
            //            }
            //            );

            $('#dlgWeixinSpreadInfo').dialog('open');
            $("#btnSave").attr('tag', 'edit');
            try {


                //加载编辑数据
                currSelectID = rows[0].WeixinSpreadID;
                $('#txtSpreadName').val(rows[0].SpreadName);
                $("#txtSpreadUrl").val(rows[0].SpreadUrl);
                $("#txtActivityID").val(rows[0].ActivityID);
                planid = rows[0].PlanID;
               
                var status = $.trim(rows[0].Status);
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
                    data: { Action: "AddWeixinSpread", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '添加成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgWeixinSpreadInfo").window("close");
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
                    data: { Action: "EditWeixinSpread", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '编辑成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgWeixinSpreadInfo").window("close");
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
                        ids.push(rows[i].WeixinSpreadID);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWeixinSpread", ids: ids.join(',') },
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
                "WeixinSpreadID": currSelectID,
                "SpreadName": $.trim($("#txtSpreadName").val()),
                "SpreadUrl": $.trim($("#txtSpreadUrl").val()),
                "ActivityID": $.trim($("#txtActivityID").val()),
                "SpreadUrl": $.trim($("#txtSpreadUrl").val()),
                "Status": rd0.checked ? "0" : "1",
                "PlanID":planid
               
            }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['SpreadName'] == '') {
                $("#txtSpreadName").val("");
                $("#txtSpreadName").focus();
                return false;
            }
            if (model['SpreadUrl'] == '') {
                $("#txtSpreadUrl").val("");
                $("#txtSpreadUrl").focus();
                return false;
            }

            return true;
        }


        //格式化状态
        function FormatStatus(value) {

            if ($.trim(value) == "0") {
                return "<font color='red'>已停止</font>";
            }
            if ($.trim(value) == "1") {
                return "<font color='green'>正在运行</font>";
            }

        }


        //管理
        function Opreate(value, row) {
            var result = new StringBuilder();
            result.AppendFormat('<a title="查看任务" href="javascript:;" onclick="GotoPlanPage({0})">查看任务</a>', row.PlanID);
            return result.ToString();
        }

        //跳转到任务管理
        function GotoPlanPage(id) {
            var url = "/Monitor/MonitorPlanManage.aspx?id=" + id;
            parent.addTab('任务管理-' + id, url, 'tu0818', true);

        }
        //跳转到任务管理




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

            var msg = v == 1 ? "确定开始选中推广?" : "确定停止选中推广?";

            $.messager.confirm("系统提示", msg, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "BatChangWeixinSpreadState", ids: ids, status: v },
                        success: function (result) {
                            if (result == "true") {
                                messager('系统提示', "操作成功！");
                                grid.datagrid('reload');
                                return;
                            }
                            $.messager.alert("更新失败", result);
                        }

                    });
                }
            });


        }



        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].WeixinSpreadID);
            }
            return ids;
        }

        //跳转链接
        function FormatSpreadUrl(value) {


            return "<a target='_blank' title='点击查看' href='"+value+"'>"+value+"</a>";
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


                <table id="list_data" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel().UserType.Equals(1))
                          { %>
                            <th field="UserID" width="10">
                            用户名
                            </th>
                              
                          <%} %>
                       
                        <th field="WeixinSpreadidHex" width="10">
                            推广码
                        </th>
                        <th field="SpreadName" width="15">
                            推广名称
                        </th>
                        <th field="SpreadUrl" formatter="FormatSpreadUrl" width="30">
                            原链接
                        </th>
                        <th field="PlanID" width="10">
                            任务ID
                        </th>
                        <th field="ActivityID" width="10">
                            活动ID
                        </th>
                        

                        <th field="Status" formatter="FormatStatus" width="10">
                            状态
                        </th>
                 
                        <th field="InsertDate" formatter="FormatDate" width="25">
                            建立日期
                        </th>
                      
                        
                        <th field="action" formatter="Opreate" width="10">
                           操作
                        </th>
                       
                    </tr>
                </thead>
            </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
      
        
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                title="新建推广" onclick="ShowAdd()" id="btnAdd" runat="server">新建推广</a> 

           

                <a href="javascript:;"
                    class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑" onclick="ShowEdit()"
                    id="btnEdit" runat="server">编辑 </a>
                    
                    <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-delete" plain="true" title="删除" onclick="Delete()" id="btnDelete"
                        runat="server">删除 </a>

                         <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-start" plain="true" title="开始" onclick="BatChangState(1)" id="A1"
                        runat="server">开始 </a>

                         <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-stop" plain="true" title="停止" onclick="BatChangState(0)" id="A2"
                        runat="server">停止 </a>
                      
                <br />
                <div>
                    <span style="font-size: 12px; font-weight: normal">推广名称:</span>
                    <input id="txtWeixinSpreadNames" style="width: 200px" />
                    <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                </div>

        </div>
    </div>
    <div id="dlgWeixinSpreadInfo" class="easyui-dialog" closed="true" modal="true" title="任务" style="width: 360px;
        height: 250px; padding: 10px">
        <div style="margin-left: 20px">
            <table>
   
                <tr>
                    <td height="25" width="30%" align="left">
                        <span style="color:Red">*</span>
                        推广名称：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtSpreadName" style="width: 200px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入推广名称"  />
                    </td>
                </tr>
               <tr>
                    <td height="25" width="30%" align="left">
                     <span style="color:Red">*</span>
                        原链接：
                    </td>
                    <td height="25" width="*" align="left">
                    <textarea id="txtSpreadUrl" style="width:200px;height:50px" class="easyui-validatebox"
                            required="true" missingmessage="请输入链接"></textarea>
                      
                    </td>
                </tr>


                <tr>
                    <td height="25" width="30%" align="left">
                        活动ID：
                    </td>
                    <td height="25" width="*" align="left">
                
                       <input type="text" id="txtActivityID" style="width:100px;" />
                    </td>
                </tr>

                <tr>
                    <td height="25" width="30%" align="left">
                        状态：
                    </td>
                    <td height="25" width="*" align="left">
                       <input type="radio" id="rd1" checked="checked" name="rdo"/>
                                <label for='rd1'>
                                    启动</label>
                                <input type="radio" id="rd0" name="rdo" />
                                <label for='rd0'>
                                    停止</label>
                    </td>
                </tr>
              
            </table>

            <div style="float:right;margin-top:5px;">
           <a href="#" class="easyui-linkbutton" id="btnSave">确定</a> 
           <a href="#" class="easyui-linkbutton" id="btnExit">取消</a>
           </div>
            
        </div>
    </div>

    
   
</asp:Content>