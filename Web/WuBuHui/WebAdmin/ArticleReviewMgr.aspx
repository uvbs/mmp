<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ArticleReviewMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.ArticleReviewMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>文章评论 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
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
                        <th field="ReplyContent" width="50">
                            评论内容
                        </th>
                        <th field="InsertDate" width="20" formatter="FormatDate">
                            时间
                        </th>
                    </tr>
                </thead>
</table>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
  <script type="text/javascript">
      var handlerUrl = "/Handler/App/ReviewHandler.ashx";
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
              queryParams: { Action: "QueryArticleReview" }
          });
          //------------加载gridview

         



      });

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
                      ids.push(rows[i].AutoId);
                  }
                  $.ajax({
                      type: "Post",
                      url: handlerUrl,
                      data: { Action: "DeleteArticleReview", ids: ids.join(',') },
                      success: function (result) {
                          alert('已删除数据' + result + '条');
                          grid.datagrid('reload');
                      }
                  });
              }
          });
      }

      function Search() {
          $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryArticleReview",KeyWord:$(txtWordS).val()}
	            });
      }



  </script>
</asp:Content>