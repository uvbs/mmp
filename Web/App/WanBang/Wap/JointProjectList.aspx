<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JointProjectList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.JointProjectList" %>

<!DOCTYPE html>
<html lang="zh-CN">
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
        <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>对接成果</title>
		<link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
       <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
       <style type="text/css">
       .search .area, .search .company {height:auto;}
       .search .search-btn{height:auto;}
    </style>
	</head>
	<body>
        <!-- Search -->
        <div class="search">

      <input type="text" id="txtName" class="company" placeholder="项目,企业,基地" style="width:70%"/>

             
          <input type="button" class="search-btn" value="搜索" id="btnSearch">                
            
        </div>
        <!--/ Search -->
        <!-- List -->
        <div class="main" id="objList">


        </div>
        
        <!--/ List -->
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
                 data: { Action: 'GetJointProjectList',  Name: $("#txtName").val(), PageIndex: PageIndex, PageSize: PageSize },
                 timeout: 60000,
                 dataType: "json",
                 success: function (resp) {
                     $("#btnSearch").html("搜索");
                     if (resp.ExObj == null) { return; }
                     var listHtml = '';
                     var str = new StringBuilder();
                     for (var i = 0; i < resp.ExObj.length; i++) {
                         //构造视图模板

                         //
                         str.AppendFormat('<dl class="comm">');
                         str.AppendFormat('<dt><img src="{0}"></dt>', resp.ExObj[i].Thumbnails);
                         str.AppendFormat('<dd>');
                         str.AppendFormat('<p>');
                         str.AppendFormat('{0}<br/>', resp.ExObj[i].ProjectName);
                         str.AppendFormat('企业：{0}<br/>', resp.ExObj[i].CompanyName);
                         str.AppendFormat('基地：{0}<br/><br/>', resp.ExObj[i].BaseName);
                         str.AppendFormat('</p>');
                         str.AppendFormat('</dd>');
                         str.AppendFormat('</dl>');

                         //

                     };
                     if (PageIndex == 1) {
                         if (resp.ExStr == "1") {
                             //显示下一页按钮
                             str.AppendFormat(' <button id="btnNext" type="button" class="dismore" onclick="BtnClick()">显示更多</button>');
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