<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master" AutoEventWireup="true" CodeBehind="ChannelIndex.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.ChannelIndex" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=websiteInfo.ChannelShowName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/customize/comeoncloud/m2/dist/all.min.css?v=2016111111" rel="stylesheet" />
    <style type="text/css">
        .row{
            margin-left: 0px;
            margin-right: 0px;
        }
        .HaveBottom{
            padding-bottom:63px;
        }
        .borderTop {
                 border-top: 0px solid #F3F3F3 !important; 
        }
        .headerbox .personerinfo {
            height:90px;
        }
        .moneyinfo {
            margin-top:0;
        }
        .linklist .linkgroup {
             margin: 0;
            }
        .headerbox .personerinfo {
            height:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<%
    
    List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo> orderList = bllDis.GetChannelAllOrder(CurrentUserInfo.UserID,CurrentUserInfo.WebsiteOwner);
    
  %>
    <div class="content">
        <div class="headerbox">
            <div class="avatar">
                <img src="images/channelicon.png" alt="" style="width:50px;"/>
            </div>
            <div class="personerinfo">

                <div>
                    <%=CurrentUserInfo.ChannelName%> ID:<%=CurrentUserInfo.AutoID  %>
                    <br />
                    <%=LevelName %>
                </div>

                <div>
                    

                </div>
               
            </div>
            <div class="moneyinfo" style="border-top:solid 1px #F3F3F3;margin-top:0; ">
                <div class="col-xs-6 borderTop  borderRight">
                    累计销售： <span class="price colorRed">
                        <%=CurrentUserInfo.DistributionSaleAmountAll%>
                    </span>元
                </div>
                <div class="col-xs-6 borderTop " >
                    累计奖励（预估）： <span class="price colorRed">
                        <%=CurrentUserInfo.HistoryDistributionOnLineTotalAmountEstimate%>
                    </span>元
                </div>
            </div>
                     <div class="moneyinfo" style="border-top:solid 1px #F3F3F3;margin-top:0;">

                <div class="col-xs-6 borderTop borderRight">
                    已提现奖励： <span class="price colorRed">
                        <%=Math.Round(bllDis.GetUserWithdrawTotalAmount(CurrentUserInfo), 2)%>
                    </span>元
                </div>
                <div class="col-xs-6 borderTop">
                    可提现奖励： <span class="price colorRed">
                         <%=Math.Round(bllDis.GetUserCanUseAmount(CurrentUserInfo),2)%>
                    </span>元
                </div>
            </div>
        </div>
        <div class="linklist bottom50 mTop1">
            <div class="linkgroup">
                       <a class="linka hideicon"
                           href="MyMemberChannel.aspx"><span class="listicon"><span class="iconfont icon-tianjia tcolor_bluegray">
                        </span></span><span class="text">渠道会员</span> <span class="linkmark">
                             <span class="barnumbox" style="float:left;">
                        <%=CurrentUserInfo.DistributionDownUserCountAll%>
                    </span>
                            
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span></a>
                <div class="listgroup">

                </div>
                <div class="listgroup" >
                    <span class="linka showlistul"><span class="listicon">
                            <svg class="icon colorDDD" aria-hidden="true">
                                <use xlink:href="#icon-tianjia"></use>
                            </svg></span><span class="text" >渠道订单</span>  </span>
                    <ul class="listul" style="position: relative; opacity: 1; height: 180px;">
                        <li class="listli"><a class="linka" href="OrderChannel.aspx?status=0"><span class="text">未付款(<%=orderList.Where(p => p.DistributionStatus == 0).Count()%>单)</span>
                            <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span><span
                                class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus==0).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="OrderChannel.aspx?status=1"><span class="text">已付款(<%=orderList.Where(p => p.DistributionStatus ==1).Count()%>单)</span>
                            <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span><span
                                class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus==1).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="OrderChannel.aspx?status=2"><span class="text">已收货(<%=orderList.Where(p => p.DistributionStatus ==2).Count()%>单)</span>
                            <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span><span
                                class="barnumbox">
                                <%=orderList.Where(p=>p.DistributionStatus ==2).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                        <li class="listli"><a class="linka" href="OrderChannel.aspx?status=3"><span class="text">已审核(<%=orderList.Where(p => p.DistributionStatus == 3).Count()%>单)</span>
                            <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span><span
                                class="barnumbox">
                               <%=orderList.Where(p=>p.DistributionStatus==3).Sum(p=>p.TotalAmount)%>元</span>
                        </a></li>
                    </ul>
                </div>

                
                <a class="linka hideicon"
                        href="Withdraw.aspx?ischannel=1"><span class="listicon">
                            <svg class="icon tcolor_bluegray" aria-hidden="true">
                                <use xlink:href="#icon-tianjia"></use>
                            </svg></span><span class="text">申请提现</span> <span class="linkmark">
                        <svg class="icon" aria-hidden="true">
                            <use xlink:href="#icon-youjiantou"></use>
                        </svg></span></a>
                
