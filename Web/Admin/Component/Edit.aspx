<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        body
        {
            font-family: 微软雅黑;
            
         }
        table td
        {
            height: 30px;
        }
        .td24
        {
            height: 24px;
        }
        .title
        {
         font-size:12px;   
         }
        #configs{
            width:800px;
        }
        .configrow
        {
            border:1px solid;
            border-radius:5px;
            border-color:#CCCCCC;
            margin-top:10px;
            width:100%;
           
         }
         .configvalue input[type=text]
         {
          width:90%;    
         }
        .fieldsort{
            float:left;
            margin-left:5px;
            margin-top:0px;
            cursor:pointer;
        }
    </style>
    <script src="/Scripts/color/jscolor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <strong><%= Request["component_model_id"]=="0"?"添加":"编辑" %>页面</strong>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox" style="min-height:430px;">
        <div style="font-size: 12px; width: 700px;">
            <table width="100%;">
                <tr>
                    <td style="width: 120px;">
                        页面名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtComponentName" value="" style="width: 450px;" placeholder="页面名称(必填)"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">
                        页面标识：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtComponentKey" value="" style="width: 450px;" placeholder="页面标识(选填，唯一)"/>
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 120px;">
                        页面描述：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtDecription" rows="3" style="width: 450px;" placeholder="页面描述"></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">
                        微信授权：
                    </td>
                    <td width="*" align="left">
                        <input id="chkIsWXSeniorOAuth" type="checkbox" checked="checked" class="positionTop2" /><label for="chkIsWXSeniorOAuth">微信高级授权</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">
                        访问级别：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAccessLevel" value="0" style="width: 450px;" placeholder="访问级别(选填)"/>
                    </td>
                </tr>
            </table>
            <strong style="font-size:18px;">配置列表:</strong>
            <span style="color:red;">请选择组件库</span>
            <select id="sltModel"></select>
            <div id="configs">

            </div>
       
            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                            <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a>
                            
                        
                        <a href="javascript:;" id="btnPageBack" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">返回</a>
                            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/component/';
        var component_id = '<%= Request["component_id"] %>'; //问题数量
        var action = component_id == 0 ? "add.ashx" : "update.ashx";
        var old_component_model_id = 0;
        var old_component_config = null;
        var loadSltModel = false;
        var component_config = {};
        var now_component_model_fields = [];
        var tnameArray = ["输入", "选择", "图片", "颜色", "数组", "对象", "广告", "工具栏", "页面", "导航数组", "商品数组"];
        var slides = <%=slides%>;
        var common_components = <%=common_components%>;
        var toolbars = <%=toolbars%>;
        var mall_cates = <% =mall_cates%>;
        var mall_tags = <% =mall_tags%>;
        var webpms_groups = '<% =webpms_groups%>';
        var art_cates = <% =art_cates%>;
        var act_cates = <% =act_cates%>;
        $(function () {
            //加载组件库选择下拉列表
            loadSelectModelData();
            $("#sltModel").live("change", function () {
                $("#configs").html("");
                loadModelData($("#sltModel").val());
            });
            
            $(".upfield").live("click",function(){
                if($(this).closest("div").prev(".configrow").length>0){
                    $(this).closest("div").prev(".configrow").before($(this).closest("div").clone());
                    $(this).closest("div").remove();
                }
            });

            $(".downfield").live("click",function(){
                if($(this).closest("div").next(".configrow").length>0){
                    $(this).closest("div").next(".configrow").after($(this).closest("div").clone());
                    $(this).closest("div").remove();
                }
            });
            $(".addarray").live("click", function () {
                var component_field = $(this).closest("table").attr("config-key");
                var component_type = $(this).closest("table").attr("config-type");
                var tdObj = $(this).closest("td").next();
                var tbObjs = $(tdObj).find("table");
                var array_index = "-1";
                if (tbObjs.length > 0) {
                    array_index = $(tbObjs[tbObjs.length - 1]).attr("data-array-index");
                }
                array_index = parseInt(array_index);
                array_index++;
                var field_data_value = "";
                var _i = 0;
                for (var i = 0; i < now_component_model_fields.length; i++) {
                    if (now_component_model_fields[i].component_field == component_field) {
                        field_data_value = now_component_model_fields[i].component_field_data_value;
                        break;
                    }
                }
                var valueHtml = AddFieldType_4or5(array_index, component_field, field_data_value, parseInt(component_type));
                $(tdObj).closest("td").append(valueHtml);
                jscolor.init();
            });
            $(".imgUpload").live("click", function () {
                var tbObjs = $(this).next().click();
            });
            
            $(".mallcate").live("click", function () {
                var tbObjs = $(this).closest("table").find(".mallcatetitle");
                var ntext = $(this).closest("li").find("label").text();
                var otitle = $.trim( $(tbObjs).val());
                if(ntext != "所有"){
                    $(tbObjs).val(ntext);
                }
            });
            $(".deleteArray").live("click", function () {
                var tbObjs = $(this).closest("table").closest("td").find("table");
                if(tbObjs.length>0){
                    $(this).closest("table").remove();
                }
                else{
                    Alert("已全部删除");
                }
            });
            $(".file").live('change', function () {
                var fpath = $.trim($(this).val());
                var fid = $.trim($(this).attr("id"));
                var txtfid = "txt" + fid;
                var imgfid = "img" + fid;
                if (fpath == "") return;
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: fid,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $("#" + txtfid).val(resp.ExStr);
                                 $("#" + imgfid).attr("src", resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    Alert(e);
                }
            });
            
            //编辑时加载原数据
            if (component_id > 0) {
                //$.messager.progress({ text: '正在加载...' });
                $.ajax({
                    type: 'post',
                    url: handlerUrl + "get.ashx",
                    data: { component_id: component_id },
                    dataType: "json",
                    success: function (resp) {
                        //$.messager.progress('close');
                        if (resp.status) {
                            $('#txtComponentName').val(resp.result.component_name);
                            $('#txtComponentKey').val(resp.result.component_key);
                            $('#txtDecription').val(resp.result.decription);
                            $('#txtAccessLevel').val(resp.result.access_level);
                            chkIsWXSeniorOAuth.checked = resp.result.is_oauth == 1?true:false;
                            old_component_model_id = resp.result.component_model_id;
                            loadModelData(old_component_model_id);
                            if (loadSltModel) $("#sltModel").val(old_component_model_id);
                            old_component_config = JSON.parse(resp.result.component_config);
                        }
                        else {
                            Alert(resp.msg);
                        }
                    }
                });
            }
            //编辑时加载原数据
            //提交
            $('#btnSave').click(function () {
                try {
                    var dv = $.trim($("#txtComponentName").val());
                    if (dv == "") {
                        Alert("请输入页面名称");
                        $("#txtComponentName").focus();
                        return false;
                    }
                    var dm = $.trim($("#sltModel").val());
                    if (dm == "" ||dm == 0) {
                        Alert("请选择页面组件库");
                        $("#sltModel").focus();
                        return false;
                    }
                    component_config = {};
                    var _index = 1;
                    $(".configvalue").each(function () {
                        var config_type = $(this).attr("config-type");
                        var config_key = $(this).attr("config-key");
                        config_type = parseFloat(config_type);
                        if (config_type == 0) {
                            config_value = $.trim($(this).find("input[type='text']").val());
                        }
                        else if (config_type == 1) {
                            config_value = $.trim($(this).find("input[type='radio']:checked").val());
                        }
                        else if (config_type == 2) {
                            config_value = $.trim($(this).find("input[type='text']").val());
                        }
                        else if (config_type == 3) {
                            config_value = $.trim($(this).find("input[type='text']").val());
                            if (config_value.length == 6) {
                                config_value = "#" + config_value;
                            }
                        }
                        else if (config_type == 4 || config_type == 6 || config_type == 9 || config_type==10) {
                            var tableObjs = $(this).find(".fieldArray");
                            if (tableObjs.length == 0) return;
                            var tempList = [];
                            for (var i = 0; i < tableObjs.length; i++) {
                                var trObjs = $(tableObjs[i]).find("tr");
                                if (trObjs.length == 0) continue;
                                var tempLi = {};
                                for (var j = 0; j < trObjs.length; j++) {
                                    var array_key = $(trObjs[j]).attr("array-key");
                                    var array_type = $(trObjs[j]).attr("array-type");
                                    var array_value;
                                    array_type = parseInt(array_type);
                                    if (array_type == 0) {
                                        if(config_key == "activitys" && array_key=="cate_id"){
                                            array_value = $(trObjs[j]).find("select").val();
                                        }
                                        else if(config_key == "cards" && array_key=="cate_id"){
                                            array_value = $(trObjs[j]).find("select").val();
                                        }
                                        else{
                                            array_value = $(trObjs[j]).find("input[type='text']").val();
                                        }
                                    }
                                    else if (array_type == 1) {
                                        array_value = $(trObjs[j]).find("input[type='radio']:checked").val();
                                    }
                                    else if (array_type == 2) {
                                        array_value = $(trObjs[j]).find("input[type='text']").val();
                                    }
                                    else if (array_type == 3) {
                                        array_value = $(trObjs[j]).find("input[type='text']").val();
                                        array_value = $.trim(array_value);
                                        if (array_value.length == 6) {
                                            array_value = "#" + array_value;
                                        }
                                    }
                                    if (array_value == undefined || array_value.length == 0) continue;
                                    if (!isNaN(array_value)) array_value = parseFloat(array_value);
                                    tempLi[array_key] = array_value;
                                }
                                if (Object.getOwnPropertyNames(tempLi).length == 0) continue;
                                var is_system = $(tableObjs[i]).attr("data-array-is_system");
                                if(is_system ==1){
                                    tempLi["is_system"] = 1;
                                    var pms_group = $(tableObjs[i]).attr("data-array-pms_group");
                                    if(pms_group!="") tempLi["pms_group"] = pms_group;
                                }
                                tempList.push(tempLi);
                            }
                            component_config[config_key] = tempList;
                        }
                        else if (config_type == 5 ) {
                            var tableObjs = $(this).find(".fieldArray");
                            if (tableObjs.length == 0) return;
                            var trObjs = $(tableObjs[0]).find("tr");
                            if (trObjs.length == 0) return;
                            var tempLi = {};
                            for (var j = 0; j < trObjs.length; j++) {
                                var array_key = $(trObjs[j]).attr("array-key");
                                var array_type = $(trObjs[j]).attr("array-type");
                                var array_value;
                                array_type = parseInt(array_type);
                                if (array_type == 0) {
                                    array_value = $(trObjs[j]).find("input[type='text']").val();
                                }
                                else if (array_type == 1) {
                                    array_value = $(trObjs[j]).find("input[type='radio']:checked").val();
                                }
                                else if (array_type == 2) {
                                    array_value = $(trObjs[j]).find("input[type='text']").val();
                                }
                                else if (array_type == 3) {
                                    array_value = $(trObjs[j]).find("input[type='text']").val();
                                    array_value = $.trim(array_value);
                                    if (array_value.length == 6) {
                                        array_value = "#" + array_value;
                                    }
                                }
                                if (array_value == undefined || array_value.length == 0) continue;
                                if (!isNaN(array_value)) array_value = parseFloat(array_value);
                                tempLi[array_key] = array_value;
                            }
                            if (Object.getOwnPropertyNames(tempLi).length == 0) return;
                            component_config[config_key] = tempLi;
                        }
                        //else if (config_type == 6) {
                        //    config_value = $.trim($(this).find("input[type='radio']:checked").val());
                        //}
                        else if (config_type == 7) {
                            config_value = $.trim($(this).find("input[type='radio']:checked").val());
                        }
                        else if (config_type == 8) {
                            config_value = $.trim($(this).find("input[type='radio']:checked").val());
                        }
                        if (config_type != 4 && config_type != 5 && config_type != 6 && config_type != 9 && config_type != 10) {
                            if (config_value.length == 0) return;
                            if (!isNaN(config_value)) config_value = parseFloat(config_value);
                            component_config[config_key] = config_value;
                        }
                    });
                    //模型
                    var postDataModel = {
                        component_id:component_id,
                        component_key:$.trim($("#txtComponentKey").val()),
                        component_name: dv,
                        component_model_id: dm,
                        decription: $.trim($("#txtDecription").val()),
                        component_config: JSON.stringify(component_config),
                        is_oauth:chkIsWXSeniorOAuth.checked?1:0,
                        access_level:$.trim($("#txtAccessLevel").val())
                    };
                    $.messager.progress({ text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl+action,
                        data: postDataModel,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status) {
                                window.location.href = "List.aspx";
                            }
                            else {
                                Alert(resp.msg);
                            }
                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });
            //提交
            //返回列表
            $('#btnPageBack').click(function () {
                window.location.href = "List.aspx";
            });
            //返回列表
        });

        //加载组件库列表
        function loadSelectModelData() {
            $.ajax({
                type: 'post',
                url: handlerUrl + "/model/list.ashx",
                data: { rows: 2000, page: 1 },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        if (resp.result.list.length == 0) {
                            Alert("请先录入页面组件库");
                        }
                        var appendhtml = new StringBuilder();
                        appendhtml.AppendFormat('<option value="0">请选择组件库</option>');
                        for (var i = 0; i < resp.result.list.length; i++) {
                            appendhtml.AppendFormat('<option value="{0}">{1}</option>', resp.result.list[i].component_model_id, resp.result.list[i].component_model_name);
                        }
                        $("#sltModel").append(appendhtml.ToString());
                        loadSltModel = true;
                        if (old_component_model_id !=0) $("#sltModel").val(old_component_model_id);
                    }
                    else {
                        Alert(resp.msg);
                    }
                }
            });
        }
        //加载对应组件库
        function loadModelData(model_id) {
            if (model_id == 0) {
                $("#configs").html("");
                return;
            }
            $.ajax({
                type: 'post',
                url: handlerUrl + "model/get.ashx",
                data: { component_model_id: model_id },
                dataType: "json",
                success: function (resp) {
                    if (!resp.result) {
                        Alert(resp.msg);
                        return;
                    }
                    if (resp.result.component_model_fields.length == 0) {
                        Alert("该组件库没有设置字段");
                        return;
                    }
                    now_component_model_fields = resp.result.component_model_fields;
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<div class="config" style="width:100%;">');
                    for (var i = 0; i < resp.result.component_model_fields.length; i++) {
                        var nfield = resp.result.component_model_fields[i];
                        var field_type = parseInt(nfield.component_field_type);
                        appendhtml.AppendFormat('<div class="configrow" style="width:100%;">');
                        appendhtml.AppendFormat('<table class="configvalue"  style="width:100%;" config-key="{0}" config-type="{1}"><tr>', nfield.component_field, nfield.component_field_type);
                        appendhtml.AppendFormat('<td style="width:120px; padding-left:10px; padding-top:12px; vertical-align:top;">{0}', nfield.component_field_name);
                        appendhtml.AppendFormat('(<span style="color:red;">{0}</span>)', tnameArray[field_type]);
                        appendhtml.AppendFormat('：');
                        if ((field_type == 4 || field_type ==6 || field_type==9 || field_type==10) && nfield.component_field_data_value != "") appendhtml.AppendFormat('<img src="/img/icons/add.png" width="20" height="20" alt="添加数组" class="addarray hand floatR" />');
                        
                        appendhtml.AppendFormat('<br/><img src="/img/icons/up.png" class="upfield fieldsort"/>');
                        appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort"/>');
                        appendhtml.AppendFormat('</td>');
                        appendhtml.AppendFormat('<td>');
                        if (field_type == 0) {
                            appendhtml.AppendFormat('<input type="text" style="width:400px;" value="{0}" />', nfield.component_field_data_value);
                        }
                        else if (field_type == 2) {
                            appendhtml.AppendFormat('<img alt="缩略图" src="/img/hb/hb1.jpg" id="imgfile_{0}" width="80px" height="80px" class="imgUpload" />', i);
                            appendhtml.AppendFormat('<input type="file" name="file1" id="file_{0}" class="file" style="display:none;" />', i);
                            appendhtml.AppendFormat('可点击缩略图上传图片，也可直接输入地址');
                            appendhtml.AppendFormat('<br />');
                            appendhtml.AppendFormat('<input type="text" style="width:400px;" id="txtfile_{0}" value="" />', i);
                        }
                        else if (field_type == 3) {
                            appendhtml.AppendFormat('<input type="text" style="width:400px;" class="color" value="{0}" />', nfield.component_field_data_value);
                        }
                        else if (field_type == 1) {
                            if (nfield.component_field_data_value && nfield.component_field_data_value != "") {
                                appendhtml.AppendFormat('<ul>');
                                var data_options = nfield.component_field_data_value.split("@");
                                for (var j = 0; j < data_options.length; j++) {
                                    var datakv = data_options[j].split("|");
                                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                                    if (j == 0) {
                                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" checked="checked" /><label class="hand" for="{1}">{2}</label>'
                                            , nfield.component_field + i, nfield.component_field + i + j, datakv[0], datakv[1]);
                                    }
                                    else {
                                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" /><label class="hand" for="{1}">{2}</label>'
                                            , nfield.component_field + i, nfield.component_field + i + j, datakv[0], datakv[1]);
                                    }
                                    appendhtml.AppendFormat('</li>');
                                }
                                appendhtml.AppendFormat('</ul>');
                            }
                        }
                        else if (field_type == 4 || field_type == 5 || field_type == 6 || field_type == 9 || field_type == 10) {
                            var valueHtml = AddFieldType_4or5(0, nfield.component_field, nfield.component_field_data_value, field_type);
                            appendhtml.AppendFormat(valueHtml);
                        }
                        //else if (field_type == 6) {
                        //        appendhtml.AppendFormat('<ul>');
                        //        appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        //        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        //            , nfield.component_field + i, nfield.component_field + i + "_1", "隐藏", "-999", 'checked="checked"');
                        //        appendhtml.AppendFormat('</li>');
                        //        for (var j = 0; j < slides.length; j++) {
                        //            appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        //            appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        //                , nfield.component_field + i, nfield.component_field + i + j, slides[j], slides[j], j == 0 ? 'checked="checked"' : '');
                        //            appendhtml.AppendFormat('</li>');
                        //        }
                        //        appendhtml.AppendFormat('</ul>');
                        //}
                        else if (field_type == 7) {
                            var toolbar_use_type = nfield.component_field.split("_")[0];
                            appendhtml.AppendFormat('<ul>');
                            
                            appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                            appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                                , nfield.component_field + i, nfield.component_field + i + "_1", "隐藏", "-999", 'checked="checked"');
                            appendhtml.AppendFormat('</li>');

                            for (var j = 0; j < toolbars.length; j++) {
                                //if(toolbar_use_type == toolbars[j].use_type){
                                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                                        , nfield.component_field + i, nfield.component_field + i + j, toolbars[j].key_type, toolbars[j].key_type, '');
                                    appendhtml.AppendFormat('</li>');
                                //}
                            }
                            appendhtml.AppendFormat('</ul>');
                        }
                        else if (field_type == 8) {
                            appendhtml.AppendFormat('<ul>');
                            appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                            appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                                , nfield.component_field + i, nfield.component_field + i + "_1", "隐藏", "-999", 'checked="checked"');
                            appendhtml.AppendFormat('</li>');

                            for (var j = 0; j < common_components.length; j++) {
                                if (common_components[j].component_type == nfield.component_field_data_value) {
                                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                                        , nfield.component_field + i, nfield.component_field + i + j, common_components[j].component_name, common_components[j].component_id,'');
                                    appendhtml.AppendFormat('</li>');
                                }
                            }
                            appendhtml.AppendFormat('</ul>');
                        }
                        appendhtml.AppendFormat('</td>');
                        appendhtml.AppendFormat('</tr></table>');
                        appendhtml.AppendFormat('</div>');
                    }
                    appendhtml.AppendFormat('</div>');
                    $("#configs").append(appendhtml.ToString());
                    if(component_id==0){
                        loadNullData();
                    }
                    else{
                        if (resp.result.component_model_id == old_component_model_id) {
                            bindOldData();
                        }
                    }
                    jscolor.init();
                }
            });
        }
        function AddFieldType_4or5(_index, _field, _field_data_value, _field_type) {
            var appendhtml = new StringBuilder();
            if (_field_data_value && _field_data_value != "") {
                appendhtml.AppendFormat('<table class="configrow fieldArray" style="width:100%;" data-array-index="{0}" data-array-is_system="{1}" data-array-pms_group="{2}">', _index,'0','');
                if(_field_type==6){
                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', 'slide_list', '1');
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', "幻灯片");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    appendhtml.AppendFormat('<ul>');
                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        , _field + _index, _field + _index + "_1", "隐藏", "-999", 'checked="checked"');
                    appendhtml.AppendFormat('</li>');

                    for (var j = 0; j < slides.length; j++) {
                        appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                            , _field + _index, _field + _index + j, slides[j], slides[j], '');
                        appendhtml.AppendFormat('</li>');
                    }
                    appendhtml.AppendFormat('</ul>');
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');
                }
                else if(_field_type==9){
                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', 'nav_list', '1');
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', "导航");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    appendhtml.AppendFormat('<ul>');
                            
                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        , _field + _index, _field + _index + "_1", "隐藏", "-999", 'checked="checked"');
                    appendhtml.AppendFormat('</li>');

                    for (var j = 0; j < toolbars.length; j++) {
                        appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                            , _field + _index, _field + _index + j, toolbars[j].key_type, toolbars[j].key_type, '');
                        appendhtml.AppendFormat('</li>');
                    }
                    appendhtml.AppendFormat('</ul>');
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');
                }
                else if(_field_type==10){
                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', 'title', '0');
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', "标题");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    appendhtml.AppendFormat('<input type="text" class="mallcatetitle" style="width:280px;" value="{0}" />', "");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');

                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', 'cate', '1');
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', "分类");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    appendhtml.AppendFormat('<ul>');
                            
                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand mallcate" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        , _field + _index, _field + _index + "_1", "所有", "", 'checked="checked"');
                    appendhtml.AppendFormat('</li>');

                    for (var j = 0; j < mall_cates.length; j++) {
                        appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand mallcate" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                            , _field + _index, _field + _index + j, mall_cates[j].cate_name, mall_cates[j].cate_id, '');
                        appendhtml.AppendFormat('</li>');
                    }
                    appendhtml.AppendFormat('</ul>');
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');

                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', 'tag', '1');
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', "标签");
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    appendhtml.AppendFormat('<ul>');
                            
                    appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                    appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                        , _field + _index+"_1", _field + _index + "_1_1", "所有", "", 'checked="checked"');
                    appendhtml.AppendFormat('</li>');

                    for (var j = 0; j < mall_tags.length; j++) {
                        appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                        appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4} /><label class="hand" for="{1}">{2}</label>'
                            , _field + _index+"_1", _field + _index + j+"_1", mall_tags[j], mall_tags[j], '');
                        appendhtml.AppendFormat('</li>');
                    }
                    appendhtml.AppendFormat('</ul>');
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');
                }
                var data_options = _field_data_value.split("@");
                for (var j = 0; j < data_options.length; j++) {
                    var datakv = data_options[j].split("|");
                    var datakt = parseInt(datakv[2]);
                    appendhtml.AppendFormat('<tr array-key="{0}" array-type="{1}">', datakv[0], datakv[2]);
                    appendhtml.AppendFormat('<td style="width:120px; padding-left:10px;" class="td24">{0}', datakv[1]);
                    appendhtml.AppendFormat('(<span style="color:red;">{0}</span>)', tnameArray[datakt]);
                    appendhtml.AppendFormat('：');
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('<td class="td24">');
                    if (j == 0 && (_field_type==4  || _field_type==6|| _field_type==9 || _field_type==10)) {
                        appendhtml.AppendFormat('<img src="/img/delete.png" style="float:right; width:20px;height:20px;" class="deleteArray"/>');
                    }

                    if (datakt == 0) {
                        if(_field == "activitys" && datakv[0]=="cate_id"){
                            appendhtml.AppendFormat('<select style="width:140px;">');
                            appendhtml.AppendFormat('<option value="{0}">{1}</option>',"0","所有");
                            for (var ij = 0; ij < act_cates.length; ij++) {
                                appendhtml.AppendFormat('<option value="{0}">{1}</option>',act_cates[ij].cate_id,act_cates[ij].cate_name);
                            }
                            appendhtml.AppendFormat('</select>');
                        }
                        else if(_field == "cards" && datakv[0]=="cate_id"){
                            appendhtml.AppendFormat('<select style="width:140px;">');
                            appendhtml.AppendFormat('<option value="{0}">{1}</option>',"0","所有");
                            for (var ij = 0; ij < art_cates.length; ij++) {
                                appendhtml.AppendFormat('<option value="{0}">{1}</option>',art_cates[ij].cate_id,art_cates[ij].cate_name);
                            }
                            appendhtml.AppendFormat('</select>');
                        }
                        else{
                            appendhtml.AppendFormat('<input type="text" style="width:280px;" value="{0}" />', datakv[3]);
                        }
                    }
                    else if (datakt == 1) {
                        appendhtml.AppendFormat('<ul>');
                        var datakli = datakv[3].split("$");
                        for (var ij = 0; ij < datakli.length; ij++) {
                            var dataklv = datakli[ij].split("#");
                            appendhtml.AppendFormat('<li style="float:left; min-width:100px;">');
                            appendhtml.AppendFormat('<input type="radio" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" {4}/><label class="hand" for="{1}">{2}</label>'
                                , _field + datakv[0] + j + _index, _field + datakv[0] + j + ij + _index, dataklv[0], dataklv[1], ij == 0 ? 'checked="checked"' : '');
                            appendhtml.AppendFormat('</li>');
                        }
                        appendhtml.AppendFormat('</ul>');
                    }
                    else if (datakt == 2) {
                        appendhtml.AppendFormat('<img alt="缩略图" src="/img/hb/hb1.jpg" id="imgfile_{0}" width="80px" height="80px" class="imgUpload" />', _field + datakv[0] + j + _index);
                        appendhtml.AppendFormat('<input type="file" name="file1" id="file_{0}" class="file" style="display:none;" />', _field + datakv[0] + j + _index);
                        appendhtml.AppendFormat('可点击缩略图上传图片，也可直接输入地址');
                        appendhtml.AppendFormat('<br />');
                        appendhtml.AppendFormat('<input type="text" style="width:400px;" id="txtfile_{0}" value="" />', _field + datakv[0] + j + _index);
                    }
                    else if (datakt == 3) {
                        appendhtml.AppendFormat('<input type="text" style="width:280px;" class="color" value="{0}" />', datakv[3]);
                    }
                    appendhtml.AppendFormat('</td>');
                    appendhtml.AppendFormat('</tr>');
                }
                appendhtml.AppendFormat('</table>');

            }
            return appendhtml.ToString();
        }
        function bindOldData() {
            var keys = Object.getOwnPropertyNames(old_component_config);
            for (var i = 0; i < keys.length; i++) {
                var nObj = $(".configvalue[config-key='"+keys[i]+"']");
                if(nObj.length ==0) continue;
                var pcount = $(nObj).closest("div").prevAll().length;
                var ccount= pcount - i;
                if(ccount >0){
                    var tObj = $(nObj).closest("div").prev(".configrow");
                    for (var j = 0; j < ccount - 1; j++) {
                        var tempObj = $(tObj).prev(".configrow");
                        if(tempObj.length>0) tObj =tempObj;
                    }
                    $(tObj).before($(nObj).closest("div").clone());
                    $(nObj).closest("div").remove();
                }
                else if(ccount < 0){
                    var tObj = $(nObj).closest("div").next(".configrow");
                    for (var j = 0; j < (0-ccount) - 1; j++) {
                        var tempObj = $(tObj).next(".configrow");
                        if(tempObj.length>0) tObj =tempObj;
                    }
                    $(tObj).after($(nObj).closest("div").clone());
                    $(nObj).closest("div").remove();
                }
            }

            $(".configvalue").each(function () {
                var config_type = $(this).attr("config-type");
                var config_key = $(this).attr("config-key");
                var config_value = old_component_config[config_key];
                if (typeof (config_value) == 'undefined'){
                    if(config_type == 4 || config_type==6 || config_type==9 || config_type==10){
                        $(this).find('.deleteArray').click();
                    }
                    return;
                } 
                config_type = parseFloat(config_type);
                if (config_type == 0) {
                    $(this).find("input[type='text']").val(config_value);
                }
                else if (config_type == 1) {
                    $(this).find("input[type='radio']").each(function () {
                        $(this).attr("checked", $(this).val() == config_value);
                    })
                }
                else if (config_type == 2) {
                    $(this).find(".imgUpload").attr("src", config_value);
                    $(this).find("input[type='text']").val(config_value);
                }
                else if (config_type == 3) {
                    $(this).find("input[type='text']").val(config_value.replace("#", ""));
                    $(this).find("input[type='text']").css("backgroundColor", config_value);
                }
                else if (config_type == 4 || config_type==6 || config_type==9 || config_type==10) {
                    if (typeof (config_value) == "string") config_value = JSON.parse(config_value);
                    for (var i = 0; i < config_value.length; i++) {
                        if (i > 0) $(this).find(".addarray").click();
                    }
                    var tableObjs = $(this).find(".fieldArray");
                    if (tableObjs.length == 0) return;
                    for (var i = 0; i < tableObjs.length; i++) {
                        var trObjs = $(tableObjs[i]).find("tr");
                        if (trObjs.length == 0) continue;
                        var tempLi = config_value[i];
                        if (typeof(tempLi) == "undefined") continue;
                        for (var j = 0; j < trObjs.length; j++) {
                            var array_key = $(trObjs[j]).attr("array-key");
                            var array_type = $(trObjs[j]).attr("array-type");
                            if (typeof(tempLi[array_key]) == "undefined") continue;
                            array_type = parseInt(array_type);
                            if (array_type == 0) {
                                if(config_key == "activitys" && array_key=="cate_id"){
                                    $(trObjs[j]).find("select").val(tempLi[array_key]);
                                }
                                else if(config_key == "cards" && array_key=="cate_id"){
                                    $(trObjs[j]).find("select").val(tempLi[array_key]);
                                }
                                else{
                                    $(trObjs[j]).find("input[type='text']").val(tempLi[array_key]);
                                }
                            }
                            else if (array_type == 1) {
                                $(trObjs[j]).find("input[type='radio']").each(function () {
                                    $(this).attr("checked", $(this).val() == tempLi[array_key]);
                                })
                            }
                            else if (array_type == 2) {
                                $(trObjs[j]).find(".imgUpload").attr("src", tempLi[array_key]);
                                $(trObjs[j]).find("input[type='text']").val(tempLi[array_key]);
                            }
                            else if (array_type == 3) {
                                $(trObjs[j]).find("input[type='text']").val(tempLi[array_key].replace("#", ""));
                                $(trObjs[j]).find("input[type='text']").css("backgroundColor", tempLi[array_key]);
                            }
                        }
                        //是否系统页面
//                        if(tempLi["is_system"] !=null && tempLi["is_system"] ==1){
//                            $(tableObjs[i]).attr("data-array-is_system",1);
//                            $(tableObjs[i]).find(".deleteArray").hide();
//                        }
                        //权限组
//                        if(tempLi["pms_group"] !=null){
//                            var pms_group = tempLi["pms_group"];
//                            $(tableObjs[i]).attr("data-array-pms_group",pms_group);
//                            if(pms_group!="" && webpms_groups ==""){
//                                $(tableObjs[i]).hide();
//                            }
//                            else if(pms_group!="" && webpms_groups !=""){
//                                var ngrouplist = pms_group.split(",");
//                                var wgrouplist = webpms_groups.split(",");
//                                var havepms = false;
//                                for (var j = 0; j < ngrouplist.length; j++) {
//                                    if(wgrouplist.indexOf(ngrouplist[j])>=0){
//                                        havepms=true;
//                                        break;
//                                    }
//                                }
//                                if(!havepms){
//                                    $(tableObjs[i]).hide();
//                                }
//                            }
//                        }
                    }
                }
                else if (config_type == 5) {
                    if (typeof (config_value) == "string") config_value = JSON.parse(config_value);
                    var tableObjs = $(this).find(".fieldArray");
                    if (tableObjs.length == 0) return;
                    var trObjs = $(tableObjs[0]).find("tr");
                    if (trObjs.length == 0) return;
                    var tempLi = config_value;
                    for (var j = 0; j < trObjs.length; j++) {
                        var array_key = $(trObjs[j]).attr("array-key");
                        var array_type = $(trObjs[j]).attr("array-type");
                        if (typeof(tempLi[array_key]) == "undefined") continue;
                        array_type = parseInt(array_type);
                        if (array_type == 0) {
                            $(trObjs[j]).find("input[type='text']").val(tempLi[array_key]);
                        }
                        else if (array_type == 1) {
                            $(trObjs[j]).find("input[type='radio']").each(function () {
                                $(this).attr("checked", $(this).val() == tempLi[array_key]);
                            })
                        }
                        else if (array_type == 2) {
                            $(trObjs[j]).find(".imgUpload").attr("src", tempLi[array_key]);
                            $(trObjs[j]).find("input[type='text']").val(tempLi[array_key]);
                        }
                        else if (array_type == 3) {
                            $(trObjs[j]).find("input[type='text']").val(tempLi[array_key].replace("#", ""));
                            $(trObjs[j]).find("input[type='text']").css("backgroundColor", tempLi[array_key]);
                        }
                    }
                }
                //else if (config_type == 6) {
                //    $(this).find("input[type='radio']").each(function () {
                //        $(this).attr("checked", $(this).val() == config_value);
                //    })
                //}
                else if (config_type == 7) {
                    $(this).find("input[type='radio']").each(function () {
                        $(this).attr("checked", $(this).val() == config_value);
                    })
                }
                else if (config_type == 8) {
                    $(this).find("input[type='radio']").each(function () {
                        $(this).attr("checked", $(this).val() == config_value);
                    })
                }
            });
        }
        function loadNullData(){
            $(".configvalue").each(function () {
                var config_type = $(this).attr("config-type");
                if(config_type == 4 || config_type==6 || config_type==9 || config_type==10){
                    $(this).find('.deleteArray').click();
                }
            });
        }
    </script>
</asp:Content>
