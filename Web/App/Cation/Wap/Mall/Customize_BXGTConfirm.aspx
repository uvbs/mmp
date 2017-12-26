<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customize_BXGTConfirm.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Customize_BXGTConfirm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="description" content="" />
<meta name="keywords" content="" />
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
<link type="text/css" href="Customize/BXGT/css/base.css" rel="stylesheet" />
<link type="text/css" href="Customize/BXGT/css/page.css" rel="stylesheet" />
</head>
<body style="background:#f2f2f2;height:100%;">
	<div class="back">
		<a title="" href="javascript:history.back();">返回</a>
		<p class="">订单确认</p>
	</div>
	<div class="spDetail">
		<div class="spCon">
			<h2>购买商品</h2>
			<div class="clearfix sp">
				<p class="fl spPic">
			    <img src="Customize/BXGT/images/cp.png" />
				</p>
				<div class="fr spFont">
                    <h3 id="showaddress"></h3>
					<h3 id="showtime"></h3>
					<p id="producttype" class="pt"></p>
					<p id="price" class="jq"></p>
				</div>
			</div>
			<p class="sl">购买数量：<span>1</span></p>
		</div>
	</div>
	<div class="XinXi">
		<div class="xxCon">
			<h2>个人信息</h2>
			<div class="bdCon">
				<p class="pp1"><input id="txtName" type="text" placeholder="姓名"/></p>
				<p class="pp2"><input id="txtPhone" type="text" placeholder="联系方式" /></p>
				<p class="pp3"><input id="txtAddress" type="text" placeholder="收货地址" /></p>
			</div>
		</div>
	</div>
	<div class="GouMai">
		<p class="">
			<a title="" href="#" id="btnSubmit"><img src="Customize/BXGT/images/tj.png" /></a>
		</p>
	</div>

</body>
<script type="text/javascript" src="Customize/BXGT/js/jquery.js"></script>
<script type="text/javascript" src="Customize/BXGT/js/huaping.js"></script>
<script type="text/javascript">
    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    var IsSubmit = false;
    var ShowAddress = "";
    var ShowTime = "";
    var Price = "";
    var DeliveryAutoId = "19";
    var PaymenttypeAutoId = "22";
    var ProductId = "236598";
    $(function () {
        switch (GetParm("showaddress")) {
            case "1":
                ShowAddress = "上海兰心大戏院（茂名南路57号）";
                break;
            case "2":
                ShowAddress = "上海大宁剧院（平型关路1222号）";
                break;
            default:

        }
        ShowTime = GetParm("showtime");
        Price = GetParm("price").replace("#","");
        $("#showaddress").html("演出地点:<br/>"+ShowAddress);
        $("#showtime").html("演出时间:" + ShowTime);
        $("#producttype").text("型号:"+Price);
        $("#price").text("价格:￥" + Price);
        $("#btnSubmit").click(function () {
            var Name = $.trim($("#txtName").val());
            var Phone = $.trim($("#txtPhone").val());
            var Address = $.trim($("#txtAddress").val());
            if (Name == "") {
                $("#txtName").focus();
                return false;
            }
            if (Phone == "") {
                $("#txtPhone").focus();
                return false;
            }
            if (Address == "") {
                $("#txtAddress").focus();
                return false;
            }
            if (IsSubmit) {
                return false;
            }
            var ajaxData =
                {
                    Action: "SubmitWxMallOrderBXGT",
                    ProductId: ProductId,
                    Name: Name,
                    Phone: Phone,
                    Address: Address,
                    ShowAddress: ShowAddress,
                    ShowTime: ShowTime,
                    Price: Price,
                    DeliveryAutoId: DeliveryAutoId,
                    PaymenttypeAutoId: PaymenttypeAutoId,
                    ProductId: ProductId

                }
            IsSubmit = true;
            $("#btnSubmit").html("正在提交...");
            $.ajax({
                type: 'post',
                url: mallHandlerUrl,
                data: ajaxData,
                timeout: 60000,
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 1) {
                        window.location = "dowxpay.aspx?oid=" + resp.ExStr;
                        return;
                    }
                    else {
                        alert(resp.Msg);
                        return;
                    }
                },
                complete: function () {
                    $("#btnSubmit").html("<img src=\"Customize/BXGT/images/tj.png\" />");
                    IsSubmit = false;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("操作超时，请重新提交订单");
                    }
                    else {
                        alert(textStatus + "请重新提交订单");
                    }
                }
            });

        })

    })
    //获取Get参数
    function GetParm(parm) {
        //获取当前URL
        var local_url = window.location.href;

        //获取要取得的get参数位置
        var get = local_url.indexOf(parm + "=");
        if (get == -1) {
            return "";
        }
        //截取字符串
        var get_par = local_url.slice(parm.length + get + 1);
        //判断截取后的字符串是否还有其他get参数
        var nextPar = get_par.indexOf("&");
        if (nextPar != -1) {
            get_par = get_par.slice(0, nextPar);
        }
        return get_par;
    }
    //获取参数
</script>

</html>