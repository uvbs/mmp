<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LogList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SignIn.LogList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*.panel.window{
                top: 1050px!important;
        }*/
        /*.window{
            top:90px !important;
        }
        .window-shadow{
            top:90px !important;
        }*/
        #go{
            padding-top: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置： 签到管理 > 签到记录><%=model.Address %>
    <span class="l-btn-right pointer" style="float: right; margin-right: 30px;"><span class="l-btn-text icon-back" id="go" style="padding-left: 20px;">返回</span></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <div>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                    onclick="ExportData()">导出签到数据</a>
         
                <%
                    if (!string.IsNullOrEmpty(model.SignInTime))
                    {
                %>

                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                    onclick="EditTime()">编辑时间段</a>&nbsp;
               
                <%
                    }
                    else
                    {
                        %>
                             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                    onclick="EditTime()">添加时间段</a>&nbsp;
                        <%
                    }  
                %>
             <input type="radio" id="rdoGroup"  name="rdoDic" value="1" class="positionTop2" /><label for="rdoGroup">按分组显示</label>
             <input type="radio" id="rdoNoGroup"  class="positionTop2" value="0" name="rdoDic" /><label for="rdoNoGroup">不按分组显示</label>
              
             <span id="show" style="display:none;"><input type="checkbox" id="ckShow" class="positionTop2"  /><label for="ckShow" >签到去重</label></span>
            </div>
          <%--  地址:<input id="txtKeyword" style="width: 200px;" />&nbsp;--%>
            用户名:<input id="txtUserName" readonly="readonly" style="width: 200px;" />&nbsp;
            <input type="hidden" id="txtUserName1" />
            <%
                if (string.IsNullOrEmpty(model.SignInTime))
                {
            %>
            <div style="display: none;">
                状态:
                        <select id="status">
                            <option value="">全部</option>
                            <option value="0">失败</option>
                            <option value="1">成功</option>
                        </select>&nbsp;
            </div>
            <%
                }    
            %>
           
            日期:
             <input class="easyui-datetimebox" id="txtStart"></input>到<input readonly="readonly" class="easyui-datetimebox" id="txtStop">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search()">查询</a>&nbsp;
            <a href="javascript:" style="color: blue;" id="clearValue">清空筛选内容</a>
        </div>
    </div>

    <div id="kefuInfo" class="easyui-dialog" closed="true" modal="true" title="(双击选择)" style="width: 400px;">
        <p style="padding: 10px;">
            关键字<input type="text" id="txtTrueNameValue" style="width: 200px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" id="search">搜索</a>
        </p>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>


    <table id="grvData" fitcolumns="true">
    </table>


    <div id="dlgTime" class="easyui-dialog" closed="true" title="" style="width: 640px; padding: 10px;">
        <div style="margin-top: 5px;">
            <table width="100%" id="tb">
                <tr>
                    <td style="width: 40px;" align="right" valign="middle">名称：
                    </td>
                    <td>
                        <input type="text" style="height: 20px;" name="txtName" placeholder="时间段名称" />
                    </td>
                    <td>开始时间</td>
                    <td style="width: *;" align="left" valign="middle">
                        <input class="easyui-datetimebox" data-easyui-input data-options="width:130,required:true,showSeconds:false" />
                    </td>
                    <td>结束时间</td>
                    <td style="width: *;" align="left" valign="middle">
                        <input class="easyui-datetimebox" data-easyui-input data-options="width:130,required:true,showSeconds:false" />
                    </td>
                    <td>
                        <img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" />
                    </td>
                </tr>
            </table>
            <div style="margin-top: 5px; margin-left: 10px;">
                <a id="addTime" class="button button-rounded button-primary">添加时间段</a>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/SignIn/Log/List.ashx";
        var submitUrl = "/serv/api/admin/signin/address/EditTime.ashx";
        var addressid =<%=addressId%>
       $(function () {
           $(".datebox :text").attr("readonly", "readonly");

           $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { address_id: addressid, is_group: '1' },
                      height: document.documentElement.clientHeight - 112,
                      striped: true,
                      rownumbers: true,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[
                                   {
                                       field: 'head_img_url', title: '微信头像', width: 10, align: 'left',
                                       formatter: function (value, rowDate) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', rowDate["head_img_url"]);
                                           return str.ToString();
                                       }
                                   },
                                   { field: 'wxnick_name', title: '微信昵称', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'name', title: '姓名', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'phone', title: '手机', width: 10, align: 'left', formatter: FormatterTitle },

                                <%  if (tempList.Count > 0)
                                    {
                                        for (int i = 0; i < tempList.Count; i++)
                                        {
                                            %>
                                                  { field: 'time<%=i%>', title: '<%=tempList[i].name%>', width: 15, align: 'left' },
                                                  { field: 'distance<%=i%>', title: '<%=tempList[i].name%>签到距离', width: 15, align: 'left' },
                                             <%
                                        }
                                    }
                                    else
                                    {
                                        %>
                                             { field: 'createtime', title: '签到时间', width: 15, align: 'left' },
                                             { field: 'remark', title: '说明', width: 15, align: 'left', formatter: FormatterWrap },
                                            { field: 'distance', title: '距离(米)', width: 12, align: 'left', formatter: FormatterTitle },
                                        <%
                                    }
                                  %>

                                  //{
                                  //    field: 'status', title: '状态', width: 6, align: 'left',
                                  //    formatter: function (value, rowDate) {
                                  //        return value == "1" ? "<span style='color:green;'>成功</span>" : "<span style='color:red;'>失败</span>";
                                  //    }
                                  //},
                      ]]
                  }
              );


           $("#go").click(function () {
               history.go(-1);
           });
       
            
           <%
            if (string.IsNullOrEmpty(model.SignInTime))
            {
                %>
                    
                   $("input[name=rdoDic]").click(function(){
                       if($(this).val()==0){
                           $("#show").show();
                       }else{
                           $("#show").hide();
                       }
                   })
                   $("#rdoNoGroup").attr("checked","checked");
                   $("#show").show();
                   $("#rdoNoGroup").click(function () {

                       $('#grvData').datagrid(
                           {
                               method: "Post",
                               url: handlerUrl,
                               queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val() },
                               columns: [[
                                    {
                                        field: 'head_img_url', title: '微信头像', width: 10, align: 'left',
                                        formatter: function (value, rowDate) {
                                            var str = new StringBuilder();
                                            str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', rowDate["head_img_url"]);
                                            return str.ToString();
                                        }
                                    },
                                   { field: 'wxnick_name', title: '微信昵称', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'name', title: '姓名', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'phone', title: '手机', width: 10, align: 'left', formatter: FormatterTitle },
                                    { field: 'createtime', title: '签到时间', width: 15, align: 'left' },
                                    { field: 'remark', title: '说明', width: 15, align: 'left', formatter: FormatterWrap },
                                    { field: 'distance', title: '距离(米)', width: 12, align: 'left', formatter: FormatterTitle }


                               ]]

                           });
                   })
                   $("#rdoGroup").click(function(){
                       alert('请先添加时间段！');
                       return;
                   })
                <%
            }
            else
            {
                %>
                 $("#rdoGroup").attr("checked","checked");
           //显示分组
                 $("input[name=rdoDic]").click(function () {
                     var isGroup = $("#rdoGroup").attr("checked");
                     var result = isGroup == 'checked' ? '1' : '';

                     if (result != "1") {

                         $('#grvData').datagrid(
                           {
                               method: "Post",
                               url: handlerUrl,
                               queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val(), is_group: result },
                               columns: [[
                                    {
                                        field: 'head_img_url', title: '微信头像', width: 10, align: 'left',
                                        formatter: function (value, rowDate) {
                                            var str = new StringBuilder();
                                            str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', rowDate["head_img_url"]);
                                            return str.ToString();
                                        }
                                    },
                                   { field: 'wxnick_name', title: '微信昵称', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'name', title: '姓名', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'phone', title: '手机', width: 10, align: 'left', formatter: FormatterTitle },
                                    { field: 'createtime', title: '签到时间', width: 15, align: 'left' },
                                    { field: 'remark', title: '说明', width: 15, align: 'left', formatter: FormatterWrap },
                                    { field: 'distance', title: '距离(米)', width: 12, align: 'left', formatter: FormatterTitle }


                               ]]

                           });

                     }
                     else {


                         $('#grvData').datagrid(
                           {
                               method: "Post",
                               url: handlerUrl,
                               queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val(), is_group: result },
                               columns: [[
                                    {
                                        field: 'head_img_url', title: '微信头像', width: 10, align: 'left',
                                        formatter: function (value, rowDate) {
                                            var str = new StringBuilder();
                                            str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', rowDate["head_img_url"]);
                                            return str.ToString();
                                        }
                                    },
                                   { field: 'wxnick_name', title: '微信昵称', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'name', title: '姓名', width: 10, align: 'left', formatter: FormatterTitle },
                                   { field: 'phone', title: '手机', width: 10, align: 'left', formatter: FormatterTitle },
                                 <%  if (tempList.Count > 0)
                                     {
                                         for (int i = 0; i < tempList.Count; i++)
                                         {
                                            %>
                                                  { field: 'time<%=i%>', title: '<%=tempList[i].name%>', width: 15, align: 'left' },
                                                  { field: 'distance<%=i%>', title: '<%=tempList[i].name%>距离', width: 15, align: 'left' },
                                             <%
                                         }
                                     }
                                     else
                                     {
                                        %>
                                             { field: 'createtime', title: '签到时间', width: 15, align: 'left' },
                                             { field: 'remark', title: '说明', width: 15, align: 'left', formatter: FormatterWrap },
                                            { field: 'distance', title: '距离(米)', width: 12, align: 'left', formatter: FormatterTitle }
                                        <%
                                     }
                                  %>


                         ]]

                     });

               }




           })
                <%
            }
            %>




           //签到去重
           $("#ckShow").click(function(){
               var isShow=$("#ckShow").attr("checked");
               var result = isShow == 'checked' ? '1' : '';
               $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val(), status: $("#status").val(), is_show: result }
                  });
           })



           //单击文本框
           $("#txtUserName").click(function () {
               $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1 } });
               $('#kefuInfo').dialog('open');
               $("#kefuInfo").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });
           });

           $('#grvUserInfo').datagrid(
                {
                    onDblClickRow: function (rowIndex, rowData) {
                        $("#txtUserName").val(rowData["TrueName"]);
                        $("#txtUserName1").val(rowData["UserID"]);
                        $('#kefuInfo').dialog('close');
                    },
                    loadMsg: "正在加载数据",
                    method: "Post",
                    pagination: true,
                    striped: true,
                    pageSize: 10,
                    rownumbers: true,
                    singleSelect: true,
                    height: 380,
                    rowStyler: function () { return 'height:25px'; },
                    columns: [[

                                { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                    ]]

                }
            );

           $("#search").click(function () {
               var txtTrueName = $("#txtTrueNameValue").val();
               $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1, KeyWord: txtTrueName } });
           });

           //清空筛选内容
           $("#clearValue").click(function () {
               $("#txtKeyword").val("");
               $("#txtUserName").val("");
               $("select option[value='']").attr("selected", "selected");
               $("#txtStart").datebox("setValue", "");
               $("#txtStop").datebox("setValue", "");
               $("#txtUserName1").val("");
           });
   
           //删除时间段
           $(".deleteoption").live('click', function () {
               $(this).parent().parent().remove();
           })
           $("#addTime").live('click', function () {
               var str = new StringBuilder();
               str.AppendFormat('<tr>');
               str.AppendFormat('<td style="width: 40px;"  align="right" valign="middle">名称：</td>');
               str.AppendFormat('<td><input type="text" style="height:20px;" name="txtName" placeholder="时间段名称"/></td>');
               str.AppendFormat('<td>开始时间</td>');
               str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" data-options="width:130,required:true" /></td>');
               str.AppendFormat('<td>结束时间</td>');
               str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" data-options="width:130,required:true"/></td>');
               str.AppendFormat('<td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" /> </td>');
               str.AppendFormat('</tr>');
               $("#tb").append(str.ToString());
               $("#tb tr :last .easyui-datetimebox").datetimebox({
                   showSeconds: false
               });
               //只读
               $(".datebox :text").attr("readonly", "readonly");
           });
           


           $("#dlgTime").dialog({
               buttons: [{
                   text: '编辑',
                   handler: function () {
                       var times = [];
                       var result = true;
                       $("#tb tr").each(function () {
                           var td = $(this).find("input[name=txtName]").val();
                           var td1 = $($(this).find(".easyui-datetimebox").get(0)).datetimebox('getValue');
                           var td2 = $($(this).find(".easyui-datetimebox").get(1)).datetimebox('getValue');
                           if (td == '' && td1 == '' && td2 == '') {
                               return true;
                           }
                           if (td == '' || td1 == '' || td2 == '') {
                               result = false;
                               return false;
                           }
                           times.push({ "name": td, "start": td1, "stop": td2 });
                       })
                       if (!result) {
                           alert('请填写完整的时间段.');
                           return;
                       }
                       var dataModel =
                           {
                               times: times.length == 0 ? "" : JSON.stringify(times),
                               address_id:'<%=addressId%>'
                           };
                       $.ajax({
                           type: 'POST',
                           url: submitUrl,
                           data: dataModel,
                           dataType: 'json',
                           success: function (data) {
                               if (data.status) {
                                   window.location.href = "/admin/signin/loglist.aspx?addressid="+'<%=addressId%>';
                                   $("#dlgTime").dialog('close');
                               } else {
                                   Alert(data.msg);
                               }
                           }
                       });
                   }
               },
               {
                   text: '取消',
                   handler: function () {
                       $("#dlgTime").dialog('close');
                   }
               }]
           });



       })
        function FormatterLogTime(value) {
            console.log(value);
            var str = new StringBuilder();
            for (var i = 0; i < value.length; i++) {
                if (i != 0) str.AppendFormat('<br />');
                str.AppendFormat('{2} {0} {1} {3}米', new Date(value[i].create_time).format("yyyy-MM-dd hh:mm"), value[i].remark, value[i].timename, value[i].distance);
            }
            return str.ToString();
        }
        function FormatterWrap(value) {
            if (value == null) {
                return "";
            }
            return "<span style='white-space: pre-wrap;'>" + value + "</span>";
        }
        function Search() {
          
            <%
            if (!string.IsNullOrEmpty(model.SignInTime))
            {
                %>
                 var isShow = '';
                 var isGroup = $("#rdoGroup").attr("checked") == 'checked' ? '1' : '';

                 $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val(), status: $("#status").val(), is_group: isGroup, is_show: isShow }
                   });
                <%
            }
            else
            {
                %>
                var isGroup = $("#rdoGroup").attr("checked") == 'checked' ? '1' : '';
                var isShow = $("#rdoNoGroup").attr("checked");
                var isCheckbox = $("#ckShow").attr("checked");
                var result = '';
                if (isShow == 'checked' && isCheckbox == 'checked') {
                    result = '1';
                }
                $('#grvData').datagrid(
                       {
                           method: "Post",
                           url: handlerUrl,
                           queryParams: { keyword: $("#txtKeyword").val(), address_id: addressid, start: $('#txtStart').datetimebox('getValue'), stop: $('#txtStop').datetimebox('getValue'), user_id: $("#txtUserName1").val(), status: $("#status").val(), is_group: isGroup, is_show: result }
                       });
                <%
            }
            %>
        }
        function ExportData() {
            var start=$('#txtStart').datetimebox('getValue');
            var stop= $('#txtStop').datetimebox('getValue')
            ; var user_id = $("#txtUserName1").val();
            <%
            if (!string.IsNullOrEmpty(model.SignInTime))
            {
                %>
                    var isGroup = $("#rdoGroup").attr("checked") == 'checked' ? '1' : '';
                    var isShow = '';
                    $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                        if (o) {
                            window.open("/serv/api/admin/signin/log/export.ashx?address=" + addressid + "&&is_group=" + isGroup + "&&is_show=" + isShow + "&&start=" + start + "&&stop=" + stop + "&&user_id=" + user_id + "");
                        }
                    });
                <%
            }
            else
            {
                %>
                    var result='';
                    var isGroup = $("#rdoGroup").attr("checked") == 'checked' ? '1' : '';
                    var isShow = $("#rdoNoGroup").attr("checked");
                    var isCheckbox = $("#ckShow").attr('checked');
                    if (isShow == 'checked' && isCheckbox == 'checked') {
                        result = '1';
                    }
                    $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                        if (o) {
                            window.open("/serv/api/admin/signin/log/export.ashx?address=" + addressid + "&&is_group=" + isGroup + "&&is_show=" + result + "&&start=" + start + "&&stop=" + stop + "&&user_id=" + user_id + "");
                        }
                    });
                <%
            }
            %>



        }
        //编辑时间段
        function EditTime() {
            $('#dlgTime').dialog({ title: '修改时间段' });
            $('#dlgTime').dialog('open');
            var signinTimes = '<%=model.SignInTime%>';
            if (signinTimes == '') {
                return;
            }
            $("#tb tr").remove();
            var times = JSON.parse(signinTimes);
            for (var i = 0; i < times.length; i++) {
                var str = new StringBuilder();
                str.AppendFormat('<tr>');
                str.AppendFormat('<td style="width: 40px;"  align="right" valign="middle">名称：</td>');
                str.AppendFormat('<td><input type="text" style="height:20px;" value="{0}" name="txtName" placeholder="时间段名称"/></td>', times[i].name);
                str.AppendFormat('<td>开始时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input value="{0}"  class="easyui-datetimebox" data-options="width:130,required:true" /></td>', times[i].start);
                str.AppendFormat('<td>结束时间</td>');
                str.AppendFormat('<td style="width: *;" align="left" valign="middle"><input  class="easyui-datetimebox" value="{0}" data-options="width:130,required:true"/></td>', times[i].stop);
                str.AppendFormat('<td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption" /> </td>');
                str.AppendFormat('</tr>');
                $("#tb").append(str.ToString());
                $("#tb tr :last .easyui-datetimebox").datetimebox({
                    showSeconds: false
                });
            }
            $(".datebox :text").attr("readonly", "readonly");
        }

    </script>
</asp:Content>
