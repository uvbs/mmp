<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.SignUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/basic.css" />
    <style>
        ::-moz-placeholder
        {
            color: #fff;
        }
        ::-webkit-input-placeholder
        {
            color: #fff;
        }
        :-ms-input-placeholder
        {
            color: #fff;
        }
    </style>
</head>
<body class="sliderbg2">
    <!--Page 1 content-->
    <div class="pages_container">
        <div class="index_container3">
            <div class="image_single">
                <img src="images/signup_header.png" alt="" title="" border="0" /></div>
            <div id="tab1" class="tabcontent">
                <div class="form">
                    <form method="post" action="">
                    <div class="form_div3">
                        根据您的身份自动为您筛选出以下课题</div>
                    <%
                           
                        switch (CurrentUserInfo.Postion)
                        {
                            case "销售顾问":

                                ;
                                Response.Write("<div class=\"form_div4 radius4\" style=\"display:none;\">");
                                Response.Write(string.Format(" <input type=\"text\" id=\"txtQuestion1\" value=\"{0}\" readonly=\"readonly\" class=\"form_input4\" />", QeuestionA[new Random().Next(0, QeuestionA.Count)]));
                                Response.Write("</div>");

                                Response.Write("<div class=\"form_div4 radius4\" style=\"display:none;\">");
                                Response.Write(string.Format(" <input type=\"text\" id=\"txtQuestion2\" value=\"{0}\" readonly=\"readonly\" class=\"form_input4\" />", QeuestionB[new Random().Next(0, QeuestionB.Count)]));
                                Response.Write("</div>");



                                break;
                            case "销售经理":
                                Response.Write("<div class=\"form_div4 radius4\" style=\"display:none;\">");
                                Response.Write(string.Format(" <input type=\"text\" id=\"txtQuestion1\" value=\"{0}\" readonly=\"readonly\" class=\"form_input4\" />", QeuestionC[new Random().Next(0, QeuestionC.Count)]));
                                Response.Write("<input type=\"hidden\" id=\"txtQuestion2\" value=\"\"></div>");

                                break;
                            case "市场经理":
                                Response.Write("<div class=\"form_div4 radius4\" style=\"display:none;\">");
                                Response.Write(string.Format(" <input type=\"text\" id=\"txtQuestion1\" value=\"{0}\" readonly=\"readonly\" class=\"form_input4\" />", QeuestionD[new Random().Next(0, QeuestionD.Count)]));
                                Response.Write("<input type=\"hidden\" id=\"txtQuestion2\" value=\"\"></div>");
                                break;
                            default:
                                break;
                        } %>
                    <div class="form_div4 radius4">
                        <input type="text" name="gw" id="txtIDCard" value="" placeholder="请输入身份证号码" class="form_input4" />
                    </div>
                    <div class="form_div4 radius4">
                        <textarea name="sj" id="txtIntroduction" class="form_input_txt" placeholder="请输入参赛宣言"></textarea>
                    </div>
                    <div class="form_sub4 radius4">
                        <a id="btnSignUp" href="javascript:void()" class="form_submit">提交报名</a>
                    </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="clearfix" style="padding-bottom: 20px">
        </div>
    </div>
</body>
<script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
<script src="js/comm.js" type="text/javascript"></script>
<script src="LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSignUp").click(function () {
            SignUp();
        })
    })

    //报名
    function SignUp() {
        layermsg("报名停止");
        return false;
        var model = {
            Action: "SignUp",
            Question1: $("#txtQuestion1").val(),
            Question2: $("#txtQuestion2").val(),
            IDCard: $("#txtIDCard").val(),
            Introduction: $("#txtIntroduction").val()
        }
        if (model.IDCard == "") {
            layermsg("请输入身份证号");
            return false;
        }
        if (model.Introduction == "") {
            layermsg("请输入参赛宣言");
            return false;
        }

        $.ajax({
            type: 'post',
            url: commHandler,
            data: model,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    layermsg("报名成功!");
                    setTimeout("window.location.href='MySignUp.aspx '",2000);
                }
                else {
                    layermsg(resp.errmsg);

                }
            }
        });

    }
</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "我要报名",
            desc: "海马精英成长平台",
            //link: '', 
            imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
        })
    })
</script>
</html>
