<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterBinding.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Member.Wap.RegisterBinding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title><%=pageName %></title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet" type="text/css" />
    <link href="/App/Cation/Wap/Style/styles/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" type="text/css" />
    <style>
        @charset "utf-8";
        @import url('<%= ico_css_file%>');
        body {
            font-size: 14px;
        }

        img {
            max-width: 100%;
        }

        .padding20 {
            padding: 20px;
        }

        .allRadius20 {
            border-radius: 20px;
        }

        .itemDiv {
            padding: 0px 20px 20px 20px;
        }

        .postDiv {
            padding: 0px 20px 20px 20px;
        }

        .item {
            border: 0px;
            border-bottom: 1px solid #44A5DF;
            padding: 5px;
            background-color: transparent !important;
        }

            .item input {
                padding-left: 15px;
                height: 40px;
            }

            .item i {
                color: #2690d0;
                font-size: 22px;
            }

        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
        }

        .item input[type="number"] {
            -moz-appearance: textfield;
            -webkit-appearance: textfield;
        }
        
        .item input[type="radio"] {
            -moz-appearance:radio;
            -webkit-appearance: radio;
            height:23px;
            position:absolute;
            top:11px;
        }
        .item textarea{
            font-size:14px;
            line-height:16px;
            padding:5px 18px;
        }
        .imgRmk{
            margin-left:10px;
            width:80px;
            font-size: 14px;
            color: #111;
        }
        .sexRmk{
            margin-left:30px;
        }
        .sexRmk label{
            margin-right:30px;
            margin-left: 15px;
        }
        .width150{
            width:150px;
        }
        .height100{
            width:100px;
        }
        .code{
            flex: 1 100px !important;
        }
        .colorRed{

        }
        .bgDiv {
            position: fixed;
            top: 0;
            left: 0;
            z-index: 2;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0, .5);
        }
        .aInfo{
            width: 300px;
            height: 450px;
            left:50%;
            top:45px;
            margin-left: -150px;
            position: absolute;
            background-color:#ffffff;
        }
        .noMove{
            overflow: hidden;
        }
        .aInfoTitle{  
            border: 0px;
            padding: 5px;  
            line-height: 1.5;
            font-size:16px;
            color: #0095E1;
        }
        .infoContent{
            height:335px;
            overflow-y:auto;
            overflow-x:hidden;
            padding-bottom:10px;
        }
        .infoContent label{
            white-space: normal;
        }
        .aInfoConfim{  
            border: 0px;
            padding: 5px;  
            line-height: 1.5;
            font-size:16px;
            color: #ee745f;
        }
        .alignCenter{
            text-align:center;
        }
    </style>
