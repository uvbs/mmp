<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreManage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <title>积分管理</title>
   
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery2.1.1.js" type="text/javascript"></script>

</head>
<body>
<section class="box">
    <div class="m_header">
        <div class="scoretext">我的积分:
            <span class="scorenum"><%=currentUserInfo.TotalScore%></span>
        </div>
        <a href="ScoreRule.aspx" class="scorerule btn orange">积分规则</a>
    </div>
    <div class="m_listbox">
        <a href="ScoreMall.aspx" class="list">
            <span class="mark purpler"><span class="icon scoremall"></span></span>
            <h2>积分商城</h2>
            <span class="righticon"></span>
        </a>
        <a href="MyScoreOrderList.aspx" class="list">
            <span class="mark purpleb"><span class="icon order"></span></span>
            <h2>积分订单</h2>
            <span class="righticon"></span>
        </a>
        <a href="ScoreRecord.aspx" class="list">
            <span class="mark green"><span class="icon scorelist"></span></span>
            <h2>积分记录</h2>
            <span class="righticon"></span>
        </a>
    </div>

    <div class="m_listbox">
        <a href="ScoreDonate.aspx" class="list">
            <span class="mark pink"><span class="icon givescore"></span></span>
            <h2>积分赠送</h2>
            <span class="righticon"></span>
        </a>
    </div>

    <div class="backbar">
        <a href="MyCenter.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</body>

</html>
