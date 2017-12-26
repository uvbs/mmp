<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityLlists.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ActivityLlists" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title>
        <%=aconfig.ShowName %>
    </title>
    <link rel="stylesheet" href="Style/styles/css/style.css?v=0.0.3" />
    <link href="Style/styles/css/style1.css" rel="stylesheet" type="text/css" />
    <% if (aconfig.ColorTheme == 1)
       { %>
    <link href="Style/styles/css/green.css" rel="stylesheet" type="text/css" />
    <%} %>
    <% if (aconfig.ColorTheme == 2)
       { %>
    <link href="Style/styles/css/orange.css" rel="stylesheet" type="text/css" />
    <%} %>
    <style type="text/css">
        .title
        {
            font-weight: bold;
        }
        .icon img
        {
            max-width: 20px;
        }
        .submit .leftlink, .submit .centerlink, .submit .rightlink
        {
            display: block;
            width: <%=ColumnWidth%>%;
            height: inherit;
            padding-top: 5px;
            box-sizing: border-box;
        }
        body
        {
            font-family: Microsoft YaHei;
        }
        .maincontext .maincontent .iconfont, .maincontext .maincontent .title
        {
            color: Black;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="categorybox">
        <div class="title"><%=aconfig.ShowName %></div>
        <ul class="categorylist">
            <%=sbCategory.ToString() %>
        </ul>
    </div>
    <div id="screentouch"></div>
    <div class="topbar">
        <span class="category" id="categorybtn">
            <svg viewBox="0 0 128 128" >
                <path fill-rule="evenodd" clip-rule="evenodd"  d="M120.4,11.3H7.6c-4.2,0-7.6,3.4-7.6,7.6v3c0,4.2,3.4,7.6,7.6,7.6
                        h112.8c4.2,0,7.6-3.4,7.6-7.6v-3C128,14.7,124.6,11.3,120.4,11.3z M120.4,54H7.6C3.4,54,0,57.4,0,61.6v3c0,4.2,3.4,7.6,7.6,7.6
                        h112.8c4.2,0,7.6-3.4,7.6-7.6v-3C128,57.4,124.6,54,120.4,54z M120.4,96.6H7.6c-4.2,0-7.6,3.4-7.6,7.6v3c0,4.2,3.4,7.6,7.6,7.6
                        h112.8c4.2,0,7.6-3.4,7.6-7.6v-3C128,100,124.6,96.6,120.4,96.6z"/>
            </svg>
        </span>
        <input class="search" type="text" id="txtName">
        <span class="searchbtn" onclick="searchName()">
            <svg viewBox="0 0 128 128" >
                <path d="M52 104c11.472 0 22.071-3.723 30.67-10.016l31.673 31.673c3.124 3.124 8.189 3.124 11.314 0s3.124-8.189 0-11.314l-31.673-31.673c6.293-8.599 10.016-19.198 10.016-30.67 0-28.719-23.281-52-52-52s-52 23.281-52 52 23.281 52 52 52zM52 8c24.3 0 44 19.7 44 44s-19.7 44-44 44-44-19.7-44-44 19.7-44 44-44z" ></path>
            </svg>
        </span>
    </div>
   
     <div class="listbox" id="OjbList">
        <div class="bottom40"></div>
        <span class="button laodmoer" onclick="LoadData()">加载更多</span>
        <div class="bottom50"></div>
    </div>


<%--    <div class="submit">
        <a href="<%=aconfig.TheOrganizers%>" class="leftlink">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M1.813 58.93c-0.074 0.091-0.139 0.187-0.209 0.279-0.087 0.115-0.177 0.229-0.257 0.349-0.075 0.113-0.141 0.23-0.21 0.345-0.065 0.108-0.133 0.214-0.193 0.325-0.063 0.119-0.117 0.242-0.175 0.363-0.054 0.115-0.111 0.228-0.161 0.346-0.049 0.119-0.089 0.24-0.132 0.361-0.045 0.125-0.093 0.25-0.133 0.378-0.037 0.122-0.064 0.245-0.094 0.369-0.033 0.13-0.069 0.258-0.095 0.39-0.028 0.143-0.045 0.287-0.066 0.432-0.017 0.114-0.038 0.227-0.050 0.342-0.052 0.525-0.052 1.055 0 1.58 0.011 0.115 0.033 0.228 0.050 0.342 0.021 0.144 0.037 0.289 0.066 0.432 0.026 0.132 0.062 0.26 0.095 0.39 0.031 0.123 0.058 0.247 0.094 0.369 0.039 0.129 0.087 0.252 0.133 0.378 0.044 0.12 0.083 0.242 0.132 0.361s0.107 0.231 0.161 0.346c0.057 0.122 0.111 0.243 0.175 0.363 0.059 0.111 0.128 0.217 0.193 0.325 0.069 0.115 0.134 0.232 0.21 0.345 0.080 0.12 0.17 0.234 0.257 0.349 0.070 0.093 0.134 0.189 0.209 0.279 0.335 0.408 0.709 0.782 1.117 1.117 0.091 0.074 0.185 0.138 0.277 0.207 0.116 0.087 0.23 0.177 0.351 0.258 0.113 0.076 0.23 0.141 0.346 0.21 0.109 0.065 0.214 0.132 0.325 0.192 0.118 0.063 0.24 0.117 0.361 0.174 0.115 0.055 0.23 0.113 0.349 0.162 0.117 0.049 0.237 0.088 0.356 0.131 0.127 0.045 0.253 0.094 0.382 0.133 0.12 0.037 0.241 0.063 0.362 0.093 0.131 0.033 0.262 0.070 0.396 0.097 0.139 0.028 0.28 0.043 0.421 0.064 0.118 0.017 0.234 0.039 0.353 0.051 0.262 0.028 0.525 0.043 0.788 0.043h8v46.222c0 5.4 4.378 9.778 9.778 9.778h22.222v-48h32v48h22.222c5.4 0 9.778-4.378 9.778-9.778v-46.222h8c0.263 0 0.526-0.014 0.789-0.040 0.119-0.011 0.236-0.034 0.353-0.051 0.141-0.020 0.281-0.037 0.421-0.064 0.134-0.027 0.265-0.064 0.397-0.097 0.121-0.030 0.242-0.057 0.362-0.093 0.13-0.039 0.255-0.088 0.382-0.133 0.119-0.043 0.239-0.082 0.356-0.131 0.119-0.049 0.233-0.107 0.349-0.162 0.121-0.057 0.242-0.11 0.361-0.174 0.111-0.059 0.217-0.128 0.325-0.192 0.115-0.069 0.233-0.134 0.346-0.21 0.121-0.081 0.235-0.171 0.351-0.258 0.093-0.069 0.187-0.133 0.277-0.207 0.408-0.335 0.782-0.709 1.117-1.117 0.074-0.091 0.139-0.187 0.209-0.279 0.087-0.115 0.177-0.229 0.257-0.349 0.076-0.113 0.141-0.23 0.21-0.345 0.065-0.109 0.132-0.214 0.193-0.325 0.064-0.12 0.118-0.243 0.175-0.365 0.054-0.115 0.111-0.227 0.16-0.344 0.050-0.12 0.090-0.242 0.133-0.363 0.044-0.124 0.093-0.248 0.131-0.376 0.037-0.122 0.064-0.245 0.095-0.369 0.033-0.13 0.069-0.258 0.095-0.39 0.028-0.143 0.045-0.287 0.066-0.432 0.017-0.114 0.038-0.227 0.050-0.342 0.053-0.525 0.053-1.055 0-1.581-0.011-0.115-0.033-0.227-0.050-0.342-0.020-0.144-0.037-0.289-0.066-0.432-0.026-0.133-0.062-0.261-0.095-0.39-0.031-0.123-0.058-0.247-0.095-0.369-0.038-0.128-0.086-0.251-0.131-0.376-0.043-0.122-0.084-0.243-0.133-0.363-0.049-0.117-0.106-0.23-0.16-0.344-0.057-0.122-0.111-0.245-0.175-0.364-0.059-0.111-0.128-0.217-0.193-0.325-0.069-0.115-0.134-0.232-0.21-0.345-0.080-0.12-0.17-0.234-0.257-0.349-0.070-0.093-0.134-0.188-0.209-0.279-0.168-0.205-0.345-0.401-0.531-0.587l-55.999-56.002c-1.448-1.448-3.448-2.343-5.657-2.343s-4.209 0.895-5.657 2.343l-55.999 55.999c-0.187 0.187-0.363 0.383-0.531 0.587z"></path>
                </svg>
            </span>
            <span class="text"><%=string.IsNullOrEmpty(aconfig.OrganizerName) ? "主办方" : aconfig.OrganizerName%></span>
        </a>

        <a href="<%=aconfig.MyRegistration%>" class="rightlink">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M112 88h-24c-4.419 0-8-3.581-8-8v-4.291c9.562-5.534 16-15.866 16-27.709v-16c0-17.673-14.327-32-32-32s-32 14.327-32 32v16c0 11.843 6.438 22.174 16 27.709v4.291c0 4.419-3.581 8-8 8h-24c-8.836 0-16 7.163-16 16v14.222c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-14.222c0-8.837-7.163-16-16-16z"></path>
                </svg>
            </span>
           <span class="text"><%=string.IsNullOrEmpty(aconfig.MyRegistrationName) ? "我的报名" : aconfig.MyRegistrationName%></span>
        </a>
        
        <a href="<%=aconfig.Activities%>" class="centerlink current">
            <span class="icon">
                <svg viewBox="0 0 128 128">
                    <path d="M44.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M84.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M118.222 16h-22.222v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-16v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-22.222c-5.4 0-9.778 4.378-9.778 9.778v92.445c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-92.445c0-5.4-4.378-9.778-9.778-9.778zM120 120h-112v-80h112v80z"></path>
                </svg>
            </span>
           <span class="text"><%=string.IsNullOrEmpty(aconfig.ActivitiesName) ? "活动日历" : aconfig.ActivitiesName%></span>
        </a>
    </div>--%>


    <%if (ToolBarGroup.Count > 0)
      {%>
          
          <div class="submit">

      <%} %>
     
    <%foreach (var item in ToolBarGroup)
      {%>
         <a href="<%=item.ToolBarTypeValue%>" class="leftlink">
            <span class="icon">

            <%if (!string.IsNullOrEmpty(item.ToolBarImage))
              {
                  Response.Write(string.Format("<span class=\"{0}\"></span>", item.ToolBarImage));
              }
              else
              {%>
              <img  src="<%=item.ImageUrl %>" alt=""/>

              <%}%>
                


            </span>
            <span class="text"><%=item.ToolBarName%></span>
           </a>
      <%} %>

      <%if (ToolBarGroup.Count > 0)
        {%>
          </div>
      <%} %>

             <%if (ToolBarGroup.Count == 0)
               {%>
        
             <div class="submit">
        <a href="/App/Cation/Wap/ActivityLlists.aspx" class="leftlink">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M1.813 58.93c-0.074 0.091-0.139 0.187-0.209 0.279-0.087 0.115-0.177 0.229-0.257 0.349-0.075 0.113-0.141 0.23-0.21 0.345-0.065 0.108-0.133 0.214-0.193 0.325-0.063 0.119-0.117 0.242-0.175 0.363-0.054 0.115-0.111 0.228-0.161 0.346-0.049 0.119-0.089 0.24-0.132 0.361-0.045 0.125-0.093 0.25-0.133 0.378-0.037 0.122-0.064 0.245-0.094 0.369-0.033 0.13-0.069 0.258-0.095 0.39-0.028 0.143-0.045 0.287-0.066 0.432-0.017 0.114-0.038 0.227-0.050 0.342-0.052 0.525-0.052 1.055 0 1.58 0.011 0.115 0.033 0.228 0.050 0.342 0.021 0.144 0.037 0.289 0.066 0.432 0.026 0.132 0.062 0.26 0.095 0.39 0.031 0.123 0.058 0.247 0.094 0.369 0.039 0.129 0.087 0.252 0.133 0.378 0.044 0.12 0.083 0.242 0.132 0.361s0.107 0.231 0.161 0.346c0.057 0.122 0.111 0.243 0.175 0.363 0.059 0.111 0.128 0.217 0.193 0.325 0.069 0.115 0.134 0.232 0.21 0.345 0.080 0.12 0.17 0.234 0.257 0.349 0.070 0.093 0.134 0.189 0.209 0.279 0.335 0.408 0.709 0.782 1.117 1.117 0.091 0.074 0.185 0.138 0.277 0.207 0.116 0.087 0.23 0.177 0.351 0.258 0.113 0.076 0.23 0.141 0.346 0.21 0.109 0.065 0.214 0.132 0.325 0.192 0.118 0.063 0.24 0.117 0.361 0.174 0.115 0.055 0.23 0.113 0.349 0.162 0.117 0.049 0.237 0.088 0.356 0.131 0.127 0.045 0.253 0.094 0.382 0.133 0.12 0.037 0.241 0.063 0.362 0.093 0.131 0.033 0.262 0.070 0.396 0.097 0.139 0.028 0.28 0.043 0.421 0.064 0.118 0.017 0.234 0.039 0.353 0.051 0.262 0.028 0.525 0.043 0.788 0.043h8v46.222c0 5.4 4.378 9.778 9.778 9.778h22.222v-48h32v48h22.222c5.4 0 9.778-4.378 9.778-9.778v-46.222h8c0.263 0 0.526-0.014 0.789-0.040 0.119-0.011 0.236-0.034 0.353-0.051 0.141-0.020 0.281-0.037 0.421-0.064 0.134-0.027 0.265-0.064 0.397-0.097 0.121-0.030 0.242-0.057 0.362-0.093 0.13-0.039 0.255-0.088 0.382-0.133 0.119-0.043 0.239-0.082 0.356-0.131 0.119-0.049 0.233-0.107 0.349-0.162 0.121-0.057 0.242-0.11 0.361-0.174 0.111-0.059 0.217-0.128 0.325-0.192 0.115-0.069 0.233-0.134 0.346-0.21 0.121-0.081 0.235-0.171 0.351-0.258 0.093-0.069 0.187-0.133 0.277-0.207 0.408-0.335 0.782-0.709 1.117-1.117 0.074-0.091 0.139-0.187 0.209-0.279 0.087-0.115 0.177-0.229 0.257-0.349 0.076-0.113 0.141-0.23 0.21-0.345 0.065-0.109 0.132-0.214 0.193-0.325 0.064-0.12 0.118-0.243 0.175-0.365 0.054-0.115 0.111-0.227 0.16-0.344 0.050-0.12 0.090-0.242 0.133-0.363 0.044-0.124 0.093-0.248 0.131-0.376 0.037-0.122 0.064-0.245 0.095-0.369 0.033-0.13 0.069-0.258 0.095-0.39 0.028-0.143 0.045-0.287 0.066-0.432 0.017-0.114 0.038-0.227 0.050-0.342 0.053-0.525 0.053-1.055 0-1.581-0.011-0.115-0.033-0.227-0.050-0.342-0.020-0.144-0.037-0.289-0.066-0.432-0.026-0.133-0.062-0.261-0.095-0.39-0.031-0.123-0.058-0.247-0.095-0.369-0.038-0.128-0.086-0.251-0.131-0.376-0.043-0.122-0.084-0.243-0.133-0.363-0.049-0.117-0.106-0.23-0.16-0.344-0.057-0.122-0.111-0.245-0.175-0.364-0.059-0.111-0.128-0.217-0.193-0.325-0.069-0.115-0.134-0.232-0.21-0.345-0.080-0.12-0.17-0.234-0.257-0.349-0.070-0.093-0.134-0.188-0.209-0.279-0.168-0.205-0.345-0.401-0.531-0.587l-55.999-56.002c-1.448-1.448-3.448-2.343-5.657-2.343s-4.209 0.895-5.657 2.343l-55.999 55.999c-0.187 0.187-0.363 0.383-0.531 0.587z"></path>
                </svg>
            </span>
            <span class="text">首页</span>
        </a>

        <a href="/App/Cation/Wap/MyActivityLlists.aspx" class="rightlink">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M112 88h-24c-4.419 0-8-3.581-8-8v-4.291c9.562-5.534 16-15.866 16-27.709v-16c0-17.673-14.327-32-32-32s-32 14.327-32 32v16c0 11.843 6.438 22.174 16 27.709v4.291c0 4.419-3.581 8-8 8h-24c-8.836 0-16 7.163-16 16v14.222c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-14.222c0-8.837-7.163-16-16-16z"></path>
                </svg>
            </span>
           <span class="text">我的报名</span>
        </a>
        
        <a href="/App/Cation/Wap/ActivityLlists.aspx" class="centerlink current">
            <span class="icon">
                <svg viewBox="0 0 128 128">
                    <path d="M44.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M84.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M118.222 16h-22.222v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-16v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-22.222c-5.4 0-9.778 4.378-9.778 9.778v92.445c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-92.445c0-5.4-4.378-9.778-9.778-9.778zM120 120h-112v-80h112v80z"></path>
                </svg>
            </span>
           <span class="text">活动列表</span>
        </a>
    </div>

      <%} %>
</section>
    <script data-main="Style/src/list" src="/lib/requirejs/require.js"></script>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script type="text/javascript">
        var cType = "";
        var pageIndex = 1;
        var pageSize = 30;
        var preId = '<%=cateRootId%>';
        $(function () {
            InitData();
            $(".categorylist>li").click(function () {
                pageIndex = 1;
                $(".listbox>a").remove();
                cType = $(this).attr("v");
                $("#OjbList").html("<div class=\"bottom40\"></div><span class=\"button laodmoer\" onclick=\"LoadData()\">加载更多</span><div class=\"bottom50\"></div>");
                InitData();
                $(".categorylist>li").removeClass("list current").addClass("list");
                $(this).addClass("list current")
            });

        });

        function LoadData() {
            if (pageIndex == 1) {
                pageIndex++;
            }
            InitData();
        }

        function FormatDate(value) {

            if (value == null || value == "") {
                return "";
            }
            var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
            var month = PadLeft(date.getMonth() + 1, 2);
            var currentDate = PadLeft(date.getDate(), 2);
            var hour = PadLeft(date.getHours(), 10);
            var minute = PadLeft(date.getMinutes(), 2);
            var second = PadLeft(date.getSeconds(), 2);
            return date.getFullYear() + "/" + month + "/" + currentDate + " " + hour + ":" + minute;
        }

        function FormatStartEndDate(value, value1) {
            if (value == null || value == "") {
                return "";
            }
            var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
            var year = date.getFullYear();
            var month = PadLeft(date.getMonth() + 1, 2);
            var currentDate = PadLeft(date.getDate(), 2);
            var hour = PadLeft(date.getHours(), 2);
            var minute = PadLeft(date.getMinutes(), 2);
            var second = PadLeft(date.getSeconds(), 2);

            if (value1 == null || value1 == "") {
                return year + "/" + month + "/" + currentDate + " " + hour + ":" + minute;
            }

            var date1 = new Date(parseInt(value1.replace("/Date(", "").replace(")/", ""), 10));
            var year1 = date1.getFullYear();
            var month1 = PadLeft(date1.getMonth() + 1, 2);
            var currentDate1 = PadLeft(date1.getDate(), 2);
            var hour1 = PadLeft(date1.getHours(), 2);
            var minute1 = PadLeft(date1.getMinutes(), 2);
            var second1 = PadLeft(date1.getSeconds(), 2);
            if (year1 == year && month1 == month && currentDate1 == currentDate) {
                return year + "/" + month + "/" + currentDate + " " + hour + ":" + minute + " ~ " + hour1 + ":" + minute1;
            } else {
                return year + "/" + month + "/" + currentDate + " " + hour + ":" + minute + " ~ " + year1 + "/" + month1 + "/" + currentDate1 + " " + hour1 + ":" + minute1;
            }
        }

        function PadLeft(num, long) {
            var temp = "0000000000" + num;
            return temp.substr(temp.length - long);
        }
        var name = "";
        function searchName() {
            pageIndex = 1;
            $(".listbox>a").remove();
            name = $("#txtName").val();
            InitData();
        }

        function InitData() {

            $.ajax({
                type: 'post',
                url: "/Handler/OpenGuestHandler.ashx",
                data: { Action: "GetActivityDataInfos", pageIndex: pageIndex, pageSize: pageSize, ActivityName: name, ctype: cType, preId: preId },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    if (resp.Status == 1) {
                        if (resp.ExObj == null) {
                            return;
                        }
                        //普版
                        // if (resp.ExInt == 0)


                        $.each(resp.ExObj, function (index, Item) {

                            //if (Item.ActivityStatus==0) {


                            str.AppendFormat('<a href="/{0}/details.chtml" class="maincontext"> ', Item.JuActivityIDHex);
                            str.AppendFormat('<img src="{0}">', Item.ThumbnailsPath);
                            str.AppendFormat('<div class="maincontent">');
                            str.AppendFormat('<span class="title">{0}', Item.ActivityName);
                            //if (Item.IsFee == 1) {
                            //    str.AppendFormat('<label class="paytag" >付费</label>');
                            //}
                            str.AppendFormat('</span>');
                            if (Item.ActivityAddress) {
                                str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span>', Item.ActivityAddress);
                            }
                            str.AppendFormat('<span class="time" style="font-size:12px;"><i class="iconfont icon-shijian"></i>{0}</span>', FormatStartEndDate(Item.ActivityStartDate, Item.ActivityEndDate));
                            //if (Item.ActivityEndDate) {
                            //    str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', FormatDate(Item.ActivityEndDate));
                            //}
                            str.AppendFormat('<span class="baomingstatus ">');
                            if (Item.ActivityStatus == 1) {
                                str.AppendFormat(' <span class="text">已停止</span>');
                                str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                            } else if (Item.ActivityStatus == 2) {
                                str.AppendFormat(' <span class="text">已满员</span>');
                                str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                            }
                            else {
                                str.AppendFormat(' <span class="text">进行中</span>');

                                str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                            }
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');


                            //}

                        });



                        //普版

                        //简版
                        // else if (resp.ExInt == 1) {
                        //     $.each(resp.ExObj, function (index, Item) {
                        //         if (index == 0 && pageIndex == 1) {
                        //             str.AppendFormat('<a href="/{0}/details.chtml" class="maincontext"> ', Item.JuActivityIDHex);
                        //             str.AppendFormat('<img src="{0}" style="max-width:100%;">', Item.ThumbnailsPath);
                        //             str.AppendFormat('<div class="maincontent">');
                        //             str.AppendFormat('<span class="title" style="font-weight:bold;">{0}</span>', Item.ActivityName);

                        //             str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>开始时间:{0}</span>', FormatDate(Item.ActivityStartDate));

                        //             if (Item.ActivityStartDate != "") {

                        //                 str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', FormatDate(Item.ActivityEndDate));

                        //             }

                        //             str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span><span class="wybm">进入</span>', Item.ActivityAddress);
                        //             //str.AppendFormat('<span style="float:right;margin-right:50px;">报名&nbsp;<label style="color:red;">{0}</label></span>', Item.SignUpCount);
                        //             str.AppendFormat('<span class="baomingstatus ">');
                        //             if (Item.IsHide == 1) {
                        //                 str.AppendFormat(' <span class="text">已停止</span>');
                        //             } else {
                        //                 str.AppendFormat(' <span class="text">进行中</span>');
                        //             }
                        //             str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                        //             str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                        //         }
                        //         else {

                        //              str.AppendFormat('<div class="rl-box">') ;
                        //              str.AppendFormat('<dl>') ;
                        //              str.AppendFormat('<dt><img src="{0}"></dt>', Item.ThumbnailsPath);
                        //              str.AppendFormat('<dd>') ;
                        //              str.AppendFormat('<h3>{0}</h3>', Item.ActivityName);

                        //              str.AppendFormat('<p class="time">开始时间:{0}<span class="bao"></p>', FormatDate(Item.ActivityStartDate));
                        //              if (Item.ActivityStartDate != "") {

                        //                  str.AppendFormat('<p class="time">结束时间:{0}<span class="bao"></p>', FormatDate(Item.ActivityEndDate));

                        //              }
                        //              str.AppendFormat('<p class="adress">{0}</p>', Item.ActivityAddress);
                        //              str.AppendFormat('<a class="baoming" href="/{0}/details.chtml">进入</a>', Item.JuActivityIDHex);
                        //              str.AppendFormat('</dd>') ;
                        //              str.AppendFormat('</dl>') ;
                        //              str.AppendFormat('<div class="hotbao"></div>') ;
                        //              str.AppendFormat(' </div>');

                        //         }

                        //     });
                        // }
                        // //简版
                        $(".laodmoer").before(str.ToString());
                        $(".laodmoer").show();
                        pageIndex++;
                    }
                    else {
                        $(".laodmoer").hide();
                    }
                }
            });
        };

    </script>
</body>
</html>
