<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMassArticleMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.WXMassArticleMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;群发&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>群发图文素材</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="WXMassArticleAddEdit.aspx" class="easyui-linkbutton" iconcls="icon-add2"
                plain="true">添加图文素材</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete"
                    plain="true" onclick="Delete()">批量删除</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                        iconcls="icon-send" plain="true" onclick="ShowSelectUser()">预览</a> <a href="javascript:void(0)"
                            class="easyui-linkbutton" iconcls="icon-send" plain="true" onclick="SendMassMessageNews()">
                            群发（服务号每月4条,订阅号每天1条）</a>
            <input type="checkbox" name="timingCheck" id="timingCheck" value="0" onclick="CheckTiming(this)" />
            <label for="timingCheck">
                定时群发</label>
            <span id="dtbox" dispaly="none">
                <input class="easyui-datetimebox" style="width: 150px;" id="timingSelector" />
            </span>
            <br />
            <label style="margin-left: 8px;">
                关键词查找</label>
            <input type="text" id="txtKeyWord" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgUser" class="easyui-dialog" closed="true" modal="true" title="选择用户" style="width: 450px;
        height: ">
        <div>
            姓名&nbsp;<input type="text" id="txtTrueName" style="width: 300px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" id="btnSearchUser">搜索</a>
        </div>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var grid;
        $(function () {

            grid = $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMassArticle" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ThumbImage', title: '缩略图', width: 10, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="100" />', value);
                                    return str.ToString();
                                }
                                },
                            { field: 'Title', title: '标题', width: 20, align: 'center' },
                            { field: 'Author', title: '作者', width: 10, align: 'center', formatter: FormatterTitle },
                            { field: 'Content_Source_Url', title: '原文链接', width: 10, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="{0}" target="_blank">{0}</a>', value);
                                return str.ToString();
                            }
                            },
                        { field: 'Sort', title: '排序', width: 10, align: 'left', formatter: function (value, rowData) {
                            var newvalue = "";
                            if (value != null) {
                                newvalue = value;
                            }
                            var str = new StringBuilder();
                            str.AppendFormat('<input type="text" value="{0}" id="txtArticle{1}" style="width:50px;" maxlength="5" > <a title="点击保存排序号"  onclick="UpdateSortIndex({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.AutoID);
                            return str.ToString();
                        }
                        },
                            { field: 'Operate', title: '操作', width: 10, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改" href="WXMassArticleAddEdit.aspx?id={0}">编辑</a>', rowData.AutoID);
                                return str.ToString();
                            }
                            }
                             ]]
	            }
            );
            $("#dtbox").hide();

            //选择用户弹框
            $('#dlgUser').dialog({
                buttons: [{
                    text: '发送预览',
                    handler: function () {
                        try {
                            var rows = grid.datagrid('getSelections');
                            var ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].AutoID);
                            }
                            var rowsUser = $("#grvUserInfo").datagrid('getSelections');
                            if (rowsUser.length == 0) {
                                Alert("请选择用户");
                                return;
                            }

                            $.messager.confirm("系统提示", "是否给选中的用户发送预览?", function (r) {
                                if (r) {

                                    jQuery.ajax({
                                        type: "Post",
                                        url: handlerUrl,
                                        data: { Action: "SendMassMessageNewsPreview", ids: ids.join(','), userAutoId: rowsUser[0]["AutoID"] },
                                        dataType: "json",
                                        success: function (resp) {
                                            Alert(resp.Msg);
                                        }
                                    });
                                }
                            });



                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUser').dialog('close');
                    }
                }]
            });

            //用户列表框
            $('#grvUserInfo').datagrid(
                  {

                      method: "Post",
                      height: 280,
                      url: handlerUrl,
                      queryParams: { Action: "QueryWebsiteUser" },
                      pagination: true,
                      striped: true,
                      singleSelect: true,
                      pageSize: 10,
                      rownumbers: true,
                      columns: [[
                                  { title: 'ck', width: 5, checkbox: true },
                                  { field: 'TrueName', title: '姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                      ]]

                  }
              );

            //搜索用户
            $("#btnSearchUser").click(function () {
                $('#grvUserInfo').datagrid({ url: handlerUrl, queryParams: { Action: 'QueryWebsiteUser', KeyWord: $("#txtTrueName").val()} });
            });


        });



        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中素材?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWXMassArticle", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            Alert(resp.Msg);
                            if (resp.Status == 1) {
                                $('#grvData').datagrid('reload');
                            }
                            else {

                            }

                        }

                    });
                }
            });


        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID
                 );
            }
            return ids;
        }

        function Search() {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMassArticle", KeyWord: $("#txtKeyWord").val() }
	            });
        }

        //群发 群发接口---------------------
        function SendMassMessageNews() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0 || num > 10) {
                Alert("请选择您要群发的素材，最多可以同时选择10条");
                return;
            }
            if ($('#timingCheck').attr("checked") == "checked" && $('#timingSelector').datetimebox('getValue') == "") {
                Alert("请选择发送时间");
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);
            }
            $.messager.confirm("系统提示", "确定群发选中素材?", function (r) {
                if (r) {

                    jQuery.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "SendMassMessageNews", ids: ids.join(','), isTiming: $('#timingCheck').attr("checked"), time: $('#timingSelector').datetimebox('getValue') },
                        dataType: "json",
                        success: function (resp) {
                            Alert(resp.Msg);
                        }
                    });
                }
            });
        };

        //检查时间框是否显示
        function CheckTiming(obj) {
            if (obj.checked) {
                $('#dtbox').show();
            }
            else {
                $('#dtbox').hide();
            }
        }

        //更新排序号
        function UpdateSortIndex(articleid) {
            var sortindex = $("#txtArticle" + articleid).val();
            if ($.trim(sortindex) == "") {
                $("#txtArticle" + articleid).focus();
                return false;
            }
            //    var re = /^[1-9]+[0-9]*]*$/;
            //    if (!re.test(sortindex)) {
            //        alert("请输入正整数");
            //        $("#txtArticle" + articleid).val("");
            //        $("#txtArticle" + articleid).focus();
            //        return false;
            //    }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateWXMassArticleSortIndex", AutoID: articleid, SortIndex: sortindex },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        Show(resp.Msg);
                        $('#grvData').datagrid("reload");
                    }
                    else {
                        Alert(resp.Msg);
                    }


                }
            });


        }

        //显示弹出用户框
        function ShowSelectUser() {

            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0 || num > 10) {
                Alert("请选择您要群发的素材，最多同时选择10条");
                return;
            }
            $('#dlgUser').dialog('open');


        }

    </script>
</asp:Content>
