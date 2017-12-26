<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BookingDoctor.List" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>膏方妇幼专家预约平台</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/customize/BookingDoctor/Style/comm.css?v=1.0.0.2" rel="stylesheet" />
    <style>
        body {
            /*padding-left: 10px;
            padding-right: 10px;
            font-family: "Microsoft YaHei" !important;*/
        }



        .navbar-brand {
            padding: 8px 15px;
        }

        .navbar-default {
            background-color: #00D5C9;
            border-color: #e7e7e7;
            height: 30px;
        }

        .navbar-fixed-top, .navbar-fixed-bottom {
            position: relative;
        }

        /*.return_ico {
            height: 35px;
            width: 35px;
        }*/

        .return_img {
            height: 35px;
            width: 35px;
        }

        .imglogo {
            width: 75px;
            height: 75px;
        }

        /*.logo {
            text-align: center;
            position: absolute;
            top: 10px;
            z-index: 10000;
            width: 100%;
        }*/

        .split {
            width: 100%;
        }

        .divsearch {
            /*margin-top: 12%;*/
            margin-bottom: 6%;
            background-color: #E3E2E2;
            border-radius: 5px;
            height: 50px;
        }

        .head {
            width: 80px;
            height: 80px;
            border-radius: 55px;
        }

        .name {
            font-size: 20px;
            margin-left: 15px;
        }

        .position {
            font-size: 16px;
            color: #908C88;
            margin-bottom: 5px;
        }

        .address {
            /*margin-left: 15px;*/
        }

        .hospital {
            margin-left: 15px;
            word-break: keep-all; /* 不换行 */
            white-space: nowrap; /* 不换行 */
            overflow: hidden; /* 内容超出宽度时隐藏超出部分的内容 */
            text-overflow: ellipsis; /* 当对象内文本溢出时显示省略标记(...) ；需与overflow:hidden;一起使用。*/
        }

        .yuyue {
            color: #00D5C9;
            text-align: center;
        }

        .keyword {
            background-color: #D4D2CE;
            border-radius: 5px;
            height: 30px;
            margin-top: 5px;
            width: 100%;
            border: 1px;
            border-bottom-style: none;
            border-top-style: none;
            border-left-style: none;
            border-right-style: none;
            color:white;
        }

        divsearch table {
            width: 100%;
        }
        .tdkeyword {
         width:100%;
        }
        .imgsearch {
            width: 20px;
            height: 20px;
            margin-left:10px;
            margin-right:10px;
        }

        ::-webkit-input-placeholder {
            color: white !important;
        }
        .divkeyword {
            background-color: #D4D2CE;
             border-radius: 5px;
             margin-top:8px;
             margin-right:5px;
        }
        .nodata {
            font-size:18px;
            /*font-weight:bold;*/
            text-align:center;
        }
        .imglogosearch {
            height:30px;
        }
        .divmore {
            text-align:center;
        }
        .btn-default {
            width:100%;
            /*color:white;
            background-color: #00D5C9;*/
        }
        #btnMore {
            margin-bottom:50px;
        }
    </style>
</head>
<body>
    <div class="container-fluid">
        <div class="">
            <nav class="navbar navbar-default navbar-fixed-top" role="navigation" >
                <div class="col-sm-1 col-xs-1">
                    <div class="navbar-header">
                        <a class="navbar-brand" >
                            
                        </a>
                    </div>
                </div>
                <div class="col-sm-10 col-xs-10 page_title">
                </div>
            </nav>
<%--            <div class="logo">

                <img src="images/logo.png" class="imglogo" />
            </div>--%>
            <div class="return_ico" onclick="window.location.href='Index.aspx'">
                 <img src="Images/return.png" class="return_img" />
            </div>
        </div>

        <div class="pLeft10 pRight10">

            <div class="divsearch">
                <table>
                    <tr>
                        <td class="tdsearchlogo">
                            <img src="images/searchlogo.png" class="imglogosearch" />

                        </td>
                        <td class="tdkeyword">

                            <div class="divkeyword">

                                <table style="width:100%;">
                                    <tr>
                                        
                                        <td class="tdkeyword">
                                            <form onsubmit="return SearchKeyBord()"> 
                                            <input placeholder="请输入您想搜索的内容..." id="txtKeyWord" class="keyword" />
                                            </form>

                                        </td>
                                        <td>
                                            <img src="images/search.png" class="imgsearch" id="btnSearch" />

                                        </td>
                                    </tr>
                                </table>

                            </div>


                        </td>


                    </tr>
                </table>

            </div>

            <div>
                <h3 class="yuyue">专家预约</h3>

                <img src="images/split.png" class="split" />
            </div>

            <div id="doctorlist">

          

                    <input  id="btnMore" type="button"  value="显示更多..." class="btn btn-default" onclick="LoadMore()"/>

          
            </div>

        </div>

    </div>



