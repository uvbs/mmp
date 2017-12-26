<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SendWxMsgList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.SendWxMsgList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sort{
            height:0!important;
        }
        .centent_r_btm{
            border:0;
        }
        .pageTopBtnBg{
            background-color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
          <%--  <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetTutor" onclick="ActionEvent('setTutor','确定所选用户设置为专家?')">批量设置专家</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnUpdateLawyer" onclick="EditRow()">修改律师资料</a>
            关键字匹配:<input id="txtKeyword" style="width: 200px;"  placeholder="用户名，姓名，邮箱" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>--%>
             <%--批次号:<input id="txtSerialNum" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>--%>

            状态筛选：
            <select id="selectStatus">
                <option value="">全部</option>
                <option value="1">成功</option>
                <option value="0">失败</option>
            </select>

            <a style="float: right;"
                    href="javascript:history.go(-1);" class="easyui-linkbutton"
                    iconcls="icon-back" plain="true">返回</a>
            <div style="clear:both;"></div>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script>
        var handlerUrl = "/Handler/App/CationHandler.ashx",planId = '<%=planId%>';


        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QuerySendWxMsgList", serialNum: planId },
                height: document.documentElement.clientHeight - 100,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    {
                        field: 'UserId', title: '用户ID', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }, {
                        field: 'TrueName', title: '真实姓名', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }, {
                        field: 'WxNikeName', title: '微信昵称', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }
                    , {
                        field: 'Phone', title: '手机', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }
                    ,

                    //{
                    //    field: 'Title', title: '标题', width: 20, align: 'center', formatter: function (value) {
                    //        return value;
                    //    }
                    //},{
                    //    field: 'Msg', title: '内容', width: 20, align: 'center', formatter: function (value) {
                    //        return value;
                    //    }
                    //},{
                    //    field: 'Url', title: '链接', width: 20, align: 'center', formatter: function (value) {
                    //        return value;
                    //    }
                    //},

                    {
                        field: 'Status', title: '状态', width: 20, align: 'center', formatter: function (value) {
                            var result = '<span style="color:green">成功</span>';
                            if (value == 0) {
                                result = '<span style="color:red">失败</span>';
                            }
                            return result;
                        }
                    },

                    {
                        field: 'StatusMsg', title: '状态信息', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    },

                    {
                        field: 'InsertDateStr', title: '发送时间', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }
                    
                ]]
            }
           );

            $('#selectStatus').change(function () {
                Search();
            });

        });

        function Search() {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QuerySendWxMsgList", serialNum: planId,status : $('#selectStatus').val() }
            });
        }
    </script>
</asp:Content>
