<%@ Page Title="团队业绩" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="TeamPerformance.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.TeamPerformance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017031001" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
        <symbol id="icon-youjiantou" viewBox="0 0 1024 1024">
            <path d="M679.374008 511.753383 280.140305 112.531959c-11.102872-11.090593-11.102872-29.109991 0-40.177048 11.090593-11.109012 29.092595-11.109012 40.188304 0l414.455383 414.450267c2.229784 1.246387 4.973268 0.947582 6.874571 2.843768 6.076392 6.076392 8.508791 14.167674 7.936763 22.103414 0.572028 7.941879-1.860371 16.034185-7.936763 22.097274-1.902326 1.908466-4.650927 1.603521-6.886851 2.856048L320.329633 951.169251c-11.096732 11.084453-29.097712 11.084453-40.188304 0-11.102872-11.114129-11.102872-29.091572 0-40.200584L679.374008 511.753383z"  ></path>
        </symbol>
    </svg>
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="wrapCommQuery">
                <div class="queryCol" style="flex: 0.6;">
                    <div class="queryColDiv">筛选</div>
                </div>
                <div class="queryCol" v-on:click="showYear()">
                    <div class="queryColDiv" v-text="yearText">年份</div>
                    <div class="arrow">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-jiantouarrowdown"></use>
                        </svg>
                    </div>
                </div>
                <div class="queryCol" v-on:click="showMonth()">
                    <div class="queryColDiv" v-text="monthText">月份</div>
                    <div class="arrow">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-jiantouarrowdown"></use>
                        </svg>
                    </div>
                </div>
            </div>
            <div class="comm-form" v-if="performance">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">管理业绩：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="my.performance">
                        </div>
                    </div>
                </div>
                <div class="performance" v-bind:class="[ my.status>0 && my.reward >0?'divBlock':'']" v-if="my.status>0 && my.reward >0">
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">管理奖金：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="my.reward">
                            </div>
                        </div>
                    </div>
                    <div v-if="my.act_id > 0">
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">票据状态：</label>
                                </div>
                                <div class="weui_cell__bd" v-text="my.act_status_name">
                                </div>
                                <%--<div class="weui_cell_ft">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <svg class="icon arrow" aria-hidden="true">
                                                <use xlink:href="#icon-youjiantou"></use>
                                            </svg>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="comm divNone"  v-bind:class="[ my.status>0 && my.reward >0 && my.act_id ==0?'divBlock':'']" v-if="my.status>0 && my.reward >0 && my.act_id ==0">
                <a href="javascript:void(0);" class="weui-btn btn-comm" v-on:click="goApplyPerformanceReward()">票据提交</a>
            </div>
            <div class="comm-form divNone" v-bind:class="[ list.length>0 ?'divBlock':'']" v-if="list.length>0">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">业绩明细&nbsp;&nbsp;&nbsp;</label>
                        </div>
                        <div class="weui_cell__bd">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
            <div class="wrapCommList divNone" v-bind:class="[ list.length>0 ?'divBlock':'']" v-if="list.length>0">
                <div class="wrapCommGroup">
                    <div class="list-title weui-flex txt14">
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    姓名
                                </div>
                            </div>
                        </div>
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    手机
                                </div>
                            </div>
                        </div>
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    业绩
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="weui-flex h40" v-for="item in list">
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    <div class="txt14" v-text="item.name"></div>
                                </div>
                            </div>
                        </div>
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    <div class="txt14" v-text="item.phone"></div>
                                </div>
                            </div>
                        </div>
                        <div class="weui-flex__item txtCenter">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    <div class="txt14" v-text="item.performance"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="comm divNone" v-bind:class="[ my.status>0 && my.reward >0?'divBlock':'']" v-if="my.status>0 && my.reward >0">
                <a href="javascript:void(0);" class="weui-btn weui-btn_blue-0099FF border-radius-10 width-70p" v-on:click="buildExcel()">生成文档</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
        <div class="weui-skin_android comm-dialog year-dialog">
            <div class="weui-mask" v-on:click="clearDialog()"></div>
            <div class="weui-actionsheet">
                <div class="actionsheet-title">
                    选择年份
                </div>
                <div class="actionsheet-list">
                    <div class="weui-actionsheet__menu">
                        <div class="weui-actionsheet__cell"
                            v-for="item in years"
                            v-text="item.text"
                            v-bind:class="[item.value == year?'cell-selected':'']"
                            v-on:click="selectYear(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="weui-skin_android comm-dialog month-dialog">
            <div class="weui-mask" v-on:click="clearDialog()"></div>
            <div class="weui-actionsheet">
                <div class="actionsheet-title">
                    选择月份
                </div>
                <div class="actionsheet-list">
                    <div class="weui-actionsheet__menu">
                        <div class="weui-actionsheet__cell"
                            v-for="item in months"
                            v-text="item.text"
                            v-bind:class="[item.value == month?'cell-selected':'']"
                            v-on:click="selectMonth(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/TeamPerformance.js?v=2017031701"></script>
</asp:Content>
