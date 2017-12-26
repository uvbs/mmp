<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="FilterWordMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.FilterWordMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
        .style1
        {
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>关键字过滤 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加关键字</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <br />
                <span style="font-size: 12px; font-weight: normal">关键字：</span>
                <input type="text" style="width: 200px" id="txtWordS" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">查询</a>
            <br />

        </div>
    </div>
    <table id="grvData" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="AutoID" width="10">
                            编号
                        </th>
                        <th field="Word" width="50">
                            关键字
                        </th>
                        <th field="FilterType" width="20" formatter="formarttype">
                            类型
                        </th>
                       
                    </tr>
                </thead>
</table>

    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 350px;
       padding: 15px;line-height:30px;">
        <table width="100%">
            <tr>
                <td>
                   关键字:
                </td>
                <td>
                    <input id="txtWord" type="text" style="width: 200px;" />
                </td>
            </tr>
          <tr>
                <td>
                    类型:
                </td>
                <td>
                    <select id="ddlFilterType">
                        <option value="0">文章</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
 
   
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
  <script type="text/javascript">
      var handlerUrl = "/Handler/App/CationHandler.ashx";
      var currSelectID = 0;
      var currAction = '';
      var grid;
      $(function () {

          //-----------------加载gridview
          grid = jQuery("#grvData").datagrid({
              method: "Post",
              url: handlerUrl,
              height: document.documentElement.clientHeight - 160,
              toolbar: '#divToolbar',
              pageSize: 50,
              fitCloumns: true,
              pagination: true,
              rownumbers: true,
              singleSelect: true,
              queryParams: { Action: "QueryFilterWord" }
          });
          //------------加载gridview

          $('#dlgInput').dialog({
              buttons: [{
                  text: '保存',
                  handler: function () {
                      var dataModel = GetDlgModel();
                      if (dataModel.Word == '') {
                          alert('请输入关键字', 3);
                          return;
                      }

                      $.ajax({
                          type: 'post',
                          url: handlerUrl,
                          data: dataModel,
                          dataType: "json",
                          success: function (resp) {
                              if (resp.Status == 1) {
                                  alert("操作成功", 2);
                                  $('#dlgInput').dialog('close');
                                  $('#grvData').datagrid('reload');
                              }
                              else {
                                  alert(resp.Msg, 2);
                              }
                          }
                      });

                  }
              }, {
                  text: '取消',
                  handler: function () {

                      $('#dlgInput').dialog('close');
                  }
              }]
          });



      });


      function ShowAdd() {
          $('#dlgInput').dialog('open');
          currAction = "AddFilterWord";
      }

      function ShowEdit() {
          var rows = grid.datagrid('getSelections');
          if (!EGCheckNoSelectMultiRow(rows)) {
              return;
          }

          $('#dlgInput').dialog('open');
          currAction = "EditFilterWord";
          //加载编辑数据
          currSelectID = rows[0].AutoID;
          $("#txtWord").val(rows[0].Word);


      }

      //批量删除
      function Delete() {
          var rows = grid.datagrid('getSelections');
          if (!EGCheckIsSelect(rows)) {
              return;
          }
          $.messager.confirm('系统提示', '确定删除选中?', function (o) {
              if (o) {
                  var ids = new Array();
                  for (var i = 0; i < rows.length; i++) {
                      ids.push(rows[i].AutoID);
                  }
                  $.ajax({
                      type: "Post",
                      url: handlerUrl,
                      data: { Action: "DeleteFilterWord", ids: ids.join(',') },
                      success: function (result) {
                          alert('已删除数据' + result + '条');
                          grid.datagrid('reload');
                      }
                  });
              }
          });
      }

      //获取对话框数据实体
      function GetDlgModel() {
          var model =
            {
                "AutoID": currSelectID,
                "Word": $.trim($(txtWord).val()),
                "FilterType": $("#ddlFilterType").val(),
                "Action": currAction
            }
          return model;
      }

      //检查输入框输入
      function CheckDlgInput(model) {
          if (model['Word'] == '') {
              $(txtWord).focus();
              return false;
          }
          return true;
      }



      function Search() {
          $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryFilterWord",Word:$(txtWordS).val()}
	            });
	        }

	        function formarttype(value) {
	            switch (value) {
	                case 0:
	                    return "文章";
	                    break;
	                default:

	            }
            
            }

  </script>
</asp:Content>

