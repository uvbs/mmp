<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreeWearApply.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.FreeWearApply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>免费穿报名</title>
    <link type="text/css" rel="stylesheet" href="css/style.css?v=1.0.0.1" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
    <style type="text/css">
        select
        {
            height: 30px;
            border-radius: 5px;
            width: 95%;
        }
        .menu5 ul li
        {
            width: 33.33%;
        }
        #formsubmit
        {
            margin-bottom: 50px;
        }
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <div id="main_panels">
                                <div class="form">
                                    <form method="post" id="formsubmit">
                                    <h2 class="page_title">
                                        校服免费穿活动报名
                                    </h2>
                                    <table width="96%" style="margin-left: 2%">
                                        <tr>
                                            <td align="center" style="">
                                                <div style="margin-top: 10px">
                                                </div>
                                                <table class="enroll-tb">
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子姓名：</dd>
                                                            <input type="text" name="K1" id="txtName" value="" placeholder="请输入孩子的昵称或姓名" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子的出生年月：</dd>
                                                            <input type="text" name="K2" id="txtBirthDay" value="" placeholder="请输入孩子的出生年月"
                                                                class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子的性别：</dd>
                                                            <input type="text" name="K3" id="txtGender" value="" placeholder="男|女" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子的身高：</dd>
                                                            <input type="text" name="K4" id="txtHeight" value="" placeholder="单位:厘米" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子的体重：</dd>
                                                            <input type="text" name="K5" id="txtWeight" value="" placeholder="单位:公斤" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                胸围：</dd>
                                                            <input type="text" name="K6" id="txtBust" value="" placeholder="单位:厘米" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                腰围：</dd>
                                                            <input type="text" name="K7" id="txtWaist" value="" placeholder="单位:厘米" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                臀围：</dd>
                                                            <input type="text" name="K8" id="txtHip" value="" placeholder="单位:厘米" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                臂展：</dd>
                                                            <input type="text" name="K9" id="txtWingspan" value="" placeholder="手臂张开后,两手指尖的距离" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子就读的学校：</dd>
                                                            <input type="text" name="K10" id="txtSchoolName" value="" placeholder="请输入孩子就读学校的全称" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                新品校服寄送地址：</dd>
                                                            <input type="text" name="K11" id="txtAddress" value="" placeholder="请输入您的地址" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                家长姓名：</dd>
                                                            <input type="text" name="Name" id="txtContact" value="" placeholder="请输入收件人姓名" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                联系电话：</dd>
                                                            <input type="text" name="Phone" id="txtPhone" value="" placeholder="请输入联系电话" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                孩子的照片：<a href="javascript:;" id="btnAngle" class="hidden" ></a>
                                                            </dd>
                                                            <img src="images/upload.png" id="imgclass" class="mAll50" onclick="txtThumbnailsPath.click();" />
                                                            <input type="file" id="txtThumbnailsPath" name="file1" style="display: none;" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <input type="button" name="submit" class="form_submit radius4 blue" id="btnSubmit"
                                                                value="提交报名" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--hidden -->
                                    <input id="activityID" type="hidden" value="314081" name="ActivityID" />
                                    <input id="loginName" type="hidden" value="dG90ZW1h" name="LoginName" />
                                    <input id="loginPwd" type="hidden" value="E51514701CFAF#020CEEEB4!B!2D298B" name="LoginPwd" />
                                    <input type="hidden" value="Name,Phone" name="DistinctKeys" />
                                    <input type="hidden" value="" id="K12" name="K12" />
                                    <!--hidden -->
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div class="menu5">
            <ul>
                <li><a href="#"><span>
                    <img src="images/en1.png" alt="" title="" />活动详情</span></a></li>
                <li><a href="index.aspx"><span>
                    <img src="http://jkbp.comeoncloud.net/open/jkbp/images/home1.png" />
                </span></a></li>
                <li><a href="#"><span>
                    <img src="images/en2.png" alt="" title="" />
                </span></a></li>
            </ul>
        </div>
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {

        $("#btnSubmit").click(function () {
            //
            var Name = $("#txtName").val();
            var Contact = $.trim($("#txtContact").val());
            var Phone = $.trim($("#txtPhone").val());


            if (Name == "") {
                alert("请输入孩子姓名");

                return false;
            }

            if (Contact == "") {
                alert("请输入家长姓名");

                return false;
            }
            if (Phone == "") {
                alert("请输入联系电话");
                return false;
            }

            $("#formsubmit").ajaxSubmit({
                url: "/serv/ActivityApiJson.ashx",
                type: "post",
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 0) {//清空
                        alert("提交成功!");
                        return;

                    }
                    else if (resp.Status == 1) {
                        alert("重复提交!");

                    }
                    else {
                        alert(resp.Msg);
                    }

                }
            });
            return false;

            //
        });

        $("#txtThumbnailsPath").live('change', function () {
            try {
                $.ajaxFileUpload({
                    url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                    secureuri: false,
                    fileElementId: 'txtThumbnailsPath',
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.Status == 1) {
                            $("#imgclass").attr("src", resp.ExStr);
                            $("#K12").val(resp.ExStr);
                            

                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }
        });

    })

</script>
</html>
