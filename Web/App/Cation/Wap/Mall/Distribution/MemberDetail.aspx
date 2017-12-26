<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master" AutoEventWireup="true" CodeBehind="MemberDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.MemberDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
我的会员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<%ZentCloud.BLLJIMP.BLLDistribution bllDis = new ZentCloud.BLLJIMP.BLLDistribution();
  if (bllDis.GetUserBetweenLevel(bllDis.GetCurrentUserInfo(),UserInfo ) == 0)
  {
      Response.Write("拒绝查看");
      Response.End();
  }
  int level =bllDis.GetDistributionRateLevel();
 %>
<div class="headerbox">
	<div class="avatar">
		<img src="<%=UserInfo.WXHeadimgurlLocal %>" alt=""/>
	</div>
	<div class="personerinfo">
		<h2 class="username"><%=UserInfo.WXNickname%></h2>
		<p>加入时间：<%=bllDis.GetUserDistributionRegTime(UserInfo).ToString("yyyy-MM-dd")%></p>
	</div>
	<div class="moneyinfo">
      	<div class="col-xs-6">累计销售：<%=UserInfo.DistributionSaleAmountLevel0+UserInfo.DistributionSaleAmountLevel1%>元</div>
		<div class="col-xs-6">累计奖励（预估）：<%=UserInfo.HistoryDistributionOnLineTotalAmountEstimate%>元</div>

	</div>
</div>

<div class="linklist bottom50">
	<div class="linkgroup">
		<div class="listgroup">
			<span class="linka">
				<span class="listicon">
                    <svg class="icon tcolor_bluegray" aria-hidden="true">
                        <use xlink:href="#icon-tianjia"></use>
                    </svg>
				</span>
				<span class="text">Ta的会员</span>
				<span class="barnumbox"><%=UserInfo.DistributionDownUserCountLevel1 %>人</span>
			</span>
			<ul class="listul">
				<li class="listli">
					<span class="linka" >
						<span class="text">会员人数</span>
						<span class="barnumbox2"><%=UserInfo.DistributionDownUserCountLevel1 %>人</span>
					</span>
				</li>
				<li class="listli">
					<span class="linka" >
						<span class="text">消费金额</span>
						<span class="barnumbox2"><%=UserInfo.DistributionSaleAmountLevel0 %>元</span>
					</span>
				</li>

			</ul>
		</div>
        <%--<%if (level>=2){ %>
              <div class="listgroup">
			<span class="linka">
				<span class="listicon">
					<span class="iconfont icon-tianjia tcolor_bluegray"></span>
				</span>
				<span class="text">Ta二级会员</span>
				<span class="barnumbox"><%=bllDis.GetDownUserCount(UserInfo.UserID,2) %>人</span>
			</span>
			<ul class="listul">
				<li class="listli">
					<span class="linka" >
						<span class="text">会员人数</span>
						<span class="barnumbox2"><%=bllDis.GetDownUserCount(UserInfo.UserID,2) %>人</span>
					</span>
				</li>
				<li class="listli">
					<span class="linka" >
						<span class="text">消费金额</span>
						<span class="barnumbox2"><%=bllDis.GetUserOrderTotalAmountByLevel(UserInfo.UserID,2) %>元</span>
					</span>
				</li>

			</ul>
		</div>
        <%} %>
		
        <%if (level>=3){ %>
          <div class="listgroup">
			<span class="linka">
				<span class="listicon">
					<span class="iconfont icon-tianjia tcolor_bluegray"></span>
				</span>
				<span class="text">Ta三级会员</span>
				<span class="barnumbox"><%=bllDis.GetDownUserCount(UserInfo.UserID,3)%>人</span>
			</span>
			<ul class="listul">
				<li class="listli">
					<span class="linka" >
						<span class="text">会员人数</span>
						<span class="barnumbox2"><%=bllDis.GetDownUserCount(UserInfo.UserID,3) %>人</span>
					</span>
				</li>
				<li class="listli">
					<span class="linka" >
						<span class="text">消费金额</span>
						<span class="barnumbox2"><%=bllDis.GetUserOrderTotalAmountByLevel(UserInfo.UserID,3)%>元</span>
					</span>
				</li>

			</ul>
		</div>
       <%} %>--%>
		
	</div>
</div>

<div class="backbar">
	<a class="col-xs-2" href="javascript:history.go(-1)">
        <svg class="icon colorDDD" aria-hidden="true">
            <use xlink:href="#icon-fanhui"></use>
        </svg>
	</a>
	<div class="col-xs-8">
		
	</div>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var listheightdata = []
    $(".listgroup").each(function (index) {
        var listul = $(this).find(".listul")
        listheightdata.push(listul.height())
        listul.css({
            "position": "relative",
            "opacity": "1",
            "height": "0"
        })
        $(this).bind("tap", function () {
            if ($(this).hasClass("showlistul")) {
                $(this).removeClass("showlistul")
                listul.css({ "height": 0 })
            } else {
                $(this).addClass("showlistul")
                listul.css({ "height": listheightdata[index] })
            }
        })
    })
</script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
