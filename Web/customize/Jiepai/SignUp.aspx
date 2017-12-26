<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Jiepai/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Jiepai.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    全球街拍报名
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/enlist.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <header class="content">
        <header class="row">
            <div class="col">
                <img src="images/header/log.png" class="full-image">
            </div>
        </header>
        <header class="row padding-add-center">
            <div class="col col-33 col-center">
                <img src="images/header/switzerland.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/newzealand.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/victoria.png" class="full-image">
            </div>
        </header>
        <header class="row padding-add-center">
            <div class="col col-33 col-center">
                <img src="images/header/news.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/lillydale.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/qee.png" class="full-image">
            </div>
        </header>
    </header>
    <section class="content">
        <div class="row padding-add-center">
            <div class="rol">
                <img src="images/enlist/log.png" class="full-image">
            </div>
        </div>
        <div class="row">
            <div class="col help-fixed">
                <label class="enlist-label font" for="for-list-1">国家及城市</label>
                <%
                    if (model != null)
                    {
                        
                %>
                <input type="text" placeholder="输入Qee粉所在地" readonly="readonly" class="full-image enlist-input" value="<%=model.Address %>" id="for-list-1">
                <%
                    }
                    else
                    {
                %>
                <input type="text" placeholder="输入Qee粉所在地" class="full-image enlist-input" id="for-list-1">
                <%
                    }    
                %>
            </div>
        </div>
        <div class="row">
            <div class="col help-fixed">
                <label class="enlist-label font" for="for-list-2">微信号</label>
                <%
                    if (model != null)
                    {
                %>
                <input type="text" placeholder="输入微信号" value="<%=model.Ex1 %>" readonly="readonly" class="full-image enlist-input" id="for-list-2">
                <%
                    }
                    else
                    {
                %>
                <input type="text" placeholder="输入微信号" class="full-image enlist-input" id="for-list-2">
                <%
                    }    
                %>
            </div>
        </div>
       <div class="row text-center">
            <div class="col text-center">
                <p class="font font-red tip">请您务必确认以上信息无误，提交后将无法修改！</p>
            </div>
        </div>
        <div class="row">
            <div class="col help-fixed">
                <label class="enlist-label-textarea font" for="for-list-3">
                    全球粉丝祝福接力<span
                        class="font-green">(限20字内)</span></label>
                <%
                    if (model != null)
                    {
                %>
                <textarea rows="4" placeholder="世界那么大，我替你去走走！" readonly="readonly" class="full-image enlist-textarea"
                    id="for-list-3"><%=model.Introduction %></textarea>
                <%
                    }
                    else
                    {
                %>
                <textarea rows="4" placeholder="世界那么大，我替你去走走！" class="full-image enlist-textarea"
                    id="for-list-3"></textarea>
                <%
                    }
                %>
            </div>
        </div>
    </section>


    <section class="content">
        <div class="text-center" id="help-height">
            <img src="images/enlist/select.png" id="enlist-select-image">
            <%if (model != null)
              {
                  %>
                    <select id="enlist-select" class="font">
                <option>请选择拍摄主题</option>
                <%
                    if (model.Ex2 == "Qee+地标建筑/其他")
                    {
                        %>
                            <option selected="selected">Qee+地标建筑/其他</option>
                        <%
                    }
                    else
                    {
                        %>
                            <option>Qee+地标建筑/其他</option>
                        <%
                    }
                %>
                <%
                    if (model.Ex2 == "Qee+风景/其他")
                    {
                        %>
                            <option selected="selected">Qee+风景/其他</option>
                        <%
                    }
                    else
                    {
                        %>
                            <option>Qee+风景/其他</option>
                        <%
                    }
                %>
                <%
                    if (model.Ex2 == "Qee+特色美食/人文")
                    {
                        %>
                            <option selected="selected">Qee+特色美食/人文</option>
                        <%
                    }
                    else
                    {
                        %>
                             <option>Qee+特色美食/人文</option>
                        <%
                    }
                 %>
                <%
                    if (model.Ex2 == "Qee+创意搞笑/其他")
                    {
                        %>
                            <option selected="selected">Qee+创意搞笑/其他</option>
                        <%
                    }
                    else
                    {
                        %>
                            <option>Qee+创意搞笑/其他</option>
                        <%
                    }
                %>
            </select>
                  <%
              }
              else
              {
                  %>
             <select id="enlist-select" class="font">
                    <option>请选择拍摄主题</option>
                    <option>Qee+地标建筑/其他</option>
                    <option>Qee+风景/其他</option>
                    <option>Qee+特色美食/人文</option>
                     <option>Qee+创意搞笑/其他</option>
                   </select>
                  <%
              }
             %>
            
        </div>
    </section>



    <section class="content">
        <div class="row">
            <div class="col">
                <div id="upload" class="text-center font">
                    <p>上传照片<span class="font-green">(每张照片大小需在1MB以内)</span></p>

                    <div class="upload-content">
                        <div class="upload-content-div" onclick="txtThumbnailsPath1.click()">
                            <%
                                if (model != null)
                                {
                            %>
                            <img src="<%=model.ShowImage1%>" id="imgshow1">
                            <%
                                }
                                else
                                {
                            %>
                            <img src="images/enlist/uploadpic.png" id="imgshow1">
                            <%
                                }
                            %>
                            <input type="file" id="txtThumbnailsPath1" value="" name="file1" style="display: none;" />
                        </div>
                        <p class="font-green" id="font-green1">Qee+地标建筑/其他</p>
                        <span class="font-green iconfont icon-shuaxin" onclick="ChangeAngle1()"></span>

                    </div>
                    <div class="upload-content">
                        <div class="upload-content-div" onclick="txtThumbnailsPath2.click()">
                            <%
                                if (model != null)
                                {
                            %>
                            <img src="<%=model.ShowImage2%>" id="imgshow2">
                            <%
                                }
                                else
                                {
                            %>
                            <img src="images/enlist/uploadpic.png" id="imgshow2">
                            <%
                                }
                            %>

                            <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                        </div>
                        <p class="font-green" id="font-green2">Qee+地标建筑/其他</p>
                        <span class="font-green iconfont icon-shuaxin" onclick="ChangeAngle2()"></span>

                    </div>
                    <div class="upload-content">
                        <div class="upload-content-div" onclick="txtThumbnailsPath3.click()">
                            <%
                                if (model != null)
                                {
                            %>
                            <img src="<%=model.ShowImage3%>" id="imgshow3">
                            <%
                                }
                                else
                                {
                            %>
                            <img src="images/enlist/uploadpic.png" id="imgshow3">
                            <%
                                }
                            %>
                            <input type="file" id="txtThumbnailsPath3" name="file3" style="display: none;" />
                        </div>
                        <p class="font-green" id="font-green3">Qee+地标建筑/其他</p>
                        <span class="font-green iconfont icon-shuaxin" onclick="ChangeAngle3()"></span>

                    </div>
                    <div class="upload-content">
                        <div class="upload-content-div" onclick="txtThumbnailsPath4.click()">
                            <%
                                if (model != null)
                                {
                            %>
                            <img src="<%=model.ShowImage4%>" id="imgshow4">
                            <%
                                }
                                else
                                {
                            %>
                            <img src="images/enlist/uploadpic.png" id="imgshow4">
                            <%
                                }
                            %>
                            <input type="file" id="txtThumbnailsPath4" name="file4" style="display: none;" />
                        </div>
                        <p class="font-green">
                            Qee+报名者<br />
                        </p>
                        <span class="font-green iconfont icon-shuaxin" onclick="ChangeAngle4()"></span>

                    </div>


                </div>
            </div>
        </div>
        <div class="row">
            <div class="col text-center">
                <button class="btn-diy margin-top-1 font" id="btnSumbit">确认提交</button>
            </div>
        </div>
    </section>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        angleArr = [0, 90, 180, 270];
        var imgAngle1 = 0;
        var imgAngle2 = 0;
        var imgAngle3 = 0;
        var imgAngle4 = 0;
        $(function () {
            $("#btnSumbit").click(function () {
                Apply();
            });


            $("#txtThumbnailsPath1").on('change', function () {
                UploadImg1();
            });

            function UploadImg1() {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Jiepai&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'text',
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow1").attr("src", resp.ExStr);
                                $("#txtThumbnailsPath1").on('change', function () {
                                    UploadImg1();
                                });
                            } else {
                                layermsg(resp.Msg);
                            }
                        }

                    });

                } catch (e) {
                    layermsg(e);

                }

            }

            $("#txtThumbnailsPath2").on('change', function () {
                UploadImg2();
            });
            
            function UploadImg2()
            {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Jiepai&filegroup=file2',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath2',
                        dataType: 'text',
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow2").attr("src", resp.ExStr);
                                $("#txtThumbnailsPath2").on('change', function () {
                                    UploadImg2();
                                });
                            } else {
                                layermsg(resp.Msg);
                            }
                        }

                    });

                } catch (e) {
                    layermsg(e);
                }
            }

            $("#txtThumbnailsPath3").on('change', function () {
               
                UploadImg3();
            });

            function UploadImg3()
            {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Jiepai&filegroup=file3',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath3',
                        dataType: 'text',
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow3").attr("src", resp.ExStr);
                                $("#txtThumbnailsPath3").on('change', function () {
                                    UploadImg3();
                                });
                            } else {
                                layermsg(resp.Msg);
                            }
                        }

                    });

                } catch (e) {
                    layermsg(e);
                }
            }

            $("#txtThumbnailsPath4").on('change', function () {
                UploadImg4();
            });
            function UploadImg4()
            {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Jiepai&filegroup=file4',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath4',
                        dataType: 'text',
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgshow4").attr("src", resp.ExStr);
                                $("#txtThumbnailsPath4").on('change', function () {
                                    UploadImg4();
                                });
                            } else {
                                layermsg(resp.Msg);
                            }
                        }

                    });

                } catch (e) {
                    layermsg(e);
                }
            }

            $("#font-green1").text($("#enlist-select").val());
            $("#font-green2").text($("#enlist-select").val());
            $("#font-green3").text($("#enlist-select").val());

            $("#enlist-select").change(function () {
                if ($(this).val() =="请选择拍摄主题") {
                    $("#font-green1").text("Qee+地标建筑/其他");
                    $("#font-green2").text("Qee+地标建筑/其他");
                    $("#font-green3").text("Qee+地标建筑/其他");
                    return;
                }
                $("#font-green1").text($(this).val());
                $("#font-green2").text($(this).val());
                $("#font-green3").text($(this).val());
            });
        })
        //旋转1
        var ai1 = 0;
        function ChangeAngle1() {
            if ($("#imgshow1").attr("src") == "images/enlist/uploadpic.png") {
                return;
            }
            ai1++;
            if (ai1 > 3)
                ai1 = 0;
            imgAngle1 = angleArr[ai1];
            $('#imgshow1').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow1').addClass('transform' + imgAngle1);
        }
        //旋转2
        var ai2 = 0;
        function ChangeAngle2() {
            if ($("#imgshow2").attr("src") == "images/enlist/uploadpic.png") {
                return;
            }
            ai2++;
            if (ai2 > 3)
                ai2 = 0;
            imgAngle2 = angleArr[ai2];
            $('#imgshow2').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow2').addClass('transform' + imgAngle2);
        }
        //旋转3
        var ai3 = 0;
        function ChangeAngle3() {
            if ($("#imgshow3").attr("src") == "images/enlist/uploadpic.png") {
                return;
            }
            ai3++;
            if (ai3 > 3)
                ai3 = 0;
            imgAngle3 = angleArr[ai3];
            $('#imgshow3').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow3').addClass('transform' + imgAngle3);
        }

        //旋转4
        var ai4 = 0;
        function ChangeAngle4() {
            if ($("#imgshow4").attr("src") == "images/enlist/uploadpic.png") {
                return;
            }
            ai4++;
            if (ai4 > 3)
                ai4 = 0;
            imgAngle4 = angleArr[ai4];
            $('#imgshow4').removeClass('transform0 transform90 transform180 transform270');
            $('#imgshow4').addClass('transform' + imgAngle4);
        }

        function Apply() {
            var model = {
                Action: "AddVoteObjectInfo",
                Ex1: $("#for-list-2").val(),
                Ex2:$("#enlist-select").val()=="请选择拍摄主题"?"Qee+地标建筑/其他":$("#enlist-select").val(),
                Address: $.trim($("#for-list-1").val()),
                Introduction: $.trim($("#for-list-3").val()),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                ShowImage3: $("#imgshow3").attr("src"),
                ShowImage4: $("#imgshow4").attr("src"),
                imgAngle1: imgAngle1,
                imgAngle2: imgAngle2,
                imgAngle3: imgAngle3,
                imgAngle4: imgAngle4
            }
            if (model.Address == "") {
                layermsg("请输入所在地");
                return false;
            }

            if (model.Ex1 == "") {
                layermsg("请输入微信号");
                return false;
            }

            if (model.Introduction == "") {
                layermsg("请输入粉丝祝福接力");
                return false;
            }
            if ($("#enlist-select").val() == "请选择拍摄主题") {
                layermsg('请选择拍摄主题');
                return;
            }
            if (model.ShowImage1 == "images/enlist/uploadpic.png") {
                layermsg("请上传第一张照片");
                return false;
            }
            if (model.ShowImage2 == "images/enlist/uploadpic.png") {
                layermsg("请上传第二张照片");
                return false;
            }
            if (model.ShowImage3 == "images/enlist/uploadpic.png") {
                layermsg("请上传第三张照片");
                return false;
            }
            if (model.ShowImage4 == "images/enlist/uploadpic.png") {
                layermsg("请上传第四张照片");
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
                        layermsgCon(resp.errmsg);
                        $("#ok").on("click", function () {
                            window.location.href = "Detail.aspx?id="+resp.Ex1;
                        });
                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })
        }

    </script>
    <script>
        //分享
        var shareTitle = "Qee全球街拍探秘项目平台";
        var shareDesc = "参加Qee全球街拍探秘，有大奖在等您！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Jiepai/Images/logo.jpg";
        var shareLink = "http://<%=Request.Url.Host %>/customize/Jiepai/index.aspx";
        //分享
    </script>
</asp:Content>
