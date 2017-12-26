<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="CrowdFundInfoAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.Admin.CrowdFundInfoAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
        input[type=text], select, textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        select{min-width:200px;}
        input[type=text]{width:90%;}
        #txtFinancAmount{width:100px;color:red;}
        #btnSave{font-weight: bold; width: 400px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="CrowdFundInfoMgr.aspx">众筹管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%=model.Title%></span>
    <a href="CrowdFundInfoMgr.aspx" style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" class="" value="<%=model.Title%>" placeholder="必填" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="<%=model.CoverImage %>" width="200px" id="imgCoverImage" /><br />
                        <a  href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtCoverImage.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtCoverImage" name="file1" style="display:none;"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        筹集金额：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtFinancAmount"  value="<%=model.FinancAmount%>" placeholder="必填" onkeyup="value=value.replace(/[^\d]/g,'')"/>元
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        单位价格：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUnitPrice"  value="<%=model.UnitPrice==0?1:model.UnitPrice%>" placeholder="单位价格" onkeyup="value=value.replace(/[^\d]/g,'')" style="width:100px;"/>元
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       截止日期：
                    </td>
                    <td width="*" align="left">
                        <input id="txtStopTime" type="text" style="width: 150px;" value=""class="easyui-datetimebox" />必选
                            
                    </td>
                </tr>
                 
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        付款时姓名必填：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlNameRequired">
                            <option value="1">是</option>
                           <%-- <option value="0">否</option>--%>
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        付款时手机必填：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlPhoneRequired">
                            <option value="1">是</option>
                           <%-- <option value="0">否</option>--%>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        付款时显示公司名称填写：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlCompanyRequired">
                            <option value="0">否</option>
                            <option value="1">是</option>
                           
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        付款时显示职位填写：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlPositionRequired">
                             <option value="0">否</option>
                            <option value="1">是</option>
                           
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        已筹集金额 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtHaveFinancAmountText" value="<%=model.HaveFinancAmountText%>" placeholder="选填"/>
                    </td>
                </tr>
                                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        总筹集金额 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtFinancAmountText"  value="<%=model.FinancAmountText%>" placeholder="选填"/>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        参与人员 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtJoinPersonnelText" value="<%=model.JoinPersonnelText%>" placeholder="选填"/>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        我要付款 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPaymentText"  value="<%=model.PaymentText%>" placeholder="选填"/>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        我要发起众筹 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddCrowdFundText"  value="<%=model.AddCrowdFundText%>" placeholder="选填"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        分享 显示名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareText"  value="<%=model.ShareText%>" placeholder="选填"/>
                    </td>
                </tr>
                <tr >
                    <td style="width: 100px;" align="right" class="tdTitle">
                        介绍内容：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtIntroduction" style="width: 100%; height: 400px;"><%=model.Introduction%></textarea>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        状态：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlStatus">
                        <option value="1">进行中</option>
                        <option value="0">已停止</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave"  class="button button-rounded button-primary">
                            保存</a> 
                            
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "AdminHandler.ashx";
        var currAction = '<%=currAction %>';
        var currId = '<%=model.AutoID %>';
        var editor;
        $(function () {
            if (currAction == 'edit') {
                $('#txtStopTime').datetimebox('setValue', '<%=model.StopTime.ToString("yyyy-MM-dd HH:mm")%>');
                $('#ddlNameRequired').val("<%=model.NameRequired%>");
                $('#ddlPhoneRequired').val("<%=model.PhoneRequired%>");
                $('#ddlCompanyRequired').val("<%=model.CompanyRequired%>");
                $('#ddlPositionRequired').val("<%=model.PositionRequired%>");
                $('#ddlStatus').val("<%=model.Status%>");

            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        Action: currAction == 'add' ? 'AddCrowdFundInfo' : 'EditCrowdFundInfo',
                        AutoID: currId,
                        Title: $.trim($('#txtTitle').val()),
                        CoverImage: $('#imgCoverImage').attr('src'),
                        FinancAmount: $("#txtFinancAmount").val(),
                        StopTime: $('#txtStopTime').datetimebox('getValue'),
                        Introduction: editor.html(),
                        NameRequired: $('#ddlNameRequired').val(),
                        PhoneRequired: $('#ddlPhoneRequired').val(),
                        CompanyRequired: $('#ddlCompanyRequired').val(),
                        PositionRequired: $('#ddlPositionRequired').val(),
                        HaveFinancAmountText: $('#txtHaveFinancAmountText').val(),
                        FinancAmountText: $('#txtFinancAmountText').val(),
                        JoinPersonnelText: $('#txtJoinPersonnelText').val(),
                        PaymentText: $('#txtPaymentText').val(),
                        AddCrowdFundText: $('#txtAddCrowdFundText').val(),
                        ShareText: $('#txtShareText').val(),
                        PV:<%=model.PV %>,
                        ShareCount:<%=model.ShareCount %>,
                        UnitPrice:$('#txtUnitPrice').val(),
                        Status: $('#ddlStatus').val()

                    }

                    if (model.Title == '') {
                        $('#txtTitle').focus();

                        return;
                    }
                    if (model.FinancAmount == '') {
                        $('#txtFinancAmount').focus();

                        return;
                    }
                    if (parseInt(model.FinancAmount) <= 0) {

                        Alert("筹集金额需要大于0");
                        return;
                    }
                    if (model.StopTime == '') {

                        Alert("请选择截止日期");
                        return;
                    }

                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                Alert(resp.Msg);
                                if (currAction == 'add') {
                                    
                                    window.location.href = 'CrowdFundInfoMgr.aspx';
                                }
                                
                            }
                            else {
                                
                            }
                        },
                        complete: function () {
                            $.messager.progress('close');


                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });

            $("#txtCoverImage").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CrowdFund',
                         secureuri: false,
                         fileElementId: 'txtCoverImage',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#imgCoverImage').attr('src', resp.ExStr);
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

        KindEditor.ready(function (K) {
            editor = K.create('#txtIntroduction', {
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
