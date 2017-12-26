<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.wap.vx.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>送话费，世贸商城送您“猴”彩头</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link rel="stylesheet" href="styles/css/style.css?v=0.0.10">
    <link rel="stylesheet" href="styles/css/animate.css">
    <link rel="stylesheet" href="styles/css/comm.css?v=0.0.10">
    <script src="/lib/layer.mobile/layer.m.js"></script>
    <script data-main="src/main" src="src/require.min.js"></script>
</head>
<body>
    <section class="box">
<!--     <div class="websiteiframebox">
        <iframe class="websiteiframe" src="http://zhengdao.comeoncloud.net/web/index.aspx" frameborder="0" scrolling="auto" ></iframe>
    </div> -->
    <div id="loadingscreen">
        <!-- <span class="loadtext">
            Loading...
        </span> -->
       <div class="spinner">
  <div class="rect1"></div>
  <div class="rect2"></div>
  <div class="rect3"></div>
  <div class="rect4"></div>
  <div class="rect5"></div>
</div>
    </div>

    <a href="javascript:;" id='btnNextpage' class="hidden" onclick="touchpic.mchange=2;touchpic.endfun();"></a>
    <div id="musicbutton" class="musicplay" style="left:0%;"></div>
    <audio id="myaudio" src="happynewyear.mp3?v=0.0.1" ></audio>
    <div id="imglist">        
        <div class="listli"><!-- 1 -->
            <span  onclick="touchpic.mchange=2;touchpic.endfun();" class="img" data-original="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findexbg.png);" style="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findexbg.png);"></span>
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fchibatangyuan.png" alt="" style="                                
position: absolute;
width: 50%;
/* bottom: 12%; */
right: 26%;
top: 15%; 
    z-index: 11;
"
                 onclick="touchpic.mchange=2;touchpic.endfun();"
                    class="animated infinite pulse" 
                >

            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findexbg.png" alt="" style="                                
    position: absolute;
    width: 44%;
    width: 100%;
    /* bottom: 64%; */
    /* right: 32%; */
    top: 0%;
    z-index: 10;
"
                onclick="touchpic.mchange=2;touchpic.endfun();"
                >

            <!-- <span class="text bottom5" style="color:#fff;" onclick="touchpic.mchange=2;touchpic.endfun();"><img src="styles/images/pic1btn.png" alt="" width="18%"></span> -->
            <!-- <span class="nextbtn"><span class="smallicon"></span></span> -->
        </div>

        <div class="listli" id="wrapTangyuan"><!-- 2 -->
            <span class="img" data-original="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findextybg.png);" style="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findextybg.png);"></span>            
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fdianwochoujiang.png" alt="" style="                                
    position: absolute;
    width: 44%;
    bottom: 64%;
    right: 32%;
    /* top: 16%; */
"
               class="imgText"

                >
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fbaisejiantou.png" alt="" style="                                
    position: absolute;
    width: 10%;
    bottom: 50%;
    right: 48%;
    /* top: 38%; */
    z-index: 101;
"
                class="imgText animated infinite pulse"

                >
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwan.png" alt="" style="                                
    position: absolute;
    width: 140%;
    bottom: -50%;
    right: -20%;
    /* top: 44%; */
    z-index: 94;
"
                >

             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan1.png" alt="" style="                                
    position: absolute;
    width: 46%;
    bottom: -2%;
        right: 50%;
    /* top: 44%; */
        z-index: 100;
"
                  class="animated infinite pulse tangyuan" 
                >
           <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan2.png" alt="" style="                                
    position: absolute;
    width: 52%;
    bottom: -2%;
     right: 8%; 
    /* top: 44%; */
        z-index: 99;
"
                class="animated infinite pulse tangyuan" 
                >
              <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan3.png" alt="" style="                                
       position: absolute;
    width: 40%;
    bottom: 13%;
    right: -1%;
    /* top: 44%; */
    z-index: 98;
"
                   class="animated infinite pulse tangyuan" 
                >
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan4.png" alt="" style="                                
    position: absolute;
    width: 38%;
    bottom: 8%;
    right: 34%;
    /* top: 44%; */
    z-index: 97;
"
                  class="animated infinite pulse tangyuan" 
                >
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan5.png" alt="" style="                                
    position: absolute;
    width: 38%;
    bottom: 13%;
    right: 60%;
    /* top: 44%; */
    z-index: 96;
