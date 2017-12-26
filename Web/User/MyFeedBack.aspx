<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="MyFeedBack.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.MyFeedBack" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var grid;
     
        //处理文件路径
        var url = "/Handler/FeedBack/FeedBackHandler.ashx";

        var currentuserId="<%=currentUserID %>";
        //加载文档
        jQuery().ready(function () {



            $(window).resize(function () {
                $("#list_data").datagrid('resize',
	            {
	                width: document.body.clientWidth,
	                height: document.documentElement.clientHeight - 55
	            });
            });



            //-----------------加载gridview
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: url,
                width: document.body.clientWidth,
                height: document.documentElement.clientHeight - 55,
                pageSize: 20,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "QueryFeedBackPerson" }
            });
            //------------加载gridview




            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var searchTitle = $("#txtTitle").val();
                var feedbackStatus = $("#ddlFeedBackStatus").val();
                var feedbackType = $("#ddlFeedBackType").val();
                var feedbackPlatformCategory = $("#ddlPlatformCategory").val();
                var targetUser = $("#ddltargetUser").val();
                grid.datagrid({ url: url, queryParams: { Action: "QueryFeedBackPerson", SearchTitle: searchTitle, FeedBackStatus: feedbackStatus, FeedbackType: feedbackType, FeedbackPlatformCategory: feedbackPlatformCategory, targetUser: targetUser} });
            });
            //搜索结束---------------------

            $("#ddlFeedBackStatus").change(function () {

                $("#btnSearch").click();


            });
            $("#ddlFeedBackType").change(function () {

                $("#btnSearch").click();


            });
            $("#ddlPlatformCategory").change(function () {

                $("#btnSearch").click();


            });
            $("#ddltargetUser").change(function () {

                $("#btnSearch").click();


            });
    //---处理中
    $("#btnProcessing").click(function () {

                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    $.messager.alert("系统提示", "请选择您要操作的问题", "warning");
                    return;
                }

                var feedbackids = [];

                for (var i = 0; i < rows.length; i++) {

                    if ((rows[i].FeedBackStatus == "待处理") || (rows[i].FeedBackStatus == "再开放")) {
                        if (rows[i].AssignationUserID == currentuserId) {
                             feedbackids.push(rows[i].FeedBackID);
                        }
                        else {
                             $.messager.alert("系统提示", " 只有指派给我的问题才能修改问题状态", "warning");
                             return;
                        }
                       
                    }
                    else {
                        $.messager.alert("系统提示", "只有问题状态为 '待处理'或 '再开放' 才能修改状态为 '处理中'", "warning");
                        return;
                    }



                }
                //-----
                $.messager.confirm("系统提示", "确定将选中问题的状态更改为 处理中?", function (r) {
                    if (r) {
                    jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "UpdateFeedBackStatus", FeedBackIDs: feedbackids.join(','), FeedBackStatus:"处理中" },
                    success: function (result) {
                        $.messager.alert("系统提示", result, "info");
                        grid.datagrid('reload');

                    }
                });
                    }
                });




    });

    //-----已处理
    $("#btnProcessCompleted").click(function () {

        var rows = grid.datagrid('getSelections');
        var num = rows.length;
        if (num == 0) {
            $.messager.alert("系统提示", "请选择您要操作的问题", "warning");
            return;
        }

        var feedbackids = [];

        for (var i = 0; i < rows.length; i++) {

            if ((rows[i].FeedBackStatus== "处理中")) {

                if (rows[i].AssignationUserID == currentuserId) {
                    feedbackids.push(rows[i].FeedBackID);
                }
                else {
                    $.messager.alert("系统提示", " 只有指派给我的问题才能修改问题状态", "warning");
                    return;
                }
            }
            else {
                $.messager.alert("系统提示", "只有问题状态为 '处理中' 才能修改状态为 '已处理'", "warning");
                return;
            }



        }
        //-----
        $.messager.confirm("系统提示", "确定将选中问题的状态更改为 已处理?", function (r) {
            if (r) {
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "UpdateFeedBackStatus", FeedBackIDs: feedbackids.join(','), FeedBackStatus: "已处理" },
                    success: function (result) {
                        $.messager.alert("系统提示", result, "info");
                        grid.datagrid('reload');

                    }
                });
            }
        });




    });

    //------关闭
    $("#btnClosedFeedback").click(function () {

        var rows = grid.datagrid('getSelections');
        var num = rows.length;
        if (num == 0) {
            $.messager.alert("系统提示", "请选择您要操作的问题", "warning");
            return;
        }

        var feedbackids = [];

        for (var i = 0; i < rows.length; i++) {

            if ((rows[i].FeedBackStatus == "已处理")) {
                if (rows[i].UserID !=currentuserId) {
                    $.messager.alert("系统提示", "只有问题的提交人才能更改问题状态为 已关闭", "warning");
                    return;
                }

                feedbackids.push(rows[i].FeedBackID);



            }
            else {
                $.messager.alert("系统提示", "只有问题状态为 '已处理' 才能修改状态为 '已关闭'", "warning");
                return;
            }



        }
        //-----
        $.messager.confirm("系统提示", "确定将选中问题的状态更改为 已关闭?", function (r) {
            if (r) {
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "UpdateFeedBackStatus", FeedBackIDs: feedbackids.join(','), FeedBackStatus: "已关闭" },
                    success: function (result) {
                        $.messager.alert("系统提示", result, "info");
                        grid.datagrid('reload');

                    }
                });
            }
        });




    });

    //-----再开放问题
    $("#btnReProcessing").click(function () {
        var rows = grid.datagrid('getSelections');
        var num = rows.length;
        if (num == 0) {
            $.messager.alert("系统提示", "请选择您要操作的问题", "warning");
            return;
        }

        var feedbackids = [];

        for (var i = 0; i < rows.length; i++) {

            if ((rows[i].FeedBackStatus == "已处理")) {
                if (rows[i].UserID != currentuserId) {
                    $.messager.alert("系统提示", "只有问题的提交人才能更改问题状态为 再开放", "warning");
                    return;
                }

                feedbackids.push(rows[i].FeedBackID);
            }
            else {
                $.messager.alert("系统提示", "只有问题状态为 '已处理' 才能修改状态为 '再开放'", "warning");
                return;
            }


        }
        //-----
        $.messager.confirm("系统提示", "确定将选中问题的状态更改为 '再开放'?", function (r) {
            if (r) {
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "UpdateFeedBackStatus", FeedBackIDs: feedbackids.join(','), FeedBackStatus: "再开放" },
                    success: function (result) {
                        $.messager.alert("系统提示", result, "info");
                        grid.datagrid('reload');

                    }
                });
            }
        });




    });


    });


        function formarttitle(value, row) {


            return "<a target='_blank' href=/FeedBack/FeedBackReply.aspx?id=" + row.FeedBackID + " title=" + value + ">" + value + "</a>";

        }


    </script>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
     <div class="pageTopBtnBgSmall">
    <a href="#" class="easyui-linkbutton" iconcls="icon-edit" id="btnProcessing" plain="true" >
                                            更改问题状态为处理中</a>
   <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnProcessCompleted" >
                                            更改问题状态为已处理</a>
      <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnClosedFeedback" >
                                            更改问题状态为已关闭</a>
   <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnReProcessing" >
                                            更改问题状态为再开放</a>
                                            <br />
    <span style="font-size: 12px;margin-left:10px;"> 筛选:</span>
    <select id="ddltargetUser" name="ddltargetUser" style="width:100px;height:20px;">
    <option value="">全部</option>
    <option value="0">我提交的</option>
    <option value="1">指派给我的</option>
   
    </select>              
                           

         <span style="font-size: 12px"> 问题状态:</span>
                            <select id="ddlFeedBackStatus" name="ddlFeedBackStatus" style="width:100px;height:20px;">
    <option value="">全部</option>
    <option value="待处理">待处理</option>
    <option value="处理中">处理中</option>
    <option value="已处理">已处理</option>
    <option value="再开放">再开放</option>
    <option value="已关闭">已关闭</option>
    </select>

                           <span style="font-size: 12px"> 问题类型:</span>
                            <select id="ddlFeedBackType" name="ddlFeedBackType" style="width:100px;height:20px;">
    <option value="">全部</option>
    <option value="投诉建议">投诉建议</option>
    <option value="Bug报告">Bug报告</option>
    <option value="功能建议">功能建议</option>
    <option value="寻求帮助">寻求帮助</option>
    </select>
    <span style="font-size: 12px"> 所属模块:</span>
    <select id="ddlPlatformCategory" name="ddlPlatformCategory" style="width:100px;height:20px;">
     <option value="">全部</option>
    <option value="微信平台">微信平台</option>
    <option value="微博平台">微博平台</option>
    <option value="短信平台">短信平台</option>
    <option value="邮件平台">邮件平台</option>
    <option value="活动平台">活动平台</option>
    <option value="客户管理平台">客户管理平台</option>
    <option value="个人中心">个人中心</option>
   
    </select>
    

                            <span style="font-size: 12px"> 关键字:</span>
                            <input type="text" id="txtTitle" style="width:200px;" />
                            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>

                           
                           
                 
            </div>
       
    <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        </th>
                        <th field="Title"   formatter="formarttitle"  width="40">
                            标题
                        </th>
                        <th field="SubmitDate" formatter="FormatDate" width="40">
                            提交日期
                        </th>

                        <th field="UserID" width="40">
                            提交人
                        </th>    
                        <th field="FeedBackType" width="40">
                            问题类型
                        </th>  
                        <th field="PlatformCategory" width="40">
                            所属模块
                        </th>  
                        <th field="Phone" width="40">
                            手机号码
                        </th>  
                        <th field="Email" width="40">
                            Email
                        </th>  
                        <th field="FeedBackStatus" width="40">
                            状态
                        </th>  
                        <th field="AssignationUserID" width="40">
                            处理人
                        </th>
                      
                    </tr>
                </thead>
            </table>
   
    </div>
</asp:Content>
