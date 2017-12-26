<%@ Page Title="新增活动抽奖页面" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LuckDrawCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.LuckDrawCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="/app/Lottery/css/admincomm.css?v=20161014" />
    <style type="text/css">
        .color {
            width: 120px;
            width: 120px;
        }

        .lbTip {
            padding: 3px 6px;
            background-color: #5C5566;
            color: #fff;
            font-size: 14px;
            border-radius: 50px;
            cursor: pointer;
            margin-left: 20px;
        }


        .layui-layer-tips .layui-layer-content {
            background-color: #5C5566 !important;
            border-bottom-color: #5C5566 !important;
        }

        .layui-layer-tips i.layui-layer-TipsL, .layui-layer-tips i.layui-layer-TipsR {
            border-bottom-color: #5C5566 !important;
        }

        input[type='text'], input[type='number'] {
            height: 23px;
            width: 98%;
        }

        .startfile img {
            width: 18px;
            height: 18px;
            cursor: pointer;
            position: relative;
            top: 4px;
        }

        .startfile img, .stopfile img {
            width: 18px;
            height: 18px;
            cursor: pointer;
            position: relative;
            top: 4px;
        }
        
         .sp-replacer{
             width:20%;
         }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:history.go(-1);">抽奖活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>新建抽奖</span>
    <a href="javascript:history.go(-1);" style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 112px;" align="right" class="tdTitle">
                        <span class="colorRed">*</span>抽奖活动名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" class="form-control" id="txtLotteryName" placeholder="抽奖活动名称(必填)" />
                    </td>
                </tr>

                <tr class="wrapImgThumbnailsPath">
                    <td style="width: 100px;" align="right" class="tdTitle">背景图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="抽奖背景图片" style="width: 250px; height: 160px;" src="http://static-files.socialcrmyun.com/img/lottery/light-bg.jpg" accept="image/*" id="imgThumbnailsPath" /><br />

                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="l-btn-text icon-delete"
                            plain="true" onclick="dBgImg()">清除图片</a>

                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*580。

                        <input type="file" id="txtThumbnailsPath" name="file1" style="display: none" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            介绍：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 112px;" align="right" class="tdTitle">
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file1Upload()">上传抽奖音乐:</a>
                        <input type="file" style="display: none" id="startMusic" name="file1" />
                    </td>
                    <td width="*" align="left">
                        <div class="startfile">
                            <div class="stop"><span class="startmusicurl">http://open-files.comeoncloud.net/www/hf/jubit/image/20161229/BCCD7437E8A44AF5BC2770DE76A3597B.mp3</span><img src="/img/delete.png" alt="删除"/></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 112px;" align="right" class="tdTitle">
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file2Upload()">上传中奖音乐:</a>
                        <input type="file" style="display: none" id="stopMusic" name="file1" />
                    </td>
                    <td width="*" align="left">
                        <div class="stopfile">
                            <div class="stop"><span class="stopmusicurl">http://open-files.comeoncloud.net/www/hf/jubit/image/20161229/F1F7F30BB1C34307B5EDD35C486C17DE.mp3</span><img src="/img/delete.png" alt="删除"/></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 112px;" align="right" class="tdTitle">主题背景颜色：
                    </td>
                    <td width="*" align="left">
                        <div class="rows">
                            <div class="col-xs-12">
                                <input type="text" value="#ff0000" class="color" id="bgcolor" /><span class="lbTip" data-tip-msg="<b>说明</b><br>1.同步抽奖页面的标题背景颜色<br>2.同步抽奖页面的按钮背景颜色">?</span>
                            </div>

                        </div>


                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">主题字体颜色：
                    </td>
                    <td width="*" align="left">
                        <div class="rows">
                            <div class="col-xs-12">
                                <input type="text" value="#ffffff" class="color" id="fontcolor" /><span class="lbTip" data-tip-msg="<b>说明</b><br>1.同步抽奖页面的标题字体颜色<br>2.同步抽奖页面的按钮字体颜色<br>3.同步抽奖页面的用户昵称字体颜色<br/>">?</span>
                            </div>

                        </div>


                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">转动框背景颜色：
                    </td>
                    <td width="*" align="left">
                        <div class="rows">
                            <div class="col-xs-12">
                                <input type="text" class="color" id="userbgcolor" /><span class="lbTip" data-tip-msg="<b>说明</b><br>1.同步抽奖页面正在转动框的背景颜色<br>">?</span>
                            </div>

                        </div>


                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">手动设置状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdostatus" id="rdostart" checked="checked" value="1" /><label
                            for="rdostart">进行中</label>
                        <input type="radio" name="rdostatus" id="rdostop" value="0" /><label for="rdostop">已停止</label>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />已停止的活动不能抽奖（手动停止后忽略设置的停止时间）
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 112px;" align="right" class="tdTitle">一次抽中数量：
                    </td>
                    <td width="*" align="left">
                        <input type="number" class="form-control" id="txtOneWinnerCount" value="1" placeholder="(必填)" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">中奖名单：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" value="0" checked="checked" class="positionTop2" name="winning" id="hide" /><label for="hide">隐藏</label>
                        <input type="radio" value="1" class="positionTop2" name="winning" id="pleft" /><label for="pleft">左侧显示</label>
                        <input type="radio" value="2" class="positionTop2" name="winning" id="pright" /><label for="pright">右侧显示</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">二维码：
                    </td>
                    <td width="*" align="left">
                        <div>
                            <input type="radio" value="0" class="positionTop2" name="qrcode" id="scode" /><label for="scode">显示</label>
                            <input type="radio" value="1" checked="checked" class="positionTop2" name="qrcode" id="hcode" /><label for="hcode">隐藏</label>
                        </div>
                        <div>
                            <input type="radio" value="0" class="positionTop2" checked="checked" name="yellow" id="pnumber" /><label for="pnumber">关注公众号二维码</label>
                            <input type="radio" value="1" class="positionTop2" name="yellow" id="page" /><label for="page">H5页面二维码</label>
                        </div>
                        <div class="showdistributor">
                            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="selectDistributor()">选择分销员</a>
                            <span  id="disUserId"></span>
                            <span class="lbTip" data-tip-msg="<b>说明</b><br>1.绑定分销员以后关注公众号进来的用户直接成为该分销员的下线<br>">?</span>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">是否显示标题：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" value="0" checked="checked" class="positionTop2" name="title" id="stitle" /><label for="stitle">显示</label>
                        <input type="radio" value="1" class="positionTop2" name="title" id="htitle" /><label for="htitle">隐藏</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
            </table>

            <div class="wrapOperate">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary">保存</a>
                <a href="javascript:;" id="btnReset" class="button button-rounded button-flat">重置</a>
            </div>
        </div>
    </div>

    <div id="dlgDistributionOwner" class="easyui-dialog" closed="true" title="" data-options="iconCls:'icon-tip'" modal="true" style="width: 675px; padding: 15px;">
          姓名:<input type="text" id="txtKeyWord" style="width: 150px;" placeholder="姓名" />

            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">
                查询</a>
        <br />
           <br />
        <table id="grvDistributionOwnerData" fitcolumns="true">
        </table>
    </div>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js"></script>
    <script type="text/javascript" src="/lib/layer/2.1/layer.js"></script>
    <script type="text/javascript">

        var lotteryType = '<%=lotteryType%>';//空和1为刮奖，2为抽奖
        var handlerUrl = "/serv/api/admin/lottery/LuckDraw/add.ashx";
        var url = '/serv/api/common/file.ashx?action=Add';

        var editor;
        $(function () {
            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });


            

            $('input[name=yellow]').click(function () {
                var type = $(this).val();
                if (type == 0) {
                    $('.showdistributor').show();
                } else {
                    $('.showdistributor').hide();
                    $('#disUserId').text('');
                }
            });
            $('#btnSave').click(function () {
                try {
                    var model = GetModel();

                    if (model.lottery_name == '') {
                        Alert('请输入抽奖活动名称！');
                        return;
                    }
                    if (model.qrcode == '1') {
                        model.dis_userid = '';
                    }
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                layerAlert('添加成功');
                                $('#btnReset').click();
                            }
                            else {
                                layerAlert('添加出错');
                            }
                        },
                        complete: function () {
                            $.messager.progress('close');
                        }

                    });

                } catch (e) {
                    layerAlert(e);
                }
            });

            $('#btnReset').click(function () {
                $('#txtLotteryName').val('');
                editor.html('');
                $("input[name=rdostatus][value=1]").attr("checked", true);
                $("input[name=winning][value=0]").attr("checked", true);
                $('#imgThumbnailsPath').attr('src', 'http://static-files.socialcrmyun.com/img/lottery/light-bg.jpg');
                $("input[name=qrcode][value=1]").attr("checked", true);
                $("input[name=title][value=0]").attr("checked", true);
                $("input[name=yellow][value=0]").attr("checked", true);
                $('#bgcolor').val('#ff0000');
                $('#fontcolor').val('#ffffff');
                $('#userbgcolor').val('');
                $('#txtOneWinnerCount').val('1');
                $('.start').remove();
                $('.stop').remove();
                $('#disUserId').text('');
            });


            $('#imgThumbnailsPath').click(function () {
                $('#txtThumbnailsPath').click();
            });

            $(document).on('change', '#txtThumbnailsPath', function () {
                var layerIndex = layer.load(0, { shade: false });
                $.ajaxFileUpload({
                    url: url,
                    secureuri: false,
                    fileElementId: 'txtThumbnailsPath',
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $('#imgThumbnailsPath').attr('src', result.file_url_list[0]);
                        }
                        else {
                            layerAlert(result.errmsg);
                        }
                    }
                });
            });

            $(document).on("change", "#startMusic", function () {
                try {
                    $.messager.progress({ text: '正在上传音乐...' });
                    $.ajaxFileUpload(
                            {
                                url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Music&filegroup=bgMusic',
                                secureuri: false,
                                fileElementId: 'startMusic',
                                dataType: 'json',
                                success: function (resp) {
                                    $.messager.progress('close');
                                    if (resp.Status == 1) {
                                        var text = '<div class="start"><span class="startmusicurl">' + resp.ExStr + '</span><img  src="/img/delete.png" alt="删除"/></div>';
                                        $(".startfile").html(text);
                                    }
                                    else {
                                        layerAlert(resp.Msg);
                                    }
                                }
                            }
                           );

                } catch (e) {
                    alert(e);
                }

            });
            $(document).on("change", "#stopMusic", function () {
                try {
                    $.messager.progress({ text: '正在上传音乐...' });
                    $.ajaxFileUpload(
                            {
                                url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Music&filegroup=bgMusic',
                                secureuri: false,
                                fileElementId: 'stopMusic',
                                dataType: 'json',
                                success: function (resp) {
                                    $.messager.progress('close');
                                    if (resp.Status == 1) {
                                        var text = '<div class="stop"><span class="stopmusicurl">' + resp.ExStr + '</span><img src="/img/delete.png" alt="删除"/></div>';
                                        $(".stopfile").html(text);
                                    }
                                    else {
                                        layerAlert(resp.Msg);
                                    }
                                }
                            }
                           );

                } catch (e) {
                    alert(e);
                }

            });
            
            $(document).on('click', '.start img,.stop img', function () {
                var obj = $(this);
                $.messager.confirm('系统提示', '确定要删除吗?', function (o) {
                    if (o) {
                        $(obj).parent().remove();
                    }
                });
            });
            $("#dlgDistributionOwner").dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $('#grvDistributionOwnerData').datagrid('getSelections');
                        if (rows.length <= 0) {
                            $.messager.alert("系统提示","请选择分销员!","warning");
                            return;
                        }
                        $('#disUserId').text(rows[0].AutoID);
                        $('#dlgDistributionOwner').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistributionOwner').dialog('close');
                    }
                }]

            });



        });


        //分销员列表
        function selectDistributor() {
  
            $('#dlgDistributionOwner').dialog({ title: '分销员列表' });
            $('#dlgDistributionOwner').dialog('open');
 
            ShowDisDataGrid();
        }
        function ShowDisDataGrid() {
            $('#grvDistributionOwnerData').datagrid(
               {
                   method: "Post",
                   url: "/Handler/App/CationHandler.ashx",
                   queryParams: { Action: "QueryWebsiteUserDistributionOnLine",isDistributionOwner: 2 },
                   height: 300,
                   pagination: true,
                   striped: true,
                   pageSize: 10,
                   rownumbers: true,
                   singleSelect: true,
                   rowStyler: function () { return 'height:25px'; },
                   columns: [[
                                  { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
                                  {
                                      field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                          if (value == '' || value == null)
                                              return "";
                                          var str = new StringBuilder();
                                          str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                          return str.ToString();
                                      }
                                  },
                                  { field: 'WXNickname', title: '微信昵称', width: 60, align: 'left', formatter: FormatterTitle },
                                  { field: 'TrueName', title: '真实姓名', width: 80, align: 'left', formatter: FormatterTitle },
                                  { field: 'Phone', title: '手机', width: 90, align: 'left', formatter: FormatterTitle }
                   ]]
               }
           );
        }        function Search() {
            $('#grvDistributionOwnerData').datagrid(
                      {
                          method: "Post",
                          url: "/Handler/App/CationHandler.ashx",
                          queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $(txtKeyWord).val(), isDistributionOwner: 2 }
                      });
        }
        function file1Upload() {
            $('#startMusic').click();
        }        function file2Upload() {
            $('#stopMusic').click();
        }

        function GetModel() {
            var model =
                    {
                        lottery_type: lotteryType,
                        lottery_name: $.trim($('#txtLotteryName').val()),
                        lottery_content: editor.html(),
                        lottery_status: $("input[name=rdostatus]:checked").val(),
                        is_hideWinningList: $('input[name=winning]:checked').val(),
                        backgroud_img: $('#imgThumbnailsPath').attr('src'),
                        ishide_qrcode: $("input[name=qrcode]:checked").val(),
                        ishide_title: $("input[name=title]:checked").val(),
                        bgcolor: $('#bgcolor').val(),
                        fontcolor: $('#fontcolor').val(),
                        user_bgcolor: $('#userbgcolor').val(),
                        one_winnercount: $('#txtOneWinnerCount').val(),
                        qrcode: $("input[name=yellow]:checked").val(),
                        start_music: $('.startmusicurl').text(),
                        stop_music: $('.stopmusicurl').text(),
                        dis_userid: $('#disUserId').text()
                    };
          


            return model;

        }


        function dBgImg() {
            $('#imgThumbnailsPath').attr('src', '');
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

    </script>
</asp:Content>
