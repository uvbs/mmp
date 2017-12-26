<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="MemberManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Member.MemberManage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var grid; //会员列表
        var groupgrid; ; //分组列表
        var grvGroupDataPager;
        //会员处理文件路径
        var url = "/Handler/Member/MemberManage.ashx";
        //分组处理文件路径
        var grouphandlerurl = "/Handler/Member/MemberGroupManage.ashx";
        var currOpRow;
        var cbaddedit;
        //加载文档
        jQuery().ready(function () {

            $(btnSendSMS).hide();
            $(btnSave).hide();
            $(btnExit).hide();

            $(window).resize(function () {
                $(grvMemberData).datagrid(
	            {
	                width: document.documentElement.clientWidth,
	                height: document.documentElement.clientHeight - 82
	            });
            });

            $('#ddlgroup').combogrid({
                panelWidth: 420,
                value: '',
                idField: 'GroupID',
                valuefield: 'GroupID',
                textField: 'GroupName',
                url: grouphandlerurl,
                queryParams: { Action: "Query" },
                pageSize: 10,
                singleSelect: false,
                pagination: true,
                columns: [[
                           { field: 'GroupName', title: '分组名称', width: 400 }
                    ]]
            });
            cbaddedit = $("#ddlUserAddOrEditGroup").combogrid({
                panelWidth: 420,
                value: '',
                idField: 'GroupID',
                valuefield: 'GroupID',
                textField: 'GroupName',
                url: grouphandlerurl,
                queryParams: { Action: "Query" },
                pageSize: 10,
                singleSelect: false,
                pagination: true,
                columns: [[
                           { field: 'GroupName', title: '分组名称', width: 400 }
                    ]]
            });

            //-----------------加载客户gridview
            grid = jQuery("#grvMemberData").datagrid({
                method: "Post",
                idField: "MemberID",
                view: fileview,
                url: url,
                height: document.documentElement.clientHeight - 81,
                pageSize: 20,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                fitcolumns: true,
                queryParams: { Action: "Query", SearchTitle: "" }
            });
            //------------加载客户gridview

            //-----------------加载客户分组gridview
            groupgrid = jQuery(grvGroupData).datagrid({
                method: "Post",
                url: grouphandlerurl,
                width: 436,
                height: 334,
                rownumbers: false,
                singleSelect: false,
                pagination: true,
                striped: true,
                pageSize: 20,
                queryParams: { Action: "Query" }

            });
            //------------加载客户分组gridview

            $(grvGroupFilterData).datagrid({
                method: "Post",
                url: grouphandlerurl,
                width: 435,
                height: 335,
                rownumbers: false,
                singleSelect: false,
                pagination: true,
                striped: true,
                pageSize: 10,
                queryParams: { Action: "Query" }
                //                ,
                //                onClickRow: function (value, row) {
                //                    var rows = groupgrid.datagrid('getSelections');
                //                    var ids = [];
                //                    for (var i = 0; i < rows.length; i++) {
                //                        ids.push(rows[i].GroupID);
                //                    }
                //                    grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: $("#txtName").val(), GroupID: ids.join(',')} })
                //                }

            });

            //自定义客户分组分页控件
            //            grvGroupDataPager = $('#grvGroupData').datagrid('getPager');

            //            grvGroupDataPager.pagination({
            //                showPageList: false,
            //                showRefresh: false,
            //                displayMsg: ''
            //            });

            //取消---------------------
            $(dlgUserOperator).find("#btnExit").bind("click", function () {
                $(dlgUserOperator).dialog("close");
            });
            //取消---------------------

            //取消---------------------
            $("#win_group").find("#btnExit_Group").bind("click", function () {
                $("#win_group").window("close");
            });
            //取消---------------------


            //取消---------------------
            $("#win_remind").find("#btnExit_Remind").bind("click", function () {
                $("#win_remind").window("close");
            });
            //取消---------------------

            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var rows = groupgrid.datagrid('getSelections');
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].GroupID);
                }
                grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: $("#txtName").val(), GroupID: ids.join(',')} })

            });
            //搜索结束---------------------

            //保存客户---------------------
            $("#btnSave").bind("click", function () {
                Save();
            }); //保存客户---------------------


            //发送短信---------------------
            $("#btnSendSMS").bind("click", function () {
                SendSMS();
            });
            //发送短信---------------------


            //保存分组---------------------
            $("#btnSave_Group").bind("click", function () {
                Save_Group();
            }); //保存分组---------------------


            //批量导入联系人---------------------
            $("#btnSave_BatchInsert").bind("click", function () {
                BatchInsertMembers();
            });
            //批量导入联系人---------------------

            //设置提醒---------------------
            $("#btnSave_Remind").bind("click", function () {
                InsertRemind();
            });
            //设置提醒---------------------


            $(btnFiterByGroup).click(function () {
                $(dlgFilterByGroup).dialog('open');
            });

            $(btnGroupMgr).click(function () {
                $(dlgGroupMgr).dialog('open');
            });

            $(btnShowSendSms).click(function () {
                $(dlgSendSms).dialog('open');
            });

            $(dlgDeleteGroup).dialog({
                buttons: [{
                    text: "确定",
                    handler: function () {
                        var rows = groupgrid.datagrid('getSelections');
                        var num = rows.length;
                        if (num == 0) {
                            messager("系统提示", "请选择您要删除的分组");
                            return;
                        }
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].GroupID);
                        }

                        var isDeleteData = chkIsDeleteDataForGroup.checked ? 1 : 0;
                        $.messager.progress({ text: "正在处理。。。" });
                        jQuery.ajax({
                            type: "Post",
                            url: grouphandlerurl,
                            data: { Action: "Delete", id: ids.join(','), isDeleteData: isDeleteData },
                            success: function (result) {
                                $.messager.progress('close');
                                var resp = $.parseJSON(result);
                                if (resp.Status > 0) {
                                    Show(resp.Msg);
                                    groupgrid.datagrid('reload');
                                    grid.datagrid('reload');
                                    LoadMemberGroup(); //加载分组下拉框
                                    $(dlgDeleteGroup).dialog('close');
                                }
                                else {
                                    Alert(resp.Msg);
                                    //$.messager.alert("系统提示", result);
                                }
                            }

                        });


                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $(dlgDeleteGroup).dialog('close');
                    }
                }]
            });

            $(dlgBatchInsert).dialog({
                title: "批量导入客户",
                closed: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                width: 600,
                height: 420,
                buttons: [{
                    text: '导入',
                    handler: function () {
                        /* $.messager.progress({ text: '正在导入数据。。。' });
                        var fd = new FormData(document.forms.namedItem("formBatchInsert"));
                        fd.append("Action", "BatchInsertMembers");
                        fd.append("GroupID", $("#ddlgroup").combogrid('getValue'));
                        $.ajax({
                        url: url,
                        type: "POST",
                        data: fd,
                        processData: false,  // tell jQuery not to process the data
                        contentType: false,   // tell jQuery not to set contentType
                        success: function (result) {
                        var resp = $.parseJSON(result);
                        $(lbResultLog).html(resp.Msg);
                        if (resp.Status == 1) {
                        try {
                        $.messager.progress('close');
                        Alert(resp.Msg);
                        grid.datagrid('reload');
                        //$(dlgBatchInsert).dialog('close');
                        document.getElementById("formBatchInsert").reset();

                        } catch (e) {
                        Alert(e);
                        }
                        }
                        else {
                        $.messager.progress('close');
                        Alert(resp.Msg);
                        }

                        var badDataUrl = resp.ExStr;
                        //Alert(badDataUrl);
                        if (badDataUrl != '' && badDataUrl != null) {
                        $(btnDownloadBadData).show();
                        $(btnDownloadBadData).attr('href', badDataUrl);
                        }
                        else {
                        $(btnDownloadBadData).hide();
                        $(btnDownloadBadData).attr('href', "");
                        }
                        }
                        });
                        */

                        //2013.10.14 charlie修改:采用ajaxFileUpload方法导入

                        try {
                            var reqUrl = new StringBuilder();

                            reqUrl.AppendFormat("{0}?Action=BatchInsertMembers&GroupID={1}", url, $("#ddlgroup").combogrid('getValue'));

                            $.messager.progress({ text: '(new)正在导入数据。。。' });
                            $.ajaxFileUpload(
                             {
                                 url: reqUrl.ToString(),
                                 secureuri: false,
                                 fileElementId: 'BatchInsertFile',
                                 dataType: 'text',
                                 success: function (result) {
                                     $.messager.progress('close');
                                     result = result.substring(result.indexOf("{"), result.indexOf("</"));
                                     var resp = $.parseJSON(result);
                                     $(lbResultLog).html(resp.Msg);
                                     if (resp.Status == 1) {
                                         try {

                                             Alert(resp.Msg);
                                             grid.datagrid('reload');
                                             //$(dlgBatchInsert).dialog('close');
                                             document.getElementById("formBatchInsert").reset();

                                         } catch (e) {
                                             Alert(e);
                                         }
                                     }
                                     else {
                                         Alert(resp.Msg);
                                     }

                                     var badDataUrl = resp.ExStr;
                                     //Alert(badDataUrl);
                                     if (badDataUrl != '' && badDataUrl != null) {
                                         $(btnDownloadBadData).show();
                                         $(btnDownloadBadData).attr('href', badDataUrl);
                                     }
                                     else {
                                         $(btnDownloadBadData).hide();
                                         $(btnDownloadBadData).attr('href', "");
                                     }
                                 }
                             }
                            );
                        } catch (e) {
                            Alert(e);
                        }


                    }
                }, {
                    text: '关闭',
                    handler: function () {
                        $(dlgBatchInsert).dialog('close');
                    }
                }]
            });

            $(dlgFilterByGroup).dialog({
                toolbar: [],
                buttons: [
                    {
                        text: '确定',
                        handler: function () {

                            $(dlgFilterByGroup).dialog('close');
                            var rows = $(grvGroupFilterData).datagrid('getSelections');
                            var ids = [];

                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].GroupID);
                            }
                            //alert(ids.join(','));
                            grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: $("#txtName").val(), GroupID: ids.join(',')} })
                        }
                    },
                    {
                        text: '取消',
                        handler: function () {
                            $(dlgFilterByGroup).dialog('close');
                        }
                    }
                ]
            });

            $(dlgGroupMgr).dialog({
                toolbar: [{
                    text: '添加分组',
                    iconCls: 'icon-add',
                    handler: function () {
                        ShowAddOrEdit_Group('add');
                    }
                }, {
                    text: '编辑分组',
                    iconCls: 'icon-edit',
                    handler: function () {
                        ShowAddOrEdit_Group('edit');
                    }
                }, {
                    text: '删除分组',
                    iconCls: 'icon-remove',
                    handler: function () {
                        DeleteGroup();
                    }
                }
                ]
            });

            $(dlgSendSms).dialog({
                buttons: [{
                    text: '确认发送',
                    handler: function () {
                        SendSMS();
                    }
                }, {
                    text: '关闭',
                    handler: function () {
                        $(dlgSendSms).dialog('close');
                    }
                }]
            });

            $(dlgUserOperator).dialog({
                //title: title,
                closed: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                width: 400,
                height: 350,
                top: ($(window).height() - 450) * 0.5,
                left: ($(window).width() - 400) * 0.5,
                buttons: [{
                    text: '保存',
                    handler: function () {
                        Save();
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $(dlgUserOperator).dialog('close');
                    }
                }]
            });


            //加载分组
            LoadMemberGroup();
        });

        //弹出添加或编辑框开始
        function ShowAddOrEdit(addoredit) {
            var title = ""; //弹出框标题
            var titleicon = "icon-" + addoredit; //弹出框标题图标
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                ClearAll();
                //设置弹出框标题
                title = "添加";

            }
            //弹出添加框结束

            //弹出编辑框开始
            else if (addoredit == "edit") {
                // 只能选择一条记录操作
                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager('系统提示', "请选择一条记录进行操作！");
                    return;
                }
                if (num > 1) {
                    $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                    return;
                }
                // 只能选择一条记录操作
                ClearAll();
                //加载信息开始
                $("#txtTrueName").val(rows[0].Name);
                var sex = rows[0].Sex;
                if (sex == "男") {
                    $("#rdoman").attr("checked", true);
                }
                else if (sex == "女") {
                    $("#rdowoman").attr("checked", true);

                }
                $("#txtBirthday").datebox("setValue", rows[0].Birthday);
                $("#txtMobile").val(rows[0].Mobile);
                $("#txtEmail").val(rows[0].Email);
                $("#txtQQ").val(rows[0].QQ);
                $("#txtTel").val(rows[0].Tel);
                $("#txtWebsite").val(rows[0].Website);
                $("#txtCompany").val(rows[0].Company);
                $("#txtTitle").val(rows[0].Title);
                $("#txtWeiboID").val(rows[0].WeiboID);
                $("#txtWeiboScreenName").val(rows[0].WeiboScreenName);
                $("#txtCardImageUrl").val(rows[0].CardImageUrl);
                $("#txtAddress").val(rows[0].Address);
                //$("#ddlMemberGroup").val(rows[0].GroupID);
                $(ddlUserAddOrEditGroup).combogrid('setValue', rows[0].GroupID);

                $(txtRemark).val(rows[0].Remark);
                $(txtWeixinOpenID).val(rows[0].WeixinOpenID);

                //$(ddlMemberType).val(rows[0].MemberType);
                $(ddlMemberType).combobox('setValue', rows[0].MemberType);
                //Alert($(ddlMemberType).combobox('getValue'));

                $(txtMobile2).val(rows[0].Mobile2);
                $(txtMobile3).val(rows[0].Mobile3);
                $(txtMobile4).val(rows[0].Mobile4);
                $(txtWeiboID2).val(rows[0].WeiboID2);
                $(txtWeiboID3).val(rows[0].WeiboID3);
                $(txtWeiboID4).val(rows[0].WeiboID4);
                $(txtWeixinOpenID2).val(rows[0].WeixinOpenID2);
                $(txtWeixinOpenID3).val(rows[0].WeixinOpenID3);
                $(txtWeixinOpenID4).val(rows[0].WeixinOpenID4);
                $(txtEmail2).val(rows[0].Email2);
                $(txtEmail3).val(rows[0].Email3);
                $(txtEmail4).val(rows[0].Email4);

                //加载信息结束
                //设置弹出框标题
                title = "编辑";


            }
            //弹出编辑框结束

            //弹出对话框
            $(dlgUserOperator).dialog({
                title: title,
                iconCls: titleicon
            });
            $(dlgUserOperator).dialog('open');
            //弹出对话框

            //设置保存按钮属性 add为添加，edit为编辑
            $("#btnSave").attr("tag", addoredit);


        }
        //展示添加或编辑客户框结束

        function ShowEditSingel(index) {
            try {
                var title = ""; //弹出框标题
                var titleicon = "icon-edit"; //弹出框标题图标

                // 只能选择一条记录操作
                var rows = grid.datagrid('getRows');
                var num = rows.length;
                if (num == 0) {
                    messager('系统提示', "系统没有加载任何行！");
                    return;
                }

                if (index > num) {
                    messager('系统提示', "选择行异常！");
                    return;
                }

                currOpRow = rows[index];

                // 只能选择一条记录操作
                ClearAll();
                //加载信息开始
                $("#txtTrueName").val(rows[index].Name);
                var sex = rows[index].Sex;
                if (sex == "男") {
                    $("#rdoman").attr("checked", true);
                }
                else if (sex == "女") {
                    $("#rdowoman").attr("checked", true);

                }
                $("#txtBirthday").datebox("setValue", rows[index].Birthday);
                $("#txtMobile").val(rows[index].Mobile);
                $("#txtEmail").val(rows[index].Email);
                $("#txtQQ").val(rows[index].QQ);
                $("#txtTel").val(rows[index].Tel);
                $("#txtWebsite").val(rows[index].Website);
                $("#txtCompany").val(rows[index].Company);
                $("#txtTitle").val(rows[index].Title);
                $("#txtWeiboID").val(rows[index].WeiboID);
                $("#txtWeiboScreenName").val(rows[index].WeiboScreenName);
                //$("#ddlGroup").val(rows[index].GroupID);
                $("#txtCardImageUrl").val(rows[index].CardImageUrl);
                $("#txtAddress").val(rows[index].Address);
                //$("#ddlMemberGroup").val(rows[index].GroupID);
                $(ddlUserAddOrEditGroup).combogrid('setValue', rows[index].GroupID);
                var g = cbaddedit.combogrid('grid'); // get datagrid object
                var r = g.datagrid('getRows'); // get the selected row
                var tmpBool = true;
                for (var i = 0; i < r.length; i++) {
                    //alert(r[i].GroupID);
                    if (rows[index].GroupID == r[i].GroupID) {
                        tmpBool = false;
                        break;
                    }
                }
                if (tmpBool) {
                    $(ddlUserAddOrEditGroup).combogrid('setText', rows[index].GroupName);
                }



                $(txtRemark).val(rows[index].Remark);
                $(txtWeixinOpenID).val(rows[index].WeixinOpenID);
                //$(ddlMemberType).val(rows[index].MemberType);
                $(ddlMemberType).combobox('setValue', rows[index].MemberType);

                $(txtMobile2).val(rows[index].Mobile2);
                $(txtMobile3).val(rows[index].Mobile3);
                $(txtMobile4).val(rows[index].Mobile4);
                $(txtWeiboID2).val(rows[index].WeiboID2);
                $(txtWeiboID3).val(rows[index].WeiboID3);
                $(txtWeiboID4).val(rows[index].WeiboID4);
                $(txtWeixinOpenID2).val(rows[index].WeixinOpenID2);
                $(txtWeixinOpenID3).val(rows[index].WeixinOpenID3);
                $(txtWeixinOpenID4).val(rows[index].WeixinOpenID4);
                $(txtEmail2).val(rows[index].Email2);
                $(txtEmail3).val(rows[index].Email3);
                $(txtEmail4).val(rows[index].Email4);

                //加载信息结束
                //设置弹出框标题
                title = "编辑";

                //弹出编辑框结束


                //弹出对话框
                //                $(dlgUserOperator).dialog({
                //                    title: title,
                //                    closed: false,
                //                    collapsible: false,
                //                    minimizable: false,
                //                    maximizable: false,
                //                    iconCls: titleicon,
                //                    resizable: false,
                //                    width: 400,
                //                    height: 450,
                //                    top: ($(window).height() - 450) * 0.5,
                //                    left: ($(window).width() - 400) * 0.5

                //                });

                //弹出对话框
                $(dlgUserOperator).dialog({
                    title: title,
                    iconCls: titleicon
                });
                $(dlgUserOperator).dialog('open');

                //设置保存按钮属性 add为添加，edit为编辑
                $("#btnSave").attr("tag", "edit");


            } catch (e) {
                alert(e);
            }
        }

        //弹出添加分组或编辑框开始
        function ShowAddOrEdit_Group(addoredit) {
            var title = ""; //弹出框标题
            var titleicon = "icon-" + addoredit; //弹出框标题图标
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                ClearAll();
                //设置弹出框标题
                title = "添加分组";

            }
            //弹出添加框结束

            //弹出编辑框开始
            else if (addoredit == "edit") {
                // 只能选择一条记录操作
                var rows = groupgrid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager('系统提示', "请选择一个分组进行操作！");
                    return;
                }
                if (num > 1) {
                    $.messager.alert("系统提示", "您选择了多个分组，只能选择一个分组进行修改。", "warning");
                    return;
                }

                if (rows[0].GroupName == "无分组") {

                    messager("系统提示", "默认分组不能修改");
                    return;

                }
                // 只能选择一条记录操作

                //加载信息开始
                $("#txtGroupName").val(rows[0].GroupName);
                //加载信息结束
                //设置弹出框标题
                title = "编辑分组";


            }
            //弹出编辑框结束


            //弹出对话框
            $("#win_group").window({
                title: title,
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: titleicon,
                resizable: false,
                width: 300,
                height: 150,
                top: ($(window).height() - 150) * 0.5,
                left: ($(window).width() - 300) * 0.5

            });
            //弹出对话框

            //设置保存按钮属性 add为添加，edit为编辑
            $("#btnSave_Group").attr("tag", addoredit);


        }
        //展示添加或编辑框结束





        //添加或编辑客户操作开始---------
        function Save() {
            try {
                var model = GetModel();
                //--检查输入---------------
                if (model.Name == "") {
                    $("#txtTrueName").focus();
                    return false;
                }
                if (model.Mobile == "") {
                    $("#txtMobile").focus();
                    return false;
                }

                //-------检查输入
                var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
                //----------执行添加操作开始
                if (action == "add") {
                    //------------添加
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Add", JsonData: JSON.stringify(model).toString() },
                        success: function (result) {
                            if (result == "true") {
                                messager("系统提示", "添加成功");
                                $(dlgUserOperator).dialog("close");
                                grid.datagrid('reload');
                            } else {
                                messager("系统提示", result);
                            }
                        }
                    });
                    //添加---------------
                }
                //-----------执行添加操作结束
                //-----------执行编辑操作开始
                else if (action == "edit") {
                    //-----------修改
                    $.messager.progress({ text: '正在操作。。。。' });
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Edit", MemberID: currOpRow.MemberID, JsonData: JSON.stringify(model).toString() },
                        success: function (result) {
                            $.messager.progress('close');
                            if (result == "true") {
                                messager("系统提示", "修改成功");
                                $(dlgUserOperator).dialog("close");
                                grid.datagrid('reload');
                            }
                            else {
                                messager("系统提示", result);
                            }
                        }
                    })               //修改
                }
                //--------------执行编辑操作结束

            } catch (e) {
                alert(e);
            }
        }
        //添加或编辑客户操作结束---------



        //添加或编辑分组操作开始---------
        function Save_Group() {
            //--检查输入---------------
            var GroupName = $("#txtGroupName").val();
            if (GroupName == "") {
                $("#txtGroupName").focus();
                return false;
            }

            //-------检查输入
            var action = $("#btnSave_Group").attr("tag"); //获取添加或编辑属性
            //----------执行添加操作开始
            if (action == "add") {
                //------------添加
                $.messager.progress({ text: '正在处理。。。' });
                jQuery.ajax({
                    type: "Post",
                    url: grouphandlerurl,
                    data: { Action: "Add", GroupName: GroupName },
                    success: function (result) {
                        $.messager.progress('close');
                        if (result == "true") {
                            messager("系统提示", "添加分组成功");
                            $("#win_group").window("close");
                            groupgrid.datagrid('reload');
                            LoadMemberGroup(); //加载分组下拉框
                        } else {
                            messager("系统提示", result);
                        }
                    }
                });

                //添加---------------
            }
            //-----------执行添加操作结束
            //-----------执行编辑操作开始
            else if (action == "edit") {
                //-----------修改
                var rows = groupgrid.datagrid('getSelections');
                jQuery.ajax({
                    type: "Post",
                    url: grouphandlerurl,
                    data: { Action: "Edit", GroupID: rows[0].GroupID, GroupName: GroupName },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "修改成功");
                            $("#win_group").window("close");
                            groupgrid.datagrid('reload');
                            grid.datagrid('reload');
                            LoadMemberGroup(); //加载分组下拉框
                        }
                        else {
                            messager("系统提示", result);
                        }
                    }
                })               //修改
            }
            //--------------执行编辑操作结束

        }
        //添加或编辑分组结束---------



        // 删除---------------------
        function BatchDeleteMember() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要删除的客户");
                return;
            }
            var ids = [];
            //var ids = new Array();

            for (var i = 0; i < rows.length; i++) {
                //Alert(rows[i].MemberID);
                ids.push(rows[i].MemberID);
            }

            //            Alert(ids.join(','));
            //            return;

            $.messager.confirm("系统提示", "是否确定删除选中客户?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Delete", id: ids.join(',') },
                        success: function (result) {
                            var resp = $.parseJSON(result);

                            if (resp.Status > 0) {
                                Show(resp.Msg);
                                grid.datagrid('clearSelections');
                                grid.datagrid('reload');

                            } else {
                                Alert(resp.Msg);
                            }


                            //                            if (result) {
                            //                                messager('系统提示', "删除成功！");
                            //                                grid.datagrid('reload');

                            //                                return;
                            //                            }
                            //                            $.messager.alert("系统提示", result);
                        }
                    });
                }
            });
        };
        // 删除---------------------


        // 发送短信---------------------
        function SendSMS() {

            var smstype = $("#ddlSendSMSType").val();
            if (smstype == "1") {//勾选发送
                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager("系统提示", "请勾选您要发送的号码");
                    return;
                }
                var mobilelist = [];
                var namelist = [];
                for (var i = 0; i < rows.length; i++) {
                    mobilelist.push(rows[i].Mobile);
                    namelist.push(rows[i].Name);
                }

                $.messager.confirm("系统提示", "确定给 " + namelist.join(',') + " 发送短信?", function (r) {
                    if (r) {
                        jQuery.ajax({
                            type: "Post",
                            url: url,
                            data: { Action: "SendSMS", Type: "1", MobileList: mobilelist.join(',') },
                            success: function (result) {
                                if (result == "true") {

                                    //window.location.href = "/SMS/Send.aspx";
                                    parent.addTab('根据勾选发送短信', "/SMS/Send.aspx", 'tu0818', true);
                                    return;
                                }
                                $.messager.alert("系统提示", result);
                            }
                        });
                    }
                });
            } //勾选发送结束
            //筛选发送开始
            else if (smstype == "0") {
                //筛选发送
                $.messager.confirm("系统提示", "确定按照筛选条件发送短信?", function (r) {
                    if (r) {
                        var rows = groupgrid.datagrid('getSelections');
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].GroupID);
                        }

                        jQuery.ajax({
                            type: "Post",
                            url: url,
                            data: { Action: "SendSMS", Type: "0", SearchTitle: $("#txtName").val(), GroupID: ids.join(',') },
                            success: function (result) {
                                if (result == "true") {
                                    //window.location.href = "/SMS/Send.aspx";
                                    parent.addTab('根据筛选发送短信', "/SMS/Send.aspx", 'tu0818', true);
                                    return;
                                }
                                $.messager.alert("系统提示", result);
                            }
                        });
                    }
                });


            }
            //筛选发送结束

            $(dlgSendSms).dialog('close');
        };
        // 发送短信---------------------

        // 删除分组---------------------
        function DeleteGroup() {
            try {

                var rows = groupgrid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager("系统提示", "请选择您要删除的分组");
                    return;
                }

                //alert(1);
                $(dlgDeleteGroup).dialog('open');

            } catch (e) {
                Alert(e);
            }
        };

        // 删除---------------------

        function operate(value, row, index) {


            return "<font>文本</font>";



        }

        //加载分组列表
        function LoadMemberGroup() {

            try {
                $('#ddlgroup').combogrid('grid').datagrid('reload');
            } catch (e) {
                alert(e);
            }

            //            $.post(url, { Action: "GetMemberGroupList" }, function (data) {
            //                $("#sp_group").html(data);
            //            });
        }
        //加载分组列表

        //获取客户模型
        function GetModel() {
            var model =
            {
                "Name": $("#txtTrueName").val(),
                "Sex": rdoman.checked ? "男" : "女",
                "Birthday": $("#txtBirthday").datebox("getValue"),
                "Mobile": $("#txtMobile").val(),
                "Email": $("#txtEmail").val(),
                "QQ": $("#txtQQ").val(),
                "Tel": $("#txtTel").val(),
                "Website": $("#txtWebsite").val(),
                "Company": $("#txtCompany").val(),
                "Title": $("#txtTitle").val(),
                "GroupID": $(ddlUserAddOrEditGroup).combogrid('getValue'),
                "CardImageUrl": $("#txtCardImageUrl").val(),
                "Address": $("#txtAddress").val(),
                "Remark": $(txtRemark).val(),
                "WeiboID": $(txtWeiboID).val(),
                "WeiboScreenName": $(txtWeiboScreenName).val(),
                "WeixinOpenID": $(txtWeixinOpenID).val(),
                "MemberType": $(ddlMemberType).combobox('getValue'), //$(ddlMemberType).val(),
                "Mobile2": $(txtMobile2).val(),
                "Mobile3": $(txtMobile3).val(),
                "Mobile4": $(txtMobile4).val(),
                "WeiboID2": $(txtWeiboID2).val(),
                "WeiboID3": $(txtWeiboID3).val(),
                "WeiboID4": $(txtWeiboID4).val(),
                "WeixinOpenID2": $(txtWeixinOpenID2).val(),
                "WeixinOpenID3": $(txtWeixinOpenID3).val(),
                "WeixinOpenID4": $(txtWeixinOpenID4).val(),
                "Email2": $(txtEmail2).val(),
                "Email3": $(txtEmail3).val(),
                "Email4": $(txtEmail4).val()

            }
            return model;
        }

        //分页保持勾选状态
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
            var row = grid.datagrid('getChecked');
            for (var i = 0; i < row.length; i++) {
                if (findCheckedItem(row[i].MemberID) == -1) {
                    checkedItems.push(row[i].MemberID);
                }
            }
        }
        function removeAllItem(rows) {

            for (var i = 0; i < rows.length; i++) {
                var k = findCheckedItem(rows[i].MemberID);
                if (k != -1) {
                    checkedItems.splice(i, 1);
                }
            }
        }
        function removeSingleItem(rowIndex, rowData) {
            var k = findCheckedItem(rowData.MemberID);
            if (k != -1) {
                checkedItems.splice(k, 1);
            }
        }
        //分布保持勾选状态

        //       //显示批量导入弹出框
        //       function ShowBatchInsertMember() {
        //          
        //           //弹出对话框
        //           $("#dlgBatchInsert").window({
        //               title: "批量导入",
        //               closed: false,
        //               collapsible: false,
        //               minimizable: false,
        //               maximizable: false,
        //               iconCls: "icon-add",
        //               resizable: false,
        //               width: 400,
        //               height: 250,
        //               top: ($(window).height() - 250) * 0.5,
        //               left: ($(window).width() - 400) * 0.5

        //           });
        //           //弹出对话框

        //       }

        //显示提醒弹出框
        function ShowRemind() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请先选择您要设置提醒的客户");
                return;
            }
            if (num > 1) {
                messager("系统提示", "一次只能选择一个客户");
                return;
            }
            ClearAll();

            $("#rd_phone").attr("checked", true);
            $("#txtTheme").val("打电话");

            //弹出对话框
            $("#win_remind").window({
                title: "设置提醒",
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: "icon-add",
                resizable: false,
                width: 400,
                height: 320,
                top: ($(window).height() - 300) * 0.5,
                left: ($(window).width() - 400) * 0.5

            });
            //弹出对话框
            $("#txtContent").val(rows[0].Name);

        }

        //插入提醒信息
        function InsertRemind() {

            var model = GetModelInfo();
            if (model.Title == "") {
                $("#txtTheme").focus();
                return false;
            }

            if (model.RemindTime == "") {
                messager("系统提示", "请选择提醒时间");
                return false;
            }
            jQuery.ajax({
                type: "Post",
                url: "/Handler/User/UserRemindManage.ashx",
                data: { Action: "Add", JsonData: JSON.stringify(model).toString() },
                success: function (result) {
                    if (result == "true") {
                        messager("系统提示", "设置提醒成功");
                        $("#win_remind").window("close");
                    } else {
                        messager("系统提示", result);
                    }
                }
            });
        }
        //插入提醒信息

        //获取提醒数据
        function GetModelInfo() {
            var model =
            {
                "MemberID": grid.datagrid('getSelections')[0].MemberID,
                "Title": $("#txtTheme").val(),
                "Content": $("#txtContent").val(),
                "RemindTime": $("#txtRemindTime").datetimebox("getValue")
            }
            return model;
        }

        function SetThemeText(obj) {
            var text = $(obj).text();
            $("#txtTheme").val(text);
        }

        function getimage(value) {
            if (value == "") {
                return "<font>无图片</font>";
            }
            return '<a href="javascript:;" title="点击查看大图"  onclick="viewimage(' + "'" + value + "'" + ')">' + " <img src=" + value + " width=50 height=25 >" + '</a>'
        }
        function viewimage(imgurl) {
            var img = "无图片";
            if (imgurl != "") {
                img = "<a href='#' title='单击关闭窗口'><img onclick='closewindow()'  src=" + imgurl + " width=350 height=262 ></a>";
            }

            $("#div_image").html(img);
            $("#win_viewimage").window({
                title: "查看图片",
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: "icon-edit",
                resizable: true,
                width: 390,
                height: 330,
                top: ($(window).height() - 330) * 0.5,
                left: ($(window).width() - 390) * 0.5

            });
        }

        function closewindow() {

            $("#win_viewimage").window("close");
        }

        //显示批量导入弹出框
        function ShowBatchInsertMember() {
            $(btnDownloadBadData).hide();
            $(btnDownloadBadData).attr('href', "");

            //弹出对话框
            $(dlgBatchInsert).dialog('open');
        }

        function FormartOpearteBtn(value, row, index) {
            //<a href=""><img alt="管理成员" class="imgAlign" src="../../images/contacts.gif" title="管理成员"></a>
            var result = new StringBuilder();
            result.AppendFormat('<a href="javascript:;" onclick="ShowEditSingel({0})" ><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>', index);
            //            result.AppendFormat('&nbsp;<a href="javascript:;" onclick="DeleteGroupForSingel(\'{0}\')"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', row.MemberID);
            return result.ToString();
        }
 
    </script>

       <script type="text/javascript">


       KindEditor.ready(function (K) {
           var editor = K.editor({
               allowFileManager: false,
               uploadJson: '/Kindeditor/asp.net/upload_json_ChangeName.ashx'

           });
           $('#btnupload').click(function () {
               editor.loadPlugin('image', function () {
                   editor.plugin.imageDialog({
                       showRemote: false,
                       imageUrl: $('#txtCardImageUrl').val(),
                       clickFn: function (url, title, width, height, border, align) {
                           $('#txtCardImageUrl').val(url);
                           editor.hideDialog();
                       }
                   });
               });
           });
           //批量导入联系人
           $('#btnBatInsert').click(function () {
               editor.loadPlugin('insertfile', function () {
                   editor.plugin.fileDialog({
                       showRemote: false,
                       fileUrl: K('#txtFilePath').val(),
                       clickFn: function (url, title) {
                           $('#txtFilePath').val(url) 
                           editor.hideDialog();
                       }
                   });
               });
           });

       });
   </script>

    <style type="text/css">
        #txtFilePath
        {
            width: 200px;
        }
        .style1
        {
            width: 65px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <table style="width: 100%">
                <tr>
                    <td>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-excel" plain="true" onclick="ShowBatchInsertMember()">
                            批量导入客户</a> <a href="#" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
                                onclick="ShowAddOrEdit('add')">在线添加客户</a><a href="#" class="easyui-linkbutton" iconcls="icon-delete"
                                    plain="true" onclick="BatchDeleteMember()">批量删除客户</a>
                        <%--<a href="#" class="easyui-linkbutton"
                                            iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">编辑客户</a>--%>
                        <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-usergroup3'"
                            title="按分组筛选" id="btnFiterByGroup">按分组筛选</a> <a href="javascript:;" class="easyui-linkbutton"
                                data-options="plain:true,iconCls:'icon-usergroup3'" title="按分组筛选" id="btnGroupMgr">
                                分组管理</a>
                    </td>
                    <td style="text-align: right">
                        <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-mobileArrow'"
                            title="发送短信" id="btnShowSendSms">发送短信</a> <a href="#" class="easyui-linkbutton" iconcls="icon-clock"
                                plain="true" onclick="ShowRemind()">设置提醒</a>
                        <%--<a href="javascript:;" class="easyui-linkbutton"
                                    data-options="plain:true,iconCls:'icon-mailReplay'" title="发送邮件" id="A2">发送邮件</a>--%>
                        <%--<a href="#" class="easyui-linkbutton" iconcls="icon-add"
                                        plain="true" onclick="ShowAddOrEdit_Group('add')">添加分组</a> <a href="#" class="easyui-linkbutton"
                                            iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit_Group('edit')">编辑分组</a>
                            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="DeleteGroup()">
                                删除分组</a>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div style="font-size: 12px; font-weight: normal; margin-top: 10px;">
            <span>客户姓名：</span>
            <input type="text" style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <%--&nbsp;|&nbsp;
                发送短信：<select id="ddlSendSMSType">
                    <option value="1">向勾选号码发送短信</option>
                    <option value="0">向筛选号码发送短信</option>
                </select>
                <a href="#" class="easyui-linkbutton" id="btnSendSMS">确认发送</a>
            --%>
        </div>
    </div>
    <table id="grvMemberData" cellspacing="0" cellpadding="0" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="10" checkbox="true">
                </th>
                <th field="Name" width="20">
                    姓名
                </th>
                <th field="Mobile" width="20">
                    手机
                </th>
                <th field="Email" width="20">
                    电子邮箱
                </th>
                <th field="Tel" width="20">
                    电话
                </th>
                <th field="QQ" width="20">
                    QQ
                </th>
                <th field="WeiboScreenName" width="20">
                    微博昵称
                </th>
                <th field="Company" width="20">
                    公司
                </th>
                <th field="Title" width="20">
                    职位
                </th>
                <th field="GroupName" width="20">
                    分组
                </th>
                <th field="GroupID" width="10" formatter="FormartOpearteBtn" align="center">
                    操作
                </th>
                <%--<th field="CardImageUrl" formatter="getimage" width="20">
                                        图片
                                    </th>--%>
            </tr>
        </thead>
    </table>
    <div id="dlgUserOperator" class="easyui-dialog" modal="true" closed="true">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td>
                        客户类别:
                    </td>
                    <td>
                        <%--<select id="ddlMemberType" style="width: 150px;">
                            <option value="0">无</option>
                            <option value="1">潜在客户</option>
                            <option value="2">意向客户</option>
                            <option value="3">待签约客户</option>
                            <option value="4">会员客户</option>
                        </select>--%>
                        <select class="easyui-combobox" id="ddlMemberType" style="width: 150px;">
                            <option value="0">无</option>
                            <option value="1">潜在客户</option>
                            <option value="2">意向客户</option>
                            <option value="3">待签约客户</option>
                            <option value="4">会员客户</option>
                        </select>
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        所属分组:
                    </td>
                    <td>
                        <span id="sp_group"></span>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        所属分组:
                    </td>
                    <td>
                        <select id="ddlUserAddOrEditGroup" class="easyui-combogrid" style="width: 150px;">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        姓名:
                    </td>
                    <td>
                        <input id="txtTrueName" type="text" class="easyui-validatebox" required="true" missingmessage="请输入姓名" />
                    </td>
                </tr>
                <tr>
                    <td>
                        性别:
                    </td>
                    <td>
                        <input type="radio" id="rdoman" name="rd" checked="checked" />
                        <label for='rdoman'>
                            男</label>
                        <input type="radio" id="rdowoman" name="rd" />
                        <label for='rdowoman'>
                            女</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        生日:
                    </td>
                    <td>
                        <input type="text" id="txtBirthday" class="easyui-datebox" required="true" validtype="datetime"
                            missingmessage="请输入正确的时间格式" invalidmessage="请输入正确的时间格式" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        手机:
                    </td>
                    <td>
                        <input type="text" id="txtMobile" class="easyui-validatebox" required="true" missingmessage="请输入手机号" />
                    </td>
                </tr>
                <tr>
                    <td>
                        电子邮箱:
                    </td>
                    <td>
                        <input type="text" id="txtEmail" />
                    </td>
                </tr>
                <tr>
                    <td>
                        QQ:
                    </td>
                    <td>
                        <input type="text" id="txtQQ" />
                    </td>
                </tr>
                <tr>
                    <td>
                        固定电话:
                    </td>
                    <td>
                        <input type="text" id="txtTel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        网址:
                    </td>
                    <td>
                        <input type="text" id="txtWebsite" />
                    </td>
                </tr>
                <tr>
                    <td>
                        公司:
                    </td>
                    <td>
                        <input type="text" id="txtCompany" />
                    </td>
                </tr>
                <tr>
                    <td>
                        职位:
                    </td>
                    <td>
                        <input type="text" id="txtTitle" />
                    </td>
                </tr>
                <%--<tr><td>用户状态:</td><td>
<input type="radio" id="rd1" name="rdstatu" checked="checked" />
                        <label for='rd1'>
                            启用</label>
                        <input type="radio" id="rd0" name="rdstatu" />
                        <label for='rd0'>
                            禁用</label>
</td></tr>--%>
                <%--<tr><td>微博用户ID:</td><td>
<input type="text" id="txtWeiboID" />

</td></tr>
<tr><td>微博用户昵称:</td><td>
<input type="text" id="txtWeiboScreenName" />

</td></tr>--%>
                <tr>
                    <td>
                        图片:
                    </td>
                    <td>
                        <input type="text" id="txtCardImageUrl" /><input type="button" id="btnupload" value="上传图片" />
                    </td>
                </tr>
                <tr>
                    <td>
                        地址:
                    </td>
                    <td>
                        <textarea id="txtAddress" style="width: 180px; height: 50px"></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        备注:
                    </td>
                    <td>
                        <textarea id="txtRemark" style="width: 180px; height: 50px"></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        微博ID:
                    </td>
                    <td>
                        <input type="text" id="txtWeiboID" />
                    </td>
                </tr>
                <tr>
                    <td>
                        微博昵称:
                    </td>
                    <td>
                        <input type="text" id="txtWeiboScreenName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        微信OPenID:
                    </td>
                    <td>
                        <input type="text" id="txtWeixinOpenID" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <h5>
                            拓展:</h5>
                    </td>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        第二个手机号:
                    </td>
                    <td>
                        <input type="text" id="txtMobile2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第三个手机号:
                    </td>
                    <td>
                        <input type="text" id="txtMobile3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第四个手机号:
                    </td>
                    <td>
                        <input type="text" id="txtMobile4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第二个微博ID:
                    </td>
                    <td>
                        <input type="text" id="txtWeiboID2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第三个微博ID:
                    </td>
                    <td>
                        <input type="text" id="txtWeiboID3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第四个微博ID:
                    </td>
                    <td>
                        <input type="text" id="txtWeiboID4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第二个微信OPenID:
                    </td>
                    <td>
                        <input type="text" id="txtWeixinOpenID2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第三个微信OPenID:
                    </td>
                    <td>
                        <input type="text" id="txtWeixinOpenID3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第四个微信OPenID:
                    </td>
                    <td>
                        <input type="text" id="txtWeixinOpenID4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第二个邮箱:
                    </td>
                    <td>
                        <input type="text" id="txtEmail2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第三个邮箱:
                    </td>
                    <td>
                        <input type="text" id="txtEmail3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        第四个邮箱:
                    </td>
                    <td>
                        <input type="text" id="txtEmail4" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                            保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                关 闭</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="win_group" class="easyui-window" modal="true" closed="true">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td>
                        分组名称:
                    </td>
                    <td>
                        <input type="text" id="txtGroupName" class="easyui-validatebox" required="true" missingmessage="请输入分组名称" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <a href="javascript:void(0)" id="btnSave_Group" class="easyui-linkbutton" iconcls="icon-ok">
                            保 存</a> <a href="javascript:void(0)" id="btnExit_Group" class="easyui-linkbutton"
                                iconcls="icon-no">关 闭</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="dlgBatchInsert" class="easyui-window" modal="true" closed="true">
        <div style="margin-left: 20px">
            <form enctype="multipart/form-data" name="formBatchInsert" id="formBatchInsert">
            <table>
                <tr>
                    <td class="style1">
                        文件路径:
                    </td>
                    <td>
                        <input type="file" name="BatchInsertFile" id="BatchInsertFile" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        所属分组:
                    </td>
                    <td>
                        <select id="ddlgroup" class="easyui-combogrid" style="width: 300px;">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        导入说明:
                    </td>
                    <td>
                        <font color="red">
                            <br />
                            1. 支持从Excel文件(*.xls)导入数据<br />
                            2. 支持Excel文件有多个Sheet<br />
                            3. Execl列名： “姓名”、“性别”、“手机”、“电子邮箱”、“电话”、<br />
                            “公司”、“职务”、“QQ”、“微博昵称”、“微博ID”、“地址”、<br />
                            “手机2”、“手机3”、“电子邮箱2”、“电子邮箱3”，无须顺序排列<br />
                            4. 最大支持10M文件大小<br />
                        </font>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        导入结果:
                    </td>
                    <td>
                        <span id="lbResultLog">[导入后可查看结果]</span>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                    </td>
                    <td>
                        <a id="btnDownloadBadData" target="_blank" style="color: Red; font-size: 18px; font-weight: bold;
                            cursor: pointer; text-decoration: underline;">点击下载错误数据文件</a>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        文档示例:
                    </td>
                    <td title="">
                        <img src="/img/memberuploadexecl.jpg" width="470px" />
                    </td>
                </tr>
            </table>
            </form>
        </div>
    </div>
    <div id="win_remind" class="easyui-window" modal="true" closed="true">
        <div style="margin-left: 20px">
            <table>
                <tr>
                    <td style="text-align: right">
                        主题:
                    </td>
                    <td>
                        <input type="text" id="txtTheme" style="width: 250px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入主题" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <input type="radio" name="rdgroup" id="rd_phone" checked="checked" /><label for="rd_phone"
                            onclick="SetThemeText(this)">打电话</label>
                        <input type="radio" name="rdgroup" id="rd_sms" /><label for="rd_sms" onclick="SetThemeText(this)">发短信</label>
                        <input type="radio" name="rdgroup" id="rd_email" /><label for="rd_email" onclick="SetThemeText(this)">发邮件</label>
                        <input type="radio" name="rdgroup" id="rd_other" /><label for="rd_other" onclick="SetThemeText(this)">其它</label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        备注：
                    </td>
                    <td>
                        <textarea id="txtContent" style="width: 250px; height: 100px;"></textarea>
                    </td>
                </tr>
                <%--<tr><td><input type="checkbox" id="cb_definitetime" /><label for="cb_definitetime">定时提醒:</label></td><td>

<select id="ddlRemindTime" >
<option>每天同一时刻</option>
<option>指定时刻</option>
<option>每周提醒</option>

</select>

</td></tr>--%>
                <tr>
                    <td style="text-align: right">
                        提醒时间:
                    </td>
                    <td>
                        <input type="text" id="txtRemindTime" class="easyui-datetimebox" required="true"
                            validtype="datetime" missingmessage="请输入正确的时间格式" invalidmessage="请输入正确的时间格式"
                            style="width: 250px;" />
                    </td>
                </tr>
                <%--<tr><td style="text-align:right">日期:</td><td><input id="txtRemindDate" style="width:200px" class="easyui-datebox" required="true"
                            validtype="datetime" missingmessage="请输入正确的日期格式" invalidmessage="请输入正确的日期格式" /></td></tr>--%>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <a href="javascript:void(0)" id="btnSave_Remind" class="easyui-linkbutton" iconcls="icon-ok">
                            保 存</a> <a href="javascript:void(0)" id="btnExit_Remind" class="easyui-linkbutton"
                                iconcls="icon-no">关 闭</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="win_viewimage" class="easyui-window" modal="true" closed="true" style="padding: 10px;
        text-align: center;">
        <div style="text-align: left" id="div_image">
        </div>
    </div>
    <div id="dlgDeleteGroup" class="easyui-dialog" title="系统提示" style="width: 280px;
        height: 180px; padding: 10px; margin-left: 20px" modal="true" closed="true">
        <table>
            <tr>
                <td>
                    <img alt="" src="/img/help/messager_question.gif" />
                </td>
                <td>
                    确定删除选中分组？
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input type="checkbox" id="chkIsDeleteDataForGroup" />
                </td>
                <td>
                    <label for="chkIsDeleteDataForGroup">
                        同时删除选中组下的数据？</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgFilterByGroup" class="easyui-dialog" title="单击选中行按分组筛选" style="width: 450px;
        height: 410px;" closed="true">
        <table id="grvGroupFilterData" cellspacing="0" cellpadding="0" fitcolumns="true">
            <thead>
                <tr>
                    <th field="ck" width="10" checkbox="true">
                    </th>
                    <th field="GroupName" width="100">
                        分组名
                    </th>
                </tr>
            </thead>
        </table>
    </div>
    <div id="dlgGroupMgr" class="easyui-dialog" title="分组管理" style="width: 450px; height: 400px;"
        closed="true">
        <table id="grvGroupData" cellspacing="0" cellpadding="0" fitcolumns="true">
            <thead>
                <tr>
                    <th field="ck" width="10" checkbox="true">
                    </th>
                    <th field="GroupName" width="100">
                        分组名
                    </th>
                </tr>
            </thead>
        </table>
    </div>
    <div id="dlgSendSms" class="easyui-dialog" title="发送短信" style="width: 350px; height: 150px;
        padding: 10px; margin-left: 20px" closed="true">
        发送短信：<select id="ddlSendSMSType">
            <option value="1">向勾选号码发送短信</option>
            <option value="0">向筛选号码发送短信</option>
        </select>
        <a href="#" class="easyui-linkbutton" id="btnSendSMS">确认发送</a>
    </div>
</asp:Content>
