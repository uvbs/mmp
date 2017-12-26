<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallPayMentTypeCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallPayMentTypeCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        }
        

        .title
        {
            
         font-size:12px;   
         }
         .return
         {
             
             float:right;
             margin-right:5px;
         }
        input[type=text],select
         {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
             
        }
        .rmb
        {
            color:Red;
            font-weight:bold;
            
        }
         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="/App/MallManage/WXMallPayMentTypeMgr.aspx" title="返回支付方式列表" >配送方式管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=HeadTitle%></span>
    <a href="WXMallPayMentTypeMgr.aspx" style="float: right; margin-right: 20px;" title="支付方式管理"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">

        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                     <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        支付方式：
                    </td>
                    <td width="*" align="left">
                    <input type="radio" name="rdopaymenttype" id="rdodelivery0" checked="checked" value="0"/><label for="rdodelivery0">线下支付</label>
                    <input type="radio" name="rdopaymenttype" id="rdodelivery1" value="1" /><label for="rdodelivery1">支付宝</label>
                    <input type="radio" name="rdopaymenttype" id="rdodelivery2" value="2" /><label for="rdodelivery2">微信支付</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        支付方式名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPaymentTypeName" value="<%=model == null ? "" : model.PaymentTypeName%>" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr id="trAlipayPartner" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                    支付宝 PID:
                    </td>
                    <td width="*" align="left" >
                    <input type="text" id="txtAlipayPartner" value="<%=model == null ? "" : model.AlipayPartner%>" class="" style="width: 100%;" />

                     </td>
                </tr>
                <tr id="trAlipayPartnerKey" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                      支付宝 KEY:
                    </td>
                    <td width="*" align="left" >
                        <input type="text" id="txtAlipayPartnerKey" value="<%=model == null ? "" : model.AlipayPartnerKey%>" class="" style="width: 100%;" />

                     </td>
                </tr>
                <tr id="trAlipaySeller_Account_Name" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                       支付宝账号:
                    </td>
                    <td width="*" align="left" >
                   <input type="text" id="txtAlipaySeller_Account_Name" value="<%=model == null ? "" : model.AlipaySeller_Account_Name%>" class="" style="width: 100%;" />

                     </td>
                </tr>

                <tr id="trWXAppId" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                     AppId:
                    </td>
                    <td width="*" align="left" >
                    <input type="text" id="txtWXAppId" value="<%=model == null ? "" : model.WXAppId%>" class="" style="width: 100%;" />

                     </td>
                </tr>
                <tr id="trWXAppKey" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                      AppKey:
                    </td>
                    <td width="*" align="left" >
                        <input type="text" id="txtWXAppKey" value="APPKEY" class="" style="width: 100%;" />

                     </td>
                </tr>
                <tr id="trWXPartner" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                       MCH_ID:
                    </td>
                    <td width="*" align="left" >
                   <input type="text" id="txtWXPartner" value="<%=model == null ? "" : model.WXPartner%>" class="" style="width: 100%;" />

                     </td>
                </tr>
                <tr id="trWXPartnerKey" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                       KEY:
                    </td>
                    <td width="*" align="left" >
                   <input type="text" id="txtWXPartnerKey" value="<%=model == null ? "" : model.WXPartnerKey%>" class="" style="width: 100%;" />

                     </td>
                </tr>

                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <br />
                        <a href="javascript:;"  id="btnSave" style="font-weight: bold;width:200px;text-decoration:underline;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:;"  id="btnReset" style="font-weight: bold;width:200px;" class="button button-rounded button-flat">
                                重置</a> 
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currAction = '<%=webAction %>';
     $(function () {
         if (currAction == 'add') {
             ShowPayMentTypeType("0");
         }
         else {

             var paymenttype = "<%=model.PaymentType%>";
             ShowPayMentTypeType(paymenttype);
             if (paymenttype == "0") {
                 $("#rdodelivery0").attr("checked", "checked");

             }
             else if (paymenttype == "1") {
                 $("#rdodelivery1").attr("checked", "checked");
             }
             else if (paymenttype == "2") {
                 $("#rdodelivery2").attr("checked", "checked");
             }


         }

         $('#btnSave').click(function () {
             try {
                 var model =
                    {

                        AutoId: "<%=autoid%>",
                        Sort: 0,
                        PaymentType: $("input[name=rdopaymenttype]:checked").val(),
                        PaymentTypeName: $.trim($('#txtPaymentTypeName').val()),
                        AlipayPartner: $.trim($("#txtAlipayPartner").val()),
                        AlipayPartnerKey: $.trim($("#txtAlipayPartnerKey").val()),
                        AlipaySeller_Account_Name: $.trim($('#txtAlipaySeller_Account_Name').val()),
                        WXAppId: $.trim($('#txtWXAppId').val()),
                        WXAppKey: $.trim($('#txtWXAppKey').val()),
                        WXPartner: $.trim($('#txtWXPartner').val()),
                        WXPartnerKey: $.trim($('#txtWXPartnerKey').val()),
                        Action: currAction == 'add' ? 'AddWXMallPaymentType' : 'EditWXMallPaymentType'

                    };
                 if (model.PaymentTypeName == '') {
                     $('#txtPaymentTypeName').focus();
                     Alert('请输入支付方式名称！');
                     return;
                 }
                 if (model.PaymentType == '') {
                     Alert('请选择支付方式');
                     return;
                 }

                 $.messager.progress({ text: '正在处理...' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         $.messager.progress('close');
                         Alert(resp.Msg);


                     }
                 });

             } catch (e) {
                 Alert(e);
             }


         });

         $('#btnReset').click(function () {
             ResetCurr();

         });

         $("input[name=rdopaymenttype]").click(function () {

             ShowPayMentTypeType($(this).val());

         });



     });





     function ResetCurr() {
         //ClearAll();

     }


     function ShowPayMentTypeType(type) {
         switch (type) {
             case "0":
                 $("#trAlipayPartner").hide();
                 $("#trAlipayPartnerKey").hide();
                 $("#trAlipaySeller_Account_Name").hide();

                 $("#trWXAppId").hide();
                 //$("#trWXAppKey").hide();
                 $("#trWXPartner").hide();
                 $("#trWXPartnerKey").hide();

                 break;
             case "1":
                 $("#trAlipayPartner").show();
                 $("#trAlipayPartnerKey").show();
                 $("#trAlipaySeller_Account_Name").show();

                 $("#trWXAppId").hide();
                 //$("#trWXAppKey").hide();
                 $("#trWXPartner").hide();
                 $("#trWXPartnerKey").hide();
                 break;
             case "2":

                 $("#trWXAppId").show();
                 //$("#trWXAppKey").show();
                 $("#trWXPartner").show();
                 $("#trWXPartnerKey").show();

                 $("#trAlipayPartner").hide();
                 $("#trAlipayPartnerKey").hide();
                 $("#trAlipaySeller_Account_Name").hide();

                 break;

             default:
                 $("#trAlipayPartner").hide();
                 $("#trAlipayPartnerKey").hide();
                 $("#trAlipaySeller_Account_Name").hide();
                 $("#trWXAppId").hide();
                 //$("#trWXAppKey").hide();
                 $("#trWXPartner").hide();
                 $("#trWXPartnerKey").hide();
                 break;

         }

     }



    </script>
</asp:Content>
