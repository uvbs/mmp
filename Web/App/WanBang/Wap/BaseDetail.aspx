<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.BaseDetail" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=model.BaseName%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link type="text/css" rel="stylesheet" href="../Css/wanbang.css">
    <link type="text/css" rel="stylesheet" href="http://at.alicdn.com/t/font_1413523014_3981616.css">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>

            <%if (!IsLogin)
             {%>
             <!-- Top -->
             <header class="head">
        	<a id="logo" href="Index.aspx" style="float:none;"><img src="../Images/logo.png"></a>
        	<form class="login" method="post" action="Login.aspx?redirecturl=/App/WanBang/Wap/BaseDetail.aspx?id=<%=model.AutoID%>">

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
                        <h3><%=model.BaseName %></h3>
                        <p>
                        所属区：<%=model.Area %><br>
                        面积：<%=model.Acreage %>平方米<br/>
                        援助对象人数：<%=model.HelpCount%> <br/>
                        
                    </dd>
                </dl>
                <div class="contact2">
                   <p>负责人：<%=model.Contacts %></p>
                   <%if (IsLogin){%>
                   <p>QQ:<%=model.QQ %></p>
                    <p class="qq">手机号：
                    <a href="tel:<%=model.Phone %>" style="color:#14b467;"><%=model.Phone %>
                     <%if (!string.IsNullOrEmpty(model.Phone)){%>
                    <span class="iconfont icon-dianhua1">
                    <%}%>
                    </a>
                    </p>
                    <p class="tel">联系电话：<a href="tel:<%=model.Tel %>" style="color:#14b467;"><%=model.Tel %>
                    <%if (!string.IsNullOrEmpty(model.Tel)){%>
                    <span class="iconfont icon-dianhua1">
                    <%}%>
                    </a>
                    </p>
	                <%}%>
                    <%else{%>
                    <p>QQ：<label style="color:#FF5809;">登录后可见</label></p>
                    <p class="qq">手机号：<label style="color:#FF5809;">登录后可见</label></p>
                    <p class="tel">联系电话：<label style="color:#FF5809;">登录后可见</label></p> 
                   <%}%>
                    
                    <p class="adress"><span class="iconfont icon-ditu"></span>地址：<%=model.Address %></p>
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
<script type="text/javascript">

    var handlerurl = "/Handler/WanBang/Wap.ashx";
    function Attenion() {
        try {
            var model =
                    {
                        Action: "AddAttentionInfo",
                        AttentionType: 0,
                        AttentionAutoID:"<%=model.AutoID%>"
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
                data: { Action: 'CancelAttention', AutoID: "<%=model.AutoID%>", Attentiontype:0 },
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
