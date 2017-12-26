<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Order.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;会员卡&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>会员卡开卡</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">


            <%if (PmsAdd)
              {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowSend();" id="btnAdd">开卡</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAddCus">增加会员卡订单</a>

             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowUpdateCardDlg();" >修改会员卡号</a>

            <% } %>

            <div>

                购买渠道:
                    <select id="ddlChannel">
                        <option value="">全部</option>
                        <option value="0">线上</option>
                        <option value="1">线下</option>
                    </select>
                开卡状态:
                    <select id="ddlStatus">
                        <option value="">全部</option>
                        <option value="0">未开卡</option>
                        <option value="1">已开卡</option>
                    </select>

                <input type="text" id="txtKeyWord" placeholder="姓名,手机号" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
       <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 300px; padding: 15px; line-height: 30px; height: 150px;">
        <table width="100%">

            <tr>
                <td>生效日期:
                </td>
                <td>
                     <input class="easyui-datetimebox" style="width: 150px;"  id="txtValidDate" />
                </td>
            </tr>

        </table>
    </div>
           <div id="dlgInputCard" class="easyui-dialog" closed="true" title="修改会员卡号" style="width: 400px; padding: 15px; line-height: 30px; height: 150px;">
        <table width="100%">

            <tr>
                <td>会员卡号:
                </td>
                <td>
                     <input type="text" style="width: 250px;"  id="txtCardNum" />
                </td>
            </tr>

        </table>
    </div>
    <div id="dlgInputCus" class="easyui-dialog" closed="true" title="添加" style="width: 300px; padding: 15px; line-height: 30px;">
        <table width="100%">

            <tr>
                <td>会员卡:
                </td>
                <td>
                    <select id="ddlCard">
                        <%foreach (var item in cardList){%>
                         <option value="<%=item.CardId %>"><%=item.CardName %></option>
                        <%} %>

                    </select>
                </td>
            </tr>
               <tr>
                <td>用户:
                </td>
                 <td>
                     <label id="lblUserId"></label> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add" onclick="ShowUserDlg();" >选择</a>

                </td>
            </tr>
        </table>
    </div>

     <div id="dlgUser" class="easyui-dialog" closed="true" title="选择用户" style="width: 600px;
        padding: 15px;">
        姓名:
        <input type="text" id="txtTrueNameKeyWord" style="width: 200px;" placeholder="姓名" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchUser()">
            查询</a>
        <table id="grvUserData" fitcolumns="true">
        </table>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/meifan/order/";
        var cardId = "";
        var userId = "";
        var orderId = "";
        var userGrid;
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "list.ashx",
                       queryParams: {},
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   //{
                                   //    field: 'img_url', title: '图片', width: 10, align: 'center', formatter: function (value) {
                                   //        if (value == '' || value == null)
                                   //            return "";
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                   //        return str.ToString();
                                   //    }
                                   //},
                                   {
                                       field: 'card_type', title: '会员卡类型', width: 20, align: 'left', formatter: function (value) {
                                           if (value == '' || value == null)
                                               return "";
                                           switch (value) {
                                               case "personal":
                                                   return "个人卡";
                                               case "family":
                                                   return "家庭卡";
                                               case "chuandong":
                                                   return "船东卡";
                                               default:

                                           }
                                       }
                                   },
                                   { field: 'title', title: '会员卡名称', width: 20, align: 'left' },
                                   { field: 'card_number', title: '会员卡号', width: 20, align: 'left' },
                                   { field: 'show_name', title: '关联会员', width: 20, align: 'left' },
                                   { field: 'show_phone', title: '关联会员手机号', width: 20, align: 'left' },
                                   { field: 'amount', title: '购买金额（卡费＋手续费）', width: 10, align: 'left' },
                                   { field: 'time', title: '购买日期', width: 10, align: 'left' },
                                   {
                                       field: 'channel', title: '购买渠道', width: 10, align: 'left', formatter: function (value, rowData) {

                                           if (value=="1") {
                                               return "线下";
                                           }
                                           return "线上";
                                       }
                                   },
                                   {
                                       field: 'apply_card_status', title: '开卡状态', width: 10, align: 'left', formatter: function (value, rowData) {

                                           var str = new StringBuilder();
                                           switch (value) {

                                               case "1":
                                                   str.AppendFormat('<font color="green">已开卡</font>');
                                                   break;
                                               default:
                                                   str.AppendFormat('<font color="red">未开卡</font>');
                                                   break;

                                           }

                                           return str.ToString();
                                       }
                                   },
                                   { field: 'valid_date', title: '开卡/生效日期', width: 10, align: 'left' },
                                   { field: 'valid_to_date', title: '失效日期', width: 10, align: 'left' }


                       ]]
                   }
            );

            $('#dlgInput').dialog({
                buttons: [{
                    text: '开卡',
                    handler: function () {
                        try {



                            var dataModel = {

                                card_id: cardId,
                                user_id: userId,
                                order_id: orderId,
                                valid_date: $('#txtValidDate').datetimebox('getValue')


                            }
                            if (dataModel.valid_date == '') {

                                Alert("请选择生效日期");
                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: '/serv/api/admin/meifan/card/send.ashx',
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
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

                        $('#dlgInput').dialog('close');
                    }
                }]
            });

            $('#dlgInputCus').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {



                            var dataModel = {

                                card_id: $("#ddlCard").val(),
                                user_id: userId
                               


                            }
                            if (dataModel.user_id == '') {

                                Alert("请选择用户");
                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: '/serv/api/admin/meifan/order/add.ashx',
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInputCus').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
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

                        $('#dlgInputCus').dialog('close');
                    }
                }]
            });

            $('#dlgInputCard').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                                card_num: $('#txtCardNum').val(),
                                order_id: orderId

                            }
                            if (dataModel.card_num == '') {

                                Alert("请输入会员卡号");
                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: '/serv/api/admin/meifan/card/updatecardnum.ashx',
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInputCard').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
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

                        $('#dlgInputCard').dialog('close');
                    }
                }]
            });



            $("#ddlStatus").change(function () {
                Search();
            })

            $("#ddlChannel").change(function () {
                Search();
            })

            $("#btnSearch").click(function () {

                Search();


            })


            $('#dlgUser').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = userGrid.datagrid('getSelections');
                        var num = rows.length;
                        if (num == 0) {
                            messager("系统提示", "请选择至少一个用户");
                            return;
                        }
                        userId = rows[0].UserID;
                        var showName = rows[0].TrueName;
                        if (showName == "") {
                            showName = rows[0].WXNickname;
                        }
                        if (showName == "") {
                            showName = rows[0].UserID;
                        }
                        $("#lblUserId").html(showName);
                        $('#dlgUser').dialog('close');

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUser').dialog('close');
                    }
                }]
            });

            userGrid = $('#grvUserData').datagrid(
             {
                 method: "Post",
                 //url: "/Handler/App/CationHandler.ashx?Action=QueryWebsiteUser",
                 url: "/serv/api/admin/member/list.ashx",
                 loadFilter: pagerFilter,
                 height: 400,
                 pagination: true,
                 striped: true,
                 pageSize: 20,
                 rownumbers: true,
                 singleSelect: true,
                 rowStyler: function () { return 'height:25px'; },
                 columns: [[
                              { title: 'ck', width: 5, checkbox: true },
                             {
                                 field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                     if (value == '' || value == null)
                                         return "";
                                     var str = new StringBuilder();
                                     str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                     return str.ToString();
                                 }
                             },
                             { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                             { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                             { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle }
                             //,
                             // { field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
                             //  { field: 'Postion', title: '职位', width: 100, align: 'left', formatter: FormatterTitle }




                 ]]

             });




        });

        function ShowSend() {


            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;


            if (rows[0].apply_card_status == 1) {
                alert("已经开过卡了");
                return;
            }

            $('#txtValidDate').datetimebox('setValue', '<%=System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")%>');
            cardId = rows[0].relation_id;
            userId = rows[0].user_id;
            orderId = rows[0].order_id;
            $('#dlgInput').dialog({ title: '开卡' });
            $('#dlgInput').dialog('open');




        }

        
        function ShowUpdateCardDlg() {


            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            if (rows[0].apply_card_status!="1") {

                Alert("未开卡,请先开卡");
                return false;
            }

            orderId = rows[0].order_id;
            
            $("#txtCardNum").val(rows[0].card_number);
            $('#dlgInputCard').dialog({ title: '修改会员卡号' });
            $('#dlgInputCard').dialog('open');




        }
        function ShowAdd() {



            $('#dlgInputCus').dialog({ title: '增加订单' });
            $('#dlgInputCus').dialog('open');




        }

        function Search() {

            $('#grvData').datagrid({ url: "/serv/api/admin/meifan/order/list.ashx", queryParams: { status: $("#ddlStatus").val(), keyword: $("#txtKeyWord").val(),channel:$("#ddlChannel").val() } });



        }
        function ShowUserDlg() {

            $('#dlgUser').dialog({ title: '选择用户' });
            $('#dlgUser').dialog('open');

        }
        //搜索
        function SearchUser() {
            $('#grvUserData').datagrid(
                    {
                        method: "Post",
                        //url: "/Handler/App/CationHandler.ashx?Action=QueryWebsiteUser",
                        url: "/serv/api/admin/member/list.ashx",
                        queryParams: { keyword: $(txtTrueNameKeyWord).val() }
                    });
        }


    </script>
</asp:Content>
