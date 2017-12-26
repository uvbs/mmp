<%@ Page Title="首页" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Index.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div>

        <%--K线图--%>
        <div class="stockChart">
            <div class="warp-stock">
                <div class="stock-title">
                    <span class="title">上证指数</span>
                    <span class="sm-title">(000001.SH)</span>
                    <span class="time" id="nowTime"></span>
                </div>
                <div class="stock-price">
                    <span class="nowPrice" id="nowPrice">0</span>
                    <span class="nowDiff" id="nowDiff">0</span>
                </div>
                <div class="warpper-detail">
                    <div class="row stock-data">
                        <div class="col-xs-2 cFont1">最高</div>
                         <div class="col-xs-2 cFont1">最低</div>
                         <div class="col-xs-2 cFont1">今开</div>
                         <div class="col-xs-2 cFont1">昨收</div>
                         <div class="col-xs-2 cFont1">成交量</div>
                         <div class="col-xs-2 cFont1">成交额</div>
                         <div class="col-xs-2" id="maxPrice">0</div>
                         <div class="col-xs-2" id="minPrice">0</div>
                         <div class="col-xs-2" id="todayOpenPrice">0</div>
                         <div class="col-xs-2" id="yestodayClosePrice">0</div>
                         <div class="col-xs-2" id="tradeNum">0</div>
                         <div class="col-xs-2" id="tradeAmount">0</div>
                    </div>
                </div>
            </div>
            <div class="stock-chart-detail">
                <div class="row">
                    <div class="banner col-xs-9" id="main" style="height: 300px;">
                    </div>

                    <div class="col-xs-3">
                        <div class="panel panel-warning warp-notice">
                            <div class="panel-heading"><img src="/customize/StockPlayer/Img/laba.jpg"/>最新公告发布</div>
                            <div class="panel-body">
                            </div>
                        </div>
                        <%--    <div class="row stockdetail ">
                            <div class="col-xs-4">最高</div>
                            <div class="col-xs-8" id="maxPrice">0</div>
                            <div class="col-xs-4">最低</div>
                            <div class="col-xs-8" id="minPrice">0</div>
                            <div class="col-xs-4">今开</div>
                            <div class="col-xs-8" id="todayOpenPrice">0</div>
                            <div class="col-xs-4">昨收</div>
                            <div class="col-xs-8" id="yestodayClosePrice">0</div>
                            <div class="col-xs-4">成交量</div>
                            <div class="col-xs-8" id="tradeNum">0</div>
                            <div class="col-xs-4">成交额</div>
                            <div class="col-xs-8" id="tradeAmount">0</div>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>


        <div class="mTop10">
            <div>
                <fieldset class="fsBorderGray">
                    <legend class="BorderText">
                        <div class="Font25">时评荟集</div>
                        <div class="Font15">SHIPING HUIJI</div>
                    </legend>
                </fieldset>
            </div>
            <div class="mTop20">
                <div class="row tables">
                    <div class="col-xs-4 sp-comment td tdGray " id="zao" data-key="zaop">早评荟集</div>
                    <div class="col-xs-4 sp-comment td tdGray" id="wu" data-key="wup">午评荟集</div>
                    <div class="col-xs-4 sp-comment td tdGray" id="wan" data-key="wanp">晚评荟集</div>
                </div>
            </div>
            <div class="mTop30">
                <div class="row index-comment">
                    <div class="col-xs-6 index-comment-item" style="display: none;">
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
                        <div class="content"><a onclick="ToDetail(1382492)">路透周四援引了解预算草案的一位希腊政府官员称，希腊预估明年经济成长率2.7%，为七年严重经济衰退之后的首次反弹，因投资加速且旅游业迅猛增长。希腊经济自2010年债务危机爆发以来萎缩了四分之一，被迫采纳严厉撙节措施，使数以百万计的人失去工作。2017年预算案定于下周一提交给议会。债权方预计希腊2016年经济萎缩0.3%。</a></div>
                        <div class="buttom">
                            <div class="comment">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png"><span>0</span>
                            </div>
                            <div class="zan">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png"><span>0</span>
                            </div>
                            <div class="score">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/score.png"><span>0</span>
                            </div>
                        </div>
                        <div class="borderdesh"></div>
                    </div>
                </div>
                <div class="mTop20 sp-more">
                    <button type="button" class="btn btn-default buttonGray" data-key="sp">查看更多</button>
                </div>
            </div>
        </div>
        <div class="mTop10">
            <div>
                <fieldset class="fsBorderGray">
                    <legend class="BorderText">
                        <div class="Font25">大师时评</div>
                        <div class="Font15">DASHI SHIPING</div>
                    </legend>
                </fieldset>
            </div>
            <div class="mTop20">
                <div class="row tables">
                    <div class="col-xs-4 ds-comment td tdGray" id="ds_zao" data-key="ds_zaop">大师早评</div>
                    <div class="col-xs-4 ds-comment td tdGray" id="ds_wu" data-key="ds_wup">大师午评</div>
                    <div class="col-xs-4 ds-comment td tdGray" id="ds_wan" data-key="ds_wanp">大师晚评</div>
                </div>
            </div>
            <div class="mTop30">
                <div class="row index-comment">
                </div>
                <div class="mTop20 ds-more">
                    <button type="button" class="btn btn-default buttonGray" data-key="ds">查看更多</button>
                </div>
            </div>
        </div>
        <div class="mTop10">
            <div>
                <fieldset class="fsBorderGray">
                    <legend class="BorderText">
                        <div class="Font25">热门博文</div>
                        <div class="Font15">REMEN BOWEN</div>
                    </legend>
                </fieldset>
            </div>
            <div class="index-bowen">
                <div class="index-bowen-item row borderdesh" style="display: none;">
                    <div class="col-xs-4">
                        <img class="img-rounded bowen-img" />
                    </div>
                    <div class="col-xs-8">
                        <div class="head">
                            <div class="userimg userHeadImg" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0" data-times="0">
                                <img src="http://static-files.socialcrmyun.com/img/europejobsites.png" />
                            </div>
                            <div class="username" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0" data-times="0">昵称</div>
                            <div class="time">17:12</div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="title">楼市火热银行忐忑 监管层重提房地产信贷风</div>
                        <div class="icon">
                            <div class="comment">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png"><span>0</span>
                            </div>
                            <div class="zan">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png"><span>0</span>
                            </div>
                            <div class="score">
                                <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/score.png"><span>0</span>
                            </div>
                        </div>
                        <div class="context">
                            <a>个人住房按揭贷款过去两个月异于寻常的增长未打消银行继续大力投放的决心，但快速上涨的房价也让银行在加大投入的同时心生忐忑。在这样一个时点，银监会主席尚福林再次强调要加强对房地产信贷压力测试和风险测试。而上一次银监会官方提及房地产信贷压力测试还是在2014年。</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="mTop20 bowen-more">
                <button type="button" class="btn btn-default buttonGray" data-key="bowen">查看更多</button>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/echarts/echarts.js?v=2016102001"></script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/Index/Index.js?v=20161122"></script>
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/Index/IndexChart.js"></script>
</asp:Content>
