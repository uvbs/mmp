<%@ Page Title="" Language="C#" MasterPageFile="~/customize/SuperTeam/Master.Master"
    AutoEventWireup="true" CodeBehind="Area.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SuperTeam.Area" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    区域选择
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/regionList.css" rel="stylesheet" type="text/css" />
    <style>
        .row-erweima .col-erweima
        {
            padding: 10px 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapRegionList">
        <div class="top">
            <img class="imgWidth" src="image/top.png">
        </div>
        <div class="content">
            <div class="row">
                <div class="col textC colorFFF font17">
                    区域列表
                </div>
            </div>
            <div class="row row-choose">
                <div class="col">
                    请选择查看的区域!
                </div>
            </div>
            <div class="row row-first">
                <div class="col">
                    <div class="row row-second">
                        <div class="col col-one mRight20" data-areacode="1">
                            黄浦、卢湾区
                        </div>
                        <div class="col col-one mLeft20" data-areacode="2">
                            静安、闸北区
                        </div>
                    </div>
                    <div class="row row-second">
                        <div class="col col-one mRight20" data-areacode="3">
                            徐汇区
                        </div>
                        <div class="col col-one mLeft20" data-areacode="4">
                            浦东新区(南)
                        </div>
                    </div>
                    <div class="row">
                        <div class="col col-one mRight20" data-areacode="5">
                            浦东新区(北)
                        </div>
                        <div class="col col-one mLeft20" data-areacode="6">
                            虹口、杨浦区
                        </div>
                    </div>
                    <div class="row">
                        <div class="col col-one mRight20" data-areacode="7">
                            宝山区
                        </div>
                        <div class="col col-one mLeft20" data-areacode="8">
                            普陀、嘉定区
                        </div>
                    </div>
                    <div class="row">
                        <div class="col col-one mRight20" data-areacode="9">
                            长宁、青浦区
                        </div>
                        <div class="col col-one mLeft20" data-areacode="10">
                            闵行区
                        </div>
                    </div>
                    <div class="row">
                        <div class="col col-one mRight20" data-areacode="11">
                            松江、金（山）奉（贤）区
                        </div>
                    </div>
                </div>
            </div>
            <div class="row row-erweima">
                <div class="col col-erweima">
                    <img class="imgWidth" src="image/qrcode.jpg"/>
                </div>
            </div>
            <div class="row row-logo">
                <div class="col col-logo">
                    <img class="imgWidth" src="image/xinwen.png">
                </div>
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
    <script>

        $(function () {

            $("[data-areacode]").click(function () {

                window.location.href = "List.aspx?areacode=" + $(this).data("areacode");

            })

        })

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