</head>
<body>
    <div class="padding20">
        <%=memberStandardDescription %>
    </div>
    <div class="itemDiv">
            <% foreach (var item in formField)
               { %>
        
            <% //照片
                   if (item.FieldType != null && item.FieldType.Equals("img"))
                   {%>
        
        <label class="item item-input">
            <span class="imgRmk">
                <%=item.MappingName %>
            </span>
            <span>
                <img class="width150 height100 upImg img<%=item.Field %> <%= item.Disabled == 1?"imgno":"imgyes" %>" data-field="<%=item.Field %>" src="<%=string.IsNullOrWhiteSpace(item.Value)?"http://open-files.comeoncloud.net/www/guicai/jubit/image/20160325/A12476F2D0CD4B8FA037267F393BF69E.png":item.Value %>">
                <input id="file<%=item.Field %>" type="file" name="file1" class="upFile file<%=item.Field %>" data-field="<%=item.Field %>" style="display: none;" />
            </span>
        </label>
            <% }%>
            <% //手机
                   else if (item.Field.Equals("Phone"))
                   {%>

                <label class="item item-input">
                    <i class="icon icon-shouji iconfont"></i>
                    <input type="number" class="<%=item.Field %>" placeholder="<%=item.MappingName %>"  value="<%=item.Value %>" <%= item.Disabled == 1?"disabled='disabled'":"" %> />
                </label>
                <label class="item item-input">
                    <i class="icon icon-lock iconfont"></i>
                    <input type="number" class="code" placeholder="验证码">
                    <button type="button" class="button button-small button-positive getSMSCode allRadius20" onclick="getSMSCode()">发送验证码</button>
                </label>
            <% }%>
            <% //性别
                   else if (item.FieldType != null && item.FieldType.Equals("sex"))
                   {%>
                <label class="item item-input">
                    <i class="icon icon-userreg iconfont"></i>
                    <span class="sexRmk">
                    <input type="radio" id="rdoSex<%=item.Field %>1" name="rdoSex<%=item.Field %>" <%= item.Value == "1"?"checked='checked'":"" %> <%= item.Disabled == 1?"disabled='disabled'":"" %> value="1" /><label for="rdoSex<%=item.Field %>1">男</label>
                    <input type="radio" id="rdoSex<%=item.Field %>2" name="rdoSex<%=item.Field %>" <%= item.Value == "0"?"checked='checked'":"" %> <%= item.Disabled == 1?"disabled='disabled'":"" %> value="0" /><label for="rdoSex<%=item.Field %>2">女</label>
                    </span>
                </label>
            <% }%>
            <% //多行文本
                   else if (item.FieldType != null && item.FieldType.Equals("mult"))
                   {%>
                <label class="item item-input">
                    <i class="icon icon-bianji iconfont"></i>
                    <textarea class="<%=item.Field %>" rows="5" placeholder="<%=item.MappingName %>" <%= item.Disabled == 1?"disabled='disabled'":"" %>><%=item.Value %></textarea>
                </label>
            <% }%>

            <%  else
                   {
                       var inputType = "text";
                       if (item.FieldType != null && item.FieldType.Equals("date")) inputType = "date";
                       if (item.FieldType != null && item.FieldType.Equals("number")) inputType = "number";
            %>
                <label class="item item-input">
                    <i class="icon icon-bianji iconfont"></i>
                    <input type="<%=inputType %>" class="<%=item.Field %>" placeholder="<%=item.MappingName %>"  value="<%=item.Value %>" <%= item.Disabled == 1?"disabled='disabled'":"" %> />
                </label>
            <% } %>
            <% }%>
    </div>
    <div class="itemDiv">
        <button type="button" class="button button-full button-positive allRadius20" onclick="postCode()">提交</button>
    </div>
    <div class="bgDiv" style="display:none;">
        <div class="aInfo">
            <div class="aInfoTitle">
                手机已有账号信息
            </div>
            <div class="infoContent">
                <div class="infoDetail">

                </div>
            </div>
            <div class="aInfoConfim alignCenter">
                手机已有账号，是否进行绑定？
            </div>
            <div class="infoButtonDiv alignCenter">
                <button type="button" class="button button-small button-positive allRadius20" onclick="postInfoBinding()">确认绑定</button>
                <button type="button" class="button button-small button-light allRadius20" onclick="closeBgDiv()">关闭</button>
            </div>
        </div>
    </div>
