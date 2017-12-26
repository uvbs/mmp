<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="AddCoupon.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.AddCoupon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    添加优惠券</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmall20150110.css" rel="stylesheet" type="text/css" />
    <style>
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="m_personinfo">
        <label for="ddlDiscount">
            <span class="title">折扣:</span>            
            <select name="" id="ddlDiscount">
                <option value="">请选择优惠券折扣</option>
                <option value="9.5">9.5折</option>
                <option value="9">9折</option>
                <option value="8.5">8.5折</option>
                <option value="8">8折</option>
                <option value="7.5">7.5折</option>
                <option value="7">7折</option>
            </select>
            </label>
        
       
        <a href="MyCenter.aspx" class="btn orange">返回</a><a id="btnSave" href="javascript:(0)" class="btn orange">添加优惠券</a>
    </div>


</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#btnSave").click(function () {
                try {

                    var model = {
                        Action: 'AddCoupon',
                        Discount: $.trim($('#ddlDiscount').val())

                    }
                    if (model.Discount == "") {
                        alert("请选择折扣");
                        return false;
                    }
                    $.ajax({
                        type: 'post',
                        url: '/handler/app/wxmallhandler.ashx',
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                alert("添加优惠券成功");
                                window.location = "CouponMgr.aspx";

                            }
                            else {

                                alert(resp.Msg);
                            }


                        }

                    });


                } catch (e) {
                    //alert(e);
                    alert(e);
                }

            });
        });
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
