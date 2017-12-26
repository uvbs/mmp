<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Reg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>注册</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/basic.css" />
    <style>
        #ddlPosition
        {
            width: 100%;
            height: 30px;
            border: none;
            font-family: 'Microsoft YaHei' !important;
        }
        .provinceselect
        {
            color: #1b64c5;
        }
        .form_div
        {
            padding: 0px;
            width: 90%;
            margin-left: 5%;
        }
        .form_input
        {
            height: 30px;
        }
        .form_input1
        {
            height: 30px;
        }
        .form_div2
        {
            width: 90%;
            margin-left: 5%;
        }
        .logo-bg
        {
            background-position: inherit;
        }
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="pages_container  sliderbg1">
        <div class="index_container3">
            <img src="images/logo.png"  style="width:100%;"/>
            <div class="menu3">
                <ul id="tabsmenu">
                    <li id="li1" class="active" onclick="tabsmenu('li1','1')"><span>销售服务店人员</span></li>
                    <li id="li2" onclick="tabsmenu('li2','2')"><span>海马轿车人员
</span></li>
                </ul>
            </div>
            <div id="tab1" class="tabcontent">
                <div class="form">
                    <form method="post" action="">
                    <div class="form_div radius4">
                        <input type="text" id="txtTrueName1" value="" placeholder="姓名" class="form_input" />
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="dep" id="dep" value="" placeholder="销售服务店" class="form_input1"
                            onclick="openPopup('dep')" />
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="bm" id="txtStoreCode" value="" placeholder="店代码" class="form_input" />
                    </div>
                    <div class="form_div radius4">
                        <select id="ddlPosition">
                            <option value="">选择岗位</option>
                            <option value="市场经理">市场经理</option>
                            <option value="销售经理">销售经理</option>
                            <option value="销售顾问">销售顾问</option>
                            <option value="其它">其它</option>
                        </select>
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="sj" id="txtPhone1" value="" placeholder="手机" class="form_input" />
                    </div>
                    <div class="form_div2">
                        <div class="form_div2_l">
                            <div class="form_div1 radius4">
                                <input type="text" name="yzm" id="txtVerCode1" value="" placeholder="验证码" class="form_input" />
                            </div>
                        </div>
                        <div class="form_div2_r">
                            <div class="form_sub1 radius4">
                                <a id="btnGetSmsVerCode1" href="javascript:vodi(0)" class="form_submit1">获取验证码</a>
                            </div>
                        </div>
                    </div>
                    <div class="form_sub radius4">
                        <a id="btnReg1" href="javascript:void(0)" class="form_submit">注册</a>
                    </div>
                    </form>
                </div>
            </div>
            <div id="tab2" class="tabcontent" style="display: none">
                <div class="form">
                    <form method="post" action="">
                    <div class="form_div radius4">
                        <input type="text" id="txtTrueName2" value="" placeholder="姓名" class="form_input" />
                    </div>
                    <div class="form_div radius4" >
                        <input type="text" name="bb" id="txtEx1" value="" placeholder="本部" class="form_input" />
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="bm" id="txtEx2" value="" placeholder="部门" class="form_input" />
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="gw" id="txtPosition2" value="" placeholder="岗位" class="form_input" />
                    </div>
                    <div class="form_div radius4">
                        <input type="text" name="sj" id="txtPhone2" value="" placeholder="手机" class="form_input" />
                    </div>
                    <div class="form_div2">
                        <div class="form_div2_l">
                            <div class="form_div1 radius4">
                                <input type="text" name="yzm" id="txtVerCode2" value="" placeholder="验证码" class="form_input" />
                            </div>
                        </div>
                        <div class="form_div2_r">
                            <div class="form_sub1 radius4">
                                <a id="btnGetSmsVerCode2" href="javascript:void(0)" class="form_submit1">获取验证码</a>
                            </div>
                        </div>
                    </div>
                    <div class="form_sub radius4">
                        <a id="btnReg2" href="javascript:void(0)" class="form_submit">注册</a>
                    </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="clearfix" style="padding-bottom: 20px">
        </div>
        <div id="filter" class="filter-div">
        </div>
        <div id="pageone" class="popup">
            <a href="javascript:" onclick="closePopup()" class="popupCloseimg">
                <img src="images/delete-black.png" style="width: 20px;" /></a>
            <br />
            <div class="menu4">
                <ul id="tabsmenu1">
                    <li id="add1" class="active" onclick="tab_address('add1','1')"><span>选择省</span></li>
                    <li id="add2" onclick="tab_address('add2','2')"><span>选择店</span></li>
                </ul>
            </div>
            <br />
            <div class="addressli_flow">
                <div id="address1" class="tabcontent1">
                    <ul class="addressli">
                        <li onclick="LoadStoreName('安徽')"><a href="javascript:">安徽</a></li>
                        <li onclick="LoadStoreName('北京')"><a href="javascript:">北京</a></li>
                        <li onclick="LoadStoreName('福建')"><a href="javascript:">福建</a></li>
                        <li onclick="LoadStoreName('甘肃')"><a href="javascript:">甘肃</a></li>
                        <li onclick="LoadStoreName('广东')"><a href="javascript:">广东</a></li>
                        <li onclick="LoadStoreName('广西')"><a href="javascript:">广西</a></li>
                        <li onclick="LoadStoreName('贵州')"><a href="javascript:">贵州</a></li>
                        <li onclick="LoadStoreName('海口')"><a href="javascript:">海口</a></li>
                        <li onclick="LoadStoreName('河北')"><a href="javascript:">河北</a></li>
                        <li onclick="LoadStoreName('河南')"><a href="javascript:">河南</a></li>
                        <li onclick="LoadStoreName('黑龙江')"><a href="javascript:">黑龙江</a></li>
                        <li onclick="LoadStoreName('湖北')"><a href="javascript:">湖北</a></li>
                        <li onclick="LoadStoreName('湖南')"><a href="javascript:">湖南</a></li>
                        <li onclick="LoadStoreName('吉林')"><a href="javascript:">吉林</a></li>
                        <li onclick="LoadStoreName('江苏')"><a href="javascript:">江苏</a></li>
                        <li onclick="LoadStoreName('江西')"><a href="javascript:">江西</a></li>
                        <li onclick="LoadStoreName('辽宁')"><a href="javascript:">辽宁</a></li>
                        <li onclick="LoadStoreName('内蒙古')"><a href="javascript:">内蒙古</a></li>
                        <li onclick="LoadStoreName('宁夏')"><a href="javascript:">宁夏</a></li>
                        <li onclick="LoadStoreName('青海')"><a href="javascript:">青海</a></li>
                        <li onclick="LoadStoreName('山东')"><a href="javascript:">山东</a></li>
                        <li onclick="LoadStoreName('山西')"><a href="javascript:">山西</a></li>
                        <li onclick="LoadStoreName('陕西')"><a href="javascript:">陕西</a></li>
                        <li onclick="LoadStoreName('上海')"><a href="javascript:">上海</a></li>
                        <li onclick="LoadStoreName('四川')"><a href="javascript:">四川</a></li>
                        <li onclick="LoadStoreName('天津')"><a href="javascript:">天津</a></li>
                        <li onclick="LoadStoreName('西藏')"><a href="javascript:">西藏</a></li>
                        <li onclick="LoadStoreName('新疆')"><a href="javascript:">新疆</a></li>
                        <li onclick="LoadStoreName('云南')"><a href="javascript:">云南</a></li>
                        <li onclick="LoadStoreName('浙江')"><a href="javascript:">浙江</a></li>
                        <li onclick="LoadStoreName('重庆')"><a href="javascript:">重庆</a></li>
                    </ul>
                </div>
                <div id="address2" class="tabcontent1" style="display: none">
                    <ul class="addressli" id="ulstore">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</body>
