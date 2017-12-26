<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="TheVoteUserInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TheVote.TheVoteUserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;所有投票&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>投票人信息</span>


     <a href="TheVoteInfoMgr.aspx" style="float: right; margin-right: 20px;" title=""
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            投票选项
             <select id="ddlItems">
                 <option value="">全部</option>

                 <% foreach (var item in Items)
                    {
                        Response.Write(string.Format(" <option value=\"{0}\">{1}</option>",item.AutoID,item.ValueStr));
                        
                    } %>
             </select>
           
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEditTag();" id="BtnTag">设置标签</a>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

        <div id="dlgTag" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">

        <table id="grvTagData" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXTheVoteInfoHandler.ashx";
     var domain = '<%=Request.Url.Host %>';
     var VoteId = '<%=id %>';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetVoteTheUserInfos", VoteId: VoteId },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 1000,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Ex1', title: '投票选项', width: 10, align: 'left' },
                                {
                                    field: 'WXHeadimgurl', title: '头像', width: 10, align: 'center', formatter: function (value) {
                                                            if (value == '' || value == null)
                                                                return "";
                                                            var str = new StringBuilder();
                                                            str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                                            return str.ToString();
                                                        }
                                },
                                { field: 'WXNickname', title: '昵称', width: 10, align: 'left' },
                                { field: 'TagName', title: '标签', width: 10, align: 'left' },
                                { field: 'TrueName', title: '真实姓名', width: 10, align: 'left' },
                                { field: 'Phone', title: '手机号码', width: 10, align: 'left' },
                                { field: 'Email', title: '邮箱', width: 10, align: 'left' },
                             ]]
	            }
            );


         //标签列表
         $('#grvTagData').datagrid(
             {
                 method: "Post",
                 url: "/Handler/App/CationHandler.ashx",
                 queryParams: { Action: "QueryMemberTag", TagType: "member" },
                 height: 400,
                 pagination: true,
                 striped: true,
                 pageSize: 1000,
                 rownumbers: true,
                 rowStyler: function () { return 'height:25px'; },
                 columns: [[
                             { title: 'ck', width: 5, checkbox: true },
                             { field: 'TagName', title: '标签名称', width: 20, align: 'left' }
                 ]]
             }
         );


         $(ddlItems).change(function () {


             Search();


         })

         //设置用户标签
         $('#dlgTag').dialog({

             buttons: [{
                 text: '设置标签',
                 handler: function () {
                     var rows = $('#grvData').datagrid('getSelections');

                     var rowsTag = $('#grvTagData').datagrid('getSelections');
                     if (!EGCheckIsSelect(rowsTag)) {
                         return;
                     }

                     var  autoIds = [];
                     var tagName = [];
                    

                     for (var i = 0; i < rowsTag.length; i++) {
                         tagName.push(rowsTag[i].TagName);
                        
                     }

                     for (var i = 0; i < rows.length; i++) {
                         autoIds.push(rows[i].AutoID);
                     }
                    
                     var dataModel = {
                         Action: "SetTheVoteUserTag",
                         AutoIDs: autoIds.join(','),
                         TagName: tagName.join(',')
                        
                     };
                     $.messager.progress({ text: '正在提交...' });
                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 Alert("操作成功");
                                 $('#dlgTag').dialog('close');
                                 $('#grvData').datagrid('reload');
                             }
                             else {

                                 // Alert(resp.Msg);
                                 Alert("操作成功");
                                 $('#dlgTag').dialog('close');
                             }
                         }
                     });

                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgTag').dialog('close');
                 }
             }]
         });



     });


     //删除
     function Delete() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteTheVoteInfo", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 0) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });
     }
     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].AutoId);
         }
         return ids;
     }

     function ConfigBarCodeInfoEdit() {
         $('#dlgPmsInfo').window(
            {
                title: '管理配置返回结果'
            }
            );

         $.post(handlerUrl, { Action: "GetConfigureConfigInfo" }, function (data) {
             var resp2 = $.parseJSON(data);
             if (resp2.Status == 0) {
                 $("#txtQueryNum").val(resp2.ExObj.QueryNum);
                 $("#txtPopupInfoon").val(resp2.ExObj.PopupInfo);
             }
             $('#dlgPmsInfo').dialog('open');
         });
     }




     function Search() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetVoteTheUserInfos", VoteId: VoteId,ItemId:$(ddlItems).val() },
	            });
     }

     //窗体关闭按钮---------------------
     $("#btnExit").live("click", function () {
         $("#dlgPmsInfo").window("close");
     });

     $("#btnSave").live("click", function () {
         var QueryNum = $("#txtQueryNum").val();
         var PopupInfoon = $("#txtPopupInfoon").val();
         $.post(handlerUrl, { Action: "ConfigureConfigInfo", QueryNum: QueryNum, PopupInfo: PopupInfoon }, function (data) {
             var resp3 = $.parseJSON(data);
             if (resp3.Status = 0) {
                 Show(resp3.Msg);
             } else {
                 Alert(resp3.Msg);
             }
         });
     });
     function OnUpload() {
         $('#UploadDiv').window(
            {
                title: '上传文件'
            }
            );
         $('#UploadDiv').dialog('open');
     }

     $("#btnClose").live("click", function () {
         $("#UploadDiv").window("close");
     });

     $("#BtnUpload").live("click", function () {
         $.ajaxFileUpload(
                     {
                         url: handlerUrl + '?action=UploadCodeInfoData',
                         secureuri: false,
                         fileElementId: 'uploadify',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             $("#UploadDiv").window("close");
                             $('#grvData').datagrid('reload');
                             show(resp.Msg)
                         }
                     }
                    );
     });
     function OnEdit() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         if (rows.length > 1) {
             Alert("只能选择一行数据");
         }
         window.location.href = "ADTheVoteInfo.aspx?id=" + GetRowsIds(rows).join(',');
     }


     //选择标签对话框
     function ShowEditTag() {
         var rows = $('#grvData').datagrid('getSelections');
         if (!EGCheckIsSelect(rows)) {

             return;
         }
         $('#dlgTag').dialog({ title: '设置标签' });
         $('#dlgTag').dialog('open');

     }

    </script>
</asp:Content>
