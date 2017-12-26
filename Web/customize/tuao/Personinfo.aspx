<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="Personinfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.Personinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    个人资料</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmall20150110.css" rel="stylesheet" type="text/css" />
    <style>
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="m_personinfo">
        <label for="txtName"><span class="title">姓名:</span><input id="txtName" type="text" value="<%=currentUserInfo.TrueName%>" ></label>
        <label for="ddlgender">
            <span class="title">性别:</span>            
            <select name="" id="ddlgender">
                <option value="">选择性别</option>
                <option value="1">男</option>
                <option value="0">女</option>
            </select></label>
        <label for="txtPhone"><span class="title">联系方式:</span><input  type="tel" id="txtPhone" value="<%=currentUserInfo.Phone%>" ></label>
        <label for="txtEmail"><span class="title">邮箱:</span><input type="email" id="txtEmail" value="<%=currentUserInfo.Email%>"></label>
       
        <a href="MyCenter.aspx" class="btn orange">返回</a><a id="btnSave" href="javascript:(0)" class="btn orange">保存</a>
    </div>


</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            var gender = "<%=currentUserInfo.Gender%>";
            if (gender == 1) {
                $("#ddlgender").val("1");
            } else {
                $("#ddlgender").val("0");
            }

            $("#btnSave").click(function () {

                try {

                    var model = {
                        Action: 'EditUserInfoV1',
                        Name: $.trim($('#txtName').val()),
                        Phone: $.trim($('#txtPhone').val()),
                        Email: $.trim($('#txtEmail').val()),
                        Gender: $.trim($('#ddlgender').val()),
                        AddressArea: ""

                    }


                    if (model.Name == "") {
                        $('#txtName').focus();
                        return false;
                    }
                    if (model.Phone == "") {
                        $('#txtPhone').focus();
                        return false;
                    }
                    if (model.Email == "") {
                        $('#txtEmail').focus();
                        return false;
                    }
                    if (model.Gender == "") {
                        //alert("请选择性别");
                        msgText.init("请选择性别", 3000);
                        return false;
                    }
                    $.ajax({
                        type: 'post',
                        url: '/Handler/User/UserHandler.ashx',
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                alert("保存成功");
                                window.location = "MyCenter.aspx";

                            }
                            else {

                                alert(resp.Msg);
                            }


                        }

                    });


                } catch (e) {
                    //alert(e);
                    alert(e);
                }

            });


        });


    </script>
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
