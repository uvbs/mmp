<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HRLoveJoin.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.HRLove.HRLoveJoin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<title>我要预约</title>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="/wubuhui/css/wubu.css?v=0.0.10">
	<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>

<h3 class="maintitle">目前只接收预约报名</br>请留下有效信息：</h3>

<div class="personerinfo">
	<form action="" id= "formsignin">
		<div class="listbox">
			<input type="text" id="txtName"  placeholder="姓名" name="Name">
			<label for="" class="wbtn_line_main">
				<span class="iconfont icon-b24"></span>
			</label>
		</div>
		<div class="listbox">
			<input type="number" id="txtPhone" placeholder="手机"  pattern="\d*" name="Phone">
			<label for="" class="wbtn_line_main">
				<span class="iconfont icon-b47"></span>
			</label>
		</div>
        <input id="activityID" type="hidden" value="267461" name="ActivityID" />
        <input id="loginName" type="hidden" value="d3VidWh1aQ==" name="LoginName">
        <input id="loginPwd" type="hidden" value="9F158FCB7BB4984E98!ACBA#E4DD81B5" name="LoginPwd">
	</form>
</div>

<div class="wcontainer hupenghuanyou">
	<a href="javascript:void(0);" class="wbtn wbtn_red">
		<span class="text" onclick="InsertData()">我要预约</span>
	</a>
</div>

</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="/wubuhui/js/jquery.js"></script>

<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/wubuhui/js/bootstrap.js"></script>
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
        <script src="/Scripts/jquery.form.js" type="text/javascript"></script>
        <script type="text/jscript">
           


            function InsertData() {
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
                    dataType: "text",
                    success: function (result) {

                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {//清空
//                            $('input:text').val("");
//                            $('textarea').val("");
                            alert("报名成功!");
                            return;

                        }
                        else if (resp.Status == 1) {
                            alert("重复提交!");

                        }
                        else {
                            alert(resp.Msg);
                        }

                    }
                });
                return false;
            };


            

        </script>
</html>
