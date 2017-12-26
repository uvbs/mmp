<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ToolBarCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.ToolBarCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="http://static-files.socialcrmyun.com/lib/ionic/ionic.css" rel="stylesheet"/>
    <link href="/Plugins/angular/zcSelectLink/zcSelectLink.css?v=20160818" rel="stylesheet" />
    <style type="text/css">
        .tdTitle {
            font-weight: bold;
        }

        table td {
            height: 30px;
        }

        input[type=text], select, textarea {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        .liIco {
            float: left;
            width: 35px;
            height: 35px;
            cursor: pointer;
            padding: 0px;
            margin: 5px;
            border: #d5d5d5 solid 1px;
            font-size:33px;
        }
        .icon {
           width: 1em; height: 1em;
           vertical-align: -0.15em;
           fill: currentColor;
           overflow: hidden;
        }

        .divIconImg {
            height: 80px;
            font-size: 80px;
        }
        .color{float:left;}
        a.l-btn span.l-btn-left{height:24px;}
        #dlgInput .dialog-content{overflow-x:hidden;}
        .sp-replacer{
            width:20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="/App/CompanyWebsite/ToolBarManage.aspx?use_type=<%=this.Request["use_type"] %>&is_system=<%= this.Request["is_system"]%>"><%=new ZentCloud.BLLJIMP.BLLCompanyWebSite().GetToolBarUseTypeName(this.Request["use_type"]) %>管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%=new ZentCloud.BLLJIMP.BLLCompanyWebSite().GetToolBarUseTypeName(this.Request["use_type"]) %><%if (model != null && webAction == "edit") { Response.Write("：" + model.ToolBarName); } %></span>
    <a href="/App/CompanyWebsite/ToolBarManage.aspx?use_type=<%=this.Request["use_type"] %>&is_system=<%= this.Request["is_system"]%>&isPc=<%= this.Request["isPc"]%>" style="float: right; margin-right: 20px;" class="easyui-linkbutton" iconcls="icon-back" plain="true" id="btnBack">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox" ng-app="editModule" ng-controller="editCtrl">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtToolBarName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">分组：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtKeyType" class="" style="width: 200px;float:left;" /><select id="ddlGroup"></select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">上级：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlPreID"></select><% if (webAction == "edit")
                                                          {%><span style="color: red;">当前导航及子导航不能选择</span><%} %>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtToolBarDescription" style="width: 100%; height: 50px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">顺序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPlayIndex" value="0" class="" style="width: 100px;float:left;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                        从小到大排列
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">图标或图片：
                    </td>
                    <td width="*" align="left">
                        <div class="divIcon divIconImg" style="display: none;">
                        </div>
                        <div class="divImg divIconImg" style="display: block;">
                            <img alt="缩略图" src="/img/hb/hb3.jpg" id="imgfile_1" style="max-height: 80px; max-width: 80px;" class="imgUpload"  data-input="file_1"/>
                        </div>
                        <div>
                            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                id="aselectimage">选择图标</a>
                            <a href="javascript:;" class="easyui-linkbutton imgUpload" iconcls="icon-add2"
                                plain="true" data-input="file_1">上传缩略图片</a>
                            <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                            <input type="file" name="file1" id="file_1" class="file" style="display: none;" />
                            <input type="text" id="txticon" value="" style="width: 200px; display: none;" />
                            <input type="text" id="txtfile_1" value="" style="width: 400px; display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">是否显示：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsShow" id="rdoShow" checked="checked" value="1" /><label
                            for="rdoShow">显示</label>
                        <input type="radio" name="IsShow" id="rdoHide" value="0" /><label for="rdoHide">隐藏</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">链接：
                    </td>
                    <td width="*" align="left">
                        <div style="width: 300px;">
                            <zc-select-link s-type="vm.Stype" s-value="vm.Svalue" s-text="vm.Stext" s-link="vm.sLink" s-l-type="vm.sLType"></zc-select-link>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">选中背景图片：
                    </td>
                    <td width="*" align="left">
                        <table>
                            <tr>
                                <td style="width: 140px; height: 36px;">
                                    <img alt="选中背景图片" src="" id="imgfile_ActBgImage" style="height: 36px; width: 140px;max-height: 36px; max-width: 140px;" class="imgUpload" data-input="file_ActBgImage" />
                                </td>
                                <td>
                                    <a href="javascript:;" class="easyui-linkbutton imgUpload" iconcls="icon-add2" plain="true" data-input="file_ActBgImage">上传背景图片</a>
                                    <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />最佳显示效果大小为140*36。
                                    <input type="file" name="file1" id="file_ActBgImage" class="file" style="display: none;" />
                                    <input type="text" id="txtfile_ActBgImage" value="" style="width: 100px;display: none;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">选中背景色：
                    </td>
                    <td width="*" align="left">
                    <%--    <label for="colorActBgColor">
                            <input type="text" id="txtActBgColor" value="" readonly="readonly" class="color" />
                            <input type="text" id="colorActBgColor" value="" class="color" data-for="txtActBgColor" />
                        </label>--%>
                            <input type="text" id="txtActBgColor" value="" readonly="readonly" class="color" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">背景图片：
                    </td>
                    <td width="*" align="left">
                        <table>
                            <tr>
                                <td style="width: 105px; height: 40px;">
                                    <img alt="背景图片" src="" id="imgfile_BgImage" style="height: 36px; width: 140px;max-height: 36px; max-width: 140px;" class="imgUpload" data-input="file_BgImage" />
                                </td>
                                <td>
                                    <a href="javascript:;" class="easyui-linkbutton imgUpload" iconcls="icon-add2" plain="true" data-input="file_BgImage">上传背景图片</a>
                                    <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />最佳显示效果大小为140*36。
                                    <input type="file" name="file1" id="file_BgImage" class="file" style="display: none;" />
                                    <input type="text" id="txtfile_BgImage" value="" style="width: 100px;display: none;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">背景色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBgColor" value="" readonly="readonly" class="color" style="width: 140px;" />
                        <%--<input type="text" id="colorBgColor" value="" class="color" data-for="txtBgColor" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">选中字色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActColor" value="" readonly="readonly" class="color" style="width: 140px;" />
                        <%--<input type="text" id="colorActColor" value="" class="color" data-for="txtActColor" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">字色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtColor" value="" readonly="readonly" class="color" style="width: 140px;" />
                        <%--<input type="text" id="colorColor" value="" class="color" data-for="txtColor" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">图标颜色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtIcoColor" value="" readonly="readonly" class="color" style="width: 140px;" />
                        <%--<input type="text" id="colorIcoColor" value="" class="color" data-for="txtIcoColor" />--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">图标位置：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlIcoPosition">
                            <option value="0">文字上方</option>
                            <option value="1">文字左边</option>
                            <option value="2">文字右边</option>
                            <option value="3">文字下边</option>
                            <option value="4">仅显示图标</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">可见设置：
                    </td>
                    <td width="*" align="left">
                        <input id="rdoVisibleSet0" type="radio" name="rdoVisibleSet" checked="checked" class="positionTop2" value="0" /><label for="rdoVisibleSet0">所有人可见</label>
                        <input id="rdoVisibleSet1" type="radio" name="rdoVisibleSet" class="positionTop2" value="1" /><label for="rdoVisibleSet1">仅分销员可见</label>
                        <input id="rdoVisibleSet2" type="radio" name="rdoVisibleSet" class="positionTop2" value="2" /><label for="rdoVisibleSet2">指定角色可见</label>
                    </td>
                </tr>
                <tr class="trPermissionGroup">
                    <td style="width: 100px;" align="right" class="tdTitle">可见角色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPermissionGroup" value="" style="width: 300px;" /><span style="color: red;">（,分隔的角色ID）</span>
                    </td>
                </tr>
                <%if (userType == 1)
                  {%>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">是否基础导航：
                    </td>
                    <td width="*" align="left">
                        <input id="chkIsSystem" type="checkbox" class="positionTop2" /><label for="chkIsSystem">是基础导航</label>
                    </td>
                </tr>
                <%}%>
                   <tr style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">是否电脑端：
                    </td>
                    <td width="*" align="left">
                        <input id="cbIsPc" type="checkbox" class="positionTop2" /><label for="cbIsPc">电脑端</label>
                    </td>
                </tr>
                <%--<tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>


                    </td>
                </tr>--%>
            </table>
            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                             <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-calm">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>
                            </div>
            <br />
            <br />
        </div>
    </div>

    <div id="dlgInput" class="easyui-dialog" closed="true" title="" style="width: 350px; height: 400px; padding: 15px; line-height: 30px;">
        <ul id="ulIcon">
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/angularjs/1.3.15/angular.min.js"></script>
    <script type="text/javascript" src="/Plugins/angular/commServices/services.js"></script>
    <script type="text/javascript" src="/Plugins/angular/zcSelectLink/zcSelectLink.js?v=2017031301"></script>
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/layer.mobile/layer.m.js"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var baseDomain = '/';
        var currAction = '<%=webAction %>';
        var currId = '<%=model.AutoID %>';
        var groups =<% = groups%>;
        var key_type = '<% =this.Request["key_type"]%>';
        var okey_type = '<% =this.Request["key_type"]%>';
        var limitPres = <% = limitPres%>;
        var iconclasses = <% = iconclasses%>;
        var nPreID = 0;
        var is_system ='<% =this.Request["is_system"]%>';
        var isSystem = "<%=model.IsSystem %>";
        var webInfo;
        var editModule = angular.module("editModule", ['zcSelectLink','comm-services']);
        editModule.controller("editCtrl", ['$scope','commService',
        function ($scope,commService) {
            <%if (webAction == "edit") { %>
            $scope.vm = webInfo ={
                sLType:'<%=model.ToolBarType%>',
                sLink:'<%=model.ToolBarTypeValue%>',
                Stype:'<%=model.Stype%>',
                Svalue:'<%=model.Svalue%>',
                Stext:'<%=model.Stext%>'
            };
            <%} else { %>
            $scope.vm = webInfo ={
                sLType:'链接',
                sLink:'',
                Stype:'自定义',
                Svalue:'',
                Stext:''
            };
            <% } %>
        }]);
        $(function () {
            loadGroups();
            loadIcons();
            $("#ddlGroup").live("change",function(){
                $("#txtKeyType").val($(this).val());
                LoadToolBarPreSelect();
            });
            $("#txtKeyType").live("change", function () {
                LoadToolBarPreSelect();
            });
            $(".imgUpload").live("click", function () {
                var input_id = $(this).attr("data-input")
                $("#"+input_id).click();
            });
            $(".file").live('change', function () {
                var fid = $(this).attr("id");
                var fpath = $.trim($(this).val());
                var txtfid = "txt"+fid;
                var imgfid = "img"+fid;
                if (fpath == "") return;
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: fid,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $("#" + txtfid).val(resp.ExStr);
                                 $("#" + imgfid).attr("src", resp.ExStr);
                                 if(fid == "file_1"){
                                     $(".divIcon").html('');
                                     $(".divIcon").hide();
                                     $(".divImg").show();
                                 }
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
            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
            if (currAction == 'edit') {
                $('#txtToolBarName').val("<%=model.ToolBarName %>");
                $('#txtToolBarDescription').val("<%=model.ToolBarDescription %>");
                $('#txtPlayIndex').val("<%=model.PlayIndex %>");
                $('#txtKeyType').val("<%=model.KeyType %>");
                $('#ddlGroup').val("<%=model.KeyType %>");
                
                var nicon = "<%=model.ToolBarImage %>";

                $('#txtfile_1').val("<%=model.ImageUrl%>");
                if($('#txtfile_1').val() ==""){
                    $("#imgfile_1").attr("src","/img/hb/hb3.jpg");
                }
                else{
                    $("#imgfile_1").attr("src",$('#txtfile_1').val());
                }
                if(nicon == ""){
                    $(".divIcon").hide();
                    $(".divImg").show();
                }else{
                    setDivIcon(nicon);
                    $("#txticon").val(nicon);
                    $(".divIcon").show();
                    $(".divImg").hide();
                }

                var isshow = "<%=model.IsShow %>";
                if (isshow == "1") {
                    $("#rdoShow").attr("checked", "checked");
                }
                else {
                    $("#rdoHide").attr("checked", "checked");
                }
                $('#txtActBgColor').val("<%=model.ActBgColor %>");
                if("<%=model.ActBgColor %>"!=""){
                    $('#txtActBgColor').css('background-color',"<%=model.ActBgColor %>");
                }
                $('#colorActBgColor').val("<%=model.ActBgColor %>");
                $('#txtBgColor').val("<%=model.BgColor %>");
                if("<%=model.BgColor %>"!=""){
                    $('#txtBgColor').css('background-color',"<%=model.BgColor %>");
                }
                $('#colorBgColor').val("<%=model.BgColor %>");
                $('#txtActColor').val("<%=model.ActColor %>");
                if("<%=model.ActColor %>"!=""){
                    $('#txtActColor').css('background-color',"<%=model.ActColor %>");
                }
                $('#colorActColor').val("<%=model.ActColor %>");
                $('#txtColor').val("<%=model.Color %>");
                if("<%=model.Color %>"!=""){
                    $('#txtColor').css('background-color',"<%=model.Color %>");
                }
                $('#colorColor').val("<%=model.Color %>");
                $('#txtIcoColor').val("<%=model.IcoColor %>");
                if("<%=model.IcoColor %>"!=""){
                    $('#txtIcoColor').css('background-color',"<%=model.IcoColor %>");
                }
                $('#colorIcoColor').val("<%=model.IcoColor %>");

                $('#txtfile_ActBgImage').val("<%=model.ActBgImage%>");
                if($('#txtfile_ActBgImage').val() ==""){
                    $("#imgfile_ActBgImage").attr("src","");
                }
                else{
                    $("#imgfile_ActBgImage").attr("src",$('#txtfile_ActBgImage').val());
                }
                
                $('#txtfile_BgImage').val("<%=model.BgImage%>");
                if($('#txtfile_BgImage').val() ==""){
                    $("#imgfile_BgImage").attr("src","");
                }
                else{
                    $("#imgfile_BgImage").attr("src",$('#txtfile_BgImage').val());
                }
                $('#ddlIcoPosition').val("<%=model.IcoPosition %>");

                nPreID = <%=model.PreID %>;

                if(is_system == 0 && isSystem ==1){
                    currAction = "add";
                }
                
                $(':radio[name="rdoVisibleSet"][value="<%=model.VisibleSet %>"]').prop("checked", "checked");
                if('<%=model.VisibleSet %>'=='2'){
                    $('#txtPermissionGroup').val('<%=model.PermissionGroup %>');
                }else{
                    $('#txtPermissionGroup').val('');
                }
             <%if (userType == 1)
               {%>
                if (isSystem == 1 && is_system != 0) {
                    $("#chkIsSystem").attr("checked", true);
                }
                else {
                    $("#chkIsSystem").attr("checked", false);
                }
            <%}%>
                if (<%=model.IsPc%>==1) {
                    $("#cbIsPc").attr("checked", true);
                }
                else {
                    $("#cbIsPc").attr("checked", false);
                }
            }
            else{
                $("#txtKeyType").val(key_type);
                $("#ddlGroup").val(key_type);
                $(':radio[name="rdoVisibleSet"][value="0"]').prop("checked", "checked");
                $('#txtPermissionGroup').val('');
                //$('#txtActBgColor').val("#F8F8F7");
                //$('#colorActBgColor').val("#F8F8F7");
                //$('#txtBgColor').val("#F8F8F7");
                //$('#txtActColor').val("#E65E60");
                //$('#txtColor').val("#4D4D4D");
                //$('#txtIcoColor').val("#5E5E5E");
                $('#ddlIcoPosition').val("0");
                nPreID = 0;
                $("#imgfile_1").attr("src","/img/hb/hb3.jpg");
            }
            LoadToolBarPreSelect();
            
            //$('#colorActBgColor').spectrum({hide:setHideColor,move:setMoveColor,change:setChangeColor});
            //$('#colorBgColor').spectrum({hide:setHideColor,move:setMoveColor,change:setChangeColor});
            //$('#colorActColor').spectrum({hide:setHideColor,move:setMoveColor,change:setChangeColor});
            //$('#colorColor').spectrum({hide:setHideColor,move:setMoveColor,change:setChangeColor});
            //$('#colorIcoColor').spectrum({hide:setHideColor,move:setMoveColor,change:setChangeColor});
            //jscolor.init();

            $('#btnSave').click(function () {
                try {
                    var model =
                       {
                           Action: currAction == 'add' ? 'AddCompanyWebsiteToolBar' : 'EditCompanyWebsiteToolBar',
                           AutoID: currId,
                           ToolBarName: $.trim($('#txtToolBarName').val()),
                           ToolBarDescription: $.trim($('#txtToolBarDescription').val()),
                           PlayIndex: $.trim($('#txtPlayIndex').val()),
                           ToolBarImage: $.trim($("#txticon").val()),
                           IsShow: $(":radio[name=IsShow]:checked").val(),
                           KeyType: $.trim($('#txtKeyType').val()),
                           UseType: '<%=this.Request["use_type"]%>',
                           ToolBarType: webInfo.sLType,
                           ToolBarTypeValue: webInfo.sLink,
                           Stype: webInfo.Stype,
                           Svalue: webInfo.Svalue,
                           Stext: webInfo.Stext,
                           ActBgColor: $.trim($('#txtActBgColor').val()),
                           BgColor: $.trim($('#txtBgColor').val()),
                           ActColor: $.trim($('#txtActColor').val()),
                           Color: $.trim($('#txtColor').val()),
                           IcoColor: $.trim($('#txtIcoColor').val()),
                           PreID: $.trim($('#ddlPreID').val()),
                           VisibleSet:$.trim($(':radio[name="rdoVisibleSet"]:checked').val()),
                           PermissionGroup:$.trim($('#txtPermissionGroup').val()),
                        <%if (userType == 1)
                          {%>
                           IsSystem:$('#chkIsSystem').attr("checked") == "checked"?"1":"0",
                        <%}%>
                           ImageUrl: $.trim($('#txtfile_1').val()),
                           ActBgImage: $.trim($('#txtfile_ActBgImage').val()),
                           BgImage: $.trim($('#txtfile_BgImage').val()),
                           IcoPosition: $.trim($('#ddlIcoPosition').val()),
                           IsPc:'<%=this.Request["isPc"]%>'
                       }

                    if (model.ToolBarName == '') {
                        $('#txtToolBarName').focus();
                        return;
                    }
                    //if (model.ToolBarImage == '') {
                    //    Alert("请上传图片");
                    //    return;
                    //}
                    if (model.KeyType == '') {
                        Alert("请输入分组");
                        return;
                    }
                    if (model.PlayIndex == '') {
                        $('#txtPlayIndex').focus();
                        return;
                    }
                    if(is_system == 0 && isSystem ==1){
                        model.BaseID = model.AutoID;
                    }
                    if(model.ToolBarImage=="" && model.ImageUrl==""){
                        model.ImageUrl = "/img/hb/hb3.jpg";
                    }
                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                document.location.href = '/App/CompanyWebsite/ToolBarManage.aspx?use_type=<%=this.Request["use_type"] %>&is_system=<%= this.Request["is_system"]%>&isPc=<%= this.Request["isPc"]%>';

                                return;
                                if(is_system == 0 && isSystem ==1){
                                    document.location.href = '/App/CompanyWebsite/ToolBarManage.aspx?use_type=<%=this.Request["use_type"] %>&is_system=<%= this.Request["is_system"]%>';
                                    return;
                                }
                                if (currAction == 'add')
                                    ResetCurr();
                                nPreID = model.PreID;
                                LoadToolBarPreSelect();
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

            $("#aselectimage,#imgThumbnailsPath").click(function () {
                $('#dlgInput').dialog({ title: '选择图标' });
                $('#dlgInput').dialog('open');
            });

            $("[data-imageclass]").click(function () {
                var tclass = $(this).attr("data-ico");
                setDivIcon(tclass);
                $("#txticon").val(tclass);
                $('#txtfile_1').val("");
                $("#imgfile_1").attr("src","/img/hb/hb3.jpg");
                $(".divIcon").show();
                $(".divImg").hide();
                $('#dlgInput').dialog('close');
            });

            //$('#grvData').datagrid(
            //       {
            //           method: "Post",
            //           url: "/Handler/App/CationHandler.ashx",
            //           queryParams: { Action: "QueryArticleCategory" },
            //           height: 300,
            //           pagination: true,
            //           striped: true,
            //           pageSize: 10,
            //           rownumbers: true,
            //           singleSelect: true,
            //           rowStyler: function () { return 'height:25px'; },
            //           columns: [[
            //                       { field: 'CategoryName', title: '分类名称', width: 100, align: 'left' }

            //                    ]]
            //       }
            //   );
        });

        function LoadToolBarPreSelect(){
            var nkey_type = $.trim($('#txtKeyType').val());
            var model ={
                Action: 'QueryCompanyWebsiteToolBarPreSelect',
                use_type: '<%=this.Request["use_type"]%>',
                key_type: nkey_type
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: model,
                dataType: "json",
                success: function (resp) {
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<option value="{0}" {1}>&nbsp;</option>', '0',nPreID==0?'selected="selected"':'');
                    if(resp.length>0){
                        for (var i = 0; i < resp.length; i++) {
                            if(limitPres.indexOf(resp[i].id)<0){
                                appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', resp[i].id, resp[i].name,nPreID==resp[i].id?'selected="selected"':'');
                            }
                        }
                    }
                    $("#ddlPreID").html("");
                    $("#ddlPreID").append(appendhtml.ToString());
                }
            });
        }

        function ResetCurr() {
            var playindex = $("#txtPlayIndex").val();
            playindex++;
            var nkeytype = $("#txtKeyType").val();
            var nactbgcolor = $("#txtActBgColor").val();
            var nbgcolor = $("#txtBgColor").val();
            var nactcolor = $("#txtActColor").val();
            var ncolor = $("#txtColor").val();
            var nicocolor = $("#txtIcoColor").val();

            $(":input[type=text]").val("");
            $("textarea").val("");

            $("#txtPlayIndex").val(playindex);
            $("#txtKeyType").val(nkeytype);
            $("#txtActBgColor").val(nactbgcolor);
            $("#txtBgColor").val(nbgcolor);
            $("#txtActColor").val(nactcolor);
            $("#txtColor").val(ncolor);
            $("#txtIcoColor").val(nicocolor);
        }
        function loadGroups() {
            var appendhtml = new StringBuilder();
            var hasDefNav = false;
            for (var i = 0; i < groups.length; i++) {
                appendhtml.AppendFormat('<option value="{0}">{0}</option>', groups[i]);
                if(groups[i] == key_type) hasDefNav = true;
            }
            if(!hasDefNav)appendhtml.AppendFormat('<option value="{0}">{0}</option>',key_type);
            $("#ddlGroup").append(appendhtml.ToString());
        }
        function loadIcons() {
            var appendhtml = new StringBuilder();
            for (var i = 0; i < iconclasses.length; i++) {
                appendhtml.AppendFormat('<li class="liIco" data-imageclass="liIco" data-ico="{0}" onmouseover="this.style.background=\'#1b9af7\'" onmouseout="this.style.background=\'#ffffff\'">', iconclasses[i]);
                appendhtml.AppendFormat('<svg class="icon" aria-hidden="true">');
                appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', iconclasses[i]);
                appendhtml.AppendFormat('</svg>');
                appendhtml.AppendFormat('</li>');
            }
            $("#ulIcon").html("");
            $("#ulIcon").append(appendhtml.ToString());
        }
        function  setHideColor(color) {
            if(color){
                $('#'+$(this).attr('data-for')).val(color.toRgbString());
                $('#'+$(this).attr('data-for')).css('background-color',color.toRgbString());
            }
            else{
                $('#'+$(this).attr('data-for')).val('');
                $('#'+$(this).attr('data-for')).css('background-color','');
            }
        }
        function  setChangeColor(color) {
            $('#'+$(this).attr('data-for')).val(color.toRgbString());
            $('#'+$(this).attr('data-for')).css('background-color',color.toRgbString());
        }
        
        function  setMoveColor(color) {
            $('#'+$(this).attr('data-for')).val(color.toRgbString());
            $('#'+$(this).attr('data-for')).css('background-color',color.toRgbString());
        }
        function setDivIcon(ico){
            ico = ico.replace('iconfont ','');
            $('.divIcon').html('');
            var appendhtml = new StringBuilder();
            appendhtml.AppendFormat('<svg class="icon" aria-hidden="true">');
            appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', ico);
            appendhtml.AppendFormat('</svg>');
            $('.divIcon').append(appendhtml.ToString());
        }
    </script>

    <% = icoScript %>
</asp:Content>
