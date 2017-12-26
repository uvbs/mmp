<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Recharge" %>
<!DOCTYPE html>
<html>
<head>
    <title>购票</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link rel="stylesheet" href="/css/vote/style.css?v=0.0.1"/>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.slides.min.js" type="text/javascript"></script>
    <script src="/Scripts/vote/voteanimate.js" type="text/javascript"></script>
    <style type="text/css">
    .spcurruser
    {
        
       margin-top:20px;
        
     }
     .curruser
     {
         
         color: #ff6000;
         font-size: 16px;
         font-weight: bold;
     }
    
    
    </style>
</head>
<body>
<section class="box">
    <div class="total votenumbar votenopay">
        <span class="votenumtext">剩余票数：<span class="num" id="spancount"><%=CanUseVoteCount%></span>
        </span>
        <a href="#" class="btn_min orange">为星星购票</a>
    </div>
    <div class="votepaybox">
        <div class="votearticle">
            <%=VoteInfo.Introduction%>
<br />
        </div>
        <label for="" class="inputtitle">选择充值票数：</label>
        <div class="clear"></div>

        <%for (int i = 0; i < RechargeList.Count; i++){ %>

          <input type="radio" class="vote_radio" name="rdopay" id="rd<%=RechargeList[i].AutoID%>"  value="<%=RechargeList[i].Amount%>"/>
          <label for="rd<%=RechargeList[i].AutoID%>" class="radio_label" id="lbl<%=RechargeList[i].AutoID%>"><%=RechargeList[i].RechargeCount%>票</label>

          <%} %>

        



        <div class="clear"></div>
                <span class="inputtitle">获得礼物：</span>


        <div class="votegift">
           <%foreach (var item in RechargeList){%>

             <div class="giftinfo">
                <h2 class="giftinfotitle"><%=item.GiftName %></h2>
                <p><%=item.GiftDesc.Replace("\n","</br>") %></p>
            </div>

          <%} %>
           

        </div>
        <label class="inputtitle">所需金额：</label><span class="payprice" id="spanpayprice">￥0.00</span>
       
       

        
        <div class="clear"></div>
        <div >
        <label class="inputtitle" style="float:left;margin-left:0px;">当前用户:</label><span class="payprice"><%=UserInfo.UserID%></span>
       
       </div>
       <br />
        <a href="javascript:void(0)" class="btn orange" id="btnPay" onclick="DoPay()">在线支付</a><a href="<%=VoteInfo.OfflinePayUrl%>" class="btn orange">线下支付</a>

    </div>


    <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
        
    
    </div>
</section>
</body>
<script type="text/javascript">


    $(function () {


        $("input[name=rdopay]").first().attr("checked", "checked");
        var obj= $("input[name=rdopay]").first();
        var defaultbuycount = $(obj).val();
        $("#spanpayprice").html("￥" + defaultbuycount);

        $("input[name=rdopay]").click(function () {

            var buycount = $(this).val();
            $("#spanpayprice").html("￥" + buycount);


        });



        $(".vote_radio").each(function (index) {
            $(this).change(function () {
                $(".giftinfo").hide()
                $(".giftinfo:eq(" + index + ")").show()
            })
        })





    });
    //投票下单
    function DoPay() {
        var autoid = $("input[name=rdopay]:checked").attr("id").replace("rd", "");
        $.ajax({
            type: 'post',
            url: "/Handler/CommHandler.ashx",
            data: { Action: "SumbitOrderPayVote",VoteRechargeId:autoid,VoteID:<%=VoteInfo.AutoID%>},
            timeout: 30000,
            dataType:"json",
            success: function (resp) {
                if (resp.Status == 1) {
                    //window.location = "DoAlipay.aspx?oid=" + resp.ExStr;
                    window.location = "DoWxpay.aspx?oid=" + resp.ExStr;

                }
                else {

                    alert(resp.Msg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("下单超时，请重新下单");

                }
            }
        });


    }


</script>


</html>
