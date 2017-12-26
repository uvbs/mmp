<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MallScoreOnline.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MallScoreOnline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>积分商城</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery2.1.1.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>

      <style>
        .qianweinav
        {
           margin-top: 10px;
            width: 160px;
            height: 30px;
            margin: 10px auto 0 auto;
            background-color: #ff7928;
            border-radius: 5px;
        }
        .qianweinav span
        {
            width: 80px;
            height: 30px;
            box-sizing: border-box;
            float: left;
            display: block;
            line-height: 30px;
            text-align: center;
            border-radius: 5px;
        }
        .current
        {
             background-color: #EC5B03;
        }
        .qianweinav span a
        {
            text-decoration: none;
            color: #fff;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <section class="box">
   <div class="qianweinav">
     <span ><a href="ScoreMall.aspx">在线礼品</a></span>
     <span class="current" ><a href="MallScoreOnline.aspx">合作商家</a></span>
   </div>
    <ul class="mainlist score_mall" id="productList">
    </ul>
    <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
    </div>
</section>
</body>
<script type="text/javascript">

    $(function () {
        LoadScoreProductList();

    })
    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    //加载积分商品
    function LoadScoreProductList(categoryid) {

        $.ajax({
            type: 'post',
            url: mallHandlerUrl,
            data: { Action: 'GetOnLineType' },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                if (resp.ExObj == null) { return; }
                var listHtml = '';
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    var str = new StringBuilder();
                    str.AppendFormat('<li>');
                    str.AppendFormat('<div class="productbox" href="#">');
                    str.AppendFormat('<a href="WxMallScoreBelowLine.aspx?TypeId={0}" class="img">', resp.ExObj[i].AutoId);
                    str.AppendFormat('<img src="{0}">', resp.ExObj[i].TypeImg);
                    str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].TypeName);
                    str.AppendFormat('</a>');
                    str.AppendFormat('<a href="WxMallScoreBelowLine.aspx?TypeId={0}" class="btn_min orange">进入店铺</a>', resp.ExObj[i].AutoId);
                    str.AppendFormat('</div>');
                    str.AppendFormat('</li>');
                    listHtml += str.ToString();
                };
                if (listHtml != "") {
                    //填入列表
                    $('#productList').html(listHtml);

                }
                else {
                    $('#productList').html("暂时没有积分商品.");
                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");

                }
            }
        });

    }
</script>
</html>
