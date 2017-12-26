<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="MyProductCollect.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.MyProductCollect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我的商品收藏
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
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
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


        //加载商品
        function LoadData() {
            $.ajax({
                type: 'post',
                url: '/handler/app/wxmallhandler.ashx',
                data: { Action: 'GetMyProductCollect', pageIndex: pageIndex, pageSize: pageSize },
                dataType: 'json',
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //
                        str.AppendFormat('<a href="javascript:void(0)" class="order">', resp.ExObj[i].PID);
                        str.AppendFormat('<div class="product">');
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].RecommendImg);
                        str.AppendFormat('<div class="info">');
                        str.AppendFormat('<span class="text">{0}</span>', resp.ExObj[i].PName);
                        var strAddToCar = "AddInOrder('" + resp.ExObj[i].PID + "','" + resp.ExObj[i].Price + "','" + resp.ExObj[i].PName + "','" + resp.ExObj[i].RecommendImg + "', 1)";
                        str.AppendFormat('<span class="price" onclick="{0}">加入购物车</span>', strAddToCar);
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('<div class="total">');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        //

                    };
                    if (str.ToString() == "" && pageIndex == 1) {
                        $('#objlist').html("暂无收藏");
                        $('#btnNext').remove();
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
