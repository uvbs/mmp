<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="DistributionConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.DistributionConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select
        {
            /*height: 30px;*/
            min-width: 200px;
            max-width: 400px;
        }
        .form-control{
            height:auto;
        }
        .sort{
            display:none;
        }
        .centent_r_btm{
            border:0;
        }
        .ActivityBox{
            padding-bottom:100px;
        }
        table tr td{
            padding-bottom:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
      <% ZentCloud.BLLJIMP.Model.WebsiteInfo currentWebsiteInfo = new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModelFromDataBase();

        dynamic websiteMini = new
        {
            DistributionOffLineDescription = currentWebsiteInfo.DistributionOffLineDescription
        };

        string websiteMiniStr = Newtonsoft.Json.JsonConvert.SerializeObject(websiteMini);

         %>
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td align="right" class="tdTitle">
                        分销等级:
                    </td>
                    <td>
                        <select id="ddlDistributionOffLineLevel" class="form-control">
                            <option value="0">只有直销</option>
                            <option value="1">一级分销</option>
                            <option value="2">二级分销</option>
                            <option value="3">三级分销</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        手机端显示分销等级:
                    </td>
                    <td>
                        <select id="ddlDistributionOffLineShowLevel" class="form-control">
                            <option value="1">只显示一级</option>
                            <option value="2">显示一到二级</option>
                            <option value="3">显示一到三级</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        佣金显示名称:
                    </td>
                    <td>
                        <input type="text" class="form-control" style="width:200px" id="txtCommissionShowName" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        分销显示名称:
                    </td>
                    <td>
                        <input type="text" class="form-control" style="width:200px" id="txtDistributionShowName" />
                    </td>
                </tr>
                  <tr>
                    <td align="right" class="tdTitle">
                        系统显示名称:
                    </td>
                    <td>
                        <input type="text" class="form-control" style="width:200px" id="txtSystemShowName" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        是否在前端显示分销比例:
                    </td>
                    <td>
                        <select id="ddlIsShowDistributionOffLineRate" class="form-control">
                            <option value="0">不显示</option>
                            <option value="1">显示</option>
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td align="right" class="tdTitle">
                        是否在前端显示会员积分:
                    </td>
                    <td>
                        <select id="ddlDistributionOffLineIsShowMemberScore" class="form-control">
                            <option value="0">不显示</option>
                            <option value="1">显示</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        幻灯片设置：
                    </td>
                    <td>
                       <select style="width:200px;" id="ddlSlideType">
                           <option value="">无</option>
                          <%
                              if (slides.Count>0)
                              {
                                  foreach (var item in slides)
                                  {
                                       %>
                                        <option value="<%=item %>"><%=item%></option>
                                        <%
                                  }
                              }    
                           %>
                       </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        分销政策:
                    </td>
                    <td>
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 375px; height: 500px;">
                                <%= currentWebsiteInfo.DistributionOffLineDescription %>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        等待审核信息:
                    </td>
                    <td>
                        <div id="divEditorApplyWaitInfo">
                            <div id="txtEditorApplyWaitInfo" style="width: 375px; height: 500px;">
                                <%= currentWebsiteInfo.DistributionOffLineApplyWaitInfo %>
                            </div>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="tdTitle">
                    </td>
                    <td>
                       
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                    </td>
                    <td>
                       
                        <h1>系统推荐码:<%=SysRecommendCode%></h1>
                    </td>
                </tr>

                
                
                       <tr>
                    <td align="left" class="tdTitle">
                        手机端二维码
                    </td>
                    <td> 
                        <img src="/Handler/ImgHandler.ashx?v=http://<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>/app/distribution/m/index.aspx" width="300" height="300">
                        <br />
                        <a href="http://<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>/app/distribution/m/index.aspx" target="_blank" >http://<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>/app/distribution/m/index.aspx</a>
                              
                        
                    </td>
                </tr>
            </table>
        </div>
        <div style="
                    border-top: 1px solid #DDDDDD;
                    position: fixed;
                    bottom: 0px;
                    height: 60px;
                    line-height: 60px;
                    text-align: center;
                    width: 100%;
                    background-color: rgb(245, 245, 245);
              ">
            
                 <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px; text-decoration: underline;"
                            class="button button-rounded button-primary" onclick="Save();">保存</a>
                    <a href="/App/Cation/ActivitySignUpTableManage.aspx?ActivityID=<%=ActivityID %>"
                            class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">分销员申请字段设置
                        </a>
                        <a href="ProjectFieldMap.aspx"
                            class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">项目字段设置
                        </a>
                        <a href="ProjectStatusMgr.aspx"
                            class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">项目状态设置
                        </a>
                        <a href="/user/userlevelconfig.aspx?type=DistributionOffLine"
                            class="easyui-linkbutton l-btn l-btn-plain" iconcls="icon-list" plain="true">分销等级设置
                        </a>
            
          </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <% ZentCloud.BLLJIMP.Model.WebsiteInfo currentWebsiteInfo = new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModelFromDataBase();

        dynamic websiteMini = new
        {
            DistributionOffLineDescription = currentWebsiteInfo.DistributionOffLineDescription
        };

        string websiteMiniStr = Newtonsoft.Json.JsonConvert.SerializeObject(websiteMini);

         %>
    <script type="text/javascript">
        var editor,editorApplyInfo;
        

        KindEditor.ready(function (K) {

            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });

            editorApplyInfo = K.create('#txtEditorApplyWaitInfo', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });

        });

        $(function () {
            $(ddlDistributionOffLineLevel).val("<%=currentWebsiteInfo.DistributionOffLineLevel %>");
            $(ddlDistributionOffLineShowLevel).val("<%=currentWebsiteInfo.DistributionOffLineShowLevel%>");
            $(ddlIsShowDistributionOffLineRate).val("<%=currentWebsiteInfo.IsShowDistributionOffLineRate%>");
            $(txtCommissionShowName).val("<%=currentWebsiteInfo.CommissionShowName%>");
            $(txtSystemShowName).val("<%=currentWebsiteInfo.DistributionOffLineSystemShowName%>");
            $('#ddlSlideType').val("<%=currentWebsiteInfo.DistributionOffLineSlideType%>");
            $('#ddlDistributionOffLineIsShowMemberScore').val("<%=currentWebsiteInfo.DistributionOffLineIsShowMemberScore%>")

            if ($(txtCommissionShowName).val() == "") {
                $(txtCommissionShowName).val("积分");
            }
            $(txtDistributionShowName).val("<%=currentWebsiteInfo.DistributionShowName%>");
            if ($(txtDistributionShowName).val() == "") {
                $(txtDistributionShowName).val("会员");
            }
        })
        function Save() {

            $.ajax({

                type: 'post',
                url: "Handler/Config/Config.ashx",
                data: {
                    DistributionOffLineLevel: $(ddlDistributionOffLineLevel).val(),
                    DistributionOffLineShowLevel: $(ddlDistributionOffLineShowLevel).val(),
                    CommissionShowName: $(txtCommissionShowName).val(),
                    DistributionShowName: $(txtDistributionShowName).val(),
                    IsShowDistributionOffLineRate: $(ddlIsShowDistributionOffLineRate).val(),
                    DistributionOffLineDescription: editor.html(),
                    DistributionOffLineSlideType: $('#ddlSlideType').val(),
                    DistributionOffLineIsShowMemberScore: $('#ddlDistributionOffLineIsShowMemberScore').val(),
                    DistributionOffLineApplyWaitInfo: editorApplyInfo.html(),
                    SystemShowName:$(txtSystemShowName).val()
                },
                success: function (resp) {
                    if (resp.status == true) {
                        layer.msg("保存成功");
                    }
                    else {

                        layer.msg(resp.msg);
                    }

                }
            });


        }


    </script>
</asp:Content>