"
                  class="animated infinite pulse tangyuan" 
                >
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ftangyuan6.png" alt="" style="                                
    position: absolute;
    width: 36%;
    bottom: 24%;
    right: 30%;
    /* top: 44%; */
    z-index: 95;
"
                  class="animated infinite pulse tangyuan" 
                >
        </div>
        
        <div class="listli" id="wrapWinning"><!-- 3 -->
            <span class="img" data-original="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findexbg.png);" style="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findexb_bianpaog.png);"></span>
        
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/bg.png" alt="" style="                                
    position: absolute;
    width: 98%;
    /* bottom: 12%; */
    right: 1%;
    top: 0%;
"
                >
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/jiang.png" alt="" style="                                
    position: absolute;
    width: 30%;
    /* bottom: 12%; */
    right: 35%;
    top: 12%;
"
                >
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/nextstep_wz.png" alt="" style="                                
    position: absolute;
        width: 38%;
    /* bottom: 12%; */
    right: 30%;
    top: 64%;
"
                class="animated infinite pulse" 
                onclick="touchpic.mchange=2;touchpic.endfun();"
                id="btnGotoSubmitInfo"
                >


            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/2/wz1.png" alt="" style="                                
    position: absolute;
    width: 75%;
    /* bottom: 12%; */
    right: 12%;
    top: 54%;
"
            id="winWz1"
                    >
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/2/wz2.png" alt="" style="                                
    position: absolute;
    width: 48%;
    /* bottom: 12%; */
    right: 24%;
    top: 48%;
"
                id="winWz2"
                >
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/2/hou.png" alt="" style="                                
    position: absolute;
        width: 28%;
    /* bottom: 12%; */
    right: 36%;
    top: 24%;
"
                id="winHou"
                >

            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/0/wz.png" alt="" style="                                
    position: absolute;
width: 62%;
    /* bottom: 12%; */
    right: 18%;
    top: 46%;
    display:none;
"
                id="lostWz"
                >

             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart/share_btn.png" alt="" style="                                
                position: absolute;
                width: 80%;
    /* bottom: 12%; */
    right: 10%;
    top: 85%;
    display:none;
                "
                 class=""
                >
        

            <span style="
                position: absolute;
          /* width: 38%; */
    /* bottom: 12%; */
    right: 34%;
    top: 62%;
    font-size: 20px;
    font-weight: bolder;
    color: #D90917;
        display: none;
                
                "
                class="animated infinite pulse btnAgain" 
                >
                点击再抽一次
            </span>
            <span style="
                position: absolute;
          /* width: 38%; */
    /* bottom: 12%; */
    right: 34%;
    top: 62%;
    font-size: 20px;
    font-weight: bolder;
    color: #D90917;
    right: 31%;
    display: none;
                "
                class="btnNoAgain" 
                >
                抽奖次数已用完
            </span>

        </div>
        
        <div class="listli" id="wrapSubmit"><!-- 4 -->
            <span  class="img" data-original="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findex_bolag.png);" style="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Findex_bolag.png);"></span>
            <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fbgimg1.png" alt="" style="                                
position: absolute;
    width: 92%;
    /* bottom: 12%; */
    right: 4%;
    top: 8%;
    z-index: 11;
" 
                class="androidProc"
               
                >

              <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fwz1.png" alt="" style="                                
position: absolute;
width: 52%;
    /* bottom: 12%; */
    right: 37%;
    top: 15%;
    z-index: 12;
" 
                  class="androidProc"
                >



               <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fbgimg2.png" alt="" style="                                
position: absolute;
    width: 92%;
    /* bottom: 12%; */
    right: 4%;
    top: 72%;
    z-index: 11;
" 
                   class="btnSubmit"
                   

                >

               <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fwz_submit.png" alt="" style="                                
position: absolute;
    width: 30%;
    /* bottom: 12%; */
    right: 34%;
    top: 74%;
    z-index: 12;
" 
                   class="btnSubmit"
                   

                >

              <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fwz_name.png" alt="" style="                                
position: absolute;
    width: 20%;
    /* bottom: 12%; */
    right: 64%;
    top: 44%;
    z-index: 12;
