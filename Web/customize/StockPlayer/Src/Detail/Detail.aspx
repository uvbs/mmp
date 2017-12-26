<%@ Page Title="详情页面" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Detail.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="/customize/StockPlayer/Src/Detail/Detail.css?v=20161206" rel="stylesheet" />
      <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Css/semantic.css" rel="stylesheet" />
      <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Css/zyComment.css" rel="stylesheet" />
    <style type="text/css" media="print, screen">
	    label {
	        font-weight: bold;
	    }
	    a {
		    font-family: Microsoft YaHei;
	    }
	    #articleComment {
		    overflow: auto;
	    }
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <%
        var cssDetail = "col-xs-8";
        if (model.ArticleType != "Bowen" && model.ArticleType != "Comment" && model.ArticleType != "Stock") cssDetail = "col-xs-12";
        
    %>
    <div class="user-detail row">
        <div class="<%=cssDetail %>">
            <div class="detail-title">
                <%if (model.ArticleType != "Comment"){ %>
                    <%=model.ActivityName %>
                <%} %>
            </div>
            <%if (model.ArticleType == "Bowen" || model.ArticleType == "Comment" || model.ArticleType == "Stock" || model.ArticleType == "CompanyPublish" || model.ArticleType == "PupilDebate"||model.ArticleType=="Cognizance")
              {%>
            <div class="detail-user">
                <div class="user-heading userimg">
                    <img src="<%=avatar %>" class="head-img img-circle" />
                </div>
                <div class="detail-nick username">
                   <%=userName %>
                </div>
                <div class="time">
                    <%=model.CreateDate.ToString("yyyy-MM-dd HH:mm") %>
                </div>
            </div>
            <%} %>
            <div class="clear"></div>

            <div class="detail-content">
                <%=model.ActivityDescription %>
            </div>
            <%
                if (model.ArticleType == "Bowen" || model.ArticleType == "Comment" || model.ArticleType == "Stock" || model.ArticleType == "CompanyPublish" || model.ArticleType == "PupilDebate")
                {
                    %>
                     <div class="warp-shengming">
                    </div>
                    <%
                }
            %>
           

            <%if (model.ArticleType == "Bowen" || model.ArticleType == "Comment" || model.ArticleType == "Stock" || model.ArticleType == "CompanyPublish" || model.ArticleType == "PupilDebate" || model.ArticleType == "Cognizance")
              {%>
            <div class="detail-btn">
                <%if(hasPraise) {%>
                <button type="button" class="btn btn-default btn-deldianzan"><img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/praise1.png">&nbsp;<span>取消点赞</span></button>
                <%} else {%>
                <button type="button" class="btn btn-default btn-dianzan"><img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/praise.png">&nbsp;<span>赞一个</span></button>
                <%}%>
                <span class="Pointer dianzan"><span class="dianzan-num"><%=model.PraiseCount %></span>个人点赞</span>
            </div>

            <div>
                <!-- JiaThis Button BEGIN -->
                <div class="jiathis_style" style="padding: 25px 0px 20px 0px;">
                    <span style="float:left;">分享到：</span>
	                <a class="jiathis_button_qzone"></a>
	                <a class="jiathis_button_tsina"></a>
	                <a class="jiathis_button_tqq"></a>
	                <a class="jiathis_button_weixin"></a>
	                <a class="jiathis_button_renren"></a>
	                <a href="http://www.jiathis.com/share" class="jiathis jiathis_txt jtico jtico_jiathis" target="_blank"></a>
	                <%--<a class="jiathis_counter_style"></a>--%>
                </div>
            </div>

            <div class="detail-comment">
                <textarea class="form-control" id="commentcontent" rows="10"></textarea>
            </div>
            <div class="detail-comment-btn">
                <button type="button" class="btn btn-default btn-comment">留言</button>
            </div>
            <div class="comment-font">
                留言(<span>0</span>)
            </div>

            <div class="comment-content">
                <div class="row comment-item" style="display:none;">
                    <div class="col-xs-1 userimg">
                        <img src="/customize/StockPlayer/Img/detail-img.png" class="head-img img-circle" />
                    </div>
                    <div class="col-xs-11 rowContent">
                        <div class="username">花样丑男子</div>
                        <div class="time">1分钟前</div>
                        <div class="k3">
                            <span class="k1"></span>
                            <span class="k2"></span>
                        </div>
                        <div class="clear"></div>
                        <div class="rowButtom">
                           
                        </div>
                        <div class="rowHuifu">
                            <img src="/customize/StockPlayer/Img/huifu.png" />
                            <span class="rcount"></span>
                            <span class="huiu-font">回复</span>
                        </div>
                        <div class="add-content" style="display:none;">
                            <div class="col-xs-12 content-text">
                                <input type="text" class="form-control text-content" />
                            </div>
                            <div class="col-xs-12 content-button">
                                <button class="btn btn-primary reply">回复</button>
                            </div>
                            <div class="rList">

                            </div>
                            <div class="detail-more-reply" style="display:none;">
                                <button type="button" class="btn btn-default more-comment">查看更多回复</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="detail-more-comment">
                <button type="button" class="btn btn-default more-comment">查看更多留言</button>
            </div>

            <%} %>
        </div>

        <%
            if (model.ArticleType == "Bowen" || model.ArticleType == "Comment" || model.ArticleType == "Stock")
            {
        %>
            <div class="col-xs-4">

                <%
                if (model.ArticleType == "Stock")
                {
                    %>
                         <div class="stock-nav">
                            <div class="font1">股权数</div>
                            <div class="font2"><%=model.K3 %></div>
                            <div class="font3">
                                <button type="button" data-id="<%=userInfo.AutoID%>" class="btn btn-default btn-send">通知<%=model.CategoryName%>家</button>
                            </div>
                        </div>
                    <%
                }     
                %>




                <div class="detail-right">
                    <div class="font1">获得淘股币</div>
                    <div class="font2"><%=model.RewardTotal %></div>
                    <div class="font3">
                        <button type="button" class="btn btn-default">赠送淘股币</button>
                    </div>
                    <div class="font4" style="display:none;">
                        <div class="tb-title">赠送人列表</div>
                        <table class="table">
                            <tr style="display:none;">
                                <td class="colorY from-user username">百事可乐</td>
                                <td>赠送</td>
                                <td class="colorR"><span class="reward-num">100</span>淘股币</td>
                            </tr>
                        </table>
                    </div>
                    <div class="font5">
                        <div class="pagination pager2"></div>
                    </div>
                </div>
            </div>
        <%
            }
        %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript">
        var type = '<%= model.ArticleType %>';
        var aid = '<%= model.JuActivityID %>';
        var notice_price = '<%= sendNoticePrice%>';
        var isfriend = '<%=isFriend%>';
        var id = '<%=userInfo.AutoID%>';
        var avatar = '<%=avatar%>';
        var username = '<%=userName%>';
        var describe = '<%=userInfo.Description%>';
        var times = '<%=userInfo.OnlineTimes%>';
        var curUserName = '<%=currUserName%>';
        var curAvatar = '<%=currAvatar%>';
        var articlename = '<%=model.ActivityName%>';
        var title1 = '<%=model.ActivityName%>';
        var shareUrl1 = 'http://' + window.location.host + '/customize/StockPlayer/Src/Detail/Detail.aspx?jid=' + aid;
        var shareImgUrl1 = '<%=model.ThumbnailsPath%>';
    </script>
    <script type="text/javascript" src="http://v3.jiathis.com/code/jia.js" charset="utf-8"></script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/Detail/Detail.js?v=20161125"></script>
</asp:Content>
