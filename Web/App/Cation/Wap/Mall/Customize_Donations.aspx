<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customize_Donations.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Customize_Donations" %>
<!DOCTYPE html>
<html lang="zh-cn" ng-app="donation">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=0">
    <meta name="format-detection" content="telephone=no" />
    <title><%=ArticleModel.ActivityName%></title>
    <link href="Customize/donation/css/gua.css" rel="stylesheet" type="text/css" />
    <style>
    .textaligncenter button{width:40%;}
    </style>
  </head>
  <body class="doantionbg" background="">
    <div class="mainbox" style="" >
      <div class="article container">
        <p>
         <%=ArticleModel.ActivityDescription%>
        </p>
      </div>
      <div class="donationinfo">
        <div class="textbox">
          <p>已有<strong><%=PeopleCount %></strong>人捐赠</p>
          <p>善款<strong><%=TotalAmount %></strong>元</p>
        </div>
        <form role="form">
          <div class="form-group">
            <input type="text" class="form-control input-lg" id="peoplename" placeholder="姓名" value="<%=CurrentUserInfo.WXNickname%>">
          </div>
          <div class="form-group">
            <input type="number" class="form-control input-lg"  pattern="\d*" id="cashnum"  onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" placeholder="捐款金额">
          </div>
          <div class="form-group">
            <span class="submitbtn  input-lg" id="btnSurePay">捐款</span>
          </div>
        </form>
      </div>
    </div>
    <!-- <modalbox></modalbox> -->
    <div class="modal fade" id="myModal"  role="dialog">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-body">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <p class="textaligncenter">请选择支付方式</p>
            <%if (IsLogin){%>
            <input class="howtopayinput" type="radio" name="howtopay" id="IWantCash1"  value="18">
            <label class="paybtn btn btn-success btn-lg" for="IWantCash1" >微信支付</label>
            <%} %>
            <input class="howtopayinput" type="radio" name="howtopay" id="IWantCash2" value="20" checked="checked">
            <label class="paybtn btn btn-warning btn-lg" for="IWantCash2" >支 付 宝</label>
           
          </div>
          <div class="modal-footer textaligncenter">
            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
            <button type="button" class="btn btn-primary" data-dismiss="modal" id="btnSubmit">支付</button>
          </div>
        </div><!-- /.modal-content -->
      </div><!-- /.modal-dialog -->
    </div><!-- /.modal -->


  </body>
    <script src="/Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(".submitbtn").click(function () {
            if ($("#peoplename").val() === "") {
                $("#peoplename").val("匿名")
            }
            if ($("#cashnum").val() === "") {
                $("#cashnum").focus()
                return
            }
            $('#myModal').modal('show')
        })
        var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
        var IsSubmit = false;
        $(function () {
            $("#btnSubmit").click(function () {
                var ajaxData =
                {
                    Action: "CustomizeDonations",
                    Name: $("#peoplename").val(),
                    PaymenttypeAutoId: $("input[name='howtopay']:checked").val(),
                    Money: $("#cashnum").val()

                }
                if (ajaxData.Money == "") {
                    $("#cashnum").focus();
                    return false;
                }
                if (IsSubmit == true) {
                    return false;
                }
                IsSubmit = true;
                $("#btnSurePay").html("正在提交...");
                $.ajax({
                    type: 'post',
                    url: mallHandlerUrl,
                    data: ajaxData,
                    timeout: 60000,
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.Status == 1) {

                            if ($("input[name='howtopay']:checked").val() == "18") {
                                window.location = "dowxpay.aspx?oid=" + resp.ExStr;
                            }
                            else {
                                window.location = "doalipay.aspx?oid=" + resp.ExStr;
                            }

                        }
                        else {
                            alert(resp.Msg);

                        }
                    },
                    complete: function () {
                        IsSubmit = false;
                        $("#btnSurePay").html("捐款");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (textStatus == "timeout") {
                            alert("操作超时，请重新提交");
                        }
                        else {
                            alert(textStatus + "请重新提交");
                        }
                    }
                });

            })

        })


    </script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=ArticleModel.ActivityName%>",
            desc: "<%=ArticleModel.Summary %>",
            //link: '', 
            imgUrl: "http://<%=Request.Url.Host%><%=ArticleModel.ThumbnailsPath %>"
        })
    })
</script>

</html>

