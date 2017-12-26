<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WebsiteConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.WebsiteConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://static-files.socialcrmyun.com/MainStyleV2/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        textarea
        {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
            padding:0px 5px;
        }
        input[type=text], input[type=number]
        {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
            padding:0px 5px;
        }
        input[type=radio], input[type=checkbox]
        {
            position: relative;
            top: 4px;
            height: 16px;
            margin: 0px !important;
        }
        label{
            padding: 0px 5px;
        }
        .showSetField
        {
            padding: 0px 10px;
            height: 32px;
            line-height: 32px;
            margin-left: 10px;
        }
        table
        {
            border-collapse: separate !important;
            border-spacing: 10px !important;
        }
        select
        {
            max-width:200px;
            
            }
        .userInfo_field{
            padding:0px 5px; border:solid #eeeeee 1px; float:left; margin:2px; width:216px;
        }
        .userInfo_field .userInfo_field-setName{
            height:30px; 
            display:flex;
            width:100%;
        }
        .userInfo_field .field-name{
            flex:1; line-height:30px;display:inline-block;
        }
        .userInfo_field .field-showname{
            flex:1; width:100%; height: 26px;
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
        .token{
            color: #fff;
            background-color: #20a0ff;
            border-color: #20a0ff;
        }
        .btn-default:hover{
            color: #fff;
            background-color: #20a0ff;
            border-color: #20a0ff;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：<span>全局设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <fileset>
            <legend>基础信息</legend>
       

        <table width="100%" class="tdMain">
           
           <%-- <tr class="">
                <td>
                </td>
                <td>
                    <a id="aweburl" href="http://<%=Request.Url.Authority %>/customize/comeoncloud/Index.aspx?key=MallHome" target="_blank">http://<%=Request.Url.Authority%>/customize/comeoncloud/Index.aspx?key=MallHome</a>
                </td>
            </tr>--%>

            
        <%--    <tr class="">
                <td align="left" colspan="2" valign="middle">
                    <hr />
                </td>
            </tr>--%>

            <tr>
                <td style="width: 200px;"  align="right" valign="middle">网站Logo：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <img alt="图片" src="<%=config==null?"":config.WebsiteImage %>" width="100px" height="100px"
                        id="imgThumbnailsPath" /><br />
                    <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                        plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为100*100。
                    <input type="file" id="txtThumbnailsPath" style="display: none;" name="file1" />
                </td>
            </tr>
             <tr class="">
                <td style="width: 200px;" align="right" valign="middle">
                    移动站点首页二维码：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <img src="/Handler/ImgHandler.ashx?v=http://<%=Request.Url.Authority%>/customize/comeoncloud/Index.aspx?key=MallHome"
                        width="120" height="120">
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">公众号名称：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtWeixinAccountNickName" class="" style="width: 100%;" value="<%=config==null?"":config.WeixinAccountNickName %>" placeholder="公众号名称" />

                </td>
            </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">公众号图标：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <img alt="图片" src="<%=config==null?"":config.DistributionQRCodeIcon %>" width="40px" height="40px"
                        id="iconThumbnailsPath" /><br />
                    <a id="iconThumbnailsPaths" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                        plain="true" onclick="txtQRcodeIconPath.click()">上传图片</a>
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为40*40。
                    <input type="file" id="txtQRcodeIconPath" style="display: none;" name="file1" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">网站标题：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtWebsiteTitle" class="" style="width: 100%;" value="<%=config==null?"":config.WebsiteTitle %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">网站描述：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <textarea id="txtWebsiteDescription" style="width: 100%; height: 50px;"><%=config == null ? "" : config.WebsiteDescription%></textarea>
                </td>
            </tr>
            <tr >
                <td style="width: 200px;" align="right" valign="middle">底部版权：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtCopyright" class="" style="width: 100%;" value="<%=config==null?"":config.Copyright %>" />
                </td>
            </tr>
            </table>
         </fileset>
        
        <fileset>
         <legend>展示配置</legend>
         <table width="100%" class="tdMain">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">我的优惠券列表标题：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtMyCardCouponsTitle" class="" style="width: 100%;" value="<%=config==null?"":config.MyCardCouponsTitle %>" placeholder="我的优惠券" />

                </td>
            </tr>
                        
                  <tr>
                <td style="width: 200px;" align="right" valign="middle">文章底部导航：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    
                        <select id="ddlArticleToolBarGroups" class="form-control">
                        <option value="">无</option>
                        <%foreach (var item in ArticleGroups)
                          {
                              if (item == config.ArticleToolBarGrous)
                              {
                                  Response.Write(string.Format("<option value=\"{0}\" selected=\"select\">{0}</option>", item));
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
                <td style="width: 200px;" align="right" valign="middle">活动底部导航：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    
                        <select id="ddlActivityToolBarGrous" class="form-control">
                        <option value="">无</option>
                        <%foreach (var item in ArticleGroups)
                          {
                              if (item == config.ActivityToolBarGrous)
                              {
                                  Response.Write(string.Format("<option value=\"{0}\" selected=\"select\">{0}</option>", item));
                              }
                              else
                              {
                                  Response.Write(string.Format("<option value=\"{0}\">{0}</option>", item));
                              }

                          } %>
                        
                        </select>
                     
                </td>
            </tr>
             <tr class="">
                <td style="width: 200px;" align="right" valign="middle">(旧)导航分组名称：</td>
                <td>
                    <select style="width: 200px;" id="txtShopNavGroupName"
                        class="form-control">
                        <%
                            if (toolBars.Count > 0)
                            {
                                foreach (var item in toolBars)
                                {
                                    if (config.ShopNavGroupName == item)
                                    {
                                             %>
                                                <option selected="selected" value="<%=item%>"><%=item%></option>
                                            <%
}
                                    else
                                    {
                                             %>
                                                <option value="<%=item%>"><%=item%></option>
                                            <%
                                                }

                                }
                            }
                        %>
                    </select>
                </td>
            </tr>
            <tr class="">
                <td style="width: 200px;" align="right" valign="middle">(旧)首页广告设置：</td>
                <td>
                    <select class="form-control" style="width: 200px;" id="txtShopAdType">
                           <%
                               if (slides.Count > 0)
                               {
                                   foreach (var item in slides)
                                   {
                                       if (config.ShopAdType == item)
                                       {
                                          %>
                                            <option selected="selected" value="<%=item %>"><%=item%></option>
                                          <%
}
                                      else
                                      {
                                          %>
                                            <option  value="<%=item %>"><%=item%></option>
                                          <%
                                              }

                                  }
                              }    
                           %>
                    </select>
                </td>
            </tr>
            <tr class="">
                <td style="width: 200px;" align="right" valign="middle">(旧)底部工具栏：</td>
                <td>
                    <select style="width: 200px;" id="txtToolBar"
                        class="form-control">
                        <%
                            if (toolBars.Count > 0)
                            {
                                foreach (var item in toolBars)
                                {
                                    if (config.BottomToolbars == item)
                                    {
                                        %>
                                            <option selected="selected" value="<%=item%>"><%=item%></option>
                                        <%
}
                                    else
                                    {
                                         %>
                                            <option  value="<%=item%>"><%=item%></option>
                                        <%
                                            }

                                }
                            }
                        %>
                    </select>
                </td>
            </tr>
         </table>
        </fileset>
        <fileset>
            <legend>系统链接配置</legend>

            <table width="100%" class="tdMain">
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">团购首页链接：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                         <input type="text" id="txtGroupBuyIndexUrl" class="" style="width: 100%;" value="<%=config.GroupBuyIndexUrl%>" placeholder="团购首页链接" />
                     
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">个人中心链接：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                         <input type="text" id="txtPersonalCenterLink" class="" style="width: 100%;" value="<%=config.PersonalCenterLink%>" placeholder="个人中心链接" />
                     
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">商城订单支付成功跳转：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                         <input type="text" id="txtMallOrderPaySuccessUrl" class="" style="width: 90%;" value="<%=currentWebsiteInfo.MallOrderPaySuccessUrl%>" placeholder="商城订单支付成功跳转链接" />
                         <span class="lbTip" data-tip-msg="<b>说明</b><br>1.订单号用{order_id}代替,小写。<br/><br/><br/>">?</span>
                        <div style="color:#ccc;">
                            <span>支付成功后进入订单详情：</span>
                            <span>http://<%=Request.Url.Authority %>/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/{order_id}#/productDetail/{order_id}</span>
                        </div>
                    </td>
                </tr>


                 
             

            </table>

        </fileset>
        <fileset>
            <legend>个人中心</legend>
            <table width="100%" class="tdMain">
             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    优先显示：
                </td>
                <td align="left" valign="middle">
                    <input type="radio" id="rdUserInfoFirstShow0" name="rdUserInfoFirstShow" value="0" /><label for="rdUserInfoFirstShow0">微信信息优先</label>
                    <input type="radio" id="rdUserInfoFirstShow1" name="rdUserInfoFirstShow" value="1" /><label for="rdUserInfoFirstShow1">用户设置优先</label>
                </td>
             </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    用户设置：
                </td>
                <td align="left" valign="middle">
                    <ul class="userInfo_fields">
                        <li class="userInfo_field" data-field="avatar">
                            <div class="userInfo_field-setName">
                                <span class="field-name">头像</span>
                            <input type="text" class="field-showname showname-avatar" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                                <input type="checkbox" id="ck_avatar_show" /><label for="ck_avatar_show">显示</label>
                                <input type="checkbox" id="ck_avatar_edit" /><label for="ck_avatar_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="truename">
                            <div class="userInfo_field-setName">
                            <span class="field-name">姓名</span>
                            <input type="text" class="field-showname showname-truename" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_truename_show" /><label for="ck_truename_show">显示</label>
                            <input type="checkbox" id="ck_truename_edit" /><label for="ck_truename_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="sex">
                            <div class="userInfo_field-setName">
                            <span class="field-name">性别</span>
                            <input type="text" class="field-showname showname-sex" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_sex_show" /><label for="ck_sex_show">显示</label>
                            <input type="checkbox" id="ck_sex_edit" /><label for="ck_sex_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="phone">
                            <div class="userInfo_field-setName">
                            <span class="field-name">电话</span>
                            <input type="text" class="field-showname showname-phone" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_phone_show" /><label for="ck_phone_show">显示</label>
                            <input type="checkbox" id="ck_phone_edit" /><label for="ck_phone_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="email">
                            <div class="userInfo_field-setName">
                            <span class="field-name">邮箱</span>
                            <input type="text" class="field-showname showname-email" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_email_show" /><label for="ck_email_show">显示</label>
                            <input type="checkbox" id="ck_email_edit" /><label for="ck_email_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="ex1">
                            <div class="userInfo_field-setName">
                            <span class="field-name">Ex1</span>
                            <input type="text" class="field-showname showname-ex1" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_ex1_show" /><label for="ck_ex1_show">显示</label>
                            <input type="checkbox" id="ck_ex1_edit" /><label for="ck_ex1_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="ex2">
                            <div class="userInfo_field-setName">
                            <span class="field-name">Ex2</span>
                            <input type="text" class="field-showname showname-ex2" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_ex2_show" /><label for="ck_ex2_show">显示</label>
                            <input type="checkbox" id="ck_ex2_edit" /><label for="ck_ex2_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="ex3">
                            <div class="userInfo_field-setName">
                            <span class="field-name">Ex3</span>
                            <input type="text" class="field-showname showname-ex3" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_ex3_show" /><label for="ck_ex3_show">显示</label>
                            <input type="checkbox" id="ck_ex3_edit" /><label for="ck_ex3_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="ex4">
                            <div class="userInfo_field-setName">
                            <span class="field-name">Ex4</span>
                            <input type="text" class="field-showname showname-ex4" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_ex4_show" /><label for="ck_ex4_show">显示</label>
                            <input type="checkbox" id="ck_ex4_edit" /><label for="ck_ex4_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="ex5">
                            <div class="userInfo_field-setName">
                            <span class="field-name">Ex5</span>
                            <input type="text" class="field-showname showname-ex5" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_ex5_show" /><label for="ck_ex5_show">显示</label>
                            <input type="checkbox" id="ck_ex5_edit" /><label for="ck_ex5_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="address">
                            <div class="userInfo_field-setName">
                            <span class="field-name">收货地址</span>
                            <input type="text" class="field-showname showname-address" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_address_show" /><label for="ck_address_show">显示</label>
                            <input type="checkbox" id="ck_address_edit" /><label for="ck_address_edit">编辑</label>
                            </div>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul class="userInfo_fields">
                        <li class="userInfo_field" data-field="identitycardphoto">
                            <div class="userInfo_field-setName">
                            <span class="field-name">实名认证</span>
                            <input type="text" class="field-showname showname-identitycardphoto" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_identitycardphoto_show" /><label for="ck_identitycardphoto_show">显示</label>
                            <input type="checkbox" id="ck_identitycardphoto_edit" /><label for="ck_identitycardphoto_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="businessintelligencecertificatephoto">
                            <div class="userInfo_field-setName">
                            <span class="field-name">公司资质</span>
                            <input type="text" class="field-showname showname-businessintelligencecertificatephoto" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_businessintelligencecertificatephoto_show" /><label for="ck_businessintelligencecertificatephoto_show">显示</label>
                            <input type="checkbox" id="ck_businessintelligencecertificatephoto_edit" /><label for="ck_businessintelligencecertificatephoto_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="invoicinginformation">
                            <div class="userInfo_field-setName">
                            <span class="field-name">开票资料</span>
                            <input type="text" class="field-showname showname-invoicinginformation" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_invoicinginformation_show" /><label for="ck_invoicinginformation_show">显示</label>
                            <input type="checkbox" id="ck_invoicinginformation_edit" /><label for="ck_invoicinginformation_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="bankcard">
                            <div class="userInfo_field-setName">
                            <span class="field-name">我的银行卡</span>
                            <input type="text" class="field-showname showname-bankcard" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_bankcard_show" /><label for="ck_bankcard_show">显示</label>
                            <input type="checkbox" id="ck_bankcard_edit" /><label for="ck_bankcard_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="qrcode">
                            <div class="userInfo_field-setName">
                            <span class="field-name">我的二维码名片</span>
                            <input type="text" class="field-showname showname-qrcode" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_qrcode_show" /><label for="ck_qrcode_show">显示</label>
                            <input type="checkbox" id="ck_qrcode_edit" /><label for="ck_qrcode_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="forgetpassword">
                            <div class="userInfo_field-setName">
                            <span class="field-name">密码修改</span>
                            <input type="text" class="field-showname showname-forgetpassword" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_forgetpassword_show" /><label for="ck_forgetpassword_show">显示</label>
                            <input type="checkbox" id="ck_forgetpassword_edit" /><label for="ck_forgetpassword_edit">编辑</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="loginout">
                            <div class="userInfo_field-setName">
                            <span class="field-name">退出登录</span>
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_loginout_show" /><label for="ck_loginout_show">显示</label>
                            </div>
                        </li>
                        <li class="userInfo_field" data-field="memberattribution">
                            <div class="userInfo_field-setName">
                            <span class="field-name">归属地</span>
                            <input type="text" class="field-showname showname-memberattribution" placeholder="别名" value="" />
                            </div>
                            <div class="userInfo_field-setOther">
                            <input type="checkbox" id="ck_memberattribution_show" /><label for="ck_memberattribution_show">显示</label>
                            <input type="checkbox" id="ck_memberattribution_edit" /><label for="ck_memberattribution_edit">编辑</label>
                            </div>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </td>
             </tr>
            </table>
        </fileset>
        <fileset>
            <legend>会员标准</legend>
         <table width="100%" class="tdMain">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">会员标准(会员级别 >= 1)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input id="rdoMemberStandard0" type="radio" name="rdoMemberStandard" <%= (config==null || config.MemberStandard==0)?"checked='checked'":"" %> value="0" /><label for="rdoMemberStandard0">默认(会员级别 >= 1)</label>
                    <input id="rdoMemberStandard1" type="radio" name="rdoMemberStandard" <%= (config!=null && config.MemberStandard==1)?"checked='checked'":"" %> value="1" /><label for="rdoMemberStandard1">需验证手机</label>
                    <input id="rdoMemberStandard2" type="radio" name="rdoMemberStandard" <%= (config!=null && config.MemberStandard==2)?"checked='checked'":"" %> value="2" /><label for="rdoMemberStandard2">需验证手机 + 完善资料</label>
                    <input id="rdoMemberStandard3" type="radio" name="rdoMemberStandard" <%= (config!=null && config.MemberStandard==3)?"checked='checked'":"" %> value="3" /><label for="rdoMemberStandard3">需申请审核</label>
                </td>
            </tr>
            <tr id="trMemberStandardField" style="display:<%= (config!=null && (config.MemberStandard==2||config.MemberStandard==3))?"table-row":"none" %>;">
                <td style="width: 200px;" align="right" valign="middle">会员标准字段：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <a href="javascript:;" onclick="top.addTab('会员标准字段','/Admin/TableFieldMap/List.aspx?table_name=ZCJ_UserInfo&mapping_type=0')">字段设置</a>
                    <div class="hidden">
                    <% for (var i = 0; i < fieldList.Count; i++)
                       {
                           string disabledStr = disabledFields.Contains(fieldList[i].Value) ? "disabled=\"disabled\"" : "";
                           string checkedStr = disabledFields.Contains(fieldList[i].Value) || fieldList[i].Selected ? "checked=\"checked\"" : "";
                           %>
                            <input id="chkMemberStandardField<%=i %>" type="checkbox" name="chkMemberStandardField" <%=checkedStr %> <%=disabledStr %> value="<%=fieldList[i].Value %>" /><label for="chkMemberStandardField<%=i %>"><%=fieldList[i].Text %></label>
                      <% }%>                    
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">会员标准说明：
                </td>
                <td style="width: *;" align="left" valign="middle">
                        <div id="divMemberStandardDescription">
                            <div id="txtMemberStandardDescription" style="width: 375px; height: 360px;">
                                <%= config.MemberStandardDescription %>
                            </div>
                        </div>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" valign="middle">
                    <br />
                   
                    <input type="button" id="But_Ddata" onclick="DefaultData()" class="hidden button button-rounded button-primary"
                        style="width: 200px; margin-left: 200px;" value="生成默认数据" />
                </td>
            </tr>
            
        </table>
        </fileset>

        <fileset>
         <legend>高级功能配置</legend>
         <table width="100%" class="tdMain" >
            <tr>
                <td style="width: 200px;" align="right" valign="middle">文章有无评论：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input id="rdoHaveComment0" type="radio" name="rdoHaveComment" <%= (config==null || config.HaveComment==0)?"checked='checked'":"" %> value="0" /><label for="rdoHaveComment0">无评论</label>
                    <input id="rdoHaveComment1" type="radio" name="rdoHaveComment" <%= (config!=null && config.HaveComment==1)?"checked='checked'":"" %> value="1" /><label for="rdoHaveComment1">有评论</label>
                </td>
            </tr>

               <tr>
                 <td style="width: 200px;" align="right" valign="middle"><%--文章活动无权限的跳转方式：--%></td>
                 <td style="width: *;" align="left" valign="middle">
                     <div style="display:none;">
                         <input type="radio" name="page" value="0" <%= (config==null || config.NoPermissionsPage==0)?"checked='checked'":"" %>  id="noPermissionPage"/><label for="noPermissionPage">跳到无权限页</label>
                         <input type="radio" name="page" value="1" <%= (config!=null && config.NoPermissionsPage==1)?"checked='checked'":"" %>  id="memberPage"/><label for="memberPage">跳到会员标准页</label>
                     </div>
                 </td>
             </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">短信签名：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtSmsSignature" class="" style="width: 100%;" value="<%=currentWebsiteInfo.SmsSignature %>" placeholder="短信签名" />
                     
                </td>
            </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">余额申请提现最低限额：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtLowestAmount" class="" style="width: 100%;" value="<%=config.LowestAmount==0?50:config.LowestAmount%>" placeholder="提现最低限额" />
                </td>
            </tr>
              <tr>
                <td style="width: 200px;" align="right" valign="middle">优惠券核销码：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtHexiaoCode" class="" style="width: 200px;" value="<%=currentWebsiteInfo.HexiaoCode %>" placeholder="优惠券核销码" />
                     
                </td>
            </tr>
            
             <tr>
                <td style="width: 200px;" align="right" valign="middle">云信App Key：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtNIMAppKey" class="" style="width: 100%;" value="<%=currentWebsiteInfo.NIMAppKey%>" placeholder="云信App Key" />
                     
                </td>
            </tr>
             
             <tr>
                <td style="width: 200px;" align="right" valign="middle">云信App Secret：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtNIMAppSecret" class="" style="width: 100%;" value="<%=currentWebsiteInfo.NIMAppSecret%>" placeholder="云信App Secret" />
                     
                </td>
            </tr>
               <tr>
                <td style="width: 200px;" align="right" valign="middle">阿里App Key：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtAliAppKey" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AliAppKey%>" placeholder="阿里App Key" />
                     
                </td>
            </tr>
             
             <tr>
                <td style="width: 200px;" align="right" valign="middle">阿里App Secret：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtAliAppSecret" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AliAppSecret%>" placeholder="阿里App Secret" />
                     
                </td>
            </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">饿了么App Key：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtElemeAppKey"  style="width: 100%;" value="<%=currentWebsiteInfo.ElemeAppKey%>" placeholder="饿了么App Key" />
                     
                </td>
            </tr>
             
             <tr>
                <td style="width: 200px;" align="right" valign="middle">饿了么App Secret：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtElemeAppSecret"  style="width: 100%;" value="<%=currentWebsiteInfo.ElemeAppSecret%>" placeholder="饿了么App Secret" />
                     
                </td>
            </tr>
             
             <tr>
                <td style="width: 200px;" align="right" valign="middle">饿了么App Accesstoken：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtElemeAccessToken"  style="width: 30%;" value="<%=currentWebsiteInfo.ElemeAccessToken%>" placeholder="饿了么App Accesstoken" />
                    <input type="button" class="btn btn-default token" id="btnToKen" value="获取token"/>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">电话咨询(手机)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtTel" class="" style="width: 100%;" value="<%=config.Tel%>" placeholder="电话咨询(手机)" />
                     
                </td>
            </tr>
              <tr>
                <td style="width: 200px;" align="right" valign="middle">在线咨询(QQ)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtQQ" class="" style="width: 100%;" value="<%=config.QQ%>" placeholder="在线咨询(QQ)" />
                     
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">扫码替换上级是系统的分销员上级：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <select id="ddlDisableReplaceDistributonOwner"   class="form-control">
                        <option value="0">可以替换</option>
                        <option value="1">不可以替换</option>
                    </select>
                     
                </td>
            </tr>
         </table>
        </fileset>

        <fileset>
         <legend>App相关设置</legend>
         <table width="100%" class="tdMain" >
            <tr>
                <td style="width: 200px;" align="right" rowspan="5" valign="middle">
                    app推送：
                </td>
                <td style="width: 200px;" align="right" valign="middle">
                    类型：
                </td>
                <td align="left" valign="middle">
                    <select id="ddlAppPushType" class="form-control">
                        <option value="" <%= string.IsNullOrWhiteSpace(currentWebsiteInfo.AppPushType)?"selected=\"selected\"":"" %> >无</option>
                        <option value="getui" <%= currentWebsiteInfo.AppPushType == "getui"?"selected=\"selected\"":"" %>>个推</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right" valign="middle">
                    AppId：
                </td>
                <td align="left" valign="middle">
                     <input type="text" id="txtAppPushAppId" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AppPushAppId %>" placeholder="第三方AppId" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="middle">
                    AppKey：
                </td>
                <td align="left" valign="middle">
                     <input type="text" id="txtAppPushAppKey" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AppPushAppKey %>" placeholder="第三方AppKey" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="middle">
                    AppSecret：
                </td>
                <td align="left" valign="middle">
                     <input type="text" id="txtAppPushAppSecret" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AppPushAppSecret %>" placeholder="第三方AppSecret" />
                </td>
            </tr>
            <tr>
                <td align="right" valign="middle">
                    MasterSecret：
                </td>
                <td align="left" valign="middle">
                     <input type="text" id="txtAppPushMasterSecret" class="" style="width: 100%;" value="<%=currentWebsiteInfo.AppPushMasterSecret %>" placeholder="第三方MasterSecret" />
                </td>
            </tr>
         </table>
        </fileset>


        <fileset>
         <legend>客服</legend>
         <table width="100%" class="tdMain" >
            <tr>
                <td style="width: 200px;" align="right" rowspan="5" valign="middle">
                    客服设置：
                </td>
                <td style="width: 200px;" align="right" valign="middle">
                    启用/停用：
                </td>
                <td align="left" valign="middle">

                 <input type="radio" id="rdoDisableKefu0" name="rdoDisableKefu" value="0" checked="checked" /><label for="rdoDisableKefu0">启用</label>
                 <input type="radio" id="rdoDisableKefu1" name="rdoDisableKefu" value="1" /><label for="rdoDisableKefu1">禁用</label>

                </td>
            </tr>
            <tr id="trKefuUrl">
                <td align="right" valign="middle">
                    客服链接：
                </td>
                <td align="left" valign="middle">
                     <input type="text" id="txtKefuUrl" class="" style="width: 100%;" value="<%=config.KefuUrl %>" placeholder="客服链接" />
                </td>
            </tr>
             <tr id="trKefuImage">
                <td align="right" valign="middle">
                    客服图标：
                </td>
                <td align="left" valign="middle">
                     
                     <img alt="图片" src="<%=config==null?"":config.KefuImage %>" width="40px" height="40px"
                        id="imgKefu" /><br />
                    <a  href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtKefuImage.click()">上传图标</a>
                    <a onclick="$('#imgKefu').attr('src','')">使用默认图标</a>
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" /> 请上传JPG、PNG格式图标，图标最佳显示效果大小为20*20。
                    <input type="file" id="txtKefuImage" style="display: none;" name="fileKefu" />


                </td>
            </tr>
             <tr>
                 <td>客服在线回复内容</td>
                 <td> <input type="text" id="txtKefuOnLineReply" class="" style="width: 100%;" value="<%=config.KefuOnLineReply %>" placeholder="客服在线自动回复内容" /></td>
             </tr>
                 <tr>
                 <td>客服离线回复内容</td>
                 <td> <input type="text" id="txtKefuOffLineReply" class="" style="width: 100%;" value="<%=config.KefuOffLineReply %>" placeholder="客服离线自动回复内容" /></td>
             </tr>
         </table>
        </fileset>

          <fileset>
         <legend>后台登录页配置</legend>
         <table width="100%" class="tdMain" >
            <tr>
                <td style="width: 200px;" align="right" valign="middle">开启/关闭 自定义登录页：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input id="rdoEnableCustomizeLoginPage0" type="radio" name="rdoEnableCustomizeLoginPage" <%= (config.IsEnableCustomizeLoginPage==0)?"checked='checked'":"" %> value="0" /><label for="rdoEnableCustomizeLoginPage0">关闭</label>
                    <input id="rdoEnableCustomizeLoginPage1" type="radio" name="rdoEnableCustomizeLoginPage" <%= (config.IsEnableCustomizeLoginPage==1)?"checked='checked'":"" %> value="1" /><label for="rdoEnableCustomizeLoginPage1">开启</label>

                </td>
            </tr>

             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    系统名称：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtSystemName" class="" style="width: 100%;" value="<%=JTokenLogin != null ? JTokenLogin["system_name"].ToString() : ""%>" placeholder="系统名称" />
                     
                </td>
            </tr>

             <tr>

                 <td>登录背景图:</td>
                 <td>
                      <img alt="图片" src="<%=JTokenLogin!= null ? JTokenLogin["background_image"].ToString() : ""%>" width="300px"
                        id="imgLoginPageBackgroundImage" /><br />
                    <a  href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtLoginPageBackgroundImage.click()">上传背景图</a>
                    
                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" /> 请上传JPG 格式图片。建议尺寸:1230*1080
                    <input type="file" id="txtLoginPageBackgroundImage" style="display: none;" name="fileKefu" />
                      

                 </td>
             </tr>

             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    登录按钮背景色：
                </td>
                <td style="width: *;" align="left" valign="middle">
                     <input type="text" id="txtLoginBtnBackgroundColor" placeholder="登录按钮背景色" class="color" style="width: 80px;" value="<%=JTokenLogin != null ? JTokenLogin["login_btn_background_color"].ToString() : ""%>" />
                     
                </td>
            </tr>
         </table>
        </fileset>

         <fileset>
         <legend>门店配置</legend>
         <table width="100%" class="tdMain" >
            <tr>
                <td style="width: 200px;" align="right" valign="middle">门店搜索范围：
                </td>
                <td style="width: *;" align="left" valign="middle">
                   
                     <input type="number" id="txtOutletsSearchRange" placeholder="门店搜索范围"  style="width: 200px;" value="<%=config.OutletsSearchRange%>" />米
                </td>
            </tr>

            
         </table>
        </fileset>


        <div style="margin-left: 200px;" class="hidden">
            <input type="button" id="btncopy" class="button button-rounded button-primary" value="复制网站链接" />
        </div>
    </div>
    <br />
    <br />
    <br />
    <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 80px;
        line-height: 60px; text-align: center; width: 100%; background-color: rgb(245, 245, 245);
        padding-top: 14px; left: 0;">
        <a href="javascript:;" style="width: 200px; font-weight: bold; text-decoration: underline;"
            id="btnSave" onclick="Save();" class="button button-rounded button-primary">保存</a>
    </div>
    <div id="dlgSetField" class="easyui-dialog" closed="true" title="补充表单字段" style="width: 400px;
        padding: 15px;">
        <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
            <div style="margin-bottom: 5px">
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                    onclick="ShowAdd();" id="btnAddField">添加</a> <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEditField">编辑</a>
            </div>
        </div>
        <table id="grvSetField" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://static-files.socialcrmyun.com/Plugins/ZeroClipboard/jquery.zclip.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#btncopy').zclip({
                path: '/Plugins/ZeroClipboard/ZeroClipboard.swf',
                copy: $('#aweburl').html(),
                afterCopy: function () { alert("网站链接已经得到到剪贴板") }
            });

            $("#ddlDisableReplaceDistributonOwner").val("<%=currentWebsiteInfo.DisableReplaceDistributonOwner%>");


        });

    </script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var userCenterFieldJson = '<%=currentWebsiteInfo.UserCenterFieldJson %>';
        var userInfoFirstShow = '<%=currentWebsiteInfo.UserInfoFirstShow %>';
        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('#txtMemberStandardDescription', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });
    
        $(function () {
            ////选择标准时显示
            $('input[type="radio"][name="rdoMemberStandard"]').live("click", function () {
                var MemberStandard = $('input[type="radio"][name="rdoMemberStandard"]:checked').val();
                if (MemberStandard == 2 || MemberStandard == 3) {
                    $("#trMemberStandardField").show();
                }
                else {
                    $("#trMemberStandardField").hide();
                }
            });
            if (userCenterFieldJson) {
                var userCenterFields = JSON.parse(userCenterFieldJson);
                var fields = Object.keys(userCenterFields);
                for (var i = 0; i < fields.length; i++) {
                    var field = fields[i];
                    if ($('.userInfo_fields .userInfo_field #ck_' + field + '_show').get(0) && userCenterFields[field].show) {
                        $('.userInfo_fields .userInfo_field #ck_' + field + '_show').get(0).checked = userCenterFields[field].show;
                    }
                    if ($('.userInfo_fields .userInfo_field #ck_' + field + '_edit').get(0) && userCenterFields[field].edit) {
                        $('.userInfo_fields .userInfo_field #ck_' + field + '_edit').get(0).checked = userCenterFields[field].edit;
                    }
                    if ($('.userInfo_fields .userInfo_field .showname-' + field).get(0) && userCenterFields[field].showname) {
                        $('.userInfo_fields .userInfo_field .showname-' + field).val(userCenterFields[field].showname);
                    }
                }
            }
            $('#rdUserInfoFirstShow' + userInfoFirstShow).get(0).checked = true;

            if ("<%=config.IsDisableKefu%>" == "1") {
               
                $('#rdoDisableKefu1').attr("checked","checked");
                $("#trKefuUrl").hide();
                $("#trKefuImage").hide();
            }
            $("input[name='rdoDisableKefu']").click(function () {

                if ($(this).val()=="1") {
                    $("#trKefuUrl").hide();
                    $("#trKefuImage").hide();
                } else {
                    $("#trKefuUrl").show();
                    $("#trKefuImage").show();
                }



            })


            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });

            ////字段列表
            //$('#grvSetField').datagrid(
            //    {
            //        method: "Post",
            //        url: handlerUrl,
            //        queryParams: { Action: "QueryMemberTag", TagType: 'member' },
            //        height: 300,
            //        pagination: true,
            //        striped: true,
            //        rownumbers: true,
            //        columns: [[
            //            { title: 'ck', width: 5, checkbox: true },
            //            { field: 'TagName', title: '标签名称', width: 20, align: 'left' }
            //        ]]
            //    }
            //);
            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 imgThumbnailsPath.src = resp.ExStr;
                                 $('#imgThumbnailsPath').attr('path', resp.ExStr);
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
            //分销二维码小图标
            $("#txtQRcodeIconPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtQRcodeIconPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 iconThumbnailsPath.src = resp.ExStr;
                                 $('#iconThumbnailsPath').attr('path', resp.ExStr);
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

            //客服图标
            $("#txtKefuImage").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtKefuImage',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 imgKefu.src = resp.ExStr;
                                 //$('#imgKefu').attr('path', resp.ExStr);
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

            //登录页背景图
            $("#txtLoginPageBackgroundImage").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
                         secureuri: false,
                         fileElementId: 'txtLoginPageBackgroundImage',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                               
                                 $("#imgLoginPageBackgroundImage").attr('src', resp.ExStr);
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
            

            //获取token
            $('#btnToKen').click(function () {
                $.ajax({
                    type: 'get',
                    url: "/serv/api/admin/Website/Eleme/get.ashx",
                    data: { },
                    dataType: "json",
                    success: function (data) {
                        if (data.status) {
                            $('#txtElemeAccessToken').val(data.result);
                        } else {
                            Alert('获取token出错');
                        }
                    }
                });
            });

        })


        function DefaultData() {
            if (confirm("确定要初始化信息吗？此操作不能恢复！")) {
                $.ajax({
                    type: 'get',
                    url: "/Handler/App/CationHandler.ashx",
                    data: { Action: 'DefaultData' },
                    dataType: "json",
                    success: function (resp) {
                        Alert(resp.Msg);
                        location.reload();
                    }
                });
            } else {
                return false;
            }

        }

        function Save() {
            var MemberStandard = $('input[type="radio"][name="rdoMemberStandard"]:checked').val();
            var HaveComment = $('input[type="radio"][name="rdoHaveComment"]:checked').val();


            var dataModel = {
                WebsiteTitle: $.trim($("#txtWebsiteTitle").val()),
                Copyright: $.trim($("#txtCopyright").val()),
                WebsiteImage: $("#imgThumbnailsPath").attr("src"),
                ShopNavGroupName: $("#txtShopNavGroupName").val(),
                ShopAdType: $("#txtShopAdType").val(),
                Buttomtoolbars: $("#txtToolBar").val(),
                WebsiteDescription: $.trim($("#txtWebsiteDescription").val()),
                MemberStandard: MemberStandard,
                HaveComment: HaveComment,
                MemberStandardDescription: editor.html(),
                MyCardCouponsTitle: $("#txtMyCardCouponsTitle").val(),
                SmsSignature: $("#txtSmsSignature").val(),
                WeixinAccountNickName: $("#txtWeixinAccountNickName").val(),
                DistributionQRCodeIcon: $("#iconThumbnailsPath").attr("src"),
                ArticleToolBarGrous: $("#ddlArticleToolBarGroups").val(),
                ActivityToolBarGrous: $("#ddlActivityToolBarGrous").val(),
                GroupBuyIndexUrl: $("#txtGroupBuyIndexUrl").val(),
                NoPermissionsPage: $("input[type=radio][name=page]:checked").val(),
                PersonalCenterLink: $("#txtPersonalCenterLink").val(),
                WeiXinBindDomain: $("#txtWeiXinBindDomain").val(),
                LowestAmount: $("#txtLowestAmount").val(),
                HexiaoCode: $("#txtHexiaoCode").val(),
                NIMAppKey: $("#txtNIMAppKey").val(),
                NIMAppSecret: $("#txtNIMAppSecret").val(),
                AliAppKey: $("#txtAliAppKey").val(),
                AliAppSecret: $("#txtAliAppSecret").val(),
                Tel: $('#txtTel').val(),
                QQ: $('#txtQQ').val(),
                DisableReplaceDistributonOwner: $("#ddlDisableReplaceDistributonOwner").val(),
                UserInfoFirstShow: $("input[type=radio][name=rdUserInfoFirstShow]:checked").val(),
                MallOrderPaySuccessUrl: $("#txtMallOrderPaySuccessUrl").val(),
                ElemeAppKey: $('#txtElemeAppKey').val(),
                ElemeAppSecret: $('#txtElemeAppSecret').val(),
                IsDisableKefu: $("input[type=radio][name=rdoDisableKefu]:checked").val(),
                KefuUrl: $("#txtKefuUrl").val(),
                KefuImage: $("#imgKefu").attr("src"),
                KefuOnLineReply: $("#txtKefuOnLineReply").val(),
                KefuOffLineReply: $("#txtKefuOffLineReply").val(),
                IsEnableCustomizeLoginPage: $("input[type=radio][name=rdoEnableCustomizeLoginPage]:checked").val(),
                LoginConfigJson: getCustomizeLoginPage(),
                OutletsSearchRange:$("#txtOutletsSearchRange").val(),
                Action: 'UpdateCompanyWebsiteConfig'
            }

            if (dataModel.OutletsSearchRange != "") {
                if (parseInt(dataModel.OutletsSearchRange)<0) {
                    Alert("搜索范围不能为负数");
                    return false;
                }

            }
            
            if (dataModel.MemberStandard == 2 || dataModel.MemberStandard == 3) {
                var MemberStandardField = [];
                $('input[type="checkbox"][name="chkMemberStandardField"]:checked').each(function () {
                    MemberStandardField.push($(this).val());
                });
                dataModel.MemberStandardField = MemberStandardField.join(",");
            }
            dataModel.AppPushType =  $.trim($("#ddlAppPushType").val());
            if (dataModel.AppPushType == 'getui') {
                dataModel.AppPushAppId = $.trim($("#txtAppPushAppId").val());
                dataModel.AppPushAppKey = $.trim($("#txtAppPushAppKey").val());
                dataModel.AppPushAppSecret = $.trim($("#txtAppPushAppSecret").val());
                dataModel.AppPushMasterSecret = $.trim($("#txtAppPushMasterSecret").val());
            } else {
                dataModel.AppPushAppId = '';
                dataModel.AppPushAppKey = '';
                dataModel.AppPushAppSecret = '';
                dataModel.AppPushMasterSecret = '';
            }

            var fields = {};
            $(".userInfo_fields .userInfo_field").each(function () {
                var field = $(this).attr('data-field');
                var showChecked = false;
                var editChecked = false;
                var showName = '';
                if ($(this).find('#ck_' + field + '_show').get(0)) showChecked = $(this).find('#ck_' + field + '_show').get(0).checked;
                if ($(this).find('#ck_' + field + '_edit').get(0)) editChecked = $(this).find('#ck_' + field + '_edit').get(0).checked;
                if ($(this).find('.showname-' + field).get(0)) showName = $.trim($(this).find('.showname-' + field).val());
                
                if (showChecked || editChecked || showName) {
                    fields[field] = {};
                    if (showChecked) fields[field].show = showChecked;
                    if (editChecked) fields[field].edit = editChecked;
                    if (showName) fields[field].showname = showName;
                }
            });
            
            if (Object.keys(fields).length > 0) {
                dataModel.UserCenterFieldJson = JSON.stringify(fields);
            } else {
                dataModel.UserCenterFieldJson = '';
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType: "json",
                success: function (resp) {
                   // Alert(resp.Msg);
                    layerAlert(resp.Msg);
                }
            });

        }
        //补充表单对话框
        function ShowSetField() {
            $('#dlgSetField').dialog({ title: '设置标签' });
            $('#dlgSetField').dialog('open');
        }

        //获取自定义登录配置
        function getCustomizeLoginPage() {

            var loginPageConfig = {
                background_image: $("#imgLoginPageBackgroundImage").attr("src"),
                system_name:$("#txtSystemName").val(),
                login_btn_background_color: $("#txtLoginBtnBackgroundColor").val()
            };

            return JSON.stringify(loginPageConfig);

        }
    </script>
</asp:Content>
