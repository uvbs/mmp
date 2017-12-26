<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vote.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Vote" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link rel="stylesheet" href="/css/vote/style.css?v=0.0.1"/>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/vote/voteanimate.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <style>
    .searchbox {
    display: block;
    width: 100%;
    height: 40px;
    padding: 0 10px;
    box-sizing: border-box;
    position: relative;
    margin: 10px 0px
}

.searchbox input[type=text] {
    display: block;
    width: 100%;
    height: 40px;
    border-radius: 40px;
    font-size: 18px;
    border: 1px solid #E4E4E4;
    box-sizing: border-box;
    padding-left: 10px
}

.searchbox .searchbtn {
    display: block;
    width: auto;
    height: 34px;
    line-height: 34px;
    border-radius: 34px;
    position: absolute;
    right: 13px;
    top: 3px;
    background-color: #7daae6;
    border: none;
    padding: 0 10px 0 30px;
    color: #fff;
    font-size: 16px;
    background-image: url(../images/searchicon.png);
    background-size: 34px 34px;
    background-position: left center;
    background-repeat: no-repeat
}
.votetitle {padding: 50px 15px 0 14px;}

    
    </style>
</head>
<body>
<section class="box">
   
    <%if (!string.IsNullOrEmpty(VoteInfo.Logo))
      {%>
           <div>
          <style type="text/css">.total{position:relative;}</style>
          <img src="<%=VoteInfo.Logo%>" style="width:100%;max-height:150px;" />
          </div>
     <% } %>
    
    
    <%if (VoteInfo.IsFree.Equals(0)){ %>
      
      <div class="total votenumbar">
      <%} %>
      <%else{ %>
       <div class="total votenumbar votenopay">
       <%} %>
       <%-- <span class="votenumtext" >剩余票数：<span class="num" id="spcanusecount"><%=CanUseVoteCount%></span>
        </span>--%>
        <a href="javascript:void(0)" class="btn_min orange" id="btnBuy">为Ta助力</a>
    </div>
    <div class="votetitle"><%=VoteInfo.VoteName %></div>
    <div style="font-size:12px;font-family: Tahoma,Arial, Helvetica, sans-serif,"Hiragino Sans Gb","Microsoft YaHei";"><%=VoteInfo.Summary %></div>
    <div>
   
    <div class="searchbox">
        <input id="txtName" type="text" placeholder="输入选手名字"/>
        <button class="searchbtn" onclick="Search()">搜索</button>
    </div>


    </div>
    <div class="votelist" id="votelist">
           
    </div>
    <%if (!string.IsNullOrEmpty(VoteInfo.BottomContent)){%>
    <div style="margin-top:50px;">
    <%=VoteInfo.BottomContent%>
    </div>
    <%}%>
</section>
<div class="screenforbid" style="display:none;" id="divDlg">
    <div class="dialogcenter">
        <div class="dialogbox">
            <label class="dialoginfo" for="">确认给<span class="votename" id="spvotename"></span>投票</label>
            <input type="number" pattern="\d*" placeholder="您要投的票数" class="votetext" value="1" id="txtcount" readonly="readonly">
            <span class="btn orange" id="btnOk">确定</span>
            <span class="btn gary2" id="btnCancel">取消</span>
        </div>
    </div>