</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Ju-Modules/bootstrap/js/bootstrap.min.js"></script>
<script src="/Scripts/Common.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.Min.js"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>

<script>
    var pageIndex = 1;
    var pageSize = 10;

    $(function () {

        LoadData();

        $("#btnSearch").click(function () {
            Search();


        });

        //$("#btnMore").click(function () {
        //    pageIndex++;
        //    LoadData();


        //});
        
        $(window).scroll(function () {
            totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
            if ($(document).height() <= totalheight) {
                $(btnMore).click();
            }
        });



    });

    function LoadData() {

        $.ajax({
            type: 'post',
            url: "Handler.ashx",
            data: { Action: "DoctorList", keyWord: $("#txtKeyWord").val(), pageIndex: pageIndex, pageSize: pageSize },
            dataType: "json",
            success: function (resp) {
                if (resp.result != null && resp.result.length > 0) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.result.length; i++) {
                        str.AppendFormat("<div onclick=\"window.location.href='Detail.aspx?id={0}'\">", resp.result[i].PID);
                        str.AppendFormat("<table class=\" mBottom6 mTop10\">");
                        str.AppendFormat("<tr>");
                        str.AppendFormat("<td>");
                        str.AppendFormat("<img class=\"head\" src=\"{0}\" />", resp.result[i].RecommendImg);

                        str.AppendFormat("</td>");
                        str.AppendFormat("<td >");
                        str.AppendFormat("<span class=\"name\">{0}</span>", resp.result[i].PName);
                        str.AppendFormat(" <span class=\"position\">{0}</span><br />", resp.result[i].ExArticleTitle_1);
                        //str.AppendFormat("<img src=\"images/address.png\" class=\"address\" />");
                        str.AppendFormat("<span class=\"hospital\">{0}</span><br/>", resp.result[i].ExArticleTitle_2);
                        str.AppendFormat("<span class=\"hospital\">擅长:{0}</span>", resp.result[i].ExArticleTitle_3);
                        str.AppendFormat("</td>");
                        str.AppendFormat("</tr>");
                        str.AppendFormat("</table>");
                        //str.AppendFormat(" <img src=\"images/split.png\" class=\"split\" />");
                        str.AppendFormat(" <hr class=\"split\" />");
                        str.AppendFormat(" </div>");

                    }
                    $("#btnMore").before(str.ToString());


                }
                else {

                    if (pageIndex == 1 && resp.result.length == 0) {
                        $("#doctorlist").html("<div class=\"nodata\">搜索不到专家,请换一个关键词。<div/>");
                        
                    }
                    if (pageIndex > 1 && resp.result.length == 0) {

                        $("#btnMore").val("没有更多了");
                    }


                }



            }
        });

    }

    function Search() {
        pageIndex = 1;
        $("#doctorlist").html("<input id=\"btnMore\" onclick=\"LoadMore()\" type=\"button\"  value=\"显示更多...\" class=\"btn btn-default\"/>");
        
        LoadData();

    }
    function SearchKeyBord() {
        pageIndex = 1;
        $("#doctorlist").html("<input id=\"btnMore\" onclick=\"LoadMore()\" type=\"button\"  value=\"显示更多...\" class=\"btn btn-default\"/>");

        LoadData();
        return false;
    }
    function LoadMore() {

        pageIndex++;
        LoadData();

    }

</script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=config.WebsiteTitle%>",
            desc: "<%=config.WebsiteDescription%>",
            //link: '', 
            imgUrl: "<%=config.WebsiteImage%>"
        })
    })
</script>
</html>
