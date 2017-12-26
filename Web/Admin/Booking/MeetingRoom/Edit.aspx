<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ui-sortable div {
            float: left;
            padding: 10px;
            position: relative;
        }

            .ui-sortable div .deleteImg {
                position: absolute;
                right: 0px;
                top: 0px;
            }

        .clear {
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=currActionName + currShowName %>
    <a href="List.aspx?type=<%=categoryType %>" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable" style="width: 800px;">
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle"><%=currShowName %>名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPName" class="commonTxt" placeholder="<%=currShowName %>名称(必填)" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle"><%=currShowName %>描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" class="commonTxt" placeholder="<%=currShowName %>描述" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">价格：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPrice" class="easyui-numberbox" value="0" data-options="min:0,precision:2,onChange:onChangePrice" placeholder="价格(必填)" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">单位：
                    </td>
                    <td width="*" align="left">
                        <% string defUnit = isAdded ? "元" : "元/小时";  %>
                        <input type="text" id="txtUnit" class="commonTxt" placeholder="单位" value="<%= defUnit %>" />
                    </td>
                </tr>
                <%if (!isAdded)
                  { %>
                <tr>
                    <td align="right" class="tdTitle">照片：
                    </td>
                    <td width="*" align="left">
                        <div class="ui-sortable">
                        </div>
                        <div class="clear">
                            <a id="auploadImg" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file1.click()">上传缩略图</a>
                            <img alt="提示" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请点击缩略图上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为460*340。可拖动排序。
                            <input type="file" id="file1" class="file file1" name="file1" style="display: none;" />
                        </div>
                    </td>
                </tr>
                <%} %>
                <%if (!isAdded)
                  { %>
                <tr>
                    <td align="right" class="tdTitle">分类：
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle"><%=currStockName %>：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCount" class="easyui-numberbox" value="1" data-options="min:0,precision:0" placeholder="<%=currStockName %>(必填)" />
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td align="right" class="tdTitle">状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsOnSale" id="rdoIsOnSale" checked="checked" /><label for="rdoIsOnSale">上架</label>
                        <input type="radio" name="IsOnSale" id="rdoIsNoOnSale" /><label for="rdoIsNoOnSale">下架</label>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSort" class="commonTxt" placeholder="排序" value="0" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'')" />数字越大越排前
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">访问权限级别：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAccessLevel" class="commonTxt" placeholder="访问权限级别" value="0" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'')" />
                    </td>
                </tr>
                <%if (!isAdded && nCategoryTypeConfig.TimeSetMethod == 1)
                  { %>
                <tr>
                    <td align="right" class="tdTitle">时间段：
                    </td>
                    <td width="*" align="left" style="height: 240px;">
                        <table id="grvTimeSetMethod1" class="easyui-datagrid" data-options="fit:true,toolbar: '#divToolbarTimeSetMethod1',singleSelect:true,fitcolumns:true">
                            <thead>
                                <tr>
                                    <th field="start" width="150">开始时间</th>
                                    <th field="end" width="150">结束时间</th>
                                    <th field="price" width="90" formatter="FormatterTimeSetMethod1PriceUnit">价格</th>
                                    <th field="action" width="80" formatter="FormatterTimeSetMethod1Action">操作</th>
                                </tr>
                            </thead>
                        </table>
                        <div id="divToolbarTimeSetMethod1">
                            <input class="easyui-datebox" id="txtTimeSetMethod1Start" data-options="editable:false,value:new Date().format('yyyy-MM-dd')" style="width: 90px" />
                            <select class="easyui-combobox" id="txtTimeSetMethod1StartHour" data-options="value:'09',valueField: 'value',textField: 'value',data:hoursData" style="width: 45px"></select>
                            <select class="easyui-combobox" id="txtTimeSetMethod1StartMinute" data-options="value:'00',valueField: 'value',textField: 'value',data:minutesData" style="width: 45px"></select>
                            —
                    <select class="easyui-combobox" id="txtTimeSetMethod1EndHour" data-options="editable:false,value:'10',valueField: 'value',textField: 'value',data:hoursData" style="width: 45px"></select>
                            <select class="easyui-combobox" id="txtTimeSetMethod1EndMinute" data-options="editable:false,value:'00',valueField: 'value',textField: 'value',data:minutesData" style="width: 45px"></select>
                            <input type="text" id="txtTimeSetMethod1Price" class="easyui-numberbox" value="0" data-options="min:0,precision:2" placeholder="价格(必填)" style="width: 60px" />元/小时
                    <a href="javascript:;" id="btnTimeSetMethod1Add" class="easyui-linkbutton" iconcls="icon-add" title="添加时间段" onclick="AddTimeSetMethod1()">添加时间段</a>
                            <a href="javascript:;" id="btnTimeSetMethod1Edit" class="easyui-linkbutton" iconcls="icon-edit" title="编辑时间段" onclick="ChangeTimeSetMethod1()" data-index="-1">编辑时间段</a>
                        </div>
                    </td>
                </tr>
                <%}
                  else if (!isAdded && nCategoryTypeConfig.TimeSetMethod == 2)
                  { %>
                <tr>
                    <td align="right" class="tdTitle">时间段：
                    </td>
                    <td width="*" align="left">
                        <% for (var i = 0; i < weekIndex.Length; i++)
                           { %>

                        <table id="grvTimeSetMethod2<%= weekIndex[i] %>" class="easyui-datagrid" data-options="title:'<%=weekIndexString[i] %>',height:240,width:652,toolbar: '#divToolbarTimeSetMethod2<%= weekIndex[i] %>',singleSelect:true,fitcolumns:true">
                            <thead>
                                <tr>
                                    <th field="start" width="150">开始时间</th>
                                    <th field="end" width="150">结束时间</th>
                                    <th field="price" width="90" formatter="FormatterTimeSetMethod1PriceUnit">价格</th>
                                    <th field="action" width="80" data-options="formatter:function(value, rowData, index){return FormatterTimeSetMethod2Action(value, rowData, index,<%= weekIndex[i] %>);}">操作
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div id="divToolbarTimeSetMethod2<%= weekIndex[i] %>">
                            <select class="easyui-combobox" id="txtTimeSetMethod2<%= weekIndex[i] %>StartHour" data-options="value:'09',valueField: 'value',textField: 'value',data:hoursData" style="width: 45px"></select>
                            <select class="easyui-combobox" id="txtTimeSetMethod2<%= weekIndex[i] %>StartMinute" data-options="value:'00',valueField: 'value',textField: 'value',data:minutesData" style="width: 45px"></select>
                            —
                    <select class="easyui-combobox" id="txtTimeSetMethod2<%= weekIndex[i] %>EndHour" data-options="editable:false,value:'10',valueField: 'value',textField: 'value',data:hoursData" style="width: 45px"></select>
                            <select class="easyui-combobox" id="txtTimeSetMethod2<%= weekIndex[i] %>EndMinute" data-options="editable:false,value:'00',valueField: 'value',textField: 'value',data:minutesData" style="width: 45px"></select>
                            <input type="text" id="txtTimeSetMethod2<%= weekIndex[i] %>Price" class="easyui-numberbox" value="0" data-options="min:0,precision:2" placeholder="价格(必填)" style="width: 60px" />元/小时
                    <a href="javascript:;" id="btnTimeSetMethod2<%= weekIndex[i] %>Add" class="easyui-linkbutton" iconcls="icon-add" title="添加时间段" onclick="AddTimeSetMethod2(<%= weekIndex[i] %>)">添加时间段</a>
                            <a href="javascript:;" id="btnTimeSetMethod2<%= weekIndex[i] %>Edit" class="easyui-linkbutton" iconcls="icon-edit" title="编辑时间段" onclick="ChangeTimeSetMethod2(<%= weekIndex[i] %>)" data-index="-1">编辑时间段</a>
                        </div>
                        <% } %>
                    </td>
                </tr>
                <%} %>
                <%if (!isAdded)
                  { %>
                <tr>
                    <td align="right" class="tdTitle">增值服务：
                <a href="javascript:SetAdded();" iconcls="icon-add" class="easyui-linkbutton">添加</a>
                    </td>
                    <td width="*" align="left" style="height: 240px;">
                        <table id="grvAddedData" class="easyui-datagrid" data-options="fit:true,fitcolumns:true">
                            <thead>
                                <tr>
                                    <th field="title" width="150" formatter="FormatterTitle">名称</th>
                                    <th field="price" width="150" formatter="FormatterPriceUnit">价格</th>
                                    <th field="action" width="40" formatter="FormatterAddedAction">操作</th>
                                </tr>
                            </thead>
                        </table>
                    </td>
                </tr>
                <%} %>
            </table>
        </div>
        <div style="text-align: center;">
            <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width: 160px;">提交</a>
            <a href="List.aspx?type=<%=categoryType %>" id="btnPageBack" class="button glow" style="width: 160px;">返回</a>
        </div>
    </div>
    <div id="dlgSetAdded" class="easyui-dialog" closed="true" modal="true" title="添加增值服务"
        style="width: 550px; height: 350px;">
        <table id="grvSetAddedData" fitcolumns="true">
            <thead>
                <tr>
                    <th field="ck" width="30" checkbox="true"></th>
                    <th field="title" width="100" formatter="FormatterTitle">名称</th>
                    <th field="price" width="60" formatter="FormatterPriceUnit">价格</th>
                </tr>
            </thead>
        </table>
        <div style="text-align: right; padding: 10px;">
            <a href="javascript:PostDlgSetAdded();" class="easyui-linkbutton">提交</a>
            <a href="javascript:CloseDlgSetAdded();" class="easyui-linkbutton">取消</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/lib/jquery/sortable/jquery-ui-sortable.min.js"></script>
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/booking/';
        var type = '<% =categoryType%>';
        var product_id = '<% =product_id%>';
        var relation_products = [];
        var timesetmethod_data = [];
        var timesetmethod2_data = {w1:[],w2:[],w3:[],w4:[],w5:[],w6:[],w0:[]};
        var hours = ['08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22'];
        var minutes = ['00','15','30','45'];
        var hoursData =[];
        var minutesData = [];
        var nDay = new Date().format('yyyy-MM-dd');
        function initHourAndMinute(){
            for (var i = 0; i < hours.length; i++) {
                hoursData.push({value:hours[i]});
            }
            for (var i = 0; i < minutes.length; i++) {
                minutesData.push({ value: minutes[i] });
            }
        }
        initHourAndMinute();
        var timesetmethod_data = [];
        $(function () {
            if (product_id != "0" && product_id != "") {
                loadOldData();
            }
            $("#btnSave").live("click", function () {
                postUpdateData();
            });
            <%if (!isAdded)
              { %>
            $(".ui-sortable div .deleteImg").live("click", function () {
                $(this).closest("div").remove();
            });
            $(".ui-sortable").sortable();
            $("#file1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'file1',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 addImgString(resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     });

                } catch (e) {
                    alert(e);
                }
            });
            <% if (nCategoryTypeConfig.TimeSetMethod == 1)
               { %>
            $("#btnTimeSetMethod1Edit").hide();
            <%}
               else if (nCategoryTypeConfig.TimeSetMethod == 2)
               { %>
            $("#btnTimeSetMethod20Edit").hide();
            $("#btnTimeSetMethod21Edit").hide();
            $("#btnTimeSetMethod22Edit").hide();
            $("#btnTimeSetMethod23Edit").hide();
            $("#btnTimeSetMethod24Edit").hide();
            $("#btnTimeSetMethod25Edit").hide();
            $("#btnTimeSetMethod26Edit").hide();
            <%} %>
            <%} %>
        });
        function addImgString(nSrc) {
            var appendhtml = new StringBuilder();
            appendhtml.AppendFormat('<div>');
            appendhtml.AppendFormat('<img alt="缩略图" src="{0}" width="130px" height="70px" class="rounded img" />', nSrc);
            appendhtml.AppendFormat('<img alt="删除" width="20" height="20" src="/img/delete.png" class="deleteImg"/>');
            appendhtml.AppendFormat('</div>');
            $(".ui-sortable").append(appendhtml.ToString());
        }
        function loadOldData() {
            $.ajax({
                type: 'post',
                url: handlerUrl + "Get.ashx",
                data: { product_id: product_id },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $("#txtPName").val(resp.result.product_title);
                        $("#txtSummary").val(resp.result.product_summary);
                        $("#txtPrice").numberbox("setValue", resp.result.price);
                        $("#txtUnit").val(resp.result.unit);
                        $("#txtSort").val(resp.result.sort);
                        if (resp.result.is_onsale == "1") {
                            rdoIsOnSale.checked = true;
                        } else {
                            rdoIsNoOnSale.checked = true;
                        }
                        $("#txtAccessLevel").val(resp.result.access_Level);
                        <%if (!isAdded)
                          { %>
                        $("#ddlCate").val(resp.result.category_id)
                        $("#txtCount").val(resp.result.totalcount);
                        if (resp.result.show_imgs) {
                            var nImgs = resp.result.show_imgs.split(",");
                            for (var i = 0; i < nImgs.length; i++) {
                                if ($.trim(nImgs[i]) != "") {
                                    addImgString(nImgs[i]);
                                }
                            }
                        }
                        relation_products = resp.result.relation_products;
                        $("#grvAddedData").datagrid("loadData", relation_products);
                        
            <% if (nCategoryTypeConfig.TimeSetMethod == 1)
               { %>
                        timesetmethod_data = resp.result.sku_list;
                        $("#grvTimeSetMethod1").datagrid("loadData", timesetmethod_data);
                        <%}
               else if (nCategoryTypeConfig.TimeSetMethod == 2)
               { %>
                        console.log(resp.result.sku_list);
                        console.log(timesetmethod2_data);
                        for (var i = 0; i < resp.result.sku_list.length; i++) {
                            timesetmethod2_data['w'+resp.result.sku_list[i].week].push(resp.result.sku_list[i]);
                        }
                        $("#grvTimeSetMethod20").datagrid("loadData", timesetmethod2_data['w0']);
                        $("#grvTimeSetMethod21").datagrid("loadData", timesetmethod2_data['w1']);
                        $("#grvTimeSetMethod22").datagrid("loadData", timesetmethod2_data['w2']);
                        $("#grvTimeSetMethod23").datagrid("loadData", timesetmethod2_data['w3']);
                        $("#grvTimeSetMethod24").datagrid("loadData", timesetmethod2_data['w4']);
                        $("#grvTimeSetMethod25").datagrid("loadData", timesetmethod2_data['w5']);
                        $("#grvTimeSetMethod26").datagrid("loadData", timesetmethod2_data['w6']);
                        <%} %>

                        <%} %>
                    }
                    else {
                        $.messager.alert("提示", resp.msg);
                    }
                }
            });
        }

        function postUpdateData() {
            var model = {
                product_id: product_id,
                product_title: $.trim($("#txtPName").val()),
                product_summary: $.trim($("#txtSummary").val()),
                price: $.trim($("#txtPrice").numberbox("getValue")),
                article_category_type: type,
                category_id: 0,
                totalcount: 1,
                unit: $.trim($("#txtUnit").val()),
                is_onsale: rdoIsOnSale.checked ? 1 : 0,
                sort: $.trim($("#txtSort").val()),
                access_Level: $.trim($("#txtAccessLevel").val()),
                time_set_method:0
            }
            if (model.unit == "") model.unit = "元";
            if (model.sort == "") model.sort = 0;
            if (model.access_Level == "") model.access_Level = 0;
            if (isNaN(model.price) || Number(model.price) < 0) {
                model.price = 0;
            }
            <%if (!isAdded)
              { %>
            var imgObjs = $(".img");
            var imgs = [];
            for (var i = 0; i < imgObjs.length; i++) {
                var nsrc = $.trim($(imgObjs[i]).attr("src"));
                if (nsrc == "") continue;
                imgs.push(nsrc);
            }
            if (imgs.length > 0) {
                model.show_imgs = imgs.join(",");
            }
            model.category_id = $.trim($("#ddlCate").val());
            if (model.category_id == "") model.category_id = 0;
            model.totalcount = $.trim($("#txtCount").val());
            if (model.totalcount == "" || model.totalcount == 0) model.totalcount = 1;

            var relation_product_ids = [];
            for (var i = 0; i < relation_products.length; i++) {
                if (relation_product_ids.indexOf(relation_products[i].product_id) < 0) {
                    relation_product_ids.push(relation_products[i].product_id);
                }
            }
            if (relation_product_ids.length > 0) {
                model.relation_product_id = relation_product_ids.join(",");
            }
            model.time_set_method = <%=nCategoryTypeConfig.TimeSetMethod %>;
            if(model.time_set_method==1){
                var time_data = [];
                for (var i = 0; i < timesetmethod_data.length; i++) {
                    time_data.push({id:timesetmethod_data[i].id,ex1:timesetmethod_data[i].start,ex2:timesetmethod_data[i].end,price:timesetmethod_data[i].price});
                }
                model.time_data = JSON.stringify(time_data);
            }
            else if(model.time_set_method==2){
                var time_data = [];
                for (var _ix = 0; _ix < 7; _ix++) {
                    for (var i = 0; i < timesetmethod2_data['w'+_ix].length; i++) {
                        time_data.push({
                            id:timesetmethod2_data['w'+_ix][i].id,
                            ex1:timesetmethod2_data['w'+_ix][i].start,
                            ex2:timesetmethod2_data['w'+_ix][i].end,
                            ex3:timesetmethod2_data['w'+_ix][i].week,
                            price:timesetmethod2_data['w'+_ix][i].price
                        });
                    }
                }
                model.time_data = JSON.stringify(time_data);
            }
            <%} %>
            $.messager.progress({ text: '正在提交。。。' });
            $.ajax({
                type: 'post',
                url: handlerUrl + (product_id == "0" ? "Add.ashx" : "Update.ashx"),
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        if (product_id == "0") {
                            ClearForm();
                            Alert(resp.msg);
                        }
                        else {
                            location.href = "List.aspx?type=<%=categoryType %>";
                        }
                    }
                    else {
                        Alert(resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
        function ClearForm() {
            $("#txtPName").val("");
            $("#txtSummary").val("");
            $("#txtPrice").numberbox("setValue", 0);
            $("#txtUnit").val("元");
            $("#txtSort").val("0");
            $("#txtAccessLevel").val("0");
            rdoIsOnSale.checked = true;
            <%if (!isAdded)
              { %>
            $("#ddlCate").val("0");
            $("#txtCount").val("1");
            $(".ui-sortable").html('');
            relation_products = [];
            $("#grvAddedData").datagrid("loadData", relation_products);
            timesetmethod_data = [];
            $("#grvTimeSetMethod1").datagrid("loadData", timesetmethod_data);
            <%} %>
        }
        function FormatterAddedAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:DeleteAdded(\'{0}\');"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>&nbsp;', rowData['product_id']);
            return str.ToString();
        }
        function DeleteAdded(nproduct_id) {
            for (var i = 0; i < relation_products.length; i++) {
                if (relation_products[i].product_id == nproduct_id) {
                    relation_products.splice(i, 1);
                    break;
                }
            }
            $("#grvAddedData").datagrid("loadData", relation_products);
        }
        function FormatterPriceUnit(value, rowData) {
            return value + " " + rowData.unit;
        }
        function SetAdded() {
            $('#grvSetAddedData').datagrid({
                url: handlerUrl + "list.ashx",
                loadFilter: pagerFilter,
                pagination: true,
                pageSize: 50,
                height: 280,
                rownumbers: true,
                queryParams: { type: type + "Added" },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $.messager.progress({ title: "正在加载" });
                },
                onLoadSuccess: function (data) {
                    $.messager.progress("close");
                    $('#dlgSetAdded').dialog("resize", { width: 550, height: 350, top: $(document).scrollTop() + 70 });
                    $('#dlgSetAdded').dialog('open');
                }
            });
        }
        function PostDlgSetAdded() {
            var rows = $('#grvSetAddedData').datagrid('getSelections');
            if (rows.length == 0) {
                $('#dlgSetAdded').dialog('close');
                return true;
            }
            var hasProduct = false;
            for (var i = 0; i < rows.length; i++) {
                if (relation_products.length == 0) {
                    relation_products.push({ product_id: rows[i].product_id, title: rows[i].title, price: rows[i].price, unit: rows[i].unit });
                }
                else {
                    var nHasProduct = false;
                    for (var j = 0; j < relation_products.length; j++) {
                        if (relation_products[j].product_id == rows[i].product_id) {
                            nHasProduct = true;
                            hasProduct = true;
                            break;
                        }
                    }
                    if (!nHasProduct) relation_products.push({ product_id: rows[i].product_id, title: rows[i].title, price: rows[i].price, unit: rows[i].unit });
                }
            }
            $("#grvAddedData").datagrid("loadData", relation_products);
            $('#dlgSetAdded').dialog('close');
            if (hasProduct) {
                $.messager.alert("提示", "重复的增值服务已排除");
            }
        }
        function CloseDlgSetAdded() {
            $('#dlgSetAdded').dialog('close');
        }
        function FormatterTimeSetMethod1Action(value, rowData, index) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:EditTimeSetMethod1({0});"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_edit.gif" title="编辑" /></a>&nbsp;', index);
            str.AppendFormat('<a href="javascript:DeleteTimeSetMethod1({0});"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', index);
            return str.ToString();
        }
        function FormatterTimeSetMethod2Action(value, rowData, index, _ix) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:EditTimeSetMethod2({0},{1});"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_edit.gif" title="编辑" /></a>&nbsp;', index,_ix);
            str.AppendFormat('<a href="javascript:DeleteTimeSetMethod2({0},{1});"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', index,_ix);
            return str.ToString();
        }
        function DeleteTimeSetMethod1(index) {
            for (var i = 0; i < timesetmethod_data.length; i++) {
                if (index == i) {
                    timesetmethod_data.splice(i, 1);
                    break;
                }
            }
            $("#grvTimeSetMethod1").datagrid("loadData", timesetmethod_data);
        }
        function EditTimeSetMethod1(index){
            var startDate = new Date(timesetmethod_data[index].start);
            var endDate = new Date(timesetmethod_data[index].end);
            var price = timesetmethod_data[index].price;
            var priceNum = price * 60 * 60000 / (endDate.getTime() - startDate.getTime());
            $("#txtTimeSetMethod1Start").datebox("setValue",startDate.format('yyyy-MM-dd'));
            $("#txtTimeSetMethod1StartHour").combobox("setValue",startDate.format('hh'));
            $("#txtTimeSetMethod1EndHour").combobox("setValue",endDate.format('hh'));
            $("#txtTimeSetMethod1StartMinute").combobox("setValue",startDate.format('mm'));
            $("#txtTimeSetMethod1EndMinute").combobox("setValue",endDate.format('mm'));
            $("#txtTimeSetMethod1Price").numberbox("setValue",priceNum);
            $("#btnTimeSetMethod1Add").hide();
            $("#btnTimeSetMethod1Edit").attr("data-index",index);
            $("#btnTimeSetMethod1Edit").show();
        }
        function DeleteTimeSetMethod2(index,_ix) {
            for (var i = 0; i < timesetmethod2_data['w'+_ix].length; i++) {
                if (index == i) {
                    timesetmethod2_data['w'+_ix].splice(i, 1);
                    break;
                }
            }
            $("#grvTimeSetMethod2"+_ix).datagrid("loadData", timesetmethod2_data['w'+_ix]);
            $("#btnTimeSetMethod2"+_ix+"Edit").attr("data-index",-1);
        }
        function EditTimeSetMethod2(index,_ix){
            var startDate = new Date(nDay + " " + timesetmethod2_data['w'+_ix][index].start);
            var endDate = new Date(nDay + " " + timesetmethod2_data['w'+_ix][index].end);
            var price = timesetmethod2_data['w'+_ix][index].price;
            var priceNum = price * 60 * 60000 / (endDate.getTime() - startDate.getTime());
            $("#txtTimeSetMethod2"+_ix+"StartHour").combobox("setValue",startDate.format('hh'));
            $("#txtTimeSetMethod2"+_ix+"EndHour").combobox("setValue",endDate.format('hh'));
            $("#txtTimeSetMethod2"+_ix+"StartMinute").combobox("setValue",startDate.format('mm'));
            $("#txtTimeSetMethod2"+_ix+"EndMinute").combobox("setValue",endDate.format('mm'));
            $("#txtTimeSetMethod2"+_ix+"Price").numberbox("setValue",priceNum);
            $("#btnTimeSetMethod2"+_ix+"Add").hide();
            $("#btnTimeSetMethod2"+_ix+"Edit").attr("data-index",index);
            $("#btnTimeSetMethod2"+_ix+"Edit").show();
        }
        function ChangeTimeSetMethod1(){
            var index = $("#btnTimeSetMethod1Edit").attr("data-index");
            var startDate = new Date($("#txtTimeSetMethod1Start").datebox("getValue") + " " + $("#txtTimeSetMethod1StartHour").combobox("getValue") + ":" + $("#txtTimeSetMethod1StartMinute").combobox("getValue"));
            var endDate = new Date($("#txtTimeSetMethod1Start").datebox("getValue") + " " + $("#txtTimeSetMethod1EndHour").combobox("getValue") + ":" + $("#txtTimeSetMethod1EndMinute").combobox("getValue"));
            if (!checkTime(startDate, endDate)) {
                return;
            }
            if (!checkTimeInData(timesetmethod_data,startDate, endDate,index)) {
                return;
            }
            var priceNum = $("#txtTimeSetMethod1Price").numberbox("getValue");
            if(isNaN(priceNum)){
                $.messager.alert("提示", "价格请输入数字");
                return;
            }
            var price =  (priceNum / 60) * ((endDate.getTime() - startDate.getTime()) / 60000);
            timesetmethod_data[index].start = startDate.format('yyyy-MM-dd hh:mm');
            timesetmethod_data[index].end = endDate.format('yyyy-MM-dd hh:mm');
            timesetmethod_data[index].price = price;
            
            $("#btnTimeSetMethod1Edit").hide();
            $("#btnTimeSetMethod1Edit").attr("data-index",-1);
            $("#btnTimeSetMethod1Add").show();
            $("#grvTimeSetMethod1").datagrid("loadData", timesetmethod_data);
        }
        function AddTimeSetMethod1() {
            var startDate = new Date($("#txtTimeSetMethod1Start").datebox("getValue") + " " + $("#txtTimeSetMethod1StartHour").combobox("getValue") + ":" + $("#txtTimeSetMethod1StartMinute").combobox("getValue"));
            var endDate = new Date($("#txtTimeSetMethod1Start").datebox("getValue") + " " + $("#txtTimeSetMethod1EndHour").combobox("getValue") + ":" + $("#txtTimeSetMethod1EndMinute").combobox("getValue"));
            if (!checkTime(startDate, endDate)) {
                return;
            }
            if (!checkTimeInData(timesetmethod_data,startDate, endDate, -1)) {
                return;
            }
            var priceNum = $("#txtTimeSetMethod1Price").numberbox("getValue");
            if(isNaN(priceNum)){
                $.messager.alert("提示", "价格请输入数字");
                return;
            }
            var price =  (priceNum / 60) * ((endDate.getTime() - startDate.getTime()) / 60000);
            timesetmethod_data.push({id:0, start: startDate.format('yyyy-MM-dd hh:mm'), end: endDate.format('yyyy-MM-dd hh:mm') ,price: price});
            $("#grvTimeSetMethod1").datagrid("loadData", timesetmethod_data);
        }
        function ChangeTimeSetMethod2(_ix){
            var index = $("#btnTimeSetMethod2"+_ix+"Edit").attr("data-index");
            var startDate = new Date(nDay + " " + $("#txtTimeSetMethod2"+_ix+"StartHour").combobox("getValue") + ":" + $("#txtTimeSetMethod2"+_ix+"StartMinute").combobox("getValue"));
            var endDate = new Date(nDay + " " + $("#txtTimeSetMethod2"+_ix+"EndHour").combobox("getValue") + ":" + $("#txtTimeSetMethod2"+_ix+"EndMinute").combobox("getValue"));
            if (!checkTime(startDate, endDate)) {
                return;
            }
            if (!checkTimeInData2(timesetmethod2_data['w'+_ix],startDate, endDate,index)) {
                return;
            }
            var priceNum = $("#txtTimeSetMethod2"+_ix+"Price").numberbox("getValue");
            if(isNaN(priceNum)){
                $.messager.alert("提示", "价格请输入数字");
                return;
            }
            var price =  (priceNum / 60) * ((endDate.getTime() - startDate.getTime()) / 60000);

            timesetmethod2_data['w'+_ix][index].start = startDate.format('hh:mm');
            timesetmethod2_data['w'+_ix][index].end = endDate.format('hh:mm');
            timesetmethod2_data['w'+_ix][index].price = price;
            
            $("#btnTimeSetMethod2"+_ix+"Edit").hide();
            $("#btnTimeSetMethod2"+_ix+"Edit").attr("data-index",-1);
            $("#btnTimeSetMethod2"+_ix+"Add").show();
            $("#grvTimeSetMethod2"+_ix+"").datagrid("loadData", timesetmethod2_data['w'+_ix]);
        }
        function AddTimeSetMethod2(_ix) {
            var startDate = new Date(nDay + " " + $("#txtTimeSetMethod2"+_ix+"StartHour").combobox("getValue") + ":" + $("#txtTimeSetMethod2"+_ix+"StartMinute").combobox("getValue"));
            var endDate = new Date(nDay + " " + $("#txtTimeSetMethod2"+_ix+"EndHour").combobox("getValue") + ":" + $("#txtTimeSetMethod2"+_ix+"EndMinute").combobox("getValue"));
            if (!checkTime(startDate, endDate)) {
                return;
            }
            if (!checkTimeInData2(timesetmethod2_data['w'+_ix],startDate, endDate, -1)) {
                return;
            }
            var priceNum = $("#txtTimeSetMethod2"+_ix+"Price").numberbox("getValue");
            if(isNaN(priceNum)){
                $.messager.alert("提示", "价格请输入数字");
                return;
            }
            var price =  (priceNum / 60) * ((endDate.getTime() - startDate.getTime()) / 60000);
            timesetmethod2_data['w'+_ix].push({id:0, start: startDate.format('hh:mm'), end: endDate.format('hh:mm'), week:_ix,price: price});
            $("#grvTimeSetMethod2"+_ix+"").datagrid("loadData", timesetmethod2_data['w'+_ix]);
        }
        function checkTime(startDate, endDate) {
            if (isNaN(startDate.getTime())) {
                $.messager.alert("提示", "开始时间格式错误");
                return false;
            }
            if (isNaN(endDate.getTime())) {
                $.messager.alert("提示", "结束时间格式错误");
                return false;
            }
            if (endDate <= startDate) {
                $.messager.alert("提示", "结束时间不能小于等于开始时间");
                return false;
            }
            return true;
        }
        function checkTimeInData(data, startDate, endDate, index) {
            var hasRp = false;
            for (var i = 0; i < data.length; i++) {
                if(i == index) continue;
                var dstart = new Date(data[i].start);
                var dend = new Date(data[i].end);
                if (!((startDate >= dend && endDate > dend) || (startDate < dstart && endDate <= dstart))) {
                    hasRp = true;
                    break;
                }
            }
            if (hasRp) {
                $.messager.alert("提示", "所选时间不能与已有时间交集");
                return false;
            }
            return true;
        }
        
        function checkTimeInData2(data, startDate, endDate, index) {
            var hasRp = false;
            for (var i = 0; i < data.length; i++) {
                if(i == index) continue;
                var dstart = new Date(nDay + " " + data[i].start);
                var dend = new Date(nDay + " " +data[i].end);
                if (!((startDate >= dend && endDate > dend) || (startDate < dstart && endDate <= dstart))) {
                    hasRp = true;
                    break;
                }
            }
            if (hasRp) {
                $.messager.alert("提示", "所选时间不能与已有时间交集");
                return false;
            }
            return true;
        }
        function onChangePrice(newValue, oldValue){
            $("#txtTimeSetMethod1Price").numberbox("setValue",newValue);
        }
        function FormatterTimeSetMethod1PriceUnit(value, rowData) {
            return value + "元";
        }
    </script>
</asp:Content>
