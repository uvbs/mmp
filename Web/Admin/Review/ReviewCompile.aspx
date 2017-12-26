<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ReviewCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Review.ReviewCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;<a href="ReviewList.aspx">话题管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>添加话题</span>
    <a href="ReviewList.aspx" style="float: right; margin-right: 20px;" title="返回活动列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text"   id="txtTitle" style="width: 100%;height:25px;" placeholder="标题(必填)" />
                    </td>
                </tr>
               
               
              

                <tr>
                    <td style="width: 100px;margin-top:20px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
               

            </table>
            <br />
             <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                            <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a>
                            
                        
                        <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;"
                                class="button button-rounded button-flat">重置</a>
                            </div>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
   <script type="text/javascript">

       var editor;
       var handlerUrl = "Handler/ReviewHandler.ashx";
      
       $(function () {
           $("#btnSave").click(function () {
               var dataModel = {
                   Action: "Add",
                   title: $.trim($('#txtTitle').val()),
                   content: editor.html()
               }
               if (dataModel.Title == '') {

                   Alert('请输入标题');
                   return;
               }
               if (dataModel.content == '') {
                   Alert('请输入内容');
                   return;
               }
               $.ajax({
                   type: 'post',
                   url: handlerUrl,
                   data: dataModel,
                   dataType: "json",
                   success: function (resp) {
                       if (resp.Status == 1) {
                           ResetCurr();
                       }
                       Alert(resp.Msg);
                   }
               });
           });
       });

       KindEditor.ready(function (K) {
           editor = K.create('#txtEditor', {
               uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
               items: [
                   'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                   'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                   'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
               filterMode: false
           });
       });
       
       function ResetCurr() {
           ClearAll();
           editor.html('');
       }
       

   </script>
    
</asp:Content>
