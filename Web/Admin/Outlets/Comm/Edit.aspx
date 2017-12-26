<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Outlets.Comm.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
        <%}%>
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
    <style type="text/css">
        .Width92P {
            width: 92% !important;
        }
        .DivTextarea {
            width: 98% !important;
            padding: 5px;
            min-height: 94px;
            line-height: 21px;
            border: 1px solid rgb(180, 180, 180);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=typeConfig.CategoryTypeDispalyName %><% =Request["id"]=="0"?"新增":"编辑" %>
    <a href="List.aspx?type=<% =Request["type"] %>" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <% StringBuilder strHtml = new StringBuilder();%>
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable" style="width: 800px;">
        <%  
            List<string> others = new List<string>() { "CategoryId,Tags,Sort" };
            bool first = true;
            strHtml = new StringBuilder();
            foreach (var item in formField.Where(p => !others.Contains(p.Field)))
            {
                if (first)
                {
                    strHtml.AppendLine(string.Format("<tr><td style=\"width: 140px;\" align=\"right\" class=\"tdTitle\">{0}：</td><td width=\"*\" align=\"left\">", item.MappingName));
                    first = false;
                }
                else
                {
                    strHtml.AppendLine(string.Format("<tr><td align=\"right\" class=\"tdTitle\">{0}：</td><td width=\"*\" align=\"left\">", item.MappingName));
                }

                string nValue = nInfoJtoken[item.Field].ToString();
                bool nIsReadOnly = item.IsReadOnly == 1 && id != "0" && !string.IsNullOrWhiteSpace(nValue);
                if (item.FieldType == "select" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("<select id='ddl{0}' {1}>", item.Field, nIsReadOnly ? "disabled=\"disabled\"" : ""));
                    strHtml.AppendLine("<option value=''></option>");
                    foreach (var opt in item.Options.Split(','))
                    {
                        strHtml.AppendLine(string.Format("<option value='{0}' {1}>{0}</option>", opt, nValue.Equals(opt) ? "selected=\"selected\"" : ""));
                    }
                    strHtml.AppendLine("</select>");
                }
                else if (item.FieldType == "radio" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("<ul id='ul{0}'>", item.Field));
                    List<string> optl = item.Options.Split(',').ToList();
                    for (int i = 0; i < optl.Count; i++)
                    {
                        strHtml.AppendLine(string.Format("<li style=\"float:left;\"><input type=\"radio\" name=\"rdo{0}\" id=\"rdo{0}{1}\" " +
                            " class=\"positionTop2 hand\" value=\"{2}\" {3} {4} /><label class=\"hand\" for=\"rdo{0}{1}\">{2}</label></li>",
                            item.Field,
                            i + 1,
                            optl[i],
                            nValue.Equals(optl[i]) ? "checked=\"checked\"" : "",
                            nIsReadOnly ? "disabled=\"disabled\"" : ""));
                    }
                    strHtml.AppendLine("</ul>");
                }
                else if (item.FieldType == "checkbox" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("<ul id='ul{0}'>", item.Field));
                    List<string> optl = item.Options.Split(',').ToList();
                    List<string> nvl = nValue.Split(',').ToList();
                    for (int i = 0; i < optl.Count; i++)
                    {
                        strHtml.AppendLine(string.Format("<li style=\"float:left;\"><input type=\"checkbox\" name=\"chk{0}\" id=\"chk{0}{1}\" " +
                            " class=\"positionTop2 hand\" value=\"{2}\" {3} /><label class=\"hand\" for=\"chk{0}{1}\">{2}</label></li>",
                            item.Field,
                            i + 1,
                            optl[i],
                            nvl.Contains(optl[i]) ? "checked=\"checked\"" : "",
                            nIsReadOnly ? "disabled=\"disabled\"" : ""));
                    }
                    strHtml.AppendLine("</ul>");
                }
                else if (item.FieldType == "img")
                {
                    strHtml.AppendLine("<div class=\"ui-sortable\"><div style=\"position: relative;\">");
                    strHtml.AppendLine(string.Format("<img alt=\"460*340\" id=\"imgImg{0}\" src=\"{1}\" width=\"138px\" height=\"102px\" class=\"rounded {2}\" data-field=\"{0}\" />",
                        item.Field,
                        string.IsNullOrWhiteSpace(nValue) ? "/img/hb/hb1.jpg" : nValue,
                        nIsReadOnly ? "" : "upImg"));
                    if (!nIsReadOnly) strHtml.AppendLine(string.Format("<br /><input type=\"text\" id=\"txtImg{0}\" class=\"commonTxt upImgTxt\" value=\"{1}\" data-field=\"{0}\" />", item.Field, nValue));
                    if (!nIsReadOnly) strHtml.AppendLine(string.Format("<input type=\"file\" id=\"fileImg{0}\" class=\"file file1 upImgFile\" name=\"file1\" style=\"display: none;\" data-field=\"{0}\" />", item.Field));
                    strHtml.AppendLine("</div></div>");
                }
                else if (item.FieldType == "mult")
                {
                    strHtml.AppendLine(string.Format("<div id=\"txt{0}\" class=\"DivTextarea\" {4}>{2}</div>",
                        item.Field,
                        item.MappingName,
                        nValue,
                        item.FieldIsNull == 1 ? "必填" : "选填",
                        item.IsReadOnly == 1 && id != "0" ? "" : "contenteditable=\"plaintext-only\""));
                }
                else
                {
                    strHtml.AppendLine(string.Format("<input type=\"text\" id=\"txt{0}\" class=\"commonTxt\" placeholder=\"{1}({3})\" value=\"{2}\" {4} />",
                        item.Field,
                        item.MappingName,
                        nValue,
                        item.FieldIsNull == 1 ? "必填" : "选填",
                        item.IsReadOnly == 1 && id != "0" ? "readonly=\"readonly\"" : ""));
                }
                strHtml.AppendLine("</td></tr>");
            }
            this.Response.Write(strHtml.ToString());
            %>
        <%if (formField.Exists(p => p.Field.Equals("CategoryId")))
        {%>
                <tr>
                    <td align="right" class="tdTitle">分类：
                        <textarea
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>
        <%}%>
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
                <tr>
                    <td style="width: 100px; vertical-align: top;" align="right" class="tdTitle">标签：
                    </td>
                    <td width="*" align="left">
                        <input id="txtTags" />
                        <a href="javascript:;" class="button button-primary button-rounded button-small" id="btnSelectTags">选择标签</a>
                    </td>
                </tr>
        <%}%>
        <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
                <tr>
                    <td align="right" class="tdTitle">坐标：</td>
                    <td width="*" align="left" valign="bottom">
                        <div id="ifAmap" style="width: 500px; height: 450px;"></div>
                        <div>
                            地址：<input id="txtMapKeyword" type="text" style="width: 200px; height: 24px;" /><a href="javascript:void(0);" class="easyui-linkbutton" onclick="placeSearch()">搜索</a>
                        </div>
                        <div>
                            经度：<span id="spanLongitude" style="color: red;"><%= nInfo.UserLongitude %></span>
                            纬度：<span id="spanLatitude" style="color: red;"><%= nInfo.UserLatitude %></span>
                        </div>
                    </td>
                </tr>
            <%}%>
            <%
                var sortField = formField.FirstOrDefault(p => p.Field.Equals("Sort"));
                if (sortField!=null)
            {%>
                <tr>
                    <td align="right" class="tdTitle">排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSort" class="commonTxt" placeholder="排序" value="<%=nInfo.Sort.HasValue?nInfo.Sort.Value:0 %>" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'')" />
                        数字越大越排前
                    </td>
                </tr>
            <%}%>
            </table>
        </div>
        <div style="text-align: center;">
            <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width: 160px;">提交</a>
            <a href="List.aspx?type=<% =Request["type"] %>" id="btnPageBack" class="button glow" style="width: 160px;">返回</a>
        </div>
    </div>
    
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
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
        <%}%>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <% 
        StringBuilder strHtml = new StringBuilder();
        List<string> others = new List<string>() { "CategoryId,Tags,Sort" };
       %>
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
    <script src="/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
        <%}%>
    <script src="/lib/layer/2.1/layer.js" type="text/javascript"></script>
        <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
        <%}%>
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/outlets/comm/';
        var nid = '<% =nInfo.JuActivityID%>';
        var type = '<%=Request["type"]%>';
        <%
        strHtml = new StringBuilder();
        if (formField.Exists(p => p.Field.Equals("CategoryId")))
        {
            strHtml.AppendLine(string.Format("var nCateId = '{0}';", nInfo.CategoryId));
        }
        if (formField.Exists(p => p.Field.Equals("Tags")))
        {
            strHtml.AppendLine(string.Format("var currTagsStr = '{0}';", nInfo.Tags));
            strHtml.AppendLine("var currTags = [];");
        }
        if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {
            if (!string.IsNullOrWhiteSpace(nInfo.UserLongitude) && !string.IsNullOrWhiteSpace(nInfo.UserLatitude))
            {
                strHtml.AppendLine(string.Format("var nLongitude = {0};", nInfo.UserLongitude));
                strHtml.AppendLine(string.Format("var nLatitude = {0};", nInfo.UserLatitude));
            }
            else
            {
                strHtml.AppendLine("var nLongitude = '121.472644';");
                strHtml.AppendLine("var nLatitude = '31.231706';");
            }
            strHtml.AppendLine("var mapObj;");
            strHtml.AppendLine("var marker;");
            strHtml.AppendLine("var toPlaceSearch;");
        }
        this.Response.Write(strHtml.ToString());
        %>

        $(function () {
        <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
            var opt = { zoom: 15 };
            if (nLongitude != "" && nLatitude != "") {
                opt.center = new AMap.LngLat(nLongitude, nLatitude);
            }
            else {
                opt.center = new AMap.LngLat("121.472644", "31.231706");
            }
            mapObj = new AMap.Map("ifAmap", opt);

            if (nLongitude != "" && nLatitude != "") {
                marker = new AMap.Marker({
                    position: [nLongitude, nLatitude]
                });
                marker.setMap(mapObj);
            }
            AMap.event.addListener(mapObj, 'click', getLnglat); //点击事件

            AMap.service(["AMap.PlaceSearch"], function () { //加载地理编码
                toPlaceSearch = new AMap.PlaceSearch({
                    city: '上海',
                    pageSize: 1,
                    pageIndex: 1,
                    extensions: 'base'
                });
            });
            <%}%>
        <%if (formField.Exists(p => p.Field.Equals("CategoryId")))
        {%>
            if (nCateId != "") { $("#ddlCate").val(nCateId); }
            
        <%}%>
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
            if (currTagsStr != '') {
                currTags = currTagsStr.split(',');
            }

            //areaBind();
            $('#txtTags').tagsInput({
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

            //标签操作按钮
            $("#btnSelectTags").bind('click', function () {
                loadSelectTagsData();
            });

            $(".warpTagSelect .warpOpeate .btnSave").bind('click', function () {

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

            $(".warpTagSelect .warpOpeate .btnCancel").bind('click', function () {
                layer.closeAll();
            });

            $(".warpTagSelect .btnTagSelect").bind('click', function () {
                var op = $(this).attr('data-op');
                if (op == 'all')
                    selectTagAll();
                if (op == 'reverse')
                    selectTagReverse();
            });
        <%}%>

            $("#btnSave").live("click", function () {
                postUpdateData();
            });
            
        <%if (formField.Exists(p => !string.IsNullOrWhiteSpace(p.FieldType) && p.FieldType.Equals("img")))
        {%>
            
            $(".upImg").live("click", function () {
                var nfield = $(this).attr("data-field");
                var fileId = 'fileImg'+nfield;
                $("#"+fileId).click();
            })
            $(".upImgTxt").live("change", function () {
                var nfield = $(this).attr("data-field");
                var imgId = 'imgImg'+nfield;
                var filep = $.trim($(this).val());
                if(filep == ""){
                    $("#"+imgId).attr("src", "/img/hb/hb1.jpg");
                }
                else
                {
                    $("#"+imgId).attr("src", filep);
                }
            })
            $("#mainTable .upImgFile").live('change', function () {
                var nfield = $(this).attr("data-field");
                var fileId = $(this).attr("id");
                var imgId = 'imgImg'+nfield;
                var txtId = 'txtImg'+nfield;
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?Action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: fileId,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $("#"+imgId).attr("src", resp.ExStr);
                                 $("#" + txtId).val(resp.ExStr);
                             }
                             else {
                                 alert(resp.Msg);
                             }
                         }
                     });

                } catch (e) {
                    alert(e);
                }
            });
        <%}%>
        });
        
        <%if (formField.Exists(p => p.Field.Equals("Tags")))
        {%>
        function loadSelectTagsData() {
            $.ajax({
                type: 'POST',
                url: '/Handler/App/CationHandler.ashx',
                data: { Action: "QueryMemberTag", TagType: 'Outlets', page: 1, rows: 100000000 },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if (data.total == 0) {
                        $('.warpTagSelect').find('.warpNoData').show();
                        $('.warpTagSelect').find('.warpTagDataList').hide();
                    } else {
                        $('.warpTagSelect').find('.warpTagDataList').show();
                        $('.warpTagSelect').find('.warpNoData').hide();

                        //构造数据
                        var strHtml = new StringBuilder();
                        for (var i = 0; i < data.rows.length; i++) {
                            if (data.rows[i].TagName == "") continue;
                            strHtml.Append('<li class="overflow_ellipsis Width92P"><label>');
                            strHtml.AppendFormat('<input type="checkbox" name="tag" class="tagChk" value="{0}" {1} />{0}', data.rows[i].TagName, currTags.Contains(data.rows[i].TagName) ? 'checked' : '');
                            strHtml.Append('</label></li>');
                        }

                        $('.warpTagSelect').find('.ulTagList').html(strHtml.ToString());


                    }
                    var tagDiv = layer.open({
                        type: 1,
                        shade: [0.2, '#000'],
                        shadeClose: true,
                        area: ['300', '320'],
                        title: ['选择标签', 'background:#1B9AF7; color:#fff;'],
                        border: [0],
                        content: $('.warpTagDiv')
                    });


                }
            });
        }

        function addTagList(list) {
            for (var i = 0; i < list.length; i++) {
                if (!$('#txtTags').tagExist(list[i]))
                    $('#txtTags').addTag(list[i]);
            }
        }

        function tagClear() {
            $('#txtTags').importTags('');
        }

        //标签全选
        function selectTagAll() {
            $('.warpTagSelect .tagChk').attr('checked', true);
        }

        //标签反选
        function selectTagReverse() {
            $('.warpTagSelect .tagChk').each(function () {
                var v = $(this).attr('checked');
                $(this).attr('checked', !v);
            });
        }
        <%}%>

        function postUpdateData() {
            var model = { id: nid, ArticleType: type };
            <%  
            strHtml = new StringBuilder();
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
            {
                strHtml.AppendLine(" model.UserLongitude = $.trim($('#spanLongitude').text());");
                strHtml.AppendLine(" model.UserLatitude = $.trim($('#spanLatitude').text());");
                strHtml.AppendLine(" if(model.UserLongitude =='' || model.UserLatitude=='') { alert('请选择地图上的地址'); return;}");
            }

            foreach (var item in formField.Where(p => !others.Contains(p.Field)))
            {
                string nValue = nInfoJtoken[item.Field].ToString();
                if (!string.IsNullOrWhiteSpace(nValue) && item.IsReadOnly == 1 && id != "0") continue; //只读不能修改，编辑时跳过。
                if (item.FieldType == "select" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#ddl{0}').val());", item.Field));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(temp{0} =='') {2} alert('请选择{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = temp{0};", item.Field));
                }
                else if (item.FieldType == "radio" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("var temp{0} = $.trim($('input[name=\"rdo{0}\"]:checked').val());", item.Field));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(temp{0} =='') {2} alert('请选择{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = temp{0};", item.Field));
                }
                else if (item.FieldType == "checkbox" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("var array{0} = [];", item.Field));
                    strHtml.AppendLine(string.Format("$('input[name=\"chk{0}\"]:checked').each(function(){1} array{0}.push($.trim($(this).val())); {2});", item.Field, "{", "}"));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(array{0}.length ==0) {2} alert('请选择{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = array{0}.join(',');", item.Field));
                }
                else if (item.FieldType == "img")
                {
                    strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#txtImg{0}').val());", item.Field));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(temp{0} =='') {2} alert('请上传或输入{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = temp{0};", item.Field));
                }
                else if (item.FieldType == "mult")
                {
                    strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#txt{0}').html());", item.Field));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(temp{0} =='') {2} alert('请输入{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = temp{0};", item.Field));
                }
                else
                {
                    strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#txt{0}').val());", item.Field));
                    if (item.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(temp{0} ==''){2} alert('请输入{1}'); return;{3}", item.Field, item.MappingName, "{", "}"));
                    strHtml.AppendLine(string.Format("model.{0} = temp{0};", item.Field));
                }
            }
            var categoryIdField = formField.FirstOrDefault(p => p.Field.Equals("CategoryId"));
            if (categoryIdField != null)
            {
                strHtml.AppendLine("var tempCategoryId = $.trim($('#ddlCate').val());");
                if (categoryIdField.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(tempCategoryId =='' || tempCategoryId=='0') {1} alert('请选择{0}'); return;{2}", categoryIdField.MappingName, "{", "}"));
                strHtml.AppendLine("model.CategoryId = tempCategoryId;");
            }
            var tagsField = formField.FirstOrDefault(p => p.Field.Equals("Tags"));
            if (tagsField != null)
            {
                strHtml.AppendLine("var tempTags = currTags.join(',');");
                if (tagsField.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(tempTags =='') {1} alert('请选择{0}'); return;{2}", categoryIdField.MappingName, "{", "}"));
                strHtml.AppendLine("model.Tags = tempTags;");
            }
            var sortField = formField.FirstOrDefault(p => p.Field.Equals("Sort"));
            if (sortField != null)
            {
                strHtml.AppendLine("var tempSort = $.trim($('#txtSort').val());");
                if (sortField.FieldIsNull == 1) strHtml.AppendLine(string.Format(" if(tempSort =='') {1} alert('请输入{0}'); return;{2}", sortField.MappingName, "{", "}"));
                strHtml.AppendLine("model.Sort = tempSort;");
            }
            this.Response.Write(strHtml.ToString());
            %>
            $.messager.progress({ text: '正在提交。。。' });
            $.ajax({
                type: 'post',
                url: handlerUrl + (nid == "0" ? "Add.ashx" : "Update.ashx"),
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        if (nid == "0") {
                            ClearForm();
                            $.messager.alert("提示", resp.msg);
                        }
                        else {
                            location.href = "List.aspx?type="+type;
                        }
                    }
                    else {
                        $.messager.alert("提示", resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
        function ClearForm() {
            <%  
            strHtml = new StringBuilder();
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
            {
                strHtml.AppendLine("$('#spanLongitude').text('');");
                strHtml.AppendLine("$('#spanLatitude').text('');");
            }

            foreach (var item in formField.Where(p => !others.Contains(p.Field)))
            {
                if (item.FieldType == "select" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("$('#ddl{0}').val('');", item.Field));
                }
                else if (item.FieldType == "radio" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("$('input[name=\"rdo{0}\"]').attr('checked',false);", item.Field));
                }
                else if (item.FieldType == "checkbox" && !string.IsNullOrWhiteSpace(item.Options))
                {
                    strHtml.AppendLine(string.Format("$('input[name=\"chk{0}\"]').attr('checked',false);", item.Field));
                }
                else if (item.FieldType == "img")
                {
                    strHtml.AppendLine(string.Format("$('#imgImg{0}').attr('src','/img/hb/hb1.jpg');", item.Field));
                    strHtml.AppendLine(string.Format("$('#txtImg{0}').val('');", item.Field));
                }
                else if (item.FieldType == "mult")
                {
                    strHtml.AppendLine(string.Format("$('#txt{0}').html('');", item.Field));
                }
                else
                {
                    strHtml.AppendLine(string.Format("$('#txt{0}').val('');", item.Field));
                }
            }
            this.Response.Write(strHtml.ToString());
            %>
        }
        
        <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
        function getLnglat(e) {
            marker.setMap();
            var nlng = e.lnglat.getLng();
            var nlat = e.lnglat.getLat();
            mapObj.setCenter([nlng, nlat]);
            marker = new AMap.Marker({
                position: [nlng, nlat]
            });
            marker.setMap(mapObj);
            $('#spanLongitude').text(nlng);
            $('#spanLatitude').text(nlat);
        }
        function placeSearch() {
            var keyword = $.trim($('#txtMapKeyword').val());
            if (keyword != '') {
                toPlaceSearch.search(keyword, function (status, result) {
                    if (status === 'complete' && result.info === 'OK') {
                        var poiArr = result.poiList.pois;
                        marker.setMap();
                        var nlng = poiArr[0].location.getLng();
                        var nlat = poiArr[0].location.getLat();
                        mapObj.setCenter([nlng, nlat]);
                        marker = new AMap.Marker({
                            position: [nlng, nlat]
                        });
                        marker.setMap(mapObj);
                        $('#spanLongitude').text(nlng);
                        $('#spanLatitude').text(nlat);
                    }
                    else if (status === 'error') {
                        alert(result);
                    }
                    else {
                        alert('地址未找到');
                    }
                });
            }
        }
        <%}%>
    </script>
</asp:Content>
