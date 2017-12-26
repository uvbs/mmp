<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="QianWeiStockStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.QianWeiStockStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>库存管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
<div style="margin-bottom:80px;">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px;text-align:center;">
        <div ><span id="timerSpan" style="color: Red;font-size:30px;font-weight:bold;"></span>&nbsp;秒后刷新</div>
        
        </div>
         
    </div>

    <table style="width:100%;">
    <tr>
    <td style="width:45%;text-align:center;" valign="top">
    
    <h3>黄浦区店</h3>
    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-refresh" plain="true"
                onclick="RefreshCategory1();" >立即刷新</a>
               
    <table id="grvData0" fitcolumns="true" >
                   
    </table>
    </td>
    <td style="width:45%;text-align:center;" valign="top">
            <h3>静安区店</h3>
    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-refresh" plain="true"
                onclick="RefreshCategory2();" >立即刷新</a>
    <table id="grvData1" fitcolumns="true">
                   
    </table>
    
    </td>
    
    </tr>
    </table>


  </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var CategoryID1 = "1";
    var CategroyID2 = "2";

    $(function () {

        $('#grvData0').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QianWeiStockStatistics", CategoryId: CategoryID1 },
	                //pagination: true,
	                striped: true,
	                //pageSize: 50,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[

                            { field: 'PName', title: '商品名称', width: 160, align: 'center' },
                            { field: 'Stock', title: '库存', width: 100, align: 'center', formatter: function (value, rowData) {

                                var str = new StringBuilder();
                                str.AppendFormat('<label id="lbl{0}">{1}</label>', rowData.PID, value);
                                return str.ToString();


                            }
                            },
                            { field: 'EditCloum', title: '操作', width: 100, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="增加库存" href="javascript:void(0);" class="button glow button-rounded button-flat-action button-tiny" style="margin-top:2px;margin-bottom:2px;margin-left:2px;margin-right:2px;font-size:12px;width:15px;height:20px;"onclick="UpdateStock({0},1)">+</a>&nbsp;&nbsp;<a title="减少库存" href="javascript:void(0);" class="button glow button-rounded button-flat-caution button-tiny" style="margin-top:2px;margin-bottom:2px;margin-left:2px;margin-right:2px;font-size:12px;width:15px;height:20px;"onclick="UpdateStock({0},-1)">-</a>', rowData.PID);

                                //str.AppendFormat('<a title="增加库存" href="javascript:void(0);"><img src="../../MainStyle/Res/easyui/themes/icons/edit_add.png" onclick="UpdateStock({0},1)"/></a>&nbsp;&nbsp;<a title="减少库存" href="javascript:void(0);"><img src="../../MainStyle/Res/easyui/themes/icons/edit_remove.png"  onclick="UpdateStock({0},-1)"/></a>', rowData.PID);
                                return str.ToString();
                            }
                            }
                             ]]

	            }
            );


        $('#grvData1').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QianWeiStockStatistics", CategoryId: CategroyID2 },
	                //pagination: true,
	                striped: true,
	                //pageSize: 50,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[

                            { field: 'PName', title: '商品名称', width: 160, align: 'center' },
                            { field: 'Stock', title: '库存', width: 100, align: 'center', formatter: function (value, rowData) {

                                var str = new StringBuilder();
                                str.AppendFormat('<label id="lbl{0}">{1}</label>', rowData.PID, value);
                                return str.ToString();


                            }
                            },
                            { field: 'EditCloum', title: '操作', width: 100, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                //str.AppendFormat('<a title="增加库存" href="javascript:void(0);"><img src="../../MainStyle/Res/easyui/themes/icons/edit_add.png" onclick="UpdateStock({0},1)"/></a>&nbsp;&nbsp;<a title="减少库存" href="javascript:void(0);"><img src="../../MainStyle/Res/easyui/themes/icons/edit_remove.png"  onclick="UpdateStock({0},-1)"/></a>', rowData.PID);

                                str.AppendFormat('<a title="增加库存" href="javascript:void(0);" class="button glow button-rounded button-flat-action button-tiny" style="margin-top:2px;margin-bottom:2px;margin-left:2px;margin-right:2px;font-size:12px;width:15px;height:20px;"onclick="UpdateStock({0},1)">+</a>&nbsp;&nbsp;<a title="减少库存" href="javascript:void(0);" class="button glow button-rounded button-flat-caution button-tiny" style="margin-top:2px;margin-bottom:2px;margin-left:2px;margin-right:2px;font-size:12px;width:15px;height:20px;"onclick="UpdateStock({0},-1)">-</a>', rowData.PID);
                                return str.ToString();
                            }
                            }
                             ]]

	            }
            );

        startTime();


    });
    var tim = 60; //定时器
    function startTime() {
        $(timerSpan).html(tim);
        if (tim <= 0) {
            tim = 60;
            //刷新
            RefreshCategory1();
            RefreshCategory2();
        }
        else {
            tim--;
        }
        setTimeout('startTime()', 1000);
    }



    function UpdateStock(productid, count) {
        $.messager.progress({ text: '正在处理...' });
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: "UpdateProductStock", ProductID: productid, UpdateCount: count },
            dataType: 'json',
            success: function (resp) {
                $.messager.progress('close');
                if (resp.Status == 1) {
                    Show("操作成功!");
                    $("#lbl" + productid).html(parseInt($("#lbl" + productid).html()) + parseInt(count));

                }
                else {
                    Alert(resp.Msg);
                }

            }
        });



    }

    function RefreshCategory1() {

        $('#grvData0').datagrid("reload");


    }
    function RefreshCategory2() {

        $('#grvData1').datagrid("reload");


    }
    $(window).resize(function () {
        var width = document.body.clientWidth / 2;
        $('#grvData0').datagrid('resize', {
            width: width

        });
        $('#grvData1').datagrid('resize', {
            width: width

        });
    });
</script>

</asp:Content>