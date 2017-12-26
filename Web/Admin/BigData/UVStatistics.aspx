<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UVStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.BigData.UVStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置： 大数据分析 > 访客统计
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <input type="radio" checked="checked" class="positionTop2" name="rdoTime" id="day" value="day"/>  <label for="day">昨天</label>  
        <input type="radio" class="positionTop2" name="rdoTime" id="week" value="week"/>  <label for="week">近7天</label>  
        <input type="radio" class="positionTop2" name="rdoTime" id="month" value="month"/>  <label for="month">近30天</label>  
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script type="text/javascript">
         var url = "/Serv/Api/Admin/bigdata/uv/list.ashx";
         var time = 'day';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: url,
                queryParams: { times: time },
                height: document.documentElement.clientHeight - 85,
                pagination: true,
                striped: true,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                   
                     {
                         field: 'head_img', title: '头像', width: 10, align: 'center', formatter: function (value) {
                         if (value == '' || value == null)
                             return "";
                         var str = new StringBuilder();
                         str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                         return str.ToString();
                     }
                     },
                    { field: 'nick_name', title: '昵称', width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'true_name', title: '姓名', width:10, align: 'left', formatter: FormatterTitle },
                    { field: 'VisitCount', title: '访问次数', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'ArticleBrowseCount', title: '文章访问', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'ActivityBrowseCount', title: '活动访问', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'ActivitySignUpCount', title: '报名次数', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'OrderCount', title: '下单次数', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'Score', title: '累计获得积分', sortable: true, width: 10, align: 'left', formatter: FormatterTitle },
                    { field: 'OtherBrowseCount', sortable: true, title: '其他访问', width: 10, align: 'left', formatter: FormatterTitle }
                ]],
             
            });
          
        });

        $("input[name=rdoTime]").click(function () {
            var val = $(this).val();
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: url,
                       queryParams: { times: val }
                   });
        })

        

      

       
    </script>
</asp:Content>
