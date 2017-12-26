<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="LoginLogsManager.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.LoginLogsManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var handlerUrl = '/Handler/Permission/LoginLogsManager.ashx';
        var grid;
        $(function () {

            $(window).resize(function () {
                $('#grvData').datagrid('resize', {
                    width: function () { return document.body.clientWidth; },
                    height: function () { return document.documentElement.clientHeight; }
                });
            });

    
            //加载datagrid
            grid = $("#grvData").datagrid(
                {
                    method: "Post",
                    url: handlerUrl,
                    queryParams: { Action: "Query" },
                    height: document.documentElement.clientHeight - 45,
                    pagination: true,
                    pageSize: 20,
                    rownumbers: true,
                    singleSelect: true
                }
            );
            //加载datagrid

            //搜索开始------------------------
                $("#btnSearch").click(function () {
                var UserID = $("#txtUserID").val();

                var From = $("#txtFrom").datetimebox("getValue"); //地点
                var To = $("#txtTo").datetimebox("getValue"); //起始时间
                if (From != "") {
                    if (!CheckDateTime(From)) {
                        $.messager.alert("系统提示", "起始时间不正确", "warning");
                        return;
                    }
                }
                if (To != "") {
                    if (!CheckDateTime(To)) {
                        $.messager.alert("系统提示", "结束时间不正确", "warning");
                        return;
                    }

                }
                if (From != "" && To != "") {

                    if (From > To) {
                        $.messager.alert("系统提示", "开始时间不能大于结束时间", "warning");
                        return;
                    }
                }

                grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", UserID: UserID, From: From, To: To} });
            });
            //搜索结束---------------------




            //            //窗体关闭按钮---------------------
            //            $("#dlgWinInfo").find("#btnExit").bind("click", function () {
            //                $("#dlgWinInfo").window("close");
            //            });
            //            //窗体关闭按钮---------------------

            //            //窗体保存按钮---------------------
            //            $("#btnSave").bind("click", function () {

            //                var tag = jQuery.trim(jQuery(this).attr("tag"));

            //                if (tag == "add") {
            //                    //添加
            //                    Add();
            //                    return;
            //                }
            //                else {
            //                    //修改
            //                    Edit();
            //                    return;
            //                }
            //            });
            //            //窗体保存按钮---------------------


        });


        //验证长时间
        function CheckDateTime(str) {
            var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
            var r = str.match(reg);
            if (r == null) return false;
            var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
            return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 35%;
            text-align:right;
        }
    
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="divToolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
         
       <span style="font-size:12px;font-weight:normal">用户名:<input id="txtUserID" type="text" style="width:200px;"  />
        
    
                       
          &nbsp;&nbsp; 登录时间:
                        <input type="text" style="width: 150px" id="txtFrom" class="easyui-datetimebox" />-
                        <input type="text" style="width: 150px" id="txtTo" class="easyui-datetimebox" />
                        </span>
          <%--<span style="font-size:12px;font-weight:normal">昵称:</span>
    
          <input  id="txtScreenName" style="width: 200px" />--%>
             <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
           <%--  <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                title="添加账号" onclick="ShowAdd()">添加账号</a>
                
        
                <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" title="编辑账号" onclick="ShowEdit()">编辑账号</a>
            
            
         
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                title="删除账号" onclick="Delete()">删除账号 </a>--%>
           

        </div>
      
    </div>
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
               <%-- <th field="ck" width="5" checkbox="true">--%>
                </th>
                <th field="InsertDate" width="25">
                   登录时间
                </th>
                <th field="UserID"   width="15">
                   用户名
                </th>
                 <th field="IP"   width="20">
                  IP
                </th>
                   <th field="IPLocation" width="20">
                   IP所在地
                </th>
      
                 <th field="BrowserID"   width="20">
                    浏览器
                </th>

                 <th field="BrowserVersion"   width="20">
                   浏览器版本
                </th>

                <th field="BrowserIsBata"   width="20">
                   正式/测试版
                </th>  
                <th field="SystemByte" width="20">
                  平台
                </th>

                 <th field="SystemPlatform" width="20">
                  系统位数
                </th>

                               

            </tr>
        </thead>
    </table>
   
   <%-- <div id="dlgWinInfo" class="easyui-dialog" title="" modal="true" closed="true" style="width: 320px;
        height: 250px; padding: 10px">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td height="25" align="left" class="style1">
                        账号：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="text" id="txtAccName" style="width: 150px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入账号"  />
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        密码：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="password" id="txtAccPassword" style="width: 150px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入密码" />
                    </td>
                </tr>
                <tr>
                    <td height="25" align="left" class="style1">
                        密码确认：
                    </td>
                    <td height="25" width="*" align="left">
                        <input type="password" id="txtAccPasswordConfirm" style="width: 150px;" class="easyui-validatebox"
                            required="true" missingmessage="确认密码不一致" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        账号状态:
                    </td>
                    <td>
                        <input type="radio" id="rdoAccountStatus" name="rdo" checked="checked" />
                        <label for='rdoAccountStatus'>
                            正常</label>
                        <input type="radio" id="rdoAccountStatusFalse" name="rdo" />
                        <label for='rdoAccountStatusFalse'>
                            异常</label>
                    </td>
                </tr>

                    <tr>
                    <td class="style1">
                        最后登录时间:
                    </td>
                    <td>
                       <label id="lblAccLastLogin"></label>
                    </td>
                </tr>


            </table>
            <div style="margin-top: 10px; margin-left: 30px">
                <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                    保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                        关 闭</a>
            </div>
        </div>
    </div>--%>
  
</asp:Content>
