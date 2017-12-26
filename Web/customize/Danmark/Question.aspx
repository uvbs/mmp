<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Danmark.Question" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <title>丹麦2015摄记你的旅程</title>
    <link href="css/ionic.css" rel="stylesheet" type="text/css" />
    <link href="css/m.css" rel="stylesheet" type="text/css" />
    <link href="css/index.css" rel="stylesheet" type="text/css" />
    <link href="css/question.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: 'Microsoft YaHei';
        }
        #divquestion2, #divquestion3, #divquestion4, #divquestion5, #divquestion6
        {
            display: none;
        }
    </style>
</head>
<body>
    <div class="wrapDanMarkIndex wrapDanMarkQuestion">
        <div class="top">
            <div class="topImgDiv">
                <img class="topImg" src="images/top.jpg" />
                <div class="phote">
                    <img class="photeImg" src="images/photo_03.png">
                </div>
                <div class="balloon">
                    <img class="balloonImg" src="images/photo_05.png">
                </div>
            </div>
        </div>
        <%--问题1--%>
        <div class="middleDiv" id="divquestion1">
            <div class="queWord">
                1.你会选择以下哪种方式前往？
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="1">
                        A.飞机</div>
                    <div class="col reWrite" data-questionindex="1">
                        B.火车</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="1">
                        C.马车</div>
                    <div class="col reWrite" data-questionindex="1">
                        D.船</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnQuestion2" class="button button-block button-positive">
                    就选它了</button>
            </div>
        </div>
        <%--问题1--%>
        <%--问题2实际问题--%>
        <div class="middleDiv" id="divquestion2">
            <div class="queWord">
                2.如果到了丹麦，你最想体验的风景是什么？
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="2" data-answer="A">
                        A.北欧小镇</div>
                    <div class="col reWrite" data-questionindex="2" data-answer="B">
                        B.华美城堡</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="2" data-answer="C">
                        C.原始森林</div>
                    <div class="col reWrite" data-questionindex="2" data-answer="D">
                        D.雪地冰川</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnQuestion3" class="button button-block button-positive">
                    就选它了</button>
            </div>
        </div>
        <%--问题2--%>
        <%--问题3--%>
        <div class="middleDiv" id="divquestion3">
            <div class="queWord">
                3.经过长途跋涉，你终于到达了童话王国，你希望接待你的人是谁?
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="3">
                        A.年轻帅气的小伙</div>
                    <div class="col reWrite" data-questionindex="3">
                        B.祥和白胡子爷爷</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="3">
                        C.甜美的小女孩</div>
                    <div class="col reWrite" data-questionindex="3">
                        D.邋遢的中年司机</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnQuestion4" class="button button-block button-positive">
                    就选它了</button>
            </div>
        </div>
        <%--问题3--%>
        <%--问题4--%>
        <div class="middleDiv" id="divquestion4">
            <div class="queWord">
                4.旅途中你受邀来到了丹麦的一片原始森林，你认为应是在什么季节？
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="4">
                        A.百花盛开的春天</div>
                    <div class="col reWrite" data-questionindex="4">
                        B.鸟叫蝉鸣的夏天</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="4">
                        C.红叶纷飞的秋天</div>
                    <div class="col reWrite" data-questionindex="4">
                        D.白雪皑皑的冬天</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnQuestion5" class="button button-block button-positive">
                    就选它了</button>
            </div>
        </div>
        <%--问题4--%>
        <%--问题5--%>
        <div class="middleDiv" id="divquestion5">
            <div class="queWord">
                5.忽然有一只森林动物出现在你面前与你交谈，你认为应该是什么?
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="5">
                        A.气质高昂的麋鹿</div>
                    <div class="col reWrite" data-questionindex="5">
                        B.健硕的马驹</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="5">
                        C.深沉的老虎</div>
                    <div class="col reWrite" data-questionindex="5">
                        D.娇小的白兔</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnQuestion6" class="button button-block button-positive">
                    就选它了</button>
            </div>
        </div>
        <%--问题5--%>
        <%--问题6--%>
        <div class="middleDiv" id="divquestion6">
            <div class="queWord">
                6.这只动物带领你进入了一间小木屋，推开门，会最引起您注意的是什么？
            </div>
            <div class="queChoose">
                <div class="row">
                    <div class="col reWrite" data-questionindex="6">
                        A.小碎花窗帘</div>
                    <div class="col reWrite" data-questionindex="6">
                        B.整洁的桌椅</div>
                </div>
                <div class="row">
                    <div class="col reWrite" data-questionindex="6">
                        C.天花板上的吊灯</div>
                    <div class="col reWrite" data-questionindex="6">
                        D.坐在角落的女孩</div>
                </div>
            </div>
            <div class="queBtn">
                <button id="btnGetAnswer" class="button button-block button-positive">
                    查看结果</button>
            </div>
        </div>
        <%--问题6--%>
        <div class="car">
        </div>
        <div class="people">
        </div>
        <div class="bottom">
            <div class="bottomImgDiv">
                <img class="bottomImg" src="images/bottom.jpg" />
                <div class="car">
                    <img class="carImg" src="images/car.png">
                </div>
                <div class="people">
                    <img class="peopleImg" src="images/people.png">
                </div>
            </div>
        </div>
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script>

    var answer1 = "";
    var answer2 = "";
    var answer3 = "";
    var answer4 = "";
    var answer5 = "";
    var answer6 = "";
    var answer = "";
    $(function () {

        $("[data-questionindex]").click(function () {

            var index = $(this).data("questionindex");
            if (index==2) {//实际的问题
                answer = $(this).data("answer");
            }
           
            $("[data-questionindex]").removeClass("selected");
            $(this).addClass("selected");
            switch (index) {
                case 1:
                    answer1 = $(this).html();
                    break;
                case 2:
                    answer2 = $(this).html();
                    break;
                case 3:
                    answer3 = $(this).html();
                    break;
                case 4:
                    answer4 = $(this).html();
                    break;
                case 5:
                    answer5 = $(this).html();
                    break;
                case 6:
                    answer6 = $(this).html();
                    break;
                default:

            }



        })

        $("#btnQuestion2").click(function () {//问题2按钮

            if (answer1 == "") {
                layermsg("请选择一个答案");
                return false;
            }

            ChangeQuestion("2");

        })

        $("#btnQuestion3").click(function () {//问题3按钮

            if (answer2 == "") {
                layermsg("请选择一个答案");
                return false;
            }

            ChangeQuestion("3");

        })

        $("#btnQuestion4").click(function () {//问题4按钮

            if (answer3 == "") {
                layermsg("请选择一个答案");
                return false;
            }

            ChangeQuestion("4");

        })
        $("#btnQuestion5").click(function () {//问题5按钮

            if (answer4 == "") {
                layermsg("请选择一个答案");
                return false;
            }

            ChangeQuestion("5");

        })
        $("#btnQuestion6").click(function () {//问题6按钮

            if (answer5 == "") {
                layermsg("请选择一个答案");
                return false;
            }

            ChangeQuestion("6");



        })




        $("#btnGetAnswer").click(function () {//查看答案

            if (answer6 == "") {
                layermsg("请选择一个答案");
                return false;
            }


            //



            //

           
            window.location.href = "Result.aspx?answer=" + answer;


        })




    })
    function layermsg(msg) {
        layer.open({
            content: msg,
            btn: ['OK']
        });
    }


    //切换答案
    function ChangeQuestion(index) {

        $("#divquestion1").hide();
        $("#divquestion2").hide();
        $("#divquestion3").hide();
        $("#divquestion4").hide();
        $("#divquestion5").hide();
        $("#divquestion6").hide();
        $("#divquestion" + index).show();
    
    }

</script>

<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "测一测，您是丹麦童话里的哪个角色",
                desc: "测一测，您是丹麦童话里的哪个角色",
                link: 'http://huiji.comeoncloud.net/customize/Danmark/Index.aspx',
                imgUrl: "http://<%=Request.Url.Host%>/customize/Danmark/images/logo.jpg"
            })
        })
    </script>
</html>
