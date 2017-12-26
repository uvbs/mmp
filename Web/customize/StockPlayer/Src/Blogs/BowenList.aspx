<%@ Page Title="谈股论金" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="BowenList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Blogs.BowenList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/Blogs/Bowen.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="mTop20  Width1000">
        <div class="row head">
            <div class="col-xs-6 head-left">
                <div class="key1" id="XINFABU">新发布</div>
                <div class="key2">&nbsp;</div>
                <div class="key3" id="REMEN">热门</div>
                <div class="key2">&nbsp;</div>
            </div>
            <div class="col-xs-6 head-right">
                <div>
                    <input type="text" class="form-control keyword" id="keyword" placeholder="请输入昵称、文章标题或相关词" />
                    <button type="button" id="Search" class="btn btn-default buttomPosition">搜索</button>
                </div>
            </div>
        </div>
        <div class="row index-blogs">
            <div class="mTop20 blogs-item" style="display:none;">
                <div class="col-xs-4 index-bowen-item">
                    <img src="" class="img-rounded bowen-img"></div>
                <div class="col-xs-8 index-bowen-content">
                    <div class="head">测试首页博文内容</div>
                    <div class="context">
                        <div class="user">
                            <div class="userimg" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0">
                                <img class="img-circle"/></div>
                            <div class="username" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0">昵称</div>
                            <div class="time">2016-09-18 18:35</div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="icon">
                            <div class="comment">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png" class="Pointer pButtom2" ><span class="mLeft3">0</span></div>
                            <div class="zan">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png" class="Pointer"><span>10</span></div>
                            <div class="score">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/score.png" class="Pointer"><span>0</span></div>
                        </div>
                        <div class="buttom"><a class="Pointer" onclick="ToDetail(426725)">测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页博文内容测试首页</a></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row index-bottom">
            <div class="pagination pager1"></div>
            <div class="pagination pager2"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="/customize/StockPlayer/Src/Blogs/Bowen.js"></script>
</asp:Content>
