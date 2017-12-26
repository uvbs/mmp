<%@ Page Title="" Language="C#" MasterPageFile="~/customize/SuperTeam/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SuperTeam.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    团队报名
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/register.css" rel="stylesheet" type="text/css" />
    <style>
        .wrapRegister .content .row .col-75 .div-reset
        {
            height: 100px;
        }
        .wrapRegister .content .row .col-75 .div-reset .jiahao
        {
            top: 40px;
        }
        #imgshow1, #imgshow2
        {
            width: 100%;
            height:90px;
        }
         #txtContact,#txtEx6,#txtEx7,#txtPhone{color: #fff;}
         #ddlArea{height:30px;color:Black;}
         .row-erweima .col-erweima
        {
            padding: 10px 90px;
        }
        .row-logo .col-logo
        {
            padding: 10px 100px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapRegister">
        <div class="top">
            <img class="imgWidth" src="image/top.png">
        </div>
        <div class="content">
            <div class="row">
                <div class="col textC colorFFF font17">
                    团队报名
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    公司品牌
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入公司品牌" id="txtEx1">
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    团队名称
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入团队名称" id="txtVoteObjectName">
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    所在区域
                </div>
                <div class="col col-75">
                    <select class="select-reset" id="ddlArea">
                        <option value="">请选择所在区域</option>
                        <option value="黄浦、卢湾区">黄浦、卢湾区</option>
                        <option value="静安、闸北区">静安、闸北区</option>
                        <option value="徐汇区">徐汇区</option>
                        <option value="浦东新区(南)">浦东新区(南)</option>
                        <option value="浦东新区(北)">浦东新区(北)</option>
                        <option value="虹口、杨浦区">虹口、杨浦区</option>
                        <option value="宝山区">宝山区</option>
                        <option value="普陀、嘉定区">普陀、嘉定区</option>
                        <option value="长宁、青浦区">长宁、青浦区</option>
                        <option value="闵行区">闵行区</option>
                        <option value="松江、金（山）奉（贤）区">松江、金（山）奉（贤）区</option>
                        
                        


                    </select>
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    团队年限
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入团队成立年限（单位：年）" id="txtEx2" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" >
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    团队人数
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入团队人数（单位：人,不超过2000）" id="txtEx3" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" >
                </div>
            </div>
                        <div class="row">
                <div class="col textR colorFFF mTop5">
                    门店数量
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入门店数量（不超过99）" id="txtEx8" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" >
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    年度业绩
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入年度总业绩（单位：万元）" id="txtEx4" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" >
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    成交单数
                </div>
                <div class="col col-75">
                    <input class="input-reset" type="text" placeholder="请输入年度成交单数（单位：单）" id="txtEx5" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')" >
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    团队介绍
                </div>
                <div class="col col-75">
                    <textarea class="textarea-reset" id="txtIntroduction" placeholder="请输入团队介绍（200字内）"></textarea>
                    
                </div>
            </div>
            <div class="row">
                <div class="col textR colorFFF mTop5">
                    团队照片(点击加号上传)
                </div>
                <div class="col col-75">
                    <div class="div-reset" id="divshowimg1" onclick="txtThumbnailsPath1.click()">
                        <i class="icon iconfont icon-dcjia jiahao"></i><span class="span-reset"></span>
                    </div>
                    <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                </div>
                <div class="col col-75">
                    <div class="div-reset" id="divshowimg2" onclick="txtThumbnailsPath2.click()">
                        <i class="icon iconfont icon-dcjia jiahao"></i><span class="span-reset"></span>
                    </div>
                    <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                </div>
            </div>
            <div class="row">
                <div class="col textR mTop5">
                    团队领导
                </div>
                <div class="col col-75">
                    <label class="item item-input item-reset">
                        <div class="input-label colorFFF font12">
                            姓名：
                        </div>
                        <input type="text"  id="txtContact" placeholder="请输入姓名">
                    </label>
                </div>
            </div>
            <div class="row">
                <div class="col">
                </div>
                <div class="col col-75">
                    <label class="item item-input item-reset">
                        <div class="input-label colorFFF font12">
                            职务：
                        </div>
                        <input type="text"  id="txtEx6" placeholder="请输入职务">
                    </label>
                </div>
            </div>
            <div class="row">
                <div class="col">
                </div>
                <div class="col col-75">
                    <label class="item item-input item-reset">
                        <div class="input-label colorFFF font12">
                            从业年限：
                        </div>
                        <input type="text"  id="txtEx7" placeholder="请输入从业年限(单位:年)" >
                    </label>
                </div>
            </div>
            <div class="row">
                <div class="col">
                </div>
                <div class="col col-75">
                    <label class="item item-input item-reset">
                        <div class="input-label colorFFF font12">
                            联系电话：
                        </div>
                        <input type="text"  id="txtPhone" placeholder="请输入联系电话">
                    </label>
                </div>
            </div>
            <div class="padding">
                <button class="button button-block button-positive button-reset" id="btnSumbit">
                    提交
                </button>
            </div>
        </div>

                <div class="row row-erweima">
            <div class="col col-erweima">
                 <img class="imgWidth" src="image/qrcode.png" style="width:100px;height:auto;text-align:center;margin-left:20%;">
            </div>
        </div>
        <div class="row row-logo">
            <div class="col col-logo">
                <img class="imgWidth" src="image/xinwen.png">
            </div>
        </div>

        <div class="bottom">
            <div class="row">
                <div class="col borderLine" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shixinshouye shouye"></i>
                </div>
                <div class="col col-80">
                    <div class="row">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            参赛细则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col" onclick="window.location.href='Area.aspx'">
                            为Team投票
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#btnSumbit").click(function () {

                Apply();

            });

            $("#txtThumbnailsPath1").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=SuperTeam&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $(divshowimg1).html("<img src=" + resp.ExStr + " id=\"imgshow1\">");

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
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=SuperTeam&filegroup=file2',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath2',
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $(divshowimg2).html("<img src=" + resp.ExStr + " id=\"imgshow2\">");

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



        function Apply() {
            var model = {
                Action: "AddVoteObjectInfo",
                VoteObjectName: $.trim($(txtVoteObjectName).val()),
                Phone: $.trim($(txtPhone).val()),
                Introduction: $.trim($(txtIntroduction).val()),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                Ex1: $(txtEx1).val(), //品牌
                Ex2: $(txtEx2).val(), //团队年限
                Ex3: $(txtEx3).val(), //团队人数
                Ex4: $(txtEx4).val(), //团队业绩
                Ex5: $(txtEx5).val(), //成交单数
                Ex6: $(txtEx6).val(), //领导职务
                Ex7: $(txtEx7).val(), //从业年限
                Ex8:$(txtEx8).val(),//门店数量
                Area: $(ddlArea).val(),
                Contact: $(txtContact).val()

            }
            if (model.Ex1 == "") {
                layermsg("请输入公司品牌");
                return false;
            }
            if (model.VoteObjectName == "") {
                layermsg("请输入团队名称");
                return false;
            }

            if (model.Area == "") {
                layermsg("请选择区域");
                return false;
            }
            if (model.Ex2 == "") {
                layermsg("请输入团队年限");
                return false;
            }
            if (model.Ex3 == "") {
                layermsg("请输入团队人数");
                return false;
            }


            if (model.Ex4 == "") {
                layermsg("请输入年度业绩");
                return false;
            }
            if (model.Ex5 == "") {
                layermsg("请输入成交单数");
                return false;
            }
            if (model.Introduction == "") {
                layermsg("请输入团队宣言");
                return false;
            }

            if (model.ShowImage1 == undefined || model.ShowImage1 == "" || model.ShowImage2 == undefined || model.ShowImage2 == "") {
                layermsg("请上传团队照片");
                return false;
            }


            if (model.Contact == "") {
                layermsg("请输入名字");
                return false;
            }
            if (model.Ex6 == "") {
                layermsg("请输入职务");
                return false;
            }
            if (model.Ex7 == "") {
                layermsg("请输入从业年限");
                return false;
            }

            if (model.Phone == "") {
                layermsg("请输入联系电话");
                return false;
            }
            var phonereg = /^(13|14|15|17|18)\d{9}$/;
            if (!phonereg.test(model.Phone)) {
                layermsg("请输入有效的手机号码");
                return false;
            }






            $.ajax({
                type: "post",
                url: "Handler.ashx",
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("报名成功,请等待后台审核。");
                        setTimeout("window.location = 'MySignUp.aspx'", 2000);

                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })


        }




    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "SuperTeam";
        var shareDesc = "SuperTeam";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/SuperTeam/image/logo.jpg";
        var shareLink = window.location.href;
        //分享
</script>
</asp:Content>
