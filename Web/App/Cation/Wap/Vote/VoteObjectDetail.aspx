<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoteObjectDetail.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.VoteObjectDetail" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link rel="stylesheet" href="/css/vote/style.css?v=0.0.1" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.slides.min.js" type="text/javascript"></script>
    <script src="/Scripts/vote/voteanimate.js" type="text/javascript"></script>
    <link rel="stylesheet" href="styles/css/style.css?v=0.0.1">
</head>
<body>
    <section class="box bottompadding ">
       <%if (!string.IsNullOrEmpty(VoteInfoModel.Logo))
         {%>
          <div>
          <style type="text/css">
              .total
              {
                  position: relative;
              }
          </style>
          <img src="<%=VoteInfoModel.Logo%>" style="width:100%;max-height:150px;" />
          </div>
     <% } %>
       <%if (VoteInfoModel.IsFree.Equals(0))
         { %>
       <div class="total votenumbar">
      <%} %>
      <%else
          { %>
       <div class="total votenumbar votenopay">
       <%} %>
       <%-- <span class="votenumtext" >剩余票数：<span class="num" id="spcanusecount"><%=CanUseVoteCount%></span>
        </span>--%>
        <a href="javascript:void(0)" class="btn_min orange" id="btnBuy">为Ta助力</a>
    </div>
    <div class="voteeifobox">
        <div class="voteebox">
            <div class="voteeimg">
            
                <img src="<%=model.VoteObjectHeadImage%>">
                <span class="voteenum"><%=model.Number%></span>
                <span class="votenum" id="spanvoteobjectvotecount"><%=model.VoteCount%>票</span>
            </div>
            <div class="voteeinfo">
             

                <p class="voteeinfobox">
                    <span class="titletext">姓名:</span>
                    <span class="infotext"><%=model.VoteObjectName%></span>
                </p>

               <%if (!string.IsNullOrEmpty(model.Height))
                 {%>
                  <p class="voteeinfobox">
                    <span class="titletext">身高:</span>
                    <span class="infotext"><%=model.Height%></span>
                </p>
                    
                <%}%>

                <%if (!string.IsNullOrEmpty(model.Area))
                  {%>

                 <p class="voteeinfobox">
                    <span class="titletext">地区:</span>
                    <span class="infotext"><%=model.Area%></span>
                </p>
                <%}%>

                <%if (!string.IsNullOrEmpty(model.Age))
                  {%>
                    <p class="voteeinfobox">
                    <span class="titletext">年龄:</span>
                    <span class="infotext"><%=model.Age%></span>
                </p>
                <%}%>
                <%if (!string.IsNullOrEmpty(model.SchoolName))
                  {%>
                    <p class="voteeinfobox">
                    <span class="titletext">学校:</span>
                    <span class="infotext"><%=model.SchoolName%></span>
                </p>
                <%}%>
                
                <%if (!string.IsNullOrEmpty(model.Constellation))
                  {%>
                <p class="voteeinfobox">
                    <span class="titletext">星座:</span>
                    <span class="infotext"><%=model.Constellation%></span>
                </p>
                <%}%>




                  <%if (!string.IsNullOrEmpty(model.Hobbies))
                    {%>
                    
                <p class="voteeinfobox">
                    <span class="titletext">爱好:</span>
                    <span class="infotext"><%=model.Hobbies%></span>
                </p>
                <%}%>

            </div>




            <%if (!string.IsNullOrEmpty(model.Introduction))
              {%>
            <div class="voteedeclaration">
                <h2 class="titletext">参赛宣言:</h2>
                <span class="infotext"><%=model.Introduction%></span>
            </div>
                <%}%>


        </div>
    </div>
    <%if (VoteInfoModel.VoteType.Equals(0))
      {%>

      
    <div class="touchbox">
    <p style="text-align:center;margin-top:20px;"><a href="javascript:void(0)" class="btn_min gary" onclick="slide();$(this).remove()">更多图片</a></p>
    </div>
  

    <%}%>
    <%if (VoteInfoModel.VoteType.Equals(1))
      {%>
    <div align="center" style="padding: 10px 0;margin-top: 10px;width: 100%;">
    <%=model.IntroductionDetail %>
    </div>
    <%}%>
    <div class="backbar">
        <a href="Vote.aspx?vid=<%=VoteInfoModel.AutoID%>" class="back"><span class="icon"></span></a>
        <a href="javascript:void(0)" class="btn orange" id="btnVote">我要投票</a>
    </div>

     <%if (VoteInfoModel.VoteType.Equals(0))
       {%>
      
    <div id="slides" style="display:none;">
        <div class="slider slider1">
            <img src="<%=model.ShowImage1%>" alt="图片" class="pic">
        </div>
        <div class="slider slider2">
            <img src="<%=model.ShowImage2%>" alt="图片" class="pic">
        </div>
        <div class="slider slider3">
            <img src="<%=model.ShowImage3%>" alt="图片" class="pic">
        </div>
        <div class="slider slider4">
            <img src="<%=model.ShowImage4%>" alt="图片" class="pic">
        </div>
         <div class="slider slider5">
            <img src="<%=model.ShowImage5%>" alt="图片" class="pic">
        </div>  
    </div>

    <%}%>
   </div>
   <div id="Ischeck">
     <div style="padding:10px; box-sizing: border-box;">
       <textarea style="resize:none; width:100%; height:70px;"  id="txtContent" ></textarea>
        <span  style="float:right" id="BtnReview" class="btn orange">提交评论</span>
        <div class="clear"></div>
    </div>
     <div id="divHtml">
     </div>
    </div>
     <%if (!string.IsNullOrEmpty(model.BottomContent))
       {%>
    <div style="margin-top:50px;">
     <%=model.BottomContent%>
    </div>
    <%}%>
