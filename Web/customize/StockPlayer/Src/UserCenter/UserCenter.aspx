<%@ Page Title="用户中心" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="UserCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.UserCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/UserCenter/UserCenter.css?v=2016102001" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <% if (userInfo == null)
       { %>
    <div class="Width1000 mTop20 user">

        <div class="border-comment">
            用户登录
        </div>
        <div class="contentLogin">
            <%--<div class="login-head">
                用户登录
            </div>--%>
            <div class="login-body">
                <form name="userRegister">
                    <div class="row">
                        <div class="col-xs-4 textRight">
                            <label class="control-label">账号：</label>
                        </div>
                        <div class="col-xs-4 textLeft">
                            <input type="text" name="user_acount" id="user_acount" class="form-control" placeholder="请输入账号" />
                        </div>
                        <div class="col-xs-4">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-xs-4 textRight">
                            <label class="control-label">密码：</label>
                        </div>
                        <div class="col-xs-4 textLeft">
                            <input type="password" name="user_pwd" id="user_pwd" class="form-control" placeholder="请输入密码" />
                        </div>
                        <div class="col-xs-4">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 textCenter">
                            <button type="button" id="loginUser" class="btn btn-default btn-login">登录</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <%}
       else if (userInfo.UserType == 6)
       { %>
    <div class="Width1000 mTop20">
        <div class="row company">
            <div class="col-xs-8">
                <div class="company-info">
                    <div class="company-logo-div">
                        <%if (string.IsNullOrWhiteSpace(userInfo.Avatar))
                          {%>
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/clogo.png" class="company-logo" />
                        <%}
                          else
                          { %>
                        <img src="<%= userInfo.Avatar%>" class="company-logo" />
                        <%} %>
                    </div>
                    <div class="company-row1">
                        <%= bllUser.GetUserDispalyName(userInfo) %>
                        <span>ID：<%= userInfo.UserID%></span>
                        
                    </div>
                    <div class="company-row2">
                        <div class="company-phone">
                            <%if (!string.IsNullOrWhiteSpace(userInfo.Phone) && (isMe || userInfo.ViewType == 1))
                              {%>
                            手机：<%= userInfo.Phone %>
                            <%} %>
                        </div>

                        <%if (!string.IsNullOrWhiteSpace(userInfo.Phone) && (isMe || userInfo.ViewType == 1))
                          {%>
                        <span class="company-empty"></span>
                        <%} %>
                    </div>
                    <div class="company-describe">
                        <%= userInfo.Description %>
                    </div>
                    <%
                   if (isMe)
                   {
                        %>
                            <div class="company-add-content">
                        <%if (userInfo.MemberApplyStatus ==9){ %>
                        <a href="/customize/StockPlayer/Src/UserCenter/ReleaseContent/ReleaseContent.aspx">发布新内容</a>
                        <%}else if (userInfo.MemberApplyStatus ==1){ %>
                        <a href="javascript:void(0);" style="color:#aaa;">发布新内容（等待审核）</a>
                        <%}else if (userInfo.MemberApplyStatus ==2){ %>
                        <a href="javascript:void(0);" style="color:#aaa;">发布新内容（请联系客服人工通过审核）</a>
                        <%} %>
                    </div>
                        <%
                   }     
                    %>
                    
                </div>
                <div class="release-head ">
                    公司发布
                </div>
                <div class="company-release">
                    <div class="company-release-item row borderdesh" style="display:none;">
                        <div class="col-xs-4">
                            <img class="img-rounded release-img" />
                        </div>
                        <div class="col-xs-8">
                            <div class="title" style="width:350px;">楼市火热银行忐忑 监管层重提房地产信贷风</div>
                            <%
                               if (isMe)
                               {
                                    %>
                                    <div class="company-operation"">
                                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/icon-edit.png" title="编辑" class="icon-edit"/>
                                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/icon-del.png"" title="删除" class="icon-del"/>
                                     </div>
                                    <%        
                               }     
                            %>
                            
                            <div class="head">
                                <div class="userimg userHeadImg" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0" data-times="0">
                                    <img src="http://static-files.socialcrmyun.com/img/europejobsites.png" />
                                </div>
                                <div class="username" data-id="0" data-nickname="昵称" data-avatar="" data-friend="0" data-times="0">昵称</div>
                                <div class="time">17:12</div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="icon">
                                <div class="comment">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png"><span>0</span>
                                </div>
                                <div class="zan">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png"><span>0</span>
                                </div>
                            </div>
                            <div class="context">
                                <a>个人住房按揭贷款过去两个月异于寻常的增长未打消银行继续大力投放的决心，但快速上涨的房价也让银行在加大投入的同时心生忐忑。在这样一个时点，银监会主席尚福林再次强调要加强对房地产信贷压力测试和风险测试。而上一次银监会官方提及房地产信贷压力测试还是在2014年。</a>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="bottom-pager">
                        <div class="pagination pager5"></div>
                    </div>
            </div>
            <div class="col-xs-4">
                <div class="right-content">
                    <div class="right-head">
                        营业执照
                    </div>
                    <div class="right-img">
                        <%if (string.IsNullOrWhiteSpace(userInfo.Ex3))
                          {%>
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zhizhao.png" class="company-img" />
                        <%}
                          else
                          { %>
                        <img src="<%= userInfo.Ex3%>" class="company-img" />
                        <%} %>
                    </div>
                </div>
                <%if (isMe)
                  { %>
                <div class="right-buttom">
                    <div class="right-ziliao">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/updateuser.png" class="Pointer" />
                        <span class="Pointer" onclick="EditUser()">编辑公司资料</span>
                    </div>
                    <div class="right-ziliao to-notice">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/xiaoxi.png" class="Pointer" />
                        <span class="Pointer">消息中心<span class="UnReadMsg"></span></span>
                    </div>
                    <div class="right-pwd">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/updatepwd.png" class="Pointer" />
                        <span class="Pointer" id="updateCompanyPwd">修改密码</span>
                    </div>
                </div>
                <%} %>
            </div>
        </div>
    </div>
    <% }
       else
       { %>
    <div class="Width1000 mTop20">
        <div class="row user">
            <div class="col-xs-8">
                <div class="user-info">
                    <div class="user-avatar">
                        <img src="<%= bllUser.GetUserDispalyAvatar(userInfo) %>" class="user-logo" />
                    </div>
                    <div class="user-row1">
                        <div class="font-nick">
                            <%= bllUser.GetUserDispalyName(userInfo) %>
                        </div>
                        <div class="color-xingji">
                            <%--（星级）--%>
                        </div>
                    </div>
                    <%if (!isMe)
                      {%>
                    <%if (curUser != null && curUser.UserType == 6) { }
                      else{%>
                    <%if (!isFriend)
                      {%>
                    <button type="button" class="btn btn-default btn-jiahaoyou" data-id="<%=userInfo.AutoID %>" data-nickname="<%=bllUser.GetUserDispalyName(userInfo) %>">加好友</button>
                    <%}
                      else
                      {%>
                    <button type="button" class="btn btn-default btn-delhaoyou" data-id="<%=userInfo.AutoID %>" data-nickname="<%=bllUser.GetUserDispalyName(userInfo) %>">删除好友</button>
                    <%} %>
                     <%} %>

                    <%} %>
                    <div class="user-row2">
                        <%if (isMe || userInfo.ViewType == 0)
                          {%>
                        <div class="user-phone">
                            手机：<%= userInfo.Phone %>
                        </div>
                        <%} %>
                        <%if (isMe && string.IsNullOrWhiteSpace(userInfo.WXOpenId))
                          {%>
                        <span class="company-wx">绑定微信</span>
                        <%} %>
                    </div>
                    <div class="user-content">
                        <%=userInfo.Description %>
                    </div>
                    <%if (isMe)
                      {%>
                    <div class="user-add-content">
                        <a href="/customize/StockPlayer/Src/UserCenter/ReleaseContent/ReleaseContent.aspx">发布新内容</a>
                    </div>
                    <%} %>
                </div>
                <div class="total-nav">
                    <div class="row table">
                        <div class="tdYellow cell" data-index="0">全部</div>
                        <div class="tdGray cell" data-index="1">
                            谈股论金<br />
                            <span class="total-bowen">0</span>
                        </div>
                        <div class="tdGray cell" data-index="2">
                            股权交易<br />
                            <span class="total-stock">0</span>
                        </div>
                        <div class="tdGray cell" data-index="3">
                            评论荟集<br />
                            <span class="total-comment">0</span>
                        </div>
                        <div class="tdGray cell" data-index="4">
                            好友<br />
                            <span class="total-friend">0</span>
                        </div>
                    </div>
                </div>
                <div class="user-0">
                    <div class="border-comment">
                        评论
                    </div>
                    <div class="usercenter-comment">
                        <div class="comment-item" style="display: none;">
                            <div class="head">
                                <div class="userimg" data-id="" data-nickname="" data-avatar="" data-friend="">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/headimg.png" />
                                </div>
                                <div class="username" data-id="" data-nickname="" data-avatar="" data-friend="">牛仔很忙</div>
                                <div class="time">1分钟前</div>
                                <div>
                                    <span class="btn btn-default">早评</span>
                                </div>
                            </div>
                            <div class="content">
                                时代开始的可能是就是你的家可是你的就考试难度就可能是的接口是你的锦囊 时代开始的开始对你可见哪款手机电脑接口
                                    时代开始的可能是就是你的家可是你的就考试难度就可能是的接口是你的锦囊 时代开始的开始对你可见哪款手机电脑接口
                            </div>
                            <div class="buttom">
                               
                                <div class="comment">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png" /><span>123</span>
                                </div>
                                <div class="zan">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png" /><span>456</span>
                                </div>
                                <div class="score">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/score.png" /><span>789</span>
                                </div>
                            </div>
                            <%
                           if (isMe)
                           {
                               %>
                                <div class="comment-operation">
                                   <button class="btn btn-danger btn-del">删除</button>
                             </div>
                               <%                         
                           }    
                             %>
                             
                        </div>
                    </div>
                    <div class="border-comment">
                        谈股论金
                    </div>
                    <div class="usercenter-bowen">
                        <div class="bowen-item" style="display: none;">
                            <div class="head">
                                <div class="userimg">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/headimg.png" />
                                </div>
                                <div class="username">侠名</div>
                                <div class="time">30分钟前</div>
                            </div>
                            <div class="bowen-title">
                                民资挺进充电桩建设：盈利模式单陷两难尴尬
                            </div>
                            <div class="bowen-icon">
                                <div class="comment">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/comment.png" /><span>123</span>
                                </div>
                                <div class="zan">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zan.png" /><span>456</span>
                                </div>
                                <div class="score">
                                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/score.png" /><span>789</span>
                                </div>
                            </div>
                            <div class="bowen-content">
                                9月4日下午，参加G20杭州峰会的36位领导人2，在主场馆杭州国际博览中心大合影。
                                         前排中央为G20“三驾马车”，指的是中国、土耳其、和德国。中国是这一届G20峰会的主席国，
                                         土耳其是上一届安塔利亚峰会的主席国，德国马上就要接棒成为下一界峰会的主席国，
                                         随着习近平主席在二十国领导人峰会上的宣布，杭州将接过安塔利亚的接力棒，
                                         开始全力筹备2016年G20峰会。2015年11月14日上午，财政部副部长朱光耀向外界透露，
                            </div>
                            <%
                               if (isMe)
                               {
                                    %>
                                     <div class="bowen-operation">
                                        <button class="btn btn-primary btn-edit">编辑</button>
                                        <button class="btn btn-danger btn-del">删除</button>
                                    </div>
                                    <%                
                               }
                            %>
                           
                        </div>
                    </div>
                    <div class="border-comment">
                        股权交易
                    </div>

                    <div class="usercenter-stock">
                        <div class="user-stock row">
                            <div class="col-xs-6" style="display: none;">
                                <div class="user-stock-top">
                                    <div class="stock-img">
                                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/Stock.png" class="body-content-img" />
                                        <div id="postion">上架中</div>
                                    </div>
                                    <div class="stock-title">
                                        <span class="Font18 title">奇境GinSPA</span>
                                        <span class="btn btn-default btn-type">类型</span>
                                    </div>
                                    <div class="stock-user">
                                        <span class="colorY username">牛仔很忙</span>
                                        <span class="time">2012-12-12 24:23</span>
                                    </div>
                                    <div class="stock-content">
                                        实打实大苏打都是收到收到收到收到
                                            实打实大苏打都是收到收到收到收到
                                            实打实大苏打都是收到收到收到收到
                                    </div>
                                    <div class="stock-buttom">股权数：<span class="stock-num">5000</span></div>

                                </div>
                                <%
                               if (isMe)
                               {
                                   %>
                                <div class="user-stock-buttom">
                                    <button type="button" class="btn btn-default button-gray btn-edit">编辑</button>
                                    <button type="button" class="btn btn-default button-gray mLeft10 btn-sj">上架</button>
                                    <button type="button" class="btn btn-default button-gray mLeft10 btn-xj">下架</button>
                                    <button type="button" class="btn btn-default button-red mLeft20 btn-del">删除</button>
                                </div>
                                        <%
                                   }    
                                 %>
                               
                            </div>
                        </div>
                    </div>
                </div>
                <div class="user-1" style="display: none;">
                    <div class="border-comment">
                        谈股论金
                    </div>
                    <div class="list-bowen">
                    </div>
                    <div class="bottom-pager">
                        <div class="pagination pager1"></div>
                    </div>
                </div>
                <div class="user-2" style="display: none;">
                    <div class="border-comment">
                        股权交易
                    </div>
                    <div class="list-stock">
                        <div class="user-stock row">
                        </div>
                    </div>
                    <div class="bottom-pager">
                        <div class="pagination pager2"></div>
                    </div>
                </div>
                <div class="user-3" style="display: none;">
                    <div class="border-comment">
                        评论
                    </div>
                    <div class="list-comment">
                    </div>
                    <div class="bottom-pager">
                        <div class="pagination pager3"></div>
                    </div>
                </div>
                <div class="user-4" style="display: none;">
                    <div class="border-comment">
                        好友
                    </div>
                    <div class="list-friend row">
                        <div class="friend-info col-xs-12" style="display: none;">
                            <div class="user-avatar">
                                <img src="http://open-files.comeoncloud.net/www/hf/jubit/image/20160603/E7FB12C89B48455CA88E87B1FD065B51.jpg" class="user-logo" />
                            </div>
                            <div class="user-row1">
                                <div class="font-nick">
                                    聚比特
                                </div>
                                <div class="color-xingji">
                                    （星级）
                                </div>
                            </div>

                         
                                    <%--<button type="button" class="btn btn-default btn-jiahaoyou" data-id="" data-nickname="">加好友</button>--%>
                                    

                                     <%if (isMe)
                                      {%>
                                            <button type="button" class="btn btn-default btn-delhaoyou" data-id="" data-nickname="">删除好友</button>
                                    <%}%>


                            
                           





                            <div class="user-row2">
                                <div class="user-phone">
                                    手机：<span>15576593692</span>
                                </div>
                            </div>
                            <div class="user-content">
                            </div>
                        </div>
                    </div>
                    <div class="bottom-pager">
                        <div class="pagination pager4"></div>
                    </div>
                </div>
            </div>
            <div class="col-xs-4">
                <div class="right-content">
                    <div class="right-title">当前淘股币</div>
                    <%
                       if (string.IsNullOrEmpty(id))
                       {
                            %>
                                    <div class="right-taobi Pointer"><%=userInfo.TotalScore %></div>
                            <%
                       }
                       else
                       {
                           %>
                                    <div class="right-taobi-friends"><%=userInfo.TotalScore %></div>
                            <%
                       }
                    %>
                    

                    <%if (isMe)
                      {%>
                    <div class="right-buttom">
                        <button type="button" class="btn btn-default btn-cz">充值</button>
                        <button type="button" class="btn btn-default btn-tx">提现</button>
                    </div>
                    <%}%>
                </div>
                <%if (isMe)
                  {%>
                <div class="right-buttom">
                    <div class="right-ziliao">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/updateuser.png" class="Pointer" />
                        <span class="Pointer" onclick="EditUser()">编辑个人资料</span>
                    </div>
                    <div class="right-ziliao to-notice">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/xxcenter.png" class="Pointer" />
                        <span class="Pointer">消息中心<span class="UnReadMsg"></span></span>
                    </div>
                    <div class="right-ziliao">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/uppwd.png" class="Pointer" />
                        <span class="Pointer" id="updatePwd">修改密码</span>
                    </div>
                    <div class="right-ziliao to-share">
                        <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/qrcode.png"  class="qrcodeimg"/>
                        <span class="Pointer" id="shareQRCode">分享到微信</span>
                    </div>
                </div>
                <%}%>
            </div>
        </div>
    </div>
    <%
       }   
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js"></script>
    <script type="text/javascript">
        var u_iscenter = true;
        var u_id = <%= userInfo == null ? 0 :userInfo.AutoID %>;
        var u_type = '<%= userInfo == null ? 0 :userInfo.UserType %>';
        var u_times = '<%= userInfo == null ? 0 :userInfo.OnlineTimes %>';
        var isme = <%= isMe ? 1 :0 %>;
        <%if (isMe)
          {%>
        var u_has_wx = '<%= (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.WXOpenId))? 1 :0 %>';
            <%if (userInfo !=null && userInfo.UserType == 6)
              {%>
        var u_ex3 = '<%= userInfo.Ex3 %>';
        var u_phone = '<%= userInfo.Phone %>';
        var u_name = '<%= userInfo.TrueName %>';
        var u_avatar = '<%= userInfo.Avatar %>';
        var u_describe = '<%=ZentCloud.Common.StringHelper.GetReplaceStr(userInfo.Description)%>';
        var u_view_type = '0';
            <%}
              else if (userInfo !=null && userInfo.UserType != 6)
              {%>
        var u_phone = '<%= userInfo.Phone %>';
        var u_name = '<%= userInfo.TrueName %>';
        var u_avatar = '<%= userInfo.Avatar %>';
        var u_describe = '<%= ZentCloud.Common.StringHelper.GetReplaceStr(userInfo.Description) %>';
        var u_view_type = '<%= userInfo.ViewType %>';

        var u_ex3 = '';
        <%}%>
        <%}%>
    </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/UserCenter/UserCenter.js?v=20161123"></script>
</asp:Content>
