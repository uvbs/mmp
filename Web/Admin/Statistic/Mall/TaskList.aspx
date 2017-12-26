<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statistic.Mall.TaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #txtFromDate, #txtToDate {
            width: 80px;
        }
        .blue {
            color: blue;
        }
        #btnNotSelectDistributionUser {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>统计</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="showAdd();" id="btnAdd">添加任务</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="deleteTask()">删除任务</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="editTask()">编辑任务</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 450px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td style="width: 50px;">时间:
                </td>
                <td>
                    <input id="txtFromDate" type="text" class="easyui-datebox" />至<input id="txtToDate" type="text" class="easyui-datebox" />
                </td>
            </tr>
            <tr>
                <td>渠道:
                </td>
                <td>
                    <select id="ddlChannel">
                        <% Response.Write(string.Format("<option value=\"{0}\">{1}</option>", "", "不指定"));
                           foreach (var item in AllChannelList)
                           {
                               Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.Value, item.Text));

                           }%>
                    </select>
                </td>
            </tr>
            <tr>
                <td>分销员:
                </td>
                <td>
                    <label id="lblDistributionName"></label>
                    <a id="btnSelectDistributionUser" class="easyui-linkbutton">选择分销员</a>
                    <a id="btnNotSelectDistributionUser" class="blue">不指定分销员</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgChannel" class="easyui-dialog" closed="true" title="选择渠道" style="width: 370px; padding: 15px; line-height: 30px;">
        <table id="grvChannelData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgDistribution" class="easyui-dialog" closed="true" title="选择分销员" style="width: 500px; height: 420px; padding: 15px; line-height: 30px;">
        <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px;"
            placeholder="姓名,昵称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="searchDistributionUser();">查询</a>
        <table id="grvDistributionData" fitcolumns="true">
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/Mall/Statistics/Task/";//接口地址
        var channelUserId = "";//渠道账户
        var distributionUserId = "";//分销账户
        var isSumbit = false;//是否正在提交


        var action = "";
        var id = '';
        $(function () {

            //任务列表
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "List.ashx",
                       height: document.documentElement.clientHeight - 112,
                       //loadFilter:function(data){
                       //    var value = {
                       //        total: data.result.totalcount,
                       //        rows: data.result.list
                       //    };
                       //    return value;

                       //},
                       onBeforeLoad: function (p) {
                           p.pageindex = p.page;
                           p.pagesize = p.rows;
                           return true;
                       },
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: false,
                       columns: [[
                                    { title: 'ck', width: 5, checkbox: true },
                                  {
                                      field: 'time_range', title: '统计时间', width: 30, align: 'left', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          str.AppendFormat('{0}&nbsp;至&nbsp;{1}', rowData.from_date, rowData.to_date);
                                          return str.ToString();
                                      }
                                  },
                                   { field: 'channel_name', title: '渠道', width: 20, align: 'left' },
                                   { field: 'distribution_name', title: '分销员', width: 20, align: 'left' },
                                   { field: 'insert_date', title: '提交任务时间', width: 15, align: 'left' },
                                   { field: 'status_str', title: '任务状态', width: 20, align: 'left' },
                                   {
                                       field: 'operate', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           if (rowData.status == 3) {
                                               str.AppendFormat('<a class="blue" href="Order/Result.aspx?taskid={0}" target="_blank">销售总计<a>&nbsp;<a class="blue" href="Product/Result.aspx?taskid={0}" target="_blank">商品明细<a>', rowData.task_id);
                                           }
                                           return str.ToString();
                                       }
                                   }

                       ]]
                   }
               );

            //分销员列表
            $('#grvDistributionData').datagrid(
                   {
                       method: "Post",
                       url: "/Handler/App/CationHandler.ashx",
                       queryParams: { Action: "QueryWebsiteUserDistributionOnLine", AddSystemUser: 1 },
                       height: 300,
                       //loadFilter: function (data) {

                       //    var defaultModel = new {
                       //        WXHeadimgurl: "",
                       //        WXNickname: "",
                       //        TrueName: "",
                       //        Phone: "",
                       //        UserID:"system",

                       //    }
                       //    var value = {
                       //        total: data.total,
                       //        rows: data.rows.push(defaultModel)
                       //    };
                       //    return value;

                       //},
                       singleSelect: true,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[


                                   { title: 'ck', width: 5, checkbox: true },
                                   {
                                       field: 'WXHeadimgurl', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                           if (value == '' || value == null)
                                               return "";
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                           return str.ToString();
                                       }
                                   },
                                { field: 'WXNickname', title: '微信昵称', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '姓名', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'Phone', title: '手机', width: 80, align: 'left', formatter: FormatterTitle }



                       ]]
                   }
               );

            //任务对话框操作
            $('#dlgInput').dialog({
                buttons: [{
                    text: '提交任务',
                    handler: function () {
                        try {

                            var fromDate = $("#txtFromDate").datebox('getValue');
                            var toDate = $("#txtToDate").datebox('getValue');
                            var dataModel = {
                                from_date: fromDate,
                                to_date: toDate,
                                channel_user_id: $("#ddlChannel").val(),
                                distribution_user_id: distributionUserId,
                                id:id

                            }

                            

                            if (dataModel.from_date == '') {

                                Alert('请选择开始日期');
                                return;
                            }
                            if (dataModel.to_date == '') {

                                Alert('请选择结束日期');
                                return;
                            }
                            if (isSumbit) {
                                alert("重复提交");
                                return false;
                            }
                            isSumbit = true;

                            var url = '';
                            if (action == 'edit') {
                                url = handlerUrl + "Update.ashx";
                            } else {
                                url = handlerUrl + "Add.ashx";
                            }
                            $.ajax({
                                type: 'post',
                                url: url,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status) {

                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
                                    }
                                    isSumbit = false;

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

            //分销对话框操作
            $('#dlgDistribution').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $('#grvDistributionData').datagrid('getSelections');
                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }
                        distributionUserId = rows[0].UserID;
                        $("#lblDistributionName").html(rows[0].TrueName);
                        $("#btnNotSelectDistributionUser").show();
                        $('#dlgDistribution').dialog('close');

                    }
                },
                //{
                //    text: '不指定',
                //    handler: function () {
                //        distributionUserId = "";
                //        $("#lblDistributionName").html("");
                //        $('#dlgDistribution').dialog('close');

                //    }
                //},

                {
                    text: '关闭',
                    handler: function () {
                        $('#dlgDistribution').dialog('close');
                    }
                }]
            });



            //选择分销员
            $("#btnSelectDistributionUser").click(function () {

                $('#dlgDistribution').dialog({ title: '选择分销员' });
                $('#dlgDistribution').dialog('open');

            })
            //不指定分销员
            $("#btnNotSelectDistributionUser").click(function () {

                distributionUserId = "";
                $(lblDistributionName).html("不指定");
                $("#btnNotSelectDistributionUser").hide();

            })

            //定时刷新
            setInterval("$('#grvData').datagrid(\"reload\");", 10000);




        });

        //显示提交任务对话框
        function showAdd() {
            action = "add";
            id = "";
            $('#dlgInput').dialog({ title: '提交任务' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");
            distributionUserId = "";
            $(lblDistributionName).html("");
            $("#btnNotSelectDistributionUser").hide();


        }

        function editTask() {
            action = "edit";

            var rows = $('#grvData').datagrid('getSelections');


            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            id = rows[0].id;

            $('#dlgInput').dialog({ title: '编辑任务' });
            $('#dlgInput').dialog('open'); 
            $("#txtFromDate").datebox('setValue', new Date(rows[0].from_date).format("yyyy-MM-dd"));
            $("#txtToDate").datebox('setValue', new Date(rows[0].to_date).format("yyyy-MM-dd"));


            distributionUserId = rows[0].distribution_user_id;
            $('#lblDistributionName').text(rows[0].distribution_name);

            $('#ddlChannel').val(rows[0].channel_user_id);
        }

        //搜索分销员
        function searchDistributionUser() {

            $('#grvDistributionData').datagrid(
        {
            method: "Post",
            url: "/Handler/App/CationHandler.ashx",
            queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $("#txtKeyWord").val(), AddSystemUser: 1 }
        });

        }

        //删除任务
        function deleteTask() {
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
                            url: handlerUrl + "Delete.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                    Alert(resp.msg);

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
