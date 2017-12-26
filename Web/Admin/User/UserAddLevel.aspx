<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UserAddLevel.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.UserAddLevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txtRight {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;&gt;&nbsp;&nbsp; 会员等级
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">添加新等级</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowView();" id="btnShow">星级明细</a>
            <br />
            <span style="margin-left:20px;">第一行为基础配置，其他行仅需上传图片。使用在线时长数计算等级，L：等级数，计算公式：<span class="spanLevelString"></span>。</span>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 460px; padding: 35px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td class="txtRight">等级图标:
                </td>
                <td>
                    <img alt="缩略图" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" onclick="txtThumbnailsPath.click()" />
                    <input type="file" id="txtThumbnailsPath" name="file1" />
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />80*80。
                </td>
            </tr>
            <tr class="trLevel">
                <td class="txtRight">等级L:
                </td>
                <td>
                    L
                </td>
            </tr>
            <tr class="trFromHistoryScore">
                <td class="txtRight">参数A:
                </td>
                <td>
                    <input id="txtFromHistoryScore" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" />
                </td>
            </tr>
            <tr class="trToHistoryScore">
                <td class="txtRight">参数B:
                </td>
                <td>
                    <input id="txtToHistoryScore" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" />
                </td>
            </tr>
            <tr class="trLevelString">
                <td class="txtRight" style="vertical-align:top;">公式:
                </td>
                <td>
                    <input id="txtLevelString" type="text" style="width: 150px;" maxlength="50" /><br />
                    仅能使用字符：0123456789ABL*+-/()<br />
                    如：A*(L*L+B*L)
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgView" class="easyui-dialog" closed="true" title="星级明细" style="width: 460px; height:500px;">
        <table id="grvView" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectID = 0;
        var hasLevelString = false;
        var currAction = '';
        var levelType = "OnlineTimes";
        var ol_a = '';
        var ol_b = '';
        var ol_s = '';
        var ol_data = {};
        var ol_num = 0;
        $(function () {
            $('#txtThumbnailsPath').hide();
            $(document).on('change', '#txtThumbnailsPath', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath',
                        dataType: 'json',
                        success: function (result) {
                            $.messager.progress('close');
                            if (result.Status == 1) {
                                $('#imgThumbnailsPath').attr('src', result.ExStr);
                            } else {
                                alert(result.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
            $('#grvView').datagrid({
                height: 465,
                striped: true,
                border: false,
                pagination: false,
                pageSize: 1000,
                //rownumbers: true,
                columns: [[
                    { field: 'lv', title: '等级', width: 50, align: 'left', formatter: FormatterLv },
                    { field: 'imgs', title: '图标', width: 240, align: 'left', formatter: FormatterLvImage },
                    { field: 'num', title: '在线时长', width: 100, align: 'left', formatter: FormatterLvNum }
                ]],
                onLoadSuccess: function (data) {
                    console.log(data);
                }
            });
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QueryUserLevelConfig", type: levelType },
                height: document.documentElement.clientHeight - 120,
                pagination: true,
                striped: true,
                pageSize: 20,
                //rownumbers: true,
                columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'LevelIcon', title: '图标', width: 50, align: 'left', formatter: FormatterImage },
                            { field: 'FromHistoryScore', title: '参数A', width: 20, align: 'left', formatter: FormatterV },
                            { field: 'ToHistoryScore', title: '参数B', width: 20, align: 'left', formatter: FormatterV }
                ]],
                onLoadSuccess: function (data) {
                    if (data.total > 0) {
                        var str = data.rows[0].LevelString;
                        var a = data.rows[0].FromHistoryScore;
                        var b = data.rows[0].ToHistoryScore;
                        $(".spanLevelString").text(str);
                        ol_num
                        if (str != ol_s || ol_a != a || ol_b != b || ol_num != data.total) {
                            ol_s = str;
                            ol_a = a;
                            ol_b = b;
                            ol_num = data.total;
                            var spl = [];
                            for (var i = 0; i < data.rows.length; i++) {
                                spl.push(data.rows[i].LevelIcon);
                            }
                            initLevelImgs(spl);
                            $('#grvView').datagrid('loadData',{
                                rows: ol_data,
                                total: ol_data.length
                            });
                        }
                    }
                }
            });

            $('#dlgInput').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                AutoID: currSelectID,
                                LevelType: levelType,
                                LevelIcon: $.trim($('#imgThumbnailsPath').attr('src')),
                                FromHistoryScore: $.trim($('#txtFromHistoryScore').val()),
                                ToHistoryScore: $.trim($('#txtToHistoryScore').val()),
                                LevelString: $.trim($('#txtLevelString').val())
                            }
                            if (dataModel.LevelIcon == '') {
                                Alert("请上传图标");
                                return false;
                            }
                            if (hasLevelString) {
                                dataModel.LevelString = dataModel.LevelString.replace(/[ 　]/g, '');//移除空格
                                if (dataModel.LevelString.indexOf('L') < 0) {
                                    Alert("公式中必须包含等级：L");
                                    return false;
                                }
                                var ostr = dataModel.LevelString.replace(/[0-9ABL*+-/()]/g, '');
                                if (ostr != '') {
                                    Alert("公式存在异常字符：" + ostr);
                                    return false;
                                }
                                try {
                                    var tstr = dataModel.LevelString.replace(/[ABL]/g, '4');
                                    eval(tstr)
                                } catch (e) {
                                    Alert("公式解析测试出错：" + tstr);
                                    return false;
                                }
                            }
                            if (dataModel.FromHistoryScore == '') {
                                dataModel.FromHistoryScore = 0;
                            }
                            if (dataModel.ToHistoryScore == '') {
                                dataModel.ToHistoryScore = 0;
                            }
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                success: function (result) {
                                    var resp = $.parseJSON(result);
                                    if (resp.Status == 1) {
                                        Show(resp.Msg);
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        Alert(resp.Msg);
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
        });

        function FormatterImage(value, rowDate) {
            var str = new StringBuilder();
            str.AppendFormat('<img src="{0}" style="max-width=50px;max-height:50px;"/>', value);
            return str.ToString();
        }
        function FormatterV(value, rowDate) {
            return value == 0 ? '-' : value;
        }
        function FormatterLv(value, rowData) {
            return value + ' 级';
        }
        function FormatterLvImage(value, rowData) {
            if (value.length == 0) return '';
            var str = new StringBuilder();
            for (var i = 0; i < value.length; i++) {
                str.AppendFormat('<img src="{0}" style="width:17px;height:17px;"/>', value[i]);
            }
            return str.ToString();
        }
        function FormatterLvNum(value, rowData) {
            return value +' 小时';
        }
        function ShowAdd() {
            currAction = 'AddUserLevelConfig';
            var data = $('#grvData').datagrid('getData');
            $('#imgThumbnailsPath').removeAttr('src');
            if (data.total > 5) {
                Alert('最多支持5个图标')
                return
            }
            if (data.total > 0) {
                $(".trFromHistoryScore,.trToHistoryScore,.trLevel,.trLevelString").hide();
                hasLevelString = false;
            } else {
                $(".trFromHistoryScore,.trToHistoryScore,.trLevel,.trLevelString").show();
                hasLevelString = true;
            }
            $("#dlgInput input").val("");
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
        }
        function Delete() {
            try {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;
                var data = $('#grvData').datagrid('getData');
                var bsId = data.rows[0].AutoId;
                var hasDelBase = false;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].AutoId == bsId) hasDelBase = true;
                }
                if (hasDelBase && data.rows.length > rows.length) {
                    Alert('不能在删除其他等级图标前删除基础设置');
                    return;
                }
                $.messager.confirm("系统提示", "确认删除选中等级?", function (result) {
                    if (result) {
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }
                        var dataModel = {
                            Action: 'DeleteUserLevelConfig',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                Alert(result);
                                $('#grvData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowEdit() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'EditUserLevelConfig';
            currSelectID = rows[0].AutoId;

            var rowIndex = $('#grvData').datagrid('getRowIndex', rows[0]);
            if (rowIndex > 0) {
                $(".trFromHistoryScore,.trToHistoryScore,.trLevel,.trLevelString").hide();
                hasLevelString = false;
            } else {
                $(".trFromHistoryScore,.trToHistoryScore,.trLevel,.trLevelString").show();
                hasLevelString = true;
            }
            $('#imgThumbnailsPath').attr('src', rows[0].LevelIcon);
            $('#txtFromHistoryScore').val(rows[0].FromHistoryScore);
            $('#txtToHistoryScore').val(rows[0].ToHistoryScore);
            $('#txtLevelString').val(rows[0].LevelString);

            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }
        function initLevelImgs(spl) {
            ol_data = [];
            var maxl = spl.length;
            var lv = {};
            for (var i = 0; i < spl.length; i++) {
                lv['li' + i] = [];
            }
            for (var i = 1; i <= Math.pow(maxl, maxl) ; i++) {
                if (lv['li' + (spl.length - 1)].length == 4) break;
                var imgs = [];
                for (var j = 0; j < spl.length; j++) {
                    lv['li' + j].push(spl[j]);
                    if (lv['li' + j].length < 4) break;
                    if (j < spl.length - 1) lv['li' + j] = [];
                }
                for (var j = spl.length - 1; j >= 0; j--) {
                    imgs = imgs.concat(lv['li' + j]);
                }
                var tstr = ol_s.replace(/[L]/g, i);
                tstr = tstr.replace(/[A]/g, ol_a);
                tstr = tstr.replace(/[B]/g, ol_b);
                ol_data.push({ lv: i, num: eval(tstr), imgs: imgs });
            }
            //console.log(data.levels);
        }
        function ShowView() {
            $('#dlgView').dialog('open');
        }
    </script>
</asp:Content>
