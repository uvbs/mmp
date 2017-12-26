<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXLotteryConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.admin.WXLotteryConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=Request.Url.Host %>';
        var currActionWXAwards = "";//奖品当前操作 添加或 编辑 
        var currSelectIDWXAwards = 0;//奖品 当前选中
        var currSelectLotteryId = 0;
        var currActionWXWinningData = ""; //中奖设置 当前操作 添加或编辑
        var currSelectIDWXWinningData = 0; //中奖设置当前选中
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLottery" },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ThumbnailsPath', title: '缩略图', width: 10, align: 'center', formatter: function (value, rowData) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img onclick="ShowQRcode(\'{0}\')" class="imgAlign" src="{1}" title="缩略图" height="50" width="50" />', rowData.AutoID, rowData.ThumbnailsPath);
                                    return str.ToString();
                                }
                                },
                                { field: 'LotteryName', title: '刮奖活动名称', width: 40, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:" onclick="ShowQRcode(\'{0}\')" title="{1}">{1}</a>', rowData.AutoID, rowData.LotteryName);
                                    return str.ToString();


                                } 
                                },
                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'op', title: '操作', width: 25, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:" onclick="ShowWXAwards(\'{0}\')" title="奖品设置" >奖品设置</a>&nbsp;<a onclick="ShowWinningData(\'{0}\')">中奖设置</a>', rowData.AutoID);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );


            $('#grvWXAwardsData').datagrid(
	            {
	                method: "Post",
	                height: 275,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'PrizeName', title: '奖品名称', width: 80, align: 'left' },
                                { field: 'PrizeCount', title: '奖品数量', width: 20, align: 'left' }
                             ]]
	            }
            );

	        $('#grvWinningData').datagrid(
	            {
	                method: "Post",
	                height: 275,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'WinningIndex', title: '位置', width: 20, align: 'left' },
                                { field: 'WXAwardName', title: '奖品', width: 75, align: 'left' }
                             ]]
	            }
            );
	            

	       $('#dlgWXAwardsOperate').dialog({
	                buttons: [{
	                    text: '保存',
	                    handler: function () {
	                        try {
	                            var dataModel = {
	                                Action: currActionWXAwards,
	                                AutoID: currSelectIDWXAwards,
                                    LotteryId:currSelectLotteryId,
	                                PrizeName: $.trim($('#txtPrizeName').val()),
	                                PrizeCount: $.trim($('#txtPrizeCount').val())
	                            }

	                            if (dataModel.PrizeName == '') {

	                                Alert('请输入奖品名称');
	                                return;
	                            }
	                            if (dataModel.PrizeCount == '') {

	                                Alert('请输入奖品数量');
	                                return;
	                            }
                                
                                if (parseInt(dataModel.PrizeCount) <= 0) {
	                                Alert('奖品数量需大于0');
	                                return;

	                            }


	                            $.ajax({
	                                type: 'post',
	                                url: handlerUrl,
	                                data: dataModel,
	                                success: function (result) {
	                                    var resp = $.parseJSON(result);
	                                    if (resp.Status == 1) {
	                                        Show(resp.Msg);
	                                        $('#dlgWXAwardsOperate').dialog('close');
	                                        $('#grvWXAwardsData').datagrid('reload');
	                                        LoadAward();
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

	                        $('#dlgWXAwardsOperate').dialog('close');
	                    }
	                }]
	            });

	       $('#dlgWinningDataOperate').dialog({
	                buttons: [{
	                    text: '保存',
	                    handler: function () {
	                        try {
	                            var dataModel = {
	                                Action: currActionWXWinningData,
	                                AutoID: currSelectIDWXWinningData,
	                                LotteryId: currSelectLotteryId,
	                                WinningIndex: $.trim($('#txtWinningIndex').val()),
	                                WXAwardsId: $.trim($('#ddlWxAward').val())
	                            }

	                            if (dataModel.WinningIndex == '') {

	                                Alert('请输入中奖位置');
	                                return;
	                            }
	                            if (dataModel.WXAwardsId == '') {

	                                Alert('请选择奖品');
	                                return;
	                            }


	                            
	                            $.ajax({
	                                type: 'post',
	                                url: handlerUrl,
	                                data: dataModel,
	                                success: function (result) {
	                                    var resp = $.parseJSON(result);
	                                    if (resp.Status == 1) {
	                                        Show(resp.Msg);
	                                        $('#dlgWinningDataOperate').dialog('close');
	                                        $('#grvWinningData').datagrid('reload');
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

	                        $('#dlgWinningDataOperate').dialog('close');
	                    }
	                }]
	            });


        });


        //获取选中行ID集合
        function GetRowsIds(rows) {
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
	                queryParams: { Action: "QueryWXLottery", LotteryName: $("#txtName").val() }
	            });
        }


        function ShowQRcode(id) {
            //dlgSHowQRCode
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Lottery/wap/Scratch.aspx?id=' + id);
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/Lottery/wap/Scratch.aspx?id=" + id;
            //$("#alinkurl").html(linkurl).attr("href", linkurl);
        }

        //显示奖品设置
        function ShowWXAwards(id) {
            currSelectLotteryId = id;
            $('#grvWXAwardsData').datagrid({ url: handlerUrl, queryParams: { Action: 'QueryWXAwards', LotteryId:id} });
            $('#dlgWXAwards').dialog('open');
        }


        //显示中奖设置
        function ShowWinningData(id) {
            currSelectLotteryId = id;
            $('#grvWinningData').datagrid({ url: handlerUrl, queryParams: { Action: 'QueryWinningData', LotteryId: id} });
            $('#dlgWinningData').dialog('open');
        }


        function ShowAddWXAwards() {
            currActionWXAwards = 'AddWXAwards';
            $('#dlgWXAwardsOperate').dialog({ title: '添加奖品' });
            $('#dlgWXAwardsOperate').dialog('open');
            $("#dlgWXAwardsOperate input").val("");


        }
        function DeleteWXAwards() {
            try {

                var rows = $('#grvWXAwardsData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoID);
                        }

                        var dataModel = {
                            Action: 'DeleteWXAwards',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                var resp = $.parseJSON(result);
                                Alert(resp.Msg);
                                $('#grvWXAwardsData').datagrid('reload');
                                LoadAward();
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowEditWXAwards() {
            var rows = $('#grvWXAwardsData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            
            currActionWXAwards = 'EditWXAwards';
            currSelectIDWXAwards = rows[0].AutoID;
            $('#txtPrizeName').val(rows[0].PrizeName);
            $('#txtPrizeCount').val(rows[0].PrizeCount);
            $('#dlgWXAwardsOperate').dialog({ title: '编辑奖品设置' });
            $('#dlgWXAwardsOperate').dialog('open');
        }

        //

        function ShowAddWXWinningData() {
            currActionWXWinningData = 'AddWinningData';
            $('#dlgWinningDataOperate').dialog({ title: '添加中奖设置' });
            $('#dlgWinningDataOperate').dialog('open');
            $("#dlgWinningDataOperate input").val("");
            LoadAward();

        }
        function DeleteWXWinningData() {
            try {

                var rows = $('#grvWinningData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoID);
                        }

                        var dataModel = {
                            Action: 'DeleteWinningData',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                var resp = $.parseJSON(result);
                                Alert(resp.Msg);
                                $('#grvWinningData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowEditWXWinningData() {

            LoadAward();
            var rows = $('#grvWinningData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;


            currActionWXWinningData = 'EditWinningData';
            currSelectIDWXWinningData = rows[0].AutoID;
            $('#txtWinningIndex').val(rows[0].WinningIndex);
            $('#ddlWxAward').val(rows[0].WXAwardsId);
            $('#dlgWinningDataOperate').dialog({ title: '编辑中奖设置' });
            $('#dlgWinningDataOperate').dialog('open');
        }
        function LoadAward() {

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "GetWxAwardListByLotteryId",LotteryId:currSelectLotteryId},
                success: function (result) {
                    var resp = $.parseJSON(result);
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        str.AppendFormat('<option value="{0}">{1}</option>', resp.ExObj[i].AutoID, resp.ExObj[i].PrizeName);

                    }
                    $(ddlWxAward).html(str.ToString());


                }
            });
        
        
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>刮奖设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">          
            
                        活动名称:<input id="txtName" style="width:200px;" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; 
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
        <div id="dlgWXAwards" class="easyui-dialog" closed="true" title="奖品设置" style="width: 450px;
        height: 350px;">
         <div  class="pageTopBtnBg" style="padding: 5px; height: auto;">
          <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAddWXAwards();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEditWXAwards();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="DeleteWXAwards()">删除</a>


         </div>
        <table id="grvWXAwardsData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgWXAwardsOperate" class="easyui-dialog" closed="true" title="" style="width: 330px;
       padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    奖品名称:
                </td>
                <td>
                    <input id="txtPrizeName" type="text" style="width: 200px;" />
                </td>
            </tr>
           <tr>
                <td>
                    奖品数量:
                </td>
                <td>
                    <input id="txtPrizeCount" type="text" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'') "  />
                </td>
            </tr>
            
        </table>
    </div>

       <div id="dlgWinningData" class="easyui-dialog" closed="true" title="中奖设置" style="width: 450px;">
         <div  class="pageTopBtnBg" style="padding: 5px; height: auto;">
          <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAddWXWinningData();" id="A1">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEditWXWinningData();" id="A2">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="DeleteWXWinningData()">删除</a>


         </div>
        <table id="grvWinningData" fitcolumns="true">
        </table>
    </div>
     <div id="dlgWinningDataOperate" class="easyui-dialog" closed="true" title="" style="width: 330px;padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    中奖位置:
                </td>
                <td>
                    <input id="txtWinningIndex" type="text" style="width: 200px;" onkeyup="value=value.replace(/[^\d]/g,'') "  />
                </td>
            </tr>
           <tr>
                <td>
                  奖品:
                </td>
                <td>
                  
                 <select id="ddlWxAward" style="width:100px;">
                 
                 
                 </select>
                </td>
            </tr>
            
        </table>
    </div>

</asp:Content>
