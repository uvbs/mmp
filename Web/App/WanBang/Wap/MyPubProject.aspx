<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyPubProject.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.MyPubProject" %>
<!DOCTYPE html>
<html lang="zh-CN">
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
		<meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>我的发布</title>
		<link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
        <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
	</head>

	<body>
        <!-- 我的发布 -->
        <div class="main">
            <div class="searchbox">
                <form method="post" action="#">
                    <div class="searchbox1">
                    <select id="ddlCategory" class="bao">
                    <option value="">全部分类</option>
                    <option value="粘贴折叠">粘贴折叠</option>
                    <option value="成品包装">成品包装</option>
                    <option value="组件装配">组件装配</option>
                    <option value="加工制作">加工制作</option>
                    <option value="纺织串接">纺织串接</option>
                    <option value="缝纫整熨">缝纫整熨</option>
                    <option value="其它项目">其它项目</option>
                     </select>

                   <select id="ddlStatus" class="zj">
                      <option value="">全部状态</option>
                      <option value="0">审核中</option>
                      <option value="1">征集中</option>
                      <option value="2">已结束</option>
                     </select>
                    </div>
                    <div class="searchbox2">
                        <input class="write" type="text" placeholder="项目名称" id="txtName"><input type="button" class="search-btn" value="搜索" id="btnSearch">
                    </div>
                </form>
            </div>
             <div id="objList">
             
             </div>
           
            
        </div>
        <!--/ 我的发布 -->
        <div class="blank"></div>

<!-- Nav -->
        <nav class="nav">
        	<a  href="Index.aspx">
        		<span class="pic">
                    <span class="iconfont icon-shouye"></span>
                </span>
        		<span class="t">首页</span>
        	</a>
        	<a href="/Web/list.aspx?cateid=164">
        		<span class="pic">
                    <span class="iconfont icon-huodong"></span>
                </span>
        		<span class="t">新闻</span>
        	</a>
            <a class="on" href="CompanyCenter.aspx">
        		<span class="pic">
                    <span class="iconfont icon-wode"></span>      
                </span>
        		<span class="t">我的</span>
        	</a>



        </nav> 
		<!--/ Nav -->
	</body>

     <script type="text/javascript">
         var PageIndex = 1; //第几页
         var PageSize = 5; //每页显示条数
         var handlerurl = "/Handler/WanBang/Wap.ashx";
         $(function () {
             LoadList();
             $("#ddlArea,#ddlCategory,#ddlStatus").change(function () {
                 PageIndex = 1;
                 $("#txtName").val("");
                 $("#btnSearch").html("正在搜索...");
                 LoadList();

             })
             $("#btnSearch").click(function () {
                 PageIndex = 1;
                 $(this).html("正在搜索...");
                 LoadList();

             })
         });

         function BtnClick() {

             PageIndex++;
             LoadList();


         }




         //加载列表分页
         function LoadList() {
             $.ajax({
                 type: 'post',
                 url: handlerurl,
                 data: { Action: 'GetMyProjectList', Category: $("#ddlCategory").val(), ProjectName: $("#txtName").val(), Status: $("#ddlStatus").val(), PageIndex: PageIndex, PageSize: PageSize },
                 timeout: 60000,
                 dataType: "json",
                 success: function (resp) {
                     $("#btnSearch").html("搜索");
                     if (resp.ExObj == null) { return; }
                     var listHtml = '';
                     var str = new StringBuilder();
                     for (var i = 0; i < resp.ExObj.length; i++) {
                         //构造视图模板
                         var status = "";
                         switch (resp.ExObj[i].Status) {
                             case 0:
                                 status = "<font color='red'>审核中</font>";
                                 break;
                             case 1:
                                 status = "<font color='green'>征集中</font>";
                                 break;
                             case 2:
                                 status = "已结束";
                                 break;
                             default:

                         }

                         var projectCycle = "";
                         switch (resp.ExObj[i].ProjectCycle) {
                             case 0:
                                 projectCycle = "临时(1个月以内)";
                                 break;
                             case 1:
                                 projectCycle = "短期(1-3个月)";
                                 break;
                             case 2:
                                 projectCycle = "中期(3-6个月)";
                                 break;
                             case 3:
                                 projectCycle = "长期(6-12个月)";
                                 break;
                             default:

                         }

                         var logistics = "";
                         switch (resp.ExObj[i].Logistics) {
                             case 0:
                                 logistics = "基地负责配送";
                                 break;
                             case 1:
                                 logistics = "企业负责配送";
                                 break;

                             default:

                         }
                         //

                         str.AppendFormat('<div class="wdfb-box">');
                         //str.AppendFormat('<form>');
                         str.AppendFormat('<dl>');
                         str.AppendFormat('<dt><img src="{0}"></dt>',resp.ExObj[i].Thumbnails);
                         str.AppendFormat('<dd>');
                         str.AppendFormat('<p>');
                         str.AppendFormat('{0}<br/>', resp.ExObj[i].ProjectName);
                         str.AppendFormat('分类:{0}<br/>', resp.ExObj[i].Category);
                         str.AppendFormat('项目周期：{0}<br/>', projectCycle);
                         str.AppendFormat('项目物流：{0}<br/>', logistics);
                         str.AppendFormat('项目状态：{0}', status);
                         str.AppendFormat('</p>');
                         str.AppendFormat('</dd>');
                         str.AppendFormat('</dl>');
                         str.AppendFormat('<div class="pub">');

                         if (resp.ExObj[i].Status ==1) {
                             str.AppendFormat('<a class="finish" onclick="EndProject(this);" id="{0}">结束项目</a>', resp.ExObj[i].AutoID);

                         }
                         str.AppendFormat('<a class="modify" href="ProjectAddEdit.aspx?action=edit&id={0}">修改</a></div>', resp.ExObj[i].AutoID);
                         //str.AppendFormat('</form>');
                         str.AppendFormat(' </div>');

            //

                     };
                     if (PageIndex == 1) {
                         if (resp.ExStr == "1") {
                             //显示下一页按钮
                           str.AppendFormat(' <button id="btnNext" type="button" class="dismore" onclick="BtnClick()">显示更多</button>');
                             //

                             listHtml += str.ToString();
                             $("#objList").html(listHtml);

                         }
                         else {
                             listHtml += str.ToString();
                             if (listHtml == "") {
                                 listHtml = "没有符合条件的项目";
                             }
                             $("#objList").html(listHtml);

                         }



                     }
                     else {
                         listHtml += str.ToString();
                         if (listHtml != "") {
                             $("#btnNext").before(listHtml);
                         }
                         else {

                             $("#btnNext").html("没有更多了");
                             $("#btnNext").removeAttr("onclick");

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


         //结束项目
         function EndProject(obj) {

             if (confirm("确定结束项目?")) {
                 $.ajax({
                     type: 'post',
                     url: handlerurl,
                     data: { Action: 'EndProject', AutoID: $(obj).attr("id") },
                     timeout: 60000,
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             alert("已结束项目");
                             $(obj).remove();
                             return false;

                         }
                         else {
                             alert(resp.Msg);
                         }


                     }

                 });


                 //
             }
             else {
                 return false;
             }


         }



     </script>

</html>