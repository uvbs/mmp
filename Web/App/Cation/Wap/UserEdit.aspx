<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true"
    CodeBehind="UserEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.UserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .head
        {
            width: 52px;
            height: 52px;
            border-radius: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <% 
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo = bllUser.GetCurrentUserInfo();
        
    %>
    <section class="box">
    <div class="header">
    <img  src="<%=bllUser.GetUserDispalyAvatar(currentUserInfo)%>" class="head"/>
      <h2> <% =bllUser.GetUserDispalyName(currentUserInfo)%></h2>
        <p>积分:<%=currentUserInfo.TotalScore.ToString()%></p>
       <%-- <a href="#" id="btnReload" class="btn">更新头像</a>--%>
        <div class="line"></div>
    </div>
    <div class="prompt">
        <p>完善个人资料后可享受更多服务！</p>
    </div>
    <div  class="formbox">
        <label for="txtName"><span style="color:red;">*</span>真实姓名:</label>
        <input type="text" class="short" id="txtName" value="<%=currentUserInfo.TrueName%>"> 
        <label for="txtPhone"><span style="color:red;">*</span>手机号码:</label>
        <input type="text" class="wide" id="txtPhone" value="<%=currentUserInfo.Phone%>">
        <label for="txtEmail">电子邮箱:</label>
        <input type="text" class="wide" id="txtEmail" value="<%=currentUserInfo.Email%>">
        <label for="txtCompany">公司名称:</label>
        <input type="text" class="wide" id="txtCompany" value="<%=currentUserInfo.Company%>">
        <label for="txtPosition">职位:</label>
        <input type="text" class="wide" id="txtPosition" value="<%=currentUserInfo.Postion%>">
        <%
            if (webSite.IsNeedDistributionRecommendCode==1)
            {
                %>
                    <label for="txtDistributionOwner">推荐码:</label>
                    <input type="text" class="wide" id="txtRecommendCode" value="">
                <%
            }
            else
            {%>

                <input type="hidden" class="wide" id="txtRecommendCode" value="">
            <% } %>    
        

        <button id="btnSave">保存</button>
    </div>
</section>
    <section class="navbar">
    <a href="javascript:window.history.go(-1)" class="backbtn">
        <span class="icon"></span>
    </a>

</section>
    <script type="text/javascript">
        //        var openID = '<%= ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel().WXOpenId %>';
        //var currMember;
        //        var currArticleSourceType = 'AddFriend';
        $(function () {

            //            $('#divArticleSourceWXHao').show();
            //            $('#divArticleSourceWebSite').hide();

            //         var modeData = {
            //             '<%=ZentCloud.JubitIMP.Web.SessionKey.systemset.WXCurrOpenerOpenIDKey %>': openID,
            //             Action: 'GetUserInfoByOpenId'
            //         }
            //         $.ajax({
            //             type: 'post',
            //             url: '/Handler/User/UserHandler.ashx',
            //             data: modeData,
            //             success: function (result) {
            //                 var resp = $.parseJSON(result);
            //                 if (resp.Status == 1) {
            //                     currMember = resp.ExObj;
            //                     $('#txtName').val(currMember.TrueName);
            //                     $('#txtPhone').val(currMember.Phone);
            //                     $('#txtEmail').val(currMember.Email);
            //                     $('#txtCompany').val(currMember.Company);
            //                     $('#txtPosition').val(currMember.Postion);
            //                     //                        $('#txtArticleSourceWebSite').val(currMember.ArticleSourceWebSite);
            //                     //                        $('#txtArticleSourceWXHao').val(currMember.ArticleSourceWXHao);
            //                     //                        $('#txtArticleSourceName').val(currMember.ArticleSourceName);

            //                     //                        if (currMember.ArticleSourceType == 'AddFriend') {
            //                     //                            $('#divArticleSourceWebSite').hide();
            //                     //                            $('#divArticleSourceWXHao').show();

            //                     //                            $('#rdoSourceAddFriend').prop("checked", true).checkboxradio("refresh");
            //                     //                            $('#rdoSourceWebSite').prop("checked", false).checkboxradio("refresh");
            //                     //                        }
            //                     //                        else if (currMember.ArticleSourceType == 'WebSite') {
            //                     //                            $('#divArticleSourceWebSite').show();
            //                     //                            $('#divArticleSourceWXHao').hide();
            //                     //                            $('#rdoSourceWebSite').prop("checked", true).checkboxradio("refresh");
            //                     //                            $('#rdoSourceAddFriend').prop("checked", false).checkboxradio("refresh");
            //                     //                        }

            //                     //                        if ($('#txtArticleSourceWebSite').val() == '')
            //                     //                            $('#txtArticleSourceWebSite').val('http://');

            //                 }
            //             }
            //         });





            $('#btnSave').live('click', function () {
                Save();
            });


            //            $('#rdoSourceAddFriend').click(function () {
            //                //alert(1);
            //                currArticleSourceType = 'AddFriend';
            //                $('#divArticleSourceWXHao').show();
            //                $('#divArticleSourceWebSite').hide();

            //            });
            //            $('#rdoSourceWebSite').click(function () {
            //                //alert(2);
            //                currArticleSourceType = 'WebSite';
            //                $('#divArticleSourceWXHao').hide();
            //                $('#divArticleSourceWebSite').show();

            //            });

            $(document).delegate('#btnReload', 'click', function () {
                //$.mobile.loading('show', { textVisible: true, text: '正在处理...' });
                $.ajax({
                    type: 'post',
                    url: '/Handler/OpenGuestHandler.ashx',
                    data: { Action: 'UpdateToLogoutSessionIsRedoOath' },
                    success: function (result) {
                        window.location.href = window.location.href;
                    }
                });
            })

        });

        function Save() {

            try {
                var modeData = {
                    Action: 'EditUserInfoByOpenId',
                    // '<%=ZentCloud.JubitIMP.Web.SessionKey.systemset.WXCurrOpenerOpenIDKey %>': openID,
                    Name: $.trim($('#txtName').val()),
                    Phone: $.trim($('#txtPhone').val()),
                    Email: $.trim($('#txtEmail').val()),
                    Company: $.trim($('#txtCompany').val()),
                    Position: $.trim($('#txtPosition').val()),
                    RecommendCode: $.trim($('#txtRecommendCode').val())
                    //                ArticleSourceType: currArticleSourceType,
                    //                ArticleSourceWXHao: $.trim($('#txtArticleSourceWXHao').val()),
                    //                ArticleSourceWebSite: $.trim($('#txtArticleSourceWebSite').val()),
                    //                ArticleSourceName: $.trim($('#txtArticleSourceName').val()),
                    //OpenId: openID,
                   
                }
//                if (modeData.DistributionOwner == '') {
//                    alert("请输入推荐码");
//                    return;
//                }

                $.ajax({
                    type: 'post',
                    url: '/Handler/User/UserHandler.ashx',
                    data: modeData,
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.Status == 1) {
                            MessageBox.show("个人资料修改成功", 1, 3000);
                            setTimeout(function () {
                                window.history.go(-1);
                            }, 2000);
                            
                        }
                        else {
                            MessageBox.show(resp.Msg, 2, 3000);
                        }

                    }
                });

            } catch (e) {
                alert(e);
            }
        }

    </script>
</asp:Content>
