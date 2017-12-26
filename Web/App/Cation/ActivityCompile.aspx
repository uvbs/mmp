<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivityCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ActivityCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="http://static-files.socialcrmyun.com/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="http://static-files.socialcrmyun.com/static-modules/lib/chosen/chosen.min.css"
        rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/static-modules/app/admin/article/style.css"
        rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/jquery/mCustomScrollbar/jquery.mCustomScrollbar.css"
        rel="stylesheet" />
    <link href="/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.css" rel="stylesheet" />
    <style type="text/css">
        body {
            font-family: 微软雅黑;
        }

        .tdTitle {
            font-size: 14px;
        }

        input[type=text] {
            height: 30px;
        }

        .panel.window {
            /*top: 1050px !important;*/
        }

        .activityitem {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 500px;
            position: relative;
        }

            .activityitem text {
                width: 400px;
            }

        #divActivityItem {
            display: none;
        }

        select {
            height: 30px;
        }

        input[name=itemdesc] {
            width: 350px;
        }

        #divRelationArticle {
            font-weight: bold;
        }

        .delrelation {
            margin-left: 10px;
            color: Blue;
        }


        /*.ActivityBox
        {
            margin-left: 100px;
        }*/
        .rightSlide {
            position: absolute;
            width: 100px;
            top: 24%;
            left: 80%;
        }

            .rightSlide .listyle {
                width: 82px;
                height: 28px;
                line-height: 26px;
                cursor: pointer;
                background-color: #C6C3BD;
                border-right-radius: 50%;
                -webkit-border-top-right-radius: 40px;
                -webkit-border-bottom-right-radius: 40px;
                text-align: left;
                margin-bottom: 10px;
                color: #fff;
                padding-left: 5px;
            }

            .rightSlide .listyleDeep {
                background-color: #46c8ff;
            }

        #dlgIosSkin {
            width: 300px;
            height: 622px;
            border-radius: 30px;
            padding: 10px;
            box-sizing: border-box;
            position: relative;
            background-color: #fff;
            box-shadow: 0 0 0 2px #eedeca, inset 0 0 4px #eedeca;
            left: 0;
            display: none;
        }

            #dlgIosSkin .preview-title {
                text-align: center;
                height: 10%;
            }

                #dlgIosSkin .preview-title .ico {
                    position: absolute;
                }

                #dlgIosSkin .preview-title .circle {
                    border-radius: 50%;
                }

                #dlgIosSkin .preview-title .ico-1 {
                    width: 6px;
                    height: 6px;
                    background-color: #000;
                    left: 50%;
                }

                #dlgIosSkin .preview-title .ico-2 {
                    width: 50px;
                    height: 6px;
                    background-color: #000;
                    left: 50%;
                    margin-left: -25px;
                    top: 30px;
                    border-radius: 3px;
                }

                #dlgIosSkin .preview-title .ico-3 {
                    width: 10px;
                    height: 10px;
                    background-color: #000;
                    left: 50%;
                    margin-left: -45px;
                    top: 28px;
                }

            #dlgIosSkin .preview-content {
                height: calc(69% - 50px);
                border: 1px solid #ccc;
                width: 92%;
                font-size: 12px !important;
                padding: 10px;
            }

            #dlgIosSkin .preview-footer {
                text-align: center;
                height: 10%;
            }

                #dlgIosSkin .preview-footer .circle {
                    width: 44px;
                    height: 44px;
                    border: 2px solid rgb(238, 222, 202);
                    margin-top: 14px;
                    margin-left: 114px;
                    border-radius: 50%;
                }

        body .layer-ext-iosskin {
            top: 20px !important;
            border-radius: 30px !important;
            overflow: hidden !important;
        }

            body .layer-ext-iosskin .layui-layer-content {
                height: 532px !important;
                overflow: hidden !important;
            }


        .lbTip {
            display: inline-block;
            padding: 0 6px;
            margin: 6px;
            background-color: #636060;
            color: #fff;
            font-size: 14px !important;
            border-radius: 50px;
            cursor: pointer;
            line-height: 1.42857143;
        }

        .layui-layer-tips i.layui-layer-TipsL, .layui-layer-tips i.layui-layer-TipsR {
            border-bottom-color: #5C5566 !important;
        }

        .layui-layer-content {
            background-color: #5C5566 !important;
        }

        .dcolor {
            background-color: #0face0 !important;
            color: #fff;
        }

            .dcolor:hover {
                background-color: #0face0;
                color: #fff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="ActivityManage.aspx">活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>活动<%if (model != null && webAction == "edit") { Response.Write("：" + model.ActivityName); } %></span>
    <a href="ActivityManage.aspx" style="float: right; margin-right: 20px;" title="返回活动列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动主题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" style="width: 100%;" placeholder="活动主题(必填)" maxlength="150" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" style="width: 100%;" placeholder="将显示在微信分享描述中" maxlength="300" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动地点：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityAddress" class="" style="width: 100%;" placeholder="活动举办地点" maxlength="200" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">活动积分：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityIntegral" style="width: 100px;" placeholder="活动积分" />提示：不填或者填0
                        则不需要活动积分
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">最多报名人数：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMaxSignUpTotalCount" style="width: 100px;" placeholder="最多报名人数" />提示：不填或者填0
                        则不限制报名人数
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动开始时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datetimebox" style="width: 150px;" editable="false" id="txtActivityStartDate" />
                        必填 点击左侧图标选择时间
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动结束时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datetimebox" style="width: 150px;" editable="false" id="txtActivityEndDate" />
                        选填 点击左侧图标选择时间
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            活动详情：</label>
                    </td>
                    <td align="left">
                        <span id="tabs">
                            <button class="button tab dcolor">默认详情</button>
                        </span>

                        <a href="javascript:;" class="easyui-linkbutton" id="addTab" iconcls="icon-add2" plain="true">添加TAB扩展内容</a>

                        <span class="lbTip" data-tip-msg="<b>示例</b><br><img src='/img/example/article_desc.png'/><br>">?</span>
                    </td>
                </tr>
                <tr>
                    <td width="*" align="left" colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <div id="zcWeixinKindeditor" style="float: right;">
                                    </div>
                                </td>
                                <td style="vertical-align: top; position: relative;">
                                    <div id="txtEditor">
                                    </div>
                                    <div class="rightSlide">
                                        <ul>

                                            <%--  <li class="listyle listyleDeep" onclick="importWord()">
                                                <span style="margin-left: 15px;">Word导入</span>
                                            </li>--%>
                                            <li class="listyle listyleDeep" onclick="saveData()">
                                                <span style="margin-left: 25px;">保存</span>
                                            </li>
                                            <li class="listyle listyleDeep" id="btnIosPreview">
                                                <span style="margin-left: 25px;">预览</span>
                                            </li>
                                            <li class="listyle">
                                                <span style="margin-left: 25px;" onclick="ClearEditer()">清空</span>
                                            </li>

                                            <%--<li class="listyle" style="display: none"><span class="icon iconfont icon-iconfontjihualiebiao">
                                            </span><span style="margin-left: 5px;">复制</span> </li>
                                            <li class="listyle" style="display: none"><span class="icon iconfont icon-xinwenzixun">
                                            </span><span style="margin-left: 5px;">抓取图文</span> </li>
                                            <li class="listyle" style="display: none"><span class="icon iconfont icon-article"></span>
                                                <span style="margin-left: 5px;">存为模板</span> </li>--%>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--<tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            活动详情：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsHide" id="rdoPro" checked="checked" v="-1" /><label for="rdoPro">待开始</label>
                        <input type="radio" name="IsHide" id="rdoIsNotHide" checked="checked" v="0" /><label
                            for="rdoIsNotHide">进行中</label>
                        <input type="radio" name="IsHide" id="rdoIsHide" v="1" /><label for="rdoIsHide">停止</label>
                        <img alt="" src="http://static-files.socialcrmyun.com/MainStyle/Res/easyui/themes/icons/tip.png" />已停止的活动不能进行在线报名
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        显示在 作者的其它发布：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsHideRecommend" id="rdShowRecommend" v="0" /><label for="rdShowRecommend">显示</label>
                        <input type="radio" name="IsHideRecommend" id="rdHideRecommend" v="1" checked="checked" /><label
                            for="rdHideRecommend">不显示</label>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动分类：
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">在报名页面显示报名人数列表：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="ShowPersonnelList" id="rdShowPersonnelList1" value="1"
                            checked="checked" /><label for="rdShowPersonnelList1">显示</label>
                        <input type="radio" name="ShowPersonnelList" id="rdShowPersonnelList0" value="0" /><label
                            for="rdShowPersonnelList0">不显示</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr id="trShowPersonnelListType">
                    <td style="width: 100px;" align="right" class="tdTitle">报名人数列表 姓名显示方式：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="ShowPersonnelListType" id="rdShowPersonnelListType0" value="0"
                            checked="checked" /><label for="rdShowPersonnelListType0">显示姓名全名</label>
                        <input type="radio" name="ShowPersonnelListType" id="rdShowPersonnelListType1" value="1" /><label
                            for="rdShowPersonnelListType1">只显示姓氏,名字用*代替</label>
                    </td>
                </tr>
                <%-- <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>--%>
                <tr style="display: none;">
                    <td style="width: 100px;" align="right" class="tdTitle">有人报名通知到以下客服:
                    </td>
                    <td width="*" align="left">
                        <select id="ddlActivityNoticeKeFu" style="width: 200px;">
                            <option value="">无</option>
                            <%=sbActivityNoticeKeFuList.ToString()%>
                        </select>
                        <a href="javascript:;" style="color: Blue" onclick="ShowAdd();" id="btnAdd">点击添加</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动报名成功后跳转链接:
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="width: 100%;" value="<%=model.ActivitySignuptUrl %>" id="txtActivitySinupUrl" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <tr style="display: none;">
                    <td style="width: 100px; vertical-align: top;" align="right" class="tdTitle">标签：
                    </td>
                    <td width="*" align="left">
                        <input id="txtTags" />
                        <a href="javascript:;" class="button button-primary button-rounded button-small"
                            id="btnSelectTags">选择标签</a>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">阅读数:
                    </td>
                    <td>
                        <input type="text" id="txtPv" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" value="<%=model.PV %>" class="commonTxt" placeholder="阅读数"
                            style="width: 100px;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">访问权限级别：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAccessLevel" class="commonTxt" value="<%=model.AccessLevel %>"
                            placeholder="访问权限级别" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'') " />&nbsp;权限访问级别为数字，数值越高用户需要的级别越高(默认为0,即所有用户都可以访问)
                        <a href="/App/Cation/UserManage.aspx" style="color: blue;">设置用户访问级别</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">免费/收费：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlIsFee" style="width: 200px;">
                            <option value="0">免费</option>
                            <option value="1">收费</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">活动模板：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlarticletemplate" style="width: 200px">
                            <option value="7">默认模板</option>
                            <%-- <option value="4">简洁模板(收费)</option>--%>
                            <option value="10">简洁模板</option>
                            <%--(免费)--%>
                            <option value="8">申请模板</option>
                             
                            <%--     <option value="1">默认模板</option>
                            <option value="0">无</option>
                           
                          <option value="3">新模板(仅适用于微信认证服务号且有高级认证)</option>
                            <option value="5">新模板(无报名)</option>--%>
                            <%--  <option value="9">收费模板</option>--%>
                           <%-- <%if (model.WebsiteOwner == "totema" || model.WebsiteOwner == "study" || model.WebsiteOwner == "songhe")
                              {%>--%>
                            <%-- <option value="13">提交信息</option>--%>
                            <%-- <% } %>--%>
                            
                        </select>
                    </td>
                </tr>
                <hr />
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">可见区域：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdoArea" class="positionTop2" value="0" checked="checked"
                            id="rdoHide" /><label for="rdoHide">隐藏</label>
                        <input type="radio" name="rdoArea" class="positionTop2" value="1" id="rdoShow" /><label
                            for="rdoShow">显示</label>
                    </td>
                </tr>
                <tr class="area">
                    <td style="width: 100px;" align="right" class="tdTitle">显示条件：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdoCondition" class="positionTop2" value="0" checked="checked"
                            id="outSignup" /><label for="outSignup">报名之后可见</label>
                        <input type="radio" name="rdoCondition" class="positionTop2" value="1" id="outPay" /><label
                            for="outPay" id="outPay1">支付之后可见</label>
                    </td>
                </tr>
                <tr class="area">
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">显示内容：
                    </td>
                    <td width="*" align="left">
                        <div id="successEditor">
                            <div id="txtsuccessEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">相关阅读： <a href="javascript:;" class="button button-primary button-rounded button-small"
                        onclick="ShowArticleDlg();" style="padding-left: 10px; padding-right: 10px;">选择相关阅读</a><br />
                        <a href="javascript:;" class="button button-primary button-rounded button-small"
                            style="padding-left: 10px; margin-top: 10px; padding-right: 10px;" onclick="clearRead()">全部清除</a>
                    </td>
                    <td width="*" align="left">
                        <div id="divRelationArticle">
                            <%
                                
                                StringBuilder sbRelation = new StringBuilder();
                                foreach (var item in RelationArticle)
                                {
                                    sbRelation.AppendFormat("<div data-relationarticleid=\"{0}\">", item.JuActivityID);
                                    sbRelation.AppendFormat(item.ActivityName);
                                    sbRelation.AppendFormat("<label class=\"delrelation\">删除</label> ");
                                    sbRelation.AppendFormat("</div> ");
                                    sbRelation.AppendFormat("<br/>");
                                }
                                Response.Write(sbRelation.ToString());
                               
                            %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">相关商品： <a href="javascript:;" class="button button-primary button-rounded button-small"
                        onclick="ShowProductDlg();" style="padding-left: 10px; padding-right: 10px;">选择相关商品</a><br />
                        <a href="javascript:;" class="button button-primary button-rounded button-small"
                            style="padding-left: 10px; margin-top: 10px; padding-right: 10px;" onclick="clearRead1()">全部清除</a>
                    </td>
                    <td width="*" align="left">
                        <div id="divRelationArticle1">
                            <%
                                StringBuilder sbRelation1 = new StringBuilder();
                                foreach (var item in RelationProduct)
                                {
                                    sbRelation1.AppendFormat("<div data-relationarticleid1=\"{0}\">", item.PID);
                                    sbRelation1.AppendFormat(item.PName);
                                    sbRelation1.AppendFormat("<label class=\"delrelation\">删除</label> ");
                                    sbRelation1.AppendFormat("</div> ");
                                    sbRelation1.AppendFormat("<br/>");
                                }
                                Response.Write(sbRelation1.ToString());
                               
                            %>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div id="divActivityItem">
                <strong style="font-size: 20px;">收费选项:</strong>
                <%for (int i = 0; i < ActivityItems.Count; i++)
                  {%>
                <div class="activityitem">
                    <img src="/img/delete.png" style="float: right;" class="deleteitem" />
                    <table style="width: 100%; margin-left: 10px;">
                        <tr>
                            <td style="width: 100px;">选项名称:
                            </td>
                            <td>
                                <input type="hidden" name="itemid" value="<%=ActivityItems[i].ItemId %>" />
                                <input type="text" name="itemname" placeholder="选项名称" value="<%=ActivityItems[i].ProductName %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>金额:
                            </td>
                            <td>
                                <input type="text" name="itemamount" placeholder="金额(必填)" value="<%=ActivityItems[i].Amount %>" />元
                            </td>
                        </tr>
                        <tr>
                            <td>原价:</td>
                            <td>
                                <input type="text" name="itemprice" onkeyup="value=value.replace(/[^\d]/g,'') " value="<%=ActivityItems[i].OriginalPrice %>" placeholder="原价(选填)" />元
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px;">描述:
                            </td>
                            <td>
                                <input type="text" name="itemdesc" placeholder="描述" value="<%=ActivityItems[i].Description %>" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%}%>
                <div class="activityitem">
                    <img src="/img/delete.png" style="float: right;" class="deleteitem" />
                    <table style="width: 100%; margin-left: 10px;">
                        <tr>
                            <td style="width: 100px;">选项名称:
                            </td>
                            <td>
                                <input type="hidden" name="itemid" value="0" />
                                <input type="text" name="itemname" placeholder="选项名称" />
                            </td>
                        </tr>
                        <tr>
                            <td>金额:
                            </td>
                            <td>
                                <input type="text" name="itemamount" placeholder="金额(必填)" />元
                            </td>
                        </tr>
                        <tr>
                            <td>原价:</td>
                            <td>
                                <input type="text" name="itemprice" onkeyup="value=value.replace(/[^\d]/g,'') " placeholder="原价(选填)" />元
                            </td>
                        </tr>
                        <tr>
                            <td>描述:
                            </td>
                            <td>
                                <input type="text" name="itemdesc" placeholder="描述" />
                            </td>
                        </tr>
                    </table>
                </div>
                <a class="button button-rounded button-primary" style="width: 500px; margin-top: 10px; margin-bottom: 10px;"
                    id="btnAddItem">添加收费选项</a>
            </div>
            <input type="hidden" id="hdRelationArticle" value="<%=model.RelationArticles %>" />
            <div style="margin-top: 32px; padding-top: 16px; padding-bottom: 16px; text-align: center; background-color: rgb(245, 245, 245); position: relative;">
                <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;"
                    class="button button-rounded button-flat">重置</a>
            </div>
        </div>
    </div>
    <div class="hidden warpTagDiv" style="border-radius: 8px; display: none;">
        <div class="warpTagSelect">
            <div class="warpContent">
                <div class="warpTagDataList hidden">
                    <div class="warpTagSelectBtn">
                        <a href="javascripe:;" class="mLeft15 btnTagSelect" data-op="all">全选</a><a href="javascripe:;"
                            class="mLeft10 btnTagSelect" data-op="reverse">反选</a>
                    </div>
                    <ul class="ulTagList">
                    </ul>
                </div>
                <div class="warpNoData">
                    暂无数据
                </div>
                <div class="clear">
                </div>
            </div>
            <hr />
            <div class="warpOpeate">
                <a href="javascript:;" class="button button-primary button-rounded button-small btnSave">确定</a> <a href="javascript:;" class="button button-rounded button-small btnCancel">取消</a>
            </div>
        </div>
    </div>
    <div id="kefuInfo" class="easyui-dialog" closed="true" modal="false" title="添加客服(双击选择)"
        style="width: 450px;">
        <div>
            姓名<input type="text" id="txtTrueName" style="width: 300px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" id="search">搜索</a>
        </div>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
    <div id="dlgArticle" class="easyui-dialog" closed="true" modal="true"
        style="width: 550px; height: 400px;">
        <br />
        <div>
            <%--  <span id="articleKeyword">

                <input type="radio" class="positionTop2" value="0" checked="checked" name="ptype" id="type1" /><label for="type1">文章活动</label>
            </span>
            <span id="productKeyword">

                <input type="radio" class="positionTop2"  value="1" id="type2" name="ptype" /><label for="type2">商品</label>
            </span>--%>
            &nbsp;<input type="text" id="txtArticle" placeholder="标题" style="width: 300px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" id="btnSearchArticle">搜索</a>
        </div>
        <br />
        <div class="article">
            <table id="grvArticle" fitcolumns="true">
            </table>
        </div>
        <div class="product">
            <table id="grvProduct" fitcolumns="true"></table>
        </div>
    </div>
    <div id="dlgIosSkin">
        <div class="preview-title">
            <div class="ico ico-1 circle">
            </div>
            <div class="ico ico-2">
            </div>
            <div class="ico ico-3 circle">
            </div>
        </div>
        <div class="preview-content">
            <div class="item">
            </div>
        </div>
        <div class="preview-footer">
            <div class="circle">
            </div>
        </div>
    </div>
    <div id="dlgTabTitle" class="easyui-dialog" closed="true" modal="true" title="请输入Tab标题"
        style="width: 400px; height: 160px;">
        <div style="margin: 25px;">
            <label>Tab标题:</label>
            <input type="text" style="width: 250px; margin-left: 10px;" maxlength="5" id="tabTitle" />

        </div>
    </div>
    <div style="display: none" id="ActivityDescription"><%=model.ActivityDescription %></div>
    <div style="display: none" id="VisibleContext"><%=model.VisibleContext %></div>
    <div style="display: none" id="TabExContent1"><%=model.TabExContent1 %></div>
    <div style="display: none" id="TabExContent2"><%=model.TabExContent2 %></div>
    <div style="display: none" id="TabExContent3"><%=model.TabExContent3 %></div>
    <div style="display: none" id="TabExContent4"><%=model.TabExContent4 %></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://static-files.socialcrmyun.com/static-modules/lib/tagsinput/jquery.tagsinput.js"
        type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/static-modules/lib/chosen/chosen.jquery.min.js"
        type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/jquery/mCustomScrollbar/jquery.mCustomScrollbar.concat.min.js"
        type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.js"
        type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script src="/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";//处理文件
        var currAction = '<%=webAction%>';//当前操作
        var currAcvityID = '<%=model.JuActivityID %>';//当前ID
        var editor;//编辑器
        var successEditor;
        var $document = $(document);
        var currTagsStr = '<%=model.Tags %>';//当前标签
        var currTags = [];
        var $warpTagSelect = $document.find('.warpTagSelect');
        var $txtTags = $document.find('#txtTags');
        var currSelectID = 0;//当前ID
        var aid=<%=aid%>;//自动ID
        var itemCount = 1; //问题数量
        var zcWeixinKindeditor;
        var isProduct='';
        var titles = [];
        var contents=[];
        var desc='';
  
        $(function () {
            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
           
            if (currAction == 'add') {
                GetRandomHb();
            }
            else {
                ShowEdit();
            }
            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, '.lbTip');
            });

            $('#addTab').click(function () {

                if (titles.length >= 4) {
                    layer.msg('Tab自定义标题最多添加4个');
                    return;
                }
                $('#dlgTabTitle').dialog('open');
                $('#tabTitle').val('');
                $('#tabTitle').focus();
            });
            $('#dlgTabTitle').dialog({
                buttons: [{
                    text: '添加',
                    handler: function () {

                        var tabTitle = $.trim($('#tabTitle').val());

                        if (tabTitle == '') {
                            layer.msg('请输入Tab标题');
                            return;
                        }
                    
                        var index = parseInt(titles.length);

                        var button = '<button class="button tab del"  style="margin-left:5px;position: relative;">' + tabTitle + '<img src="/img/deltab.png" class="deltab" title="删除" style="position: absolute;right: 0;" /></button>';

                        $('#tabs').append(button);

                        titles.push(tabTitle);

                        contents.push('默认tab内容');

           

                        $('#dlgTabTitle').dialog('close');

                    }
                }, {

                    text: '取消',
                    handler: function () {

                        $('#dlgTabTitle').dialog('close');
                    }
                }]
            });


            $(document).on('click','.deltab',function(){

                $(this).parent().remove();

                var bText=$(this).parent().text();

                var index=titles.indexOf(bText);
              
                if(index>=0){
                    titles.splice(index,1);
                    contents.splice(index,1);
                }
            });

            $(document).on('click', '.tab', function () {

                $(this).addClass('dcolor').siblings().removeClass('dcolor');

                var bText=$(this).text();

                var index=titles.indexOf(bText);
              
                if(index<0){
                    editor.html(desc);
                }else{
                    editor.html(contents[index]);
                }

              
            });

            //$('input[name=ptype]').click(function(){
            //    var kk=$(this).val();
            //    if(kk==1){
            //        $('.article').hide();
            //        $('.product').show();
            //    }else{
            //        $('.product').hide();
            //        $('.article').show();
            //    }
            //});


            if($("#ddlIsFee").val()==0){
                $("#outPay").hide();
                $("#outPay1").hide();
            }else{
                $("#outPay").show();
                $("#outPay1").show();
            }

            $("#ddlIsFee").change(function(){
                if($("#ddlIsFee").val()==0){
                    $("#outPay").hide();
                    $("#outPay1").hide();
                }else{
                    $("#outPay").show();
                    $("#outPay1").show();
                }
            })

           
            

            $("#rdoHide").click(function(){
                $(".area").hide();
                successEditor.html('');
                $("#outSignup").attr("checked",true);
            })
            $("#rdoShow").click(function(){
                $(".area").show();
            })

            //保存按钮
            $('#btnSave').click(function () {

                saveData();
            });

            //重置按钮
            $('#btnReset').click(function () {
                ResetCurr();
            });

            //显示报名列表
            $("input[name='ShowPersonnelList']").click(function () {
                if ($(this).val()=="1") {
                    $("#trShowPersonnelListType").show();
                }
                else {
                    $("#trShowPersonnelListType").hide();
                }
            });
            
            //活动缩略图
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
                                 imgThumbnailsPath.src = resp.ExStr;
                                 
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     });

                } catch (e) {
                    alert(e);
                }
            });


            //处理初始化tags
            if (currTagsStr != '') {
                currTags = currTagsStr.split(',');
            }

            $txtTags.tagsInput({
                height: '60px',
                width: 'auto',
                interactive: false,
                onAddTag: function (tag) {
                    //currTags.push(tag);
                    //console.log('添加了' + tag);
                },
                onRemoveTag: function (tag) {
                    currTags.RemoveItem(tag);
                    //console.log('删除了' + tag);
                }
            });
            addTagList(currTags);
            //标签操作按钮
            $document.on('click', '#btnSelectTags', function () {
                loadSelectTagsData();
            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnSave', function () {

                //构造标签新数组
                currTags = [];
                var chekList = $('.warpTagSelect .tagChk');
                for (var i = 0; i < chekList.length; i++) {
                    if ($(chekList[i]).attr('checked')) {
                        currTags.push($(chekList[i]).val());
                    }
                }

                //显示标签
                tagClear();
                addTagList(currTags);

                layer.closeAll();

            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnCancel', function () {

                layer.closeAll();

            });

            $document.on('click', '.warpTagSelect .btnTagSelect ', function () {
                var op = $(this).attr('data-op');

                if (op == 'all')
                    selectTagAll();

                if (op == 'reverse')
                    selectTagReverse();

            });

            //会员列表
            $('#grvUserInfo').datagrid(
                  {
                      onDblClickRow: function (rowIndex, rowData) {
                          if(rowData["TrueName"]==""||rowData["TrueName"]==null){
                              alert('缺少姓名'); 
                              return;
                          }
                          if(rowData["WXOpenId"]==""||rowData["WXOpenId"]==null){
                              alert('缺少OpenID'); 
                              return;
                          }
                          var dataModel = {
                              Action: "AddKeFu",
                              AutoID: currSelectID,
                              TrueName: rowData["TrueName"],
                              Phone: rowData["Phone"],
                              WeiXinOpenID: rowData["WXOpenId"]
                          }
                          $.ajax({
                              type: 'post',
                              url: "/Handler/App/CationHandler.ashx",
                              data: dataModel,
                              dataType: "json",
                              success: function (resp) {
                                  if (resp.Status == 1) {
                                      $('#kefuInfo').dialog('close');
                                      $("#ddlActivityNoticeKeFu").append("<option selected='selected' value='"+resp.ExStr+"'>"+dataModel.TrueName+"</option>");
                                  }
                                  else {
                                      Alert(resp.Msg);
                                  }
                              }
                          });
                      },
                      loadMsg : "正在加载数据",
                      method: "Post",
                      height: 280,
                      pagination: true,
                      striped: true,
                      singleSelect:true,
                      pageSize: 10,
                      rownumbers: true,

                      columns: [[
                                  { field: 'TrueName', title: '姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                  { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                  
                                  { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                      ]]

                     
                  }
              );

            //会员搜索
            $("#search").click(function(){
                var txtTrueName=$("#txtTrueName").val();
                $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser',HaveTrueName: 1,KeyWord: txtTrueName} });
            });

           
            //添加收费选项
            $("#btnAddItem").click(function () {

                var appendHtml = new StringBuilder();
                appendHtml.AppendFormat("<div class=\"activityitem\" data-item-index=\"{0}\">", itemCount);
                appendHtml.AppendFormat("<img src=\"/img/delete.png\" style=\"float: right;\" class=\"deleteitem\" />");
                appendHtml.AppendFormat("<table style=\"width: 100%; margin-left: 10px;\">");
                appendHtml.AppendFormat("<tr>");
                appendHtml.AppendFormat("<td style=\"width: 100px;\">");
                appendHtml.AppendFormat(" 选项名称:");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("<input type=\"hidden\" name=\"itemid\" value=\"0\"/>");
                appendHtml.AppendFormat(" <input type=\"text\" name=\"itemname\" placeholder=\"选项名称\" />");
                appendHtml.AppendFormat(" </td>");
                appendHtml.AppendFormat("</tr>");

                //金额
                appendHtml.AppendFormat("<tr>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("金额:");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("<input type=\"text\" name=\"itemamount\"   placeholder=\"金额(必填)\" />元");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("</tr>");
                
                //原价
                appendHtml.AppendFormat("<tr>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("原价:");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("<input type=\"text\" name=\"itemprice\" onkeyup=\"value=value.replace(/[^\\d]/g,'')\"  placeholder=\"原价(必填)\" />元");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("</tr>");
                //描述
                appendHtml.AppendFormat("<tr>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("描述:");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("<td>");
                appendHtml.AppendFormat("<input type=\"text\" name=\"itemdesc\" placeholder=\"描述\"/>");
                appendHtml.AppendFormat("</td>");
                appendHtml.AppendFormat("</tr>");

                appendHtml.AppendFormat("</table>");
                appendHtml.AppendFormat("</div>");
                $(this).before(appendHtml.ToString());
               
            });
            //添加收费选项

            //删除收费选项
            $('.deleteitem').live("click", function () {
                if ($('.deleteitem').length <= 1) {
                    Alert("至少添加一个选项");
                    return false;
                }
                $(this).parent().remove();
            });
            //删除收费选项


            //是否收费下拉框改变
            $(ddlIsFee).change(function(){
            
                if ($(this).val()=="1") {
                    $(divActivityItem).show();
                    LoadFeeTemplate();
                }
                else {
                    $(divActivityItem).hide();
                    LoadFreeTemplate();
                }
            
            
            })
            $('#dlgArticle').dialog({
                modal: false,
                buttons: [{
                    text: '添加',
                    handler: function () {
                        if(isProduct=='article'){
                            var rows = $('#grvArticle').datagrid('getSelections');
                            if (rows.length==0) {
                                return false;
                            }
                            var sbRelation = new StringBuilder();
                            for (var i = 0; i < rows.length; i++) {
                                if(i>10) continue;
                                sbRelation.AppendFormat("<div data-relationarticleid=\"{0}\">", rows[i].JuActivityID);
                                sbRelation.AppendFormat(rows[i].ActivityName);
                                sbRelation.AppendFormat("<label class=\"delrelation\">删除</label> ");
                                sbRelation.AppendFormat("</div> ");
                                sbRelation.AppendFormat("<br/>");
                            }
                            $("#divRelationArticle").append(sbRelation.ToString());
                            $('#dlgArticle').dialog('close');
                        }else{
                            var rows = $('#grvProduct').datagrid('getSelections');
                            if (rows.length==0) {
                                return false;
                            }

                            var sbRelation = new StringBuilder();
                            for (var i = 0; i < rows.length; i++) {
                                if(i>10) continue;
                                sbRelation.AppendFormat("<div data-relationarticleid1=\"{0}\">", rows[i].product_id);
                                sbRelation.AppendFormat(rows[i].product_title);
                                sbRelation.AppendFormat("<label class=\"delrelation\">删除</label> ");
                                sbRelation.AppendFormat("</div> ");
                                sbRelation.AppendFormat("<br/>");
                            }
                            $("#divRelationArticle1").append(sbRelation.ToString());
                            $('#dlgArticle').dialog('close');
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgArticle').dialog('close');
                    }
                }]
            });
            
            //搜索文章或商品
            $("#btnSearchArticle").click(function () {
                var activityName=$("#txtArticle").val();

                if(isProduct=='article'){
                    $('#grvArticle').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "", ActivityName: activityName }
                    });
                }else{
                    $('#grvProduct').datagrid(
                   {
                       method: "Post",
                       url: '/serv/api/admin/Mall/Product.ashx',
                       queryParams: { Action: "List",'keyword':activityName}
                   });
                }

            
            })

            ////加载商品列表[推荐文章或活动]
            $("#grvArticle").datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "", ArticleTypeEx1: "", CategoryId: 0 },
	                height:400,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ThumbnailsPath', title: '缩略图', width: 10, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'ActivityName', title: '标题', width: 85, align: 'left'
                                }

	                ]]
	            }
            );
            //加载商品列表[推荐商品]
            $("#grvProduct").datagrid(
              {
                  method: "Post",
                  url: '/serv/api/admin/Mall/Product.ashx',
                  queryParams: { Action: "List",keyword:''},
                  height:400,
                  pagination: true,
                  striped: true,
                  pageSize: 50,
                  loadFilter: pagerFilter1,
                  singleSelect: false,
                  rownumbers: true,
                  columns: [[
                              { title: 'ck', width: 5, checkbox: true },
                              { field: 'img_url', title: '缩略图', width: 10, align: 'center', formatter: function (value) {
                                  if (value == '' || value == null)
                                      return "";
                                  var str = new StringBuilder();
                                  str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                  return str.ToString();
                              }
                              },
                              { field: 'product_title', title: '标题', width: 85, align: 'left'
                              }

                  ]]
              }
          );

            //删除关联文章
            $('.delrelation').live('click', function () {
                var obj = $(this).closest("div");
                $(obj).remove();
            });
            
            //预览按钮
            $document.on('click','#btnIosPreview',function(){
                loadPagePreview();
            });

        });
        
        //弹出预览框
        var $previewContent =  $document.find('#dlgIosSkin').find('.preview-content');
        function loadPagePreview(){
            $previewContent.find('.item').html(editor.html());
            $previewContent.mCustomScrollbar({ theme: "minimal-dark" });
            var previewDiv = layer.open({
                type: 1,
                title: false,
                scrollbar:false,
                closeBtn: 0,
                shadeClose: true,
                skin: 'layer-ext-iosskin',
                content: $('#dlgIosSkin')
            });
        }
        function ShowAdd() {
 
            $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser',HaveTrueName: 1 } });
            $('#kefuInfo').dialog('open');
            $("#kefuInfo").window("move", { top: $(document).scrollTop() + ($(window).height() - 450) * 0.5 });
        }




        function ShowEdit() 
        {
            var tabExTitle1 = '<%=model.TabExTitle1%>';
            var tabExContent1 =$('#TabExContent1').html();
            var tabExTitle2 = '<%=model.TabExTitle2%>';
            var tabExContent2 =$('#TabExContent2').html();
            var tabExTitle3 = '<%=model.TabExTitle3%>';
            var tabExContent3 =$('#TabExContent3').html();
            var tabExTitle4 = '<%=model.TabExTitle4%>';
            var tabExContent4 =$('#TabExContent4').html();
            desc =$('#ActivityDescription').html();
            console.log('desc',desc);
            if (tabExTitle1 != '') {
                var html = '<button class="button tab del"  style="margin-left:5px;position: relative;">' + tabExTitle1 + '<img src="/img/deltab.png" class="deltab" title="删除" style="position: absolute;right: 0;" /></button>';
                $('#tabs').append(html);
                titles.push(tabExTitle1);
                contents.push(tabExContent1);
            }
            if (tabExTitle2 != '') {
                var html ='<button class="button tab del"  style="margin-left:5px;position: relative;">' + tabExTitle2 + '<img src="/img/deltab.png" class="deltab" title="删除" style="position: absolute;right: 0;" /></button>';
                $('#tabs').append(html);
                titles.push(tabExTitle2);
                contents.push(tabExContent2);

            }
            if (tabExTitle3 != '') {
                var html ='<button class="button tab del"  style="margin-left:5px;position: relative;">' + tabExTitle3 + '<img src="/img/deltab.png" class="deltab" title="删除" style="position: absolute;right: 0;" /></button>';
                $('#tabs').append(html);
                titles.push(tabExTitle3);
                contents.push(tabExContent3);
            }
            if (tabExTitle4 != '') {
                var html = '<button class="button tab del"  style="margin-left:5px;position: relative;">' + tabExTitle4 + '<img src="/img/deltab.png" class="deltab" title="删除" style="position: absolute;right: 0;" /></button>';
                $('#tabs').append(html);
                titles.push(tabExTitle4);
                contents.push(tabExContent4);
            }
           
            $('#txtActivityName').val("<%=model.ActivityName%>");
            $('#txtActivityAddress').val("<%=model.ActivityAddress %>");
            $("#ddlcategory").val("<%=model.CategoryId %>");
            
            $("#ddlIsFee").val("<%=model.IsFee %>");
            if ($("#ddlIsFee").val()=="1") {
                $("#divActivityItem").show();
                LoadFeeTemplate();
            }
            else {
                LoadFreeTemplate();
            }
            $('#ddlarticletemplate').val("<%=model.ArticleTemplate %>");
            $("#txtSummary").val("<%=model.Summary %>");
            $("#txtMaxSignUpTotalCount").val("<%=model.MaxSignUpTotalCount %>");
                             <% if (model.ActivityStartDate != null)
                                {%>
            $('#txtActivityStartDate').datetimebox('setValue','<%=model.ActivityStartDate.Value.ToString("yyyy-MM-dd HH:mm") %>');
            <%} %>
                             <% if (model.ActivityEndDate != null)
                                {%>
            $('#txtActivityEndDate').datetimebox('setValue','<%=model.ActivityEndDate.Value.ToString("yyyy-MM-dd HH:mm") %>');
                            <%} %>
            $('#imgThumbnailsPath').attr('src', "<%=model.ThumbnailsPath %>");
            $("#ddlActivityNoticeKeFu").val("<%=model.ActivityNoticeKeFuId %>");
            $("#txtActivityIntegral").val("<%=model.ActivityIntegral%>");
                             

            if (<%=model.IsHide%>== 1){
                rdoIsHide.checked = true;}
            else if(<%=model.IsHide%>==-1){
                rdoPro.checked = true;}
            else{
                rdoIsNotHide.checked = true;}

            if (<%=model.IsShowPersonnelList %> == 1){
                rdShowPersonnelList1.checked = true;}
            else{
                rdShowPersonnelList0.checked = true;
                $("#trShowPersonnelListType").hide();
            }
                                
                               
            if (<%=model.ShowPersonnelListType%>== 1){
                rdShowPersonnelListType1.checked = true;}
            else{
                rdShowPersonnelListType0.checked = true;
                               
            }
            if(<%=model.VisibleArea%>==0){
                $(".area").hide();
                rdoHide.checked=true;
            }else{
                rdoShow.checked=true;
                if(<%=model.ShowCondition%>==0){
                    outSignup.checked=true;
                }else{
                    outPay.checked=true;
                }
                
            }
            //
            //$.ajax({
            //    type: 'post',
            //    url: handlerUrl,
            //    dataType:"json",
            //    data: { Action: 'GetSingelJuActivity', JuActivityID: currAcvityID },
            //    success: function (resp) {
            //        try {
            //            if (resp.Status == 1) {
            //                var model = resp.ExObj;
            //                editor.html(model.ActivityDescription);
            //                successEditor.html(model.VisibleContext);

            //            }
            //            else {
            //                Alert(resp.Msg);
            //            }
            //        } catch (e) {
            //            Alert(e);
            //        }
            //    }

            //});
            //



        }


        function ResetCurr() {
            ClearAll();
            editor.html('');
            titles=[];
            contents=[];
            $("#divRelationArticle").html("");
            $("#divRelationArticle1").html("");
            $('.del').each(function(k,v){
                var index=$(v).attr('data-index');
                var item=$(this);
                if(index>0){
                    item.remove();
                }
            });
            successEditor.html('');
            $('.del').remove();
        }

        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $(".area").hide();
           
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                afterFocus : function() {
                    //GetTabContent();
                },
                <%if (Request.Browser.Browser.Contains("Firefox"))
                  {%>
                extraFileUploadParams: { userID: '<%= new ZentCloud.BLLJIMP.BLL().GetCurrUserID() %>' },
                <%}%>
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'importword','video', 'image', 'multiimage', 'link', 'unlink','lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                filterMode: false,
                width: "80%",
                height:"600px",
                cssPath: ['/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css','/Weixin/ArticleTemplate/css/comm.css'],
                afterCreate:function(){
                    //var ss = $('#ActivityDescription').html();
                    zcWeixinKindeditor = $("#zcWeixinKindeditor").zcWeixinKindeditor({ keditor: this, def_cate:'1216' });
                    var dhtml = document.getElementById("ActivityDescription").innerHTML
                    this.html(dhtml);
                },
                afterChange:function(){
                    //这个方法为什么初始化就会调用，并且此时变量 editor 的值为undefined
                    //这个方法为什么只要在编辑器里面点击一次就会调用一次，内容没变啊
                    var bText = $('.dcolor').text();
                    var index=titles.indexOf(bText);
                    if(index<0){
                        desc=this.html();
                    }else{
                        contents[index]=this.html();
                    }
                }
            });
            
            successEditor = K.create('#txtsuccessEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',               
                extraFileUploadParams: { userID: '<%= new ZentCloud.BLLJIMP.BLL().GetCurrUserID() %>' },               
                items: [
                    'source', '|',  'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink','lineheight', '|', 'baidumap', '|', 'table'],
                filterMode: false,
                afterCreate:function(){
                    var dhtml = document.getElementById("VisibleContext").innerHTML
                    this.html(dhtml);
                }
            });
        });

        var $ulTagList = $warpTagSelect.find('.ulTagList'), $warpNoTagData = $warpTagSelect.find('.warpNoData'), $warpTagDataList = $warpTagSelect.find('.warpTagDataList');
        function loadSelectTagsData() {
            $.ajax({
                type: 'POST',
                url: '/Handler/App/CationHandler.ashx',
                data: { Action: "QueryMemberTag", TagType: '', page: 1, rows: 100000000 },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if (data.total == 0) {
                        $warpNoTagData.show();
                        $warpTagDataList.hide();
                    } else {
                        $warpTagDataList.show();
                        $warpNoTagData.hide();

                        //构造数据
                        var strHtml = new StringBuilder();
                        for (var i = 0; i < data.rows.length; i++) {
                            strHtml.Append('<li class="overflow_ellipsis"><label>');
                            strHtml.AppendFormat('<input type="checkbox" name="tag" class="tagChk" value="{0}" {1} />{0}', data.rows[i].TagName, currTags.Contains(data.rows[i].TagName) ? 'checked' : '');
                            strHtml.Append('</label></li>');
                        }
                        $ulTagList.html(strHtml.ToString());
                    }
                    var tagDiv = layer.open({
                        type: 1,
                        shade: [0.2, '#000'],
                        shadeClose: true,
                        area: ['300', '320'],
                        title: ['选择标签', 'background:#1B9AF7; color:#fff;'],
                        border: [0],
                        content: $('.warpTagDiv')
                    });


                }
            });
        }

        function GetTabContent() {

            setInterval(function () {

                var bText = $('.dcolor').text();

                console.log(bText);

                var index=titles.indexOf(bText);

                if(index<0){
                    desc=editor.html();
                }else{
                    contents[index]=editor.html();
                }

            }, 1000);
        }

        function addTagList(list) {
            for (var i = 0; i < list.length; i++) {
                if (!$txtTags.tagExist(list[i]))
                    $txtTags.addTag(list[i]);
            }
        }

        //清除标签
        function tagClear() {
            $txtTags.importTags('');
        }

        //标签全选
        function selectTagAll() {
            $('.warpTagSelect .tagChk').attr('checked', true);
        }

        //标签反选
        function selectTagReverse() {
            $('.warpTagSelect .tagChk').each(function () {
                var $this = $(this),
                    v = $this.attr('checked');
                $this.attr('checked', !v);
            });
        }

        //获取活动收费选项列表
        function GetItemData() {
            var itemList=[];
            $(".activityitem").each(function () {
                var item = {
                    ItemId:0,
                    ItemName: '',
                    ItemAmount: 0,
                    ItemPrice:0,
                    ItemDesc: ''
                }; //问题模型

                item.itemId=$(this).find("input[name='itemid']").first().val();
                item.itemName= $(this).find("input[name='itemname']").first().val();
                item.itemAmount= $(this).find("input[name='itemamount']").first().val();
                item.ItemPrice= $(this).find("input[name='itemprice']").first().val();
                item.itemDesc= $(this).find("input[name='itemdesc']").first().val();

                if (item.itemName!=""&&item.itemAmount!="") {
                    itemList.push(item);
                }
            });
            var itemObj={
                ItemList:itemList
            
            };
            return itemObj;
            
        }

        //文章选择框
        function ShowArticleDlg() {
            $("#dlgArticle").dialog('open');
            isProduct='article';
            $('.product').hide();
            $('.article').show();
            $('#dlgArticle').panel({title: "选择关联文章和活动"});
            $("#dlgArticle").window("move",{top:$(document).scrollTop() + ($(window).height()-400) * 0.5});  
        }

        //商品选择框
        function ShowProductDlg(){
            $("#dlgArticle").dialog('open');
            isProduct='product';
            $('.article').hide();
            $('.product').show();
            $('#dlgArticle').panel({title: "选择关联商品"});
            $("#dlgArticle").window("move",{top:$(document).scrollTop() + ($(window).height()-400) * 0.5}); 
        }

        //获取设置的关联文章
        function GetRelationArticleIds() {
            ids = [];
            $("[data-relationarticleid]").each(function () {
                ids.push($(this).data("relationarticleid"));
            });
            return ids.join(",");
        }
        function GetRelationProductIds(){
            var pids=[];
            $("[data-relationarticleid1]").each(function () {
                pids.push($(this).data("relationarticleid1"));
            });
            return pids.join(",");
        }
        //加载免费模板
        function LoadFreeTemplate(){
        
            var appendHtml = new StringBuilder();
            appendHtml.AppendFormat("<option value=\"7\">默认模板</option>");
            appendHtml.AppendFormat("<option value=\"10\">简洁模板</option>");
            appendHtml.AppendFormat("<option value=\"8\">申请模板</option>");
            //appendHtml.AppendFormat("<option value=\"13\">提交信息</option>");
            $(ddlarticletemplate).html(appendHtml.ToString());
        }

            //加载收费模板
            function LoadFeeTemplate(){
                var appendHtml = new StringBuilder();
                appendHtml.AppendFormat("<option value=\"9\">默认模板</option>");
                appendHtml.AppendFormat("<option value=\"4\">简洁模板</option>");
                appendHtml.AppendFormat("<option value=\"12\">Tab模板</option>");
                //appendHtml.AppendFormat("<option value=\"13\">提交信息</option>");
                $(ddlarticletemplate).html(appendHtml.ToString());
        
            }


            //保存或编辑
            function saveData() {
                var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
                if ($btnSave.hasClass('disabled ')) {
                    return;
                }
                if ($("#btnSave").attr("disabled") == "disabled") {
                    return;
                }
                $("#btnSave").attr({ "disabled": "disabled" });
                $btnSave.addClass('disabled').text('正在处理...');
                $btnReset.addClass('disabled');

                try {
                    //保存前清空选中
                    zcWeixinKindeditor.clearEditorSelect();
                    var model =
                    {
                        IsSignUpJubit: 1,
                        ActivityName: $.trim($('#txtActivityName').val()),
                        ActivityWebsite: "",
                        ActivityStartDate: $('#txtActivityStartDate').datetimebox('getValue'),
                        ActivityEndDate: $('#txtActivityEndDate').datetimebox('getValue'),
                        ActivityDescription: desc, 
                        ThumbnailsPath: $('#imgThumbnailsPath').attr('src'),
                        Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                        JuActivityID: currAcvityID,
                        IsHide: rdoIsHide.checked ? 1 : 0,
                        alluser:1,
                        IsByWebsiteContent: 0,
                        ArticleType: 'activity',
                        ArticleTemplate:$("#ddlarticletemplate").val(),
                        IsSpread: 1,
                        ActivityAddress: $.trim($('#txtActivityAddress').val()),
                        ActivityNoticeKeFuId:$('#ddlActivityNoticeKeFu').val(),
                        CategoryId:$("#ddlcategory").val(),
                        Summary:$("#txtSummary").val(),
                        IsShowPersonnelList:$("input[name='ShowPersonnelList']:checked").val(),
                        ShowPersonnelListType:$("input[name='ShowPersonnelListType']:checked").val(),
                        ActivityIntegral:$.trim($("#txtActivityIntegral").val()==""?"0":$("#txtActivityIntegral").val()),
                        MaxSignUpTotalCount:$.trim($("#txtMaxSignUpTotalCount").val()==""?"0":$("#txtMaxSignUpTotalCount").val()),
                        Tags: currTags.join(','),
                        PV:$("#txtPv").val(),
                        AccessLevel:$(txtAccessLevel).val(),
                        ActivitySinupUrl:$("#txtActivitySinupUrl").val(),
                        IsFee:$("#ddlIsFee").val(),
                        RelationArticle:GetRelationArticleIds(),
                        RelationProduct:GetRelationProductIds(),
                        ItemListJson:JSON.stringify(GetItemData()),
                        VisibleArea:$("input[name=rdoArea]:checked").val(),
                        ShowCondition:$("input[name=rdoCondition]:checked").val(),
                        VisibleContext:successEditor.html(),
                        TabExTitle1: titles[0]==undefined?null:titles[0],
                        TabExContent1: contents[0]==undefined?null:contents[0],
                        TabExTitle2: titles[1]==undefined?null:titles[1],
                        TabExContent2: contents[1]==undefined?null:contents[1],
                        TabExTitle3: titles[2]==undefined?null:titles[2],
                        TabExContent3:contents[2]==undefined?null:contents[2],
                        TabExTitle4: titles[3]==undefined?null:titles[3],
                        TabExContent4: contents[3]==undefined?null:contents[3],

                    };

                    if(model.ActivityName.length>150){
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        $("#btnSave").removeAttr("disabled");
                        Alert('标题字数过长');
                        return;
                    }
                    if(model.ActivitySinupUrl!=''){
                        if(!IsURL(model.ActivitySinupUrl)){
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            Alert('跳转链接格式不正确');
                            return;
                        }
                    }



                    if (model.IsFee=="1") {//收费
                        if (model.ArticleTemplate!="4"&&model.ArticleTemplate!="9"&&model.ArticleTemplate!="12") {
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            Alert('收费活动暂时只支持 简洁模板与收费模板,请重新选择模板');
                            return;
                        }
                        if(GetItemData().ItemList.length==0){
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            Alert('收费选项不允许为空');
                            return;
                        }
                        
                       
                    }
                    if (model.IsFee=="0") {//免费
                        if (model.ArticleTemplate=="4"||model.ArticleTemplate=="9") {
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            Alert('此模板暂不支持免费,请重新选择模板');
                            return;
                        }
                        
                       
                    }

                    if (rdoPro.checked) {//待开始
                        model.IsHide=-1;
                    }

                    if (model.ActivityName == '') {
                        $('#txtActivityName').focus();
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        $("#btnSave").removeAttr("disabled");
                        //Alert('请输入活动名称！');
                        return;
                    }
                    if (model.ActivityStartDate == '') {
                        Alert('请选择活动开始时间！');
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        $("#btnSave").removeAttr("disabled");
                        return;
                    }

                    if(model.ActivityEndDate!=''){
                        if(model.ActivityStartDate>=model.ActivityEndDate){
                            Alert('活动结束时间必须大于开始时间！');
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            return;
                        }
                    }
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType:"json",
                        success: function (resp) {
                            $.messager.progress('close');
                            $btnReset.removeClass('disabled');
                            $btnSave.removeClass('disabled').text('保存');
                            $("#btnSave").removeAttr("disabled");
                            if (resp.Status == 1) {
                                Alert(resp.Msg);
                                if (currAction == 'add')
                                    ResetCurr();
                               
                            }
                            
                        }
                    });
                } catch (e) {
                    $btnReset.removeClass('disabled');
                    $btnSave.removeClass('disabled').text('保存');
                    $("#btnSave").removeAttr("disabled");
                    Alert(e);
                }
            }

            function ClearEditer() {
                editor.html('');
            }

            function importWord() {
                editor.clickToolbar('importword');
            }

            //全部清除
            function clearRead(){
                $("#divRelationArticle").html("");
                if (!RegUrl.test(str)) { 
                    return false; 
                } 
                return true; 
            }

            //全部清除
            function clearRead(){
                $("#divRelationArticle").html("");
            }

            //全部清除
            function clearRead1(){
                $("#divRelationArticle1").html("");
            }
            //easyui格式转换
            function pagerFilter1(result) {
                var data = result;
                return {
                    total: data.totalcount,
                    rows: data.list
                };
            }
    </script>
</asp:Content>
