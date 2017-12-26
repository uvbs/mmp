<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="QuestionnaireSetMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.QuestionnaireSetMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置:<strong>答题管理</strong>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">新建答题</a>
            <span style="font-size: 12px; font-weight: normal">标题：</span>
            <input type="text" style="width: 200px" id="txtTitle" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <a href="/App/Questionnaire/QuestionnaireMgr.aspx?type=0" iconcls="icon-list" class="easyui-linkbutton" id="btnToQuestions">题库管理</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script type="text/javascript">

         var handlerUrl = "/Serv/API/Admin/QuestionnaireSet/";
         var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                {
                                    field: 'img', title: '缩略图', width: 20, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img  height="50" width="50" src="{0}" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'title', title: '标题', width: 45, align: 'left' },
                                    {
                                        field: 'ippv', title: 'PV/IP', width: 15, align: 'left', formatter: function (value, rowData) {
                                            var str = new StringBuilder();
                                            if (rowData.pv == 0) {
                                                str.AppendFormat('{0}', 0);
                                            } else {
                                                str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&share=2" title="点击查看统计详情">{1}/{2}</a>', rowData.id, rowData.pv, rowData.ip);
                                            }
                                            return str.ToString();
                                        }
                                    },
                                   {
                                       field: 'uv', title: '微信阅读人数', width: 17, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           if (value == 0) {
                                               str.AppendFormat('{0}', 0);
                                           } else {
                                               str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1&share=2" title="点击查看统计详情">{1}</a>', rowData.id, value);
                                           }
                                           return str.ToString();
                                       }
                                   },
                                {
                                    field: 'time', title: '时间', width: 40, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        var sD = new Date(rowData.start_date);
                                        var eD = new Date(rowData.end_date);
                                        var cD = new Date(1900,1,1);
                                        var sStr = "";
                                        if (sD > cD) sStr = sD.format("yyyy-MM-dd hh:mm:ss");
                                        var eStr = "";
                                        if (eD > cD) eStr = eD.format("yyyy-MM-dd hh:mm:ss");
                                        str.AppendFormat('开始时间：{0}', sStr);
                                        str.AppendFormat('<br />');
                                        str.AppendFormat('结束时间：{0}', eStr);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'score', title: '积分', width: 30, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('胜利可得积分数：{0}', rowData.score);
                                        str.AppendFormat('<br />');
                                        str.AppendFormat('可得积分次数：{0}', rowData.score_num);
                                        return str.ToString();
                                    }
                                },
                                { field: 'isrand_question', title: '问题随机', width: 15, align: 'left', formatter: function (value, rowData) {
                                    return value == 1?"是":"否";
                                } },
                                {
                                    field: 'isrand_option', title: '选项随机', width: 15, align: 'left', formatter: function (value, rowData) {
                                        return value == 1 ? "是" : "否";
                                    }
                                },
                                {
                                    field: 'answer_count', title: '回答数', width: 15, align: 'left', formatter: FormatterTitle
                                },
                                { field: 'op', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看二维码" href="javascript:void()" onclick="ShowQRcode(\'{0}\')">二维码</a>', rowData.id);
                                    str.AppendFormat('&nbsp;');
                                    str.AppendFormat('<a title="点击修改" href="QuestionnaireSetEdit.aspx?id={0}">编辑</a>',rowData.id);
                                    return str.ToString();
                                } 
                                }
	                ]],
	                onLoadSuccess: function () {
	                    //加载完数据关闭等待的div   
	                    $('#grvData').datagrid('loaded');
	                }
	            }
            );

         $('#grvData').datagrid('getPager').pagination({
             onSelectPage: function (pPageIndex, pPageSize) {
                 //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                 loadData();
             }
         });
         //初始加载
         loadData();

         $("#btnSearch").click(function () {
             var title = $("#txtTitle").val();
             $('#grvData').datagrid({ url: handlerUrl + "List.ashx", queryParams: { title: title } });
         });



     });

     function ShowAdd() {
         window.location.href = "QuestionnaireSetEdit.aspx?id=0";
     }
     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;
             $.messager.confirm("系统提示", "确认删除选中答题?<br/>选中答题相关数据将会全部删除!", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].id);
                     }

                     var dataModel = {
                         AutoIDs: ids.join(',')
                     }
                     $.ajax({
                         type: 'post',
                         url: handlerUrl + "Delete.ashx",
                         data: dataModel,
                         success: function (result) {
                             if (result.status) {
                                 $.messager.show({
                                     title: '系统提示',
                                     msg: result.msg
                                 });
                                 $('#grvData').datagrid('reload');
                             }
                             else {
                                 $.messager.alert("系统提示", result.msg);
                             }
                         }
                     });
                 }
             });

         } catch (e) {
             Alert(e);
         }
     }

     function ShowQRcode(id) {
         var linkurl = "http://" + domain + "/customize/dati/index.aspx?id=" + id;
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=' + encodeURIComponent(linkurl));
         $('#dlgSHowQRCode').dialog('open');
         $("#alinkurl").html(linkurl);
         $("#alinkurl").attr("href", linkurl);
     }

     function loadData() {
         var gridOpts = $('#grvData').datagrid('options');
         $('#grvData').datagrid('loading');//打开等待div   
         var title = $("#txtTitle").val();
         $.post(
             handlerUrl + "list.ashx",
             { page: gridOpts.pageNumber, rows: gridOpts.pageSize, title: title },
             function (data, status) {
                 if (data.status && data.result.list) {
                     $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                 }
             });
     }
    </script>
</asp:Content>
