<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SupplierList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.SupplierList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        #txtDesc {
            height:50px;
            width:90%;
        }
        .selectAddress{
            width:103px;
            height:25px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：<span>商户管理</span>

   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
            onclick="ShowAdd();">新增商户</a>

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
            onclick="ShowEdit();">编辑</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
        <br />
        <div style="margin-bottom: 5px">
            名称:
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px;"
                placeholder="关键字搜索" />

            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

    <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="增加" style="width: 500px; padding: 15px;height:400px;">

        <table width="100%">
                <tr>
                <td>角色:
                </td>
                <td>
                    <span id="sp_menu"></span>
                </td>
            </tr>
           <tr class="add">
                <td>账号:
                </td>
                <td>
                    <input id="txtUserId" type="text" style="width: 90%;" />
                </td>
            </tr>
           <tr class="add">
                <td>密码:
                </td>
                <td>
                    <input id="txtPassword" type="password"  style="width: 90%;" />
                </td>
            </tr>
             <tr class="add">
                <td>确认密码:
                </td>
                <td>
                    <input id="txtPasswordConfirm" type="password" style="width: 90%;" />
                </td>
            </tr>
                <tr>
                <td>商户名称:
                </td>
                <td>
                    <input id="txtCompanyName" type="text" style="width: 90%;" />
                </td>
            </tr>
               
                 <tr>
                <td>开户行:
                </td>
                <td>
                    <input id="txtBackDeposit" type="text" style="width: 90%;" />
                </td>
            </tr>
                 <tr>
                <td>银行账号:
                </td>
                <td>
                    <input id="txtBackAccount" type="text" style="width: 90%;" />
                </td>
            </tr>
                  <tr>
                <td>商户代码:
                </td>
                <td>
                    <input id="txtEx2" type="text" style="width: 90%;" />
                </td>
            </tr>
            
            <tr>
                <td>联系人:
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 90%;" />
                </td>
            </tr>

            <tr>
                <td>联系手机:
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 90%;" />
                </td>
            </tr>
               <tr>
                <td>Email:
                </td>
                <td>
                    <input id="txtEmail" type="text" style="width: 90%;" />
                </td>
            </tr>
                <tr>
                <td>链接:
                </td>
                <td>
                    <input id="txtEx1" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>
                    所在地区
                </td>
                <td>
                    <select class="selectAddress" id="selectProvince">
                    </select>
                    <select class="selectAddress" id="selectCity">
                    </select>
                    <select class="selectAddress" id="selectArea">
                    </select>
                </td>
            </tr>
              <tr>
                <td>地址:
                </td>
                <td>
                    <input id="txtAddress" type="text" style="width: 90%;" />
                </td>
            </tr>
              <tr>
                <td>说明:
                </td>
                <td>
                   <textarea id="txtDesc"></textarea>
                </td>
            </tr>

            <tr>
                <td>Logo
                </td>
                <td>
                    <img alt="" src="" width="80px" height="80px" id="imglogo" /><br />
                    <a id="auploadThumbnails"
                        href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                        onclick="filelogo.click()">上传</a>
                    <input type="file" id="filelogo" name="file1" style="display: none;" />

                </td>
            </tr>
                <tr>
                <td>大图
                </td>
                <td>
                    <img alt="" src="" width="80px" height="80px" id="img1" /><br />
                    <a 
                        href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                        onclick="file1.click()">上传</a>
                    <input type="file" id="file1" name="file2" style="display: none;" />

                </td>
            </tr>
               <tr>
                <td style="width:80px;">底部说明:
                </td>
                <td>
                   <textarea id="txtEx3"></textarea>
                </td>
            </tr>
            <tr>
                <td style="width:80px;">提醒:
                </td>
                <td>
                   <textarea id="txtEx4"></textarea>
                </td>
            </tr>
        </table>

    </div>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/User/Supplier/";
        var handlerPermissionUrl = '/serv/api/admin/permissioncolumn/';
        var currentAction = "Add.ashx";
        var selectAutoId = "0";
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#txtEx3', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false,
                width: "100%",
                height: "200px",
            });
        });
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl+"List.ashx",
	                queryParams: { keyWord:$("#txtKeyWord").val() },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'id', title: '编号', width: 10, align: 'left' },
                                { field: 'ex2', title: '商户代码', width: 20, align: 'left' },
                                { field: 'user_id', title: '账号', width: 25, align: 'left' },
                                { field: 'permission_group_name', title: '角色', width: 10, align: 'left' },
                                { field: 'company_name', title: '名称', width: 25, align: 'left' },
                                { field: 'true_name', title: '联系人', width: 15, align: 'left' },
                                { field: 'phone', title: '联系电话', width: 15, align: 'left' },
                                { field: 'email', title: 'Eamil', width: 20, align: 'left' },
                                { field: 'back_deposit', title: '开户行', width: 20, align: 'left' },
                                { field: 'desc', title: '说明', width: 20, align: 'left' },
                                { field: 'op', title: '门店', width: 20, align: 'left',formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a class="listClickNum" target="_blank"  href="/Admin/Outlets/Edit.aspx?id=0&supplier_id={0}" title="管理门店">管理门店</a>', rowData.id);
                                    return str.ToString();
                                } },

	                ]]
	            });


            //添加编辑
            $('#dlgUserInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        dataModel = {
                            id: selectAutoId,
                            userId: $("#txtUserId").val(),
                            passWord: $("#txtPassword").val(),
                            passWordConfirm: $("#txtPasswordConfirm").val(),
                            trueName: $("#txtTrueName").val(),
                            companyName: $("#txtCompanyName").val(),
                            phone: $("#txtPhone").val(),
                            email: $("#txtEmail").val(),
                            description: $("#txtDesc").val(),
                            permissionGroupId: $.trim($("#ddlPermissionGroup").val()),
                            headImage: $("#imglogo").attr("src"),
                            image: $("#img1").attr("src"),
                            ex1:$("#txtEx1").val(),
                            ex2:$("#txtEx2").val(),
                            ex3: editor.html(),
                            ex4: $("#txtEx4").val(),
                            address: $('#txtAddress').val(),
                            backDeposit: $('#txtBackDeposit').val(),
                            backAccount: $('#txtBackAccount').val(),
                            province: $("#selectProvince").find("option:selected").text(),
                            province_code: $('#selectProvince').val(),
                            city: $("#selectCity").find("option:selected").text(),
                            city_code: $('#selectCity').val(),
                            district: $("#selectArea").find("option:selected").text(),
                            district_code:$('#selectArea').val()
                        }
                        if (dataModel.companyName=="") {
                            alert("名称必填");
                            return false;
                        }

                        if (dataModel.province_code) {
                            if (!dataModel.city_code || !dataModel.district_code) {
                                alert('请填写完整地区');
                                return false;
                            }
                        } else {
                            dataModel.province = "";
                            dataModel.province_code = "";
                            dataModel.city = "";
                            dataModel.city_code = "";
                            dataModel.district = "";
                            dataModel.district_code = "";
                        }


                       
                       //if ($("#ddlPermissionGroup").find("option:selected").text().indexOf('商户')==-1) {
                       //    alert("请先到 系统管理-角色管理-添加角色 商户");
                       //     return false;
                       // }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl+currentAction,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status == true) {
                                    Alert("操作成功");
                                    $('#dlgUserInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    
                                }
                                else {
                                    Alert(resp.msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUserInfo').dialog('close');
                    }
                }]
            });

           

           
            LoadSelect();


            //load
            $("#filelogo").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                        {
                            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                            secureuri: false,
                            fileElementId: 'filelogo',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    $("#imglogo").attr("src", resp.ExStr);

                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        }
                       );

                } catch (e) {
                    alert(e);
                }
            });

            $("#file1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                        {
                            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                            secureuri: false,
                            fileElementId: 'file1',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    $("#img1").attr("src", resp.ExStr);

                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        }
                       );

                } catch (e) {
                    alert(e);
                }
            });


            areaBind();


        });


        function areaBind() {
            //初始化处理省份选项
            setProvinceSelect();
            
            $(document).on('change', '#selectProvince', function () {
                //选择省份
                setCitySelect($(this).val());
            });

            $(document).on('change', '#selectCity', function () {
                //选择城市
                var selectCode = $(this).val();
                setAreaSelect(selectCode);
            });
        }
        
        function setProvinceSelect(selectVal) {
            var data = zymmp.location.getProvince();

            $(document).find('#selectProvince').html('').append(getAreaSelectOptionDom(data,'选择省份'));
            if (selectVal) {
                $(document).find('#selectProvince').val(selectVal);
            }
        }
        function setCitySelect(provinceCode, selectVal) {

            try {
                var selectCode = provinceCode, cityData = zymmp.location.getCity(selectCode);
                $(document).find('#selectCity').html('').append(getAreaSelectOptionDom(cityData,'选择城市'));
                $(document).find('#selectArea').html('<option value=""></option>');
                if (selectVal) {
                    $(document).find('#selectCity').val(selectVal);
                }
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }
        function setAreaSelect(cityCode, selectVal) {

            try {
                var selectCode = cityCode, areaData = zymmp.location.getDistrict(selectCode);
                $(document).find('#selectArea').html('').append(getAreaSelectOptionDom(areaData,'选择地区'));
                if (selectVal) {
                    $(document).find('#selectArea').val(selectVal);
                }
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }
        function getAreaSelectOptionDom(data,showText) {
            var strHtml = new StringBuilder();
            if (data.length > 0) strHtml.Append('<option value="">' + showText + '</option>');
            for (var i = 0; i < data.length; i++) {
                strHtml.AppendFormat('<option value="{0}">{1}</option>', data[i].DataKey, data[i].DataValue);
            }
            return strHtml.ToString();
        }
        function LoadSelect() {
            $.post(handlerPermissionUrl + "selectrolelist.ashx", {}, function (data) {
                if (data.status && data.result) {
                    $("#sp_menu").html(data.result);
                    $("#ddlPermissionGroup option").each(function () {
                       
                        if ($(this).text().indexOf('商户') == -1) {
                            $(this).remove();
                        }

                    })

                }
            });
        }
        //搜索
        function Search() {

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl+"List.ashx",
                        queryParams: { keyword: $("#txtKeyWord").val()}
                    });
                }

        //添加
        function ShowAdd() {
                    $("#dlgUserInfo input[type='text']").val("");
                    $("#ddlPermissionGroup").val("0");
                    $("#dlgUserInfo img").attr("src","");
                    currentAction = "Add.ashx";
                    $(".add").show();
                    $('#selectArea').html("");
                    $('#selectCity').html("");
                    $('#selectProvince').val("");
                    $('#dlgUserInfo').dialog({ title: '新增商户' });
                    $('#dlgUserInfo').dialog('open');

        }

         //编辑
        function ShowEdit() {

                    var rows = $('#grvData').datagrid('getSelections');

                    if (!EGCheckIsSelect(rows))
                        return;

                    if (!EGCheckNoSelectMultiRow(rows))
                        return;

                    $("#txtTrueName").val(rows[0].true_name);
                    $("#txtCompanyName").val(rows[0].company_name);
                    $("#txtDesc").val(rows[0].desc);
                    $("#txtPhone").val(rows[0].phone);
                    $("#txtEmail").val(rows[0].email);
                    $("#txtEx1").val(rows[0].ex1);
                    $("#txtEx2").val(rows[0].ex2);
                    $("#txtEx4").val(rows[0].ex4);
                    $("#imglogo").attr("src", rows[0].head_img_url);
                    $("#img1").attr("src", rows[0].image);
                    $(".add").hide();
                    $("#ddlPermissionGroup").val(rows[0].permission_group_id);
                    $('#txtAddress').val(rows[0].address);
                    $('#txtBackDeposit').val(rows[0].back_deposit);
                    $('#txtBackAccount').val(rows[0].back_account);
                    
                    if (rows[0].province_code) setProvinceSelect(rows[0].province_code);
                    if (rows[0].city_code) setCitySelect(rows[0].province_code, rows[0].city_code);
                    if (rows[0].district_code) setAreaSelect(rows[0].city_code,rows[0].district_code);

                    editor.html(rows[0].ex3);
                    currentAction = "Update.ashx";
                    selectAutoId = rows[0].id;
                
                    $('#dlgUserInfo').dialog({ title: '编辑' });
                    $('#dlgUserInfo').dialog('open');

        }

        function Delete() {
                    try {

                        var rows = $('#grvData').datagrid('getSelections');

                        if (!EGCheckIsSelect(rows))
                            return;

                        $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                            if (r) {
                                var ids = [];

                                for (var i = 0; i < rows.length; i++) {
                                    ids.push(rows[i].id);
                                }

                                var dataModel = {
                                   
                                    ids: ids.join(',')
                                }

                                $.ajax({
                                    type: 'post',
                                    url: handlerUrl+"Delete.ashx",
                                    data: dataModel,
                                    success: function (resp) {
                                        if (resp.status) {
                                            Alert("删除成功");
                                            $('#grvData').datagrid('reload');
                                        }
                                        else {
                                            Alert("删除失败");
                                        }
                                        
                                       
                                    }
                                });
                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }
        }

    </script>
</asp:Content>
