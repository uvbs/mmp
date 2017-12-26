<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="UserManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.UserManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>站点初始账户管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">新建账户</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑账户</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="btnEditPwd();" id="btnEditPwd">修改密码</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowSetPms()">权限组分配</a>
            <br />

            <div style="display:none;">
                <input class="rdoShow" name="rdoShow" type="radio" id="rdoShowAdmin" checked /><label for="rdoShowAdmin">只显示管理员</label>
                <input class="rdoShow" name="rdoShow" type="radio" id="rdoShowAll" /><label for="rdoShowAll">显示所有账户</label>
            </div>
            账户名:<input type="text" id="txtUserIds" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgAdd" class="easyui-dialog" closed="true" title="发布新站点" style="width: 420px;
         padding: 15px;">
        <table width="100%">
            <tr>
                <td style="width:100px;">
                    账户名:
                </td>
                <td>
                    <input id="txtUserID" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr id="pwd1">
                <td>
                    登录密码:
                </td>
                <td>
                    <input id="txtPwd" type="password" style="width: 250px;" />
                </td>
            </tr>
            <tr id="pwd2">
                <td>
                    登录密码确认:
                </td>
                <td>
                    <input id="txtPwd2" type="password" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    公司名称:
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    真实姓名:
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    登录手机:
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    联系手机:
                </td>
                <td>
                    <input id="txtPhone3" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    职位:
                </td>
                <td>
                    <input id="txtPostion" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr style="display:none;">
                <td>
                    所属账户:
                </td>
                <td>
                    <input id="txtWebsiteOwner" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    微信高级认证:
                </td>
                <td>
                    <input id="rdoweixinauth" type="radio" class="positionTop2" name="rdweixinauth" value="1" /><label for="rdoweixinauth">已开通</label>
                    <input id="rdoweixinnotauth" type="radio" class="positionTop2" name="rdweixinauth" checked="checked" value="0"/><label for="rdoweixinnotauth">未开通</label>
                </td>
            </tr>
            <tr id="trAutoRegNewWxUser" style="display:none;">
                <td>
                    自动注册新账户:
                </td>
                <td>
                    <input id="rdoAutoRegNewWxUser1" type="radio" class="positionTop2" name="rdoAutoRegNewWxUser" checked="checked" value="0" /><label for="rdoAutoRegNewWxUser1">自动注册</label>
                    <input id="rdoAutoRegNewWxUser2" type="radio" class="positionTop2" name="rdoAutoRegNewWxUser" value="1"/><label for="rdoAutoRegNewWxUser2">手动注册(跳转注册页)</label>
                    <input id="rdoAutoRegNewWxUser3" type="radio" class="positionTop2" name="rdoAutoRegNewWxUser" value="2"/><label for="rdoAutoRegNewWxUser3">手动注册(不跳转注册页)</label>
                </td>
            </tr>
            <tr id="trWXAuthPageMustLogin">
                <td style="height:24px;">
                    跳转至登录页:
                    &nbsp;
                </td>
                <td>
                    <input id="chkWXAuthPageMustLogin" type="checkbox" class="positionTop2" name="chkWXAuthPageMustLogin" value="0" /><label for="chkWXAuthPageMustLogin">需微信授权登录时，跳转至登录页</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgPwd" class="easyui-dialog" closed="true" title="修改密码" style="width: 380px;
         padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    登录密码:
                </td>
                <td>
                    <input id="txtPasswrod" type="password" style="width: 250px;" />
                </td>
            </tr>
             <tr style="margin-top:20px;">
                <td>
                    确认密码:
                </td>
                <td>
                    <input id="txtPasswrod1" type="password" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSetPms" class="easyui-dialog" closed="true" title="设置权限" style="width: 670px;
       padding: 15px; top:50px;">
        <fieldset>
            <legend>请勾选要设定的权限组</legend>
            <div style="max-height: 160px;  overflow: auto;">
                <ul>
            <%
                StringBuilder strHtml = new StringBuilder();
                List<ZentCloud.BLLPermission.Model.PermissionGroupInfo> pmsGroup = new ZentCloud.BLLPermission.BLLMenuPermission("").GetList<ZentCloud.BLLPermission.Model.PermissionGroupInfo>("");
                foreach (var item in pmsGroup.Where(p=>p.GroupType==0))
                {
                    strHtml.AppendFormat(@"<li style=""float:left; min-width:200px;"">");
                    strHtml.AppendFormat(@"<input type=""checkbox"" name=""chkPms"" v=""{0}"" id=""chkPms{0}"" /><label for=""chkPms{0}"">{1}</label><br />",
                            item.GroupID,
                            item.GroupName
                        );
                    strHtml.AppendFormat(@"</li>");
                }
                Response.Write(strHtml.ToString());
            %>
                    </ul>
            </div>
        </fieldset>
        <fieldset style="margin-top:5px;">
            <legend>请选择要设定的版本</legend>
            <div style="max-height: 120px;  overflow: auto;">
                <ul>
            <%
                strHtml = new StringBuilder();
                foreach (var item in pmsGroup.Where(p => p.GroupType == 1))
                {
                    strHtml.AppendFormat(@"<li style=""float:left; min-width:200px;"">");
                    strHtml.AppendFormat(@"<input type=""radio"" name=""rdoPms"" v=""{0}"" id=""rdoPms{0}"" /><label for=""rdoPms{0}"">{1}</label><br />",
                            item.GroupID,
                            item.GroupName
                        );
                    strHtml.AppendFormat(@"</li>");
                }
                Response.Write(strHtml.ToString());
            %>
                    </ul>
            </div>
        </fieldset>
    </div>
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currUserAction = '';

     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QuerySysUserInfo", ShowAll: rdoShowAll.checked ? 1 : 0, UserId: $("#txtUserIds").val() },
	                height: document.documentElement.clientHeight - 165,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserID', title: '账户名', width: 40, align: 'left' },
                                { field: 'TrueName', title: '真实姓名', width: 40, align: 'left' },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Company', title: '公司名', width: 40, align: 'left' },
                                { field: 'Phone', title: '登陆手机', width: 30, align: 'left' },
                                { field: 'Phone3', title: '联系手机', width: 30, align: 'left' },
                                { field: 'Postion', title: '职位', width: 40, align: 'left' },
                                { field: 'WebsiteOwner', title: '所属账户', width: 40, align: 'left' }
                             ]]
	            }
            );

         $('#dlgAdd').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     var dataModel = {
                         Action: currUserAction,
                         UserID: $.trim($('#txtUserID').val()),
                         Pwd: $.trim($('#txtPwd').val()),
                         Pwd2: $.trim($('#txtPwd2').val()),
                         TrueName: $.trim($('#txtTrueName').val()),
                         WebsiteOwner: $.trim($('#txtWebsiteOwner').val()),
                         Company: $.trim($('#txtCompany').val()),
                         Phone: $.trim($('#txtPhone').val()),
                         Phone3:$.trim($('#txtPhone3').val()),
                         Postion: $.trim($('#txtPostion').val()),
                         WeixinIsAdvancedAuthenticate: $(":radio[name=rdweixinauth]:checked").val(),
                         AutoRegNewWxUser: $(":radio[name=rdoAutoRegNewWxUser]:checked").val(),
                         WXAuthPageMustLogin: $("#chkWXAuthPageMustLogin")[0].checked ? 1 : 0
                     }
                     if (dataModel.UserID == '') {
                         Alert("登录名不能为空");
                         return;
                     }
                     if (dataModel.Action == "AddSysUser"){
                         if (dataModel.Pwd == '') {
                             Alert("密码不能为空");
                             return;
                         }
                         if (dataModel.Pwd != dataModel.Pwd2) {
                             Alert("确认密码输入不一致");
                             return;
                         }
                     } else {
                         if (dataModel.WebsiteOwner != dataModel.UserID) {
                             Alert("仅支持编辑站点初始账户");
                             return;
                         }
                     }

                     dataModel.WebsiteOwner = dataModel.UserID;

                     //if (dataModel.WebsiteOwner == '') {
                     //    Alert("网站所有者账户名不能为空!");
                     //    return;
                     //}

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             if (resp.Status > 0) {
                                 $('#dlgAdd').dialog('close');
                                 $('#grvData').datagrid('reload');
                             }
                             else {

                             }
                             Alert(resp.Msg);

                         }
                     });


                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgAdd').dialog('close');
                 }
             }]
         });

         $('#dlgSetPms').dialog({
             buttons: [{
                 text: "确定",
                 handler: function () {
                     var pmsListStr = GetCheckGroupVal("chkPms", "v");
                     var pmsListStr1 = GetCheckGroupVal("rdoPms", "v");
                     if (pmsListStr != "" && pmsListStr1 != "") {
                         pmsListStr += "," + pmsListStr1;
                     }
                     else if (pmsListStr == "" && pmsListStr1 != "") {
                         pmsListStr += pmsListStr1;
                     }
                     var rows = $('#grvData').datagrid('getSelections');

                     if (!EGCheckIsSelect(rows))
                         return;

                     if (!EGCheckNoSelectMultiRow(rows))
                         return;

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: { Action: "SetSysUserPms", UserID: rows[0].UserID, Ids: pmsListStr },
                         dataType: "json",
                         success: function (resp) {
                             Alert(resp.Msg);
                         }
                     });

                 }
             }, {
                 text: "取消",
                 handler: function () {
                     $('#dlgSetPms').dialog('close');
                 }
             }]
         });

         $('.rdoShow').click(function () {
             $('#grvData').datagrid(
	            {
	                pageNumber:1,
	                queryParams: { Action: "QuerySysUserInfo", ShowAll: rdoShowAll.checked ? 1 : 0, UserId: $("#txtUserIds").val() }
	            });
         });
         $('#dlgPwd').dialog({
             buttons: [{
                 text: "确定",
                 handler: function () {
                     
                     var rows = $('#grvData').datagrid('getSelections');



                     var Pwd = $("#txtPasswrod").val();
                     var confirmPwd = $("#txtPasswrod1").val();
                     if (Pwd.trim() == '') {
                         Alert('请输入密码');
                         return;
                     }
                     if (confirmPwd.trim() == '') {
                         Alert('请输入确认密码');
                         return;
                     }
                     if (Pwd != confirmPwd) {
                         Alert('两次输入的密码不一致');
                         return;
                     }


                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: { Action: "UpdateUserPwd", id: rows[0].AutoID, Password: Pwd },
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status == 1) {
                                 Alert(resp.Msg);
                                 var Pwd = $("#txtPasswrod").val("");
                                 var confirmPwd = $("#txtPasswrod1").val("");
                                 $('#dlgPwd').dialog('close');
                             } else {
                                 Alert(resp.Msg);
                             }
                         }
                     });

                 }
             }, {
                 text: "取消",
                 handler: function () {
                     $('#dlgPwd').dialog('close');
                 }
             }]
         });

         //是否开通高级验证
         $('input[type="radio"][name="rdweixinauth"]').live("click", function () {
             if (rdoweixinauth.checked) {
                 $('#trAutoRegNewWxUser').show();
                 $('#trWXAuthPageMustLogin').hide();
             }
             else {
                 $('#trWXAuthPageMustLogin').show();
                 $('#trAutoRegNewWxUser').hide();
             }
         });

     });

     function ShowAdd() {
         currUserAction = "AddSysUser";
         txtUserID.readOnly = false;
         $('#txtCompany').val("");
         $('#pwd1').show();
         $('#pwd2').show();
         $('#txtPhone').val("");
         $('#txtPhone3').val("");
         $('#txtPwd').val("");
         $('#txtPwd2').val("");
         $('#txtTrueName').val("");
         $('#txtWebsiteOwner').val("");
         $('#txtUserID').val("");
         $('#txtPostion').val("");
         $("#rdoweixinnotauth").attr("checked", "checked");

         $('#trPageToReg').show();
         $('#trAutoRegNewWxUser').hide();
         $("#rdoAutoRegNewWxUser1").attr("checked", "checked");
         $("#chkWXAuthPageMustLogin")[0].checked = false;
         
         $('#dlgAdd').dialog({ title: '添加账户!' });
         $('#dlgAdd').dialog('open');
     }

     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;

         $('#txtCompany').val(rows[0].Company);
         $('#txtPhone').val(rows[0].Phone);
         $('#txtPhone3').val(rows[0].Phone3);
         $('#pwd1').hide();
         $('#pwd2').hide();
         $('#txtTrueName').val(rows[0].TrueName);
         $('#txtWebsiteOwner').val(rows[0].WebsiteOwner);
         $('#txtUserID').val(rows[0].UserID);
         $('#txtPostion').val(rows[0].Postion);
         var WeixinIsAdvancedAuthenticate = rows[0].WeixinIsAdvancedAuthenticate;
         if (WeixinIsAdvancedAuthenticate == "1") {
             $("#rdoweixinauth").attr("checked", "checked");
             $('#trWXAuthPageMustLogin').hide();
             $('#trAutoRegNewWxUser').show();
         }
         else {
             $("#rdoweixinnotauth").attr("checked", "checked");
             $('#trWXAuthPageMustLogin').show();
             $('#trAutoRegNewWxUser').hide();
         }

         if (rows[0].AutoRegNewWxUser==0) {
             $("#rdoAutoRegNewWxUser1").attr("checked", "checked");
         }
         else if (rows[0].AutoRegNewWxUser == 2) {
             $("#rdoAutoRegNewWxUser3").attr("checked", "checked");
         } else {
             $("#rdoAutoRegNewWxUser2").attr("checked", "checked");
         }

         if (rows[0].WXAuthPageMustLogin) {
             $("#chkWXAuthPageMustLogin")[0].checked = true;
         }
         else {
             $("#chkWXAuthPageMustLogin")[0].checked = false;
         }

         currUserAction = "SuperEditSysUser";
         txtUserID.readOnly = true;
         $('#dlgAdd').dialog({ title: '编辑账户!' });
         $('#dlgAdd').dialog('open');

     }

     function btnEditPwd() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;
         $('#dlgPwd').dialog('open');

     }


     function ShowSetPms() {

         $("[name=chkPms]").each(function () {
             this.checked = false;
         });
         if ($("[name=rdoPms]").length > 0) {
             $("[name=rdoPms]")[0].checked = true;
         }

         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;

         $.ajax({
             type: 'post',
             url: handlerUrl,
             data: { Action: 'GetUserAllPmsGroup', inputUserID: rows[0].UserID },
             dataType: "json",
             success: function (resp) {
                 SetCheckGroupVal('chkPms', resp.Msg, 'v');
                 SetCheckGroupVal('rdoPms', resp.Msg, 'v');
                 $('#dlgSetPms').dialog('open');
             }
         });
     }


     function Search() {
         //pmsGroup
         $('#grvData').datagrid({
             queryParams: {
                 Action: "QuerySysUserInfo",
                 ShowAll: rdoShowAll.checked ? 1 : 0,
                 UserId: $("#txtUserIds").val()
             }
         });
     }

    </script>
</asp:Content>
