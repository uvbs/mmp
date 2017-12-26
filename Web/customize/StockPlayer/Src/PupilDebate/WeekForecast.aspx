<%@ Page Title="" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="WeekForecast.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.PupilDebate.WeekForecast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/PupilDebate/WeekForecast.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="warp-week">
        <div class="weekhead row">
            <div class="col-xs-6 head-title">多空论战>><span></span></div>
            <div class="col-xs-6 head-button">
                <button type="button" class="btn btn-default btnHistory">我的支持历史</button>
            </div>
        </div>
        <div class="weekbody row">
            <div class="col-xs-4 weekbody-left">
                <p class="duoText">多方阵营</p>
                <p class="duoTotal"></p>
            </div>
            <div class="col-xs-4 weekbody-vs">
                <span class="weekbody-v">V</span>
                <span class="weekbody-s">S</span>
            </div>
            <div class="col-xs-4 weekbody-right">
               <p class="kongText">空方阵营</p>
                <p class="kongTotal"></p>
            </div>
        </div>
        <div class="weekButton row">
            <div class="col-xs-6">
                <button class="btn btn-default weekButton-left">我站这边</button>
            </div>
            <div class="col-xs-6">
                <button class="btn btn-default weekButton-right">我站这边</button>
            </div>
        </div>

        <div class="weekGuandian row">
            <div class="col-xs-6 dfGuandian">
               <div class="col-xs-12 dfTitle">正方观点</div>
               <div class="col-xs-12 dfContent">
                    <script id="just" class="textarea" type="text/plain">
                    </script>
               </div>
                <div class="col-xs-12 dfButton">
                    <button data-type="just" class="btn btn-primary submit">提交</button>
                </div>
            </div>


             <div class="col-xs-6 kfGuandian">
               <div class="col-xs-12 dfTitle">反方观点</div>
               <div class="col-xs-12 dfContent">
                   <script id="back" class="textarea" type="text/plain">
                    </script>
               </div>
                <div class="col-xs-12 dfButton">
                    <button data-type="back" class="btn btn-primary submit">提交</button>
                </div>
            </div>


        </div>




        <div class="weekContent row">
            <div class="col-xs-6 duofang">
                <div class="duofang-item">
                    <div class="col-xs-12 bs_just_item" style="display:none;">
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
                <div class="duofang-more" style="display:none;" data-key="duofang">
                    <button type="button" class="btn btn-default buttonGray" data-key="sp">查看更多</button>
                </div>
            </div>
            <div class="col-xs-6 kongfang">
                <div class="kongfang-item">
                    <div class="col-xs-12 bs_back_item" style="display:none;">
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
                <div class="mTop20 kongfang-more" style="display:none;" data-key="kongfang">
                    <button type="button" class="btn btn-default buttonGray" data-key="sp">查看更多</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript">
        var rootId = '<%=rootId%>';
    </script>
    <script type="text/javascript" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/lib/ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/PupilDebate/WeekForecast.js"></script>
</asp:Content>