<script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
<script src="js/comm.js" type="text/javascript"></script>
<script src="LayerM/layer.m.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
<script type="text/javascript">
    function tabsmenu(obj, v) {
        $("#tabsmenu").find("li").removeClass("active");
        //
        $("#" + obj).addClass("active");
        $(".tabcontent").css("display", "none");
        $("#tab" + v).css("display", "block");
    }
    function tab_address(obj, v) {
        if (v == "1") {
            $("#tabsmenu1").find("li").removeClass("active");
        }
        $("#" + obj).addClass("active");
        $(".tabcontent1").css("display", "none");
        $("#address" + v).css("display", "block");
    }
    function openPopup(obj) {
        targetValue = obj;
        $('#filter').css("display", "block");
        $('#pageone').css("display", "block");

    }
    function closePopup() {
        $('#filter').css("display", "none");
        $('#pageone').css("display", "none");

    }
            
</script>
<script type="text/javascript">
    var province = "";
    var storeName = "";
    $(function () {


        //销售店人员获取验证码
        $("#btnGetSmsVerCode1").click(function () {
            GetSmsVerCode($("#txtPhone1").val());

        });
        //海马人员获取验证码
        $("#btnGetSmsVerCode2").click(function () {
            GetSmsVerCode($("#txtPhone2").val());

        });


        //销售店人员注册
        $("#btnReg1").click(function () {
            Reg1();

        });
        //海马人员注册
        $("#btnReg2").click(function () {
            Reg2();

        });





    })

    //获取手机验证码
    function GetSmsVerCode(phone) {
        if (phone == "") {
            layermsg("请输入手机号");
            return false;
        }
        var myreg = /^(13|14|15|17|18)\d{9}$/;
        if (!myreg.test(phone)) {
            layermsg("请输入有效的手机号码!");
            return;
        }
        $.ajax({
            type: 'post',
            url: commHandler,
            data: { Action: "GetSmsVercode", phone: phone },
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    layermsg("验证码发送成功");

                }
                else {
                    layermsg(resp.errmsg);

                }
            }
        });
    }

    //销售店人员注册
    function Reg1() {
        var trueName = $("#txtTrueName1").val();
        var phone = $("#txtPhone1").val();
        var verCode = $("#txtVerCode1").val();
        var position = $("#ddlPosition").val();
        var storeCode = $("#txtStoreCode").val();
        var model = {
            Action: "Reg",
            RegType: 2,
            Phone: phone,
            VerCode: verCode,
            TrueName: trueName,
            Province: province,
            StoreName: storeName,
            StoreCode: storeCode,
            Position: position



        }

        if (model.TrueName == "") {
            layermsg("请输入姓名");
            return false;
        }
        if (model.Province == "" || model.StoreName == "") {
            layermsg("请选择销售服务店");
            return false;
        }
        if (model.StoreCode == "") {
            layermsg("请输入店代码");
            return false;
        }
        if (model.Position == "") {
            layermsg("请选择岗位");
            return false;
        }
        if (model.Phone == "") {
            layermsg("请输入手机号码");
            return false;
        }
        var myreg = /^(13|14|15|17|18)\d{9}$/;
        if (!myreg.test(model.Phone)) {
            layermsg("请输入有效的手机号码!");
            return;
        }
        if (model.VerCode == "") {
            layermsg("请输入收到的手机验证码");
            return false;
        }

        $.ajax({
            type: 'post',
            url: commHandler,
            data: model,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    window.location = 'Index.aspx';
                    //layermsg("注册成功");
                    //setTimeout("window.location='Index.aspx'", 2000);
                }
                else {
                    layermsg(resp.errmsg);

                }
            }
        });






    }



    //海马人员注册
    function Reg2() {
        var trueName = $("#txtTrueName2").val();
        var phone = $("#txtPhone2").val();
        var verCode = $("#txtVerCode2").val();
        var position = $("#txtPosition2").val();
        var ex1 = $("#txtEx1").val();
        var ex2 = $("#txtEx2").val();
        var model = {
            Action: "Reg",
            RegType: 3,
            Phone: phone,
            VerCode: verCode,
            TrueName: trueName,
            Position: position,
            Ex1: ex1,
            Ex2: ex2
        }

        if (model.TrueName == "") {
            layermsg("请输入姓名");
            return false;
        }
        if (model.Ex1 == "") {
            layermsg("请输入本部");
            return false;
        }
        if (model.Ex2 == "") {
            layermsg("请输入部门");
            return false;
        }
        if (model.Position == "") {
            layermsg("请输入岗位");
            return false;
        }
        if (model.Phone == "") {
            layermsg("请输入手机号码");
            return false;
        }
        var myreg = /^(13|14|15|17|18)\d{9}$/;
        if (!myreg.test(model.Phone)) {
            layermsg("请输入有效的手机号码!");
            return;
        }
        if (model.VerCode == "") {
            layermsg("请输入收到的手机验证码");
            return false;
        }

        $.ajax({
            type: 'post',
            url: commHandler,
            data: model,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    window.location = 'Index.aspx';
//                    layermsg("注册成功");
//                    setTimeout("window.location='Index.aspx'", 2000);
                }
                else {
                    layermsg(resp.errmsg);

                }
            }
        });






    }


    ///根据省份加载门店列表
    function LoadStoreName(provin) {
        province = provin;
        tab_address('add2', '2');



        $("#ulstore").html("正在加载门店...");
        $.ajax({
            type: 'post',
            url: commHandler,
            data: { Action: "GetStoreListByProvince", Province: province },
            dataType: "json",
            success: function (resp) {
                //
                var str = new StringBuilder();
                for (var i = 0; i < resp.length; i++) {

                    var clickEvent = 'onclick=SelectStoreName("' + resp[i].StoreName + '") ';
                    str.AppendFormat('<li {0}>', clickEvent);
                    str.AppendFormat('<a href="javascript:">{0}</a>', resp[i].StoreName);
                    str.AppendFormat('</li>');
                };
                $("#ulstore").html(str.ToString());
                //


            }
        });


    }

    //选择门店
    function SelectStoreName(stoName) {
        closePopup();
        storeName = stoName;
        $("#dep").val(province + " " + storeName);



    }
</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "注册",
            desc: "海马精英成长平台",
            //link: '', 
            imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
        })
    })
</script>
</html>
