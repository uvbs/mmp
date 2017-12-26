<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.ProjectDetail" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=model.ProjectName%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="http://at.alicdn.com/t/font_1413523014_3981616.css">
    <link type="text/css" rel="stylesheet" href="../Css/wanbang.css">
</head>
<body>
<body>
            <%if (!IsLogin)
             {%>
             <!-- Top -->
             <header class="head">
        	<a id="logo" href="Index.aspx" style="float:none;"><img src="../Images/logo.png"></a>
        	<form class="login" method="post" action="Login.aspx?redirecturl=/App/WanBang/Wap/ProjectDetail.aspx?id=<%=model.AutoID%>">

                <input type="submit" value="立即登录"/>
                        	</form>
          
        </header>
		<!--/ Top -->
             <%}%>
        <!-- CompanyDetail -->
    <div class="main">
            <div class="company-box">
                <dl class="company">
                    <dt><img src="<%=model.Thumbnails%>"></dt>
                    <dd> 
                        <h3><%=model.ProjectName %></h3>
                        <p>
                        发布企业:<a style="color:#58b5e1; text-decoration:underline;" href="CompanyDetail.aspx?id=<%=companymodel.AutoID%>"><%=companymodel.CompanyName%></a><br/>
                        时间：<%=string.Format("{0:f}",model.InsertDate)%><br/>
                        所属区：<%=model.Area %><br>
                        分类：<%=model.Category %><br/>
                      
                    </dd>
                </dl>
                <div class="contact2">
                <p>状态：
                   <%switch (model.Status)
                     {
                         case 1:
                             Response.Write("<font color='green'>征集中</font>");
                             break;
                         case 2:
                             Response.Write("<font color='red'>已结束</font>");
                             break;
                         default:
                             break;
                     } %>
                  </p>
                  <p>工期：<%=model.TimeRequirement %></p> 
                  <p>周期：
                   <%switch (model.ProjectCycle)
                     {
                         case 0:
                             Response.Write("临时(1个月以内)");
                             break;
                         case 1:
                             Response.Write("短期(1-3个月)");
                             break;
                         case 2:
                             Response.Write("中期(3-6个月)");
                             break;
                         case 3:
                             Response.Write("长期(6-12个月)");
                             break;
                         default:
                             break;
                     } %>
                  </p> 
                  <p>物流：
                    <%switch (model.Logistics)
                     {
                         case 0:
                             Response.Write("基地负责配送");
                             break;
                         case 1:
                             Response.Write("企业负责配送");
                             break;
                         default:
                             break;
                     } %>
                  </p> 
                  <p>联系人：<%=companymodel.Contacts%></p> 
                   <%if (IsLogin){%>
                   
                    <p class="qq">手机号：
                    <a href="tel:<%=companymodel.Phone %>" style="color:#14b467;"><%=companymodel.Phone%>
                     <%if (!string.IsNullOrEmpty(companymodel.Phone)){%>
                    <span class="iconfont icon-dianhua1"></span>
                    <%}%>
                    </a>
                    </p>
                    <p class="tel">联系电话：<a href="tel:<%=companymodel.Tel %>" style="color:#14b467;"><%=companymodel.Tel%>
                    <%if (!string.IsNullOrEmpty(companymodel.Tel))
                      {%>
                    <span class="iconfont icon-dianhua1"></span>
                    <%}%>
                    </a>
                    </p>
	                <%}%>
                    <%else{%>
                    <p>QQ：<label style="color:#FF5809;">登录后可见</label></p>
                    <p class="qq">手机号：<label style="color:#FF5809;">登录后可见</label></p>
                    <p class="tel">联系电话：<label style="color:#FF5809;">登录后可见</label></p> 
                   <%}%>
                    
                    
                </div>
                 <%if (!IsAttention){%>
                <a href="javascript:Attenion()" class="guan"><span class="iconfont icon-guanzhu1"></span>关注</a>
                <%}%>
                <%else{%>
                 <a href="javascript:CancelAttention()" class="guan"><span class="iconfont icon-guanzhu1"></span>取消关注</a>
                 <%}%>
            </div>
            <p class="detail-text"><%=model.Introduction %></p>
            
        </div>
        <!--/ CompanyDetail -->
        <div class="blank"></div>


        <!-- Back -->
<a href="javascript:window.history.go(-1)" class="back">
    <span class="iconfont icon-fanhui"></span>
</a>
        <!--/ Back -->

</body>



</body>
<script type="text/javascript">

    var handlerurl = "/Handler/WanBang/Wap.ashx";

    function Attenion() {
        try {
            var model =
                    {
                        Action: "AddAttentionInfo",
                        AttentionType:2,
                        AttentionAutoID: "<%=model.AutoID%>"
                    };
            $.ajax({
                type: 'post',
                url: handlerurl,
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("关注成功");
                    }
                    else {
                        alert(resp.Msg);
                    }

                }
            });

        } catch (e) {
            alert(e);
        }


    }

    function CancelAttention() {

        if (confirm("确定取消关注?")) {
            $.ajax({
                type: 'post',
                url: handlerurl,
                data: { Action: 'CancelAttention', AutoID: "<%=model.AutoID%>", Attentiontype: 2 },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("已取消关注");
                    }
                    else {
                        alert("操作失败");
                    }


                }
            });


            //
        }


    }
</script>
</html>