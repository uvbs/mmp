<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WxQuestionnaireManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.WxQuestionnaireManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>所有微问卷</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
     <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
         
            <br />
            问卷名称:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
               <a href="javascript:;" onclick="ShowQRcode()" style="color: blue;">获取转发中心链接</a>
            <br />
        </div>
    </div>

      <table id="grvData" fitcolumns="true">
      
      </table>
   <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script type="text/javascript">
        var url = "/serv/api/admin/Forward/list.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        $(function () {
            //列表
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: url,
                       queryParams: { type: "questionnaire" },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       singleSelect: false,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                {
                                    field: 'activity_name', title: '问卷名称', width: 20, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">{1}</a>', rowData.activity_id, rowData.activity_name);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'forwarnum', title: '转发人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.forwarnum == 0) {
                                            str.AppendFormat("{0}", rowData.forwarnum);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" title="点击查看转发人数" href="WxQuestionnaireUserInfo.aspx?id={0}&Mid={1}" >{2}</a>', rowData.activity_id, rowData.mid, rowData.forwarnum);
                                        }
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'uv', title: '微信阅读人数', sortable: true, width: 10, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.uv == '' || rowData.uv == null) {
                                            str.AppendFormat('{0}', 0);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>', rowData.activity_id, rowData.uv);
                                        }
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'ippv', title: 'IP/PV', sortable: true, width: 10, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.pv == 0) {
                                            str.AppendFormat("{0}", 0);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={2}" title="点击查看统计详情">{0}/{1}</a>', rowData.ip, rowData.pv, rowData.activity_id);
                                        }

                                        return str.ToString();
                                    }
                                },
                                { field: 'insert_date', title: '创建时间', width: 20, align: 'left', formatter: FormatDate }
                       ]]
                   }
               );


            $("#btnSearch").click(function () {
                $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: url,
	                queryParams: { type: "questionnaire", keyword: $("#txtName").val() }
	            });
            })
        });



        function Delete() {
            var rows = $("#grvData").datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) return;
            $.messager.confirm('友情提示', '确定要删除选中的信息吗?', function (o) {
                if (o) {
                    $.ajax({
                        type: "POST",
                        url: "/serv/api/admin/Forward/delete.ashx",
                        data: {  ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                Alert(resp.msg)
                                $("#grvData").datagrid('reload');
                            } else {
                                Alert(resp.msg)
                            }
                        }
                    });
                }
            });

        }

        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].activity_id);
            }
            return ids;
        }

        function ShowQRcode() {
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Forward/wap/allforwardlistwap.aspx');
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/Forward/wap/allforwardlistwap.aspx";
            $("#alinkurl").html(linkurl).attr("href", linkurl);
        }



</script>

</asp:Content>
