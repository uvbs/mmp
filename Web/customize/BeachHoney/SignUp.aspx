<%@ Page Title="" Language="C#" MasterPageFile="~/customize/BeachHoney/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BeachHoney.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
沙滩宝贝-我要报名
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body
        {
            background-color: White;
        }
        .imgbottom
        {
            margin-bottom: 50px;
        }
        table
        {
            width: 95%;
        }
        .form_input
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="image_single">
        <img src="images/signup_04.png" alt="" title="" border="0"  />
    </div>
    <div class="list2">
        <div class="form ">
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">姓名</span>
                        </td>
                        <td>
                            <input type="text" id="txtVoteObjectName" name="user" value="" placeholder="输入你的姓名"
                                class="form_input" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">城市</span>
                        </td>
                        <td>
                            <input type="text" name="city" id="txtArea" value="" placeholder="输入你所在的城市" class="form_input" maxlength="5" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">手机</span>
                        </td>
                        <td>
                            <input type="text" name="moblie" id="txtPhone" value="" placeholder="输入你的手机号码" class="form_input" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_error">
                *必须填写正确号码信息，需凭该号码收到的验证码领奖</div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">参赛宣言</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="text" name="moblie" id="txtIntroduction" value="" placeholder="输入你的参赛宣言"
                                class="form_input" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">上传照片</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="signup_msg">
                                请报名者<span>点击加号</span>上传<span>本人自拍照3-5张</span>，其中<span>1张自拍须手持写有“沙滩宝贝”字样的纸张</span>，请注意照片附件大小在1MB以内。报名成功后仍然可以修改照片，如果照片上传不成功，仍然可以报名，请稍后发送照片到公众号。
                                </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div class="upload">
                                <img src="images/demo.jpg" /><br />
                                手持“沙滩宝贝”的样张
                            </div>
                            <div class="menu6">
                                <ul>
                                    <li><a href="javascript:void(0)" onclick="txtThumbnailsPath1.click()">
                                        <img src="images/signup_01.jpg" alt="" title="" id="imgshow1" width="52" height="52" />
                                        <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                                    </a><span class="re_span" onclick="ChangeAngle1()">
                                        <img src="images/signup_03.jpg" alt="" title="" /></span> </li>
                                    <li><a href="javascript:void(0)" onclick="txtThumbnailsPath2.click()">
                                        <img src="images/signup_01.jpg" alt="" title="" id="imgshow2" width="52" height="52" />
                                        <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                                    </a><span class="re_span" onclick="ChangeAngle2()">
                                        <img src="images/signup_03.jpg" alt="" title="" /></span> </li>
                                    <li><a href="javascript:void(0)" onclick="txtThumbnailsPath3.click()">
                                        <img src="images/signup_01.jpg" alt="" title="" id="imgshow3" width="52" height="52" />
                                        <input type="file" id="txtThumbnailsPath3" name="file3" style="display: none;" />
                                    </a><span class="re_span" onclick="ChangeAngle3()">
                                        <img src="images/signup_03.jpg" alt="" title="" /></span> </li>
                                    <li><a href="javascript:void(0)" onclick="txtThumbnailsPath4.click()">
                                        <img src="images/signup_01.jpg" alt="" title="" id="imgshow4" width="52" height="52" />
                                        <input type="file" id="txtThumbnailsPath4" name="file4" style="display: none;" />
                                    </a><span class="re_span" onclick="ChangeAngle4()">
                                        <img src="images/signup_03.jpg" alt="" title="" /></span> </li>
                                    <li><a href="javascript:void(0)" onclick="txtThumbnailsPath5.click()">
                                        <img src="images/signup_01.jpg" alt="" title="" id="imgshow5" width="52" height="52" />
                                        <input type="file" id="txtThumbnailsPath5" name="file5" style="display: none;" />
                                    </a><span class="re_span" onclick="ChangeAngle5()">
                                        <img src="images/signup_03.jpg" alt="" title="" /></span> </li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div3">
                <a href="javascript:void(0)" id="btnSumbit">
                    <img src="images/signup_05.jpg" /></a>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
    <script type="text/javascript">

        angleArr = [0, 90, 180, 270];
        var imgAngle1 = 0;
        var imgAngle2 = 0;
        var imgAngle3 = 0;
        var imgAngle4 = 0;
        var imgAngle5 = 0;

        $(function () {
            $("#btnSumbit").click(function () {

                Apply();

            });

            $("#txtThumbnailsPath1").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=BeachHoney&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $("#imgshow1").attr("src", resp.ExStr);
                                imgAngle1 = 0;

                            } else {
                                layermsg(resp.Msg);
                            }
                        }

                    });

                } catch (e) {
                    layermsg(e);
                }

            });

            $("#txtThumbnailsPath2").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=BeachHoney&filegroup=file2',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath2',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow2").attr("src", resp.ExStr);
                                imgAngle2 = 0;

                            } else {
                                layermsg(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    layermsg(e);
                }
            });

            $("#txtThumbnailsPath3").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=BeachHoney&filegroup=file3',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath3',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow3").attr("src", resp.ExStr);
                                imgAngle3 = 0;

                            } else {
                                layermsg(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    layermsg(e);
                }
            });

            $("#txtThumbnailsPath4").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=BeachHoney&filegroup=file4',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath4',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow4").attr("src", resp.ExStr);
                                imgAngle4 = 0;

                            } else {
                                layermsg(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    layermsg(e);
                }
            });

            $("#txtThumbnailsPath5").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=BeachHoney&filegroup=file5',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath5',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow5").attr("src", resp.ExStr);
                                imgAngle5 = 0;

                            } else {
                                layermsg(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    layermsg(e);
                }

            });


        })

        var ai1 = 0;
        function ChangeAngle1() {
            ai1++;
            if (ai1 > 3)
                ai1 = 0;
            imgAngle1 = angleArr[ai1];
            $('#imgshow1').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow1').addClass('transform' + imgAngle1);

        }
        //2
        var ai2 = 0;
        function ChangeAngle2() {
            ai2++;
            if (ai2 > 3)
                ai2 = 0;
            imgAngle2 = angleArr[ai2];
            $('#imgshow2').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow2').addClass('transform' + imgAngle2);

        }
        //2
        //3
        var ai3 = 0;
        function ChangeAngle3() {
            ai3++;
            if (ai3 > 3)
                ai3 = 0;
            imgAngle3 = angleArr[ai3];
            $('#imgshow3').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow3').addClass('transform' + imgAngle3);

        }
        //3
        //4
        var ai4 = 0;
        function ChangeAngle4() {
            ai4++;
            if (ai4 > 3)
                ai4 = 0;
            imgAngle4 = angleArr[ai4];
            $('#imgshow4').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow4').addClass('transform' + imgAngle4);

        }
        //4
        //5
        var ai5 = 0;
        function ChangeAngle5() {
            ai5++;
            if (ai5 > 3)
                ai5 = 0;
            imgAngle5 = angleArr[ai5];
            $('#imgshow5').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow5').addClass('transform' + imgAngle5);

        }
        //5

        function Apply() {
            var model = {
                Action: "AddVoteObjectInfo",
                VoteObjectName: $.trim($(txtVoteObjectName).val()),
                Area: $.trim($(txtArea).val()),
                Introduction: $.trim($(txtIntroduction).val()),
                Phone: $.trim($(txtPhone).val()),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                ShowImage3: $("#imgshow3").attr("src"),
                ShowImage4: $("#imgshow4").attr("src"),
                ShowImage5: $("#imgshow5").attr("src"),
                imgAngle1: imgAngle1,
                imgAngle2: imgAngle2,
                imgAngle3: imgAngle3,
                imgAngle4: imgAngle4,
                imgAngle5: imgAngle5
            }
            if (model.VoteObjectName == "") {
                //layermsg("请输入你的姓名");
                layermsg("请输入你的姓名");
                return false;
            }
            if (model.Area == "") {
                layermsg("请输入你所在的城市");
                return false;
            }
            if (model.Phone == "") {
                layermsg("请输入你的手机号");
                return false;
            }
            var phonereg = /^(13|14|15|17|18)\d{9}$/;
            if (!phonereg.test(model.Phone)) {
                layermsg("请输入有效的手机号码");
                return false;
            }
            if (model.Introduction == "") {
                layermsg("请输入参赛宣员");
                return false;
            }
//            if (model.ShowImage1 == "images/signup_01.jpg") {
//                layermsg("请上传第一张照片");
//                return false;
//            }
//            if (model.ShowImage1 == "images/signup_01.jpg" && model.ShowImage2 == "images/signup_01.jpg" && model.ShowImage3 == "images/signup_01.jpg" && model.ShowImage4 == "images/signup_01.jpg" && model.ShowImage5 == "images/signup_01.jpg") {
//                layermsg("请至少上传一张照片");
//                return false;
            //            }
            if (model.ShowImage1 == "images/signup_01.jpg") {
                model.ShowImage1 = "";
            }
            if (model.ShowImage2 == "images/signup_01.jpg") {
                model.ShowImage2 = "";
            }
            if (model.ShowImage3 == "images/signup_01.jpg") {
                model.ShowImage3 = "";
            }
            if (model.ShowImage4 == "images/signup_01.jpg") {
                model.ShowImage4 = "";
            }
            if (model.ShowImage5 == "images/signup_01.jpg") {
                model.ShowImage5 = "";
            }
            $.ajax({
                type: "post",
                url: "Handler.ashx",
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("资料提交成功！请等待审核,审核结果可在我的报名中查看");
                        setTimeout("window.location = 'MySignUp.aspx'", 3000);

                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })


        }


        function UploadFile1() {



        }

        function UploadFile2() {



        }

        function UploadFile3() {



        }

        function UploadFile4() {


        }

        function UploadFile5() {



        }
        //分享
        var shareTitle = "1000人疯抢400000元奖品，看美女拿奖品两不误！快来参加沙滩宝贝微信评选！";
        var shareDesc = "duang~~ 上海热带风暴水上乐园暑期盛大开园，BeachHoney沙滩宝贝隆重登场了，只需一分钟报名，玩乐两不误，海量奖品任你拿！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
        var shareLink = window.location.href;
        //分享


    </script>
</asp:Content>
