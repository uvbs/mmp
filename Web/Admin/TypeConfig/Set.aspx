<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Set.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.TypeConfig.Set" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=nCategoryTypeConfig.CategoryTypeDispalyName %>类型设置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable" style="width: 100%">
                <tr style="display:none;">
                    <td style="width: 140px;" align="right" class="tdTitle">类型：
                    </td>
                    <td width="*" align="left" style="width: 400px;">
                        <input type="text" id="txtType" class="commonTxt" placeholder="类型(必填)" value="<%=nCategoryTypeConfig.CategoryType %>" />
                    </td>
                    <td rowspan="5">
                        <img alt="说明" style="width:300px;margin-left: 13px;" src="http://open-files.comeoncloud.net/www/comeoncloud/jubit/image/20160413/9E81BAF557914D09BB155B66D863C548.png" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">时间方式：
                    </td>
                    <td width="*" align="left" style="width: 400px;">
                        <input id="rdoTimeSetMethod0" name="TimeSetMethod" class="positionTop2" type="radio" data-value="0" <%=nCategoryTypeConfig.TimeSetMethod==0?"checked='checked'":"" %> /><label for="rdoTimeSetMethod0">日历时间</label>
                        <input id="rdoTimeSetMethod1" name="TimeSetMethod" class="positionTop2" type="radio" data-value="1" <%=nCategoryTypeConfig.TimeSetMethod==1?"checked='checked'":"" %> /><label for="rdoTimeSetMethod1">设置时间</label>
                        <input id="rdoTimeSetMethod2" name="TimeSetMethod" class="positionTop2" type="radio" data-value="2" <%=nCategoryTypeConfig.TimeSetMethod==2?"checked='checked'":"" %> /><label for="rdoTimeSetMethod2">设置周期</label>
                    </td>
                    <td rowspan="5">
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">时间展示：
                    </td>
                    <td width="*" align="left" style="width: 400px;">
                        <input id="rdoTimeSetStyle0" name="TimeSetStyle" class="positionTop2" type="radio" data-value="0" <%= (nCategoryTypeConfig.TimeSetMethod!=1 || nCategoryTypeConfig.TimeSetStyle==0)?"checked='checked'":"" %> /><label for="rdoTimeSetStyle0">日历</label>
                        <input id="rdoTimeSetStyle1" name="TimeSetStyle" class="positionTop2" type="radio" data-value="1" <%= nCategoryTypeConfig.TimeSetMethod==1 && nCategoryTypeConfig.TimeSetStyle==1?"checked='checked'":"" %> style="display:<%=nCategoryTypeConfig.TimeSetMethod==1?"inline":"none" %>"  /><label for="rdoTimeSetStyle1" style="display:<%=nCategoryTypeConfig.TimeSetMethod==1?"inline":"none" %>">列表</label>
                    </td>
                    <td rowspan="5">
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">消费方式：
                    </td>
                    <td width="*" align="left" style="width: 400px;">
                        <input id="rdoSpendMethod0" name="SpendMethod" class="positionTop2" type="radio" data-value="0" <%=nCategoryTypeConfig.SpendMethod==0?"checked='checked'":"" %> /><label for="rdoSpendMethod0">金额和积分组合</label>
                        <input id="rdoSpendMethod1" name="SpendMethod" class="positionTop2" type="radio" data-value="1" <%=nCategoryTypeConfig.SpendMethod==1?"checked='checked'":"" %> /><label for="rdoSpendMethod1">金额消费</label>
                        <input id="rdoSpendMethod2" name="SpendMethod" class="positionTop2" type="radio" data-value="2" <%=nCategoryTypeConfig.SpendMethod==2?"checked='checked'":"" %> /><label for="rdoSpendMethod2">积分消费</label>
                    </td>
                    <td rowspan="5">
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">微信标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" class="commonTxt" placeholder="微信标题(必填)" value="<%=nCategoryTypeConfig.CategoryTypeTitle %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">首页标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtHomeTitle" class="commonTxt" placeholder="首页标题(必填)" value="<%=nCategoryTypeConfig.CategoryTypeHomeTitle %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">订单列表标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtOrderListTitle" class="commonTxt" placeholder="订单列表标题(必填)" value="<%=nCategoryTypeConfig.CategoryTypeOrderListTitle %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">订单详情标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtOrderDetailTitle" class="commonTxt" placeholder="订单详情标题(必填)" value="<%=nCategoryTypeConfig.CategoryTypeOrderDetailTitle %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">商品别名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtDispalyName" class="commonTxt" placeholder="商品别名(必填)" value="<%=nCategoryTypeConfig.CategoryTypeDispalyName %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分类别名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtExDispalyName" class="commonTxt" placeholder="分类别名(必填)" value="<%=nCategoryTypeConfig.CategoryTypeExDispalyName %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">容量别名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtStockName" class="commonTxt" placeholder="容量别名(必填)" value="<%=nCategoryTypeConfig.CategoryTypeStockName %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareTitle" class="commonTxt" placeholder="分享标题(选填)" value="<%=nCategoryTypeConfig.ShareTitle %>" />
                    </td>
                    <td>

                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享图片：
                    </td>
                    <td width="*" align="left" colspan="2">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" />
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display:none;" />
                        <input type="text" id="txtShareImg" class="commonTxt" style="width:90%;" placeholder="分享图片" value="<%=nCategoryTypeConfig.ShareImg %>" />
                        <br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="提示" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为140*140。
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareDesc" class="commonTxt" placeholder="分享描述(选填)" value="<%=nCategoryTypeConfig.ShareDesc %>" />
                    </td>
                    <td>

                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareLink" class="commonTxt" placeholder="分享链接(选填)" value="<%=nCategoryTypeConfig.ShareLink %>" />
                    </td>
                    <td>

                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center;">
            <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width: 160px;">提交</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/typeconfig/';
        $(function () {
            checkImg();
            $("input[name='TimeSetMethod']").bind("click", function () {
                var nTimeSetMethod = $.trim($(this).attr("data-value"));
                if (nTimeSetMethod != "1") {
                    $("input[name='TimeSetStyle']").each(function () {
                        var nTimeSetStyle = $.trim($(this).attr("data-value"));
                        if (nTimeSetStyle == "0") { this.checked = true; }
                        else {
                            $(this).hide();
                            $(this).next().hide();
                        }
                    })
                }
                else {
                    $("input[name='TimeSetStyle']").show();
                    $("input[name='TimeSetStyle']").next().show();
                }
            });
            $("#txtShareImg").bind("change", function () {
                checkImg();
            });
            $('#txtThumbnailsPath').on('change', function () {
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
                                $("#txtShareImg").val(resp.ExStr);
                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
            $("#btnSave").bind("click", function () {
                postSet();
            });
        });
        function checkImg(){
            var sImg = $.trim($("#txtShareImg").val());
            if (sImg != "") imgThumbnailsPath.src = sImg;
        }
        function getModel() {
            var model = {
                type: $.trim($("#txtType").val()),
                time_set_method: $.trim($("input[name='TimeSetMethod']:checked").attr("data-value")),
                time_set_style: $.trim($("input[name='TimeSetStyle']:checked").attr("data-value")),
                spend_method: $.trim($("input[name='SpendMethod']:checked").attr("data-value")),
                title: $.trim($("#txtTitle").val()),
                home_title: $.trim($("#txtHomeTitle").val()),
                order_list_title: $.trim($("#txtOrderListTitle").val()),
                order_detail_title: $.trim($("#txtOrderDetailTitle").val()),
                name: $.trim($("#txtDispalyName").val()),
                name_ex: $.trim($("#txtExDispalyName").val()),
                stock_name: $.trim($("#txtStockName").val()),
                share_title: $.trim($("#txtShareTitle").val()),
                share_img: $.trim($("#txtShareImg").val()),
                share_desc: $.trim($("#txtShareDesc").val()),
                share_link: $.trim($("#txtShareLink").val())
            }
            return model;
        }
        function checkModel(model) {
            if (model.type == "") {
                $.messager.alert("提示", "请输入类型");
                $("#txtType").focus();
                return false;
            }
            if (model.title == "") {
                $.messager.alert("提示", "请输入微信标题");
                $("#txtTitle").focus();
                return false;
            }
            if (model.home_title == "") {
                $.messager.alert("提示", "请输入首页标题");
                $("#txtHomeTitle").focus();
                return false;
            }
            if (model.name == "") {
                $.messager.alert("提示", "请输入商品别名");
                $("#txtDispalyName").focus();
                return false;
            }
            if (model.name_ex == "") {
                $.messager.alert("提示", "请输入分类别名");
                $("#txtExDispalyName").focus();
                return false;
            }
            return true;
        }
        function postSet() {
            var model = getModel();
            if (!checkModel(model)) {
                return;
            }
            $.messager.progress({ text: '正在提交...' });
            $.ajax({
                type: 'post',
                url: handlerUrl + "Set.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    $.messager.alert("提示", resp.msg);
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
    </script>
</asp:Content>
