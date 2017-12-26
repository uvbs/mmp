<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle {
            font-weight: bold;
        }

        table td {
            height: 40px;
        }

        .sort {
            width: 780px;
        }

        .title {
            font-size: 12px;
        }

        input[type=text], select, textarea {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
            padding-left: 10px;
        }

        .sort {
            height: 0 !important;
        }

        .centent_r_btm {
            border: 0;
        }

        .lbTip {
            padding: 3px 6px;
            background-color: #5C5566;
            color: #fff;
            font-size: 14px;
            border-radius: 50px;
            cursor: pointer;
            margin-left: 20px;
        }

        .layui-layer-tips .layui-layer-content {
            background-color: #5C5566 !important;
            border-bottom-color: #5C5566 !important;
        }

        .layui-layer-tips i.layui-layer-TipsL, .layui-layer-tips i.layui-layer-TipsR {
            border-bottom-color: #5C5566 !important;
        }

        .warpText {
            width: 200px;
            height: 20px !important;
            display: inline-block;
        }

        .warpButton {
            border-top: 1px solid #DDDDDD;
            position: fixed;
            bottom: 0px;
            height: 60px;
            line-height: 60px;
            text-align: center;
            width: 100%;
            background-color: rgb(245, 245, 245);
            padding-top: 14px;
        }

        legend {
            display: block;
            width: 100%;
            padding: 0;
            margin-bottom: 20px;
            font-size: 24px;
            line-height: inherit;
            color: #333;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
        }

        .ActivityBox {
            margin: 0px;
            padding: 20px;
        }

        .selecter {
            width: 200px;
        }

        .scorePayRadio {
            width: 200px;
            display: inline-block;
        }

        .baifenbi {
            font-size: 20px;
            color: #333;
            margin-left: 5px;
        }

        .sp-replacer {
            width: 20%;
        }
         .store-since-items {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 98%;
            position: relative;
        }
          .home-delivery-items {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 98%;
            position: relative;
        }
        .fieldsort {
            float: left;
            margin-left: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .delete-item {
            float: right;
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .items input[type=text] {
            width: 90%;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%; padding-bottom: 80px;">
            <fileset>
            <legend>基础信息</legend>
            <table width="100%">
                <%-- <tr>
                    <td align="right" class="tdTitle">
                        商城名称：
                    </td>
                    <td>
                        <input style="width: 100%;" type="text" id="txtWXMallName" value="<%=currWebSiteInfo.WXMallName??"" %>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        商城Logo：
                    </td>
                    <td>
                        <img alt="缩略图" src="<%=currWebSiteInfo.WXMallBannerImage%>" width="400px" height="100px"
                            id="imgThumbnailsPath" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果宽高比例为2:1
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>--%>
                <tr style="display: none;">
                    <td style="width: 100px;" align="right" class="tdTitle">商城模板：
                    </td>
                    <td>
                        <input type="radio" name="rdomalltemplate" id="rdomalltemplate0" value="0" checked="checked" /><label
                            for="rdomalltemplate0">&nbsp;一般商城</label>
                        &nbsp;&nbsp;
                        <input type="radio" name="rdomalltemplate" id="rdomalltemplate1" value="1" /><label
                            for="rdomalltemplate1">&nbsp;外卖商城</label>
                    </td>
                </tr>
                <tr id="trdeliverytime" style="display: none;">
                    <td align="right" class="tdTitle">配送最早时间(外卖模板有效)
                    </td>
                    <td>&nbsp;&nbsp;自订单开始
                        <input style="width: 100px;" type="text" id="txtMinDeliveryDate" value="<%=currWebSiteInfo.MinDeliveryDate%>"
                            onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />分钟后
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="width: 100px;" align="right" class="tdTitle">展示方式：
                    </td>
                    <td>
                        <input type="radio" name="rdoMallType" id="MallType1" value="0" checked="checked" /><label
                            for="MallType1">&nbsp;普通展示</label>
                        &nbsp;&nbsp;
                        <input type="radio" name="rdoMallType" id="MallType2" value="1" /><label for="MallType2">&nbsp;商品展示</label>
                    </td>
                </tr>


                <tr>
                    <td style="width: 20%;" align="right" class="tdTitle">商品图片比例：
                    </td>
                    <td>
                        <input type="text" id="txtProductImgRatio1" style="width: 80px;" value="<%=currWebSiteInfo.ProductImgRatio1 %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                        ：
                        <input type="text" id="txtProductImgRatio2" style="width: 80px;" value="<%=currWebSiteInfo.ProductImgRatio2 %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />&nbsp;默认 600:600
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;" align="right" class="tdTitle">系统页面主题色：
                    </td>
                    <td>
                        <input type="text" id="txtThemeColor" class="color" style="width: 80px;" value="<%=currWebSiteInfo.ThemeColor %>" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;" align="right" class="tdTitle">导航分组名称：
                    </td>
                    <td>
                        <select style="width: 200px;" id="txtShopNavGroupName">
                            <option value="">分类目录</option>
                            <%
                                if (toolBars.Count > 0)
                                {
                                    foreach (var item in toolBars)
                                    {
                            %>
                            <option value="<%=item%>" <%=currWebSiteInfo.ShopNavGroupName == item?"selected=\"selected\"":"" %>><%=item%></option>
                            <%
                                    }

                                }
                            %>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;" align="right" class="tdTitle">底部导航名称：
                    </td>
                    <td>
                        <select style="width: 200px;" id="ddlShopFoottool">
                            <option value=""></option>
                            <%
                                if (toolBars.Count > 0)
                                {
                                    foreach (var item in toolBars)
                                    {
                            %>
                            <option value="<%=item%>" <%=currWebSiteInfo.ShopFoottool == item?"selected=\"selected\"":"" %>><%=item%></option>
                            <%
                                    }
                                }
                            %>
                        </select>
                    </td>
                </tr>
                   <tr id="wrapIsShowOldPrice">
                    <td style="width: 100px;" align="right" class="tdTitle">显示原价：
                    </td>
                    <td>
                        <input id="chkIsShowOldPrice" class="positionTop2" type="checkbox" <%=currWebSiteInfo.IsShowOldPrice == 1?"checked=\"checked\"":"" %> /><label for="chkIsShowOldPrice">显示原价</label>
                    </td>
                </tr>
                 <tr id="wrapIsShowProductSale">
                    <td style="width: 100px;" align="right" class="tdTitle">显示商品销量：
                    </td>
                    <td>
                        <input id="rdoHideProductSale" checked="checked" value="0" name="sales" class="positionTop2" type="radio" /><label for="rdoHideProductSale">否</label>
                        <input id="rdoShowProductSale" class="positionTop2" value="1" name="sales" type="radio" /><label for="rdoShowProductSale">是</label>
                    </td>
                </tr>
             
                <tr id="wrapIsShowOpenGroup">
                    <td style="width: 100px;" align="right" class="tdTitle">允许用户开团：
                    </td>
                    <td>
                        <input id="rdoNoAllowUserCreateGroup" checked="checked" value="0" name="group" class="positionTop2" type="radio" /><label for="rdoNoAllowUserCreateGroup">否</label>
                        <input id="rdoAllowUserCreateGroup" class="positionTop2" value="1" name="group" type="radio" /><label for="rdoAllowUserCreateGroup">是</label>
                    </td>
                </tr>

                <tr id="wrapFillIn">
                    <td style="width: 100px;" align="right" class="tdTitle">填写下单人手机姓名：
                    </td>
                    <td>
                        <input id="rdoNoNeedNamePhone" checked="checked" value="0" name="NeedNamePhone" class="positionTop2" type="radio" /><label for="rdoNoNeedNamePhone">否</label>
                        <input id="rdoNeedNamePhone" class="positionTop2" value="1" name="NeedNamePhone" type="radio" /><label for="rdoNeedNamePhone">是</label>
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">订单设置：
                    </td>
                    <td>
                        <input id="chkIsClaimMallOrderArrivalTime" class="positionTop2" type="checkbox" <%=currWebSiteInfo.IsClaimMallOrderArrivalTime == 1?"checked=\"checked\"":"" %> /><label for="chkIsClaimMallOrderArrivalTime">要求商城订单填写送达时间</label>
                    </td>
                </tr>
              
            </table>
        </fileset>




            <fileset>
            <legend>积分设置</legend>

            <table style="width:100%;">
                

                <%--  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                       商城描述：
                    </td>
                    <td>
                       <textarea style="width: 100%; height: 100px;" id="txtShopDescription"><%=currWebSiteInfo.ShopDescription%></textarea>
                    </td>
                </tr>--%>

                <tr style="display: none;">
                    <td style="width:20%;" align="right" class="tdTitle">订单提交成功提示信息：
                    </td>
                    <td>
                        <textarea style="width: 100%; height: 100px;" id="txtSumbitOrderPromptInformation"><%=currWebSiteInfo.SumbitOrderPromptInformation%></textarea>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="width:20%;" align="right" class="tdTitle">会员卡优惠信息：
                    </td>
                    <td>
                        <textarea style="width: 100%; height: 100px;" id="txtWXMallMemberCardMessage"><%=currWebSiteInfo.WXMallMemberCardMessage%></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;"  align="right" class="tdTitle">积分支付比例：
                    </td>
                    <td>
                       <input type="number"  class="form-control scorePayRadio" value="<%=currWebSiteInfo.MallScorePayRatio %>" id="txtMallScorePayRatio" /><label class="baifenbi">%</label>
                    </td>
                </tr>

                <tr>
                    <td style="width:20%;"  align="right" class="tdTitle">订单返积分比例：
                    </td>
                    <td>订单金额
                       <input type="text" value="<%=scoreConfig.OrderAmount %>" id="txtOrderAmount" onkeyup="this.value=this.value.replace(/\D/g,'')" style="color: Red;" />元
                       可获得
                       <input type="text" value="<%=scoreConfig.OrderScore %>" id="txtOrderScore" onkeyup="this.value=this.value.replace(/\D/g,'')" style="color: Red;" />
                        积分

                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">返积分取整类型：
                    </td>
                    <td>
                        <select style="width: 200px;" id="ddlRebateScoreGetIntType">
                            <option value="0">四舍五入</option>
                            <option value="1">向上取整</option>
                            <option value="2">向下取整</option>
                        </select>
                    </td>
                </tr>
            
                <tr>
                    <td style="width:20%;"  align="right" class="tdTitle">订单限制：
                    </td>
                    <td>

                        <label>
                            <input id="chkIsRebateScoreMustAllCash" type="checkbox" <%=currWebSiteInfo.IsRebateScoreMustAllCash == 1?"checked=\"checked\"":"" %> />
                            订单支付全额现金的时候才可获得积分
                        </label>
                        <br />
                        <label>
                            <input id="chkIsOrderRebateScoreByMallOrder" type="checkbox" <%=currWebSiteInfo.IsOrderRebateScoreByMallOrder == 1?"checked=\"checked\"":"" %> />
                            商城正常购买可获积分
                        </label>
                        <br />
                        <label>
                            <input id="chkIsOrderRebateScoreByCreateGroupBuy" type="checkbox" <%=currWebSiteInfo.IsOrderRebateScoreByCreateGroupBuy == 1?"checked=\"checked\"":"" %> />
                            开团可获积分
                        </label>
                        <br />
                        <label>
                            <input id="chkIsOrderRebateScoreByJoinGroupBuy" type="checkbox" <%=currWebSiteInfo.IsOrderRebateScoreByJoinGroupBuy == 1?"checked=\"checked\"":"" %> />
                            参团可获积分
                        </label>

                    </td>
                </tr>
                <tr>
                    <td style="width:20%;"  align="right" class="tdTitle"></td>
                    <td></td>
                </tr>
                <tr id="wrapScore">
                    <td style="width:20%;"  align="right" class="tdTitle">积分兑换：
                    </td>
                    <td>
                        <input type="text" value="<%=scoreConfig.ExchangeScore %>" id="txtExchangeScore" onkeyup="this.value=this.value.replace(/\D/g,'')" style="color: Red;" />
                        积分 可兑换
                       <input type="text" value="<%=scoreConfig.ExchangeAmount %>" id="txtExchangeAmount" onkeyup="this.value=this.value.replace(/\D/g,'')" style="color: Red;" />
                        元
                    </td>
                </tr>
                </table>
             </fileset>


            <fileset>
            <legend>订单评价配置</legend>

            <table style="width:100%;">
                     <tr>
                    <td style="width:20%;" align="right" class="tdTitle">订单自动评价：
                    </td>
                    <td>
                        <input type="radio" id="rdoCloseAutoComment" <%=currWebSiteInfo.IsOrderAutoComment == 0?"checked=\"checked\"":"" %> name="rdoAutoComment" value="0" class="positionTop3" /><label for="rdoCloseAutoComment">未启用</label>
                        <input type="radio" id="rdoOpenAutoComment" <%=currWebSiteInfo.IsOrderAutoComment == 1?"checked=\"checked\"":"" %> name="rdoAutoComment" value="1" class="positionTop3" /><label for="rdoOpenAutoComment">启用</label>
                    </td>
                </tr>
                 <tr>
                    <td style="width:20%;" align="right" class="tdTitle">自动好评时间：
                    </td>
                    <td>
                        <input type="number" id="txtOrderAutoCommentDay"  value="<%=currWebSiteInfo.OrderAutoCommentDay %>" class="form-control warpText" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.确认收货后多少天自动好评。<br>">?</span>
                    </td>
                </tr>
                 <tr>
                    <td style="width:20%;" align="right" class="tdTitle">自动好评内容：
                    </td>
                    <td>
                         <div id="divOrderAutoCommentContent">
                            <div id="txtOrderAutoCommentContent" style="width: 375px; height: 300px;">
                                <%= currWebSiteInfo.OrderAutoCommentContent %>
                            </div>
                        </div>
                        <div class="warp-desc">
                            说明：定义随机好评内容,每个内容之间用分号隔开.（分号为英文状态下的分号。）
                        </div>
                    </td>
                </tr>
            </table>
            </fileset>

            <fileset>
            <legend>商城头部配置</legend>

            <table style="width:100%;">
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">商城自定义头部：
                    </td>
                    <td>
                        <input id="chkMallCustomeHead0" name="chkMallCustomeHead" class="positionTop2" type="radio" value="0" <%=currWebSiteInfo.IsCustomizeMallHead == 0?"checked=\"checked\"":"" %> /><label for="chkMallCustomeHead0">关闭自定义头部</label>
                        <input id="chkMallCustomeHead1" name="chkMallCustomeHead" class="positionTop2" type="radio" value="1" <%=currWebSiteInfo.IsCustomizeMallHead == 1?"checked=\"checked\"":"" %> /><label for="chkMallCustomeHead1">开启商城自定义头部</label>
                        <input id="chkMallCustomeHead2" name="chkMallCustomeHead" class="positionTop2" type="radio" value="2" <%=currWebSiteInfo.IsCustomizeMallHead == 2?"checked=\"checked\"":"" %> /><label for="chkMallCustomeHead2">开启App自定义头部</label>
                        <input id="chkMallCustomeHead3" name="chkMallCustomeHead" class="positionTop2" type="radio" value="3" <%=currWebSiteInfo.IsCustomizeMallHead == 3?"checked=\"checked\"":"" %> /><label for="chkMallCustomeHead3">开启商城和App自定义头部</label>
                    </td>
                </tr>                
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">左边显示：
                    </td>
                    <td>
                        <input id="rdoHeadLeftType0" name="rdoHeadLeftType" class="positionTop2" type="radio" value="0" onclick="setHeadLeftType(0)" /><label for="rdoHeadLeftType0">Logo</label>
                        <input id="rdoHeadLeftType1" name="rdoHeadLeftType" class="positionTop2" type="radio" value="1" onclick="setHeadLeftType(1)" /><label for="rdoHeadLeftType1">图标链接</label>
                    </td>
                </tr>
                <tr class="leftIco" style="display:none;">
                    <td style="width:20%;" align="right" class="tdTitle">左边图标和链接：
                    </td>
                    <td>
                        <select class="selecterLeft">
                            <option value=""></option>
                            <%
                                if (toolBars.Count > 0)
                                {
                                    foreach (var item in toolBars)
                                    {
                            %>
                                        <option value="<%=item%>" <%=currWebSiteInfo.ShopNavGroupName == item?"selected=\"selected\"":"" %>><%=item%></option>
                            <%
                                    }
                                }
                            %>
                        </select>
                    </td>
                </tr>
                <tr class="leftLogo" style="display:none;">
                    <td style="width:20%;" align="right" class="tdTitle">头部Logo：
                    </td>
                    <td>
                        <img alt="图片" src="" style="width:64px;height:64px;" id="imgThumbnailsPath" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为75*40。
                        <input type="file" id="txtThumbnailsPath" style="display: none;" name="file1" />
                    </td>
                </tr>                
                <tr class="leftLogo" style="display:none;">
                    <td style="width:20%;" align="right" class="tdTitle">左边Logo链接：
                    </td>
                    <td>
                        <input type="text" class="form-control warpText logo-url" style="width:80%;"  />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">背景颜色：
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">搜索框：
                    </td>
                    <td>
                        <input id="chkSearch" class="positionTop2" type="checkbox"  /><label for="chkSearch">显示搜索框</label>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">搜索框颜色：
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">跳转页面链接：
                    </td>
                    <td>
                        <input type="text" id="txtSearchUrl" class="form-control warpText" style="width:98%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">其它图标和链接：
                    </td>
                    <td>
                        <select class="selecter">
                            <option value=""></option>
                            <%
                                if (toolBars.Count > 0)
                                {
                                    foreach (var item in toolBars)
                                    {
                            %>
                                        <option value="<%=item%>" <%=currWebSiteInfo.ShopNavGroupName == item?"selected=\"selected\"":"" %>><%=item%></option>
                            <%
                                    }

                                }
                            %>
                        </select>
                    </td>
                </tr>

            </table>
            </fileset>

            <fileset>
            <legend>高级功能配置</legend>

            <table style="width:100%;">
                   <tr id="wrapIsShowStock">
                    <td style="width:20%;" align="right" class="tdTitle">文字代替库存值：
                    </td>
                    <td>
                        <input id="chkIsShowStock" class="positionTop2" type="checkbox" <%=currWebSiteInfo.IsShowStock == 1?"checked=\"checked\"":"" %> /><label for="chkIsShowStock">显示库存充足或库存紧张</label>
                    </td>
                </tr>
                <tr id="warpSetStock">
                    <td style="width:20%;" align="right" class="tdTitle"></td>
                    <td>
                        <input type="text" id="IsShowStockValue" class="form-control warpText" value="<%=currWebSiteInfo.IsShowStockValue %>" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.默认值为0。<br>2.当商品库存高于设定的值时显示库存充足，低于设定的值时显示库存紧张。<br>">?</span>
                    </td>
                </tr>
                  <tr>
                    <td style="width:20%;" align="right" class="tdTitle">'下单人'自定义名称：
                    </td>
                    <td>
                        <input type="text" id="txtNeedMallOrderCreaterNamePhoneRName" value="<%=currWebSiteInfo.NeedMallOrderCreaterNamePhoneRName %>" class="form-control warpText" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.默认就是下单人,比如：“下单人名称”可以改成“赠花人名称”。<br>2.展示在下单页面。<br>3.如若看不见自定义名称,请检查“填写下单人手机姓名”设置。<br>">?</span>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">库存告急阈值：
                    </td>
                    <td>
                        <input type="number" id="txtProductStockThreshold" value="<%=currWebSiteInfo.ProductStockThreshold %>" class="form-control warpText" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.当商品的库存少于当前设置的值时,该商品就属于告急商品。<br>2.商品列表可筛选库存告急商品。<br>3.库存告急阈值默认为0。<br>">?</span>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">待付款订单自动取消时间：
                    </td>
                    <td>
                        <input type="text" value="<%=currWebSiteInfo.OrderCancelMinute %>" id="txtOrderCancelMinute" onkeyup="this.value=this.value.replace(/\D/g,'')" style="color: Red;" />
                        分钟
                       <span class="lbTip" data-tip-msg="<b>说明</b><br>不填或者填写0,表示2小时后自动取消。<br>">?</span>
                    </td>
                </tr>
                
                <%--    <tr>
                    <td align="left" class="tdTitle">
                        商城链接
                    </td>
                    <td>
                        <a target="_blank" href="<%=WXMallIndexUrl%>">
                            <%=WXMallIndexUrl%></a>
                    </td>
                </tr>
               <tr>
                    <td align="left" class="tdTitle">
                        商城二维码
                    </td>
                    <td>
                        <img src="/Handler/ImgHandler.ashx?v=<%=WXMallIndexUrl%>" width="300" height="300">
                    </td>
                </tr>--%>



                <tr>
                    <td  style="width:20%;" align="right" class="tdTitle">商品描述头：
                    </td>
                    <td>
                        <div id="txtMallDescTop"><%=currWebSiteInfo.MallDescTop %></div>
                    </td>
                </tr>
                <tr>
                    <td  style="width:20%;" align="right" class="tdTitle">商品描述底：
                    </td>
                    <td>
                        <div id="txtMallDescBottom"><%=currWebSiteInfo.MallDescBottom %></div>
                    </td>
                </tr>
              
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">库存模式：
                    </td>
                    <td>
                        <input type="radio" id="rdoStockType0" <%=CompanyWebsiteConfig.StockType == 0?"checked=\"checked\"":"" %> name="rdoStockType" value="0" class="positionTop3" /><label for="rdoStockType0"> 商户/门店 独立商品独立库存</label>
                        <input type="radio" id="rdoStockType1" <%=CompanyWebsiteConfig.StockType == 1?"checked=\"checked\"":"" %> name="rdoStockType" value="1" class="positionTop3" /><label for="rdoStockType1">商户/门店 共用商品独立库存</label>
                    </td>
                </tr>

                   <tr>
                    <td style="width:20%;" align="right" class="tdTitle">自动分配订单：
                    </td>
                    <td>
                        <input type="radio" id="rdoIsAutoAssignOrder0" <%=CompanyWebsiteConfig.IsAutoAssignOrder == 0?"checked=\"checked\"":"" %> name="rdoIsAutoAssignOrder" value="0" class="positionTop3" /><label for="rdoIsAutoAssignOrder0"> 关闭</label>
                        <input type="radio" id="rdoIsAutoAssignOrder1" <%=CompanyWebsiteConfig.IsAutoAssignOrder == 1?"checked=\"checked\"":"" %> name="rdoIsAutoAssignOrder" value="1" class="positionTop3" /><label for="rdoIsAutoAssignOrder1">开启</label>
                    </td>
                </tr>
                 <tr>
                    <td style="width:20%;" align="right" class="tdTitle">自动分单范围：
                    </td>
                    <td>
                        <input type="number" id="txtAutoAssignOrderRange"  value="<%=CompanyWebsiteConfig.AutoAssignOrderRange %>" class="form-control warpText" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.自动分单范围。<br>">?</span>
                    </td>
                </tr>

                   <tr>
                    <td style="width:20%;" align="right" class="tdTitle">购物车单独结算：
                    </td>
                    <td>
                        <input type="radio" id="rdoShopCartAlongSettlement0" <%=CompanyWebsiteConfig.ShopCartAlongSettlement == 0?"checked=\"checked\"":"" %> name="rdoShopCartAlongSettlement" value="0" class="positionTop3" /><label for="rdoShopCartAlongSettlement0"> 关闭</label>
                        <input type="radio" id="rdoShopCartAlongSettlement1" <%=CompanyWebsiteConfig.ShopCartAlongSettlement == 1?"checked=\"checked\"":"" %> name="rdoShopCartAlongSettlement" value="1" class="positionTop3" /><label for="rdoShopCartAlongSettlement1">开启</label>
                    </td>
                </tr>

                   <tr>
                    <td style="width:20%;" align="right" class="tdTitle">是否开启门店自提：
                    </td>
                    <td>
                        <input type="radio" id="rdoIsStoreSince0" <%=CompanyWebsiteConfig.IsStoreSince == 0?"checked=\"checked\"":"" %> name="rdoIsStoreSince" value="0" class="positionTop3" /><label for="rdoIsStoreSince0"> 关闭</label>
                        <input type="radio" id="rdoIsStoreSince1" <%=CompanyWebsiteConfig.IsStoreSince == 1?"checked=\"checked\"":"" %> name="rdoIsStoreSince" value="1" class="positionTop3" /><label for="rdoIsStoreSince1">开启</label>
                    </td>
                </tr>
                 <tr>
                    <td style="width:20%;" align="right" class="tdTitle">门店自提优惠：
                    </td>
                    <td>
                        <input type="number" value="<%=CompanyWebsiteConfig.StoreSinceDiscount %>" id="txtStoreSinceDiscount"/>
                        元
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">门店自提时间段:
                    </td>
                    <td>



                         <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddItemStoreSinceTime">添加时间段</a>
                       
                    </td>
                </tr>

                   <tr>
                    <td style="width:20%;" align="right" class="tdTitle">是否开启送货上门：
                    </td>
                    <td>
                        <input type="radio" id="rdoIsHomeDelivery0" <%=CompanyWebsiteConfig.IsHomeDelivery == 0?"checked=\"checked\"":"" %> name="rdoIsHomeDelivery" value="0" class="positionTop3" /><label for="rdoIsHomeDelivery0"> 关闭</label>
                        <input type="radio" id="rdoIsHomeDelivery1" <%=CompanyWebsiteConfig.IsHomeDelivery == 1?"checked=\"checked\"":"" %> name="rdoIsHomeDelivery" value="1" class="positionTop3" /><label for="rdoIsHomeDelivery1">开启</label>
                    </td>
                </tr>
                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">送货上门时间段:
                    </td>
                    <td>
                       

                         <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddItemHomeDeliveryTime">添加时间段</a>
                        
                    </td>
                </tr>

                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">最早送货时间：
                    </td>
                    <td>
                        下单后
                        <input type="number" id="txtEarliestDeliveryTime"  value="<%=CompanyWebsiteConfig.EarliestDeliveryTime %>" class="form-control warpText" />
                        小时
                    </td>
                </tr>

                <tr>
                    <td style="width:20%;" align="right" class="tdTitle">快递送货最小范围
                    </td>
                    <td>
                        
                        <input type="number" id="txtExpressRange"  value="<%=CompanyWebsiteConfig.ExpressRange %>" class="form-control warpText" />
                        米以外
                    </td>
                </tr>
                  <tr>
                    <td style="width:20%;" align="right" class="tdTitle">店员配送最大范围
                    </td>
                    <td>
                        
                        <input type="number" id="txtStoreExpressRange"  value="<%=CompanyWebsiteConfig.StoreExpressRange %>" class="form-control warpText" />
                        米以内
                    </td>
                </tr>
               </table>
              <fileset>


             <fileset>
            <legend>自动退款设置</legend>

            <table style="width:100%;">
                     <tr>
                    <td style="width:20%;" align="right" class="tdTitle">退款自动关闭：
                    </td>
                    <td>
                        <input type="radio" id="rdoIsAutoCloseRefund0" <%=CompanyWebsiteConfig.IsAutoCloseRefund == 0?"checked=\"checked\"":"" %> name="rdoAutoCloseRefund" value="0" class="positionTop3" /><label for="rdoIsAutoCloseRefund0">未启用</label>
                        <input type="radio" id="rdoIsAutoCloseRefund1" <%=CompanyWebsiteConfig.IsAutoCloseRefund == 1?"checked=\"checked\"":"" %> name="rdoAutoCloseRefund" value="1" class="positionTop3" /><label for="rdoIsAutoCloseRefund1">启用</label>
                    </td>
                </tr>
                 <tr>
                    <td style="width:20%;" align="right" class="tdTitle">自动关闭退款时间：
                    </td>
                    <td>
                        <input type="number" id="txtAutoCloseRefundDay"  value="<%=CompanyWebsiteConfig.AutoCloseRefundDay %>" class="form-control warpText" />
                        <span class="lbTip" data-tip-msg="<b>说明</b><br>1.商家拒绝退款后多少天后自动关闭退款。<br>">?</span>
                    </td>
                </tr>
                 
            </table>
            </fileset>
        </div>
        <div class="warpButton">
            <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;"
                class="button button-rounded button-primary">保存</a>

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/lib/layer/2.1/layer.js"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currRebateScoreGetIntType=<%=currWebSiteInfo.RebateScoreGetIntType%>;
        var isOpenGroup='<%=currWebSiteInfo.IsOpenGroup%>';
        var isShowProductSale='<%=currWebSiteInfo.IsShowProductSaleCount%>';
        var isNeedCreaterOrderNamePhone='<%=currWebSiteInfo.IsNeedMallOrderCreaterNamePhone%>';
        var headConfig='<%=currWebSiteInfo.CustomizeMallHeadConfig%>';
        var editerMallDescTop;
        var editerMallDescBottom;
        var storeSinceTimeJson=<%=StoreSinceTimeJson%>;//门店自提时间段json
        var homeDeliveryTimeJson=<%=HomeDeliveryTimeJson%>;//送货上门时间段json

        var timeList=[0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23];
        KindEditor.ready(function (K) {
            editor = K.create('#txtOrderAutoCommentContent', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [],
                filterMode: false
            });
        });
        $(function () {
            InitKindEditor();
            if(isOpenGroup=='0'){
                $('#rdoNoAllowUserCreateGroup').attr('checked',true);
            }else{
                $('#rdoAllowUserCreateGroup').attr('checked',true);
            }

            if(isShowProductSale=='0'){
                $('#rdoHideProductSale').attr('checked',true);
            }else{
                $('#rdoShowProductSale').attr('checked',true);
            }

            if(isNeedCreaterOrderNamePhone=='0'){
                $('#rdoNoNeedNamePhone').attr('checked',true);
            }else{
                $('#rdoNeedNamePhone').attr('checked',true);
            }

            if(headConfig){
                var obj=JSON.parse(headConfig);                
                if(obj.nav_left_type){
                    $('[name="rdoHeadLeftType"][value="'+obj.nav_left_type+'"]').get(0).checked = true;
                    setHeadLeftType(obj.nav_left_type);
                }else{
                    $('[name="rdoHeadLeftType"][value="0"]').get(0).checked = true;
                    setHeadLeftType(0);
                }
                if(obj.nav_left){
                    $('.selecterLeft').val(obj.nav_left);
                }
                if(obj.logo){
                    $('#imgThumbnailsPath').attr('src',obj.logo);
                }
                if(obj.logo_url){
                    $('.logo-url').val(obj.logo_url);
                }

                if(obj.show_search==1){
                    $('#chkSearch').attr('checked',true);
                }else{
                    $('#chkSearch').attr('checked',false);
                }
                if(obj.search_color){
                    $('#txtSearchColor').val(obj.search_color);
                }
                if(obj.search_url){
                    $('#txtSearchUrl').val(obj.search_url);
                }
                if(obj.nav_type){
                    $('.selecter').val(obj.nav_type);
                }
                if(obj.background_color){
                    $('#txtBackgroundColor').val(obj.background_color);
                }else{
                    $('#txtBackgroundColor').val('#FFFFFF');
                }
            }


            
            $('#ddlRebateScoreGetIntType').val(currRebateScoreGetIntType);

            var $wrapScore = $('#wrapScore');

            //$wrapScore.hide();

            $.getJSON('/serv/api/admin/permission/get.ashx', function (data) {
                
                for (var i = 0; i < data.result.length; i++) {
                    if (data.result[i] == "PMS_SCORESHOP") {
                        $wrapScore.show();
                        break;
                    }
                }
            });

            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });


            $('#chkIsShowStock').click(function(){
                if($(this).get(0).checked){
                    $('#warpSetStock').show();
                }else{
                    $('#warpSetStock').hide();
                }
            });
            if($('#chkIsShowStock').get(0).checked){
                $('#warpSetStock').show();
            }else{
                $('#warpSetStock').hide();
            }


            if ($.browser.msie) { //ie 下
                //缩略图
                $("#txtThumbnailsPath").show(); //缩略图
                $("#auploadThumbnails").hide();
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }


            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

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
                                 layer.msg(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    layer.msg(e);
                }
            });
            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        WXMallBannerImage: $('#imgThumbnailsPath').attr("src"),
                        SumbitOrderPromptInformation: $("#txtSumbitOrderPromptInformation").val(),
                        MallTemplateId: $(":radio[name=rdomalltemplate]:checked").val(),
                        MallType: $(":radio[name=rdoMallType]:checked").val(),
                        WXMallName: $("#txtWXMallName").val(),
                        WXMallMemberCardMessage: $("#txtWXMallMemberCardMessage").val(),
                        MinDeliveryDate: $("#txtMinDeliveryDate").val(),
                        ProductImgRatio1: $("#txtProductImgRatio1").val(),
                        ProductImgRatio2: $("#txtProductImgRatio2").val(),
                        ShopDescription: $("#txtShopDescription").val(),
                        ShopNavGroupName: $("#txtShopNavGroupName").val(),
                        ShopFoottool: $("#ddlShopFoottool").val(),
                        OrderAmount: $("#txtOrderAmount").val(),
                        OrderScore: $("#txtOrderScore").val(),
                        ExchangeScore: $("#txtExchangeScore").val(),
                        ExchangeAmount: $("#txtExchangeAmount").val(),
                        IsShowOldPrice: $("#chkIsShowOldPrice").get(0).checked ? 1 : 0,
                        IsShowStock: $("#chkIsShowStock").get(0).checked ? 1 : 0,
                        ThemeColor: $('#txtThemeColor').val(),
                        IsRebateScoreMustAllCash: $("#chkIsRebateScoreMustAllCash").get(0).checked ? 1 : 0,
                        IsOrderRebateScoreByMallOrder: $("#chkIsOrderRebateScoreByMallOrder").get(0).checked ? 1 : 0,
                        IsOrderRebateScoreByCreateGroupBuy: $("#chkIsOrderRebateScoreByCreateGroupBuy").get(0).checked ? 1 : 0,
                        IsOrderRebateScoreByJoinGroupBuy: $("#chkIsOrderRebateScoreByJoinGroupBuy").get(0).checked ? 1 : 0,
                        RebateScoreGetIntType:$('#ddlRebateScoreGetIntType').val(),
                        IsOpenGroup:$("[name=group]:checked").val(),
                        IsClaimMallOrderArrivalTime:$('#chkIsClaimMallOrderArrivalTime').get(0).checked ? 1 : 0,
                        ProductStockThreshold:$('#txtProductStockThreshold').val(),
                        IsShowProductSaleCount:$('[name=sales]:checked').val(),
                        NeedMallOrderCreaterNamePhoneRName:$('#txtNeedMallOrderCreaterNamePhoneRName').val(),
                        IsNeedMallOrderCreaterNamePhone:$('[name=NeedNamePhone]:checked').val(),
                        IsShowStockValue:$('#IsShowStockValue').val(),
                        MallDescTop:$.trim(editerMallDescTop.html()),
                        MallDescBottom:$.trim(editerMallDescBottom.html()),
                        OrderCancelMinute:$('#txtOrderCancelMinute').val(),
                        OrderAutoCommentDay:$('#txtOrderAutoCommentDay').val(),
                        IsOrderAutoComment:$('[name=rdoAutoComment]:checked').val(),
                        OrderAutoCommentContent:editor.text(),
                        CustomizeMallHeadConfig:GetMallHeadConfig(),                        
                        IsCustomizeMallHead:$('[name=chkMallCustomeHead]:checked').val(),
                        MallScorePayRatio:$('#txtMallScorePayRatio').val(),
                        IsAutoCloseRefund:$('[name=rdoAutoCloseRefund]:checked').val(),//是否自动退款
                        AutoCloseRefundDay:$("#txtAutoCloseRefundDay").val(),//自动退款天数

                        StockType:$('[name=rdoStockType]:checked').val(),//库存模式 0 独立库存 1 同一商品不同分店不同库存
                        IsAutoAssignOrder:$('[name=rdoIsAutoAssignOrder]:checked').val(),//是否自动 分单
                        AutoAssignOrderRange:$('#txtAutoAssignOrderRange').val(),//自动分配订单范围
                        ShopCartAlongSettlement:$('[name=rdoShopCartAlongSettlement]:checked').val(),//是否购物车自动结算
                        IsStoreSince:$('[name=rdoIsStoreSince]:checked').val(),//是滞门店自提
                        StoreSinceTimeJson:getStoreSinceTimeJson(),//门店自己提时间段
                        IsHomeDelivery:$('[name=rdoIsHomeDelivery]:checked').val(),//是否送货上门
                        EarliestDeliveryTime:$('#txtEarliestDeliveryTime').val(),//最早配送时间
                        HomeDeliveryTimeJson:getHomeDeliveryTimeJsonJson(),//上门配送时间段
                        StoreExpressRange:$(txtStoreExpressRange).val(),
                        ExpressRange:$(txtExpressRange).val(),
                        StoreSinceDiscount:$(txtStoreSinceDiscount).val(),//门店自提优惠
                        Action: "UpdateWXMallConfig"
                    };

                    //console.log(model);
                    
                    if (model.OrderAmount.length>9) {
                        layer.msg('订单金额超过有效长度');
                        return;
                    }
                    if (model.OrderScore.length>9) {
                        layer.msg('积分超过有效长度');
                        return;
                    }
                    if (model.ExchangeScore.length >9) {
                        layer.msg('兑换积分超过有效长度');
                        return;
                    }
                    if(model.ExchangeAmount.length>18){
                        layer.msg('兑换金额超过有效长度');
                        return;
                    }
                    if(parseInt(model.MallScorePayRatio)<0||parseInt(model.MallScorePayRatio)>100){
                        layer.msg('积分支付比例不能大于100且不能小于0');
                        return;
                    }
                    if (model.IsAutoCloseRefund=="1") {
                    
                        if (model.AutoCloseRefundDay==""||model.AutoCloseRefundDay<=0) {
                            layer.msg('请输入正确的天数');
                            return;
                        }
                    }
                    
                    $.messager.progress({ text: '正在保存...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            layer.msg(resp.Msg);
                        }
                    });

                } catch (e) {
                    alert(e);
                    layer.msg(e);
                }
            });
            var templateid = "<%=currWebSiteInfo.MallTemplateId%>";
            switch (templateid) {
                case "0":
                    $("#rdomalltemplate0").attr("checked", "checked");
                    $("#trdeliverytime").hide();
                    break;
                case "1":
                    $("#rdomalltemplate1").attr("checked", "checked");
                    $("#trdeliverytime").show();
                    break;

                default:

            }
            var MallType = "<%=currWebSiteInfo.MallType %>";
            switch (MallType) {
                case "0":
                    $("#MallType1").attr("checked", "checked");
                    break;
                case "1":
                    $("#MallType2").attr("checked", "checked");
                    break;
            }
            $("input[name=rdomalltemplate]").click(function () {

                if ($(this).val() == "1") {
                    $("#trdeliverytime").show();
                }
                else {
                    $("#trdeliverytime").hide();
                }

            });

            //删除
            $('.delete-item').live("click", function () {

                if (confirm("确定要删除?")) {

                    $(this).parent().remove();
                }



            });
            //删除


            loadItemsStoreSinceTime();//上门自提时间段

            loadItemsHomeDeliveryTime();//送货上门时间段

        });

        //获取商城头部配置
        function GetMallHeadConfig(){            
            var navLeftType = $('[name="rdoHeadLeftType"]:checked').val();
            var navLeft = $('.selecterLeft').val();
            var logo=$('#imgThumbnailsPath').attr('src');
            var logoUrl=$.trim($('.logo-url').val());
            var showSearch=$('#chkSearch').get(0).checked?1:0;            
            var searchUrl=$.trim($('#txtSearchUrl').val());
            var navType=$('.selecter').val();
            var searchColor=$('#txtSearchColor').val();
            var backGroundColor=$('#txtBackgroundColor').val();
            var obj={                nav_left_type:navLeftType,
                nav_left:navLeft,
                logo:logo,
                logo_url:logoUrl,
                show_search:showSearch,
                search_url:searchUrl,
                nav_type:navType,
                search_color:searchColor,
                background_color:backGroundColor
            };

            console.log('obj',JSON.stringify(obj));
            return JSON.stringify(obj);



        }
        function InitKindEditor(){
            KindEditor.ready(function (K) {
                editerMallDescTop = K.create('#txtMallDescTop', {
                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                    items: [
                        'source', '|', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                        'insertunorderedlist', '|', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table', 'cleardoc'],
                    filterMode: false,
                    extraFileUploadParams: { userID: '<%=currWebSiteInfo.WebsiteOwner %>' },
                    width: '500px',
                    height: "360px",
                    //cssData: ' body, html{overflow: auto !important;}img{ max-width: 100%;}',
                    //cssPath: ['/customize/comeoncloud/m2/dist/all.min.css?v=20160715'],
                    afterChange: function (ex1, ex2) {}
                });
                editerMallDescBottom = K.create('#txtMallDescBottom', {
                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                    items: [
                        'source', '|', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                        'insertunorderedlist', '|', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table', 'cleardoc'],
                    filterMode: false,
                    extraFileUploadParams: { userID: '<%=currWebSiteInfo.WebsiteOwner %>' },
                    width: '500px',
                    height: "360px",
                    //cssData: ' body, html{overflow: auto !important;}img{ max-width: 100%;}',
                    //cssPath: ['/customize/comeoncloud/m2/dist/all.min.css?v=20160715'],
                    afterChange: function (ex1, ex2) {}
                });
            });
        }        
        function setHeadLeftType(num){
            if(num==0){
                $('.leftIco').hide();
                $('.leftLogo').show();
            }else{
                $('.leftLogo').hide();
                $('.leftIco').show();
            }
        }

        //获取门店自提时间
        function getStoreSinceTimeJson(){
            
            var items=[];
            $(".store-since-items").each(function () {

                var item = {
                    from: '',
                    to: ''
                    

                };
                
                item.from = $(this).find(".time-from").first().val();
                item.to = $(this).find(".time-to").first().val();
               
               items.push(item);
            });
            return JSON.stringify(items);
        }
         
        //获取送货上门时间
        function getHomeDeliveryTimeJsonJson(){
            var items=[];
            $(".home-delivery-items").each(function () {

                var item = {
                    from: '',
                    to: ''
                    

                };
                
                item.from = $(this).find(".time-from").first().val();
                item.to = $(this).find(".time-to").first().val();
               
                items.push(item);
            });
             return JSON.stringify(items);

        }

        //加载门店自提时间段
        function loadItemsStoreSinceTime() {

            var appendhtml = new StringBuilder();
            
            for (var i = 0; i < storeSinceTimeJson.length; i++) {

                
                appendhtml.AppendFormat('<div class="store-since-items" data-item-index="{0}" data-item-id="">', "0");
                //appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort" />');
                //appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort" />');
                appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
                appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat('<td>');
                appendhtml.AppendFormat('');
                appendhtml.AppendFormat('<select class="time-from">');
 

                for (var l = 0; l < timeList.length; l++) {
                    if (storeSinceTimeJson[i].from == timeList[l]) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{1}</option>', timeList[l],timeList[l]);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',timeList[l], timeList[l]);
                    }

                }

                appendhtml.AppendFormat('</select>');
                appendhtml.AppendFormat('点-&nbsp;');
                appendhtml.AppendFormat('<select class="time-to">');
                for (var k = 0; k< timeList.length; k++) {
                    if (storeSinceTimeJson[i].to == timeList[k]) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{1}</option>', timeList[k],timeList[k]);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',timeList[k], timeList[k]);
                    }

                }

                appendhtml.AppendFormat('</select>点');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('</table>');
                appendhtml.AppendFormat('</div>');


            }

            $("#btnAddItemStoreSinceTime").before(appendhtml.ToString());


        }


        //添加 门店自提时间段
        $("#btnAddItemStoreSinceTime").click(function () {

            var appendhtml = new StringBuilder();
            appendhtml.AppendFormat('<div class="store-since-items" data-item-index="{0}" data-item-id="">', "0");
            //appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort" />');
            //appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort" />');
            appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
            appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
            appendhtml.AppendFormat('<tr>');
            appendhtml.AppendFormat('<td>');
            appendhtml.AppendFormat('');
            appendhtml.AppendFormat('<select class="time-from">');
            for (var i = 0; i < timeList.length; i++) {
                    
                appendhtml.AppendFormat('<option value="{0}">{1}</option>', timeList[i], timeList[i]);
                    

            } 

            appendhtml.AppendFormat('</select>');
            appendhtml.AppendFormat('点-&nbsp;');
            appendhtml.AppendFormat('<select class="time-to">');
            for (var i = 0; i < timeList.length; i++) {
                    
                appendhtml.AppendFormat('<option value="{0}">{1}</option>', timeList[i], timeList[i]);
                    

            }

            appendhtml.AppendFormat('</select>点');
            appendhtml.AppendFormat('</td>');
            appendhtml.AppendFormat('</tr>');
            appendhtml.AppendFormat('</table>');
            appendhtml.AppendFormat('</div>');

            $(this).before(appendhtml.ToString());

           


        });
        //添加

        //加载上门送货时间段
        function loadItemsHomeDeliveryTime() {

            var appendhtml = new StringBuilder();
            
            for (var i = 0; i < homeDeliveryTimeJson.length; i++) {

                
                appendhtml.AppendFormat('<div class="home-delivery-items" data-item-index="{0}" data-item-id="">', "0");
                //appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort" />');
                //appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort" />');
                appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
                appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat('<td>');
                appendhtml.AppendFormat('');
                appendhtml.AppendFormat('<select class="time-from">');
 

                for (var l = 0; l < timeList.length; l++) {
                    if (homeDeliveryTimeJson[i].from == timeList[l]) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{1}</option>', timeList[l],timeList[l]);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',timeList[l], timeList[l]);
                    }

                }

                appendhtml.AppendFormat('</select>');
                appendhtml.AppendFormat('点-&nbsp;');
                appendhtml.AppendFormat('<select class="time-to">');
                for (var k = 0; k< timeList.length; k++) {
                    if (homeDeliveryTimeJson[i].to == timeList[k]) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{1}</option>', timeList[k],timeList[k]);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',timeList[k], timeList[k]);
                    }

                }

                appendhtml.AppendFormat('</select>点');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('</table>');
                appendhtml.AppendFormat('</div>');


            }

            $("#btnAddItemHomeDeliveryTime").before(appendhtml.ToString());


        }


        //添加 上门自取时间段
        $("#btnAddItemHomeDeliveryTime").click(function () {

            var appendhtml = new StringBuilder();
            appendhtml.AppendFormat('<div class="home-delivery-items" data-item-index="{0}" data-item-id="">', "0");
            //appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort" />');
            //appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort" />');
            appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
            appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
            appendhtml.AppendFormat('<tr>');
            appendhtml.AppendFormat('<td>');
            appendhtml.AppendFormat('');
            appendhtml.AppendFormat('<select class="time-from">');
            for (var i = 0; i < timeList.length; i++) {
                    
                appendhtml.AppendFormat('<option value="{0}">{1}</option>', timeList[i], timeList[i]);
                    

            } 

            appendhtml.AppendFormat('</select>');
            appendhtml.AppendFormat('点-&nbsp;');
            appendhtml.AppendFormat('<select class="time-to">');
            for (var i = 0; i < timeList.length; i++) {
                    
                appendhtml.AppendFormat('<option value="{0}">{1}</option>', timeList[i], timeList[i]);
                    

            }

            appendhtml.AppendFormat('</select>点');
            appendhtml.AppendFormat('</td>');
            appendhtml.AppendFormat('</tr>');
            appendhtml.AppendFormat('</table>');
            appendhtml.AppendFormat('</div>');

            $(this).before(appendhtml.ToString());

           


        });
        //添加

    </script>

</asp:Content>
