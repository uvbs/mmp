<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Query.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Cer.Query" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title>优证+教育优质课程平台</title>
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="//static-files.socialcrmyun.com/lib/layer.mobile/need/layer.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/css/exam/all.min.css" rel="stylesheet" />
    <link href="//static-files.socialcrmyun.com/lib/layer.mobile/need/layer.css" rel="stylesheet" />    

    <style>
        body {
            font-size: 14px;
            margin: 0;
            overflow: auto;
            vertical-align: bottom !important;
        }



        .return_img {
            height: 20px;
        }

        .navbar-text {
            float: none;
            text-align: center;
        }

        .navbar-brand {
            padding: 15px 0px;
        }

        .search {
            text-align: center;
        }

            .search button {
                width: 95%;
                background-color: #FF7D00;
                color: white;
                border-color: #f08519;
                /*height: 40px;*/
                font-weight: bold;
            }
              .search a {
                width: 95%;
                background-color: #FF7D00;
                color: white;
                border-color: #f08519;
                /*height: 40px;*/
                line-height:20px;
                font-weight: bold;
            }


                .search button:hover {
                    background-color: #f08519;
                    color: white;
                    border-color: #f08519;
                }

        .footer-bar {
            position: absolute;
            bottom: 0;
            left: 0;
            height: 60px;
            width: 100%;
            border-top: 1px solid #ddd;
            z-index: 30;
            display: -webkit-box;
        }

        #divResult {
            text-align: center;
            margin-top: 20px;
            margin-bottom: 80px;
        }

            #divResult img {
                width: 100%;
            }

        .navbar-default {
            background-color: white;
        }

        .navbar-text {
            font-weight: bold;
        }

        .navbar {
            margin-bottom: 0px;
        }

        table {
            width:98%;
            border-collapse: separate;
            border-spacing: 0px 10px;
            text-align: right;
        }

        .red {
            color: red;
        }

        #divNoResult {
            display: none;
            text-align: center;
            background-color: #FAFAF9;
            width: 90%;
            margin-left: 5%;
            height: 150px;
            vertical-align: middle;
            border: 1px dashed #ddd;
            border-radius: 5px;
        }

            #divNoResult img {
                max-width: 80px;
            }

        .form-control {
            width: 98%;
            border: 1px solid #ccc;
        }

        .notresult {
            margin-top: 25px;
        }

        .content .foot {
            font-size: 14px !important;
        }

            .content .foot .col {
                padding-top: 9px !important;
                padding-bottom: 8px !important;
            }

        .foot {
            margin: 0;
            filter:alpha(Opacity=96);-moz-opacity:0.96;opacity: 0.96;
        }

            .foot img {
                width: 25px;
            }

            .foot .col {
                margin-top: 2px;
                padding: 0px;
            }
                    .orange {
            color: #FF7E00 !important;
        }

    </style>
</head>

<body>

    <nav class="navbar navbar-default" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <a class="navbar-brand" href="javascript:history.go(-1);">
                    <img src="../Images/back.png" class="return_img" />
                </a>
                <p class="navbar-text">证书查询</p>

            </div>
        </div>
    </nav>



    <table>
        <tr>
            <td>姓名:&nbsp;</td>
            <td>
                <input id="txtName" type="text" class="form-control" placeholder="请输入姓名" />
            </td>
        </tr>
        <tr>
            <td>身份证号:&nbsp;</td>
            <td>
                <input id="txtIdCode" type="text" class="form-control" placeholder="请输入身份证号">
            </td>
        </tr>
        <tr>
            <td>证书编号:&nbsp;</td>
            <td>

                <input id="txtCode" type="text" class="form-control" placeholder="请输入证书编号" />

            </td>
        </tr>

    </table>




    <div class="search">
        <button type="button" class="btn btn-default" id="btnSumbit">搜索证书</button>
       <br/><br/>
        <a href="http://www.costic.org/index.php?r=page/ShowCer" class="btn btn-default">中国商业联合会证书查验系统</a>
    </div>

    <div>


        <hr />
    </div>


    <div id="divResult">
    </div>

    <div id="divNoResult">


        <img src="../images/search.png" class="notresult">
        <br />
        没有搜索到证书


    </div>


 <div class="row foot text-center">
       <a class="col" href="/customize/comeoncloud/Index.aspx?key=MallHome">
            <img src="../images/home.png" />
            <br />
            首页</a>

        <a class="col" href="/Customize/YouZheng/Course/List.aspx">
            <img src="../images/book.png" />
            <br />
            课程</a>


        <a class="col" href="/Customize/YouZheng/Activity/List.aspx">
            <img src="../images/huodong.png" />
            <br />
            活动</a>
         <a class="col" href="/Customize/YouZheng/Activity/List.aspx?isgame=1">
            <img src="../images/jiangbei.png" />
            <br />
            竞赛</a>

         <a class="col orange" href="/customize/comeoncloud/Index.aspx?key=PersonalCenter">
            <img src="../images/myselect.png" />
            <br />
            我的</a>

    </div>





</body>
 <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/Scripts/jquery-2.1.1.min.js"></script>
 <script src="//static-files.socialcrmyun.com/lib/layer.mobile/layer.m.js" type="text/javascript" ></script>
<script src="/Ju-Modules/bootstrap/js/bootstrap.min.js"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script src="/Scripts/StringBuilder.Min.js"></script>
<script>

    window.alert = function (msg) {


        layer.open({
            content: msg,
            btn: ['OK']
        });


    }

    $(btnSumbit).click(function () {
        if ($.trim($('#txtName').val()) == "") {
            alert("请填写姓名");
            return;
        }
        if ($.trim($('#txtIdCode').val()) == "") {
            alert("请填写身份证号");
            return ;
        }
        if ($.trim($(txtCode).val()) == "" ) {
            alert("请填写证书编号");
            return;
        }
        layer.open({ type: 2 });
        $.ajax({
            type: 'post',
            url: "/serv/api/exam/cer/get.ashx",
            data: { code: $(txtCode).val(), name: "", idcode: ""},
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                layer.closeAll();
                if (resp.status == true) {

                    var sb = new StringBuilder();
                    sb.AppendFormat("<span>查询到&nbsp;<label class=\"red\"\>{0}</label>&nbsp;条结果</span><br/><br/>", resp.result.length);
                    for (var i = 0; i < resp.result.length; i++) {

                        sb.AppendFormat("<img src=\"{0}\"/ class=\"imgPreview\">", resp.result[i].ImageUrl);
                        sb.AppendFormat("<br/>");
                        sb.AppendFormat("<br/>");
                    }


                    $("#divResult").html(sb.ToString());
                    review();
                    $("#divNoResult").hide();
                    $("#divResult").show();
                }
                else {

                    $("#divNoResult").show();
                    $("#divResult").hide();

                }


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    layermsg("超时，请重新查询");

                }
            }
        });




    });

    function review() {
        var __mls = [];
        $.each($('img'), function (i, item) {
            var _this = $(this);
            if (item.src && _this.hasClass('imgPreview')) {
                __mls.push(item.src);
                $(item).click(function (e) {
                    
                    wx.previewImage({
                        current: this.src,
                        urls: __mls
                    });
                });
            }
        });

    }

</script>
</html>
