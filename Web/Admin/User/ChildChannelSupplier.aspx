<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ChildChannelSupplier.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.ChildChannelSupplier" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;渠道分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>渠道商户管理</span>

    
    <a href="SupplierChannelList.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
            onclick="AddSupplier();">添加 </a>

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
            onclick="Delete();">删除</a>


        <br />
        <div style="margin-bottom: 5px">

            商户:
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px;"
                placeholder="商户" />


            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

  

    <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="添加商户" style="width: 450px; padding: 15px;">
        商户名称:<input id="txtKeyWord1" placeholder="名称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchSupplier()">查询</a>
        <br />
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currentAction = "AddChildChannelSupplier";
        var selectAutoId = "0";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryChildChannelSupplier", parentChannel: "<%=Request["parentChannel"]%>" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                //{ title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: 'ID', width: 20, align: 'left' },
                                //{ field: 'HexiaoCode', title: '核销码', width: 50, align: 'left' },

                                //{
                                //    field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                //        if (value == '' || value == null)
                                //            return "";
                                //        var str = new StringBuilder();
                                //        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                //        return str.ToString();
                                //    }
                                //},
                                //{ field: 'WXNickname', title: '微信昵称', width: 80, align: 'left', formatter: FormatterTitle },

                                {
                                    field: 'TrueName', title: '名称', width: 80, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" title="{0}">{0} </a>', rowData.Company);
                                        return str.ToString();
                                    }
                                },
                                //{ field: 'Phone', title: '手机', width: 80, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                 {
                                     field: 'ParentChannelName', title: '所属渠道', width: 80, align: 'center', formatter: function (value, row) {

                                         //return row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';



                                         return value;



                                     }
                                 }
                                //{ field: 'TagName', title: '标签', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'Email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },

                                //{ field: 'SalesQuota', title: '累计销售', width: 100, align: 'left', formatter: FormatterTitle },
	                            //{ field: 'HistoryDistributionOnLineTotalAmountEstimate', title: '累计奖励（预估）', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'OverCanUseAmount', title: '已提现奖励', width: 100, align: 'center', formatter: FormatterTitle },
                                //{ field: 'CanUseAmount', title: '可提现奖励', width: 100, align: 'center', formatter: FormatterTitle },

                                //CumulativeReward

                                //{
                                //    field: 'HistoryDistributionOnLineTotalAmount', title: '累计佣金', width: 80, align: 'center', sortable: true
                                // }

                                //,

                                   //{
                                   //    field: 'DistributionDownUserCountLevel1', title: '一级会员数', width: 80, sortable: true, align: 'center'

                                   //},
                                   //{ field: 'DistributionDownUserCountAll', title: '所有会员数', width: 80, sortable: true, align: 'center' },
                                   //{ field: 'DistributionSaleAmountLevel1', title: '一级销售额', sortable: true, width: 80, align: 'center' },
                                   //{ field: 'DistributionSaleAmountAll', title: '所有销售额', sortable: true, width: 80, align: 'center' }
                                   //{ field: 'DistributionDownUserCountLevel2', title: '二级', width: 80, align: 'center', sortable: true,
                                   //    formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMember.aspx?level=2&autoid={0}">{1}</a>', row["AutoID"], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   //{ field: 'DistributionDownUserCountLevel3', title: '三级', width: 80, align: 'center', sortable: true,
                                   //    formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMember.aspx?level=3&autoid={0}">{1}</a>', row["AutoID"], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   //{
                                   //    field: 'DistributionSaleAmountLevel0', title: '自己消费额', width: 80, align: 'center', formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   // {
                                   //     field: 'DistributionSaleAmountLevel1', title: '会员消费额', width: 100, align: 'center', sortable: true, formatter: function (value, row) {
                                   //         var str = new StringBuilder();
                                   //         str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                   //         return str.ToString();

                                   //     }
                                   // }
                                    //,
                                    //{ field: 'DistributionSaleAmountLevel2', title: '二级销售额', width: 80, align: 'center', formatter: function (value, row) {
                                    //    var str = new StringBuilder();
                                    //    str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                    //    return str.ToString();

                                    //}
                                    //},
                                    //{ field: 'DistributionSaleAmountLevel3', title: '三级销售额', width: 80, align: 'center', formatter: function (value, row) {
                                    //    var str = new StringBuilder();
                                    //    str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                    //    return str.ToString();

                                    //}
                                    //}





	                ]]
	            });



            //商户列表
            $('#dlgUserInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $("#grvUserInfo").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;


                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { Action: "AddChildChannelSupplier", ParentChannel: "<%=Request["parentChannel"]%>", UserId: rows[0].user_id },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == 1) {
                                    Alert("操作成功");
                                    $("#dlgUserInfo").dialog('close');
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
                        $('#dlgUserInfo').dialog('close');
                    }
                }]
            });

           

            $('#grvUserInfo').datagrid(
               {
                   method: "Post",
                   url: "/Serv/Api/Admin/User/Supplier/List.ashx",
                   queryParams: { keyword: "" },
                   height: 300,

                   pagination: true,
                   striped: true,
                   pageSize: 20,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'company_name', title: '名称', width: 20, align: 'left' },
                                { field: 'user_id', title: '账号', width: 20, align: 'left' }
                   ]]
               });

           


            //load


        });

        //搜索
        function Search() {

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QueryChildChannelSupplier", keyword: $(txtKeyWord).val(), parentChannel: "<%=Request["parentChannel"]%>" }
                    });
                }



        //添加商户
                function AddSupplier() {


                    $('#dlgUserInfo').dialog({ title: '选择商户' });
                    $('#dlgUserInfo').dialog('open');


                }
              

                //搜索
                function SearchSupplier() {
                    $('#grvUserInfo').datagrid(
                            {
                                method: "Post",
                                url: "/Serv/Api/Admin/User/Supplier/List.ashx",
                                queryParams: { KeyWord: $(txtKeyWord1).val() }
                            });
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
                                data: { Action: "DeleteChildChannelSupplier", UserId: rows[0].UserID },
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

                    })






                }

    </script>
</asp:Content>

