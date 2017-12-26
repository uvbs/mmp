<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master"
    AutoEventWireup="true" CodeBehind="UserManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.UserManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .wrapWXHead{
            position: relative;
        }
        .wrapWXHead img{
            
        }
        .weixinFollowerTip{
            position: absolute;
            right: 0;
            bottom: 0px;
            border: 1px solid #f75b00;
            color: #f37329;
            border-radius: 50px;
            font-size: 12px;
            padding: 0px;
            width: 16px;
            height: 16px;
            font-weight: bolder;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：<span>会员管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
<% StringBuilder sbHtml = new StringBuilder(); %>
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <%
                if (websiteOwner == "songhe")
                {
            %>

           <%-- <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="SetEmployee();">设置员工</a>--%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                            onclick="ShowEditAddScore();">批量修改<%=moduleName %></a>


            <%}
                else
                { %>

            <%
                if (isHideAddBtn == 0)
                {
                    %>
                        
                    <%
                        }
            %>
            <%if (websiteOwner=="meifan"){%>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddUser();" id="btnAdd">添加</a>
                  
            <%} %>
            

            <%
                    if (isHideEditBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("updateinfo")))
                {
                    %>
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEdit();" id="btnEdit">编辑</a>
                    <%
                        }
            %>
            
            <%--<a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="SetTutor();" id="Btn">设置导师</a>--%>
             <%
                    if (isHideTagBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("updatetag")))
                 {
                    %>
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEditTag();" id="BtnTag">设置标签</a>
                    <%
                        }
            %>
            
           
            <%-- <%if (new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModel().IsDistributionMall == 1)
              {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowEditDistributionOwner();" id="A1">设置分销上级</a>
            <%}%>--%>

            <%if (IsShowUserType)
                {%>
            <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="SetAgent();">设置为经销商</a>
            <%} %>
            <%
                if (IsShowScore)
                {
            %>

            <%if (websiteInfo.MemberMgrBtn.Contains("updatescore"))
              {%>
                   <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                            onclick="ShowEditAddScore();">批量修改<%=moduleName %></a>

              <%} %>
           

             

            <%
                }
            %>

             <%
                    if (isHideScoreClearBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("clearscore")))
                 {
                    %>
                       <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                            onclick="BatchZero();">批量<%=moduleName %>清零</a>
                    <%
                        }
            %>
             <%
                    if (isHideMemberLevelBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("updatememberlevel")))
                 {
                    %>
                        <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="ShowSetAccessLevel();">设置会员等级</a>
                    <%
                        }
            %>
           

            <%
                    if (IsShowAccountAmount && isHideAccountAmount == 0 && (websiteInfo.MemberMgrBtn.Contains("updateaccountamount")))
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowAddAccountAmount();">修改余额</a>
            <%
                }
            %>

             <%
                    if (isHideOrderInfoBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("synmemberinfo")))
                 {
                    %>
                      <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="SynMemberInfo();">从订单,活动报名数据中同步会员信息</a>
                    <%
                        }
            %>

              <%
                    if (isHideChannelBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("updatechannel")))
                  {
                    %>
                        <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="SetChannel();">设置为渠道</a>
                    <%
                        }
            %>
           


            <%
                if (IsShowSendTempMessage && isHideWxNewsBtn == 0)
                {
            %>

            <%if (websiteInfo.MemberMgrBtn.Contains("sendweixinmsg")){%>
              
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-send" plain="true"
                onclick="ShowSendTemplateMsg();">发送微信消息</a> 
                  
             <% } %>

            <%if (websiteInfo.MemberMgrBtn.Contains("sendweixinmsgbytag")){%>
              
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-send" plain="true" onclick="ShowSendTemplateMsgByTag();">发送微信消息[通过用户标签]</a>
            <%
                }
            %>
            <%
                }
            %>


             <%
                    if (isHideDataClrar == 0 && (websiteInfo.MemberMgrBtn.Contains("cleardata")))
                 {
                    %>
                         <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                onclick="Clean();">数据清洗</a> 
                    <%
                        }
            %>
          <%
              if (isHideDisableBtn == 1)
              {
                  %>
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                        onclick="DisableUser(1);">批量禁用</a> 
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                                onclick="DisableUser(0);">批量启用</a> 
                   <%
                       }
          %>

           <%
               if (isHideEditPwdBtn == 1)
               {
                    %>
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-reload" plain="true"
                        onclick="EditPassword();">修改密码</a> 
                 
                   <%
                       }
           %>
                

            <%} %>

            <br />
            关键字:
            <input type="text" id="txtKeyWord" style="width: 500px;    display: inline-block;padding: 6px;" class="" placeholder="请输入关键字，可以在昵称，姓名，进行模糊匹配" />
           <%
               if (isHideTagBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("tagfilter")))
               {
                   %>
            标签名:
            <input type="text" id="txtTagName" readonly="readonly" onclick="ShowTagName();" style="width: 120px; position: inherit; display: inline-block;padding: 6px;" class="" />
                    <%
               }     
           %>
            

            <%
                if (isHidefilterBtn == 0 && (websiteInfo.MemberMgrBtn.Contains("advancefilter")))
                {
                    %>
                         <a href="javascript:;" class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list"
                plain="true" id="shaixuan"><span class="l-btn-left" style="margin-top: -4px;">高级筛选</span></a>
                    <%
                }     
            %>
             
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="<% = formField.Where(p => p.IsShowInList == 1 && !limitForeach.Contains(p.Field)).Count() < 8?"true":"false" %>">
    </table>
    <% List<ZentCloud.BLLJIMP.Model.TableFieldMapping> tempFormFieldList = formField.Where(p => !limitForeach.Contains(p.Field)).ToList(); %>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: <%= tempFormFieldList.Count>14?"800px":"500px" %>; padding: 15px;">
        <table width="100%">
            <% 
            int colNum = tempFormFieldList.Count > 14 ? 2 : 1;
            int colSpanWXHeadimgurl = tempFormFieldList.Count > 14 ? 3: 1;
            %>
            <tr>
                <td align="right">微信头像:</td>
                <td colspan="<%=colSpanWXHeadimgurl %>"><img id="imgEditWXHeadimgurl" style="width:50px;height:50px;" /></td>
            </tr>
            <% 
            int nCol = 0;
            sbHtml = new StringBuilder();
            foreach (ZentCloud.BLLJIMP.Model.TableFieldMapping item in tempFormFieldList)
            {
                nCol++;
                if ((nCol + 1) % colNum ==0) sbHtml.AppendFormat("<tr>");
                sbHtml.AppendLine(string.Format("<td align=\"right\">{0}{1}:</td><td>", item.MappingName, (item.FieldIsNull == 1 && item.IsReadOnly != 1) ? "<span style=\"color:red;\">*<span>" : ""));
                if (item.FieldType == "sex")
                {
                    sbHtml.AppendLine(string.Format("<select id=\"ddlEdit{0}\" {1}>", item.Field, item.IsReadOnly == 1 ? "disabled=\"disabled\"" : ""));
                    sbHtml.AppendLine(string.Format("<option value=\"1\">男</option>"));
                    sbHtml.AppendLine(string.Format("<option value=\"0\">女</option>"));
                    sbHtml.AppendLine(string.Format("</select>"));
                }
                else if (item.FieldType == "number")
                {
                    sbHtml.AppendLine(string.Format("<input id=\"txtEdit{0}\" type=\"number\" style=\"width: 250px;\" {1} />", item.Field, item.IsReadOnly == 1 ? "readonly=\"readonly\"" : ""));
                }
                else if (item.FieldType == "date")
                {
                    sbHtml.AppendLine(string.Format("<input id=\"txtEdit{0}\" type=\"date\" style=\"width: 250px;\" {1} />", item.Field, item.IsReadOnly == 1 ? "readonly=\"readonly\"" : ""));
                }
                else if (item.FieldType == "img")
                {
                    sbHtml.AppendLine(string.Format("<img id=\"imgEdit{0}\" style=\"width:50px;height:50px;\" class=\"{1}\" />", item.Field, item.IsReadOnly != 1 ? "imgEdit" : ""));
                    if (item.IsReadOnly != 1)
                    {
                        sbHtml.AppendLine(string.Format("<input id=\"fileEdit{0}\" class=\"fileTxtEdit\" type=\"file\" name=\"file1\" style=\"width: 250px;display:none;\" {1} />", item.Field, item.IsReadOnly == 1 ? "readonly=\"readonly\"" : "")); 
                        sbHtml.AppendLine(string.Format("<br /><input id=\"txtEdit{0}\" class=\"imgTxtEdit\" type=\"text\" style=\"width: 250px;\" {1} />", item.Field, item.IsReadOnly == 1 ? "readonly=\"readonly\"" : "")); 
                    }
                }
                else
                {
                    sbHtml.AppendLine(string.Format("<input id=\"txtEdit{0}\" type=\"text\" style=\"width: 250px;\" {1} {2} />", 
                        item.Field, 
                        item.FormatValiFunc == "number"?"onkeyup=\"this.value=this.value.replace(/\\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\\D/g,'')\"":"", 
                        item.IsReadOnly == 1 ? "readonly=\"readonly\"" : ""));
                }
                sbHtml.AppendLine(string.Format("</td>"));
                if ((nCol + 1) % colNum != 0) sbHtml.AppendLine(string.Format("</tr>"));
            }
            if ((nCol + 1) % colNum == 0) sbHtml.AppendLine(string.Format("<td align=\"right\"></td><td></td></tr>"));
            this.Response.Write(sbHtml.ToString());
            %>
        </table>
    </div>
        <div id="dlgAddUser" class="easyui-dialog" closed="true" title="添加用户" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>姓名:
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>手机号:
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgDistribution" class="easyui-dialog" closed="true" title="设置上级" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>上级用户名:
                </td>
                <td>
                    <input id="txtDistributionOwner" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgTag" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <div style="margin-top: 2px;">
            <input id="rdotags1" type="radio" name="tags" checked="checked" value="add" /><label
                for="rdotags1" style="font-size: 14px;"><b>增加标签</b></label>
            <input id="rdotags2" type="radio" name="tags" value="delete" /><label for="rdotags2"
                style="font-size: 14px;"><b>减少标签</b></label>
            <input id="rdotags3" type="radio" name="tags" value="update" /><label for="rdotags3"
                style="font-size: 14px;"><b>覆盖标签</b></label>
        </div>
        <br />
        <table id="grvTagData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgTagName" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvTagNameData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgAddScore" class="easyui-dialog" closed="true" title="修改<%=moduleName %>" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    <%=moduleName %>:
                </td>
                <td>
                    <input id="txtAddScore" type="text" style="width: 180px;" maxlength="6" /><span>正数则增加,负数则减少</span>
                </td>
            </tr>
            <tr>
                <td>
                    说明:
                </td>
                <td>
                    <textarea style="width: 280px;" rows="8" id="txtDesc"></textarea>
                </td>
            </tr>
        </table>
    </div>

    <div id="dlgAccessLevel" class="easyui-dialog" closed="true" title="设置用户权限访问级别" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>会员等级:
                </td>
                <td>
                    <input id="txtAccessLevel" type="text" style="width: 200px;" onkeyup="value=value.replace(/[^\d]/g,'') "
                        placeholder="访问权限级别为数字" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSendTemplateMsg" class="easyui-dialog" closed="true" title="发送消息模板" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr <%=websiteOwner=="meifan"? "style=\"display:none\"":"" %>>
                <td>类型:
                </td>
                <td>
                    <select id="ddlTemplateType">
                        <option value="notify">通知</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>标题:
                </td>
                <td>
                    <input id="txtTitle" type="text" style="width: 300px;" placeholder="最多输入500个字" />
                </td>
            </tr>
            <tr>
                <td>内容:
                </td>
                <td>
                    <textarea id="txtContent" style="width: 300px;" placeholder="最多输入1000个字"></textarea>
                </td>
            </tr>
            <tr>
                <td>链接:
                </td>
                <td>
                    <input id="txtLink" type="text" style="width: 300px;" placeholder="最多输入500个字符" />
                </td>
            </tr>
            <tr <%=websiteOwner=="meifan"? "style=\"display:none\"":"" %>>
                <td>发送到:
                </td>
                <td>
                    <input id="rdoSendWeixin" class="positionTop2" type="radio" name="rdoSend" checked="checked" value="0" />
                    <label for="rdoSendWeixin">微信</label>

                    
                    <input id="rdoSendApp" class="positionTop2" type="radio" name="rdoSend" value="1" />
                    <label for="rdoSendApp">App</label>
                    <input id="rdoSendAppAndWx" class="positionTop2" type="radio" name="rdoSend" value="2" />
                    <label for="rdoSendAppAndWx">App和微信</label>
                   
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgSendTemplateMsgByTag" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table id="grvTagData2" fitcolumns="true">
        </table>
        <hr />
        <table width="100%;padding:20px;" >
            <tr <%=websiteOwner=="meifan"? "style=\"display:none\"":"" %>>
                <td>类型:
                </td>
                <td>
                    <select id="ddlTemplateType1">
                        <option value="notify">通知</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>标题:
                </td>
                <td>
                    <input id="txtTitle1" type="text" style="width: 300px;" placeholder="最多输入500个字" />
                </td>
            </tr>
            <tr>
                <td>内容:
                </td>
                <td>
                    <textarea id="txtContent1" style="width: 300px;" placeholder="最多输入1000个字"></textarea>
                </td>
            </tr>
            <tr>
                <td>链接:
                </td>
                <td>
                    <input id="txtLink1" type="text" style="width: 300px;" placeholder="最多输入500个字符" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAccountAmount" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>余额变动值:
                </td>
                <td>
                    <input id="txtAccountAmount" type="text" maxlength="8" style="width: 200px;" 
                        placeholder="请输入金额(正数或负数)" />
                </td>
            </tr>

            <tr>
                <td>变动说明:
                </td>
                <td>
                    <input id="txtAccountAmountMsg" type="text" style="width: 200px;" 
                        placeholder="请输入变动说明" />
                </td>
            </tr>

        </table>
    </div>
    <div id="digCheck" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%;padding:20px;">
            <tr>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbFans" /><label
                        for="cbFans">公众号粉丝</label>
                </td>
                <td>
                    
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbMember" /><label
                        for="cbMember">标准会员</label>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbPhoneVer" /><label
                        for="cbPhoneVer">手机认证会员</label>
                </td>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbDisOnLine" /><label
                        for="cbDisOnLine">移动分销会员</label>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbWxnickName" /><label
                        for="cbWxnickName">有昵称</label>
                </td>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbName" /><label
                        for="cbName">有姓名</label>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbPhone" /><label
                        for="cbPhone">有手机</label>
                </td>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbEmail" /><label
                        for="cbEmail">有邮箱</label>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbApp" /><label
                        for="cbApp">有登录App</label>
                </td>
                <td>
                </td>
            </tr>
            <tr style="display:none;">
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbDisOffLine" /><label
                        for="cbDisOffLine">业务分销会员</label>
                </td>
                <td>
                    <input type="checkbox" name="cbUserType" class="positionTop2" id="cbReg" /><label
                        for="cbReg">注册用户</label>
                </td>
            </tr>
        </table>

        <br />
        <br />
        <span>条件组合规则：</span>
        <input type="radio"  name="group" id="rdoHe" checked="checked" class="positionTop2"/><label for="rdoHe">和</label>
        <input type="radio"  name="group" id="rdoHuo" class="positionTop2"/><label for="rdoHuo">或</label>
    </div>

     <div id="dlgPassword" class="easyui-dialog" closed="true" title="修改密码" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    新密码:
                </td>
                <td>
                    <input id="editPwd" type="text" style="width: 200px;" maxlength="20" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<% StringBuilder sbHtml = new StringBuilder(); %>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var handlerMemberUrl = "/serv/api/admin/member/";
        var currUserAction = '';
        var isFans = "";//是否是公众号粉丝
        var isReg = "";//是否是注册会员
        var isDisOnLineUser = "";//是否是商城分销会员
        var isDisOffLineUser = "";//是否是业务分销会员
        var isPhoneReg = "";//是否手机认证会员
        var isName = "";//姓名
        var isPhone = "";//手机
        var isEmail = "";//邮箱
        var isApp = "";//有装App
        var isWxnickName = "";//昵称
        var isMember = "";//标准会员
        var isOrAnd = "1";//是否 和、或
        var userAutoId = "<%=Request["aid"] %>";//用户AutoId
        var mapping_type = "<%=Request["mapping_type"] %>";//用户AutoId
        var curAutoID = 0;
        var defImg = "http://open-files.comeoncloud.net/www/guicai/jubit/image/20160325/A12476F2D0CD4B8FA037267F393BF69E.png";
        var pageType = '<%=page_type%>';
        var moduleName = '<%=moduleName%>';
        var userTye = '<%=userType%>';
        $(function () {

            $('#dlgAddUser').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {



                            var dataModel = {

                                true_name: $("#txtTrueName").val(),
                                phone: $("#txtPhone").val()



                            }
                            if (dataModel.true_name =="") {

                                Alert("请输入姓名");
                                return;
                            }
                            if (dataModel.phone == '') {

                                Alert("请输入手机号");
                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: '/serv/api/admin/user/add.ashx',
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgAddUser').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAddUser').dialog('close');
                    }
                }]
            });


            $("#cbName").attr("checked", true);
            //显示全部
            //$("input[type='radio'][name='identity']").click(function () {
            //    Search();
            //});

            $('.imgTxtEdit').live("change", function () {
                var tempSrc = $.trim($(this).val());
                if (tempSrc == "") tempSrc = defImg;
                $(this).prev().prev().prev().attr('src', tempSrc);
            });
            $('.imgEdit').live("click", function () {
                $(this).next().click();
            });
            isName = "1";

            <%if (websiteOwner=="meifan"){%> 
            
            isName = "";
            <%}%>
	
		 
	           
            if (pageType == 'fans') {
                isFans = "1";
                $(cbFans).attr("checked", true);
                isName = "";
                $("#cbName").attr("checked", false);
            }

            
            $(".fileTxtEdit").live('change', function () {
                var fpath = $.trim($(this).val());
                var fid = $.trim($(this).attr("id"));
                var zid = fid.replace("fileEdit", "");
                var txtfid = "txtEdit" + zid;
                var imgfid = "imgEdit" + zid;
                if (fpath == "") return;
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=MemberInfo',
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


            //标签列表
            $('#grvTagNameData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: 'member' },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize:1000,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TagName', title: '标签名称', width: 20, align: 'left' }


	                ]]
	            }
            );


            //标签列表
            $('#grvTagData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: "member" },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 1000,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TagName', title: '标签名称', width: 20, align: 'left' }
	                ]]
	            }
            );


            //标签列表
            $('#grvTagData2').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryMemberTag", TagType: "member" },
	                height: 200,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'TagName', title: '用户标签', width: 20, align: 'left' }
	                ]]
	            });

            //显示会员
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerMemberUrl+"list.ashx",
	                queryParams: { mapping_type: mapping_type, isName: isName, autoId: userAutoId, isOrAnd: isOrAnd, isFans: isFans ,user_type:userTye},
	                height: document.documentElement.clientHeight - 120,
	                pagination: true,
	                striped: true,
	                loadFilter: pagerFilter,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '<%= idFieldName %>', width: 60, align: 'left', formatter: FormatterTitle },
                                {
                                    field: 'WXHeadimgurl', title: '<%= wxHeadimgurlFieldName %>', width: 60, align: 'center', formatter: function (value, rowData) {
                                        if (value == '' || value == null) value = defImg;
                                        var str = new StringBuilder();
                                        str.AppendFormat('<div class="wrapWXHead">');
                                        if (rowData.IsWeixinFollower == 1) {

                                            str.AppendFormat('<span class="weixinFollowerTip">粉</span>', value);
                                        } 
                                        str.AppendFormat('<a href="javascript:;"><img alt="" class="imgAlign" src="{0}" title="缩略图" height="25" width="25" /></a>', value);
                                        str.AppendFormat('</div>');
                                        return str.ToString();
                                    }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'center' },
                                <% GetDatagridField(); %>

                                <%if (websiteOwner != "meifan") { %>
                                  
                                   {
                                     field: 'TotalScore', title: '<%= totalScoreFieldName %>', width: 80, align: 'center', formatter: function (value, rowData) {
                                     var str = new StringBuilder();
                                     str.AppendFormat("<a class='listClickNum' title='<%=moduleName %>历史记录' href='Scorehistory.aspx?autoid={1}'>{0}</a>", rowData.TotalScore, rowData.AutoID);
                                     return str.ToString();
                                    }
                                    },
                                 <% }%>
                                  <%if (websiteOwner== "meifan") { %>

                                  {
                                      field: 'Ex2', title: '会员卡类型', width: 100, align: 'center', formatter: function (value, rowData) {

                                          switch (value) {
                                              case "personal":
                                                  return "个人卡";
                                              case "family":
                                                  return "家庭卡";
                                              case "chuandong":
                                                  return "船东卡";
                                              default:

                                          }

                                      }
                                  },
                                  { field: 'Ex1', title: '会员卡号', width: 100, align: 'center' },
                                  


                                  <% }%>
                                <%if (IsShowUserType)
                                  {%>
                                {
                                    field: 'UserType', title: '是否<%= userTypeFieldName %>', width: 50, align: 'center', formatter: function (value) {
                                        if (value == 4) { return "<%= userTypeFieldName %>"; }
                                        else {return "会员";}
                                    }
                                },
                                <%} %>
                                <%
                                if (isHideTagBtn == 0&&websiteOwner!="meifan")
                                {
                                    %>
                                { field: 'AccessLevel', title: '<%= accessLevelFieldName %>', width: 80, align: 'center' },
                                    <%
                                }
                                %>

	                            <%
        if (isHideAccountAmount == 0 && websiteOwner != "meifan")
                                {
                                    %>
                                   { field: 'AccountAmount', title: '<%= accountAmountLevelFieldName %>', width: 100, align: 'center' },
                                    <%
                                }
                                %>

                                <%
                                if (isHideRecommended == 0&&websiteOwner!="meifan")
                                {
                                    %>
                                        {
                                            field: 'RecommendTrueName', title: '推荐人', width: 100, align: 'center', formatter: function (value, row) {
                                                var result = '';
                                                if (row.DistributionOnLineRecomendUserInfo) {
                                                    if (row.WebsiteOwner == row.DistributionOnLineRecomendUserInfo.UserID) {
                                                        result = "系统";
                                                    } else {
                                                        result = row.DistributionOnLineRecomendUserInfo.TrueName + '(' + row.DistributionOnLineRecomendUserInfo.AutoID + ')';
                                                    }

                                                } else {
                                                    result = "";
                                                }

                                                return result;

                                            }
                                        },
                                    <%
                                }
                                %>
                                   {
                                       field: 'uv', title: '浏览记录', width: 100, align: 'center', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat("<a class='listClickNum' title='浏览历史' href='/App/Monitor/PVEventDetails.aspx?userId={0}'>查看</a>", rowData.UserID);
                                           return str.ToString();
                                       }
                                   }
                                 



	                ]]
	            });

            //编辑用户信息对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = { mapping_type: mapping_type, AutoID: curAutoID };
                        <%
                        sbHtml = new StringBuilder();
                        foreach (ZentCloud.BLLJIMP.Model.TableFieldMapping item in formField.Where(p => p.IsReadOnly != 1 && !limitForeach.Contains(p.Field)))
                        {
                            if (item.FieldType == "sex")
                            {
                                sbHtml.AppendLine(string.Format("dataModel.{0} = $.trim($('#ddlEdit{0}').val());", item.Field));
                            }
                            else if (item.FieldType == "img")
                            {
                                sbHtml.AppendLine(string.Format("dataModel.{0} = $.trim($('#imgEdit{0}').attr('src'));", item.Field));
                                sbHtml.AppendLine(string.Format("if(dataModel.{0} == defImg) dataModel.{0}='';", item.Field));
                            }
                            else
                            {
                                sbHtml.AppendLine(string.Format("dataModel.{0} = $.trim($('#txtEdit{0}').val());", item.Field));
                            }
                            if (item.FieldIsNull == 1 && item.IsReadOnly != 1)
                            {
                                sbHtml.AppendLine(string.Format("if(dataModel.{2} == ''){0} $.messager.alert('系统提示', '{3}不能为空'); return;{1}", "{", "}", item.Field, item.MappingName));
                            }
                        }
                        this.Response.Write(sbHtml.ToString());
                        %>

                        $.ajax({
                            type: 'post',
                            url: handlerMemberUrl+ (dataModel.AutoID==0?"add.ashx": "update.ashx"),
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    $('#grvTagNameData').datagrid('reload');
                                    $('#grvTagData').datagrid('reload');
                                    $.messager.show({
                                        title: '系统提示',
                                        msg: resp.msg
                                    });
                                }
                                else {
                                    $.messager.alert('系统提示', resp.msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });


            //设置分销上级对话框
            $('#dlgDistribution').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {

                        var AutoIDS = GetRowsIds($('#grvData').datagrid('getSelections')).toString();
                        var dataModel = {
                            Action: "SetDistributionOwner",
                            AutoIDS: AutoIDS,
                            DistributionOwner: $.trim($('#txtDistributionOwner').val())
                        }

                        if (dataModel.UserID == '') {
                            Alert("UserID不能为空!");
                            return;
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgDistribution').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                }
                                Alert(resp.Msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgDistribution').dialog('close');
                    }
                }]
            });

            ///添加积分对话框
            $('#dlgAddScore').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            Action: "AddScore",
                            AddScore: $.trim($('#txtAddScore').val()),
                            Descript: $.trim($('#txtDesc').val()),
                            ids: GetRowsIds(rows).join(','),
                            userid: GetUserIds(rows).join(','),
                            module:moduleName
                        }

                        if (dataModel.AddScore == '') {
                            Alert("<%=moduleName %>不能为空!");
                            return;
                        }
                        if (dataModel.Descript == '') {
                            Alert("说明不能为空!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status > 0) {
                                    $('#dlgAddScore').dialog('close');
                                    $('#grvData').datagrid('reload');
                                    $('#txtAddScore').val('');
                                    $('#txtDesc').val('');
                                }
                                else {

                                }
                                Alert(resp.Msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAddScore').dialog('close');
                    }
                }]
            });

            //修改密码
            $('#dlgPassword').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            Action: "UpdatePassword",
                            pwd: $.trim($('#editPwd').val()),
                            autoid: GetRowsIds(rows).join(',')

                        }

                        if (dataModel.pwd == '') {
                            Alert("密码不能为空!");
                            return;
                        }
                        if (dataModel.pwd.length < 6) {
                            Alert("长度不能少于6位!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: '/serv/api/admin/user/UpdatePassword.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgPassword').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                $('#editPwd').val("");
                                Show(resp.msg);
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgPassword').dialog('close');
                    }
                }]
            });
            //修改余额对话框
            //$('#dlgAddAccountAmount').dialog({
            //    buttons: [{
            //        text: '确定',
            //        handler: function () {
            //            var rows = $('#grvData').datagrid('getSelections');

            //            var dataModel = {
            //                Action: "SetUserAccountAmount",
            //                AccountAmount: $.trim($("#txtAccount").val()),
            //                AutoID: $('#grvData').datagrid('getSelections')[0].AutoID
            //            };

            //            if (dataModel.AccountAmount == '') {
            //                Alert('余额不能为空');
            //                return;
            //            }

            //            $.ajax({
            //                type: 'post',
            //                url: handlerUrl,
            //                data: dataModel,
            //                dataType: "json",
            //                success: function (resp) {
            //                    if (resp.Status > 0) {
            //                        $('#dlgAddAccountAmount').dialog('close');
            //                        Alert("设置用户余额成功。");
            //                        $('#grvData').datagrid('reload');
            //                    }
            //                    else {
            //                        Alert(resp.Msg);
            //                    }
            //                }
            //            });
            //        }
            //    }, {
            //        'text': '取消',
            //        handler: function () {
            //            $('#dlgAddAccountAmount').dialog('close');
            //        }
            //    }
            //    ]
            //});

            ///设置访问级别对话框
            $('#dlgAccessLevel').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $('#grvData').datagrid('getSelections');


                        var dataModel = {
                            Action: "SetUserAccessLevel",
                            AccessLevel: $.trim($('#txtAccessLevel').val()),
                            UserAutoIds: GetRowsIds(rows).join(',')

                        }

                        if (dataModel.AccessLevel == '') {
                            Alert("会员等级不能为空!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status > 0) {
                                    $('#dlgAccessLevel').dialog('close');
                                    Alert("设置会员等级成功。");
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAccessLevel').dialog('close');
                    }
                }]
            });


            ///发送微信模板消息
            $('#dlgSendTemplateMsg').dialog({
                buttons: [{
                    text: '发送',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var SendTo = $('input[name="rdoSend"]:checked').val();
                        var dataModel = {
                            Action: "SendTemplateMsg",
                            TemplateType: $(ddlTemplateType).val(),
                            SendTo:SendTo,
                            Title: $(txtTitle).val(),
                            Content: $(txtContent).val(),
                            Url: $(txtLink).val(),
                            UserAutoIds: GetRowsIds(rows).join(',')
                        }

                        if (dataModel.Title == '') {
                            Alert("标题不能为空!");
                            return;
                        }
                        if (dataModel.Content == '') {
                            Alert("内容不能为空!");
                            return;
                        }
                        if (dataModel.Title.length >= 500) {
                            Alert("标题不能超过500个字!");
                            return;
                        }
                        if (dataModel.Content.length >= 1000) {
                            Alert("内容不能超过1000个字!");
                            return;
                        }
                        if (dataModel.Url.length >= 500) {
                            Alert("链接不能超过500个字符!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgSendTemplateMsg').dialog('close');
                                    Alert(resp.Msg);
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgSendTemplateMsg').dialog('close');
                    }
                }]
            });

            //通过标签发送微信消息
            $('#dlgSendTemplateMsgByTag').dialog({
                buttons: [{
                    text: '发送',
                    handler: function () {
                        var rows = $("#grvTagData2").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        var tags = [];
                        for (var i = 0; i < rows.length; i++) {
                            tags.push(rows[i].TagName);
                        }

                        var dataModel = {
                            Action: "SendTemplateMsgByTag",
                            TemplateType: $(ddlTemplateType1).val(),
                            Title: $(txtTitle1).val(),
                            Content: $(txtContent1).val(),
                            Url: $(txtLink1).val(),
                            Tags: tags.join(',')

                        }

                        if (dataModel.Title == '') {
                            Alert("标题不能为空!");
                            return;
                        }
                        if (dataModel.Content == '') {
                            Alert("内容不能为空!");
                            return;
                        }
                        if (dataModel.Title.length >= 500) {
                            Alert("标题不能超过500个字!");
                            return;
                        }
                        if (dataModel.Content.length >= 1000) {
                            Alert("内容不能超过1000个字!");
                            return;
                        }
                        if (dataModel.Url.length >= 500) {
                            Alert("链接不能超过500个字符!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgSendTemplateMsgByTag').dialog('close');
                                    Alert(resp.Msg);

                                }
                                else {
                                    Alert(resp.Msg);
                                }


                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgSendTemplateMsgByTag').dialog('close');
                    }
                }]
            });

            $('#dlgAccountAmount').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $('#grvData').datagrid('getSelections');


                        var dataModel = {

                            amount: $.trim($('#txtAccountAmount').val()),
                            msg: $.trim($('#txtAccountAmountMsg').val()),
                            autoid: GetRowsIds(rows).join(',')

                        }

                        if (dataModel.amount == '') {
                            Alert("请填写金额");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: "/serv/api/admin/user/account/AddAmount.ashx",
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgAccountAmount').dialog('close');
                                    Alert("操作成功");
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAccountAmount').dialog('close');
                    }
                }]
            });





            //
            $(' #comment textarea ').keyup(function () {     //输入字符后键盘up时触发事件
                var txtLeng = $(' #comment textarea ').val().length;      //把输入字符的长度赋给txtLeng
                //拿输入的值做判断
                if (txtLeng > 300) {
                    //输入长度大于300时span显示0
                    $(' #comment p span ').text(' 0 ');
                    //截取输入内容的前300个字符，赋给fontsize
                    var fontsize = $('#comment textarea').val().substring(0, 300);
                    //显示到textarea上
                    $(' #comment textarea ').val(fontsize);
                } else {
                    //输入长度小于300时span显示300减去长度
                    $('#comment p span').text(300 - txtLeng);
                }
            });

            //标签搜索
            $('#dlgTagName').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rowsTag = $('#grvTagNameData').datagrid('getSelections');
                        var TagName = [];
                        for (var i = 0; i < rowsTag.length; i++) {
                            TagName.push(rowsTag[i].TagName);

                        }

                        $("#txtTagName").val(TagName.join(','));
                        $('#dlgTagName').dialog('close');
                        Search();

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTagName').dialog('close');
                    }
                }]
            });


        });

            //搜索
            function Search() {
                var model = {
                    mapping_type: mapping_type,
                    KeyWord: $("#txtKeyWord").val(),
                    TagName: $("#txtTagName").val(),
                    IsFans: isFans,//是否是公众号粉丝
                    IsReg: isReg,//是否是注册会员
                    IsDisOnLineUser: isDisOnLineUser,//是否是商城分销会员
                    IsDisOffLineUser: isDisOffLineUser,//是否是业务分销会员
                    IsPhoneReg: isPhoneReg,//是否手机认证会员
                    isName: isName,//是否有姓名
                    isPhone: isPhone,//是否有手机
                    isEmail: isEmail,//是否有邮箱
                    isApp: isApp,//是否有登录App
                    isWxnickName: isWxnickName,//是否有微信昵称,
                    isMember: isMember,
                    autoId: userAutoId,
                    isOrAnd:isOrAnd
                }
                $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerMemberUrl+"list.ashx",
                        queryParams: model
                    });
            }


            //高级筛选
            $("#shaixuan").click(function () {
                $('#digCheck').dialog({ title: '高级筛选' });
                $('#digCheck').dialog('open');
            });

            //高级筛选
            $('#digCheck').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        if (cbFans.checked) {
                            isFans = 1;
                        }
                        else {
                            isFans = "";
                        }

                        if (cbReg.checked) {
                            isReg = 1;
                        }
                        else {
                            isReg = "";
                        }

                        if (cbPhoneVer.checked) {
                            isPhoneReg = 1;
                        }
                        else {
                            isPhoneReg = "";
                        }

                        if (cbDisOffLine.checked) {
                            isDisOffLineUser = 1;
                        }
                        else {
                            isDisOffLineUser = "";
                        }

                        if (cbDisOnLine.checked) {
                            isDisOnLineUser = 1;
                        }
                        else {
                            isDisOnLineUser = "";
                        }

                        if (cbName.checked) {
                            isName = 1;
                        } else {
                            isName = "";
                        }

                        if (cbPhone.checked) {
                            isPhone = 1;
                        } else {
                            isPhone = "";
                        }

                        if (cbEmail.checked) {
                            isEmail = 1;
                        } else {
                            isEmail = "";
                        }
                        if (cbApp.checked) {
                            isApp = 1;
                        } else {
                            isApp = "";
                        }

                        if (cbWxnickName.checked) {
                            isWxnickName = 1;
                        } else {
                            isWxnickName = "";
                        }
                        if (cbMember.checked) {
                            isMember = 1;
                        } else {
                            isMember = "";
                        }
                        if (rdoHe.checked) {
                            isOrAnd = 1;
                        } else {
                            isOrAnd = 2;
                        }
                        Search();
                        $('#digCheck').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#digCheck').dialog('close');
                    }
                }]
            });

            //设置用户为导师
            //function SetTutor() {

            //    var rows = $('#grvData').datagrid('getSelections');

            //    if (!EGCheckIsSelect(rows))
            //        return;

            //    if (!EGCheckNoSelectMultiRow(rows))
            //        return;
            //    $.ajax({
            //        type: 'post',
            //        url: handlerUrl,
            //        data: { Action: "SaveTutor", AutoId: rows[0].UserID },
            //        dataType: "json",
            //        success: function (resp) {
            //            Alert(resp.Msg);
            //        }
            //    });

            //}
            //显示编辑用户信息对话框
            function ShowAdd() {
                curAutoID = 0;
                $('#imgEditWXHeadimgurl').attr('src', defImg);
                <%
                foreach (ZentCloud.BLLJIMP.Model.TableFieldMapping item in formField.Where(p => !limitForeach.Contains(p.Field)))
                {
                    sbHtml = new StringBuilder();
                    if (item.FieldType == "sex")
                    {
                        sbHtml.AppendLine(string.Format("$('#ddlEdit{0}').val('');", item.Field));
                    }
                    else if (item.FieldType == "img")
                    {
                        sbHtml.AppendLine(string.Format("$('#imgEdit{0}').attr('src',defImg);", item.Field));
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val(defImg);", item.Field));
                    }
                    else if (item.FieldType == "date")
                    {
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val('');", item.Field));
                    }
                    else
                    {
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val('');", item.Field));
                    }
                    this.Response.Write(sbHtml.ToString());
                } 
                %>

                $('#dlgInfo').dialog({ title: '新增用户' });
                $('#dlgInfo').dialog('open');
            }
            //显示编辑用户信息对话框
            function ShowEdit() {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckNoSelectMultiRow(rows))
                    return;

                curAutoID = rows[0].AutoID;
                var imgSrcWXHeadimgurl = $.trim(rows[0].WXHeadimgurl);
                if (imgSrcWXHeadimgurl == '') imgSrcWXHeadimgurl = defImg;
                $('#imgEditWXHeadimgurl').attr('src', imgSrcWXHeadimgurl);
                <%
                foreach (ZentCloud.BLLJIMP.Model.TableFieldMapping item in formField.Where(p => !limitForeach.Contains(p.Field)))
                {
                    sbHtml = new StringBuilder();
                    if (item.FieldType == "sex")
                    {
                        sbHtml.AppendLine(string.Format("$('#ddlEdit{0}').val(rows[0].{0});", item.Field));
                    }
                    else if (item.FieldType == "img")
                    {
                        sbHtml.AppendLine(string.Format("var imgSrc{0}= $.trim(rows[0].{0});", item.Field));
                        sbHtml.AppendLine(string.Format("if(imgSrc{0}== '') imgSrc{0} = defImg;", item.Field));
                        sbHtml.AppendLine(string.Format("$('#imgEdit{0}').attr('src',imgSrc{0});", item.Field));
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val(imgSrc{0});", item.Field));
                    }
                    else if (item.FieldType == "date")
                    {
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val(new Date(rows[0].{0}).format('yyyy-MM-dd'));", item.Field));
                    }
                    else
                    {
                        sbHtml.AppendLine(string.Format("$('#txtEdit{0}').val(rows[0].{0});", item.Field));
                    }
                    this.Response.Write(sbHtml.ToString());
                } 
                %>

                $('#dlgInfo').dialog({ title: '编辑用户' });
                $('#dlgInfo').dialog('open');
            }

            //显示添加积分对话框
            function ShowEditAddScore() {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $('#dlgAddScore').dialog({ title: '修改<%=moduleName %>' });
                $('#dlgAddScore').dialog('open');

            }

        //批量清零
            function BatchZero() {
                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;
                $.messager.confirm("系统提示", "确定要清空选中用户的<%=moduleName %>?", function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: "/Serv/API/Admin/User/Score/Delete.ashx",
                            data: { ids: GetRowsIds(rows).join(',') },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                    Show(resp.msg);
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }

                        });
                    }
                });

            }

            //显示修余额对话框
            function ShowAddAccountAmount() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                if (!EGCheckNoSelectMultiRow(rows))
                    return;

                //$("#lblUserName").text(rows[0].UserID);
                $('#dlgAccountAmount').dialog({ title: '修改余额' });
                $('#dlgAccountAmount').dialog('open');


            }

            //显示设置访问级别对话框
            function ShowSetAccessLevel() {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;
                $('#dlgAccessLevel').dialog({ title: '设置会员等级' });
                $('#dlgAccessLevel').dialog('open');

            }

            //发送微信消息模板
            function ShowSendTemplateMsg() {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;
                $('#dlgSendTemplateMsg').dialog({ title: '发送微信消息' });
                $('#dlgSendTemplateMsg').dialog('open');

            }

            //发送微信消息模板
            function ShowSendTemplateMsgByTag() {

                $('#dlgSendTemplateMsgByTag').dialog({ title: '发送微信消息' });
                $('#dlgSendTemplateMsgByTag').dialog('open');

            }

            //显示设置分销上级对话框
            function ShowEditDistributionOwner() {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;
                $('#dlgDistribution').dialog({ title: '设置上级用户' });
                $('#dlgDistribution').dialog('open');

            }

            //显示设置标签对话框
            function ShowTagName() {
                var rows = $('#grvTagData').datagrid('getSelections');
                $('#dlgTagName').dialog({ title: '设置标签' });
                $('#dlgTagName').dialog('open');

            }

            //选择标签对话框
            function ShowEditTag() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows)) {

                    return;
                }
                $('#dlgTag').dialog({ title: '设置标签' });
                $('#dlgTag').dialog('open');

            }

            //设置用户标签
            $('#dlgTag').dialog({

                buttons: [{
                    text: '保存',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');

                        var rowsTag = $('#grvTagData').datagrid('getSelections');
                        if (!EGCheckIsSelect(rowsTag)) {
                            return;
                        }

                        var AutoID = [];
                        var TagName = [];
                        var AccessLevel = [];

                        for (var i = 0; i < rowsTag.length; i++) {
                            TagName.push(rowsTag[i].TagName);
                            AccessLevel.push(rowsTag[i].AccessLevel);
                        }

                        for (var i = 0; i < rows.length; i++) {
                            AutoID.push(rows[i].AutoID);
                        }
                        var tagType = $("input[name=tags]:checked").val();
                        if (tagType == "add") {
                            action = "UpdateUserTagNameByAddTag";
                        } else if (tagType == "delete") {
                            action = "UpdateUserTagNameByDeleteTag";
                        } else if (tagType == "update") {
                            action = "UpdateUserTagName"
                        }
                        var dataModel = {
                            Action: action,
                            AutoID: AutoID.join(','),
                            TagName: TagName.join(','),
                            AccessLevel: AccessLevel.join(',')
                        };
                        $.messager.progress({ text: '正在提交...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    Alert("保存成功");
                                    $('#dlgTag').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgTag').dialog('close');
                    }
                }]
            });

            //设置经销商
            function SetAgent() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                if (!EGCheckNoSelectMultiRow(rows))
                    return;
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: { Action: "SetAgent", AutoId: rows[0].AutoID },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            Alert('设置成功');
                            $('#grvData').datagrid('reload');
                        }
                        else {
                            Alert('操作失败');
                        }


                    }
                });

            }
            //设置经销商
            function SetChannel() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                if (!EGCheckNoSelectMultiRow(rows))
                    return;
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: { Action: "SetChannel", ids: GetRowsIds(rows).join(',') },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            Alert('设置成功');
                            $('#grvData').datagrid('reload');
                        }
                        else {
                            Alert('操作失败');
                        }


                    }
                });

            }

            //获取选中行ID集合
            function GetRowsIds(rows) {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].AutoID);

                }
                return ids;
            }

            function GetUserIds(rows){
                var uids=[];
                for (var i = 0; i < rows.length; i++) {
                    uids.push(rows[i].UserID);
                }
                return uids;
            }


            //同步会员信息
            function SynMemberInfo() {
                $.messager.confirm("系统提示", "确定同步会员信息?此过程可能需要几分钟", function (r) {
                    if (r) {
                        $.messager.progress({ text: '正在同步,请稍候。此过程可能需要几分钟...' });
                        jQuery.ajax({
                            type: "Post",
                            url: handlerUrl,
                            dataType: "json",
                            data: { Action: "SynMemberInfo" },
                            success: function (resp) {
                                $.messager.progress('close');
                                Alert(resp.Msg);
                                $('#grvData').datagrid('reload');

                            }
                        });
                    }
                });
            };
            

            //清洗会员数据
            function Clean() {
                $.messager.confirm("系统提示", "确定要清洗取消关注公众号的会员吗?", function (r) {
                    if (r) {
                       
                        jQuery.ajax({
                            type: "Post",
                            url: handlerUrl,
                            dataType: "json",
                            data: { Action: "CleanUser" },
                            success: function (resp) {
                                Alert(resp.Msg);
                              

                            }
                        });
                    }
                });
            };
            //批量禁用、启用
            function DisableUser(disableValue) {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                var currSelectUsers = new Array();

                for (var i = 0; i < rows.length; i++) {
                    currSelectUsers.push(rows[i]);
                }

                var ids = new Array();

                for (var i = 0; i < currSelectUsers.length; i++) {
                    ids.push("'" + currSelectUsers[i].UserID + "'");
                }

                var model = {
                    Action: 'DisableUser',
                    userIds: ids.join(','),
                    disableValue: disableValue
                }

                if ($.messager.confirm('友情提示', "确定" + (disableValue == 1 ? "禁用" : "启用") + "当前选择的用户?", function (o) {
                  if (o) {
                      $.ajax({
                            type: "Post",
                            url: '/Handler/App/CationHandler.ashx',
                            data: model,
                            dataType: "json",
                            success: function (resp) {
                                        if (resp.Status > 0) {
                                              $('#grvData').datagrid('reload');
                                              Show('操作成功');
                                }
                                else {
                                   Alert('操作失败');
                                }
                            }
                        });
                    }
                }));

            }

            function SetEmployee() {
                var ids = GetCurrSelectIds();

                if (ids.length == 0) {
                    return;
                }

                var reqModel = {
                    action: 'SetEmployee',
                    ids: ids.join(',')
                }

                $.ajax({
                    type: "Post",
                    url: '/Handler/App/CationHandler.ashx',
                    data: reqModel,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.IsSuccess) {
                            $('#grvData').datagrid('reload');
                            Show('操作成功');
                        }
                        else {
                            Alert('操作失败');
                        }
                    }
                });

            }
            
            //获取当前用户的ids
            function GetCurrSelectIds() {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return [];

                var currSelectUsers = new Array();

                for (var i = 0; i < rows.length; i++) {
                    currSelectUsers.push(rows[i]);
                }

                var ids = new Array();

                for (var i = 0; i < currSelectUsers.length; i++) {
                    ids.push(currSelectUsers[i].AutoID);
                }

                return ids;
            }

            //修改密码
            function EditPassword() {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                if (!EGCheckNoSelectMultiRow(rows))
                    return;
                $('#dlgPassword').dialog({ title: '修改密码' });
                $('#dlgPassword').dialog('open');

            }

            function ShowAddUser() {



                $('#dlgAddUser').dialog({ title: '新增用户' });
                $('#dlgAddUser').dialog('open');




            }

    </script>
</asp:Content>
