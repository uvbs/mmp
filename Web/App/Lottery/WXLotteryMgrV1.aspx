<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXLotteryMgrV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryMgrV1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&gt;&nbsp;<span>所有<%=moduleName %>活动</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <%
                if (lotteryType == "luckydraw")
                {
            %>
            <a href="/App/LuckDraw/LuckDrawCompile.aspx?backUrl=/App/Lottery/WXLotteryMgrV1.aspx&lotteryType=<%=lotteryType %>" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">新建<%=moduleName %>活动</a>
            <%
            }
            else
            {
            %>
            <a href="/App/Lottery/WXLotteryCompileV1.aspx?backUrl=/App/Lottery/WXLotteryMgrV1.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">新建<%=moduleName %>活动</a>
            <%
            }   
            %>


            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>

            <%
                if (isHideResetLottery == 0)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="Reset()">重置抽奖</a>
            <%
                }
                 
            %>

            <%
                if (isHideDefaultLottery == 0)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="SetWinData()">设置默认中奖名单</a>
            <%
                }     
            %>

            <%
                if (isHideRecordsRealTime == 0)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowLotteryRecordsRealTime()">显示实时中奖名单</a>
            <%
                }
            %>


            <%
                if (isHidePersionCount == 1)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="l-btn-text icon-list" plain="true" onclick="AddParticipant()">添加用户抽奖</a>
            <%
                }    
            %>

            <%
                if (isHideWinning == 1)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="l-btn-text icon-list" plain="true" onclick="GoWinning()">去抽奖</a>
            <%
                }     
            %>

            <%
                if (isHideUrl == 1)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="l-btn-text icon-list" plain="true" onclick="GetLotteryUrl()">获取链接</a>
            <%
                }    
            %>

            <br />

            活动名称:<input id="txtName" style="width: 200px;" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
    <div id="dlgWinData" class="easyui-dialog" closed="true" title="" style="width: 500px; padding: 15px; line-height: 30px;">
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAddWinData()">添加</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEditWinData()">编辑</a>
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="DeleteWinData()">删除</a>

        <table id="grvWinData" fitcolumns="true"></table>
    </div>
    <div id="dlgAddEditWinData" class="easyui-dialog" closed="true" title="" style="width: 300px; padding: 15px; line-height: 30px;">
        用户名:
         <input type="text" id="txtUserId" />
        <br />
        奖品:&nbsp;&nbsp;
         <select id="ddlawards">
         </select>


    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     var currAction = "AddWinData";
     var lotteryType = '<%=lotteryType%>';
        var domain = '<%=Request.Url.Authority%>';
     var LotteryID = 0;
     var AutoID = 0;
     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLotteryV1", "LotteryType": lotteryType },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                <%
        if (isHideImg == 0)
        {
                                    %>
                                         {
                                             field: 'ThumbnailsPath', title: '缩略图', width: 10, align: 'center', formatter: function (value, rowData) {
                                                 if (value == '' || value == null)
                                                     return "";
                                                 var str = new StringBuilder();
                                                 str.AppendFormat('<img class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', rowData.ShareImg);
                                                 return str.ToString();
                                             }
                                         },
                                    <%
                                }
                                %>

                                {
                                    field: 'LotteryName', title: '<%=moduleName%>活动名称', width: 20, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a  title="{0}">{0}</a>', rowData.LotteryName);
                                        return str.ToString();


                                    }
                                },
                                {
                                    field: 'Status', title: '状态', width: 10, align: 'left', formatter: function (value) {
                                        if (value == 1) {
                                            return '<span style="color:green">进行中</span>';
                                        }
                                        else {
                                            return '<span style="color:red">已停止</span>';
                                        }
                                    }
                                },
                                <%
        if (lotteryType.ToLower() == "luckydraw")
        {
                                    %>
                                    { field: 'WinnerCount', title: '参与人数', width: 20, align: 'left' },
                                    <%
                                }
                                else
                                {
                                     %>
                                     { field: 'PersionCount', title: '参与人数', width: 20, align: 'left' },
                                    <%
                                }
                                %>
                               

                                <%
        if (isHidePV == 0)
        {
                                    %>
                                          {
                                              field: 'ippv', title: 'PV/IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                                  var str = new StringBuilder();
                                                  if (rowData.PV == 0) {
                                                      str.AppendFormat('{0}', 0);
                                                  } else {
                                                      str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.LotteryID, rowData.PV, rowData.IP);
                                                  }
                                                  return str.ToString();
                                              }
                                          },
                                    <%
                                }
                                %>

                               

                                 <%
        if (isHideUV == 0)
        {
                                        %>
                                             {
                                                 field: 'UV', title: '微信阅读人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                                     var str = new StringBuilder();
                                                     if (value == 0) {
                                                         str.AppendFormat('{0}', 0);
                                                     } else {
                                                         str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>', rowData.LotteryID, value);
                                                     }
                                                     return str.ToString();
                                                 }
                                             },
                                        <%
                                }
                                %>

                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate },
                                {
                                    field: 'op', title: '操作', width: 25, align: 'center', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        <%
        if (lotteryType == "luckydraw")
        {
                                            %>
                                        str.AppendFormat('<a href="/App/LuckDraw/LuckDrawEdit.aspx?lotteryId={0}" title="点击编辑" >编辑|&nbsp;<a href="/App/LuckDraw/LuckDrawRecords.aspx?lotteryId={0}" title="点击查看中奖名单">查看中奖名单</a>', rowData.LotteryID);
                                            <%
                                        }
                                        else
                                        {
                                            %>
                                        str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">二维码</a>|&nbsp;<a href="/App/Lottery/WXLotteryEditV1.aspx?id={0}&backUrl=/App/Lottery/WXLotteryMgrV1.aspx" title="点击编辑" >编辑</a>|&nbsp;<a href="/App/Lottery/WXLotteryRecordsV1.aspx?id={0}" title="点击查看中奖名单">查看中奖名单</a>', rowData.LotteryID);
                                            <%
                                        }
                                        %>

                                        return str.ToString();
                                    }
                                }
	                ]]
	            }
            );


         $('#dlgAddEditWinData').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     try {
                         var dataModel = {
                             Action: currAction,
                             AutoID: AutoID,
                             LotteryID: LotteryID,
                             UserID: $.trim($('#txtUserId').val()),
                             AwardId: $("#ddlawards").val()
                         }
                         if (dataModel.UserID == '') {
                             Alert('请输入用户名');
                             return;
                         }

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.Status == 1) {
                                     Show("操作成功");
                                     $('#dlgAddEditWinData').dialog('close');
                                     $('#grvWinData').datagrid('reload');

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
                     $('#dlgAddEditWinData').dialog('close');
                 }
             }]
         });

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
                                        data: { Action: "DeleteWXLotteryV1", ids: GetRowsIds(rows).join(','), LotteryType: "scratch" },
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



                        //重置抽奖
                        function Reset() {

                            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            $.messager.confirm("系统提示", "确定重置所选抽奖?抽奖记录及中奖记录将会被清除", function (o) {
                                if (o) {
                                    $.ajax({
                                        type: "Post",
                                        url: handlerUrl,
                                        data: { Action: "ResetWXLotteryV1", ids: GetRowsIds(rows).join(',') },
                                        dataType: "json",
                                        success: function (resp) {
                                            Alert(resp.Msg);
                                        }

                                    });
                                }
                            });
                        }

                        function ResetLottery() {
                            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            $.messager.confirm("系统提示", "确定重置所选抽奖?参与者信息及中奖记录将会被清除", function (o) {
                                if (o) {
                                    $.ajax({
                                        type: "Post",
                                        url: '/serv/api/admin/lottery/LotteryUserInfo/SetLotteryInfo.ashx',
                                        data: { lottery_id: rows[0].LotteryID },
                                        dataType: "json",
                                        success: function (resp) {
                                            if (resp.status) {
                                                alert('重置成功');
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
                                ids.push(rows[i].LotteryID
                                    );
                            }
                            return ids;
                        }
                        //获取选中行ID集合
                        function GetRowsIdsWinData(rows) {
                            var ids = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].AutoID
                                    );
                            }
                            return ids;
                        }




                        function Search() {

                            $('#grvData').datagrid(
                                   {
                                       method: "Post",
                                       url: handlerUrl,
                                       queryParams: { Action: "QueryWXLotteryV1", LotteryType: lotteryType, LotteryName: $("#txtName").val() }
                                   });
                        }



                        function ShowQRcode(id) {
                            //dlgSHowQRCode
                            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Lottery/wap/ScratchV1.aspx?id=' + id);
                            $('#dlgSHowQRCode').dialog('open');
                            var linkurl = "http://" + domain + "/App/Lottery/wap/ScratchV1.aspx?id=" + id;
                            $("#alinkurl").html(linkurl).attr("href", linkurl);
                        }

                        function SetWinData() {


                            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }

                            if (!EGCheckNoSelectMultiRow(rows)) {
                                return;
                            }

                            LotteryID = rows[0].LotteryID;
                            var LotteryName = rows[0].LotteryName;
                            LoadAwardSelect();
                            $('#grvWinData').datagrid(
                                   {
                                       method: "Post",
                                       url: handlerUrl,
                                       queryParams: { Action: "QueryWXLotteryWinDataV1", LotteryID: LotteryID },
                                       height: 300,
                                       pagination: true,
                                       striped: true,
                                       pageSize: 10,
                                       rownumbers: true,
                                       rowStyler: function () { return 'height:25px'; },
                                       columns: [[
                                                   { title: 'ck', width: 5, checkbox: true },
                                                   {
                                                       field: 'LotteryName', title: '活动名称', width: 20, align: 'left', formatter: function (value, rowData) {
                                                           return LotteryName;
                                                       }
                                                   },
                                                   { field: 'UserId', title: '用户名', width: 20, align: 'left' },
                                                   { field: 'WXAwardName', title: '奖品名称', width: 20, align: 'left' }


                                       ]]
                                   }
                               );



                            $('#dlgWinData').dialog({ title: '中奖名单' });
                            $('#dlgWinData').dialog('open');

                        }


                        function ShowAddWinData() {

                            currAction = "AddWinData";
                            $('#dlgAddEditWinData').dialog({ title: '添加中奖名单' });
                            $('#dlgAddEditWinData').dialog('open');


                        }

                        function ShowEditWinData() {
                            var rows = $("#grvWinData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            if (!EGCheckNoSelectMultiRow(rows)) {
                                return;
                            }
                            AutoID = rows[0].AutoID;
                            LotteryID = rows[0].LotteryId;
                            currAction = "EditWinData";
                            $(txtUserId).val(rows[0].UserId);
                            $(ddlawards).val(rows[0].WXAwardsId);
                            $('#dlgAddEditWinData').dialog({ title: '编辑中奖名单' });
                            $('#dlgAddEditWinData').dialog('open');

                        }

                        function LoadAwardSelect() {
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: { Action: "QueryAwardsSelect", LotteryID: LotteryID },
                                dataType: "json",
                                success: function (resp) {
                                    var str = new StringBuilder();
                                    for (var i = 0; i < resp.ExObj.length; i++) {
                                        str.AppendFormat('<option value="{0}">{1}</option>', resp.ExObj[i].AutoID, resp.ExObj[i].PrizeName);

                                    }
                                    var html = str.ToString();

                                    $("#ddlawards").html(html);
                                }
                            });




                        }

                        //删除
                        function DeleteWinData() {
                            var rows = $("#grvWinData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                                if (o) {
                                    $.ajax({
                                        type: "Post",
                                        url: handlerUrl,
                                        data: { Action: "DeleteWinData", ids: GetRowsIdsWinData(rows).join(',') },
                                        dataType: "json",
                                        success: function (resp) {
                                            if (resp.Status == 1) {
                                                Show("删除成功");
                                                $("#grvWinData").datagrid('reload');

                                            }
                                            else {
                                                Alert(resp.Msg);
                                            }
                                        }

                                    });
                                }
                            });


                        }
                        //显示实时中奖名单
                        function ShowLotteryRecordsRealTime() {
                            var rows = $("#grvData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            if (!EGCheckNoSelectMultiRow(rows)) {
                                return;
                            }
                            window.open("LotteryRecordsRealTime.aspx?id=" + rows[0].LotteryID);
                        }
                        //添加参与者
                        function AddParticipant() {
                            var rows = $("#grvData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            window.location.href = '/app/LuckDraw/LuckDrawUserInfo.aspx?lotteryId=' + rows[0].LotteryID;
                        }
                        //去抽奖
                        function GoWinning() {
                            var rows = $("#grvData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }
                            window.open("/app/LuckDraw/LuckDrawPage.aspx?lotteryId=" + rows[0].LotteryID);
                        }

                        function GetLotteryUrl() {
                            var rows = $("#grvData").datagrid('getSelections'); //获取选中的行
                            if (!EGCheckIsSelect(rows)) {
                                return;
                            }

                            var url = "http://"+domain+"/App/LuckDraw/wap/detail.aspx?lotteryId=" + rows[0].LotteryID;
                            layer.open({
                                title: '抽奖活动链接',
                                content: url
                            });

                        }

    </script>
</asp:Content>
