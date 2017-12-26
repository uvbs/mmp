<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="BarCodeMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.BarCode.BarCodeMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>产品真伪查询</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="ConfigBarCodeInfoEdit.aspx" class="easyui-linkbutton" iconcls="icon-add2"
                plain="true">添加</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                    plain="true" onclick="OnUpload()">exel表格产品导入</a> <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-delete" plain="true" onclick="OnEdit()">修改</a> <a href="javascript:;"
                            class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                            删除</a> <a href="javascript:;" class="easyui-linkbutton" iconcls=" icon-add" plain="true"
                                onclick="ConfigBarCodeInfoEdit()">配置返回结果</a> <a href="javascript:;" class="easyui-linkbutton"
                                    iconcls=" icon-add" plain="true" onclick="EmptyQuerys()">清空查询次数</a>
            <br />
            条形码:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px;
        height: 185px; padding: 10px">
        <table>
            <tr>
                <td height="25" align="left">
                    查询次数：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtQueryNum" style="width: 150px;" class="easyui-numberspinner"
                        required="true" missingmessage="请输入查询次数" value="0" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">
                    配置返回结果：
                </td>
                <td height="25" width="*" align="left">
                    <textarea rows="4" style="width: 200px;" id="txtPopupInfoon"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)"
                        id="btnExit" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="UploadDiv" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px;
        height: 140px; padding: 10px">
        <table>
            <tr>
                <td height="25" align="left">
                    选择文件：
                </td>
                <td height="25" width="*" align="left">
                    <input name="pictPath" type="file" id="uploadify" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">
                </td>
                <td height="25" width="*" align="left">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <a href="javascript:void(0)" id="BtnUpload" class="easyui-linkbutton">上 传</a> <a
                        href="javascript:void(0)" id="btnClose" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/WXBarCodeHandler.ashx";
    var domain = '<%=Request.Url.Host %>';
    $(function () {
        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetBarCodeInfoList" },
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CodeName', title: '名称', width: 20, align: 'left' },
                                { field: 'BarCode', title: '条形码', width: 10, align: 'left' },
                                { field: 'ModelCode', title: '型号', width: 10, align: 'left' },
                                { field: 'Agency', title: '经销商', width: 10, align: 'left' },
                                { field: 'InsetData', title: '创建时间', width: 20, align: 'left' },
                                { field: 'TimeOne', title: '第一次查询时间', width: 10, align: 'left' },
                                { field: 'TimeTwo', title: '第二次查询时间', width: 10, align: 'left' },
                                { field: 'TimeThree', title: '第三次查询时间', width: 10, align: 'left' },
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
        $.messager.confirm("系统提示", "确定删除选中?", function (o) {
            if (o) {
                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "DeleteBarCodeInfoId", ids: GetRowsIds(rows).join(',') },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 0) {
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
            ids.push(rows[i].AutoId);
        }
        return ids;
    }

    function ConfigBarCodeInfoEdit() {
        $('#dlgPmsInfo').window(
            {
                title: '管理配置返回结果'
            }
            );
        //            $('#dlgPmsInfo').dialog('open');
        //            $(btnSave).attr('tag', 'edit');

        $.post(handlerUrl, { Action: "GetConfigureConfigInfo" }, function (data) {
            var resp2 = $.parseJSON(data);
            if (resp2.Status == 0) {
                $("#txtQueryNum").val(resp2.ExObj.QueryNum);
                $("#txtPopupInfoon").val(resp2.ExObj.PopupInfo);
            }
            $('#dlgPmsInfo').dialog('open');
        });
    }




    function Search() {
        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetBarCodeInfoList", BarCode: $("#txtName").val() }
	            });
    }

    //窗体关闭按钮---------------------
    $("#btnExit").live("click", function () {
        $("#dlgPmsInfo").window("close");
    });

    $("#btnSave").live("click", function () {
        var QueryNum = $("#txtQueryNum").val();
        var PopupInfoon = $("#txtPopupInfoon").val();
        $.post(handlerUrl, { Action: "ConfigureConfigInfo", QueryNum: QueryNum, PopupInfo: PopupInfoon }, function (data) {
            var resp3 = $.parseJSON(data);
            if (resp3.Status = 0) {
                Show(resp3.Msg);
            } else {
                Alert(resp3.Msg);
            }
        });
    });
    function OnUpload() {
        $('#UploadDiv').window(
            {
                title: '上传文件'
            }
            );
        $('#UploadDiv').dialog('open');
    }

    $("#btnClose").live("click", function () {
        $("#UploadDiv").window("close");
    });

    $("#BtnUpload").live("click", function () {
        $("#UploadDiv").window("close");
        $.messager.progress({ text: '正在上传数据。。。' });
        $.ajaxFileUpload(
                     {
                         url: handlerUrl + '?action=UploadCodeInfoData',
                         secureuri: false,
                         fileElementId: 'uploadify',
                         dataType: 'text',
                         success: function (result) {
                             try {
                                 result = result.substring(result.indexOf("{"), result.indexOf("</"));
                             } catch (e) {
                                 alert(e);
                             }
                             $.messager.progress('close');
                             var resp = $.parseJSON(result);
                             Alert(resp.Msg);
                             $('#grvData').datagrid('reload');
                         },
                         TimeOut: 600000

                         //                         ,
                         //                         error: function (data)            //相当于java中catch语句块的用法
                         //                         {
                         //                             console.log(data);
                         //                             $.messager.progress('close');
                         //                             Alert(data.Msg);
                         //                             $('#grvData').datagrid('reload');
                         //                         }
                     }
                    );
    });

    function OnEdit() {
        var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
        if (!EGCheckIsSelect(rows)) {
            return;
        }
        if (rows.length > 1) {
            Alert("只能选择一行数据");
        }
        window.location.href = "ConfigBarCodeInfoEdit.aspx?id=" + GetRowsIds(rows).join(',');
    }

    function EmptyQuerys() {
        var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
        if (!EGCheckIsSelect(rows)) {
            return;
        }
        $.messager.confirm("系统提示", "确定清空查询次数选中?", function (o) {
            if (o) {
                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "EmptyQuerys", ids: GetRowsIds(rows).join(',') },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 0) {
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

    </script>
</asp:Content>
