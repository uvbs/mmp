<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="FieldList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Order.FieldList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">

        #dlgInput table td:first-child {
            text-align:right;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;专家预约&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>预约字段</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 450px;
        height: 300px;padding:10px 10px 10px 10px;">
        <table width="100%">
            <tr>
                <td>
                    选择字段:
                </td>
                <td>
                    <select id="ddlField">
                        <option value="Ex7">扩展字段1</option>
                        <option value="Ex8">扩展字段2</option>
                        <option value="Ex9">扩展字段3</option>
                        <option value="Ex10">扩展字段4</option>
                        <option value="Ex11">扩展字段5</option>
                        <option value="Ex12">扩展字段6</option>
                        <option value="Ex13">扩展字段7</option>
                        <option value="Ex14">扩展字段8</option>
                        <option value="Ex15">扩展字段9</option>
                        <option value="Ex16">扩展字段10</option>
                        <option value="Ex17">扩展字段11</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="txtFieldShowName" type="text" style="width: 250px;" placeholder="请输入名称"/>
                </td>
            </tr>
            <tr>
                <td>
                    是否必填:
                </td>
                <td>
                    <select id="ddlIsNull">
                        <option value="1">可以不填</option>
                        <option value="0">必填</option>
                    </select>
                </td>
            </tr>
              <tr>
                <td>
                    排序号(数字越大越靠前显示):
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 100px;"  onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" placeholder="请输入数字" maxlength="2"/>
                </td>
            </tr>

              <tr>
                <td align="right">
                    输入类型:
                </td>
                <td style="text-align: left">
                    <select id="ddlInputType" style="width: 200px;">
                        <option value="text">文本框</option>
                        <option value="combox">下拉框</option>
                        <option value="checkbox">多选框</option>
                    </select>
                </td>
            </tr>
                <tr id="trismu">
                <td align="right">
                    单行/多行 文本:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsMultiline" name="rdoIsMultiline" />
                    <label for='rdoIsMultiline'>
                        多行</label>
                    <input type="radio" id="rdoIsNotMultiline" name="rdoIsMultiline" checked="checked"/>
                    <label for='rdoIsNotMultiline'>
                        单行</label>
                </td>
            </tr>
            <tr id="troptions" style="display: none;">
                <td align="right">
                    选项列表,多个选项用逗号隔开
                </td>
                <td style="text-align: left">
                    <textarea id="txtOptions" rows="3"></textarea>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "../Handler/Handler.ashx";
        var currSelectID = 0;
        var currAction = '';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl + "?Action=FieldList",
	                height: document.documentElement.clientHeight - 150,
	                pagination: false,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Field', title: '扩展字段', width: 10, align: 'left', formatter: function (value) {
                                  switch (value) {
                                      case "Ex7":
                                      return "扩展字段1";
                                      case "Ex8":
                                          return "扩展字段2";
                                      case "Ex9":
                                          return "扩展字段3";
                                      case "Ex10":
                                          return "扩展字段4";
                                      case "Ex11":
                                          return "扩展字段5";
                                      case "Ex12":
                                          return "扩展字段6";
                                      case "Ex13":
                                          return "扩展字段7";
                                      case "Ex14":
                                          return "扩展字段8";
                                      case "Ex15":
                                          return "扩展字段9";
                                      case "Ex16":
                                          return "扩展字段10";
                                      case "Ex17":
                                          return "扩展字段11";
                                                                        }

                                                                    }
                                                                },
                                { field: 'MappingName', title: '名称', width: 20, align: 'left' },
                                                                {
                                                                    field: 'FieldIsNull', title: '是否必填', width: 10, align: 'left', formatter: function (value) {
                                                                        if (value == 1) {
                                                                            return "可以不填";
                                                                        }
                                                                        else if (value == 0) {
                                                                            return "必填";


                                                                        }
                                                                    }
                                                                },
                                { field: 'FieldType', title: '输入类型', width: 10, align: 'left', formatter: function (value) {

                                       switch (value) {
                                           case "text":
                                               return "文本框";
                                           case "combox":
                                               return "下拉框";
                                           case "checkbox":
                                               return "多选框";
                                           default:
                                               return "文本框";

                                       }

                                 }
                                },
                                 { field: 'Options', title: '选项', width: 80, align: 'left' },
                                

                                { field: 'Sort', title: '排序号', width: 10, align: 'left' }


	                ]]
	            }
            );



            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                AutoID: currSelectID,
                                Field: $.trim($('#ddlField').val()),
                                FieldShowName: $.trim($('#txtFieldShowName').val()),
                                IsNull: $(ddlIsNull).val(),
                                Sort: $(txtSort).val(),
                                FieldType: $(ddlInputType).val(),
                                Options: $(txtOptions).val().replace("，",","),
                                IsMultiline: rdoIsMultiline.checked ? 1 : 0
                            }
                            if (dataModel.FieldShowName == "") {
                                Alert("名称必填");
                                return false;
                            }
                            if (dataModel.Sort=="") {
                                dataModel.Sort = 0;
                            }
                            if (dataModel.Action == "FieldAdd") {
                                var rows = $("#grvData").datagrid("getRows");
                                for (var i = 0; i < rows.length; i++) {
                                    if (rows[i].Field==dataModel.Field) {
                                        Alert("此扩展已经添加,请选择别的扩展字段");
                                        return false;
                                    }


                                }

                            }



                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        Alert("操作失败");
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

            $("#ddlInputType").change(function () {
                if ($(this).val() == 'text') {

                    $(troptions).hide();
                    $(txtOptions).val("");
                    $(trismu).show();

                }
                else {
                    $(troptions).show();
                    $(trismu).hide();
                }


            });



        });

        function ShowAdd() {
            currAction = 'FieldAdd';
            $(txtSort).val(0);
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");


        }

        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;



                $.messager.confirm("系统提示", "确认删除?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }

                        var dataModel = {
                            autoIds: ids.join(','),
                            Action:"FieldDelete"
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status == true) {
                                    Show("删除成功");
                                    $('#grvData').datagrid('reload');
                                } else {
                                    Show("删除失败");
                                }


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


            currAction = 'FieldUpdate';
            currSelectID = rows[0].AutoId;
            $(ddlField).val(rows[0].Field);
            $(txtFieldShowName).val(rows[0].MappingName);
            $(ddlIsNull).val(rows[0].FieldIsNull);
            $(txtSort).val(rows[0].Sort);
            $("#ddlInputType").val(rows[0].FieldType);
            $("#txtOptions").val(rows[0].Options);
            if (rows[0].FieldType == "text") {
                $(troptions).hide();
                $(trismu).show();
                
            }
            else {
                $(troptions).show();
                $(trismu).hide();
            }
            var isMultiline = rows[0].IsMultiline;
            //alert(isMultiline);
            if (isMultiline == "1") {
                //$("#rdoIsMultiline").attr("checked",true);  
                rdoIsMultiline.checked = true;
            }
            else if (isMultiline == "0") {
                rdoIsNotMultiline.checked = true;
                //$("#rdoIsNotMultiline").attr("checked",true);  
            }
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>