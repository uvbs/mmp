<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="PlanList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.PlanList" %>
<!DOCTYPE html>
<html>
<head>
    <title>至云移动营销管理平台</title>
    <!--[if lte IE 9]><link rel="stylesheet" href="/css/game/game-ie.css"><![endif]-->
    <link href="/css/game/game.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/game/inpagecommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <link href="/MainStyle/Res/easyui/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/MainStyle/Res/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="/MainStyle/Res/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/MainStyle/Res/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>


</head>
<body class="insidepage">
    <div class="breadnav">微游戏>游戏列表</div>
    <div class="mainbox">
        <div class="handlebar">
        <a href="javascript:void(0);" class="icobtn redbtn" onclick="Delete()">
                <span class="btnico delico"></span>
            </a>
            <a href="GameList.aspx" class="icobtn greenbtn">
                <span class="btnico addico"></span>
                <span class="text">添加游戏</span>
            </a>
            <%--
            <a href="javascript:void(0);" class="icobtn redbtn">
                <span class="btnico delico"></span>
                <span class="text">添加广告</span>
            </a>
            <a href="javascript:void(0);" class="nbtn">
                <span class="text">搜索</span>
            </a>
            <a href="javascript:void(0);" class="nbtn forbidbtn">
                <span class="text">搜索</span>
            </a>
            <a href="javascript:void(0);" class="icobtn redbtn">
                <span class="btnico delico"></span>
            </a>
            <div class="clear"></div>--%>
<%--            <div class="bulkactionbox">
                <label for="dd" class="nlabel">批量操作：</label>
                <select name="" id="dd">
                    <option value="0">选择操作</option>
                    <option value="1">删除</option>
                    <option value="2">上架</option>
                    <option value="3">下架</option>
                    <option value="4">置顶</option>
                    <option value="5">排序</option>
                </select>
                <button class="nbtn">
                    <span class="text">执行</span>
                </button>
            </div>--%>
            <div class="searchbox">
                <input id="txtPlanName" placeholder="输入游戏名称" type="text" class="middletext" />
                <button id="btnSearch" class="icobtn">
                    <span class="btnico searchico"></span>
                    <span class="text">搜索</span>
                </button>
            </div>
        </div>
        <div class="tabletitle">
            <table>
                <tr>
                    <td class="tabletd1">
                        <label for="cball"><input type="checkbox" id="cball"/></label></td>
                    <td class="leftalign">名称</td>
                    <td>游戏</td>
                    <td>IP/PV</td>
                    <%--<td>
                        <select name="" id="dd">
                            <option value="0">全部分类</option>
                            <option value="1">分类1</option>
                            <option value="2">分类2</option>
                            <option value="3">分类3</option>
                            <option value="4">分类4</option>
                            <option value="5">分类5</option>
                        </select>
                    </td>--%>
                    <td>广告点击</td>
                    
                </tr>
            </table>
        </div>
        <div class="tableconcent">
            <table id="tablecontent" class="concenttable">

                
               
            </table>
        </div>
        <div class="tablefooter">
            <a id="btnFirstPage" class="icobtn bluebtn">
                <span class="btnico firstico"></span>
            </a>
            <a id="btnPrePage" class="icobtn bluebtn">
                <span class="btnico previco"></span>
            </a>
            <span class="textbox">
                第<input class="narrowtext" type="text" id="txtCurrentPage" value="1" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')">页,共<label id="lbltotalpage">0</label>页
            </span>
            <a  id="btnNextPage" class="icobtn bluebtn">
                <span class="btnico nextico"></span>
            </a>
            <a id="btnLastPage" class="icobtn bluebtn">
                <span class="btnico lastico"></span>
            </a>
            <a id="btnGoToPage" class="icobtn bluebtn">
                <span class="btnico refreshico"></span>
            </a>
            <span class="righttextbox">
                共<label id="lbltotalcount">0</label>记录
            </span>
        </div>
    </div>

        <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 400px; height: 330px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
                <br />
         <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
       
    
    </div>
