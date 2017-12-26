<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddAdvert.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.AddAdvert" %>


<!DOCTYPE html>
<html>
<head>
    <title>至云公众号平台</title>
    <meta charset="utf-8">

    <link href="/css/game/game.css" rel="stylesheet" type="text/css" />
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/game/inpagecommon.js" type="text/javascript"></script>
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <script src="/lib/layer/2.1/layer.js"></script>

</head>
<body class="insidepage" ng-app="">
    <div class="breadnav">微游戏>新建游戏</div>
    <div class="mainbox">
        <div class="handlebar">
            <a href="GameList.aspx" class="icobtn bluebtn">
                <span class="btnico backico"></span>
                <span class="text">返回</span>
            </a>
            <%--<a href="javascript:void(0);" class="icobtn greenbtn previewbtn">
                <span class="btnico rqcodeico" rqsrc="http://wx.comeoncloud.net/Handler/ImgHandler.ashx?v=http://wx.comeoncloud.net/Handler/QRLogin.ashx?tiket=MDU0NWE1NWYtZWI4Yy00YjNjLThhMDEtYzY4N2NmOGQ3Zjc2"></span>
                <span class="text">预览游戏</span>
            </a>--%>
                   
            
            
            </div>
		<div class="scorllbox">
<%--			<div class="gamebox showgame">
                <img src="/styles/images/pic.png" alt="" class="showpic">
                <div class="showdesc">
                    <h2 class="showtitle">游戏名称</h2>
                    <p class="showtext">游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍游戏介绍</p>
                </div>
            </div>--%>
			<div class="inputbox">
				<p class="inputtitle">游戏名称:</p>
				<input type="text" id="txtPlanName" class="middletext" placeholder="必填"/>
			</div>
            <div class="inputbox">
                <p class="inputtitle">挂载广告:</p>
                <span style="position:relative;top:8px;">
                    <input type="checkbox" ng-model="showAd" />
                </span>                
			</div>
            <div ng-show="showAd" class="ng-hide">

			    <div class="inputbox">
				    <p class="inputtitle">广告图片1:</p>
				    <label for="txtAdPath1" class="checkpic" id="lblad1" >选择图片</label>
				    <input type="file" id="txtAdPath1" name="file1" class="hiddeninput"/>
				    <div class="hintbar">
					    <span class="hinticon"></span>
					    <span class="hinttext">此图片将显示在您游戏的广告位中,尽量使用320*60尺寸的图片</span>
				    </div>
			    </div>
			    <div class="inputbox">
				    <p class="inputtitle">广告链接1:</p>
				    <input type="text" id="txtAdUrl1"  class="largetext" placeholder="必填"/>
			    </div>
			    <br/>
			    <div class="inputbox">
				    <p class="inputtitle">广告图片2:</p>
				    <label for="txtAdPath2" class="checkpic" id="lblad2">选择图片</label>
				    <input type="file" id="txtAdPath2"  name="file2" class="hiddeninput">
			    </div>
			    <div class="inputbox">
				    <p class="inputtitle">广告链接2:</p>
				    <input type="text" id="txtAdUrl2" class="largetext" placeholder="选填"/>
			    </div>
			    <div class="inputbox">
				    <p class="inputtitle">广告图片3:</p>
				    <label for="txtAdPath3" class="checkpic" id="lblad3">选择图片</label>
				    <input type="file" id="txtAdPath3"  name="file3" class="hiddeninput"/>
			    </div>
			    <div class="inputbox">
				    <p class="inputtitle">广告链接3:</p>
				    <input type="text" id="txtAdUrl3" class="largetext" placeholder="选填"/>
			    </div>

            </div>
			<div class="inputbox">
				<p class="inputtitle">&nbsp;</p>
            	<a id="btnSave" href="javascript:void(0);" class="nbtn">
            	    <span class="text">保存</span>
            	</a>
            	<a href="javascript:void(0);" class="nbtn">
            	    <span class="text">取消</span>
            	</a>
			</div>
		</div>
    </div>
	
	<div class="screenforbid">
		<div class="loadcenterbox">
			<div class="loadbarbox">
				<div class="loadtext">正在加载,请稍后...</div>
			    <div class="loadbar"></div>	
			</div>
		</div>
	</div>
</body>
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var tableheight = new tableHeight(".scorllbox", 60, [".breadnav", ".handlebar"]);

    window.alert = function (msg) {
        layer.msg(msg);
    }

   // showrqcode(".rqcodeico");
    $("#txtAdPath1").live('change', function () {
        try {

            $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=game&filegroup=file1',
                         secureuri: false,
                         fileElementId: 'txtAdPath1',
                         dataType: 'text',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 $("#lblad1").html("<img id=\"img1\" style=\"width:320px;height:60px;\" src=" + resp.ExStr + ">");

                             }
                             else {
                                 alert(resp.Msg);
                             }
                         }
                     }
                    );

        } catch (e) {
            alert(e);
        }
    });

    $("#txtAdPath2").live('change', function () {
        try {

            $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=game&filegroup=file2',
                         secureuri: false,
                         fileElementId: 'txtAdPath2',
                         dataType: 'text',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 $("#lblad2").html("<img id=\"img2\" style=\"width:320px;height:60px;\" src=" + resp.ExStr + ">");

                             }
                             else {
                                 alert(resp.Msg);
                             }
                         }
                     }
                    );

        } catch (e) {
            alert(e);
        }
    });

    $("#txtAdPath3").live('change', function () {
        try {

            $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=game&filegroup=file3',
                         secureuri: false,
                         fileElementId: 'txtAdPath3',
                         dataType: 'text',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 $("#lblad3").html("<img id=\"img3\" style=\"width:320px;height:60px;\" src=" + resp.ExStr + ">");

                             }
                             else {
                                 alert(resp.Msg);
                             }
                         }
                     }
                    );

        } catch (e) {
            alert(e);
        }
    });

    $("#btnSave").click(function () {

        var planName = $("#txtPlanName").val();
        var url1 = $("#txtAdUrl1").val();
        var url2 = $("#txtAdUrl2").val();
        var url3 = $("#txtAdUrl3").val();
        var img1 = $("#img1").attr("src");
        var img2 = $("#img2").attr("src");
        var img3 = $("#img3").attr("src");


        try {
            var model =
                    {
                        GameId: "<%=GameId%>",
                        Action: "AddGamePlan",
                        PlaneName: planName,
                        AdvertUrl1: url1,
                        AdvertUrl2: url2,
                        AdvertUrl3: url3,
                        AdvertImage1: img1,
                        AdvertImage2: img2,
                        AdvertImage3: img3
                    };

            if (model.PlaneName == '') {
                $('#txtPlanName').focus();
                alert('请输入广告名称');
                return;
            }



            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: model,
                success: function (result) {

                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {
                        alert(resp.Msg);

                        setTimeout(function () {
                            window.location = "/Game/PlanList.aspx";
                        }, 1000);

                        
                    }
                    else {
                        alert(resp.Msg);
                    }
                }
            });

        } catch (e) {
            alert(e);
        }


    });


</script>
</html>


