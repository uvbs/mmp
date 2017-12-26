<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivityConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ActivityConfig" %>

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
        input[type=text]
        {
            height: 30px;
        }
        .hide
        {
            display: none;
        }
        .select
        {
         height:30px;   
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>活动配置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr class="hide">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <input type="text" id="txtOrganizerName" value="主办方" placeholder="主办方" />
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTheOrganizersUrl" placeholder="主办方链接" style="width: 100%;" />
                    </td>
                </tr>
                <tr class="hide">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <input type="text" id="txtActivitiesName" placeholder="活动日历" />
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivitiesUrl" placeholder="活动日历链接" style="width: 100%;" />
                    </td>
                </tr>
                <tr class="hide">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <input type="text" id="txtMyRegistrationName" placeholder="我的报名" />
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMyRegistrationUrl" placeholder="我的报名链接" style="width: 100%;" />
                    </td>
                </tr>
                  <tr >
                    <td style="width: 100px;" align="left" class="tdTitle">
                       底部导航
                    </td>
                    <td width="*" align="left">
                        <select id="ddlToolBarGroup">
                        <option value="">默认</option>
                        <%foreach (var item in Groups)
                          {
                              if (item==activityConfig.ToolBarGroups)
                              {
                                  Response.Write(string.Format("<option value=\"{0}\" selected=\"select\">{0}</option>",item));
                              }
                              else
                              {
                                  Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item));
                              }
                              
                          } %>
                        
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="left" class="tdTitle">
                        主题颜色：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdostyle" id="rdostyle0" checked="checked" value="0" /><label
                            for="rdostyle0">经典蓝</label>
                        <input type="radio" name="rdostyle" id="rdostyle1" value="1" /><label for="rdostyle1">清爽绿</label>
                        <input type="radio" name="rdostyle" id="rdostyle2" value="2" /><label for="rdostyle2">日光橙</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="left" class="tdTitle">
                        二维码类型：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdotype" id="rdotype0" checked="checked" value="0" /><label
                            for="rdotype0">文本</label>
                        <input type="radio" name="rdotype" id="rdotype1" value="1" /><label for="rdotype1">链接</label>
                    </td>
                </tr>
                <tr class="hide">
                    <td style="width: 100px;" align="left" class="tdTitle">
                        停止的活动：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdoactivty" id="rdoactivty0" value="0" /><label for="rdoactivty0">显示</label>
                        <input type="radio" name="rdoactivty" id="rdoactivty1" value="1" /><label for="rdoactivty1">不显示</label>
                    </td>
                </tr>
                <tr id="trshowfield">
                    <td style="width: 100px;" align="left" class="tdTitle">
                        文本二维码显示字段：
                    </td>
                    <td width="*" align="left">
                        <input type="checkbox" id="chk0" name="ck" checked="checked" value="0" /><label for="chk0">签到码</label>
                        <input type="checkbox" id="chk1" name="ck" value="1" /><label for="chk1">姓名</label>
                        <input type="checkbox" id="chk2" name="ck" value="2" /><label for="chk2">手机</label>
                        <input type="checkbox" id="chk3" name="ck" value="3" /><label for="chk3">活动编号</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="left" class="tdTitle">
                        标题显示名称:
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShowName" placeholder="标题显示名称" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="left" class="tdTitle">
                        入场券显示名称:
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTicketShowName" placeholder="入场券显示名称" style="width: 100%;" />
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">
                            保存</a> 
                            
                            <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;display:none;"
                                class="button button-rounded button-flat">重置</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXShowInfoHandler.ashx";
        $(function () {
            ShowEdit();

            $('#btnSave').click(function () {
                var RegisterCode = [];
                try {
                    $('input:checkbox:checked').each(function () {

                        RegisterCode.push($(this).val());
                    });
                    var model =
                    {
                        Action: 'SavaActivityConfig',
                        TheOrganizers: $("#txtTheOrganizersUrl").val(),
                        Activities: $("#txtActivitiesUrl").val(),
                        MyRegistration: $("#txtMyRegistrationUrl").val(),
                        RegisterCode: RegisterCode.join(','),
                        OrganizerName: $("#txtOrganizerName").val(),
                        ActivitiesName: $("#txtActivitiesName").val(),
                        MyRegistrationName: $("#txtMyRegistrationName").val(),
                        QCodeType: $("input[name='rdotype']:checked").val(),
                        ColorTheme: $("input[name='rdostyle']:checked").val(),
                        IsShowHideActivity: $("input[name='rdoactivty']:checked").val(),
                        ShowName: $("#txtShowName").val(),
                        TicketShowName: $("#txtTicketShowName").val(),
                        ToolBarGroups: $("#ddlToolBarGroup").val()
                    };
                    if (!model.IsShowHideActivity) model.IsShowHideActivity = 0;
                    //if (model.TheOrganizers != "") {
                    //    if (model.TheOrganizers.match(/http:\/\/.+/) == null) {
                    //        Alert("链接不正确");
                    //        return false;
                    //    }

                    //}
                    //if (model.Activities != "") {
                    //    if (model.Activities.match(/http:\/\/.+/) == null) {
                    //        Alert("链接不正确");
                    //        return false;
                    //    }

                    //}
                    //if (model.MyRegistration != "") {
                    //    if (model.MyRegistration.match(/http:\/\/.+/) == null) {
                    //        Alert("链接不正确");
                    //        return false;
                    //    }

                    //}

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


            function ShowEdit() {
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: { Action: 'GetActivityConfig' },
                    dataType: "json",
                    success: function (resp) {
                        try {
                            if (resp.Status == 1) {
                                $("#txtTheOrganizersUrl").val(resp.ExObj.TheOrganizers);
                                if (resp.ExObj.Activities != "") {
                                    $("#txtActivitiesUrl").val(resp.ExObj.Activities);
                                }
                                else {
                                    $("#txtActivitiesUrl").val("http://" + window.location.host + "/App/Cation/Wap/ActivityLlists.aspx");
                                }
                                if (resp.ExObj.MyRegistration != "") {
                                    $("#txtMyRegistrationUrl").val(resp.ExObj.MyRegistration);
                                }
                                else {
                                    $("#txtMyRegistrationUrl").val("http://" + window.location.host + "/App/Cation/Wap/MyActivityLlists.aspx");

                                }
                                //
                                $("#txtOrganizerName").val(resp.ExObj.OrganizerName);
                                $("#txtActivitiesName").val(resp.ExObj.ActivitiesName);
                                $("#txtMyRegistrationName").val(resp.ExObj.MyRegistrationName);
                                $("#txtShowName").val(resp.ExObj.ShowName);
                                $("#txtTicketShowName").val(resp.ExObj.TicketShowName);
                                if (resp.ExObj.QCodeType == 1) {
                                    rdotype1.checked = true;
                                    $("#trshowfield").hide();
                                }
                                else {

                                }
                                if (resp.ExObj.ColorTheme == 1) {
                                    rdostyle1.checked = true;

                                }
                                else {
                                    rdostyle0.checked = true;
                                }
                                if (resp.ExObj.IsShowHideActivity == 0) {
                                    rdoactivty0.checked = true;
                                } else {
                                    rdoactivty1.checked = true;
                                }

                                var strs = new Array(); //定义一数组 
                                if (resp.ExObj.RegisterCode != null) {
                                    strs = resp.ExObj.RegisterCode.split(","); //字符分割 
                                }

                                for (var i = 0; i < strs.length; i++) {
                                    if (strs[i] != null) {
                                        switch (strs[i]) {
                                            case "0":
                                                $("#chk0").attr("checked", "checked")
                                                break;
                                            case "1":
                                                $("#chk1").attr("checked", "checked")
                                                break;
                                            case "2":
                                                $("#chk2").attr("checked", "checked")
                                                break;
                                            case "3":
                                                $("#chk3").attr("checked", "checked")
                                                break;

                                        };
                                    };
                                }
                            }
                            else {
                                $("#txtActivitiesUrl").val("http://" + window.location.host + "/App/Cation/Wap/ActivityLlists.aspx");
                                $("#txtMyRegistrationUrl").val("http://" + window.location.host + "/App/Cation/Wap/MyActivityLlists.aspx");

                            }
                        } catch (e) {
                            Alert(e);
                        }
                    }
                });
            }


            $("#btnReset").click(function () {

                $("input[type='text']").val("");



            })

            $("input[name='rdotype']").click(function () {

                if ($(this).val() == "0") {
                    $("#trshowfield").show();
                }
                else {
                    $("#trshowfield").hide();
                }



            })
        })

    </script>
</asp:Content>
