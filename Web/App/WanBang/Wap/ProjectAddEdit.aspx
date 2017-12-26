<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.ProjectAddEdit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title><%=string.IsNullOrEmpty(Request["id"])?"发布":"修改项目信息"%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link type="text/css" rel="stylesheet" href="../Css/wanbang.css">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <style>
    .selectbox {
height: 28px;
border: 1px solid #d1d1d1;
border-radius: 5px;
background: #fff;
color: #404040;
font-size: 12px;
font-weight: bold;
}
    
    </style>
</head>


<body>
        <!-- 发布项目 -->
        <div class="fbxm">
            <form method="post" action="#">
                <p>
                    <label>项目名称：</label><br />
                    <input id="txtProjectName"  style="width:95%;" class="input100 name" type="text" placeholder="项目名称(必填)" value="<%=model.ProjectName%>">
                   

                </p>
                <p>
                    <label>项目分类：</label>
                    <select id="ddlCategory" class="selectbox" style="width:60%;" >
                    <option value="">选择分类</option>
                    <option value="粘贴折叠">粘贴折叠</option>
                    <option value="成品包装">成品包装</option>
                    <option value="组件装配">组件装配</option>
                    <option value="加工制作">加工制作</option>
                    <option value="纺织串接">纺织串接</option>
                    <option value="缝纫整熨">缝纫整熨</option>
                    <option value="其它项目">其它项目</option>
                    <option value="阳光办公室">阳光办公室</option>
                    </select>
                </p>
                <p>
                    <label>项目周期：</label>
               <select id="ddlProjectCycle" class="selectbox" style="width:60%;">
               <option value="">选择周期</option>
               <option value="0">临时(1个月以内)</option>
               <option value="1">短期(1-3个月)</option>
               <option value="2">中期(3-6个月)</option>
               <option value="3">长期(6-12个月)</option>
               </select>
                    

                </p>
                <p class="logistics">

                    <span class="wl">项目物流：</span>
                    <input id="enterprise" class="radiobox" type="radio" checked="checked"  name="radiogroup" value="1">
                    <label for="enterprise"><span class="wbtn"><span class="iconfont"></span></span>企业负责配送</label>
                    <input class="radiobox" type="radio" id="base" name="radiogroup" value="0"><label for="base"><span class="wbtn"><span class="iconfont"></span></span>基地负责配送</span>

                </p>
                <p>
                    <label style="float:none;">工期要求：</label><br/>


                    <textarea class="req" rows="10" id="txtTimeRequirement" placeholder="工期要求"><%=model.TimeRequirement%></textarea>
                </p>
                <p style="margin-bottom:0;">
                    <label style="float:none;">项目介绍：</label><br/>
                    <textarea id="txtIntroduction" placeholder="项目介绍" class="req" rows="10" style="height:88px;"><%=model.Introduction%></textarea>
                </p>
                <input class="enter" type="button" id="btnSave" value="<%=string.IsNullOrEmpty(Request["id"])?"发布":"保存"%>">
            </form>
        </div>
        <!--/ 发布项目 -->
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

         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        AutoID: '<%=model==null?0:model.AutoID%>',
                        Action: '<%=Request["action"]%>' == 'edit' ? 'EditProjectInfo' : 'AddProjectInfo',
                        ProjectName: $.trim($("#txtProjectName").val()),
                        Category: $.trim($('#ddlCategory').val()),
                        Logistics: $("input[name='radiogroup']:checked").val(),
                        ProjectCycle: $.trim($('#ddlProjectCycle').val()),
                        TimeRequirement: $.trim($('#txtTimeRequirement').val()),
                        Introduction: $.trim($('#txtIntroduction').val())
                    };
                 if (model.ProjectName == '') {
                     $('#txtProjectName').focus();
                     return;
                 }
                 if (model.Category == '') {
                     alert("请选择分类");
                     return;
                 }

                 if (model.Logistics == '') {
                     alert("请选择物流");
                     return;
                 }
                 if (model.ProjectCycle == '') {
                     alert("请选择周期");
                     return;
                 }

                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             alert("保存成功");
                             window.location = "MyPubProject.aspx";
                         }
                         else {
                             alert(resp.Msg);
                         }
                     }
                 });

             } catch (e) {
                 alert(e);
             }


         });


         <%if (Request["id"]!=null) {%>
         $("#ddlProjectCycle").val("<%=model.ProjectCycle%>");
         $("#ddlCategory").val("<%=model.Category%>");
         var Logistics="<%=model.Logistics%>";
         if ($.trim(Logistics)=="0") {
            $("#base").attr("checked","checked");
            }
            else {
                $("#enterprise").attr("checked","checked");
              
            }
            
       <%} %>

     });
 </script>
</html>
