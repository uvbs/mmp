<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXTempmsgEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.WXTempmsg.WXTempmsgEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:<strong><%= Request["id"]=="0"?"添加":"编辑" %>模板</strong>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 600px;">
            <table width="100%;">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        模板名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtDataValue" value="" style="width: 450px;" placeholder="模板名称(必填)"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        微信模板Id：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtDataKey" value="" style="width: 450px;" placeholder="微信模板Id(必填)"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtOrderBy" value="1" style="width: 450px;" placeholder="排序"/>
                    </td>
                </tr>
            </table>
            <strong style="font-size:20px;">字段列表:</strong><span style="color:red;">字段必须填满，且对应字段不能相同</span>
            <div class="field">
                <table>
                <tr data-field-index="1"><td style="width: 100px;" align="right" class="tdTitle">K1:</td><td><input type="text" value="" id="txtK1" style="width: 450px;" placeholder="K1对应微信模板的字段" /></td><td></td></tr>
                <tr data-field-index="2"><td style="width: 100px;" align="right" class="tdTitle">K2:</td><td><input type="text" value="" id="txtK2" style="width: 450px;" placeholder="K2对应微信模板的字段" /></td><td></td></tr>
                <tr data-field-index="3"><td style="width: 100px;" align="right" class="tdTitle">K3:</td><td><input type="text" value="" id="txtK3" style="width: 450px;" placeholder="K3对应微信模板的字段" /></td><td></td></tr>
                <tr data-field-index="4"><td style="width: 100px;" align="right" class="tdTitle">K4:</td><td><input type="text" value="" id="txtK4" style="width: 450px;" placeholder="K4对应微信模板的字段" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除" class="deletefield"/></td></tr>
                <tr><td></td><td><a class="button button-rounded button-primary addfield">添加字段</a></td><td></td></tr>
                </table>
            </div>
            <div style="text-align:center;">
            <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width:200px;">提交</a> 
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var id = '<%= Request["id"] %>'; //问题数量
        $(function () {

            //编辑时加载原数据
            if (id > 0) {
                //console.log(1);
                //$.messager.progress({ text: '正在加载...' });
                $.ajax({
                    type: 'post',
                    url: "Handler/WXTempmsgHandler.ashx",
                    data: { Action: "GetWXTempmsg", AutoId: id },
                    dataType: "json",
                    success: function (resp) {
                        //$.messager.progress('close');
                        if (resp.isSuccess) {
                            $("#txtDataKey").val(resp.returnObj.DataKey);
                            $("#txtDataValue").val(resp.returnObj.DataValue);
                            $("#txtOrderBy").val(resp.returnObj.OrderBy);

                            if (resp.returnObj.KeyFields.length < 4) {
                                //移除多余字段
                                for (var i = 4; i > resp.returnObj.KeyFields.length; i--) {
                                    $("[data-field-index='" + i + "']").remove();
                                }
                                //加删除按钮到最后的字段
                                $($("[data-field-index='" + resp.returnObj.KeyFields.length + "']").find("td")[2]).html('<img src="/img/delete.png" width="20" height="20" alt="删除" class="deletefield"/>');
                            }
                            else if (resp.returnObj.KeyFields.length > 4) {
                                //移除所有删除按钮
                                $(".field").find(".deletefield").remove();
                                //添加字段
                                for (var i = 5; i <= resp.returnObj.KeyFields.length; i++) {
                                    var appendhtml = new StringBuilder();
                                    appendhtml.AppendFormat('<tr data-field-index="{0}"><td style="width: 100px;" align="right" class="tdTitle">K{0}:</td><td><input type="text" value="" id="txtK{0}" style="width: 450px;" placeholder="K{0}对应微信模板的字段" /></td><td></td></tr>', i);
                                    $(".addfield").closest("tr").before(appendhtml.ToString());
                                }
                                //加删除按钮到最后的字段
                                $($("[data-field-index='" + resp.returnObj.KeyFields.length + "']").find("td")[2]).html('<img src="/img/delete.png" width="20" height="20" alt="删除" class="deletefield"/>');
                            }
                            for (var i = 1; i <= resp.returnObj.KeyFields.length; i++) {
                                $("#txt" + resp.returnObj.KeyFields[i - 1].DataKey).val(resp.returnObj.KeyFields[i - 1].DataValue);
                            }
                        }
                        else {
                            Alert(resp.errmsg);
                        }
                    }
                });
            }
            //编辑时加载原数据

            //删除字段
            $('.deletefield').live("click", function () {
                var ofieldcount = $(this).closest("tr").attr("data-field-index");
                if (Number(ofieldcount) < 3) {
                    Alert("最少需要2个字段");
                    return;
                }
                $($(this).closest("tr").prev().find("td")[2]).html('<img src="/img/delete.png" width="20" height="20" alt="删除" class="deletefield"/>');
                $(this).closest("tr").remove();
            });
            //删除字段

            //添加字段
            $(".addfield").live("click", function () {
                var ofieldcount = $(this).closest("tr").prev().attr("data-field-index");
                var nfieldcount = Number(ofieldcount) + 1;
                if (nfieldcount > 10) {
                    Alert("最多支持10个字段");
                    return;
                }
                var appendhtml = new StringBuilder();
                $(this).closest(".field").find(".deletefield").remove();
                appendhtml.AppendFormat('<tr data-field-index="{0}"><td style="width: 100px;" align="right" class="tdTitle">K{0}:</td><td><input type="text" value="" id="txtK{0}" style="width: 450px;" placeholder="K{0}对应微信模板的字段" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除" class="deletefield"/></td></tr>', nfieldcount);
                $(this).closest("tr").before(appendhtml.ToString());
            });
            //添加字段

            //提交
            $('#btnSave').click(function () {
                try {
                    var dv = $.trim($("#txtDataValue").val());
                    if ($.trim($("#txtDataValue").val()) == "") {
                        Alert("模板名称");
                        $("#txtDataValue").focus();
                        return false;
                    }
                    var dk = $.trim($("#txtDataKey").val());
                    if ($.trim($("#txtDataKey").val()) == "") {
                        Alert("请输入微信模板Id");
                        $("#txtDataKey").focus();
                        return false;
                    }
                    var ob = $.trim($("#txtOrderBy").val());
                    if (isNaN(ob)) {
                        $("#txtOrderBy").focus();
                        Alert("排序请输入数字");
                        return false;
                    }
                    //检查
                    var KeyVauleData = {
                        Action: "PostWXTempmsg",
                        AutoId: id,
                        DataValue: dv,
                        DataKey: dk,
                        OrderBy: ob,
                        KeyFields: ""
                    }; //问题模型
                    var tks = $("input[id^='txtK']");
                    var fields = [];
                    var fieldNs = [];
                    for (var i = 0; i < tks.length; i++) {
                        var KFieldName = $.trim($(tks[i]).val());
                        if (KFieldName == "") {
                            $($(tks[i])).focus();
                            //Alert("请输入对应字段");
                            return false;
                        }
                        else {
                            var kindex = $($(tks[i])).closest("tr").attr("data-field-index");
                            if (fieldNs.indexOf(KFieldName) >= 0) {
                                $($(tks[i])).focus();
                                //Alert("已经存在相同的对应字段");
                                return false;
                            }
                            fieldNs.push(KFieldName);
                            fields.push({ DataKey: "K" + kindex, DataValue: KFieldName, OrderBy: kindex });
                        }
                    }
                    KeyVauleData.KeyFields = JSON.stringify(fields);

                    $.messager.progress({ text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: "Handler/WXTempmsgHandler.ashx",
                        data: KeyVauleData,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.isSuccess) {
                                Alert("提交完成");
                                window.location.href = "WXTempmsgList.aspx";
                            }
                            else {
                                Alert(resp.errmsg);
                            }
                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });
            //提交

        });
    </script>
</asp:Content>
