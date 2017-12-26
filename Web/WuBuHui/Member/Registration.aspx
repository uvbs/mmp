<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Member.Registration" %>

<!DOCTYPE html>
<html>
<head>
    <title>五步会注册</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="../css/weshow.css" rel="stylesheet" type="text/css" />
    <link href="../css/wubu.css" rel="stylesheet" type="text/css" />
    <script src="../js/require.js" type="text/javascript"></script>
</head>
<body>
    <section class="box">
    <div id="imglist">
        <div class="listli">
            <span class="img" data-original="background-image:url(../img/wubuhui.jpg?v=1);" style=""></span>
            <span class="nextbtn"><span class="smallicon"></span></span>
        </div>
        <div class="listli">
            <span class="img" style="opacity:0;background-color:#eee;">
            <h3 class="maintitle">个人资料</h3>
            <div class="personerinfo">
                <form action="">
                    <div class="listbox">
                     <input type="text" placeholder="姓名" id="txtName" name="txtNmae" maxlength="15">

                        <label for="" class="wbtn_line_main">
                            <span class="iconfont icon-b24"></span>
                        </label>
                    </div>
                    <div class="listbox">
                <input type="number" placeholder="手机" id="txtPhone" name="txtPhone" pattern="\d*">

                        <label for="" class="wbtn_line_main">
                            <span class="iconfont icon-b47"></span>
                        </label>
                    </div>
                    <div class="listbox">
                                    <input type="text" placeholder="邮箱" id="txtEmail" name="txtEmail">

                        <label for="" class="wbtn_line_main">
                            <span class="iconfont icon-b12"></span>
                        </label>
                    </div>
                    <div class="listbox">
                       <input type="text" placeholder="公司" id="txtCompanyl" name="txtCompanyl">
                        <label for="" class="wbtn_line_main">
                            <span class="iconfont icon-b42"></span>
                        </label>
                    </div>
                    <div class="listbox">
                       <input type="text" placeholder="职位" id="txtPostion" name="txtPostion">
                        <label for="" class="wbtn_line_main">
                            <span class="iconfont icon-b42"></span>
                        </label>
                    </div>

                </form>
            </div>

            <div class="wcontainer hupenghuanyou">
 
               <span  id="signinbtn" class="wbtn wbtn_red"><span class="text" id="btnReg">注册会员</span> </span>

                <br/>
                <br/>
                <a href="../MyCenter/Index.aspx" class="">
                    <span class="text">残忍拒绝，继续浏览</span>
                </a>
            </div>
            </span>
        </div>
    </div>
</section>
    <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        提交成功</p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</body>
<script>

    require.config({
        baseUrl: "../js/",
        shim: {
            bootstrap: ["jquery"]
        },
        paths: {
            jquery: "jquery",
            WeShow: "weshow",
            bootstrap: "bootstrap"
        },
        urlArgs: "v=" + (new Date()).getTime()
    })
    require(["jquery", "WeShow", "bootstrap"], function ($, WeShow) {

        touchpic = new WeShow("#imglist", function (_this, snum) {
            var current = $(".listli:eq(" + snum + ")");
            switch (snum) {
                case 0:
                    _this.animation(current.find(".nextbtn"), 20)
                    break;
                case 1:
                    current.find(".img").css({ "opacity": "1" })
                    _this.animation(current.find(".nextbtn"), 20)
                    break;
                default:
                    _this.animation(current.find(".img"), 20, function () {
                        _this.animation(current.find(".nextbtn"), 20)
                    })
            }
        });
        touchpic.init()
        var uid = '<%=Uid %>'
        var url = '<%=Url %>';
        var IsSumbit = false;

        function SubInfo() {
            var model = {
                Action: 'SaveInfo',
                UserId: uid,
                Uname: $.trim($("#txtName").val()),
                UPhone: $.trim($("#txtPhone").val()),
                UEmail: $.trim($("#txtEmail").val()),
                UCompanyl: $.trim($("#txtCompanyl").val()),
                Postion: $.trim($("#txtPostion").val())
            };

            if (model.Uname == null) {
                $('#gnmdb').find("p").text("请输入您的姓名");
                $('#gnmdb').modal('show');
                $("#txtNmae").focus();
                return;
            }

            if (model.UPhone == null) {
                $('#gnmdb').find("p").text("请输入手机号");
                $('#gnmdb').modal('show');
                $("#txtPhone").focus();
                return;
            }

            var myreg = /^(13|14|15|18)\d{9}$/;
            if (!myreg.test(model.UPhone)) {
                $('#gnmdb').find("p").text('请输入有效的手机号码！');
                $('#gnmdb').modal('show');
                $("#txtPhone").focus();
                return;
            }
            if (model.UEmail == null) {
                $('#gnmdb').find("p").text("请输入邮箱");
                $('#gnmdb').modal('show');
                $("#txtEmail").focus();
                return;
            }
            var pattern = /^([\.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
            if (!pattern.test(model.UEmail)) {
                $('#gnmdb').find("p").text("请输入正确的邮箱地址。");
                $('#gnmdb').modal('show');
                $("#txtEmail").focus();
                return;
            }
            if (model.UCompanyl == null) {
                $('#gnmdb').find("p").text("请输入公司名称");
                $('#gnmdb').modal('show');
                $("#txtCompanyl").focus();
                return;
            }

            if (IsSumbit) {
                //$('#gnmdb').find("p").text("请不要重复提交");
                //$('#gnmdb').modal('show');
                return;
            }
            IsSumbit = true;
            $("#btnReg").text("正在注册...");
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
                data: model,
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 0) {
                        window.location.href = "guanzhu_v2.html";
                        //window.location.href = "/WuBuHui/MyCenter/MyCenter.aspx";
                    }
                    else {
                        $('#gnmdb').find("p").text(resp.Msg);
                        $('#gnmdb').modal('show');
                    }
                },
                complete: function () {
                    IsSumbit = false;
                    $("#btnReg").text("注册会员");
                }

            });
        };

        $("#signinbtn").bind("click", SubInfo)

        //        function GetSmsVerCode() {

        //               // $('#gnmdb').find("p").text("平台暂未开放");
        //                //$('#gnmdb').modal('show');
        //               // return false;
        //            
        //            var phone = $.trim($("#txtPhone").val());
        //            if (phone == "") {
        //                $('#gnmdb').find("p").text("请输入手机号");
        //                $('#gnmdb').modal('show');
        //                return false;
        //            }
        //            var myreg = /^(13|14|15|18)\d{9}$/;
        //            if (!myreg.test(phone)) {
        //                $('#gnmdb').find("p").text("请输入有效的手机号码!");
        //                $('#gnmdb').modal('show');
        //                $("#txtPhone").focus();
        //                return;
        //            }
        //            $.ajax({
        //                type: 'post',
        //                url: "/Handler/OpenGuestHandler.ashx",
        //                data: { Action: "GerVerCodeWubuHui", phone: phone },
        //                dataType: "json",
        //                success: function (resp) {
        //                    if (resp.Status == 1) {
        //                        $('#gnmdb').find("p").text("通关密码发送成功");
        //                        $('#gnmdb').modal('show');
        //                    }
        //                    else {
        //                        $('#gnmdb').find("p").text(resp.Msg);
        //                        $('#gnmdb').modal('show');
        //                    }
        //                }
        //            });
        //        }


    })
</script>
</html>
