<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ListAll.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Train.SignUp.ListAll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                <style>
                #dlgInputAdd input[type='text'] {
                    height: 25px;
                    width: 95%;
                }
                    #txtUserRemark {
                        width:95%;
                    }
            </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;订单管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>培训订单</span>
    <a href="../List.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

              

            <%if (PmsAdd){%>
               <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowDlgAdd();">添加</a>
             <% } %>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowDlg();">填写教练评语</a>
            <div>

            <span style="font-size: 12px; font-weight: normal">报名时间：</span>
            <input class="easyui-datebox" id="txtFrom" />&nbsp;至
                <input class="easyui-datebox" id="txtTo" />
                培训名称:
                <input type="text" id="txtActivityName" placeholder="培训名称" />
                关键字：<input type="text" id="txtKeyWord" placeholder="下单人姓名、下单人手机" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
     <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 600px; padding: 15px; line-height: 30px;height:400px;">
        <table width="100%">

            <tr>
                <td style="width:50px;">教练评语:
                </td>
                <td>
                    <textarea id="txtRemark" ></textarea>

                </td>
            </tr>
        </table>
    </div>
     <div id="dlgInputAdd" class="easyui-dialog" closed="true" title="添加" style="width: 600px; padding: 15px; line-height: 30px;height:400px;">
        <table width="100%">

             <tr>
                <td style="width:50px;">名称
                </td>
                <td>
                    <select id="ddlActivity" onchange="loadDateList()">
                       <%foreach (var item in ActivityList)
                         {%>
                        <option value="<%=item.JuActivityID %>"><%=item.ActivityName %></option>
                       <%} %>
                    </select>

                </td>
            </tr>
            <tr>
                <td style="width:50px;">姓名
                </td>
                <td>
                    <input id="txtName" type="text" placeholder="请输入姓名"/ >

                </td>
            </tr>

            <tr>
                <td style="width:50px;">联系电话
                </td>
                <td>
                    <input id="txtPhone" type="text" placeholder="请输入联系电话"/ >

                </td>
            </tr>
            <tr>
                <td style="width:50px;">邮箱
                </td>
                <td>
                    <input id="txtEmail" type="text" placeholder="请输入邮箱"/ >

                </td>
            </tr>
             <tr>
                <td style="width:50px;">性别
                </td>
                <td>
                    <input id="rdoSex1" type="radio" value="男" name="rdoSex" checked="checked" /><label for="rdoSex1">男</label>
                    <input id="rdoSex0" type="radio" value="女" name="rdoSex" /><label for="rdoSex0">女</label>
                </td>
            </tr>
               <tr>
                <td style="width:50px;">生日
                </td>
                <td>
                    <input id="txtBirthDay" type="text" >
                </td>
                   </tr>

                 <tr>
                <td style="width:50px;">时间范围
                </td>
                <td>

                    <select id="ddlDateRange"></select>
                     
                   

                </td>
            </tr>

                <tr>
                <td style="width:50px;">组别
                </td>
                <td>
                    <select id="ddlGroup">
                        <%foreach (var item in GroupList){%>
                                 <option value="<%=item.TagName%>"><%=item.TagName %></option>
                                 <% } %>
                    </select>

                </td>
            </tr>
            <tr>
                <td style="width:50px;">会员类型
                </td>
                <td>
                    <select id="ddlMemberType">
                        <option value="全部">全部</option>
                        <option value="会员">会员</option>
                        <option value="非会员">非会员</option>
                    </select>

                </td>
            </tr>

             <tr>
                <td style="width:50px;">支付状态
                </td>
                <td>
                    <select id="ddlPayStatus">
                        <option value="0">未付款</option>
                        <option value="1">已经付款</option>
                    </select>

                </td>
            </tr>

               <tr>
                <td style="width:50px;">金额
                </td>
                <td>
                     <input id="txtAmount" type="number"/ >

                </td>
            </tr>
            <tr>
                <td style="width:50px;">用户
                </td>
                <td>
                     <label id="lblUserId"></label> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add" onclick="ShowUserDlg();" >选择</a>

                </td>
            </tr>
            <tr>
                <td style="width:50px;">备注
                </td>
                <td>
                     <textarea id="txtUserRemark" ></textarea>

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

        var handlerUrl = "/serv/api/admin/meifan/activity/signup/";
        var activityType = "train";
        var currSelectID = "";
        var selectUserId = "";
       
        var userGrid;
        $(function () {

            loadDateList();
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "list.ashx",
                       queryParams: { activity_type: activityType },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: true,
                       
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'ActivityName', title: '培训名称', width: 20, align: 'left' },
                                   { field: 'Name', title: '报名人姓名', width: 20, align: 'left' },
                                   { field: 'Phone', title: '报名人手机', width: 20, align: 'left' },
                                   { field: 'Sex', title: '性别', width: 10, align: 'left' },
                                   { field: 'BirthDay', title: '出生日期', width: 20, align: 'left' },
                                   { field: 'Email', title: '邮箱', width: 20, align: 'left' },
                                   { field: 'UserRemark', title: '备注', width: 20, align: 'left' },
                                   { field: 'DateRange', title: '报名时段', width: 30, align: 'left' },
                                   { field: 'GroupType', title: '组别', width: 20, align: 'left' },
                                   { field: 'IsMember', title: '是否会员', width: 10, align: 'left' },
                                   { field: 'Amount', title: '报名费', width: 20, align: 'left' },
                                   {
                                       field: 'PaymentStatus', title: '付款状态', width: 10, align: 'left', formatter: function (value, rowData) {

                                           var str = new StringBuilder();
                                           switch (value) {
                                               case 1:
                                                   str.AppendFormat('<font color="green">已付款</font>');
                                                   break;
                                               case 0:
                                                   str.AppendFormat('<font color="red">未付款</font>');
                                                   break;

                                                   break;
                                               default:

                                           }

                                           return str.ToString();
                                       }


                                   },
                                   {
                                       field: 'Remarks', title: '教练评语', width: 20, align: 'left', formatter: function (value, rowData) {

                                          
                                           var str = new StringBuilder();
                                           if (value!=""&&value!=null) {
                                               str.AppendFormat('<a onclick="showRemarks(\'{0}\')">已评 [查看]</a>', rowData.OrderId);
                                           } else {
                                               str.AppendFormat('<a>空</a>');
                                           }
                                           return str.ToString();
                                       }
                                   },
                                   { field: 'K1', title: '下单人姓名', width: 20, align: 'left' },
                                   { field: 'K2', title: '下单人手机', width: 20, align: 'left' },
                                   { field: 'InsertDateStr', title: '报名时间', width: 20, align: 'left' }
                                  
                                   
                                   

                       ]]
                   }
            );

            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {

                                id: currSelectID,
                                remark: editor.html()

                            }


                            if (dataModel.remark == '') {


                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: "/serv/api/admin/meifan/train/signup/update.ashx",
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



            $("#btnSearch").click(function () {

                Search();


            })

            //
            $('#dlgInputAdd').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var sex = "男";
                            if (rdoSex0.checked) {
                                sex = "女";
                            }
                            var dataModel = {

                                activity_id: $("#ddlActivity").val(),
                                name: $("#txtName").val(),
                                phone: $("#txtPhone").val(),
                                email: $("#txtEmail").val(),
                                sex: sex,
                                birthDay: $("#txtBirthDay").val(),
                                date_range: $("#ddlDateRange").val(),
                                group_type: $("#ddlGroup").val(),
                                member_type: $("#ddlMemberType").val(),
                                amount: $("#txtAmount").val(),
                                remark: $("#txtUserRemark").val(),
                                user_id: selectUserId,
                                is_pay: $("#ddlPayStatus").val()

                            }

                            if (dataModel.name == '') {
                                Alert("请输入姓名");
                                return;
                            }
                            if (dataModel.phone == '') {
                                Alert("请输入联系电话 ");
                                return;
                            }
                            if (dataModel.email == '') {
                                Alert("请输入邮箱");
                                return;
                            }
                            if (dataModel.sex == '') {
                                Alert("请选择性别");
                                return;
                            }
                            if (dataModel.birthDay == '') {
                                Alert("请输入生日");
                                return;
                            }
                            if (dataModel.amount == '') {
                                Alert("请输入金额");
                                return;
                            }
                            if (dataModel.user_id == '') {
                                Alert("请选择关联用户");
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: "/serv/api/admin/meifan/train/signup/add.ashx",
                                data: { data: JSON.stringify(dataModel) },
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInputAdd').dialog('close');
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
                        $('#dlgInputAdd').dialog('close');
                    }
                }]
            });


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
                        selectUserId = rows[0].UserID;
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
                                //{ field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
                                // { field: 'Postion', title: '职位', width: 100, align: 'left', formatter: FormatterTitle }




                   ]]

               });


            //


        })

        function Search() {

            var fromDate = $("#txtFrom").datebox('getValue');
            var toDate = $("#txtTo").datebox('getValue');
            $('#grvData').datagrid({ url: handlerUrl + "list.ashx", queryParams: { keyword: $("#txtKeyWord").val(), from_date: fromDate, to_date: toDate, activity_name: $("#txtActivityName").val(), activity_type: activityType } });
        }


               function ShowDlg() {

                   var rows = $('#grvData').datagrid('getSelections');

                   if (!EGCheckIsSelect(rows))
                       return;

                   if (!EGCheckNoSelectMultiRow(rows))
                       return;
                   currSelectID = rows[0].OrderId;
                   $('#dlgInput').dialog({ title: '填写评语' });
                   $('#dlgInput').dialog('open');
                   $("#dlgInput input").val("");



               }


               KindEditor.ready(function (K) {
                   editor = K.create('#txtRemark', {
                       uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                       items: [
           'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
           'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
           'insertunorderedlist', '|', 'importword', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                       filterMode: false,
                       width: "100%",
                       height: "300px",
                   });
               });
               function ShowDlgAdd() {


                   $('#dlgInputAdd').dialog({ title: '手动添加' });
                   $('#dlgInputAdd').dialog('open');
                   $("#dlgInputAdd input").val("");



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
                               url: "/serv/api/admin/member/list.ashx",
                               queryParams: { keyword: $(txtTrueNameKeyWord).val() }
                           });
               }



               
               function loadDateList() {

                   var str = new StringBuilder();
                  
                   $.ajax({
                       type: 'post',
                       url: "/serv/api/meifan/train/get.ashx",
                       data: { activity_id: $("#ddlActivity").val() },
                       dataType: "json",
                       success: function (resp) {
                           if (resp.status == true) {
                               console.log();
                               for (var i = 0; i < resp.result.date_list.length; i++) {
                                   str.AppendFormat('<option value="{0}">{0}</option>', resp.result.date_list[i]);

                               }
                              
                               $("#ddlDateRange").html(str.ToString());

                           }
                           else {
                               Alert(resp.msg);
                           }


                       }
                   });


                  



               }
            
                //教练评语
               function showRemarks(orderId) {
                  

                   $.get("/serv/api/admin/meifan/train/signup/get.ashx", { order_id: orderId }, function (data) {

                       layer.open({
                           title: '教练评语',
                           type: 1,
                           skin: 'layui-layer-rim', //加上边框
                           area: ['400px', '400px'], //宽高
                           content: '<div style="text-align: center;padding: 10px;"><style> img{max-width:300px;}</style>' + data.result.Remarks + '</div>'


                       });
  
                   });



    

               }


    </script>
</asp:Content>
