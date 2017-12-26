<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Page.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WebPc.Page" %>

<!doctype html>
<html>
<head>
    <title><%=model.PageName%></title>
    <meta charset="utf-8">
    <meta name="author" content="comeoncloud">
    <meta name="renderer" content="webkit">
    <meta name="description" content="">
    <meta name="keywords" content="">
    <link rel="shortcut icon" href="<%=model.Logo %>" />
    <link rel="stylesheet" type="text/css" href="style/page.css" />
    <link rel="stylesheet" href="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/css/swiper.min.css" />
</head>
<body>

    <%--顶部开始--%>
    <%if (!string.IsNullOrEmpty(model.TopContent))
      { %>
    <div class="topbar">
        <div class="topbar-inner">
            <%=model.TopContent %>
        </div>
    </div>
    <%} %>
    <%--顶部结束--%>


    <%--顶部导航开始--%>
    <%if (!string.IsNullOrEmpty(model.TopMenu))
      {%>
    <div class="navbar" id="navbar">
        <div class="content">
            <img class="logo" src=" <%=model.Logo%>" />
            <%List<ZentCloud.BLLJIMP.Model.CompanyWebsite_ToolBar> navList = bll.GetList<ZentCloud.BLLJIMP.Model.CompanyWebsite_ToolBar>(7, string.Format(" KeyType='{0}'", model.TopMenu), " PlayIndex ASC");%>
            <%foreach (var item in navList)
              {%>
            <a class="link" href="<%=item.ToolBarTypeValue%>" target="_blank"><%=item.ToolBarName %></a>
            <%} %>
        </div>
    </div>

    <%} %>
    <%--顶部导航结束--%>



    <%--中部开始--%>

    <%
        System.Text.StringBuilder sbMidd = new StringBuilder();
        int i = 0;
        foreach (var item in middList)
        {
            switch (item.Type)
            {
                case "slide"://幻灯片
                    sbMidd.AppendFormat("<div class=\"swiper-container\" data-slide-index={0} data-slide-type=\"{1}\" >", i, item.SlideType);
                    sbMidd.AppendFormat("<div class=\"swiper-wrapper\">");

                    foreach (var slide in bll.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='{1}' Order By Sort ASC", bll.WebsiteOwner, item.SlideName)))
                    {
                        sbMidd.AppendFormat("<div class=\"swiper-slide\" data-url=\"{1}\"><img  src=\"{0}\"/></div>", slide.ImageUrl, slide.Link);
                    }
                    sbMidd.AppendFormat(" </div>");
                    sbMidd.AppendFormat("<div class=\"swiper-pagination\"></div>");
                    sbMidd.AppendFormat("</div>");
                    break;
                case "html":
                    sbMidd.AppendFormat("<div class=\"customize-html\">{0}</div>", item.Content);
                    break;
                default:
                    break;
            }
            i++;
        }
        Response.Write(sbMidd.ToString());
    %>

    <%--中部结束--%>

    <%--底部开始--%>
    <%if (!string.IsNullOrEmpty(model.BottomContent))
      {%>
    <div class="footer">
        <%=model.BottomContent %>
       
    </div>
    <%} %>
    <%--底部结束--%>
    <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/Scripts/jquery-2.1.1.min.js"></script>
    <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/js/swiper.min.js"></script>
    <script src="Script/index.js"></script>
    <script>
    $(function () {

            //生成幻灯片
            $.each($("[data-slide-index]"), function () {

                var slideType = $(this).attr("data-slide-type");
                switch (slideType) {

                    case "cube"://3d 方块
                        new Swiper(this, {
                            pagination: '.swiper-pagination',
                            effect: 'cube',
                            grabCursor: true,
                            cube: {
                                shadow: true,
                                slideShadows: true,
                                shadowOffset: 20,
                                shadowScale: 0.94
                            },
                            autoplay: 2000//可选选项，自动滑动
                        });
                        break;
                    case "coverflow"://3d 覆盖流
                        new Swiper(this, {
                            pagination: '.swiper-pagination',
                            effect: 'coverflow',
                            grabCursor: true,
                            centeredSlides: true,
                            slidesPerView: 'auto',
                            coverflow: {
                                rotate: 50,
                                stretch: 0,
                                depth: 100,
                                modifier: 1,
                                slideShadows: true,
                                autoplay: 2000//可选选项，自动滑动
                            },
                            autoplay: 2000//可选选项，自动滑动
                        });
                        break;

                    case "perview"://分组显示 / Carousel 模式
                        new Swiper(this, {
                            pagination: '.swiper-pagination',
                            slidesPerView: 3,
                            paginationClickable: true,
                            spaceBetween: 30,
                            autoplay: 2000//可选选项，自动滑动
                        });
                        break;

                    default://默认
                        new Swiper(this, {
                            pagination: '.swiper-pagination',
                            paginationClickable: true,
                            autoplay: 2000//可选选项，自动滑动
                        });
                        break;
                }






            })

            //跳转链接
            $("[data-url]").click(function () {
                var link = $(this).attr("data-url");
                if (link != "" && link != undefined) {
                    window.open(link);
                   
                }
            })
        })
    </script>
</body>
</html>
