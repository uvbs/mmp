<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Iluxday/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Iluxday.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我要报名
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/register.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #imgshow1
        {
            max-height: 400px;
            width: 100%;
        }
        .wrapRegister
        {
            background-size: 100%;
            background-repeat: no-repeat;
        }
        .wrapRegister .contact
        {
          
            margin-top: 30px;
            
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapRegister">
        <div class="null">
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col pTop11">
                    <div class="">
                        <div class="line">
                        </div>
                        <div class="wrapBtn register">
                            <a href="javascript:;">参赛者报名表</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput mTopZero24">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>姓名</span>
                            <input type="text" value="" id="txtVoteObjectName" placeholder="请输入您的姓名">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>手机</span>
                            <input type="text" value="" id="txtPhone" placeholder="请输入您的手机号">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="word">
            *必须填写正确号码信息，以便主办方联系
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span class="share">分享理由（7个字以上）</span><br />
                            <textarea class="textArea" id="txtIntroduction" placeholder="在此输入分享理由"></textarea>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span class="share">上传照片</span>
                        </label>
                        <div class="pLeft16 color000000">
                            请报名者<span class="colorC38749">点击加号</span>上传获赞<span class="colorC38749">微信截屏1张</span>，</div>
                        <div class="pLeft16 color000000">
                            请注意照片附件大小在1MB以内。</div>
                        <div class="imgUploadList">
                            <div class="row">
                                <div class="col" id="spimg">
                                    <span class="imgUpload" onclick="txtThumbnailsPath1.click()">
                                        <%--                                    <i class="iconfont icon-tupian colorGrey_6">
                                    </i>--%>
                                        <i class="iconfont icon-tianjia color000000"></i></span>
                                    <div onclick="txtThumbnailsPath1.click()">
                                        <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="word">
            提示：资料提交,审核通过后不可修改。
        </div>
        <div class="wrapBtn">
            <a id="btnSumbit" href="javascript:;">确认提交</a>
        </div>
        <div class="contact">
            扫描二维码<br />
            获取活动最新进程<br />
            了解奖品领取方法<br />
            咨询活动相关事项
        </div>
        <div class="erweima">
            <img src="images/erweima.png" class="erweimaImg0" />
        </div>
        <div class="intro">
            “爱奢汇”-专业跨境进口电商平台，精致生活，<br />
            我们只做最赞的！<br />
            <span class="">长按二维码，关注爱奢汇马上明白</span><br />
            <span class="zhubanfang">活动主办方：跨境电商爱奢汇</span>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine pLeft0" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shouye shouye"></i>
                </div>
                <div class="col col-80 pLeft0 pRight0">
                    <div class="row pLeft0 pRight0">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            活动规则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            我要报名
                        </div>
                        <div class="col pRight0" onclick="window.location.href='List.aspx'">
                            为TA点赞
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
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Iluxday&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $("#spimg").html("<img src=" + resp.ExStr + " id=\"imgshow1\">");

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
                ShowImage1: $("#imgshow1").attr("src")

            }
            if (model.VoteObjectName == "") {
                layermsg("请输入姓名");
                return false;
            }

            if (model.Phone == "") {
                layermsg("请输入手机号");
                return false;
            }
            var phonereg = /^(13|14|15|17|18)\d{9}$/;
            if (!phonereg.test(model.Phone)) {
                layermsg("请输入有效的手机号码");
                return false;
            }

            if (model.Introduction == "") {
                layermsg("请输入分享理由");
                return false;
            }
            if (model.Introduction.length < 7) {
                layermsg("分享理由七个字以上");
                return false;
            }
            if (model.ShowImage1 == undefined || model.ShowImage1 == "") {
                layermsg("请上传照片");
                return false;
            }


            $.ajax({
                type: "post",
                url: handlerPath,
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
        var shareTitle = "【爱奢汇】秀最赞微信，赢最赞时尚大奖";
        var shareDesc = "秀最赞微信，赢最赞时尚大奖。选取朋友圈获赞微信内容上传，即表示报名成功，小伙伴们可以火热开启拉票啦！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Iluxday/images/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
