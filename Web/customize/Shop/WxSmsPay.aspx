<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="WxSmsPay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Shop.WxSmsPay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="css/WxSmsPay.css?v=20170308" type="text/css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="warp-sms">
        <div class="sms-head">
           <div class="sms-pay-title">
               短信充值
           </div>
        </div>

        <div class="sms-body">
            <div class="sms-pay">
                选择套餐：
            </div>
            <div class="weui-flex">
                <div class="weui-flex__item sms-item" data-price="100" data-total="1000">
                    <div class="placeholder sms-number">1000条</div>
                    <div class="placeholder sms-price">售价:100元</div>
                </div>
                <div class="weui-flex__item sms-item" data-price="500" data-total="6000">
                    <div class="placeholder sms-number">6000条</div>
                    <div class="placeholder sms-price">售价:500元</div>
                </div>
                <div class="weui-flex__item sms-item" data-price="1000" data-total="15000">
                    <div class="placeholder sms-number">15000条</div>
                    <div class="placeholder sms-price">售价:1000元</div>
                </div>
            </div>

            <div class="weui-flex warp-sms_count">

                <div class="sms-buy-count">
                    购买份数：
                </div>
                <div class="weui-flex__item">
                    <div class="sms-jia">
                        +
                    </div>
                    <div class="sms-input">
                        <input type="text" value="1" id="smsBuyCount" maxlength="3" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[\D0]/g,'')}else{this.value=this.value.replace(/[^0-9]/g,'')}" />
                    </div>
                     <div class="sms-jian">
                        —
                    </div>
                </div>

            </div>

            <div class="clear"></div>

            <div class="weui-flex warp-sms_pay">
                <div class="pay-number">
                    <span>支付：</span>
                    <span class="number">0</span>
                    <span>元</span>
                </div>

            </div>

        </div>

        <div class="sms-foot">
            <a href="javascript:;" id="pay" class="weui-btn weui-btn_plain-primary">充值</a>
        </div>

        <div>
             <div class="weui-msg__extra-area">
                <div class="weui-footer">
                    <p class="weui-footer__links">
                        <a href="javascript:void(0);" class="weui-footer__link">上海至云信息科技有限公司</a>
                    </p>
                    <p class="weui-footer__text">Copyright &copy; 2017</p>
                </div>
            </div>
        </div>

    </div>


    <div class="sms-success">
        <div class="weui-msg">
            <div class="weui-msg__icon-area"><i class="weui-icon-success weui-icon_msg"></i></div>
            <div class="weui-msg__text-area">
                <h2 class="weui-msg__title">支付成功</h2>
                <%--<p class="weui-msg__desc">内容详情，可根据实际需要安排，如果换行则不超过规定长度，居中展现<a href="javascript:void(0);">文字链接</a></p>--%>
            </div>
            <%--<div class="weui-msg__opr-area">
                <p class="weui-btn-area">
                    <a href="javascript:history.back();" class="weui-btn weui-btn_primary">推荐操作</a>
                    <a href="javascript:history.back();" class="weui-btn weui-btn_default">辅助操作</a>
                </p>
            </div>--%>
            <div class="weui-msg__extra-area">
                <div class="weui-footer">
                    <p class="weui-footer__links">
                        <a href="javascript:void(0);" class="weui-footer__link">上海至云信息科技有限公司</a>
                    </p>
                    <p class="weui-footer__text">Copyright &copy; 2017</p>
                </div>
            </div>
        </div>
    </div>


    <div class="sms-fail">
        <div class="weui-msg">
            <div class="weui-msg__icon-area"><i class="weui-icon-warn weui-icon_msg"></i></div>
            <div class="weui-msg__text-area">
                <h2 class="weui-msg__title">支付失败</h2>
                <%--<p class="weui-msg__desc">内容详情，可根据实际需要安排，如果换行则不超过规定长度，居中展现<a href="javascript:void(0);">文字链接</a></p>--%>
            </div>
            <%-- <div class="weui-msg__opr-area">
                <p class="weui-btn-area">
                    <a href="javascript:history.back();" class="weui-btn weui-btn_primary">推荐操作</a>
                    <a href="javascript:history.back();" class="weui-btn weui-btn_default">辅助操作</a>
                </p>
            </div>--%>
            <div class="weui-msg__extra-area">
                <div class="weui-footer">
                    <p class="weui-footer__links">
                        <a href="javascript:void(0);" class="weui-footer__link">上海至云信息科技有限公司</a>
                    </p>
                    <p class="weui-footer__text">Copyright &copy; 2017</p>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var orderId = '';
        var userid = '<%=userId%>';
        $(function () {
            $('.sms-item').click(function () {
                $(this).addClass('sms-item-selected').siblings().removeClass('sms-item-selected');
                var dataPrice = $(this).attr('data-price');
                var buyCount = $('#smsBuyCount').val();
                if (!dataPrice || !buyCount) return;

                var totalPrice = parseFloat(dataPrice) * parseFloat(buyCount);

                $('.number').text(totalPrice);

            });
            $('.sms-item')[0].click();

            $('.sms-jian').click(function () {
                var buyCount = $('#smsBuyCount').val();
                var count = parseFloat(buyCount);
                if (count <= 1 || !count) return;
                count = count - 1;
                $('#smsBuyCount').val(count);
                var dataPrice = $('.sms-item-selected').attr('data-price');
                if (!dataPrice) return;
                var totalPrice = parseFloat(dataPrice) * parseFloat(count);
                $('.number').text(totalPrice);
            });

            $('.sms-jia').click(function () {
                var buyCount = $('#smsBuyCount').val();
                var count = parseFloat(buyCount);
                if (!count) return;
                count = count + 1;
                $('#smsBuyCount').val(count);
                var dataPrice = $('.sms-item-selected').attr('data-price');
                if (!dataPrice) return;
                var totalPrice = parseFloat(dataPrice) * parseFloat(count);
                $('.number').text(totalPrice);
            });

            $('#smsBuyCount').keyup(function () {
                var count = $(this).val();
                if (!count) $('.number').text('0');
                var dataPrice = $('.sms-item-selected').attr('data-price');
                if (!count || !dataPrice) return;
                var totalPrice = parseFloat(dataPrice) * parseFloat(count);
                $('.number').text(totalPrice);
            });


            $('#pay').click(function () {
                var buyCount = $('#smsBuyCount').val();
                var dataPrice = $('.sms-item-selected').attr('data-price');
                var dataTotal = $('.sms-item-selected').attr('data-total');
                var totalPrice = $('.number').text();
                var smsTotal = parseFloat(buyCount) * parseFloat(dataTotal);
                if (!buyCount || !dataPrice || !dataTotal) return;
                var data = {
                    type: '7',
                    pay_type: 0,
                    userid: userid,
                    sms_number: buyCount,
                    sms_price: dataPrice,
                    sms_count: dataTotal
                };
                $.ajax({
                    type: 'POST',
                    url: '/serv/api/user/order/addSms.ashx',
                    data: { data: JSON.stringify(data) },
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.status) {
                            orderId = resp.result;
                            //
                            //发起微信支付


                            $.ajax({
                                type: 'post',
                                url: "/serv/api/user/pay/weixin/paysmsrecharge.ashx",
                                data: { order_id: orderId },
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status) {
                                        // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
                                        WeixinJSBridge.invoke('getBrandWCPayRequest', resp.result.pay_req, function (res) {

                                            // 返回res.err_msg,取值 

                                            // get_brand_wcpay_request:cancel 用户取消 

                                            // get_brand_wcpay_request:fail 发送失败 

                                            // get_brand_wcpay_request:ok 发送成功 
                                            if (res.err_msg == "get_brand_wcpay_request:ok") {

                                                //支付成功
                                                $('.warp-sms').hide();
                                                $('.sms-fail').hide();
                                                $('.sms-success').show();

                                            }
                                            else {

                                                //支付失败或取消
                                                $('.warp-sms').hide();
                                                $('.sms-success').hide();
                                                $('.sms-fail').show();

                                            }

                                        });
                                        //


                                    }
                                    else {

                                        //支付失败或取消
                                        $('.warp-sms').hide();
                                        $('.sms-success').hide();
                                        $('.sms-fail').show();
                                    }
                                    //



                                }
                            });




                            //




                        }
                    }
                });
            });

        });


    </script>
</asp:Content>
