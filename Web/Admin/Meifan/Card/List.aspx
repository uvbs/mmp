<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Card.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        #dlgInput input {
            width:100%;
            height:25px;
        }
        #txtAmount, #txtServerAmount, #txtValidMonth{
            width:100px;
            font-size:14px;
            color:red;
           
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;会员卡&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>会员卡管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

             <%if (PmsAdd){%>
           <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                 id="btnAdd">添加</a>
            <% } %>
            
             <%if (PmsUpdate){%>
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <% } %>
            <%if (PmsDelete){%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                >删除</a>
            <% } %>

            <%if (PmsEnable){%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateEnable(0)">启用</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateEnable(1)">禁用</a>
            <% } %>
            <div>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 600px; padding: 15px; line-height: 30px;height:500px;">
        <table width="100%">

            <tr>
                <td>类型:
                </td>
                <td>
                    <select id="ddlType">
                        <option value="personal">个人卡</option>
                        <option value="family">家庭卡</option>
                        <option value="chuandong">船东卡</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td style="width:80px;">名称:
                </td>
                <td>
                    <input id="txtCardName" type="text" placeholder="请输入名称" />
                </td>
            </tr>
            <tr>
                <td>名称(英文):
                </td>
                <td>
                    <input id="txtCardNameEn" type="text" placeholder="请输入名称(英文)"/>
                </td>
            </tr>

            <tr>
                <td>费用(元):
                </td>
                <td>
                    <input id="txtAmount" type="number" placeholder="请输入金额" />
                </td>
            </tr>
            <tr>
                <td>服务费(元):
                </td>
                <td>
                    <input id="txtServerAmount" type="number" placeholder="请输入服务费"/>
                </td>
            </tr>
            <tr>
                <td>有效期(月):
                </td>
                <td>
                    <input id="txtValidMonth" type="number" placeholder="请输入有效期"/>
                </td>
            </tr>
            <tr>
                <td>图片:
                </td>
                <td>
                    <img alt="图片" id="imgThumbnailsPath" style="max-width: 100px;" />
                    <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                        onclick="txtThumbnailsPath.click()">上传图片</a>
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display: none;" />
                </td>
            </tr>
            <tr>
                <td>会员权益:
                </td>
                <td>
                    <textarea id="txtDescription" ></textarea>

                </td>
            </tr>
        </table>
    </div>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/meifan/card/";
        var currSelectID = 0;
        var currAction = '';

        var editor;
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl+"list.ashx",
                       queryParams: {  },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect:true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   {
                                       field: 'card_img', title: '图片', width: 10, align: 'center', formatter: function (value) {
                                           if (value == '' || value == null)
                                               return "";
                                           var str = new StringBuilder();
                                           str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'card_type', title: '类型', width: 10, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           switch (value) {
                                               case "personal":
                                                   str.AppendFormat('个人卡');
                                                   break;
                                               case "family":
                                                   str.AppendFormat('家庭卡');
                                                   break;
                                               case "chuandong":
                                                   str.AppendFormat('船东卡');
                                                   break;
                                               default:

                                           }
                                          
                                           return str.ToString();
                                       }
                                   },
                                   { field: 'card_name', title: '名称', width: 20, align: 'left' },
                                   { field: 'card_name_en', title: '名称(英文)', width: 20, align: 'left' },
                                   { field: 'amount', title: '金额(元)', width: 10, align: 'left' },
                                   { field: 'server_amount', title: '服务费(元)', width: 10, align: 'left' },
                                   { field: 'valid_month', title: '有效期(月)', width: 10, align: 'left' },
                                   { field: 'is_enable', title: '状态', width: 10, align: 'left', formatter: function (value, rowData) {

                                                                               var str = new StringBuilder();
                                                                               switch (value) {
                                                                                   case 0:
                                                                                       str.AppendFormat('<font color="green">启用</font>');
                                                                                       break;
                                                                                   case 1:
                                                                                       str.AppendFormat('<font color="red">禁用</font>');
                                                                                       break;
                                                                                   
                                                                                       break;
                                                                                   default:

                                                                               }

                                                                               return str.ToString();
                                                                           }
                                                                       }


                       ]]
                   }
            );



            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                               
                                card_id: currSelectID,
                                card_name: $.trim($('#txtCardName').val()),
                                card_name_en: $.trim($('#txtCardNameEn').val()),
                                card_type: $('#ddlType').val(),
                                card_img: $("#imgThumbnailsPath").attr("src"),
                                amount: $(txtAmount).val(),
                                server_amount: $(txtServerAmount).val(),
                                valid_month: $(txtValidMonth).val(),
                                description:editor.html()

                            }

                           

                            if (dataModel.card_name == '') {

                                $('#txtCardName').focus();
                                return;
                            }
                            if (dataModel.amount == '') {

                                $('#txtAmount').focus();
                                return;
                            }
                            if (dataModel.server_amount == '') {

                                $('#txtServerAmount').focus();
                                return;
                            }
                            var addUpdatePath = handlerUrl;
                            if (currAction=="add") {
                                addUpdatePath +="add.ashx";
                            }
                            else {
                                addUpdatePath +="update.ashx";
                            }

                            

                            $.ajax({
                                type: 'post',
                                url: addUpdatePath,
                                data: { data: JSON.stringify(dataModel) },
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status ==true) {
                                        Show("操作成功");
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');
                                       
                                    }
                                    else {
                                        Alert(resp.msg);
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });


            


          

          $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                        {
                            url: '/serv/api/common/file.ashx?action=add',
                            secureuri: false,
                            fileElementId: 'txtThumbnailsPath',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.errcode ==0) {

                                    $('#imgThumbnailsPath').attr('src', resp.file_url_list[0]);
                                }
                                else {
                                    Alert(resp.errmsg);
                                }
                            }
                        }
                       );

                } catch (e) {
                    alert(e);
                }
            });

        });

        function ShowAdd() {

            currAction = 'add';
            $("#txtCardName").val("");
            $('#txtCardNameEn').val("");
            $("#imgThumbnailsPath").attr("src", "");
            $('#ddlType').val("");
            $('#txtAmount').val("");
            $('#txtServerAmount').val("");
            $('#txtValidMonth').val("");
            editor.html("");

            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");
           


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
                            ids.push(rows[i].card_id);
                        }

                        var dataModel = {
                            
                            card_ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl+"delete.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status==true) {
                                    Alert("删除成功");
                                } else {
                                    Alert("删除失败");
                                }
                               
                                $('#grvData').datagrid('reload');
                               
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }


        function UpdateEnable(isEnable) {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                var msg = "禁用";
                if (isEnable==0) {
                    msg = "启用";
                }
                $.messager.confirm("系统提示", "确认"+msg+"选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].card_id);
                        }

                        var dataModel = {
                            card_ids: ids.join(','),
                            is_enable:isEnable
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "updateenable.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status == true) {
                                    Alert("操作成功");
                                } else {
                                    Alert("操作失败");
                                }

                                $('#grvData').datagrid('reload');

                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }


        function ShowEdit() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;


            currAction = 'update';
            currSelectID = rows[0].card_id;
            $("#txtCardName").val($.trim(rows[0].card_name));
            $('#txtCardNameEn').val(rows[0].card_name_en);
            $("#imgThumbnailsPath").attr("src", rows[0].card_img);
            $('#ddlType').val(rows[0].card_type);
            $('#txtAmount').val(rows[0].amount);
            $('#txtServerAmount').val(rows[0].server_amount);
            $('#txtValidMonth').val(rows[0].valid_month);
            editor.html(rows[0].description);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtDescription', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
    'insertunorderedlist', '|', 'importword', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                filterMode: false,
                width: "100%",
                height: "300px",
            });
        });

    </script>
</asp:Content>
