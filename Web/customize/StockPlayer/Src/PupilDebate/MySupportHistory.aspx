<%@ Page Title="我的支持历史" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="MySupportHistory.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.PupilDebate.MySupportHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/customize/StockPlayer/Src/PupilDebate/MySupportHistory.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <%if (curUser == null)
      {
          %>
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
                
           <%
      } else{
      
            %>
                <div class="notice">
                    <div class="notice-head">
                        我的支持历史<span class="historyTotal">0</span>
                    </div>
                    <div class="list-notice">
                        <div class="notice-item" style="display:none;">
                            <div class="head"><div class="time"></div></div>
                            <div class="content">
                            </div>
                            <div class="action">
                            </div>
                        </div>
                    </div>
                    <div class="bottom-pager">
                        <div class="pagination pager1"></div>
                    </div>
                </div>
                
              
            <%
      }%>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
     <script type="text/javascript">
         var author = '<%=curUser!=null?curUser.AutoID:0%>';
     </script>
     <script type="text/javascript" src="/customize/StockPlayer/Src/PupilDebate/MySupportHistory.js"></script>
</asp:Content>
