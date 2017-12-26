<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseInfoEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.BaseInfoEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>基地资料</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link type="text/css" rel="stylesheet" href="../Css/wanbang.css">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>

</head>
<body>
        <!-- 基地资料 -->
        <div style="padding:0 10px"> <img  src="<%=model.Thumbnails%>" /></div>
        <div class="base-info">
            <form method="post" action="#">
                <p>
                    <label>基地名称：</label>
                    <input id="txtBaseName" placeholder="基地名称(必填)"  style="width:60%;" class="input101" type="text" value="<%=model.BaseName%>">
                  
                </p>
                <p>
                    <label>所属区：</label>

                <select id="ddlArea" class="selectbox" style="width:60%;" >
               <option value="黄浦区">黄浦区</option>
               <option value="长宁区">长宁区</option>
               <option value="徐汇区">徐汇区</option>
               <option value="静安区">静安区</option>
               <option value="杨浦区">杨浦区</option>
               <option value="虹口区">虹口区</option>
               <option value="闸北区">闸北区</option>
               <option value="普陀区">普陀区</option>
               <option value="浦东新区">浦东新区</option>
               <option value="宝山区">宝山区</option>
               <option value="闵行区">闵行区</option>
               <option value="金山区">金山区</option>
               <option value="嘉定区">嘉定区</option>
               <option value="青浦区">青浦区</option>
               <option value="松江区">松江区</option>
               <option value="奉贤区">奉贤区</option>
               <option value="崇明县">崇明县</option>
               </select>
                </p>
                <p>
                    <label>地址：</label>
                    <input style="width:60%;" class="input101 cycle" type="text" id="txtAddress" placeholder="基地地址" value="<%=model.Address%>">
                    
                </p>
                <p>
                    <label>电话：</label>
                    <input class="input101 cycle" type="text" id="txtTel" placeholder="电话" value="<%=model.Tel%>">
                   
                </p>
                <p>
                    <label>手机：</label>
                    <input class="input101 cycle" type="text" id="txtPhone" placeholder="手机" value="<%=model.Phone%>">

                </p>
                <p>
                    <label>QQ：</label>
                    <input style="width:54.3%;" class="input101 cycle" type="text" id="txtQQ" placeholder="QQ" value="<%=model.QQ%>">
                     
                </p>
                <p>
                    <label>负责人： </label>
                     
                    <input style="width:45.3%;" class="input101 cycle" type="text" id="txtContacts" placeholder="负责人(必填)" value="<%=model.Contacts%>">
                </p>
                <p>
                    <label>基地面积： </label>
                     
                    <input style="width:45.3%;" class="input101 cycle" type="text" id="txtAcreage" placeholder="基地面积" value="<%=model.Acreage%>">&nbsp;平方米
                </p>
                <p>
                    <label>援助人数： </label>
                     
                    <input style="width:45.3%;" class="input101 cycle" type="text" id="txtHelpCount" placeholder="帮助人数" value="<%=model.HelpCount%>">&nbsp;人
                </p>
              
                <p class="projectcont">
                    <label style="float:none;">基地介绍：</label><br>
                    <textarea class="req" rows="10"  id="txtIntroduction" placeholder="基地介绍"><%=model.Introduction%></textarea>




                </p>
                <div style="margin:20px auto 0; height:40px;"><input class="enter" type="button" value="保存" id="btnSave" ></div>
            </form>
        </div>
        <!--/ 基地资料 -->
        <div class="blank"></div>
        <!-- Back -->
<a href="javascript:window.history.go(-1)" class="back">
    <span class="iconfont icon-fanhui"></span>
</a>
        <!--/ Back -->
	</body>
 <script type="text/javascript">
     var handlerUrl = "/Handler/WanBang/Wap.ashx";
     $(function () {
         $("#ddlArea").val("<%=model.Area%>");
         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        Action: "UpdateBaseInfo",
                        BaseName: $.trim($("#txtBaseName").val()),
                        Thumbnails: $('#imgThumbnailsPath').attr('src'),
                        Address: $.trim($("#txtAddress").val()),
                        Area: $.trim($('#ddlArea').val()),
                        Tel: $.trim($('#txtTel').val()),
                        Phone: $.trim($('#txtPhone').val()),
                        QQ: $.trim($('#txtQQ').val()),
                        Contacts: $.trim($('#txtContacts').val()),
                        Acreage: $.trim($('#txtAcreage').val()),
                        HelpCount: $.trim($('#txtHelpCount').val()),
                        Introduction: $("#txtIntroduction").val()
                    };


                 if (model.BaseName == '') {
                     $('#txtBaseName').focus();
                     return;
                 }
                 if (model.Contacts == '') {
                     $('#txtContacts').focus();
                     return;
                 }

                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             window.location = "BaseCenter.aspx";
                         }
                         alert(resp.Msg);
                     }
                 });

             } catch (e) {
                 alert(e);
             }


         });

     });




 </script>
</html>

