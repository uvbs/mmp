<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXPositionInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Position.WXPositionInfo" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta name="format-detection" content="telephone=no" />
    <title>职位详情</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body class="whitebg">
    <div class="wcontainer optiontitle">
        <h1>
            <asp:Literal ID="txtTitle" runat="server" /></h1>
        <p class="salarybox">
            <%=pInfo.Company%>
        </p>
        <p class="companyname">
            <asp:Literal ID="txtPersonal" runat="server" /></p>
        <span class="companylogo">
            <img src="<%=pInfo.IocnImg%>" alt="" id='ImgLogo'>
        </span>
    </div>
    <div class="wcontainer companyinfo">
        <p class="infolist">
            <span class="title">发布时间：</span><span class="concent"><asp:Literal ID="txtTime" runat="server" /></span>
        </p>
        <p class="infolist">
            <span class="title">工作地点：</span><span class="concent"><asp:Literal ID="txtAddress"
                runat="server" /></span>
        </p>
        <p class="infolist">
            <span class="title">企业规模：</span><span class="concent"><asp:Literal ID="txtEnterpriseScale"
                runat="server" /></span>
        </p>
    </div>
    <div class="wcontainer applyposition" id="applyposition">
        <div class="applybox">
            <h4>
                我的技能</h4>
            <%for (int i = 0; i < ProfessionalList.Count; i++)
              {
                  Response.Write(string.Format("<input class=\"checkbox\" type=\"checkbox\" id=\"cbprofessional{0}\" name=\"cbprofessional\" value=\"{1}\">", i, ProfessionalList[i].CategoryName));
                  Response.Write(string.Format("<label for=\"cbprofessional{0}\" class=\"discusstag\">", i));
                  Response.Write(string.Format("<span class=\"wbtn wbtn_gary\"><span class=\"iconfont\"></span></span>{0}", ProfessionalList[i].CategoryName));
                  Response.Write("</label>");
              } %>
            <div class="clearfix">
            </div>
            <h4>
                所在行业</h4>
            <%for (int i = 0; i < TradeList.Count; i++)
              {
                  Response.Write(string.Format("<input class=\"checkbox\" type=\"checkbox\" id=\"cbtrade{0}\" name=\"cbtrade\" value=\"{1}\">", i, TradeList[i].CategoryName));
                  Response.Write(string.Format("<label for=\"cbtrade{0}\" class=\"discusstag\">", i));
                  Response.Write(string.Format("<span class=\"wbtn wbtn_gary\"><span class=\"iconfont\"></span></span>{0}", TradeList[i].CategoryName));
                  Response.Write("</label>");
              } %>
        </div>
        <div class="input-group infosubmitbtn">
            <span class="wbtn wbtn_main"><span class="text" onclick="ApplyPosition()" id="btnSubmit">
                申请职位</span> </span>
        </div>
    </div>
    <div class="wcontainer articlebox bottom50">
        <!-- 这点内容是文章内容 需要删除  start -->
        <asp:Literal Text="text" ID="txtContent" runat="server" />
        <!-- 这点内容是文章内容 需要删除  end -->
        <div class="sharebtn">
            <span class="wbtn wbtn_main weixinsharezhidao">分享给好友</span> <span class="wbtn wbtn_main weixinsharezhidao">
                分享到朋友圈</span>
        </div>
    </div>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="WXPositionList.aspx"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <%if (!IsApply)
              {%>
            <a href="#" id="applythisposition" class="wbtn wbtn_line_main"><span class="iconfont icon-39 smallicon">
            </span>立即申请 </a>
            <% } %>
            <%else
                {%>
            <a href="#" id="a1" class="wbtn wbtn_line_main"><span class="iconfont icon-39 smallicon">
            </span>已经申请 </a>
            <% } %>
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        提交成功</p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
    <div class="modal fade bs-example-modal-sm" id="gnmdbReg" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                    </p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal" onclick="gotomp()">立即注册</span>
                    <span class="wbtn wbtn_main" data-dismiss="modal">继续浏览</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- footerbar -->
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="../js/jquery.js" type="text/javascript"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.js" type="text/javascript"></script>
    <script src="/WuBuHui/js/gotopageanywhere.js"></script>
    <script src="/WuBuHui/js/weixinsharebtn.js"></script>
    <!--<script src="/WuBuHui/js/positioninfo.js"></script>-->
    <script type="text/javascript">

        function SaveJiFen() {
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
                data: { Action: "SaveSharePosition", Id: "<%=pInfo.AutoId%>", wxsharetype: "0" },
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 1) {
                        $('#gnmdb').find("p").text(resp.Msg);
                        $('#gnmdb').modal('show');
                    }
                    else {
                        //$('#gnmdb').find("p").text(resp.Msg);
                        //$('#gnmdb').modal('show');
                    }
                    $(".weixinshareshade").hide();
                }
            });
        };

        function SaveJiFenTimeLine() {
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
                data: { Action: "SaveSharePosition", Id: "<%=pInfo.AutoId%>", wxsharetype: "1" },
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 1) {

                    }
                    else {

                    }
                    $(".weixinshareshade").hide();
                }
            });
        };



    </script>
    <script type="text/javascript">
      var IsSumbit = false;
      var handlerUrl = "/Handler/App/WXWuBuHuiPosintionHandler.ashx";
      function ApplyPosition() {
      var Trade =[];
      var Professional= [];
      $("input[name='cbprofessional']:checked").each(function () { 
      Professional.push($(this).val());
      })
      $("input[name='cbtrade']:checked").each(function () { 
      Trade.push($(this).val());
      })
      if (Trade.length==0) {
       $('#gnmdb').find("p").text("请选择行业");
       $('#gnmdb').modal('show');
       return false;
      }
     if (Professional.length==0) {
       $('#gnmdb').find("p").text("请选择专业");
       $('#gnmdb').modal('show');
       return false;
      }
      if (IsSumbit) 
       {
        return false;
      }
       IsSumbit = true;
       $("#btnSubmit").text("正在提交...");
          $.ajax({
              type: 'post',
              url: handlerUrl,
              data: { Action: "SavaApplyPositionInfo", Trade: Trade.join(','), Professional: Professional.join(','),AutoId:<%=AutoId%>},
              dataType: "json",
              success: function (resp) {
                  if (resp.Status == 1) {
                     
                      $('#gnmdb').find("p").text("申请成功!");
                      $('#gnmdb').modal('show');
                      setTimeout("window.location.href=\"WXPositionList.aspx\";",2000);
                     
                  }
                  else {
                      $('#gnmdb').find("p").text(resp.Msg);
                      $('#gnmdb').modal('show');
                     
                  }
              },
              complete:function(){
                     IsSumbit = false;
                     $("#btnSubmit").text("申请职位");
              
              }
          });
        
        
        }
    
    $(".weixinsharezhidao").weixinsharebtn();
    $("#applythisposition").bind("touchstart",function(e){

    e.preventDefault()
    var isRegUser = "<%=isUserRegistered %>";
        if (isRegUser == "False") {

          $('#gnmdbReg').find("p").text("您还没有注册五步会，立即注册获得25积分和更多功能！");
          $('#gnmdbReg').modal('show');
          return;

        }


	gotopageanywhere('#applyposition',function(){	
		if(!$("#applyposition").attr("style")){
			var applyheight=$(".applybox").height()+60
			$("#applyposition").css({"height":applyheight})
		}
	})
})

 function gotomp() {
        window.location.href = "../MyCenter/MyCenter.aspx";

    }
    </script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        var lineLink = "http://" + window.location.host + "/WuBuHui/Position/WXPositionInfo.aspx?id=" + '<%=AutoId %>';
        var descContent = "<%=pInfo.Company%>\n<%=pInfo.Address%>\n<%=pInfo.InsertDate.ToString()%>";
        var shareTitle ="<%= pInfo.Title%>";
        var imgUrl =  "<%=pInfo.IocnImg%>";
        var wxconfig = $.parseJSON('<%=new ZentCloud.BLLJIMP.BLLWeixin("").GetJSAPIConfig("")%>');
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: wxconfig.appId, // 必填，公众号的唯一标识
            timestamp: wxconfig.timestamp, // 必填，生成签名的时间戳
            nonceStr: wxconfig.nonceStr, // 必填，生成签名的随机串
            signature: wxconfig.signature, // 必填，签名，见附录1
            jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });
        wx.ready(function () {

            // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            wx.onMenuShareTimeline({
                title: shareTitle, // 分享标题
                link: lineLink, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    SaveJiFenTimeLine();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });
            wx.onMenuShareAppMessage({
                title: shareTitle, // 分享标题
                desc: descContent, // 分享描述
                link: lineLink, // 分享链接
                imgUrl: imgUrl, // 分享图标
                type: '', // 分享类型,music、video或link，不填默认为link
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                    // 用户确认分享后执行的回调函数
                    SaveJiFen();
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });


        });

        wx.error(function (res) {
            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
            //alert(res.errMsg);
        });

        //
    </script>

</body>
</html>
