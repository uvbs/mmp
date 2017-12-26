<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master"
    AutoEventWireup="true" CodeBehind="MyMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.MyMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我的会员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #divNext
        {
            text-align: center;
            margin-top: 10px;
            margin-bottom: 50px;
        }
        /*.cashlistbox .cashlist, .viplistbox .cashlist, .cashlistbox .viplista, .viplistbox .viplista
        {
            position: relative;
            display: block;
            background-color: #fff;
            margin-top: 10px;
            padding: 10px;
            text-decoration: none;
            width: 100%;
            height: auto;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="topbar">
        <span class="col-xs-12 categorylink">
            <%
                switch (Request["level"])
                {
                    case "1":
                       // Response.Write("一");
                        break;
                    case "2":
                        //Response.Write("二");
                        break;
                    case "3":
                       // Response.Write("三");
                        break;
                    default:
                        break;
                }
                ZentCloud.BLLJIMP.BLLDistribution bllDis = new ZentCloud.BLLJIMP.BLLDistribution();
                int level = bllDis.GetDistributionRateLevel();
                if (int.Parse(Request["level"]) > level)
                {
                    Response.Write(string.Format("只能查看{0}级会员", level));
                    Response.End();
                }
            %>
            会员<label id="lblcount">0</label>人</span>
    </div>
    <div class="viplistbox bottom50 top50">
        <div id="divNext">
            <button class="btn_main" id="btnNext" onclick="BtnNext()" style="display: none;">
                查看更多</button>
        </div>
    </div>
    <div class="backbar">
        <a class="col-xs-2" href="javascript:history.go(-1);">
            <svg class="icon colorDDD" aria-hidden="true">
                <use xlink:href="#icon-fanhui"></use>
            </svg>

        </a>
        <div class="col-xs-8">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var pageIndex = 1;//页码
        var pageSize = 10;//页数
        $(function () {

            LoadData();


        })

        //
        //加载数据
        function LoadData() {
            var ajaxData = {
                Action: 'QueryMyMember',
                Level: GetParm("level"),
                IsPay: GetParm("ispay"),
                pageIndex: pageIndex,
                pageSize: pageSize,
                AutoID:'<%=Request["AutoID"]==null?"0":Request["AutoID"] %>'
            }
            $("#btnNext").text("加载中...");
            $.ajax({
                type: 'post',
                url: handerurl,
                data: ajaxData,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    $("#btnNext").text("查看更多");
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        str.AppendFormat('<a href="memberdetail.aspx?id={0}" class="viplista">', resp.ExObj[i].AutoID);
                        str.AppendFormat('<img class="avatar" src="{0}">', resp.ExObj[i].WXHeadimgurlLocal);
                        if (resp.ExObj[i].WXNickname) {
                            str.AppendFormat('<span class="vipname">{0}</span>', resp.ExObj[i].WXNickname);
                        } else {
                            str.AppendFormat('<span class="vipname">微信用户</span>');
                        }
                        
                        //str.AppendFormat('<span class="vipname">姓名：{0}</span>', resp.ExObj[i].TrueName);
                        //str.AppendFormat('<span class="vipname">手机：{0}</span>', resp.ExObj[i].Phone);
                        //str.AppendFormat('<span class="viptime">加入时间:{0}</span>', FormatDate(resp.ExObj[i].Regtime));
                        str.AppendFormat('</a>');

                    };
                    if (pageIndex == 1) {
                        if (resp.ExObj.length == 0) {
                            $("#divNext").before("没有数据");
                            $("#divNext").remove();
                            return;

                        }
                        else {
                            $("#btnNext").show();
                        }


                    }
                    else {

                        if (resp.ExObj.length == 0) {
                            $("#btnNext").text("没有更多");
                            $("#btnNext").removeAttr("onclick");

                        }

                    }
                    $("#divNext").before(str.ToString());
                    $("#lblcount").text(resp.ExInt);

                },
                complete: function () { },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时");

                    }

                }
            });
        }

        //下一页
        function BtnNext() {
            pageIndex++;
            LoadData();

        }
    </script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
