<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SupplierChannelList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.SupplierChannelList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        textarea {
            width:95%;
        }
        .red {

            color:red;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;渠道分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商户渠道管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
                onclick="ShowAdd();">增加</a>
              <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEdit();">编辑</a>
             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete();">删除</a>
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block;padding: 6px;display:none;"
placeholder="渠道名称" />
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();" style="display:none;">
                查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>





      <div id="dlgChannelInfo" class="easyui-dialog" closed="true" title="渠道" style="width: 400px;
        padding: 15px;">

           <table width="100%">
               <tr class="accountUserID">
                <td>渠道名称<span class="red">*</span>
                </td>
                <td>
                    <input id="txtChannelName" type="text" style="width: 90%;" placeholder="必填"/>
                </td>
            </tr>
            <tr class="accountUserID">
                <td>渠道说明
                </td>
                <td>
                   
                    <textarea id="txtDesc" rows="3"></textarea>
                </td>
            </tr>
              
               <tr class="accountUserID">
                <td>等级
                </td>
                <td>
                    <select id="ddlLevel">
                        <%
                            foreach (var item in LevelList)
                          {
                              Response.Write(string.Format("<option value=\"{0}\">{1}</option>",item.AutoId,item.LevelString));
                              
                          } %>

                    </select>
                </td>
            </tr>
            <tr>
                <td>联系人姓名
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 90%;" />
                </td>
            </tr>
             <tr>
                <td>联系手机
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>公司名称
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>职位
                </td>
                <td>
                    <input id="txtPosition" type="text" style="width: 90%;" />
                </td>
            </tr>

            <tr>
                <td>邮箱
                </td>
                <td>
                    <input id="txtEmail" type="text" style="width: 90%;" />
                </td>
            </tr>
            </table>
        
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = "";
        var selectAutoId = "";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QuerySupplierChannel" },
	                height: document.documentElement.clientHeight - 100,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                //{ title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '渠道ID', width: 50, align: 'left' },
                                { field: 'ChannelName', title: '渠道名称', width: 100, align: 'left' },
                                { field: 'Description', title: '渠道描述', width: 100, align: 'left' },
                                { field: 'LevelName', title: '等级', width: 100, align: 'left', formatter: FormatterTitle },
                                {
                                    field: 'FirstLevelDistributionCount', title: '商户数量', width: 100, align: 'center',
                                    formatter: function (value, row) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a  class="listClickNum" title="点击查看" href="ChildChannelSupplier.aspx?parentChannel={0}">{1}</a>', row["UserID"], value);
                                        return str.ToString();

                                    }
                                },
                                {
                                    field: 'DistributionSaleAmountAll', title: '累计销售', width: 100,  align: 'center', formatter: function (value, row) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a   title="点击查看销售" >{1}</a>', row['AutoID'], value);
                                        return str.ToString();

                                    }
                                }
                                //,
	                            //{ field: 'HistoryDistributionOnLineTotalAmountEstimate', title: '累计奖励（预估）', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'OverCanUseAmount', title: '已提现奖励', width: 100, align: 'center', formatter: FormatterTitle },
                                //{ field: 'CanUseAmount', title: '可提现奖励', width: 100, align: 'center', formatter: FormatterTitle }







	                ]]
	            });






            //渠道添加编辑
            $('#dlgChannelInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        dataModel = {
                            Action: currAction,
                            AutoId: selectAutoId,
                            ChannelName: $(txtChannelName).val(),
                            Description: $(txtDesc).val(),
                            ///ParentChannel: $(ddlParentChannel).val(),
                            TrueName: $(txtTrueName).val(),
                            Company: $(txtCompany).val(),
                            Position: $(txtPosition).val(),
                            Phone: $(txtPhone).val(),
                            Email: $(txtEmail).val(),
                            ChannelLevelId: $(ddlLevel).val()
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == true) {
                                    Alert("操作成功");
                                   
                                    $('#dlgChannelInfo').dialog('close');
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
                        $('#dlgChannelInfo').dialog('close');
                    }
                }]
            });




            //load
        });

        //搜索
        function Search() {

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QuerySupplierChannel", keyword: $(txtKeyWord).val() }
                    });
        }


        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);

            }
            return ids;
        }



        //添加渠道
        function ShowAdd() {
            $("#dlgChannelInfo input[type='text']").val("");
            $('#dlgChannelInfo').dialog({ title: '增加渠道' });
            $('#dlgChannelInfo').dialog('open');
            currAction = "AddSupplierChannel";

        }

        //编辑渠道
        function ShowEdit() {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $("#txtChannelName").val(rows[0].ChannelName.replace('└', '').replace(/(^\s*)|(\s*$)/g, ""));
            $("#txtDesc").val(rows[0].Description);
            $("#ddlParentChannel").val(rows[0].ParentChannel);
            $("#ddlLevel").val(rows[0].ChannelLevelId);
            $(txtTrueName).val(rows[0].TrueName);
            $(txtCompany).val(rows[0].Company);
            $(txtPosition).val(rows[0].Postion);
            $(txtPhone).val(rows[0].Phone);
            $(txtEmail).val(rows[0].Email);

            $('#dlgChannelInfo').dialog({ title: '编辑渠道' });
            $('#dlgChannelInfo').dialog('open');
            currAction = "EditSupplierChannel";
            selectAutoId = rows[0].AutoID;

        }





        function Delete() {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            $.messager.confirm("系统提示", "确认删除?", function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "DeleteSupplierChannel", UserId: rows[0].UserID },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.IsSuccess == true) {
                                Alert("操作成功");

                                $('#grvData').datagrid('reload');
                            }
                            else {
                                Alert(resp.Msg);
                            }


                        }
                    });


                }
            }
                )








        }


        

    </script>
</asp:Content>