<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketNews.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.News.MarketNews" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>市场新闻</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://cdn.bootcss.com/bootstrap/3.2.0/css/bootstrap.min.css">
    <link href="../css/wubu.css" rel="stylesheet" type="text/css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script>            var IsHaveUnReadMessage = "<%=IsHaveUnReadMessage%>"; </script>
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_greenyellow">
            <span class="iconfont icon-78"></span>市场新闻
        </div>
        <ul class="catlist">
            <li class="catli current" categoryid="<%=RootCategoryId%>"><a href="javascript:void(0)">
                全部</a></li>
            <%=sbCategory%>
        </ul>
    </div>
    <div class="wtopbar">
        <div class="col-xs-2">
            <span class="wbtn wbtn_line_greenyellow" id="categorybtn"><span class="iconfont icon-fenlei bigicon">
            </span></span>
        </div>
        <div class="col-xs-10">
            <span class="wbtn wbtn_main"><span class="iconfont icon-111" id="btnSearch"></span>
            </span>
            <input type="text" class="searchtext" id="txtName">
        </div>
    </div>
    <div class="mainlist top50 bottom50" id="needload">
        <div class="tagbar">
        
            <a href="#" class="wbtn wbtn_greenyellow" id="newslistbtn"><span class="iconfont icon-78">
            </span><span class="title">市场新闻</span> </a><a href="MarketInterPreted.aspx"
                class="wbtn " id="discusslistbtn"><span class="iconfont icon-78"></span><span class="title">
                    市场解读</span> </a>
        </div>
        <div id="objList">
        </div>
         <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <!-- mainlist -->
       <!-- footerbar -->
<script type="text/javascript" src="../js/footer.js"></script>
    <!-- footerbar -->
     <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog" aria-hidden="true">
       
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        </p>
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

</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="../js/behindbar.js" type="text/javascript"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>

<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">
     var handlerUrl = "/Handler/App/WXWuBuHuiUserHandler.ashx";
     var PageIndex = 1; //第几页
     var PageSize =10; //每页显示条数
     var RootCategoryID = "<%=MarketNewsIds%>"; //总分类
     var CategoryId=RootCategoryID;
     $(function () {
         LoadList();
         $("#btnSearch").click(function () {
             PageIndex = 1;
             LoadList();

         })

         $("[categoryid]").click(function () {
             $(this).siblings().removeClass("current");
             $(this).addClass("current");
             CategoryId = $(this).attr("categoryid");
             Reset();
             PageIndex = 1;
             LoadList();

         })
     });

     function BtnClick() {

         PageIndex++;
         LoadList();

     }




     //加载列表分页
     function LoadList() {
         $.ajax({
             type: 'post',
             url: handlerUrl,
             data: { Action: 'GetNewsList', CategoryId: CategoryId, ArticleName: $("#txtName").val(), PageIndex: PageIndex, PageSize: PageSize },
             timeout: 60000,
             dataType: "json",
             success: function (resp) {
                
                 if (resp.ExObj == null) {  $(".loadnote").text(" ");return; }
                 var listHtml = '';
                 var str = new StringBuilder();
                 for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    str.AppendFormat('<a href="NewsDetail.aspx?id={0}" class="listbox">', resp.ExObj[i].JuActivityID);
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3 class="shorttitle">{0}</h3>', resp.ExObj[i].ActivityName);
                    str.AppendFormat('<p>{0}</p>', resp.ExObj[i].Summary);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="tagbox">');
                    str.AppendFormat('<span class="wbtn_tag wbtn_red">');
                    str.AppendFormat('<span class="iconfont icon-eye"></span>{0}', resp.ExObj[i].PV);
                    str.AppendFormat('</span>');
                    str.AppendFormat('<span class="wbtn_tag wbtn_main">');
                    str.AppendFormat(resp.ExObj[i].CategoryName);
                    str.AppendFormat('</span>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_greenyellow timetag">');
                    str.AppendFormat(FormatDate(resp.ExObj[i].LastUpdateDate));
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wbtn_fly wbtn_flytr wbtn_main ">');
                    str.AppendFormat('新闻');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>'); 

                 };
                 if (PageIndex == 1) {
                     if (resp.ExStr == "1") {
                         //显示下一页按钮
                         str.AppendFormat('<button id="btnNext" type="button" class="dismore" style="display:none;"></button>');
                         listHtml += str.ToString();
                         $("#objList").html(listHtml);

                     }
                     else {
                         listHtml += str.ToString();
                         if (listHtml == "") {
                             listHtml = "没有符合条件的新闻";
                         }
                         $("#objList").html(listHtml);

                     }



                 }
                 else {
                     listHtml += str.ToString();
                     if (listHtml != "") {
                         $("#btnNext").before(listHtml);
                     }
                     else {

                        // $("#btnNext").html("没有更多了");
                         //$("#btnNext").removeAttr("onclick");
                         $(".loadnote").text("没有更多");
                     }

                 }
                  //$(".loadnote").text("　");
                  

             },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                 if (textStatus == "timeout") {
                     $('#alertconcent').text("加载超时，请刷新页面");
                     $('#alertbox').modal('show');
                 }
                 
             },
             complete:function(){
                 $("#needload").bottomLoad(function () {
                  $(".loadnote").text("正在加载...");
                   PageIndex++;
                   LoadList();
                })
             }
         });




     }

     function FormatDate(value) {

         if (value == null || value == "") {
             return "";
         }
         var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
         var month = padLeft(date.getMonth() + 1, 10);
         var currentDate = padLeft(date.getDate(), 10);
         var hour = padLeft(date.getHours(), 10);
         var minute = padLeft(date.getMinutes(), 10);
         var second = padLeft(date.getSeconds(), 10);
         return date.getFullYear() + "-" + month + "-" + currentDate;
     }


     function padLeft(str, min) {
         if (str >= min)
             return str;
         else
             return "0" + str;
     }


</script>
<script>
    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");
    function Reset() {

        //复位动画禁点击
        $(window).bind("click", function (e) {
            e.preventDefault();
        })
        //动画完 取消禁点击
        $(arr[0])[0].addEventListener("webkitTransitionEnd", cc)

        function cc() {
            $(arr[0])[0].removeEventListener("webkitTransitionEnd", cc)
            $(".behindbar").hide();
            setTimeout(function () {
                $(window).unbind("click");
            }, 500)
        }

        //复位动画
        for (var i in arr) {
            $(arr[i]).removeClass("sdiebartranslate")
        }
        $(".sidebarhidebtn").hide();


    }
</script>
</html>