" 
                  class="lbName"
                >

              <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fwz_phone.png" alt="" style="                                
position: absolute;
    width: 20%;
    /* bottom: 12%; */
    right: 64%;
    top: 52%;
    z-index: 12;
" 
                  class="lbPhone"
                >

              <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fsubmit%2Fwz_descripte.png" alt="" style="                                
position: absolute;
    width: 76%;
    /* bottom: 12%; */
    right: 10%;
    top: 60%;
    z-index: 12;
" 
                  class="androidProc"
                >


            <input id="txtUserName" type="text" class="txtInput"/>

            <input id="txtUserPhone" type="number" max="11" class="txtInput"/>

            <!-- <span class="text bottom5" style="color:#fff;" onclick="touchpic.mchange=2;touchpic.endfun();"><img src="styles/images/pic1btn.png" alt="" width="18%"></span> -->
            <!-- <span class="nextbtn"><span class="smallicon"></span></span> -->
        </div>

        <div class="listli"><!-- 5 -->
            <span class="img" data-original="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fshare2.png);" style="background-image:url(http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fshare2.png);"></span>
           
            <!-- <span class="text bottom5" style="color:#fff;" onclick="touchpic.mchange=2;touchpic.endfun();"><img src="styles/images/pic1btn.png" alt="" width="18%"></span> -->
            <!-- <span class="nextbtn"><span class="smallicon"></span></span> -->
        </div>
        
    </div>
        <div class="wrapFooter androidProc">
             <img src="http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Ffooterwz.png" alt="">
        </div>
    <div id="sharebox" ><img src="styles/images/sharetext.png?v=0.0.1" alt=""></div>
</section>
<style>
    #sharebox{
        display:none;
        background-color:rgba(0,0,0,0.7);
        position: fixed;
        top: 0px;
        left: 0px;
        width: 100%;
        height: 100%;
    }
    #sharebox img{
        width: 60%;
        float: right;
        margin-right: 5%;
    }
    .pic8btn{
        display:inline-block;
        width: 35%;
        margin: 0 5px;
    }
    .pic8btn img{
        width: 100%;
    }
