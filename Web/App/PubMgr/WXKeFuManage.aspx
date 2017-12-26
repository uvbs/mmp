<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXKeFuManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.WXKeFuManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style>
        /*.panel.window{
                top: 1050px!important;
        }*/
        .window{
            top:118px !important;
        }
        .window-shadow{
            top:118px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;微客服&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>客服列表 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加新客服</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑客服信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgKeFu" class="easyui-dialog" closed="true" title="添加新客服" style="width: 370px;
        padding: 15px;line-height:30px;">
        <table width="100%">
            <tr>
                <td>
                    姓名:
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    手机号:
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                   OpenID:
                </td>
                <td>
                  
                    <textarea id="txtWeiXinOpenID" style="width:250px;height:50px;"></textarea>
                </td>
            </tr>
             <tr>
             <td></td>
                <td style="color:Blue;">
                   获取OpenID请在公众号中直接输入 openid                  
                </td>
                
            </tr>

          
        </table>
    </div>

    <div id="kefuInfo" class="easyui-dialog" closed="true" modal="true" title="添加客服(双击选择)" style="width: 450px; height: ">
       <div>
           姓名<input type="text" id="txtTrueNameValue" style="width:300px;height:18px;">
           <a  class="easyui-linkbutton" iconcls="icon-search" id="search">搜索</a>
           
       </div>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
 
   
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var currSelectID = 0;
    var keFuAction = '';
    $(function () {
        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryKuFuList" },
	                height: document.documentElement.clientHeight - 80,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                 { field: 'ck', checkbox: true },
                                { field: 'TrueName', title: '姓名', width: 40, align: 'left' },
                                { field: 'Phone', title: '手机号', width: 40, align: 'left' },
                                { field: 'WeiXinOpenID', title: 'OpenId', width: 40, align: 'left' }
                             ]]
	            }
            );



        $('#dlgKeFu').dialog({
            buttons: [{
                text: '保存',
                handler: function () {
                    try {
                        var dataModel = {
                            Action: keFuAction,
                            AutoID: currSelectID,
                            TrueName: $.trim($('#txtTrueName').val()),
                            Phone: $.trim($('#txtPhone').val()),
                            WeiXinOpenID: $.trim($('#txtWeiXinOpenID').val())

                        }

                        if (dataModel.TrueName == '') {

                            Alert('请输入姓名');
                            return;
                        }

                        if (dataModel.WeiXinOpenID == '') {

                            Alert('请输入微信OpenID');
                            return;
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    Show(resp.Msg);
                                    $('#dlgKeFu').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
                                }


                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }
                }
            }, {
                text: '取消',
                handler: function () {

                    $('#dlgKeFu').dialog('close');
                }
            }]
        });
        $('#grvUserInfo').datagrid(
                  {
                      onDblClickRow: function (rowIndex, rowData) {
                          if (rowData["TrueName"] == "" || rowData["TrueName"] == null) {
                              $.messager.alert('提示:', '缺少姓名');
                              return;
                          }
                          if (rowData["WXOpenId"] == "" || rowData["WXOpenId"] == null) {
                              $.messager.alert('提示:', '缺少OpenID');
                              return;
                          }
                          var dataModel = {
                              Action: "AddKeFu",
                              AutoID: currSelectID,
                              TrueName: rowData["TrueName"],
                              Phone: rowData["Phone"],
                              WeiXinOpenID: rowData["WXOpenId"]
                          }
                          $.ajax({
                              type: 'post',
                              url: "/Handler/App/CationHandler.ashx",
                              data: dataModel,
                              dataType: "json",
                              success: function (resp) {
                                  if (resp.Status == 1) {
                                      Show(resp.Msg);
                                      $('#kefuInfo').dialog('close');
                                      $('#grvData').datagrid('reload');
                                  }
                                  else {
                                      Alert(resp.Msg);
                                  }
                              }
                          });

                      },
                      loadMsg: "正在加载数据",
                      method: "Post",
                      height: 280,
                      pagination: true,
                      striped: true,
                      singleSelect: true,
                      pageSize: 10,
                      rownumbers: true,

                      columns: [[
                                   
                                  { field: 'TrueName', title: '姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                  
                                  { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                      ]]

                  }
              );

        $("#search").click(function () {
            var txtTrueName = $("#txtTrueNameValue").val();
            $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1, KeyWord: txtTrueName } });
        });




    });

    function ShowAdd() {
        $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1 } });
        $('#kefuInfo').dialog('open');


    }
    function Delete() {
        try {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                if (r) {
                    var ids = [];

                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].AutoID);
                    }

                    var dataModel = {
                        Action: 'DeleteKeFu',
                        ids: ids.join(',')
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: dataModel,
                        success: function (result) {
                            Alert(result);
                            $('#grvData').datagrid('reload');
                        }
                    });
                }
            });

        } catch (e) {
            Alert(e);
        }
    }

    function ShowEdit() {
        var rows = $('#grvData').datagrid('getSelections');

        if (!EGCheckIsSelect(rows))
            return;

        if (!EGCheckNoSelectMultiRow(rows))
            return;


        keFuAction = 'EditKeFu';
        currSelectID = rows[0].AutoID;
        $('#txtTrueName').val(rows[0].TrueName);
        $('#txtPhone').val(rows[0].Phone);
        $('#txtWeiXinOpenID').val(rows[0].WeiXinOpenID);
        $('#dlgKeFu').dialog({ title: '编辑客服信息' });
        $('#dlgKeFu').dialog('open');
    }


    </script>
</asp:Content>

