<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Policy.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
    <style type="text/css">
        .Width92P {
            width: 92% !important;
        }

        .Width100 {
            width: 100px !important;
        }

        .DivTextarea {
            padding: 5px;
            min-height: 94px;
            line-height: 21px;
            border: 1px solid grey;
        }

        .SmallCheckUl {
            width: 100%;
            list-style-type: none;
        }

            .SmallCheckUl li {
                width: 15%;
                float: left;
            }

        .AutoUl {
            width: 100%;
            list-style-type: none;
        }

            .AutoUl li {
                float: left;
            }
            .divFileClass{
            }
            .divFileParent{position:relative;}
            .divFileName{
                padding-right: 30px;
                line-height: 18px;
            }
            .divFileName a{
                color:blue;
            }
            .deleteFile{
                position:absolute;
                top:0px;
                right:0px;
                cursor:pointer;
            }
            .deleteFile img{
                width:18px;
                height:18px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    政策<%=Request["id"]=="0"?"新增":"编辑" %>
    <a href="List.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable" style="width: 800px;">
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">政策名称：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" class="commonTxt" placeholder="政策名称(必填)" value="<%=nInfo.ActivityName %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">政策对象：</td>
                    <td width="*" align="left">
                        <ul class="SmallCheckUl">
                            <li>
                                <input id="rdoPolicyObject1" type="radio" name="PolicyObject" class="positionTop2" data-value="个人" value="个人" <% = nInfo.K2 != "单位"?"checked='checked'":"" %> /><label for="rdoPolicyObject1">个人</label></li>
                            <li>
                                <input id="rdoPolicyObject2" type="radio" name="PolicyObject" class="positionTop2" data-value="单位" value="单位" <% = nInfo.K2 == "单位"?"checked='checked'":"" %> /><label for="rdoPolicyObject2">单位</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle"> <%= nInfo.K2 != "单位"?"户籍":"单位" %>所在地：</td>
                    <td width="*" align="left">
                        <ul class="SmallCheckUl">
                            <li>
                                <input id="chkDomicilePlace1" type="checkbox" name="DomicilePlace" class="positionTop2" value="黄浦" <% =  CheckDomicilePlace("黄浦") %> /><label for="chkDomicilePlace1">黄浦</label></li>
                            <li>
                                <input id="chkDomicilePlace2" type="checkbox" name="DomicilePlace" class="positionTop2" value="徐汇" <% =  CheckDomicilePlace("徐汇")  %> /><label for="chkDomicilePlace2">徐汇</label></li>
                            <li>
                                <input id="chkDomicilePlace3" type="checkbox" name="DomicilePlace" class="positionTop2" value="长宁" <% =  CheckDomicilePlace("长宁")  %> /><label for="chkDomicilePlace3">长宁</label></li>
                            <li>
                                <input id="chkDomicilePlace4" type="checkbox" name="DomicilePlace" class="positionTop2" value="静安" <% =  CheckDomicilePlace("静安")  %> /><label for="chkDomicilePlace4">静安</label></li>
                            <li>
                                <input id="chkDomicilePlace5" type="checkbox" name="DomicilePlace" class="positionTop2" value="普陀" <% =  CheckDomicilePlace("普陀")  %> /><label for="chkDomicilePlace5">普陀</label></li>
                            <li>
                                <input id="chkDomicilePlace6" type="checkbox" name="DomicilePlace" class="positionTop2" value="虹口" <% =  CheckDomicilePlace("虹口")  %> /><label for="chkDomicilePlace6">虹口</label></li>
                            <li>
                                <input id="chkDomicilePlace7" type="checkbox" name="DomicilePlace" class="positionTop2" value="杨浦" <% =  CheckDomicilePlace("杨浦")  %> /><label for="chkDomicilePlace7">杨浦</label></li>
                            <li>
                                <input id="chkDomicilePlace8" type="checkbox" name="DomicilePlace" class="positionTop2" value="闵行" <% =  CheckDomicilePlace("闵行")  %> /><label for="chkDomicilePlace8">闵行</label></li>
                            <li>
                                <input id="chkDomicilePlace9" type="checkbox" name="DomicilePlace" class="positionTop2" value="宝山" <% =  CheckDomicilePlace("宝山")  %> /><label for="chkDomicilePlace9">宝山</label></li>
                            <li>
                                <input id="chkDomicilePlace10" type="checkbox" name="DomicilePlace" class="positionTop2" value="嘉定" <% = CheckDomicilePlace("嘉定")  %> /><label for="chkDomicilePlace10">嘉定</label></li>
                            <li>
                                <input id="chkDomicilePlace11" type="checkbox" name="DomicilePlace" class="positionTop2" value="浦东" <% = CheckDomicilePlace("浦东")  %> /><label for="chkDomicilePlace11">浦东</label></li>
                            <li>
                                <input id="chkDomicilePlace12" type="checkbox" name="DomicilePlace" class="positionTop2" value="金山" <% = CheckDomicilePlace("金山")  %> /><label for="chkDomicilePlace12">金山</label></li>
                            <li>
                                <input id="chkDomicilePlace13" type="checkbox" name="DomicilePlace" class="positionTop2" value="松江" <% = CheckDomicilePlace("松江")  %> /><label for="chkDomicilePlace13">松江</label></li>
                            <li>
                                <input id="chkDomicilePlace14" type="checkbox" name="DomicilePlace" class="positionTop2" value="青浦" <% = CheckDomicilePlace("青浦")  %> /><label for="chkDomicilePlace14">青浦</label></li>
                            <li>
                                <input id="chkDomicilePlace15" type="checkbox" name="DomicilePlace" class="positionTop2" value="奉贤" <% = CheckDomicilePlace("奉贤")  %> /><label for="chkDomicilePlace15">奉贤</label></li>
                            <li>
                                <input id="chkDomicilePlace16" type="checkbox" name="DomicilePlace" class="positionTop2" value="崇明" <% = CheckDomicilePlace("崇明")  %> /><label for="chkDomicilePlace16">崇明</label></li>
                            <li>
                                <input id="chkDomicilePlace17" type="checkbox" name="DomicilePlace" class="positionTop2" value="无要求" <% = CheckDomicilePlace("无要求")  %> /><label for="chkDomicilePlace17">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Personal" <%=CheckPolicyObjectShow("个人") %>>
                    <td align="right" class="tdTitle">性别：</td>
                    <td width="*" align="left">
                        <ul class="SmallCheckUl">
                            <li>
                                <input id="rdoSex1" type="radio" name="Sex" class="positionTop2" data-value="男" value="男" <% = CheckSex("男") %> /><label for="rdoSex1">男</label></li>
                            <li>
                                <input id="rdoSex2" type="radio" name="Sex" class="positionTop2" data-value="女" value="女" <% =  CheckSex("女")  %> /><label for="rdoSex2">女</label></li>
                            <li>
                                <input id="rdoSex3" type="radio" name="Sex" class="positionTop2" data-value="无要求" value="无要求" <% =  CheckSex("无要求")  %> /><label for="rdoSex3">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Personal Male" <%=CheckAgeShow("男") %>>
                    <td align="right" class="tdTitle">男性年龄段：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtMaleAgeMin" class="commonTxt Width100" placeholder="" value="<%=nInfo.K9 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />周岁
                        至
                        <input type="text" id="txtMaleAgeMax" class="commonTxt Width100" placeholder="" value="<%=nInfo.K10 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />周岁
                    </td>
                </tr>
                <tr class="Personal Female" <%=CheckAgeShow("女") %>>
                    <td align="right" class="tdTitle">女性年龄段：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtFemaleAgeMin" class="commonTxt Width100" placeholder="" value="<%=nInfo.K11 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />周岁
                        至
                        <input type="text" id="txtFemaleAgeMax" class="commonTxt Width100" placeholder="" value="<%=nInfo.K12 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />周岁
                    </td>
                </tr>
                <tr class="Personal" <%=CheckPolicyObjectShow("个人") %>>
                    <td align="right" class="tdTitle">学历：</td>
                    <td width="*" align="left">
                        <ul class="AutoUl">
                            <li>
                                <input id="chkEducation1" type="checkbox" name="Education" class="positionTop2" value="研究生及以上" <% = CheckMultField(nInfo.K13,"研究生及以上") %> /><label for="chkEducation1">研究生及以上</label></li>
                            <li>
                                <input id="chkEducation2" type="checkbox" name="Education" class="positionTop2" value="大学本科" <% =  CheckMultField(nInfo.K13,"大学本科")  %> /><label for="chkEducation2">大学本科</label></li>
                            <li>
                                <input id="chkEducation3" type="checkbox" name="Education" class="positionTop2" value="大专高职" <% =  CheckMultField(nInfo.K13,"大专高职")  %> /><label for="chkEducation3">大专高职</label></li>
                            <li>
                                <input id="chkEducation4" type="checkbox" name="Education" class="positionTop2" value="中专职技校" <% =  CheckMultField(nInfo.K13,"中专职技校")  %> /><label for="chkEducation4">中专职技校</label></li>
                            <li>
                                <input id="chkEducation5" type="checkbox" name="Education" class="positionTop2" value="初中" <% =  CheckMultField(nInfo.K13,"初中")  %> /><label for="chkEducation5">初中</label></li>
                            <li>
                                <input id="chkEducation6" type="checkbox" name="Education" class="positionTop2" value="无要求" <% =  CheckMultField(nInfo.K13,"无要求")  %> /><label for="chkEducation6">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Personal" <%=CheckPolicyObjectShow("个人") %>>
                    <td align="right" class="tdTitle">毕业年限：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtGraduationYearMin" class="commonTxt Width100" placeholder="" value="<%=nInfo.K14 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />年
                        至
                        <input type="text" id="txtGraduationYearMax" class="commonTxt Width100" placeholder="" value="<%=nInfo.K15 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />年
                    </td>
                </tr>
                <tr class="Personal" <%=CheckPolicyObjectShow("个人") %>>
                    <td align="right" class="tdTitle">就业状态：</td>
                    <td width="*" align="left">
                        <ul class="AutoUl">
                            <li>
                                <input id="chkEmploymentStatus1" type="checkbox" name="EmploymentStatus" class="positionTop2" data-value="就业" value="就业" <% = CheckMultField(nInfo.K16,"就业") %> /><label for="chkEmploymentStatus1">就业</label></li>
                            <li>
                                <input id="chkEmploymentStatus2" type="checkbox" name="EmploymentStatus" class="positionTop2" data-value="登记失业" value="登记失业" <% =  CheckMultField(nInfo.K16,"登记失业")  %> /><label for="chkEmploymentStatus2">登记失业</label></li>
                            <li>
                                <input id="chkEmploymentStatus3" type="checkbox" name="EmploymentStatus" class="positionTop2" data-value="未登记失业（无业）" value="未登记失业（无业）" <% =  CheckMultField(nInfo.K16,"未登记失业（无业）")  %> /><label for="chkEmploymentStatus3">未登记失业（无业）</label></li>
                            <li>
                                <input id="chkEmploymentStatus4" type="checkbox" name="EmploymentStatus" class="positionTop2" data-value="无要求" value="无要求" <% =  CheckMultField(nInfo.K16,"无要求")  %> /><label for="chkEmploymentStatus4">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Personal CurrentJobLife" <%=CheckJobLifeShow() %>>
                    <td align="right" class="tdTitle">目前岗位工作年限：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtCurrentJobLifeMin" class="commonTxt Width100" placeholder="" value="<%=nInfo.K17 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />年
                        至
                        <input type="text" id="txtCurrentJobLifeMax" class="commonTxt Width100" placeholder="" value="<%=nInfo.K18 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />年
                    </td>
                </tr>
                <tr class="Personal UnemploymentPeriod" <%=CheckUnemploymentPeriodShow() %>>
                    <td align="right" class="tdTitle">失业期限：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtUnemploymentPeriodMin" class="commonTxt Width100" placeholder="" value="<%=nInfo.K19 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />个月
                        至
                        <input type="text" id="txtUnemploymentPeriodMax" class="commonTxt Width100" placeholder="" value="<%=nInfo.K20 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />个月
                    </td>
                </tr>
                <tr class="Company" <%=CheckPolicyObjectShow("单位") %>>
                    <td style="width: 140px;" align="right" class="tdTitle">单位类型：</td>
                    <td width="*" align="left">
                        <ul class="SmallCheckUl">
                            <li>
                                <input id="rdoCompanyType1" type="radio" name="CompanyType" class="positionTop2" data-value="劳务派遣" value="劳务派遣" <% = nInfo.K21 != "非劳务派遣"?"checked='checked'":"" %> /><label for="rdoCompanyType1">劳务派遣</label></li>
                            <li>
                                <input id="rdoCompanyType2" type="radio" name="CompanyType" class="positionTop2" data-value="非劳务派遣" value="非劳务派遣" <% = nInfo.K21 == "非劳务派遣"?"checked='checked'":"" %> /><label for="rdoCompanyType2">非劳务派遣</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Company" <%=CheckPolicyObjectShow("单位") %>>
                    <td style="width: 140px;" align="right" class="tdTitle">注册资金：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtRegisteredCapitalMin" class="commonTxt Width100" value="<%=nInfo.K22 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />元
                        至
                        <input type="text" id="txtRegisteredCapitalMax" class="commonTxt Width100" value="<%=nInfo.K23 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />元
                    </td>
                </tr>
                <tr class="Company" <%=CheckPolicyObjectShow("单位") %>>
                    <td style="width: 140px;" align="right" class="tdTitle">人员规模：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtPersonnelSizeMin" class="commonTxt Width100" value="<%=nInfo.K24 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />人
                        至
                        <input type="text" id="txtPersonnelSizeMax" class="commonTxt Width100" value="<%=nInfo.K25 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />人
                    </td>
                </tr>
                <tr class="Company" <%=CheckPolicyObjectShow("单位") %>>
                    <td align="right" class="tdTitle">单位规模：</td>
                    <td width="*" align="left">
                        <ul class="AutoUl">
                            <li>
                                <input id="chkCompanySize1" type="checkbox" name="CompanySize" class="positionTop2" value="大型" <% = CheckMultField(nInfo.K26,"大型") %> /><label for="chkCompanySize1">大型</label></li>
                            <li>
                                <input id="chkCompanySize2" type="checkbox" name="CompanySize" class="positionTop2" value="中型" <% =  CheckMultField(nInfo.K26,"中型")  %> /><label for="chkCompanySize2">中型</label></li>
                            <li>
                                <input id="chkCompanySize3" type="checkbox" name="CompanySize" class="positionTop2" value="小型" <% =  CheckMultField(nInfo.K26,"小型")  %> /><label for="chkCompanySize3">小型</label></li>
                            <li>
                                <input id="chkCompanySize4" type="checkbox" name="CompanySize" class="positionTop2" value="微型" <% =  CheckMultField(nInfo.K26,"微型")  %> /><label for="chkCompanySize4">微型</label></li>
                            <li>
                                <input id="chkCompanySize5" type="checkbox" name="CompanySize" class="positionTop2" value="无要求" <% =  CheckMultField(nInfo.K26,"无要求")  %> /><label for="chkCompanySize6">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr class="Company" <%=CheckPolicyObjectShow("单位") %>>
                    <td align="right" class="tdTitle">所属行业：</td>
                    <td width="*" align="left">
                        <ul class="AutoUl">
                            <li>
                                <input id="rdoIndustry1" type="radio" name="Industry" class="positionTop2" data-value="特定行业（绿化市容、物业管理、养老服务、农业）" value="特定行业（绿化市容、物业管理、养老服务、农业）" <% = CheckField(nInfo.K27,"特定行业（绿化市容、物业管理、养老服务、农业）") %> /><label for="rdoIndustry1">特定行业（绿化市容、物业管理、养老服务、农业）</label></li>
                            <li>
                                <input id="rdoIndustry2" type="radio" name="Industry" class="positionTop2" data-value="非特定行业" value="非特定行业" <% =  CheckField(nInfo.K27,"非特定行业")  %> /><label for="rdoIndustry2">非特定行业</label></li>
                            <li>
                                <input id="rdoIndustry3" type="radio" name="Industry" class="positionTop2" data-value="无要求" value="无要求" <% =  CheckField(nInfo.K27,"无要求")  %> /><label for="rdoIndustry3">无要求</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">补贴标准：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtSubsidyStandard" class="commonTxt" placeholder="补贴标准(选填)" value="<%=nInfo.K3 %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">补贴期限：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtSubsidyPeriod" class="commonTxt" placeholder="补贴期限(选填)" value="<%=nInfo.K4 %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">政策文号：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtPolicyNumber" class="commonTxt" placeholder="政策文号(选填)" value="<%=nInfo.K5 %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">政策级别：</td>
                    <td width="*" align="left">
                        <ul class="SmallCheckUl">
                            <li>
                                <input id="rdoPolicyLevel1" type="radio" name="PolicyLevel" class="positionTop2" data-value="市级政策" value="市级政策" <% = nInfo.K6 != "区级政策"?"checked='checked'":"" %> /><label for="rdoPolicyLevel1">市级政策</label></li>
                            <li>
                                <input id="rdoPolicyLevel2" type="radio" name="PolicyLevel" class="positionTop2" data-value="区级政策" value="区级政策" <% = nInfo.K6 == "区级政策"?"checked='checked'":"" %> /><label for="rdoPolicyLevel2">区级政策</label></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">政策大致描述：</td>
                    <td width="*" align="left">
                        <div id="txtPolicySummary" class="DivTextarea" contenteditable="plaintext-only">
                            <%=nInfo.Summary %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file1.click()">政策原文附件</a>
                    </td>
                    <td width="*" align="left" style="border-bottom:solid grey 1px;">
                        <div class="ui-sortable divFileClass divFileClass1">
                            <% foreach (var item in nFiles.Where(p=>p.FileClass==1))
                               {%>
                                <div class="divFileParent">
                                    <div class="divFileName" data-id="<%=item.AutoId %>"><a href="<%=item.FilePath %>" target="_blank"><%=item.FileName %></a></div>
                                    <div class="deleteFile"><img src="/img/delete.png" alt="删除" /></div>
                                </div>
                             <% }%>
                        </div>
                        <div class="clear">
                            <input type="file" id="file1" class="file file1" name="file1" style="display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file2.click()">办事指南原文附件</a>
                    </td>
                    <td width="*" align="left">
                        <div class="ui-sortable divFileClass divFileClass2">
                            <% foreach (var item in nFiles.Where(p=>p.FileClass==2))
                               {%>
                                <div class="divFileParent">
                                    <div class="divFileName" data-id="<%=item.AutoId %>"><a href="<%=item.FilePath %>" target="_blank"><%=item.FileName %></a></div>
                                    <div class="deleteFile"><img src="/img/delete.png" alt="删除" /></div>
                                </div>
                             <% }%>
                        </div>
                        <div class="clear">
                            <input type="file" id="file2" class="file file2" name="file2" style="display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSort" class="commonTxt" placeholder="排序" value="<%=nInfo.Sort.HasValue?nInfo.Sort.Value:0 %>" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'')" />
                        数字越大越排前
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center;">
            <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width: 160px;">提交</a>
            <a href="List.aspx" id="btnPageBack" class="button glow" style="width: 160px;">返回</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/lib/layer/2.1/layer.js"></script>
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/policy/';
        var nid = '<% =nInfo.JuActivityID%>';
        $(function () {
            $('input[name="PolicyObject"]').live("click", function () {
                var nPolicyObject = $('input[name="PolicyObject"]:checked').val();
                if (nPolicyObject == "单位") {
                    $(".Company").show();
                    $(".Personal").hide();
                } else {
                    ShowPersonal();
                }
            });
            $('input[name="Sex"]').live("click", function () {
                var nSex = $('input[name="Sex"]:checked').val();
                if (nSex == "男") {
                    $(".Male").show();
                    $(".Female").hide();
                }
                else if (nSex == "女") {
                    $(".Female").show();
                    $(".Male").hide();
                }
            });
            $('input[name="EmploymentStatus"]').live("click", function () {
                var nEmploymentStatus = $('input[name="EmploymentStatus"]:checked').val();
                if (nEmploymentStatus == "就业") {
                    $(".CurrentJobLife").show();
                    $(".UnemploymentPeriod").hide();
                }
                else if (nEmploymentStatus.indexOf("失业") > 0) {
                    $(".UnemploymentPeriod").show();
                    $(".CurrentJobLife").hide();
                }
                else {
                    $(".UnemploymentPeriod").hide();
                    $(".CurrentJobLife").hide();
                }
            });

            checkCheckBox('DomicilePlace');
            checkCheckBox('Education');
            checkCheckBox('EmploymentStatus');
            checkCheckBox('CompanySize');

            $('.deleteFile').live('click', function () {
                var fobj = $(this).closest('.divFileParent');
                var fname = $(fobj).find('.divFileName a').text();
                $.messager.confirm('系统提示', '确定删除附件 [' + fname + ']？', function (o) {
                    if (o) {
                        $(fobj).remove();
                    }
                });
            });
            $(".file").live('change', function () {
                var fpath = $.trim($(this).val());
                var fns = fpath.lastIndexOf("\\");
                var fname = fpath.substring(fns+1);
                var fid = $.trim($(this).attr("id"));
                if (fpath == "") return;
                var divClass = $(this).closest('.clear').prev();
                try {
                    $.messager.progress({ text: '正在上传...' });
                    $.ajaxFileUpload(
                     {
                         url: '/serv/api/common/file.ashx?action=Add&dir=file',
                         secureuri: false,
                         fileElementId: fid,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                                 var appendhtml = new StringBuilder();
                                 appendhtml.AppendFormat('<div class="divFileParent"><div class="divFileName" data-id="0"><a href="{0}" target="_blank">{1}</a></div>', resp.file_url_list[0], fname);
                                 appendhtml.AppendFormat('<div class="deleteFile"><img src="/img/delete.png" alt="删除" /></div></div>');
                                 $(divClass).append(appendhtml.ToString());
                             }
                             else {
                                 $.messager.alert('温馨提示', resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    $.messager.alert('温馨提示', e);
                }
            });
            $("#btnSave").live("click", function () {
                postUpdateData();
            });
        });
        function checkCheckBox(cname) {
            $('input[name="' + cname + '"][value!="无要求"]').live("click", function () {
                if ($('input[name="' + cname + '"][value!="无要求"]:checked').length == 0) {
                    $('input[name="' + cname + '"][value="无要求"]')[0].checked = true;
                }
                else {
                    $('input[name="' + cname + '"][value="无要求"]')[0].checked = false;
                }
            });
        }
        function postUpdateData() {
            var model = {
                id: nid,
                policy_name: $.trim($("#txtActivityName").val()),
                policy_object: $.trim($('input[name="PolicyObject"]:checked').attr('data-value')),
                subsidy_standard: $.trim($("#txtSubsidyStandard").val()),
                subsidy_period: $.trim($("#txtSubsidyPeriod").val()),
                policy_number: $.trim($("#txtPolicyNumber").val()),
                policy_level: $.trim($('input[name="PolicyLevel"]:checked').attr('data-value')),
                summary: $.trim($("#txtPolicySummary").html()),
                sort: $.trim($("#txtSort").val())
            }
            if (model.policy_name == '') {
                $.messager.alert('温馨提示', '请输入政策名称');
                return;
            }
            //所在地
            var domiciles = [];
            $('input[name="DomicilePlace"][value!="无要求"]:checked').each(function () {
                domiciles.push($(this).val());
            });
            if (domiciles.length == 0) domiciles.push('无要求');
            model.domicile_place = domiciles.join(',');

            if (model.policy_object == "个人") {
                model.sex = $.trim($('input[name="Sex"]:checked').attr('data-value'));

                if (model.sex == "男" || model.sex == "无要求") {
                    model.male_age_min = $.trim($("#txtMaleAgeMin").val());
                    model.male_age_max = $.trim($("#txtMaleAgeMax").val());
                }
                if (model.sex == "女" || model.sex == "无要求") {
                    model.famale_age_min = $.trim($("#txtFemaleAgeMin").val());
                    model.famale_age_max = $.trim($("#txtFemaleAgeMax").val());
                }

                var educations = [];
                $('input[name="Education"][value!="无要求"]:checked').each(function () {
                    educations.push($(this).val());
                });
                if (educations.length == 0) educations.push('无要求');
                model.education = educations.join(',');

                model.graduation_year_min = $.trim($("#txtGraduationYearMin").val());
                model.graduation_year_max = $.trim($("#txtGraduationYearMax").val());

                var employmentstatus = [];
                $('input[name="EmploymentStatus"][value!="无要求"]:checked').each(function () {
                    employmentstatus.push($(this).val());
                });
                if (employmentstatus.length == 0) employmentstatus.push('无要求');
                model.employment_status = employmentstatus.join(',');
                if (model.employment_status == "就业") {
                    model.current_job_life_min = $.trim($("#txtCurrentJobLifeMin").val());
                    model.current_job_life_max = $.trim($("#txtCurrentJobLifeMax").val());
                }
                if (model.employment_status.indexOf("失业") > 0) {
                    model.unemployment_period_min = $.trim($("#txtUnemploymentPeriodMin").val());
                    model.unemployment_period_max = $.trim($("#txtUnemploymentPeriodMax").val());
                }
            }
            else {
                model.company_type = $.trim($('input[name="CompanyType"]:checked').attr('data-value'));
                model.registered_capital_min = $.trim($("#txtRegisteredCapitalMin").val());
                model.registered_capital_max = $.trim($("#txtRegisteredCapitalMax").val());
                model.personnel_size_min = $.trim($("#txtPersonnelSizeMin").val());
                model.personnel_size_max = $.trim($("#txtPersonnelSizeMax").val());

                var companysizes = [];
                $('input[name="CompanySize"][value!="无要求"]:checked').each(function () {
                    companysizes.push($(this).val());
                });
                if (companysizes.length == 0) companysizes.push('无要求');
                model.company_size = companysizes.join(',');

                model.industry = $.trim($('input[name="Industry"]:checked').attr('data-value'));
            }
            var fileList = [];
            var divFile1 = $('.divFileClass1').children();
            if(divFile1.length>0){
                for (var i = 0; i < divFile1.length; i++) {
                    var dobj = $(divFile1[i]).find('.divFileName');
                    var fid = $(dobj).attr('data-id');
                    var fpath = $(dobj).find('a').attr('href');
                    var fname = $(dobj).find('a').text();
                    fileList.push({ id: fid, file_class: 1, file_name: fname, file_path: fpath });
                }
            }
            var divFile2 = $('.divFileClass2').children();
            if(divFile2.length>0){
                for (var i = 0; i < divFile2.length; i++) {
                    var dobj = $(divFile2[i]).find('.divFileName');
                    var fid = $(dobj).attr('data-id');
                    var fpath = $(dobj).find('a').attr('href');
                    var fname = $(dobj).find('a').text();
                    fileList.push({ id: fid, file_class: 2, file_name: fname, file_path: fpath });
                }
            }
            model.file_list = JSON.stringify(fileList);
            if (model.sort == "") model.sort = 0;
            $.messager.progress({ text: '正在提交。。。' });
            $.ajax({
                type: 'post',
                url: handlerUrl + (nid == "0" ? "Add.ashx" : "Update.ashx"),
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        if (nid == "0") {
                            ClearForm();
                            $.messager.alert("提示", resp.msg);
                        }
                        else {
                            location.href = "List.aspx";
                        }
                    }
                    else {
                        $.messager.alert("提示", resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
        function HideSex() {
            var nSex = $('input[name="Sex"]:checked').val();
            if (nSex == "男") {
                $(".Female").hide();
            }
            else if (nSex == "女") {
                $(".Male").hide();
            }
        }
        function HideEmploymentStatus() {
            var nEmploymentStatus = $('input[name="EmploymentStatus"]:checked').val();
            if (nEmploymentStatus == "就业") {
                $(".UnemploymentPeriod").hide();
            }
            else if (nEmploymentStatus.indexOf("失业") > 0) {
                $(".CurrentJobLife").hide();
            }
            else {
                $(".UnemploymentPeriod").hide();
                $(".CurrentJobLife").hide();
            }
        }
        function ShowPersonal() {
            $(".Personal").show();
            $(".Company").hide();
            HideSex();
            HideEmploymentStatus();
        }

        function ClearForm() {
            $("#txtActivityName").val("");
            $("#txtSubsidyStandard").val("");
            $("#txtSubsidyPeriod").val("");
            $("#txtPolicyNumber").val("");
            $("#txtPolicySummary").html("");
            $("#txtMaleAgeMin").val("");
            $("#txtMaleAgeMax").val("");
            $("#txtFemaleAgeMin").val("");
            $("#txtFemaleAgeMax").val("");
            $("#txtGraduationYearMin").val("");
            $("#txtGraduationYearMax").val("");
            $("#txtCurrentJobLifeMin").val("");
            $("#txtCurrentJobLifeMax").val("");
            $("#txtUnemploymentPeriodMin").val("");
            $("#txtUnemploymentPeriodMax").val("");
            $("#txtRegisteredCapitalMin").val("");
            $("#txtRegisteredCapitalMax").val("");
            $("#txtPersonnelSizeMin").val("");
            $("#txtPersonnelSizeMax").val("");
            $("#txtSort").val("0");
            rdoPolicyObject1.checked = true;
            $('input[name="DomicilePlace"]').each(function () {
                this.checked = ($(this).val() == '无要求');
            });
            rdoSex3.checked = true;
            $('input[name="Education"]').each(function () {
                this.checked = ($(this).val() == '无要求');
            }); 
            $('input[name="EmploymentStatus"]').each(function () {
                this.checked = ($(this).val() == '无要求');
            });
            rdoCompanyType1.checked = true;
            $('input[name="CompanySize"]').each(function () {
                this.checked = ($(this).val() == '无要求');
            });
            rdoIndustry3.checked = true;
            rdoPolicyLevel1.checked = true;
            $('.divFileClass1').html('');
            $('.divFileClass2').html('');
        }
    </script>
</asp:Content>