</section>
    <div class="screenforbid" style="display: none;" id="divDlg">
        <div class="dialogcenter">
            <div class="dialogbox">
                <label class="dialoginfo" for="">
                    确认给<span class="votename"><%=model.VoteObjectName%></span>投票</label>
                <input type="number" pattern="\d*" placeholder="您要投的票数" class="votetext" value="1"
                    id="txtcount" readonly="readonly">
                <span class="btn orange" id="btnOk">确定</span> <span class="btn gary2" id="btnCancel">
                    取消</span>
            </div>
        </div>
    </div>
    <div id="textdialogbox" style="padding-top: 10px; padding-left: 10px; padding: 10px;">
        <div class="textdialog">
            <textarea name="" id="TxtReplyContent" cols="30" rows="10" placeholder="你的内容"></textarea>
            <span class="button" id="btnExit">取消</span> <span class="button blue" id="btnSave">回复</span>
        </div>
    </div>
</body>
<script type="text/javascript">
    var voteid = "<%=VoteInfoModel.AutoID%>";
    var voteobjectid = "<%=model.AutoID%>";
    var voteObject = document.getElementById('btnVote');
    $(function () {
        $("#btnVote").click(function () {
            $("#txtcount").val("1");
            $("#divDlg").show();
            $("#txtcount").focus();
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
                    if (resp.errcode ==0) {
                        //
                        vote.votesuccess(voteObject, strcount);

                        //$("#spanvoteobjectvotecount").html(resp.ExInt + "票");
                        //vote.votesuccess(voteObject, strcount);
                        //$("#spcanusecount").html(resp.ExStr);
                        alert("投票成功");
                        window.location.href = window.location.href;
                    }
                    else {
                        //
                        // vote.votefailure(voteObject);
                        alert(resp.errmsg);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("投票超时，请重新投票");

                    }
                }
            });




        });

        $("#btnCancel").click(function () {

            $("#divDlg").hide();
        });

        $("#btnBuy").click(function () {
            //window.location.href = "pay.htm?count=" + $("#spcanusecount").text();
            window.location.href = "Recharge.aspx?vid=" + voteid;
        });

        if (voteid == "121") {
            $("#txtcount").attr("readonly","readonly");
        }

    })

