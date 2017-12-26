<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom.Order.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=currShowName %>预订
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="30" checkbox="true"></th>
                <th field="order_id" width="40" formatter="FormatterTitle">订单号</th>
                <th field="true_name" width="70" formatter="FormatterOrderUser">下单人</th>
                <%--<th field="phone" width="55" formatter="FormatterTitle">手机</th>--%>
                <%--<th field="date" width="60" formatter="FormatterOrderDate">预订日期</th>--%>
                <th field="order_details" width="80" formatter="FormatterOrderDetails">预订时间</th>
                <th field="title" width="150" formatter="FormatterOrder">订单详情</th>
                <%--<th field="added_details" width="60" formatter="FormatterAddedDetails">增值服务</th>--%>
                <th field="total_amount" width="100" formatter="FormatterAmount">金额</th>
                <th field="pay_type" width="50" formatter="FormatterPayType">支付方式</th>
                <th field="order_status" width="40" formatter="FormatterTitle">订单状态</th>
                <th field="order_time" width="65" formatter="FormatterTitle">下单时间</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="设置订单状态" onclick="SetOrderStatus()" id="btnSetOrderStatus" runat="server">设置订单状态</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ExportData()">导出订单数据</a>
            <br />
            订单状态:<select id="ddlSearchOrderStatus" style="width: 200px;">
                <option value="" selected="selected"></option>
                <option value="待付款">待付款</option>
                <option value="待审核">待审核</option>
                <option value="预约成功">预约成功</option>
                <option value="预约失败">预约失败</option>
            </select>
            订单号:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td style="width: 80px;">订单状态:
                </td>
                <td>
                    <select id="ddlOrderStatus" style="width: 200px;">
                        <option value="" selected="selected"></option>
                        <option value="待付款">待付款</option>
                        <option value="待审核">待审核</option>
                        <option value="预约成功">预约成功</option>
                        <option value="预约失败">预约失败</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>

    <div id="digData" class="easyui-dialog" closed="true" title="" style="width: 500px; padding: 20px;">
        <table width="100%;padding:20px;">
            <tr>
                <td>
                    <label>时间段</label>
                </td>
                <td>
                    <input class="easyui-datetimebox"  id="txtStartTime" />
                    <label>到</label>
                    <input class="easyui-datetimebox"  id="txtStopTime" />
                </td>
            </tr>
            <tr>
                <td><label>订单状态：</label></td>
                <td>
                    <input type="checkbox" name="ckStatus" class="positionTop2" value="待付款" id="ckDfk" /><label for="ckDfk">待付款</label>
                    <input type="checkbox" name="ckStatus" class="positionTop2" value="待审核" id="ckDfh"/><label for="ckDfh">待审核</label>
                    <input type="checkbox" name="ckStatus" class="positionTop2" value="已取消" id="ckCancel"/><label for="ckCancel">已取消</label>
                    <input type="checkbox" name="ckStatus" class="positionTop2" value="预约成功" id="ckThtk"/><label for="ckThtk">预约成功</label>
                    <input type="checkbox" name="ckStatus" class="positionTop2" value="预约失败" id="ckYfh"/><label for="ckYfh">预约失败</label>
                </td>
            </tr>
            <tr>
                <td><label>会员</label></td>
                <td>
                    <input type="text" size="60" id="txtNumber" onclick="ShowUserInfo()" readonly="readonly"/>
                    <input type="hidden" id="txtUserName1"/>
                </td>
            </tr>
            <tr>
                <td><label>会员标签</label></td>
                <td>
                     <input type="text" size="60" id="txtTag" onclick="ShowTagName()"  readonly="readonly"/>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgTagName" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvTagNameData" fitcolumns="true">
        </table>
    </div>      <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
          <div class="mBottom10">
           关键字<input type="text" id="txtTrueNameValue" style="width:200px;height:18px;">
           <a  class="easyui-linkbutton" iconcls="icon-search" id="search">搜索</a>
       </div>
        <table id="grvUserInfoData" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/booking/order/';
        var handlerOldUrl = "/serv/api/admin/mall/order.ashx";
        var type = '<% =categoryType%>';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                queryParams: { type: type },
                height: document.documentElement.clientHeight - 70,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                loadFilter: pagerFilter,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loading');
                },
            });

            //批量设置访问级别对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var IDs = [];
                        for (var i = 0; i < rows.length; i++) {
                            IDs.push(rows[i].order_id);
                        }
                        var order_status = $.trim($('#ddlOrderStatus').val());
                        if (order_status == "") {
                            $.messager.alert("提示", "请选择订单状态");
                            return false;
                        }
                        var dataModel = {
                            ids: IDs.join(','),
                            order_status: order_status
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "UpdateOrderStatus.ashx",
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $.messager.show({ title: '系统提示', msg: resp.msg })
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }
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

            //时间设置只读
            $(".datebox :text").attr("readonly", "readonly");

            //按条件导出订单
            $('#digData').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $("#grvData").datagrid('getSelections');//获取选中的行
                        var fromData = $('#txtStartTime').datetimebox('getValue');
                        var toData = $('#txtStopTime').datetimebox('getValue');
                        var autoIds = $("#txtUserName1").val();
                        var tagNames = $("#txtTag").val();
                        var status = [];
                        $("input[name=ckStatus]:checked").each(function () {
                            status.push($(this).val());
                        })

                        var orderType = type == "MeetingRoom" ? 5 : 6;
                        if (rows.length > 0) {
                            window.open('/Serv/API/Admin/Mall/ExportOrder.ashx?order_type=' + orderType + '&oids=' + GetRowsIds(rows).join(',') + '&from_date=' + fromData + '&to_date=' + toData + '&user_aids=' + autoIds + '&user_tags=' + tagNames + '&status=' + status + '&is_yuyue=' + type + '');
                        } else {
                            window.open('/Serv/API/Admin/Mall/ExportOrder.ashx?order_type=' + orderType + '&from_date=' + fromData + '&to_date=' + toData + '&user_aids=' + autoIds + '&user_tags=' + tagNames + '&status=' + status + '&is_yuyue=' + type + '');
                        }

                        $('#digData').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#digData').dialog('close');
                    }
                }]
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

                        $("#txtTag").val(TagName.join(','));
                        $('#dlgTagName').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTagName').dialog('close');
                    }
                }]
            });            $('#dlgUserInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvUserInfoData').datagrid('getSelections');
                        var TrueName = [];
                        var autoIds = [];
                        for (var i = 0; i < rows.length; i++) {
                            TrueName.push(rows[i].TrueName);
                            autoIds.push(rows[i].AutoID);
                        }
                        $("#txtNumber").val(TrueName.join(','));
                        $("#txtUserName1").val(autoIds.join(','));
                        $('#dlgUserInfo').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUserInfo').dialog('close');
                    }
                }]
            });            //标签列表
            $('#grvTagNameData').datagrid(
	            {
	                method: "Post",
	                url: "/Handler/App/CationHandler.ashx",
	                queryParams: { Action: "QueryMemberTag", TagType: 'member' },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TagName', title: '标签名称', width: 20, align: 'left' }


	                ]]
	            }
            );            $("#search").click(function () {
                var txtTrueName = $("#txtTrueNameValue").val();
                $('#grvUserInfoData').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1, KeyWord: txtTrueName } });
            });            $('#grvUserInfoData').datagrid(
	            {
	                method: "Post",
	                url: "/Handler/App/CationHandler.ashx",
	                queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1 },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { field: 'AutoID', title: '编号', hidden: "true", width: 0, align: 'left', formatter: FormatterTitle },
                              { field: 'TrueName', title: '真实姓名', width: 25, align: 'left', formatter: FormatterTitle },
                              { field: 'UserID', title: '用户名', width: 50, align: 'left', formatter: FormatterTitle },
                              { field: 'WXNickname', title: '昵称', width: 25, align: 'left', formatter: FormatterTitle }


	                ]]
	            }
            );


        });
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?product_id={0}&type={1}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['product_id'], type);
            return str.ToString();
        }
        function FormatterOrderUser(value, rowData) {
            var str = new StringBuilder();
            if ($.trim(rowData.true_name) != "") {
                str.AppendFormat('姓名：{0}', rowData.true_name);
                str.AppendFormat('<br />');
                str.AppendFormat('手机：{0}', rowData.phone);
            }
            return str.ToString();
        }
        function FormatterOrder(value, rowData) {
            var str = new StringBuilder();
            if (rowData.price > 0) {
                str.AppendFormat('{0} <span style="color:red;">{1}</span>{2}*<span style="color:red;">{3}</span><br />', rowData.product_name, rowData.price, rowData.unit, rowData.order_details.length);
            }
            else {
                str.AppendFormat('{0} 免费<br />', rowData.product_name);
            }
            if (rowData.added_details.length > 0) {
                for (var i = 0; i < rowData.added_details.length; i++) {
                    if (i != 0) str.AppendFormat('<br />');
                    if (rowData.added_details[i].price > 0) {
                        str.AppendFormat('{0} <span style="color:red;">{1}</span>{2}*<span style="color:red;">{3}</span>', rowData.added_details[i].product_name, rowData.added_details[i].price, rowData.added_details[i].unit, rowData.added_details[i].count);
                    }
                    else {
                        str.AppendFormat('{0} 免费<br />', rowData.added_details[i].product_name);
                    }
                }
            }
            return str.ToString();
        }
        function FormatterOrderDate(value, rowData) {
            var str = new StringBuilder();
            var nr = rowData.order_details[0];
            str.AppendFormat('{0}', new Date(nr.start_date).format("yyyy-MM-dd"));
            return str.ToString();
        }
        function FormatterOrderDetails(value) {
            var str = new StringBuilder();
            for (var i = 0; i < value.length; i++) {
                if (i != 0) str.AppendFormat('<br />');
                str.AppendFormat('{0}-{1}', new Date(value[i].start_date).format("yyyy-MM-dd hh:mm"), new Date(value[i].end_date).format("hh:mm"));
            }
            return str.ToString();
        }
        function FormatterAmount(value, rowData) {
            var str = new StringBuilder();

            if (rowData.use_score > 0) {
                str.AppendFormat('积分抵扣：<span style="color:red;">{1}</span>元(<span style="color:blue;">{0}</span>分)<br />', rowData.use_score, rowData.use_score_amount);
            }
            if (rowData.use_amount > 0) {
                str.AppendFormat('使用余额：<span style="color:red;">{0}</span>元<br />', rowData.use_amount);
            }
            if (rowData.total_amount > 0) {
                str.AppendFormat('实付金额：<span style="color:red;">{0}</span>元<br />', rowData.total_amount);
            }
            return str.ToString();
        }
        function FormatterPriceUnit(value, rowData) {
            return value + " " + rowData.unit;
        }
        function FormatterPayType(value, rowData) {
            if (value == "WEIXIN") {
                return "微信支付";
            }
            else if (value == "ALIPAY") {
                return "支付宝支付";
            }
            return "";
        }

        function Search() {
            var model = { type: type, keyword: $("#txtKeyword").val(), order_status: $("#ddlSearchOrderStatus").val() }
            $('#grvData').datagrid('load', model);
        }

        //设置访问等级
        function SetOrderStatus() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $("#ddlOrderStatus").val("");
            $("#dlgInfo").dialog({ title: "设置订单状态" });
            $("#dlgInfo").dialog("open");
        }


        //导出订单数据
        function ExportData() {
            $('#digData').dialog({ title: '按条件导出订单' });
            $('#digData').dialog('open');
        }
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].order_id
                    );
            }
            return ids;
        }
        function ShowTagName() {
            $('#dlgTagName').dialog({ title: '标签列表' });
            $('#dlgTagName').dialog('open');
        }

        function ShowUserInfo() {
            $('#dlgUserInfo').dialog({ title: '用户列表' });
            $('#dlgUserInfo').dialog('open');
        }


    </script>
</asp:Content>
