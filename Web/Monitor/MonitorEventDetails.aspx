<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="MonitorEventDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Monitor.MonitorEventDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/Monitor/MonitorHandler.ashx";
        var planId = '<%=this.ViewState["planId"].ToString() %>';
        var eventType = '<%=this.ViewState["eventType"].ToString() %>';
        var linkid  = '<%=this.ViewState["linkid"].ToString() %>';
        var ispostback = false;
        $(function () {
            $(grvData).datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryPlanEventDetail", planId: planId, eventType: eventType,linkId:linkid },
	                height: 520,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
                    singleSelect: true,
                    columns: [[ <%=strGrid.ToString() %>]]
	            });
               



            $(btnExportToFile).click(function () {
                DownLoadData();
            });

            $(btnShowSet).click(function () {

                if(!ispostback)
                {
                    $('#filedsetMember input').each(function(){
                        this.checked = true;
                    });
                    ispostback = true;
                }
                $(dlgShowSet).dialog('open');
            });

            //弹出导入框---------------------
            $("#btnImport").bind("click", function () {
                ShowBatchInsertEmailAddress();
            });
            //弹出导入框---------------------

            //取消---------------------
            $("#win_batchinsert").find("#btnExit_BatchInsert").bind("click", function () {
                $("#win_batchinsert").window("close");
            });
            //取消---------------------

            $("#rdo1").bind("click", function () {
                $("#txtNewGroup").hide();
            });

            $("#rdo0").bind("click", function () {
                $("#txtNewGroup").show();
                $("#txtNewGroup").focus();
            });
            //批量导入邮箱---------------------
            $("#btnSave_BatchInsert").bind("click", function () {
                BatchInsertEmailAddress();
            });
            //批量导入邮箱---------------------

            $(dlgShowSet).dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        //读取选中项
                        var isHaveMemberInfo = 0;
                        //var strColumns = new StringBuilder();
                        var arr = new Array();

                        $('#filedsetMember input').each(function(){
                            if( this.checked == true)
                            {
                                //strColumns.AppendFormat("{ field: '{0}', title: '{1}', width: 100, align: 'left' },",$(this).val(),$(this).attr('text'));
                                arr.push("{ field: '"+ $(this).val() +"', title: '"+ $(this).attr('text') +"', width: 100, align: 'left' }");
                                isHaveMemberInfo = 1;
                            }
                        });

                    $('#filedsetEmailState input').each(function(){
                            if( this.checked == true)
                            {
                                //strColumns.AppendFormat("{ field: '{0}', title: '{1}', width: 100, align: 'left' },",$(this).val(),$(this).attr('text'));
                                arr.push("{ field: '"+ $(this).val() +"', title: '"+ $(this).attr('text') +"', width: 100, align: 'left' }");
                            }
                        });

                        var strColumns = '[[' + arr.join(',') + ']]';

                        //alert(strColumns);

                        $(grvData).datagrid(
                        {
                            columns:eval(strColumns),
                            data:{Action:"QueryEmailEventDetails", planId: planId, eventType: eventType,isHaveMemberInfo:isHaveMemberInfo }
                        });

                        //更改grid结构并重载数据
                        $(dlgShowSet).dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $(dlgShowSet).dialog('close');
                    }
                }

                ]
            });

            
           

        });

        function Search(o) {
            var serachtype = $(o).attr('serachtype');
            var schar = $(o).html();
            if (serachtype == 'showall') {
                $(grvData).datagrid(
	            {
	                queryParams: { Action: "QueryEmailEventDetails", planId: planId, eventType: eventType }
	            });
            }
            else {
                $(grvData).datagrid(
	            {
	                queryParams: { Action: "QueryEmailEventDetails", planId: planId, eventType: eventType, schar: schar }
	            });
            }
        }

        function DownLoadData() {
            $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                if (o) {
                    window.open(handlerUrl + '?Action=DownLoadEmailEventDetails&planId=' + planId + '&eventType=' + eventType);
                }
            });
        }

        //加载邮箱分组下拉框
        function LoadEmailAddressGroup(spanid, selectid) {

            jQuery.ajax({
                type: "Post",
                url: handlerUrl,
                data: { Action: "GetEmailAddressGroupList", ID: selectid },
                dataType: "html",
                async: false,
                success: function (data) {
                    $("#" + spanid).html(data);
                }
            })

        }
        //加载邮箱分组下拉框

        //显示批量导入弹出框
        function ShowBatchInsertEmailAddress() {

            //弹出对话框
            $("#win_batchinsert").window({
                title: "导出到收件人列表",
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
//                iconCls: "icon-add",
                resizable: false,
                width: 450,
                height: 200,
                top: ($(window).height() - 250) * 0.5,
                left: ($(window).width() - 450) * 0.5

            });
            //弹出对话框
            $("#txtNewGroup").val("");



        }


        //批量导入电子邮箱地址
        function BatchInsertEmailAddress() {
            var type = 0; //导入类型 0代表导入新建列表 1代表导入已有列表
            var NewGroupName = $("#txtNewGroup").val();
            var msg = "";
            if (rdo0.checked) {//导入新建列表
                if ($.trim(NewGroupName) == "") {
                    $("#txtNewGroup").focus();
                    return false;
                }
                msg = "正在将邮箱导入 " + NewGroupName + "..."

            }
            if (rdo1.checked) {//导入已有列表
                type = 1;
                msg = "正在将邮箱导入 " + $("#ddlEmailAddressGroup option:selected").text() + "..."
            }

            $("#win_batchinsert").window("close");

            $.messager.progress({
                title: '请稍候',
                msg: msg
            });
            try {


                $.post(handlerUrl, { Action: "ImportEmailAddressFromEventDetails", planId: planId, eventType: eventType, GroupID: $(ddlEmailAddressGroup).val(), ImportType: type, NewGroupName: NewGroupName }, function (result) {
                    $.messager.progress('close');
                    $.messager.alert("系统提示", result, "info");
                    if (type == 0) {
                        LoadEmailAddressGroup("sp_group", "ddlEmailAddressGroup"); //加载导入邮箱框分组下拉框
                    }

                }, "text");

            } catch (e) {
                alert(e);
            }

        }
        //批量导入电子邮箱地址


    </script>
    <style type="text/css">
        .ShowSetTable td
        {
            width: 80px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：监测平台&nbsp;<span>详细</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="pageTopBtnBg">
        <table width="100%">
            <tr>
                <td colspan="2">
                    <%--<a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-setting'"
                                id="btnShowSet">选择列</a>
                    --%>
                    <%--<a style="float: right;" id="btnReturn" href="/Monitor/MonitorLink.aspx?id=<%=this.ViewState["planId"]%>"
                                    class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>--%>
                   
                    <a style="float: right;" href="javascript:history.go(-1);" class="easyui-linkbutton"
                        iconcls="icon-back" plain="true">返回上一页</a>
                </td>
            </tr>
            <%-- <tr>
                <td style="font-size: 12px; width: 150px;" align="right">
                    首字符：
                </td>
                <td style="font-size: 12px; padding-left: 15px; width: *;" align="left">
                    <%=strSearchHtml.ToString() %>
                    <a href="javascript:;" serachtype="showall" onclick="Search(this);" style="text-decoration: underline;">
                        查看全部</a>
                </td>
            </tr>--%>
        </table>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgShowSet" class="easyui-dialog" closed="true" modal="true" title="高级显示"
        style="width: 460px; height: 400px; padding: 5px;">
        <table width="100%">
            <tr>
                <td align="left">
                    <br />
                    选择要显示的列
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <fieldset id="filedsetEmailState">
                        <legend>选择列</legend>
                        <table width="100%" class="ShowSetTable">
                            <tr>
                                <td align="left">
                                    <input type="checkbox" value="SourseIP" id="chkSourseIP" text="IP地址" /><label for="chkSourseIP">IP地址</label>
                                </td>
                                <td align="left">
                                    <input type="checkbox" value="EventBrowserID" id="chkEventBrowserID" text="浏览器版本" /><label
                                        for="chkEventBrowserID">浏览器版本</label>
                                </td>
                                <td align="left">
                                    <input type="checkbox" value="EventDate" id="chkEventDate" text="触发时间" /><label for="chkEventDate">触发时间</label>
                                </td>
                                <td align="left">
                                    <%if (this.ViewState["eventType"].ToString() == "1")
                                      { %>
                                    <input type="checkbox" value="ClickUrl" id="chkClickUrl" text="点击URL" /><label for="chkClickUrl">点击URL</label>
                                    <%} %>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
