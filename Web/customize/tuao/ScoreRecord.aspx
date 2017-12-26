<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="ScoreRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.ScoreRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    积分记录
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style>
        .divnext
        {
            text-align: center;
        }
        .recordlist
        {
            text-align: center;
        }
        .recordbox
        {
            padding-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="recordbox">



        <div class="recordlist" id="recordlist">


           
        </div>
        <div class="divnext">
        <a id="btnNext" class="btn orange">显示更多</a>
        </div>

    </div>

</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
        var PageIndex = 1;
        var PageSize = 10;

        $(function () {
            LoadData();

            $("#btnNext").click(function () {

                PageIndex++;
                LoadData();


            })

        })
        //加载记录
        function LoadData() {
            $.ajax({
                type: 'post',
                url: mallHandlerUrl,
                data: { Action: 'QueryScoreRecord', page: PageIndex, rows: PageSize },
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var listHtml = '';
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        var type = resp.ExObj[i].Type;
                        var iconclass = "";
                        var typetitle = "";
                        typetitle = "微商城";
                        iconclass = "icon scoremall";
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

                    };
                    listHtml = str.ToString();
                    if (listHtml != "") {
                        //填入列表
                        $('#recordlist').append(listHtml);

                    }
                    else {
                        if (PageIndex == 1) {
                            $('#recordlist').html("没有记录");
                            $('#btnNext').remove();
                        }
                        $('#btnNext').html("没有更多");

                    }


                }
            });



        }


    </script>
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