</body>
</html>
    <script src="http://static-files.socialcrmyun.com/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">
    var nlayerprogress;
    $(function () {
        window.alert = function (msg) {
            layer.open({
                content: msg,
                btn: ['确认'],
                time: 2
            });
        };
        $(".upImg.imgyes").live("click",function(){
            var nfield = $(this).attr("data-field");
            $(".file"+nfield).click();
        });
        $(".upFile").live('change', function () {
            var nfield = $(this).attr("data-field");
            try {
                nlayerprogress = layer.open({type: 2,time: 300});
                $.ajaxFileUpload(
                 {
                     url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=MemberInfo',
                     secureuri: false,
                     fileElementId: "file"+nfield,
                     dataType: 'json',
                     success: function (resp) {
                         layer.close(nlayerprogress);
                         if(resp.status === false){
                             alert(resp.msg);
                             return;
                         }
                         if (resp.Status == 1) {
                             $(".img"+nfield).attr("src",resp.ExStr);
                         }
                         else {
                             alert(resp.Msg);
                         }
                     }
                 });

            } catch (e) {
                layer.close(nlayerprogress);
                alert("访问出错");
            }
        });
    })
    var timeNum = 0;
    var Inter;
    var handerUrl = "/Serv/API/Member/";
    var redirect = '<%= Request["redirect"]%>';
    var uid = 0;
    function startInter() {
        Inter = setInterval(function () {
            timeNum--;
            if (!$(".getSMSCode").hasClass("button-stable")) {
                $(".getSMSCode").addClass("button-stable");
                $(".getSMSCode").removeClass("button-positive");
            }
            $(".getSMSCode").html("等待(" + timeNum + ")");
            if (timeNum <= 0) {
                $(".getSMSCode").addClass("button-positive");
                $(".getSMSCode").removeClass("button-stable");
                $(".getSMSCode").html("发送验证码");
                clearInterval(Inter);
            }
        }, 1000);
    }
    function getSMSCode() {
        if (timeNum > 0) return;
        var phone = $.trim($(".Phone").val());
        if (phone == "") {
            alert("请输入手机号码");
            return;
        }
        timeNum = 90;
        startInter();
        nlayerprogress = layer.open({type: 2,time: 90});
        $.ajax({
            type: 'POST',
            url: handerUrl + "GetSMSCode.ashx",
            data: { phone: phone },
            success: function (result) {
                layer.close(nlayerprogress);
                alert(result.msg);
                if (result.status === false) {
                    timeNum = 0;
                }
            },
            failure: function (result) {
                layer.close(nlayerprogress);
                alert("访问出错");
            }
        });
    }
    function postCode() {
        var model = {};
        var Phone = $.trim($(".Phone").val());
        if (Phone == "") {
            alert("请完善手机号码");
            $(".Phone").focus();
            return;
        }
        model.Phone = Phone;
        var code = $.trim($(".code").val());
        if (code == "") {
            alert("请输入手机验证码");
            return;
        }
        model.code = code;
        <% foreach (var item in formField)
           { %>

        <% if (item.FieldType != null && item.FieldType.Equals("img"))
           {%>
        var <%=item.Field %> = $.trim($(".img<%=item.Field %>").attr("src"));
        if (<%=item.FieldIsNull %> == 1 && (<%=item.Field %> == "" || <%=item.Field %> == "http://open-files.comeoncloud.net/www/guicai/jubit/image/20160325/A12476F2D0CD4B8FA037267F393BF69E.png")) {
            alert("请上传<%=item.MappingName %>");
            return;
        } 
        model.<%=item.Field %> = <%=item.Field %>;
        <% }%>
                <% else if (item.FieldType != null && item.FieldType.Equals("sex"))
           {%>
        var <%=item.Field %> = $.trim($('input[type="radio"][name="rdoSex<%=item.Field %>"]:checked').val());
        if (<%=item.Field %> == "") {
            alert("请选择<%=item.MappingName %>");
            $(".<%=item.Field %>").focus();
            return;
        } 
        model.<%=item.Field %> = <%=item.Field %>;
        <% } %>

                <% else if (!item.Field.Equals("Phone"))
           {%>
        var <%=item.Field %> = $.trim($(".<%=item.Field %>").val());
        if (<%=item.Field %> == "" && <%=item.FieldIsNull %> == 1) {
            alert("请完善<%=item.MappingName %>");
            $(".<%=item.Field %>").focus();
            return;
        } 
        model.<%=item.Field %> = <%=item.Field %>;
        <% } %>

        <% } %>
        nlayerprogress = layer.open({type: 2,time: 300});
        $.ajax({
            type: 'POST',
            url: handerUrl + "RegisterBinding.ashx",
            data: model,
            success: function (result) {
                layer.close(nlayerprogress);
                if (result.status) {
                    if(redirect != ""){
                        document.location.href = redirect;
                    }
                    else{
                        document.location.href = "/customize/comeoncloud/Index.aspx?key=MallHome";
                    }
                }
                else {
                    if(result.code == 10031){
                        uid = result.result.id;
                        showBgDiv(result.result.info_list)
                    }
                    else{
                        alert(result.msg);
                    }
                }
            },
            failure: function (result) {
                layer.close(nlayerprogress);
                alert("访问出错");
            }
        });
    }
    function showBgDiv(info){
        var appendhtml = new StringBuilder();
        for (var i = 0; i < info.length; i++) {
            appendhtml.AppendFormat('<label class="item item-input">{0}：{1}</label>',info[i].field_name,info[i].value);
        }
        $(".infoDetail").html("")
        $(".infoDetail").append(appendhtml.ToString());
        console.log( $(window).height());
        console.log($(".infoContent").height());
        $(".bgDiv").show();
        $("html").addClass("noMove");
        return;
    }
    function closeBgDiv(){
        $(".bgDiv").hide();
        $("html").removeClass("noMove");
    }
    function postInfoBinding(){
        var model = {};
        model.id = uid;
        var Phone = $.trim($(".Phone").val());
        if (Phone == "") {
            alert("请完善手机号码");
            $(".Phone").focus();
            return;
        }
        model.Phone = Phone;
        nlayerprogress = layer.open({type: 2,time: 300});
        $.ajax({
            type: 'POST',
            url: handerUrl + "Binding.ashx",
            data: model,
            success: function (result) {
                layer.close(nlayerprogress);
                if (result.status) {
                    if(redirect != ""){
                        document.location.href = redirect;
                    }
                    else{
                        document.location.href = "/customize/comeoncloud/Index.aspx?key=MallHome";
                    }
                }
                else {
                    alert(result.msg);
                }
            },
            failure: function (result) {
                layer.close(nlayerprogress);
                alert("访问出错");
            }
        });
    }
</script>
