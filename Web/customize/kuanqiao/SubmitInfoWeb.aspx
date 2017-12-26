<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitInfoWeb.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.kuanqiao.SubmitInfoWeb" %>

<!DOCTYPE html>
<html>
<head>
    <title>申请企业核名-企业帮</title>
    <meta charset="utf-8">
    <link href="/customize/kuanqiao/css/SubmitInfoWeb.css?v=0001" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="toper">
    <div class="header">
        <div class="logo"></div>
        <div class="describe"></div>
        <a href="#" class="mainbtn"></a>
        <div class="personalinfo">您好，<%=userInfo.WXNickname%> 欢迎登陆宽桥企业帮!&nbsp;&nbsp;&nbsp;&nbsp;<a class="logout" href="/QLogin.aspx?redirecturl=/customize/kuanqiao/SubmitInfoWeb.aspx">退出</a></div>
    </div>
</div>
<div class="step">
    <div class="bgbar">
        <div class="leftpoint"></div>
        <div class="rightpoint"></div>
    </div>
    <div class="stepbox current step1">
        <span class="stepnum">1</span>
        <h2>提交审核</h2>
    </div>
    <div class="stepbox step2">
        <span class="stepnum">2</span>
        <h2>等待审核</h2>
    </div>
    <div class="stepbox step3">
        <span class="stepnum">3</span>
        <h2>审核完成</h2>
    </div>
</div>
<div class="concent">

    <p class="title">请填写以下资料(标<span>*</span>为必填)</p>
    <form id="formsignin">
        <label for="K2"><span>*</span>申请企业名称</label>
        <input name="K2" id="K2" type="text"  placeholder="企业名称"  >
        <label for="K3"><span>&nbsp;</span>备选企业名称</label>
        <div class="inputbox">
            <input name="K3" id="K3" type="text" class="fleft"  placeholder="备选企业名称(选填)" >
            <input name="K4" id="K4" type="text" class="fleft"  placeholder="备选企业名称2" style="display:none;" >
            <input name="K5" id="K5" type="text" class="fleft"  placeholder="备选企业名称3" style="display:none;">
        </div>
        <label for="K6"><span>*</span>注册资本（万元）</label>
        <input name="K6" id="K6" type="text" onkeyup="this.value=this.value.replace(/\D|^0/g,'')"  >
        <label for="K7" style="display:none;"><span>*</span>企业类型</label>
        <select name="K7" id="K7" style="display:none;">
            <option value=''>请选择</option>
            <option value="有限责任公司">有限责任公司</option>
            <option value="非公司企业法人">非公司企业法人</option>
            <option value="合伙企业">合伙企业</option>
            <option value="股份有限公司">股份有限公司</option>
            <option value="企业非法人分支机构">企业非法人分支机构</option>
            <option value="个人独资企业">个人独资企业 </option>
            <option value="分公司">分公司</option>
            <option value="营业单位">营业单位</option>
            <option value="其他">其他</option>
        </select>
        <label for="K8"><span>&nbsp;</span>经营范围</label>
        <input type="text"  name="K8" id="K8" placeholder="经营范围(选填)"  />

        <div class="partner">
        <label for="K9"><span>*</span>投资人1</label>
        <div class="inputbox">
            <input type="text" name="K9" id="K9"  class="fleft" placeholder="姓名" style="display:none;">
            <input type="text" name="K10" id="K10" class="fleft wide" placeholder="证照号码" style="display:none;">
            <button type="button" class="idcard" id="btnfile1front">点击上传身份证正面照</button>
            <button type="button" class="idcard" id="btnfile1back">点击上传身份证反面照</button>
            <input type="file" id="file1front" name="file1front" style="display:none;" />
            <input type="file" id="file1back" name="file1back" style="display:none;" />
        </div>
        </div>