</body>
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
    var tableheight = new tableHeight(".tableconcent", 110, [".breadnav", ".handlebar", ".tabletitle"]);
    var page=1;
    var rows = 10;
    $(function () {
        LoadData();

        $("#cball").click(function () {

            if ($(this).attr("checked") == "checked") {//全部已选 
                $("input[name=cbplan]").each(function () {
                    $(this).attr("checked", true);
                });
            }
            else {//全部未选 
                $("input[name=cbplan]").each(function () {
                    $(this).attr("checked", false);
                });
            }


        });


        $("#btnGoToPage").click(function () {

            var totalcount = $("#lbltotalcount").text();
            var totalpage = Math.ceil(totalcount / rows);
            page = $("#txtCurrentPage").val();
            if (page <= 0) {
                page = 1;
                $("#txtCurrentPage").val("1");
            }
            if (page >= totalpage) {
                page = totalpage;
                $("#txtCurrentPage").val(totalpage);
            }
            LoadData();

        });
        $("#btnPrePage").click(function () {
            page--;
            if (page <= 0) {
                page = 1;
            }
            $("#txtCurrentPage").val(page);
            LoadData();


        });
        $("#btnNextPage").click(function () {

            page++;
            if (page <= 0) {
                page = 1;
            }
            var totalcount = $("#lbltotalcount").text();
            var totalpage = Math.ceil(totalcount / rows);
            if (page >= totalpage) {
                page = totalpage;
            }
            $("#txtCurrentPage").val(page);
            LoadData();


        });
        $("#btnFirstPage").click(function () {
            page = 1;
            $("#txtCurrentPage").val("1");
            LoadData();


        });
        $("#btnLastPage").click(function () {
            var totalcount = $("#lbltotalcount").text();
            var totalpage = Math.ceil(totalcount / rows);
            page = totalpage;
            $("#txtCurrentPage").val(page);
            LoadData();


        });



        $("#btnSearch").click(function () {
            page = 1;
            $("#txtCurrentPage").val("1");
            LoadData();

        });

    });

    function LoadData() {

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: 'QueryGamePlan', page: page, rows: rows, planName: $("#txtPlanName").val() },
            success: function (result) {

                var resp = $.parseJSON(result);
                if (resp.ExObj == null) { return; }
                $("#lbltotalcount").html(resp.ExInt);
                var listHtml = '';
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    var str = new StringBuilder();
                    str.AppendFormat('<tr>');
                    str.AppendFormat('<td class="tabletd1">');
                    str.AppendFormat('<label for="cb_{0}"><input type="checkbox" id="cb_{0}" name="cbplan"/></label>', resp.ExObj[i].AutoID);
                    str.AppendFormat('</td>');
                    str.AppendFormat('<td class="leftalign" onclick="ShowQRcode({0});">{1}</td>', resp.ExObj[i].AutoID, resp.ExObj[i].PlanName + "&nbsp[二维码]");
                    str.AppendFormat('<td onclick="ShowQRcode({0});">{1}</td>', resp.ExObj[i].AutoID, resp.ExObj[i].GameName);
                    str.AppendFormat('<td><a title="点击查看" href="EventDetail.aspx?pid={0}">{1}/{2}&nbsp;查看</a></td>', resp.ExObj[i].AutoID, resp.ExObj[i].IP, resp.ExObj[i].PV);
                    str.AppendFormat('<td><a title="点击查看" href="EventDetailClick.aspx?pid={0}">{1}&nbsp;查看</a></td>', resp.ExObj[i].AutoID, resp.ExObj[i].AdvertClickCount);
                    str.AppendFormat('</tr>');
                    listHtml += str.ToString();
                };

                if (listHtml != "") {
                    //填入列表
                    $("#tablecontent").html(listHtml);
                    var totalpage = Math.ceil(resp.ExInt / rows);
                    $("#lbltotalpage").html(totalpage);
                }
                else {
                    $('#tablecontent').html("无数据");
                }


            }
        });
    
    
    
    
    
    }

    function ShowQRcode(aid) {

        $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/Game/Game.aspx?pid='+ aid);
        $('#dlgSHowQRCode').dialog('open');
        var linkurl = "http://" + domain + "/Game/Game.aspx?pid=" + aid;
        $("#alinkurl").html(linkurl);
        $("#alinkurl").attr("href", linkurl);
    }

    //删除
    function Delete() {
        if (GetRowsIds().length <= 0) {
            alert("请选择要删除的广告");
            return false;
        }
        $.messager.confirm("系统提示", "确定删除选中广告?", function (o) {
            if (o) {
                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "DeleteGamePlan", ids: GetRowsIds(rows).join(',') },
                    success: function (result) {
                        alert(result);
                        LoadData();
                    }

                });
            }
        });


    }

    //获取选中行ID集合
    function GetRowsIds() {
        var ids = [];
        $("input[name=cbplan]:checked").each(function () {
            var id = $(this).attr("id").split('_')[1];
            ids.push(id);

        });
        return ids;
    }


</script>
</html>
