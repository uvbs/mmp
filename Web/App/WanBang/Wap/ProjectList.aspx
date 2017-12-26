<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.ProjectList" %>
<!DOCTYPE html>
<html lang="zh-CN">
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
		<meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>项目列表</title>
        <link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
       <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
	</head>
	 <body>
        <!-- 项目列表 -->
        <div class="main">
            <div class="searchbox">
                <form method="post" action="#">
                    <div class="searchbox1">
              <select id="ddlArea" class="bao">
              <option value="">全部区县</option>
               <option value="黄浦区">黄浦区</option>
               <option value="长宁区">长宁区</option>
               <option value="徐汇区">徐汇区</option>
               <option value="静安区">静安区</option>
               <option value="杨浦区">杨浦区</option>
               <option value="虹口区">虹口区</option>
               <option value="闸北区">闸北区</option>
               <option value="普陀区">普陀区</option>
               <option value="浦东新区">浦东新区</option>
               <option value="宝山区">宝山区</option>
               <option value="闵行区">闵行区</option>
               <option value="金山区">金山区</option>
               <option value="嘉定区">嘉定区</option>
               <option value="青浦区">青浦区</option>
               <option value="松江区">松江区</option>
               <option value="奉贤区">奉贤区</option>
               <option value="崇明县">崇明县</option>
               
      </select>
      <select id="ddlCategory" class="zj">
                    <option value="">全部分类</option>
                    <option value="粘贴折叠">粘贴折叠</option>
                    <option value="成品包装">成品包装</option>
                    <option value="组件装配">组件装配</option>
                    <option value="加工制作">加工制作</option>
                    <option value="纺织串接">纺织串接</option>
                    <option value="缝纫整熨">缝纫整熨</option>
                    <option value="其它项目">其它项目</option>
                    <option value="阳光办公室">阳光办公室</option>
      </select>
                    </div>
                    <div class="searchbox2">
                        <input class="write" type="text" id="txtName" placeholder="项目名称"><input type="button" class="search-btn" value="搜索" id="btnSearch">
                    </div>
                </form>
            </div>
            <div id="objList"></div>
          
        </div>
        <!--/ 项目列表 -->
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
            <%if (ZentCloud.JubitIMP.Web.DataLoadTool.CheckWanBangLogin())
       {
         
           if (HttpContext.Current.Session[ZentCloud.JubitIMP.Web.SessionKey.WanBangUserType].ToString().Equals("0"))
           {
               Response.Write("<a href=\"BaseCenter.aspx\">");
           }
           else
           {
               Response.Write("<a href=\"CompanyCenter.aspx\">");
           }
       }
       else
       {
           Response.Write("<a href=\"Login.aspx\">");
       }
         %>
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
         $(function () {
             LoadList();
             $("#ddlArea").change(function () {
                 PageIndex = 1;
                 $("#txtName").val("");
                 $("#btnSearch").html("正在搜索...");
                 LoadList();

             })
             $("#ddlCategory").change(function () {
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
                 url: "/Handler/WanBang/Wap.ashx",
                 data: { Action: 'GetProjectList', Area: $("#ddlArea").val(), Category: $("#ddlCategory").val(), ProjectName: $("#txtName").val(), PageIndex: PageIndex, PageSize: PageSize },
                 timeout: 60000,
                 dataType: "json",
                 success: function (resp) {
                     $("#btnSearch").html("搜索");
                     if (resp.ExObj == null) { return; }
                     var listHtml = '';
                     var str = new StringBuilder();
                     for (var i = 0; i < resp.ExObj.length; i++) {
                         //构造视图模板
                         var statusclass = "";
                         switch (resp.ExObj[i].Status) {
                             case 1:
                                 statusclass = "project-box";
                                 break;
                             case 2:
                                 statusclass = "project-box project-on";
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

                           

                         }
                         
                         str.AppendFormat('<div class="{0}" onclick="window.location.href=\'ProjectDetail.aspx?id={1}\'">', statusclass, resp.ExObj[i].AutoID);
                         str.AppendFormat('<dl>');
                         str.AppendFormat('<dt><img src="{0}"></dt>', resp.ExObj[i].Thumbnails);
                         str.AppendFormat('<dd>');
                         str.AppendFormat('<p>');
                         str.AppendFormat('{0}<br/>', resp.ExObj[i].ProjectName);
                         str.AppendFormat('周期:{0}<br/>',projectCycle);
                         str.AppendFormat('发布企业：{0}<br/>',resp.ExObj[i].CompanyName);
                         str.AppendFormat('{0}',resp.ExObj[i].InsertDate);
                         str.AppendFormat('</p>');
                         str.AppendFormat('</dd>');
                         str.AppendFormat('</dl>');
                         str.AppendFormat('</div>');
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




 </script>

</html>