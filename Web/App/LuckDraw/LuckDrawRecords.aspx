<%@ Page Title="中奖人员列表" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LuckDrawRecords.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.LuckDrawRecords" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;所有抽奖活动&nbsp;&gt;&nbsp;<span>中奖信息</span>
      <a href="javascript:history.go(-1);" style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
     <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           

            微信昵称:<input id="txtName" class="form-control" style="width: 300px;display:inline-block" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="SearchUser();">查询</a>
            <br />
        </div>
    </div>

    <table id="grvData" fitcolumns="true"></table>

    
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script type="text/javascript">
         var url = "/serv/api/admin/lottery/LotteryUserInfo/list.ashx";
         var lotteryId = '<%=lotteryId%>';
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: url,
                       queryParams: { lottery_id: lotteryId,is_winning:'1' },
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
                                },
                                 {
                                     field: 'number', title: '中奖编号', width: 10, align: 'left'
                                 }
                       ]]
                   }
            );
        });
        //查询参与者
        function SearchUser() {
            var txtNickname = $.trim($('#txtName').val());
            var model = {
                lottery_id: lotteryId,
                keyword: txtNickname,
                is_winning: '1'
            };
            $('#grvData').datagrid({
                method: "Post",
                url: url,
                queryParams: model

            });
        }
    </script>
</asp:Content>
