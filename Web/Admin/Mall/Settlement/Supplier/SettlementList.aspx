<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SettlementList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.Supplier.SettlementList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .blue {
            color: blue;
        }
        .notice {
            font-size:15px;
            font-weight:bold;
            margin-bottom:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;平台结算&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商户结算</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <div class="notice">
            结算流程:
            每月1号由系统生成上月结算单 ->商户已确认->商城已确认->财务已确认->财务已打款->商户已确认收款
            </div>
            <div>

                <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="setIsInvoice()">标记为已经开票</a>
                 <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-excel" plain="true" onclick="exportToExcel()">导出</a>
            </div>
            时间:
             <select id="ddlYear" >
                 <option value=""></option>
                 <%foreach (var item in yearList)
                  {%>
                <option value="<%=item %>"><%=item %></option>
                <%} %>
             </select>年
            <select id="ddlMonth">
                <option value=""></option>
                <option value="1">1月</option>
                <option value="2">2月</option>
                <option value="3">3月</option>
                <option value="4">4月</option>
                <option value="5">5月</option>
                <option value="6">6月</option>
                <option value="7">7月</option>
                <option value="8">8月</option>
                <option value="9">9月</option>
                <option value="10">10月</option>
                <option value="11">11月</option>
                <option value="12">12月</option>
            </select>月
            商户
            <select id="ddlSupplier" onchange="searchs()">
                <option value="">全部</option>
                <%foreach (var item in supplierList)
                  {%>
                <option value="<%=item.UserID %>"><%=item.Company %></option>
                <%} %>
            </select>
            状态
            <select id="ddlStatus" onchange="searchs()">
                <option value="">全部</option>
                <option value="待供应商确认">待商户确认</option>
                <option value="供应商已确认">商户已确认</option>
                <option value="商城已确认">商城已确认</option>
                <option value="财务已确认">财务已确认</option>
                <option value="财务已打款">财务已打款</option>
                <option value="供应商已确认收款">商户已确认收款</option>
            </select>

            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="输入结算单号" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search"
                onclick="searchs();">查询</a>
        </div>
    </div>


    <table id="grvData" fitcolumns="true">
    </table>

    <div id="dlgInput" class="easyui-dialog" closed="true" title="打款" style="width: 370px;
         padding: 15px;line-height:30px;">
        <table width="100%">

            <tr>
                <td>
                    打款证明图片:
                </td>
                <td>
                      <img alt="图片" id="imgThumbnailsPath" style="max-width:200px;max-height:100px;"/>
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                         onclick="txtThumbnailsPath.click()">上传图片</a>
                        
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display:none;" />
                </td>
            </tr>
          <tr>
                <td>
                   打款说明:
                </td>
                <td>
                  <textarea id="txtRemark" style="width: 250px;height:50px;"></textarea>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(ddlYear).val("<%=DateTime.Now.Month==1?(DateTime.Now.Year-1):DateTime.Now.Year %>");
        $(ddlMonth).val("<%=DateTime.Now.Month==1?12:(DateTime.Now.Month-1) %>");
        var handlerUrl = "/Serv/Api/Admin/Mall/Settlement/Supplier/SettlementList.ashx";
        var settlementId = "";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { settlement_id: $("#txtKeyWord").val(), status: $("#ddlStatus").val(), date: getDate() },
                       height: document.documentElement.clientHeight - 150,
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[

                                  {
                                      field: 'time_range', title: '起止时间', width: 30, align: 'left', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          str.AppendFormat('{0}&nbsp;至&nbsp;{1}', rowData.from_date, rowData.to_date);
                                          return str.ToString();
                                      }
                                  },
                                  { field: 'settlement_id', title: '结算单号', width: 20, align: 'left' },
                                   { field: 'supplier_name', title: '商户名称', width: 20, align: 'left' },
                                   { field: 'total_baseamount', title: '产品结算金额', width: 30, align: 'left' },
                                   { field: 'settlement_sale_amount', title: '销售额', width: 30, align: 'left' },
                                   { field: 'settlement_server_amount', title: '服务费<br>服务费=销售额-产品结算额', width: 30, align: 'left' },
                                   { field: 'total_transportfee', title: '代收运费', width: 10, align: 'left' },
                                   { field: 'settlement_total_amount', title: '结算金额(元)<br>结算金额=产品结算金额+代收运费', width: 30, align: 'left' },
                                   
                                   {
                                       field: 'status', title: '结算状态', width: 10, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           //str.AppendFormat('<a class="blue" href="SettlementDetail.aspx?settlement_id={0}" target="_blank">查看结算明细<a>', rowData.settlement_id);

                                           switch (rowData.status) {
                                               case "待供应商确认":
                                                   str.AppendFormat("{0}", rowData.status);
                                                   break;
                                               case "供应商已确认":
                                                   str.AppendFormat("{0}", rowData.status);
                                                   break;
                                               case "商城已确认":
                                                   str.AppendFormat("{0}", rowData.status);
                                                   break;
                                               case "财务已确认":
                                                   str.AppendFormat("{0}", rowData.status);
                                                   break;
                                               case "财务已打款":
                                                   str.AppendFormat("<font color='green'>{0}</font>", rowData.status);
                                                   break;
                                               case "供应商已确认收款":
                                                   str.AppendFormat("<font color='green'>{0}</font>", rowData.status);
                                                   break;

                                               default:

                                           }
                                           return str.ToString();
                                       }
                                   },
                                   
                                   //{ field: 'total_baseamount', title: '总基价', width: 10, align: 'left' },
                                   
                                   //{ field: 'refund_total_amount', title: '总退款金额(元)', width: 10, align: 'left' },
                                   
                                   {
                                     field: 'settlement_detail', title: '结算单明细', width: 10, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a class="blue" href="SettlementDetail.aspx?settlement_id={0}" target="_blank">【查看】<a>', rowData.settlement_id);
                                           return str.ToString();
                                       }
                                   },
                                  {
                                    field: 'is_invoice', title: '已开票', width: 10, align: 'left', formatter: function (value, rowData) {
                                                         
                                          if (value==0) {
                                              return "<font color='red'>未开票</font>";
                                          }
                                          if (value == 1) {
                                              return "<font color='grean'>已开票</font>";
                                          }

                                    }
                                    },
                                   {
                                       field: 'operate', title: '操作', width: 15 , align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           //str.AppendFormat('<a class="blue" href="SettlementDetail.aspx?settlement_id={0}" target="_blank">查看结算明细<a>', rowData.settlement_id);

                                           switch (rowData.status) {
                                               case "待供应商确认":
                                                   break;
                                               case "供应商已确认":
                                                   str.AppendFormat("&nbsp;<a class=\"blue\"  onclick=\"updateStatus('{0}','商城已确认')\">[商城已确认]<a>", rowData.settlement_id);
                                                   break;
                                               case "商城已确认":
                                                   str.AppendFormat("&nbsp;<a class=\"blue\"  onclick=\"updateStatus('{0}','财务已确认')\">[财务已确认]<a>", rowData.settlement_id);
                                                   break;
                                               case "财务已确认":
                                                   str.AppendFormat("&nbsp;<a class=\"blue\"  onclick=\"updateStatusTran('{0}','财务已打款')\">[财务已打款]<a>", rowData.settlement_id);
                                                   break;
                                               case "财务已打款":
                                                   if (rowData.img_url != "" && rowData.img_url != null) {
                                                       str.AppendFormat("<a class=\"blue\" href=\"{0}\" target=\"_blank\">[查看图片]<a>", rowData.img_url);
                                                   }
                                                   if (rowData.remark != "" && rowData.remark != null) {
                                                       str.AppendFormat("<br/>{0}", rowData.remark);
                                                   }
                                                   break;
                                               case "供应商已确认收款":
                                                   if (rowData.img_url != "" && rowData.img_url != null) {
                                                       str.AppendFormat("<a class=\"blue\" href=\"{0}\" target=\"_blank\">[查看图片]<a>", rowData.img_url);
                                                   }

                                                   if (rowData.remark != "" && rowData.remark != null) {
                                                       str.AppendFormat("<br/>{0}", rowData.remark);
                                                   }
                                                   break;
                                                   
                                               default:

                                           }
                                           return str.ToString();
                                       }
                                   }
                       ]]
                   }
            );


            $("#ddlYear").change(function () {
               
                if ($(this).val() != "") {
                    if ($("#ddlMonth").val()=="") {
                        $("#ddlMonth").val("1")
                    }
                    

                } else {
                    $("#ddlMonth").val("");
                   
                }
                searchs();

            }
           );

            $("#ddlMonth").change(function () {

                if ($(this).val() == "") {
                    $("#ddlYear").val("");

                } else {
                    if ($(ddlYear).val()=="") {
                        
                        Alert("请选择年份");
                        return false;
                    }

                }
                searchs();

            }
);
            $('#dlgInput').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        //
                        $.messager.confirm("系统提示", "确认打款?", function (r) {
                            if (r) {

                                $.ajax({
                                    type: 'post',
                                    url: "/Serv/Api/Admin/Mall/Settlement/Supplier/UpdateStatus.ashx",
                                    data: { settlement_id: settlementId, status: "财务已打款", remark: $("#txtRemark").val(), img: $("#imgThumbnailsPath").attr("src") },
                                    dataType: "json",
                                    success: function (resp) {
                                        if (resp.status) {
                                            Alert(resp.msg);
                                            $('#grvData').datagrid('reload');
                                            $('#dlgInput').dialog('close');
                                        } else {
                                            Alert(resp.msg);
                                        }


                                    }
                                });
                            }
                        });
                        //

                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });



            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                        {
                            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                            secureuri: false,
                            fileElementId: 'txtThumbnailsPath',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {

                                    $('#imgThumbnailsPath').attr('src', resp.ExStr);
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        }
                       );

                } catch (e) {
                    alert(e);
                }
            });



        });
        //搜索
        function searchs() {

            $('#grvData').datagrid(
            {
                method: "Post",
                url: handlerUrl,
                queryParams: { settlement_id: $("#txtKeyWord").val(), status: $("#ddlStatus").val(), supplier_user_id: $("#ddlSupplier").val(),date:getDate() },
            });


        }
        //function yearChange() {
          
        //    if ($(this).val()!="") {
        //        $("#ddlMonth").val("1");
                
        //    }
        //    search();


        //}
        //function monthChange() {
            
        //        if ($(this).val()=="") {
        //            $("#ddlYear").val("");

        //        }
        //    searchs();


        //}
        function getDate() {

            var year = $(ddlYear).val();
            var month = $(ddlMonth).val();
            if (year == "") {
                return "";
            }
            if (month=="") {
                return "";
            }
            return year.toString() + "-" +padLeft(month).toString()+ "-01";


        }
        function padLeft(value){
            if (parseInt(value)<10) {
                return "0" + value.toString();
            }
            return value.toString();
        }

        function updateStatus(settlementId,status) {
           
            $.messager.confirm("系统提示", "确认"+status+"?", function (r) {
                if (r) {

                    $.ajax({
                        type: 'post',
                        url: "/Serv/Api/Admin/Mall/Settlement/Supplier/UpdateStatus.ashx",
                        data: { settlement_id: settlementId, status: status },
                        dataType:"json",
                        success: function (resp) {
                            if (resp.status) {
                                Alert(resp.msg);
                                $('#grvData').datagrid('reload');
                            } else {
                                Alert(resp.msg);
                            }
                           
                           
                        }
                    });
                }
            });


        }


        function updateStatusTran(settlement_id, status) {

            settlementId = settlement_id;
            $('#dlgInput').dialog('open');
            $("#imgThumbnailsPath").attr("src", "");
            $("#txtRemark").val("");
           
          


        }

        // 设置开票
        function setIsInvoice() {


            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }

            $.messager.confirm("系统提示", "确认标记为已经开票?", function (r) {
                if (r) {

                    $.ajax({
                        type: 'post',
                        url: "/Serv/Api/Admin/Mall/Settlement/Supplier/UpdateIsInvoice.ashx",
                        data: { id: rows[0].id, is_invoice: 1  },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                Alert(resp.msg);
                                $('#grvData').datagrid('reload');
                            } else {
                                Alert(resp.msg);
                            }


                        }
                    });
                }
            });

        }


        // 导出excel
        function exportToExcel() {

            $.messager.confirm("系统提示", "确认按所选条件导出?", function (r) {
                if (r) {
                    var location = "/Serv/Api/Admin/Mall/Settlement/Supplier/ExportSettlementList.ashx?settlement_id=" + $("#txtKeyWord").val() + "&status=" + $("#ddlStatus").val() + "&supplier_user_id=" + $("#ddlSupplier").val() + "&date=" + getDate();
                    window.open(location);

                }
            });

        }
    </script>
</asp:Content>
