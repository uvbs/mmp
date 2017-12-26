<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="NewsImgManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.NewsImgManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;公众号设置&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>图文素材</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div>
        <div class="center">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAddOrEdit('add')">
                    添加</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">
                    编辑</a> 
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                    删除</a>
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="Broadcast()">
                    群发(客服接口)（48小时内未与公众号互动的粉丝收不到）</a>

                    <input type="checkbox" name="timingCheck" id="timingCheck" value="0" onclick="CheckTiming(this)"/>
                    <label for="timingCheck">定时群发</label>
                    <span id="dtbox" dispaly="none">
                        <input class="easyui-datetimebox" style="width: 150px;" id="timingSelector" />
                    </span>
                    
                    <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-reload" plain="true" onclick="SynWeixinSource()">
                    同步微信素材</a>

                <br />
                <span style="font-size: 12px; font-weight: normal">标题：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>
            <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        </th>
                        <th field="PicUrl" formatter="operate" width="10">
                            图片
                        </th>
                        <th field="Title" width="20">
                            标题
                        </th>
                        <th field="Url" width="30" formatter="gotowebsite">
                            链接
                        </th>
                        <th field="Description" width="30">
                            描述
                        </th>
                        <th field="operate" width="10" formatter="formartSendRecord">
                            发送记录
                        </th>
                    </tr>
                </thead>
            </table>
            <div id="dlgInput" class="easyui-dialog" closed="true" modal="true"  title="素材" style="width: 330px;
        padding: 15px;">
                <form id="fm" method="post" novalidate>
                <div style="margin-left: 20px">
                    <table>
                        <tr>
                            <td>
                                标题:
                            </td>
                            <td>
                                <input name="Title" id="txtSourceName" style="width:200px;" type="text" class="easyui-validatebox" required="true"
                                    missingmessage="请输入标题" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                图片:
                            </td>
                            <td>
                                <input type="text" id="txtImgSrc" value="" class="easyui-validatebox" required="true"
                                    missingmessage="请输入图片地址或上传本地图片" />
                                <input type="button" id="btnupload" value="上传图片" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                跳转链接:
                            </td>
                            <td>
                                <input name="Title" id="txtUrl" style="width:200px;" type="text" class="easyui-validatebox" required="true"
                                    missingmessage="点击图片跳转到的链接地址  格式如 http://www.host.com" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                描述:
                            </td>
                            <td>
                                <textarea id="txtDescription" style="width: 200px; height: 50px"></textarea>
                            </td>
                        </tr>
            
                    </table>
                </div>
                </form>
            </div>
        </div>
    </div>
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     KindEditor.ready(function (K) {
         var editor = K.editor({
             allowFileManager: true,
             uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx'

         });
         $('#btnupload').click(function () {
             editor.loadPlugin('image', function () {
                 editor.plugin.imageDialog({
                     showRemote: false,
                     imageUrl: $('#txtImgSrc').val(),
                     clickFn: function (url, title, width, height, border, align) {
                         $('#txtImgSrc').val(url);
                         editor.hideDialog();
                     }
                 });
             });
         });
     });
    </script>
    <script type="text/javascript">
        var grid;
        //处理文件路径
        var url = "/Handler/App/CationHandler.ashx";
        var currentAction = ""; //当前操作 add edit
        //加载文档
        jQuery().ready(function () {

            $('#dtbox').hide();

            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: url,
                height: document.documentElement.clientHeight - 112,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryNewsReplyImg", SearchTitle: "" }
            });
            //------------加载gridview




            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var SearchTitle = $("#txtName").val();
                grid.datagrid({ url: url, queryParams: { Action: "QueryNewsReplyImg", SearchTitle: SearchTitle} });
            });
            //搜索结束---------------------
            $('#dlgInput').dialog({
                buttons: [{
                    text: "确定",
                    handler: function () {
                        Save();
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgInput').dialog('close');
                    }
                }]
            });

        });
        //保存---------------------


        //弹出添加或编辑框开始
        function ShowAddOrEdit(addoredit) {
            var title = ""; //弹出框标题
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                Clear("txtSourceName|txtUrl|txtImgSrc|txtDescription");
                //设置弹出框标题
                title = "添加";

            }
            //弹出添加框结束

            //弹出编辑框开始
            else if (addoredit == "edit") {
                // 只能选择一条记录操作
                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    if (!EGCheckIsSelect(rows)) {
                        return;
                    }

                }
                if (num > 1) {
                    if (!EGCheckNoSelectMultiRow(rows)) {
                        return;
                    }
                }
                // 只能选择一条记录操作

                //加载信息开始
                $("#txtSourceName").val(rows[0].Title);
                $("#txtImgSrc").val(rows[0].PicUrl);
                $("#txtUrl").val(rows[0].Url);
                var description = replacebrtag(rows[0].Description);
                $("#txtDescription").val(description);
                //加载信息结束

                //设置弹出框标题
                title = "编辑";


            }
            //弹出编辑框结束

            currentAction = addoredit;
            $('#dlgInput').dialog({ title: title });
            $("#dlgInput").dialog("open");



        }
        //展示添加或编辑框结束


        //添加或编辑操作开始---------
        function Save() {
            var SourceName = $("#txtSourceName").val();
            var PicUrl = $("#txtImgSrc").val();
            var LinkUrl = $("#txtUrl").val();
            var Description = $("#txtDescription").val();

            //--检查输入---------------
            if (SourceName == "") {
                $("#txtSourceName").focus();
                return false;
            }
            if (PicUrl == "") {
                $("#txtImgSrc").focus();
                return false;
            }

            //----------执行添加操作开始
            if (currentAction == "add") {
                //------------添加
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "AddNewsReplyImg", SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "添加成功");
                            $("#dlgInput").dialog("close");
                            grid.datagrid('reload');
                        } else {
                            Alert(result);
                        }
                    }
                });
                //添加---------------
            }
            //-----------执行添加操作结束
            //-----------执行编辑操作开始
            else if (currentAction == "edit") {
                //-----------修改
                var rows = grid.datagrid('getSelections');
                var SourceID = rows[0].SourceID;
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "EditNewsReplyImg", SourceID: SourceID, SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "保存成功");
                            $("#dlgInput").dialog("close");
                            grid.datagrid('reload');
                        }
                        else {
                            Alert(result);
                        }
                    }
                });
                //修改
            }
            //--------------执行编辑操作结束

        }
        //添加或编辑操作结束---------

        // 删除---------------------
        function Delete() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {

                Alert("系统提示", "请选择您要删除的记录");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].SourceID);
            }

            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "DeleteNewsReplyImg", id: ids.join(',') },
                        success: function (result) {
                            if (result) {
                                messager('系统提示', "删除成功！");
                                grid.datagrid('reload');

                                return;
                            }
                            $.messager.alert("删除失败。");
                        }
                    });
                }
            });
        };
        // 删除---------------------

        // 群发---------------------
        function Broadcast() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0 || num > 10) {

                Alert("请选择您要群发的记录，不能超过10条");
                return;
            }

            if ($('#timingCheck').attr("checked") == "checked" && $('#timingSelector').datetimebox('getValue') == "") {
                Alert("请选择定时时间");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].SourceID);
            }

            $.messager.confirm("系统提示", "是否确定要群发选中信息?", function (r) {
                if (r) {

                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "BroadcastImageText", id: ids.join(','), isTiming: $('#timingCheck').attr("checked"), time: $('#timingSelector').datetimebox('getValue') },
                        success: function (result) {
                            if (result != "") {
                                Alert(result);
                                grid.datagrid('reload');
                                return;
                            }
                            Alert("群发失败");
                        }
                    });
                }
            });
        };


        function CheckTiming(obj) {
            if (obj.checked) {
                $('#dtbox').show();
            }
            else {
                $('#dtbox').hide();
            }
        }
        // 群发---------------------

        //网站图片--------------
        function operate(value, row, index) {

           
            var img = "<img src=" + value + " width=50 height=50 >";

            return img;

        }
        //网站图片--------------

        //网站链接--------------
        function gotowebsite(value, row, index) {


            var d = "<a href=" + value + " title=\"点击跳转\" target=_blank>" + value + "</a>";

            return d;

        }
        //网站链接--------------

        //同步微信素材
        function SynWeixinSource() {

            $.messager.confirm("系统提示", "确定同步微信素材?<br/>此过程可能需要几分钟?", function (r) {
                if (r) {
                    $.messager.progress({ text: '正在同步,请稍候。此过程可能需要几分钟...' });
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "SynWeixinNews" },
                        success: function (result) {
                            $.messager.progress('close');
                            Alert("本次同步了"+result+"条微信素材");
                            grid.datagrid('reload');

                        }
                    });
                }
            });
        };


        function formartSendRecord(value,row) {


            

        }
 
    </script>
</asp:Content>