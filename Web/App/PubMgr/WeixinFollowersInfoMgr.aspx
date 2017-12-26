<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WeixinFollowersInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinFollowersInfoMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        #txtKeyWord {
            width:200px;
            height:30px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;客户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>微信粉丝</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div>
        <div class="center">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
                <%--               <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                                onclick="SynchronousAllFollowers()">更新微信粉丝数量</a>--%>

                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                    onclick="UpdateAllFollowersInfo()">更新微信粉丝信息</a>
                &nbsp;


                <%
                    if (IsShowSendTempMessage)
                    {
                %>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-send" plain="true"
                    onclick="ShowSendTemplateMsg();">发送微信消息</a>
                 <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-send" plain="true"
                    onclick="ShowAllSendTemplateMsg();">给所有已关注粉丝群发消息</a>

                <%
                }     
                %>


                                                <%
                                    if (model != null)
                                    {
                                %>
                                <span style="color: red;">最后一次更新时间:<%=model.InsertDateString %>&nbsp;<%=model.StatusString %></span>

                <%
                                    }    
                %>


                <br />
                <%--                                <label>所属用户组</label>
                                 <input type="checkbox" name="PmsGroup" v="无" id="Checkbox4" /><label for="Checkbox4">无</label>

                               <input type="checkbox" name="PmsGroup" v="游客" id="chkArticleCate1" /><label for="chkArticleCate1">游客</label>
            &nbsp;
            <input type="checkbox" name="PmsGroup" v="正式学员" id="Checkbox1" /><label for="Checkbox1">正式学员</label>
            &nbsp;
            <input type="checkbox" name="PmsGroup" v="教师" id="Checkbox2" /><label for="Checkbox2">教师</label>
            &nbsp;
            <input type="checkbox" name="PmsGroup" v="管理员" id="Checkbox3" /><label for="Checkbox3">管理员</label>
            &nbsp;--%>
               
                <input type="text" id="txtKeyword" placeholder="输入粉丝昵称搜索" />
                <select id="sFollow">
                    <option value="">全部</option>
                    <option value="1">已关注</option>
                    <option value="0">未关注</option>
                </select>

                 <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>
            <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                      <th title="ck" width="5" checkbox="true">
                         
                        </th> 

                        <th field="HeadImgUrl" width="10" formatter="formatheadimg">头像
                        </th>

                        <th field="NickName" width="20">昵称
                        </th>
                        <th field="Sex" width="10" formatter="formatsex">性别
                        </th>

                        <th field="FlowKeyword" width="20" formatter="formatarea">地区
                        </th>
                        <th field="IsWeixinFollower" width="20" formatter="formartisfollower">是否已经关注
                        </th>
                        <th field="Subscribe_time" width="20" >关注时间
                        </th>
                        <th field="UnSubscribeTime" width="20" >取消关注时间
                        </th>
                        <th field="Source" width="20" >来源
                        </th>
                        <th field="ParentShowName" width="20" >推荐人
                        </th>
                        

                    </tr>
                </thead>
            </table>

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

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var grid;
        //处理文件路径
        var url = "/Handler/App/CationHandler.ashx";
        //Action
        var currentAction = '';
        //加载文档
        jQuery().ready(function () {

            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: url,
                height: document.documentElement.clientHeight - 112,
                fitCloumns: true,
                nowrap: true,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "QueryWeixinFollowersInfo" }
            });
            //------------加载gridview

            $("#btnSearch").click(function () {

                $('#grvData').datagrid({
                    queryParams: {
                        Action: "QueryWeixinFollowersInfo",
                        KeyWord: $("#txtKeyword").val(),
                        IsFollower: $(sFollow).val()
                    }
                });



            })
            $(sFollow).change(function () {
                $("#btnSearch").click();

            })






            ///发送微信模板消息
            $('#dlgSendTemplateMsg').dialog({
                buttons: [{
                    text: '发送',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            Action: currentAction,
                            TemplateType: $(ddlTemplateType).val(),
                            Title: $(txtTitle).val(),
                            Content: $(txtContent).val(),
                            Url: $(txtLink).val(),
                            AutoIds:currentAction!=''?GetRowsIds(rows).join(','):''

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
                            url: url,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgSendTemplateMsg').dialog('close');
                                    Alert(resp.Msg);
                                    $('#grvData').datagrid('reload');
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


        })

        //发送微信消息模板
        function ShowSendTemplateMsg() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgSendTemplateMsg').dialog({ title: '发送微信消息' });
            $('#dlgSendTemplateMsg').dialog('open');
            currentAction = 'SendTemplateMsgByFans';

        }
        //群发微信消息模板
        function ShowAllSendTemplateMsg() {
            var rows = $('#grvData').datagrid('getSelections');
            $('#dlgSendTemplateMsg').dialog({ title: '群发微信消息' });
            $('#dlgSendTemplateMsg').dialog('open');
            currentAction = 'SendTemplateMsgByAllFans';
        }
        function formatheadimg(value) {

            if (value == "" || value == null) {
                return "";
            }
            return "<img src=\"" + value + "\" width=\"50px\" height=\"50px\">"

        }


        function formatarea(value, row) {

            return row.Country + " " + row.Province + " " + row.City;


        }
        function formartisfollower(value,row){
        
            if (value==0) {
                return "<font color='red'>未关注</font>";
            }
            else if (value == 1) {
                return "<font color='green'>已关注</font>";
            }
        }
       
        function formatsex(value) {

            if (value == "0") {
                return "未知";
            }
            if (value == "1") {
                return "男";
            }
            if (value == "2") {
                return "女";
            }

        }
        //     function SynchronousAllFollowers() {
        //         $.messager.confirm("系统提示", "确定更新微信粉丝数量吗?<br/>此过程可能需要几分钟?", function (r) {
        //             if (r) {
        //                 $.messager.progress({ text: '正在同步,请稍候。此过程可能需要几分钟...' });
        //                 jQuery.ajax({
        //                     type: "Post",
        //                     url: url,
        //                     data: { Action: "SynchronousAllFollowers" },
        //                     success: function (result) {
        //                         $.messager.progress('close');
        //                         Alert(result);
        //                         grid.datagrid('reload');

        //                     }
        //                 });
        //             }
        //         });
        //     };


        function UpdateAllFollowersInfo() {

            $.messager.confirm("系统提示", "确定更新微信粉丝信息吗?<br/>粉丝列表将稍后更新?", function (r) {
                if (r) {
                    //$.messager.progress({ text: '正在同步,请稍候。此过程可能需要几分钟...' });
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "UpdateAllFollowersInfo" },
                        success: function (result) {
                            $.messager.progress('close');
                            Alert(result);
                            //grid.datagrid('reload');

                        }
                    });
                }
            });
        };


        //function Search() {
        //    try {


        //        $('#grvData').datagrid({
        //            queryParams: {
        //                Action: "QueryWeixinFollowersInfo"
        //                // , pmsGroup: GetCheckGroupVal('PmsGroup', 'v')//$('#selectPmsGroup').combobox('getText')
        //            }
        //        });
        //    } catch (e) {
        //        alert(e);
        //    }
        //}
        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);

            }
            return ids;
        }

    </script>
</asp:Content>
