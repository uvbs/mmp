<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" CodeBehind="index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">首页</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
 <section class="box">
    <%=sbCategory.ToString()%>
     <div class="kindbox_s">
     <%=sbSecondCategory.ToString()%>
     </div>
         <div class="kindbox_s_noshow">
        <span class="btn orange" id="quxiao">取消</span>
    </div>
    <ul class="mainlist" id="productList">
        
    </ul>
    <div class="toolbar">
        <a href="javascript:" class="left current"><span class="homeicon"></span><br/>首页</a>
        <a href="/App/Cation/Wap/Mall/Orderv1.aspx" id="cart"><span class="carticon"></span><span class="cartnum" style="display:block;">0</span></a>
        <a href="MyCenter.aspx" class="right"><span class="mycenter"></span><br/>个人中心</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="/Scripts/wxmall/quo.js" type="text/javascript"></script>
<script src="/Scripts/wxmall/touchkind.js" type="text/javascript"></script>
    <script type="text/javascript">
     var categoryid = "<%=Request["cid"]%>";    
    </script>
    <script src="/Scripts/wxmall/initv1.js" type="text/javascript"></script>
    <script>
     $(function () {

             $(".fenleip").click(function () {
                 $(".kindbox .current").removeClass("current")
                 $(this).parent().addClass("current")
                 var indexid = $(this).attr("sid")
                 kindboxshow(indexid)
             })

             $("#quxiao").click(function () {
                 kindboxhide()
             })

             function kindboxshow(indexid) {
                 $(".kindbox_s").show()
                 $(".kindbox_s").show().find(".kind").hide()
                 $("#" + indexid).show()
                 $(".kindbox_s_noshow").show()
             }
             function kindboxhide() {
                 $(".kindbox_s").hide()
                 $(".kindbox_s_noshow").hide()
             }
         })
    </script>
    <script type="text/javascript">
    var type = '<%=currWebSiteInfo.MallType %>'
    $(function () {
        Init();
    });

    function Init() {
        if (type == '1') {
            $("#cart").hide();
        }
    }
</script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "<%=currWebSiteInfo.WXMallName%>",
                desc: "<%=currWebSiteInfo.WXMallName%>",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%><%=currWebSiteInfo.WXMallBannerImage%>"
            })
        })
    </script>
</asp:Content>
