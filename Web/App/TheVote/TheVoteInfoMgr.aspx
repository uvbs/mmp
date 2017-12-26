<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="TheVoteInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TheVote.TheVoteInfoMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;所有投票&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>投票系统</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="ADTheVoteInfo.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">
                添加</a> 
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" onclick="OnEdit()"
                    plain="true">编辑</a>
             <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" onclick="Delete()"
                    plain="true">删除</a>
                        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-remove" onclick="Clear()"
                    plain="true">清空投票数据</a>

            <br />
            投票名称:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXTheVoteInfoHandler.ashx";
     var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTheVoteInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'VoteName', title: '名称', width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">{1}</a>', rowData.AutoId, value);
                                    return str.ToString();
                                }
                                },
                                { field: 'VoteSelect', title: '投票类型', width: 10, align: 'left', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    if (value == 1) return "单选";
                                    if (value == 2) return "多选";
                                }
                                },
                                { field: 'VoteNumbers', title: '总票数', width: 10, align: 'left' },
                                { field: 'PNumber', title: '投票人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (value == 0) {
                                        str.AppendFormat("{0}",value);
                                    } else {
                                        str.AppendFormat('<a class="listClickNum" title="查看投票人信息" href="TheVoteUserInfo.aspx?id={0}" >{1}</a>', rowData.AutoId, value);
                                    }
                                    return str.ToString();
                                }
                                },
                                 {
                                     field: 'ippv', title: 'PV/IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (rowData.PV == 0) {
                                             str.AppendFormat('{0}', 0);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&share=3" title="点击查看统计详情">{1}/{2}</a>', rowData.AutoId, rowData.PV, rowData.IP);
                                         }
                                         return str.ToString();
                                     }
                                 },
                                 {
                                     field: 'UV', title: '微信阅读人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (value == 0) {
                                             str.AppendFormat('{0}', 0);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1&share=3" title="点击查看统计详情">{1}</a>', rowData.AutoId, value);
                                         }
                                         return str.ToString();
                                     }
                                 },
                                { field: 'TheVoteGUID', title: '唯一标示', width: 18, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('$TOUPIAO@{0}TOUPIAO$', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'InsetDate', title: '创建时间', width: 15, align: 'left', formatter: FormatDate },
                                { field: 'TheVoteOverDate', title: '结束时间', width: 15, align: 'left', formatter: FormatDate },
                                { field: 'Operate', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a target="_blank" title="查看投票人信息" href="TheVoteInfoChart.aspx?id={0}" >查看投票结果图</a>', rowData.AutoId);
                                    str.AppendFormat('&nbsp;<a target="_blank" title="实时图表" href="/App/TheVote/Histogram/index.html#/chart/{0}" >实时图表</a>', rowData.AutoId);
                                    return str.ToString();
                                }
                                },
                             ]]
	            }
            );
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


     //清除投票数据
     function Clear() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定清空投票数据?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "ClearTheVoteInfoData", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             Alert("操作成功");
                             $('#grvData').datagrid('reload');
                            
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
	                queryParams: { Action: "GetTheVoteInfos", VoteName: $("#txtName").val() }
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
             return;
         }
         window.location.href = "ADTheVoteInfo.aspx?id=" + GetRowsIds(rows).join(',');
     }
     function ShowQRcode(id) {
         //dlgSHowQRCode
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/TheVote/wap/WxTheVoteInfo.aspx?autoid=' + id);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/TheVote/wap/WxTheVoteInfo.aspx?autoid=" + id;
         $("#alinkurl").html(linkurl).attr("href", linkurl);
     }
     


    </script>
</asp:Content>

