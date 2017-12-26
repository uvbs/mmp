<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="NavigateCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.NavigateCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
       input[type=text],select,textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
             
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;" onclick="window.location.href='/App/Cation/ArticleManage.aspx'">模块管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>模块<%if (model != null && webAction == "edit") { Response.Write("：" + model.NavigateName); } %></span>
    <a href="NavigateManage.aspx" style="float:right;margin-right:20px;" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNavigateName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                       
                        <textarea id="txtNavigateDescription" style="width:100%;height:50px;"></textarea>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        顺序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPlayIndex" value="0" class="" style="width: 100px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                        顺序从小到大排列
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        封面图片：
                    </td>
                    <td width="*" align="left">
                       
                         <img alt="图片" src="/img/hb/hb1.jpg" width="200px" height="100px" id="imgThumbnailsPath" /><br />
                         <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传图片</a><br />
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为720*360。
                        <br />
                        <input type="file" id="txtThumbnailsPath" style="display:none;" name="file1" />       
                               
                        
                      
                        
                    </td>
                </tr>


                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        是否显示：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsShow" id="rdoShow" checked="checked" value="1"/><label
                            for="rdoShow">显示</label>
                        <input type="radio" name="IsShow" id="rdoHide" value="0" /><label for="rdoHide">隐藏</label>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       类型：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlNavigateType">
                        <option value="链接">链接</option>
                        <option value="分类">分类</option>
                        <option value="图文">图文</option>
                        </select>
                        <a id="btnSelectCategory" class="easyui-linkbutton" style="display:none">点击选择分类</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       类型值：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNavigateTypeValue" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;width:200px;" class="button button-rounded button-flat">
                                重置</a> 
                                
                                
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>

    <div id="dlgInput" class="easyui-dialog" closed="true" title="选择分类" style="width: 480px;
        height:370px; ">
        <table id="grvData" fitcolumns="true">
    </table>
    </div>
    </div>

    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currAction = '<%=webAction %>';
     var currId = '<%=model.AutoID %>';
     $(function () {


         if (currAction == 'edit') {

             $('#txtNavigateName').val("<%=model.NavigateName %>");
             $('#txtNavigateDescription').val("<%=model.NavigateDescription %>");
             $('#txtPlayIndex').val("<%=model.PlayIndex %>");
             $('#imgThumbnailsPath').attr("src", "<%=model.NavigateImage %>");
             var isshow = "<%=model.IsShow %>";
             if (isshow == "1") {
                 $("#rdoShow").attr("checked", "checked");
             }
             else {
                 $("#rdoHide").attr("checked", "checked");
             }

             $('#ddlNavigateType').val("<%=model.NavigateType %>");
             $('#txtNavigateTypeValue').val("<%=model.NavigateTypeValue %>");

             var NavigateType = "<%=model.NavigateType %>";

             if (NavigateType == "分类") {
                 $("#btnSelectCategory").show();
                 $("#txtNavigateTypeValue").attr("readonly", "readonly");
             }
             else {
                 $("#btnSelectCategory").hide();
                 $("#txtNavigateTypeValue").removeAttr("readonly");

             }

         }

         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        Action: currAction == 'add' ? 'AddCompanyWebsiteNavigate' : 'EditCompanyWebsiteNavigate',
                        AutoID: currId,
                        NavigateName: $.trim($('#txtNavigateName').val()),
                        NavigateDescription: $.trim($('#txtNavigateDescription').val()),
                        PlayIndex: $.trim($('#txtPlayIndex').val()),
                        NavigateImage: $('#imgThumbnailsPath').attr('src'),
                        IsShow: $(":radio[name=IsShow]:checked").val(),
                        NavigateType: $('#ddlNavigateType').val(),
                        NavigateTypeValue: $('#txtNavigateTypeValue').val()

                    }

                 if (model.NavigateName == '') {
                     $('#txtNavigateName').focus();

                     return;
                 }
                 if (model.NavigateImage == '') {
                     Alert("请上传图片");
                     return;
                 }
                 if (model.PlayIndex == '') {
                     $('#txtPlayIndex').focus();
                     return;
                 }
                 $.messager.progress({ text: '正在处理。。。' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {
                             if (currAction == 'add')
                                 ResetCurr();
                             Alert(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }
                 });

             } catch (e) {
                 Alert(e);
             }


         });

         $('#btnReset').click(function () {
             ResetCurr();

         });



         $("#txtThumbnailsPath").live('change', function () {
             try {

                 $.messager.progress({ text: '正在上传图片。。。' });
                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 imgThumbnailsPath.src = resp.ExStr;
                                 $('#imgThumbnailsPath').attr('path', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         $("#ddlNavigateType").change(function () {

             if ($(this).val() == "分类") {
                 $("#btnSelectCategory").show();
                 $("#txtNavigateTypeValue").attr("readonly", "readonly");
             }
             else {
                 $("#btnSelectCategory").hide();
                 $("#txtNavigateTypeValue").removeAttr("readonly");

             }


         });

         $("#btnSelectCategory").click(function () {


             $('#dlgInput').dialog({ title: '选择分类' });
             $('#dlgInput').dialog('open');




         });

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: "/Handler/App/CationHandler.ashx",
	                queryParams: { Action: "QueryArticleCategory" },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { field: 'CategoryName', title: '分类名称', width: 100, align: 'left' }

                             ]]
	            }
            );

         $('#dlgInput').dialog({
             buttons: [{
                 text: '确定',
                 handler: function () {
                     var row = $('#grvData').datagrid('getSelected');
                     $("#txtNavigateTypeValue").val(row.AutoID);
                     $('#dlgInput').dialog('close');

                 }
             }, {
                 text: '取消',
                 handler: function () {

                     $('#dlgInput').dialog('close');
                 }
             }]
         });



     });





     function ResetCurr() {


         var playindex = $("#txtPlayIndex").val();
         playindex++;
         $(":input[type=text]").val("");
         $("textarea").val("");
         $("#txtPlayIndex").val(playindex);
     }

    </script>
</asp:Content>


