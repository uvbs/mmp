<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Distribution.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;商城分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>分销员管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

              <%
                  if (websiteOwner != "songhe")
                  {
            %>



            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowSetPreUser();">修改分销上级</a>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="SynDistribution();">更新下级人数</a>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="SynSaleAmount();">更新销售额</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="GetUserQrcode();">获取指定用户分销二维码</a>

            <%--<a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowDlgSetHexiaoCode();">设置核销码</a>--%>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEditTag();" id="BtnTag">设置标签</a>

            <%
                if (webSite.DistributionGetWay == 1)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowAddDistributionModal();" id="BtnDis">添加分销员</a>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowDistributionLevelModal();" id="BtnSetLevel">设置分销员等级</a>
            <%
                }     
            %>

            <br />

            <%--<a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="ShowAddHistoryAmount();">增加累计财富</a>
            --%>

            <%} %>



             <select id="txtDistributionOwner" style="height:30px;display:none;">
            <option value="2">分销员</option>
            <option value="0">全部</option>
           <%-- <option value="1">渠道</option>--%>
            

            </select>
            姓名:
            <input type="text" id="txtKeyWord" style="width: 200px; position: inherit; display: inline-block; padding: 6px;"
                placeholder="姓名" />

            标签:
            <input type="text" id="txtTagName" readonly="readonly" onclick="ShowTagName();" style="width: 200px; position: inherit; display: inline-block; padding: 6px;" class="" />

            推荐人:
            <input type="text" id="txtRecommendName" readonly="readonly" onclick="ShowRecommendName();" style="width: 200px; position: inherit; display: inline-block; padding: 6px;" class="" />

            <input type="radio" name="rdopay" value="0" id="rdoall" checked="checked" /><label for="rdoall">全部</label>
            <input type="radio" name="rdopay" value="1" id="rdopay" /><label for="rdopay">已消费</label>
            <input type="radio" name="rdopay" value="0" id="rdounpay" /><label for="rdounpay">未消费</label>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="false">
    </table>

    <%--修改分销上级--%>
    <div id="dlgDistribution" class="easyui-dialog" closed="true" title="设置上级" style="width: 600px; padding: 15px;">
        上级姓名:
        <input type="text" id="txtKeyWord1" style="width: 200px;" placeholder="姓名" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchPre()">查询</a>
        <table id="grvPreUserData" fitcolumns="true">
        </table>
    </div>

    <%--        <div id="dlgAddHistoryAmount" class="easyui-dialog" closed="true" title="增加累计财富" style="width: 400px;
        padding: 15px;">

                <table width="100%">
                 <tr>
                <td style="width:70px;">
                    增加累计财富金额:
                </td>
                <td>
                     <input id="txtAddHistoryAmount" onkeyup="this.value=this.value.replace(/\D/g,'')"/>
                </td>
            </tr>
            </table>
        
    </div>--%>

    <%--设置分销码--%>
    <div id="dlgSetHexiaoCode" class="easyui-dialog" closed="true" title="设置分销码" style="width: 400px; padding: 15px;">

        <table width="100%">
            <tr>
                <td style="width: 70px;">姓名/名称:
                </td>
                <td>
                    <label id="lblTrueName"></label>
                </td>
            </tr>
            <tr>
                <td style="width: 70px;">核销码:
                </td>
                <td>
                    <input id="txtHexiaoCode" placeholder="核销码为4-6位字母数字组合" maxlength="6" style="width: 200px;" />
                </td>
            </tr>
        </table>

    </div>

    <%--设置标签--%>
    <div id="dlgTag" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <div style="margin-top: 2px;">
            <input id="rdotags1" type="radio" name="tags" checked="checked" value="add" /><label
                for="rdotags1" style="font-size: 14px;"><b>增加标签</b></label>
            <input id="rdotags2" type="radio" name="tags" value="delete" /><label for="rdotags2"
                style="font-size: 14px;"><b>减少标签</b></label>
            <input id="rdotags3" type="radio" name="tags" value="update" /><label for="rdotags3"
                style="font-size: 14px;"><b>覆盖标签</b></label>
        </div>
        <br />
        <table id="grvTagData" fitcolumns="true">
        </table>
    </div>


    <div id="dlgTagName" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvTagNameData" fitcolumns="true">
        </table>
    </div>


    <div id="dlgRecommendName" class="easyui-dialog" closed="true" title="" style="width: 450px; padding: 5px;">
        姓名/名称:<input id="txtRecommendNames" placeholder="推荐人姓名/名称" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchRecommend()">查询</a>
        <br />
        <table id="grvRecommendName" fitcolumns="true">
        </table>
    </div>

    <%--设置分销员等级--%>
    <div id="dlgDistibutionLevel" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvDistibutionLevel" fitcolumns="true"></table>
    </div>

    <%--设置分销员--%>
    <div id="dlgDistributionSet" class="easyui-dialog" closed="true" title="" style="width: 700px; padding: 15px;">
        <div style="margin-bottom:10px;">
            <label>关键字：</label>
            <input type="text" id="txtUserKey" class="form-control" placeholder="昵称、姓名、手机 搜索" style="width:300px;display: inline-block;"/>
            <input id="ckTrueName" name="ckKey" checked="checked" class="positionTop3" type="checkbox" /><label for="ckTrueName">有姓名</label>
            <input id="ckPhone" name="ckKey" class="positionTop3" type="checkbox" /><label for="ckPhone">有手机</label>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchUser();">查询</a>
        </div>
        <table id="grvSetDistribution" fitcolumns="true"></table>
        <div style="margin-top:20px;">
            <label>分销员等级：</label>
            <select class="form-control" id="sDistribution" style="height: 35px;width: 100%;">
                <option value="">分销员等级</option>
            </select>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var distibutionLevelUrl = '/Handler/App/CationHandler.ashx';
        var servUrl = '/serv/api/admin/user/SetDistributionLevel.ashx';
        var userUrl = '/serv/api/admin/member/list.ashx';
        var userAutoId = ""; //用户AutoId
        var websiteOwner = "<%=websiteOwner%>";
        var recomendUserIdStr = "";
        $(function () {
            //显示
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWebsiteUserDistributionOnLine", autoId: userAutoId, isDistributionOwner: "2" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '用户ID', width: 50, align: 'left' },
                                //{ field: 'HexiaoCode', title: '核销码', width: 50, align: 'left' },

                                {
                                    field: 'WXHeadimgurl', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '姓名', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'Phone', title: '手机', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'MemberLevel', title: '分销员等级', width: 80, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
	                //{ field: 'Postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                 {
                                     field: 'RecommendTrueName', title: '推荐人', width: 80, align: 'center', formatter: function (value, row) {

                                         //return row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';
                                         var result = '';
                                         if (row.DistributionOnLineRecomendUserInfo) {
                                             if (websiteOwner == row.DistributionOnLineRecomendUserInfo.UserID) {
                                                 result = "系统";
                                             } else {

                                                 result = row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';

                                             }

                                         } else {
                                             result = "";
                                         }

                                         return result;



                                     }
                                 },
                                { field: 'TagName', title: '标签', width: 100, align: 'left', formatter: FormatterTitle },
                                //{ field: 'Email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },

                                { field: 'SalesQuota', title: '累计销售', width: 100, sortable: true, align: 'left', formatter: FormatterTitle },
	                            { field: 'HistoryDistributionOnLineTotalAmountEstimate', title: '累计奖励（预估）', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'OverCanUseAmount', title: '已提现奖励', width: 100, align: 'center', formatter: FormatterTitle },
                                { field: 'CanUseAmount', title: '可提现奖励', width: 100, align: 'center', formatter: FormatterTitle },

                                //CumulativeReward

                                //{
                                //    field: 'HistoryDistributionOnLineTotalAmount', title: '累计佣金', width: 80, align: 'center', sortable: true
                                // }

                                //,

                                   {
                                       field: 'DistributionDownUserCountLevel1', title: '会员数', width: 80, align: 'center', sortable: true,
                                       formatter: function (value, row) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看" href="/App/Cation/Wap/Mall/Distribution/MyMember.aspx?level=1&autoid={0}">{1}</a>', row["AutoID"], value);
                                           return str.ToString();

                                       }
                                   },
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
                                   {
                                       field: 'DistributionSaleAmountLevel0', title: '自己消费额', width: 80, align: 'center', sortable: true, formatter: function (value, row) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                           return str.ToString();

                                       }
                                   },
                                    {
                                        field: 'DistributionSaleAmountLevel1', title: '会员消费额', width: 100, align: 'center', sortable: true, formatter: function (value, row) {
                                            var str = new StringBuilder();
                                            str.AppendFormat('<a target="_blank" class="listClickNum" title="点击查看销售额" href="/App/Distribution/DistributionOrder.aspx?uid={0}">{1}</a>', row['AutoID'], value);
                                            return str.ToString();

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

            $("[name='rdopay']").click(function () {
                Search();


            })


            //设置分销上级对话框
            $('#dlgDistribution').dialog({
                buttons: [{
                    text: '修改分销上级',
                    handler: function () {

                        var auIds = GetRowsIds($('#grvData').datagrid('getSelections')).toString();
                        var rows = $("#grvPreUserData").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;
                        var preUserId = $('#grvPreUserData').datagrid('getSelections')[0].UserID;
                        var preAutoId = $('#grvPreUserData').datagrid('getSelections')[0].AutoID;
                        if ($.inArray(preAutoId, GetRowsIds($('#grvData').datagrid('getSelections'))) >= 0) {
                            Alert("上级用户不能跟选择的用户相同，请检查");
                            return false;
                        }



                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { Action: "UpdateDistributionOnLinePreUser", autoIds: auIds, preUserId: preUserId },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    Alert("操作成功,下级人数将稍后更新");
                                    $('#dlgDistribution').dialog('close');
                                    // $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistribution').dialog('close');
                    }
                }]
            });

            //            //增加累计财富
            //            $('#dlgAddHistoryAmount').dialog({
            //                buttons: [{
            //                    text: '增加累计财富',
            //                    handler: function () {
            //                        var autoIds = GetRowsIds($('#grvData').datagrid('getSelections')).toString();
            //                        var amount = $(txtAddHistoryAmount).val();
            //                        if (amount == "") {
            //                            Alert("请输入金额");
            //                            return false;
            //                        }
            //                        if (isNaN(amount)) {
            //                            Alert("请输入数字");
            //                            return false;
            //                        }
            //                        $.ajax({
            //                            type: 'post',
            //                            url: "Handler/User/AddHistoryTotalAmount.ashx",
            //                            data: { autoIds: autoIds, amount: amount },
            //                            dataType: "json",
            //                            success: function (resp) {
            //                                if (resp.status) {
            //                                    Alert("操作成功");
            //                                    $('#dlgAddHistoryAmount').dialog('close');
            //                                    $('#grvData').datagrid('reload');
            //                                }
            //                                else {
            //                                    Alert(resp.msg);
            //                                }


            //                            }
            //                        });

            //                    }
            //                }, {
            //                    text: '取消',
            //                    handler: function () {
            //                        $('#dlgAddHistoryAmount').dialog('close');
            //                    }
            //                }]
            //            });

            //设置核销码
            $('#dlgSetHexiaoCode').dialog({
                buttons: [{
                    text: '设置核销码',
                    handler: function () {
                        var hexiaoCode = $("#txtHexiaoCode").val();
                        if (hexiaoCode == "") {
                            Alert("请输入核销码");
                            return false;
                        }
                        if (hexiaoCode.length < 4 || hexiaoCode.length > 6) {
                            Alert("核销码为4-6位字母数字组合");
                            return false;
                        }


                        $.ajax({
                            type: 'post',
                            url: "/Serv/Api/Admin/User/SetHexiaoCode.ashx",
                            data: { autoId: $('#grvData').datagrid('getSelections')[0].AutoID, hexiaoCode: hexiaoCode },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    Alert("操作成功");
                                    $('#dlgSetHexiaoCode').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgSetHexiaoCode').dialog('close');
                    }
                }]
            });


            //设置用户标签
            $('#dlgTag').dialog({

                buttons: [{
                    text: '保存',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');

                        var rowsTag = $('#grvTagData').datagrid('getSelections');
                        if (!EGCheckIsSelect(rowsTag)) {
                            return;
                        }

                        var AutoID = [];
                        var TagName = [];
                        var AccessLevel = [];

                        for (var i = 0; i < rowsTag.length; i++) {
                            TagName.push(rowsTag[i].TagName);
                            AccessLevel.push(rowsTag[i].AccessLevel);
                        }

                        for (var i = 0; i < rows.length; i++) {
                            AutoID.push(rows[i].AutoID);
                        }
                        var tagType = $("input[name=tags]:checked").val();
                        if (tagType == "add") {
                            action = "UpdateUserTagNameByAddTag";
                        } else if (tagType == "delete") {
                            action = "UpdateUserTagNameByDeleteTag";
                        } else if (tagType == "update") {
                            action = "UpdateUserTagName"
                        }
                        var dataModel = {
                            Action: action,
                            AutoID: AutoID.join(','),
                            TagName: TagName.join(','),
                            AccessLevel: AccessLevel.join(',')
                        };
                        $.messager.progress({ text: '正在提交...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    Alert("保存成功");
                                    $('#dlgTag').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTag').dialog('close');
                    }
                }]
            });




            //标签列表
            $('#grvTagData2').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryMemberTag", TagType: "member" },
                height: 200,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'TagName', title: '用户标签', width: 20, align: 'left' }
                ]]
            });

            //用户等级配置

            $('#grvDistibutionLevel').datagrid(
               {
                   method: "Post",
                   url: distibutionLevelUrl,
                   queryParams: { Action: "QueryUserLevelConfig", type: 'DistributionOnLine' },
                   height: 300,
                   pagination: true,
                   striped: true,
                   pageSize: 10,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[
                               { title: 'ck', width: 5, checkbox: true },
                               { field: 'LevelNumber', title: '等级数字', width: 80, align: 'center' },
                               { field: 'LevelString', title: '等级名称', width: 100, align: 'center' }
                   ]]
               });

            //用户列表
            
            $('#grvSetDistribution').datagrid(
               {
                   method: "Post",
                   url: userUrl,
                   queryParams: { mapping_type: '1', isName: '1', noDistributionOwner: '1' },
                   height: 300,
                   loadFilter: pagerFilter,
                   pagination: true,
                   striped: true,
                   pageSize: 20,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[
                               {
                                   field: 'WXHeadimgurl', title: '头像', width: 50, align: 'center', formatter: function (value) {
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

            //用户等级配置

            $.ajax({
                type: 'post',
                url: distibutionLevelUrl,
                data: { Action: "QueryUserLevelConfig", type: 'DistributionOnLine',page:1,rows:20 },
                dataType: 'json',
                success: function (result) {
                    var list = result.rows;
                    for (var i = 0; i < list.length; i++) {
                        var option = $("<option value=" + list[i].LevelNumber + ">" + list[i].LevelString + "</option>")
                        $('#sDistribution').append(option);
                    }
                }
            });


            //标签搜索
            $('#dlgTagName').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rowsTag = $('#grvTagNameData').datagrid('getSelections');
                        var TagName = [];
                        for (var i = 0; i < rowsTag.length; i++) {
                            TagName.push(rowsTag[i].TagName);
                        }

                        $("#txtTagName").val(TagName.join(','));
                        $('#dlgTagName').dialog('close');
                        Search();

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTagName').dialog('close');
                    }
                }]
            });




            //推荐人搜索
            $('#dlgRecommendName').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rowsRecommendName = $('#grvRecommendName').datagrid('getSelections');
                        var recommendName = [];
                        var recomendUserId = [];
                        for (var i = 0; i < rowsRecommendName.length; i++) {
                            recommendName.push(rowsRecommendName[i].TrueName);
                            recomendUserId.push(rowsRecommendName[i].UserID);
                        }

                        $("#txtRecommendName").val(recommendName.join(','));
                        $('#dlgRecommendName').dialog('close');
                        recomendUserIdStr = recomendUserId.join(',');
                        Search();

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgRecommendName').dialog('close');
                    }
                }]
            });

            //分销员等级dialog
            $('#dlgDistibutionLevel').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var userRows = $('#grvData').datagrid('getSelections');
                        var levelRows = $('#grvDistibutionLevel').datagrid('getSelections');

                        if (levelRows.length <= 0) {
                            alert('请选择分销员等级');
                            return;
                        }

                        var autoIds = [];

                        for (var i = 0; i < userRows.length; i++) {
                            autoIds.push(userRows[i].AutoID);
                        }

                        var dataModel =
                        {
                            autoid: autoIds.join(','),
                            member_level: levelRows[0].LevelNumber
                        };

                        $.ajax({
                            type: 'POST',
                            url: servUrl,
                            data: dataModel,
                            dataType: 'json',
                            success: function (result) {
                                if (result.status) {
                                    alert('设置成功');
                                    $('#dlgDistibutionLevel').dialog('close');
                                    $('#grvData').datagrid('reload');
                                } else {
                                    alert('设置出错');
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistibutionLevel').dialog('close');
                    }
                }]
            });

            //设置分销员dialog
            $('#dlgDistributionSet').dialog({
                buttons: [{
                    text: '添加',
                    handler: function () {
                       
                        var levelRows = $('#grvSetDistribution').datagrid('getSelections');

                        if (levelRows.length <= 0) {
                            alert('请选择会员');
                            return;
                        }

                        var distributionLevel=$("#sDistribution").val();
                        if (distributionLevel == '') {
                            alert('请选择分销员等级');
                            return;
                        }

                        var autoIds = [];

                        for (var i = 0; i < levelRows.length; i++) {
                            autoIds.push(levelRows[i].AutoID);
                        }

                        var dataModel =
                        {
                            member_level: distributionLevel,
                            autoid: autoIds.join(',')
                        };

                        $.ajax({
                            type: 'POST',
                            url:'/serv/api/admin/user/editdistribution.ashx',
                            data: dataModel,
                            dataType: 'json',
                            success: function (result) {
                                if (result.status) {
                                    alert(result.msg);
                                    $('#dlgDistributionSet').dialog('close');
                                    $('#grvSetDistribution').datagrid('reload');
                                    $('#grvData').datagrid('reload');
                                } else {
                                    alert('设置出错');
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistributionSet').dialog('close');
                    }
                }]
            });


            $('[name=ckKey]').click(function () {
                SearchUser();
            });


        });

        //搜索
        function Search() {

            var payStatus = "";
            if (rdopay.checked) {
                payStatus = 1;
            }
            if (rdounpay.checked) {
                payStatus = 0;
            }
            if (rdoall.checked) {
                payStatus = "";
            }

            $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $(txtKeyWord).val(), autoId: userAutoId, isDistributionOwner: $(txtDistributionOwner).val(), TagName: $("#txtTagName").val(), RecommendUserIds: recomendUserIdStr, PayStatus: payStatus }
                    });
        }
        //搜索
        function SearchRecommend() {

            $('#grvRecommendName').datagrid(
            {
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $("#txtRecommendNames").val() }
            });
        }

        //搜索上级
        function SearchPre() {
            $('#grvPreUserData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $(txtKeyWord1).val() }
                    });
        }

        //显示设置分销上级对话框
        function ShowSetPreUser() {

            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#grvPreUserData').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { Action: "QueryWebsiteUserDistributionOnLine" },
                   height: 400,
                   pagination: true,
                   striped: true,
                   pageSize: 20,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[

                               {
                                   field: 'WXHeadimgurl', title: '头像', width: 50, align: 'center', formatter: function (value) {
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



            //

            $('#dlgDistribution').dialog({ title: '请选择上级用户' });
            $('#dlgDistribution').dialog('open');

        }

        //显示设置核销码对话框
        function ShowDlgSetHexiaoCode() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $("#txtHexiaoCode").val("");
            $("#lblTrueName").html(rows[0].TrueName);
            $('#dlgSetHexiaoCode').dialog({ title: '设置核销码' });
            $('#dlgSetHexiaoCode').dialog('open');

        }


        //显示设置分销上级对话框
        function ShowAddHistoryAmount() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgAddHistoryAmount').dialog({ title: '增加累计财富' });
            $('#dlgAddHistoryAmount').dialog('open');

        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoID);

            }
            return ids;
        }

        function GetUserQrcode() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $.get(handlerUrl, { action: 'GetDistributionWxQrcodeLimitUrl', user_id: rows[0].UserID, ishecheng: 1 }, function (data) {
                if (data === "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=") {
                    alert("无法生成二维码，请检查微信配置");
                    return;
                }
                var name = rows[0].TrueName;
                if (name == null) {
                    name = rows[0].WXNickname;
                }
                if (name == null) {
                    name = "";
                }

                layer.open({
                    title: '用微信扫描二维码',
                    type: 1,
                    skin: 'layui-layer-rim', //加上边框
                    area: ['300px', '300px'], //宽高
                    content: '<div style="text-align: center;padding: 10px;"><img width="80%" height="80%" src="' + data + '"><br/>' + name + '</div>'

                    //content: '<div><img src="' + data + '"></div>'
                });
            });

        }

        //同步
        function SynDistribution() {

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "SynDistribution" },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        Alert("操作成功,下级人数将稍后更新");


                    }
                    else {
                        Alert(resp.msg);
                    }


                }
            });

        }

        //更新销售额
        function SynSaleAmount() {

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "SynDistributionSaleAmount" },
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        Alert("操作成功,销售额将稍后更新");

                    }
                    else {
                        Alert(resp.msg);
                    }


                }
            });

        }

        //选择标签对话框
        function ShowEditTag() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {

                return;
            }
            //标签列表
            $('#grvTagData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: "member" },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TagName', title: '标签名称', width: 20, align: 'left' }
	                ]]
	            }
            );



            $('#dlgTag').dialog({ title: '设置标签' });
            $('#dlgTag').dialog('open');

        }
        //显示设置分销员等级对话框
        function ShowDistributionLevelModal() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }


            $('#dlgDistibutionLevel').dialog({ title: '分销员等级列表' });
            $('#dlgDistibutionLevel').dialog('open');
        }


        //设置分销员
        function ShowAddDistributionModal() {

            $('#dlgDistributionSet').dialog({ title: '用户列表' });
            $('#dlgDistributionSet').dialog('open');
        }


        //显示设置标签对话框
        function ShowTagName() {

            //标签列表
            $('#grvTagNameData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: 'member' },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TagName', title: '标签名称', width: 20, align: 'left' }


	                ]]
	            }
            );
            $('#dlgTagName').dialog({ title: '标签' });
            $('#dlgTagName').dialog('open');

        }
        //推荐人对话框
        function ShowRecommendName() {

            $('#dlgRecommendName').dialog({ title: '推荐人' });
            $('#dlgRecommendName').dialog('open');

            //推荐人列表
            $('#grvRecommendName').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWebsiteUserDistributionOnLine", autoId: userAutoId },
	                height: 300,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                               { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '用户ID', width: 50, align: 'left' },
                                { field: 'HexiaoCode', title: '核销码', width: 50, align: 'left' },

                                {
                                    field: 'WXHeadimgurl', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '姓名/名称', width: 80, align: 'left', formatter: FormatterTitle }


	                ]]
	            }
            );


        }

        function SearchUser() {
            var keywords = $('#txtUserKey').val();
            var isName = $('#ckTrueName').attr('checked')=='checked'?'1':'';
            var isPhone = $('#ckPhone').attr('checked') == 'checked' ? '1' : '';

           
            var dataModel={
                KeyWord:keywords,
                mapping_type:'1',
                isName: isName,
                isPhone: isPhone,
                isOrAnd: '1',
                noDistributionOwner:'1'
            };
            $('#grvSetDistribution').datagrid(
                   {
                       method: "Post",
                       url: userUrl,
                       queryParams: dataModel
                   });
        }
    </script>
</asp:Content>
