<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreProductDetail.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreProductDetail" %>

<!DOCTYPE html>
<html>
<head>
    <title>
        <%=model.PName%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <link href="/css/wxmall/wxmall20150110.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery2.1.1.js" type="text/javascript"></script>
    <script src="/WuBuHui/js/comm.js" type="text/javascript"></script>
    <script src="/WuBuHui/js/wubuslider.js?v=0.0.1" type="text/javascript"></script>
</head>
<body>
    <section class="box">
    <div class="productinfo">
        <div class="leftscorenum">
            剩余积分:<%=ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel().TotalScore %>
        </div>
        <div class="img" id="slider">
            <%if (!string.IsNullOrEmpty(model.RecommendImg)){%>
            <span class="sliderlist"><img  src="<%=model.RecommendImg%>"  > </span>
            <%} %>
            <%if (!string.IsNullOrEmpty(model.ShowImage1)){%>
            <span class="sliderlist"><img  src="<%=model.ShowImage1%>"  > </span>
            <%} %>
            <%if (!string.IsNullOrEmpty(model.ShowImage2)){%>
            <span class="sliderlist"><img  src="<%=model.ShowImage2%>"  > </span>
            <%} %>
            <%if (!string.IsNullOrEmpty(model.ShowImage3)){%>
            <span class="sliderlist"><img  src="<%=model.ShowImage3%>" > </span>
            <%} %>
            <%if (!string.IsNullOrEmpty(model.ShowImage4)){%>
            <span class="sliderlist"><img  src="<%=model.ShowImage4%>"  > </span>
            <%} %>
            <%if (!string.IsNullOrEmpty(model.ShowImage5)){%>
            <span class="sliderlist"><img  src="<%=model.ShowImage5%>" > </span>
            <%} %>
        </div>
        <div class="title">
            <h2><%=model.PName%></h2>
            <span class="price" >需要积分:<%=model.Score%></span>
        </div>
        <div class="describe">
            
            <p><%=model.PDescription%></p>
            
        </div>
    </div>




      <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("wubuhui") || ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("xixinxian"))
        {%>

        <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('/Wubuhui/MyCenter/Index.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">

           <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel().TotalScore<model.Score)
           {%>
            <a href="#" class="btn red" style="color:#fff;">积分不足 </a>
            <% }%>
            <%else{%>
             <a href="ScoreOrderDelivery.aspx?pid=<%=model.AutoID%>" class="btn red" style="color:#fff;">立即兑换 </a>
            <% }%>



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
        <a  href="ScoreOrderDelivery.aspx?pid=<%=model.AutoID%>" class="btn red">立即兑换</a>
    </div>

     <%} %>


</section>
</body>
<script type="text/javascript">
        $(function () {

           $("#slider").touchSlider({
            animatetime:300,
            automatic:false,
            timeinterval:4000,
            sliderpoint:true,
            sliderpointwidth:8,
            sliderpointcolor: "#ff7928"
            })

        })
   
</script>
</html>
