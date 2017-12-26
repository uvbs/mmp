<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Vote/Comm/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=currVote.VoteName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/css/wubu.css" />
    <link rel="stylesheet" href="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/css/swiper.min.css" />

    <style>
        
            <%
                if(!string.IsNullOrWhiteSpace(currVote.VotePageBgColor))
                {
                    %>
                         body
                            {
                         background-color:<%=currVote.VotePageBgColor%>;
                          }
                            .sliderbg1 {
                                background: <%=currVote.VotePageBgColor%>;
                            }
                            .menu5{
                                background:<%=currVote.VotePageBgColor%>;
                            }
                    <%
                }else{

                     %>
                         body
                            {
                         background-color:White;
                         }
                     <%
                 }
            %>
       body{
            background-color: ##A8FF6E;

            }
        .wbtn_main {
            background-color: #FFDE3B;
            color: #0F0F0F;
            border:none;
            box-shadow: 2px 4px 4px rgba(18, 21, 21, 0.5);
           
        }
        .wbtn {
            border-radius: 6px;
        }
        
        .panels_slider ol.flex-control-nav
        {
            margin-top: 5px;
        }
        .code2
        {
            right: 0;
            margin-top: -120px;
            margin-right: 20px;
            text-align: right;
            position: absolute;
        }
        #imgqrcode
        {
            width: 100px;
        }
        .filter-div
        {
            border: 1px #000 solid;
            position: fixed;
            top: 0;
            left: 0;
            z-index: 99999;
            width: 100%;
            height: 100%;
            background: #000;
            filter: alpha(opacity=60);
            opacity: 0.6;
            display: none;
        }
        .popup
        {
            width: 80%;
            left: 10%;
            top: 15%;
            height: auto;
            min-height: 30%;
            background: #53a3da;
            padding-top: 10%;
            padding-bottom: 10%;
            position: fixed;
            z-index: 99999;
            filter: alpha(opacity=80);
            opacity: 0.8;
            display: none;
        }
        .popup1
        {
            width: 80%;
            left: 10%;
            top: 15%;
            height: auto;
            padding-top: 10%;
            padding-bottom: 10%;
            position: fixed;
            z-index: 99999;
            display: none;
        }
        .radius8
        {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }
        .op_td img
        {
            max-width: 60%;
        }
        .op_td .td1
        {
            padding-left: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
        .op_td
        {
            width: 100%;
        }
        table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .op_td .td2
        {
            padding-right: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
        .op_td .td3
        {
            padding-top: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
        .center{
            text-align:center;
            margin-top:20px;
        }
        .yellowBtn{
            display: inline-block;
            background-color: #DBDE0B;
            padding: 3% 10%;
            color: #020E0E;
            font-size: 16px;
            border-radius: 8px;
            box-shadow: 4px 4px 5px -1px #906626;
        }
        .wrapVoteResult{
            width: 100%;
            text-align: center;
            font-size: 22px;
            color: #fff;
            height: 70px;
            line-height: 35px;
        }
        .layermshow .layermchild {
            width:100%;
        }
        .sharebtn {
            margin-top:100px;
            text-align:center;
            width:95%;
            margin-left:2.5%;
            padding-bottom: 10px;
        }
            
        #sharefriend,#sharetimeline {

           background-color: #FFDE3B;
           color: #0F0F0F;
          
           border-radius: 6px;
    box-shadow: 2px 4px 4px rgba(18, 21, 21, 0.5);
    display: block;
    text-align: center;
    font-size: 13px;
    line-height: 35px;
    color: #000;
    font-weight: bolder;
        }
        
        .sharebtn .wbtn:first-child {
    background-image: url(http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/weixin.png);
    margin-right: 2%;
}
        .sharebtn .wbtn {
    background-image: url(http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/weixin2.png);
    background-size: auto 30px;
    padding-left: 40px;
    background-repeat: no-repeat;
    background-position: 5px center;
        background-position-x: 30px;
    width: 48%;
}


         #sharetimeline {
        }
    </style>
    <style>
	    .links{
	    	padding: 8px;
	    	background-color: #52b529;
	    	margin: 5px;
	    	-webkit-transition: all 1s;
	    	-o-transition: all 1s;
	    	transition: all 1s;
	    }
	    .links a{
	    	color: #fff;
	    	display: inline-block;
	    	width: 100%;
	    	height: 100%;
	    	text-decoration: none;
	    }
	    .links:hover{
		background-color: #1e824c;
	    }
	    .current{
	    	background-color: #22a7f0;
	    }
	    .swiper-container {
	        width:300px;
	        max-width: 100%;
	        height: 300px;
	        max-height: 100%;
	        margin: 20px auto;
	    }
	    .swiper-slide {
	        text-align: center;
	        font-size: 18px;
	        background: #fff;

	        /* Center slide text vertically */
	        display: -webkit-box;
	        display: -ms-flexbox;
	        display: -webkit-flex;
	        display: flex;
	        -webkit-box-pack: center;
	        -ms-flex-pack: center;
	        -webkit-justify-content: center;
	        justify-content: center;
	        -webkit-box-align: center;
	        -ms-flex-align: center;
	        -webkit-align-items: center;
	        align-items: center;
	    }
        .swiper-slide
	    {
	        background-position-x: center;
	    }
        .swiper-wrapper img {
            width:100%;
        }
        .col-md-12 {
            padding-left:0px;
        }
        .htmleaf-container {

            margin-bottom:300px;
        }
        @media screen and (min-width: 320px) { 
        .swiper-container  {width: 320px} 
        } 
        @media screen and (min-width: 360px) { 
        .swiper-container  {width: 360px} 
        } 
        @media screen and (min-width: 375px) { 
        .swiper-container  {width: 375px} 
        } 
                @media screen and (min-width: 414px) { 
        .swiper-container  {width: 414px} 

        } 
        /* css注释：设置了浏览器宽度不小于1201px时 abc 显示1200px宽度 */ 



	    </style>

     <%=styleCustomize.ToString()%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

    <%if (!string.IsNullOrWhiteSpace(currVote.BgMusic))
        { %>
     <audio id="audioBg" src="<%=currVote.BgMusic %>" ></audio>
    <div id="musicbutton" class="musicplay" style="left:0%;" onclick="changeMusicCtrl()"></div>
    <%} %>

    <%
        ZentCloud.BLLJIMP.BLLVote bllVote = new ZentCloud.BLLJIMP.BLLVote();
        ZentCloud.BLLJIMP.Model.VoteObjectInfo model = bllVote.GetVoteObjectInfo(int.Parse(Request["id"]));
        if (model == null)
        {
            Response.End();
        }
        if (model.Status!=1)
        {
            Response.Write("审核未通过");
            Response.End();
        }
    %>

    <div class="wrapDetail">

    <div class="image_single">

        <img src="<%=currVote.VoteObjDetailBannerImg %>" alt="" title="" border="0" />

    </div>
    <div class="list1 ">


        <div class="htmleaf-container">

		<div class="container" style="height: 25vh;">
			<div class="">
				<div class="">
					<!-- Swiper -->
					    <div class="swiper-container">
					        <div class="swiper-wrapper">
					            
                        <%
                        if (!string.IsNullOrEmpty(model.VoteObjectHeadImage))
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.VoteObjectHeadImage));
                        }
                        if (!string.IsNullOrEmpty(model.Ex1))
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.Ex1));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage1) && model.ShowImage1!= "/img/hb/hb1.jpg")
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.ShowImage1));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage2) && model.ShowImage2 != "/img/hb/hb1.jpg")
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.ShowImage2));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage3) && model.ShowImage3 != "/img/hb/hb1.jpg")
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.ShowImage3));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage4) && model.ShowImage4 != "/img/hb/hb1.jpg")
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.ShowImage4));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage5) && model.ShowImage5 != "/img/hb/hb1.jpg")
                        {
                            Response.Write(string.Format(" <div class=\"swiper-slide\" style=\"background-image:url({0});background-size: contain;background-repeat: no-repeat;\"></div>", model.ShowImage5));
                        } 
                                                
                    %>


					            
					        </div>
					        <!-- Add Pagination -->
					        <div class="swiper-pagination"></div>
					    </div>
				</div>
				
			</div>
		</div>
        </div>

	</div>




        <div class="page_padding6 slogan">
            <input type="hidden" value="<%=model.Contact%>" id="currContact" />
            <h4>
              <%--  <%=model.Contact+"," %>&nbsp;--%>
                <%=model.VoteObjectName %>,<%=model.Area %>&nbsp;第<%=model.Number %>号</h4>
            <%if (!string.IsNullOrWhiteSpace(model.SchoolName))
                {

                 %>
            <br />
            <span>
                学校：<%=model.SchoolName %>
            </span>

            <%} %>
            <%if (!string.IsNullOrWhiteSpace(model.Ex2))
                {

                 %>
            <br />
            <span>
                指导老师：<%=model.Ex2 %>
            </span>

            <%} %>
            <p class="slogan">
                <%=model.Introduction %>
            </p>
        </div>
        <div class="page_padding8 sliderbg1">
            <div class="menu5">
                <ul>
                    <li>
                        <table>
                            <tr>
                                <td>
                                 <%--   <a href="#tab1">
                                        <img src="images/detail_03.png" alt="" title="" /></a>--%>
                                    
                                    <svg class="icon iconRank font26" aria-hidden="true">
                                        <use xlink:href="#icon-aixin2"></use>
                                    </svg>
                                </td>
                                <td>

                                    <span class="count font18" id="lblvotecount">
                                        <%=model.VoteCount %>票</span>
                                </td>
                            </tr>
                        </table>
                    </li>
                    <li >
                        <table>
                            <tr>
                                <td>
                                   <%-- <a href="#tab2">
                                        <img src="images/detail_04.png" alt="" title="" /></a>--%>
                                    <svg class="icon iconRank colorGreen font26" aria-hidden="true">
                                        <use xlink:href="#icon-xingbiao"></use>
                                    </svg>
                                </td>
                                <td>
                                    <span class="level colorGreen font18">第<%=model.Rank %>名</span>
                                </td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </div>
        </div>
        <%if (currVote.VoteStatus != 2)
            { %>
       <%-- <div class="page_padding8 sliderbg1">
            <div class="menu4">
                <ul>
                    <li id="li1" class="active"  style="margin-left: 38%;">
                        <a href="javascript:void(0)">
                        
                            <span class="btnToVote font20  pTop4 pBottom4" id="imgvote" >投我一票</span>

                        </a>
                    </li>
                </ul>
            </div>
        </div>--%>
        <div class="form_div3 mTop16">
            <span class="btnToVote font20  pTop4 pBottom4" id="imgvote" >投我一票</span>
        </div>
        



        
        <%} %>
             <div class="sharebtn">
            <span id="sendtofriendbtn" class="wbtn wbtn_main weixinsharezhidao themeBgColor themeFontColor">好友拉票</span> 
            
            <span id="sharebtn" class="wbtn wbtn_main weixinsharezhidao  themeBgColor themeFontColor">
                马上分享
            </span>
        </div>
        <% if (currVote.WebsiteOwner == "dali")
            { %>
        <div class="form_div3 mTop16">
            <a class="btnToVote font20  pTop4 pBottom4" style="color:Red" href="http://shop111225.cn/jP84SS" >我要领福利 </a>
        </div>
        <%} %>
    </div>
        
            <%if (!string.IsNullOrEmpty(model.OtherInfoLink))
              {
                  %>
                    <div class="center">
                        <a style="color:blue;" href="<%=model.OtherInfoLink %>"><%=currVote.OtherInfoLinkText %></a>
                     </div>
                  <%
              }
                
            %>
        
        <div class="pBottom10">
    <%if (!string.IsNullOrWhiteSpace(currVote.PartnerImg) && currVote.PartnerImg != "/img/hb/hb1.jpg")
    { %>
            <img class="width100P" src="<%=currVote.PartnerImg %>" />
    <%}else{ %>
    <br />
    <br />
    <%} %>
        </div>
    </div>


    <div id="filter" class="filter-div">
    </div>

    <div id="success1" class="popup radius8">
    </div>

    <div id="success" class="popup1">
        <a href="javascript:void(0);" onclick="closePopup()" style="float: right; margin-top: -30px;
            margin-right: 10px;">
            <img src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/delete-black.png" style="width: 30px" /></a>
        <div class="image_single wrapVoteResult">
            <%--<img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Fvote%2Ftest%2Fvotesuc.png" id="imgvoteresult" />--%>
            <div id="lbVoteResult">
                投票成功<br>
                感谢您的支持！
            </div>
        </div>
        
        <% if (currVote.IsHideSignUp == 0)
            {
            %>
<%--        <table class="op_td">
            <tr>
                <td align="center" colspan="2" class="td3">
                    <a class="yellowBtn" href="SignUp.aspx?vid=<%=currVote.AutoID %>">
                      
                        我也要参加
                    </a>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="td3">
                    <a class="yellowBtn toLottery hide" href="http://mp.weixin.qq.com/s?__biz=MjM5ODAxODY2MQ==&mid=507366622&idx=2&sn=29e573b6fcbdb279a4cdf6a9d16f8977#rd">
                        
                        我要去抽奖
                    </a>
                </td>
            </tr>
        </table>--%>
        <%} %>
    </div>

    <%=footerHtml.ToString()%>
            <div style="width: 100%; height: 2000px; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; text-align: right;
            display: none;" id="sharebox">
            <img src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/sharetip.png" width="100%" />
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; text-align: right;
            display: none;" id="followbox">
            <img src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/sharetip.png" width="100%" />
        </div>
    <script type="text/javascript">
        var number=<%=model.Number%>;
        var intro="<%=model.Introduction %>";
        var headImg="<%=model.VoteObjectHeadImage %>";
        var autoId="<%=model.AutoID %>";
    </script>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
  <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/js/swiper.min.js"></script>
    
    <script type="text/javascript">

        var currVoteId = '<%=currVote.AutoID%>';
        var currVoteType = '<%=currVote.VoteType%>';
        $(function(){

            var h = parseInt(screen.width / 0.8);
            $('.swiper-container').css({
                "width":screen.width + 'px'
                ,"height":h + 'px'
            });
            
            $('.htmleaf-container').css({
                "margin-bottom":h + 'px'
                ,"height": "42px"
            });

            if (currVoteId == '135') {
                $('.toLottery').show();
            }
            var swiper = new Swiper('.swiper-container', {
                pagination: '.swiper-pagination',
                paginationClickable: true,
                autoplay: 2500,                loop:true
            });

            //$('.swiper-slide,.swiper-container').css({
            //    "width":screen.width + 'px'
            //});
            $("#imgvote").click(function(){
        
                Vote();
        
            })

<%--            //加载二维码
            $.ajax({
                type: 'post',
                url: '/handler/qcode.ashx',
                data: { code: "http://<%=Request.Url.Host %>/customize/beachhoney/detail.aspx?id="+autoId },
                success: function (path) {
                    $("#imgqrcode").attr("src", path);
                }
            });--%>

        });
        //投票
        function Vote() {

            var layerIndex = layer.open({type:2,shadeClose:false});

            $.ajax({
                type: 'post',
                url: handlerurl,
                data: { Action: "UpdateVoteObjectVoteCount", id: "<%=Request["id"]%>",vid:'<%=currVote.AutoID%>' },
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    
                    layer.close(layerIndex);

                    if (resp.errcode == 0) {
                        //投票成功
                        var oldcount = $("#lblvotecount").text();
                        var newcount = parseInt(oldcount) + 1;
                        $("#lblvotecount").html(newcount+"票");
                        if (currVoteType=="2") {//投票后填写姓名抽奖
                            openSumbitInfoDlg("投票成功");
                            return false;
                        }
                        openPopup();
                        

                    }
                    else {
                        //$("#imgvoteresult").attr("src","http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Fvote%2Ftest%2Fvoteed.png");
                        $("#lbVoteResult").html(resp.errmsg);
                        var smsCount=<%=SMSCount%>;
                        if (smsCount==0&&currVoteType=="2") {
                            openSumbitInfoDlg(" 你已经投过票");
                            return false;
                        }
                        openPopup();
                        //layermsg(resp.errmsg);
                    }
                   
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("投票超时，请重新投票");

                    }
                }
            });


        }
        //分享

         function openPopup() {
            $('#filter').css("display", "block");
            $('#success').css("display", "block");
             $('#success1').css("display", "block");
            $(".sliderbg1").hide();


        }
        function closePopup() {
            $('#filter').css("display", "none");
            $('#success').css("display", "none");
             $('#success1').css("display", "none");
            $(".sliderbg1").show();
            
        }

        //打开提交信息框
        function openSumbitInfoDlg(msg){
           
            layer.open({
                title: [
                  msg,
                  'background-color:#FFDE3B; color:#fff;'
                ]
                ,anim: 'up'
                ,content: '<input type="text" id="txtPhone" placeholder="输入手机号抽奖">'
                ,btn: ['提交信息'],
                yes: function(){
                    
                var phone=$("#txtPhone").val();
                if (phone=="") {
                    $("#txtPhone").focus();
                    return false;
                    }
      //
        $.ajax({
          type: 'post',
          url: handlerurl,
          data: { Action: "SubmitInfoDali",vid:"<%=Request["vid"]%>",Phone:phone },
                        timeout: 30000,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.errcode==0) {
                                layer.closeAll();
                                layermsg("信息已经提交");
                            }
                            else {
                                if (resp.errcode=="code:1002") {
                                    layermsg("短信余额不足,请稍后再重新提交手机号码");
                                }
                                else {
                                    layermsg(resp.errmsg);
                                }
                               
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                    
                        }
                    });

                   }
            });
          
        }

       var shareTitle="我是"+number+"号大赛选手，快投我一票吧！";
        var shareDesc="<%=currVote.VoteName%>";//intro;
       var shareImgUrl=headImg;
       var shareLink=window.location.href;
      

         //分享
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
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
        });


</script>

</asp:Content>
