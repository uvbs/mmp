<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreRecord" %>
<!DOCTYPE html>
<html>
<head>
    <title>积分记录</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery2.1.1.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>

</head>
<body>
<section class="box">
    <div class="recordbox">

        <div class="recordlist">
            <div class="recordtitle toptitle">
               <%-- <span class="momth"></span>--%>
                <span class="incom">总收入：<label style="color:Red"><%=ScoreTotalIn %></label></span>
                <span class="expend">总支出:<label style="color:Red"><%=ScoreTotalOut%></label></span>
            </div>
           
        </div>

        <div class="recordlist" id="recordlist">


           
        </div>

    </div>
    <div class="backbar">
        <a href="ScoreManage.aspx" class="back"><span class="icon"></span></a>
        
    </div>
</section>
</body>
<script type="text/javascript">
    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    var PageIndex = 1;
    var PageSize = 100;

    $(function () {
        LoadScoreRecordList();
    })
    //加载记录
    function LoadScoreRecordList() {
        $.ajax({
            type: 'post',
            url: mallHandlerUrl,
            data: { Action: 'QueryScoreRecord', page: PageIndex, rows: PageSize },
            dataType: "json",
            success: function (resp) {
                if (resp.ExObj == null) { return; }
                var listHtml = '';
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    var str = new StringBuilder();
                    var type = resp.ExObj[i].Type;
                    var iconclass = "";
                    var typetitle = "";
                    if (type == 1) {
                        typetitle = "微商城";
                        iconclass = "icon scoremall";
                    }
                    else if (type == 2) {
                        typetitle = "积分商城";
                        iconclass = "icon vipscore";
                    }
                    else if (type == 3) {

                        typetitle = "积分赠送";
                        iconclass = "icon givescore";
                    }
                    str.AppendFormat('<div class="record">');
                    str.AppendFormat('<div class="iconbox">');
                    str.AppendFormat('<span class="mark purpleb"><span class="{0}"></span></span>', iconclass);
                    str.AppendFormat('<p class="title">{0}</p>', typetitle);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="concent">');
                    str.AppendFormat('<p class="info">{0}</p>', resp.ExObj[i].Remark);
                    str.AppendFormat('<p class="time">{0}</p>', resp.ExObj[i].InsertDateStr);
                    str.AppendFormat('</div>');
                    var score = resp.ExObj[i].Score;
                    if (score > 0) {
                        score = "+" + resp.ExObj[i].Score;
                    }
                    str.AppendFormat('<div class="recordprice">{0}</div>', score);
                    str.AppendFormat('</div>');
                    listHtml += str.ToString();
                };
                if (listHtml != "") {

                    //填入列表
                    $('#recordlist').html(listHtml);

                }
                else {
                    $('#recordlist').html("暂时没有记录");
                }


            }
        });



    }

</script>
</html>