<%--        <div class="partner">
        <label for="K11"><span>&nbsp;</span>投资人2(选填)</label>
        <div class="inputbox">
            <input type="text" name="K11" id="K11" class="fleft" placeholder="姓名" style="display:none;">
            <input type="text" name="K12" id="K12" class="fleft wide" placeholder="证照号码" style="display:none;">
            <button type="button" class="idcard" id="btnfile2front">点击上传身份证正面照</button>
            <button type="button" class="idcard" id="btnfile2back">点击上传身份证反面照</button>
            <input type="file" id="file2front" name="file2front" style="display:none;" />
            <input type="file" id="file2back" name="file2back" style="display:none;" />

        </div>
        </div>--%>

<%--        <div class="partner">
        <label for="K13"><span>&nbsp;</span>投资人3(选填)</label>
        <div class="inputbox">
            <input type="text" name="K13" id="K13" class="fleft" placeholder="姓名" style="display:none;">
            <input type="text" name="K14" id="K14" class="fleft wide" placeholder="证照号码" style="display:none;">
            <button type="button" class="idcard" id="btnfile3front">点击上传身份证正面照</button>
            <button type="button" class="idcard" id="btnfile3back">点击上传身份证反面照</button>
            <input type="file" id="file3front" name="file3front" style="display:none;" />
            <input type="file" id="file3back" name="file3back" style="display:none;" />

        </div>
        </div>--%>

        <span class="addpartner" id="spaddpartner" onclick="addpartner.add();">添加投资人</span>
        
        <label for="txtName"><span>*</span>联系姓名</label>
        <input name="Name" id="txtName" type="text"  placeholder="联系姓名"  >
        <label for="txtName"><span>*</span>联系手机号码</label>
        <input name="Phone" id="txtPhone" onkeyup="this.value=this.value.replace(/\D|^0/g,'')" type="text"  placeholder="联系手机号码"  >
        <label for="txtName" style="display:none;"><span>*</span>联系Email</label>
        <input name="K1" id="K1" type="text"   placeholder="联系Email" style="display:none;" >
        <input id="activityID" type="hidden" value="130725" name="ActivityID" />
        <input id="loginName" type="hidden" value="a3VhbnFpYW8=" name="LoginName" />
        <input id="loginPwd" type="hidden" value="5D9F5A5A!E18DAEE54F7BC2501F5!8#B" name="LoginPwd" />
        <input id="WXCurrOpenerOpenID" type="hidden" value="<%=WxOpenId%>" name="WXCurrOpenerOpenID" />
        <input type="hidden" value="待处理" name="K15" />
        <input type="hidden" value="待处理" name="K16" />
        <input type="hidden" value="" name="K18" id="K18" />
        <input type="hidden" value="WeixinOpenID,K2,K15" name="DistinctKeys" />
        <button class="submit" id="btnSignIn"></button>
    </form>
   <div class="footer">
        ICP备 11027501 号 公网安备 11010802012285 号 
        <a href="https://www.sgs.gov.cn/shaic/" class="wangjinicon"></a>
        Copyright © 2014  宽桥企业帮. All Rights Reserved.
    </div>


