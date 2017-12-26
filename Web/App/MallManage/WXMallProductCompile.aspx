<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallProductCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallProductCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        }
        
        
        .title
        {
            font-size: 12px;
        }
        .return
        {
            float: right;
            margin-right: 5px;
        }
        input[type=text], select
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        .rmb
        {
            color: Red;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="WXMallProductMgr.aspx" title="返回商品列表">商品管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=headTitle%></span>
        <a href="WXMallProductMgr.aspx" style="float: right; margin-right: 20px;" title="返回商品列表"
            class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        商品名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPName" value=" <%=productModel.PName%>" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        原价：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtPreviousPrice" value="<%=productModel.PreviousPrice==0 ? "" :productModel.PreviousPrice.ToString()%>">
                        <label class="rmb">
                            元</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        现价：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtPrice" value="<%=productModel.Price ==0 ? "" : productModel.Price.ToString()%>">
                        <label class="rmb">
                            元</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        库存：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtStock" value="<%=productModel.Stock%>">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        商品主图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=productModel.RecommendImg == null ? "" : productModel.RecommendImg%>"
                            width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机图片</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片1：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage1%>" width="400px" height="200" id="showimage1" /><br />
                        <a id="ashowimage1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="fileshowimage1.click()">上传图片(提示:图片最佳尺寸为 600*600)</a>
                        <input type="file" id="fileshowimage1" name="fileshowimage1" style="display: none;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片2：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage2%>" width="400px" height="200" id="showimage2" /><br />
                        <a id="a1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileshowimage2.click()">上传图片(提示:图片最佳尺寸为 600*600)</a>
                        <input type="file" id="fileshowimage2" name="fileshowimage2" style="display: none;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片3：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage3%>" width="400px" height="200" id="showimage3" /><br />
                        <a id="a2" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileshowimage3.click()">上传图片(提示:图片最佳尺寸为 600*600)</a>
                        <input type="file" id="fileshowimage3" name="fileshowimage3" style="display: none;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片4：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage4%>" width="400px" height="200" id="showimage4" /><br />
                        <a id="a3" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileshowimage4.click()">上传图片(提示:图片最佳尺寸为 600*600)</a>
                        <input type="file" id="fileshowimage4" name="fileshowimage4" style="display: none;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片5：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage5%>" width="400px" height="200" id="showimage5" /><br />
                        <a id="a4" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileshowimage5.click()">上传图片(提示:图片最佳尺寸为 600*600)</a>
                        <input type="file" id="fileshowimage5" name="fileshowimage5" style="display: none;" />
                    </td>
                </tr>
                <%--                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        所属门店：
                    </td>
                    <td width="*" align="left">
                    <select id="ddlstore">
                        <option value="">无</option>
                        <%=sbStores.ToString()%>
                    </select>
                     还没有门店?&nbsp;<a style="color:Blue;" href="/App/MallManage/WXMallStoresMgr.aspx">去添加</a>
                    </td>
                </tr>--%>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        商品分类：
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                        还没有分类?&nbsp;<a style="color: Blue;" href="/App/MallManage/WXMallCategoryMgr.aspx">去添加</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        是否上架：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdoIsOnSale" id="rdoOnSale" checked="checked" /><label
                            for="rdoOnSale">上架</label>
                        <input type="radio" name="rdoIsOnSale" id="rdoNotOnSale" /><label for="rdoNotOnSale">下架</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标签：
                    </td>
                    <td width="*" align="left">
                        <input type="checkbox" id="cbhot" value="1" /><label id="lblhot" for="cbhot">热卖</label>
                        <input type="checkbox" id="cbnew" value="1" /><label id="lblnew" for="cbnew">最新上架</label>
                        <input type="checkbox" id="cbspecial" value="1" /><label id="lblspecial" for="cbspecial">特价商品</label>

                   <input type="checkbox" id="cbrecommend" value="1" /><label id="lblrecommend" for="cbrecommend">推荐</label>
                    </td>
                </tr>
                
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            商品介绍：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                                <%=productModel.PDescription == null ? "" : productModel.PDescription%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px; text-decoration: underline;"
                            class="button button-rounded button-primary">保存</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var editor;
        var grid;
        $(function () {

            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
            if (currAction == 'add') {
                GetRandomHb();
            }
            else { //编辑
                var categoryid = "<%=productModel.CategoryId%>"; //分类编号
                $("#ddlcategory").val(categoryid);

                var isonsale = "<%=productModel.IsOnSale%>"; //是否上架
                if (isonsale == "1") {
                    $("#rdoOnSale").attr("checked", "checked");
                }
                else {
                    $("#rdoNotOnSale").attr("checked", "checked");
                }





            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {

                        PID: "<%=pid%>",
                        PName: $.trim($('#txtPName').val()),
                        PreviousPrice: $.trim($("#txtPreviousPrice").val()),
                        CategoryId: $.trim($("#ddlcategory").val()),
                        Price: $.trim($('#txtPrice').val()),
                        PDescription: editor.html(),
                        RecommendImg: $('#imgThumbnailsPath').attr('src'),
                        IsOnSale: rdoOnSale.checked ? 1 : 0,
                        Stock: $("#txtStock").val(),
                        ShowImage1: $("#showimage1").attr("src"),
                        ShowImage2: $("#showimage2").attr("src"),
                        ShowImage3: $("#showimage3").attr("src"),
                        ShowImage4: $("#showimage4").attr("src"),
                        ShowImage5: $("#showimage5").attr("src"),
                        Action: currAction == 'add' ? 'AddWXMallProductInfo' : 'EditWXMallProductInfo'

                    };
                    if (model.PName == '') {
                        $('#txtPName').focus();
                        Alert('请输入商品名称！');
                        return;
                    }
                    if (model.Price == '') {
                        $('#txtPrice').focus();
                        Alert('请输入商品价格！');
                        return;
                    }


                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
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

            //商品主图
            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片。。。' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
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


            //展示照片1
            $("#fileshowimage1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=fileshowimage1',
                         secureuri: false,
                         fileElementId: 'fileshowimage1',
                         dataType: 'text',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#showimage1').attr('src', resp.ExStr);
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

            //展示照片2
            $("#fileshowimage2").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=fileshowimage2',
                         secureuri: false,
                         fileElementId: 'fileshowimage2',
                         dataType: 'text',
                         success: function (result) {

                             var resp = $.parseJSON(result);
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#showimage2').attr('src', resp.ExStr);
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

            //展示照片3
            $("#fileshowimage3").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=fileshowimage3',
                         secureuri: false,
                         fileElementId: 'fileshowimage3',
                         dataType: 'text',
                         success: function (result) {
                             var resp = $.parseJSON(result);
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#showimage3').attr('src', resp.ExStr);
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

            //展示照片4
            $("#fileshowimage4").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=fileshowimage4',
                         secureuri: false,
                         fileElementId: 'fileshowimage4',
                         dataType: 'text',
                         success: function (result) {

                             var resp = $.parseJSON(result);
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#showimage4').attr('src', resp.ExStr);
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

            //展示照片5
            $("#fileshowimage5").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=fileshowimage5',
                         secureuri: false,
                         fileElementId: 'fileshowimage5',
                         dataType: 'text',
                         success: function (result) {

                             var resp = $.parseJSON(result);
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#showimage5').attr('src', resp.ExStr);
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
            ClearAll();
            editor.html('');
        }

        //获取随机图片
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
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
