<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Vote/Comm/Master.Master" AutoEventWireup="true" CodeBehind="MySignUpDali.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm.MySignUpDali" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=currVote.VoteName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body
        {
            background-color: White;
        }
        table
        {
            width: 95%;
        }
        .form_input
        {
            width: 100%;
        }
        .form_span
        {
            height: auto;
            padding: 7px 5px 9px 5px;
        }
        .votedetail
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 2px 5px 2px 5px;
        }
                .form_span {
            height:30px;
        }
                #txtIntroduction {
            width:100%;
            height:100px;
        }
                        td {
            vertical-align:middle;
        }
        .form_span {
            padding: 5px 5px 2px 5px;

        }
    </style>
     <%=styleCustomize.ToString()%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

     <%if (!string.IsNullOrWhiteSpace(currVote.BgMusic))
        { %>
     <audio id="audioBg" src="<%=currVote.BgMusic %>" ></audio>    <div id="musicbutton" class="musicplay" style="left:0%;" onclick="changeMusicCtrl()"></div>
    <%} %>

    <%--<div class="image_single">
        <img src="images/signup_04.png" alt="" title="" border="0" />
    </div>--%>

    <div class="wrapSignUp" style="background-image:url(<%=currVote.BannerBg%>);" >
        <div class="list2 mainInputBox" style="margin-top:<%=currVote.BannerHeight%>;">
            <div class="form ">
                <% if (model.Status.Equals(1))
                   {%>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">参赛编号:&nbsp;
                                    <%=model.Number%>号 
                                    <%if (currVote.VoteStatus == 1)
                                        { %>
                                    &nbsp;&nbsp;<a class="votedetail" href="Detail.aspx?vid=<%=currVote.AutoID %>&id=<%=model.AutoID %>">进入我的投票页</a>
                                    <%} %>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <%} %>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">审核状态:
                                    <%switch (model.Status)
                                      {
                                          case 0:
                                              Response.Write("等待审核");
                                              break;
                                          case 1:
                                              Response.Write(string.Format("<font color='grean'>审核已通过 {0}</font>", model.Remark));
                                              break;
                                          case 2:
                                              Response.Write(string.Format("<font color='red'>审核未通过 原因:{0}</font>", model.Remark));
                                              break;
                                          default:
                                              break;
                                      }
                                    %>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <% if (model.Status.Equals(1))
                   {%>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">排名:&nbsp; 第<%=model.Rank %>名</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">我的票数:&nbsp;
                                    <%=model.VoteCount %>票</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <%} %>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">设计主题</span>
                            </td>
                            <td>
                                <input type="text" id="txtVoteObjectName" name="user" value="<%=model.VoteObjectName %>"
                                    placeholder="设计主题" class="form_input" maxlength="20" />
                            </td>
                        </tr>
                    </table>
                </div>
 <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">设计灵感 </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <textarea id="txtIntroduction" maxlength="100" row="5"  placeholder="设计灵感(100个字以内)"><%=model.Introduction %></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
                     <div class="form_div radius4">
                    <table>
                        <tr>
                            <td >
                                <span class="form_span" style="margin-top:5px;">学校</span>
                            </td>
                            <td>
                              
                                   <select id="ddlSchoolName">
                                       <option value="">请选择</option>
                                        <option value="江苏工程职业技术学院">江苏工程职业技术学院</option>
                                        <option value="无锡工艺职业技术学院">无锡工艺职业技术学院</option>
                                        <option value="盐城工业职业技术学院">盐城工业职业技术学院</option>
                                        <option value="成都纺织高等专科学校">成都纺织高等专科学校</option>
                                        <option value="山东轻工职业学院">山东轻工职业学院</option>
                                        <option value="浙江纺织服装职业技术学院">浙江纺织服装职业技术学院</option>
                                        <option value="苏州工艺美术职业技术学院">苏州工艺美术职业技术学院</option>
                                        <option value="杭州职业技术学院">杭州职业技术学院</option>

                                    </select>

                            </td>
                        </tr>
                    </table>
                </div>
                     <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">学生名</span>
                            </td>
                            <td>
                                <input type="text" id="txtContact" name="user" placeholder="学生名字" value="<%=model.Contact%>"
                                    class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>
                  <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">指导老师</span>
                            </td>
                            <td>
                                <input type="text" id="txtEx2" name="user"  value="<%=model.Ex2%>" placeholder="指导老师"
                                    class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>
                     <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">联系电话</span>
                            </td>
                            <td>
                                 <input type="text" name="moblie" id="txtPhone"  value="<%=model.Phone%>" placeholder="联系电话" class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>
                                     <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">Email</span>
                            </td>
                            <td>
                                <input type="text" id="txtEx3" name="user"  value="<%=model.Ex3%>" placeholder="Email"
                                    class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>


                  <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="signup_msg">
                                    参赛效果图 (点击图片重新上传，尺寸建议 400px*500px)

                                    
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">

                                <div class="menu6">
                                    <ul>
                                        <li><a href="javascript:void(0)" onclick="txtVoteObjectHeadImage.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.VoteObjectHeadImage) ? "images/signup_01.jpg" : model.VoteObjectHeadImage%>" alt="" title="" id="imgVoteObjectHeadImage" width="100"  />
                                            <input type="file" id="txtVoteObjectHeadImage" name="fileheadimg" style="display: none;" />
                                        </a>

                                        </li>


                                       
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>


                 <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="signup_msg">
                                   平面结构图 (点击图片重新上传，尺寸建议 400px*500px)

                                    
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">

                                <div class="menu6">
                                    <ul>
                                        <li><a href="javascript:void(0)" onclick="txtEx1.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.Ex1) ? "images/signup_01.jpg" : model.Ex1%>" alt="" title="" id="imgEx1" width="52" height="52" />
                                            <input type="file" id="txtEx1" name="fileex1" style="display: none;" />
                                        </a></li>


                                       
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
               

                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="signup_msg">
                                    实物图 (点击图片重新上传，尺寸建议 400px*500px)

                                    
                                    </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">

                                <div class="menu6">
                                    <ul>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath1.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.ShowImage1) ? "images/signup_01.jpg" : model.ShowImage1%>" alt="" title="" id="imgshow1" width="52" height="52" />
                                            <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                                        </a><span class="re_span" onclick="ChangeAngle1()">

                                            <%--<img src="images/signup_03.jpg" alt="" title="" />--%>
                                            <svg class="icon iconRank themeColor font26" aria-hidden="true">
                                                <use xlink:href="#icon-icon_rotate"></use>
                                            </svg>

                                            </span> </li>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath2.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.ShowImage2) ? "images/signup_01.jpg" : model.ShowImage2%>" alt="" title="" id="imgshow2" width="52" height="52" />
                                            <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                                        </a><span class="re_span" onclick="ChangeAngle2()">

                                            <%--<img src="images/signup_03.jpg" alt="" title="" />--%>

                                            <svg class="icon iconRank themeColor font26" aria-hidden="true">
                                                <use xlink:href="#icon-icon_rotate"></use>
                                            </svg>

                                            </span> </li>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath3.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.ShowImage3) ? "images/signup_01.jpg" : model.ShowImage3%>" alt="" title="" id="imgshow3" width="52" height="52" />
                                            <input type="file" id="txtThumbnailsPath3" name="file3" style="display: none;" />
                                        </a><span class="re_span" onclick="ChangeAngle3()">

                                            <%--<img src="images/signup_03.jpg" alt="" title="" />--%>

                                            <svg class="icon iconRank themeColor font26" aria-hidden="true">
                                                <use xlink:href="#icon-icon_rotate"></use>
                                            </svg>

                                            </span> </li>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath4.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.ShowImage4) ? "images/signup_01.jpg" : model.ShowImage4%>" alt="" title="" id="imgshow4" width="52" height="52" />
                                            <input type="file" id="txtThumbnailsPath4" name="file4" style="display: none;" />
                                        </a><span class="re_span" onclick="ChangeAngle4()">

                                            <%--<img src="images/signup_03.jpg" alt="" title="" />--%>
                                            <svg class="icon iconRank themeColor font26" aria-hidden="true">
                                                <use xlink:href="#icon-icon_rotate"></use>
                                            </svg>

                                            </span> </li>

                                        <li style="display:none;"><a href="javascript:void(0)" onclick="txtThumbnailsPath5.click()">
                                            <img src="<%=string.IsNullOrEmpty(model.ShowImage5) ? "images/signup_01.jpg" : model.ShowImage5%>" alt="" title="" id="imgshow5" width="52" height="52" />
                                            <input type="file" id="txtThumbnailsPath5" name="file5" style="display: none;" />
                                        </a><span class="re_span" onclick="ChangeAngle5()">
                                            <%--<img src="images/signup_03.jpg" alt="" title="" />--%>
                                            <svg class="icon iconRank themeColor font26" aria-hidden="true">
                                                <use xlink:href="#icon-icon_rotate"></use>
                                            </svg>
                                            </span> </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
               
                <div class="form_div3">
                    <a href="javascript:void(0)" id="btnSumbit">
                        <%--<img src="images/signup_05.jpg" />--%>
                        <span class="btnToVote font20  pTop4 pBottom4">确认提交</span>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <%if (!string.IsNullOrWhiteSpace(currVote.PartnerImg))
        { %>

        <img class="width100P" src="<%=currVote.PartnerImg %>" />

    <%} %>

    <%=footerHtml.ToString()%>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
    <script type="text/javascript">
        var status=<%=model.Status%>;
        angleArr = [0, 90, 180, 270];
        var imgAngle1 = 0;
        var imgAngle2 = 0;
        var imgAngle3 = 0;
        var imgAngle4 = 0;
        var imgAngle5 = 0;

        $(function () {

            $(ddlSchoolName).val("<%=model.SchoolName %>");

            $("#btnSumbit").click(function () {

                Apply();

            });

            $("#txtVoteObjectHeadImage").on('change', function () {
                try {
                    if (status==1) {
                        layermsg("您已通过审核,审核通过后不可修改");
                        return false;
                    }
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Dali&filegroup=fileheadimg',
                        secureuri: false,
                        fileElementId: 'txtVoteObjectHeadImage',
                        dataType: 'text',
                        success: function (result) {

                            //try {
                            //    result = result.substring(result.indexOf("{"), result.indexOf("</"));
                            //} catch (e) {
                            //    alert(e);
                            //}
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgVoteObjectHeadImage").attr("src", resp.ExStr);
                              

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }

            });


            $("#txtEx1").on('change', function () {
                try {
                    if (status==1) {
                        layermsg("您已通过审核,审核通过后不可修改");
                        return false;
                    }
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Dali&filegroup=fileex1',
                        secureuri: false,
                        fileElementId: 'txtEx1',
                        dataType: 'text',
                        success: function (result) {

                            //try {
                            //    result = result.substring(result.indexOf("{"), result.indexOf("</"));
                            //} catch (e) {
                            //    alert(e);
                            //}
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $("#imgEx1").attr("src", resp.ExStr);


                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }

            });


            $("#txtThumbnailsPath1").on('change', function () {
                if (status==1) {
                    layermsg("您已通过审核,审核通过后不可修改");
                    return false;
                }
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=Dali&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'text',
                        success: function (result) {

                            //                            try {
                            //                                result = result.substring(result.indexOf("{"), result.indexOf("</"));
                            //                            } catch (e) {
                            //                                layermsg(e);
                            //                            }
                            var resp = $.parseJSON(result);
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
                if (status==1) {
                    layermsg("您已通过审核,审核通过后不可修改");
                    return false;
                }
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=Dali&filegroup=file2',
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
                if (status==1) {
                    layermsg("您已通过审核,审核通过后不可修改");
                    return false;
                }
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=Dali&filegroup=file3',
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
                if (status==1) {
                    layermsg("您已通过审核,审核通过后不可修改");
                    return false;
                }
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=Dali&filegroup=file4',
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
                if (status==1) {
                    layermsg("您已通过审核,审核通过后不可修改");
                    return false;
                }
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=Dali&filegroup=file5',
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

            if (status==1) {
                //$(".menu6 ul li a").removeAttr("onclick");
                //$(".menu6 ul li span").removeAttr("onclick");
                $(".form input").attr("readonly","readonly");
                $(".form select").attr("readonly","readonly");
                $("#btnSumbit").remove();
                
            }

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
                Action: "EditVoteObjectInfo",
                VoteObjectName: $.trim($(txtVoteObjectName).val()),
                SchoolName: $(ddlSchoolName).val(),
                Introduction: $.trim($(txtIntroduction).val()),
                Conatact: $(txtContact).val(),
                Ex2: $(txtEx2).val(),
                Phone: $.trim($(txtPhone).val()),
                Ex3: $(txtEx3).val(),
                VoteObjectHeadImage:$(imgVoteObjectHeadImage).attr("src"),
                Ex1: $(imgEx1).attr("src"),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                ShowImage3: $("#imgshow3").attr("src"),
                ShowImage4: $("#imgshow4").attr("src"),
                imgAngle1: imgAngle1,
                imgAngle2: imgAngle2,
                imgAngle3: imgAngle3,
                imgAngle4: imgAngle4,
                vid:'<%=currVote.AutoID%>'
               
            }
            if (status==1) {
                layermsg("您已通过审核,审核通过后不可修改");
                return false;
            }
            if (model.VoteObjectName == "") {
                
                alert("请输入主题");
                return false;
            }
            if (model.Introduction == "") {

                alert("请输入灵感");
                return false;
            }
            if (model.SchoolName == "") {
                alert("请选择学校");
                return false;
            }
            if (model.Conatact == "") {

                alert("请输入学生名");
                return false;
            }
            if (model.Ex2 == "") {

                alert("请输入指导老师名字");
                return false;
            }

            //if (model.Area == "") {
            //    alert("请输入你所在的城市");
            //    return false;
            //}
            if (model.Phone == "") {
                alert("请输入联系电话");
                return false;
            }

            var phonereg = /^(13|14|15|17|18)\d{9}$/;
            if (!phonereg.test(model.Phone)) {
                alert("请输入有效的手机号码");
                return false;
            }
            if (model.Ex3 == "") {

                alert("请输入Email");
                return false;
            }
            if (model.VoteObjectHeadImage == "images/signup_0111.jpg") {
                alert("请上传参赛效果图");
                return false;
            }
            if (model.Ex1 == "images/signup_0111.jpg") {
                alert("请上传平面结构图");
                return false;
            }
            if (model.ShowImage1 == "images/signup_0111.jpg") {
                alert("请上传实物图1");
                return false;
            }
            if (model.ShowImage2 == "images/signup_0111.jpg") {
                alert("请上传实物图2");
                return false;
            }
            if (model.ShowImage3 == "images/signup_0111.jpg") {
                alert("请上传实物图3");
                return false;
            }
            if (model.ShowImage4 == "images/signup_0111.jpg") {
                alert("请上传实物图4");
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
                        layermsg("资料修改成功！");
                        setTimeout("window.location = 'MySignUpDali.aspx?vid=<%=currVote.AutoID%>'", 2000);

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
        var shareTitle = "<%=currVote.ShareTitle%>";
        var shareDesc = "<%=currVote.Summary%>";
        var shareImgUrl = "<%=currVote.VoteImage.StartsWith("http")? currVote.VoteImage:"http://" + Request.Url.Host + currVote.VoteImage%>";  //"http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
        var shareLink = window.location.href;
        //分享

    </script>
</asp:Content>
