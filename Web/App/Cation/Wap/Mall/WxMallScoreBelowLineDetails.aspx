<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxMallScoreBelowLineDetails.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.WxMallScoreBelowLineDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>兑换</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wxmall/orderdelivery.js" type="text/javascript"></script>
    <style type="text/css">
        input[type=text], textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        .titlestyle
        {
            font-size: 18px;
            font-weight: bold;
            padding: 20px;
            box-sizing: border-box;
            line-height: 30px;
        }
        .concentstyle
        {
            box-sizing: border-box;
            padding: 0 15px;
        }
        .jifenstyle
        {
            margin-top: 20px;
            display: block;
            padding: 20px;
            box-sizing: border-box;
            font-size: 20px;
            color: #ff7928;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="orderinfobox paddingbottom">
        <div class="order" id="orderconfirm">
         <div >
         <%--<img src="<%=model.RecommendImg%>" >--%>
          <span class="titlestyle"><%=model.PName%></span>
         <div >
          <span class="concentstyle" ><%=model.PDescription%></span>
         </div>
          <span class="jifenstyle"><%=model.Score%>&nbsp;积分</span>
         </div>
        <div style="margin: 0 0 0 10px;">兑换数量:<input type="text" id="txtCount" value="1" maxlength="5" style="font-weight:bold;color:Red;margin-bottom:5px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></div> 
    </div>
    </div>
    
   
    <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
        <a href="javascript:void(0)" id="btnSumbitOrder" class="btn orange">兑换(商家点击)</a>
    </div>
</section>
</body>
<script type="text/javascript">

    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    $(function () {

        //兑换
        $('#btnSumbitOrder').click(function () {
            if (confirm("确定兑换该商品?")) {

                try {

                    var ajaxData = {
                        PID: "<%=model.AutoID%>",
                        Action: 'OnlineExchangeProdect',
                        Count: $("#txtCount").val()
                    }
                    $("#btnSumbitOrder").html("正在提交...");
                    $.ajax({
                        type: 'post',
                        url: mallHandlerUrl,
                        data: ajaxData,
                        timeout: 60000,
                        dataType: "json",
                        success: function (resp) {
                            $("#btnSumbitOrder").html("兑换(商家点击)");
                            
                            if (resp.Status == 1) {
                                window.location = "success.html";

                                //window.location = "success.aspx?oid=" + resp.ExStr + "&gopage=ScoreMall.aspx&orderdetailpage=MyScoreOrderDetails.aspx";
                                return;
                            }
                            else {
                                alert(resp.Msg);
                                return;
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            $("#btnSumbitOrder").html("兑换(商家点击)");
                            if (textStatus == "timeout") {
                                alert("操作超时，请重新兑换(商家点击)");
                            }
                            else {
                                alert(textStatus + "请重新兑换(商家点击)");
                            }
                        }
                    }); //

                } catch (e) {
                    alert(e);
                }
            }

        })

    })

    //判断是不是手机号码
    function isPhone(value) {
        return /^(13|15|18)\d{9}$/i.test(value);
    }
</script>
</html>
