<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CaseEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Case.CaseEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="CaseList.aspx">案例管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=Request["id"]=="0"?"新增":"编辑" %>案例</span>
    <a href="CaseList.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回案例列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" class="commonTxt" placeholder="标题(必填)"  value="<%=model.ActivityName %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" class="commonTxt"  value="<%=model.Summary %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=model.ThumbnailsPath %>" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*750。
                        <input type="file" id="txtThumbnailsPath" name="file1" class="hidden"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                                <%=model.ActivityDescription%>
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
                    <td style="width: 100px;" align="right" class="tdTitle">所属地区：
                    </td>
                    <td width="*" align="left">
                        <select id="selectProvince">
                            <option value="0">全国</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; vertical-align: top;" align="right" class="tdTitle">标签：
                    </td>
                    <td width="*" align="left">
                        <input id="txtTags" />
                        <a href="javascript:;" class="button button-primary button-rounded button-small" id="btnSelectTags">选择标签</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsHide" id="rdoIsNotHide" checked="checked" value="0" /><label for="rdoIsNotHide">显示</label>
                        <input type="radio" name="IsHide" id="rdoIsHide" value="1" /><label for="rdoIsHide">隐藏</label>
                    </td>
                </tr>
                 <tr>
                    <td  style="width: 100px;" align="right" class="tdTitle">
                        阅读数:
                    </td>
                    <td>
                    <input type="text" id="txtPv" value="<%=model.PV %>" class="commonTxt" placeholder="阅读数" style="width:100px;" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>


                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
    <div class="hidden warpTagDiv" style="border-radius: 8px;">
        <div class="warpTagSelect">
            <div class="warpContent">
                <div class="warpTagDataList hidden">
                    <div class="warpTagSelectBtn"><a href="javascripe:;" class="mLeft15 btnTagSelect" data-op="all">全选</a><a href="javascripe:;" class="mLeft10 btnTagSelect" data-op="reverse">反选</a></div>
                    <ul class="ulTagList">
                </ul>
                </div>
                <div class="warpNoData">
                    暂无数据
                </div>
                <div class="clear"></div>
            </div>
            <hr />
            <div class="warpOpeate">
                <a href="javascript:;" class="button button-primary button-rounded button-small btnSave">确定</a>
                <a href="javascript:;" class="button button-rounded button-small btnCancel">取消</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
    <script src="/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="/static-modules/lib/layer/layer.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "Handler/CaseHandler.ashx",
            currAcvityID = '<%=model.JuActivityID %>',
            editor,
            currProvince = '<%=model.ProvinceCode %>',
            currTagsStr = '<%=model.Tags %>',
            currTags = [],
            $document = $(document),
            $txtTags = $document.find('#txtTags'),
            $warpTagSelect = $document.find('.warpTagSelect');

        init();

        function init() {
            
            areaBind();
            //处理初始化tags
            if (currTagsStr != '') {
                currTags = currTagsStr.split(',');
            }
            if(currAcvityID != 0){
                $('#btnReset').hide();
            }
            $txtTags.tagsInput({
                height: '60px',
                width: 'auto',
                interactive: false,
                onAddTag: function (tag) {
                    //currTags.push(tag);
                    console.log('添加了' + tag);
                },
                onRemoveTag: function (tag) {
                    currTags.RemoveItem(tag);
                    console.log('删除了' + tag);
                }
            });

            addTagList(currTags);
            ShowEdit();
            bindEvent();
            
        }

        function areaBind() {
            //初始化省份
            $.ajax({
                type: 'POST',
                url: handlerUrl,
                data: { Action: "GetGetKeyVauleDatas", type: 'province' },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if(data.totalcount && data.totalcount>0){
                        var pro = $("#selectProvince");
                        for (var i = 0; i < data.list.length; i++) {
                            var strHtml = new StringBuilder();
                            var selectStr = currProvince == data.list[i].id?'selected="selected"':'';
                            strHtml.AppendFormat('<option value="{0}" {2}>{1}</option>',data.list[i].id,data.list[i].name,selectStr);
                            pro.append(strHtml.ToString());
                        }
                    }
                    chosenBind({ id: '#selectProvince', placeholder: '选择区域' ,width:'200px'});
                }
            });
        }
        function chosenBind(data) {
            $document.find(data.id).attr('data-placeholder', data.placeholder).chosen({
                no_results_text: '没有找到结果',
                width: data.width
            });
        }

        function bindEvent() {

            $document.on('click', '#btnSave', function () {
                saveData();
            });

            $document.on('click', '#btnReset', function () { ResetCurr(); });
            
            $document.on('change', '#txtThumbnailsPath', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
            //标签操作按钮
            $document.on('click', '#btnSelectTags', function () {
                loadSelectTagsData();
            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnSave', function () {
                //构造标签新数组
                currTags = [];
                var chekList = $('.warpTagSelect .tagChk');
                for (var i = 0; i < chekList.length; i++) {
                    if ($(chekList[i]).attr('checked')) {
                        currTags.push($(chekList[i]).val());
                    }
                }

                //显示标签
                tagClear();
                addTagList(currTags);
                layer.closeAll();

            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnCancel', function () {
                layer.closeAll();
            });

            $document.on('click', '.warpTagSelect .btnTagSelect ', function () {
                var op = $(this).attr('data-op');
                if (op == 'all')
                    selectTagAll();
                if (op == 'reverse')
                    selectTagReverse();

            });

        }

        var $ulTagList = $warpTagSelect.find('.ulTagList'), $warpNoTagData = $warpTagSelect.find('.warpNoData'), $warpTagDataList = $warpTagSelect.find('.warpTagDataList');
        function loadSelectTagsData() {
            $.ajax({
                type: 'POST',
                url: '/Handler/App/CationHandler.ashx',
                data: { Action: "QueryMemberTag", TagType: 'all', page: 1, rows: 100000000 },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if (data.total == 0) {
                        $warpNoTagData.show();
                        $warpTagDataList.hide();
                    } else {
                        $warpTagDataList.show();
                        $warpNoTagData.hide();

                        //构造数据
                        var strHtml = new StringBuilder();
                        for (var i = 0; i < data.rows.length; i++) {
                            strHtml.Append('<li class="overflow_ellipsis"><label>');
                            strHtml.AppendFormat('<input type="checkbox" name="tag" class="tagChk" value="{0}" {1} />{0}', data.rows[i].TagName, currTags.Contains(data.rows[i].TagName) ? 'checked' : '');
                            strHtml.Append('</label></li>');
                        }

                        $ulTagList.html(strHtml.ToString());


                    }


                    var tagDiv = $.layer({
                        type: 1,
                        shade: [0.2, '#000'],
                        shadeClose: true,
                        area: ['300', '300'],
                        title: ['选择标签', 'background:#1B9AF7; color:#fff;'],
                        border: [0],
                        page: { dom: '.warpTagDiv' }
                    });


                }
            });
        }

        function addTagList(list) {
            for (var i = 0; i < list.length; i++) {
                if (!$txtTags.tagExist(list[i]))
                    $txtTags.addTag(list[i]);
            }
        }

        function tagClear() {
            $txtTags.importTags('');
        }

        //标签全选
        function selectTagAll() {
            $('.warpTagSelect .tagChk').attr('checked', true);
        }

        //标签反选
        function selectTagReverse() {
            $('.warpTagSelect .tagChk').each(function () {
                var $this = $(this),
                    v = $this.attr('checked');
                $this.attr('checked', !v);
            });
        }

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var model = {
                Action:'EditCase',
                ActivityName: $.trim($('#txtActivityName').val()),
                JuActivityID: <% =model.JuActivityID %>,
                IsHide: rdoIsHide.checked ? 1 : 0,
                ActivityDescription: editor.html(),
                Summary: $("#txtSummary").val(),
                Tags: currTags.join(','),
                PV: $("#txtPv").val(),
                ThumbnailsPath: $("#imgThumbnailsPath").attr("src"),
                ProvinceCode: $("#selectProvince").val()
            };
            setTimeout(function () {
                if (model.ActivityName == '') {
                    $('#txtActivityName').focus();
                    alert('标题不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                if(model.ThumbnailsPath == ''){
                    alert('请上传缩略图', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                //$.messager.progress({
                //    text: '正在处理...'
                //});
                //layer.load('正在处理...');
                //var loadi = layer.load(5, 0);
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        //layer.closeLoad();
                        //$.messager.progress('close');
                        if (resp.Status == 1) {
                            if (model.JuActivityID == '0')
                            { 
                                ResetCurr();
                            }
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            }, 400);

        }

        function ShowEdit() {
            if ('<%= model.IsHide %>' == '1') {
                rdoIsHide.checked = true;
            } else {
                rdoIsNotHide.checked = true;
            }
        }

        function ResetCurr() {
            ClearAll();
            $document.find('#selectProvince').chosen('destroy');
            chosenBind({ id: '#selectProvince', placeholder: '选择类型' ,width:'200px'});

            currTags =[];
            tagClear();

            $("#txtPv").val(0);
            editor.html('');
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'
                ],
                filterMode: false
            });
        });

    </script>
</asp:Content>
