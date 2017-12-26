<%@ Page Title="会员中心" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="MemberCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.MemberCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030901" rel="stylesheet" />
    <style>
        .wrapImg{
            padding: 14px;
            padding-right: 4px;
        }
        .wrapImg a{
            font-size: 14px;
            margin-right: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent" style="min-height: 100%;">
            <div class="wrapHead">
                <div class="wrapImg">
                    <%--<img class="img-bg" src="/App/Wap/img/HeadLogo.png" />--%>

                    <a href="/app/wap/ApplyMember.aspx" style=" float:left">注册</a>
                    <a href="javascript:void(0)" style=" float:left" v-on:click="loginout()">登录</a>
                    <a href="javascript:void(0)" style=" float:right" v-on:click="goUpdatePassword()">修改密码</a>
                    <a href="javascript:void(0)" style=" float:right" v-on:click="goFeedback()">在线客服</a>
                    <div style=" clear:both;"></div>
                </div>
            </div>
            <div class="wrapMember">
                <img class="userAvatar" v-bind:src="login_user.avatar" />
                <span class="userName" v-text="login_user.name"></span>
                <span class="userPhone" v-text="'账号：'+login_user.phone"></span>
                <span class="userArea" v-text="'地区：'+(!login_user.area?'——':login_user.area)"></span>
                <span class="userType" v-text="'类型：'+login_user.levelname"></span>
            </div>
            <div class="warpNav">
                <div class="nav-table">
                    <div class="nav-row-4">
                        <div class="nav-cell" v-on:click="goSignIn()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_03.jpg" />
                            <div class="nav-text">签到</div>
                        </div>
                        <div class="nav-cell" v-on:click="goTgCode()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_05.jpg" />
                            <div class="nav-text">推广</div>
                        </div>
                        <div class="nav-cell" v-on:click="goOfflineRecharge()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_07.jpg" />
                            <div class="nav-text">充值</div>
                        </div>
                        <div class="nav-cell" v-on:click="goTransferAccounts()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_09.jpg" />
                            <div class="nav-text">转账</div>
                        </div>
                    </div>
                    <div class="nav-row-4">
                        <div class="nav-cell" v-on:click="goScore()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_15.jpg" />
                            <div class="nav-text">积分</div>
                        </div>
                        <div class="nav-cell">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_16.jpg" />
                            <div class="nav-text">佣金</div>
                        </div>
                        <div class="nav-cell" v-on:click="goNull()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_17.jpg" />
                            <div class="nav-text">期权股权</div>
                        </div>
                        <div class="nav-cell" v-on:click="goUpgradeMember()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_19.jpg" />
                            <div class="nav-text">升级</div>
                        </div>
                    </div>
                    <div class="nav-row-4">
                        <div class="nav-cell" v-on:click="goWithdraw()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_25.jpg" />
                            <div class="nav-text">提现</div>
                        </div>
                        <div class="nav-cell" v-on:click="goBankCardList()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_26.jpg" />
                            <div class="nav-text">银行卡</div>
                        </div>
                        <div class="nav-cell" v-on:click="goFinancialDetails()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_27.jpg" />
                            <div class="nav-text">财务</div>
                        </div>
                        <div class="nav-cell" v-on:click="goMyTeamPerformance()">
                            <img class="nav-img" src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页＋按钮_29.png" />
                            <div class="nav-text">团队收益</div>
                        </div>
                    </div>
                </div>
            </div>
             <div class="warpTitle">
                <div class="title-table">
                    <div class="title-text">我的订单</div>
                    <div class="title-link" v-on:click="goOrder()">查看全部></div>
                </div>
            </div>
            <div class="warpNav" style="padding: 12px 24px;">
                <div class="nav-table">
                    <div class="nav-row">
                        <div class="nav-cell nav-bd-right" v-on:click="goOrder('待付款')">
                            <div class="nav-num" v-text="orderWaitPayCount">0</div>
                            <div class="nav-text">待付款</div>
                        </div>
                        <div class="nav-cell nav-bd-right" v-on:click="goOrder('待发货')">
                            <div class="nav-num" v-text="orderWaitSendCount">0</div>
                            <div class="nav-text">待发货</div>
                        </div>
                        <div class="nav-cell nav-bd-right" v-on:click="goOrder('待收货')">
                            <div class="nav-num" v-text="orderWaitGetCount">0</div>
                            <div class="nav-text">待收货</div>
                        </div>
                        <div class="nav-cell nav-bd-right" v-on:click="goOrder('待评价')">
                            <div class="nav-num" v-text="orderWaitReviewCount">0</div>
                            <div class="nav-text">待评价</div>
                        </div>
                        <div class="nav-cell nav-bd-right" v-on:click="goOrder('待退款')">
                            <div class="nav-num" v-text="orderWaitBackCount">0</div>
                            <div class="nav-text">退款/售后</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="warpTitle">我的财富</div>
            <div class="warpNav pAll0">
                <div class="nav-table">
                    <div class="nav-row-3">
                        <div class="nav-cell h68" v-on:click="goScore()">
                            <div class="nav-text">积分:</div>
                            <div class="nav-num" v-text="login_user.totalscore">0</div>
                        </div>
                        <div class="nav-cell h68" v-on:click="goAccountAmount()">
                            <div class="nav-text">账面佣金:</div>
                            <div class="nav-num"  v-text="login_user.lockamount">0</div>
                        </div>
                        <div class="nav-cell h68">
                            <div class="nav-text">可用佣金:</div>
                            <div class="nav-num" v-text="login_user.totalamount">0</div>
                        </div>
                    </div>
                    <div class="nav-row-3">
                        <div class="nav-cell h68">
                            <div class="nav-text">股权:</div>
                            <div class="nav-num">—</div>
                        </div>
                        <div class="nav-cell h68">
                            <div class="nav-text">期权:</div>
                            <div class="nav-num">—</div>
                        </div>
                        <div class="nav-cell h68" v-on:click="goEstimateFund()">
                            <div class="nav-text">公积金:</div>
                            <div class="nav-num" v-text="login_user.accumfund">—</div>
                        </div>
                    </div>
                    <div class="nav-row-3">
                        <div class="nav-cell h68" v-on:click="goFinancialDetails()">
                            <div class="nav-text">现金余额:</div>
                            <div class="nav-num" v-text="login_user.totalamount">0</div>
                        </div>
                        <div class="nav-cell h68">
                            <div class="nav-text"></div>
                            <div class="nav-num"></div>
                        </div>
                        <div class="nav-cell h68">
                            <div class="nav-text"></div>
                            <div class="nav-num"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="blockLine"></div>
            <div class="warpTitle">我的业务</div>
            <div class="warpNav pAll0">
                <div class="nav-table">
                    <div class="nav-row-3">
                        <div class="nav-cell h50" v-on:click="goNull()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_09.png" />
                                </div>
                                <div class="nav-group-text">团队</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goMyTeamPerformance()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_03.png" />
                                </div>
                                <div class="nav-group-text">业绩</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goHelpApplyMember()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_06.png" />
                                </div>
                                <div class="nav-group-text">代注册</div>
                            </div>
                        </div>
                    </div>
                    <div class="nav-row-3">
                        <div class="nav-cell h50" v-on:click="goNull()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_14.png" />
                                </div>
                                <div class="nav-group-text">楼盘</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goNull()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_15.png" />
                                </div>
                                <div class="nav-group-text">商品</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goNull()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_17.png" />
                                </div>
                                <div class="nav-group-text">项目</div>
                            </div>
                        </div>
                    </div>
                    <div class="nav-row-3">
                        <div class="nav-cell h50" v-on:click="goUploadCredentials()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_25.png" />
                                </div>
                                <div class="nav-group-text">资质</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goNull()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_27.png" />
                                </div>
                                <div class="nav-group-text">活动</div>
                            </div>
                        </div>
                        <div class="nav-cell h50" v-on:click="goFeedback()">
                            <div class="nav-group">
                                <div class="nav-group-img">
                                    <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/我的业务副本_23.png" />
                                </div>
                                <div class="nav-group-text">反馈</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="blockLine"></div>
            <div class="warpTitle">
                <div class="title-table">
                    <div class="title-img">
                        <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页-底部_03.png" />
                    </div>
                    <div class="title-text">购物车</div>
                    <div class="title-link" v-on:click="goShopBasket()">查看&nbsp;></div>
                </div>
            </div>
            <div class="warpTitle">
                <div class="title-table">
                    <div class="title-img">
                        <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页-底部_07.png" />
                    </div>
                    <div class="title-text">我的收藏</div>
                    <div class="title-link" v-on:click="goCollect()">查看&nbsp;></div>
                </div>
            </div>
            <div class="warpTitle">
                <div class="title-table">
                    <div class="title-img">
                        <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页-底部_11.png" />
                    </div>
                    <div class="title-text">优惠卡券</div>
                    <div class="title-link" v-on:click="goSVCard()">查看&nbsp;></div>
                </div>
            </div>
            <div class="warpTitle">
                <div class="title-table">
                    <div class="title-img">
                        <img src="http://file-cdn.songhebao.com/img/icon/20170313/images/会员中心首页-底部_14.png" />
                    </div>
                    <div class="title-text">限时特卖</div>
                    <div class="title-link" v-on:click="goSale()">查看&nbsp;></div>
                </div>
            </div>
            <div class="mTop20 txtCenter">
                <a href="javascript:;" 
                    class="weui-btn weui-btn_mini weui-btn_warn redBtn" 
                    v-on:click="loginout()">
                    退出登录
                </a>
            </div>
            <%--<div class="warpNav" v-if="nav" 
                v-bind:style="{
                'margin':nav.margin_top+'px '+nav.margin_right+'px '+nav.margin_bottom+'px '+nav.margin_left+'px',
                'padding':'0px '+nav.padding_left_right+'px',
                'background':nav.bg_color
                }">
                <div class="nav-table">
                    <div class="nav-row">
                        <div class="nav-cell" 
                            v-for="item in nav.nav_list"
                            v-bind:style="{'width':nav.col_width+'px','height':nav.col_width+'px'}"
                            v-on:click="goOrder('待付款')">
                            <img v-if="item.img"
                                 v-bind:style="{'width':nav.ico_size+'px','height':nav.ico_size+'px'}"
                                 v-bind:src="item.img">
                            <div class="nav-text" v-bind:style="{'color':item.color,'font-size':nav.font_size+'px'}"
                                 v-html="item.title">
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div style="height: 60px;"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/UserCenter.js?v=2017031001"></script>
</asp:Content>
