<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallScoreOrderMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallScoreOrderMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:&nbsp;会员&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>积分订单管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除订单</a>
                    
                   <%-- <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'" id="btnExportToFile">导出订单</a>--%>
                                                    
                                                 
            <br />
           <label style="margin-left:8px;">订单状态</label>
          <select id="ddlOrderStatus" >
          <option  value="" >全部</option>
           <%=sbOrderStatuList.ToString()%>
           </select>
            <label style="margin-left:8px;">订单编号:</label>
           <input type="text" id="txtOrderID" style="width:200px;" />


           <label style="margin-left:8px;">时间:</label>
           <input type="text" id="txtFrom" style="width:100px;" readonly="readonly" class="easyui-datebox" />-
           <input type="text" id="txtTo" style="width:100px;" readonly="readonly" class="easyui-datebox" />
           
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallScoreOrderInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'OrderID', title: '订单编号', width: 10, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a  style="color:#005ea7;" title="点击查看订单详情" href="/App/MallManage/WXMallScoreOrderDetail.aspx?oid={0}" ">{1}</a>', rowData.OrderID, value);
                                return str.ToString();
                            }
                            },
                            { field: 'Status', title: '订单状态', width: 10, align: 'center', formatter: FormatterTitle },
                            { field: 'HeadImgNickName', title: '头像/昵称', width: 20, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<img src="{0}" width="50" height="50">{1}', rowData.OrderUserInfo.WXHeadimgurlLocal, rowData.OrderUserInfo.WXNickname);
                                return str.ToString();
                            }
                        },
                            { field: 'OrderUserID', title: '下单用户', width: 10, align: 'center', formatter: FormatterTitle },
                            { field: 'Consignee', title: '收货人', width: 10, align: 'center', formatter: FormatterTitle },
                            { field: 'Phone', title: '手机号', width: 10, align: 'center', formatter: FormatterTitle },
                            { field: 'TotalAmount', title: '积分', width: 10, align: 'center', formatter: formartcolor },
                            { field: 'ProductCount', title: '商品总数量', width: 10, align: 'center' },
                            { field: 'InsertDate', title: '下单时间', width: 10, align: 'center', formatter: FormatDate },
                            { field: 'EditCloum', title: '详情', width: 10, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="/App/MallManage/WXMallScoreOrderDetail.aspx?oid={0}" ">查看</a>', rowData.OrderID);
                                return str.ToString();
                            }
                            }




                             ]]
	            }
            );

            $(".datebox :text").attr("readonly", "readonly");



            //导出文件
            $("#btnExportToFile").click(function () {

                var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                if (!EGCheckIsSelect(rows)) {
                    return;
                }
                $.messager.confirm('系统提示', '确认导出所选订单？', function (o) {
                    if (o) {

                        ExportOrder(GetRowsIds(rows).join(','));

                    }
                });

            });


        });

        //导出订单
        function ExportOrder(oids) {
            window.open(handlerUrl + "?Action=ExportOrder&oids=" + oids);

        }




        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中订单?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWXMallScoreOrderInfo", oid: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            Alert("成功删除了" + result + "个订单");
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });


        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].OrderID
                 );
            }
            return ids;
        }


        function Search() {

            var FromDate = $("#txtFrom").datebox('getValue');
            var ToDate = $("#txtTo").datebox('getValue');
            var OrderStatus = $("#ddlOrderStatus").val();

            //            if (FromDate != "") {
            //                if (!CheckDateTime(FromDate)) {
            //                    $.messager.alert("系统提示", "开始时间不正确", "warning");
            //                    return;
            //                }
            //            }
            //            if (ToDate != "") {
            //                if (!CheckDateTime(ToDate)) {
            //                    $.messager.alert("系统提示", "结束时间不正确", "warning");
            //                    return;
            //                }

            //            }
            //            if (FromDate != "" && ToDate != "") {

            //                if (From > To) {
            //                    $.messager.alert("系统提示", "开始时间不能大于结束时间", "warning");
            //                    return;
            //                }
            //            }



            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallScoreOrderInfo", OrderID: $("#txtOrderID").val(), FromDate: FromDate, ToDate: ToDate, OrderStatus: OrderStatus }
	            });
        }


        //验证长时间
        function CheckDateTime(str) {


            //            var r ＝ str.match(/^(/d{1,4})(-|//)(/d{1,2})/2(/d{1,2})$/); 
            //            if(r＝＝null)return false; 
            //            var d＝ new Date(r[1], r[3]-1, r[4]); 
            //            return (d.getFullYear()＝＝r[1]&&(d.getMonth()+1)＝＝r[3]&&d.getDate()＝＝r[4]);


        }

        function formartcolor(value) {

            return "<font color='red'>" + value + "</font>";
        }
    </script>

</asp:Content>
