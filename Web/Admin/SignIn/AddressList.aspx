<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AddressList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SignIn.AddressList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
   当前位置： 签到管理 > 签到地点
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="AddressByAdd.aspx" class="easyui-linkbutton" iconcls="icon-list" plain="true">新增签到地点</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btndelIten" onclick="DelItem()">删除签到地点</a>
            <div style=" display:none;">
            签到地址：<span id="spanSignInHttp" style="color:blue; cursor:pointer;" ></span>
            </div>
        </div>
    </div>
   

    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
         <a id="alinkurl" href="javascript:" target="_blank" title="点击查看"></a>
    </div>
     <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/SignIn/Address/";
        var ActionType = "";
        var curId = 0;
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        $(function () {
            $("#spanSignInHttp").text("http://" + window.location.host + "/app/cation/wap/lbssigin/signinauto.aspx");
            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 80,
                pagination: true,
                striped: true,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                    { field: 'id', title: '地点编号', width: 20, align: 'left', formatter: FormatterTitle },
                    { field: 'address', title: '地点名称', width: 40, align: 'left', formatter: FormatterTitle },
                    { field: 'longitude', title: '经度', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'latitude', title: '纬度', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'range', title: '范围(米)', width: 20, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'cz', title: '操作', width: 30, align: 'center', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            str.AppendFormat('<a href="AddressByAdd.aspx?id={0}">[编辑]</a>', rowData["id"]);
                            str.AppendFormat('&nbsp;<a href="javascript:" onclick="ShowQRcode({0})" >[二维码链接]</a>', rowData["id"]);
                            str.AppendFormat('&nbsp;<a href="LogList.aspx?addressid={0}" >[签到记录]</a>', rowData["id"]);
                            return str.ToString();
                        }
                    }
                ]],
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                }
            });
            $('#grvData').datagrid('getPager').pagination({
                onSelectPage: function (pPageIndex, pPageSize) {
                    //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                    loadData();
                }
            });
            //初始加载
            loadData();
        });


       
        function loadData() {
            var gridOpts = $('#grvData').datagrid('options');
            $('#grvData').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "List.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize,type:'' },
                function (data, status) {
                    if (data.status && data.result.list) {
                        $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                    }
                });
        }

        function ShowQRcode(id) {
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Cation/wap/LBSSigIn/SignIn.aspx?addressId=' + id);
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/App/Cation/wap/LBSSigIn/SignIn.aspx?addressId=" + id;
            $("#alinkurl").html(linkurl).attr("href", linkurl);
        }

        ////删除
        function DelItem() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                if (r) {
                    var idarry = [];
                    for (var i = 0; i < rows.length; i++) {
                        idarry.push(rows[i].id);
                    }
                    var dataModel = {
                        ids: idarry.join(',')
                    }
                    $.ajax({
                        type: 'post',
                        url: handlerUrl + "Delete.ashx",
                        data: dataModel,
                        dataType: 'json',
                        success: function (data){
                            if (data.status) {
                                loadData();
                            } else {
                                Alert(data.msg);
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
