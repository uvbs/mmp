<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.Contact" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>联系我们</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
     <style>
    .menu5 ul li{width:33.33%;}
    </style> 
</head>
<body>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <div class="about_container1">
                                
                                <h3 class="contact_h">校服定制联系方式：</h3>
                                <ul class="contact_ul">
                                    <li>电话： <a href="tel:021-31261560">021-31261560</a>
                                        <br />
                                        工作时间: 周一至周五，9：00-18：00</li>
                                    <li>邮箱：<br />
                                       <a href="email:order@teamstyle.cn">order@teamstyle.cn</a>
                                        <br />
                                        <a href="email:service@teamstyle.cn">service@teamstyle.cn</a>
                                        ​</li>
                                    <li>官网：<br />
                                        <a href="http://www.totema.cn">www.totema.cn</a><br />
                                        <a href="http://www.teamstyle.cn">www.teamstyle.cn</a>
                                        </li>
                                    <li>实体展厅：<br />
                                        上海市闵行区莘凌路211号1号楼302-307室​</li>
                                </ul>
                            </div>
                        </div>
                        <!--End of page container-->
                    </div>
                </div>
            </div>
        </div>
    </div>
            <div id="footer">
        <div class="menu5">
            <ul>
                <li><a href="eventdetails.aspx"><span>
                    <img src="images/en1.png" alt="" title="" />活动详情</span></a></li>
                    <li><a href="index.aspx"><span>
                <img src="http://jkbp.comeoncloud.net/open/jkbp/images/home1.png" />
                    </span></a></li>

                <li><a href="<%=new ZentCloud.BLLJIMP.BllVote().GetTotemaApplyLingOrMyClassLink()%>"><span>
                    <img src="images/en3.png" alt="" title="" />我要报名</span></a></li>
            </ul>
        </div>
    </div>
</body>
</html>
