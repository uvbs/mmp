<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ArticleStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ArticleStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/plugins/zTree/css/demo.css" rel="stylesheet" type="text/css" />
    <link href="/plugins/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <style>
        body
        {
            padding: 0px;
            margin: 0;
            font-size: 10pt;
            padding-top: 0px;
        }
        a
        {
            color: #333;
            text-decoration: none;
        }
        a:hover
        {
            color: #FF0000;
            text-decoration: underline;
        }
        /*下面这个样式用来控制title属性显示的那个方框样式的*/
        DIV#qTip
        {
            border: #abab98 1px solid;
            display: none;
            font-size: 12px;
            z-index: 1000;
            background: #fefeda;
            color: #5f5f52;
            line-height: 16px;
            font-family: "Tahoma";
            position: absolute;
            text-align: center;
            padding: 4px;
            margin-top: -4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>转发统计</span> <a href="javascript:window.history.go(-1);" class="easyui-linkbutton"
        iconcls="icon-redo" plain="true" style="float: right; margin-right: 10px;">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div style="float: left">
        <ul id="tree" class="ztree">
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/plugins/zTree/js/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script type="text/javascript" src="/plugins/zTree/js/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript" src="/plugins/zTree/js/jquery.ztree.exedit-3.5.js"></script>
    <script type="text/javascript">
		<!--
        var setting = {
            async: {
                enable: true,
                url: getUrl
            },
            check: {
                enable: false
            },
            data: {
                key:{title:"tip"},
                simpleData: {
                    enable: true
                }
            },
            view: {
                expandSpeed: "",
                nameIsHTML: true
            },
            callback: {
                beforeExpand: beforeExpand,
                onAsyncSuccess: onAsyncSuccess,
                onAsyncError: onAsyncError
            }
        };

        var zNodes = [<%=RootNodes%>];
        var log, className = "dark",
		startTime = 0, endTime = 0, perCount = 100, perTime = 100;
        function getUrl(treeId, treeNode) {
            var curCount = (treeNode.children) ? treeNode.children.length : 0;
            var getCount = (curCount + perCount) > treeNode.count ? (treeNode.count - curCount) : perCount;
            var param = "id=" + treeNode.id+ "&Ex_SpreadUserID=" + treeNode.Ex_SpreadUserID+ "&Ex_ShareTimestamp=" + treeNode.Ex_ShareTimestamp+ "&articleId=" +"<%=articleId%>";
	    return "/Handler/App/CationHandler.ashx?Action=GetArticleSpreadTree&" + param;		
        }
        function beforeExpand(treeId, treeNode) {
            if (!treeNode.isAjaxing) {
                startTime = new Date();
                treeNode.times = 1;
                ajaxGetNodes(treeNode, "refresh");
                return true;
            } else {
                alert("zTree 正在下载数据中，请稍后展开节点。。。");
                return false;
            }
        }
        function onAsyncSuccess(event, treeId, treeNode, msg) {
            if (!msg || msg.length == 0) {
                return;
            }
             tooltip.init();
            var zTree = $.fn.zTree.getZTreeObj("tree"),
			totalCount = treeNode.count;
            if (treeNode.children.length < totalCount) {
                //setTimeout(function () { ajaxGetNodes(treeNode); }, perTime);
            } else {
           if (treeNode.isParent) {   
      var childrenNodes =treeNode.children;   
      if (childrenNodes) {   
          for (var i = 0; i < childrenNodes.length; i++) {   
               zTree.expandNode(childrenNodes[i], true, false, false);
          }   
      } 


}
                zTree.addNodes(treeNode,msg);  
                treeNode.icon = "";
                return;
                zTree.updateNode(treeNode);
                zTree.selectNode(treeNode.children[0]);
                endTime = new Date();
                var usedTime = (endTime.getTime() - startTime.getTime()) / 1000;
                className = (className === "dark" ? "" : "dark");
               
               
            }
        }
        function onAsyncError(event, treeId, treeNode, XMLHttpRequest, textStatus, errorThrown) {
            var zTree = $.fn.zTree.getZTreeObj("tree");
            alert("异步获取数据出现异常,请重新刷新页面");
            treeNode.icon = "";
            zTree.updateNode(treeNode);
        }
        function ajaxGetNodes(treeNode, reloadType) {
            var zTree = $.fn.zTree.getZTreeObj("tree");
            if (reloadType == "refresh") {
               // treeNode.icon = "../../../css/zTreeStyle/img/loading.gif";
                //zTree.updateNode(treeNode);
            }
           
           // zTree.reAsyncChildNodes(treeNode, reloadType, true);
        }
        function showLog(str) {
            if (!log) log = $("#log");
            log.append("<li class='" + className + "'>" + str + "</li>");
            if (log.children("li").length > 4) {
                log.get(0).removeChild(log.children("li")[0]);
            }
        }
        function getTime() {
            var now = new Date(),
			h = now.getHours(),
			m = now.getMinutes(),
			s = now.getSeconds(),
			ms = now.getMilliseconds();
            return (h + ":" + m + ":" + s + " " + ms);
        }
       

        $(document).ready(function () {
            $.fn.zTree.init($("#tree"), setting, zNodes);

        });
		//-->
	</script>
    <script type="text/javascript">

        //////////////////////////////////////////////////////////////////
        // qTip - CSS Tool Tips - by Craig Erskine
        // http://qrayg.com | http://solardreamstudios.com
        //
        // Inspired by code from Travis Beckham
        // http://www.squidfingers.com | http://www.podlob.com
        //////////////////////////////////////////////////////////////////
        var qTipTag = "a"; //Which tag do you want to qTip-ize? Keep it lowercase!//
        var qTipX = -30; //This is qTip's X offset//
        var qTipY = 25; //This is qTip's Y offset//
        //There's No need to edit anything below this line//
        tooltip = {
            name: "qTip",
            offsetX: qTipX,
            offsetY: qTipY,
            tip: null
        }
        tooltip.init = function () {
            var tipNameSpaceURI = "http://www.w3.org/1999/xhtml";
            if (!tipContainerID) { var tipContainerID = "qTip"; }
            var tipContainer = document.getElementById(tipContainerID);
            if (!tipContainer) {
                tipContainer = document.createElementNS ? document.createElementNS(tipNameSpaceURI, "div") : document.createElement("div");
                tipContainer.setAttribute("id", tipContainerID);
                document.getElementsByTagName("body").item(0).appendChild(tipContainer);
            }
            if (!document.getElementById) return;
            this.tip = document.getElementById(this.name);
            if (this.tip) document.onmousemove = function (evt) { tooltip.move(evt) };
            var a, sTitle;
            var anchors = document.getElementsByTagName(qTipTag);

            for (var i = 0; i < anchors.length; i++) {

                a = anchors[i];

                sTitle = a.getAttribute("title");

                if (sTitle) {
                    a.setAttribute("tiptitle", sTitle);
                    a.removeAttribute("title");
                    a.onmouseover = function () { tooltip.show(this.getAttribute('tiptitle')) };
                    a.onmouseout = function () { tooltip.hide() };
                }
            }
        }
        tooltip.move = function (evt) {
            var x = 0, y = 0;
            if (document.all) {//IE
                x = (document.documentElement && document.documentElement.scrollLeft) ? document.documentElement.scrollLeft : document.body.scrollLeft;
                y = (document.documentElement && document.documentElement.scrollTop) ? document.documentElement.scrollTop : document.body.scrollTop;
                x += window.event.clientX;
                y += window.event.clientY;

            } else {//Good Browsers
                x = evt.pageX;
                y = evt.pageY;
            }
            this.tip.style.left = (x + this.offsetX) + "px";
            this.tip.style.top = (y + this.offsetY) + "px";
        }
        tooltip.show = function (text) {
            if (!this.tip) return;
            this.tip.innerHTML = text;
            this.tip.style.display = "block";

        }
        tooltip.hide = function () {
            if (!this.tip) return;
            this.tip.innerHTML = "";
            this.tip.style.display = "none";

        }
        window.onload = function () {
            tooltip.init();
        }

    </script>
</asp:Content>
