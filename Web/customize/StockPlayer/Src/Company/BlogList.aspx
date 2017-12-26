<%@ Page Title="公司发布" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="BlogList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Company.BlogList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/Company/Blog.css?v=20161125" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="Width1000 mTop20">
        <div class="row blog-head">
            <div class="col-xs-12">
                <input type="text" class="form-control blog-head-text" id="keyword" placeholder="请输入关键字"/>
                <button type="button" class="btn btn-default blog-head-button">搜索</button>
            </div>
        </div>
        <div class="row index-blogs">
                 <div class="mTop20 blogs-item" style="display:none;">
                       <div class="col-xs-4 index-bowen-item">
                         <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/bowen.png" class="img-rounded bowen-img"/>
                     </div>
                       <div class="col-xs-8 index-bowen-content">
                         <div class="head">
                                 民资挺进充电桩建设：盈利模式单陷两难尴尬
                         </div>
                         <div class="context">
                              <div class="user">
                                <div class="userimg"><img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/bowenuser.png" class="head-img img-circle"/></div>
                                <div class="company-name username">上海某某公司</div>
                                <div class="time">20分钟前</div>
                                <div class="clearfix"></div>
                         </div>
                             <div class="icon">
                                   <div class="comment">
                                       <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png"/><span>123</span></div>
                                  <div class="zan">
                                       <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png"/><span>123</span></div>
                             </div>
                         </div>
                         <div class="buttom">
                             <a class="Pointer">
                             9月4日下午，参加G20杭州峰会的36位领导人2，在主场馆杭州国际博览中心大合影。
                             前排中央为G20“三驾马车”，指的是中国、土耳其、和德国。中国是这一届G20峰会的主席国，
                             土耳其是上一届安塔利亚峰会的主席国，德国马上就要接棒成为下一界峰会的主席国，
                             随着习近平主席在二十国领导人峰会上的宣布，杭州将接过安塔利亚的接力棒，
                             开始全力筹备2016年G20峰会。2015年11月14日上午，财政部副部长朱光耀向外界透露，
                                 </a>
                         </div>
                     </div>
                 </div>
             </div>
        
             <div class="row index-bottom">
                <div class="pagination pager"></div>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
      <script type="text/javascript" src="/customize/StockPlayer/Src/Company/Blog.js"></script>
</asp:Content>
