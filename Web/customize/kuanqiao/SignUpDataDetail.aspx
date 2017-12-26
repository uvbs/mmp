<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SignUpDataDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.kuanqiao.SignUpDataDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.content
{
 margin-top:20px;
 margin-left:25px;
 margin-right:5px;   
 }
 table
 {
    line-height:25px;
    
 }
td
{
    
    border:1px solid #000000;
    text-align:center;
    vertical-align:center;
   
}
.td1
{
    
    border:1px solid #000000;
    text-align:center;
    vertical-align:center;
    border-top-color:White;
    font-size:14px;
    

   
}
.tdtitle
{
  width:100px;
  text-align:center; 
  font-size:14px;
  vertical-align:center;
  
    
}
.bottom
{
   border:1px solid;
   border-radius:5px;
   border-color:#CCCCCC;
   background-color:#F0F0F0;
   margin-top:10px;
    
}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
 <div>当前位置：<a href="SignUpData.aspx" title="返回核名列表" >企业核名</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;详细 <span></span>
 </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
<div class="content">

<table style="width:100%;border-collapse:collapse;" cellpadding="1" cellspacing="0" border="1"  bordercolor="#000000" >
	<tbody>
    <tr>
			<td colspan="2" style="text-align:center;" >
            <label style="font-size:20px;font-weight:bold;">□企业设立名称预先核准</label>
			</td>
			
   </tr>
		<tr>
			<td class="tdtitle">
				申请企业名称
			</td>
			<td>
            <label style="font-weight:bold;font-size:14px;"><%=model.K2%></label>
				
			</td>
		</tr>
		<tr>
			<td class="tdtitle">
				备选
                <br />
                企业名称1

			</td>
			<td>
				
               <%=model.K3%>
			</td>
		</tr>
		<tr>
			<td class="tdtitle">
				备选
                <br />
                企业名称2
			</td>
			<td>
				<%=model.K4%>
			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				备选
                <br />
                企业名称3
			</td>
			<td>
				<%=model.K5%>
			</td>
		</tr>
        
          <tr>
			<td class="tdtitle">
				注册资本
			</td>
			<td>
				<%=model.K6%>
                &nbsp;&nbsp;(万元)
			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				企业类型
			</td>
			<td>
                <%=model.K7%>

			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				经营范围
			</td>
			<td>
				<%=model.K8%>
			</td>
		</tr>
               
      
         
	</tbody>
</table>



<table style="width:100%;border-collapse:collapse;" cellpadding="0" cellspacing="0" border="1"  >
	<tbody>
    
		<tr>
			<td class="td1">
				投资人
			</td>
			<td class="td1">
				名称或姓名
			</td>
            <td class="td1">
				证照号码
			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				1
			</td>
			<td>
				<%=model.K9%>
			</td>
            <td>
				<%=model.K10%>
			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				2
			</td>
			<td>
				<%=model.K11%>
			</td>
            <td>
				<%=model.K12%>
			</td>
		</tr>

        <tr>
			<td class="tdtitle">
				3
			</td>
			<td>
				<%=model.K13%>
			</td>
            <td>
				<%=model.K14%>
			</td>
		</tr>

      
         
	</tbody>
</table>

<table style="width:100%;border-collapse:collapse;margin-top:0px;" cellpadding="1" cellspacing="0" border="1" >
	<tbody>
    
    <tr>
			<td colspan="2" style="text-align:center;" class="td1">
            <label style="font-size:14px;font-weight:bold;">联系资料</label>
			</td>
			
   </tr>
    <tr>
			<td class="tdtitle">
				姓名
			</td>
			<td>
				<%=model.Name%>
			</td>
		</tr>
      <tr>
			<td class="tdtitle">
				手机
			</td>
			<td>
				<%=model.Phone%>
			</td>
		</tr>
        <tr>
			<td class="tdtitle">
				邮箱
			</td>
			<td>
				<%=model.K1%>
			</td>
		</tr>
         
	</tbody>
</table>

<div style="margin-top:20px;">

<%if (!string.IsNullOrEmpty(model.K18))
  {
      StringBuilder sb = new StringBuilder();
      foreach (var path in model.K18.Split(','))
      {
        sb.AppendLine(string.Format("<img width=\"345\" src=\"{0}\">",path));
        sb.AppendLine("</br>");

      }
      Response.Write(sb.ToString());
      
  } %>

<div id="div_applyimagelist" style="margin-left:50px;text-align:center;">


</div>



<div class="bottom">

<h5 style="font-size:12px;font-weight:bold;margin-left:5px;margin-top:5px;">申请结果:</h5>
<br />

