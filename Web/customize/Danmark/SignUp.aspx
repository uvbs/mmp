<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Danmark.SignUp" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <title>丹麦2015摄记你的旅程</title>
    <link href="css/ionic.css" rel="stylesheet" type="text/css" />
    <link href="css/m.css" rel="stylesheet" type="text/css" />
    <link href="css/regist.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: 'Microsoft YaHei';
        }
    </style>
</head>
<body>
    <div class="wrapDanMarkQuestion">
        <div class="top">
            <div class="topImgDiv">
                <img class="topImg" src="images/top.jpg" />
                <div class="phote">
                    <img class="photeImg" src="images/photo_03.png">
                </div>
                <div class="logo">
                    <img class="logoImg" src="images/logo.png">
                </div>
            </div>
        </div>
        <div class="middleDiv">
            <div class="regInputDiv">
                <div class="list">
                    <form id="formsignin">
                    <label class="item item-input">
                      <label>姓名：</label>
                        <input class="mLeft10 colorFFF" type="text" placeholder="" id="txtName" name="Name">
                    </label>
                    <label class="item item-input">
                       <label>联系电话：</label>
                        <input class="mLeft30 colorFFF" type="text" placeholder="" id="txtPhone" name="Phone">
                    </label>
                    <label class="item item-input">
                      <label>到场人数：</label>
                        <input class="mLeft30 colorFFF" type="text" placeholder="" id="txtK1" name="K1">
                    </label>
                    <div class="notOver">
                        说明：每个家庭报名到场人数不超过5人
                    </div>
                   <input id="activityID" type="hidden" value="313226" name="ActivityID" /><input id="loginName" type="hidden" value="anc=" name="LoginName" /><input id="loginPwd" type="hidden" value="C24FDA!E419!412E92CF298F79E11891" name="LoginPwd" />
                    </form>
                </div>
            </div>
            <div class="regBtn">
                <button id="btnSumbit" class="button button-block button-positive">
                    提交报名
                </button>
            </div>
            <div class="descDiv">

                时间：2015年8月16日 13:30
                <br />
                地点：上海市黄浦区汉口路266号申大厦9楼申CLUB
            </div>
        </div>
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
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $("#btnSumbit").live("click", function () {

            var model = {
                Name: $.trim($(txtName).val()),
                Phone: $.trim($(txtPhone).val()),
                K1: $("#txtK1").val()


            }

            if (model.Name == "" || model.Phone == "" || model.K1 == "") {
                layermsg("请填写完整信息");
                return false;
            }


            try {

                var option = {
                    url: "/serv/ActivityApiJson.ashx",
                    type: "post",
                    dataType: "json",
                    success: function (resp) {

                        if (resp.Status == 0) {
                            layermsg("提交成功!");
                            return;

                        }
                        else if (resp.Status == 1) {
                            //该用户已提交过数据
                            layermsg("您已经提交过资料了!");
                        }
                        else {
                            layermsg(resp.Msg);

                        }

                    }
                };
                $("#formsignin").ajaxSubmit(option);
                return false;

            }
            catch (e) {
                alert(e);
            }



        });


    });

    function layermsg(msg) {
        layer.open({
            content: msg,
            btn: ['OK']
        });
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
