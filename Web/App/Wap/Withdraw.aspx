<%@ Page Title="提现" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Withdraw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
        <symbol id="icon-gantanhao2-copy" viewBox="0 0 1024 1024">
            <path d="M525.982 21.639c-271.948 0-492.093 220.497-492.093 492.093s220.497 492.093 492.093 492.093 492.093-220.147 492.093-492.093-220.497-492.093-492.093-492.093zM587.932 184.736l-23.101 468.994h-78.399l-23.101-468.994h124.599zM577.081 823.479c-14.701 12.949-31.85 19.249-51.45 19.249s-36.398-6.65-49.698-20.299c-14-12.25-21.349-28.701-21.349-49.698 0-20.999 6.998-37.45 21.349-49.698 12.949-12.949 29.399-19.249 49.698-19.249 20.999 0 38.15 6.299 51.45 19.249 13.65 13.65 20.299 30.099 20.299 49.698 0 20.299-6.998 37.1-20.299 50.749z"></path>
        </symbol>
        <symbol id="icon-youjiantou" viewBox="0 0 1024 1024">
            <path d="M679.374008 511.753383 280.140305 112.531959c-11.102872-11.090593-11.102872-29.109991 0-40.177048 11.090593-11.109012 29.092595-11.109012 40.188304 0l414.455383 414.450267c2.229784 1.246387 4.973268 0.947582 6.874571 2.843768 6.076392 6.076392 8.508791 14.167674 7.936763 22.103414 0.572028 7.941879-1.860371 16.034185-7.936763 22.097274-1.902326 1.908466-4.650927 1.603521-6.886851 2.856048L320.329633 951.169251c-11.096732 11.084453-29.097712 11.084453-40.188304 0-11.102872-11.114129-11.102872-29.091572 0-40.200584L679.374008 511.753383z"></path>
        </symbol>
    </svg>
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="weui-tab divNone" v-bind:class="[check.payPwd.ok?'divBlock':'']" v-if="check.payPwd.ok">
                <div class="weui-navbar comm-navbar">
                    <div class="weui-navbar__item" v-bind:class="[tab===0?'weui-bar__item_on':'']" v-on:click="selectTab(0)">
                        提现
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===1?'weui-bar__item_on':'']" v-on:click="selectTab(1)">
                        提现记录
                    </div>
                </div>
                <div class="weui-tab__panel" v-if="tab===0">
                    <div>
                        <div class="comm-form comm-textarea-form">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">可用<%=website.TotalAmountShowName %>：</label>
                                    </div>
                                    <div class="weui_cell__bd" v-text="login_user.totalamount">
                                    </div>
                                </div>
                            </div>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">提现<%=website.TotalAmountShowName %>：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input"
                                            type="tel"
                                            placeholder="请输入提现<%=website.TotalAmountShowName %>"
                                            v-model="form.amount"
                                            maxlength="7"
                                            v-on:keyup.enter="withdraw()" />
                                    </div>
                                </div>
                            </div>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui_cell__bd">
                                        <svg class="icon color-red" aria-hidden="true">
                                            <use xlink:href="#icon-gantanhao2-copy"></use>
                                        </svg>
                                        实际到账93.22%
                                    </div>
                                </div>
                            </div>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">银行卡：</label>
                                    </div>
                                    <div class="weui_cell__bd"
                                        v-bind:class="[!check.ex3.ok?'tip':'']"
                                        v-html="check.ex3.text"
                                        v-on:click="showCardDialog()">
                                    </div>
                                    <div class="weui_cell__ft cell-level"
                                        v-if="cards.length>0"
                                        v-on:click="showCardDialog()">
                                        <div class="cell-tb">
                                            <div class="cell-td">
                                                <svg class="icon arrow" aria-hidden="true">
                                                    <use xlink:href="#icon-jiantouarrowdown"></use>
                                                </svg>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="weui-cells__title">备注：</div>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__bd">
                                        <textarea class="weui-textarea" placeholder="请输入备注" v-model="form.content" rows="5" maxlength="500"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="color-red font14 mTop10 divNone" style="padding:5px 15px;" v-bind:class="[login_user.level>21?'divBlock':'']" v-if="login_user.level>21">
                            <p>尊敬的经销商，您好！</p>
                            <p>您此次提现，需要开具合规的发票资料，方能将款项汇至贵公司账户。</p>
                            <p>具体开票资料如下：</p>
                            <p>公司名称：上海颂和科技股份有限公司</p>
                            <p>发票类型：服务费</p>
                            <p>公司地址：上海市龙田路195号4号楼3楼</p>
                            <p>联系电话：400-858-2628</p>
                        </div>
                        <div class="color-red font14 txtCenter mTop10 divNone" v-bind:class="[check.canApply==2?'divBlock':'']" v-if="check.canApply ==2">
                            很抱歉，只能在每月5号、15号、25号才可提现。
                        </div>
                        <div class="comm divNone" v-bind:class="[tab==0?'divBlock':'']">
                            <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="!(form.amount>0 && form.ex3 && check.canApply==1)">提现</a>
                            <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.amount>0 && form.ex3 && check.canApply==1" v-on:click="withdraw()">提现</a>
                        </div>
                    </div>
                </div>
                <div class="weui-tab__panel" v-if="tab===1">
                    <div class="wrapCommList">
                        <div class="wrapCommGroup" v-for="group in groupLogs">
                            <div class="list-title">
                                <table class="wrapCommTitleTable">
                                    <tr>
                                        <td>
                                            <span v-text="group.group"></span>
                                        </td>
                                        <td class="txtRight">
                                            <span v-text="group.groupAmount"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="weui-flex" v-for="log in group.logs" v-on:click="goDetail(log)">
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt15" v-text="log.start_d"></div>
                                            <div class="txt13" v-text="log.start_m"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt14" v-text="log.ex1"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt13"
                                                v-bind:class="[log.status==8 || log.status==10 || log.status==11||log.status==12?'color-red':'' ]"
                                                v-text="log.status_s">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt14" v-text="log.amount"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item to-detail-icon txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <svg class="icon" aria-hidden="true">
                                                <use xlink:href="#icon-youjiantou"></use>
                                            </svg>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length < log.total?'divBlock':'']" v-if="log.list.length < log.total">-- 正在加载中 -- </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length >= log.total?'divBlock':'']" v-if="log.list.length >= log.total">-- 没有更多了 -- </div>
                </div>
            </div>
            <check-pay-password v-on:checkpayok="checkPayPwdOk()" v-if="!check.payPwd.ok"></check-pay-password>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
        <div class="weui-skin_android comm-dialog" v-if="cards.length>0">
            <div class="weui-mask" v-on:click="clearDialog()"></div>
            <div class="weui-actionsheet">
                <div class="actionsheet-title">
                    银行卡
                </div>
                <div class="actionsheet-list">
                    <div class="weui-actionsheet__menu">
                        <div class="weui-actionsheet__cell"
                            v-for="item in cards"
                            v-html="item.Text"
                            v-on:click="selectCard(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/component/CheckPayPwd/CheckPayPwd.js?v=2017030101"></script>
    <script type="text/javascript" src="/App/Wap/js/Withdraw.js?v=2017030101"></script>
</asp:Content>
