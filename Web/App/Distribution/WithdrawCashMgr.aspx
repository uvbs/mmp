<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WithdrawCashMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Distribution.WithdrawCashMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp; &nbsp;分销-提现管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           
              
                  
           
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ChangStatus(1)">标记为已受理</a> 
             <%if (!PmsTransfersAudit){ %>
            <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ChangStatus(2)">标记为已打款</a> 
              <% } %>
            <a href="javascript:void(0);"
                        class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ChangStatus(3)">
                        标记为失败</a> 
                     <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="CreateBatchFile()">
                            导出excel</a>
            <br />
            状态:
            <select id="ddlStatus">
                <option value="">全部</option>
                <option value="0">待审核</option>
                <option value="1">已受理</option>
                <option value="2">已打款</option>
                <option value="3">失败</option>
            </select>
            到账方式:
            <select id="ddlType">
                <option value="">全部</option>
                <option value="0">银行卡</option>
                <option value="1">微信</option>
                <option value="2">账户余额</option>
            </select>
            开户名:<input id="txtUserId" style="width: 200px;" />
            时间:
            <input type="text" id="txtFrom" style="width: 100px;" readonly="readonly" class="easyui-datebox" />-
            <input type="text" id="txtTo" style="width: 100px;" readonly="readonly" class="easyui-datebox" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var IsSubmit = false;
     var withdrawCashType = "DistributionOnLine";
//     if ("<%=Request["type"]%>"=="DistributionOffLine") {
//        withdrawCashType="DistributionOffLine";
//     }
//     else {
//     withdrawCashType="DistributionOnLine";
//}
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWithrawCash",WithdrawCashType:withdrawCashType },
	                height: document.documentElement.clientHeight - 180,
	                pagination: true,
	                striped: true,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'InsertDate', title: '时间', width: 80, align: 'left', formatter: FormatDate },
                                { field: 'TransfersType', title: '到账方式', width: 50, align: 'center', formatter: function (value, rowData) {
                                    if (value == 0) {
                                        return "银行卡";
                                    }
                                    else if (value == 1) {
                                        return "微信";
                                    }
                                    else if (value == 2) {
                                        return "账户余额";
                                    }
                                }
                                },
                                                                { field: 'Status', title: '状态', width: 50, align: 'center', formatter: function (value, rowData) {
                                    if (value == 0) {
                                        return "待审核";
                                    }
                                    else if (value == 1) {
                                        return "已受理";
                                    }
                                    else if (value == 2) {
                                        return "已打款";
                                    }
                                    else if (value == 3) {
                                        return "失败";
                                    }

                                }
                                },
                                { field: 'AutoID', title: '商户流水号', width: 50, align: 'left' },
                                { field: 'UserId', title: '申请者', width: 50, align: 'left', formatter: function (value, row) {
                                        return row.TrueName + "(" + row.AutoID + ")";

                                    }
                                },
                                { field: 'Phone', title: '手机', width: 50, align: 'left' },
                                { field: 'Amount', title: '申请金额', width: 50, align: 'left' },
                                { field: 'ServerFee', title: '服务费', width: 50, align: 'left' },
                                { field: 'RealAmount', title: '实际金额(扣除服务费)', width: 50, align: 'left' },
                                { field: 'BankName', title: '银行', width: 50, align: 'left' },
                                { field: 'AccountName', title: '银行开户名', width: 50, align: 'left' },
                                { field: 'BankAccount', title: '银行账号', width: 50, align: 'left' }



                             ]]
	            }
            );
         $(".datebox :text").attr("readonly", "readonly");
     });
     function Search() {
         var FromDate = $("#txtFrom").datebox('getValue');
         var ToDate = $("#txtTo").datebox('getValue');
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWithrawCash", UserId: $("#txtUserId").val(), Status: $("#ddlStatus").val(), FromDate: FromDate, ToDate: ToDate,WithdrawCashType:withdrawCashType,Type:$(ddlType).val() }
	            });
     }

     //标记为已受理
     function ChangStatus(status) {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         var tranids = [];
         for (var i = 0; i < rows.length; i++) {
             if (status == 1) {//修改为受理中
                 if (rows[i].Status != 0) {
                     Alert("只有状态为待审核的记录才能标记为已受理");
                     return false;
                 }
             }
             if (status == 2) {//修改为成功
                 if (rows[i].Status != 1) {
                     Alert("只有状态为受理中的记录才能标记为已打款");
                     return false;
                 }
             }
             if (status == 3) {//修改为失败
                 if (rows[i].Status != 1) {
                     Alert("只有状态为受理中的记录才能标记为失败");
                     return false;
                 }
             }



         }
         for (var i = 0; i < rows.length; i++) {

             tranids.push(rows[i].AutoID);


         }

         var msg = "";

         switch (status) {
             case 1:
                 msg = "确定将所选记录标记为受理中?";
                 break;
             case 2:
                 msg = "标记为已打款后,用户账户余额将会被扣除，<br/>如果到账方式是微信,则会直接打款到用户的微信!请确认是否继续?";
                 break;
             case 3:
                 msg = "标记为提现失败后,用户冻结的提现金额将被解冻,是否继续?";
                 break;

             default:

         }
         $.messager.confirm("系统提示", msg, function (r) {

             if (r) {
                 //
                 if (IsSubmit) {
                     return false;
                 }
                 IsSubmit = true;
                 $.messager.progress({ text: '正在处理...' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: { Action: 'UpdateWithrawCashStatus', TranIds: tranids.join(','), Status: status,withdrawCashType:withdrawCashType },
                     dataType: 'json',
                     success: function (resp) {
                         if (resp.Status == 1) {
                             alert("操作成功");
                             $('#grvData').datagrid('reload');
                         }
                         else {
                             if (resp.Msg.indexOf("GetProductAuthority") > -1) {
                                 alert("尚未开通企业付款功能,请登录 商户平台-产品中心 申请开通企业付款功能");
                             }
                             else {
                                 alert(resp.Msg);

                             }
                             
                         }


                     },
                     complete: function () {
                         IsSubmit = false;
                         $.messager.progress('close');
                     }


                 });
                 //

             }

         })


     }


     function CreateBatchFile() {


         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         var tranids = [];
         for (var i = 0; i < rows.length; i++) {
             tranids.push(rows[i].AutoID);
         }
         window.location.href = "/Handler/App/DistributionExportHandler.ashx?tranids=" + tranids.join(',');
     }

 </script>
</asp:Content>