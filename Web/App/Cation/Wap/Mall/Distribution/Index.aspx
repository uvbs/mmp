<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master"
    AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.Index" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/customize/comeoncloud/m2/dist/all.min.css?v=2017021302" rel="stylesheet" />
    <style type="text/css">
        .row {
            margin-left: 0px;
            margin-right: 0px;
        }

        .HaveBottom {
            padding-bottom: 63px;
        }

        .borderTop {
            border-top: 0px solid #F3F3F3 !important;
        }

        .headerbox .personerinfo {
            height: 90px;
        }

        .moneyinfo {
            margin-top: 0;
        }

        .linklist .linkgroup {
            margin: 0;
        }

        .content .foot {
            font-size: 14px !important;
        }

            .content .foot .col {
                padding-top: 9px !important;
                padding-bottom: 8px !important;
            }
            .footer-weixin{
                position:relative;
                border-top: 0px solid #ddd;
            }
        .dis-notice {
            font-size:14px;
            padding:10px 5px 5px 10px
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser("");
        ZentCloud.BLLJIMP.BLLWebSite bllWebsite=new ZentCloud.BLLJIMP.BLLWebSite();
       // List<ZentCloud.BLLJIMP.Model.UserInfo> userList = bllDis.GetDownUserList(CurrentUserInfo.UserID, 1);
        //userList.Add(CurrentUserInfo);
        //userList.AddRange(bllDis.GetDownUserList(CurrentUserInfo.UserID,2));
        //userList.AddRange(bllDis.GetDownUserList(CurrentUserInfo.UserID,3));
        List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo> orderList = new List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(); ;
        //if (userList.Count > 0)
        //{
            //string userIds = "";
            //foreach (var user in userList)
            //{
            //    userIds += "'" + user.UserID + "',";

            //}
            //userIds = userIds.TrimEnd(',');





            orderList = bllDis.GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And OrderUserId in (SELECT UserID FROM [dbo].[ZCJ_UserInfo] where distributionowner='{1}' Or UserID='{1}') And Status!='已取消' And OrderType in(0,1,2) And IsRefund=0 And TotalAmount>0 And IsNull(IsMain,0)=0", bllUser.WebsiteOwner, CurrentUserInfo.UserID));

        //}
        ZentCloud.BLLJIMP.Model.UserInfo preUserInfo = bllUser.GetUserInfo(CurrentUserInfo.DistributionOwner);
        ZentCloud.BLLJIMP.Model.CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig();
        if (config==null)
        {
            config = new ZentCloud.BLLJIMP.Model.CompanyWebsite_Config();
        }
    
    %>
    <div class="content">
        <%
            int level = bllDis.GetDistributionRateLevel();%>
        <input type="hidden" id="myOrderCount" value="<%=bllDis.GetMyOrderCount() %>" />
        <input type="hidden" id="isDistributionMember" value="<%=isDistributionMember %>" />
        <input type="hidden" id="websiteOwner" value="<%=websiteOwner %>" />
        <div class="headerbox">
            <div class="avatar">
                <img src="<%=bllUser.GetUserDispalyAvatar(CurrentUserInfo) %>" alt="">
            </div>
            <div class="personerinfo">
                <%--<h2 class="username"><%=CurrentUserInfo.WXNickname%></h2>--%>
                <%--<div>昵称:<%=CurrentUserInfo.WXNickname%></div>
		<p>加入时间：<%=bllDis.GetUserDistributionRegTime(CurrentUserInfo).ToString("yyyy-MM-dd")%></p>--%>
                <%--<h3>可提现财富:
            <span class="price colorRed">
            <%=Math.Round(bllDis.GetUserCanUseAmount(CurrentUserInfo),2)%>
            </span>
            元

		</h3>--%>
                <div>
                    <%=bllUser.GetUserDispalyName(CurrentUserInfo)%> ID:<%=CurrentUserInfo.AutoID  %>
                    <br />
                    <%if (isDistributionMember == 1)
                      {
                          Response.Write(bllDis.GetUserLevel(CurrentUserInfo).LevelString);
                      } %>
                </div>
                <% if (isDistributionMember != 1)
                   { %>
                <div>
                    您还不是代言人 (<a class="colorRed " href="<%=websiteInfo.ToBeDistributionMemberUrl %>">点击链接成为代言人</a>)
                </div>
                <%}
                   else
                   {%>





                <%}%>
                <div>


                    <%if (preUserInfo != null)
                      {%>
                         
                      您是由 [<%=bllUser.GetUserDispalyName(preUserInfo) %>] 推荐

                      <%} %>
                </div>
                <%-- <div>加入时间：<%=bllDis.GetUserDistributionRegTime(CurrentUserInfo).ToString("yyyy-MM-dd")%></div>--%>
            </div>
            <div class="moneyinfo" style="border-top: solid 1px #F3F3F3; margin-top: 0;">
                <div class="col-xs-6 borderTop  borderRight">
                    累计销售： <span class="price colorRed">
                        <%=CurrentUserInfo.DistributionSaleAmountLevel1+CurrentUserInfo.DistributionSaleAmountLevel0 %>
                    </span>元
                </div>
                <div class="col-xs-6 borderTop ">
                    累计<%=websiteInfo.DistributionCommissionName %>（预估）： <span class="price colorRed">
                        <%=CurrentUserInfo.HistoryDistributionOnLineTotalAmountEstimate%>
                    </span>元
                </div>
            </div>
            <div class="moneyinfo" style="border-top: solid 1px #F3F3F3; margin-top: 0;">

                <div class="col-xs-6 borderTop borderRight">
                    已提现<%=websiteInfo.DistributionCommissionName %>： <span class="price colorRed">
                        <%=Math.Round(bllDis.GetUserWithdrawTotalAmount(CurrentUserInfo), 2)%>
                    </span>元
                </div>
                <div class="col-xs-6 borderTop">
                    可提现<%=websiteInfo.DistributionCommissionName %>： <span class="price colorRed">
                        <%=Math.Round(bllDis.GetUserCanUseAmount(CurrentUserInfo),2)%>
                    </span>元
                </div>
            </div>
        </div>
        <div class="linklist bottom50 mTop1">
            <div class="linkgroup">
                <%--                       <a class="linka hideicon"
                           href="MyMember.aspx?level=1"><span class="listicon"><span class="iconfont icon-tianjia tcolor_bluegray">
                        </span></span><span class="text">我的会员</span> <span class="linkmark">
                             <span class="barnumbox" style="float:left;">
                        <%=CurrentUserInfo.DistributionDownUserCountLevel1 %>
                         
                        
                    </span>
                            
                            <span class="iconfont icon-youjiantou">
                        </span></span></a>--%>


                <div class="listgroup">
                    <span class="linka">
                        <span class="listicon">
                            <svg class="icon colorDDD" aria-hidden="true">
                                <use xlink:href="#icon-tianjia"></use>
                            </svg>
                        </span>
                        <span class="text">我的会员</span> <span class="barnumbox">
                            <%=CurrentUserInfo.DistributionDownUserCountLevel1 %>
                        
                        </span></span>

                    <ul class="listul">
                        <li class="listli"><a class="linka" href="MyMember.aspx?level=1&ispay=1"><span class="text">已下单(<%=DownUserCountHaveOrder%>)</span>
                            <span class="linkmark">
                                <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg>
                            </span></a></li>

                        <li class="listli"><a class="linka" href="MyMember.aspx?level=1&ispay=0">
                            <span class="text">未下单(<%=DownUserCountUnHaveOrder%>)</span> 
                            <span class="linkmark">
                            <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg>
                        </span>

                        </a></li>



                    </ul>
                </div>
                <div class="listgroup">
                    <span class="linka"><span class="listicon">
                            <svg class="icon colorDDD" aria-hidden="true">
                                <use xlink:href="#icon-tianjia"></use>
                            </svg></span><span class="text">会员订单</span>  </span>
                    <ul class="listul">
                        <li class="listli"><a class="linka" href="Order.aspx?status=0"><span class="text">未付款(<%=orderList.Where(p => p.DistributionStatus == 0).Count()%>单)</span>
                            <span class="linkmark">
                            <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg>
                            </span><span
                                class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus==0).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="Order.aspx?status=1"><span class="text">已付款(<%=orderList.Where(p => p.DistributionStatus ==1).Count()%>单)</span>
                            <span class="linkmark">
                            <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg></span><span class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus ==1).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="Order.aspx?status=2"><span class="text">已收货(<%=orderList.Where(p => p.DistributionStatus==2).Count()%>单)</span>
                            <span class="linkmark">
                            <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg></span><span
                                class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus==2).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="Order.aspx?status=3"><span class="text">已审核(<%=orderList.Where(p => p.DistributionStatus == 3).Count()%>单)</span>
                            <span class="linkmark">
                            <svg class="icon" aria-hidden="true">
                                <use xlink:href="#icon-youjiantou"></use>
                            </svg></span><span class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus==3).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                    </ul>
                </div>


                <%--                <a class="linka hideicon" href="#"><span class="listicon"><span class="iconfont icon-tianjia tcolor_bluegray">
                </span></span><span class="text">可提现</span><span class="barnumbox">
                    <%=Math.Round(bllDis.GetUserCanUseAmount(CurrentUserInfo),2)%>元</span> </a>--%>


                <%if (isDistributionMember == 1)
                    {%>


                <a class="linka hideicon"
                    href="javascript:toWithdraw('Withdraw.aspx');">
                    <span class="listicon">
                        <svg class="icon tcolor_bluegray" aria-hidden="true">
                            <use xlink:href="#icon-tianjia"></use>
                        </svg>
                    </span>
                    <span class="text">申请提现</span>
                    <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg>
                    </span>
                </a>

                <a class="linka hideicon" href="javascript:toMyQrcode();">
                    <span class="listicon">
                        <svg class="icon tcolor_bluegray" aria-hidden="true">
                            <use xlink:href="#icon-tianjia"></use>
                        </svg>

                    </span>
                    <span class="text">我的二维码</span> 
                    <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg>
                    </span>

                </a>

                <%if (websiteOwner != "songhe")
                    { %>

                <a class="linka hideicon" href="/App/Cation/Wap/Mall/GetMyShopUrl.aspx">
                    <span class="listicon">
                        <svg class="icon tcolor_bluegray" aria-hidden="true">
                            <use xlink:href="#icon-tianjia"></use>
                        </svg>
                    </span>
                    <span class="text">我的专属店</span>
                    <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg>
                    </span>
                </a>

                <%} %>
                
                
                <% } %>

                <% if (bllPms.CheckUserAndPmsKey(websiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.MobileOpportunities))
                   { %>

                <%--                    <a class="linka hideicon" href="/app/distribution/m/index.aspx?ngroute=/mystatic#/mystatic">
                    <span class="listicon"><span class="iconfont icon-tianjia tcolor_bluegray"></span></span>
                            <span class="text">我的商机</span> 
                    <span class="linkmark">
                        <span class="iconfont icon-youjiantou">
                        </span>
                    </span>

                </a>--%>

                <%} %>

                <%--        <a class="linka hideicon" href="http://dev.comeoncloud.net/309f5/details.chtml">
			<span class="listicon">
				<span class="iconfont icon-tianjia tcolor_bluegray"></span>
			</span>
			<span class="text">关于分销</span>
			<span class="linkmark">
				<span class="iconfont icon-youjiantou"></span>
			</span>
		</a>--%>
            </div>

            <div class="dis-notice"><%=config.QRCodeUseGuide %></div>
        </div>

        
        <div class="row foot text-center">
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var listheightdata = [];
        $(function () {
            GetPersonalCenterFoot();
            $(".listgroup").each(function (index) {
                var listul = $(this).find(".listul")
                listheightdata.push(listul.height())
                listul.css({
                    "position": "relative",
                    "opacity": "1",
                    "height": "0"
                })
                $(this).find("span.linka").bind("tap", function () {
                    if ($(this).hasClass("showlistul")) {
                        $(this).removeClass("showlistul")
                        listul.css({ "height": 0 })
                    } else {
                        $(this).addClass("showlistul")
                        listul.css({ "height": listheightdata[index] })
                    }
                })
            });
        });

        /**
         * @turl 链接
         * 获取链接的参数，json对象返回
         */
        function GetParms(turl) {
            var tParms = {};
            turl = turl.substr(turl.indexOf("?") + 1);
            if (turl != "") {
                var plit = turl.split('&');
                for (var i = 0; i < plit.length; i++) {
                    var nli = plit[i];
                    var pnlit = nli.split('=');
                    if (pnlit.length == 2) {
                        tParms[pnlit[0]] = pnlit[1];
                    }
                }
            }
            return tParms;
        }
        /**
         * @wurl  window链接
         * @turl 所配链接
         * 比较2个链接，是否有何所配链接的参数和值是否都在window链接上
         */
        function ExistsParm(wurl, turl) {
            //获取当前URL
            var wParms = GetParms(wurl);
            var tParms = GetParms(turl);

            var tParmList = Object.getOwnPropertyNames(tParms); //所配参数为空则返回true;
            if (tParmList.length == 0) return true;

            var wParmList = Object.getOwnPropertyNames(wParms); //当前链接参数少于所配返回 false;
            if (wParmList.length < tParmList.length) return false;

            for (var i = 0; i < tParmList.length; i++) {
                var tParm = tParmList[i];
                if (!wParms[tParm] || wParms[tParm] != tParms[tParm]) return false; //当前链接不存在某参数，或参数值不等于所配值 返回false;
            }
            return true;
        }
        function GetPersonalCenterFoot() {
            var key = '<% = this.Request["key"] %>';
            if (!key) key = "MallHome";
            $.ajax({
                type: 'get',
                url: "/serv/api/Component/GetKeyConfig.ashx",
                data: { key: key, property: "foottool_list" },
                dataType: "json",
                success: function (data) {
                    if (data.status && data.result && data.result.list && data.result.list.length > 0) {
                        var appendhtml = new StringBuilder();
                        if (data.result.style == 1) {
                            if (data.result.list.length > 0) {
                                var img_sizeStr = "";
                                var ico_sizeStr = "";
                                if (!!data.result.ico_size) {
                                    img_sizeStr = ' style="width: ' + data.result.ico_size + 'px;height: ' + data.result.ico_size + 'px;"';
                                    ico_sizeStr = ' style="font-size:' + data.result.ico_size + 'px;"';
                                } else {
                                    img_sizeStr = ' style="width: 24px;height: 24px;"';
                                }
                                var firstSelect = checkSelect(data.result.list[0].url);
                                var firstColorStr = "";
                                if (firstSelect && data.result.list[0].active_color != "") {
                                    firstColorStr = 'color:' + data.result.list[0].active_color + ';';
                                }else if (!firstSelect && data.result.list[0].color != "") {
                                    firstColorStr = 'color:' + data.result.list[0].color + ';';
                                }
                                var firstBgColorStr = "";
                                if (firstSelect && data.result.list[0].active_bg_color != "") {
                                    firstBgColorStr = 'background-color:' + data.result.list[0].active_bg_color + ';';
                                }else if (!firstSelect && data.result.list[0].bg_color != "") {
                                    firstBgColorStr = 'background-color:' + data.result.list[0].bg_color + ';';
                                }
                                appendhtml.AppendFormat('<div class="footer-weixin">');
                                appendhtml.AppendFormat('<div class="footer-row">');
                                appendhtml.AppendFormat('<div class="footer-first" style="{0}" onclick="toUrl(\'{1}\',\'{2}\')">',
                                    firstBgColorStr,data.result.list[0].type,data.result.list[0].url);
                                if (data.result.list[0].img) {
                                    appendhtml.AppendFormat('<div class="imgdiv">');
                                    appendhtml.AppendFormat('<img class="img" style="{0}"/>', img_sizeStr);
                                    appendhtml.AppendFormat('</div>');
                                } else if(data.result.list[0].ico) {
                                    appendhtml.AppendFormat('<div>');
                                    appendhtml.AppendFormat('<svg class="icon" {0}{1} aria-hidden="true">',firstColorStr, ico_sizeStr);
                                    var ico = data.result.list[0].ico;
                                    ico = ico.replace('iconfont ', '');
                                    appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', data.result.list[0].ico.replace('iconfont ', ''));
                                    appendhtml.AppendFormat('</svg>');
                                    appendhtml.AppendFormat('</div>');
                                }
                                appendhtml.AppendFormat('</div>');
                                appendhtml.AppendFormat('<div class="footer-other">');
                                if (data.result.list.length > 1) {
                                    appendhtml.AppendFormat('<div class="footer-other-table">');
                                    appendhtml.AppendFormat('<div class="footer-other-row">');
                                    var wp = (100 / (data.result.list.length-1));
                                    var wStr = "width:"+wp+"%";
                                    for (var i = 1; i < data.result.list.length; i++) {
                                        var itemSelect = checkSelect(data.result.list[i].url);
                                        var itemColorStr = "";
                                        if (itemSelect && data.result.list[i].active_color != "") {
                                            itemColorStr = 'color:' + data.result.list[i].active_color + ';';
                                        } else if (!itemSelect && data.result.list[i].color != "") {
                                            itemColorStr = 'color:' + data.result.list[i].color + ';';
                                        }
                                        var itemBgColorStr = "";
                                        if (itemSelect && data.result.list[i].active_bg_color != "") {
                                            itemBgColorStr = 'background-color:' + data.result.list[i].active_bg_color + ';';
                                        } else if (!itemSelect && data.result.list[i].bg_color != "") {
                                            itemBgColorStr = 'background-color:' + data.result.list[i].bg_color + ';';
                                        }
                                        appendhtml.AppendFormat('<div class="footer-other-cell" style="{0}{1}{2}"  onclick="toUrl(\'{3}\',\'{4}\')">',
                                            itemBgColorStr, itemColorStr, wStr, data.result.list[i].type, data.result.list[i].url);
                                        appendhtml.AppendFormat('{0}',data.result.list[i].title);
                                        appendhtml.AppendFormat('</div>');
                                    }
                                    appendhtml.AppendFormat('</div>');
                                    appendhtml.AppendFormat('</div>');
                                }
                                appendhtml.AppendFormat('</div>');
                                appendhtml.AppendFormat('</div>');
                                appendhtml.AppendFormat('</div>');
                            }
                        } else {
                            for (var i = 0; i < data.result.list.length; i++) {
                                if (data.result.list[i].type == "电话") {
                                data.result.list[i].url = "tel:" + data.result.list[i].url;
                                }
                                else if (data.result.list[i].type == "短信") {
                                    data.result.list[i].url = "sms:" + data.result.list[i].url;
                                }
                                var is_select = false;
                                if (!!data.result.list[i].url && document.location.href.toLowerCase().indexOf(data.result.list[i].url.toLowerCase()) >= 0) {
                                    is_select = true;
                                }
                                var hrefStr = "";
                                if (data.result.list[i].url != "") {
                                    hrefStr = 'href="' + data.result.list[i].url + '"';
                                }
                                var colorStr = "";
                                if (data.result.list[i].color != "") {
                                    colorStr = 'color:' + data.result.list[i].color + ';';
                                }
                                if (is_select) {
                                    colorStr = 'color:' + data.result.list[i].active_color + ';';
                                }
                                var bg_colorStr = "";
                                if (data.result.list[i].bg_color != "") {
                                    bg_colorStr = 'background-color:' + data.result.list[i].bg_color + ';';
                                }
                                if (is_select) {
                                    bg_colorStr = 'background-color:' + data.result.list[i].active_bg_color + ';';
                                }

                                var img_sizeStr = "";
                                var ico_sizeStr = "";
                                if (!!data.result.ico_size) {
                                    img_sizeStr = ' style="width: ' + data.result.ico_size + 'px;height: ' + data.result.ico_size + 'px;"';
                                    ico_sizeStr = ' style="font-size:' + data.result.ico_size + 'px;"';
                                } else {
                                    img_sizeStr = ' style="width: 24px;height: 24px;"';
                                }

                                var font_sizeStr = "";
                                if (!!data.result.size) {
                                    font_sizeStr = ' style="font-size:' + data.result.size + 'px;"';
                                }

                                if (data.result.list[i].img) {
                                    appendhtml.AppendFormat('<a class="col" style="{0}{1}" {2}><img {5} src="{3}" alt="" /><br>{4}</a>'
                                    , bg_colorStr, colorStr, hrefStr, data.result.list[i].img, data.result.list[i].title, img_sizeStr);
                                } else {
                                    appendhtml.AppendFormat('<a class="col" style="{0}{1}" {2}>', bg_colorStr, colorStr, hrefStr);
                                    appendhtml.AppendFormat('<svg class="icon foot-ico" {0} aria-hidden="true">', ico_sizeStr);
                                    var ico = data.result.list[i].ico;
                                    ico = ico.replace('iconfont ', '');
                                    appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', ico);
                                    appendhtml.AppendFormat('</svg><br><span {1}>{0}<span></a>', data.result.list[i].title, font_sizeStr);
                                }
                            }
                        }

                        $(".foot").html("");
                        $(".foot").append(appendhtml.ToString());

                    }
                }
            })
        }
        function checkSelect(url) {
            if ($.trim(url) != "" && window.location.href.indexOf(url) >= 0 && ExistsParm(window.location.href, url)) return true;
            return false;
        }
        function toUrl(type, url) {
            if ($.trim(url) == "") {
                alert('即将推出，敬请期待');
                return;
            }
            if (type == "电话") {
                url = "tel:" + url;
            }
            else if (type == "短信") {
                url = "sms:" + url;
            }
            window.location.href = url;
        }
        function toWithdraw(url) {
            var isDistributionMember = parseInt($('#isDistributionMember').val());
            var websiteOwner = $('#websiteOwner').val();

            if (websiteOwner == "songhe") {
                window.location.href = "/app/wap/Withdraw.aspx?back_url=/App/Cation/Wap/Mall/Distribution/Index.aspx";
                return;
            }

            if (websiteOwner == 'youxiu') {
                var myOrderCount = parseInt($('#myOrderCount').val());
                if (myOrderCount == 0) {
                    alert('需要购买一件商品才能申请提现');
                } else {
                    window.location.href = url;
                }
            }
            else {
                if (isDistributionMember == 0) {
                    alert('您还不是代言人，代言人才能申请提现');
                } else {
                    window.location.href = url;
                }
            }




        }

        function toMyQrcode() {
            var isDistributionMember = parseInt($('#isDistributionMember').val());
            var websiteOwner = $('#websiteOwner').val();
            if (websiteOwner == 'youxiu') {
                var myOrderCount = parseInt($('#myOrderCount').val());
                if (myOrderCount == 0) {
                    alert('您还不是代言人，代言人才能获取我的二维码');
                } else {
                    window.location.href = 'MyDistributionQCode.aspx';
                }
            }
            else {
                if (isDistributionMember == 0) {
                    alert('您还不是代言人，代言人才能获取我的二维码');
                } else {
                    window.location.href = 'MyDistributionQCode.aspx';
                }
            }
        }

        //function notMember() {
        //    alert('你还不是代言人，必须有一个推荐人邀请');
        //    setTimeout(function () {
        //        history.go(-1);
        //    }, 2000);
        //}

        <% if (string.IsNullOrWhiteSpace(bllDis.GetCurrentUserInfo().DistributionOwner))
           { %>
        //notMember();
        <%}%>

    </script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
