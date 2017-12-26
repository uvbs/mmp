<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivityManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ActivityManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;活动管理&nbsp;&gt;&nbsp;<span>所有活动</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:top.addTab('新建活动','/App/Cation/ActivityCompile.aspx?Action=add')" class="easyui-linkbutton" iconcls="icon-usergroup3"
                plain="true" id="btnAdd">新建活动</a>
            <%if (!isHide)
              { %>
             <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">删除活动</a>
            <%}%>
              
             
           
            <a href="javascript:;"
                        class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnEditSignUpTable">报名字段设置</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                id="btnCreateSignInQRCode">生成签到二维码</a> 

                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                id="btnCreateSignInQRCodeWeixin">生成微信专属签到二维码</a> 
            <a href="javascript:;" style="display:none;" class="easyui-linkbutton"
                    iconcls="icon-list" plain="true" id="btnSignInData">查看签到数据</a> 分类:
           
            <%=sbCategory1.ToString()%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="BatchSetArticleCategory()">设置分类</a> 
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ActivityForward()">加入转发列表</a>
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="BatchSetAccessLevel()">访问等级</a>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;分类：
            
            <%=sbCategory.ToString()%>
           
            主题:<input id="txtArticleName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
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
      <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px;
        padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    访问等级:
                </td>
                <td>
                    <input id="txtAccessLevel" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currDlgAction = '';
     var currSelectAcvityID = 0;
     var pageIndex = 1;
     var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     $(function () {
         if (window.sessionStorage.getItem('keyWord_Activity') != null) {
             $("#txtArticleName").val(window.sessionStorage.getItem('keyWord_Activity'));
         }
         if (window.sessionStorage.getItem('category_Activity') != null) {
             $("#ddlcategory").val(window.sessionStorage.getItem('category_Activity'));
         }
         if (window.sessionStorage.getItem('pageNumber_Activity') != null) {
             pageIndex = parseInt(window.sessionStorage.getItem('pageNumber_Activity'));
         }
         GetData(pageIndex);

         $('#btnEditSignUpTable').click(function () {
             var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
             if (!EGCheckNoSelectMultiRow(rows)) {
                 return;
             }
//             if (rows[0].IsSignUpJubit == 0) {
//                 Alert('该活动未设置报名！');
//                 return;
//             }
             //window.location.href = 'javascript:top.addTab(\'报名字段设置-' + rows[0].ActivityName + '\',\'/App/Cation/ActivitySignUpTableManage.aspx?ActivityID=' + rows[0].SignUpActivityID + '\')';
             window.location.href = '/App/Cation/ActivitySignUpTableManage.aspx?ActivityID=' + rows[0].SignUpActivityID ;
             //window.open('/Activity/ActivityTable.aspx?ActivityID=' + rows[0].SignUpActivityID);
         });

         //btnCreateSignInQRCode
         $('#btnCreateSignInQRCode').click(function () {
             var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
             if (!EGCheckNoSelectMultiRow(rows)) {
                 return;
             }

             //弹出二维码框
             $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Cation/Wap/SignIn.aspx?id=' + rows[0].JuActivityID);

             //                var linkurl = "http://" + domain + "/App/Cation/Wap/SignIn.aspx?id=" + rows[0].JuActivityID;
             //                $("#alinkurl").html(linkurl);
             //                $("#alinkurl").attr("href", linkurl);
             $("#alinkurl").html("");
             $('#dlgSHowQRCode').dialog('open');

         });

         //btnCreateSignInQRCode//微信二维码
         $('#btnCreateSignInQRCodeWeixin').click(function () {

             var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
             if (!EGCheckNoSelectMultiRow(rows)) {
                 return;
             }
             $('#imgQrcode').attr('src', "");
             $("#alinkurl").html("");
             $.ajax({
                 type: 'post',
                 url: "/serv/api/common/WeixinQrcode.ashx",
                 data: { type: "ActivitySignIn", id: rows[0].JuActivityID, code: "activitysignin_" + rows[0].JuActivityID },
                 dataType: "json",
                 success: function (resp) {
                     if (resp.status) {

                         //弹出二维码框
                         $('#imgQrcode').attr('src', resp.result.qrcode_url);
                         $("#alinkurl").html("");

                     }
                     else {
                        alert(resp.msg);
}

                 }
             });


             $('#dlgSHowQRCode').dialog('open');


         });



         $('#btnSignInData').click(function () {
             var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
             if (!EGCheckNoSelectMultiRow(rows)) {
                 return;
             }
             window.location.href = '/App/Cation/WXSignInDataManage.aspx?jid=' + rows[0].JuActivityID;

             //window.location.href = 'javascript:top.addTab(\'查看签到数据-' + rows[0].ActivityName + '\',\'/App/Cation/WXSignInDataManage.aspx?jid=' + rows[0].JuActivityID + '\')';

         });
         //批量设置访问级别对话框
         $('#dlgInfo').dialog({
             buttons: [{
                 text: '确定',
                 handler: function () {
                     var rows = $('#grvData').datagrid('getSelections');
                     var JuActivityIDs = [];
                     for (var i = 0; i < rows.length; i++) {
                         JuActivityIDs.push(rows[i].JuActivityID);
                     }

                     var dataModel = {
                         Action: "UpdateAccessLevel",
                         AccessLevel: $.trim($('#txtAccessLevel').val()),
                         JuActivityID: JuActivityIDs.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status > 0) {
                                 $('#dlgInfo').dialog('close');
                                 $('#grvData').datagrid('reload');
                             }
                             else {

                             }
                             Alert(resp.Msg);

                         }
                     });

                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgInfo').dialog('close');
                 }
             }]
         });


         //设置页码
         if (window.sessionStorage.getItem('pageNumber_Activity') != null) {
             var $getPager = $("#grvData").datagrid('getPager');
             var $pagination = $($getPager).pagination("options");
             if ($pagination != undefined) {
                 $pagination.pageNumber = parseInt(window.sessionStorage.getItem('pageNumber_Activity'));
             }
         }
     });


     function GetData(page) {
         $('#grvData').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "Activity", ActivityName: $("#txtArticleName").val(), CategoryId: $("#ddlcategory").val() },
                   height: document.documentElement.clientHeight - 112,
                   pagination: true,
                   striped: true,
                   pageNumber: page,
                   rownumbers: true,
                   singleSelect: false,
                   rowStyler: function () { return 'height:25px'; },

                   columns: [[
                               { title: 'ck', width: 5, checkbox: true },
                               { field: 'JuActivityID', title: '编号', width: 20, align: 'left', formatter: FormatterTitle },
                               {
                                   field: 'ThumbnailsPath', title: '缩略图', width: 20, align: 'center', formatter: function (value) {
                                       if (value == '' || value == null)
                                           return "";
                                       var str = new StringBuilder();
                                       str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                       return str.ToString();
                                   }
                               },
                               {
                                   field: 'ActivityName', title: '主题', width: 110, align: 'left', formatter: function (value, rowData) {
                                       var str = new StringBuilder();
                                       str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\',\'{2}\')" title="{0}">{0}</a>', value, rowData.JuActivityIDHex, rowData.JuActivityID);
                                       return str.ToString();
                                   }
                               },
                               { field: 'CategoryName', title: '分类', width: 30, align: 'left', formatter: FormatterTitle },
                                { field: 'AccessLevel', title: '访问等级', width: 20, align: 'left', formatter: FormatterTitle },
                               { field: 'ActivityAddress', title: '地点', width: 30, align: 'left', formatter: FormatterTitle },
                               { field: 'ActivityStartDate', title: '活动时间', width: 25, align: 'left', formatter: FormatDate },
                               {
                                   field: 'IsHide', title: '状态', width: 20, align: 'center', formatter: function (value) {
                                       if (value == 1) {
                                           return '<span style="color:red">已停止</span>';
                                       }
                                       else if (value == -1) {
                                           return '<span style="color:green">待开始</span>';
                                       }
                                       else {
                                           return '<span style="color:green">进行中</span>';
                                       }
                                   }
                               },

                                <%
                                 if (IsShowActivityPv)
                                     {
                                        %>
                                            {
                                                field: 'IP', title: 'IP/PV', width: 30, align: 'center', formatter: function (value, rowData) {
                                                    var str = new StringBuilder();
                                                    if (rowData.PV == 0) {
                                                        str.AppendFormat("{0}", rowData.PV);
                                                    } else {
                                                        str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                                        //str.AppendFormat('<a class="listClickNum" href="javascript:top.addTab(\'IP/PV统计详情-{3}\',\'/App/Monitor/EventDetails.aspx?aid={0}\')" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV, rowData.ActivityName);
                                                    }
                                                    return str.ToString();
                                                }
                                            },

                                        <%
                                     }
                               %>

                                  {
                                      field: 'UV', title: '微信阅读人数', width: 30, align: 'center', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          if (rowData.UV == 0) {
                                              str.AppendFormat("{0}", rowData.UV);
                                          } else {
                                              str.AppendFormat('<a class="listClickNum"  href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>', rowData.JuActivityID, rowData.UV);
                                              //str.AppendFormat('<a class="listClickNum" href="javascript:top.addTab(\'微信阅读-{2}\',\'/App/Monitor/EventDetails.aspx?aid={0}&uv=1\')" title="点击查看统计详情">{1}</a>', rowData.JuActivityID, rowData.UV, rowData.ActivityName);
                                          }
                                          return str.ToString();
                                      }
                                  },


                                 //{ field: 'ShareTotalCount', title: '分享统计', width: 40, align: 'center',
                                 //    formatter: function (value, rowData) {
                                 //        var str = new StringBuilder();
                                 //        str.AppendFormat('<a href="/App/Cation/ArticleStatistics.aspx?articleId={0}"  title="点击查看统计详情" >{1}</a>', rowData.JuActivityID, value);
                                 //        return str.ToString();
                                 //    }
                                 //},
                                 {
                                     field: 'SignUpTotalCount', title: '报名人数', width: 20, align: 'center', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (value == 0) {
                                             str.AppendFormat("{0}", value);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Cation/ActivitySignUpDataManage.aspx?ActivityID={0}"  title="点击查看报名详情">{1}</a>', rowData.SignUpActivityID, value);
                                             //str.AppendFormat('<a class="listClickNum" href="javascript:top.addTab(\'报名列表-{2}\',\'/App/Cation/ActivitySignUpDataManage.aspx?ActivityID={0}\')"  title="点击查看报名详情">{1}</a>', rowData.SignUpActivityID, value, rowData.ActivityName);
                                         }
                                         return str.ToString();
                                     }
                                 },
                                 {
                                     field: 'ShareMonitorId', title: '传播路径', width: 30, align: 'center', formatter: function (value, rowData) {
                                         
                                         var str = new StringBuilder();
                                         if (value == 0) {
                                             str.Append("<span>-</span>");
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/Admin/ShareMonitor/ShareTree/tree.html?mid={0}"  title="点击查看分享路径">查看</a>', value);
                                         }
                                         
                                         return str.ToString();
                                     }
                                 },
                             {
                                 field: 'Sort', title: '排序', width: 25, align: 'left', formatter: function (value, rowData) {
                                     var newvalue = "";
                                     if (value != null) {
                                         newvalue = value;
                                     }
                                     var str = new StringBuilder();
                                     str.AppendFormat('<input type="text" value="{0}" id="txtArticle{1}" style="width:20px;" maxlength="5" > <a title="点击保存排序号"  onclick="UpdateSortIndex({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.JuActivityID);
                                     return str.ToString();
                                 }
                             },
                                {
                                    field: 'IsSignUpJubit', title: '操作', width: 20, align: 'center', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        //str.AppendFormat('<a href="/App/Cation/ActivityCompile.aspx?Action=edit&aid={0}"><img alt="编辑该活动" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该活动" /></a>', rowData['JuActivityID'], rowData.ActivityName);
                                        str.AppendFormat('<a href="javascript:top.addTab(\'编辑活动-{1}\',\'/App/Cation/ActivityCompile.aspx?Action=edit&aid={0}\')"><img alt="编辑该活动" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该活动" /></a>', rowData['JuActivityID'], rowData.ActivityName);
                                        return str.ToString();
                                    }
                                }
                   ]],
                   onLoadSuccess: function (data) {
                       var pager = $('#grvData').datagrid('options').pageNumber;
                       window.sessionStorage.setItem("pageNumber_Activity", pager);
                   }
               }
            );
     }

     function Search() {
         $('#grvData').datagrid('options').pageNumber = '1';
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "activity", CategoryId: $("#ddlcategory").val(), ActivityName: $("#txtArticleName").val() }
	            });
         //把关键字存入sessionStorage keyWord
         var keyWord = $("#txtArticleName").val();
         var category = $("#ddlcategory").val();
         window.sessionStorage.setItem("keyWord_Activity", keyWord);
         window.sessionStorage.setItem("category_Activity", category);
     }

     //更新排序号
     function UpdateSortIndex(articleid) {

         var sortindex = $("#txtArticle" + articleid).val();
         if ($.trim(sortindex) == "") {
             $("#txtArticle" + articleid).focus();
             return false;
         }



         var re = /^[1-9]+[0-9]*]*$/;
         if (!re.test(sortindex)) {
             alert("请输入正整数");
             $("#txtArticle" + articleid).val("");
             $("#txtArticle" + articleid).focus();
             return false;
         }


         $.ajax({
             type: 'post',
             url: handlerUrl,
             data: { Action: "UpdateArticleSortIndex", ArticleID: articleid, SortIndex: sortindex },
             dataType: "json",
             success: function (resp) {
                     if (resp.Status == 1) {
                         //Show(resp.Msg);
                         $('#grvData').datagrid("reload");
                     }
                     else {
                         Alert(resp.Msg);
                     }
                 

             }
         });

        
     }

     function ShowQRcode(aid, juid) {
         var linkurl = "http://" + domain + "/" + aid + "/" + "details.chtml";
         if (domain == "forbes.comeoncloud.net") {
             linkurl = "http://forbes.comeoncloud.net/customize/forbes/index.html#/activity_single/" + juid;
         }
         $.ajax({
             type: 'post',
             url: "/Handler/QCode.ashx",
             data: { code: linkurl },
             success: function (result) {
                 $("#imgQrcode").attr("src", result);
             }
         });
         $('#dlgSHowQRCode').dialog('open');
         $("#alinkurl").html(linkurl);
         $("#alinkurl").attr("href", linkurl);
     }

     //删除
     function Delete() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中活动?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteJuActivity", ids: GetRowsIds(rows).join(','),type:"activity" },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
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
             ids.push(rows[i].JuActivityID
                 );
         }
         return ids;
     }

     
     //加入转发列表
     function ActivityForward() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) return;
         if (confirm("是否加入转发列表")) {
             $.ajax({
                 type: "post",
                 url: handlerUrl,
                 data: { Action: "ActivityForwardInfo", ids: GetRowsIds(rows).join(',') },
                 dataType: "json",
                 success: function (resp) {
                     if (resp.Status == 0) {
                         Alert(resp.Msg)
                     } else {
                         Alert(resp.Msg)
                     }
                 }
             });
         }
     }

     function BatchSetArticleCategory() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         if ($("#ddlsetcategory").val() == "") {

             Alert("请选择要设置的分类");
             return;
         }
         var categoryname = $("#ddlsetcategory").find("option:selected").text().replace('└', '');
         //
         $.messager.confirm("系统提示", "确定将所选活动的分类修改为 " + categoryname, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "BatchSetArticleCategory", ids: GetRowsIds(rows).join(','), CategroyId: $("#ddlsetcategory").val() },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                             Alert(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });

         //
     }

     //设置访问级别
     function BatchSetAccessLevel()
     {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $('#dlgInfo').dialog({ title: '设置访问级别' });
         $('#dlgInfo').dialog('open');
     }


           
 </script>
</asp:Content>

