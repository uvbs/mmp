<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Trave/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Trave.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我要报名
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<style>
.form_error {
  width: auto;
}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="pages_container">
        <div class="image_single">
            <img src="images/signup_04.png" alt="" title="" border="0" />
        </div>
        <div class="list2">
            <div class="form ">
                <form method="post" action="">
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">宝贝姓名</span>
                            </td>
                            <td>
                                <input type="text" name="user" id="txtVoteObjectName" value="" placeholder="输入宝贝姓名" class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">宝贝年龄</span>
                            </td>
                            <td>
                                <input type="text" name="city" id="txtAge" value="" placeholder="输入宝贝年龄(岁)" class="form_input" onkeyup="this.value=this.value.replace(/\D/g,'')"  onafterpaste="this.value=this.value.replace(/\D/g,'')"  maxlength="2"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_error">
                    *必须填写正确号码信息，以便主办方联系
                </div>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">家长手机</span>
                            </td>
                            <td>
                                <input type="text" name="moblie" id="txtPhone" value="" placeholder="输入家长手机" class="form_input" onkeyup="this.value=this.value.replace(/\D/g,'')" maxlength="13"/>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">宝贝去过的旅行目的地</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" name="moblie" id="txtAddress" value="" placeholder="输入去过的目的地" class="form_input" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_div radius4">
                    <table>
                        <tr>
                            <td valign="top">
                                <span class="form_span">小小旅行家宣言（限30字内）</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="text" name="moblie" id="txtIntroduction" value="" placeholder="输入小小旅行家宣言" class="form_input" />
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
                                    请报名者<span>点击加号</span>上传<span>宝贝图片2张</span>，其中<span>旅行图片+亲子合照（共2张）</span>，请注意照片附件大小在1MB以内。</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div class="menu6">
                                    <ul>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath1.click()">
                                            <img src="images/signup_01.png" id="imgshow1" max-width="74" max-height="74" /></a><span class="re_span"><img
                                                src="images/signup_03.png" alt="" onclick="ChangeAngle1()" /></span>
                                                 <input type="file" id="txtThumbnailsPath1" name="file1" style="display: none;" />
                                                
                                                </li>
                                        <li><a href="javascript:void(0)" onclick="txtThumbnailsPath2.click()">
                                            <img src="images/signup_01.png" id="imgshow2" max-width="74" max-height="74"/></a><span class="re_span"><img
                                                src="images/signup_03.png" alt="" onclick="ChangeAngle2()" /></span>
                                                 <input type="file" id="txtThumbnailsPath2" name="file2" style="display: none;" />
                                                </li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_div3">
                    <a href="javascript:void(0)">
                        <img src="images/signup_05.png" id="btnSumbit" /></a>
                </div>
                </form>
            </div>
        </div>
        <div class="list3">
            <div class="page_padding10">
                <div class="image_single">
                    <img src="images/code.png" alt="" title="" border="0" />
                    <img src="images/code_msg.png" alt="" title="" border="0" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        angleArr = [0, 90, 180, 270];
        var imgAngle1 = 0;
        var imgAngle2 = 0;


        $(function () {
            $("#btnSumbit").click(function () {

                Apply();

            });
            $("#txtThumbnailsPath1").on('change', function () {
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Trave&filegroup=file1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'text',
                        success: function (result) {
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
                try {
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Trave&filegroup=file2',
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

        function Apply() {
            var model = {
                Action: "AddVoteObjectInfo",
                VoteObjectName: $.trim($(txtVoteObjectName).val()),
                Age: $.trim($(txtAge).val()),
                Address: $.trim($(txtAddress).val()),
                Phone: $.trim($(txtPhone).val()),
                Introduction: $.trim($(txtIntroduction).val()),
                ShowImage1: $("#imgshow1").attr("src"),
                ShowImage2: $("#imgshow2").attr("src"),
                imgAngle1: imgAngle1,
                imgAngle2: imgAngle2

            }
            if (model.VoteObjectName == "") {
                layermsg("请输入宝贝姓名");
                return false;
            }
            if (model.Age == "") {
                layermsg("请输入宝贝年龄");
                return false;
            }
            if (model.Phone == "") {
                layermsg("请输入家长手机号");
                return false;
            }
            var phonereg = /^(13|14|15|17|18)\d{9}$/;
            if (!phonereg.test(model.Phone)) {
                layermsg("请输入有效的手机号码");
                return false;
            }
            if (model.Address == "") {
                layermsg("请输入去过的目的地");
                return false;
            }
            if (model.Introduction == "") {
                layermsg("请输入小小旅行家宣言");
                return false;
            }
            if (model.ShowImage1 == "images/signup_01.png") {
                layermsg("请上传第一张照片");
                return false;
            }
            if (model.ShowImage1 == "images/signup_01.png" && model.ShowImage2 == "images/signup_01.png") {
                layermsg("请至少上传一张照片");
                return false;
            }
            if (model.ShowImage1 == "images/signup_01.png") {
                model.ShowImage1 = "/";
            }
            if (model.ShowImage2 == "images/signup_01.png") {
                model.ShowImage2 = "";
            }

            $.ajax({
                type: "post",
                url:handlerPath,
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("报名成功");
                        setTimeout("window.location = 'Detail.aspx?id="+resp.errmsg+"'", 2000);

                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })


        }




    </script>
   <script type="text/javascript">
       //分享
       var shareTitle = "中青旅遨游网，寻找小小旅行家，境外亲子游大奖等你来赢!";
       var shareDesc = "晒宝贝旅行靓照，分享旅途趣闻，赢取境外亲子游等丰厚大奖！";
       var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Trave/images/logo.png";
       var shareLink = window.location.href;
       //分享
</script>
</asp:Content>
