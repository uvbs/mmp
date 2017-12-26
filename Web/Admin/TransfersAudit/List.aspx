<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.TransfersAudit.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp; &nbsp;打款审核
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
             <a href="javascript:void(0);" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="setPerKefu()">设置审核权限客服</a>
            <a href="javascript:void(0);" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="pass()">打款</a>

            <br />
            状态:
            <select id="ddlStatus">
                <option value="">全部</option>
                <option value="0">待审核</option>
                <option value="1">已打款</option>

            </select>
             类型:
            <select id="ddlType">
                <option value="">全部</option>
                <option value="MallRefund">商城退款</option>
                <option value="DistributionWithdraw">分销提现</option>
                
            </select>
           <input id="txtKeyWord" style="width: 200px;display:none;"/>
            时间:
            <input type="text" id="txtFrom" style="width: 100px;" readonly="readonly" class="easyui-datebox" />-
            <input type="text" id="txtTo" style="width: 100px;" readonly="readonly" class="easyui-datebox" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnsearch"
                onclick="Search()">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
        <div id="kefuInfo" class="easyui-dialog" closed="true" modal="true" title="设置审核权限客服" style="width: 450px; height:335px ">

        <table id="grvKefu" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/TransfersAudit/";
        var isSubmit;
        $(function () {
            //记录列表
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "List.ashx",
                       height: document.documentElement.clientHeight - 120,
                       pagination: true,
                       striped: true,
                       singleSelect: true,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'insert_date', title: '时间', width: 10, align: 'left' },
                                   {
                                     field: 'type', title: '类型', width: 10, align: 'center', formatter: function (value, rowData) {
                                                                              if (value == "MallRefund") {
                                                                                  return "商城退款";
                                                                              }
                                                                              else if (value == "DistributionWithdraw") {
                                                                                  return "分销提现";
                                                                              }


                                                                          }
                                                                      },

                                   {
                                    field: 'status', title: '状态', width: 10, align: 'center', formatter: function (value, rowData) {
                                                                           if (value == 0) {
                                                                               return "<font color='red'>待审核</font>";
                                                                           }
                                                                           else if (value == 1) {
                                                                               return "<font color='grean'>已打款</font>";
                                                                           }


                                                                       }
                                                                   },


                                   { field: 'amount', title: '金额(元)', width: 10, align: 'left' },

                                   { field: 'tran_info', title: '说明', width: 55, align: 'left' }



                       ]]
                   }
               );
            $(".datebox :text").attr("readonly", "readonly");

            //打款状态
            $(ddlStatus).change(function () {
                Search();
            })

            //类型
            $(ddlType).change(function () {

                Search();
            })

            //客服
            $('#grvKefu').datagrid(
        {
            method: "Post",
            url: "/Handler/App/CationHandler.ashx",
            queryParams: { Action: "QueryKuFuList" },
            height:300,
            pagination: true,
            striped: true,
            rownumbers: true,
            singleSelect:false,
            rowStyler: function () { return 'height:25px'; },
            columns: [[
                        
                        { field: 'TrueName', title: '姓名', width: 20, align: 'left' },
                        { field: 'Phone', title: '手机号', width: 20, align: 'left' },
                        {
                           field: 'IsTransfersAuditPer', title: '审核权限', width: 15, align: 'left', formatter: function (value, rowData) {

                               if (value==0) {
                                   return "<font color='red'>无</font>";
                               }

                               if (value ==1) {
                                   return "<font color='grean'>有</font>";
                               }
                            }
                        },
                        {
                          field: 'Opera', title: '操作', width: 30, align: 'left', formatter: function (value, rowData) {
                                                        var str = new StringBuilder();
                                                        if (rowData.IsTransfersAuditPer ==1) {
                                                           
                                                            
                                                            str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="updateKefuPer({0},0);"><span class="l-btn-left"><span class="l-btn-text">取消审核权限</span></span></a><br />', rowData.AutoID);
                                                        }

                                                        if (rowData.IsTransfersAuditPer== 0) {
                                                            str.AppendFormat('<a href="javascript:;" class="l-btn" onclick="updateKefuPer({0},1);"><span class="l-btn-left"><span class="l-btn-text">设置审核权限</span></span></a><br />', rowData.AutoID);
                                                        }
                                                        return str.ToString();
                                                    }
                                                }
            ]]
        }
    );




        });

        //搜索
        function Search() {
            var fromDate = $("#txtFrom").datebox('getValue');
            var toDate = $("#txtTo").datebox('getValue');
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "List.ashx",
                       queryParams: {  keyword: $("#txtKeyWord").val(), status: $("#ddlStatus").val(), from_date: fromDate, to_date: toDate, type: $(ddlType).val() }
                   });
        }

        //打款
        function pass(status) {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            if (rows[0].status==1) {
                Alert("该记录已打款");
                return;

            }
            $.messager.confirm("系统提示", "确定打款", function (r) {

                if (r) {
                    //
                    if (isSubmit) {
                        return false;
                    }
                    isSubmit = true;
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl+"Update.ashx",
                        data: {  id: rows[0].id},
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.status == true) {
                                alert("操作成功");
                                $('#grvData').datagrid('reload');
                            }
                            else {
                                
                                    alert(resp.msg);
                            }


                        },
                        complete: function () {
                            isSubmit = false;
                            $.messager.progress('close');
                        }


                    });
                    //

                }

            })


        }

        //设置具有审核权限的客服
        function setPerKefu() {

            $('#kefuInfo').dialog('open');


        }

        //设置客服权限
        function updateKefuPer(autoId,value) {
            $.ajax({
                type: 'post',
                url: '/Serv/API/Admin/TransfersAudit/UpdateKefuPer.ashx',
                data: {id:autoId,value:value},
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvKefu').datagrid("reload");
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });


        }


    </script>
</asp:Content>