</style>
<script>
    var lottery = <%=ZCJson.JsonConvert.SerializeObject(resp.Result)%>;
    var awardList = [
        {
            id:0,
            name:'未中奖',
        },{
            id:2,
            name:'2元',
        },{
            id:5,
            name:'5元',
        },{
            id:8,
            name:'8元',
        },{
            id:10,
            name:'10元',
        }
        
    ];
    var currAwardInfo = null;//当前中奖信息
    var handlerUrl = '/serv/AWARDAPI.ashx';

    window.alert = function(msg){
        layer.open({
            style: 'border:none; background-color:red; color:#fff;',
            content:msg
        })
    };

    require.config({
        baseUrl:"./src/",
        shim:{ 
            wxshare:["jquery"]
        },
        paths:{
            jquery:"commonjs/jquery.min",
            WeShow:"commonjs/weshow",
            wx:"http://res.wx.qq.com/open/js/jweixin-1.0.0",
            AudioPlay:'commonjs/audioplay'
        }
    })
    require(["jquery","WeShow","wx","AudioPlay"],function($,WeShow,wx,AudioPlay){

        var page
        var bindCount = 0;
        var bindCount4 = 0;
        
        var $wrapFooter = $('.wrapFooter');

        touchpic=new WeShow("#imglist",function(_this,snum){
            var current=$(".listli:eq("+snum+")");
        
            touchpic.touchcontrol=false;
            switch (snum) {

            
                case 0:
                    $wrapFooter.show();
                    touchpic.touchcontrol=true;
                break;
           
                case 1:
                    $wrapFooter.hide();
                    touchpic.touchcontrol = false;
                break;

                case 2:
                    touchpic.touchcontrol=false;
                    $wrapFooter.hide();

                    break;

                case 3:
                    touchpic.touchcontrol = false;
                    $wrapFooter.show();

                    break;
            
                case 4:
                    touchpic.touchcontrol = false;
                    $wrapFooter.hide();

                    break;


                default:
                    touchpic.touchcontrol=false;
                    $wrapFooter.show();
                    break;
            }
        });

        touchpic.init();

        
        //初始化处理
        $(function(){

            if(lottery.currIsAward == 1){//已经中奖了
                currAwardInfo = getAwardByName(lottery.currAwardName);
                showAward();

                touchpic.maini = 1;touchpic.mchange=2;touchpic.endfun();

                if (lottery.currIsSubmitInfo == 1) {
                    $('#btnGotoSubmitInfo').hide();
                }

            }else if (lottery.luckRest < 1) {//摇奖次数为0
                $('#btnGotoSubmitInfo,#winWz1,#winWz2').hide();
                $('#lostWz').show();
                currAwardInfo = awardList[0];
                isSetAward = true;
                if(lottery.luckRest > 0)
                {
                    $('.btnAgain').show();
                    $('.btnNoAgain').hide();
                }else{
                    $('.btnAgain').hide();
                    $('.btnNoAgain').show();
                }

                $('#btnGotoSubmitInfo').hide();
                touchpic.maini = 1;touchpic.mchange=2;touchpic.endfun();

            }else{//还可以继续摇奖

            }
            
            var btnSubmitLog = false;
            $('.btnSubmit').on('click',function(){

                if (btnSubmitLog) {
                    return;
                }

                var reqData = {
                    action:'submitinfo',
                    id :lottery.id,
                    name: $.trim($('#txtUserName').val()),
                    phone:$.trim($('#txtUserPhone').val())
                };

                if (reqData.name == '') {
                    alert('请填入姓名');
                    btnSubmitLog = false;
                    return;
                }
                if (reqData.phone == '') {
                    alert('请填入手机');
                    btnSubmitLog = false;
                    return;
                }

                $.post(handlerUrl,reqData,function(data){

                    data = $.parseJSON(data);

                    if(data.IsSuccess){                        
                        touchpic.mchange = 2; touchpic.endfun();
                    }else{
                        alert(data.Msg);
                    }
                });

            });

            $('.tangyuan').each(function(){
                var $this = $(this);
                $this.attr({
                    "data-bottom":$this.css("bottom"),
                    "data-width":$this.css("width"),
                    "data-right":$this.css("right")
                });

            });

            //点击汤圆
            var tangyuanLock = false;
            $('.tangyuan').on('click', function () {
                if (tangyuanLock) {
                    return;
                }

                lottery.luckRest--;

                tangyuanLock = true;
                var $this = $(this);

                $('.imgText').fadeOut('fast', function () {
                    $this.animate({ bottom: "52%", width: '76%', right: "12%" });
                });

                $('.tangyuan').removeClass('pulse');
        
                setTimeout(function () {
                    $this.addClass('pulse');//flip

                    //抽奖并判断结果
                    $.get(handlerUrl,{
                        action:'scratch',
                        id:lottery.id
                    },function(data){
                        data = $.parseJSON(data);
                        console.log('抽奖结果',data);
                        var isSetAward = false;

                        if (data.isAward) {
                            $('#btnGotoSubmitInfo,#winWz1,#winWz2').show();
                            $('#lostWz').hide();
                            $('.btnAgain').hide();
                            $('.btnNoAgain').hide();

                            currAwardInfo = getAwardByName(data.awardName);
                            isSetAward = true;
                            
                        }else{//未中奖
                            $('#btnGotoSubmitInfo,#winWz1,#winWz2').hide();
                            $('#lostWz').show();
                            currAwardInfo = awardList[0];
                            isSetAward = true;
                            if(lottery.luckRest > 0)
                            {
                                $('.btnAgain').show();
                                $('.btnNoAgain').hide();
                            }else{
                                $('.btnAgain').hide();
                                $('.btnNoAgain').show();
                            }
                            

                        }

                        showAward();

                        //进入下一页展示抽奖结果
                        if(isSetAward){
                            touchpic.mchange = 2; touchpic.endfun();
                        }
                    });


                }, 1000);


            });

            //再抽一次
            $('.btnAgain').on('click',function(){
                $('.tangyuan').addClass('pulse');
                $('.imgText').show();
                $('.tangyuan').each(function(){
                    var $this = $(this);
                    $this.css({
                        "bottom":$this.attr("data-bottom"),
                        "width":$this.attr("data-width"),
                        "right":$this.attr("data-right")
                    });

                });
                tangyuanLock = false;



                touchpic.mchange=1;touchpic.endfun();
            });

            var oHeight = $(document).height(); //浏览器当前的高度
    
            $(window).resize(function(){
 
                if($(document).height() < oHeight){
                    //$("#wrapSubmit img,#wrapSubmit input,.wrapFooter").css("position","static");
                    $(".androidProc").hide();
                    $('.lbName,#txtUserName').css("top","36%");
                }else{
                    $(".androidProc").show();
                    $('.lbName,#txtUserName').css("top","44%");
                    //$("#wrapSubmit img,#wrapSubmit input,.wrapFooter").css("position","absolute");
                }
                
            });

        });

        function getAwardByName(name){
            for (var i = 0; i < awardList.length; i++) {
                if(awardList[i].name == name){
                    return awardList[i];                    
                }
            }
        }

        function showAward(){
            $('#winWz1').attr('src','http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/' + currAwardInfo.id + '/wz1.png');
            $('#winWz2').attr('src','http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/' + currAwardInfo.id + '/wz2.png');
            $('#winHou').attr('src','http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fwin/' + currAwardInfo.id + '/hou.png');
        }

        $.ajax({
            url: "http://"+location.host+"/serv/wxapi.ashx",
            data: {
                action : "getjsapiconfig",
                url:location.href
            },
            dataType : "json",
            success:function(wxapidata){
                wx.config({
                    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: wxapidata.appId, // 必填，公众号的唯一标识
                    timestamp: wxapidata.timestamp, // 必填，生成签名的时间戳
                    nonceStr: wxapidata.nonceStr, // 必填，生成签名的随机串
                    signature:  wxapidata.signature,// 必填，签名，见附录1
                    jsApiList: [
                        "onMenuShareTimeline",
                        "onMenuShareAppMessage",
                        "onMenuShareQQ",
                        "onMenuShareWeibo",
                        "startRecord",
                        "stopRecord",
                        "onVoiceRecordEnd",
                        "playVoice",
                        "pauseVoice",
                        "stopVoice",
                        "onVoicePlayEnd",
                        "uploadVoice",
                        "downloadVoice",
                        "chooseImage",
                        "previewImage",
                        "uploadImage",
                        "downloadImage",
                        "translateVoice",
                        "getNetworkType",
                        "openLocation",
                        "getLocation",
                        "hideOptionMenu",
                        "showOptionMenu",
                        "hideMenuItems",
                        "showMenuItems",
                        "hideAllNonBaseMenuItem",
                        "showAllNonBaseMenuItem",
                        "closeWindow",
                        "scanQRCode",
                        "chooseWXPay",
                        "openProductSpecificView",
                        "addCard",
                        "chooseCard",
                        "openCard"
                    ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });
            },
            error:function (errmes) {
                console.dir(errmes)
            }
        })

        wxs={
            title:"送话费，世贸商城送您“猴”彩头",
            link:location.href,
            imgUrl:"http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/img%2Flottery%2Fsmart%2Fshare_logo.png",
            desc:"世贸商城“闹元宵，猴彩头”抽奖活动，轻松赢取手机话费，为新年的工作生活讨个好“彩头”！"
        }

        wx.ready(function(){
            wx.onMenuShareTimeline({
                title: wxs.title, // 分享标题
                link: wxs.link, // 分享链接
                imgUrl: wxs.imgUrl, // 分享图标
                success: function() {
                    $("#sharebox").hide();
                },
                cancel: function() {

                }
            });
            wx.onMenuShareAppMessage({
                title: wxs.title, // 分享标题
                desc: wxs.desc, // 分享描述
                link: wxs.link, // 分享链接
                imgUrl: wxs.imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function() {
                    $("#sharebox").hide();
                },
                cancel: function() {

                }
            });
            wx.onMenuShareQQ({
                title: wxs.title, // 分享标题
                desc: wxs.desc, // 分享描述
                link: wxs.link, // 分享链接
                imgUrl: wxs.imgUrl, // 分享图标
                success: function() {
                    $("#sharebox").hide();
                },
                cancel: function() {

                }
            });
        })

        $("#sharebtn,.sharebtn").bind("touchend",function(){
            // console.log(0)
            $("#sharebox").show();
        })
        $("#sharebox").bind("touchend",function(){
            $("#sharebox").hide();
        })

    })


</script>
</body>
</html>