</div>
</body>
<script type="text/javascript">
    var voteid = "<%=VoteInfo.AutoID%>";
    var pageindex = 1;//当前第几页
    var pagesize=40; //每页数量
    var name = "";
    var number = "";
    var voteobjectid = 0;
    var voteObject;
    var sort = "addtimeasc";
    $(function () {

        try {

            LoadData();

            $("#btnNext").live("click", function () {
                pageindex++;
                LoadData();

            });

            $("#btnOk").click(function () {

                var strcount = $("#txtcount").val();
                if (strcount == "") {
                    $("#txtcount").focus();
                    return false;
                }
                if (isNaN(strcount)) {
                    $("#txtcount").val("").focus();
                    return false;
                }
                if (parseInt(strcount) <= 0) {
                    $("#txtcount").val("").focus();
                    return false;

                }
                //

                var vote = new voteControl();
                //
                $("#divDlg").hide();
                $.ajax({
                    type: 'post',
                    url: "/App/Cation/Wap/Vote/Comm/Handler.ashx",
                    data: { Action: "UpdateVoteObjectVoteCount", vid: voteid, id: voteobjectid, count: strcount },
                    timeout: 30000,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.errcode == 0) {
                            alert("投票成功");
                        }
                        else {
                            //
                            alert(resp.errmsg);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (textStatus == "timeout") {
                            alert("投票超时，请重新投票");

                        }
                    }
                });


                //


                //


            });

            $("#btnCancel").click(function () {

                $("#divDlg").hide();



            });

            $("#btnBuy").click(function () {
             //window.location.href = "pay.htm?count=" + $("#spcanusecount").text();
                window.location.href = "Recharge.aspx?vid=" + voteid;
            });

            if (voteid == "121") {
                $("#txtcount").attr("readonly", "readonly");
            }

        } catch (e) {
            alert(e);
        }

    })
    //加载选手列表 分页
    function LoadData() {
        $.ajax({
            type: 'post',
            url: "/Handler/CommHandler.ashx",
            data: { Action: 'GetVoteObjectVoteList', voteid: voteid, pageindex: pageindex, pagesize: pagesize, name: name, number: number,Sort:sort },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    str.AppendFormat('<div class="voteebox">');
                    str.AppendFormat('<a href="VoteObjectDetail.aspx?id={0}" class="voteeimg">',resp.ExObj[i].AutoID);
                    str.AppendFormat('<img src="{0}" >',resp.ExObj[i].VoteObjectHeadImage);
                    str.AppendFormat('<span class="voteenum">{0}</span>',resp.ExObj[i].Number);
                    str.AppendFormat('<span class="votenum" id="votecount_{0}">{1}票</span>', resp.ExObj[i].AutoID, resp.ExObj[i].VoteCount);
                    //str.AppendFormat('<span class="votelogo" style="background-image: url(/img/vote/guicailogo.png)"></span>');
                    str.AppendFormat('</a>');
                    str.AppendFormat('<span class="voteeinfo"><span class="voteobjectname">{0}</span> &nbsp;&nbsp; {1}</span>',resp.ExObj[i].VoteObjectName,resp.ExObj[i].Area);
                    str.AppendFormat('<span class="btn_min gary" data-voteobjectid="{0}" onclick="Vote(this)">我要投票</span>',resp.ExObj[i].AutoID);
                    str.AppendFormat('</div>');

                }; 
                if (pageindex==1) {
                if (resp.ExInt == 1) {
                         //显示下一页按钮
                    str.AppendFormat('<div class="voteebox" style="width:100%;height:20px;" id="btnNext">');
                    str.AppendFormat('<div style="margin-top:5px;">');
                            str.AppendFormat('<a href="javascript:void(0)" >显示更多</a>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            listHtml += str.ToString();
                            $("#votelist").html(listHtml);

                        }
                        else {
                            listHtml += str.ToString();
                            $("#votelist").html(listHtml);
 
                        }



                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml != "") {
                             $("#btnNext").before(listHtml);
                         }
                         else {

                             $("#btnNext").remove();

                         }

                    }



            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");

                }
            }
        });




    }


    function Vote (object) {
        var voteObjectName = $(object).prev("span").find(".voteobjectname").html();
        voteobjectid = $(object).attr("data-voteobjectid");
        voteObject =object;
        $("#txtcount").val("1");
        $("#spvotename").html(voteObjectName);
        $("#divDlg").show();
        $("#txtcount").focus();
    
    
    }

    function Search() {
        pageindex = 1;
        name = $("#txtName").val();
        LoadData();
    
    }

</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=VoteInfo.VoteName%>",
            desc: "<%=VoteInfo.Summary%>",
            //link: '', 
            imgUrl: "<%=VoteInfo.VoteImage%>"
        })
    })
</script>
</html>
