<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="ProductDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.ProductDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    商品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .sliderbg1
        {
            height: auto;
        }
        
        .text_box
        {
            width: 85px;
        }
        #btnSub_Product, #btnAdd_Product
        {
            font-weight: bold;
            font-size: 20px;
        }
        .text_box_1
        {
            background-image: url(images/add.png);
            background-repeat: no-repeat;
            background-position: center center;
        }
        .text_box_2
        {
            background-image: url(images/del.png);
            background-repeat: no-repeat;
            background-position: center center;
        }
        .goods_span4
        {
            font-size: 20px;
        }
        .goods_div1, .goods_div2, .goods_div3
        {
            padding-left: 10px;
        }
        #lblProductDescription
        {
            margin-bottom: 100px;
        }
        .goods_div4
        {
        }
        .goods_span4
        {
            margin-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="swiper-slide sliderbg1">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <div class="index_container3">
                                <div class="panels_slider">
                                    <ul class="slides" id="ulslidesproduct">
                                    </ul>
                                </div>
                                <div class="index_container4">
                                    <div class="goods_div1">
                                        <div class="goods_div1_l">
                                            <h4 id="lblProductName">
                                            </h4>
                                            <p>
                                            </p>
                                        </div>
                                        <div class="goods_div1_r">
                                            <a href="javascript:void(0)" class="sharetofriend">
                                                <img src="images/goods_01.png" width="45px" /></a></div>
                                    </div>
                                    <div class="goods_div2">
                                        <div class="goods_div2_l">
                                            <span class="goods_span1" id="spbuycount"></span>
                                            <br />
                                           <%-- <span class="goods_span2" id="lblPrePrice"> </span>--%>
                                        </div>
                                        <div class="goods_div2_r" style="width: 60%; float: left; text-align: right">
                                            <span class="goods_span3">原价</span><span class="goods_span4" id="lblProductPrice">￥10.0</span></div>
                                    </div>
                                    <div class="goods_div3">
                                        购买数量
                                        <div style="float: right">
                                            <input id="btnSub_Product" name="" type="button" value="" class="btn_box text_box_1" />
                                            <input id="txtCount_Product" name="" type="text" value="1" class="text_box" style="min-width: 60px" />
                                            <input id="btnAdd_Product" name="" type="button" value="" class="btn_box text_box_2" />
                                        </div>
                                    </div>
                                    <div class="goods_div4 radius10">
                                        详情
                                    </div>
                                    <div class="image_single" id="lblProductDescription">
                                    </div>
                                </div>
                            </div>
                            <div style="position: fixed; bottom: 0px; width: 100%; background: #D8D6D0;">
                                <table class="goods_tb1">
                                    <tr>
                                        <td class="td1">
                                        </td>
                                        <td class="td2">
                                            <a href="javascript:void(0)" class="form_a1 green radius6" id="btnAddInOrder">加入购物车</a>
                                        </td>
                                        <td class="td3">
                                            <a href="javascript:void(0)" id="btnAddProductCollect">
                                                <img src="images/goods_02.png" width="35px" /></a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; height: 100%;min-height:2000px; display: none; background: #000; opacity: 0.7;
        position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
        &nbsp;
    </div>
    <div style="position: absolute; z-index: 1000000; min-height:2000px;right: 0; width: 100%; text-align: right;
        display: none;" id="sharebox">
        <img src="images/sharetip.png" width="100%" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
   <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script src="js/tuao.js" type="text/javascript"></script>
    <script src="js/jquery.flexslider.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#footer").hide();
            //获得文本框对象
            var t = $("#txtCount_Product");
            //数量增加操作
            $("#btnAdd_Product").click(function () {
                t.val(parseInt(t.val()) + 1)
                if (parseInt(t.val()) != 1) {
                    $('#btnSub_Product').attr('disabled', false);
                }

            })
            //数量减少操作
            $("#btnSub_Product").click(function () {
                if ((parseInt(t.val()) - 1) < 1) {
                    t.val("1");
                    return;
                }
                t.val(parseInt(t.val()) - 1);
                if (parseInt(t.val()) == 1) {
                    $('#btnSub_Product').attr('disabled', true);
                }

            })

            $(".sharetofriend").click(function () {
                $("#sharebg,#sharebox").show();
                $("#sharebox").css({ "top": $(window).scrollTop() })
            });

            $("#sharebg,#sharebox").click(function () {
                $("#sharebg,#sharebox").hide();
            });




        });
       
    </script>
  
</asp:Content>
