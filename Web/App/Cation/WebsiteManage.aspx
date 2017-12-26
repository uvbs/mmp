<%@ Page Title="站点管理" Language="C#" MasterPageFile="~/Master/WebMainContent.Master"
    AutoEventWireup="true" CodeBehind="WebsiteManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WebsiteManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>站点管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="WebsiteCompile.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true" id="btnAdd">发布新站点</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowWebsiteEdit();" id="btnEdit">编辑站点</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">批量删除站点</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="SetDomain()">站点域名配置</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="SetWebsiteMenus()">站点菜单配置</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="SetPermissionColumns()">站点栏目配置</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="SetPermissionDisable()">禁用权限配置</a>
            <br />
            <span style="font-size: 12px; font-weight: normal">网站名称：</span>
            <input type="text" style="width: 200px" id="txtName" />
            <%
                List<ZentCloud.BLLPermission.Model.PermissionGroupInfo> pmsGroup = new ZentCloud.BLLPermission.BLLMenuPermission("").GetList<ZentCloud.BLLPermission.Model.PermissionGroupInfo>("");     
            %>
            <select id="version">
                <option value="">请选择版本</option>
                <%
                    foreach (var item in pmsGroup.Where(p => p.GroupType == 1))
                    {
                %>
                <option value="<%=item.GroupID %>"><%=item.GroupName %></option>
                <%
                    }
                %>
            </select>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgAdd" class="easyui-dialog" closed="true" title="发布新站点" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>网站名称
                </td>
                <td>
                    <input id="txtWebsiteName" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td>站点所有者
                </td>
                <td>
                    <input id="txtWebsiteOwner" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td>站点有效期至
                </td>
                <td>
                    <input class="easyui-datebox" style="width: 200px;" showseconds="false" id="txtWebsiteExpirationDate" />
                </td>
            </tr>
            <tr>
                <td>可建立的子账号数量
                </td>
                <td>
                    <input id="txtMaxSubAccountCount" type="text" style="width: 50px;" onkeyup="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
            <tr>
                <td>日志保存天数
                </td>
                <td>
                    <input id="txtLogDay" type="text" style="width: 50px;" onkeyup="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
            <tr>
                <td>关闭商品购买日期设置
                </td>
                <td>
                    <input type="radio" name="time" id="rdotime0" value="1" /><label for="rdotime0">开启</label><input type="radio" id="rdotime1" value="0" name="time" checked="checked" /><label for="rdotime1">关闭</label>
                </td>
            </tr>
            <tr>
                <td>开启余额支付
                </td>
                <td>
                    <input type="radio" id="rdoenableamountpay" value="1" name="enableamountpay" />
                    <label for="rdoenableamountpay">开启</label>
                    <input type="radio" id="rdodisableamountpay" value="0" name="enableamountpay" checked="checked" />
                    <label for="rdodisableamountpay">关闭</label>
                </td>
            </tr>
            <tr>
                <td>余额支付前端显示名称
                </td>
                <td>
                    <input id="txtAccountAmountPayShowName" type="text" />
                </td>
            </tr>
            <tr>
                <td>分销员标准规则
                </td>
                <td>
                    <input id="cbDistributionMemberStandardsHaveParent" type="checkbox" value="1" />
                    <label for="cbDistributionMemberStandardsHaveParent">有上级</label>
                    <input id="cbDistributionMemberStandardsHavePay" type="checkbox" value="1" />
                    <label for="cbDistributionMemberStandardsHavePay">有付款的订单</label>
                    <input id="cbDistributionMemberStandardsHaveSuccessOrder" type="checkbox" value="1" />
                    <label for="cbDistributionMemberStandardsHaveSuccessOrder">有交易成功的订单</label>
                </td>
            </tr>
            <tr>
                <td>分销关系建立规则
                </td>
                <td>
                    <input id="cbDistributionRelationBuildQrCode" type="checkbox" value="1" />
                    <label for="cbDistributionRelationBuildQrCode">关注二维码</label>
                    <input id="cbDistributionRelationBuildSpreadActivity" type="checkbox" value="1" />
                    <label for="cbDistributionRelationBuildSpreadActivity">微转发报名</label>
                    <input id="cbDistributionRelationBuildMallOrder" type="checkbox" value="1" />
                    <label for="cbDistributionRelationBuildMallOrder">分享商城链接下单</label>
                </td>
            </tr>
            <tr>
                <td>网站说明
                </td>
                <td>
                    <textarea id="txtWebsiteDescription" style="width: 200px; height: 150px"></textarea>
                </td>
            </tr>
            <tr id="trtemplate" style="display: none;">
                <td>行业模板
                </td>
                <td>
                    <select id="ddlTemplate" style="width: 98%;">
                        <%=sbTemplateList.ToString()%>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSetDomain" class="easyui-dialog" closed="true" modal="true" title="站点域名配置"
        style="width: 450px;">
        <table id="grvDomainData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgAddWebSiteDomain" class="easyui-dialog" closed="true" modal="true" title="添加域名"
        style="width: 380px; height: 130px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>域名
                </td>
                <td>
                    <input id="txtWebsiteDomain" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSetWebsiteMenu" class="easyui-dialog" closed="true" modal="true"
        title="站点菜单配置" style="width: 800px;" toolbar="#dlgSetWebsiteMenuToolbar">
        <table id="grvWebsiteMenuData" fitcolumns="true" data-options="">
        </table>
    </div>
    <div id="dlgSetWebsiteMenuToolbar">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" title="添加菜单" onclick="AddWebsiteMenu()" id="btnAddWebsiteMenu">添加菜单</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑菜单" onclick="EditWebsiteMenu()" id="btnEditWebsiteMenu">编辑菜单</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" title="删除菜单" onclick="DelWebsiteMenu()" id="btnDelWebsiteMenu">删除菜单</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="隐藏菜单" onclick="HideWebsiteMenu()" id="btnHideWebsiteMenu">隐藏菜单</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="显示菜单" onclick="ShowWebsiteMenu()" id="btnShowWebsiteMenu">显示菜单</a>
        <select id="sltWebsiteMenuHide" class="easyui-combobox" data-options="editable:false,onSelect:SearchWebsiteMenuData">
            <option value="0" selected="selected">显示</option>
            <option value="1">全部</option>
        </select>
    </div>
    <div id="dlgAddWebsiteMenu" class="easyui-dialog" closed="true" modal="true" title="添加菜单"
        style="width: 500px; padding: 15px;">
        <table style="width: 100%;">
            <tr>
                <td height="25" width="100" align="left">所属菜单：
                </td>
                <td height="25" width="*" align="left">
                    <span id="sp_menu" style="width: 90%;"></span>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">节点名称<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtNodeName" style="width: 90%;" class="easyui-validatebox" required="true" missingmessage="请输入节点名称" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">链接：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtUrl" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">图标样式：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtICOCSS" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">菜单排序<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtMenuSort" style="width: 90%;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" class="easyui-validatebox"
                        required="true" missingmessage="请输入排序" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">是否新标签显示：
                </td>
                <td height="25" width="*" align="left">
                    <input type="radio" id="rdtb0" checked="checked" name="rdotb" class="positionTop2" /><label
                        for='rdtb0'>本页</label>
                    <input type="radio" id="rdtb1" name="rdotb" class="positionTop2" /><label for='rdtb1'>新页</label>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">显示级别：
                </td>
                <td height="25" width="*" align="left">
                    <% if (CurrentUserInfo != null && CurrentUserInfo.UserType == 1)
                       {%>
                    <input type="radio" id="rdLevel1" name="rdoShowLevel" class="positionTop2" /><label
                        for='rdLevel1'>超级管理员可见</label>
                    <%} %>
                    <input type="radio" id="rdLevel2" name="rdoShowLevel" class="positionTop2" /><label
                        for='rdLevel2'>站点管理员可见</label>
                    <input type="radio" id="rdLevel3" name="rdoShowLevel" class="positionTop2" checked="checked" /><label
                        for='rdLevel3'>站点子帐号可见</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgEditBaseWebsiteMenu" class="easyui-dialog" closed="true" modal="true"
        title="编辑基础菜单" style="width: 500px; padding: 15px;">
        <table style="width: 100%;">
            <tr>
                <td height="25" align="left">节点名称<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtbNodeName" style="width: 90%;" class="easyui-validatebox"
                        required="true" missingmessage="请输入节点名称" />
                    <span id="spanbTempName" style="display: none;"></span>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">链接(<span style="color: red;">只读</span>)：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtbUrl" readonly="readonly" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">菜单排序<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtbMenuSort" style="width: 90%;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" class="easyui-validatebox"
                        required="true" missingmessage="请输入排序" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">显示级别：
                </td>
                <td height="25" width="*" align="left">
                    <% if (CurrentUserInfo != null && CurrentUserInfo.UserType == 1)
                       {%>
                    <input type="radio" id="rdbLevel1" name="rdoShowLevel" class="positionTop2" /><label
                        for='rdbLevel1'>超级管理员可见</label>
                    <%} %>
                    <input type="radio" id="rdbLevel2" name="rdoShowLevel" class="positionTop2" /><label
                        for='rdbLevel2'>站点管理员可见</label>
                    <input type="radio" id="rdbLevel3" name="rdoShowLevel" class="positionTop2" checked="checked" /><label
                        for='rdbLevel3'>站点子帐号可见</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSetPermissionColumns" class="easyui-dialog" closed="true" modal="true"
        title="站点栏目配置" style="width: 800px;" toolbar="#dlgSetPermissionColumnToolbar">
        <table id="grvSetPermissionColumnData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgSetPermissionColumnToolbar">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" title="添加栏目" onclick="AddPermissionColumn()" id="btnAddPermissionColumn">添加栏目</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑栏目" onclick="EditPermissionColumn()" id="btnEditPermissionColumn">编辑栏目</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" title="删除栏目" onclick="DelPermissionColumn()" id="btnDelPermissionColumn">删除栏目</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="隐藏栏目" onclick="SetPermissionColumnHide(1)" id="btnHidePermissionColumn">隐藏栏目</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="显示栏目" onclick="SetPermissionColumnHide(0)" id="btnShowPermissionColumn">显示栏目</a>
        <select id="ddlSelectColumnHide" class="easyui-combobox" data-options="editable:false,onSelect:SearchPermissionColumnData">
            <option value="0" selected="selected">显示</option>
            <option value="1">全部</option>
        </select>
    </div>
    <div id="dlgAddPermissionColumn" class="easyui-dialog" title="编辑栏目" modal="true"
        closed="true" style="width: 500px; padding: 15px">
        <table style="width: 100%;">
            <tr>
                <td height="25" width="100" align="left">所属栏目：
                </td>
                <td height="25" width="*" align="left">
                    <span id="spPermissionColumn_menu" style="width: 90%;"></span>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">栏目名称<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtPermissionColumnName" style="width: 90%;" class="easyui-validatebox"
                        required="true" missingmessage="请输入栏目名称" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">同级排序<span style="color: red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtPermissionColumnSort" style="width: 90%;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" class="easyui-validatebox"
                        required="true" missingmessage="请输入排序" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgPermissionColumnPmsSet" class="easyui-dialog" title="栏目权限设置" modal="true"
        closed="true" style="width: 800px; height: 500px; top: 20px;">
        <div id="divPermissionColumnPmss" class="easyui-panel" style="height: 425px; width: 786px;">
        </div>
    </div>
    <div id="dlgPermissionColumnMenuSet" class="easyui-dialog" title="栏目菜单设置" modal="true"
        closed="true" style="width: 800px; height: 500px; top: 20px;">
        <div id="divPermissionColumnMenus" class="easyui-panel" style="height: 425px; width: 786px;">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host %>';
        var weisiteAction = '';
        var websiteDomainAction = '';
        var currGridSelectWebsiteOwner = '';
        var currGridSelectWebsiteDomain = '';
        var handlerMenuUrl = "/Handler/Permission/MenuManager.ashx";
        var websiteMenuAction = '';
        var currSelectMenuID = 0;
        var handlerPermissioncolumnUrl = '/serv/api/admin/permissioncolumn/';
        var handlerPermissionUrl = '/serv/api/admin/permission/';
        var websitePermissColumnAction = '';
        var currSelectPermissionColumnID = 0;
        var currSetPermissionColumnMenuID = 0;
        var currSetPermissionColumnPerID = 0;
        var columCount = 3;
        var _w = 740 / columCount;
        var mColumCount = 4;
        var _mw = 740 / mColumCount;
        var curSetPerType = "";
        $(function () {
            GetWebsitePermissionList();
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "QueryWebsiteInfo" },
                       height: document.documentElement.clientHeight - 150,
                       pagination: true,
                       striped: true,
                       pageSize: 20,
                       rownumbers: true,
                       singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'WebsiteOwner', title: '站点所有者', width: 40, align: 'left' },
                                   {
                                       field: 'WebsiteName', title: '网站名称', width: 60, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="javascript:;" title="{0}">{0}</a>', value);
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'Version', title: '版本', width: 35, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           var colorStr = value == "未购买" ? "red" : "green";
                                           str.AppendFormat('<span style="color:{1};" title="{0}">{0}</span>', value, colorStr);
                                           return str.ToString();
                                       }
                                   },
                                    { field: 'MaxSubAccountCount', title: '可建立的子账号数', width: 50, align: 'left' },
                                    { field: 'LogLimitDay', title: '日志保存天数', width: 45, align: 'left' },
                                   {
                                       field: 'WebsiteDescription', title: '网站说明', width: 70, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="javascript:;" title="{0}">{0}</a>', value);
                                           return str.ToString();
                                       }
                                   },

                                   { field: 'Creater', title: '发布人', width: 50, align: 'left' },
                                   {
                                       field: 'CreateDate', title: '发布时间', width: 60, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="javascript:;" title="{0}">{0}</a>', FormatDate(value));
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'WebsiteExpirationDate', title: '有效期至', width: 60, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="javascript:;" title="{0}">{0}</a>', FormatDate(value));
                                           return str.ToString();
                                       }
                                   }
                       ]]
                   }
               );

            $('#grvWebsiteMenuData').datagrid(
                   {
                       method: "Post",
                       height: 360,
                       striped: true,
                       pageSize: 10,
                       rownumbers: true,
                       columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'NodeName', title: '节点名称', width: 100, align: 'left', formatter: FormatterMenuTitle },
                            { field: 'MenuType', title: '菜单类型', width: 20, align: 'left', formatter: FormatMenuType },
                            { field: 'TargetBlank', title: '新页显示', width: 15, align: 'left', formatter: FormatTargetBlank },
                            { field: 'IsHide', title: '显示隐藏', width: 15, align: 'left', formatter: FormatHide },
                            { field: 'ShowLevel', title: '显示级别', width: 20, align: 'left', formatter: FormatShowLevel }
                       ]]
                   }
               );
            $("#grvSetPermissionColumnData").datagrid(
                   {
                       method: "Post",
                       height: 360,
                       pagination: true,
                       pageSize: 10,
                       rownumbers: true,
                       loadFilter: pagerFilter,
                       columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'id', title: '栏目编号', width: 10, align: 'left', formatter: FormatterTitle },
                            { field: 'name', title: '栏目名称', width: 50, align: 'left', formatter: FormatterTitle },
                            { field: 'is_hide', title: '是否隐藏', width: 10, align: 'left', formatter: FormatterIsHide },
                            { field: 'order_num', title: '同级排序', width: 10, align: 'left', formatter: FormatterTitle },
                            { field: 'type', title: '类型', width: 10, align: 'left', formatter: FormatterIsNewInfo },
                            { field: 'has_menu', title: '菜单', width: 7, align: 'left', formatter: FormatterMenu },
                            { field: 'has_permission', title: '权限', width: 7, align: 'left', formatter: FormatterPermission }
                       ]]
                   }
               );
            $('#grvDomainData').datagrid(
                   {
                       method: "Post",
                       height: 280,
                       pagination: true,
                       striped: true,
                       pageSize: 10,
                       rownumbers: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'WebsiteDomain', title: '绑定域名', width: 100, align: 'left', formatter: FormatterTitle }
                       ]]
                   }
               );

            $('#dlgAdd').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                                WebsiteOwner: $.trim($('#txtWebsiteOwner').val()),
                                WebsiteName: $.trim($('#txtWebsiteName').val()),
                                WebsiteDescription: $.trim($('#txtWebsiteDescription').val()),
                                WebsiteExpirationDate: $.trim($('#txtWebsiteExpirationDate').datebox("getValue")),
                                LogLimitDay: $.trim($("#txtLogDay").val()),
                                Action: weisiteAction,
                                TemplateId: $("#ddlTemplate").val(),
                                IsEnableLimitProductBuyTime: $("input[name=time]:checked").val(),
                                IsEnableAmountPay: $("input[name=enableamountpay]:checked").val(),
                                AccountAmountPayShowName: $("#txtAccountAmountPayShowName").val(),
                                MaxSubAccountCount: $(txtMaxSubAccountCount).val(),
                                DistributionMemberStandardsHaveParent: 0,//分销会员标准 有上级
                                DistributionMemberStandardsHavePay: 0,//分销会员标准 有付款的订单
                                DistributionMemberStandardsHaveSuccessOrder: 0,//分销会员标准 有交易完成的订单

                                DistributionRelationBuildQrCode: 0,//分销关系建立规则 关注二维码
                                DistributionRelationBuildSpreadActivity: 0,//分销关系建立规则 转发报名
                                DistributionRelationBuildMallOrder: 0//分销关系建立规则 商城下单



                            }
                            if (cbDistributionMemberStandardsHaveParent.checked) {
                                dataModel.DistributionMemberStandardsHaveParent = 1;
                            }
                            if (cbDistributionMemberStandardsHavePay.checked) {
                                dataModel.DistributionMemberStandardsHavePay = 1;
                            }
                            if (cbDistributionMemberStandardsHaveSuccessOrder.checked) {
                                dataModel.DistributionMemberStandardsHaveSuccessOrder = 1;
                            }


                            if (cbDistributionRelationBuildQrCode.checked) {
                                dataModel.DistributionRelationBuildQrCode = 1;
                            }
                            if (cbDistributionRelationBuildSpreadActivity.checked) {
                                dataModel.DistributionRelationBuildSpreadActivity = 1;
                            }
                            if (cbDistributionRelationBuildMallOrder.checked) {
                                dataModel.DistributionRelationBuildMallOrder = 1;
                            }
                            if (dataModel.WebsiteName == '') {
                                //GTip('#txtWebsiteName', '请输入站点名称');
                                Alert('请输入网站名称');
                                return;
                            }

                            if (dataModel.WebsiteOwner == '') {
                                //GTip('#txtWebsiteOwner', '请输入站点所有ID');
                                Alert('请输入站点所有者登录名');
                                return;
                            }
                            if (dataModel.WebsiteExpirationDate == '') {
                                //GTip('#txtWebsiteOwner', '请输入站点所有ID');
                                Alert('请输入站点有效期');
                                return;
                            }
                            if (dataModel.LogLimitDay == '') {
                                Alert('请输入日志保存天数');
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        //Show(resp.Msg);
                                        $('#dlgAdd').dialog('close');
                                        //$('#grvData').datagrid('reload');
                                    }
                                    else {

                                    }
                                    Alert(resp.Msg);
                                    $('#grvData').datagrid('reload');
                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgAdd').dialog('close');
                    }
                }]
            });

            $('#dlgSetDomain').dialog({
                toolbar: [{
                    iconCls: 'icon-add2',
                    text: '添加域名',
                    handler: function () {
                        websiteDomainAction = 'AddWebSiteDomain';
                        $('#dlgAddWebSiteDomain').dialog({ title: '添加域名' });
                        $('#dlgAddWebSiteDomain').dialog('open');

                    }
                }, {
                    iconCls: 'icon-edit',
                    text: '编辑域名',
                    handler: function () {

                        var rows = $('#grvDomainData').datagrid('getSelections');

                        if (!EGCheckIsSelect(rows))
                            return;

                        if (!EGCheckNoSelectMultiRow(rows))
                            return;

                        currGridSelectWebsiteDomain = rows[0].WebsiteDomain;
                        $('#txtWebsiteDomain').val(currGridSelectWebsiteDomain);

                        websiteDomainAction = 'EditWebSiteDomain';
                        $('#dlgAddWebSiteDomain').dialog({ title: '编辑域名' });
                        $('#dlgAddWebSiteDomain').dialog('open');
                    }
                },
                   {
                       iconCls: 'icon-delete',
                       text: '批量删除域名',
                       handler: function () {

                           var rows = $('#grvDomainData').datagrid('getSelections');

                           if (!EGCheckIsSelect(rows))
                               return;

                           $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                               if (r) {
                                   var ids = [];

                                   for (var i = 0; i < rows.length; i++) {
                                       ids.push("'" + rows[i].WebsiteDomain + "'");
                                   }

                                   var dataModel = {
                                       Action: 'DeleteWebSiteDomain',
                                       domain: ids.join(',')
                                   }

                                   $.ajax({
                                       type: 'post',
                                       url: handlerUrl,
                                       data: dataModel,
                                       dataType: "json",
                                       success: function (resp) {
                                           Alert(resp.Msg);
                                           $('#grvDomainData').datagrid('reload');
                                       }
                                   });
                               }
                           });
                       }
                   }]
            });

            $('#dlgAddWebSiteDomain').dialog({
                buttons: [{
                    text: "保存",
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: websiteDomainAction,
                                domain: $.trim($('#txtWebsiteDomain').val()),
                                inputWebsiteOwner: currGridSelectWebsiteOwner,
                                oldDomain: currGridSelectWebsiteDomain,
                                newDomain: $.trim($('#txtWebsiteDomain').val()),
                                templateId: $("#ddlTemplate").val()
                            }

                            if (dataModel.domain == '') {
                                Alert("请填写域名!");
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status > 0) {
                                        $('#dlgAddWebSiteDomain').dialog('close');
                                    }
                                    Alert(resp.Msg);
                                    $('#grvDomainData').datagrid('reload');
                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgAddWebSiteDomain').dialog('close');
                    }
                }]
            });

            $("#btnSearch").click(function () {
                var WebSiteName = $("#txtName").val();
                var Version = $("#version").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWebsiteInfo", WebSiteName: WebSiteName, Version: Version } });
            });

            $('#dlgAddWebsiteMenu').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        try {
                            var model = GetDlgMenuModel();
                            if (!CheckDlgInput(model)) {
                                return false;
                            }
                            $.ajax({
                                type: "Post",
                                url: handlerMenuUrl,
                                data: { Action: websiteMenuAction, JsonData: JSON.stringify(model).toString() },
                                success: function (result) {
                                    if (result == "true") {
                                        $.messager.show({
                                            title: '系统提示',
                                            msg: '提交成功.'
                                        });
                                        LoadMenuSelectList();
                                        $('#grvWebsiteMenuData').datagrid('reload');
                                        $("#dlgAddWebsiteMenu").window("close");
                                    } else {
                                        $.messager.alert("系统提示", "提交失败：" + result);
                                    }
                                }
                            });

                        } catch (e) {
                            alert(e);
                        }
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgAddWebsiteMenu').dialog('close');
                    }
                }]
            });
            $('#dlgEditBaseWebsiteMenu').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        try {
                            var model = GetDlgBaseMenuModel();
                            if (model['show_level'] == '') {
                                Alert("请选择显示级别");
                                return false;
                            }
                            $.ajax({
                                type: "Post",
                                url: handlerMenuUrl,
                                data: model,
                                success: function (result) {
                                    if (result == "true") {
                                        $.messager.show({
                                            title: '系统提示',
                                            msg: '提交成功.'
                                        });
                                        $('#grvWebsiteMenuData').datagrid('reload');
                                        $("#dlgEditBaseWebsiteMenu").window("close");
                                    } else {
                                        $.messager.alert("系统提示", "提交失败：" + result);
                                    }
                                }
                            });

                        } catch (e) {
                            alert(e);
                        }
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgEditBaseWebsiteMenu').dialog('close');
                    }
                }]
            });
            $('#dlgAddPermissionColumn').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        var nHandlerUrl = "";
                        if (websitePermissColumnAction == "Add") {
                            nHandlerUrl = handlerPermissioncolumnUrl + "add.ashx";
                        }
                        else if (websitePermissColumnAction == "Edit") {
                            nHandlerUrl = handlerPermissioncolumnUrl + "update.ashx";
                        }
                        var dataModel = {
                            "id": currSelectPermissionColumnID,
                            "name": $.trim($(txtPermissionColumnName).val()),
                            "pre_id": $.trim($('#ddlPermissionColumn').val()),
                            "order_num": $.trim($(txtPermissionColumnSort).val()),
                            "websiteOwner": currGridSelectWebsiteOwner
                        }
                        if (dataModel['name'] == '') {
                            $(txtPermissionColumnName).val("");
                            $(txtPermissionColumnName).focus();
                        }
                        if (dataModel["order_num"] == '') {
                            dataModel["order_num"] = 0;
                        }
                        if (dataModel["pre_id"] == '') {
                            dataModel["pre_id"] = 0;
                        }
                        $.ajax({
                            type: 'post',
                            url: nHandlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status == true) {
                                    $.messager.show({
                                        title: '系统提示',
                                        msg: resp.msg
                                    });
                                    LoadPermissColumnSelectList();
                                    $('#grvSetPermissionColumnData').datagrid('reload');
                                    $("#dlgAddPermissionColumn").window("close");
                                } else {
                                    $.messager.alert("系统提示", resp.msg);
                                }
                            }
                        });
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgAddPermissionColumn').dialog('close');
                    }
                }]
            });

            $("#divPermissionColumnPmss .checkPerType").live("click", function () {
                $(this).closest("fieldset").find(".checkPer").attr("checked", this.checked);
            })
            $("#divPermissionColumnMenus .checkMenuParent").live("click", function () {
                $(this).closest("fieldset").find(".checkMenu").attr("checked", this.checked);
            })
            $("#divPermissionColumnMenus .checkMenu").live("click", function () {
                if (this.checked) {
                    $(this).closest("fieldset").find(".checkMenuParent").attr("checked", this.checked);
                }
                else if ($(this).closest("fieldset").find(".checkMenu:checked").length == 0) {
                    $(this).closest("fieldset").find(".checkMenuParent").attr("checked", this.checked);
                }
            })

            $('#dlgPermissionColumnMenuSet').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        $.messager.confirm('系统提示', '确定设定栏目菜单?', function (o) {
                            if (o) {
                                try {
                                    var menuIdsStr = GetSelectPermissionColumnMenus();
                                    $.ajax({
                                        type: "post",
                                        url: handlerPermissionUrl + "setmenucheckedlist.ashx",
                                        data: { relation_id: currSetPermissionColumnMenuID, menu_ids: menuIdsStr },
                                        success: function (result) {
                                            if (result.status == true) {
                                                $.messager.show({ title: '系统提示', msg: '设置栏目菜单成功' });
                                                loadPermissionColumnData();
                                            } else {
                                                $.messager.alert("系统提示", result.msg);
                                            }
                                        }
                                    });

                                } catch (e) {
                                    alert(e);
                                }
                            } else {
                            }
                            $(dlgPermissionColumnMenuSet).dialog('close');
                        });
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgPermissionColumnMenuSet').dialog('close');
                    }
                }]
            });
            $('#dlgPermissionColumnPmsSet').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        if (curSetPerType != "SetDisablePer") {
                            $.messager.confirm('系统提示', '确定设定栏目权限?', function (o) {
                                if (o) {
                                    try {
                                        var pmsIdsStr = GetSelectPermissionColumnPms();
                                        $.ajax({
                                            type: "post",
                                            url: handlerPermissionUrl + "setpermissioncheckedlist.ashx",
                                            data: { relation_id: currSetPermissionColumnPerID, pms_ids: pmsIdsStr, rel_type: 2 },
                                            success: function (result) {
                                                if (result.status == true) {
                                                    $(dlgPermissionColumnPmsSet).dialog('close');
                                                    $('#grvData').datagrid('reload');
                                                    $.messager.show({ title: '系统提示', msg: '设置栏目权限成功' });
                                                    loadPermissionColumnData();
                                                } else {
                                                    $.messager.alert("系统提示", result.msg);
                                                }
                                            }
                                        });

                                    } catch (e) {
                                        alert(e);
                                    }
                                } else {
                                    $(dlgPermissionColumnPmsSet).dialog('close');
                                }
                            });
                        }
                        else {
                            $.messager.confirm('系统提示', '确定设置禁用权限?', function (o) {
                                if (o) {
                                    try {
                                        var pmsIdsStr = GetSelectPermissionColumnPms();
                                        $.ajax({
                                            type: "post",
                                            url: handlerPermissionUrl + "setpermissioncheckedlist.ashx",
                                            data: { relation_id: currGridSelectWebsiteOwner, pms_ids: pmsIdsStr, rel_type: 9 },
                                            success: function (result) {
                                                if (result.status == true) {
                                                    $(dlgPermissionColumnPmsSet).dialog('close');
                                                    $.messager.show({ title: '系统提示', msg: '设置禁用权限成功' });
                                                } else {
                                                    $.messager.alert("系统提示", result.msg);
                                                }
                                            }
                                        });

                                    } catch (e) {
                                        alert(e);
                                    }
                                } else {
                                    $(dlgPermissionColumnPmsSet).dialog('close');
                                }
                            });
                        }
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgPermissionColumnPmsSet').dialog('close');
                    }
                }]
            });

        });

        //显示添加对话框
        function ShowAdd() {
            weisiteAction = 'AddWebsite';
            $('#txtWebsiteDescription').val("");
            $('#txtWebsiteName').val("");
            $('#txtWebsiteOwner').val("");
            $('#txtWebsiteExpirationDate').datebox("setValue", "");
            $('#txtMaxSubAccountCount').val("5");
            $('#txtLogDay').val("7");
            //$("#trtemplate").show();
            $('#dlgAdd').dialog({ title: '添加新站点' });
            $('#dlgAdd').dialog('open');
            this.txtWebsiteOwner.readOnly = false;
        }
        //删除对话框
        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push("'" + rows[i].WebsiteOwner + "'");
                        }

                        var dataModel = {
                            Action: 'DeleteWebsite',
                            inputWebsiteOwner: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                Alert(resp.Msg);
                                $('#grvData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        //显示编辑
        function ShowWebsiteEdit() {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            window.location.href = "WebsiteCompile.aspx?websiteowner=" + rows[0].WebsiteOwner;
            //weisiteAction = 'EditWebsite';
            //this.txtWebsiteOwner.readOnly = true;
            //$('#txtWebsiteDescription').val(rows[0].WebsiteDescription);
            //$('#txtWebsiteName').val(rows[0].WebsiteName);
            //$('#txtWebsiteOwner').val(rows[0].WebsiteOwner);
            //$('#txtLogDay').val(rows[0].LogLimitDay);
            //var IsEnableLimitProductBuyTime=rows[0].IsEnableLimitProductBuyTime;
            //if (IsEnableLimitProductBuyTime ==1) {
            //    rdotime0.checked = true;
            //} else {
            //    rdotime1.checked = true;
            //}
            //var IsEnableAmountPay=rows[0].IsEnableAccountAmountPay;
            //if (IsEnableAmountPay ==1) {
            //    rdoenableamountpay.checked = true;
            //} else {
            //    rdodisableamountpay.checked = true;
            //}
            //var WebsiteExpirationDate = rows[0].WebsiteExpirationDate;
            //if (WebsiteExpirationDate != "") {
            //    WebsiteExpirationDate = FormatDate(WebsiteExpirationDate);
            //}
            //$('#txtWebsiteExpirationDate').datebox("setValue", WebsiteExpirationDate);
            //$('#txtAccountAmountPayShowName').val(rows[0].AccountAmountPayShowName);
            //$("#trtemplate").hide();
            //$('#txtMaxSubAccountCount').val(rows[0].MaxSubAccountCount);

            if (rows[0].DistributionMemberStandardsHaveParent == 1) {
                cbDistributionMemberStandardsHaveParent.checked = true;
            }
            else {
                cbDistributionMemberStandardsHaveParent.checked = false;
            }

            if (rows[0].DistributionMemberStandardsHavePay == 1) {
                cbDistributionMemberStandardsHavePay.checked = true;
            }
            else {
                cbDistributionMemberStandardsHavePay.checked = false;
            }

            if (rows[0].DistributionMemberStandardsHaveSuccessOrder == 1) {
                cbDistributionMemberStandardsHaveSuccessOrder.checked = true;
            }
            else {
                cbDistributionMemberStandardsHaveSuccessOrder.checked = false;
            }

            if (rows[0].DistributionRelationBuildQrCode == 1) {
                cbDistributionRelationBuildQrCode.checked = true;
            }
            else {
                cbDistributionRelationBuildQrCode.checked = false;
            }

            if (rows[0].DistributionRelationBuildSpreadActivity == 1) {
                cbDistributionRelationBuildSpreadActivity.checked = true;
            }
            else {
                cbDistributionRelationBuildSpreadActivity.checked = false;
            }

            if (rows[0].DistributionRelationBuildMallOrder == 1) {
                cbDistributionRelationBuildMallOrder.checked = true;
            }
            else {
                cbDistributionRelationBuildMallOrder.checked = false;
            }
        }

        //域名分配
        function SetDomain() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            currGridSelectWebsiteOwner = rows[0].WebsiteOwner;
            $('#grvDomainData').datagrid({ url: handlerUrl, queryParams: { Action: 'QueryWebSiteDomain', inputWebsiteOwner: rows[0].WebsiteOwner } });
            $('#dlgSetDomain').dialog('open');
        }
        function AddWebsiteMenu() {
            websiteMenuAction = 'Add';
            ClearWinDataByTag('input', dlgAddWebsiteMenu);
            $("#rd0").attr("checked", true);
            $("#rdtb0").attr("checked", true);
            $("#rdLevel3").attr("checked", true);
            $('#ddlPreMenu').val(0);
            $('#dlgAddWebsiteMenu').dialog({ title: '添加菜单' });
            $('#dlgAddWebsiteMenu').dialog('open');
        }
        function EditWebsiteMenu() {
            var rows = $('#grvWebsiteMenuData').datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            if (rows[0].MenuType == 1) {
                ClearWinDataByTag('input', dlgEditBaseWebsiteMenu);
                //加载编辑数据
                currSelectMenuID = rows[0].MenuID;
                var showLevel = rows[0].ShowLevel;
                if (showLevel == "1") {
                    if (rdLevel1) $("#rdbLevel1").attr("checked", true);
                } else if (showLevel == "2") {
                    $("#rdbLevel2").attr("checked", true);
                } else {
                    $("#rdbLevel3").attr("checked", true);
                }
                $(txtbNodeName).val($.trim(rows[0].NodeName.replace('└', '')));
                $(txtbUrl).val(rows[0].Url);
                $(txtbMenuSort).val(rows[0].MenuSort);
                $('#dlgEditBaseWebsiteMenu').dialog('open');
            }
            else {
                ClearWinDataByTag('input', dlgAddWebsiteMenu);
                //加载编辑数据
                currSelectMenuID = rows[0].MenuID;
                $('#ddlPreMenu').val(rows[0].PreID);
                $(txtNodeName).val($.trim(rows[0].NodeName).replace('└', ''));
                $(txtUrl).val(rows[0].Url);
                $(txtICOCSS).val(rows[0].ICOCSS);
                $(txtMenuSort).val(rows[0].MenuSort);
                var ishide = rows[0].IsHide;
                if (ishide == "1") {
                    $("#rd1").attr("checked", true);
                } else {
                    $("#rd0").attr("checked", true);
                }

                var targetblank = rows[0].TargetBlank;
                if (targetblank == "1") {
                    $("#rdtb1").attr("checked", true);
                } else {
                    $("#rdtb0").attr("checked", true);
                }

                var showLevel = rows[0].ShowLevel;
                if (showLevel == "1") {
                    if (rdLevel1) $("#rdLevel1").attr("checked", true);
                } else if (showLevel == "2") {
                    $("#rdLevel2").attr("checked", true);
                } else {
                    $("#rdLevel3").attr("checked", true);
                }

                websiteMenuAction = 'Edit';
                $('#dlgAddWebsiteMenu').dialog({ title: '编辑菜单' });
                $('#dlgAddWebsiteMenu').dialog('open');
            }
        }
        function DelWebsiteMenu() {
            var rows = $('#grvWebsiteMenuData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].MenuType == 2) ids.push(rows[i].MenuID);
            }
            if (ids.length == 0) {
                Alert("仅能删除自定义菜单");
                return;
            }
            $.messager.confirm("系统提示", "确认删除选中的自定义菜单?", function (r) {
                if (r) {
                    var dataModel = {
                        Action: 'Delete',
                        ids: ids.join(','),
                        WebsiteOwner: currGridSelectWebsiteOwner,
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerMenuUrl,
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            Alert("删除" + resp + "行记录");
                            $('#grvWebsiteMenuData').datagrid('reload');
                        }
                    });
                }
            });
        }
        function HideWebsiteMenu() {
            var rows = $('#grvWebsiteMenuData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            $.messager.confirm("系统提示", "确认隐藏选中的菜单?", function (r) {
                if (r) {

                    var ids = [];
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].MenuID);
                    }

                    var dataModel = {
                        Action: 'HideWebsiteMenu',
                        ids: ids.join(','),
                        WebsiteOwner: currGridSelectWebsiteOwner,
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerMenuUrl,
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            Alert("隐藏" + resp + "行菜单");
                            $('#grvWebsiteMenuData').datagrid('reload');
                        }
                    });
                }
            });
        }
        function ShowWebsiteMenu() {
            var rows = $('#grvWebsiteMenuData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            $.messager.confirm("系统提示", "确认显示选中的菜单?", function (r) {
                if (r) {

                    var ids = [];
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].MenuID);
                    }

                    var dataModel = {
                        Action: 'ShowWebsiteMenu',
                        ids: ids.join(','),
                        WebsiteOwner: currGridSelectWebsiteOwner,
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerMenuUrl,
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            Alert("显示" + resp + "行菜单");
                            $('#grvWebsiteMenuData').datagrid('reload');
                        }
                    });
                }
            });
        }
        function SearchWebsiteMenuData() {
            var model = {
                Action: 'Query',
                menuType: 2,
                showLevel: 1,
                websiteOwner:
                currGridSelectWebsiteOwner,
                showHide: $('#sltWebsiteMenuHide').combobox('getValue')
            }
            $('#grvWebsiteMenuData').datagrid('load', model);
        }
        function SetWebsiteMenus() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            //$.messager.progress({ title: "正在加载" });
            currGridSelectWebsiteOwner = rows[0].WebsiteOwner;
            LoadMenuSelectList();

            $('#dlgSetWebsiteMenu').dialog({ title: '站点 [' + currGridSelectWebsiteOwner + '] 菜单' });
            $('#dlgSetWebsiteMenu').dialog('open');
            var showHide = $('#sltWebsiteMenuHide').combobox('getValue');
            $('#grvWebsiteMenuData').datagrid({
                url: handlerMenuUrl,
                queryParams: { Action: 'Query', menuType: 2, showLevel: 1, websiteOwner: currGridSelectWebsiteOwner, showHide: showHide }
            });
        }
        function AddPermissionColumn(){
            websitePermissColumnAction = 'Add';
            ClearWinDataByTag('input', dlgAddPermissionColumn);
            $('#dlgAddPermissionColumn').dialog({ title: '添加栏目' });
            $('#dlgAddPermissionColumn').dialog('open');
        }
        function EditPermissionColumn() {
            var rows = $('#grvSetPermissionColumnData').datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            //加载编辑数据
            currSelectPermissionColumnID = rows[0].id;
            ClearWinDataByTag('input', dlgAddPermissionColumn);

            $('#ddlPermissionColumn').val(rows[0].pre_id);
            $(txtPermissionColumnName).val($.trim(rows[0].name).replace('└', ''));
            $(txtPermissionColumnSort).val(rows[0].order_num);

            websitePermissColumnAction = 'Edit';
            $('#dlgAddPermissionColumn').dialog({ title: '编辑菜单' });
            $('#dlgAddPermissionColumn').dialog('open');
        }
        function DelPermissionColumn() {
            var rows = $('#grvSetPermissionColumnData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].website_owner == currGridSelectWebsiteOwner) ids.push(rows[i].id);
            }
            if (ids.length == 0) {
                Alert("仅能删除自定义菜单");
                return;
            }
            $.messager.confirm("系统提示", "确认删除选中的自定义栏目(将递归删除子栏目，菜单，权限)?", function (r) {
                if (r) {
                    var dataModel = {
                        ids: ids.join(',')
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerPermissioncolumnUrl + "delete.ashx",
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: resp.msg
                                });
                                LoadPermissColumnSelectList();
                                $('#grvSetPermissionColumnData').datagrid('reload');
                            } else {
                                $.messager.alert("系统提示", "删除失败：" + resp.msg);
                            }
                        }
                    });
                }
            });
        }
        function SetPermissionColumnHide(ishide) {
            var rows = $('#grvSetPermissionColumnData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            if (ids.length == 0) {
                Alert("仅能删除自定义菜单");
                return;
            }
            var str = ishide == 1 ? '隐藏' : '显示';
            $.messager.confirm("系统提示", "确认" + str + "选中的栏目?", function (r) {
                if (r) {
                    var dataModel = {
                        ids: ids.join(','),
                        websiteOwner: currGridSelectWebsiteOwner,
                        hide:ishide
                    }
                    $.ajax({
                        type: 'post',
                        url: handlerPermissioncolumnUrl + "SetHide.ashx",
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: resp.msg
                                });
                                LoadPermissColumnSelectList();
                                $('#grvSetPermissionColumnData').datagrid('reload');
                            } else {
                                $.messager.alert("系统提示", str+"失败：" + resp.msg);
                            }
                        }
                    });
                }
            });
        }
        function SetPermissionColumns() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            currGridSelectWebsiteOwner = rows[0].WebsiteOwner;
            LoadPermissColumnSelectList();
            var showHide = $('#ddlSelectColumnHide').combobox('getValue');
            $('#grvSetPermissionColumnData').datagrid({
                url: handlerPermissioncolumnUrl + "list.ashx",
                queryParams: { website_owner: currGridSelectWebsiteOwner, show_hide: showHide }
            });
            GetWebsiteMenuList();
            $('#dlgSetPermissionColumns').dialog({ title: '站点 [' + currGridSelectWebsiteOwner + '] 栏目' });
            $('#dlgSetPermissionColumns').dialog('open');
        }
        function SearchPermissionColumnData() {
            var model = {
                website_owner: currGridSelectWebsiteOwner,
                show_hide: $('#ddlSelectColumnHide').combobox('getValue')
            }
            $('#grvSetPermissionColumnData').datagrid('load', model);
        }
        
        //加载菜单选择列表
        function LoadPermissColumnSelectList() {
            $.post(handlerPermissioncolumnUrl + "selectlist.ashx", { website_owner: currGridSelectWebsiteOwner }, function (data) {
                if (data.status && data.result) {
                    $("#spPermissionColumn_menu").html(data.result);
                }
            });
        }
        function FormatShowLevel(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">超级管理员可见</span>');
            }
            else if (_Num == 2) {
                str.AppendFormat('<span style="color:blue;">站点管理员可见</span>');
            }
            else {
                str.AppendFormat('所有帐号可见');
            }
            return str.ToString();
        }
        function FormatTargetBlank(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">新页</span>');
            }
            else {
                str.AppendFormat('本页');
            }
            return str.ToString();
        }
        function FormatHide(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">隐藏</span>');
            }
            else {
                str.AppendFormat('显示');
            }
            return str.ToString();
        }
        function FormatMenuType(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">基础菜单</span>');
            }
            else if (_Num == 2) {
                str.AppendFormat('<span style="color:blue;">自定义菜单</span>');
            }
            return str.ToString();
        }
        function FormatterMenuTitle(value, rowData) {
            var BaseName = $.trim(rowData["BaseName"]);
            if (BaseName == "") {
                return FormatterTitle(value);
            }
            else {
                var str = new StringBuilder();
                str.AppendFormat('{0}<span class=\"dOldName\" style=\"color:red;\">(原:{1})</span>', value, BaseName);
                return str.ToString();
            }
        }
        //加载菜单选择列表
        function LoadMenuSelectList() {
            var showHide = $('#sltWebsiteMenuHide').combobox('getValue');
            $.post(handlerMenuUrl, { Action: "GetMenuSelectList", menuType: 2, showLevel: 1, showPreMenu: 1, websiteOwner: currGridSelectWebsiteOwner, showHide: showHide }, function (data) {
                $("#sp_menu").html(data);
            });
        }
        //获取对话框数据实体
        function GetDlgMenuModel() {
            var showLevel = "";
            if (rdLevel2.checked) {
                showLevel = "2";
            }
            else if (rdLevel3.checked) {
                showLevel = "3";
            }
            else if (rdLevel1.checked) {
                showLevel = "1";
            }

            var model =
            {
                "MenuID": currSelectMenuID,
                "NodeName": $.trim($(txtNodeName).val()),
                "Url": $.trim($(txtUrl).val()),
                "PreID": $('#ddlPreMenu').val(),
                "ICOCSS": $.trim($(txtICOCSS).val()),
                "MenuSort": $.trim($(txtMenuSort).val()),
                "IsHide": 0,
                "TargetBlank": rdtb0.checked ? 0 : 1,
                "WebsiteOwner": currGridSelectWebsiteOwner,
                "MenuType": 2
            }
            if (showLevel != "") model["ShowLevel"] = showLevel;
            return model;
        }
        //获取对话框数据实体
        function GetDlgBaseMenuModel() {
            var showLevel = "";
            if (rdbLevel2.checked) {
                showLevel = "2";
            }
            else if (rdbLevel3.checked) {
                showLevel = "3";
            }
            else if (rdbLevel1.checked) {
                showLevel = "1";
            }

            var model =
            {
                "Action": "EditBaseMenu",
                "menu_id": currSelectMenuID,
                "menu_name": $.trim($(txtbNodeName).val()),
                "menu_sort": $.trim($(txtbMenuSort).val()),
                "website_owner": currGridSelectWebsiteOwner
            }
            if (showLevel != "") model["show_level"] = showLevel;
            return model;
        }
        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['NodeName'] == '') {
                $(txtNodeName).val("");
                $(txtNodeName).focus();
                return false;
            }
            return true;
        }
        function FormatterIsHide(value, rowData) {
            if (value == "1") return '<span style="color:red;">隐藏</span>'
            return '显示';
        }
        function FormatterIsNewInfo(value, rowData) {
            var str = new StringBuilder();
            if (rowData["website_owner"] && rowData["website_owner"] != "") {
                if (rowData["base_id"] != 0) {
                    str.AppendFormat('<span style="{0}">{1}</span>', "color:blue;", "修改栏目");
                }
                else {
                    str.AppendFormat('<span style="{0}">{1}</span>', "color:green;", "自定义栏目");
                }
            }
            else {
                str.AppendFormat('<span style="{0}">{1}</span>', "color:red;", "基础栏目");
            }
            return str.ToString();
        }
        function FormatterMenu(value, rowData) {
            var str = new StringBuilder();
            if (rowData["website_owner"] && rowData["website_owner"] != "" && rowData["base_id"] == 0) {
                var color = value == false ? "color:red;" : "color:green;";
                str.AppendFormat('<a style="{0}" href="javascript:ShowPermissionColumnMenus({1})">菜单</a>', color, rowData["id"]);
            }
            return str.ToString();
        }
        function FormatterPermission(value, rowData) {
            var str = new StringBuilder();
            if (rowData["website_owner"] && rowData["website_owner"] != "" && rowData["base_id"] == 0) {
                var color = value == false ? "color:red;" : "color:green;";
                str.AppendFormat('<a style="{0}" href="javascript:ShowPermissionColumnPermissions({1})">权限</a>', color, rowData["id"]);
            }
            return str.ToString();
        }
        function ShowPermissionColumnMenus(col_id) {
            if (currSetPermissionColumnMenuID == col_id) {
                $(dlgPermissionColumnMenuSet).dialog('open');
                return;
            }
            $.messager.progress({ text: '正在加载。。。' });
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "checkedmenulist.ashx",
                data: { relation_id: col_id },
                success: function (result) {
                    $.messager.progress('close');
                    if (result.status == true) {
                        currSetPermissionColumnMenuID = col_id;
                        $("#divPermissionColumnMenus .ckMenu").attr("checked", false);
                        for (var i = 0; i < result.result.length; i++) {
                            var nmenu_id = result.result[i];
                            $("#divPermissionColumnMenus .cbMenu_" + nmenu_id).attr("checked", true);
                        }
                        $(dlgPermissionColumnMenuSet).dialog('open');
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }
        function ShowPermissionColumnPermissions(col_id) {
            if (currSetPermissionColumnPerID == col_id) {
                $(dlgPermissionColumnPmsSet).dialog('open');
                return;
            }
            curSetPerType = "SetColumnPer";//设置栏目权限

            $.messager.progress({ text: '正在加载。。。' });
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "checkedpermissionlist.ashx",
                data: { relation_id: col_id },
                success: function (result) {
                    $.messager.progress('close');
                    if (result.status == true) {
                        currSetPermissionColumnPerID = col_id;
                        $("#divPermissionColumnPmss .ckPer").attr("checked", false);
                        for (var i = 0; i < result.result.length; i++) {
                            var npms_id = result.result[i];
                            $("#divPermissionColumnPmss .ckPer_" + npms_id).attr("checked", true);
                        }
                        $(dlgPermissionColumnPmsSet).dialog({ title: '栏目权限设置' });
                        $(dlgPermissionColumnPmsSet).dialog('open');
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }


        function GetWebsiteMenuList() {
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "websitemenulist.ashx",
                data: { websiteowner: currGridSelectWebsiteOwner },
                success: function (result) {
                    if (result.status == true) {
                        var str = new StringBuilder();
                        for (var i = 0; i < result.result.length; i++) {
                            str.AppendFormat('<fieldset style="padding: 0px 10px 10px 10px; margin-top:10px; ">');
                            str.AppendFormat('<legend><input id="cbMenu_{0}" title="{1}" type="checkbox" name="checkmenu" class="positionTop2 checkMenuParent ckMenu cbMenu_{0}" {2} value="{0}" /> <label title="{1}" for="cbMenu_{0}">{1}</label></legend>',
                                result.result[i].menu_id, result.result[i].menu_name,
                                    result.result[i].menu_checked ? 'checked="checked"' : "");
                            str.AppendFormat('<ul style="width:100%;">');
                            for (var j = 0; j < result.result[i].children.length; j++) {
                                str.AppendFormat('<li style="width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;">', _mw);
                                str.AppendFormat('<input id="cbMenu_{0}" title="{1}" type="checkbox" name="checkmenu" class="positionTop2 checkMenu ckMenu cbMenu_{0}" {2} value="{0}" /><label title="{1}" for="cbMenu_{0}">{1}</label><br />',
                                    result.result[i].children[j].menu_id, result.result[i].children[j].menu_name,
                                    result.result[i].children[j].menu_checked ? 'checked="checked"' : "");
                                str.AppendFormat('</li>');
                            }
                            str.AppendFormat("</ul>");
                            str.AppendFormat('</fieldset>');
                        }
                        $("#divPermissionColumnMenus").html("");
                        $("#divPermissionColumnMenus").append(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }

        function GetWebsitePermissionList() {
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "websitepermissionlist.ashx",
                success: function (result) {
                    if (result.status == true) {
                        var str = new StringBuilder();
                        for (var i = 0; i < result.result.length; i++) {
                            str.AppendFormat('<fieldset style="padding: 0px 10px 10px 10px; margin-top:10px; ">');
                            str.AppendFormat('<legend><input id="cbPerType_{0}" title="{1}" type="checkbox" name="checkPerType" class="positionTop2 checkPerType ckPer " value="{0}" /> <label title="{1}" for="cbPerType_{0}">{1}</label></legend>', result.result[i].cate_id, result.result[i].cate_name);
                            str.AppendFormat('<ul style="width:100%;">');
                            for (var j = 0; j < result.result[i].permission_list.length; j++) {
                                str.AppendFormat('<li style="width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;">', _w);
                                str.AppendFormat('<input id="cbPer_{0}" title="{1}" type="checkbox" name="checksingle" class="positionTop2 checkPer ckPer ckPer_{0}" {2} value="{0}" /><label title="{1}" for="cbPer_{0}">{1}</label><br />',
                                    result.result[i].permission_list[j].permission_id, result.result[i].permission_list[j].permission_name,
                                    result.result[i].permission_list[j].permission_checked ? 'checked="checked"' : "");
                                str.AppendFormat('</li>');
                            }
                            str.AppendFormat("</ul>");
                            str.AppendFormat('</fieldset>');
                        }
                        $("#divPermissionColumnPmss").html("");
                        $("#divPermissionColumnPmss").append(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }

        //获取选中菜单
        function GetSelectPermissionColumnMenus() {
            var ids = [];
            $('#divPermissionColumnMenus input[type="checkbox"][name="checkmenu"]:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }

        //获取选中权限
        function GetSelectPermissionColumnPms() {
            var ids = [];
            $('#divPermissionColumnPmss .checkPer:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }
        function SetPermissionDisable() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            currGridSelectWebsiteOwner = rows[0].WebsiteOwner;

            curSetPerType = "SetDisablePer";//设置禁用权限

            var curWebsiteDisablePermissions = $.trim(rows[0].DisablePermissions);
            $("#divPermissionColumnPmss .ckPer").attr("checked", false);
            if (curWebsiteDisablePermissions != "") {
                var disablePermissionsList = curWebsiteDisablePermissions.split(',');
                for (var i = 0; i < disablePermissionsList.length; i++) {
                    var npms_id = disablePermissionsList[i];
                    $("#divPermissionColumnPmss .ckPer_" + npms_id).attr("checked", true);
                }
            }

            $(dlgPermissionColumnPmsSet).dialog({ title: '禁用权限设置' });
            $(dlgPermissionColumnPmsSet).dialog('open');
        }
    </script>
</asp:Content>
