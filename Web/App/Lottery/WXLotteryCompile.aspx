<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXLotteryCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="/kindeditor-4.1.10/kindeditor.js" type="text/javascript"></script>
    <script src="/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var currID = '<%=model.AutoID%>';
        var editor;
        var editorup;
        $(function () {

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        AutoID: currID,
                        ThumbnailsPath: $('#imgThumbnailsPath').attr('src'),
                        LotteryName: $.trim($('#txtLotteryName').val()),
                        LotteryTitle: $.trim($('#txtLotteryTitle').val()),
                        ScratchUpAreaContent: editorup.html(),
                        ScratchDownAreaContent: editor.html(),
                        Action: currAction == 'add' ? 'AddWXLottery' : 'EditWXLottery',
                        Status: $("input[name=rdostatus]:checked").val(),
                        PrizeSet: $.trim($('#txtPrizeSet').val()),
                        MaxCount: $.trim($('#txtMaxCount').val()),
                        LotteryActivityID:$.trim($("#txtLotteryActivityID").val())
                    };

                    if (model.LotteryName == '') {

                        Alert('请输入刮奖活动名称！');
                        return;
                    }
                    if (model.MaxCount == '') {

                        Alert('请输入每个用户最多刮奖次数！');
                        return;
                    }

                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            $.messager.progress('close');
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {

                                if (currAction == 'add')
                                    ResetCurr();
                                Alert(resp.Msg);
                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });

            $('#btnReset').click(function () {
                ResetCurr();

            });

            if (currAction == "edit") {
                var status = "<%=model.Status%>";
                if (status == "1") {
                    $("#rdostart").attr("checked","checked");
                }
                else {
                    $("#rdostop").attr("checked", "checked");
                }

            }

            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }

            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=LotteryImage',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');

                             try {
                                 result = result.substring(result.indexOf("{"), result.indexOf("</"));
                             } catch (e) {
                                 alert(e);
                             }
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 imgThumbnailsPath.src = resp.ExStr;
                                 $('#imgThumbnailsPath').attr('path', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    alert(e);
                }
            });

        });

        function ResetCurr() {
            $(":input[type!=radio]").val("");
            editor.html("");
            editorup.html("");
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
               uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });

        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
           // $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
        }

    </script>
    <style type="text/css">
         body{font-family:微软雅黑;}
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
        input[type=text]
        {
        height:30px;    
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="/App/Lottery/WXLotteryMgr.aspx" >刮奖活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (model != null && webAction == "edit") { Response.Write("：" + model.LotteryName); } %></span>
     <a href="WXLotteryMgr.aspx" style="float:right;margin-right:20px;color:Black;" title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" >
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">


        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        刮奖活动名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtLotteryName" value="<%=model==null?"":model.LotteryName %>" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtLotteryTitle" value="<%=model==null?"":model.LotteryTitle%>" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=string.IsNullOrEmpty(model.ThumbnailsPath)?"/img/hb/1.jpg":model.ThumbnailsPath%>" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                       
                        <input type="file" id="txtThumbnailsPath" name="file1" />

                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        刮奖区域上方内容：
                    </td>
                    <td width="*" align="left">
                    <div id="UpArea" style="width: 100%; height: 400px;">
                    <%=model == null ? "" : model.ScratchUpAreaContent%>
                    </div>
                    </td>
                </tr>

               
               
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            刮奖区域下方内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            <%=model == null ? "" : model.ScratchDownAreaContent%>
                            </div>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdostatus" id="rdostart" checked="checked" value="1" /><label
                            for="rdostart">进行中</label>
                        <input type="radio" name="rdostatus" id="rdostop" value="0" /><label for="rdostop">已停止</label>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />已停止的活动不能刮奖
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        每个用户最多刮奖次数：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMaxCount" value="<%=model.MaxCount==0?1:model.MaxCount%>" class="" style="width: 100px;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        奖项设置：
                    </td>
                    <td width="*" align="left">
                       <textarea id="txtPrizeSet" style="width: 100%;;height:50px;"><%=model==null?"":model.PrizeSet%></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        关联活动编号：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtLotteryActivityID" value="<%=model.LotteryActivityID%>" class="" style="width: 100px;" />提示：(如果填写,则只有签到过的人员才能刮奖) 请在活动管理->所有活动->找到活动编号
                    </td>
                </tr>
                 
                
               
            </table>
           
            <br />
           
            <table align="center">
                <tr>
                    <td>
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-flat-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;width:200px;" class="button button-rounded button-flat">
                                重置</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

