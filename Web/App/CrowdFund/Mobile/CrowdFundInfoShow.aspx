<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrowdFundInfoShow.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.Mobile.CrowdFundInfoShow" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title><%=model.Title %></title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1">
    <link href="styles/button.css" rel="stylesheet" type="text/css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
   <style>
    .col-xs-8 {width:100%;}
    body{font-family: "Microsoft YaHei" ! important;font-size:14px;}
    .tagbox{margin-top:10px;}
    .ratio-wrap, .ratio-wrap span {
  height: 14px;
  display: block;
  line-height: 14px;
  font-size: 1px;
  border-radius: 7px;
  box-shadow: 0px 1px 1px rgba(0,0,0,0.2) inset;
}
.ratio-wrap {
  width: 100%;
  background: #dbdbdb;
  overflow: hidden;
  margin-top:5px;
  margin-bottom:5px;
}
.icon-progress {
  background-image: url(images/icon-progress.gif);
  background-repeat: repeat-x;
  background-size: 30px auto;
}
.text{font-size:14px;}
.col-xs-3{width:33.33%;}
.footerbar a{margin-top:10px;}
.divmore{text-align:center;margin-bottom:5px;}
#btnloadmore{margin-top:5px;}
table{width:100%;border-bottom:1px solid #ccc; }
.right{text-align:right;}
.left{text-align:left;}
.applypeoplelist .outerlistbox .listbox {
  height:auto;
  border-top:none;
}
hr{margin:0;}
.red{color:Red;}
.wlink:link, .wlink:visited, a:link, a:visited {
  color: White;
}
.button-small{padding:0;width:90%;}
    </style>
</head>
<body class="whitebg">
    <div class="wcontainer activetitle">
        <h1><%=model.Title %></h1>
        <div class="tagbox">
            <span class="wbtn_tag wbtn_red">
            阅读:<%=model.PV %>
            </span>
            <span class="wbtn_tag wbtn_orange">
            <label >分享:<%=model.ShareCount %></label></span>
           
        </div>
    </div>
    <div class="mainlist activelist activeinfomainlist">
        <div class="listbox">
            <div class="mainimg">
                <img src="<%=model.CoverImage %>"> 
            </div>

                <span class="baomingstatus">
                <span class="text">
                  <%if (model.Status.Equals(1))
                    {
                        Response.Write("进行中");
                    }
                    else
                    {
                        Response.Write("已结束");
                    }%>
                  
                </span>
                <svg class="sanjiao" version="1.1" viewbox="0 0 100 100">
				<polygon points="100,100 0.2,100 100,0.2" />
			</svg>
                </span>
            <div class="activeconcent">
                <div class="textbox">
                    <p>
                      <span class="text">
                       <%=string.IsNullOrEmpty(model.HaveFinancAmountText) ? "已筹资" : model.HaveFinancAmountText%>
                       ¥<%=model.TotalPayAmount%>
                         (<%=model.PayPercent %>%)
                        </span>
                         <span class="text" style="float:right;">
                          <%=string.IsNullOrEmpty(model.FinancAmountText) ? "目标" : model.FinancAmountText%>
                         ¥<%=model.FinancAmount %></span>
                         </p>
                      
                <div class="ratio-con">
	          <div class="ratio-wrap">
	            <span style="width:<%=model.PayPercent %>%;" class="ratio-red icon-progress"></span>
	          </div>
	        </div>
                      

                   
                    <p>
                       
                        <span class="text"><%=model.PayPersionCount%>人参与</span>
                         <span class="text" style="float:right;margin-right:25px;">剩余<%=model.RemainingDays %>天</span>
                    </p>
                </div>

            </div>
           
        </div>
        <!-- listbox -->
    </div>

    <!-- mainlist -->
    <div class="wcontainer articlebox">
       <%=model.Introduction %>
    </div>

    <div class="wcontainer bottom50 applypeoplelist">
        <div class="listtitle">
            <h3 class="wbtn_line_orange">
                
                <%=string.IsNullOrEmpty(model.JoinPersonnelText) ? "参与人员" : model.JoinPersonnelText%>
                </h3>
            <span class="wbtn_tag wbtn_orange"><span class="iconfont icon-36"></span>
            <label id="lblsignuptotalcountbottom"><%=model.PayPersionCount %></label>
            </span>
        </div>
        <div class="outerlistbox ">
            <div class="listbox loadmorebtn" id="append">
                
            </div>
            <div class="divmore"><span id="btnloadmore" class="wbtn wbtn_main" onclick="LoadRPInfo()">显示更多</span></div>
            
        </div>
    </div>
    <div class="footerbar">
        <div class="col-xs-3 ">
              <%if (model.Status.Equals(1)){ %>
             <a class="button button-primary button-rounded button-small" href="DoPayment.aspx?id=<%=model.AutoID %>">
              <%=string.IsNullOrEmpty(model.PaymentText) ? "付款" : model.PaymentText%>
             </a>
              <%} %>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-3">
        <a class="button button-primary button-rounded button-small" id="btnAdd"><%=string.IsNullOrEmpty(model.AddCrowdFundText) ? "发起" : model.AddCrowdFundText%></a>
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-3 ">
            <a class="button button-primary button-rounded button-small" id="btnShare"><%=string.IsNullOrEmpty(model.ShareText) ? "分享" : model.ShareText%></a>
        </div>
        <!-- /.col-lg-2 -->

    </div>
    <!-- footerbar -->
    <div class="wcontainer discusscontainer maxh100">
        <div class="modal fade " id="gnmdb" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body textcenter">
                        <p>
                        即将开放,敬请期待
                        </p>
                    </div>
                    <div class="modal-footer textcenter">
                        <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </div>

           <div style="width: 100%; height: 20000px; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 500; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute;z-index: 1000; right: 0; width: 100%;height:20000px; text-align: right;
            display: none;" id="sharebox">
            <img src="images/sharetip.png" width="100%" />
        </div>
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="/WuBuHui/js/jquery.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/WuBuHui/js/bootstrap.js"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">
    var PageIndex = 1;
    var PageSize = 10;
    $(function () {
       
        LoadRPInfo();
            $("#btnShare").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() });
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });
        $("#btnAdd").click(function () {
          $('#gnmdb').modal('show');
        });
       


    });
    function LoadRPInfo() {
        $.ajax({
            type: 'post',
            url: "MobileHandler.ashx",
            data: { Action: "QueryPayPersion", PageIndex: PageIndex,PageSize:PageSize, CrowdFundID: <%=model.AutoID %> },
            dataType: 'json',
            success: function (resp) {
                if (resp.length == 0 && PageIndex == 1) {
                    $("#btnloadmore").remove();
                    $("#append").html("暂无记录");
                }
                PageIndex++;
                var html=new StringBuilder();
                    if (resp.length == 0) {
                        $("#btnloadmore").html("没有更多");
                        $("#btnloadmore").removeAttr("onclick");
                        return;
                    }
                    $.each(resp, function (index, item) {
////                       html.AppendFormat('<div class="listbox">');

////                        html.AppendFormat('<table>');
////                        html.AppendFormat('<tr>');
////                        html.AppendFormat('<td >');
////                       html.AppendFormat('<span class="wbtn_round">');
////                       html.AppendFormat('<img src="http://dev.comeoncloud.net{0}">',item.HeadIimg);
////                       html.AppendFormat('</span>');
////                        html.AppendFormat('</td>');
////                        html.AppendFormat('<td class="left">');
////                        html.AppendFormat('{0}',item.ShowName);
////                        html.AppendFormat('</td>');
////                        html.AppendFormat('<td class="right">');
////                        html.AppendFormat('付款:<span class="red">{0}</span>元',item.Amount);
////                        html.AppendFormat('</td>');
////                        html.AppendFormat('</tr>');
////                        html.AppendFormat('<tr>');
////                        html.AppendFormat('<td>');
////                        html.AppendFormat('</td>');
////                        html.AppendFormat('<td class="left" style="width:50%;">');
////                        html.AppendFormat('{0}',item.Review);
////                        html.AppendFormat('</td>');
////                        html.AppendFormat('<td class="right">');
////                        html.AppendFormat('{0}',item.InsertDate);

////                        html.AppendFormat('</td>');
////                        html.AppendFormat('</tr>');
////                         html.AppendFormat('</table>');
////                          html.AppendFormat('<hr/>');
////                       html.AppendFormat('</div>');

                       //
                        html.AppendFormat('<div class="listbox">');
                        html.AppendFormat('<table style="float:left;">');
                        html.AppendFormat('<tr>');
                        html.AppendFormat('<td rowspan="2">');
                        //头像
                       
                        html.AppendFormat('<span class="wbtn_round">');
                       html.AppendFormat('<img src="http://dev.comeoncloud.net{0}">',item.HeadIimg);
                       html.AppendFormat('</span>');
                        //
                        html.AppendFormat('</td>');
                        html.AppendFormat('<td class="left">');
                        html.AppendFormat('{0}',item.ShowName);
                        html.AppendFormat('</td>');
                        html.AppendFormat('<td class="right">');
                        html.AppendFormat('付款:<span class="red">{0}</span>元',item.Amount);
                        html.AppendFormat('</td>');
                        html.AppendFormat('</tr>');
                        html.AppendFormat('<tr>');
//                        html.AppendFormat('<td>');
//                        html.AppendFormat('</td>');
                        html.AppendFormat('<td class="left" style="width:50%;">');
                        html.AppendFormat('{0}',item.Review);
                        html.AppendFormat('</td>');
                        html.AppendFormat('<td class="right">');
                        html.AppendFormat('{0}',item.InsertDate);

                        html.AppendFormat('</td>');
                        html.AppendFormat('</tr>');
                         html.AppendFormat('</table>');
                         
                       html.AppendFormat('</div>');
                       //
                      
                    });
                    $("#append").append(html.ToString());

                }
                
            
        });
    };
    
   


</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=model.Title %>",
            desc: "<%=model.Title %>",
            imgUrl: "http://<%=Request.Url.Host%><%=model.CoverImage%>"
        }
            , {
                message_s: function () {
                    WXShareComlete();
                },
                message_c: function () {
                    WXShareComlete();
                },
                timeline_s: function () {
                    WXShareComlete();
                },
                timeline_c: function () {
                    WXShareComlete();
                }
            }
    )
    })
  function WXShareComlete() { 
  
   $.ajax({
            type: 'post',
            url: "MobileHandler.ashx",
            data: { Action: "WXShareComlete",id: <%=model.AutoID %> },
            dataType: 'json',
            success: function (resp) {

                   

                }
                
            
        });
  
  }
</script>
</html>