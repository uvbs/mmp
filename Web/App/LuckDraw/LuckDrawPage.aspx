<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LuckDrawPage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.LuckDrawPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>抽奖页面</title>
    <link href="/lib/bootstrap/3.3.4/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/css/buttons2.css" rel="stylesheet" type="text/css" />
    <link href="css/luckdraw.css?v=20161229" rel="stylesheet" type="text/css" />
    <link href="/css/Common.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="audio">
            <audio class="audiobox" src="<%=model.StartMusic %>" preload="preload" loop="loop" controls="controls">
                Your browser does not support the audio element.
            </audio>
        </div>

        <div class="mainbox">
            <%
                if (model.IsHideWinningList != 2)
                {
            %>
            <div class="col-xs-12 leftbox">
                <%
                }
                else
                {
                %>

                <div class="col-xs-9 leftbox">
                    <%
                }
                    %>

                   



                    <%
                        //是否显示标题
                        if (model.IsHideTitle == 0)
                        {
                    %>
                    <div class="titlebox"><%=model.LotteryName%></div>
                    <%
                        }
                        else
                        {
                    %>
                    <div class="titlebox1"></div>
                    <%
                        }     
                    %>
                    <div class="titleheader">
                    </div>
                    <div class="titleboxcount">
                        抽奖总人数:<span class="total">0</span>
                    </div>
                    <div class="awardlist">
                        <div class="user" style="display: block;">
                        </div>
                        <div class="awardbar" style="display: none;">



                          
                            <div class="awardbarinfo1 col-xs-4">
                            <%
                           
                                if (model.IsHideWinningList == 1)
                                {
                                    %>
                                        
                                            <div class="userinfo">
                                                <ul class="winerinfo">
                                                </ul>
                                            </div>
                                       
                                    <%
                                }     
                            %>
                             </div>





                              <%
                                if (model.IsHideWinningList == 2)
                                {
                                    %>
                                         <div class="awardbarinfo1 col-xs-5">
                                    <%
                                }
                                else
                                {
                                    %>
                                         <div class="awardbarinfo1 col-xs-4">
                                    <%
                                }  
                             %>

                           
                                <div class="userinfo">
                                    <div class="puserinfo">
                                        <img  src="http://file.comeoncloud.net/img/europejobsites.png" class="img-rounded avatar">
                                        <div class="wxnickname">
                                            昵称
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%
                                if (model.IsHideWinningList == 2)
                                {
                                    %>
                                         <div class="awardbarinfo1 col-xs-3">
                                    <%
                                }
                                else
                                {
                                    %>
                                         <div class="awardbarinfo1 col-xs-4">
                                    <%
                                }  
                             %>

                                        <div style="padding: 0px 10px;">
                                            <a href="javascript:;" class="button button-3d button-action button-circle button-caution start">开始</a>
                                            <a href="javascript:;" class="button button-3d button-action button-circle button-highlight stop hidden">停止</a>
                                            <a href="javascript:;" class="button button-3d button-action button-circle button-caution hidden continue ">继续</a><br />
                                            <a href="javascript:;" class="button button-3d button-action button-circle button-caution hidden jixu">稍等</a><br />

                                            <a href="javascript:;" class="button button-glow button-border button-rounded button-primary reset" style="border: 0; box-shadow: 0 0 0 0 #fff;">重置抽奖</a>
                                        </div>
                                        <div class="goIndex hidden">

                                            <a href="javascript:;" class="btnGoIndex">返回首页</a>
                                        </div>

                                    </div>
                                </div>
                             </div>

                            <div class="tooltar row">
                                <div class="col-xs-4 tooltarleft">
                                    <div class="wxqrcode">
                                        <%
                                            if (model.IsHideQRCode == 0)
                                            {
                                        %>
                                        <p class="p1">扫码加入抽奖队列</p>
                                        <p class="QRCode">
                                            <img src="" />
                                        </p>
                                        <p class="pLeft15 p3">
                                            <img class="position" src="images/fangdajing.png" /><span class="position">点击放大</span>
                                        </p>
                                        <%
                                            } 
                                        %>
                                    </div>
                                </div>
                                <div class="col-xs-4 tooltarcenter">
                                    <a href="javascript:;" class="button button-3d button-action button-circle button-caution start">开始</a>
                                    <a href="javascript:;" class="button button-3d button-action button-circle button-highlight stop hidden">停止</a>
                                    <a href="javascript:;" class="button button-3d button-action button-circle button-caution hidden continue ">继续</a><br />
                                    <a href="javascript:;" class="button button-3d button-action button-circle button-caution hidden jixu">稍等</a><br />
                                    <a href="javascript:;" class="button button-glow button-border button-rounded button-primary reset" style="border: 0; height: 36px; box-shadow: 0 0 0 0 #fff;">重置抽奖</a>

                                </div>
                                <div class="col-xs-4 tooltarright">
                                    <div class="divGoIndex">
                                        <a href="javascript:;" class="btnGoIndex hidden">返回首页</a>
                                    </div>
                                </div>
                            </div>
                        </div>






                        <%
                            if (model.IsHideWinningList == 2)
                            {
                        %>
                        <div class="col-xs-3 rightbox">
                            <div class="topbar"></div>
                            <div class="listbox">
                                <h3 class="toptitle">全部中奖名单</h3>
                                <div class="awardlistbox">
                                    <table class="table table-hover winningList">
                                        <thead>
                                            <tr>
                                                <th style="width: 33%;">中奖编号</th>
                                                <th style="width: 34%;">昵称</th>
                                                <th style="width: 33%;">时间</th>
                                            </tr>

                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                                <h3 class="bottomtitle">已有<span class="awardnumber"></span>人中奖</h3>
                            </div>
                        </div>
                        <%
                            }     
                        %>
                    </div>
                </div>
    </form>

    <script src="/lib/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script type="text/javascript">
        var url = '/serv/api/admin/lottery/LotteryUserInfo/list.ashx';
        var lotteryId = '<%=lotteryId%>';
        var domain = '<%=Request.Url.Authority%>';
        var isHideWinning = '<%=model.IsHideWinningList%>';
        var status = '<%=model.Status%>';
        var bgImg = '<%=model.BackGroudImg%>';
        var titlebgcolor = '<%=model.BackGroundColor%>';
        var titlefontcolor = '<%=model.TitleFontColor%>';
        var userbgcolor = '<%=model.UserBackGroudColor%>';
        var startTimer;
        var currUserList = [];
        var currUserAutoIDList = [];
        var currUser = {};
        var currUserIndex = 0;
        var winningList = [];
        var winerCount;
        var currDate;
        var total = 0;
        var lcount = 0;//执行次数
        var oneWinnerCount = '<%=model.OneWinnerCount%>';
        var qrcode = '<%=model.QRCode%>';
        var stopMusic = '<%=model.StopMusic%>';
        var startMusic = '<%=model.StartMusic%>';
        var disUserId = '<%=model.DistributorUserId%>';
        var startSetIntervalTimer;
        var stopSetIntervalTimer;
        var page = 1;
        var indexLoad = 0;
        Array.prototype.RemoveIndexOf = function (dx) {
            if (isNaN(dx) || dx > this.length) {
                return false;
            }
            this.splice(dx, 1);
        }
        Date.prototype.Format = function (fmt) { //author: meizz 
            var o = {
                "M+": this.getMonth() + 1, //月份 
                "d+": this.getDate(), //日 
                "h+": this.getHours(), //小时 
                "m+": this.getMinutes(), //分 
                "s+": this.getSeconds(), //秒 
                "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
                "S": this.getMilliseconds() //毫秒 
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }


        $(function () {
            var height = screen.height - 150;
            $('.listbox').css('height', height + 'px');
            $('.awardlistbox').css('height', height - 100 + 'px');

            //背景图片
            if (!!bgImg) {
                $(document).find('.mainbox').css('background', 'url(' + bgImg + ')');
                $(document).find('.mainbox').addClass('bgimg');
            }
            //主题背景颜色
            if (!!titlebgcolor) {
                $(document).find('.titlebox').css('background', titlebgcolor);
                $(document).find('.titleboxcount').css('color', titlebgcolor);
                $(document).find(".reset").css("background", titlebgcolor);
            }
            //主题字体颜色
            if (!!titlefontcolor) {

                $(document).find('.titlebox').css('color', titlefontcolor);
                $(document).find('.button-circle').css('color', titlefontcolor);
                $(document).find('.reset').css('color', titlefontcolor);
                $(document).find('.wxnickname').css('color', titlefontcolor);
                $(document).find(".reset").css("color", titlefontcolor);
            }
            //用户转动框背景颜色
            if (userbgcolor) {
                $(document).find('.puserinfo').css('background', userbgcolor);
                $(document).find('.winerinfo').css('background', userbgcolor);
            }

            if (isHideWinning == 1) {
                $('.rightbox').hide();
            }

            //二维码显示隐藏
            $('.wxqrcode').mousemove(function () {
                $(this).addClass('wxqrcode');
                $(this).addClass('wxqrcode1');
            });
            $('.wxqrcode').mouseout(function () {
                $(this).removeClass('wxqrcode1');
                $(this).addClass('wxqrcode');
            });

            //全部参与者
            loadUserList('', page);

            //中奖参与者
            loadWinningUser();


            //二维码
            loadQRCode();

            //开始抽奖
            $('.start').click(function () {

                if (status == 0) {
                    layer.msg('活动已停止');
                    return;
                }
                if (total == 0) {
                    layer.msg('请先添加参加活动参与者');
                    return;
                }


                $('.audiobox').attr({ 'src': startMusic, 'autoplay': 'autoplay' });

                $('.tooltarcenter').addClass('hidden');

                $('.user').fadeOut(function () {
                    $('.awardbar').fadeIn();
                    $('.userinfo').hide();
                    start();
                });

            });
            //停止抽奖
            $('.stop').click(function () {
                stop();
            });
            //继续抽奖
            $('.continue').click(function () {

                if (indexLoad > 0) {
                    return;
                }
                indexLoad = indexLoad + 10;
                console.log(['stopSetIntervalTimer', stopSetIntervalTimer]);
                $('.audiobox').attr({ 'src': startMusic, 'autoplay': 'autoplay' });
                $('.winerinfo').hide();
                lcount = 0;
                $('.winerinfo').html('');
                setTimeout(function () {
                    start();
                }, 500)
            });
            //去首页
            $('.btnGoIndex').click(function () {
                GoIndex();
            });
            //重置抽奖
            $('.reset').click(function () {
                Reset();
            });
            //放大二维码
            $('.p3 img,.p3 span').click(function () {
                BigImg();
            });

        });


        function loadUserList(times, page) {

            if (!times) times = ''
            $.ajax({
                type: 'POST',
                url: url,
                data: { lottery_id: lotteryId, curr_date: times, rows: 80, page: page },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        currDate = resp.msg;
                        if (resp.result.list.length > 0) {
                            for (var i = 0; i < resp.result.list.length; i++) {
                                var ru = resp.result.list[i];
                                if (currUserAutoIDList.indexOf(ru.autoid) == -1) {
                                    total = total + 1;
                                    currUserAutoIDList.push(ru.autoid);
                                    if (ru.iswiner == 0) {
                                        currUserList.push(ru);
                                    }
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img onerror="{0}"  src="' + ru.head_img_url + '" class="img-rounded"/>', "javascript:this.src='http://file.comeoncloud.net/img/europejobsites.png'");
                           
                         
                                    $('.user').prepend(str.ToString());

                                }
                            }
                            $('.total').text(total);
                        }
                        if (resp.result.list.length < resp.result.totalcount) {
                            page++;
                            setTimeout(function () {
                                loadUserList('', page);
                            }, 1000);
                        } else {
                            setTimeout(function () {
                                loadUserList(currDate);
                            }, 2000);
                        }

                    }
                }
            });
        }


        function loadWinningUser() {
            $.ajax({
                type: 'GET',
                url: url,
                data: { lottery_id: lotteryId, is_winning: '1', rows: 1000 },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('.awardnumber').text(resp.result.totalcount);
                        winningList = resp.result.list;
                        winerCount = resp.result.totalcount;
                        $.each(resp.result.list, function (k, v) {
                            var tr = $('<tr></tr>');

                            var td1 = $('<td>' + v.number + '</td>');
                            var td2 = $('<td>' + v.nick_name + '</td>');
                            var td3 = $('<td>' + v.wintime + '</td>');

                            tr.append(td1).append(td2).append(td3);

                            $('.winningList').prepend(tr);
                        });
                    }
                }
            });
        }

        function loadQRCode() {
            if (qrcode == '1') {
                $('.QRCode img').attr('src', '/Handler/ImgHandler.ashx?is_logo=1&v=http://' + domain + '/App/LuckDraw/wap/join.aspx?lotteryId=' + lotteryId);
            } else {
                if ($.trim(lotteryId) == '') {
                    layer.msg('二维码获取参数错误[lotteryId]');
                    return;
                }
                var code = 'LotteryCode_' + lotteryId;

                if (disUserId != '') {
                    code += '_' + disUserId;
                }

                $.ajax({
                    type: 'POST',
                    url: '/serv/api/common/wxqrcode.ashx',
                    data: { code: code },
                    dataType: 'json',
                    success: function (result) {
                        if (result.status) {
                            $('.QRCode img').attr('src', result.result.qrcode_url);
                        } else {
                            layer.msg('获取二维码出错');
                        }
                    }
                });
            }
        }



        function start() {


            checkWiner();

            if (stopSetIntervalTimer) {
                clearInterval(stopSetIntervalTimer);
            }

            $('.winning').hide();

            if (winerCount == total) {
                layer.msg('已经全部抽完');
                $('.audiobox').attr('src', '');
                return;
            }

            startTimer = setInterval(function () {
                getCurrUser();
            }, 100);

            if (lcount == 0 || lcount >= oneWinnerCount) {
                $('.stop').removeClass('hidden');
            }

            $('.start').addClass('hidden');

            $('.continue').addClass('hidden');


            startSetIntervalTimer = setTimeout(function () {
                if (lcount > 0 && lcount < parseInt(oneWinnerCount)) {
                    stop();
                }
            }, 1000);
        }




        function getCurrUser() {


            var index = 0;

            if (currUserList.length > 1) {
                var goAhead = false;
                do {
                    index = GetRandomNum(0, currUserList.length - 1);
                    goAhead = currUserIndex == index;

                } while (goAhead);
            }


            currUser = currUserList[index];
            currUserIndex = index;

            if (currUser) {
                $('.userinfo').show();
                $('.avatar').attr('src', currUser.head_img_url);
                $('.awardbar .wxnickname').text(currUser.nick_name);
                console.log(['currUser', currUser]);

            } else {
                console.log(index);
            }
            return {
                user: currUser,
                index: index,
                time: new Date().Format('yyyy-MM-dd hh:mm:ss')
            };
        }

        function stop() {

            clearInterval(startSetIntervalTimer);
            lcount++;

            $('.audiobox').attr({ 'src': stopMusic, 'autoplay': 'autoplay' });

            if (oneWinnerCount >= 1) {
                $('.winerinfo').show();
            }

            clearInterval(startTimer);
            var nModel = getCurrUser();

            if (nModel.user === undefined) {
                return;
            }

            if (nModel.user) {
                var html = '';
                html += "<li class=\"col3\">";
                html += "<img src=" + nModel.user.head_img_url + " class=img-rounded'>";
                html += " <div class='wxnick'>" + nModel.user.nick_name + "</div>";
                html += "</li>";
                $('.winerinfo').prepend(html);
            }


            winningList.push(nModel.user);

            currUserList.RemoveIndexOf(nModel.index);

            $('.stop').addClass('hidden');


            $('.btn-disabled').removeClass('hidden');

            $('.jixu').removeClass('hidden').addClass(' disabled');

            var layerIndex = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'POST',
                url: '/serv/api/admin/lottery/LotteryUserInfo/SetWinner.ashx',
                data: { id: nModel.user.autoid, lottery_id: lotteryId },
                dataType: "json",
                success: function (resp) {
                    layer.close(layerIndex);
                    if (resp.status) {
                        var tr = $('<tr></tr>');
                        var td1 = $('<td>' + resp.msg + '</td>');
                        var td2 = $('<td>' + nModel.user.nick_name + '</td>');
                        var td3 = $('<td>' + nModel.time + '</td>');
                        tr.append(td1).append(td2).append(td3);

                        $('.winningList').prepend(tr);

                        winerCount = winerCount + 1;

                        $('.awardnumber').text(winerCount);

                        $('.goIndex').removeClass('hidden');

                        $('.winning').show();
                    } else {
                        alert('请检查网络');
                    }
                }
            });
            indexLoad = 0;
            stopSetIntervalTimer = setTimeout(function () {
                if (lcount < parseInt(oneWinnerCount)) {
                    start();
                } else {
                    $('.continue').removeClass('hidden');
                    $('.jixu').addClass('hidden');
                }
                $('.audiobox').attr('src', '');
            }, 500);



        }




        function GetRandomNum(Min, Max) {
            var Range = Max - Min;
            var Rand = Math.random();
            return (Min + Math.round(Rand * Range));
        }

        function GoIndex() {
            window.location.href = '/app/luckdraw/LuckDrawPage.aspx?lotteryId=' + lotteryId;
        }

        function checkRepeatUser(user) {
            if (winningList.length == 0) {
                return false;
            }
            for (var i = 0; i < winningList.length; i++) {
                if (winningList[i].userid == user.userid)
                    return true;
            }
        }

        function Reset() {
            if (winningList.length == 0) {
                layer.msg('暂时不能重置抽奖');
                return;
            }
            layer.confirm('确定要重置活动抽奖吗？', {
                btn: ['确定', '取消'] //按钮
            }, function () {
                var layerIndex = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
                $.ajax({
                    type: 'POST',
                    url: '/serv/api/admin/lottery/LotteryUserInfo/ResetWinning.ashx',
                    data: { lottery_id: lotteryId },
                    dataType: "json",
                    success: function (resp) {
                        layer.close(layerIndex);
                        if (resp.status) {
                            layer.msg('重置抽奖成功');
                            window.location.href = '/app/luckdraw/LuckDrawPage.aspx?lotteryId=' + lotteryId;
                        } else {
                            layer.msg('重置抽奖失败');
                        }
                    }
                });
            });
        }

        function BigImg() {

            var imgSrc = $('.QRCode img').attr('src');
            var html = '<div><img class="bigImg" src="' + imgSrc + '"/></div>';

            layer.open({
                type: 1,
                title: false,
                closeBtn: 0,
                shadeClose: true,
                skin: 'yourclass',
                content: html
            });
        }

        function checkWiner() {
            for (var i = currUserList.length - 1; i >= 0; i--) {
                if (currUserList[i].iswiner == "1") {
                    currUserList.splice(i, 1);
                }
            }
        }

    </script>
</body>
</html>
