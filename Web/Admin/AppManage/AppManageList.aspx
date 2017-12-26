<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AppManageList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.AppManage.AppManageList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    应用列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/appmanage/list.ashx',pagination:true,striped:true,loadFilter:pagerFilter,rownumbers:true,showFooter:true,
        queryParams:{websiteOwner:'<%= Request["websiteOwner"] %>' }">
        <thead>
            <tr>
                <th field="AppId" width="50" formatter="FormatterTitle">应用Id</th>
                <th field="AppName" width="50" formatter="FormatterTitle">应用名称</th>
                <th field="AppInfo" width="100" formatter="FormatterTitle">应用信息</th>
                <th field="AndroidCount" width="50" formatter="FormatterAndroid">安卓</th>
                <th field="IosCount" width="50" formatter="FormatterIos">苹果</th>
                <th field="pay" width="50" formatter="FormatterPay">支付设置</th>
                <th field="act" width="30" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add2"
            plain="true" id="btnAdd" onclick="OpenAddShow()">新增应用</a>
        <br />
        关键字:<input type="text" id="txtKeyword" style="width: 200px" />
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
    </div>
    <div id="dlgAdd" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'新增应用',width:450,height:320,modal:true,buttons:'#dlgAddButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" style="width:95%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:80px;">应用Id <span style="color:red;">*</span>：</td>
                <td>
                    <input id="txtAppId" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>应用名称 <span style="color:red;">*</span>：</td>
                <td>
                    <input id="txtAppName" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>应用信息：</td>
                <td>
                    <div class="txtContent" 
                        contenteditable="true" 
                        style="width: 90%;line-height:21px;min-height:117px;padding:5px;border:solid #d3d3d3 1px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td>启动广告：</td>
                <td>
                    <input id="txtStartAdHref" type="text" style="width: 90%;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAddButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="SavePost()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgAdd').dialog('close');">关闭</a>
    </div>
    <div id="dlgPay" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'支付设置',width:500,height:320,modal:true,buttons:'#dlgPayButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" style="width:95%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:120px;">支付宝AppId：</td>
                <td>
                    <input id="txtAlipayAppId" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>支付宝私钥：</td>
                <td>
                    <input id="txtAlipayPrivatekey" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>支付宝公钥：</td>
                <td>
                    <input id="txtAlipayPublickey" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>支付宝签名类型：</td>
                <td>
                    <input id="txtAlipaySignType" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>微信开放平台AppId：</td>
                <td>
                    <input id="txtWxAppId" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>微信开放平台Secret：</td>
                <td>
                    <input id="txtWxAppSecret" type="text" style="width: 90%;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgPayButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="SavePostPay()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgPay').dialog('close');">关闭</a>
    </div>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var websiteOwner = '<%= Request["websiteOwner"]%>';
        var handlerUrl = '/serv/api/admin/AppManage/';
        var AutoID = 0;
        var nIndex = -1;
        var setWebsiteOwner = '';
        var postLock = false;
        var searchModel = {
            websiteOwner: websiteOwner
        };
        function FormatterAndroid(value, rowData) {
            var str = new StringBuilder();
            if (value > 0) {
                str.AppendFormat('<a style="color:blue;" href="/Admin/AppManage/VersionList.aspx?os=android&websiteOwner={0}&mid={2}&oWebsiteOwner={3}">({1})</a> '
                    , rowData.WebsiteOwner, value, rowData.AutoID, websiteOwner);
                if (rowData.AndroidPath) {
                    str.AppendFormat(' <a style="color:blue;" href="javascript:void(0);" onclick="showQRCode(\'{0}\')">二维码</a>'
                      , rowData.AndroidPath);
                }
            } else {
                str.AppendFormat('<a style="color:red;" href="/Admin/AppManage/VersionList.aspx?os=android&websiteOwner={0}&mid={2}&oWebsiteOwner={3}">({1})</a>'
                    , rowData.WebsiteOwner, value, rowData.AutoID, websiteOwner);
            }
            return str.ToString();
        }
        function FormatterIos(value, rowData) {
            var str = new StringBuilder();
            if (value > 0) {
                str.AppendFormat('<a style="color:blue;" href="/Admin/AppManage/VersionList.aspx?os=ios&websiteOwner={0}&mid={2}&oWebsiteOwner={3}">({1})</a> '
                    , rowData.WebsiteOwner, value, rowData.AutoID, websiteOwner);
                if (rowData.AndroidPath) {
                    str.AppendFormat(' <a style="color:blue;" href="javascript:void(0);" onclick="showQRCode(\'{0}\')">二维码</a>'
                      , rowData.IosPath);
                }
            } else {
                str.AppendFormat('<a style="color:red;" href="/Admin/AppManage/VersionList.aspx?os=ios&websiteOwner={0}&mid={2}&oWebsiteOwner={3}">({1})</a>'
                    , rowData.WebsiteOwner, value, rowData.AutoID, websiteOwner);
            }
            return str.ToString();
        }
        function FormatterAction(value, rowData,index) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="javascript:OpenEditShow({0})">编辑</a>', index);
            return str.ToString();
        }
        function FormatterPay(value, rowData, index) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="javascript:OpenPayShow({0})">支付设置</a>', index);
            return str.ToString();
        }

        function OpenAddShow() {
            ClearWinDataByTag('input', '#dlgAdd');
            AutoID = 0;
            $(".txtContent").html('');
            $('#dlgAdd').dialog({title: '新增应用'});
            $('#dlgAdd').dialog('open');
        }
        function OpenEditShow(n_index) {
            var rows = $('#grvData').datagrid('getData').rows;
            var row = rows[n_index];
            ClearWinDataByTag('input', '#dlgAdd');
            nIndex = n_index;
            AutoID = row.AutoID;
            $('#txtAppId').val(row.AppId);
            $('#txtAppName').val(row.AppName);
            $('.txtContent').html(row.AppInfo);
            $('#txtStartAdHref').val(row.StartAdHref);
            $('#dlgAdd').dialog({ title: '编辑应用' });
            $('#dlgAdd').dialog('open');
        }
        function SavePost() {
            if (postLock) {
                return;
            }
            postLock = true;
            var model = GetDlgModel();
            if (!CheckDigModel(model)) {
                postLock = false;
                return;
            }
            $.ajax({
                type: "post",
                url: handlerUrl + "Post.ashx",
                data: model,
                success: function (result) {
                    postLock = false;
                    if (result.status == true) {
                        $.messager.show({ title: '系统提示', msg: '提交成功' });
                        var rows = $('#grvData').datagrid('getData').rows;
                        var row = rows[nIndex];
                        row.StartAdHref = model.StartAdHref;
                        $('#dlgAdd').dialog('close');
                        $('#grvData').datagrid('reload');
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                },
                error: function () {
                    postLock = false;
                }
            });
        }
        function GetDlgModel() {
            var model = {
                AutoID: AutoID,
                AppId: $.trim($("#txtAppId").val()),
                AppName: $.trim($("#txtAppName").val()),
                AppInfo: $.trim($(".txtContent").html()),
                StartAdHref: $.trim($("#txtStartAdHref").val()),
                WebsiteOwner: (websiteOwner == 'all' ? '' : websiteOwner)
            }
            return model;
        }
        function CheckDigModel(model) {
            if (model.AppId == "") {
                $("#txtAppId").val("");
                $("#txtAppId").focus();
                $.messager.alert("系统提示", "应用Id不能为空");
                return false;
            }
            if (model.AppName == "") {
                $("#txtAppName").val("");
                $("#txtAppName").focus();
                $.messager.alert("系统提示", "应用名称不能为空");
                return false;
            }
            return true;
        }
        function OpenPayShow(n_index) {
            var rows = $('#grvData').datagrid('getData').rows;
            var row = rows[n_index];
            ClearWinDataByTag('input', '#dlgPay');
            nIndex = n_index;
            AutoID = row.AutoID;
            setWebsiteOwner = row.WebsiteOwner;
            if (row.hasOther) {
                $('#txtAlipayAppId').val(row.AlipayAppId);
                $('#txtAlipayPrivatekey').val(row.AlipayPrivatekey);
                $('#txtAlipayPublickey').val(row.AlipayPublickey);
                $('#txtAlipaySignType').val(row.AlipaySignType);
                $('#txtWxAppId').val(row.WxAppId);
                $('#txtWxAppSecret').val(row.WxAppSecret);
            } else {
                $.ajax({
                    type: "post",
                    url: handlerUrl + "Get.ashx",
                    data: {
                        id: AutoID,
                        websiteOwner: row.WebsiteOwner
                    },
                    success: function (data) {
                        if (data.status) {
                            row.AlipayAppId = data.result.AlipayAppId;
                            row.AlipayPrivatekey = data.result.AlipayPrivatekey;
                            row.AlipayPublickey = data.result.AlipayPublickey;
                            row.AlipaySignType = data.result.AlipaySignType;
                            row.WxAppId = data.result.WxAppId;
                            row.WxAppSecret = data.result.WxAppSecret;
                            row.hasOther = true;
                            $('#txtAlipayAppId').val(row.AlipayAppId);
                            $('#txtAlipayPrivatekey').val(row.AlipayPrivatekey);
                            $('#txtAlipayPublickey').val(row.AlipayPublickey);
                            $('#txtAlipaySignType').val(row.AlipaySignType);
                            $('#txtWxAppId').val(row.WxAppId);
                            $('#txtWxAppSecret').val(row.WxAppSecret);
                        }
                    },
                    error: function () {
                    }
                });
            }
            $('#dlgPay').dialog('open');
        }
        function SavePostPay() {
            if (postLock) {
                return;
            }
            postLock = true;
            var model = GetDlgPayModel();
            if (!CheckDigModel(model)) {
                postLock = false;
                return;
            }
            $.ajax({
                type: "post",
                url: handlerUrl + "PostPay.ashx",
                data: model,
                success: function (result) {
                    postLock = false;
                    if (result.status == true) {
                        $.messager.show({ title: '系统提示', msg: '提交成功' });
                        var rows = $('#grvData').datagrid('getData').rows;
                        var row = rows[nIndex];
                        row.AlipayAppId = model.AlipayAppId;
                        row.AlipayPrivatekey = model.AlipayPrivatekey;
                        row.AlipayPublickey = model.AlipayPublickey;
                        row.AlipaySignType = model.AlipaySignType;
                        row.WxAppId = model.WxAppId;
                        row.WxAppSecret = model.WxAppSecret;
                        row.hasOther = true;
                        $('#dlgPay').dialog('close');
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                },
                error: function () {
                    postLock = false;
                }
            });
        }
        function GetDlgPayModel() {
            var model = {
                AutoID: AutoID,
                AlipayAppId: $.trim($("#txtAlipayAppId").val()),
                AlipayPrivatekey: $.trim($("#txtAlipayPrivatekey").val()),
                AlipayPublickey: $.trim($("#txtAlipayPublickey").val()),
                AlipaySignType: $.trim($("#txtAlipaySignType").val()),
                WxAppId: $.trim($("#txtWxAppId").val()),
                WxAppSecret: $.trim($("#txtWxAppSecret").val()),
                WebsiteOwner: setWebsiteOwner
            }
            return model;
        }
        function showQRCode(url) {
            //弹出二维码框
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=' + encodeURI(url));
            $("#alinkurl").attr("href", url);
            $('#dlgSHowQRCode').dialog('open');
        }
        function Search() {
            searchModel.keyword = $("#txtKeyword").val();
            $('#grvData').datagrid('load', searchModel);
        }
    </script>
</asp:Content>