</script>
<script type="text/javascript">

    function slide() {
        
        $("#slides").slidesjs({
            width: 100,
            height: 200,
            play: {
                active: true,
                auto: true,
                interval: 4000,
                swap: true
            }
        });


    }

</script>
<script>
    $(function () {
        $.ajax({
            type: "post",
            url: "/Handler/App/WXReviewInfoHandler.ashx",
            data: { Action: "IsCheck", Wherestr: "VoteId" },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 0) {
                    InitData();
                } else {
                    $('#Ischeck').hide(); //隐藏
                }
            }
        })
    })

    function InitData() {
        $.ajax({
            type: "post",
            url: "/Handler/CommHandler.ashx",
            data: { Action: "GetVoteReviewInfo", voteid: voteobjectid },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                $("#divHtml").html("");
                var Html = "";
                if (resp.Status == 0) {
                    Html += '<div id="discussbox">';
                    $.each(resp.ExObj, function (index, data) {
                        Html += '<div class="commentbox"><p class="commentcb">';
                        Html += '<span class="portrait"><img src="styles/images/pic.png" alt=""></span>';
                        Html += '<span class="commentc"><span class="peoplename">' + data.UserName + '：</span>' + data.ReviewContent + '</span></p>';
                        Html += '<div class="commentdealbox">';
                        Html += '<div class="commenttime">' + data.InsertDateStr + '</div>';
                        Html += '<div class="recommentbtn"  onclick="ShowReply(' + data.AutoId + ')"><span class="icon" ></span>回复:(' + data.NumCount + ')</div>'
                        Html += '<div class="recommentbox"></div></div> ';
                        $.each(data.rrInfos, function (index, dat) {
                            Html += '<div class="recommentbox"><p class="commentcb">';
                            Html += '<span class="commentc"><span class="peoplename">' + dat.UserName + '：</span><span>' + dat.ReplyContent + '</span>';
                            Html += '</p></div>'
                        });
                        Html += '</div>';
                    });
                    Html += '</div>';
                    $("#divHtml").html(Html);
                }
            }
        });
    }
    var autoId = 0;
    function ShowReply(AutoId) {
        autoId = AutoId;
        $("#textdialogbox").show();
        $("#TxtReplyContent").focus();
    }

    $("#btnExit").click(function () {
        $("#textdialogbox").hide();
    });

    $("#btnSave").click(function () {
        var replyContent = $("#TxtReplyContent").val();



        if (replyContent == "") {
            alert("请输入评论内容");
            return;
        }
        $.ajax({
            type: "post",
            url: "/Handler/App/WXReviewInfoHandler.ashx",
            data: { Action: "ReplyReviewInfo", ReviewID: autoId, ReplyContent: replyContent, AutoId: voteobjectid },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 0) {
                    $("#TxtReplyContent").val("");
                    $("#textdialogbox").hide();
                    InitData();
                } else {
                    alert(resp.Msg);
                }
            }
        })
    });

    $("#BtnReview").click(function () {
        var Content = $("#txtContent").val()
        if (Content == "") {
            alert("请输入评论内容");
            return;
        }
        $.ajax({
            type: "Post",
            url: "/Handler/CommHandler.ashx",
            data: { Action: "AddVoteReviewInfo", voteid: voteobjectid, Content: Content, ReviewType: "投票", FName: '<%=model.VoteObjectName%>' },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 0) {
                    $("#txtContent").val()
                    InitData();
                } else {
                    alert(resp.Msg);
                }
            }
        });
    });
</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=VoteInfoModel.VoteName%>",
            desc: "<%=model.VoteObjectName%>",
            //link: '', 
            imgUrl: "<%=model.VoteObjectHeadImage%>"
        })
    })
</script>
</html>
