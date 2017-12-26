<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LuckDrawUserInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.LuckDrawUserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;所有抽奖活动&nbsp;&gt;&nbsp;<span>参与者信息</span>
      <a href="javascript:history.go(-1);" style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="AddParticipant()">添加参与者</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>

            <br />

            微信昵称:<input id="txtName" class="form-control" style="width: 300px;display:inline-block" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="SearchUser();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true"></table>

    <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="" data-options="iconCls:'icon-tip'" modal="true" style="width: 675px; padding: 15px 15px 0px 15px;">
        姓名:<input type="text"  id="txtKeyWord" style="width: 150px; position: inherit; display: inline-block; padding: 6px;"
            placeholder="姓名" />
        标签名:
            <input type="text" id="txtTagName" readonly="readonly" onclick="ShowTagName();" style="width: 150px; position: inherit; display: inline-block; padding: 6px;" class="" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        <br />
        <br />
        <table id="grvUserData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgTagName" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvTagNameData" fitcolumns="true">
        </table>
    </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var url = "/serv/api/admin/lottery/LotteryUserInfo/list.ashx";
        var userUrl = '/serv/api/admin/member/list.ashx';
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var lotteryId = '<%=lotteryId%>';
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: url,
                       queryParams: { "lottery_id": lotteryId },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                               {
                                   field: 'head_img_url', title: '微信头像', width: 10, align: 'center', formatter: function (value, rowData) {
                                       if (value == '' || value == null)
                                           return "";
                                       var str = new StringBuilder();
                                       str.AppendFormat('<img class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', rowData.head_img_url);
                                       return str.ToString();
                                   }
                               },

                                {
                                    field: 'nick_name', title: '微信昵称', width: 20, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a  title="{0}">{0}</a>', rowData.nick_name);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'time', title: '加入时间', width: 10, align: 'left'
                                }
                       ]]
                   }
            );

            //用户列表

            $('#grvUserData').datagrid(
                  {
                      method: "Post",
                      url: userUrl,
                      queryParams: { mapping_type: '1',isWxnickName:'1'},
                      height: document.documentElement.clientHeight - 112,
                      pagination: true,
                      striped: true,
                      loadFilter: pagerFilter,
                      rownumbers: true,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[
                                  { title: 'ck', width: 5, checkbox: true },
                              {
                                  field: 'WXHeadimgurl', title: '微信头像', width: 10, align: 'center', formatter: function (value, rowData) {
                                      if (value == '' || value == null)
                                          return "";
                                      var str = new StringBuilder();
                                      str.AppendFormat('<img class="imgAlign" src="{0}" title="微信头像" height="50" width="50" />', rowData.WXHeadimgurl);
                                      return str.ToString();
                                  }
                              },

                               {
                                   field: 'WXNickname', title: '微信昵称', width: 20, align: 'left', formatter: function (value, rowData) {
                                       var str = new StringBuilder();
                                       str.AppendFormat('<a  title="{0}">{0}</a>', rowData.WXNickname);
                                       return str.ToString();
                                   }
                               },
                                 {
                                     field: 'TrueName', title: '真实姓名', width: 20, align: 'left', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         str.AppendFormat('<a  title="{0}">{0}</a>', rowData.TrueName);
                                         return str.ToString();
                                     }
                                 },
                               {
                                   field: 'Phone', title: '手机', width: 10, align: 'left'
                               }
                      ]]
                  }
           );


            $('#dlgUserInfo').dialog({
                buttons: [
                    {
                        text: '确定',
                        handler: function () {
                            var rows = $('#grvUserData').datagrid('getSelections');
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            var ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].AutoID);
                            }
                            var model = {
                                lottery_id: lotteryId,
                                uids: ids.join(',')
                            };
                            $.messager.progress({ text: '正在提交...' });
                            $.ajax({
                                type: 'post',
                                url: '/serv/api/admin/lottery/lotteryuserinfo/add.ashx',
                                data: model,
                                dataType: 'json',
                                success: function (resp) {
                                    $.messager.progress('close');
                                    if (resp.status) {
                                        Alert(resp.msg);
                                        $('#dlgUserInfo').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    
                                }
                            });
                        }
                    },
                    {
                        text: '取消',
                        handler: function () {
                            $('#dlgUserInfo').dialog('close');
                        }
                    }
                ]
            });

            //标签搜索
            $('#dlgTagName').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rowsTag = $('#grvTagNameData').datagrid('getSelections');
                        var TagName = [];
                        for (var i = 0; i < rowsTag.length; i++) {
                            TagName.push(rowsTag[i].TagName);

                        }

                        $("#txtTagName").val(TagName.join(','));
                        $('#dlgTagName').dialog('close');
                        Search();

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTagName').dialog('close');
                    }
                }]
            });
            //标签列表
            $('#grvTagNameData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: 'member' },
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


        });


        //添加参与者 dlg
        function AddParticipant() {
            $('#dlgUserInfo').dialog({ title: '用户列表' });
            $('#dlgUserInfo').dialog('open');
            
        }
        //显示设置标签对话框
        function ShowTagName() {
            var rows = $('#grvTagNameData').datagrid('getSelections');
            $('#dlgTagName').dialog({ title: '标签列表' });
            $('#dlgTagName').dialog('open');

        }
        //搜索
        function Search() {
            var model = {
                mapping_type: 1,
                isWxnickName:1,
                KeyWord: $("#txtKeyWord").val(),
                TagName: $("#txtTagName").val(),
            }
            $('#grvUserData').datagrid(
                {
                    method: "Post",
                    url: userUrl,
                    queryParams: model
                });
        }

        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids=[];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].autoid);
            }
            console.log(ids);
            $.messager.confirm('系统提示', '确定要删除选中的参与者吗?', function (o) {
                if (o) {
                    $.messager.progress({ text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: '/serv/api/admin/lottery/lotteryuserinfo/delete.ashx',
                        data: { autoids: ids .join(','),lottery_id:lotteryId},
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status) {
                                Alert(resp.msg);
                                $('#grvData').datagrid('reload');
                            }
                        }
                    });
                }
            });
        }

        //查询参与者
        function SearchUser() {
            var txtNickname = $.trim($('#txtName').val());
            var model = {
                lottery_id: lotteryId,
                keyword: txtNickname
            };
            $('#grvData').datagrid({
                method: "Post",
                url: url,
                queryParams: model

            });
        }
    </script>
</asp:Content>
