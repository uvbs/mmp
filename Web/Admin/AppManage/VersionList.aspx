<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VersionList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.AppManage.VersionList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%= Request["os"] =="android"?"安卓":"苹果" %>版本列表

    <a href="/Admin/AppManage/AppManageList.aspx?websiteOwner=<%= Request["oWebsiteOwner"] %>" style="float: right; margin-right: 20px;" title="返回活动列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/appmanage/Version/list.ashx',pagination:true,striped:true,loadFilter:pagerFilter,rownumbers:true,showFooter:true,
        queryParams:{websiteOwner:'<%= Request["websiteOwner"] %>',os:'<%= Request["os"] %>' }">
        <thead>
            <tr>
                <th field="AppVersion" width="50" formatter="FormatterTitle">版本</th>
                <th field="AppVersionInfo" width="50" formatter="FormatterTitle">版本信息</th>
                <th field="AppVersionPublishPath" width="100" formatter="FormatterQrCode">二维码</th>
                <th field="AppVersionPublishDate" width="50" formatter="FormatterTitle">发布时间</th>
                <th field="act" width="30" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add2"
            plain="true" id="btnAdd" onclick="OpenAddShow()">新增版本</a>
        <br />
        关键字:<input type="text" id="txtKeyword" style="width: 200px" />
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
    </div>
    <div  id="dlgAdd" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'新增版本',width:560,height:420,modal:true,buttons:'#dlgAddButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" style="width:95%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:100px;">版本 <span style="color:red;">*</span>：</td>
                <td>
                    <input id="txtAppVersion" type="text" style="width: 90%;" placeholder="格式如：1.0.1" />
                </td>
            </tr>
            <tr>
                <td style="width:80px;">发布平台：</td>
                <td>
                    <input id="txtAppVersionPublish" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td style="line-height: 16px;">发布地址<span style="color:red;">*</span><br />（二维码）：</td>
                <td>
                    <input id="txtAppVersionPublishPath" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td style="line-height: 16px;">安装包地址<span style="color:red;">*</span><br />（自动更新）：</td>
                <td>
                    <input id="txtAppVersionInstallPath" type="text" style="width: 90%;" />
                    <% if(Request["os"] == "android"){ %>
                        <a href="javascript:void(0);" style="color:blue;" onclick="document.getElementById('fileAppVersionInstallPath').click()">上传</a>
                        <input id="fileAppVersionInstallPath" type="file" style="width:0px; height:0px;" onchange="uploadApk(this.files[0])" />
                    <%} %>
                </td>
            </tr>
            <tr>
                <td>发布日期：</td>
                <td>
                    <input id="txtAppVersionPublishDate" class="easyui-datebox" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>版本信息：</td>
                <td>
                    <div class="txtContent" contenteditable="true" 
                        style="width: 90%;line-height:21px;min-height:117px;padding:5px;border:solid #d3d3d3 1px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAddButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="SavePost()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgAdd').dialog('close');">取消</a>
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
        var os = '<%= Request["os"]%>';
        var mid = '<%= Request["mid"]%>';
        var handlerUrl = '/serv/api/admin/AppManage/Version/';
        var AutoID = 0;
        var postLock = false;
        var searchModel = {
            websiteOwner: websiteOwner
        };
        function FormatterQrCode(value, rowData) {
            var str = new StringBuilder();
            if (value) {
                str.AppendFormat('<a style="color:blue;" href="javascript:void(0);" onclick="showQRCode(\'{0}\')">二维码</a>'
                  , value);
            } else {
                str.AppendFormat('-');
            }
            return str.ToString();
        }
        function FormatterAction(value, rowData,index) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="javascript:OpenEditShow({0})">编辑</a>', index);
            return str.ToString();
        }
        function OpenAddShow() {
            ClearWinDataByTag('input', '#dlgAdd');
            AutoID = 0;
            $('#txtAppVersionPublishDate').datebox('setValue', '');
            $(".txtContent").html('');
            $('#dlgAdd').dialog({title: '新增应用'});
            $('#dlgAdd').dialog('open');
        }
        function OpenEditShow(n_index) {
            var rows = $('#grvData').datagrid('getData').rows;
            var row = rows[n_index];
            ClearWinDataByTag('input', '#dlgAdd');
            AutoID = row.AutoID;
            $('#txtAppVersion').val(row.AppVersion);
            $('#txtAppVersionPublish').val(row.AppVersionPublish);
            $('#txtAppVersionPublishPath').val(row.AppVersionPublishPath);
            $('#txtAppVersionInstallPath').val(row.AppVersionInstallPath);
            $('#txtAppVersionPublishDate').datebox('setValue', row.AppVersionPublishDate);
            $('.txtContent').html(row.AppVersionInfo);
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
                AppVersion: $.trim($("#txtAppVersion").val()),
                AppVersionPublish: $.trim($("#txtAppVersionPublish").val()),
                AppVersionPublishPath: $.trim($("#txtAppVersionPublishPath").val()),
                AppVersionInstallPath: $.trim($("#txtAppVersionInstallPath").val()),
                AppVersionInfo: $.trim($(".txtContent").html()),
                AppVersionPublishDate: $.trim($("#txtAppVersionPublishDate").datebox('getValue')),
                AppOS: os,
                ManageId: mid,
                WebsiteOwner:websiteOwner
            }
            return model;
        }
        function CheckDigModel(model) {
            if (model.AppVersion == "") {
                $("#txtAppVersion").val("");
                $("#txtAppVersion").focus();
                $.messager.alert("系统提示", "版本不能为空");
                return false;
            }
            if (model.AppVersionPublishPath == "") {
                $("#txtAppVersionPublishPath").val("");
                $("#txtAppVersionPublishPath").focus();
                $.messager.alert("系统提示", "发布地址不能为空");
                return false;
            }
            if (model.AppVersionInstallPath == "") {
                $("#txtAppVersionInstallPath").val("");
                $("#txtAppVersionInstallPath").focus();
                $.messager.alert("系统提示", "安装包地址不能为空");
                return false;
            }
            return true;
        }
        function uploadApk(file) {
            var fd = new FormData();//创建表单数据对象
            fd.append('file1', file);//将文件添加到表单数据中
            fd.append('action', 'Add');
            fd.append('dir', 'app');
            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", progress, false);//监听上传进度
            xhr.addEventListener("load", complete, false);
            xhr.addEventListener("error", error, false);
            xhr.open("POST", '/Serv/API/Common/File.ashx');
            xhr.send(fd);
            $.messager.progress();
        }
        function progress(progressData) {

        }
        function complete(completeData) {
            $.messager.progress('close');
            var resp = JSON.parse(completeData.target.responseText);
            if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                $("#txtAppVersionInstallPath").val(resp.file_url_list[0]);
            }
            else {
                $.messager.alert("系统提示", resp.errmsg);
            }
        }
        function error(errorData) {
            $.messager.progress('close');
            $.messager.alert("系统提示", "上传安装包出错");
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
