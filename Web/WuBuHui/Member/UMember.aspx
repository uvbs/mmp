<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Member.UMember" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>修改用户信息</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.7">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
    <h3 class="maintitle">
        个人资料</h3>
    <%--<%=uinfo.UserID %>
    <%=Uid %>--%>
    <div class="personerinfo">
        <form action="">
        <div class="listbox">
            <input type="text" placeholder="姓名" id="txtName" name="txtNmae" maxlength="15">
            <label for="" class="wbtn_line_main">
                <span class="iconfont icon-b24"></span>
            </label>
        </div>
        <div class="listbox">
            <input type="number" placeholder="手机" id="txtPhone" name="txtPhone" pattern="\d*">
            <label for="" class="wbtn_line_main">
                <span class="iconfont icon-b47"></span>
            </label>
        </div>
        <div class="listbox">
            <input type="email" placeholder="邮箱" id="txtEmail" name="txtEmail">
            <label for="" class="wbtn_line_main">
                <span class="iconfont icon-b12"></span>
            </label>
        </div>
        <div class="listbox">
            <input type="text" placeholder="公司" id="txtCompanyl" name="txtCompanyl">
            <label for="" class="wbtn_line_main">
                <span class="iconfont icon-b42"></span>
            </label>
        </div>
        </form>
    </div>
    <div class="wcontainer hupenghuanyou">
        <a href="/WuBuHui/MyCenter/MyCenter.aspx" class="wbtn wbtn_main" style="margin-right: 10px;">
            <span class="text">取消</span> </a><span onclick="SubInfo()" class="wbtn wbtn_red"><span
                class="text">修改信息</span> </span>
    </div>
    <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        提交成功</p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="../js/jquery.js" type="text/javascript"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            GetInfo();
        });

        //获取用户信息
        function GetInfo() {
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
                data: { Action: "GetCurrUserInfo" },
                success: function (result) {
                    var resp = $.parseJSON(result);
                    if (resp.Status == 0) {
                        $("#HeadImg").attr("src", resp.ExObj.WXHeadimgurl);
                        $("#txtName").val(resp.ExObj.TrueName);
                        $("#txtPhone").val(resp.ExObj.Phone)
                        $("#txtEmail").val(resp.ExObj.Email)
                        $("#txtCompanyl").val(resp.ExObj.Company)
                    }
                    else {
                        alert("系统出错")
                    }
                }
            });
        }

        function SubInfo() {
            var model = {
                Action: 'UpdateMyInfo',
                Uname: $.trim($("#txtName").val()),
                UPhone: $.trim($("#txtPhone").val()),
                UEmail: $.trim($("#txtEmail").val()),
                UCompanyl: $.trim($("#txtCompanyl").val())
            };
            
           if (model.Uname == null) {
                $('#gnmdb').find("p").text("请输入用户名");
                $('#gnmdb').modal('show');
                $("#txtNmae").focus();
                return;
            }

            if (model.UPhone == null) {
                $('#gnmdb').find("p").text("请输入手机号");
                $('#gnmdb').modal('show');
                $("#txtPhone").focus();
                return;
            }

            var myreg = /^(13|14|15|18)\d{9}$/;
            if (!myreg.test(model.UPhone)) {
                $('#gnmdb').find("p").text('请输入有效的手机号码！');
                $('#gnmdb').modal('show');
                $("#txtPhone").focus();
                return;
            }
            if (model.UEmail == null) {
                $('#gnmdb').find("p").text("请输入邮箱");
                $('#gnmdb').modal('show');
                $("#txtEmail").focus();
                return;
            }
            var pattern = /^([\.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
            if (!pattern.test(model.UEmail)) {
                $('#gnmdb').find("p").text("请输入正确的邮箱地址。");
                $('#gnmdb').modal('show');
                $("#txtEmail").focus();
                return;
            }
            if (model.UCompanyl == null) {
                $('#gnmdb').find("p").text("请输入公司名称");
                $('#gnmdb').modal('show');
                $("#txtCompanyl").focus();
                return;
            }
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        $('#gnmdb').find("p").text("保存成功!");
                        $('#gnmdb').modal('show');
                        setTimeout("window.location.href = \"/WuBuHui/MyCenter/MyCenter.aspx\";",2000);
                    }
                    else {
                        $('#gnmdb').find("p").text(resp.Msg);
                        $('#gnmdb').modal('show');
                    }
                }
            });
        };
    </script>

    <div class="modal fade"id="alertok" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        提交成功</p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
</body>
</html>