<%--                    <a class="linka hideicon" href="MyDistributionQCodeChannel.aspx?sid=<%=CurrentUserInfo.AutoID %>"><span
                            class="listicon"><span class="iconfont icon-tianjia tcolor_bluegray"></span></span>
                            <span class="text">渠道二维码</span> <span class="linkmark"><span class="iconfont icon-youjiantou">
                            </span></span>

                </a>--%>
              
            </div>
        </div>
        <div class="row foot text-center">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var listheightdata = [];
        $(function () {
           
            
            setTimeout("$(\".listul\").css({\"position\": \"relative\",\"opacity\": \"1\",\"height\": \"180px\"})",1000);
            
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

        function GetPersonalCenterFoot() {
            $.ajax({
                type: 'get',
                url: "/serv/api/Component/GetKeyConfig.ashx",
                data: { key: "MallHome", property: "foottool_list" },
                dataType: "json",
                success: function (data) {
                    if (data.status && data.result && data.result.length > 0) {
                        var appendhtml = new StringBuilder();
                        for (var i = 0; i < data.result.length; i++) {
                            if (data.result[i].type == "电话") {
                                data.result[i].url = "tel:" + data.result[i].url;
                            }
                            else if (data.result[i].type == "短信") {
                                data.result[i].url = "sms:" + data.result[i].url;
                            }
                            var is_select = false;
                            if (!!data.result[i].url && document.location.href.toLowerCase().indexOf(data.result[i].url.toLowerCase()) >= 0) {
                                is_select = true;
                            }
                            var hrefStr = "";
                            if (data.result[i].url != "") {
                                hrefStr = 'href="' + data.result[i].url + '"';
                            }
                            var colorStr = "";
                            if (data.result[i].color != "") {
                                colorStr = 'color:' + data.result[i].color + ';';
                            }
                            if (is_select) {
                                colorStr = 'color:' + data.result[i].active_color + ';';
                            }
                            var bg_colorStr = "";
                            if (data.result[i].bg_color != "") {
                                bg_colorStr = 'background-color:' + data.result[i].bg_color + ';';
                            }
                            if (is_select) {
                                bg_colorStr = 'background-color:' + data.result[i].active_bg_color + ';';
                            }

                            if (data.result[i].img) {
                                appendhtml.AppendFormat('<a class="col" style="{0}{1}" {2}><img style="width: 24px;height: 24px;" src="{3}" alt="" /><br>{4}</a>'
                                , bg_colorStr, colorStr, hrefStr, data.result[i].img, data.result[i].title);
                            } else {
                                appendhtml.AppendFormat('<a class="col" style="{0}{1}" {2}>', bg_colorStr, colorStr, hrefStr);
                                appendhtml.AppendFormat('<svg class="icon foot-ico" aria-hidden="true">');
                                var ico = data.result[i].ico;
                                ico = ico.replace('iconfont ', '');
                                appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', ico);
                                appendhtml.AppendFormat('</svg><br>{0}</a>', data.result[i].title);
                            }

                        }
                        $(".foot").html("");
                        $(".foot").append(appendhtml.ToString());

                    }
                }
            })
        }






    </script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>