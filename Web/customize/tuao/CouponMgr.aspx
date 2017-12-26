<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="CouponMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.CouponMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    优惠券管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style>
        .divnext
        {
            text-align: center;
        }
        .orderinfobox, .orderbox
        {
            padding-bottom: 0px;
        }
        
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
        .orderinfobox .product img, .orderbox .product img{height:auto;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
            <div class="m_listbox">
        <a href="AddCoupon.aspx" class="list">
            <span class="mark green"><span class="icon add"></span></span>
            <h2>添加优惠券</h2>
            <span class="righticon"></span>
        </a>
    </div>
    <div id="objlist" class="orderbox">
    </div>
        <div class="divnext">
        <a id="btnNext" class="btn orange">显示更多</a>
        </div>

</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/tuao.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageIndex = 1;
        var pageSize = 10;

        $(function () {

            LoadData();

            $("#btnNext").click(function () {

                pageIndex++;
                LoadData();


            })

        })


        //加载数据
        function LoadData() {
            $.ajax({
                type: 'post',
                url: '/handler/app/wxmallhandler.ashx',
                data: { Action: 'GetMyCoupon', pageIndex: pageIndex, pageSize: pageSize },
                dataType: 'json',
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //
                        str.AppendFormat('<a href="CouponDetail.aspx?id={0}" class="order">', resp.ExObj[i].AutoId);
                        str.AppendFormat('<div class="product">');
                        str.AppendFormat('<img src="/customize/tuao/images/logo.png" >');
                        str.AppendFormat('<div class="info">');
                        str.AppendFormat('<span class="text">优惠券号码 <br/>{0}</span>', resp.ExObj[i].CouponNumber);
                        str.AppendFormat('<span class="text">折扣 {0}折</span>', resp.ExObj[i].Discount);
                        str.AppendFormat('<span class="price" onclick="{0}"></span>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('<div class="total">');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        //

                    };
                    if (str.ToString() == "" && pageIndex == 1) {
                        $('#objlist').html("<div style=\"text-align:center;\">暂无优惠券</div>");
                    }
                    else {
                        $('#objlist').append(str.ToString());
                    }
                    if (str.ToString() == "") {
                        $('#btnNext').html('没有更多');
                    }


                }
            });



        }


        
    </script>
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
