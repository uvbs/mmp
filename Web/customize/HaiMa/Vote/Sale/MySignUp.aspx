<%@ Page Title="" Language="C#" MasterPageFile="~/customize/HaiMa/Vote/Sale/Master.Master"
    AutoEventWireup="true" CodeBehind="MySignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale.MySignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    海马真英雄-我的报名
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .transform90
        {
            -webkit-transform: rotate(90deg);
            -o-transform: rotate(90deg);
            -ms-transform: rotate(90deg);
            transform: rotate(90deg);
        }
        
        .transform180
        {
            -webkit-transform: rotate(180deg);
            -o-transform: rotate(180deg);
            -ms-transform: rotate(180deg);
            transform: rotate(180deg);
        }
        
        .transform270
        {
            -webkit-transform: rotate(270deg);
            -o-transform: rotate(270deg);
            -ms-transform: rotate(270deg);
            transform: rotate(270deg);
        }
        .imgUpload
        {
            height: 60px;
            width: 50px;
        }
        .imgUpload img
        {
            width: 50px;
            height: 50px;
            margin-top: 4px;
        }
        .col{padding:0px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapUserCenter mBottom48">
        <div class="header">
            <img src="images/header.jpg" alt="">
        </div>
        <% if (model.Status.Equals(1))
           {%>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>我的参赛编号：</span>
                            <%=model.Number%>号
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
                            <span>我的票数：</span>
                            <%=model.VoteCount%>票
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
                            <span>我的排名：</span> 第<%=model.Rank%>名
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <%} %>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>姓名：</span>
                            <input type="text" value="<%=model.VoteObjectName %>" id="txtVoteObjectName" readonly="readonly">
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
                            <span>职位：</span>
                            <input type="text" value="<%=model.Ex1 %>" readonly="readonly">
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
                            <span>所属经销商：</span>
                            <input type="text" value="<%=model.Ex2%>" readonly="readonly">
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
                            <span>电话：</span>
                            <input type="text" readonly="readonly" value="<%=model.Phone %>">
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
                            <span>身份证号码：</span>
                            <input type="text" value="<%=CurrentUserInfo.Ex8 %>" readonly="readonly">
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
                            <span>审核状态：</span>
                            <% if (model.Status.Equals(1))
                               {%>
                            <input type="text" value="审核已通过" readonly="readonly">
                            <%}else
                               {%>
                               
                            <input type="text" value="等待审核" readonly="readonly">
                            <%} %>
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
                            <span>参赛宣言</span><br />
                            <textarea id="txtIntroduction"><%=model.Introduction %></textarea>
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
                            <span>我的照片</span>
                        </label>
                        <div>
                            请点击+号上传本人照片2~4张</div>
                        <div>
                            请注意照片大小在1M以内</div>
                        <div class="imgUploadList">
                            <div class="row">
                                <div class="col">
                                    <span id="spimgshow1" class="imgUpload" onclick="txtThumbnailsPath1.click()">
                                        <%if (!string.IsNullOrEmpty(model.ShowImage1))
                                          {%>
                                        <img id="imgshow1" src="<%=model.ShowImage1%>" />
                                        <%}%>
                                        <%else
                                            {%>
                                        <i class="iconfont icon-dcjia"></i>
                                        <%}%>
                                    </span>
                                    <div>
                                        <i class="iconfont icon-shuaxin" onclick="ChangeAngle1()"></i>
                                    </div>
                                    <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                                </div>
                                <div class="col">
                                    <span class="imgUpload" id="spimgshow2" onclick="txtThumbnailsPath2.click()">
                                        <%if (!string.IsNullOrEmpty(model.ShowImage2))
                                          {%>
                                        <img id="imgshow2" src="<%=model.ShowImage2%>" />
                                        <%}%>
                                        <%else
                                            {%>
                                        <i class="iconfont icon-dcjia"></i>
                                        <%}%>
                                    </span>
                                    <div>
                                        <i class="iconfont icon-shuaxin" onclick="ChangeAngle2()"></i>
                                    </div>
                                    <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                                </div>
                                <div class="col">
                                    <span class="imgUpload" id="spimgshow3" onclick="txtThumbnailsPath3.click()">
                                        <%if (!string.IsNullOrEmpty(model.ShowImage3))
                                          {%>
                                        <img id="imgshow3" src="<%=model.ShowImage3%>" />
                                        <%}%>
                                        <%else
                                            {%>
                                        <i class="iconfont icon-dcjia"></i>
                                        <%}%>
                                    </span>
                                    <div>
                                        <i class="iconfont icon-shuaxin" onclick="ChangeAngle3()"></i>
                                    </div>
                                    <input type="file" id="txtThumbnailsPath3" name="file3" style="display: none;" />
                                </div>
                                <div class="col">
                                    <span class="imgUpload" id="spimgshow4" onclick="txtThumbnailsPath4.click()">
                                        <%if (!string.IsNullOrEmpty(model.ShowImage4))
                                          {%>
                                        <img id="imgshow4" src="<%=model.ShowImage4%>" />
                                        <%}%>
                                        <%else
                                            {%>
                                        <i class="iconfont icon-dcjia"></i>
                                        <%}%>
                                    </span>
                                    <div>
                                        <i class="iconfont icon-shuaxin" onclick="ChangeAngle4()"></i>
                                    </div>
                                    <input type="file" id="txtThumbnailsPath4" name="file4" style="display: none;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapVideo mTop10">
            <div class="row title">
                <div class="col">
                    课题1：<%=model.Ex3%></div>
            </div>
            <div class="row videoPlay">
                <div class="col">
                    <%if (!string.IsNullOrEmpty(model.Ex4))
                      {%>
                    <video id="video1" src="<%=model.Ex4%>" wdith="200px;" controls="controls">
                        
</video>
                    <%} %>
                </div>
            </div>
            <div class="row title">
                <div class="col">
                    课题2：<%=model.Ex5%></div>
            </div>
            <div class="row videoPlay">
                <div class="col">
                    <%if (!string.IsNullOrEmpty(model.Ex6))
                      {%>
                    <video id="video2" src="<%=model.Ex6%>" wdith="300px;" preload="preload" controls="controls">
Your browser does not support the video tag.
</video>
                    <%} %>
                </div>
            </div>
        </div>
        <div class="wrapBtn">
            <a href="javascript:;" id="btnSumbit">确定提交</a>
        </div>
        <div class="footer">
            <img src="images/footer.png" alt="">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        try {
            var myVideo = document.getElementById("video1");
            myVideo.height = 166;
            var myVideo2 = document.getElementById("video2");
            myVideo2.height = 166;
        } catch (e) {

        }


        angleArr = [0, 90, 180, 270];
        var imgAngle1 = 0;
        var imgAngle2 = 0;
        var imgAngle3 = 0;
        var imgAngle4 = 0;

        $(function () {
            $("#btnSumbit").click(function () {

                UpdateMyInfo();

            });

            $("#txtThumbnailsPath1").live('change', function () {

                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=HaiMaVoteSale&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'text',
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#spimgshow1").html("<img id=\"imgshow1\" src=" + resp.ExStr + ">");
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
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=HaiMaVoteSale&filegroup=file2',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath2',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#spimgshow2").html("<img id=\"imgshow2\" src=" + resp.ExStr + ">");
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
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=HaiMaVoteSale&filegroup=file3',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath3',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#spimgshow3").html("<img id=\"imgshow3\" src=" + resp.ExStr + ">");
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
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=HaiMaVoteSale&filegroup=file4',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath4',
                        dataType: 'text',
                        success: function (result) {

                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#spimgshow4").html("<img id=\"imgshow4\" src=" + resp.ExStr + ">");
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


        function UpdateMyInfo() {
            var model = {
                Action: "EditVoteObjectInfo",
                VoteObjectName: $.trim($(txtVoteObjectName).val()),
                Introduction: $.trim($(txtIntroduction).val()),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                ShowImage3: $("#imgshow3").attr("src"),
                ShowImage4: $("#imgshow4").attr("src"),
                imgAngle1: imgAngle1,
                imgAngle2: imgAngle2,
                imgAngle3: imgAngle3,
                imgAngle4: imgAngle4

            }

            if (model.VoteObjectName == "") {
                layermsg("请输入你的姓名");
                return false;
            }

            if (model.Introduction == "") {
                layermsg("请输入参赛宣言");
                return false;
            }
            if (model.ShowImage1 == "images/signup_01.jpg") {
                layermsg("请上传第一张照片");
                return false;
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

            $.ajax({
                type: "post",
                url: handlerUrl,
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("个人资料更新成功！");
                        setTimeout("window.location.href='MySignUp.aspx'", 2000);

                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })


        }


    </script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "海马精英营销大赛,大奖等你来!",
                desc: "海马精英营销大赛,大奖等你来!",
                link: 'http://<%=Request.Url.Host %>/customize/haima/vote/sale/Detail.aspx?id=<%=model.AutoID%>',
                imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
            })
        })
    </script>
</asp:Content>
