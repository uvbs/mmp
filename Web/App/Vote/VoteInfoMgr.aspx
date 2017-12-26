<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VoteInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteInfoMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>投票活动管理 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="VoteInfoCompile.aspx?action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true">新增投票活动</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
            <a href="OrderMgr.aspx" class="easyui-linkbutton" iconcls="icon-edit" plain="true" >购票记录</a>
            <a href="VoteLogInfoMgr.aspx" class="easyui-linkbutton" iconcls="icon-edit" plain="true" >投票活动记录</a>
            <br />
                <span style="font-size: 12px; font-weight: normal">投票活动名称：</span>
                <input type="text" style="width: 200px" id="txtVoteName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <br />


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

       <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
                <br />
         <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
       
    
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },

                                //{ field: 'VoteImage', title: '图片', width: 10, align: 'center', formatter: function (value) {
                                //    if (value == '' || value == null)
                                //        return "";
                                //    var str = new StringBuilder();
                                //    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="80" width="80" />', value);
                                //    return str.ToString();
                                //}
                                //},
                                { field: 'AutoID', title: '活动ID', width: 5, align: 'left' },
                                {
                                    field: 'VoteName', title: '投票活动名称', width: 20, align: 'left', formatter: function (value, rowData) {

                                        var str = new StringBuilder();
                                        str.AppendFormat('<div>');
                                        str.AppendFormat('<img alt="" class="imgAlign floatL" src="{0}" title="缩略图" height="80" width="80" />', rowData.VoteImage);
                                        str.AppendFormat('<span class="floatL mTop10 mLeft10">{0}</span>', rowData.VoteName);
                                        str.AppendFormat('<div class="clear"></div>');
                                        str.AppendFormat('</div>');
                                        return str.ToString();

                                    }
                                },
                                { field: 'StopDate', title: '截止时间', width: 10, align: 'left' },
                                { field: 'VoteStatus', title: '状态', width: 10, align: 'left', formatter: function (value, rowData) {
                                    if (value == "0") {
                                        return "<font color='red'>停止投票<font/>";
                                    } else if (value == "1") {
                                        return "<font color='green'>列表模式<font/>";
                                    } else if (value == "2") {
                                        return "<font color='red'>展示模式<font/>";
                                    } else if (value == "3") {
                                        return "<font color='green'>PK模式<font/>";
                                    }

                                }
                                },
                                //{ field: 'CreateUserID', title: '创建用户', width: 10, align: 'left' },
//                                { field: 'IsFree', title: '是否免费', width: 10, align: 'left', formatter: function (value, rowData) {
//                                    if (value == "0") {
//                                        return "<font color='red'>收费<font/>";
//                                    }
//                                    else {
//                                        return "<font color='green'>免费<font/>";
//                                    }

//                                }
//                                },
                             { field: 'Op', title: '操作', width: 25, align: 'center', formatter: function (value, rowData) {
                                 var str = new StringBuilder();
                                 str.AppendFormat('<a href="VoteInfoCompile.aspx?action=edit&aid={0}" >[修改]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="VoteObjectInfoMgr.aspx?vid={0}" >[参与者]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="VotePKModeMgr.aspx?vid={0}" >[PK列表]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="javascript:void(0)" onclick="ShowQRcode({0})">[二维码]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="VoteRechargeMgr.aspx?vid={0}" >[充值设置]</a>', rowData['AutoID']);
                                 return str.ToString();
                             }
                             }

                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var VoteName = $("#txtVoteName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryVoteInfo", VoteName: VoteName} });
         });

     })


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
                         Action: 'DeleteVoteInfo',
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

     function ShowQRcode(aid) {

         //$('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Cation/Wap/Vote/Vote.aspx?vid=' + aid);
         //$('#dlgSHowQRCode').dialog('open');
         //var linkurl = "http://" + domain + "/App/Cation/Wap/Vote/Vote.aspx?vid=" + aid;
         //$("#alinkurl").html(linkurl);
         //$("#alinkurl").attr("href", linkurl);

         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Cation/Wap/Vote/Comm/Index.aspx?vid=' + aid);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/Cation/Wap/Vote/Comm/Index.aspx?vid=" + aid;
         $("#alinkurl").html(linkurl);
         $("#alinkurl").attr("href", linkurl);
     }
 </script>
</asp:Content>
