<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttentionList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.AttentionList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>我的关注</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link type="text/css" rel="stylesheet" href="../Css/wanbang.css">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <style type="text/css">
    .comm-btn{width:222px;}
    .main {margin-bottom:60px;}
    </style>
</head>
<body>



    <div class="main">
    <ul class="comm-btn" >
                <li class="on" id="liCompany">企业</li>
                <li style="left:73px;" id="liBase">基地</li>
                <li style="left:145px;background: #f48221;" id="liProject">项目</li>
    </ul>


      <div class="comm-cont" id="ulCompanyList">
     
     
      </div>

      <div class="comm-cont" id="ulBaseList" style="display:none;">
     
     
      </div>

     <div class="comm-cont" id="ulProjectList" style="display:none;">
     
     
      </div>
   </div>


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
               Response.Write("<a class=\"on\" href=\"BaseCenter.aspx\">");
           }
           else
           {
               Response.Write("<a class=\"on\" href=\"CompanyCenter.aspx\">");
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

     var BasePageIndex = 1; //基地第几页
     var BasePageSize = 5; //基地每页显示条数

     var CompanyPageIndex = 1; //企业第几页
     var CompanyPageSize = 5; //企业每页显示条数


     var ProjectPageIndex = 1; //项目第几页
     var ProjectPageSize = 5; //项目每页显示条数

     var handlerurl = "/Handler/WanBang/Wap.ashx";
     $(function () {
         LoadAttentionBaseList();
         LoadAttentionCompanyList();
         LoadAttentionProjectList();
         //
         $("#liCompany").click(function () {

             $(this).addClass('on');
             $("#liBase").removeClass('on');
             $("#liProject").removeClass('on');
             $("#ulCompanyList").show();
             $("#ulBaseList").hide();
             $("#ulProjectList").hide();


         });
         $("#liBase").click(function () {
             $(this).addClass('on');
             $("#liCompany").removeClass('on');
             $("#liProject").removeClass('on');
             $("#ulCompanyList").hide();
             $("#ulProjectList").hide();
             $("#ulBaseList").show();


         });

         $("#liProject").click(function () {
             $(this).addClass('on');
             $("#liCompany").removeClass('on');
             $("#liBase").removeClass('on');
             $("#ulCompanyList").hide();
             $("#ulBaseList").hide();
             $("#ulProjectList").show();


         });

     });

     //加载
     function LoadAttentionBaseList() {
         $.ajax({
             type: 'post',
             url: handlerurl,
             data: { Action: 'GetAttentionBaseList', PageIndex: BasePageIndex, PageSize: BasePageSize },
             timeout: 60000,
             dataType:"json",
             success: function (resp) 
             {
                 if (resp.ExObj == null) { return; }
                 var listHtml = '';
                 var str = new StringBuilder();
                 for (var i = 0; i < resp.ExObj.length; i++) {
                     //构造视图模板
                     //
                     str.AppendFormat('<dl class="comm" onclick="window.location.href=\'BaseDetail.aspx?id={0}\'">', resp.ExObj[i].AutoID);
                     str.AppendFormat('<dt><img src="{0}"></dt>', resp.ExObj[i].Thumbnails);
                     str.AppendFormat('<dd>');
                     str.AppendFormat('<p>');
                     str.AppendFormat('{0}<br/>', resp.ExObj[i].BaseName);
                     str.AppendFormat('所属区：{0}<br/>', resp.ExObj[i].Area);
                     str.AppendFormat('面积：{0}平方米        负责人：{1}<br>', resp.ExObj[i].Acreage, resp.ExObj[i].Contacts);
                     str.AppendFormat('地址：{0}', resp.ExObj[i].Address);
                     str.AppendFormat('</p>');
                     str.AppendFormat('</dd>');
                     str.AppendFormat('</dl>');

                 };
                 if (BasePageIndex == 1) {
                     if (resp.ExStr == "1") {
                         //显示下一页按钮
                         str.AppendFormat(' <button id="btnNextBase" type="button" class="dismore" onclick="BtnBaseClick()">显示更多</button>');

                         //

                         listHtml += str.ToString();
 
                         $("#ulBaseList").html(listHtml);

                     }
                     else {
                         listHtml += str.ToString();
                         if (listHtml == "") {
                             listHtml = "暂时没有关注的基地";
                         }
                         $("#ulBaseList").html(listHtml);

                     }



                 }
                 else {
                     listHtml += str.ToString();
                     if (listHtml != "") {
                         $("#btnNextBase").before(listHtml);
                     }
                     else {

                         $("#btnNextBase").html("没有更多了");
                         $("#btnNextBase").removeAttr("onclick");

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

     function BtnBaseClick() {

         BasePageIndex++;
         LoadAttentionBaseList();

     }

     //加载
     function LoadAttentionCompanyList() {
         $.ajax({
             type: 'post',
             url: handlerurl,
             data: { Action: 'GetAttentionCompanyList', PageIndex: CompanyPageIndex, PageSize: CompanyPageSize },
             timeout: 60000,
             dataType: "json",
             success: function (resp) {
                 if (resp.ExObj == null) { return; }
                 var listHtml = '';
                 var str = new StringBuilder();
                 for (var i = 0; i < resp.ExObj.length; i++) {
                     //构造视图模板


                     str.AppendFormat('<dl class="comm" onclick="window.location.href=\'CompanyDetail.aspx?id={0}\'">', resp.ExObj[i].AutoID);
                     str.AppendFormat('<dt><img src="{0}"></dt>', resp.ExObj[i].Thumbnails);
                     str.AppendFormat('<dd>');
                     str.AppendFormat('<p>');
                     str.AppendFormat('{0}<br/>', resp.ExObj[i].CompanyName);
                     str.AppendFormat('所属区：{0}<br/>', resp.ExObj[i].Area);
                     str.AppendFormat('营业执照号码：{0}<br>', resp.ExObj[i].BusinessLicenseNumber);
                     str.AppendFormat('地址：{0}', resp.ExObj[i].Address);
                     str.AppendFormat('</p>');
                     str.AppendFormat('</dd>');
                     str.AppendFormat('</dl>');


                 };
                 if (CompanyPageIndex == 1) {
                     if (resp.ExStr == "1") {
                         //显示下一页按钮
                         str.AppendFormat(' <button id="btnNextCompany" type="button" class="dismore" onclick="BtnCompanyClick()">显示更多</button>');

                         //

                         listHtml += str.ToString();
 
                         $("#ulCompanyList").html(listHtml);

                     }
                     else {
                         listHtml += str.ToString();
                         if (listHtml == "") {
                             listHtml = "暂时没有关注的企业";
                         }
                         $("#ulCompanyList").html(listHtml);

                     }



                 }
                 else {
                     listHtml += str.ToString();
                     if (listHtml != "") {
                         $("#btnNextCompany").before(listHtml);
                     }
                     else {

                         $("#btnNextCompany").html("没有更多了");
                         $("#btnNextCompany").removeAttr("onclick");

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


     function BtnCompanyClick() {

         CompanyPageIndex++;
         LoadAttentionCompanyList();

     }


     //
     //加载 项目
     function LoadAttentionProjectList() {
         $.ajax({
             type: 'post',
             url: handlerurl,
             data: { Action: 'GetAttentionProjectList', PageIndex: ProjectPageIndex, PageSize: ProjectPageSize },
             timeout: 60000,
             dataType: "json",
             success: function (resp) {
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
                     str.AppendFormat('周期:{0}<br/>', projectCycle);
                     //str.AppendFormat('发布企业：{0}<br/>', resp.ExObj[i].CompanyName);
                    // str.AppendFormat('{0}', resp.ExObj[i].InsertDate);
                     str.AppendFormat('</p>');
                     str.AppendFormat('</dd>');
                     str.AppendFormat('</dl>');
                     str.AppendFormat('</div>');




                 };
                 if (ProjectPageIndex == 1) {
                     if (resp.ExStr == "1") {
                         //显示下一页按钮
                         str.AppendFormat(' <button id="btnNextProject" type="button" class="dismore" onclick="BtnProjectClick()">显示更多</button>');

                         //

                         listHtml += str.ToString();

                         $("#ulProjectList").html(listHtml);

                     }
                     else {
                         listHtml += str.ToString();
                         if (listHtml == "") {
                             listHtml = "暂时没有关注的项目";
                         }
                         $("#ulProjectList").html(listHtml);

                     }



                 }
                 else {
                     listHtml += str.ToString();
                     if (listHtml != "") {
                         $("#btnNextProject").before(listHtml);
                     }
                     else {

                         $("#btnNextProject").html("没有更多了");
                         $("#btnNextProject").removeAttr("onclick");

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


     function BtnProjectClick() {

         ProjectPageIndex++;
         LoadAttentionProjectList();

     }
     //
 </script>


</html>
