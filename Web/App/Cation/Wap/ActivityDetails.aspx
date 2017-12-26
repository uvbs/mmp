<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityDetails.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ActivityDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title>
        <asp:Literal runat="server" ID="liTitle"></asp:Literal></title>
    <link href="Style/styles/css/style.css" rel="stylesheet" type="text/css" />
    <script src="http://dev.comeoncloud.net/test/lib/weixin/weixinshare.min.js"></script>
    <script data-main="Style/src/main" src="http://dev.comeoncloud.net/test/lib/requirejs/require.js"></script>
    <style>

        .a {
            position:fixed;
            bottom:0;
        
        }
     </style>
</head>
<body>

    <div class="account-item" ng-if="vm.groupConfig.is_enable_account_amount_pay && vm.userInfo.account_amount > 0 ">
      <div class="floatL font16">使用{{vm.groupConfig.account_amount_pay_showname}} (共有{{vm.userInfo.account_amount}}元)</div>
      <svg class="icon font18 floatR" aria-hidden="true" ng-click="vm.acountExtend=!vm.acountExtend" ng-show="vm.acountExtend">
        <use xlink:href="#icon-jiahao"></use>
      </svg>
      <svg class="icon font18 floatR" aria-hidden="true" ng-click="vm.acountExtend=!vm.acountExtend" ng-show="!vm.acountExtend">
        <use xlink:href="#icon-jianhao"></use>
      </svg>
      <div class="clear-both"></div>
      <div ng-show="!vm.acountExtend" style="float: left; height: 20px; line-height: 20px; margin-top: 10px;">
        <label class="floatL account-score" for="leftmoney" ng-show="!vm.acountExtend">
          <input class="account-checkbox" type="checkbox" id="leftmoney"
                 ng-model="vm.isAcountChecked" ng-change="vf.acountChange()" style="margin-bottom:4px;">
          <span class="font16">可用{{vm.useAcount}}元</span>
        </label>
      </div>
      <div class="font16 floatR" ng-show="!vm.acountExtend" style="margin-top: 10px;">抵{{vm.useAcount}}元</div>
      <div class="clear-both"></div>
    </div>


    <section class="box padding10">
        <header class="header">
		<h1><asp:Literal runat="server" ID="LiName"></asp:Literal></h1>
		<div class="headerinfo">
			<span class="reading">
				阅读   
				<span class="orange"><asp:Literal runat="server" ID="liOpen"></asp:Literal></span>&nbsp;
				分享
				<span class="orange"><asp:Literal runat="server" ID="liShareTotal"></asp:Literal>+</span>
			</span>
		</div>  
	</header>

    <div class="maincontext"> 
        <div class="maincontent">
            <span class="adress"><asp:Literal runat="server" ID="LiAddress"></asp:Literal></span>
            <span class="time"><asp:Literal runat="server" ID="LiStartTime"></asp:Literal>
                <asp:Literal runat="server" ID="LiEndTiem"></asp:Literal> </span>
        </div>
    </div>
    <span class="rule">&nbsp; &nbsp; &nbsp; &nbsp;<asp:Literal ID="liContext" runat="server" /></span>
        
         <div  class="sendinfobox2">
            <form action="" id="formsignin">
            <input class="textbox" placeholder="姓名" name="Name"  id="txtName" type="text">
            <input class="textbox" placeholder="手机" name="Phone"  id="txtPhone" pattern="\d*" type="text">
            </form>
            <span class="mainbtn submitbtn"  onclick="InsertData()" >提交报名</span>
        </div>

        <div class="baomg">
            <span>参加人员</span>
        </div>
        <div class="usernum">
            已有 <span>
                <asp:Literal ID="liPCount" runat="server" /></span> 人参加
        </div>
        <div class="user">
            <ul>
               
                <li id="append"><span onclick="LoadRPInfo()" >更多</span></li>
            </ul>
        </div>
        
        <div class="footer">
            <span class="weixinshare green weixinsharebtn">分享给好友</span> <span
                class="weixinshare blue weixinsharebtn">分享到朋友圈</span>
        </div>

        <div class="submit">
        <a href="#" class="leftlink" id="theUrl">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M1.813 58.93c-0.074 0.091-0.139 0.187-0.209 0.279-0.087 0.115-0.177 0.229-0.257 0.349-0.075 0.113-0.141 0.23-0.21 0.345-0.065 0.108-0.133 0.214-0.193 0.325-0.063 0.119-0.117 0.242-0.175 0.363-0.054 0.115-0.111 0.228-0.161 0.346-0.049 0.119-0.089 0.24-0.132 0.361-0.045 0.125-0.093 0.25-0.133 0.378-0.037 0.122-0.064 0.245-0.094 0.369-0.033 0.13-0.069 0.258-0.095 0.39-0.028 0.143-0.045 0.287-0.066 0.432-0.017 0.114-0.038 0.227-0.050 0.342-0.052 0.525-0.052 1.055 0 1.58 0.011 0.115 0.033 0.228 0.050 0.342 0.021 0.144 0.037 0.289 0.066 0.432 0.026 0.132 0.062 0.26 0.095 0.39 0.031 0.123 0.058 0.247 0.094 0.369 0.039 0.129 0.087 0.252 0.133 0.378 0.044 0.12 0.083 0.242 0.132 0.361s0.107 0.231 0.161 0.346c0.057 0.122 0.111 0.243 0.175 0.363 0.059 0.111 0.128 0.217 0.193 0.325 0.069 0.115 0.134 0.232 0.21 0.345 0.080 0.12 0.17 0.234 0.257 0.349 0.070 0.093 0.134 0.189 0.209 0.279 0.335 0.408 0.709 0.782 1.117 1.117 0.091 0.074 0.185 0.138 0.277 0.207 0.116 0.087 0.23 0.177 0.351 0.258 0.113 0.076 0.23 0.141 0.346 0.21 0.109 0.065 0.214 0.132 0.325 0.192 0.118 0.063 0.24 0.117 0.361 0.174 0.115 0.055 0.23 0.113 0.349 0.162 0.117 0.049 0.237 0.088 0.356 0.131 0.127 0.045 0.253 0.094 0.382 0.133 0.12 0.037 0.241 0.063 0.362 0.093 0.131 0.033 0.262 0.070 0.396 0.097 0.139 0.028 0.28 0.043 0.421 0.064 0.118 0.017 0.234 0.039 0.353 0.051 0.262 0.028 0.525 0.043 0.788 0.043h8v46.222c0 5.4 4.378 9.778 9.778 9.778h22.222v-48h32v48h22.222c5.4 0 9.778-4.378 9.778-9.778v-46.222h8c0.263 0 0.526-0.014 0.789-0.040 0.119-0.011 0.236-0.034 0.353-0.051 0.141-0.020 0.281-0.037 0.421-0.064 0.134-0.027 0.265-0.064 0.397-0.097 0.121-0.030 0.242-0.057 0.362-0.093 0.13-0.039 0.255-0.088 0.382-0.133 0.119-0.043 0.239-0.082 0.356-0.131 0.119-0.049 0.233-0.107 0.349-0.162 0.121-0.057 0.242-0.11 0.361-0.174 0.111-0.059 0.217-0.128 0.325-0.192 0.115-0.069 0.233-0.134 0.346-0.21 0.121-0.081 0.235-0.171 0.351-0.258 0.093-0.069 0.187-0.133 0.277-0.207 0.408-0.335 0.782-0.709 1.117-1.117 0.074-0.091 0.139-0.187 0.209-0.279 0.087-0.115 0.177-0.229 0.257-0.349 0.076-0.113 0.141-0.23 0.21-0.345 0.065-0.109 0.132-0.214 0.193-0.325 0.064-0.12 0.118-0.243 0.175-0.365 0.054-0.115 0.111-0.227 0.16-0.344 0.050-0.12 0.090-0.242 0.133-0.363 0.044-0.124 0.093-0.248 0.131-0.376 0.037-0.122 0.064-0.245 0.095-0.369 0.033-0.13 0.069-0.258 0.095-0.39 0.028-0.143 0.045-0.287 0.066-0.432 0.017-0.114 0.038-0.227 0.050-0.342 0.053-0.525 0.053-1.055 0-1.581-0.011-0.115-0.033-0.227-0.050-0.342-0.020-0.144-0.037-0.289-0.066-0.432-0.026-0.133-0.062-0.261-0.095-0.39-0.031-0.123-0.058-0.247-0.095-0.369-0.038-0.128-0.086-0.251-0.131-0.376-0.043-0.122-0.084-0.243-0.133-0.363-0.049-0.117-0.106-0.23-0.16-0.344-0.057-0.122-0.111-0.245-0.175-0.364-0.059-0.111-0.128-0.217-0.193-0.325-0.069-0.115-0.134-0.232-0.21-0.345-0.080-0.12-0.17-0.234-0.257-0.349-0.070-0.093-0.134-0.188-0.209-0.279-0.168-0.205-0.345-0.401-0.531-0.587l-55.999-56.002c-1.448-1.448-3.448-2.343-5.657-2.343s-4.209 0.895-5.657 2.343l-55.999 55.999c-0.187 0.187-0.363 0.383-0.531 0.587z"></path>
                </svg>
            </span>
            <span class="text">主办方</span>
        </a>

        <a href="MyActivityLlists.aspx" class="rightlink">
            <span class="icon">
                <svg  viewBox="0 0 128 128">
                    <path d="M112 88h-24c-4.419 0-8-3.581-8-8v-4.291c9.562-5.534 16-15.866 16-27.709v-16c0-17.673-14.327-32-32-32s-32 14.327-32 32v16c0 11.843 6.438 22.174 16 27.709v4.291c0 4.419-3.581 8-8 8h-24c-8.836 0-16 7.163-16 16v14.222c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-14.222c0-8.837-7.163-16-16-16z"></path>
                </svg>
            </span>
           <span class="text">我的报名</span>
        </a>

        <a href="ActivityLlists.aspx" class="centerlink current">
            <span class="icon">
                <svg viewBox="0 0 128 128">
                    <path d="M44.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M84.040 24c2.187 0 3.96-1.773 3.96-3.96v-16.080c0-2.187-1.773-3.96-3.96-3.96h-0.080c-2.187 0-3.96 1.773-3.96 3.96v16.080c0 2.187 1.773 3.96 3.96 3.96h0.080z"></path>
                    <path d="M118.222 16h-22.222v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-16v4.040c0 6.595-5.365 11.96-11.96 11.96h-0.080c-6.595 0-11.96-5.365-11.96-11.96v-4.040h-22.222c-5.4 0-9.778 4.378-9.778 9.778v92.445c0 5.4 4.378 9.778 9.778 9.778h108.445c5.4 0 9.778-4.378 9.778-9.778v-92.445c0-5.4-4.378-9.778-9.778-9.778zM120 120h-112v-80h112v80z"></path>
                </svg>
            </span>
           <span class="text">活动日历</span>
        </a>
    </div>
    </section>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.form.js" type="text/javascript"></script>
    <script type="text/jscript">
        var signUpId = '<%=jActivityeInfo.SignUpActivityID %>';
        var srcurl = '<%=aconfig.TheOrganizers %>';
        var PageIndex=1;
        
         function LoadRPInfo(){
                 $.ajax({
                type: 'post',
                url: "/Handler/OpenGuestHandler.ashx",
                data: { Action: "GetADInfos", PageIndex: PageIndex,ActivityId:signUpId },
                dataType:"json",
                success: function (resp) {
                    var html = "";
                    if (resp.Status == 0) {
                        if (resp.ExObj == null) {
                          $("#laodmoer").hide();
                            return;
                        }
                        $.each(resp.ExObj, function (index, Item) {
                            html+='<li><img src=\"'+Item.K1+'\" />'+Item.Name+'<span class=\"times\">'+Item.InsertDateStr+'</span></li>';
                        });
                        $("#append").before(html);
                        PageIndex++;
                    }
                    else {
                        $("#laodmoer").hide();
                    }
                }
            });
         };
        $(function () {
          $("#theUrl").attr("href",srcurl);
            InitSignUpDataInfo();
            LoadRPInfo();

        });
              
               function InsertData () {
                var Name = $("#txtName").val();
                var Phone = $("#txtPhone").val();
                if (Name == "" || (Phone == "")) {
                    alert("请输入姓名、手机号码");
                    return false;
                }
                //                try {


                $("#formsignin").ajaxSubmit({
                    url: "/serv/ActivityApiJson.ashx",
                    type: "post",
                    dataType:"json",
                    success: function (resp) {
                        if (resp.Status == 0) {//清空
                            $('input:text').val("");
                            $('textarea').val("");
                            //取得返回的Activity和ActivityData
                            activity = resp.ExObj["Activity"];
                            activityData = resp.ExObj["ActivityData"];
                            activityFieldMapping = resp.ExObj["ActivityFieldMapping"];
                            window.location.href = "MyCenter.aspx?activityid="+<%=jActivityeInfo.SignUpActivityID %>+"&name="+Name+"&Uid="+resp.ExObj["ActivityData"].UID; 
                            return;

                        }
                        else if (resp.Status == 1) {
                            //该用户已提交过数据
                            alert("该用户已提交过数据!");

                        }
                        else {
                            alert(resp.Msg);
                        }

                    }
                });
                return false;
            };


        //获取报名字段信息
        function InitSignUpDataInfo() {

            $.ajax({
                type: 'post',
                url: "/Handler/OpenGuestHandler.ashx",
                data: { Action: "SignUpDataInfo", signUpId: signUpId },
                dataType:"json",
                success: function (resp) {
                    var html = "";
                    if (resp.Status == 0) {
                        //item.FieldName, item.MappingName, tmpValue
                        $.each(resp.ExObj, function (index, item) {
                            if (item.IsMultiline == "1") {
                                html += '<textarea rows=\"5\" type=\"text\" name=\" ' + item.FieldName + '\" id=\"' + item.FieldName + '\" value=\"\" placeholder=\"' + item.MappingName + '\" </textarea>';
                            } else {
                                html += '<input class=\"textbox\"  name=\"' + item.FieldName + '\" id=\"' + item.FieldName + '\" placeholder=\"' + item.MappingName + '\" type=\"text\">';
                            }
                        })
                        html += '<input id=\"activityID\" type=\"hidden\" value=\"' + signUpId + '\" name=\"ActivityID\" />';
                        $("#formsignin").append(html);
                        $("#formsignin").append(resp.ExStr);
                    }
                    else {
                        Alert(resp.Msg);
                    }
                }
            });
        };

    </script>
</body>
</html>
