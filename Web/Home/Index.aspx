<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>至云客户关系营销平台</title>
    <meta property="wb:webmaster" content="5c9c029bcbf98d9c" />
    <link rel="stylesheet" href="style.css" />
    <script src="http://tjs.sjs.sinajs.cn/open/api/js/wb.js?appkey=3140228481" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript" src="script/jquery.js"></script>
    <script type="text/javascript" src="script/commonscript.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".textinput").find("input").focus(function () {
                $(".inputfocus").removeClass("pwwrong").removeClass("inputfocus")
                $(this).parents(".textinput").addClass("inputfocus");
            })
            $(".textinput").find("input").focusout(function () {
                $(this).parents(".inputfocus").removeClass("pwwrong").removeClass("inputfocus");
            });

            /*密码错误*/
            //pwwrong();

            /*账号错误*/
            //idwrong();
        });
    </script>
    <!--[if lt IE 9]>
<style type="text/css">
.textinput input{line-height:43px;}
</style>
<![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div class="mainbox">
        <div class="loginbox">
            <div class="header">
            </div>
            <div class="concent">
                <div class="textinput">
                    <input type="text" id="account" name="account" runat="server" />
                    <label class="texticon" for="account">
                    </label>
                    <div class="inputleft">
                    </div>
                    <div class="inputright">
                    </div>
                </div>
                <div class="textinput">
                    <input type="password" id="password" name="password" runat="server" />
                    <label class="pwicon" for="password">
                    </label>
                    <div class="inputleft">
                    </div>
                    <div class="inputright">
                    </div>
                </div>
                <div class="textinput verificationbox">
                    <div class="verification">
                        <input type="text" name="verification" />
                        <div class="inputleft">
                        </div>
                        <div class="inputright">
                        </div>
                    </div>
                    <div class="verificationimg">
                    </div>
                </div>
                <div class="btnbox">
                    <%-- <a href="javascript:void(0);" class="loginbtn" onclick='$("#login").submit()'>登录</a>
                    <a href="javascript:void(0);" class="signinbtn">申请试用</a>--%>
                    <asp:LinkButton ID="lbtnogin" CssClass="loginbtn" runat="server" OnClick="lbtnogin_Click">登录</asp:LinkButton>
                    <asp:LinkButton ID="lbtnReg" CssClass="regbtn" Visible="true" runat="server" OnClick="lbtnReg_Click"
                        Font-Underline="True">在线注册</asp:LinkButton>
                    
                </div>
                <div class="prompt">
                    感谢您使用至云客户关系营销平台</div>
            </div>
        </div>
    </div>
    <div class="copyright">
        <a href="http://www.miibeian.gov.cn">备案号:沪ICP备13000474号-1</a>&nbsp;&nbsp;&nbsp; Copyright
        © <a href="http://www.comeoncloud.com/">上海至云信息科技有限公司</a>
    </div>
    <div class="mainbg">
        <img src="images/mainbg.png" onload="mainbgshow()" /></div>
    <script type="text/javascript">
        $(".concent").append('<div class="leftbg"></div><div class="rightbg"></div><div class="bottombg"></div>')
        $(".loginbox").append('<div class="leftbottombg"></div><div class="rightbottombg"></div>')
    </script>
    </form>
</body>
</html>
