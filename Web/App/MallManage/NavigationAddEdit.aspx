<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="NavigationAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.NavigationAddEdit" %>

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
            width: 80%;
        }
        #txtSort
        {
            width: 50px;
        }
        
        #ddlType
        {
            width: 100px;
        }
        #ddlPreNavigation
        {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="SlideMgrV2.aspx" title="返回">导航管理</a>&gt;&nbsp;<span><%=HeadTitle%></span>
        <a href="NavigationMgr.aspx" style="float: right; margin-right: 20px;" title="返回"
            class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        类型：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlType">
                            <option value="top">顶部导航</option>
                            <option value="left">左侧导航</option>
                            <option value="bottom">底部导航</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        上一级导航：
                    </td>
                    <td width="*" align="left" id="tdPre">
                        <select id="ddlPreNavigation">
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNavigationName" value="<%=model.NavigationName%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="<%=model.NavigationImage%>" id="imgThumbnailsPath" />
                        <br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNavigationLink" value="<%=model.NavigationLink%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSort" value="<%=model.Sort%>" onkeyup="this.value=this.value.replace(/\D/g,'')"
                            onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                        提示：排序号越大越靠前显示
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    </td>
                    <td width="*" align="center">
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
                //GetRandomHb();
                 GetParentNavigation("top",0);
            }
            else {

            $("#ddlType").val("<%=model.NavigationLinkType%>");
            GetParentNavigation("<%=model.NavigationLinkType%>",<%=model.ParentId%>);

            }
            $(ddlType).change(function () {


                GetParentNavigation($(this).val(),<%=model.ParentId%>);


            })

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        AutoID: "<%=model.AutoID%>",
                        NavigationImage: $('#imgThumbnailsPath').attr('src'),
                        NavigationLink: $.trim($('#txtNavigationLink').val()),
                        Sort: $("#txtSort").val(),
                        ParentId:0,
                        NavigationName: $("#txtNavigationName").val(),
                        NavigationLinkType: $("#ddlType").val(),
                        Action: currAction == 'add' ? 'AddNavigation' : 'EditNavigation'

                    };
                    if (model.NavigationName == "") {
                        Alert("名称必填");
                        return false;
                    }
                    if ($(ddlPreNavigation).val()!=undefined&&$(ddlPreNavigation).val()!='') {
                        model.ParentId =$(ddlPreNavigation).val();
}

                    if (model.Sort == "") {
                        model.Sort = 0;
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
                                Alert(resp.Msg);
                                window.location.href = "NavigationMgr.aspx";
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



        });


        //获取随机图片
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $('#imgThumbnailsPath').attr('src', "/img/hb/hb" + randInt + ".jpg");
        }



        //获取上级导航菜单
        function GetParentNavigation(type,value) { 
                    var model =
                    {
                        
                        Action:'GeNavigationTree',
                        NavigationLinkType: type
                        

                    };

        $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "html",
                        success: function (resp) {
                           $(tdPre).html(resp);
                           $(ddlPreNavigation).val(value);
                        }
                    });
        
        
        }


    </script>
</asp:Content>
