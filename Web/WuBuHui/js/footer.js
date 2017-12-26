document.write("<div class=\"footerbar\">")
document.write("	<div class=\"col-xs-3\" id=\"divindex\">")
document.write("		<a  class=\"wbtn wbtn_line_main\" type=\"button\" href=\"/Wubuhui/MyCenter/Index.aspx\"><span class=\"iconfont icon-b11\">")
document.write("		</span><span class=\"navtext\">首页</span> </a>")
document.write("	</div>")

document.write("	<div id=\"divmynews\" class=\"col-xs-3 \">")
document.write("		<a class=\"wbtn wbtn_line_main\" type=\"button\"  href=\"/WuBuHui/News/MarketNews.aspx\">")
document.write("			<span class=\"iconfont icon-b14\"></span><span class=\"navtext\">资讯</span> </a>")
document.write("	</div>")

document.write("	<div id=\"divmymessage\" class=\"col-xs-3 \">")

if (typeof (IsHaveUnReadMessage) == "undefined") {

}
else {

    if (IsHaveUnReadMessage.toLowerCase() == "true") {
        document.write("<span class=\"redpoint\"></span>")

    }

}

document.write("		<a class=\"wbtn wbtn_line_main\" type=\"button\" href=\"/wubuhui/mycenter/systemmessagebox.aspx\"><span")
document.write("			class=\"iconfont icon-b51\"></span><span class=\"navtext\">消息</span> </a>")
document.write("	</div>")




document.write("	<div id=\"divmycenter\" class=\"col-xs-3 \">")
document.write("		<a class=\"wbtn wbtn_line_main\" type=\"button\" href=\"/WuBuHui/MyCenter/MyCenter.aspx\"><span class=\"iconfont icon-b24\">")
document.write("		</span><span class=\"navtext\">我的</span> </a>")
document.write("	</div>")
document.write("</div>")
//给当前页面加样式
var pathName = window.location.pathname.toLowerCase();
switch (pathName) {
    case "/wubuhui/news/marketnews.aspx":
        document.getElementById("divmynews").className = 'col-xs-3 current';
        break;
    case "/wubuhui/news/marketinterpreted.aspx":
        document.getElementById("divmynews").className = 'col-xs-3 current';
        break;
    case "/wubuhui/mycenter/systemmessagebox.aspx":
        document.getElementById("divmymessage").className = 'col-xs-3 current';
        break;
    case "/wubuhui/member/scoretop.aspx":
        document.getElementById("divmycenter").className = 'col-xs-3 current';
        break;
    case "/wubuhui/member/tscoretop.aspx":
        document.getElementById("divmycenter").className = 'col-xs-3 current';
        break;
    case "/wubuhui/mycenter/mycenter.aspx":
        document.getElementById("divmycenter").className = 'col-xs-3 current';
        break;

    default:
        document.getElementById("divindex").className = 'col-xs-3 current';
        break;
}
//给当前页面加样式









