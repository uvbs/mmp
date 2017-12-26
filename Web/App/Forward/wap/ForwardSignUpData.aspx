<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForwardSignUpData.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.ForwardSignUpData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报名</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/easyresource.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <style type="text/css">
        .mainlist li div
        {
            display: block;
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 0 6px rgba(0,0,0,0.2);
        }
        #needList .list{margin-bottom: 0px;}
        #needList{
            padding-top: 8px;
        }
    </style>
</head>
<body>
    <section class="box">
  <%--    <div class="searchbox">
        <input type="text" id="txtActivityName" placeholder="输入标题"/>
        <button class="searchbtn" type="button" id="btnSearch">搜索</button>
    </div>--%>
    <%--<ul class="mainlist articlelist currentlist" id="needList">
    </ul>--%>

        <div id="needList">

        </div>
    </section>
</body>
<script type="text/javascript">
    var PageIndex = 1; //第几页
    var PageSize = 5; //每页显示条数
    var Category = ""; //分类
    var totalCount = 0;
    var AdInfoLength = 0;
    var adInfoList = [];//

    $(function () {
        LoadNeed(true);
        $("#ddlCategory").change(function () {
            PageIndex = 1;
            Category = $(this).val();
            $("#txtActivityName").val("");
            $("#btnSearch").html("正在搜索...");
            LoadNeed(true);

        })
        $("#btnSearch").click(function () {
            PageIndex = 1;
            $(this).html("正在搜索...");
            LoadNeed(true);

        })

        $(window).scroll(function () {
            //当内容滚动到底部时加载新的内容
            if ($(this).scrollTop() + $(window).height() >= $(document).height()) {
                //判断当没有数据的时候不加载
                if (totalCount > AdInfoLength) {
                    LoadNeed(false);
                }
            }
        });

    });

    //function BtnClick() {
    //    PageIndex++;
    //    LoadNeed();
    //}




    //加载列表分页
    function LoadNeed(isNew) {
        if (isNew) {
            adInfoList = [];
            PageIndex = 1;
        }
        else {
            PageIndex++;
        }
        $.ajax({
            type: 'get',
            url: "/Handler/App/WXForwardHandler.ashx",
            data: { Action: 'GetSignUpActivityInfo', PageIndex: PageIndex, PageSize: PageSize, MonitorPlanID: '<%=MonitorPlanID %>', ActivityId: '<%=ActivityId %>' }, //ActivityName: $("#txtActivityName").val(), 
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                $("#btnSearch").html("搜索");
                if (resp.ExObj == null) { return; }
                totalCount = resp.ExObj.totalCount;
                AdInfoLength = resp.ExObj.AdInfo.length;
                //adInfoList = resp.ExObj.AdInfo;
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.AdInfo.length; i++) {
                    //构造视图模板
                    //str.AppendFormat('<li>');
                    //str.AppendFormat('<div>', "");
                    //str.AppendFormat('姓名:{0}<br/>', resp.ExObj.AdInfo[i].Name);
                    //str.AppendFormat('电话:{0}<br/>', resp.ExObj.AdInfo[i].Phone);
                    //str.Append(GetSignUpInfo(resp.ExObj.AdInfo[i], resp.ExObj.mppInfo, resp.ExObj.MappingNum));
                    //str.AppendFormat('时间:{0}<br/>', resp.ExObj.AdInfo[i].InsertDateStr);
                    //str.AppendFormat('</div>');
                    //str.AppendFormat('</div>');
                    //str.AppendFormat('</li>');


                    adInfoList.concat(resp.ExObj.AdInfo[i]);

                    str.AppendFormat('<div class="list">');
                    str.AppendFormat('<a class="item item-thumbnail-left" href="#">');
                    if (resp.ExObj.AdInfo[i].WXHeadimgurlLocal == null) {
                        str.AppendFormat('<img src="http://files.comeoncloud.net/img/europejobsites.png">');
                    } else {
                        str.AppendFormat('<img src="{0}">', resp.ExObj.AdInfo[i].WXHeadimgurlLocal);
                    }
                    str.AppendFormat('<h2>{0}</h2>', resp.ExObj.AdInfo[i].Name);
                    str.AppendFormat('<p>{0}</p>', resp.ExObj.AdInfo[i].Phone);
                    str.AppendFormat('<p>{0}</p>', GetSignUpInfo(resp.ExObj.AdInfo[i], resp.ExObj.mppInfo, resp.ExObj.MappingNum));
                    str.AppendFormat('</a>');

                    str.AppendFormat('</div>');
                };
                if (PageIndex == 1) {
                    if (resp.ExStr == "1") {
                        //显示下一页按钮
                        //str.AppendFormat('<li>');
                        //str.AppendFormat('<a id="btnNext" onclick="BtnClick()">');
                        //str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', PageSize);
                        //str.AppendFormat('</a>');
                        //str.AppendFormat('</li>');
                        

                        listHtml += str.ToString();
                        $("#needList").html(listHtml);

                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml == "") {
                            listHtml = "暂时没有报名";
                        }
                        $("#needList").html(listHtml);

                    }



                }
                else {
                    listHtml += str.ToString();
                    if (listHtml != "") {
                        $("#needList").append(listHtml);
                        //$("#btnNext").before(listHtml);
                    }
                    else {

                        $("#needList").append(listHtml);
                        //$("#btnNext").remove();

                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");
                }
                $("#btnSearch").html("搜索");
            }
        });
    }

    function GetSignUpInfo(data, Mapping, Nums) {
        var str = "";
        for (var i = 0; i < Mapping.length; i++) {
            if (Mapping[i].IsHideInSubmitPage != "1") {

           

            if (i <= Nums) {
                if (i == 0) {
                    str += Mapping[i].MappingName + ':' + data.K1 + '<br/>';
                }
                if (i == 1) {
                    str += Mapping[i].MappingName + ':' + data.K2 + '<br/>';
                }
                if (i == 2) {
                    str += Mapping[i].MappingName + ':' + data.K3 + '<br/>';
                }
                if (i == 3) {
                    str += Mapping[i].MappingName + ':' + data.K4 + '<br/>';
                }
                if (i == 4) {
                    str += Mapping[i].MappingName + ':' + data.K5 + '<br/>';
                }
                if (i == 5) {
                    str += Mapping[i].MappingName + ':' + data.K6 + '<br/>';
                }
                if (i == 6) {
                    str += Mapping[i].MappingName + ':' + data.K7 + '<br/>';
                }
                if (i == 7) {
                    str += Mapping[i].MappingName + ':' + data.K8 + '<br/>';
                }
                if (i == 8) {
                    str += Mapping[i].MappingName + ':' + data.K9 + '<br/>';
                }
                if (i == 9) {
                    str += Mapping[i].MappingName + ':' + data.K10 + '<br/>';
                }
                if (i == 10) {
                    str += Mapping[i].MappingName + ':' + data.K11 + '<br/>';
                }
                if (i == 11) {
                    str += Mapping[i].MappingName + ':' + data.K12 + '<br/>';
                }
                if (i == 12) {
                    str += Mapping[i].MappingName + ':' + data.K13 + '<br/>';
                }
                if (i == 13) {
                    str += Mapping[i].MappingName + ':' + data.K14 + '<br/>';
                }
                if (i == 14) {
                    str += Mapping[i].MappingName + ':' + data.K15 + '<br/>';
                }
                if (i == 15) {
                    str += Mapping[i].MappingName + ':' + data.K16 + '<br/>';
                }
                if (i == 16) {
                    str += Mapping[i].MappingName + ':' + data.K17 + '<br/>';
                }

                if (i == 17) {
                    str += Mapping[i].MappingName + ':' + data.K18 + '<br/>';
                }
                if (i == 18) {
                    str += Mapping[i].MappingName + ':' + data.K19 + '<br/>';
                }
                if (i == 19) {
                    str += Mapping[i].MappingName + ':' + data.K10 + '<br/>';
                }
                if (i == 20) {
                    str += Mapping[i].MappingName + ':' + data.K21 + '<br/>';
                }
                if (i == 21) {
                    str += Mapping[i].MappingName + ':' + data.K22 + '<br/>';
                }
                if (i == 22) {
                    str += Mapping[i].MappingName + ':' + data.K23 + '<br/>';
                }
                if (i == 23) {
                    str += Mapping[i].MappingName + ':' + data.K24 + '<br/>';
                }
                if (i == 24) {
                    str += Mapping[i].MappingName + ':' + data.K25 + '<br/>';
                }
                if (i == 25) {
                    str += Mapping[i].MappingName + ':' + data.K26 + '<br/>';
                }
                if (i == 26) {
                    str += Mapping[i].MappingName + ':' + data.K27 + '<br/>';
                }
                if (i == 27) {
                    str += Mapping[i].MappingName + ':' + data.K28 + '<br/>';
                }
                if (i == 28) {
                    str += Mapping[i].MappingName + ':' + data.K29 + '<br/>';
                }
                if (i == 29) {
                    str += Mapping[i].MappingName + ':' + data.K30 + '<br/>';
                }
                if (i == 30) {
                    str += Mapping[i].MappingName + ':' + data.K31 + '<br/>';
                }
                if (i == 31) {
                    str += Mapping[i].MappingName + ':' + data.K32 + '<br/>';
                }
                if (i == 32) {
                    str += Mapping[i].MappingName + ':' + data.K33 + '<br/>';
                }
                if (i == 33) {
                    str += Mapping[i].MappingName + ':' + data.K34 + '<br/>';
                }
                if (i == 34) {
                    str += Mapping[i].MappingName + ':' + data.K35 + '<br/>';
                }
                if (i == 35) {
                    str += Mapping[i].MappingName + ':' + data.K36 + '<br/>';
                }
                if (i == 36) {
                    str += Mapping[i].MappingName + ':' + data.K37 + '<br/>';
                }
                if (i == 37) {
                    str += Mapping[i].MappingName + ':' + data.K38 + '<br/>';
                }
                if (i == 38) {
                    str += Mapping[i].MappingName + ':' + data.K39 + '<br/>';
                }
                if (i == 39) {
                    str += Mapping[i].MappingName + ':' + data.K40 + '<br/>';
                }
                if (i == 40) {
                    str += Mapping[i].MappingName + ':' + data.K41 + '<br/>';
                }
                if (i == 41) {
                    str += Mapping[i].MappingName + ':' + data.K42 + '<br/>';
                }
                if (i == 42) {
                    str += Mapping[i].MappingName + ':' + data.K43 + '<br/>';
                }
                if (i == 43) {
                    str += Mapping[i].MappingName + ':' + data.K44 + '<br/>';
                }
                if (i == 44) {
                    str += Mapping[i].MappingName + ':' + data.K45 + '<br/>';
                }
                if (i == 45) {
                    str += Mapping[i].MappingName + ':' + data.K46 + '<br/>';
                }
                if (i == 46) {
                    str += Mapping[i].MappingName + ':' + data.K47 + '<br/>';
                }
                if (i == 47) {
                    str += Mapping[i].MappingName + ':' + data.K48 + '<br/>';
                }
                if (i == 48) {
                    str += Mapping[i].MappingName + ':' + data.K49 + '<br/>';
                }
                if (i == 49) {
                    str += Mapping[i].MappingName + ':' + data.K50 + '<br/>';
                }
                if (i == 50) {
                    str += Mapping[i].MappingName + ':' + data.K51 + '<br/>';
                }
                if (i == 51) {
                    str += Mapping[i].MappingName + ':' + data.K52 + '<br/>';
                }
                if (i == 52) {
                    str += Mapping[i].MappingName + ':' + data.K53 + '<br/>';
                }
                if (i == 53) {
                    str += Mapping[i].MappingName + ':' + data.K54 + '<br/>';
                }
                if (i == 54) {
                    str += Mapping[i].MappingName + ':' + data.K55 + '<br/>';
                }
                if (i == 55) {
                    str += Mapping[i].MappingName + ':' + data.K56 + '<br/>';
                }
                if (i == 56) {
                    str += Mapping[i].MappingName + ':' + data.K57 + '<br/>';
                }
                if (i == 57) {
                    str += Mapping[i].MappingName + ':' + data.K58 + '<br/>';
                }
                if (i == 58) {
                    str += Mapping[i].MappingName + ':' + data.K59 + '<br/>';
                }
                if (i == 59) {
                    str += Mapping[i].MappingName + ':' + data.K60 + '<br/>';
                }
            } else {
                break;
            }

        }

            

        }
        return str;
    }
    
</script>
</html>
