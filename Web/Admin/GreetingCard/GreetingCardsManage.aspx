<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="GreetingCardsManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.GreetingCard.GreetingCardsManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
        当前位置：&nbsp;<span>贺卡管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
     <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
        <a  onclick="ShowQRcode()">点击查看手机端二维码</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Handler/GreetingCard/JuActivityHandler.ashx";

        $(function () {


            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "GreetingCard" },
	                pagination: true,
	                height: document.documentElement.clientHeight - 155,
	                striped: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                {
                                    field: 'ThumbnailsPath', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'ActivityName', title: '贺卡标题', width: 120, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;"  title="点我扫一扫分享">{0}</a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'ActivityLecturer', title: '作者', width: 80, align: 'left' },
                                { field: 'CreateDate', title: '创建时间', width: 80, align: 'left', formatter: FormatDate },

                                {
                                    field: 'IP', title: 'IP/PV', width: 40, align: 'center', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" title="点击查看统计详情" onclick="window.location.href=\'/App/Monitor/EventDetails.aspx?aid={0}\'">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                        return str.ToString();
                                    }
                                },
                                { field: 'ShareTotalCount', title: '分享次数', width: 40, align: 'center' }
	                ]]
	            }
            );
        });


        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中贺卡?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: "/Handler/JuActivity/JuActivityHandler.ashx",
                        data: { Action: "DeleteJuActivity", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            //
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                $('#grvData').datagrid('reload');
                                Show(resp.Msg);

                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }

                    });
                }
            });
        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].JuActivityID
                 );
            }
            return ids;
        }
        function ShowQRcode() {

            var linkurl = "http://<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>/app/cation/wap/newgreetingcard.aspx";
            $.ajax({
                type: 'post',
                url: "/Handler/QCode.ashx",
                data: { code: linkurl },
                success: function (result) {
                    $("#imgQrcode").attr("src", result);
                }
            });
            $('#dlgSHowQRCode').dialog('open');
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl);
        }
    </script>
</asp:Content>
