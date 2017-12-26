<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="Workbench.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.Workbench" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            background: none;
        }
        td, th
        {
            border: 0px solid #e8e7e1;
            vertical-align: top;
        }
        td a
        {
            text-decoration: underline;
            color: Black;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var url = "/Handler/User/UserRemindManage.ashx";
        //定时检查待办事项
        setInterval(GetRemindList, 60000);

        $(function () {

            GetRemindList();
            $("div[group=remindgroup]>span").live("click", function () {
                UpdateRemindState($(this).attr("remindid"));
            });
            $("div[group=remindgroup]>a").live("click", function () {

                GetSingleRemindInfo($(this).attr("remindid"));
            });
            $(".panel-title>span").live("click", function () {

                ShowAdd();
            });
            //取消---------------------
            $("#win_remindadd").find("#btnExit").bind("click", function () {
                $("#win_remindadd").window("close");
            });
            //取消---------------------

            //保存---------------------
            $("#btnSave").bind("click", function () {
               Add();
            });
            //保存---------------------




        });
       //获取提醒列表
        function GetRemindList() {

            try {


                $.post(url, { Action: "GetRemindList" }, function (result) {
                  
                    $("#rw").html(result);


                })
        }
        catch (e) {
            alert(e);
            }


    }
    //更新提示状态
    function UpdateRemindState(id) {

        $.post(url, { Action: "UpdateRemindState", RemindID: id, IsEnable: "0" }, function (result) {
            if (result == "true") {
                $("[remindid =" + id + "]").parent().remove();
            }


        })

    }
    
    //获取单条提醒信息
    function GetSingleRemindInfo(id) {
        try {

            $.getJSON(url, { Action: "GetSingleRemindInfo", Id: id }, function (data) {

                $("#lbl_title").text(data.Title);
                $("#sp_content").html(data.Content);
                var remindtime = FormatDate(data.RemindTime);
                $("#lbl_time").text(remindtime);
                //弹出对话框
                $("#win_remindinfo").window({
                    title: "详细信息",
                    closed: false,
                    collapsible: false,
                    minimizable: false,
                    maximizable: false,
                    iconCls: "icon-edit",
                    resizable: false,
                    width: 350,
                    height: 300,
                    top: ($(window).height() - 300) * 0.5,
                    left: ($(window).width() - 350) * 0.5

                });
                //弹出对话框

            });


        }
        catch (e) {
            alert(e);
        }
    
    }

    function SetThemeText(obj) {

        var text = $(obj).text();
        $("#txtTheme").val(text)


    }
    //弹出添加或编辑框开始
    function ShowAdd() {
        var title = ""; //弹出框标题
        var titleicon = "icon-add"; //弹出框标题图标
        //弹出添加框开始
            //清除数据
              ClearAll();
              $("#rd_phone").attr("checked", true);
              $("#txtTheme").val("打电话");
              title = "添加提醒";

        //弹出对话框
        $("#win_remindadd").window({
            title: title,
            closed: false,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            iconCls: titleicon,
            resizable: false,
            width: 400,
            height: 300,
            top: ($(window).height() - 300) * 0.5,
            left: ($(window).width() - 400) * 0.5

        });
        //弹出对话框



    }
    //展示添加或编辑框结束

    //添加提醒
    function Add() {
        var model = GetModelInfo();
        if (model.Title == "") {
            $("#txtTheme").focus();
            return false;
        }

        if (model.RemindTime == "") {
            messager("系统提示", "请选择提醒时间");
            return false;
        }
        //------------添加
        jQuery.ajax({
            type: "Post",
            url: url,
            data: { Action: "Add", JsonData: JSON.stringify(model).toString() },
            success: function (result) {
                if (result == "true") {
                    messager("系统提示", "添加成功");
                    $("#win_remindadd").window("close");
                    GetRemindList();
                } else {
                    messager("系统提示", result);
                }
            }
        });

        //添加提醒



    }


    //获取提醒数据
    function GetModelInfo() {


        var model =
            {

                "Title": $("#txtTheme").val(),
                "Content": $("#txtContent").val(),
                "RemindTime": $("#txtRemindTime").datetimebox("getValue"),
                "IsEnable": 1
            }

        return model;
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="box">
        <span style="font-size:14px;font-weight: bold;">
            <% ZentCloud.BLLJIMP.Model.UserInfo user = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel(); %>
            您好,<span style="color: #2a7aca; font-weight: bold; cursor: hand;"><%=user == null? "": user.UserID %></span>
            欢迎您! 短信余额： <span style="color: #2a7aca; font-weight: bold; cursor: hand;">
                <%=user == null? "0" : user.Points.ToString() %>
            </span>邮件余额： <span style="color: #2a7aca; font-weight: bold; cursor: hand;">
                <%=user == null? "0": user.EmailPoints.ToString() %>
            </span><span style="font-weight: bold; cursor: hand;"><a href="/User/ChangePwd.aspx">
                【修改密码】</a></span>
        </span>
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="width: 50%;">
            </td>
            <td style="width: 50%;">
            </td>
        </tr>
        <tr>
            <td style="width: 50%;">
                <div id="gg" class="easyui-panel" title="公告" width="100%" style="height: 200px; padding: 10px;
                    background: #fafafa;" collapsible="true">
                    <p>
                        <%--<a href="javascript:;">聚比特整合营销新版将于2012年12月24日正式上线</a>--%>
                    </p>
                </div>
            </td>
            <td style="width: 50%;">
                <div id="tj" class="easyui-panel" title="统计信息" width="100%" style="height: 200px;
                    padding: 10px; background: #fafafa;" collapsible="true">
                    <p>
                        本月共发送短信：<a href="javascript:;"><%= user == null? "0" : user.GetCurrMonthSendSmsTotalCount().ToString() %></a>条</p>
                    <p>
                        本月共发送邮件：<a href="javascript:;"><%= user == null? "0" : user.GetCurrMonthSendEdmTotalCount().ToString() %></a>封</p>
                    <%--<p>
                        本月新增微博粉丝：<a href="javascript:;">732</a>个</p>--%>
                </div>
            </td>
        </tr>
        <p>
        </p>
        <tr>
            <td style="width: 50%;">
                <div id="xx" class="easyui-panel" title="我的信箱" width="100%" style="height: 200px;
                    padding: 10px; background: #fafafa;" collapsible="true">
                </div>
            </td>
            <td style="width: 50%;">
                <div id="rw" class="easyui-panel" title="待办任务 <span>添加</span>" width="100%" style="height: 200px;
                    padding: 10px; background: #fafafa;" collapsible="true">
                   

                </div>
            </td>
        </tr>
    </table>

   <div id="win_remindinfo" class="easyui-window" modal="true" closed="true">
   <div style="margin-left:20px">
   <table>
   <tr><td>主题:</td><td>
   <label id="lbl_title"></label>
   
   </td></tr>
    <tr><td>备注:</td><td> <span id="sp_content"></span></td></tr>
     <tr><td>时间:</td><td> <label id="lbl_time"></label></td></tr>
      <tr><td></td><td></td></tr>
   </table>
   

   </div>
   
   </div>

    <div id="win_remindadd" class="easyui-window" modal="true" closed="true">
               
                    <div style="margin-left:20px">
                                 <table  >

                      
<tr><td style="text-align:right">主题:</td><td>
<input type="text" id="txtTheme" style="width:250px;"class="easyui-validatebox" required="true" missingmessage="请输入主题名称" />


</td></tr>
<tr><td></td><td>
<input type="radio" name="rdgroup" id="rd_phone"  /><label for="rd_phone" onclick="SetThemeText(this)">打电话</label>
<input type="radio" name="rdgroup" id="rd_sms"  /><label for="rd_sms" onclick="SetThemeText(this)">发短信</label>
<input type="radio" name="rdgroup" id="rd_email"  /><label for="rd_email" onclick="SetThemeText(this)">发邮件</label>
<input type="radio" name="rdgroup" id="rd_other"  /><label for="rd_other" onclick="SetThemeText(this)">其它</label>
</td></tr>
<tr><td style="text-align:right">备注：</td><td>

<textarea id="txtContent" style="width:250px;height:100px;"></textarea>


</td></tr>


<tr><td style="text-align:right">提醒时间:</td><td><input id="txtRemindTime" style="width:200px" readonly="readonly" class="easyui-datetimebox" required="true"
                            validtype="datetime" missingmessage="请输入正确的时间格式" invalidmessage="请输入正确的时间格式"/></td></tr>
                           


<tr><td ></td><td>
               <br />
<a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                确定</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>

             </td></tr>
                    </table>
                        

                    </div>


            </div>
</asp:Content>
