<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ArticleCategoryManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ArticleCategoryManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .style1 {
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=currShowName %> </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">添加<%=currShowName %></a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑<%=currShowName %></a>
            <%
                if (!isHide)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <%
                }     
            %>

            <br />
            <%--<span style="font-size: 12px; font-weight: normal"><%=currShowName %>名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">查询</a>
            <br />--%>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="AutoID" width="15">编号
                </th>
                <th field="CategoryName" width="100">名称
                </th>
                <%if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.ListFields) && nCategoryTypeConfig.ListFields.Contains("ImgSrc"))
                  { %>
                <th field="ImgSrc" width="100" formatter="FormatterImage">图片
                </th>
                <%} %>
                <%if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.ListFields) && nCategoryTypeConfig.ListFields.Contains("Summary"))
                  { %>
                <th field="Summary" width="100">描述
                </th>
                <%} %>
                <%if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.ListFields) && nCategoryTypeConfig.ListFields.Contains("Link"))
                  { %>
                <th field="op" width="100" formatter="formartlink">链接
                </th>
                <%} %>
                <%if (!string.IsNullOrWhiteSpace(hasKeyValue) && hasKeyValue=="1")
                  { %>
                <th field="ac" width="30" formatter="FormartToSetKeyValue">操作
                </th>
                <%} %>
            </tr>
        </thead>
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加<%=currShowName %>" style="width: 500px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr class="trPreCate">
                <td style="width: 90px;">上级:
                </td>
                <td>
                    <span id="sp_menu"></span>
                </td>
            </tr>
            <tr>
                <td>名称:
                </td>
                <td>
                    <input id="txtCategoryName" type="text" style="width: 200px;" maxlength="20" />
                </td>
                <% if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.EditFields) && nCategoryTypeConfig.EditFields.Contains("ImgSrc"))
                   {  %>
            </tr>
            <tr>
                <% }
                   else
                   {%>
            </tr>
            <tr style="display: none;">
                <% }%>
                <td>图片:
                </td>
                <td>
                    <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" />
                    <input type="file" id="txtThumbnailsPath" name="file1" />
                    <input id="txtImgSrc" type="text" style="width: 200px;" />
                    <br />
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a>
                    <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                    <br />
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                </td>
                <%if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.ListFields) && nCategoryTypeConfig.ListFields.Contains("Summary"))
                  { %>
            </tr>
            <tr>
                <% }
                  else
                  {%>
            </tr>
            <tr style="display: none;">
                <% }%>
                <td>描述:
                </td>
                <td>
                    <input id="txtSummary" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td>排序:
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 50px;" value="0" />提示：从小到大排序
                </td>
            </tr>
            <tr class="trSysType">
                <td>系统类型:
                </td>
                <td>
                    <select id="selectSystype">
                        <option value="0">Normal</option>
                        <option value="1">System</option>
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
        var CategoryType = "<%=CategoryType%>";
        var cateRootId = "<%=cateRootId%>";
        var isNoPreSelect = "<%=isNoPreSelect%>";
        var currUserType = "<%=currUser.UserType%>";
        var selectMaxDepth = "<%=selectMaxDepth%>";
        var websiteowner = "<%=this.Request["websiteowner"]%>"

        var grid;
        $(function () {

            if (isNoPreSelect == 1) {
                $('.trPreCate').hide();
            }
            if (currUserType != 1) {
                $('.trSysType').hide();
            }
            $('#txtThumbnailsPath').hide();
            $('#txtThumbnailsPath').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath.src = resp.ExStr;
                                $("#txtImgSrc").val(resp.ExStr);
                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });

            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight - 160,
                toolbar: '#divToolbar',
                fitCloumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryArticleCategory", CategoryType: CategoryType, cateRootId: cateRootId, websiteowner: websiteowner }
            });
            //------------加载gridview

            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {

                        var dataModel = GetDlgModel();
                        if (dataModel.CategoryName == '') {
                            alert('请输入<%=currShowName %>名称', 3);
                            return;
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    alert(resp.Msg, 1);
                                    $('#dlgInput').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    LoadCategorySelectList();
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



            //加载<%=currShowName %>
            LoadCategorySelectList();

        });
        function FormatterImage(value, rowDate) {
            var str = new StringBuilder();
            str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', rowDate["ImgSrc"]);
            return str.ToString();
        }
        function FormartToSetKeyValue(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="/Admin/KeyVauleData/KeyVauleDataManage.aspx?isAutoKey=1&type={1}&preKey={2}&websiteowner={3}&redirect={4}" title="{0}">{0}</a>', '设置', CategoryType, rowData.AutoID, websiteowner, encodeURIComponent(document.location.href));
            return str.ToString();
        }
        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $("#txtImgSrc").val("/img/hb/hb" + randInt + ".jpg");
        }

        function ShowAdd() {
            ClearWinDataByTag('input', dlgInput);
            $(txtSort).val("0");
            $('#txtImgSrc').val("/img/hb/hb1.jpg");
            imgThumbnailsPath.src = "/img/hb/hb1.jpg";
            //加载菜单
            // LoadCategorySelectList();
            $('#dlgPmsInfo').window({ title: '添加<%=currShowName %>' });
          $('#dlgInput').dialog('open');
          currAction = "AddArticleCategory";
      }

      function ShowEdit() {
          var rows = grid.datagrid('getSelections');
          if (!EGCheckNoSelectMultiRow(rows)) {
              return;
          }
          ClearWinDataByTag('input', dlgInput);

          $('#dlgPmsInfo').window({ title: '编辑<%=currShowName %>' });
          $('#dlgInput').dialog('open');
          currAction = "EditArticleCategory";
          //加载编辑数据
          currSelectID = rows[0].AutoID;
          $('#ddlPreMenu').val(rows[0].PreID);
          $('#selectSystype').val(rows[0].SysType);
          $(txtCategoryName).val($.trim(rows[0].CategoryName).replace('└', ''));
          $(txtSort).val(rows[0].Sort);
          $('#txtImgSrc').val(rows[0].ImgSrc);
          imgThumbnailsPath.src = rows[0].ImgSrc;
          $('#txtSummary').val(rows[0].Summary);
      }
      //批量删除
      function Delete() {
          var rows = grid.datagrid('getSelections');
          if (!EGCheckIsSelect(rows)) {
              return;
          }
          $.messager.confirm('系统提示', '确定删除选中<%=currShowName %>？', function (o) {
              if (o) {
                  var ids = new Array();
                  for (var i = 0; i < rows.length; i++) {
                      ids.push(rows[i].AutoID);
                  }
                  $.ajax({
                      type: "Post",
                      url: handlerUrl,
                      data: { Action: "DeleteArticleCategory", ids: ids.join(','), websiteowner: websiteowner },
                      success: function (result) {
                          //$.messager.show({
                          //    title: '系统提示',
                          //    msg: '已删除数据' + result + '条'
                          //});
                          alert('已删除数据' + result + '条');
                          grid.datagrid('reload');
                          LoadCategorySelectList();
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
                "CategoryName": $.trim($(txtCategoryName).val()),
                "PreID": $('#ddlPreMenu').val() == 0 ? cateRootId : $('#ddlPreMenu').val(),
                "CategoryType": CategoryType,
                "Summary": $.trim($('#txtSummary').val()),
                "ImgSrc": $.trim($('#txtImgSrc').val()),
                "Sort": $(txtSort).val(),
                "Action": currAction,
                SysType: $('#selectSystype').val(),
                "websiteowner": websiteowner
            }
          return model;
      }

      //检查输入框输入
      function CheckDlgInput(model) {
          if (model['CategoryName'] == '') {
              $(txtCategoryName).val("");
              $(txtCategoryName).focus();
              return false;
          }

          return true;
      }

      //加载<%=currShowName %>选择列表
        function LoadCategorySelectList() {
            $.post(handlerUrl, { Action: "GetCategorySelectList", CategoryType: CategoryType, cateRootId: cateRootId, maxDepth: selectMaxDepth }, function (data) {
                $("#sp_menu").html(data);
            });
        }

        function Search() {
            $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { Action: "QueryArticleCategory", cateRootId: cateRootId, CategoryType: CategoryType, CategoryName: $("#txtName").val() }
                  });
        }
        function formartlink(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a target="_blank" href="http://<%=Request.Url.Host %>/Web/list.aspx?cateid={0}" >http://<%=Request.Url.Host %>/Web/list.aspx?cateid={0}</a>', rowData.AutoID);
            return str.ToString();
        }

    </script>
</asp:Content>

