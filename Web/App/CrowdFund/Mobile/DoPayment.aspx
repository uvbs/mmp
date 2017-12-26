<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoPayment.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.Mobile.DoPayment" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"><title>

</title><meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="styles/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="styles/applications.css" rel="stylesheet" type="text/css" />
    <link href="styles/common.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    body{font-family: "Microsoft YaHei" ! important;font-size:14px;}
    #btnSumbit{width:98%;margin-bottom:20px;}
    #txtCount{color:Red;}
    #txtReview{min-width:200px;border-radius:4px;border-bottom-left-radius:0;border-top-left-radius:0;border: 1px solid #ccc;}
    .red{color:Red;}
    .input-group{width:98%;margin-top:10px;margin-left:2px;}
    </style>
</head>
<body>
    
  <div class="container-fluid">
        <div class="row">
            <nav class="navbar navbar-default navbar-fixed-top" role="navigation" onclick="javascript:history.go(-1)">
                <div class="col-sm-1 col-xs-1">
                    <div class="navbar-header">
                      <a class="navbar-brand" >
                        <div class="return_ico">
                            <img alt="retrun" src="images/return.png">
                        </div>
                      </a>
                    </div>
                </div>
                <div class="col-sm-10 col-xs-10 page_title">
                    <p class="navbar-text">确认付款</p>
                </div>
            </nav>
            
        </div>
        
        <div class="row" style="margin-top:60px;font-size:16px;font-weight:bold;margin-left:10px;">
            
               <%=model.Title %>
          
        </div>
        <div class="row">
            <div class="info">
                 <ul class="list-inline">
                  <li>单位价格:<label class="red"><%=model.UnitPrice %></label>元</li>
                  <br />
                  <li>总金额:<label class="red" id="lbltotalamount"><%=model.UnitPrice %></label>元</li>
                </ul>
               
               
                
            </div>
        </div>

        <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    数量
                </span>
                <input id="txtCount" type="text" class="form-control" value="1" placeholder="必填" onkeyup="value=value.replace(/[^\d]/g,'')">
            </div>   
        </div>
                <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    姓名
                </span>
                <input id="txtName" type="text" class="form-control" value="" placeholder="必填">
            </div>   
        </div>
                <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    手机
                </span>
                <input id="txtPhone" type="text" class="form-control" value="" placeholder="必填" onkeyup="value=value.replace(/[^\d]/g,'')">
            </div>   
        </div>

        <%if (model.CompanyRequired.Equals(1)){%>

           <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    公司
                </span>
                <input id="txtCompany" type="text" class="form-control" value="" placeholder="" >
            </div>   
        </div>
        <% }else{%>
         <input id="txtCompany" type="hidden" value="" >
        <%}%>
         <%if (model.PositionRequired.Equals(1)){%>

           <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    职位
                </span>
                <input id="txtPosition" type="text" class="form-control" value="" placeholder="" >
            </div>   
        </div>
        <% }else{%>
         <input id="txtPosition" type="hidden" value="" >
        <%}%>
         <div class="row">
            <div class="input-group">
                <span class="input-group-addon">
                    留言
                </span>
               <textarea id="txtReview" style="height:50px;width:100%;"></textarea>
            </div>   
        </div>

        <div class="row">
           <div class="applic_btn">
                    <button class="btn" id="btnSumbit">确认付款</button>
           </div>
    </div>
    </div>

</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script>

   var IsSubmit=false;
   $(function () {

       $("#btnSumbit").click(function () {
           var model = {
               Action: "AddCrowdFundRecord",
               CrowdFundID:<%=model.AutoID %>,
               Amount: (parseInt($("#txtCount").val())*<%=model.UnitPrice%>),
               Name: $("#txtName").val(),
               Phone: $("#txtPhone").val(),
               Company: $("#txtCompany").val(),
               Position: $("#txtPosition").val(),
               Review: $("#txtReview").val()

           }
           if ($.trim($("#txtCount").val())=="") {
           $("#txtCount").focus();
             return false;
}
          if (model.Name=="") {
           $("#txtName").focus();
             return false;
}
           if (model.Phone=="") {
           $("#txtPhone").focus();
             return false;
}
           if (IsSubmit==true) {
            return false;
            }
            IsSubmit=true;
            $("#btnSumbit").html("正在处理...");
           $.ajax({
               type: 'post',
               url: "MobileHandler.ashx",
               data: model,
               dataType: 'json',
               success: function (resp) {
                   if (resp.Status == 1) {
                       window.location.href = "DoWXPay.aspx?recordid=" + resp.ExInt + "&showwxpaytitle=1&crowdfundid="+<%=model.AutoID%>;
                   }
                   else {
                        IsSubmit=false;
                       alert(resp.Msg);
                   }


               },
               complete:function(){
               
               $("#btnSumbit").html("确认付款");
               }


           });






       })

           $("#txtCount").keyup(function () {
           
            if ($(this).val()=="0") {
            $(this).val("1");
}
            $("#lbltotalamount").text($(this).val() * <%=model.UnitPrice %>);

        })

   })

</script>



</html>