</div>
</body>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
<script type="text/javascript">

    setInterval("CheckIE()", 1000);
    $(function () {

       

        $("#btnSignIn").live("click", function () {

            var Name = $.trim($("#txtName").val());
            var Phone = $.trim($("#txtPhone").val());
            var Email = $.trim($("#K1").val());
            var K2 = $.trim($("#K2").val());
            var K6 = $.trim($("#K6").val()); //
            var K7 = $.trim($("#K7").val()); //企业类型
            var K9 = $.trim($("#K9").val());
            var K10 = $.trim($("#K10").val());
            //            var K11 = $.trim($("#K11").val());
            //            var K12 = $.trim($("#K12").val());
            //            var K13 = $.trim($("#K13").val());
            //            var K14 = $.trim($("#K14").val());
            if (K2 == "") {
                $("#K2").focus();
                $("#K2").addClass("fleft wrong");
                return false;
            } else {
                $("#K2").removeClass("fleft wrong");
            }
            if (K6 == "") {
                $("#K6").focus();
                $("#K6").addClass("fleft wrong");
                return false;
            }
            else {
                $("#K6").removeClass("fleft wrong");
            }
            //            if (K7 == "") {
            //                alert("请选择企业类型");
            //                return false;
            //            }
            //            else {
            //                $("#K7").removeClass("fleft wrong");
            //            }

            //            if (K9 == "") {
            //                $("#K9").focus();
            //                $("#K9").addClass("fleft wrong");
            //                return false;
            //            }
            //            else {
            //                $("#K9").removeClass("fleft wrong");
            //            }
            //            if (K10 == "") {
            //                $("#K10").focus();
            //                $("#K10").addClass("fleft wrong");
            //                return false;
            //            }
            //            else {
            //                $("#K10").removeClass("fleft wrong");
            //            }

            if (Name == "") {
                $("#txtName").focus();
                $("#txtName").addClass("fleft wrong");
                return false;

            }
            else {
                $("#txtName").removeClass("fleft wrong");
            }
            if (Phone == "") {
                $("#txtPhone").focus();
                $("#txtPhone").addClass("fleft wrong");
                return false;
            }
            else {
                $("#txtPhone").removeClass("fleft wrong");
            }

            //            if (Email == "") {
            //                $("#K1").focus();
            //                $("#K1").addClass("fleft wrong");
            //                return false;
            //            }
            //            else {
            //                $("#K1").removeClass("fleft wrong");
            //            }


            if ($("#img1front").attr("src") == "" || $("#img1front").attr("src") == undefined) {
                alert("请上传投资人1正面身份证照片");
                return false;
            }

            if ($("#img1back").attr("src") == "" || $("#img1back").attr("src") == undefined) {
                alert("请上传投资人1反面身份证照片");
                return false;
            }


            var imglist = [];
            imglist.push($("#img1front").attr("src"));
            imglist.push($("#img1back").attr("src"));

            if ($("#img2front").attr("src") != undefined && $("#img2front").attr("src") != "") {
                imglist.push($("#img2front").attr("src"));

            }
            if ($("#img2back").attr("src") != undefined && $("#img2back").attr("src") != "") {
                imglist.push($("#img2back").attr("src"));

            }

            if ($("#img3front").attr("src") != undefined && $("#img3front").attr("src") != "") {
                imglist.push($("#img3front").attr("src"));

            }
            if ($("#img3back").attr("src") != undefined && $("#img3back").attr("src") != "") {
                imglist.push($("#img3back").attr("src"));

            }
            $("#K18").val(imglist.join(','));
            try {
                var option = {
                    url: "/serv/ActivityApiJson.ashx",
                    type: "post",
                    dataType: "text",
                    timeout: 60000,
                    success: function (result) {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {//清空
                            $('input:text').val("");
                            $('textarea').val("");
                            //alert("申请提交成功!");
                            window.location.href = '/customize/kuanqiao/SubmitInfoWebStatus.htm';
                        }
                        else if (resp.Status == 1) {
                            alert("企业名称重复");

                        }
                        else {
                            alert(resp.Msg);

                        }

                    },
                    fail: function () {
                        alert("网络超时，请重新提交");

                    }
                };
                $("#formsignin").ajaxSubmit(option);
                return false;

            }
            catch (e) {
                alert(e);
            }

        })

        $("#btnfile1front").click(function () {

            if (!$.browser.msie) {

                $("#file1front").click();

            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }






        });

        $("#btnfile1back").click(function () {

            if (!$.browser.msie) {

                $("#file1back").click();

            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }





        });

        $("#btnfile2front").live("click", function () {

            if (!$.browser.msie) {


                $("#file2front").click();
            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }





        });

        $("#btnfile2back").live("click", function () {

            if (!$.browser.msie) {

                $("#file2back").click();

            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }





        });

        $("#btnfile3front").live("click", function () {

            if (!$.browser.msie) {


                $("#file3front").click();
            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }





        });

        $("#btnfile3back").live("click", function () {

            if (!$.browser.msie) {

                $("#file3back").click();

            }
            else {
                alert("请点击下方浏览按钮上传图片");
            }




        });

        $("#file1front").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file1front',
                         secureuri: false,
                         fileElementId: 'file1front',
                         dataType: 'json',
                         success: function (resp) {

                             try {
                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img1front' width='345' src=" + resp.ExStr + ">";

                                     $("#btnfile1front").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });
        //
        $("#file1back").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file1back',
                         secureuri: false,
                         fileElementId: 'file1back',
                         dataType: 'json',
                         success: function (resp) {

                             try {
                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img1back' width='345' src=" + resp.ExStr + ">";
                                     $("#btnfile1back").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });

        //
        $("#file2front").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file2front',
                         secureuri: false,
                         fileElementId: 'file2front',
                         dataType: 'json',
                         success: function (resp) {

                             try {
                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img2front' width='345' src=" + resp.ExStr + ">";

                                     $("#btnfile2front").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });
        //
        $("#file2back").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file2back',
                         secureuri: false,
                         fileElementId: 'file2back',
                         dataType: 'json',
                         success: function (resp) {

                             try {

                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img2back' width='345' src=" + resp.ExStr + ">";
                                     $("#btnfile2back").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });

        $("#file3front").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file3front',
                         secureuri: false,
                         fileElementId: 'file3front',
                         dataType: 'json',
                         success: function (resp) {

                             try {
                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img3front' width='345' src=" + resp.ExStr + ">";

                                     $("#btnfile3front").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });
        //
        $("#file3back").live('change', function () {
            try {

                $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=qiyebang&filegroup=file3back',
                         secureuri: false,
                         fileElementId: 'file3back',
                         dataType: 'json',
                         success: function (resp) {

                             try {
                                 if (resp.Status == 1) {
                                     var imghtml = "<img id='img3back' width='345'  src=" + resp.ExStr + ">";
                                     $("#btnfile3back").html(imghtml);
                                 }
                                 else {
                                     alert(resp.Msg);
                                 }

                             } catch (e) {
                                 alert(e);
                             }

                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });


    })

    function CheckIE() {
       
        if ($.browser.msie) { //ie 下


            $("#file1front").show();
            $("#file1back").show();
            $("#file2front").show();
            $("#file2back").show();
            $("#file3front").show();
            $("#file3back").show();

        }


    }
           
       
   
</script>

<script type="text/javascript">

    var addpartner = {
        add: function () {
            var _this = this;
            var partnernum = $(".partner").length;
            if (partnernum >= 3) {

                $("#spaddpartner").html("最多可以添加" + partnernum + "个投资人");
                return false;
            }
            partnernum++;
            $(".addpartner").before("<div class='partner'><label for=''>投资人<em></em></label><span class='delpartner'>删除投资人</span><div class='inputbox'><button type='button' class='idcard' id='btnfile" + partnernum + "front'>身份证正面照</button><button type='button' class='idcard' id='btnfile" + partnernum + "back'>身份证反面照</button><input type='file' id='file" + partnernum + "front' name='file" + partnernum + "front' style='display:none;' /><input type='file' id='file" + partnernum + "back' name='file" + partnernum + "back' style='display:none;'/> </div></div>");
            partnernum++;
            $(".delpartner").unbind("click");
            $(".delpartner").bind("click", function () {
                if ($(this).parent(".partner").next(".partner").length >= 1) {
                    alert("请从最后一个投资人开始删除");
                    return false;
                }
                $(this).parent(".partner").remove();
                $("#spaddpartner").html("添加投资人");

            });
            num();
        }
    }

    function num() {
       
        $(".partner").each(function (e) {
           
            $(this).find("em").text(e + 1)
        })
    
    
    }


   

</script>


</html>
