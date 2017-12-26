<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="UserSignIn.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.UserSignIn.UserSignIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Plugins/swiper/css/swiper.min.css" rel="stylesheet" />
    <link href="css/UserSignIn.css?v=2017041721" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .swiper-slide {
            text-align: center;
            font-size: 18px;
            background: #F0F0F0;
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

        .swiper-container {
            max-width: 100%;
            max-height: 100%;
        }

        .adsImg {
            max-width: 100%;
        }

        .popuo-login {
            border: none;
            background-color: #78BA32;
            color: #fff;
            width: 70% !important;
        }
            .popuo-login .layui-m-layercont {
                padding: 0px;
            }
            .imgStyle{
                border-radius:64px !important;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="container">

        <div class="page__td">

            <div class="slides">
                <div class="slidebox">
                    <div class="htmleaf-container">
                        <div class="swiper-container">
                            <div class="swiper-wrapper">
                            </div>
                            <!-- Add Pagination -->
                            <div class="swiper-pagination"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="page__hd">
            <img class="bgImg" src="" />
            <div class="weui-flex">
                <div class="weui-flex__item">
                    <div class="placeholder-blue">签到</div>
                    <div class="placeholder-gray">已签到</div>
                </div>
            </div>
            <div class="scores">
            </div>
        </div>


        <div class="page__bd">
            <div class="bd_title">签到记录</div>
            <%--<div class="bd_desc">已签到<span class="day">0</span>天,连续一周签到即可抽取每周大奖。</div>--%>
            <div class="solidSign">
            </div>

            <div class="bd_week">
                <div class="weui-flex top">
                </div>
                <div class="weui-flex buttom">
                </div>
            </div>
        </div>



        <div class="page__fd">
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">

    <script src="http://static-files.socialcrmyun.com/lib/swiper/js/swiper.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var url = '/serv/api/signin/get.ashx';

        var id = '';
        var address = '';
        var signinCount = 0;
        var lotteryId = 0;
        var lotteryType = '';
        var rscore = 0;
        var swiper = null;
        var haveSign = '';
        var noHaveSign = '';


        var ads = null;


        $(function () {


            loadWeekList();

            //签到
            $('.placeholder-blue').click(function () {
                SignIn();
            });
            //补签
            $(document).on('click', '.weui-btn_primary', function () {

                var weekDate = $(this).attr('week_name_date');

                var weekDay = $(this).attr('data-day');

                var tmpLock = false;

                var open = layer.open({
                    content: '您确定要使用' + rscore + '积分进行补签嘛？'
                    , btn: ['补签', '取消']
                    , yes: function () {
                        layer.close(open);
                        if (tmpLock) {
                            return;
                        }
                        tmpLock = true;
                        $.ajax({
                            type: 'POST',
                            url: '/serv/api/Signin/Retroactive.ashx',
                            data: { week_name_date: weekDate, type: 'Sign', address: address, id: id },
                            dataType: 'json',
                            success: function (resp) {
                                tmpLock = false;
                                if (resp.status) {
                                    rscore = resp.msg;
                                    if (ads) {
                                        layer.open({
                                            className: 'popuo-login',
                                            content: '<div style="font-size: -webkit-xxx-large;"><img src="' + ads[0].img + '"  class="adsImg" data-link="' + ads[0].url + '" ></div>'
                                        });
                                    } else {
                                        layer.open({
                                            content: '补签成功'
                                     , skin: 'msg'
                                     , time: 1 //1秒后自动关闭
                                        });
                                    }
                                   
                                    conventClass(weekDay);

                                } else {
                                    layer.open({
                                        content: resp.msg
                                     , skin: 'msg'
                                     , time: 1 //1秒后自动关闭
                                    });
                                }
                            }
                        });
                    }
                });
            });

            $(document).on('click', '.swiper-slide', function () {
                var link = $(this).attr('data-link');
                if (link) {
                    zcToUrl(link);
                }
            });

            $(document).on('click', '.adsImg', function () {
                var link = $(this).attr('data-link');
                if (link) {
                    zcToUrl(link);
                }
            });
        });



        
        var tmpSignInLock = false;
        //签到
        function SignIn() {
            if (tmpSignInLock) {
                return;
            }
            tmpSignInLock = true;
            $.ajax({
                type: 'POST',
                url: '/serv/api/signin/AddSignIn.ashx',
                data: { type: 'Sign', address: address, id: id },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {

                        //var html = '';
                        //html+=' <div class="slides">';
                        //html+=' <div class="slidebox">';
                        //html+='<div class="htmleaf-container">';
                        //html+='<div class="swiper-container">';
                        //html+='<div class="swiper-wrapper">';
                        //html+='</div>';
                        //html+='<div class="swiper-pagination"></div>';
                        //html+='</div>';
                        //html+='</div>';
                        //html+='</div>';
                        //html+='</div>';
                        if (ads) {
                            layer.open({
                                className: 'popuo-login',
                                content: '<div style="font-size: -webkit-xxx-large;"><img  src="' + ads[0].img + '"  class="adsImg" data-link="' + ads[0].url + '" ></div>'
                            });
                        } else {
                            layer.open({
                                content: '签到成功'
                                   , skin: 'msg'
                                   , time: 1 //1秒后自动关闭
                            });
                        }
                        
                        $('.placeholder-blue').hide();
                        $('.placeholder-gray').show();
                        var week = new Date().getDay();
                        conventClass(week);
                        tmpSignInLock = false;
                    }
                }

            });
        }



      


        var s_width = 0;
        var s_height = 0;

        function getSlide(slideName) {
            $.ajax({
                type: 'POST',
                url: '/serv/api/common/SlideList.ashx',
                data: { type: slideName },
                dataType: 'json',
                success: function (result) {
                    if (result.result.length > 0) {
                        s_width = result.result[0].width;
                        s_height = result.result[0].height;
                        var _w = $(window).width();
                        var zw = _w * 2;
                        var zh = Math.round(s_height / s_width * zw);
                        var last = '?x-oss-process=image/resize,m_pad,w_' + zw + ',h_' + zh + ',color_ffffff/format,png';

                        var num = 0;
                        for (var i = 0; i < result.result.length; i++) {
                            if (!result.result[i].img) continue;
                            num++;
                            $('.swiper-wrapper').append('<div data-link="' + result.result[i].link + '" class="swiper-slide"><img style="width:100%;" src="' + result.result[i].img + last + '" /></div>');
                        }

                        if (num > 0) {
                            swiper = new Swiper('.swiper-container', {
                                pagination: '.swiper-pagination',
                                paginationClickable: true,
                                autoplay: 2500,                                loop: true
                            });
                        }
                    }
                }
            });
        }

        function conventClass(week) {

            signinCount = (parseInt(signinCount) + parseInt(1));

            $('.day').text(signinCount);

            week = parseInt(week);
            switch (week) {
                case 1:
                    var score = $('.Monday').attr("data-score");
                    if (haveSign) {
                        $('.Monday img').attr("src", haveSign);
                    } else {
                        $('.Monday img').attr("src", "image/dui.png");
                    }
                    $('.Monday span').text("+" + score + "积分");
                    $('.Monday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Monday a').hide();
                    $('.Monday .score').show();

                    break;
                case 2:
                    var score = $('.Tuesday').attr("data-score");
                    if (haveSign) {
                        $('.Tuesday img').attr("src", haveSign);
                    } else {
                        $('.Tuesday img').attr("src", "image/dui.png");
                    }
                    $('.Tuesday span').text("+" + score + "积分");
                    $('.Tuesday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Tuesday a').hide();
                    $('.Tuesday .score').show();
                    break;
                case 3:
                    var score = $('.Wednesday').attr("data-score");
                    if (haveSign) {
                        $('.Wednesday img').attr("src", haveSign);
                    } else {
                        $('.Wednesday img').attr("src", "image/dui.png");
                    }
                    $('.Wednesday span').text("+" + score + "积分");
                    $('.Wednesday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Wednesday a').hide();
                    $('.Wednesday .score').show();
                    break;
                case 4:
                    var score = $('.Thursday').attr("data-score");
                    if (haveSign) {
                        $('.Thursday img').attr("src", haveSign);
                    } else {
                        $('.Thursday img').attr("src", "image/dui.png");
                    }
                    $('.Thursday span').text("+" + score + "积分");
                    $('.Thursday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Thursday a').hide();
                    $('.Thursday .score').show();
                    break;
                case 5:
                    var score = $('.Friday').attr("data-score");
                    if (haveSign) {
                        $('.Friday img').attr("src", haveSign);
                    } else {
                        $('.Friday img').attr("src", "image/dui.png");
                    }
                    $('.Friday span').text("+" + score + "积分");
                    $('.Friday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Friday a').hide();
                    $('.Friday .score').show();
                    break;
                case 6:
                    var score = $('.Saturday').attr("data-score");
                    if (haveSign) {
                        $('.Saturday img').attr("src", haveSign);
                    } else {
                        $('.Saturday img').attr("src", "image/dui.png");
                    }
                    $('.Saturday span').text("+" + score + "积分");
                    $('.Saturday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Saturday a').hide();
                    $('.Saturday .score').show();

                    break;
                case 0:
                    var score = $('.Sunday').attr("data-score");
                    if (haveSign) {
                        $('.Sunday img').attr("src", haveSign);
                    } else {
                        $('.Sunday img').attr("src", "image/dui.png");
                    }
                    $('.Sunday img').attr("src", haveSign);
                    $('.Sunday span').text("+" + score + "积分");
                    $('.Sunday span').removeClass("noHaveSign").addClass("haveSign");
                    $('.Sunday a').hide();
                    $('.Sunday .score').show();
                    break;
            }
        }

        function CheckIsShow(isSignIn) {
            var week = new Date().getDay();
            switch (week) {
                case 1:
                    if (!isSignIn) {
                        $('.Monday').hide();
                    }
                    break;
                case 2:
                    if (!isSignIn) {
                        $('.Tuesday').hide();
                    }
                    break;
                case 3:
                    if (!isSignIn) {
                        $('.Wednesday').hide();
                    }
                    break;
                case 4:
                    if (!isSignIn) {
                        $('.Thursday').hide();
                    }
                    break;
                case 5:
                    if (!isSignIn) {
                        $('.Friday').hide();
                    }
                    break;
                case 6:
                    if (!isSignIn) {
                        $('.Saturday').hide();
                    }

                    break;
                case 0:
                    if (!isSignIn) {
                        $('.Sunday').hide();
                    }
                    break;
            }
        }

        function checkPicurl(url) {

            var img = new Image();
            img.src = url;
            return img.height;
        }

        function loadWeekList() {
            $.ajax({
                type: 'POST',
                url: url,
                data: { type: 'Sign' },
                dataType: 'json',
                success: function (result) {
                    console.log(['result', result]);
                    if (result.result.desc) {
                        $('.page__fd').append(result.result.desc);
                    }
                    if (result.result.background_image) {
                        var url = result.result.background_image + "?v=" + Date.parse(new Date())
                        $('.bgImg').attr("src", url);
                    }

                    haveSign = result.result.have_sign_image;
                    noHaveSign = result.result.no_have_sign_image;

                    id = result.result.id;

                    address = result.result.address;

                    if (result.result.is_signin) {
                        $('.placeholder-gray').show();
                    } else {
                        $('.placeholder-blue').show();
                    }

                    signinCount = result.result.sign_day;

                    lotteryId = result.result.lottery_id;

                    rscore = result.result.retroactive_score;

                    lotteryType = result.result.lottery_type;
                    if (result.result.button_color) {
                        $(document).find(".placeholder-blue").css("background-color", result.result.button_color);
                        $(document).find(".weui-flex__item").css("border-color", result.result.button_color);
                    }

                    $('.day').text(signinCount);

                    GetWeekList(result.result.weeks, haveSign, noHaveSign);

                    if (result.result.thumbnail) {
                        $(document).find(".lottery_img").attr('src', result.result.thumbnail);
                    }

                    if (result.result.slide_name) {
                        getSlide(result.result.slide_name);
                    }
                    var week = new Date().getDay();
                    switch (week) {
                        case 1:
                            if (result.result.monday_ads) ads = JSON.parse(result.result.monday_ads);
                            break;
                        case 2:
                            if (result.result.tuesday_ads) ads = JSON.parse(result.result.tuesday_ads);
                            break;
                        case 3:
                            if (result.result.wednesday_ads) ads = JSON.parse(result.result.wednesday_ads);
                            break;
                        case 4:
                            if (result.result.thursday_ads) ads = JSON.parse(result.result.thursday_ads);
                            break;
                        case 5:
                            if (result.result.friday_ads) ads = JSON.parse(result.result.friday_ads);
                            break;
                        case 6:
                            if (result.result.saturday_ads) ads = JSON.parse(result.result.saturday_ads);
                            break;
                        case 0:
                            if (result.result.sunday_ads) ads = JSON.parse(result.result.sunday_ads);
                            break;
                        default:
                            break;
                    }
                }
            });
        }



        function GetWeekList(data, haveSign, noHaveSign) {

            for (var i = 0; i < data.length; i++) {
                var html = "";
                html += "<div class=\"weui-flex__item\">";
                html += "<div class=\"placeholder_top\">";
                html += "<div class=\"z_week\">" + data[i].week_name + "</div>";
                html += "<div class=\"z_month\">(" + data[i].month_name + ")</div>";
                html += "</div>";
                html += "<div class=\"placeholder_buttom " + data[i].class_name + "\" data-score=" + data[i].score + " >";


                if (data[i].is_signin) {
                    if (data[i].is_signin) {
                        if (haveSign) {
                            html += " <img class=\"imgStyle\"  src=" + haveSign + "><br />";
                        } else {
                            html += " <img class=\"defimg\"  src=\"image/dui.png\"><br />";
                        }
                        html += "<span class=\"haveSign\">+" + data[i].score + "积分</span>";
                    } else {

                        html += "<a href=\"javascript:;\" data-day=" + data[i].week_day + "   week_name_date=" + data[i].week_name_date + " class=\"weui-btn weui-btn_mini weui-btn_primary\">补签</a>";

                        html += " <img style=\"display:none;\"  src=\"image/dui.png\"/><br />";
                        html += "<span style=\"display:none;\">+" + data[i].score + "积分</span>";
                    }
                } else {
                    if (data[i].is_show) {
                        if (noHaveSign) {
                            html += " <img  src=" + noHaveSign + " ><br />";
                        } else {
                            html += " <img class=\"defimg\" src=\"image/graygou.png\"><br />";
                        }
                        html += "<a href=\"javascript:;\" data-day=" + data[i].week_day + "   week_name_date=" + data[i].week_name_date + " class=\"weui-btn weui-btn_mini weui-btn_primary\">补签</a>";
                        html += "<span class=\"score\" style=\"display:none;\">+" + data[i].score + "积分</span>";
                    } else {
                        if (noHaveSign) {
                            html += " <img class=\"imgStyle\"  src=" + noHaveSign + " ><br />";
                        } else {

                            html += " <img class=\"defimg\"  src=\"image/graygou.png\"><br />";
                        }
                        html += "<span class=\"noHaveSign\">待签到</span>";
                    }
                }

                html += "</div>";
                html += "</div>";

                if (i < 4) {
                    $('.top').append(html);
                } else {
                    $('.buttom').prepend(html);
                }
            }
            var text = "";
            text += "<div class=\"weui-flex__item lottery\">";
            text += "<div class=\"placeholder_top\" style=\"line-height: 45px;\">";
            text += "<div class=\"z_week\">奖品</div>";
            text += "</div>";
            text += "<div class=\"placeholder_lottery\" onclick=\"ToLottery()\"><img src=\"\" class=\"lottery_img\" /></div>";
            text += "</div>";
            $('.buttom').prepend(text);
        }

        //去抽奖
        function ToLottery() {
            var day = new Date().getDay();
            if (!lotteryId) {
                alert('管理员未设置奖品');
                return;
            }
            if (signinCount >= 7 && day == 0) {
                if (lotteryType == "scratch") {
                    window.location.href = "http://" + window.location.host + "/App/Lottery/wap/ScratchV1.aspx?id=" + lotteryId;
                } else {
                    window.location.href = "http://" + window.location.host + "/customize/shake/?ngroute=/shake/" + lotteryId + "#/shake/" + lotteryId;
                }
            } else {
                alert('暂时不能抽奖');
            }
        }
    </script>
</asp:Content>
