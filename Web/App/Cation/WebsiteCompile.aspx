<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WebsiteCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WebsiteCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        input {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        textarea {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        input[type=radio], input[type=checkbox] {
            position: relative;
            top: 8px;
            height: 23px;
            margin: 0px 5px 0px 5px !important;
        }

        .showSetField {
            padding: 0px 10px;
            height: 32px;
            line-height: 32px;
            margin-left: 10px;
        }

        .ActivityBox table {
            border-collapse: separate !important;
            border-spacing: 10px !important;
        }

        .combo-panel {
            padding: 2px !important;
        }

            .combo-panel table {
                border-collapse: separate !important;
            }

        .loginConfig {
            width: 100%;
            border: 1px solid #ddd;
            padding: 5px;
        }

        .bgImg {
            width: 64px;
            height: 64px;
        }

        .Font20 {
            font-size: 20px;
        }
        .clearImg{
            text-decoration:none;
        }
    </style> 
    <link href="/MainStyleV2/css/bootstrap.min.css" rel="stylesheet" />
    <%--<script src="/Plugins/ZeroClipboard/jquery.zclip.min.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>站点管理</span>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=action%>站点</span>
    <a href="WebsiteManage.aspx" style="float: right; margin-right: 20px;" title="返回站点管理"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <fieldset>
            <legend>站点配置</legend>
            <table width="100%" id="tbMain">
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">网站名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtWebsiteName" value="<%=model.WebsiteName %>" style="width: 100%" placeholder="网站名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">站点所有者：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtWebsiteOwner" value="<%=model.WebsiteOwner %>" style="width: 100%" placeholder="站点所有者" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">可建立子帐号数量：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtMaxSubAccountCount" style="width: 100%" value="<%=model.MaxSubAccountCount==0?5:model.MaxSubAccountCount %>" placeholder="可简历子帐号数量" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">日志保存天数：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtLogDay" style="width: 100%" value="<%=model.LogLimitDay==0?7:model.LogLimitDay %>" placeholder="日志保存天数" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">余额支付前端显示名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtAccountAmountPayShowName" value="<%=model.AccountAmountPayShowName %>" style="width: 100%" placeholder="余额支付前端显示名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">佣金前端显示名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtTotalAmountShowName" value="<%=model.TotalAmountShowName %>" style="width: 100%" placeholder="佣金前端显示名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">积分前端显示名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtScorePayShowName" value="<%=model.ScorePayShowName %>" style="width: 100%" placeholder="积分前端显示名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">优惠券前端显示名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtCardCouponShowName" value="<%=model.CardCouponShowName %>" style="width: 100%" placeholder="优惠券前端显示名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">渠道前端显示名称：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtChannelShowName" value="<%=model.ChannelShowName %>" style="width: 100%" placeholder="渠道前端显示名称" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">微信绑定域名：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtWeiXinBindDomain" value="<%=model.WeiXinBindDomain %>" style="width: 100%" placeholder="微信绑定域名" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">商城统计限制天数：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtMallStatisticsLimitDate" value="<%=model.MallStatisticsLimitDate==0?30:model.MallStatisticsLimitDate %>" style="width: 100%" placeholder="商城统计限制天数" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">站点有效期至：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input class="easyui-datebox" data-options="width:200" value="<%=model.WebsiteExpirationDate %>" showseconds="false" id="txtWebsiteExpirationDate" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">分销支持级别
                    </td>
                    <td>
                        <input id="cbDistributionLimitLevel1" name="cbDistributionLimitLevel" type="radio" data-value="1" checked="checked" />
                        <label for="cbDistributionLimitLevel1">仅一级</label>

                        <input id="cbDistributionLimitLevel2" name="cbDistributionLimitLevel" type="radio" data-value="2" />
                        <label for="cbDistributionLimitLevel2">有二级</label>

                        <input id="cbDistributionLimitLevel3" name="cbDistributionLimitLevel" type="radio" data-value="3" />
                        <label for="cbDistributionLimitLevel3">有三级</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">分销员级别获取
                    </td>
                    <td>
                        <input id="cbDistributionGetWay0" name="cbDistributionGetWay" type="radio" data-value="0" checked="checked" />
                        <label for="cbDistributionGetWay0">累计佣金升级</label>

                        <input id="cbDistributionGetWay1" name="cbDistributionGetWay" type="radio" data-value="1" />
                        <label for="cbDistributionGetWay1">系统设置级别</label>


                        满<input type="text" id="txtAutoUpdateLevelMinAmout" value="<%=model.AutoUpdateLevelMinAmout %>" />
                        元自动升级为
                            <select id="ddlAutoUpdateLevel">
                                <%foreach (var item in levelList)
                                  {
                                      Response.Write(string.Format("<option value=\"{0}\">{1}</option>", item.AutoId, item.LevelString));

                                  } %>
                            </select>
                        （系统设置级别有效）
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">分销员标准规则
                    </td>
                    <td>
                        <input id="cbDistributionMemberStandardsHaveParent" type="checkbox" disabled="disabled" value="1" checked="checked" />
                        <label for="cbDistributionMemberStandardsHaveParent">有上级</label>

                        <input id="cbDistributionMemberStandardsHavePay" type="checkbox" value="1" />
                        <label for="cbDistributionMemberStandardsHavePay">有付款的订单</label>

                        <input id="cbDistributionMemberStandardsHaveSuccessOrder" type="checkbox" value="1" />
                        <label for="cbDistributionMemberStandardsHaveSuccessOrder">有交易成功的订单</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">分销关系建立规则
                    </td>
                    <td>
                        <input id="cbDistributionRelationBuildQrCode" type="checkbox" value="1" />
                        <label for="cbDistributionRelationBuildQrCode">关注二维码</label>

                        <input id="cbDistributionRelationBuildSpreadActivity" type="checkbox" value="1" />
                        <label for="cbDistributionRelationBuildSpreadActivity">微转发报名</label>

                        <input id="cbDistributionRelationBuildMallOrder" type="checkbox" value="1" />
                        <label for="cbDistributionRelationBuildMallOrder">分享商城链接下单</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">开启余额支付：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" id="rdoenableamountpay" value="1" name="enableamountpay" /><label for="rdoenableamountpay">开启</label>
                        <input type="radio" id="rdodisableamountpay" value="0" name="enableamountpay" checked="checked" /><label for="rdodisableamountpay">关闭</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">关闭商品购买日期设置：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="time" id="rdotime0" value="1" /><label for="rdotime0">开启</label>
                        <input type="radio" id="rdotime1" value="0" name="time" checked="checked" /><label for="rdotime1">关闭</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">是否需要分销推荐码：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="code" id="codeOpen" value="1" checked="checked" /><label for="codeOpen">开启</label>
                        <input type="radio" name="code" id="codeClose" value="0" /><label for="codeClose">关闭</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">是否自动同步数据：</td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="data" value="1" checked="checked" id="synchronization" /><label for="synchronization">开启</label>
                        <input type="radio" name="data" value="0" id="noSynchronization" /><label for="noSynchronization">关闭</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">分佣：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="rdoIsDisabledCommission" id="rdoIsDisabledCommission0" value="0" checked="checked" /><label for="rdoIsDisabledCommission0">开启</label>
                        <input type="radio" name="rdoIsDisabledCommission" id="rdoIsDisabledCommission1" value="1" /><label for="rdoIsDisabledCommission1">关闭</label>
                    </td>
                </tr>
                
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">是否开启饿了么订单同步：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="elemeOrderSynchronous" id="rdoOpenElemeOrderSynchronous" value="1" /><label for="rdoOpenElemeOrderSynchronous">开启</label>
                        <input type="radio" name="elemeOrderSynchronous" id="rdoCloseElemeOrderSynchronous" value="0" checked="checked" /><label for="rdoCloseElemeOrderSynchronous">关闭</label>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 200px;" align="right" valign="middle">添加/编辑商品 商户必填：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="radio" name="rdoRequiredSupplier" id="rdoRequiredSupplier1" value="1" /><label for="rdoRequiredSupplier1">是</label>
                        <input type="radio" name="rdoRequiredSupplier" id="rdoRequiredSupplier0" value="0" checked="checked" /><label for="rdoRequiredSupplier0">否</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">接口Key：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="text" id="txtApiKey" value="<%=model.ComeoncloudOpenAppKey %>" maxlength="32" style="width: 100%;" placeholder="32位长度" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">登陆页面配置：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <div class="loginConfig">
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>优先显示</td>
                                                <td>
                                                    <input type="radio" value="0" checked="checked" class="positionTop2" name="page" id="loginPage" /><label for="loginPage">登陆页面</label>
                                                    <input type="radio" value="1" class="positionTop2" name="page" id="regPage" /><label for="regPage">注册页面</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>页面背景图片</td>
                                                <td>
                                                    <div>
                                                        <img src="" class="bgImg" id="bgImg" onclick="txtBgImg.click();" alt="背景图片" />

                                                        <input type="file" id="txtBgImg" class="hidden " name="file1" />

                                                        <a href="javascript:;" class="clearImg" onclick="DeleteImg()">清除</a>
                                                   </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr align="center">
                                    <td class="Font20">登陆页面</td>
                                    <td class="Font20">注册页面</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>登陆按钮文字</td>
                                                <td>
                                                    <input type="text" id="txtLoginBtnText_login" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>登陆按钮背景颜色</td>
                                                <td>
                                                    <input type="text" id="txtLoginBtnBgColor_login" class="form-control color" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>登陆按钮透明度</td>
                                                <td>
                                                    <input type="number" min="1" id="txtTmd_login" max="100" class="form-control" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>距离头部百分比</td>
                                                <td>
                                                    <input type="number" min="1" id="txtDisHead_login" max="100" class="form-control" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>验证码按钮背景颜色</td>
                                                <td>
                                                    <input type="text" id="txtCodeBgColor_login" class="form-control color" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>验证码按钮字体颜色</td>
                                                <td>
                                                    <input type="text" id="txtCodeFontColor_login" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>输入框边框颜色</td>
                                                <td>
                                                    <input type="text" id="txtTextBorderColor_login" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>输入框提示字体颜色</td>
                                                <td>
                                                    <input type="text" id="txtTextTipFontColor_login" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>登陆完跳转链接</td>
                                                <td>
                                                    <input type="text" id="txtLoginerUrl" class="form-control" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>是否使用默认文本框</td>
                                                <td>
                                                    <input type="radio" value="0" class="positionTop2" name="DefText_login" id="rdoDefBorder_login" /><label for="rdoDefBorder_login">默认</label>
                                                    <input type="radio" value="1" class="positionTop2" name="DefText_login" id="rdoCustomize_login" /><label for="rdoCustomize_login">自定义</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>输入框背景颜色是否透明</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="chkIsTmd_login" /><label for="chkIsTmd_login">透明</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>是否显示密码框</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="isShowPwdBorder_login" /><label for="isShowPwdBorder_login">显示</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>头部是否显示</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2" name="page" id="isShowHead_login" /><label for="isShowHead_login">显示</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>是否显示去注册</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="isShowReg_login" /><label for="isShowReg_login">显示</label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>注册按钮文字</td>
                                                <td>
                                                    <input type="text" id="txtLoginBtnText_reg" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>注册按钮背景颜色</td>
                                                <td>
                                                    <input type="text"  id="txtLoginBtnBgColor_reg"  class="form-control Width color" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>注册按钮透明度</td>
                                                <td>
                                                    <input type="number" id="txtTmd_reg" max="100" class="form-control Width" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>距离头部百分比</td>
                                                <td>
                                                    <input type="number" id="txtDisHead_reg" max="100" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>验证码按钮背景颜色</td>
                                                <td>
                                                    <input type="text" id="txtCodeBgColor_reg" class="form-control color" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>验证码按钮字体颜色</td>
                                                <td>
                                                    <input type="" id="txtCodeFontColor_reg" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>输入框边框颜色</td>
                                                <td>
                                                    <input type="text" id="txtTextBorderColor_reg" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>输入框提示字体颜色</td>
                                                <td>
                                                    <input type="text" id="txtTextTipFontColor_reg" class="form-control color" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>注册完跳转链接</td>
                                                <td>
                                                    <input type="text" id="txtRegisterUrl" class="form-control" />
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td>是否使用默认文本框</td>
                                                <td>
                                                    <input type="radio" value="0" class="positionTop2" name="DefText_reg" id="rdoDefBorder_reg" /><label for="rdoDefBorder_reg">默认</label>
                                                    <input type="radio" value="1" class="positionTop2" name="DefText_reg" id="rdoCustomize_reg" /><label for="rdoCustomize_reg">自定义</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>输入框背景颜色是否透明</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="chkIsTmd_reg" /><label for="chkIsTmd_reg">透明</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>是否显示密码框</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="isShowPwdBorder_reg" /><label for="isShowPwdBorder_reg">显示</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>头部是否显示</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="isShowHead_reg" /><label for="isShowHead_reg">显示</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>是否显示去登陆</td>
                                                <td>
                                                    <input type="checkbox" class="positionTop2"  id="isShowLogin_reg"  /><label for="isShowLogin_reg">显示</label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 200px;" align="right" valign="middle">
                        会员管理按钮：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdate" value="updateinfo" /><label for="rdoMemberMgrBtnUpdate">编辑</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdateTag" value="updatetag" /><label for="rdoMemberMgrBtnUpdateTag">设置标签</label>  
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdateScore" value="updatescore" /><label for="rdoMemberMgrBtnUpdateScore">批量修改积分</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnClearScore" value="clearscore" /><label for="rdoMemberMgrBtnClearScore">批量积分清零</label>  
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdateMemberLevel" value="updatememberlevel" /><label for="rdoMemberMgrBtnUpdateMemberLevel">设置会员等级</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdateAccountAmount" value="updateaccountamount" /><label for="rdoMemberMgrBtnUpdateAccountAmount">设置余额</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnSynMemberInfo" value="synmemberinfo" /><label for="rdoMemberMgrBtnSynMemberInfo">从订单,活动报名数据中同步会员信息 </label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnUpdateChannel" value="updatechannel" /><label for="rdoMemberMgrBtnUpdateChannel">设置渠道</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnSendWxMsg" value="sendweixinmsg" /><label for="rdoMemberMgrBtnSendWxMsg">发送微信消息</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnSendWxMsgByTag" value="sendweixinmsgbytag" /><label for="rdoMemberMgrBtnSendWxMsgByTag">发送微信消息[标签]</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnClearData" value="cleardata" /><label for="rdoMemberMgrBtnClearData">数据清洗</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnAdvancedfilter" value="advancefilter" /><label for="rdoMemberMgrBtnAdvancedfilter">高级筛选</label>
                        <input type="checkbox" name="cbMemberMgrBtn" id="rdoMemberMgrBtnTagfilter" value="tagfilter" /><label for="rdoMemberMgrBtnTagfilter">标签名</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">接口白名单(IP地址间用英文逗号分隔)：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <textarea style="width: 100%; height: 200px;" id="txtWhiteIP" placeholder="接口白名单(IP地址间用英文逗号分隔)"><%=model.WhiteIP %></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">网站说明：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <textarea style="width: 100%; height: 400px;" id="txtWebsiteDescription" placeholder="网站说明"><%=model.WebsiteDescription %></textarea>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 70px; line-height: 60px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 14px; left: 0;">
            <a href="javascript:;" style="width: 200px; font-weight: bold; text-decoration: none;" id="btnSave""
                class="button button-rounded button-primary">保存</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    
    <script type="text/javascript">



        var weisiteAction = '<%=webAction%>';
        var action = '<%=action%>';
        var handlerUrl = "/Handler/App/CationHandler.ashx";

        var enableamountpay = '<%=model.IsEnableAccountAmountPay%>';

        var time = '<%=model.IsEnableLimitProductBuyTime%>';

        var code = '<%=model.IsNeedDistributionRecommendCode%>';

        var data = '<%=model.IsSynchronizationData%>';

        var model = '<%=model%>';

        var isDisabledCommission=<%=model.IsDisabledCommission%>;

        var distributionMemberStandardsHaveParent = '<%=model.DistributionMemberStandardsHaveParent%>';
        var distributionMemberStandardsHavePay = '<%=model.DistributionMemberStandardsHavePay%>';
        var distributionMemberStandardsHaveSuccessOrder = '<%=model.DistributionMemberStandardsHaveSuccessOrder%>';
        var distributionRelationBuildQrCode = '<%=model.DistributionRelationBuildQrCode%>';
        var distributionRelationBuildSpreadActivity = '<%=model.DistributionRelationBuildSpreadActivity%>';
        var distributionRelationBuildMallOrder = '<%=model.DistributionRelationBuildMallOrder%>';
        var distributionLimitLevel = Number('<%=model.DistributionLimitLevel%>');
        var distributionGetWay = Number('<%=model.DistributionGetWay%>');
        var isOpenOrderSynchronous=Number('<%=model.IsOpenElemeOrderSynchronous%>');
        var loginPageConfig='<%=model.LoginPageConfig%>';
        var requiredSupplier=<%=model.RequiredSupplier%>;
        $("#ddlAutoUpdateLevel").val("<%=model.AutoUpdateLevelId%>");

       

        function getLoginPageConfig(){
            var loginPage={};
            var logins={};
            var regs={};
            var obj=[];

            var firstShow=$('[name=page]:checked').val();
            var pageBgImg=$('.bgImg').attr('src');
            loginPage.first_show=firstShow;
            loginPage.page_bgimg=pageBgImg;

            var login_show_head=$('#isShowHead_login').get(0).checked?1:0;
            var login_btn_text=$('#txtLoginBtnText_login').val();
            var login_btn_color=$('#txtLoginBtnBgColor_login').val();
            var login_tmd=$('#txtTmd_login').val();
            var login_dis_head=$('#txtDisHead_login').val();
            var login_code_bgcolor=$('#txtCodeBgColor_login').val();
            var logn_code_fontcolor=$('#txtCodeFontColor_login').val();
            var login_def_border=$('[name=DefText_login]:checked').val();
            var login_show_pwd_border=$('#isShowPwdBorder_login').get(0).checked?1:0;
            var login_show_reg=$('#isShowReg_login').get(0).checked?1:0;
            var login_text_border_color=$('#txtTextBorderColor_login').val();
            var login_text_bgcolor_istmd=$('#chkIsTmd_login').get(0).checked?1:0;
            var login_text_tip_fontcolor=$('#txtTextTipFontColor_login').val();
            var login_text_url=$.trim($('#txtLoginerUrl').val());

            logins.login_show_head=login_show_head;
            logins.login_btn_text=login_btn_text;
            logins.login_btn_color=login_btn_color;
            logins.login_tmd=login_tmd;
            logins.login_dis_head=login_dis_head;
            logins.login_code_bgcolor=login_code_bgcolor;
            logins.logn_code_fontcolor=logn_code_fontcolor;
            logins.login_def_border=login_def_border;
            logins.login_show_pwd_border=login_show_pwd_border;
            logins.login_show_reg=login_show_reg;
            logins.login_text_border_color=login_text_border_color;
            logins.login_text_bgcolor_istmd=login_text_bgcolor_istmd;
            logins.login_text_tip_fontcolor=login_text_tip_fontcolor;
            logins.login_text_url=login_text_url;

            var reg_show_head=$('#isShowHead_reg').get(0).checked?1:0;
            var reg_btn_text=$('#txtLoginBtnText_reg').val();
            var reg_btn_color=$('#txtLoginBtnBgColor_reg').val();
            var reg_tmd=$('#txtTmd_reg').val();
            var reg_dis_head=$('#txtDisHead_reg').val();
            var reg_code_bgcolor=$('#txtCodeBgColor_reg').val();
            var reg_code_fontcolor=$('#txtCodeFontColor_reg').val();
            var reg_def_border=$('[name=DefText_reg]:checked').val();
            var reg_show_pwd_border=$('#isShowPwdBorder_reg').get(0).checked?1:0;
            var reg_show_login=$('#isShowLogin_reg').get(0).checked?1:0;
            var reg_text_border_color=$('#txtTextBorderColor_reg').val();
            var reg_text_bgcolor_istmd=$('#chkIsTmd_reg').get(0).checked?1:0;
            var reg_text_tip_fontcolor=$('#txtTextTipFontColor_reg').val();
            var reg_text_url=$.trim($('#txtRegisterUrl').val());

            regs.reg_show_head=reg_show_head;
            regs.reg_btn_text=reg_btn_text;
            regs.reg_btn_color=reg_btn_color;
            regs.reg_tmd=reg_tmd;
            regs.reg_dis_head=reg_dis_head;
            regs.reg_code_bgcolor=reg_code_bgcolor;
            regs.reg_code_fontcolor=reg_code_fontcolor;
            regs.reg_def_border=reg_def_border;
            regs.reg_show_pwd_border=reg_show_pwd_border;
            regs.reg_show_login=reg_show_login;
            regs.reg_text_border_color=reg_text_border_color;
            regs.reg_text_bgcolor_istmd=reg_text_bgcolor_istmd;
            regs.reg_text_tip_fontcolor=reg_text_tip_fontcolor;
            regs.reg_text_url=reg_text_url;

            obj.push(logins);
            obj.push(regs);
            loginPage.pages=obj;

            return JSON.stringify(loginPage);
        }

        function setLoginPageConfig(){

            if(!loginPageConfig) return;
            console.log('loginPageConfig',loginPageConfig);
            var configs=JSON.parse(loginPageConfig);
            var logins=configs.pages[0];
            var regs=configs.pages[1];

            var firstShow=configs.first_show;//优先显示
            //页面背景图片
            if(firstShow==0){
                loginPage.checked=true;
            }else{
                regPage.checked=true;
            }
            $('#bgImg').attr('src',configs.page_bgimg);


            //登陆页面
            if(logins.login_show_head==1){
                isShowHead_login.checked=true;
            }else{
                isShowHead_login.checked=false;
            }
            $('#txtLoginBtnText_login').val(logins.login_btn_text);
            $('#txtLoginBtnBgColor_login').val(logins.login_btn_color);
            $('#txtTmd_login').val(logins.login_tmd);
            $('#txtDisHead_login').val(logins.login_dis_head);
            $('#txtCodeBgColor_login').val(logins.login_code_bgcolor);
            $('#txtCodeFontColor_login').val(logins.logn_code_fontcolor);
            $('#txtTextBorderColor_login').val(logins.login_text_border_color);
            $('#txtTextTipFontColor_login').val(logins.login_text_tip_fontcolor);
            $('#txtLoginerUrl').val(logins.login_text_url);
            var defBorder=logins.login_def_border;
            if(defBorder==0){
                rdoDefBorder_login.checked=true;
            }else{
                rdoCustomize_login.checked=true;
            }
            if(logins.login_show_pwd_border==1){
                isShowPwdBorder_login.checked=true;
            }else{
                isShowPwdBorder_login.checked=false;
            }
            if(logins.login_show_reg==1){
                isShowReg_login.checked=true;
            }else{
                isShowReg_login.checked=false;
            }
            if(logins.login_text_bgcolor_istmd==1){
                chkIsTmd_login.checked=true;
            }else{
                chkIsTmd_login.checked=false;
            }
            //注册页面
            if(regs.reg_show_head==1){
                isShowHead_reg.checked=true;
            }else{
                isShowHead_reg.checked=false;
            }
            $('#txtLoginBtnText_reg').val(regs.reg_btn_text);
            $('#txtLoginBtnBgColor_reg').val(regs.reg_btn_color);
            $('#txtTmd_reg').val(regs.reg_tmd);
            $('#txtDisHead_reg').val(regs.reg_dis_head);
            $('#txtCodeBgColor_reg').val(regs.reg_code_bgcolor);
            $('#txtCodeFontColor_reg').val(regs.reg_code_fontcolor);
            $('#txtTextBorderColor_reg').val(regs.reg_text_border_color);
            $('#txtTextTipFontColor_reg').val(regs.reg_text_tip_fontcolor);
            $('#txtRegisterUrl').val(regs.reg_text_url);

            var defTextBorder=regs.reg_def_border;
            if(defTextBorder==0){
                rdoDefBorder_reg.checked=true;
            }else{
                rdoCustomize_reg.checked=true;
            }
            if(regs.reg_show_pwd_border==1){
                isShowPwdBorder_reg.checked=true;
            }else{
                isShowPwdBorder_reg.checked=false;
            }
            if(regs.reg_show_login==1){
                isShowLogin_reg.checked=true;
            }else{
                isShowLogin_reg.checked=false;
            }
            if(regs.reg_text_bgcolor_istmd==1){
                chkIsTmd_reg.checked=true;
            }else{
                chkIsTmd_reg.checked=false;
            }
        }


        $(function () {

            if (weisiteAction != '' || weisiteAction != null) {
                setLoginPageConfig();

                setMemberMgrBtn("<%=model.MemberMgrBtn%>");
                if (enableamountpay == 1) {
                    rdoenableamountpay.checked = true;
                } else {
                    rdodisableamountpay.checked = true;
                }

                if (time == 1) {
                    rdotime0.checked = true;
                } else {
                    rdotime1.checked = true;
                }

                if (code == 1) {
                    codeOpen.checked = true;
                } else {
                    codeClose.checked = true;
                }
                if (isDisabledCommission==1) {
                    rdoIsDisabledCommission1.checked=true;
                }
                else{
                    rdoIsDisabledCommission0.checked=true;
                }

                cbDistributionMemberStandardsHaveParent.checked = true;

                //if (distributionMemberStandardsHaveParent == 1) {
                //    cbDistributionMemberStandardsHaveParent.checked = true;
                //}
                //else {
                //    cbDistributionMemberStandardsHaveParent.checked = false;
                //}

                if (distributionMemberStandardsHavePay == 1) {
                    cbDistributionMemberStandardsHavePay.checked = true;
                }
                else {
                    cbDistributionMemberStandardsHavePay.checked = false;
                }

                if (distributionMemberStandardsHaveSuccessOrder == 1) {
                    cbDistributionMemberStandardsHaveSuccessOrder.checked = true;
                }
                else {
                    cbDistributionMemberStandardsHaveSuccessOrder.checked = false;
                }

                if (distributionRelationBuildQrCode == 1) {
                    cbDistributionRelationBuildQrCode.checked = true;
                }
                else {
                    cbDistributionRelationBuildQrCode.checked = false;
                }

                if (distributionRelationBuildSpreadActivity == 1) {
                    cbDistributionRelationBuildSpreadActivity.checked = true;
                }
                else {
                    cbDistributionRelationBuildSpreadActivity.checked = false;
                }

                if (distributionRelationBuildMallOrder == 1) {
                    cbDistributionRelationBuildMallOrder.checked = true;
                }
                else {
                    cbDistributionRelationBuildMallOrder.checked = false;
                }
                if (!!distributionLimitLevel) {
                    $('#cbDistributionLimitLevel' + distributionLimitLevel).get(0).checked = true;
                }
                if (!!distributionGetWay) {
                    $('#cbDistributionGetWay' + distributionGetWay).get(0).checked = true;
                }
                if(isOpenOrderSynchronous==1){
                    rdoOpenElemeOrderSynchronous.checked=true;
                }else{
                    rdoOpenElemeOrderSynchronous.checked=false;
                }
                if (requiredSupplier==1) {
                    rdoRequiredSupplier1.checked=true;
                      
                }
                else {
                    rdoRequiredSupplier0.checked=true;
                }
                
            }
            if (weisiteAction == 'EditWebsite') {

                if (data != '') {

                    if (data == 1) {
                        synchronization.checked = true;
                    } else {
                        noSynchronization.checked = true;
                    }
                }

            }
            
            $("#txtBgImg").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtBgImg',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 bgImg.src=resp.ExStr;
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
            $("#btnSave").click(function () {
                var dataModel = {
                    WebsiteOwner: $.trim($('#txtWebsiteOwner').val()),
                    WebsiteName: $.trim($('#txtWebsiteName').val()),
                    WebsiteDescription: $.trim($('#txtWebsiteDescription').val()),
                    WebsiteExpirationDate: $.trim($('#txtWebsiteExpirationDate').datebox("getValue")),
                    LogLimitDay: $.trim($("#txtLogDay").val()),
                    Action: weisiteAction,
                    TemplateId: $("#ddlTemplate").val(),
                    IsEnableLimitProductBuyTime: $("input[name=time]:checked").val(),
                    IsEnableAmountPay: $("input[name=enableamountpay]:checked").val(),
                    AccountAmountPayShowName: $("#txtAccountAmountPayShowName").val(),
                    TotalAmountShowName: $("#txtTotalAmountShowName").val(),
                    MaxSubAccountCount: $('#txtMaxSubAccountCount').val(),
                    DistributionMemberStandardsHaveParent: 0,//分销会员标准 有上级
                    DistributionMemberStandardsHavePay: 0,//分销会员标准 有付款的订单
                    DistributionMemberStandardsHaveSuccessOrder: 0,//分销会员标准 有交易完成的订单
                    DistributionRelationBuildQrCode: 0,//分销关系建立规则 关注二维码
                    DistributionRelationBuildSpreadActivity: 0,//分销关系建立规则 转发报名
                    DistributionRelationBuildMallOrder: 0,//分销关系建立规则 商城下单
                    IsNeedDistributionRecommendCode: $("input[name=code]:checked").val(),//是否需要分销推荐码
                    IsSynchronizationData: $("input[name=data]:checked").val(),//是否自动同步数据
                    IsDisabledCommission: $("input[name=rdoIsDisabledCommission]:checked").val(),//是否禁用分佣
                    WeiXinBindDomain: $.trim($("#txtWeiXinBindDomain").val()),
                    WhiteIP: $(txtWhiteIP).val(),
                    MallStatisticsLimitDate: $("#txtMallStatisticsLimitDate").val(),
                    ChannelShowName: $("#txtChannelShowName").val(),
                    DistributionLimitLevel: $('input[name="cbDistributionLimitLevel"]:checked').attr('data-value'),
                    DistributionGetWay: $('input[name="cbDistributionGetWay"]:checked').attr('data-value'),
                    ScorePayShowName: $("#txtScorePayShowName").val(),
                    CardCouponShowName: $("#txtCardCouponShowName").val(),
                    ComeoncloudOpenAppKey:$("#txtApiKey").val(),
                    AutoUpdateLevelMinAmout:$("#txtAutoUpdateLevelMinAmout").val(),
                    AutoUpdateLevelId:$("#ddlAutoUpdateLevel").val(),
                    IsOpenElemeOrderSynchronous:$('[name=elemeOrderSynchronous]:checked').val(),
                    RequiredSupplier:$('[name=rdoRequiredSupplier]:checked').val(),
                    LoginPageConfig:getLoginPageConfig(),
                    MemberMgrBtn:getMemberMgrBtn()

                }
                if (cbDistributionMemberStandardsHaveParent.checked) {
                    dataModel.DistributionMemberStandardsHaveParent = 1;
                }
                if (cbDistributionMemberStandardsHavePay.checked) {
                    dataModel.DistributionMemberStandardsHavePay = 1;
                }
                if (cbDistributionMemberStandardsHaveSuccessOrder.checked) {
                    dataModel.DistributionMemberStandardsHaveSuccessOrder = 1;
                }


                if (cbDistributionRelationBuildQrCode.checked) {
                    dataModel.DistributionRelationBuildQrCode = 1;
                }
                if (cbDistributionRelationBuildSpreadActivity.checked) {
                    dataModel.DistributionRelationBuildSpreadActivity = 1;
                }
                if (cbDistributionRelationBuildMallOrder.checked) {
                    dataModel.DistributionRelationBuildMallOrder = 1;
                }
                if (dataModel.WebsiteName == '') {
                    Alert('请输入网站名称');
                    return;
                }

                if (dataModel.WebsiteOwner == '') {
                    Alert('请输入站点所有者登录名');
                    return;
                }
                if (dataModel.WebsiteExpirationDate == '') {
                    Alert('请输入站点有效期');
                    return;
                }
                if (dataModel.LogLimitDay == '') {
                    Alert('请输入日志保存天数');
                    return;
                }
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: dataModel,
                    dataType: "json",
                    success: function (resp) {
                        if (action == "新建") {
                            $("input[type=text]").val('');
                            $("#txtWebsiteDescription").val('');
                            rdodisableamountpay.checked = true;
                            rdotime1.checked = true;
                        }
                        layerAlert(resp.Msg);
                    }
                });
            })



        })

        function DeleteImg(){
            $('#bgImg').attr('src','');
        }

        //获取会员管理页面按钮
        function getMemberMgrBtn(){
        
            var btnArry=[];
            if (rdoMemberMgrBtnUpdate.checked) {
                btnArry.push("updateinfo");

            }
            if (rdoMemberMgrBtnUpdateTag.checked) {
                btnArry.push("updatetag");

            }
            if (rdoMemberMgrBtnUpdateScore.checked) {
                btnArry.push("updatescore");

            }
            if (rdoMemberMgrBtnClearScore.checked) {
                btnArry.push("clearscore");

            }
            if (rdoMemberMgrBtnUpdateMemberLevel.checked) {
                btnArry.push("updatememberlevel");

            }
            if (rdoMemberMgrBtnUpdateAccountAmount.checked) {
                btnArry.push("updateaccountamount");

            }
            if (rdoMemberMgrBtnSynMemberInfo.checked) {
                btnArry.push("synmemberinfo");

            }
            if (rdoMemberMgrBtnUpdateChannel.checked) {
                btnArry.push("updatechannel");

            }
            if (rdoMemberMgrBtnSendWxMsg.checked) {
                btnArry.push("sendweixinmsg");

            }
            if (rdoMemberMgrBtnSendWxMsgByTag.checked) {
                btnArry.push("sendweixinmsgbytag");

            }
            if (rdoMemberMgrBtnClearData.checked) {
                btnArry.push("cleardata");

            }
            if (rdoMemberMgrBtnAdvancedfilter.checked) {
                btnArry.push("advancefilter");

            }
            if (rdoMemberMgrBtnTagfilter.checked) {
                btnArry.push("tagfilter");

            }
            return btnArry.join(',');


        
        }
        //设置会员管理页面按钮
        function setMemberMgrBtn(btnStr){
        
        
            if (btnStr.indexOf("updateinfo")>-1) {
                rdoMemberMgrBtnUpdate.checked=true;
            }
            if (btnStr.indexOf("updatetag")>-1) {
                rdoMemberMgrBtnUpdateTag.checked=true;
            }
            if (btnStr.indexOf("updatescore")>-1) {
                rdoMemberMgrBtnUpdateScore.checked=true;
            }
            if (btnStr.indexOf("clearscore")>-1) {
                rdoMemberMgrBtnClearScore.checked=true;
            }
            if (btnStr.indexOf("updatememberlevel")>-1) {
                rdoMemberMgrBtnUpdateMemberLevel.checked=true;
            }
            if (btnStr.indexOf("updateaccountamount")>-1) {
                rdoMemberMgrBtnUpdateAccountAmount.checked=true;
            }
            if (btnStr.indexOf("synmemberinfo")>-1) {
                rdoMemberMgrBtnSynMemberInfo.checked=true;
            }
            if (btnStr.indexOf("updatechannel")>-1) {
                rdoMemberMgrBtnUpdateChannel.checked=true;
            }
            if (btnStr.indexOf("sendweixinmsg")>-1) {
                rdoMemberMgrBtnSendWxMsg.checked=true;
            }
            if (btnStr.indexOf("sendweixinmsgbytag")>-1) {
                rdoMemberMgrBtnSendWxMsgByTag.checked=true;
            }
            if (btnStr.indexOf("cleardata")>-1) {
                rdoMemberMgrBtnClearData.checked=true;
            }
            if (btnStr.indexOf("advancefilter")>-1) {
                rdoMemberMgrBtnAdvancedfilter.checked=true;
            }
            if (btnStr.indexOf("tagfilter")>-1) {
                rdoMemberMgrBtnTagfilter.checked=true;
            }

        }

    </script>
</asp:Content>
