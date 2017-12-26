<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LawyerList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.LawyerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;律师列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetTutor" onclick="ActionEvent('setTutor','确定所选用户设置为专家?')">批量设置专家</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnUpdateLawyer" onclick="EditRow()">修改律师资料</a>
            关键字匹配:<input id="txtKeyword" style="width: 200px;"  placeholder="用户名，姓名，邮箱" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="律师资料修改" style="width: 700px;padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    用户名:
                </td>
                <td>
                    <input id="txtUserID" type="text" readonly="readonly" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    姓名:
                </td>
                <td>
                    <input id="txtName" type="text" style="width: 250px;" />
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
            <tr>
                <td>
                    律师事务所:
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    事务所地址:
                </td>
                <td>
                    <input id="txtCompanyAddress" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    律师执业证编号:
                </td>
                <td>
                    <input id="txtLawyerLicenseNo" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    律师执业证照片:
                </td>
                <td style="height:106px;">
                    <img alt="缩略图" src="" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                    <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*750。
                    <input type="file" id="txtThumbnailsPath" name="file1" class="hidden"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/UserHandler.ashx";
        var currDlgAction = '';
        var $document = $(document);
        var domain = '<%=Request.Url.Host%>';

     $(function () {
         $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getUserList",type:3 },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        //{ field: 'userId', title: '用户名', width: 80, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'avatar', title: '头像', width: 40, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'name', title: '姓名', width: 50, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="/#/userspace/{1}" title="{0}" target="_blank">{0}</a>', value, rowData.id);
                                return str.ToString();
                            }
                        },
                        { field: 'email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'IDPhoto1', title: '身份照片', width: 100, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                if (value != null && value != '')
                                    str.AppendFormat(' <a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}"  height="50" width="50" /></a>', value);
                                if (rowData.IDPhoto2 != null && rowData.IDPhoto2 != '')
                                    str.AppendFormat(' <a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}"  height="50" width="50" /></a>', rowData.IDPhoto2);

                                if (str.ToString() != "") { str.AppendFormat('<br />');}
                                str.AppendFormat('身份：<span title="{0}">{0}</span><br />', rowData.userType);
                                str.AppendFormat('身份证：<span title="{0}">{0}</span>', rowData.idCardNo);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'lawyerLicensePhoto', title: '执业证', width: 40, align: 'left', formatter: function (value, rowData) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="60" /></a>', value);
                                if (str.ToString() != "") { str.AppendFormat('<br />'); }
                                str.AppendFormat('{0}', rowData.lawyerLicenseNo);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'phone', title: '联系方式', width: 80, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('手机：<span title="{0}">{0}</span><br />', value);
                                str.AppendFormat('座机：<span title="{0}">{0}</span>', rowData["tel"]);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'company', title: '公司', width: 100, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('公司：<span title="{0}">{0}</span><br />', value);
                                str.AppendFormat('地址：<span title="{0}">{0}</span>', rowData["companyAddress"]);
                                return str.ToString();
                            }
                        }
	                ]]
	            }
            );
         $('#dlgInfo').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     var dataModel = {
                         Action: "updateLawyer",
                         UserID: $.trim($('#txtUserID').val()),
                         TrueName: $.trim($('#txtName').val()),
                         Postion:$.trim($('#txtPostion').val()),
                         Company: $.trim($('#txtCompany').val()),
                         CompanyAddress: $.trim($('#txtCompanyAddress').val()),
                         LawyerLicenseNo: $.trim($('#txtLawyerLicenseNo').val()),
                         LawyerLicensePhoto: $('#imgThumbnailsPath').attr("src")
                     }
                     if (dataModel.UserID == '') {
                         Alert("用户名不能为空!");
                         return;
                     }
                     if (dataModel.TrueName == '') {
                         Alert("姓名不能为空!");
                         return;
                     }
                     if (dataModel.Postion == '') {
                         Alert("职位不能为空!");
                         return;
                     }
                     if (dataModel.Company == '') {
                         Alert("公司不能为空!");
                         return;
                     }
                     if (dataModel.CompanyAddress == '') {
                         Alert("公司地址不能为空!");
                         return;
                     }
                     if (dataModel.LawyerLicenseNo == '') {
                         Alert("职业证号不能为空!");
                         return;
                     }
                     if (dataModel.LawyerLicensePhoto == '') {
                         Alert("执业证照片不能为空!");
                         return;
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status == 1) {
                                 $('#dlgInfo').dialog('close');
                                 $('#grvData').datagrid('reload');
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     });

                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgInfo').dialog('close');
                 }
             }]
         });
         bindEvent();
     });
     function bindEvent() {
         $document.on('change', '#txtThumbnailsPath', function () {
             try {
                 $.messager.progress({
                     text: '正在上传图片...'
                 });
                 $.ajaxFileUpload({
                     url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                     secureuri: false,
                     fileElementId: 'txtThumbnailsPath',
                     dataType: 'json',
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {
                             imgThumbnailsPath.src = resp.ExStr;

                         } else {
                             alert(resp.Msg);
                         }
                     }
                 });

             } catch (e) {
                 alert(e);
             }
         });
     }
     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].userId);
         }
         return ids;
     }

     //删除
     function ActionEvent(action,msg) {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", msg, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: action, ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });
     }
     function EditRow() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckNoSelectMultiRow(rows)) {
             return;
         }
         $('#txtUserID').val(rows[0].userId);
         $('#txtName').val(rows[0].name);
         $('#txtPostion').val(rows[0].postion);
         $('#txtCompany').val(rows[0].company);
         $('#txtCompanyAddress').val(rows[0].companyAddress);
         $('#txtLawyerLicenseNo').val(rows[0].lawyerLicenseNo);
         $('#imgThumbnailsPath').attr("src", rows[0].lawyerLicensePhoto);

         txtUserID.readOnly = true;
         $('#dlgInfo').dialog('open');
     }
     function Search() {
         $('#grvData').datagrid({
	        method: "Post",
	        url: handlerUrl,
	        queryParams: { Action: "getUserList", type: 3, keyword: $("#txtKeyword").val() }
	    });
     }
    </script>
</asp:Content>
