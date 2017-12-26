<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Course.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #F3F2F1;
            overflow: auto;
            font-family: 'STHeiti','Microsoft YaHei',Helvetica,Arial,sans-serif;
        }

        label {
            font-weight: normal;
        }

        .top {
            background-color: white;
        }

        .course-img{
            position: relative;
        }

        .course-img img {
            width: 100%;
            border-radius: 0px !important;
        }

        .share img {
            height: auto;
        }

        .collect img {
            height: auto;
        }

        .course-name {
            font-size: 20px;
            margin-left: 10px;
        }

        .summary {
            margin-top: 10px;
            width: 95%;
        }

        .summary-left {
            width: 60%;
            float: left;
        }

        .summary-right {
            width: 40%;
            float: left;
            text-align: center;
        }

        .category {
            color: #96928D;
        }

        .pre-price {
            color: #96928D;
            font-size: 14px;
        }

        .exam-cer-category {
            width: 100%;
            padding-bottom: 20px;
        }

        .summary-content {
            height: 50px;
            margin-left: 10px;
        }

        .btn-exam {
            background-color: #FF7D00;
            color: white;
            height: 40px;
            font-size: 18px;
            padding-top: 8px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 4px;
            width: 90%;
        }
         .btn-exam1{
            background-color: #FF7D00;
            color: white;
            height: 40px;
            font-size: 16px;
            padding-top: 8px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 4px;
            width: 90%;
        }
        .btn-ask {
            background-color: white;
            color: #00B53A;
            height: 40px;
            font-size: 18px;
            padding-top: 8px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 5px;
            width: 90%;
            border: 1px solid #00B53A;
        }

        .price {
            font-size: 22px;
            color: #FF7E00;
        }

        .money-flag {
            color: #FF7E00;
        }

        .pre-price {
            text-decoration: line-through;
            padding-left: 10px;
        }

        .exam-cer-category {
            margin-left: 10px;
        }

        .exam-cer-name-unselect {
            background-color: #EDEDED;
            color: #272624;
            height: 30px;
            font-size: 12px;
            padding-top: 6px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 5px;
            font-weight: normal;
        }

        .exam-cer-name-select {
            background-color: #FF7D00;
            color: white;
            height: 30px;
            font-size: 12px;
            padding-top: 6px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 4px;
            font-weight: normal;
        }

        .exam-cer-category-title {
            font-size: 18px;
            padding-top: 10px;
            padding-bottom: 10px;
        }

        .split {
            height: 1px;
        }

        .tab {
            background-color: white;
        }

        .course-tab {
            margin-top: 15px;
            min-height: 40px;
            margin-bottom: 10px;
        }

        .course-tab-title-unselect {
            width: <%=tabWidth%>%;
            text-align: center;
            float: left;
            font-size: 15px;
            margin-top: 10px;
            border-right: 1px solid #F3F2F1;
            border-bottom: 1px solid #F3F2F1;
            padding-bottom: 10px;
            overflow: hidden;
        }

        .course-tab-title-select {
            width: <%=tabWidth%>%;
            text-align: center;
            float: left;
            font-size: 15px;
            margin-top: 10px;
            border-right: 1px solid #F3F2F1;
            color: #FF7D00;
            border-bottom: 1px solid #FF7D00;
            padding-bottom: 10px;
            overflow: hidden;
        }

        .tab {
        }

        .course-tab {
        }

        .recommend {
            background-color: white;
            margin-top: 15px;
        }

        .recommend-title {
            font-size: 18px;
            padding-top: 10px;
            margin-left: 10px;
        }

        .recommend-list {
            margin-right: 10px;
        }

        .recommend-item {
            width: 25%;
            text-align: center;
            float: left;
            padding-left: 10px;
        }

            .recommend-item img {
                max-width: 100%;
                border-radius: 3px !important;
            }

            .recommend-item label {
                color: #428bca;
                word-break: break-all;
                display: -webkit-box;
                -webkit-line-clamp: 2;
                -webkit-box-orient: vertical;
                overflow: hidden;
            }
            .labelText{
                  color: #428bca;
                word-break: break-all;
                display: -webkit-box;
                -webkit-line-clamp: 2;
                -webkit-box-orient: vertical;
                overflow: hidden;
            }

        .course-detail {
            background-color: white;
            margin-top: 15px;
            margin-bottom:50px;
        }

        .course-detail-title {
            font-size: 15px;
            font-weight: bold;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-left: 10px;
        }

        .course-liucheng {
            background-color: white;
            margin-bottom: 80px;
        }

        .course-liucheng-title {
            font-size: 15px;
            font-weight: bold;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-left: 10px;
        }

        .course-detail-content {
            padding: 10px;
            word-break:break-all;
        }

        .bottom {
            position: fixed;
            bottom: 0;
            z-index: 1000000;
            width: 100%;
            border-top: 1px solid #ccc;
            background-color: #fff;
        }

        .bottom-left {
            text-align: center;
            float: left;
            width: 50%;
            padding-top: 5px;
        }

        .bottom-right {
            text-align: center;
            float: left;
            width: 50%;
            padding-top: 5px;
        }

        .tip {
            color: #FF7D00;
            font-weight: bold;
            font-size: 20px;
        }

        .course-tab-content {
            display: none;
            padding: 0px 10px 10px 10px;
            word-break:break-all;
        }

            .course-tab-content img {
                width: 100% !important;
            }

        .share {
            position: absolute;
            float: right;
            z-index: 100;
            bottom: 18px;
            right: 95px;
            background-color: #CFF2F7;
            padding-top: 5px;
            padding-bottom: 5px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 5px;
        }

            .share img {
                width: 16px;
            }

        .collect {
            position: absolute;
            float: right;
            z-index: 100;
            bottom: 18px;
            right: 18px;
            background-color: #CFF2F7;
            padding-top: 5px;
            padding-bottom: 5px;
            padding-left: 10px;
            padding-right: 10px;
            border-radius: 5px;
        }

            .collect img {
                width: 16px;
            }

        .layermbox0 .layermchild {
            /*width: 95% !important;
          
            height: 150px;*/
        }

        .layermbox0 .layermchild {
            /*width: 95% !important;*/
        }

        .layermchild h3 {
            height: 40px;
        }

        .ask-left {
            width: 43%;
            display: inline-block;
            margin-right: 12%;
            margin-left: -8%;
        }

        .ask-center {
            width: 10px;
            float: left;
        }

        .ask-right {
            width: 45%;
            display: inline-block;
        }

        .ask-online {
            background-color: white;
            color: #00B53A;
            height: 40px;
            font-size: 18px;
            padding-top: 8px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 5px;
            border: 1px solid #00B53A;
            width: 98%;
        }

            .ask-online img {
                width: 25px;
            }

        .ask-phone {
            background-color: white;
            color: #00B53A;
            height: 40px;
            font-size: 18px;
            padding-top: 8px;
            padding-left: 12px;
            padding-right: 12px;
            border-radius: 5px;
            border: 1px solid #00B53A;
            width: 100%;
        }

            .ask-phone img {
                width: 25px;
            }

            .ask-phone a {
                color: #00B53A;
            }

        .green {
            color: #00B53A;
        }

        .course-detail-content img {
            max-width: 98%;
        }

        .course-liucheng img {
            max-width: 100%;
        }

        .divButton {
            height: 10px;
            margin-bottom: 50px;
        }
         table{
            text-align: center;
        }
         .layui-m-layer *{
                 -webkit-box-sizing: inherit !important;
                box-sizing: inherit !important;
         }
         .layui-m-layer0 .layui-m-layerchild{
             padding-bottom:20px !important;
         }
         .layui-m-layercont{
             padding:0px 30px !important;
         }
         .recommend-img{
             width:80px;
             height:110px;
         }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="top">

        <div class="course-img">


            <img src="<%=productInfo.RecommendImg %>" />

            <div class="share" id="sharebtn">

                <img src="../images/share.png" />
                分享
            </div>
            <div class="collect">
                <%if (IsCollect == 1)
                  {%>
                <img src="../images/xingxingselect.png" id="imgCollect" />
                <span id="lblCollect">已收藏</span>
                <%}
                  else
                  {%>
                <img src="../images/xingxing.png" id="imgCollect" />
                <span id="lblCollect">收藏</span>
                <%}%>
            </div>
        </div>

        <div class="summary">
            <div class="course-name">
                <%=productInfo.PName %>
            </div>


            <div class="summary-content">

                <div class="summary-left">

                    <span class="money-flag">￥</span>
                    <span class="price" id="lblPrice"><%=Math.Round(productInfo.Price,1) %></span>
                    <span class="money-flag">元</span>


                    <span class="pre-price">￥<%=Math.Round(productInfo.PreviousPrice,1)%><br />
                    </span>

                    <span class="category"><%=productInfo.CategoryName %></span>
                </div>
                <div class="summary-right">

                    <label class="btn-exam">我要考证</label>
                </div>
            </div>
            <div class="split">
                <hr />
            </div>

            <div class="exam-cer-category">
                <div class="exam-cer-category-title">证书类型</div>

                <%foreach (var item in skuList)
                  { %>
                <label class="exam-cer-name-unselect" data-sku_id="<%=item.SkuId %>" data-sku_price="<%=item.Price %>" data-sku_name="<%=item.ShowProps %>"><%=item.ShowProps %></label>
                <% } %>
            </div>



        </div>


    </div>

    <%if (tabCount > 0)
      {%>

    <div class="tab">

        <div class="course-tab">

            <%if (!string.IsNullOrEmpty(productInfo.TabExTitle1))
              {%>
            <div class="course-tab-title-unselect" data-tab-title-index="1"><%=productInfo.TabExTitle1 %></div>
            <% } %>

            <%if (!string.IsNullOrEmpty(productInfo.TabExTitle2))
              {%>
            <div class="course-tab-title-unselect" data-tab-title-index="2"><%=productInfo.TabExTitle2 %></div>
            <% } %>


            <%if (!string.IsNullOrEmpty(productInfo.TabExTitle3))
              {%>
            <div class="course-tab-title-unselect" data-tab-title-index="3"><%=productInfo.TabExTitle3 %></div>
            <% } %>

            <%if (!string.IsNullOrEmpty(productInfo.TabExTitle4))
              {%>
            <div class="course-tab-title-unselect" data-tab-title-index="4"><%=productInfo.TabExTitle4 %></div>
            <% } %>

            <%if (!string.IsNullOrEmpty(productInfo.TabExTitle5))
              {%>
            <div class="course-tab-title-unselect" data-tab-title-index="5"><%=productInfo.TabExTitle5 %></div>
            <% } %>
        </div>

        <%if (!string.IsNullOrEmpty(productInfo.TabExTitle1))
          {%>
        <div class="course-tab-content" data-tab-content-index="1"><%=productInfo.TabExContent1 %></div>
        <% } %>

        <%if (!string.IsNullOrEmpty(productInfo.TabExTitle2))
          {%>
        <div class="course-tab-content" data-tab-content-index="2"><%=productInfo.TabExContent2%></div>
        <% } %>

        <%if (!string.IsNullOrEmpty(productInfo.TabExTitle3))
          {%>
        <div class="course-tab-content" data-tab-content-index="3"><%=productInfo.TabExContent3%></div>
        <% } %>


        <%if (!string.IsNullOrEmpty(productInfo.TabExTitle4))
          {%>
        <div class="course-tab-content" data-tab-content-index="4"><%=productInfo.TabExContent4%></div>
        <% } %>

        <%if (!string.IsNullOrEmpty(productInfo.TabExTitle5))
          {%>
        <div class="course-tab-content" data-tab-content-index="5"><%=productInfo.TabExContent5%></div>
        <% } %>




        <div style="clear: both;"></div>

    </div>

    <%} %>



    <%if (recommendProductList.Count > 0)
      {%>
    <div class="recommend">

        <div class="recommend-title">

            <span class="tip">|</span>
            推荐教材

        </div>
       <%-- <div style="overflow-x: auto; width: 100%;">
            <table style="<%= recommendProductList.Count<4? "width:"+(25*recommendProductList.Count)+"%": "" %>">
                <tr>
                    <%foreach (var item in recommendProductList)
                      {%>
                    <td style="padding: 5px; <%= recommendProductList.Count>4? "width:25%": "" %>">
                        <img src="<%=item.RecommendImg %>@110h_80w_1e_1c" />
                        <label><%=item.PName %></label>
                    </td>
                    <%} %>
                </tr>
            </table>
        </div>--%>

        <div class="recommend-list">
          <%--  <%foreach (var item in recommendProductList)
              {%>
            <div class="recommend-item" onclick="window.location.href='/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/<%=item.PID %>#/productDetail/<%=item.PID %>'">

                <img src="<%=item.RecommendImg %>@110h_80w_1e_1c" />

                <label><%=item.PName %></label>
            </div>
            <%} %>--%>

             <div style="overflow-x: auto; width: 100%;">
                <table style="<%= recommendProductList.Count<4? "width:"+(25*recommendProductList.Count)+"%": "" %>">
                    <tr>
                        <%foreach (var item in recommendProductList)
                          {%>
                        <td style="padding: 5px; <%= recommendProductList.Count>4? "width:25%": "" %>">
                            <a href="/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/<%=item.PID %>#/productDetail/<%=item.PID %>">
                                <img src="<%=item.RecommendImg %>" class="recommend-img" />
                            </a>
                            <div style="height:40px;">
                            
                                <label class="labelText"><%=item.PName %></label>
                            </div>
                        </td>
                        <%} %>
                    </tr>
                </table>
        </div>
        </div>
        <div style="clear: both;"></div>
    </div>
    <%} %>



    <%if (!string.IsNullOrEmpty(productInfo.PDescription))
      {%>
    <div class="course-detail">


        <%-- <div class="course-detail-title"><span class="tip">|</span>&nbsp;课程详情</div>--%>
        <div class="course-detail-content"><%=productInfo.PDescription %></div>

        <div style="clear: both;"></div>
    </div>
    <%} %>



    <%--   <div class="course-liucheng">


        <div class="course-liucheng-title"><span class="tip">|</span>&nbsp;考试流程</div>
       

        <div style="clear: both;"></div>
    </div>--%>
    <div class="divButton">
    </div>
    <div class="bottom">
        <div class="bottom-left">


            <label class="btn-ask" id="btnAsk">咨询</label>



        </div>
        <div class="bottom-right">
            <label id="btnSumbit" class="btn-exam">我要考证</label>
        </div>

    </div>


    <div style="width: 100%; height: 2000px; display: none; background: #000; opacity: 0.7; position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;"
        id="sharebg">
        &nbsp;
    </div>

    <div style="position: absolute; z-index: 1000000; right: 0; top: 0; width: 100%; text-align: right; display: none;"
        id="sharebox">
        <img src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/sharetip.png" width="100%" />
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.min.js"></script>



    <script>

        var skuId = 0;//选择的证书
        var skuPrice=0;//证书价格
        var skuName="";//证书名称
        var collectAction = "Add";
        if (<%=IsCollect%>==1) {
            collectAction="Delete";
        }
        $(function () {


            document.title="课程信息详情";
            //证书选择
            $(".exam-cer-name-unselect").click(function () {

                try {

                    $(".exam-cer-name-select").each(function () {
                        $(this).removeClass("exam-cer-name-select");
                        $(this).addClass("exam-cer-name-unselect");
                    })


                    $(this).removeClass("exam-cer-name-unselect");
                    $(this).addClass("exam-cer-name-select");
                    skuId = $(this).attr("data-sku_id");
                    skuPrice = $(this).attr("data-sku_price");
                    skuName = $(this).attr("data-sku_name");
                    $("#lblPrice").html(skuPrice);

                } catch (e) {
                    alert(e);
                }




            })

            //tab 点击
            $("[data-tab-title-index]").click(function () {

                try {

                    $("[data-tab-title-index]").each(function () {
                        $(this).removeClass("course-tab-title-select");
                        $(this).addClass("course-tab-title-unselect");
                    })


                    $(this).removeClass("course-tab-title-unselect");
                    $(this).addClass("course-tab-title-select");
                    var index = $(this).attr("data-tab-title-index");



                    $("[data-tab-content-index]").each(function () {
                        $(this).hide();

                    })


                    $("[data-tab-content-index='" + index + "']").show();


                } catch (e) {
                    alert(e);
                }




            })


            
            //咨询
            $("#btnAsk").click(function () {

                



                //
                layer.open({
                    title: [
                      "请选择咨询方式",
                      'margin-top:0px;'

                    ]
              , anim: 'up'
              , content: 
                  '<div class="ask"><div class="ask-left"><div class="ask-online"><a class=\"green\" href=\"http://wpa.qq.com/msgrd?v=3&uin=<%=config.QQ%>&site=qq&menu=yes\"><img src=\"../images/online.png\"/>在线咨询</a></div></div><div class="ask-center"></div><div class="ask-right"><div class="ask-phone"><a href="tel:<%=config.Tel%>"><img src="../images/phone.png"/>电话咨询</a></div></div></div>'

                });
                $(".layermbox0 .layermchild").css({"width":"95% !important","height":"150px;","max-width":"500px;"});

                $(".layui-m-layercont").css({"padding":"0 30px 26px;"});

            })

            //去付款
            $(".btn-exam").click(function () {


                if (skuId==0) {

                    alert("请选择证书类型");
                    return false;
                }

                var data=[];
                var orderData={
                    num:1,
                    img_url:"<%=productInfo.RecommendImg%>",
                    quote_price:<%=productInfo.PreviousPrice%>,
                    title:"<%=productInfo.PName%>",
                    count:1,
                    price:parseFloat(skuPrice),
                    score:0,
                    product_id:"<%=productInfo.PID%>",
                    is_cashpay_only:<%=productInfo.IsCashPayOnly%>,
                    is_no_express:<%=productInfo.IsNoExpress%>,
                    totalcount:1,
                    properties:"证书类型:"+skuName,
                    sku_id:parseFloat(skuId),
                    tags:"<%=productInfo.Tags%>",
                    sale_id:"",
                    grouptype:"Course"
                   

                }
                data.push(orderData);
                layer.open({ type: 2 });
                $.ajax({
                    type: 'post',
                    url: "/serv/api/course/check.ashx",
                    data: { sku_id: orderData.sku_id},
                    timeout: 30000,
                    dataType: "json",
                    success: function (resp) {
                        layer.closeAll();
                        if (resp.status==true) {
                            sessionStorage.setItem("orderProducts",JSON.stringify(data));
                            window.location.href="/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/"+<%=productInfo.PID%>+"#/createOrder";

                        }
                        else {
                            
                            alert(resp.msg);
                            return false;
                        }


                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        
                    }
                });








            })


            
            //收藏
            $(".collect").click(function () {
                
                $.ajax({
                    type: 'post',
                    url: "/serv/api/mall/collect.ashx",
                    data: {Action:collectAction,id:"<%=Request["id"]%>"},
                    dataType: "json",
                    success: function (resp) {
                        if (resp.errcode==0) {

                            if (collectAction=="Add") {
                                $("#imgCollect").attr("src", "../images/xingxingselect.png");
                                collectAction = "Delete";
                                $("#lblCollect").text("已收藏");
                            }
                            else {

                                $("#imgCollect").attr("src", "../images/xingxing.png");
                                collectAction = "Add";
                                $("#lblCollect").text("收藏");
                            }

                        }


                    }

                });




            })



            //显示默认tab
            if ($("[data-tab-title-index]").length > 0) {
                $("[data-tab-title-index='1']")[0].click();
            }

            //分享
            $("#sharebtn,#sendtofriendbtn").click(function () {
                $("#sharebg,#sharebox").show();
                $("#sharebox").css({ "top": $(window).scrollTop() })
            });
            $("#followbtn").click(function () {
                $("#sharebg,#followbox").show();
                $("#followbox").css({ "top": $(window).scrollTop() })
            });
            $("#sharebg,#sharebox,#followbox").click(function () {
                $("#sharebg,#sharebox,#followbox").hide();
                $("#sendtofriendbtn,#sharebtn").removeClass("ui-btn-active");
            });
            $(".ui-loader-default").remove();

        })


        wx.ready(function () {
            wxapi.wxshare({
                title: "<%=productInfo.PName%>",
                desc: "<%=productInfo.Summary%>",
                //link: pageData.shareUrl,
                imgUrl: "<%=productInfo.RecommendImg%>",
            })
        });

    </script>
</asp:Content>
