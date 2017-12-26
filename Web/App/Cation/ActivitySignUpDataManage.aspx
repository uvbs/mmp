<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivitySignUpDataManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ActivitySignUpDataManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #dlgEditData
        {
            max-height: 370px;
            /*overflow: auto !important;*/
            overflow-y: auto !important;
            overflow-x: hidden;
        }
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%--当前位置：&nbsp;<a href="ActivityManage.aspx">活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%Response.Write("\"" + ActivityInfo.ActivityName + "\""); %>的报名数据</span>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px;float:left;">
            <%
                if (!isDelete)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <%
                }     
            %>
            <%--<a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit"
                    plain="true" onclick="ShowGroupSendSms();">发短信</a>--%>
            <%
                if (!isHide)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'"
                id="btnExportToFile">导出到文件</a>
            <%
                }
                else
                {
            %>
            <input type="hidden" id="btnExportToFile" />
            <%
                }
            %>
            <%if (activityType != "DistributionOffLine")
              { %>
            <%-- <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd()">添加</a> --%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEdit()">编辑</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                    plain="true" onclick="ShowAddDlg()">帮会员报名</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-send" plain="true"
                onclick="ShowSendTemplateMsg();">发送微信消息</a> 
            

            <%} %>
             <%if (activityType == "PayActivity")
              { %>
              <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateDealStatus()">批量设置交易完成</a> 

              <%} %>

            <%--  <a href="/App/Cation/ActivitySignUpTableManage.aspx?ActivityID=<%=ActivityID %>" class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">
                字段设置
            </a>--%>
            <%if (activityType == "DistributionOffLine")
              { %>
            <span style="padding-left: 10px; border-left: 1px solid #999;">分销员审核： </span><a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-ok" plain="true" onclick="ShowPass()">通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-cancel" plain="true"
                onclick="ShowNoPass()">不通过</a>
            <%} %>
            <%if (isHideBackBtn == 0)
              { %>
            <%--<a style="float: right;" href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-back" plain="true">返回</a>--%>
            <%} %>

            
        </div>
        <div style="text-align:right;float:right;">
             <a href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true" >返回</a>
        </div>
         <div style="clear:both;"></div>
        <div>
            <%if (activityType == "DistributionOffLine")
              { %>
            状态:
            <select id="status">
                <option value="">全部</option>
                <option value="0">待审核</option>
                <option value="1001">已通过</option>
                <option value="4001">已拒绝</option>
                <option value="4003">微转发审核通过</option>
            </select>
            来源:
            <select id="ddlSource">
                <option value="">全部</option>
                <option value="0">推荐码注册</option>
                <option value="1">微转发</option>
            </select>
            <%} %>


              <%if (activityType == "PayActivity")
              { %>
            交易状态:
            <select id="ddlDealStatusSearch">
                <option value="">全部</option>
                <option value="0">未完成</option>
                <option value="1">交易完成</option>
            </select>
            支付状态:
            <select id="ddlPaymentStatus">
                <option value="">全部</option>
                <option value="0">未付款</option>
                <option value="1">已付款</option>
            </select>
            <%} %>


            姓名:
            <input type="text" style="width: 200px" id="txtKeyWord" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">
                查询</a>
        </div>
    </div>
    <table id="list_data" cellspacing="0" cellpadding="0">
        <thead>
            <tr>
                <th field="ck" width="10" checkbox="true">
                </th>
                <%=Columns %>
                <%if (activityType != "DistributionOffLine")
                  { %>
                <th field="op" width="40" formatter="FormartOpearteBtn" align="center">
                    操作
                </th>
                <%} %>
            </tr>
        </thead>
    </table>
    <div id="dlgEditData" class="easyui-dialog" modal="true" closed="true" style="width: 420px;
        overflow: auto;">
        <table width="94%">
            <%=strEditTable.ToString() %>
        </table>
    </div>
    <div id="dlgNoPass" class="easyui-dialog" modal="true" closed="true" style="width: 420px;
        padding: 20px 10px">
        <table width="98%">
            <tr>
                <td align="right">
                    原因&nbsp;&nbsp;
                </td>
                <td>
                    <textarea class="form-control" style="height: 80px" id="txtNoPassRemark"></textarea>
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
     <div id="dlgSendTemplateMsg" class="easyui-dialog" closed="true" title="发送消息模板" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>类型:
                </td>
                <td>
                    <select id="ddlTemplateType">
                        <option value="notify">通知</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>标题:
                </td>
                <td>
                    <input id="txtTitle" type="text" style="width: 300px;" placeholder="最多输入500个字" />
                </td>
            </tr>
            <tr>
                <td>内容:
                </td>
                <td>
                    <textarea id="txtContent" style="width: 300px;" placeholder="最多输入1000个字"></textarea>
                </td>
            </tr>
            <tr>
                <td>链接:
                </td>
                <td>
                    <input id="txtLink" type="text" style="width: 300px;" placeholder="最多输入500个字符" />
                </td>
            </tr>
        </table>
    </div>

    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var grid;
        //处理文件路径
        var url = "/Handler/Activity/ActivityData.ashx";
        //传入的活动ID
        var ActivityID = "<%=ActivityID%>";
        var LinkName = "<%=LinkName%>";
        var currSelectUid = 0;
        var currMember;
        var activityType = "<%=activityType%>";
        var CurrentAction = "EditActivityData";

        var userGrid;
        //加载文档
        jQuery().ready(function () {

            //$(window).resize(function () {
            //    var width = document.body.clientWidth - 15;
            //    var height = document.documentElement.clientHeight - 170;
            //    $('#list_data').datagrid('resize', {
            //        width: width,
            //        height: height
            //    });
            //});
          
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: url,
                height: document.documentElement.clientHeight - 112,
                
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "Query", ActivityID: ActivityID, SearchTitle: "", LinkName: LinkName }

            });
            //            //取消---------------------
            //            $("#win").find("#btnWinClose").bind("click", function () {
            //                $("#win").window("close");
            //            });
            //            //取消---------------------

            $(btnExportToFile).click(function () {

                DownLoadData();
            });

            $('#dlgEditData').dialog({
                title: '信息',
                buttons: [{
                    text: "保存",
                    handler: function () {
                        $.messager.progress({ text: '正在处理...' });
                        var model = {
                            <%=strSaveEditAssignment.ToString() %>
                            Action: CurrentAction,
                            ActivityID: ActivityID,
                            UID: currSelectUid
                        }

                        $.ajax({
                            type: 'post',
                            url: url,
                            data: model,
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    //Show('操作成功');
                                    $('#dlgEditData').dialog('close');
                                    grid.datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        }
                        );
                    }
                },
                {
                    text: "取消",
                    handler: function () {
                        $('#dlgEditData').dialog('close');
                    }
                }
                ]

            });

            $("#ddlSource").change(function(){
            
            Search();



            
            })

            $("#ddlDealStatusSearch").change(function(){
            
            Search();



            
            })
            
            $("#ddlPaymentStatus").change(function(){
            
            Search();



            
            })


            //dlgDistributionOffLineApply

            //            $('#digGroupSendSms').dialog({
            //                title: '群发短信：每个活动可以免费发送两次群发短信',
            //                buttons: [{
            //                    text: "发送",
            //                    handler: function () {
            //                        $.messager.progress({text:'正在处理。。。'});

            //                        var model = {
            //                            Action:"GroupSendSms",
            //                            ActivityID:ActivityID,
            //                            SmsContent:$('#txtSendSmsContent').val()
            //                        }

            //                        $.ajax({
            //                            type:'post',
            //                            url:'/Handler/Activity/ActivityHandler.ashx',
            //                            data:model,
            //                            dataType:"json",
            //                            success:function(resp){
            //                            $.messager.progress('close');
            //                                    if(resp.Status == 1)
            //                                    {
            //                                        Alert(resp.Msg);
            //                                        $('#digGroupSendSms').dialog('close');
            //                                    }
            //                                    else
            //                                    {
            //                                         Alert(resp.Msg);
            //                                    }
            //                                }
            //                            }
            //                        );

            //                    }
            //                },
            //                {
            //                    text: "取消",
            //                    handler: function () {
            //                        $('#digGroupSendSms').dialog('close');
            //                    }
            //                }
            //                ]

            //            });

                     
        userGrid= $('#grvUserData').datagrid(
	            {
	                method: "Post",
	                url: "/Handler/App/CationHandler.ashx?Action=QueryWebsiteUser&HaveTrueName=1",
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
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
                                { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                 { field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'Postion', title: '职位', width: 100, align: 'left', formatter: FormatterTitle }




	                ]]

	            });

        //设置分销员
        $('#dlgUser').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                    AddSignUpData();
                           

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUser').dialog('close');
                    }
                }]
            });



        });

        ///发送微信模板消息
        $('#dlgSendTemplateMsg').dialog({
            buttons: [{
                text: '发送',
                handler: function () {
                    var rows = $('#list_data').datagrid('getSelections');
                    var dataModel = {
                        Action: "SendTemplateMsg",
                        TemplateType: $(ddlTemplateType).val(),
                        Title: $(txtTitle).val(),
                        Content: $(txtContent).val(),
                        Url: $(txtLink).val(),
                        UserAutoIds: GetRowsIds(rows).join(',')

                    }

                    if (dataModel.Title == '') {
                        Alert("标题不能为空!");
                        return;
                    }
                    if (dataModel.Content == '') {
                        Alert("内容不能为空!");
                        return;
                    }
                    if (dataModel.Title.length >= 500) {
                        Alert("标题不能超过500个字!");
                        return;
                    }
                    if (dataModel.Content.length >= 1000) {
                        Alert("内容不能超过1000个字!");
                        return;
                    }
                    if (dataModel.Url.length >= 500) {
                        Alert("链接不能超过500个字符!");
                        return;
                    }
                    $.ajax({
                        type: 'post',
                        url: '/Handler/App/CationHandler.ashx',
                        data: dataModel,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $('#dlgSendTemplateMsg').dialog('close');
                                Alert(resp.Msg);
                                $('#list_data').datagrid('reload');
                            }
                            else {
                                Alert(resp.Msg);
                            }


                        }
                    });

                }
            }, {
                text: '取消',
                handler: function () {
                    $('#dlgSendTemplateMsg').dialog('close');
                }
            }]
        });
        //            //群发短信
        //        function ShowGroupSendSms(){
        //        $.messager.progress({text:'正在处理。。。'});
        //        //检查是否已经进行手机验证
        //        $.ajax({
        //                type: 'post',
        //                url: '/Handler/User/UserHandler.ashx',
        //                data: { Action: 'GetUserInfo' },
        //                dataType:"json",
        //                success: function (resp) {
        //                    $.messager.progress('close');
        //                    if (resp.Status == 1) {
        //                        currMember = resp.ExObj;

        //                        if (currMember.IsPhoneVerify == 1) {
        //                             //检查发送是否超过次数
        //                            $('#txtSendSmsContent').val('{姓名}您好，');
        //                            $.ajax({
        //                                type:'post',
        //                                url:'/Handler/Activity/ActivityHandler.ashx',
        //                                data:{Action:'GetActivityInfo',ActivityID:ActivityID},
        //                                dataType:"json",
        //                                success:function(resp){
        //                                        if (resp.Status == 0) {
        //                                            Alert(resp.Msg);
        //                                            return;
        //                                        }

        //                                        if (resp.ExObj.GroupSendSmsCount < 2) {
        //                                            $('#digGroupSendSms').dialog({title:'群发短信：该活动还可以群发短信' + (2 - resp.ExObj.GroupSendSmsCount) + "次"});
        //                                            $('#digGroupSendSms').dialog('open');
        //                                        }
        //                                        else {
        //                                            Alert("该活动两次免费群发资格已用完！");
        //                                        }
        //                                }
        //                            });

        //                        }
        //                        else {
        //                            $.messager.confirm('系统提示','您还未进行手机验证，手机验证通过后才能群发短信，现在去验证？',function(r){
        //                                if(r)
        //                                {
        //                                    window.location.href = '/fshare/user/SafetyCenter.aspx';
        //                                }
        //                            });    
        //                        }
        //                    }
        //                }
        //            });


        //           
        //        }



        // 删除---------------------
        function Delete() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要删除的记录");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID);
            }
            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Delete", ActivityID: ActivityID, ids: ids.join(',') },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                messager('系统提示', "删除成功！");
                                grid.datagrid('reload');

                            }
                            else {
                                $.messager.alert("删除失败。");
                            }

                        }
                    });
                }
            });
        };
        //通过审核
        function ShowPass()
        {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要通过的记录");
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID);
            }
            $.messager.confirm("系统提示", "是否确定通过选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "ActivityDataByIsPass", ActivityID: ActivityID, UID: ids.join(','), Status: 1001 },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                messager('系统提示', "操作成功！");
                                grid.datagrid('reload');
                            }
                            else {
                                $.messager.alert("操作出错",resp.Msg);
                            }

                        }
                    });
                }
            });
        }
        //不通过审核
        function ShowNoPass() {
         
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要操作的记录");
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID);
            }
            $('#txtNoPassRemark').focus();
            $('#dlgNoPass').dialog({
                title: '审核不通过',
                buttons: [{
                    text: "确定",
                    handler: function () {
                        var remarks = $('#txtNoPassRemark').val().trim();

                        if (!remarks) {
                            $('#txtNoPassRemark').focus();
                            return;
                        }

                        jQuery.ajax({
                            type: "Post",
                            url: url,
                            data: { Action: "ActivityDataByIsPass", ActivityID: ActivityID, UID: ids.join(','), Status: 4001, Remarks: remarks },
                            dataType: 'json',
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    messager('系统提示', "操作成功！");
                                     $('#dlgNoPass').dialog('close');
                                    grid.datagrid('reload');
                                }
                                else {
                                    $.messager.alert("操作出错。",resp.Msg);
                                }

                            }
                        });
                    }
                },
                {
                    text: "取消",
                    handler: function () {
                        $('#dlgNoPass').dialog('close');
                    }
                }
                ]

            });
            $('#dlgNoPass').dialog('open');
            //$.messager.confirm("系统提示", "是否确定不通过选中信息?", function (r) {
            //    if (r) {
            //        jQuery.ajax({
            //            type: "Post",
            //            url: url,
            //            data: { Action: "ActivityDataByIsPass", ActivityID: ActivityID, UID: ids.join(','), Status: 4001 },
            //            dataType: 'json',
            //            success: function (resp) {
            //                if (resp.Status == 1) {
            //                    messager('系统提示', "操作成功！");
            //                    grid.datagrid('reload');
            //                }
            //                else {
            //                    $.messager.alert("操作出错。",resp.Msg);
            //                }

            //            }
            //        });
            //    }
            //});
        }
        // 删除---------------------


        //        //显示发送短信窗口
        //        function ShowSMS() {
        //            var rows = grid.datagrid('getSelections');
        //            var num = rows.length;
        //            if (num == 0) {
        //                messager("系统提示", "请选择您要发送短信的手机号码");
        //                return;
        //            }
        //            //           var strsendsmslist="";
        //            var phonelist = [];
        //            //           var namelist=[];
        //            for (var i = 0; i < rows.length; i++) {
        //                phonelist.push(rows[i].Phone);
        //                //               namelist.push(rows[i].Name);
        //                //               strsendsmslist+=rows[i].Phone+" "+rows[i].Name+"</br>";
        //            }

        //            //           $("#lblPhoneList").html("");
        //            //           $("#txtSMSContent").val("");

        //            //            //显示窗体
        //            //            $("#win").window({
        //            //                title: "发送短信",
        //            //                closed: false,
        //            //                collapsible: false,
        //            //                minimizable: false,
        //            //                maximizable: false,
        //            //                iconCls: "icon-add",
        //            //                resizable: false,
        //            //                width: 400,
        //            //                height: 300,
        //            //                top: ($(window).height() - 400) * 0.5,
        //            //                left: ($(window).width() - 400) * 0.5
        //            //            });

        //            //            $("#lblPhoneList").html(strsendsmslist);
        //            //            
        //            jQuery.ajax({
        //                type: "Post",
        //                url: url,
        //                data: { Action: "SendSMS", PhoneList: phonelist.join('\n') },
        //                success: function (result) {
        //                    if (result == "true") {
        //                        window.location = '/SMS/Send.aspx';


        //                    }
        //                    else {
        //                        $.messager.alert(result);
        //                    }

        //                }
        //            });

        //        }

        //        //发送短信
        //        function SendSMS(){
        //                var SMSContent=$("#txtSMSContent").val();
        //                if (SMSContent=="") {
        //               
        //                $("#txtSMSContent").focus();
        //                return false;
        //        }
        //           var rows = grid.datagrid('getSelections');
        //           var num = rows.length;
        //           if (num == 0) {
        //               messager("系统提示", "请选择您要发送短信的记录");
        //               return false;
        //           }
        //           var phonelist = [];
        //          
        //           for (var i = 0; i < rows.length; i++) {
        //               phonelist.push(rows[i].Phone);
        //            
        //           }

        //              $.messager.confirm("系统提示", "确定发送短信?", function (r) {
        //               if (r) {
        //                   jQuery.ajax({
        //                       type: "Post",
        //                       url: url,
        //                       data: { Action: "SendSMS", PhoneList: phonelist.join(','), SMSContent:SMSContent},
        //                       success: function (result) {
        //                           if (result=="true") {
        //                             $("#win").window("close");
        //                              
        //                               
        //                           }
        //                           else {
        //                                $.messager.alert(result);
        //                        }
        //                          
        //                       }
        //                   });
        //               }
        //           });


        //        }



        function DownLoadData() {
            $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
                if (o) {
                    window.open(url + '?Action=DownLoadActivityData&&type='+activityType+'&ActivityID=' + ActivityID);
                }
            });
        }

        function FormartOpearteBtn(value, row, index) {

            var result = new StringBuilder();

            result.AppendFormat('<a href="javascript:;" onclick="ShowEditSingel({0},{1})" ><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>', row.ActivityID, row.UID);

            return result.ToString();
        }
        //格式化状态
        function FormartStatus(value, row, index) {

            var result = new StringBuilder();

            if (value == 0) {
                result.AppendFormat('<span style="color:#999;">待审核<span>');
            }
            else if (value == 1 || value == 1001||value==4003) {
                result.AppendFormat('<span style="color:green;">通过<span>');
            }
            else if (value == 2) {
                result.AppendFormat('<span style="color:blue;">通过返金<span>');
            }
            else if(value == -1 || value == 4001) {
                result.AppendFormat('<span style="color:red;">未通过<span>');
            }
            else if (value == -2) {
                result.AppendFormat('<span style="color:red;">未过返金<span>');
            }
            
            return result.ToString();
        }
          //格式化付款状态
          function FormartPaymentStatus(value, row, index) {
            var result = new StringBuilder();
            if(value ==0) {
                result.AppendFormat('<span style="color:red;">未付款<span>');
            }
            else if (value == 1) {
                result.AppendFormat('<span style="color:Green;">已付款<span>');
            }
            
            return result.ToString();
        }
          //格式化交易状态
          function FormartDealStatus(value, row, index) {
            var result = new StringBuilder();
            if(value ==0) {
                result.AppendFormat('<span >未完成<span>');
            }
            else if (value == 1) {
                result.AppendFormat('<span style="color:Green;">交易完成<span>');
            }
            
            return result.ToString();
        }

        //格式化来源
        function FormartSource(value, row, index) {
            var result = new StringBuilder();

             if (value!=""&&value!=null) {
                result.AppendFormat('<span>微转发<span>');
            }
                        else  {
                result.AppendFormat('<span>推荐码注册<span>');
            }
            return result.ToString();
        }
        function ShowEditSingel(ActivityID, UID) {
            //Alert("ActivityID:" + ActivityID + "  UID:" + UID);
            currSelectUid = UID;
            $.ajax({
                type: 'post',
                url: url,
                data: { Action: 'GetActivityData', ActivityID: ActivityID, UID: UID },
                dataType: "json",
                success: function (resp) {
                    if (resp != "") {
                        <%=strShowEditAssignment.ToString() %>
                }
            }
        });
        CurrentAction = "EditActivityData";
        $('#dlgEditData').dialog('open');
    }

        function ShowEdit() {
        var rows = $('#list_data').datagrid('getSelections'); //获取选中的行
        if (!EGCheckNoSelectMultiRow(rows)) {
            return;
        }
        currSelectUid = rows[0].UID;
        $.ajax({
            type: 'post',
            url: url,
            data: { Action: 'GetActivityData', ActivityID: ActivityID, UID: currSelectUid },
            dataType: "json",
            success: function (resp) {
                if (resp != "") {
                    <%=strShowEditAssignment.ToString() %>
                    }
                }
            });
            CurrentAction = "EditActivityData";
            $('#dlgEditData').dialog('open');
        }
        function ShowAdd() {
            CurrentAction = "AddActivityData";
            $("#dlgEditData input").val("");
            $('#dlgEditData').dialog('open');
        }

         function ShowAddDlg() {
            
            $('#dlgUser').dialog('open');
        }

       function UpdateDealStatus() {
            
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择记录");
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID);
            }
            $.messager.confirm("系统提示", "是否确定修改选中记录的交易状态为 交易完成?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "UpdateDealStatus", ActivityID: ActivityID, UID: ids.join(',') },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                                messager('系统提示', "操作成功！");
                                grid.datagrid('reload');
                            }
                            else {
                                $.messager.alert("操作出错",resp.Msg);
                            }

                        }
                    });
                }
            });

           
        }

        function Search() {
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: url,
                pageSize: 20,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "Query", ActivityID: ActivityID, SearchTitle: $("#txtKeyWord").val(), LinkName: LinkName,Status:$("#status").val(),Source: $("#ddlSource").val(),PaymentStatus:$("#ddlPaymentStatus").val(),DealStatus:$("#ddlDealStatusSearch").val()}

            });



        }


        //搜索
        function SearchUser() {
            $('#grvUserData').datagrid(
                    {
                        method: "Post",
                        url: "/Handler/App/CationHandler.ashx?Action=QueryWebsiteUser&HaveTrueName=1",
                        queryParams: { keyword: $(txtTrueNameKeyWord).val() }
                    });
        }


        function AddSignUpData(){
        
            var rows = userGrid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择至少一个用户");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);
            }

                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Add", ActivityID: ActivityID, Ids: ids.join(',') },
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.Status == 1) {
                               $('#dlgUser').dialog('close');
                                messager('系统提示', "已成功报名"+resp.ExInt+"人");
                                grid.datagrid('reload');


                            }
                            else {
                               Alert(resp.Msg);
                            }

                        }
                    });
        }

        //发送微信消息模板
        function ShowSendTemplateMsg() {
            var rows = $('#list_data').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgSendTemplateMsg').dialog({ title: '发送微信消息' });
            $('#dlgSendTemplateMsg').dialog('open');
        }

        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].AutoID>0) {
                    ids.push(rows[i].AutoID);
                }
            }
            return ids;
        }
    </script>
</asp:Content>
