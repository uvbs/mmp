<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ChildChannelList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.ChildChannelList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
           <style>

       .lbTip {
    padding: 3px 6px;
    background-color: #5C5566;
    color: #fff;
    font-size: 14px;
    border-radius: 50px;
    cursor: pointer;
    margin-left: 20px;
}
       </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;渠道分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>二维码管理</span>

    <%if (Request["parentChannel"] != null)
      { %>
    <a href="ChannelList.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    <% }%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">

        <%if (Request["parentChannel"] != null)
          { %>

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
            onclick="ShowAdd();">新建</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
            onclick="AddFromWXUser();">从已有用户选择增加 </a>
       <%-- <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
            onclick="AddFromUser();">从已有二维码选择增加</a>--%>

        <% }%>

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
            onclick="ShowEdit();">编辑</a>

        <%-- <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                onclick="GetUserQrcode();">获取二维码</a>--%>

        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
            onclick="Delete();">删除</a>


        <br />
        <div style="margin-bottom: 5px">
            <%if (Request["parentChannel"] == null || Request["parentChannel"]=="")
              { %>
                        渠道

                     <select id="ddlChannelS">
              <%
              Response.Write(string.Format("<option value=\"{0}\">{1}</option>", "", "全部"));
              foreach (var item in AllChannelList)
              {
                  Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.Value, item.Text));

              } %>
                     </select>

            <% }
              else
              {%>
            <select id="ddlChannelS" style="display: none;">
            </select>

            <% }%>




            名称/ID:
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px;"
                placeholder="名称,ID" />


            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

    <div id="dlgChannelInfo" class="easyui-dialog" closed="true" title="增加" style="width: 400px; padding: 15px;">

        <table width="100%">

            <tr>
                <td>名称
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>所属渠道
                </td>
                <td>
                    <select id="ddlChannel">
                        <%
                            foreach (var item in AllChannelList)
                            {
                                Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.Value, item.Text));

                            } %>
                    </select>
                </td>
            </tr>





            <tr style="display: none;">
                <td>联系手机
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>公司名称
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>职位
                </td>
                <td>
                    <input id="txtPosition" type="text" style="width: 90%;" />
                </td>
            </tr>

            <tr style="display: none;">
                <td>邮箱
                </td>
                <td>
                    <input id="txtEmail" type="text" style="width: 90%;" />
                </td>
            </tr>
        </table>

    </div>

    <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="渠道" style="width: 450px; padding: 15px;">
        姓名/昵称:<input id="txtKeyWord1" placeholder="姓名/昵称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchWeixinUser()">查询</a>
        <br />
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
    <div id="dlgUserInfo1" class="easyui-dialog" closed="true" title="渠道" style="width: 450px; padding: 15px;">
        姓名/昵称:<input id="txtKeyWord2" placeholder="姓名/昵称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchUser()">查询</a>
        <br />
        <table id="grvUserInfo1" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currentAction = "AddFirstLevelDistribution";
        var selectAutoId = "0";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryChildChannel", parentChannel: "<%=Request["parentChannel"]%>" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                //{ title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: 'ID', width: 20, align: 'left' },
                                //{ field: 'HexiaoCode', title: '核销码', width: 50, align: 'left' },

                                //{
                                //    field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                //        if (value == '' || value == null)
                                //            return "";
                                //        var str = new StringBuilder();
                                //        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                //        return str.ToString();
                                //    }
                                //},
                                //{ field: 'WXNickname', title: '微信昵称', width: 80, align: 'left', formatter: FormatterTitle },

                                {
                                    field: 'TrueName', title: '名称', width: 80, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\',\'{2}\')" title="{0}">{0} [二维码]</a>', value, rowData.UserID, rowData.TrueName);
                                        return str.ToString();
                                    }
                                },
                                //{ field: 'Phone', title: '手机', width: 80, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                 {
                                     field: 'ParentChannelName', title: '所属渠道', width: 80, align: 'center', formatter: function (value, row) {

                                         //return row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';



                                         return value;



                                     }
                                 },
                                //{ field: 'TagName', title: '标签', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'Email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },

                                //{ field: 'SalesQuota', title: '累计销售', width: 100, align: 'left', formatter: FormatterTitle },
	                            //{ field: 'HistoryDistributionOnLineTotalAmountEstimate', title: '累计奖励（预估）', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'OverCanUseAmount', title: '已提现奖励', width: 100, align: 'center', formatter: FormatterTitle },
                                //{ field: 'CanUseAmount', title: '可提现奖励', width: 100, align: 'center', formatter: FormatterTitle },

                                //CumulativeReward

                                //{
                                //    field: 'HistoryDistributionOnLineTotalAmount', title: '累计佣金', width: 80, align: 'center', sortable: true
                                // }

                                //,

                                   //{
                                   //    field: 'DistributionDownUserCountLevel1', title: '一级会员数', width: 80, sortable: true, align: 'center'

                                   //},
                                   { field: 'DistributionDownUserCountAll', title: '所有会员数<span class="lbTip" data-tip-msg="说明:该会员发展的所有下级会员(无限级)">?</span>', width: 80, sortable: true, align: 'center' },

                                    
                                   //{ field: 'DistributionSaleAmountLevel1', title: '一级销售额', sortable: true, width: 80, align: 'center' },
                                   { field: 'DistributionSaleAmountAll', title: '所有销售额<span class="lbTip" data-tip-msg="说明:所有会员的销售额(无限级)">?</span>', sortable: true, width: 80, align: 'center' }

                                   //{ field: 'DistributionDownUserCountLevel2', title: '二级', width: 80, align: 'center', sortable: true,
                                   //    formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMember.aspx?level=2&autoid={0}">{1}</a>', row["AutoID"], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   //{ field: 'DistributionDownUserCountLevel3', title: '三级', width: 80, align: 'center', sortable: true,
                                   //    formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMember.aspx?level=3&autoid={0}">{1}</a>', row["AutoID"], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   //{
                                   //    field: 'DistributionSaleAmountLevel0', title: '自己消费额', width: 80, align: 'center', formatter: function (value, row) {
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                   //        return str.ToString();

                                   //    }
                                   //},
                                   // {
                                   //     field: 'DistributionSaleAmountLevel1', title: '会员消费额', width: 100, align: 'center', sortable: true, formatter: function (value, row) {
                                   //         var str = new StringBuilder();
                                   //         str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                   //         return str.ToString();

                                   //     }
                                   // }
                                    //,
                                    //{ field: 'DistributionSaleAmountLevel2', title: '二级销售额', width: 80, align: 'center', formatter: function (value, row) {
                                    //    var str = new StringBuilder();
                                    //    str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                    //    return str.ToString();

                                    //}
                                    //},
                                    //{ field: 'DistributionSaleAmountLevel3', title: '三级销售额', width: 80, align: 'center', formatter: function (value, row) {
                                    //    var str = new StringBuilder();
                                    //    str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                    //    return str.ToString();

                                    //}
                                    //}





	                ]]
	            });


            //渠道添加编辑
            $('#dlgChannelInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        dataModel = {
                            Action: currentAction,
                            AutoId: selectAutoId,
                            ParentChannel: "<%=Request["parentChannel"]%>",
                            ParentChannels: $(ddlChannel).val(),
                            TrueName: $(txtTrueName).val(),
                            Company: $(txtCompany).val(),
                            Position: $(txtPosition).val(),
                            Phone: $(txtPhone).val(),
                            Email: $(txtEmail).val()

                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == true) {
                                    Alert("操作成功");
                                    $('#dlgChannelInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    LoadChannelList();
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
                        $('#dlgChannelInfo').dialog('close');
                    }
                }]
            });

            //设置分微信二维码
            $('#dlgUserInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $("#grvUserInfo").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;


                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { Action: "SetFirstLevelDistribution", ParentChannel: "<%=Request["parentChannel"]%>", UserId: rows[0].UserID },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == 1) {
                                    Alert("操作成功");
                                    $("#dlgUserInfo").dialog('close');
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
                        $('#dlgUserInfo').dialog('close');
                    }
                }]
            });

            $('#dlgUserInfo1').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $("#grvUserInfo1").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;


                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { Action: "SetFirstLevelDistribution", ParentChannel: "<%=Request["parentChannel"]%>", UserId: rows[0].UserID },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == 1) {
                                    Alert("操作成功");
                                    $("#dlgUserInfo1").dialog('close');
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
                        $('#dlgUserInfo1').dialog('close');
                    }
                }]
            });

            $('#grvUserInfo').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { Action: "QueryWebsiteUser", isWxnickName:1 },
                   height: 400,
                   pagination: true,
                   striped: true,
                   pageSize: 20,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[

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




                   ]]
               });

            $('#grvUserInfo1').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { Action: "QueryWebsiteUser", isWeixinUser: -1 },
                   height: 400,
                   pagination: true,
                   striped: true,
                   pageSize: 20,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[

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




                   ]]
               });

            $(ddlChannelS).change(function () {


                Search();

            })

            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });
            //load


        });

        //搜索
        function Search() {

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QueryChildChannel", keyword: $(txtKeyWord).val(), parentChannel: "<%=Request["parentChannel"]%>", parentChannels: $(ddlChannelS).val() }
                    });
                }

                //添加渠道
                function ShowAdd() {
                    $("#dlgChannelInfo input[type='text']").val("");
                    $('#dlgChannelInfo').dialog({ title: '增加渠道二维码' });
                    $('#dlgChannelInfo').dialog('open');

                }
                //编辑渠道
                function ShowEdit() {

                    var rows = $('#grvData').datagrid('getSelections');

                    if (!EGCheckIsSelect(rows))
                        return;

                    if (!EGCheckNoSelectMultiRow(rows))
                        return;

                    $(txtTrueName).val(rows[0].TrueName);
                    $(txtCompany).val(rows[0].Company);
                    $(txtPosition).val(rows[0].Postion);
                    $(txtPhone).val(rows[0].Phone);
                    $(txtEmail).val(rows[0].Email);
                    $(ddlChannel).val(rows[0].ParentChannel);
                    $('#dlgChannelInfo').dialog({ title: '编辑' });
                    $('#dlgChannelInfo').dialog('open');
                    currentAction = "EditFirstLevelDistribution";
                    selectAutoId = rows[0].AutoID;

                }
                //设置微信管理员
                function AddFromWXUser() {


                    $('#dlgUserInfo').dialog({ title: '选择用户' });
                    $('#dlgUserInfo').dialog('open');


                }
                //设置微信管理员
                function AddFromUser() {


                    $('#dlgUserInfo1').dialog({ title: '选择用户' });
                    $('#dlgUserInfo1').dialog('open');


                }
                //搜索上级
                function SearchWeixinUser() {
                    $('#grvUserInfo').datagrid(
                            {
                                method: "Post",
                                url: handlerUrl,
                                queryParams: { Action: "QueryWebsiteUser", isWxnickName: 1, KeyWord: $(txtKeyWord1).val() }
                            });
                }

                //搜索上级
                function SearchUser() {
                    $('#grvUserInfo1').datagrid(
                            {
                                method: "Post",
                                url: handlerUrl,
                                queryParams: { Action: "QueryWebsiteUser", isWeixinUser: -1, KeyWord: $(txtKeyWord2).val() }
                            });
                }


                //生成渠道二维码
                function GetUserQrcode() {
                    var rows = $('#grvData').datagrid('getSelections');
                    if (!EGCheckNoSelectMultiRow(rows))
                        return;

                    $.get(handlerUrl, { action: 'GetDistributionWxQrcodeLimitUrl', user_id: rows[0].UserID, type: "", ishecheng: "1" }, function (data) {
                        if (data === "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=") {
                            alert("无法生成二维码，请检查微信配置");
                            return;
                        }

                        layer.open({
                            title: '用微信扫描二维码',
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['300px', '300px'], //宽高
                            content: '<div style="text-align: center;padding: 10px;"><img width="80%" height="80%" src="' + data + '"><br/>' + rows[0].TrueName + '</div>'


                        });
                    });

                }

                function ShowQRcode(userId, name) {

                    $.get(handlerUrl, { action: 'GetDistributionWxQrcodeLimitUrl', user_id: userId, type: "", ishecheng: "1" }, function (data) {
                        if (data === "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=") {
                            alert("无法生成二维码，请检查微信配置");
                            return;
                        }
                        layer.open({
                            title: '用微信扫描二维码',
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['300px', '300px'], //宽高
                            content: '<div style="text-align: center;padding: 10px;"><img width="80%" height="80%" src="' + data + '"><br/>' + name + '</div>'


                        });
                    });

                }
                function Delete() {

                    var rows = $('#grvData').datagrid('getSelections');

                    if (!EGCheckIsSelect(rows))
                        return;

                    if (!EGCheckNoSelectMultiRow(rows))
                        return;

                    $.messager.confirm("系统提示", "确认删除?", function (o) {
                        if (o) {

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: { Action: "DeleteFirstLevelDistribution", UserId: rows[0].UserID },
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.IsSuccess == true) {
                                        Alert("操作成功");
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        Alert(resp.Msg);
                                    }


                                }
                            });
                        }

                    })






                }

    </script>
</asp:Content>
