<%@ Page Title="" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Cognizances.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Cognizance.Cognizances" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/Cognizance/Cognizances.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="row head">
        <div class="col-xs-6 head-left">
            <div class="key1" id="XINFABU">全部</div>
            <div class="key2">&nbsp;</div>
            <div class="key3" id="REMEN">我的</div>
            <div class="key2">&nbsp;</div>
        </div>
        <div class="col-xs-6 warp-button">
            <button type="button" class="btn btn-primary">发布</button>
        </div>
    </div>

    <div class="warp-cognizance">
        <%--<div class="cognizance-content row">
            <div class="warp-content-item" style="display: none;">
                <div class="col-xs-12 title">
                    博客网站排行榜博客网站排行榜博客网站排行榜博客网站排行榜
                </div>
                <div class="col-xs-12 user">
                    <div class="userimg" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0">
                        <img class="img-circle" src="http://open-files.comeoncloud.net/www/stockplayer/Phonebc5bfdcf-f936-432a-8cfc-55cfcd0b55a2/image/20161024/1C72337B8D0242EA96EF4A9460CC252E.jpg" />
                    </div>
                    <div class="username" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0">昵称</div>
                    <div class="time">2016-09-18 18:35</div>
                    <div class="clearfix"></div>
                </div>
                <div class="col-xs-12 content">
                    假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云
                    假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云
                    假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云假如让贺利军给自己运营的本地生活社区天通苑生活圈按满分10分打起，贺利军告诉猎云
                </div>
                <div class="col-xs-12 border-desh">
                </div>
            </div>
        </div>--%>
        <div class="row index-comment">
            <div class="col-xs-6 index-comment-item" style="display:none;">
                <div class="head">
                    <div class="userimg" data-id="1917077" data-nickname="大亨理财" data-avatar="http://static-files.socialcrmyun.com/img/europejobsites.png" data-friend="0" data-times="28">
                        <img src="http://static-files.socialcrmyun.com/img/europejobsites.png">
                    </div>
                    <div class="userinfo">
                        <div class="username" data-id="1917077" data-nickname="大亨理财" data-avatar="http://static-files.socialcrmyun.com/img/europejobsites.png" data-friend="0" data-times="28">大亨理财</div>
                        <div class="time">10:39</div>
                    </div>
                    
                    <div class="clearfix"></div>
                </div>
                <div class="content"></div>
                <div class="buttom">
                    <div class="comment">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png"><span>0</span>
                    </div>
                    <div class="zan">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png"><span>0</span>
                    </div>
                </div>
                <div class="borderdesh"></div>
            </div>
        </div>
        <div class="row index-bottom">
            <div class="pagination pager"></div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="/customize/StockPlayer/Src/Cognizance/Cognizances.js?v=20161122"></script>
</asp:Content>
