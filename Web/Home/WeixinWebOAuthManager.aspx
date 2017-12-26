<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WeixinWebOAuthManager.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.WeixinWebOAuthManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>微信网页授权</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div>
        <div class="center" style="margin: 0px;">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
                <div style="margin-bottom: 5px">
                    <div>
                        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAddOrEdit('add')">
                            添加</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">
                                编辑</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                                    删除</a>
                    </div>
                            <div>
            <span style="font-size:12px;font-weight:normal">路径：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
                </div>
            </div>
            <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        </th>
                        <th field="PagePath" width="50">
                            页面路径关键字
                        </th>
                        <th field="MatchType" width="50" formatter="MatchTypeFormat">
                            匹配类型
                        </th>
                         <th field="Ex1" width="50">
                            授权方式
                        </th>
                        <th field="FilterDescription" width="50">
                            说明
                        </th>
                    </tr>
                </thead>
            </table>
            <div id="win" class="easyui-window" modal="true" closed="true" style="padding: 10px;width:300px;height:270px;">
                
                <table style="margin: auto;">
                    <tr>
                        <td align="left" class="style1">
                            页面路径关键字:
                        </td>
                        <td style="text-align: left">
                            <input type="text" id="txtPagePath" style="width: 200px;" class="easyui-validatebox"
                                required="true" missingmessage="请输入页面路径" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style1">
                            匹配类型:
                        </td>
                        <td style="text-align: left">
                            <select class="easyui-combobox" style="width: 200px;" editable="false" id="selectMatchType">
                                <option value="contains">包含匹配</option>
                                <option value="all">全文匹配</option>
                                <option value="start">开头匹配</option>
                                <option value="end">结尾匹配</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style1">
                            授权方式:
                        </td>
                        <td style="text-align: left">
                            <select class="easyui-combobox" style="width: 200px;" editable="false" id="selectEx1">
                                <option value="snsapi_base">snsapi_base</option>
                                <option value="snsapi_userinfo">snsapi_userinfo</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style1">
                            说明:
                        </td>
                        <td style="text-align: left">
                            <textarea id="txtFilterDescription" style="width: 200px; height: 80px"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                        <td align="right">
                        <br />
                            <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" >
                                保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" >
                                    关 闭</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
  <script type="text/javascript">
        var grid;
        //处理文件路径
        var url = "/Handler/Permission/ModuleFilterInfoManage.ashx";
        //加载文档
        jQuery().ready(function () {

//            $(window).resize(function () {
//                $(grvData).datagrid('resize',
//	            {
//	                height: document.documentElement.clientHeight - 45
//	            });
//            });

            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: url,
                height: document.documentElement.clientHeight - 112,
                
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "Query", FilterType:  "WXOAuth " }
            });
            //------------加载gridview

            //取消---------------------
            $("#win").find("#btnExit").bind("click", function () {
                $("#win").window("close");
            });
            //取消---------------------


            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var SearchTitle = $("#txtName").val();
                grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: SearchTitle, FilterType: "WXOAuth "} });
            });
            //搜索结束---------------------

            //保存---------------------
            $("#btnSave").bind("click", function () {

                //               var tag = jQuery.trim(jQuery(this).attr("tag"));

                //               if (tag == "add") {
                //                   //添加
                //                   Add();
                //                   return;
                //               }
                //               //修改
                Save();
            }); //保存---------------------

        });

        //弹出添加或编辑框开始
        function ShowAddOrEdit(addoredit) {
            var title = ""; //弹出框标题
            var titleicon = "icon-" + addoredit; //弹出框标题图标
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                Clear("txtPagePath|txtFilterDescription");
                //设置默认值
                $("#rdlogin").attr("checked", true);
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


                //加载信息开始
                $("#txtPagePath").val(rows[0].PagePath);
                $("#txtFilterDescription").val(rows[0].FilterDescription);

                $('#selectMatchType').combobox('setValue', rows[0].MatchType);
                $('#selectEx1').combobox('setValue', rows[0].Ex1);

                //                var FilterType = rows[0].FilterType;
                //                if (FilterType == "login") {
                //                    $("#rdlogin").attr("checked", true);


                //                } else if (FilterType == "pms") {
                //                    $("#rdpms").attr("checked", true);
                //                }               //加载信息结束

                //设置弹出框标题
                title = "编辑";


            }
            //弹出编辑框结束


            //弹出对话框
            $("#win").window({
                title: title,
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: titleicon,
                resizable: false,
                top: ($(window).height() - 200) * 0.5,
                left: ($(window).width() - 370) * 0.5

            });
            //弹出对话框

            //设置保存按钮属性 add为添加，edit为编辑
            $("#btnSave").attr("tag", addoredit);


        }
        //展示添加或编辑框结束


        //添加或编辑操作开始---------
        function Save() {
            //alert(1);
            var PagePath = $("#txtPagePath").val();
            var FilterDescription = $("#txtFilterDescription").val();
            var FilterType = "WXOAuth";
            //            if ($("#rdlogin").attr("checked")) {
            //                FilterType = "login";


            //            } else if ($("#rdpms").attr("checked")) {
            //                FilterType = "pms";
            //            }

            var MatchType = $('#selectMatchType').combobox('getValue');
            var Ex1 = $('#selectEx1').combobox('getValue');

            //            alert(MatchType);
            //            alert(Ex1);

            //--检查输入---------------
            if (PagePath == "") {
                $("#txtPagePath").focus();
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
                    data: { Action: "Add", PagePath: PagePath, FilterDescription: FilterDescription, FilterType: "WXOAuth", MatchType: MatchType, Ex1: Ex1 },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "添加成功");
                            $("#win").window("close");
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
                var rows = grid.datagrid('getSelections');
                var OldPagePath = rows[0].PagePath;
                var OldFilterType = rows[0].FilterType;
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "Edit", PagePath: PagePath, FilterDescription: FilterDescription, FilterType: FilterType, OldPagePath: OldPagePath, OldFilterType: OldFilterType, MatchType: MatchType, Ex1: Ex1 },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "修改成功");
                            $("#win").window("close");
                            grid.datagrid('reload');
                        }
                        else {
                            messager("系统提示", result);
                        }
                    }
                });
                //修改
            }
            //--------------执行编辑操作结束

        }
        //添加或编辑操作结束---------




        // 删除---------------------
        function Delete() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要删除的记录");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].PagePath);
            }

            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Delete", id: ids.join(',') },
                        success: function (result) {
                            if (result) {
                                messager('系统提示', "删除成功！");
                                grid.datagrid('reload');

                                return;
                            }
                            $.messager.alert("删除失败。");
                        }
                    });
                }
            });
        };
        // 删除---------------------

        //       //清除数据-----------

        function operate(value, row, index) {


            return "<font>文本</font>";



        }

        function MatchTypeFormat(value) {
            switch (value) {
                case 'all':
                    return "全文匹配";
                case 'start':
                    return "开头匹配";
                case 'end':
                    return "结尾匹配";
                case 'contains':
                    return "包含匹配";
                default:
                    return "";
            }
        }


 
    </script>
</asp:Content>