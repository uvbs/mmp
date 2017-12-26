<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyActivityLlists.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.MyActivityLlists1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link rel="stylesheet" href="Style/styles/css/style.css?v=0.0.3" />
    <link href="Style/styles/css/style1.css?v=20160724" rel="stylesheet" type="text/css" />
    <% if (aconfig.ColorTheme == 1)
       { %>
    <link href="Style/styles/css/green.css" rel="stylesheet" type="text/css" />
    <%} %>
    <% if (aconfig.ColorTheme == 2)
       { %>
    <link href="Style/styles/css/orange.css" rel="stylesheet" type="text/css" />
    <%} %>
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/buttons2.css" rel="stylesheet" />
    <title>我的活动</title>
    <style type="text/css">
        .articletag
        {
            width: 100%;
        }
        .articletag span
        {
            width: 100%;
            position: relative;
            border: solid 0px #fafafa !important;
        }
        table
        {
            width: 100%;
        }
        table tr td
        {
            width: 25%;
        }
        .articletag .active
        {
            border-top-right-radius: 0px;
            border-bottom-right-radius: 0px;
            right: 0px;
        }
        
        .articletag span
        {
            border: solid 0px #fafafa !important;
        }
        .tab
        {
            background-color: #fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/AB22C69BD456421299DF64D5C9156578.png") no-repeat;
            background-size: 100% 100%;
        }
        .articletag .current
        {
            background-color: #fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/493E8050E6C8473C9755BBD8612AB0E8.png") no-repeat;
            background-size: 100% 100%;
        }
        #objList2, #objList3, #objList4
        {
            display: none;
        }
        .button
        {
            color: White;
            background-color: #5acbdb;
        }
        .divloadmore
        {
            text-align: center;
        }
        .title
        {
            font-weight: bold;
        }
        
        
        .paytag
        {
            border: 1px solid #a5d24a;
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            display: block;
            float: right;
            padding: 5px;
            text-decoration: none;
            background: #cde69c;
            color: #638421;
            margin-right: 25px;
            margin-bottom: 5px;
            margin-top: 5px;
            font-family: helvetica;
            font-size: 13px;
        }
        
        .box
        {
            width: auto;
            margin-bottom:0px !important;
        }
        .icon img
        {
            max-width: 20px;
        }
        body
        {
            font-family: Microsoft YaHei;
        }
        .submit .leftlink, .submit .centerlink, .submit .rightlink
        {
            display: block;
            width: <%=ColumnWidth%>%;
            height: inherit;
            padding-top: 5px;
            box-sizing: border-box;
        }
        .maincontext .maincontent .iconfont, .maincontext .maincontent .title
        {
            color: Black;
        }
        .listbox {
           background:#FFF;
           padding:10px;
        }
        .laodmoer {
            display: block;
            width: 50%;
            height: 30px;
            line-height: 30px;
            text-align: center;
            margin: 0 auto;
        }
       
        
        .baomingstatus {
                display: block;
                width: 50px;
                height: 50px;
                position: absolute;
                bottom: 0px;
                right: 0px;
                overflow: hidden;
                border-bottom-right-radius: 5px;
            }
        .baomingstatus .text {
                display: block;
                width: 50px;
                height: 20px;
                text-align: center;
                line-height: 20px;
                -webkit-transform: translate3d(9px, 21px, 0px) rotate(-45deg);
                color: #fff;
                position: absolute;
                top: 0px;
                left: 0px;
                text-shadow: 0 0 4px rgba(0, 0, 0, 0.3);
                font-weight: bold;
            }
        .wybm{
            margin-top: 5px;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="articletag">
    <table style="width:100%;">
    <tr>
    <td><span class="article tab tab1 current"  id="rdoStatus1">即将开始</span></td>
    <td><span class="active tab tab2"  id="rdoStatus2">已签到</span></td>
    <td><span class="activerank tab tab3"  id="rdoStatus3">已结束</span></td>
    <td><span class="activerank tab tab4"  id="rdoStatus4">待支付</span></td>
    </tr>
    </table>
    </div>
     <div class="listbox" id="objList1">
       
        
        <div class="divloadmore" id="btnNext1"> 
        <span  class="button laodmoer" onclick="LoadData1()">加载更多</span>
        </div>
       
        <div class="bottom50"></div>
    </div>

       <div class="listbox" id="objList2">
        
        
        <div class="divloadmore" id="btnNext2"> 
        <span  class="button laodmoer" onclick="LoadData2()">加载更多</span>
        </div>
        <div class="bottom50"></div>
    </div>

       <div class="listbox" id="objList3">
        
        <div class="divloadmore" id="btnNext3"> 
        <span  class="button laodmoer" onclick="LoadData3()">加载更多</span>
        </div>
        <div class="bottom50"></div>
    </div>

      <div class="listbox" id="objList4" >
       
        
        <div class="divloadmore" id="btnNext4"> 
        <span  class="button laodmoer" onclick="LoadData4()">加载更多</span>
        </div>
        <div class="bottom50"></div>
    </div>

    
<%--    <div class="submit">
        <a href="<%=aconfig.TheOrganizers%>" class="leftlink" >
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M1.813 58.93c-0.074 0.091-0.139 0.187-0.209 0.279-0.087 0.115-0.177 0.229-0.257 0.349-0.075 0.113-0.141 0.23-0.21 0.345-0.065 0.108-0.133 0.214-0.193 0.325-0.063 0.119-0.117 0.242-0.175 0.363-0.054 0.115-0.111 0.228-0.161 0.346-0.049 0.119-0.089 0.24-0.132 0.361-0.045 0.125-0.093 0.25-0.133 0.378-0.037 0.122-0.064 0.245-0.094 0.369-0.033 0.13-0.069 0.258-0.095 0.39-0.028 0.143-0.045 0.287-0.066 0.432-0.017 0.114-0.038 0.227-0.050 0.342-0.052 0.525-0.052 1.055 0 1.58 0.011 0.115 0.033 0.228 0.050 0.342 0.021 0.144 0.037 0.289 0.066 0.432 0.026 0.132 0.062 0.26 0.095 0.39 0.031 0.123 0.058 0.247 0.094 0.369 0.039 0.129 0.087 0.252 0.133 0.378 0.044 0.12 0.083 0.242 0.132 0.361s0.107 0.231 0.161 0.346c0.057 0.122 0.111 0.243 0.175 0.363 0.059 0.111 0.128 0.217 0.193 0.325 0.069 0.115 0.134 0.232 0.21 0.345 0.080 0.12 0.17 0.234 0.257 0.349 0.070 0.093 0.134 0.189 0.209 0.279 0.335 0.408 0.709 0.782 1.117 1.117 0.091 0.074 0.185 0.138 0.277 0.207 0.116 0.087 0.23 0.177 0.351 0.258 0.113 0.076 0.23 0.141 0.346 0.21 0.109 0.065 0.214 0.132 0.325 0.192 0.118 0.063 0.24 0.117 0.361 0.174 0.115 0.055 0.23 0.113 0.349 0.162 0.117 0.049 0.237 0.088 0.356 0.131 0.127 0.045 0.253 0.094 0.382 0.133 0.12 0.037 0.241 0.063 0.362 0.093 0.131 0.033 0.262 0.070 0.396 0.097 0.139 0.028 0.28 0.043 0.421 0.064 0.118 0.017 0.234 0.039 0.353 0.051 0.262 0.028 0.525 0.043 0.788 0.043h8v46.222c0 5.4 4.378 9.778 9.778 9.778h22.222v-48h32v48h22.222c5.4 0 9.778-4.378 9.778-9.778v-46.222h8c0.263 0 0.526-0.014 0.789-0.040 0.119-0.011 0.236-0.034 0.353-0.051 0.141-0.020 0.281-0.037 0.421-0.064 0.134-0.027 0.265-0.064 0.397-0.097 0.121-0.030 0.242-0.057 0.362-0.093 0.13-0.039 0.255-0.088 0.382-0.133 0.119-0.043 0.239-0.082 0.356-0.131 0.119-0.049 0.233-0.107 0.349-0.162 0.121-0.057 0.242-0.11 0.361-0.174 0.111-0.059 0.217-0.128 0.325-0.192 0.115-0.069 0.233-0.134 0.346-0.21 0.121-0.081 0.235-0.171 0.351-0.258 0.093-0.069 0.187-0.133 0.277-0.207 0.408-0.335 0.782-0.709 1.117-1.117 0.074-0.091 0.139-0.187 0.209-0.279 0.087-0.115 0.177-0.229 0.257-0.349 0.076-0.113 0.141-0.23 0.21-0.345 0.065-0.109 0.132-0.214 0.193-0.325 0.064-0.12 0.118-0.243 0.175-0.365 0.054-0.115 0.111-0.227 0.16-0.344 0.050-0.12 0.090-0.242 0.133-0.363 0.044-0.124 0.093-0.248 0.131-0.376 0.037-0.122 0.064-0.245 0.095-0.369 0.033-0.13 0.069-0.258 0.095-0.39 0.028-0.143 0.045-0.287 0.066-0.432 0.017-0.114 0.038-0.227 0.050-0.342 0.053-0.525 0.053-1.055 0-1.581-0.011-0.115-0.033-0.227-0.050-0.342-0.020-0.144-0.037-0.289-0.066-0.432-0.026-0.133-0.062-0.261-0.095-0.39-0.031-0.123-0.058-0.247-0.095-0.369-0.038-0.128-0.086-0.251-0.131-0.376-0.043-0.122-0.084-0.243-0.133-0.363-0.049-0.117-0.106-0.23-0.16-0.344-0.057-0.122-0.111-0.245-0.175-0.364-0.059-0.111-0.128-0.217-0.193-0.325-0.069-0.115-0.134-0.232-0.21-0.345-0.080-0.12-0.17-0.234-0.257-0.349-0.070-0.093-0.134-0.188-0.209-0.279-0.168-0.205-0.345-0.401-0.531-0.587l-55.999-56.002c-1.448-1.448-3.448-2.343-5.657-2.343s-4.209 0.895-5.657 2.343l-55.999 55.999c-0.187 0.187-0.363 0.383-0.531 0.587z"></path>
                </svg>
            </span>
            <span class="text"><%=string.IsNullOrEmpty(aconfig.OrganizerName) ? "主办方" : aconfig.OrganizerName%></span>
        </a>

        <a href="<%=aconfig.MyRegistration%>" class="rightlink current">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M112 88h-24c-4.419 0-8-3.581-8-8v-4.291c9.562-5.534 16-15.866 16-27.709v-16c0-17.673-14.327-32-32-32s-32 14.327-32 32v16c0 11.843 6.438 22.174 16 27.709v4.291c0 4.419-3.581 8-8 8h-24c-8.836 0-16 7.163-16 16v14.222c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-14.222c0-8.837-7.163-16-16-16z"></path>
                </svg>
            </span>
           <span class="text"><%=string.IsNullOrEmpty(aconfig.MyRegistrationName) ? "我的报名" : aconfig.MyRegistrationName%></span>
        </a>

        <a href="<%=aconfig.Activities%>" class="centerlink">
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
     
       <%if (ToolBarGroup.Count==0){%>
        
      <div class="submit">
        <a href="/App/Cation/Wap/ActivityLlists.aspx" class="leftlink" >
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M1.813 58.93c-0.074 0.091-0.139 0.187-0.209 0.279-0.087 0.115-0.177 0.229-0.257 0.349-0.075 0.113-0.141 0.23-0.21 0.345-0.065 0.108-0.133 0.214-0.193 0.325-0.063 0.119-0.117 0.242-0.175 0.363-0.054 0.115-0.111 0.228-0.161 0.346-0.049 0.119-0.089 0.24-0.132 0.361-0.045 0.125-0.093 0.25-0.133 0.378-0.037 0.122-0.064 0.245-0.094 0.369-0.033 0.13-0.069 0.258-0.095 0.39-0.028 0.143-0.045 0.287-0.066 0.432-0.017 0.114-0.038 0.227-0.050 0.342-0.052 0.525-0.052 1.055 0 1.58 0.011 0.115 0.033 0.228 0.050 0.342 0.021 0.144 0.037 0.289 0.066 0.432 0.026 0.132 0.062 0.26 0.095 0.39 0.031 0.123 0.058 0.247 0.094 0.369 0.039 0.129 0.087 0.252 0.133 0.378 0.044 0.12 0.083 0.242 0.132 0.361s0.107 0.231 0.161 0.346c0.057 0.122 0.111 0.243 0.175 0.363 0.059 0.111 0.128 0.217 0.193 0.325 0.069 0.115 0.134 0.232 0.21 0.345 0.080 0.12 0.17 0.234 0.257 0.349 0.070 0.093 0.134 0.189 0.209 0.279 0.335 0.408 0.709 0.782 1.117 1.117 0.091 0.074 0.185 0.138 0.277 0.207 0.116 0.087 0.23 0.177 0.351 0.258 0.113 0.076 0.23 0.141 0.346 0.21 0.109 0.065 0.214 0.132 0.325 0.192 0.118 0.063 0.24 0.117 0.361 0.174 0.115 0.055 0.23 0.113 0.349 0.162 0.117 0.049 0.237 0.088 0.356 0.131 0.127 0.045 0.253 0.094 0.382 0.133 0.12 0.037 0.241 0.063 0.362 0.093 0.131 0.033 0.262 0.070 0.396 0.097 0.139 0.028 0.28 0.043 0.421 0.064 0.118 0.017 0.234 0.039 0.353 0.051 0.262 0.028 0.525 0.043 0.788 0.043h8v46.222c0 5.4 4.378 9.778 9.778 9.778h22.222v-48h32v48h22.222c5.4 0 9.778-4.378 9.778-9.778v-46.222h8c0.263 0 0.526-0.014 0.789-0.040 0.119-0.011 0.236-0.034 0.353-0.051 0.141-0.020 0.281-0.037 0.421-0.064 0.134-0.027 0.265-0.064 0.397-0.097 0.121-0.030 0.242-0.057 0.362-0.093 0.13-0.039 0.255-0.088 0.382-0.133 0.119-0.043 0.239-0.082 0.356-0.131 0.119-0.049 0.233-0.107 0.349-0.162 0.121-0.057 0.242-0.11 0.361-0.174 0.111-0.059 0.217-0.128 0.325-0.192 0.115-0.069 0.233-0.134 0.346-0.21 0.121-0.081 0.235-0.171 0.351-0.258 0.093-0.069 0.187-0.133 0.277-0.207 0.408-0.335 0.782-0.709 1.117-1.117 0.074-0.091 0.139-0.187 0.209-0.279 0.087-0.115 0.177-0.229 0.257-0.349 0.076-0.113 0.141-0.23 0.21-0.345 0.065-0.109 0.132-0.214 0.193-0.325 0.064-0.12 0.118-0.243 0.175-0.365 0.054-0.115 0.111-0.227 0.16-0.344 0.050-0.12 0.090-0.242 0.133-0.363 0.044-0.124 0.093-0.248 0.131-0.376 0.037-0.122 0.064-0.245 0.095-0.369 0.033-0.13 0.069-0.258 0.095-0.39 0.028-0.143 0.045-0.287 0.066-0.432 0.017-0.114 0.038-0.227 0.050-0.342 0.053-0.525 0.053-1.055 0-1.581-0.011-0.115-0.033-0.227-0.050-0.342-0.020-0.144-0.037-0.289-0.066-0.432-0.026-0.133-0.062-0.261-0.095-0.39-0.031-0.123-0.058-0.247-0.095-0.369-0.038-0.128-0.086-0.251-0.131-0.376-0.043-0.122-0.084-0.243-0.133-0.363-0.049-0.117-0.106-0.23-0.16-0.344-0.057-0.122-0.111-0.245-0.175-0.364-0.059-0.111-0.128-0.217-0.193-0.325-0.069-0.115-0.134-0.232-0.21-0.345-0.080-0.12-0.17-0.234-0.257-0.349-0.070-0.093-0.134-0.188-0.209-0.279-0.168-0.205-0.345-0.401-0.531-0.587l-55.999-56.002c-1.448-1.448-3.448-2.343-5.657-2.343s-4.209 0.895-5.657 2.343l-55.999 55.999c-0.187 0.187-0.363 0.383-0.531 0.587z"></path>
                </svg>
            </span>
            <span class="text">主页</span>
        </a>

        <a href="/App/Cation/Wap/MyActivityLlists.aspx" class="rightlink current">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M112 88h-24c-4.419 0-8-3.581-8-8v-4.291c9.562-5.534 16-15.866 16-27.709v-16c0-17.673-14.327-32-32-32s-32 14.327-32 32v16c0 11.843 6.438 22.174 16 27.709v4.291c0 4.419-3.581 8-8 8h-24c-8.836 0-16 7.163-16 16v14.222c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-14.222c0-8.837-7.163-16-16-16z"></path>
                </svg>
            </span>
           <span class="text">我的报名</span>
        </a>

        <a href="/App/Cation/Wap/ActivityLlists.aspx" class="centerlink">
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
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/lib/weixin/weixinshare.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageIndexStatus1 = 1; //待开始页码
        var pageIndexStatus2 = 1; //已签到页码
        var pageIndexStatus3 = 1; //已结束页码
        var pageIndexStatus4 = 1; //待支付页码
        var pageSize = 10; //页数
        var activityStatus = "即将开始"; //活动默认状态
        $(function () {

            LoadData1();

            $("#rdoStatus1").click(function () {//待支付
                //$(".currentlist").removeClass("currentlist");
                $(".current").removeClass("current");
                $(".tab1").addClass("current");
                activityStatus = "即将开始";
                $(objList1).show();
                $(objList2).hide();
                $(objList3).hide();
                $(objList4).hide();
            }); //待支付

            $("#rdoStatus2").click(function () {//已签到

                $(".current").removeClass("current");
                $(".tab2").addClass("current");
                activityStatus = "已签到";
                $(objList1).hide();
                $(objList2).show();
                $(objList3).hide();
                $(objList4).hide();
                LoadData2();
            }); //已签到

            $("#rdoStatus3").click(function () {//已结束
                //$(".currentlist").removeClass("currentlist");
                $(".current").removeClass("current");
                $(".tab3").addClass("current");
                activityStatus = "已结束";
                $(objList1).hide();
                $(objList2).hide();
                $(objList3).show();
                $(objList4).hide();
                LoadData3();
            }); //已结束

            $("#rdoStatus4").click(function () {//待支付
                $(".current").removeClass("current");
                $(".tab4").addClass("current");
                activityStatus = "待支付";
                $(objList1).hide();
                $(objList2).hide();
                $(objList3).hide();
                $(objList4).show();
                LoadData4();
            }); //待支付

        });


        //即将开始
        function LoadData1() {
            $.ajax({
                type: 'post',
                url: "/Handler/Activity/ActivityHandler.ashx",
                data: { Action: "GetMyActivityList", pageIndex: pageIndexStatus1, pageSize: pageSize, activityStatus: activityStatus },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    /* 
                    //$.each(resp.Result, function (index, Item) {
                    //    str.AppendFormat('<a onclick="GotoHref({0},{1},{2},{3},{4})"  class="maincontext"> ', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                    //    str.AppendFormat('<img src="{0}">', Item.ActivityImage);
                    //    str.AppendFormat('<div class="maincontent">');
                    //    str.AppendFormat('<span class="title">{0}', Item.ActivityName);
                    //    //                        if (Item.IsFee == 1) {
                    //    //                            str.AppendFormat('<label class="paytag" >付费</label>');
                    //    //                        }
                    //    str.AppendFormat('</span>');
                    //    str.AppendFormat('<span class="adress">{0}</span>', Item.Address);
                    //    str.AppendFormat('<span class="time">{0}</span>', Item.StartTime);

                    //    str.AppendFormat('<span class="baomingstatus ">');
                    //    if (Item.ActivityStatus == 1) {
                    //        str.AppendFormat(' <span class="text">已停止</span>');
                    //        str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                    //    } else if (Item.ActivityStatus == 2) {
                    //        str.AppendFormat(' <span class="text">已满员</span>');
                    //        str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                    //    }
                    //    else {
                    //        str.AppendFormat(' <span class="text">进行中</span>');

                    //        str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                    //    }
                    //    str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                    //});
                    */
                    $.each(resp.Result, function (index, Item) {
                        if (index == 0 && pageIndexStatus1 == 1) {
                            str.AppendFormat('<a onclick="GotoHref({0},{1},{2},{3},{4})"  class="maincontext"> ', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('<img src="{0}" style="max-width:100%;">', Item.ActivityImage);
                            str.AppendFormat('<div class="maincontent">');
                            str.AppendFormat('<span class="title" style="font-weight:bold;">{0}</span>', Item.ActivityName);
                            str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>开始时间:{0}</span>', Item.StartTime);
                            if (Item.StartTime != "")
                            {
                                str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span>', Item.Address);
                            }
                            str.AppendFormat('<span class="wybm">进入</span>');
                            str.AppendFormat('<span class="baomingstatus ">');
                            if (Item.ActivityStatus == 1){
                                str.AppendFormat(' <span class="text">已停止</span>');
                            } else if (Item.ActivityStatus == 2) {
                                str.AppendFormat(' <span class="text">满员</span>');
                            }
                            else {
                                str.AppendFormat(' <span class="text">进行中</span>');
                            }
                            str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                        }
                        else {
                            str.AppendFormat('<div class="rl-box">');
                            str.AppendFormat('<dl>');
                            str.AppendFormat('<dt><img src="{0}"></dt>', Item.ActivityImage);
                            str.AppendFormat('<dd>');
                            str.AppendFormat('<h3>{0}</h3>', Item.ActivityName);

                            str.AppendFormat('<p class="time">开始时间:{0}<span class="bao"></p>', Item.StartTime);
                            if (Item.ActivityStartDate != "")
                            {
                                str.AppendFormat('<p class="time">结束时间:{0}<span class="bao"></p>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<p class="adress">{0}</p>', Item.Address);
                            }
                            str.AppendFormat('<a class="baoming" href="javascript:;" onclick="GotoHref({0},{1},{2},{3},{4})">进入</a>', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('</dd>');
                            str.AppendFormat('</dl>');
                            // str.AppendFormat('<div class="hotbao"></div>');
                            str.AppendFormat('<span class="baomingstatus">');
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
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></span></div></a>');
                            str.AppendFormat(' </div>');

                        }

                    });


                    $("#btnNext1").before(str.ToString());
                    $("#btnNext1").show();
                    pageIndexStatus1++;
                    if (resp.Result.length == 0) {
                        $("#btnNext1").html("没有更多");
                    }

                }
            });
        };

        //已签到
        function LoadData2() {
            $.ajax({
                type: 'post',
                url: "/Handler/Activity/ActivityHandler.ashx",
                data: { Action: "GetMyActivityList", pageIndex: pageIndexStatus2, pageSize: pageSize, activityStatus: activityStatus },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();

                    $.each(resp.Result, function (index, Item) {
                        if (index == 0 && pageIndexStatus2 == 1) {
                            str.AppendFormat('<a onclick="GotoHref({0},{1},{2},{3},{4})"  class="maincontext"> ', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('<img src="{0}" style="max-width:100%;">', Item.ActivityImage);
                            str.AppendFormat('<div class="maincontent">');
                            str.AppendFormat('<span class="title" style="font-weight:bold;">{0}</span>', Item.ActivityName);
                            str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>开始时间:{0}</span>', Item.StartTime);
                            if (Item.StartTime != "") {
                                str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span>', Item.Address);
                            }
                            str.AppendFormat('<span class="wybm">进入</span>');
                            str.AppendFormat('<span class="baomingstatus ">');
                            if (Item.ActivityStatus == 1) {
                                str.AppendFormat(' <span class="text">已停止</span>');
                            } else if (Item.ActivityStatus == 2) {
                                str.AppendFormat(' <span class="text">满员</span>');
                            }
                            else {
                                str.AppendFormat(' <span class="text">进行中</span>');
                            }
                            str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                        }
                        else {
                            str.AppendFormat('<div class="rl-box">');
                            str.AppendFormat('<dl>');
                            str.AppendFormat('<dt><img src="{0}"></dt>', Item.ActivityImage);
                            str.AppendFormat('<dd>');
                            str.AppendFormat('<h3>{0}</h3>', Item.ActivityName);

                            str.AppendFormat('<p class="time">开始时间:{0}<span class="bao"></p>', Item.StartTime);
                            if (Item.ActivityStartDate != "") {
                                str.AppendFormat('<p class="time">结束时间:{0}<span class="bao"></p>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<p class="adress">{0}</p>', Item.Address);
                            }
                            str.AppendFormat('<a class="baoming" href="javascript:;" onclick="GotoHref({0},{1},{2},{3},{4})">进入</a>', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('</dd>');
                            str.AppendFormat('</dl>');
                            // str.AppendFormat('<div class="hotbao"></div>');
                            str.AppendFormat('<span class="baomingstatus">');
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
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></span></div></a>');
                            str.AppendFormat(' </div>');

                        }

                    });



                    $("#btnNext2").before(str.ToString());
                    $("#btnNext2").show();
                    pageIndexStatus2++;
                    if (resp.Result.length == 0) {
                        $("#btnNext2").html("没有更多");
                    }

                }
            });
        };

        //已结束
        function LoadData3() {
            $.ajax({
                type: 'post',
                url: "/Handler/Activity/ActivityHandler.ashx",
                data: { Action: "GetMyActivityList", pageIndex: pageIndexStatus3, pageSize: pageSize, activityStatus: activityStatus },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();

                    $.each(resp.Result, function (index, Item) {
                        if (index == 0 && pageIndexStatus3 == 1) {
                            str.AppendFormat('<a onclick="GotoHref({0},{1},{2},{3},{4})"  class="maincontext"> ', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('<img src="{0}" style="max-width:100%;">', Item.ActivityImage);
                            str.AppendFormat('<div class="maincontent">');
                            str.AppendFormat('<span class="title" style="font-weight:bold;">{0}</span>', Item.ActivityName);
                            str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>开始时间:{0}</span>', Item.StartTime);
                            if (Item.StartTime != "") {
                                str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span>', Item.Address);
                            }
                            str.AppendFormat('<span class="wybm">进入</span>');
                            str.AppendFormat('<span class="baomingstatus ">');
                            str.AppendFormat(' <span class="text">已停止</span>');
                            str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                        }
                        else {
                            str.AppendFormat('<div class="rl-box">');
                            str.AppendFormat('<dl>');
                            str.AppendFormat('<dt><img src="{0}"></dt>', Item.ActivityImage);
                            str.AppendFormat('<dd>');
                            str.AppendFormat('<h3>{0}</h3>', Item.ActivityName);

                            str.AppendFormat('<p class="time">开始时间:{0}<span class="bao"></p>', Item.StartTime);
                            if (Item.ActivityStartDate != "") {
                                str.AppendFormat('<p class="time">结束时间:{0}<span class="bao"></p>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<p class="adress">{0}</p>', Item.Address);
                            }
                            str.AppendFormat('<a class="baoming" href="javascript:;" onclick="GotoHref({0},{1},{2},{3},{4})">进入</a>', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('</dd>');
                            str.AppendFormat('</dl>');
                            str.AppendFormat('<span class="baomingstatus">');
                            str.AppendFormat(' <span class="text">已停止</span>');
                            str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#ddd">');
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></span></div></a>');
                            str.AppendFormat(' </div>');

                        }

                    });



                    $("#btnNext3").before(str.ToString());
                    $("#btnNext3").show();
                    pageIndexStatus3++;
                    if (resp.Result.length == 0) {

                        $("#btnNext3").html("没有更多");
                    }

                }
            });
        };

        //待支付
        function LoadData4() {
            $.ajax({
                type: 'post',
                url: "/Handler/Activity/ActivityHandler.ashx",
                data: { Action: "GetMyActivityList", pageIndex: pageIndexStatus4, pageSize: pageSize, activityStatus: activityStatus },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();

                    $.each(resp.Result, function (index, Item) {
                        if (index == 0 && pageIndexStatus4 == 1) {
                            str.AppendFormat('<a onclick="GotoHref({0},{1},{2},{3},{4})"  class="maincontext"> ', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('<img src="{0}" style="max-width:100%;">', Item.ActivityImage);
                            str.AppendFormat('<div class="maincontent">');
                            str.AppendFormat('<span class="title" style="font-weight:bold;">{0}</span>', Item.ActivityName);
                            str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>开始时间:{0}</span>', Item.StartTime);
                            if (Item.StartTime != "") {
                                str.AppendFormat('<span class="time"><i class="iconfont icon-shijian"></i>结束时间:{0}</span>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<span class="adress"><i class="iconfont icon-address"></i>{0}</span>', Item.Address);
                            }
                            str.AppendFormat('<span class="wybm">进入</span>');
                            str.AppendFormat('<span class="baomingstatus ">');
                            if (Item.ActivityStatus == 1) {
                                str.AppendFormat(' <span class="text">已停止</span>');
                            } else if (Item.ActivityStatus == 2) {
                                str.AppendFormat(' <span class="text">满员</span>');
                            }
                            else {
                                str.AppendFormat(' <span class="text">进行中</span>');
                            }
                            str.AppendFormat('<svg class="sanjiao" version="1.1" viewBox="0 0 100 100" fill="#5ddc73">');
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></div></a>');

                        }
                        else {
                            str.AppendFormat('<div class="rl-box">');
                            str.AppendFormat('<dl>');
                            str.AppendFormat('<dt><img src="{0}"></dt>', Item.ActivityImage);
                            str.AppendFormat('<dd>');
                            str.AppendFormat('<h3>{0}</h3>', Item.ActivityName);

                            str.AppendFormat('<p class="time">开始时间:{0}<span class="bao"></p>', Item.StartTime);
                            if (Item.ActivityStartDate != "") {
                                str.AppendFormat('<p class="time">结束时间:{0}<span class="bao"></p>', Item.StopTime);
                            }
                            if (Item.Address != "") {
                                str.AppendFormat('<p class="adress">{0}</p>', Item.Address);
                            }
                            str.AppendFormat('<a class="baoming" href="javascript:;" onclick="GotoHref({0},{1},{2},{3},{4})">进入</a>', Item.SignUpActivityID, Item.IsFee, Item.PaymentStatus, Item.OrderId, Item.JuactivityId);
                            str.AppendFormat('</dd>');
                            str.AppendFormat('</dl>');
                            // str.AppendFormat('<div class="hotbao"></div>');
                            str.AppendFormat('<span class="baomingstatus">');
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
                            str.AppendFormat('<polygon points="100,100 0.2,100 100,0.2" /> </svg></span></span></div></a>');
                            str.AppendFormat(' </div>');

                        }

                    });

                    $("#btnNext4").before(str.ToString());
                    $("#btnNext4").show();
                    pageIndexStatus4++;
                    if (resp.Result.length == 0) {
                        $("#btnNext4").html("没有更多");
                    }

                }
            });
        };

        function GotoHref(signUpActivityID, isFee, paymentStatus, orderId, jid) {
            if (isFee == 1 && paymentStatus == 0) {//收费活动未支付
                window.location.href = 'RemindPay.aspx?activityid=' + signUpActivityID + '&orderId=' + orderId;
            }
            else {
                window.location.href = 'MyCenter.aspx?activityid=' + signUpActivityID + '&jid=' + jid;
            }

        }

    </script>
</body>
</html>
