<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallDeliveryCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallDeliveryCompile" %>
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
    <div class="title">当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="/App/MallManage/WXMallDeliveryMgr.aspx" title="返回配送方式列表" >配送方式管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=HeadTitle%></span>
    
     <a href="WXMallDeliveryMgr.aspx" style="float: right; margin-right: 20px;" title="配送方式管理"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">

        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                     <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        配送方式：
                    </td>
                    <td width="*" align="left">
                    <input type="radio" name="rdodelivery" id="rdodelivery0" checked="checked" value="0"/><label for="rdodelivery0">快递</label>
                    <input type="radio" name="rdodelivery" id="rdodelivery1" value="1" /><label for="rdodelivery1">上门自取</label>
                    <input type="radio" name="rdodelivery" id="rdodelivery2" value="2" /><label for="rdodelivery2">卖家承担</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        配送方式名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtDeliveryName" value=" <%=model == null ? "" : model.DeliveryName%>" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       
                    </td>
                    <td width="*" align="left" >
                        <table id="tbkuandi">
                        <tr align="center"><td>起始件数</td><td>起始费用</td><td>每增加&nbsp;件</td><td>增加&nbsp;元</td></tr>
                        <tr>
                        <td>
                        <input type="text" id="txtInitialProductCount" value=" <%=model == null ? "1" : model.InitialProductCount.ToString()%>"  style="width:50px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"  />件
                        </td>
                        <td>
                        <input type="text" id="txtInitialDeliveryMoney" value=" <%=model == null ? "" : model.InitialDeliveryMoney.ToString()%>"  style="width:50px;" />元
                       </td>
                        <td>
                        <input type="text" id="txtAddProductCount" value=" <%=model == null ? "1" : model.AddProductCount.ToString()%>"  style="width:50px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"  />件
                       </td>
                        <td>
                        <input type="text" id="txtAddMoney" value=" <%=model == null ? "0" : model.AddMoney.ToString()%>"  style="width:50px;" />元
                       </td>
                        </tr>
                        </table>
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

         }
         else {

             var deliverytype = "<%=model.DeliveryType%>";
             ShowDeliveryType(deliverytype);
             if (deliverytype == "0") {
                 $("#rdodelivery0").attr("checked", "checked");

             }
             else if (deliverytype == "1") {
                 $("#rdodelivery1").attr("checked", "checked");
             }
             else {
                 $("#rdodelivery2").attr("checked", "checked");
             }
         }

         $('#btnSave').click(function () {
             try {
                 var model =
                    {

                        AutoId: "<%=autoid%>",
                        Sort: 0,
                        DeliveryType: $("input[name=rdodelivery]:checked").val(),
                        DeliveryName: $.trim($('#txtDeliveryName').val()),
                        InitialProductCount: $.trim($("#txtInitialProductCount").val()),
                        InitialDeliveryMoney: $.trim($("#txtInitialDeliveryMoney").val()),
                        AddProductCount: $.trim($('#txtAddProductCount').val()),
                        AddMoney: $.trim($('#txtAddMoney').val()),
                        Action: currAction == 'add' ? 'AddWXMallDelivery' : 'EditWXMallDelivery'

                    };
                 if (model.DeliveryName == '') {
                     $('#txtDeliveryName').focus();
                     Alert('请输入配送方式名称！');
                     return;
                 }
                 if (model.DeliveryType == '') {
                     Alert('请选择配送方式');
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
             // ResetCurr();

         });

         $("input[name=rdodelivery]").click(function () {

             ShowDeliveryType($(this).val());

         });



     });





     function ResetCurr() {
         ClearAll();

     }


     function ShowDeliveryType(type) {
         switch (type) {
             case "0":
                 $("#tbkuandi").show();
                 break;
             default:
                 $("#tbkuandi").hide();
                 break;

         }

     }



    </script>
</asp:Content>
