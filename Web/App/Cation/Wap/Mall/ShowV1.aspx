<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" CodeBehind="ShowV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ShowV1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">商品详情</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<style>
.shareuser{background-color:Gray;color:White;vertical-align:middle;width:auto;margin-bottom:10px;font-size:16px;}

#imgshareuser{width:auto;margin-top:2px;margin-left:2px;margin-bottom:2px;vertical-align:middle;}

</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="productinfo">
        <div class="img">
            <img id="productImg" src="" >
        </div>
        <div class="title">
            <h2 id="lblProductName"></h2>
            原价<del><span class="price" id="lblPrePrice">￥500</span></del>
            <br />
            现价<span class="price" id="lblProductPrice"></span>
            <span id="lblProductStock"></span>
        </div>
       
        <div class="describe">
         <%if (ShareUserInfo!=null)
           {%>
           <div class="shareuser">
           <table><tr><td style="width:55px;"><img id="imgshareuser" width="50" height="50" src="<%=ShareUserInfo.WXHeadimgurl %>"/></td><td style="vertical-align:middle;">来自 <%=ShareUserInfo.WXNickname %> 的推荐
            ☆☆☆☆☆</td></tr></table>
           </div>
           <%} %>
            <h2>商品详情</h2>
            <p id="lblProductDescription"></p>
        </div>
    </div>
    <a href="/App/Cation/Wap/Mall/Orderv1.aspx" id="cart">
    <span class="carticon"></span><span class="cartnum" style="display:block;">99</span></a>
    <div class="backbar">
        <a href="index.aspx" class="back"><span class="icon"></span></a>
        <a id="btnAddInOrder" href="javascript:" class="btn orange">加入购物车</a>
        <a id="btnBuyNow" href="javascript:void(0)" class="btn red">立即下单</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="/Scripts/wxmall/initv1.js" type="text/javascript"></script>
    <script>
        var type = '<%=websiteInfo.MallType %>'
        $(function () {
            Init();
        });

        function Init() {
            if (type == '1') {
                $("#btnAddInOrder").hide()
                $("#btnBuyNow").hide();
                $("#cart").hide();
            }
        }
    </script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=model.PName %>",
            desc: "<%=model.PName %>",
            link: "<%=shareLink %>",
            imgUrl: "http://<%=Request.Url.Host%><%=model.RecommendImg %>"
        }
        //    ,{
        //		message_s:function(){
        //			alert("好友分享成功")
        //		},
        //		message_c:function(){
        //			alert("好友分享取消")
        //		},
        //		timeline_s:function(){
        //			alert("朋友圈分享成功")
        //		},
        //		timeline_c:function(){
        //			alert("朋友圈分享取消")
        //		}
        //	}
    )
    })
</script>

</asp:Content>