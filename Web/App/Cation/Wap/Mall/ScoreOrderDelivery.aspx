<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreOrderDelivery.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreOrderDelivery" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>提交订单</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <link href="/css/wxmall/wxmall20150110.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wxmall/orderdelivery.js" type="text/javascript"></script>
    <script src="/WuBuHui/js/comm.js" type="text/javascript"></script>
    <script src="/Scripts/jsAddress.js?v=0.001" type="text/javascript"></script>
    <style type="text/css">
        input[type=text], textarea, select
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        select
        {
            float: left;
            margin-left: 5px;
        }
        textarea
        {
            margin-top: 5px;
        }
        .lblred
        {
            color: Red;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="deliverytype nopaddingbottom">
        <div href="#" class="addressbox noaddressbox" id="person">
            <span class="noaddress">点击添加收货人信息</span>
            <span class="name">收货人:<span class="nameinfo"><%=currentUserInfo.TrueName%></span></span>
            <span class="phone"><%=currentUserInfo.Phone%></span>
            <span class="address">地址 :
            <span class="addressinfo1"><%=currentUserInfo.Province%></span>
            <span class="addressinfo2"><%=currentUserInfo.City%></span>
            <span class="addressinfo3"><%=currentUserInfo.District%></span>
            <span class="addressinfo"><%=currentUserInfo.Address%></span></span>
            <span class="icon"></span>
        </div>
        <div class="orderinfo">
         <textarea name="" id="txtOrderMemo" placeholder="备注" rows="2"></textarea>
        </div>
    </div>

    <div class="checkinfo" id="personbox">
        <input id="txtLinkerName" class="textinput personname" type="text" placeholder="姓名" value="<%=currentUserInfo.TrueName==null?"":currentUserInfo.TrueName%>"/>
        <input id="txtLinkerPhone" class="textinput persontell" type="tel" placeholder="联系电话" value="<%=currentUserInfo.Phone==null?"":currentUserInfo.Phone%>"/>
        <br />
         <select id="ddlProvince" class="personprovince personaddress1">
         </select>
         
         <select id="ddlCity" class="personaddress2">
         </select>
         <select id="ddlDistrict" class="personaddress3">
         </select>
        <textarea id="txtLinkerAddress" class="personaddress" placeholder="街道地址"><%=currentUserInfo.Address%></textarea>
        <div class="btn orange close">取消</div>
        <div class="btn orange close saveaddress">确定</div>
    </div>

    <div class="orderinfobox paddingbottom">
        <div class="order" id="orderconfirm">
         <div class="product">
         <img src="<%=model.RecommendImg%>" >
         <div class="info">
         <span class="text"><%=model.PName%></span>
          
         <span class="price"><%=model.Score%>&nbsp;积分</span>
         </div>
         </div>
        <div style="margin: 5 5 5 5;">购买数量:<input type="text" id="txtCount" value="1" maxlength="5" style="font-weight:bold;color:Red;margin-bottom:5px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
        <br />
        库存:<label class="lblred"><%=model.Stock%></label>
        <br />
        需要积分:<label class="lblred" id="lblneedscore"><%=model.Score%></label>
        <br />
        目前积分:<label class="lblred" ><%=currentUserInfo.TotalScore%></label>
        
        </div> 
    </div>
    </div>
    
      <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("wubuhui") || ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("xixinxian") || ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("hf"))
        {%>

      <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('/Wubuhui/MyCenter/Index.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
             <a href="javascript:void(0)" id="btnSumbitOrder2" class="btn red" style="color:#fff;" >
            </span>提交订单 </a>

        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="/Wubuhui/MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->
     <%}%>
     <%else
         {%>
         <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
        <a href="javascript:void(0)" id="btnSumbitOrder" class="btn orange">提交订单</a>
    </div>

     <%} %>

</section>
    <!-- footerbar -->
    <div class="wcontainer discusscontainer maxh100">
        <div class="modal fade " id="gnmdb" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body textcenter">
                        <p>
                            蜜，下了订单就没法取消了哟，积分也不还的哟。</p>
                    </div>
                    <div class="modal-footer textcenter">
                        <span class="wbtn wbtn_main" data-dismiss="modal">取消</span> <span class="wbtn wbtn_main"
                            id="btnSumbitOrder">提交订单</span>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </div>
</body>
<script src="/WuBuHui/js/bootstrap.js"></script>
<script src="/WuBuHui/js/gotopageanywhere.js"></script>

<script src="/Wubuhui/js/weixinsharebtn.js" type="text/javascript"></script>
<script src="/WuBuHui/js/partyinfo.js"></script>
<script type="text/javascript">

    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    var singlescore=<%=model.Score%>;
    var IsSumbit=false;
    $(function () {

        $("#btnSumbitOrder2").click(function(){
                             
         $('#gnmdb').modal('show');
            
        })

        //提交订单
        $('#btnSumbitOrder').click(function () {

            try {

                var count = $("#txtCount").val();
                if ($.trim(count) == "") {
                    $("#txtCount").focus();
                    return false;

                }
                if (parseInt(count) <= 0) {
                    $("#txtCount").focus();
                    return false;
                }
                var ajaxData = {
                    PID: "<%=model.AutoID%>",
                    Consignee: $.trim($('#txtLinkerName').val()),
                    Phone: $.trim($('#txtLinkerPhone').val()),
                    Province:$("#ddlProvince").val(),
                    City:$("#ddlCity").val(),
                    District:$("#ddlDistrict").val(),
                    Address:$.trim($('#txtLinkerAddress').val()),
                    OrderMemo: $.trim($('#txtOrderMemo').val()),
                    Count: count,
                    Action: 'SubmitWxMallScoreOrder'
                }
                if (ajaxData.Consignee == '') {
                    alert('请输入收货人姓名');
                    return;
                }
                if (ajaxData.Phone == '') {
                    alert('请输入手机号码');
                    return;
                }

                if (!isPhone(ajaxData.Phone)) {
                    alert('手机号码格式有误');
                    return;
                }
                if (ajaxData.Address=="null") {
                    alert('请输入收货地址');
                    return;
                }
                if (ajaxData.Province=="请选择省") {
                    alert('请选择省份');
                    return;
                }
                if (IsSumbit) {
                     return false;
                }
                IsSumbit=true;
                $("#btnSumbitOrder").html("正在提交...");
                $.ajax({
                    type: 'post',
                    url: mallHandlerUrl,
                    data: ajaxData,
                    timeout: 60000,
                    dataType:"json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            window.location = 'successwbh.htm';
                            //alert("订单提交成功");
                            // window.location = "success.aspx?oid=" + resp.ExStr + "&gopage=ScoreMall.aspx&orderdetailpage=MyScoreOrderDetails.aspx";
                            return;
                        }
                        else {
                            alert(resp.Msg);
                            return;
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (textStatus == "timeout") {
                            alert("操作超时，请重新提交订单");
                        }
                        else {
                            alert(textStatus + "请重新提交订单");
                        }
                    },
                    complete:function(){IsSumbit=false;$("#btnSumbitOrder").html("提交订单");}
                }); //

            } catch (e) {
                alert(e);
            }



        })


    <%if (!string.IsNullOrEmpty(currentUserInfo.Province)){%>
     addressInit('ddlProvince', 'ddlCity', 'ddlDistrict', "<%=currentUserInfo.Province%>", "<%=currentUserInfo.City%>", "<%=currentUserInfo.District%>");
    <%}else{ %>
     addressInit('ddlProvince', 'ddlCity', 'ddlDistrict', "请选择省", "", "");
     <%} %>

        $("#txtCount").keyup(function () {
           
            $("#lblneedscore").text($(this).val() * singlescore);

        })
        

    })

    //判断是不是手机号码
    function isPhone(value) {
        return /^(13|15|18)\d{9}$/i.test(value);
    }
</script>
</html>
