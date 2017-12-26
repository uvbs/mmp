<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivitySignUpTableManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ActivitySignUpTableManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #winInputInfo tr
        {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="ActivityManage.aspx">活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%Response.Write(activityInfo.ActivityName); %>的报名字段设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd()">添加字段</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit()">编辑字段</a> <a href="javascript:void(0)"
                        class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                        删除字段</a> <a href="javascript:void(0)" class="easyui-linkbutton" id="btnDistinctKeys"
                            iconcls="icon-edit" plain="true">设置不能重复的字段</a> <a href="javascript:void(0)" title="上移"
                                class="easyui-linkbutton" plain="true" onclick="SetFieldSort(0)">
                                <img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a>
            <a href="javascript:void(0)" title="下移" class="easyui-linkbutton" plain="true" onclick="SetFieldSort(1)">
                <img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a> 
            
            <a style="float: right;"
                    href="javascript:history.go(-1);" class="easyui-linkbutton" iconcls="icon-back"
                    plain="true">返回</a>
        </div>
    </div>
    <table id="dataGrid" cellspacing="0" cellpadding="0" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true">
                </th>
                <th field="FieldName" width="20">
                    Field
                </th>
                <th field="MappingName" width="20">
                    名称
                </th>
                <th field="FormatValiFunc" formatter="formartvalitype" width="20">
                    验证格式
                </th>
                <th field="FieldIsNull" formatter="changeisnull" width="10">
                    必填项
                </th>
                <th field="FieldIsDefauld" formatter="changeisdefault" width="10">
                    默认字段
                </th>
                <th field="FieldType" formatter="formartfieldtype" width="10">
                    格式类型
                </th>
                <th field="IsHideInSubmitPage" formatter="formartshow" width="10">
                    在活动提交页面显示
                </th>
                <th field="InputType" formatter="formartinputtype" width="10">
                    输入类型
                </th>
                <th field="Options" width="10">
                    选项
                </th>
            </tr>
        </thead>
    </table>
    <div id="winInputInfo" class="easyui-dialog" closed="true" title="报名表" modal="true"
        style="width: 400px; padding: 10px;">
        <table style="margin: auto;">
            <tr>
                <td align="right">
                    Field:
                </td>
                <td style="text-align: left">
                    <label id="lbFieldName">
                    </label>
                    <input type="hidden" id="txtExFieldIndex" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    名称:
                </td>
                <td style="text-align: left">
                    <input type="text" id="txtMappingName" class="easyui-validatebox" required="true"
                        missingmessage="请输入名称" invalidmessage="请输入名称" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    是否必填:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsNotNull" name="rd" checked="checked" />
                    <label for='rdoIsNotNull'>
                        是</label>
                    <input type="radio" id="rdoIsNull" name="rd" />
                    <label for='rdoIsNull'>
                        否</label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    是否多行:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsMultiline" name="rdoIsMultiline" />
                    <label for='rdoIsMultiline'>
                        是</label>
                    <input type="radio" id="rdoIsNotMultiline" name="rdoIsMultiline" />
                    <label for='rdoIsNotMultiline'>
                        否</label>
                </td>
            </tr>
            <tr style="display: none;">
                <td align="right">
                    类型:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdFieldtype0" name="rdFieldtype" checked="checked" />
                    <label for='rdFieldtype0'>
                        普通字段</label>
                    <input type="radio" id="rdFieldtype1" name="rdFieldtype" />
                    <label for='rdFieldtype1'>
                        微信推广字段</label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    格式验证:
                </td>
                <td style="text-align: left">
                    <select id="ddlFormatValiFunc" style="width: 200px;">
                        <option value="none">无</option>
                        <option value="email">电子邮箱</option>
                        <option value="url">网址</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">
                    是否在提交活动信息页面显示:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoShowInSubmitPage" name="rdoIsShowInSubmitPage" />
                    <label for='rdoShowInSubmitPage'>
                        显示</label>
                    <input type="radio" id="rdoHideInSubmitPage" name="rdoIsShowInSubmitPage" />
                    <label for='rdoHideInSubmitPage'>
                        不显示</label>
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
    <div id="dlgDistinctKeys" class="easyui-dialog" closed="true" title="重复设置" modal="true"
        style="width: 350px; padding: 10px;">
        <div>
            <input type="radio" id="rdodefaultdistinctkey" value="" name="rdodistinctkey" checked="checked" /><label
                for="rdodefaultdistinctkey">默认(一个活动中手机号码不能重复)</label></div>
        <div>
            <input type="radio" id="rdononedistinctkey" value="" name="rdodistinctkey" /><label
                for="rdononedistinctkey">关闭重复检查</label></div>
        <div>
            <input type="radio" id="rdocustomdistinctkey" value="" name="rdodistinctkey" /><label
                for="rdocustomdistinctkey">自定义</label></div>
        <ul id="uldistinctkeys" style="list-style-type: none;">
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <%string acid = this.ViewState["ActivityID"].ToString(); %>
    <script language="javascript" type="text/javascript">
        var currentAction = ""; //当前操作add edit
        var dataGrid; //扩展数据
        //处理文件路径
        var handlerUrl = "/Handler/Activity/ActivityTable.ashx";
        var options = ""; //选项集合,多个选项用逗号分隔
        //加载
        $(function () {
            $("#rdFieldtype1").click(function () {//微信推广
                $("#txtMappingName").val("推广人");

            });

            $("#rdFieldtype0").click(function () {
                if ($("#txtMappingName").val() == "推广人") {
                    $("#txtMappingName").val("");
                }


            });



            $("#ddlInputType").change(function () {
                if ($(this).val() == 'text') {

                    $(troptions).hide();


                }
                else {
                    $(troptions).show();
                }


            });
            //$(window).resize(function () {
            //    var width = document.body.clientWidth - 15;
            //    var height = document.documentElement.clientHeight - 170;
            //    $('#dataGrid').datagrid('resize', {
            //        width: width,
            //        height: height
            //    });
            //});
            //grid加载默认数据
            dataGrid = $("#dataGrid").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight - 112,
                fitCloumns: true,
                nowrap: true,
                pagination: false,
                rownumbers: true,
                singleSelect: false,
                view: fileview,
                idField: "FieldName",
                queryParams: { Action: "Query", ActivityID: '<%=acid %>' }
            });

            //            //编辑信息框取消按钮绑定
            //            $("#btnWinClose").bind("click", function () {
            //                $("#winInputInfo").window("close");
            //            });

            //            //编辑信息框保存按钮绑定
            //            $("#btnWinSave").bind("click", function () {
            //                var tag = $.trim($(this).attr("tag"));
            //                switch (tag) {
            //                    case "add": //添加
            //                        Add();
            //                        break;
            //                    case "edit": //编辑
            //                        Edit();
            //                        break;
            //                    default:
            //                        break;
            //                }
            //            });
            $("#rdodefaultdistinctkey").click(function () {
                //rdocustomdistinctkey.checked=false;
                $("#uldistinctkeys").hide();
                $('input[name="cbdistinkeys"]').each(function () {
                    this.checked = false;


                });
                //            
            })
            $("#rdononedistinctkey").click(function () {
                $("#uldistinctkeys").hide();
                // rdocustomdistinctkey.checked=false;
                $('input[name="cbdistinkeys"]').each(function () {
                    this.checked = false;
                })
            });
            $("#rdocustomdistinctkey").click(function () {
                $("#uldistinctkeys").show();
            });


            $("#btnDistinctKeys").click(function () {

                var sbdistinictlist = new StringBuilder();
                var rows = dataGrid.datagrid("getRows");
                var rowlength = rows.length;
                for (var i = 0; i < rows.length; i++) {

                    if (rows[i].MappingName == "姓名") {
                        sbdistinictlist.AppendFormat('<li><input type=\"checkbox\" value=\"Name\" name=\"cbdistinkeys\" id=\"lblName\"><label for=\"lblName\">姓名</label></li>');
                        sbdistinictlist.AppendFormat('<hr/>');
                    }
                    else if (rows[i].MappingName == "手机") {
                        sbdistinictlist.AppendFormat('<li><input type=\"checkbox\" value=\"Phone\" name=\"cbdistinkeys\" id=\"lblPhone\"><label for=\"lblPhone\">手机</label></li>');
                        sbdistinictlist.AppendFormat('<hr/>');

                    }
                    else {
                        sbdistinictlist.AppendFormat('<li><input type=\"checkbox\" value=\"K{0}\" name=\"cbdistinkeys\" id=\"lbl{0}\"><label for=\"lbl{0}\">{1}</label></li>', rows[i].ExFieldIndex, rows[i].MappingName);
                        sbdistinictlist.AppendFormat('<hr/>');
                    }

                }

                $("#uldistinctkeys").html(sbdistinictlist.ToString());
                jQuery.ajax({//获取自定义不重复字段
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "GetDistinctKeys", ActivityID: '<%=acid%>' },
                    dataType: "html",
                    success: function (result) {
                        if (result == "") {//默认
                            rdodefaultdistinctkey.checked = true;
                            $("#uldistinctkeys").hide();
                        }
                        else if (result == "none") {//关闭重复性检查
                            rdononedistinctkey.checked = true;
                            $("#uldistinctkeys").hide();
                        }
                        else {//自定义重复性
                            rdocustomdistinctkey.checked = true;
                            SetCheckGroupVal("cbdistinkeys", result, "value");
                            $("#uldistinctkeys").show();
                        }


                    }
                })

                $("#dlgDistinctKeys").dialog("open");

            });


            //new 
            $("#winInputInfo").dialog({
                buttons: [
                           {
                               text: '确定',
                               handler: function () {
                                   if (currentAction == "add") {
                                       Add();
                                   }
                                   else if (currentAction == "edit") {
                                       Edit();
                                   }

                               }
                           },
                           {
                               text: '取消',
                               handler: function () {

                                   $("#winInputInfo").dialog('close');
                               }
                           }
                           ]
            });

            $("#dlgDistinctKeys").dialog({
                buttons: [
                           {
                               text: '确定',
                               handler: function () {
                                   SetDistinctKeys();
                               }
                           },
                           {
                               text: '取消',
                               handler: function () {

                                   $("#dlgDistinctKeys").dialog('close');
                               }
                           }
                           ]
            });

        });
        //设置不允许重复的字段
        function SetDistinctKeys() {

            var distinkey = GetCheckGroupVal("cbdistinkeys", "value");
            if (distinkey != "") {//自定义不允许重复的字段
                distinkey = distinkey + ",ActivityID";

            }
            else {
                if (rdodefaultdistinctkey.checked) {//默认
                    distinkey = "";
                }
                else if (rdononedistinctkey.checked) {//关闭重复检查
                    distinkey = "none";
                }
            }

            $.messager.progress({ text: '正在处理。。。' });
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "EditDistinctKeys", ActivityID: '<%=acid%>', DistinctKeys: distinkey },
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.Status == 1) {

                        $("#dlgDistinctKeys").dialog('close');
                        Alert(resp.Msg);
                        $('#dataGrid').datagrid('reload');



                    }
                    else {
                        Alert(resp.Msg);
                    }
                }
            });


        }

        //窗体清除数据
        function ClearWinData() {
            ClearWinDataByTag("input|textarea");
            $(rdoIsNull).attr("checked", true);
            $(ddlFormatValiFunc).val("none");
        }

        function ClearWinDataByTag(o) {
            var arr = o.split('|');
            for (var i = 0; i < arr.length; i++) {
                $(winInputInfo).find(arr[i]).val("");
            }
        }

        //根据信息窗体获取活动数据
        function GetModelInfoByWin() {


            var model =
            {
                "ActivityID": '<%=acid %>',
                "ExFieldIndex": $(txtExFieldIndex).val(),
                "MappingName": $(txtMappingName).val(),
                "FieldIsNull": rdoIsNotNull.checked ? 1 : 0,
                "FormatValiFunc": $(ddlFormatValiFunc).val(),
                "FieldType": rdFieldtype1.checked ? 1 : 0,
                "IsMultiline": rdoIsMultiline.checked ? 1 : 0,
                "IsHideInSubmitPage": rdoShowInSubmitPage.checked ? 0 : 1,
                "InputType": $("#ddlInputType").val(),
                "Options": $("#txtOptions").val()
            }

            return model;
        }

        //检查输入
        function CheckActivityInfoInput() {
            if ($("#txtMappingName").val() == "") {

                return false;
            }

            return true;
        }

        //显示添加信息输入框
        function ShowAdd() {
            ClearWinData();
            try {
                var rows = dataGrid.datagrid("getRows");
                var newIndex = 0;
                var rowlength = rows.length;

                var arrOldList = new Array();
                for (var i = 0; i < rows.length; i++) {
                    arrOldList.push(rows[i].ExFieldIndex);
                }
                for (var i = 1; i <= 60; i++) {
                    if (!arrOldList.Contains(i)) {
                        newIndex = i;
                        break;
                    }
                }

                $(txtExFieldIndex).val(newIndex);
                $(lbFieldName).text("K" + newIndex);
                $(rdoShowInSubmitPage).attr("checked", true);
                $("#rdoIsNotMultiline").attr("checked", true);

                if (newIndex > 60) {
                    $.messager.alert("系统提示", "最多能添加60个扩展字段");
                    return;
                }

            } catch (e) {
                alert(e);
            }

            //显示窗体
            //            $("#winInputInfo").window({
            //                title: "添加",
            //                closed: false,
            //                collapsible: false,
            //                minimizable: false,
            //                maximizable: false,
            //                iconCls: "icon-add",
            //                resizable: false,
            //                width: 400,
            //                height: 250,
            //                top: ($(window).height() - 320) * 0.5,
            //                left: ($(window).width() - 350) * 0.5
            //            });

            $("#winInputInfo").dialog('open');
            //设置保存按钮目标为添加
            //$("#btnWinSave").attr("tag", "add");
            currentAction = "add";
            $("#rdFieldtype0").attr("checked", true);

        }

        function ShowEdit() {

            // 只能选择一条记录操作
            var rows = dataGrid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return false;
            }

            //               var num = rows.length;
            //               if (num == 0) {
            //                   messager('系统提示', "请选择一条记录进行操作！");

            //                   return;
            //               }
            //               if (num > 1) {
            //                   $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
            //                   return;
            //               }

            if (rows[0].FieldIsDefauld == "1") {
                //messager('系统提示', "默认信息不能修改！");  
                Alert("默认信息不能修改");
                return;

            }
            // 只能选择一条记录操作

            //加载信息开始
            $("#txtMappingName").val(rows[0].MappingName);
            var isnull = rows[0].FieldIsNull;
            if (isnull == "1") {
                $("#rdoIsNotNull").attr("checked", true);
            }
            else if (isnull == "0") {
                $("#rdoIsNull").attr("checked", true);
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

            var fieldType = rows[0].FieldType;
            if (fieldType == "1") {
                $("#rdFieldtype1").attr("checked", true);
            }
            else {
                $("#rdFieldtype0").attr("checked", true);
            }
            var isHideInSubmitPage = rows[0].IsHideInSubmitPage;
            if (isHideInSubmitPage == "1") {
                $(rdoHideInSubmitPage).attr("checked", true);
            }
            else {
                $(rdoShowInSubmitPage).attr("checked", true);
            }
            $("#ddlFormatValiFunc").val(rows[0].FormatValiFunc);
            $("#lbFieldName").text(rows[0].FieldName);
            $("#txtExFieldIndex").val(rows[0].ExFieldIndex);
            $("#ddlInputType").val(rows[0].InputType);
            $("#txtOptions").val(rows[0].Options);
            if (rows[0].InputType == "text") {
                $(troptions).hide();
            }
            else {
                $(troptions).show();
            }
            //显示窗体
            //            $("#winInputInfo").window({
            //                title: "编辑",
            //                closed: false,
            //                collapsible: false,
            //                minimizable: false,
            //                maximizable: false,
            //                iconCls: "icon-edit",
            //                resizable: false,
            //                width: 400,
            //                height: 250,
            //                top: ($(window).height() - 250) * 0.5,
            //                left: ($(window).width() - 400) * 0.5
            //            });

            //设置保存按钮目标为添加
            //$("#btnWinSave").attr("tag", "edit");
            currentAction = "edit";
            $("#winInputInfo").dialog('open');

        }


        //添加活动
        function Add() {
            try {
                var model = GetModelInfoByWin();
                if (!CheckActivityInfoInput()) {
                    $("#txtMappingName").focus();
                    return false;
                }
            }
            catch (e) {
                alert(e);
            }

            try {
                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "Add", ActivityID: '<%=acid %>', JsonData: JSON.stringify(model).toString() },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            messager("系统提示", "添加成功");
                            $("#winInputInfo").window("close");
                            dataGrid.datagrid('reload');
                        } else {
                            Alert(resp.Msg);
                        }
                    }
                });
            } catch (e) {
                alert(e);
            }
        }
        function Edit() {

            try {
                var model = GetModelInfoByWin();
                if (!CheckActivityInfoInput()) {
                    $("#txtMappingName").focus();
                    return false;
                }


            }
            catch (e) {
                alert(e);
            }
            var rows = dataGrid.datagrid('getSelections');
            $.ajax({
                type: "Post",
                url: handlerUrl,
                data: { Action: "Edit", ActivityID: '<%=acid %>', JsonData: JSON.stringify(model).toString() },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        messager("系统提示", "修改成功");
                        $("#winInputInfo").window("close");
                        dataGrid.datagrid('reload');
                    } else {
                        Alert(resp.Msg);
                    }
                }
            });
        }
        // 删除---------------------
        function Delete() {
            var rows = dataGrid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return false;
            }
            //           var num = rows.length;
            //           if (num == 0) {
            //                Alert("请选择您要删除的记录");
            //               
            //               return;
            //           }

            var IsDelete = "true";

            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].ExFieldIndex);
                if (rows[i].FieldIsDefauld == "1") {
                    IsDelete = "false";

                }
            }
            if (IsDelete == "false") {
                Alert("默认字段不能删除！");
                //messager('系统提示', "默认字段不能删除！"); 
                return;
            }



            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Delete", ActivityID: '<%=acid %>', id: ids.join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                messager('系统提示', "删除成功！");
                                dataGrid.datagrid('reload');
                                return;
                            }
                            else {
                                Alert(resp.Msg);


                            }

                        }
                    });
                }
            });
        };
        // 删除---------------------


        //       function changeisnull(value){

        //       if (value=="1") {
        //            return "<font color='green'>是</font>"
        //            }
        //         else if (value=="0") {
        //                  return "<font color='red'>否</font>"
        //            }

        //       }
        function changeisnull(value, row) {
            if (row.FieldName == "Name" || row.FieldName == "Phone") {
                return "<font color='green'>是</font>";
            }

            if (value == "1") {
                return "<font color='green'>是</font>"
            }
            else if (value == "0") {
                return "<font color='red'>否</font>"
            }

        }
        function changeisdefault(value) {

            if (value == "0") {
                return "<font color='red'>否</font>"
            }
            else if (value == "1") {
                return "<font color='green'>默认</font>"
            }

        }
        function formartfieldtype(value) {

            if (value == "1") {
                return "<font color='red'>微信推广字段</font>"
            }
            else {
                return "<font color='green'>普通字段</font>"
            }

        }
        function formartshow(value) {

            if (value == "1") {
                return "<font color='red'>不显示</font>"
            }
            else {
                return "<font color='green'>显示</font>"
            }

        }



        function formartinputtype(value) {

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

        function formartvalitype(value) {

            if (value == "none") {
                return "无";
            }
            else if (value == "email") {
                return "电子邮箱";
            }

            else if (value == "url") {
                return "网址";
            }
            return "无";

        }

        function SetFieldSort(dir) {
            var rows = dataGrid.datagrid('getSelections');
            var allrows = dataGrid.datagrid("getRows");
            if (!EGCheckNoSelectMultiRow(rows)) {
                return false;
            }


            var FieldSort = "";
            var ids = [];
            var oldindex = 0;
            var newindex = 0;

            for (var i = 0; i < allrows.length; i++) {
                ids.push(allrows[i].FieldName);
                if (rows[0].FieldName == allrows[i].FieldName) {
                    oldindex = i;
                }
            }
            if (dir == 0) {//上移
                newindex = oldindex - 1;
                if (rows[0].MappingName == allrows[0].MappingName) {
                    Alert("已经最靠前了");
                    return false;
                }
            }
            else if (dir == 1) {//下移
                newindex = oldindex + 1;
                if (rows[0].MappingName == allrows[allrows.length - 1].MappingName) {
                    Alert("已经最靠后了");
                    return false;
                }

            }
            //alert(ids.join(','));
            ids = SortArray(oldindex, newindex, ids);
            //           if (dir==0) {//上移
            //                        var pre=ids[oldindex-1];
            //                        ids[oldindex-1]=rows[0].FieldName;
            //                        ids[oldindex]=pre;

            //                        }
            //                    else if(dir==1) {
            //                        var next=ids[oldindex+1];
            //                        ids[oldindex+1]=rows[0].FieldName;
            //                        ids[oldindex]=next;

            //                    }


            //alert(ids.join(','));
            //return false;

            jQuery.ajax({
                type: "Post",
                url: handlerUrl,
                data: { Action: "SetFieldSort", ActivityID: '<%=acid %>', FieldSort: ids.join(',') },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        messager('系统提示', "操作成功！");
                        dataGrid.datagrid('reload');

                    }
                    else {
                        Alert(resp.Msg);
                    }


                }
            });


        }

        function SortArray(oldIndex, newIndex, arr) {
            try {
                var tmp = arr[newIndex];
                arr[newIndex] = arr[oldIndex];
                arr[oldIndex] = tmp;
                return arr;
            } catch (e) {
                alert(e)
            }
        }

        var fileview = $.extend({}, $.fn.datagrid.defaults.view, { onAfterRender: function (target) { ischeckItem(); } });

        var checkedItems = [];
        function ischeckItem() {

            for (var i = 0; i < checkedItems.length; i++) {
                grid.datagrid('selectRecord', checkedItems[i]); //根据id选中行 

            }



        }

        function findCheckedItem(ID) {
            for (var i = 0; i < checkedItems.length; i++) {
                if (checkedItems[i] == ID) return i;
            }
            return -1;
        }

        function addcheckItem() {
            var row = dataGrid.datagrid('getChecked');
            for (var i = 0; i < row.length; i++) {
                if (findCheckedItem(row[i].FieldName) == -1) {
                    checkedItems.push(row[i].FieldName);
                }
            }
        }
        function removeAllItem(rows) {

            for (var i = 0; i < rows.length; i++) {
                var k = findCheckedItem(rows[i].FieldName);
                if (k != -1) {
                    checkedItems.splice(i, 1);
                }
            }
        }
        function removeSingleItem(rowIndex, rowData) {
            var k = findCheckedItem(rowData.FieldName);
            if (k != -1) {
                checkedItems.splice(k, 1);
            }
        }
        ///获取选项
        function GetOptions() {
            return options;
        }
    </script>
</asp:Content>
