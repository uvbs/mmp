<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ChannelList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.ChannelList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        textarea {
            width: 95%;
        }

        .red {
            color: red;
        }



        .lbTip {
            padding: 3px 6px;
            background-color: #5C5566;
            color: #fff;
            font-size: 12px;
            border-radius: 50px;
            cursor: pointer;
            margin-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;渠道分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>渠道管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-userAdd" plain="true"
                onclick="ShowAdd();">增加渠道</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEdit();">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete();">删除</a>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="SetWeixinMgr();">设置微信管理员</a>

            <%--<a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="GetUserQrcode();">获取渠道二维码</a>
            --%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                onclick="GetMobileMgrQrcode();">获取手机管理页面二维码</a>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                onclick="Syn();">重新统计数据</a>
            <%if (task != null && task.FinishDate != null)
              {%>
            <span style="color: red;">最后一次更新时间:<%=task.FinishDate %></span>
            <%} %>
            <br />
            <%-- 渠道名称:--%>
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px; display: none;"
                placeholder="渠道名称" />
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();" style="display: none;">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>



    <div id="dlgUserInfo" class="easyui-dialog" closed="true" title="渠道" style="width: 450px; padding: 15px;">
        姓名/昵称:<input id="txtKeyWord1" placeholder="姓名/昵称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchWeixinUser()">查询</a>
        <br />
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>

    <div id="dlgChannelInfo" class="easyui-dialog" closed="true" title="渠道" style="width: 400px; padding: 15px;">

        <table width="100%">
            <tr class="accountUserID">
                <td>渠道名称<span class="red">*</span>
                </td>
                <td>
                    <input id="txtChannelName" type="text" style="width: 90%;" placeholder="必填" />
                </td>
            </tr>
            <tr class="accountUserID">
                <td>渠道描述
                </td>
                <td>

                    <textarea id="txtDesc" rows="3"></textarea>
                </td>
            </tr>
            <tr class="accountUserID">
                <td>上级渠道
                </td>
                <td>
                    <select id="ddlParentChannel">
                        <%
          Response.Write(string.Format("<option value=\"{0}\">{1}</option>", "", "系统"));
          foreach (var item in AllChannelList)
          {
              Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.Value, item.Text));

          } %>
                    </select>
                </td>
            </tr>
            <tr class="accountUserID">
                <td>等级
                </td>
                <td>
                    <select id="ddlLevel">
                        <%
          foreach (var item in LevelList)
          {
              Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.AutoId, item.LevelString));

          } %>
                    </select>
                </td>
            </tr>
            <tr>
                <td>联系人姓名
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>联系手机
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>公司名称
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>职位
                </td>
                <td>
                    <input id="txtPosition" type="text" style="width: 90%;" />
                </td>
            </tr>

            <tr>
                <td>邮箱
                </td>
                <td>
                    <input id="txtEmail" type="text" style="width: 90%;" />
                </td>
            </tr>
        </table>

    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = "";
        var selectAutoId = "";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryChannel"},
	                height: document.documentElement.clientHeight - 100,
	                pagination: false,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                //{ title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '渠道ID', width: 50, align: 'left' },
                                { field: 'ChannelName', title: '渠道名称', width: 100, align: 'left' },
                                { field: 'Description', title: '渠道描述', width: 100, align: 'left' },
                                { field: 'LevelName', title: '等级', width: 100, align: 'left', formatter: FormatterTitle },
                                {
                                    field: 'FirstLevelDistributionCount', title: '二维码数量', width: 100, sortable: true, align: 'center',
                                                                   formatter: function (value, row) {
                                                                       var str = new StringBuilder();
                                                                       str.AppendFormat('<a  class="listClickNum" title="点击查看" href="ChildChannelList.aspx?parentChannel={0}">{1}</a>', row["UserID"], value);
                                                                       return str.ToString();

                                                                   }
                                },
                               {
                                   field: 'DistributionDownUserCountLevel1', title: '直接会员数', width: 100, sortable: true, align: 'center',
                                                                       formatter: function (value, row) {
                                                                           var str = new StringBuilder();
                                                                           str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMemberChannel.aspx?autoId={0}&level=1">{1}</a>', row["AutoID"], value);
                                                                           return str.ToString();

                                                                       }
                                  },

                                   {
                                       field: 'DistributionDownUserCountAll', title: '所有会员数<span class="lbTip" data-tip-msg="说明:该渠道及下级渠道所有二维码的数量加上 <br/>该渠道及下级渠道所有二维码发展的所有下级会员(无限级)">?</span>', width: 100, sortable: true, align: 'center',
                                       formatter: function (value, row) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMemberChannel.aspx?autoId={0}">{1}</a>', row["AutoID"], value);
                                           return str.ToString();

                                       }
                                   },

                                //{
                                //     field: 'RecommendTrueName', title: '上级渠道', width: 80, align: 'center', formatter: function (value, row) {

                                //         //return row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';
                                //         var result = '';
                                //         if (row.ParentChannelUserInfo) {
                                //           result = row.ParentChannelUserInfo.ChannelName;

                                //         } else {
                                //             result = "";
                                //         }

                                //         return result;



                                //     }
                                //},

                               
                                //{ field: 'Email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },
                                //{
                                //    field: 'DistributionSaleAmountLevel1', title: '直接销售', width: 100, sortable: true, align: 'center', formatter: function (value, row) {
                                //                                        var str = new StringBuilder();
                                //                                        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售" href="/App/Cation/Wap/Mall/Distribution/OrderChannel.aspx?autoid={0}&status=-1&level=1">{1}</a>', row['AutoID'], value);
                                //                                        return str.ToString();

                                //                                    }
                                // },
                                {
                                    field: 'DistributionSaleAmountAll', title: '累计销售<span class="lbTip" data-tip-msg="说明:所有会员的销售额(无限级)">?</span>', width: 100, sortable: true, align: 'center', formatter: function (value, row) {
                                            var str = new StringBuilder();
                                            str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售" href="/App/Cation/Wap/Mall/Distribution/OrderChannel.aspx?autoid={0}&status=-1">{1}</a>', row['AutoID'], value);
                                            return str.ToString();

                                        }
                                },
	                            { field: 'HistoryDistributionOnLineTotalAmountEstimate', title: '累计奖励（预估）', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'OverCanUseAmount', title: '已提现奖励', width: 100, align: 'center', formatter: FormatterTitle },
                                { field: 'CanUseAmount', title: '可提现奖励', width: 100, align: 'center', formatter: FormatterTitle },

                                //CumulativeReward

                                //{
                                //    field: 'HistoryDistributionOnLineTotalAmount', title: '累计佣金', width: 80, align: 'center', sortable: true
                                // }

                                //,

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
                                    //    field: 'DistributionSaleAmountLevel1', title: '累计销售', width: 100, align: 'center', formatter: function (value, row) {
                                    //        var str = new StringBuilder();
                                    //        str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Cation/Wap/Mall/Distribution/OrderChannel.aspx?autoid={0}&status=-1">{1}</a>', row['AutoID'], value);
                                    //        return str.ToString();

                                    //    }
                                    //},
                                    {
                                        field: 'MgrUserShowName', title: '微信管理员', width: 80, align: 'center', formatter: function (value, row) {


                                                                           var result = '';
                                                                           if (row.MgrUserInfo) {
                                                                               result = row.MgrUserInfo.TrueName;
                                                                               if (result == null || result == "") {
                                                                                   result = row.MgrUserInfo.WXNickname;
                                                                               }


                                                                           } else {
                                                                               result = "";

                                                                           }

                                                                           return result;



                                                                       }
                                                                   }
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

            $('#grvUserInfo').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWebsiteUser", isWxnickName: 1 },
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


            //设置分微信管理员
            $('#dlgUserInfo').dialog({
                buttons: [{
                    text: '设置为微信管理员',
                    handler: function () {
                        $.messager.confirm("系统提示","是否设置您为渠道的代言人二维码？设置为代言人二维码会设置您以及您的下级会员的渠道为当前渠道?", function (o) {
                            if (o) {
                                var rowsTa = $("#grvData").datagrid('getSelections');
                                var rows = $("#grvUserInfo").datagrid('getSelections');
                                if (!EGCheckIsSelect(rows))
                                    return;
                                if (!EGCheckNoSelectMultiRow(rows))
                                    return;
                                $.ajax({
                                    type: 'post',
                                    url: handlerUrl,
                                    data: { Action: "SetWeixinMgrAndSetFirstDistributionLevel", userId: rowsTa[0].UserID, mgrUserId: rows[0].UserID },
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
                            else {
                                var rowsTa = $("#grvData").datagrid('getSelections');
                                var rows = $("#grvUserInfo").datagrid('getSelections');
                                if (!EGCheckIsSelect(rows))
                                    return;
                                if (!EGCheckNoSelectMultiRow(rows))
                                    return;
                                $.ajax({
                                    type: 'post',
                                    url: handlerUrl,
                                    data: { Action: "SetWeixinMgr", userId: rowsTa[0].UserID, mgrUserId: rows[0].UserID },
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
                            
                        }
                        )






                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#grvUserInfo').dialog('close');
                    }
                }]
            });


            //渠道添加编辑
            $('#dlgChannelInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        dataModel = {
                            Action: currAction,
                            AutoId:selectAutoId,
                            ChannelName: $(txtChannelName).val(),
                            Description: $(txtDesc).val(),
                            ParentChannel:$(ddlParentChannel).val(),
                            TrueName: $(txtTrueName).val(),
                            Company: $(txtCompany).val(),
                            Position: $(txtPosition).val(),
                            Phone: $(txtPhone).val(),
                            Email: $(txtEmail).val(),
                            ChannelLevelId: $(ddlLevel).val()
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.IsSuccess == true) {
                                    Alert("操作成功");
                                    LoadChannelList();
                                    $('#dlgChannelInfo').dialog('close');
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
                        $('#dlgChannelInfo').dialog('close');
                    }
                }]
            });

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
                        queryParams: { Action: "QueryChannel", keyword: $(txtKeyWord).val() }
                    });
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





        

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);

            }
            return ids;
        }
        //生成渠道二维码
        function GetUserQrcode() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $.get(handlerUrl, { action: 'GetDistributionWxQrcodeLimitUrl', user_id: rows[0].UserID,type:"channel" }, function (data) {
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
        //生成渠道二维码
        function GetMobileMgrQrcode() {

            layer.open({
                title: '用微信扫描二维码',
                type: 1,
                skin: 'layui-layer-rim', //加上边框
                area: ['350px', '350px'], //宽高
                content: '<div style="text-align: center;padding: 10px;"><img width="80%" height="80%" src="' + "/Handler/ImgHandler.ashx?v=http://" + "<%=Request.Url.Host%>" + "/App/Cation/Wap/Mall/Distribution/ChannelIndex.aspx" + '"><br/><a>http://<%=Request.Url.Host%>/App/Cation/Wap/Mall/Distribution/ChannelIndex.aspx</a></div>'

                //content: '<div><img src="' + data + '"></div>'
            });


        }


        //添加渠道
        function ShowAdd() {
            $("#dlgChannelInfo input[type='text']").val("");
            $('#dlgChannelInfo').dialog({ title: '增加渠道' });
            $('#dlgChannelInfo').dialog('open');
            currAction = "AddChannel";
           
        }

        //编辑渠道
        function ShowEdit() {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $("#txtChannelName").val(rows[0].ChannelName.replace('└', '').replace(/(^\s*)|(\s*$)/g, ""));
            $("#txtDesc").val(rows[0].Description);
            $("#ddlParentChannel").val(rows[0].ParentChannel);
            $("#ddlLevel").val(rows[0].ChannelLevelId);
            $(txtTrueName).val(rows[0].TrueName);
            $(txtCompany).val(rows[0].Company);
            $(txtPosition).val(rows[0].Postion);
            $(txtPhone).val(rows[0].Phone);
            $(txtEmail).val(rows[0].Email);

            $('#dlgChannelInfo').dialog({ title: '编辑渠道' });
            $('#dlgChannelInfo').dialog('open');
            currAction = "EditChannel";
            selectAutoId = rows[0].AutoID;
            
        }

        //设置微信管理员
        function SetWeixinMgr() {

            var rows = $("#grvData").datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            $('#dlgUserInfo').dialog({ title: '选择用户' });
            $('#dlgUserInfo').dialog('open');


        }
        //加载所有渠道
        function LoadChannelList() {
           
            $("#ddlParentChannel").html("");
            var sb = new StringBuilder();
            $.get(handlerUrl, { action: 'QueryAllChannel' }, function (data) {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", "", "无");
                data = $.parseJSON(data);
                for (var i = 0; i < data.length; i++) {
                   
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", data[i].Value, data[i].Text);
                }
                
                $("#ddlParentChannel").html(sb.ToString());

            })
           

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
                        data: { Action: "DeleteChannel", UserId: rows[0].UserID },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.IsSuccess ==true) {
                                Alert("操作成功");

                                $('#grvData').datagrid('reload');
                            }
                            else {
                                Alert(resp.Msg);
                            }


                        }
                    });


                }
            }
                )








        }


        function Syn() {//同步数据


            $.messager.confirm("系统提示", "确定重新统计?", function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "FlashChannelData" },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.IsSuccess == true) {
                                Alert("已加入任务,数据将稍后更新");

                               
                            }
                            else {
                                Alert(resp.Msg);
                            }


                        }
                    });


                }
            }
                )








        }

        </script>
</asp:Content>