<input  type="radio" name="rdapplystatus" id="rdonew" value="待处理"/><label for="rdonew">待处理</label>
<input  type="radio" name="rdapplystatus" id="rdoprocess" value="正在处理"/><label for="rdoprocess">正在处理</label>
<input  type="radio" name="rdapplystatus" id="rdosuccess" value="审核通过"/><label for="rdosuccess">审核通过</label>
<input  type="radio" name="rdapplystatus" id="rdofail"  value="审核失败"/><label for="rdofail">审核失败</label>
<div style="display:none" id="divFail" >
<br />
<textarea id="txtFailReason" style="width:95%;height:50px;margin-left:5px;" ><%=model.K17%></textarea>
</div>
<br />
<a href="javascript:;" id="btnUpdateApplyStatus" style="font-size:12px;margin-left:10px;margin-top:10px;margin-bottom:5px;" class="button button-rounded button-flat-primary">
                            修改申请结果</a>

</div>

<div class="bottom" style="margin-bottom:50px;">
<label style="font-size:12px;font-weight:bold;margin-bottom:5px;margin-left:5px;margin-top:5px;">处理状态:</label>
<input type="text" id="txtResult" style="width:250px;height:20px;" value="<%=model.K16%>" />
<a href="javascript:;" id="btnResult" style="font-size:12px;margin-left:10px;margin-bottom:5px;" class="button button-rounded button-flat-primary">修改处理状态</a>
                            
</div>

</div>




</div>

<script type="text/javascript">

    var handlerUrl = "/handler/app/kuanqiaohandler.ashx";
    $(function () {
       
        $("input[name='rdapplystatus']").click(
        function () {
            if ($(this).val() == "审核失败") {
                $("#divFail").show();
            }
            else {
                $("#divFail").hide();
            }

        }

        );
//        parent.SetIframeHeight(1000);
        $("#btnUpdateApplyStatus").click(function () {

            UpdateApplyStatus();



        });
        $("#btnResult").click(function () {

            UpdateApplyResult();



        });

        var applystauts = "<%=model.K15%>";
        switch (applystauts) {
            case "待处理":
                $("#rdonew").attr("checked", "checked");
                break;
            case "正在处理":
                $("#rdoprocess").attr("checked", "checked");
                break;
            case "审核通过":
                $("#rdosuccess").attr("checked", "checked");
                break;
            case "审核失败":
                $("#rdofail").attr("checked", "checked");
                $("#divFail").show();
                break;
            default:

        }

        LoadApplyImage();

    });

    ///更新处理状态
    function UpdateApplyStatus () {

        var applystatus = $("input[name='rdapplystatus']:checked").val();
        if (applystatus==undefined) {
           
            Alert("请选择申请结果");
            return false;
        }
        if (applystatus == "审核失败") {
            if ($.trim($("#txtFailReason").val())=="") {
               
                Alert("请填写审核失败原因");
             return false;

            }
           
        }

     $.messager.confirm("系统提示", "确定将申请结果修改为 " + applystatus+" ?", function (o) {
         if (o) {
             var model =
                    {
                        UID: "<%=model.UID%>",
                        Action:"UpdateApplyStatus",
                        ApplyStauts: applystatus,
                        FailReason:$("#txtFailReason").val()
                    };
            
             $.ajax({
                 type: "Post",
                 url: handlerUrl,
                 data:model,
                 success: function (result) {
                     var resp = $.parseJSON(result);
                     Alert(resp.Msg);
                 }

             });
         }
     });




 }

 ///更新处理状态
 function UpdateApplyResult() {

     var result =$.trim($("#txtResult").val());

     if (result == "") {
             Alert("请填写处理状态");
             return false;
     }

         $.messager.confirm("系统提示", "确定将处理状态修改为 " + result + " ?", function (o) {
         if (o) {
             var model =
                    {
                        UID: "<%=model.UID%>",
                        Action: "UpdateApplyResult",
                        Result: result
                        
                    };

             $.ajax({
                 type: "Post",
                 url: handlerUrl,
                 data: model,
                 success: function (result) {
                     var resp = $.parseJSON(result);
                     Alert(resp.Msg);
                 }

             });
         }
     });




 }
 var handlerUrl = "/Handler/App/KuanqiaoHandler.ashx";
 function LoadApplyImage() {
     $.post(handlerUrl, { Action: "GetApplyImageList", oid: "<%=model.WeixinOpenID%>" }, function (data) {
         var strhtml = "";
         var objData = eval(data);
         $.each(objData, function (index, item) {
             strhtml += "<img width='345' src=" + item.FilePath + ">";
             strhtml += "<br/>";
         });
         strhtml += "";
         $("#div_applyimagelist").html(strhtml);
         if (data == "") {

         }
     });

 }

</script>
</asp:Content>
