<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ProjectList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.ProjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         .table {
            margin: auto;
            width: 100%;
            border-collapse: collapse;
        }

            .table td {
                border: solid #dddddd 1px;
                padding: 0px 5px;
                line-height: 26px;
            }
            .table td li{
                line-height:20px;
            }
            .table td li+li{
                border-top: solid 1px #e1e1e1;
            }
            .window-mask{
                width:100% !important;
                height:100% !important;
            }
            .datagrid-wrap{
                width:100% !important;
            }
            .btn-update{
                display:none;
            }
            
            .ImgDiv {
                display: inline-block; 
                padding: 3px 0px;
            }
            .ImgDiv a{
                display: inline-block;
                margin:3px;
                position:relative;
                border: solid #f3f3f3 1px;
                width: 80px; 
                height: 80px;
            }
            .ImgDiv a .showImg{
                width: 80px; 
                height: 80px;
            }
            .ImgDiv a .addImg{
                position:absolute;
                top:32px;
                left:32px;
                width: 16px; 
                height: 16px;
            }
            .ImgDiv a .fileImg,.ImgDiv a .addFileImg{
                position:absolute;
                top:0px;
                left:0px;
                width: 80px; 
                height: 80px;
                opacity:0;
                z-index:1;
            }
            .ImgDiv a .delImg{
                position:absolute;
                top:-5px;
                right:-5px;
                width: 16px; 
                height: 16px;
                z-index:2;
            }
           .dlgUpdateMemberInfo .dialog-content{
               overflow-x:hidden;
           }           .textWidth{
               width:300px;
               border-radius: 0px;
           }             .combo-arrow-hover,.combo-arrow{
                position: relative;
                top: 3px;
                left: -9px;
            }                    .textArea{
                width: 300px;
                border-radius: 0px;
                height: 100px;
           }           .selectStyle{
                height: 30px;
                width: 309px;
           }           .inputKeyword{
                width: 400px; 
                display: inline-block;
                padding: 6px;
           }           .selectList{
                width: 100px; 
                padding: 6px;
                display: inline-block;
           }
           #dlgHouseRecommend input,#dlgCompanyBranchApply input,#dlgCompanyBranchRecommend input,#dlgHouseAppointment input,#dlgHouseBuyerRecommend input {
                padding:3px;
           }
           #dlgHouseRecommend select,#dlgCompanyBranchApply select,#dlgCompanyBranchRecommend select,#dlgHouseAppointment select,#dlgHouseBuyerRecommend select{
               padding:3px;
               border-radius:0px;
           }
           .district{
               width:99px;
               display:inline;
               height:30px;
           }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    
    <%
        if (string.IsNullOrEmpty(moduleType))
        {
    %>
    当前位置：分销&nbsp;&gt;&nbsp;
    <span>移动商机</span>
    <%
        }
        else
        {
    %>
    当前位置：&nbsp;&gt;&nbsp;
    <span><%=moduleName %></span>
    <%
        }    
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <%
                if (!string.IsNullOrEmpty(Request["userId"]))
                {
                    ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
                    ZentCloud.BLLJIMP.BLLDistributionOffLine bllDis = new ZentCloud.BLLJIMP.BLLDistributionOffLine();
                    System.Text.StringBuilder sbTable = new StringBuilder();

                    ZentCloud.BLLJIMP.Model.UserInfo userInfo = bllUser.GetUserInfo(Request["userId"]);


                    sbTable.Append("<table>");
                    sbTable.AppendFormat("<tr>");

                    sbTable.AppendFormat("<td>姓名:{0}<br/>等级:{1}</td>", userInfo.TrueName, bllDis.GetUserLevel(userInfo).LevelString);
                    sbTable.AppendFormat("<td style='width: 30px;'></td>");
                    sbTable.AppendFormat("<td>直销商机数:{0}<br/>直销佣金:{1}</td>", bllDis.GetDirectSaleCount(userInfo.UserID), bllDis.GetCommissionAmount(userInfo.UserID, 0));
                    sbTable.AppendFormat("<td style='width: 30px;'></td>");
                    sbTable.AppendFormat("<td>一级分销人数:{0}<br/>一级分销佣金:{1}</td>", bllDis.GetDownUserCount(userInfo.UserID, 1), bllDis.GetCommissionAmount(userInfo.UserID, 1));
                    sbTable.AppendFormat("<td style='width: 30px;'></td>");
                    sbTable.AppendFormat("<td>二级分销人数:{0}<br/>二级分销佣金:{1}</td>", bllDis.GetDownUserCount(userInfo.UserID, 2), bllDis.GetCommissionAmount(userInfo.UserID, 2));
                    sbTable.AppendFormat("<td style='width: 30px;'></td>");
                    sbTable.AppendFormat("<td>三级分销人数:{0}<br/>三级分销佣金:{1}</td>", bllDis.GetDownUserCount(userInfo.UserID, 3), bllDis.GetCommissionAmount(userInfo.UserID, 3));

                    sbTable.AppendFormat("</tr>");
                    sbTable.Append("</table>");
                    Response.Write(sbTable);
                }
            %>



            <%-- <a  href="javascript:top.addTab('分销员申请字段管理','/App/Cation/ActivitySignUpTableManage.aspx?ActivityID=<%=ActivityID %>')" 
                            class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">分销员申请字段管理
                        </a>--%>




            <%
                if (string.IsNullOrEmpty(moduleType))
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:top.addTab('商机字段管理','/Admin/DistributionOffLine/ProjectFieldMap.aspx')"
                class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">商机字段管理
            </a>
            <a href="javascript:top.addTab('商机状态管理','/Admin/DistributionOffLine/ProjectStatusMgr.aspx')"
                class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">商机状态管理
            </a>
            <a href="javascript:;" onclick="ShowQRcode()" style="color: blue;">获取我的的商机微信链接</a>&nbsp;&nbsp;
                    <%
                }
                else
                {
                    %>
           <%-- <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAddHouse();" id="btnHouseAdd">添加</a>--%>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="ShowEditHouse();" id="btnHouseEdit">编辑</a>
            <%
                }
            %>



            <div>
                <%
                    if (moduleType != "CompanyBranchApply"&&moduleType != "CompanyBranchRecommend")
                    {                                                      
                        %>
                          <span style="font-size: 12px; font-weight: normal"><%=moduleName %>状态：</span>
                            <select id="ddlStatusSearch" class="selectList">
                                <%
                                    Response.Write(string.Format("<option value=\"\">全部</option>"));
                                    foreach (var item in statusList)
                                    {
                                        Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item.OrderStatu));
                                    } %>
                            </select>
                        <%
                    }
                %>
             
                <span style="font-size: 12px; font-weight: normal">关键字：</span>
                <input type="text" placeholder="可按<%=tipMsg%>搜索" class="inputKeyword" id="txtKeyWord" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    

    <%
        if (moduleType == "HouseRecommend")
        {
            %>
                <div id="dlgHouseRecommend" class="easyui-dialog" closed="true" title="操作" style="width: 500px; padding: 15px; line-height: 30px;height: 500px;overflow-y: auto;">
        <table width="100%">
            <tr>
                <td>楼盘名称:
                </td>
                <td>
                    <input id="txtEx1" type="text" class="textWidth form-control" />
                </td>
            </tr>
            <tr>
                <td>楼盘所在城市:
                </td>
                  <td>
                        <select id="selectProvince"  class="form-control district">
                        </select>
                        <select id="selectCity"  class="form-control district">
                        </select>
                        <select id="selectArea"  class="form-control district">
                        </select>
                    </td>
            </tr>
            <tr>
                <td><%=moduleName %>状态:
                </td>
                <td>
                    <select id="selStatus" class="form-control selectStyle">
                        <%foreach (var item in statusList)
                          {
                              Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item.OrderStatu));
                          } %>
                    </select>
                </td>
            </tr>
            <tr>
                <td>提交者姓名:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtEx8" />

                </td>
            </tr>
            <tr>
                <td>提交者联系方式:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtEx9" />

                </td>
            </tr>
            <tr>
                <td>开发商联系人:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtContack" />

                </td>
            </tr>
            <tr>
                <td>开发商联系方式:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtPhone" />

                </td>
            </tr>
            <tr>
                <td>征信报告:
                </td>
                <td>
                    <label>
                        <input type="radio"  id="hasZhenxin" name="zhenxin"  value="1"  class="positionTop3"/>
                        有
                    </label>
                    <label>
                        <input type="radio" id="noZhenxin" name="zhenxin"  value="0" class="positionTop3" />
                        无
                    </label>
                </td>
            </tr>
            <tr>
                <td>抵押:
                </td>
                <td>
                    <label>
                        <input type="radio"  id="hasDiya" name="diya" class="positionTop3" value="1" />
                        有
                    </label>
                    <label>
                        <input type="radio" id="noDiya" name="diya" class="positionTop3" value="0" />
                        无
                    </label>
                </td>
            </tr>
            <tr>
                <td>诉讼:
                </td>
                <td>
                    <label>
                        <input type="radio"  id="hasSusong" name="susong" class="positionTop3" value="1" />
                        有
                    </label>

                    <label>
                        <input type="radio" id="noSusong" name="susong" class="positionTop3" value="0" />
                        无
                    </label>

                </td>
            </tr>
            <tr>
                <td>房型:
                </td>
                <td>
                    <label>
                        <input type="radio" id="rNewHouse" name="roomType" class="positionTop3" value="NewHouse" />
                        新房
                    </label>

                    <label>
                        <input type="radio" id="rSecondHandHouse" name="roomType" class="positionTop3" value="SecondHandHouse" />
                        二手房
                    </label>

                </td>
            </tr>
            <tr>
                <td>项目概括:
                </td>
                <td>
                    <textarea id="txtProjectInfo" class="form-control textArea"></textarea>
                </td>
            </tr>
            <tr>
                <td>:
                </td>
                <td>
                    <label>
                        <input type="radio"  id="shangYe" name="projectinfo" class="positionTop3" value="商业" />
                        商业
                    </label>
                    <label>
                        <input type="radio" id="zhuZhai" name="projectinfo" class="positionTop3" value="住宅" />
                        住宅
                    </label>
                    <label>
                        <input type="radio" id="shangZhu" name="projectinfo" class="positionTop3" value="商住两用" />
                        商住两用
                    </label>
                </td>

            </tr>
        </table>
    </div>
            <%
        }
        else if (moduleType == "CompanyBranchApply")
        {
            %>
                <div id="dlgCompanyBranchApply" class="easyui-dialog" closed="true" title="操作" style="width: 450px; padding: 15px; line-height: 30px;height: 240px;overflow-y: auto;">
        <table width="100%">
            <tr>
                <td>分公司所在城市:
                </td>
                <td>
                     <select id="selectProvince"  class="form-control district">
                        </select>
                        <select id="selectCity"  class="form-control district">
                        </select>
                        <select id="selectArea"  class="form-control district">
                        </select>
                </td>
            </tr>
            <tr>
                <td>提交者姓名:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtContack" />

                </td>
            </tr>
            <tr>
                <td>提交者联系方式:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtPhone" />

                </td>
            </tr>
        </table>
    </div>
            <%
        }
        else if (moduleType == "CompanyBranchRecommend")
        {
            %>
                 <div id="dlgCompanyBranchRecommend" class="easyui-dialog" closed="true" title="操作" style="width: 450px; padding: 15px; line-height: 30px;height: 240px;overflow-y: auto;">
        <table width="100%">
            <tr>
                <td>分公司所在城市:
                </td>
                <td>
                    <select id="selectProvince"  class="form-control district">
                        </select>
                        <select id="selectCity"  class="form-control district">
                        </select>
                        <select id="selectArea"  class="form-control district">
                        </select>
                </td>
            </tr>
            <tr>
                <td>提交者姓名:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtContack" />

                </td>
            </tr>
            <tr>
                <td>提交者联系方式:
                </td>
                <td>

                    <input type="text" class="form-control textWidth" id="txtPhone" />

                </td>
            </tr>
        </table>
    </div>
            <%
        }
        else if (moduleType == "HouseAppointment")
        {
            %>
                <div id="dlgHouseAppointment" class="easyui-dialog" closed="true" title="操作" style="width: 480px; padding: 15px; line-height: 30px;height: 350px;overflow-y: auto;">
                    <table width="100%">
                        <tr>
                            <td style="width:23%;">看房时间:
                            </td>
                            <td>
                                <input id="txtEx3" type="text"  class="easyui-datebox form-control textWidth" />
                            </td>
                        </tr>
                        <tr>
                            <td>看房人姓名:
                            </td>
                            <td>

                                <input type="text" id="txtContack" class="form-control textWidth" />

                            </td>
                        </tr>
                        <tr>
                            <td>联系方式:
                            </td>
                            <td>

                                <input type="text" id="txtPhone" class="form-control textWidth"/>

                            </td>
                        </tr>
                        <tr>
                              <td><%=moduleName %>状态:
                </td>
                <td>
               

                    <select id="selStatus" class="form-control selectStyle">
                        <%foreach (var item in statusList)
                          {
                              Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item.OrderStatu));
                          } %>
                    </select>
                </td>
                        </tr>
                              <tr>
                                <td>备注信息:
                                </td>
                                <td>
                                    <textarea id="txtProRemark" class="textArea form-control"></textarea>
                                </td>
                            </tr>
                    </table>
                </div>
            <%
        }
        else if (moduleType == "HouseBuyerRecommend")
        {
              %>
                <div id="dlgHouseBuyerRecommend" class="easyui-dialog" closed="true" title="操作" style="width: 480px; padding: 15px; line-height: 30px;height: 400px;overflow-y: auto;">
                    <table width="100%">
                        <tr>
                            <td style="width:23%">看房人姓名:
                            </td>
                            <td>

                                <input type="text" id="txtContack" class="form-control textWidth" />

                            </td>
                        </tr>
                        <tr>
                            <td>联系方式:
                            </td>
                            <td>

                                <input type="text" id="txtPhone" class="form-control textWidth"/>

                            </td>
                        </tr>
                        <tr>
                              <td><%=moduleName %>状态:
                </td>
                <td>
                    <select id="selStatus" class="form-control selectStyle">
                        <%foreach (var item in statusList)
                          {
                              Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item.OrderStatu));
                          } %>
                    </select>
                </td>
                        </tr>
                              <tr>
                                <td>备注信息:
                                </td>
                                <td>
                                    <textarea id="txtRemarkInfo" class="textArea form-control"></textarea>
                                </td>
                            </tr>
                    </table>
                </div>
            <%
        }
        else
        {
            %>
                <div id="dlgInput" class="easyui-dialog" closed="true" title="操作" style="width: 400px; padding: 15px; line-height: 30px;">
                    <table width="100%">
                        <tr>
                            <td>商机名称:
                            </td>
                            <td>
                                <input id="txtProjectName" type="text" style="width: 250px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>商机金额(元):
                            </td>
                            <td>
                                <input id="txtAmount" type="text" style="width: 250px; font-weight: bold; color: Red; font-size: 25px;"
                                    onkeyup="this.value=this.value.replace(/\D/g,'')" />
                            </td>
                        </tr>
                        <tr>
                            <td>商机状态:
                            </td>
                            <td>
                                <select id="ddlStatus">
                                    <%foreach (var item in statusList)
                                      {
                                          Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item.OrderStatu));
                                      } %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>分销员:
                            </td>
                            <td>

                                <input type="text" id="txtTrueName" value="" onclick="ShowDlgUser()" />

                            </td>
                        </tr>
                        <tr>
                            <td>商机介绍:
                            </td>
                            <td>
                                <textarea id="txtProjectIntro" style="width: 250px; height: 200px;"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>备注(审核不通过原因在此填写):
                            </td>
                            <td>
                                <input id="txtRemark" type="text" style="width: 250px;" />
                            </td>
                        </tr>
                    </table>
                </div>
            <%
        }
   %>

    
    
    

    <div id="dlgHousesInfo" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'楼盘详情',width:600,height:400,modal:true,buttons:'#dlgMemberInfoButtons'"
        style="padding: 15px 10px 10px 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:18%;">楼盘名称：</td>
                <td colspan="3">
                    <span class="housesName member-label"></span>
                </td>
            </tr>
            <tr>
                <td>提交者姓名：</td>
                <td>
                    <span class="ex8 member-label"></span>
                </td>
                <td>提交者手机：</td>
                <td>
                    <span class="ex9 member-label"></span>
                </td>
            </tr>
            <tr class="trUpUser">
                <td>开发商联系人：</td>
                <td  class="memberUpUser">
                    <span class="contact member-label"></span>
                </td>
                <td>开发商联系方式：</td>
                <td class="memberRegUser">
                    <span class="phone member-label"></span>
                </td>
            </tr>
            <tr class="trRegUser">
                <td>所在城市：</td>
                <td colspan="3">
                    <span class="ex4 member-label"></span>
                </td>
            </tr>
             <tr>
                <td>房型：</td>
                <td class="memberLevel">
                    <span class="ex14 member-label"></span>
                </td>
                <td>土地用途：</td>
                <td class="memberApplyStarttime">
                    <span class="ex13 member-label"></span>
                </td>
            </tr>

            <tr>
                <td>征信报告：</td>
                <td colspan="3" class="memberLevel">
                    <span class="ex10 member-label"></span>
                </td>
            </tr>
            <tr>
                
                <td>抵押：</td>
                <td colspan="3" class="memberStatus">
                    <span class="ex11 member-label"></span>
                </td>
            </tr>
            <tr>
                <td>诉讼：</td>
                <td colspan="3" class="memberRegtime">
                    <span class="ex12 member-label"></span>
                </td>
               
            </tr>
          
            <tr>
                <td>审核状态：</td>
                <td>
                    <span class="project_status member-label"></span>
                </td>
                <td>提交时间：</td>
                <td class="memberApplyStarttime">
                    <span class="project_time member-label"></span>
                </td>
            </tr>
            <tr>
                <td>项目概况：</td>
                <td colspan="3">
                    <span class="project_introduction member-label">

                    </span>
                </td>
            </tr>
        </table>
    </div>

    <div id="dlgUser" class="easyui-dialog" closed="true" title="设置分销员" style="width: 600px; padding: 15px;">
        姓名:
        <input type="text" id="txtTrueNameKeyWord" style="width: 200px;" placeholder="姓名" />
        <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchMember()">查询</a>
        <table id="grvUserData" fitcolumns="true">
        </table>
    </div>

    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script src="http://static-files.socialcrmyun.com/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var baseHandlerUrl = "Handler/Project/";
        var currSelectID = 0;
        var currAction = '';
        var userId = "<%=Request["userId"] %>";
        var currentSelectUserId = "";
        var mType = '<%=moduleType%>';
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        $(function () {

            areaBind();
        
            //商机列表
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: baseHandlerUrl + "List.ashx",
                       queryParams: { userId: userId, module_type: mType },
                       height: document.documentElement.clientHeight - 50,
                       loadFilter: pagerfilter,
                       pagination: true,
                       striped: true,
                       singleSelect: true,
                       rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },

                                   <%
                                        if (string.IsNullOrEmpty(moduleType))
                                        {
                                             %>
                                             { field: 'ProjectId', title: '商机编号', width: 10, align: 'left' },

                                               {
                                                   field: 'Amount', title: '金额(元)', width: 15, align: 'left', formatter: function (value) {
                                                       return "<font color='Red'>" + value + "</font>";
                                                   }
                                               },


                                               {
                                                   field: 'UserInfo0', title: '分销员编号', width: 10, align: 'left', formatter: function (value, row) {
                                                       if (row.UserInfo != null) {
                                                           return row.UserInfo.AutoID;
                                                       }

                                                   }
                                               },
                                               {
                                                   field: 'UserInfo', title: '分销员姓名', width: 10, align: 'left', formatter: function (value, row) {
                                                       if (row.UserInfo != null) {
                                                           return row.UserInfo.TrueName;
                                                       }
                                                   }
                                               },
                                        <%
                                        }
                                    %>







                                   <%
                                        if (moduleType == "CompanyBranchApply" || moduleType == "CompanyBranchRecommend" || moduleType == "HouseAppointment" || moduleType == "HouseBuyerRecommend")
                                        {
                                        %>
                                             { field: 'Contact', title: '姓名', width: 10, align: 'left' },

                                            { field: 'Phone', title: '联系方式', width: 10, align: 'left' },
                                        <%
                                        }

                                        if (moduleType == "HouseAppointment" || moduleType == "HouseBuyerRecommend")
                                        {
                                        %>
                                             { field: 'Introduction', title: '备注信息', width: 10, align: 'left' },

                                        <%
                                        }
                                    %>


                                   { field: 'Status', title: '<%=moduleName%>状态', width: 10, align: 'left' },

                                   { field: 'InsertDate', title: '时间', width: 18, align: 'left', formatter: FormatDate },
                               <%=Columns %>

                               <%
                                 if (moduleType == "HouseRecommend")
                                {
                                    %>
                                     , {
                                         field: 'caozuo', title: '操作', width: 8, align: 'center', formatter: function (value, rowData, rowIndex) {
                                             var str = new StringBuilder();
                                             str.AppendFormat('<a href="javascript:OpenShow({0})"><img alt="详情" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/list.png" title="详情" /></a>', rowIndex);
                                             return str.ToString();
                                         }
                                     }
                                     <%
                                }
                                %>
                                  


                       ]]
                   }
            );




            //商机框
            $('#dlgInput').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                projectName: $.trim($('#txtProjectName').val()),
                                projectIntro: $.trim($('#txtProjectIntro').val()),
                                userId: currentSelectUserId,
                                amount: $("#txtAmount").val(),
                                status: $("#ddlStatus").val(),
                                Remark: $("#txtRemark").val()
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }
                            if (dataModel.projectName == "") {
                                layerAlert("请输入商机名称");
                                return false;
                            }
                            if (dataModel.amount == "") {
                                layerAlert("请输入商机金额");
                                return false;
                            }
                            if (dataModel.userId == "") {
                                layerAlert("请选择分销员");
                                return false;
                            }

                            var commissionStatus = "<%=CommissionStatus%>";
                            if (dataModel.status == commissionStatus) {
                                $.messager.confirm("系统提示", "确定将商机状态变成" + commissionStatus + "?<br/>确定后将给相关人员分发佣金!", function (r) {
                                    if (r) {

                                        $.ajax({
                                            type: 'post',
                                            url: operaHandler,
                                            data: dataModel,
                                            dataType: "json",
                                            success: function (resp) {
                                                if (resp.status == true) {
                                                    layerAlert("操作成功");
                                                    $('#dlgInput').dialog('close');
                                                    $('#grvData').datagrid('reload');
                                                }
                                                else {
                                                    layerAlert(resp.msg);
                                                }
                                            }
                                        });
                                    }
                                });

                            }
                            else {

                                $.ajax({
                                    type: 'post',
                                    url: operaHandler,
                                    data: dataModel,
                                    dataType: "json",
                                    success: function (resp) {
                                        if (resp.status == true) {
                                            layerAlert("操作成功");
                                            $('#dlgInput').dialog('close');
                                            $('#grvData').datagrid('reload');
                                        }
                                        else {
                                            layerAlert(resp.msg);
                                        }
                                    }
                                });
                            }
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });

            //楼盘框(楼盘推荐)
            $('#dlgHouseRecommend').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                ex1: $.trim($('#txtEx1').val()),
                                ex2: $('#selectProvince option:selected').text(),
                                ex3: $('#selectProvince').val(),
                                ex4: $('#selectCity option:selected').text(),
                                ex5: $('#selectCity').val(),
                                ex6: $('#selectArea option:selected').text(),
                                ex7: $('#selectArea').val(),
                                ex8: $.trim($('#txtEx8').val()),
                                ex9: $.trim($('#txtEx9').val()),
                                ex10: $('[name=zhenxin]:checked').val(),
                                ex11: $('[name=diya]:checked').val(),
                                ex12: $('[name=susong]:checked').val(),
                                ex13: $('[name=projectinfo]:checked').val(),
                                ex14:$('[name=roomType]:checked').val(),
                                projectIntro: $.trim($('#txtProjectInfo').val()),
                                contack: $("#txtContack").val(),
                                phone:$('#txtPhone').val(),
                                status: $("#selStatus").val(),
                                projectName:'<%=moduleName%>',
                                type: mType
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }
                            if (!dataModel.ex1) {
                                layerAlert('请输入楼盘名称');
                                return false;
                            }


                            if (!dataModel.ex3) {
                                layerAlert("请选择省份");
                                return false;
                            }
                            if (!dataModel.ex5) {
                                layerAlert("请选择城市");
                                return false;
                            }
                            if (!dataModel.ex7) {
                                layerAlert("请选择地区");
                                return false;
                            }
                            if (!dataModel.ex8) {
                                layerAlert("请输入提交者姓名");
                                return false;
                            }
                            if (!dataModel.ex9) {
                                layerAlert("请输入提交者联系方式");
                                return false;
                            }

                            if (!dataModel.contack) {
                                layerAlert("请输入开发商联系人");
                                return false;
                            }
                            if (!dataModel.phone) {
                                layerAlert("请输入开发商联系方式");
                                return false;
                            }
                            if (!dataModel.ex10) {
                                layerAlert("请选择征信报告情况");
                                return false;
                            }
                            if (!dataModel.ex11) {
                                layerAlert("请选择抵押情况");
                                return false;
                            }
                            if (!dataModel.ex12) {
                                layerAlert("请选择诉讼情况");
                                return false;
                            }
                            if (!dataModel.ex14) {
                                layerAlert("请选择房型");
                                return false;
                            }
                            if (!dataModel.projectIntro) {
                                layerAlert("请输入项目概况");
                                return false;
                            }
                            if (!dataModel.ex13) {
                                layerAlert("请选择土地用途");
                                return false;
                            }
                            $.ajax({
                                type: 'post',
                                url: operaHandler,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        layerAlert("操作成功");
                                        $('#dlgHouseRecommend').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        layerAlert(resp.msg);
                                    }
                                }
                            });
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgHouseRecommend').dialog('close');
                    }
                }]
            });
            //楼盘框(分公司申请)
            $('#dlgCompanyBranchApply').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                ex1: $('#selectProvince option:selected').text(),
                                ex2: $('#selectProvince').val(),
                                ex3: $('#selectCity option:selected').text(),
                                ex4: $('#selectCity').val(),
                                ex5: $('#selectArea option:selected').text(),
                                ex6: $('#selectArea').val(),
                                contack: $("#txtContack").val(),
                                phone: $('#txtPhone').val(),
                                status:'待审核',
                                projectName: '<%=moduleName%>',
                                type: mType
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }
                            if (!dataModel.ex2) {
                                layerAlert("请选择省份");
                                return false;
                            }
                            if (!dataModel.ex4) {
                                layerAlert("请选择城市");
                                return false;
                            }
                            if (!dataModel.ex6) {
                                layerAlert("请选择地区");
                                return false;
                            }
                            if (!dataModel.contack) {
                                layerAlert("请输入提交者姓名");
                                return false;
                            }
                            if (!dataModel.phone) {
                                layerAlert("请输入提交者联系方式");
                                return false;
                            }
                            $.ajax({
                                type: 'post',
                                url: operaHandler,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        layerAlert("操作成功");
                                        $('#dlgCompanyBranchApply').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        layerAlert(resp.msg);
                                    }
                                }
                            });
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgCompanyBranchApply').dialog('close');
                    }
                }]
            });
            //楼盘框(分公司推荐)
            $('#dlgCompanyBranchRecommend').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                ex1: $('#selectProvince option:selected').text(),
                                ex2: $('#selectProvince').val(),
                                ex3: $('#selectCity option:selected').text(),
                                ex4: $('#selectCity').val(),
                                ex5: $('#selectArea option:selected').text(),
                                ex6: $('#selectArea').val(),
                                contack: $("#txtContack").val(),
                                phone: $('#txtPhone').val(),
                                status: '待审核',
                                projectName: '<%=moduleName%>',
                                type: mType
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }

                            if (!dataModel.ex2) {
                                layerAlert("请选择省份");
                                return false;
                            }
                            if (!dataModel.ex4) {
                                layerAlert("请选择城市");
                                return false;
                            }
                            if (!dataModel.ex6) {
                                layerAlert("请选择地区");
                                return false;
                            }
                            if (!dataModel.contack) {
                                layerAlert("请输入提交者姓名");
                                return false;
                            }
                            if (!dataModel.phone) {
                                layerAlert("请输入提交者联系方式");
                                return false;
                            }
                            $.ajax({
                                type: 'post',
                                url: operaHandler,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        layerAlert("操作成功");
                                        $('#dlgCompanyBranchRecommend').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        layerAlert(resp.msg);
                                    }
                                }
                            });
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgCompanyBranchRecommend').dialog('close');
                    }
                }]
            });
            //楼盘框(预约看房)
            $('#dlgHouseAppointment').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                ex3: $('#txtEx3').datebox('getValue'),
                                contack: $("#txtContack").val(),
                                phone: $('#txtPhone').val(),
                                status: $(selStatus).val(),
                                projectName: '<%=moduleName%>',
                                projectIntro: $('#txtProRemark').val(),
                                type: mType
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }

                            if (!dataModel.ex3) {
                                layerAlert("请输入看房时间");
                                return false;
                            }
                            if (!dataModel.contack) {
                                layerAlert("请输入姓名");
                                return false;
                            }
                            if (!dataModel.phone) {
                                layerAlert("请输入联系方式");
                                return false;
                            }
                            if (!dataModel.projectIntro) {
                                layerAlert("请输入备注信息");
                                return false;
                            }

                            $.ajax({
                                type: 'post',
                                url: operaHandler,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        layerAlert("操作成功");
                                        $('#dlgHouseAppointment').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        layerAlert(resp.msg);
                                    }
                                }
                            });
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgHouseAppointment').dialog('close');
                    }
                }]
            });
            //楼盘框(推荐购房顾客)
            $('#dlgHouseBuyerRecommend').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                ProjectId: currSelectID,
                                contack: $("#txtContack").val(),
                                phone: $('#txtPhone').val(),
                                status: $(selStatus).val(),
                                projectName: '<%=moduleName%>',
                                projectIntro: $('#txtRemarkInfo').val(),
                                type: mType
                            }
                            var operaHandler = baseHandlerUrl;
                            if (currAction == "Add") {
                                operaHandler += "Add.ashx";
                            }
                            else {
                                operaHandler += "Update.ashx";
                            }

                            if (!dataModel.contack) {
                                layerAlert("请输入姓名");
                                return false;
                            }
                            if (!dataModel.phone) {
                                layerAlert("请输入联系方式");
                                return false;
                            }
                            if (!dataModel.projectIntro) {
                                layerAlert("请输入备注信息");
                                return false;
                            }

                            $.ajax({
                                type: 'post',
                                url: operaHandler,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        layerAlert("操作成功");
                                        $('#dlgHouseBuyerRecommend').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        layerAlert(resp.msg);
                                    }
                                }
                            });
                        } catch (e) {
                            layerAlert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgHouseBuyerRecommend').dialog('close');
                    }
                }]
            });
            //商机搜索
            $("#btnSearch").click(function () {
                var keyWord = $("#txtKeyWord").val();
                var option = {
                    keyWord: keyWord,
                    userId: userId,
                    module_type: mType
                };
                if (mType == '' || mType == 'HouseRecommend' || mType == 'HouseBuyerRecommend' || mType == 'HouseAppointment') {
                    
                    option.status= $(ddlStatusSearch).val();
                }
                $('#grvData').datagrid({ url: baseHandlerUrl + "List.ashx", queryParams: option });
            });

            //分销员
            $('#grvUserData').datagrid(
                  {
                      method: "Post",
                      url: "/Handler/App/CationHandler.ashx",
                      queryParams: { Action: "QueryWebsiteUserDistributionOnLine" },
                      height: 400,
                      pagination: true,
                      striped: true,
                      pageSize: 20,
                      rownumbers: true,
                      singleSelect: true,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[

                                  {
                                      field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                          if (value == '' || value == null)
                                              return "";
                                          var str = new StringBuilder();
                                          str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                          return str.ToString();
                                      }
                                  },
                                  { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle }
                      ]],
                      onDblClickRow: function (rowIndex, rows) {
                          $(txtTrueName).val(rows.TrueName);
                          currentSelectUserId = rows.UserID;
                          $('#dlgUser').dialog('close');
                      }
                  });

            //设置分销员
            $('#dlgUser').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $("#grvUserData").datagrid('getSelections');
                        if (!EGCheckIsSelect(rows))
                            return;
                        if (!EGCheckNoSelectMultiRow(rows))
                            return;
                        $(txtTrueName).val(rows[0].TrueName);
                        currentSelectUserId = rows[0].UserID;
                        $('#dlgUser').dialog('close');

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgUser').dialog('close');
                    }
                }]
            });

            $('#dlgHousesInfo').dialog({
                buttons: [{
                    text: '关闭',
                    handler: function () {
                        $('#dlgHousesInfo').dialog('close');
                    }
                }]
            });


        });


        function pagerfilter(data) {
            if (!mType) return data;
            if (mType == 'HouseRecommend') {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].Ex14 == "NewHouse") data.rows[i].Ex14 = "新房";
                    if(data.rows[i].Ex14 == "SecondHandHouse")  data.rows[i].Ex14 = "二手房";
                    if (data.rows[i].Ex10 == 0) {
                        data.rows[i].Ex10 = '无';
                    } else {
                        data.rows[i].Ex10 = '有';
                    }
                    if (data.rows[i].Ex11 == 0) {
                        data.rows[i].Ex11 = '无';
                    } else {
                        data.rows[i].Ex11 = '有';
                    }
                    if (data.rows[i].Ex12 == 0) {
                        data.rows[i].Ex12 = '无';
                    } else {
                        data.rows[i].Ex12 = '有';
                    }
                }
            }
            return data;
        }

        function OpenShow(rowIndex) {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[rowIndex];
            $('#dlgHousesInfo').dialog('open');
            $.ajax({
                type: 'POST',
                url: baseHandlerUrl + 'Get.ashx',
                data: { project_id: row.ProjectId },
                dataType: 'json',
                success: function (data) {
                    SetDialogHousesInfo(data);
                    console.log(data);
                }
            });
        }
        function SetDialogHousesInfo(data) {
            if (!data) return;
            $('.housesName').text(data.ex1);
            $('.ex8').text(data.ex8);
            $('.ex9').text(data.ex9);
            $('.contact').text(data.contact);
            $('.phone').text(data.phone);
            var dis = "";
            if (data.ex2) dis += data.ex2;
            if (data.ex4) dis += "" + data.ex4;
            if (data.ex6) dis += "" + data.ex6;
            $('.ex4').text(dis);
            $('.project_status').text(data.project_status);
            if (data.project_time) $('.project_time').text(FormatDate(data.project_time));
            $('.project_introduction').text(data.project_introduction);
            if (data.ex10 == 1) {
                $('.ex10').text('有');
            } else {
                $('.ex10').text('无');
            }
            if (data.ex11 == 1) {
                $('.ex11').text('有');
            } else {
                $('.ex11').text('无');
            }
            if (data.ex12 == 1) {
                $('.ex12').text('有');
            } else {
                $('.ex12').text('无');
            }
            $('.ex13').text(data.ex13);
            
            if (data.ex14 == 'NewHouse') {
                $('.ex14').text('新房');
            } else {
                $('.ex14').text('二手房');
            }
        }
        //添加商机弹框
        function ShowAdd() {
            currAction = 'Add';
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");

            currentSelectUserId = "";
        }
        function ShowAddHouse() {
            currAction = 'Add';
            $('#dlg' + mType + '').dialog({ title: '添加' });
            $('#dlg' + mType + ' input[type=text]').val('');
            $("input[type='radio']").removeAttr('checked');
            $('#dlg' + mType + ' textarea').val('');
            $('#dlg' + mType + '').dialog('open');
        }
        //编辑商机弹框
        function ShowEdit() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'Update';
            currSelectID = rows[0].ProjectId;
            $("#txtProjectName").val($.trim(rows[0].ProjectName));
            $('#txtProjectIntro').val(rows[0].Introduction);
            $('#txtAmount').val(rows[0].Amount);
            $('#ddlStatus').val(rows[0].Status);
            $('#txtRemark').val(rows[0].Remark);
            currentSelectUserId = rows[0].UserId;
            $(txtTrueName).val(rows[0].UserInfo.TrueName);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }
        //编辑  楼盘推荐
        function ShowEditHouse() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'Update';
            currSelectID = rows[0].ProjectId;


            if (mType == "HouseRecommend") {
                $("#txtEx1").val(rows[0].Ex1);

                if (rows[0].Ex3) {
                    setCitySelect(rows[0].Ex3);
                    $('#selectProvince').val(rows[0].Ex3);
                }
                if (rows[0].Ex5) {
                    setAreaSelect(rows[0].Ex5);
                    $('#selectCity').val(rows[0].Ex5);
                }
                $('#selectArea').val(rows[0].Ex7);


                $("#selStatus").val(rows[0].Status);
                $("#txtEx8").val(rows[0].Ex8);
                $("#txtEx9").val(rows[0].Ex9);

                $("#txtContack").val(rows[0].Contact);
                $("#txtPhone").val($.trim(rows[0].Phone));
                $('#txtProjectInfo').val(rows[0].Introduction);
                var zhenxin = rows[0].Ex10;
                var diya = rows[0].Ex11;
                var susong = rows[0].Ex12;
                var rType = rows[0].Ex13;
                var roomType = rows[0].Ex14;

                if (zhenxin == '有') {
                    $('#hasZhenxin').attr('checked', 'checked');
                } else {
                    $('#noZhenxin').attr('checked', 'checked');
                }
                if (diya == '有') {
                    $('#hasDiya').attr('checked', 'checked');
                } else {
                    $('#noDiya').attr('checked', 'checked');
                }
                if (susong == '有') {
                    $('#hasSusong').attr('checked', 'checked');
                } else {
                    $('#noSusong').attr('checked', 'checked');
                }
                if (rType == '商业') {
                    $('#shangYe').attr('checked', 'checked');
                } else if (rType == '住宅') {
                    $('#zhuZhai').attr('checked', 'checked');
                } else {
                    $('#shangZhu').attr('checked', 'checked');
                }
                if (roomType == '新房') {
                    $('#rNewHouse').attr('checked', 'checked');
                } else {
                    $('#rSecondHandHouse').attr('checked', 'checked');
                }
            } else if (mType == "CompanyBranchApply") {

                if (rows[0].Ex2) {
                    setCitySelect(rows[0].Ex2);
                    $('#selectProvince').val(rows[0].Ex2);
                }
                if (rows[0].Ex4) {
                    setAreaSelect(rows[0].Ex4);
                    $('#selectCity').val(rows[0].Ex4);
                }
                $('#selectArea').val(rows[0].Ex6);

                $("#txtContack").val(rows[0].Contact);
                $("#txtPhone").val($.trim(rows[0].Phone));
            } else if (mType == "CompanyBranchRecommend") {

                if (rows[0].Ex2) {
                    setCitySelect(rows[0].Ex2);
                    $('#selectProvince').val(rows[0].Ex2);
                }
                if (rows[0].Ex4) {
                    setAreaSelect(rows[0].Ex4);
                    $('#selectCity').val(rows[0].Ex4);
                }
                $('#selectArea').val(rows[0].Ex6);
                $("#txtContack").val(rows[0].Contact);
                $("#txtPhone").val(rows[0].Phone);
            } else if (mType == "HouseAppointment") {
                $('#txtEx3').datebox('setValue', rows[0].Ex3);
                $("#txtContack").val(rows[0].Contact);
                $("#txtPhone").val(rows[0].Phone);
                $('#selStatus').val(rows[0].Status);
                $("#txtProRemark").val(rows[0].Introduction);
            } else if (mType == "HouseBuyerRecommend") {
                $("#txtContack").val(rows[0].Contact);
                $("#txtPhone").val(rows[0].Phone);
                $('#selStatus').val(rows[0].Status);
                $("#txtRemarkInfo").val(rows[0].Introduction);
            }
            $('#dlg' + mType + '').dialog({ title: '编辑' });
            $('#dlg' + mType + '').dialog('open');
        }

        //显示选择分销员
        function ShowDlgUser() {
            $('#dlgUser').dialog({ title: '选择分销员,双击或点击确定按钮' });
            $('#dlgUser').dialog('open');
        }

        //搜索分销员
        function SearchMember() {
            $('#grvUserData').datagrid(
                    {
                        method: "Post",
                        url: "/Handler/App/CationHandler.ashx",
                        queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $(txtTrueNameKeyWord).val() },
                    });
        }

        function ShowQRcode() {

            var code = "http://" + domain + "/app/distribution/m/index.aspx?ngroute=/sbinfo#/sbinfo";
            $.ajax({
                type: "Post",
                url: "/serv/api/common/QrCode.ashx",
                data: { code: code },
                dataType: "json",
                success: function (resp) {
                    if (resp.status == true) {
                        $('#imgQrcode').attr('src', resp.result.qrcode_url);

                    }

                }

            });
            $('#dlgSHowQRCode').dialog('open');
            var linkurl = "http://" + domain + "/app/distribution/m/index.aspx?ngroute=/sbinfo#/sbinfo";
            $("#alinkurl").html(linkurl).attr("href", linkurl);
        }

        function areaBind() {

            //初始化处理省份选项
            setProvinceSelect();

            $(document).on('change', '#selectProvince', function () {
                //选择省份
                setCitySelect($(this).val());
            });

            $(document).on('change', '#selectCity', function () {
                //选择城市
                var selectCode = $(this).val();
                setAreaSelect(selectCode);
            });
        }        function setProvinceSelect(selectVal) {

            try {
                var data = zymmp.location.getProvince();
                $(document).find('#selectProvince').html('').append(getAreaSelectOptionDom(data));
                if (selectVal) {
                    $(document).find('#selectProvince').val(selectVal);
                }
            } catch (e) {
                console.log('setProvinceSelect', e);
            }

        }
        function setCitySelect(provinceCode, selectVal) {

            try {
                var selectCode = provinceCode, cityData = zymmp.location.getCity(selectCode);

                $(document).find('#selectCity').html('').append(getAreaSelectOptionDom(cityData));
                $(document).find('#selectArea').html('<option value=""></option>');

                if (selectVal) {
                    $(document).find('#selectCity').val(selectVal);
                }

                if (cityData.length > 0) {
                    $('#selectCity_chosen').css({ visibility: 'visible' });
                    $('#selectArea_chosen').css({ visibility: 'visible' });
                } else {
                    $('#selectCity_chosen').css({ visibility: 'hidden' });
                    $('#selectArea_chosen').css({ visibility: 'hidden' });
                }
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }

        function setAreaSelect(cityCode, selectVal) {

            try {
                var selectCode = cityCode, areaData = zymmp.location.getDistrict(selectCode);
                $(document).find('#selectArea').html('').append(getAreaSelectOptionDom(areaData));
                if (selectVal) {
                    $(document).find('#selectArea').val(selectVal);
                }
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }

        function getAreaSelectOptionDom(data) {
            var strHtml = new StringBuilder();
            strHtml.Append('<option value=""></option>');
            for (var i = 0; i < data.length; i++) {
                strHtml.AppendFormat('<option value="{0}">{1}</option>', data[i].DataKey, data[i].DataValue);
            }
            return strHtml.ToString();
        }

    </script>
</asp:Content>
