<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.Template.EditPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>页面编辑</title>
    <link href="http://static-files.socialcrmyun.com/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/layer.mobile/need/layer.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="http://static-files.socialcrmyun.com/kindeditor-4.1.10/themes/plugins.css" rel="stylesheet" type="text/css" />
    <%--扩展组件样式--%>
    <link href="/customize/comeoncloud/m2/dist/all.min.css?v=2017022501" rel="stylesheet" />
    <link href="/Plugins/angular/zcShop/zcShop.css?v=2017030301" rel="stylesheet" />
    <link href="/Plugins/angular/zcSelectLink/zcSelectLink.css?v=2017022501" rel="stylesheet" />
    <link href="/admin/component/css/app_simulate.css?v=2017022502" rel="stylesheet" />
    <link href="/admin/component/css/shop_drag.css?v=2017022502" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/jquery/slider/jquery-ui-slider.min.css" rel="stylesheet" />
</head>
<body>
    <div class="editBody" ng-app="editComponentModule"
        ng-controller="editComponentCtrl">
        <div class="qrcode" style="display: none;">
            <p>微信“扫一扫”浏览</p>
            <img class="qrcodeImg" width="120" height="120">
            <a class="qrcodeCopy" href="javascript:void(0);">复制链接</a>
        </div>
        <table class="component-table">
            <tr>
                <td class="left-col">
                    <div class="form-div m-top fixeddrag">
                        <table class="form-table">
                            <tr class="shareset">
                                <td>可选组件列表（点击添加）
                                </td>
                            </tr>
                            <tr class="marginTop">
                                <td class="tdComponentModelButton draglist">
                                    <div class="dragsingle" ng-click="vmFunc.selectRightControl(1)">
                                        <div ng-bind-html="'icon-shezhikongxin'|FormatterIcon">
                                        </div>
                                        <span>页面设置</span>
                                    </div>
                                    <div class="component-model-button dragsingle"
                                        ng-class="{'component-model-button-disabled':field.disabled }"
                                        ng-repeat="field in vm.component_model.component_model_fields"
                                        ng-click="vmFunc.addControl(field,null,true)"
                                        ng-if="field.component_field!='sidemenubox'">
                                        <div ng-bind-html="'icon-dcjia'|FormatterIcon">
                                        </div>
                                        <span ng-bind="field.component_field_name"></span>
                                        <!--<i class="pull-right right-arrow"></i>-->
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="submitDiv">
                        <button class="button button-calm" ng-click="vmFunc.saveTemplateDialog()">保存</button>
                        <button class="button button-stable" ng-click="vmFunc.toListPage()">返回</button>
                    </div>
                </td>
                <td class="main-col">
                    <div class="app-inner clearfix">
                        <div class="app-init-container">
                            <div class="app_content js-app-main">
                                <div class="app-design clearfix">
                                    <div class="app-preview">
                                        <div class="app-header"></div>
                                        <div class="app-entry">
                                            <div class="app-config js-config-region">
                                                <div class="app-field clearfix" ng-click="vmFunc.selectRightControl(1)">
                                                    <h1><span ng-bind="vm.only_contorls.pageinfo.title"></span></h1>
                                                </div>
                                            </div>
                                            <div class="js-fields-region" style="{{vmFunc.getPageBg(vm.only_contorls.pageinfo.bg_img,vm.only_contorls.pageinfo.bg_img_style,vm.only_contorls.pageinfo.bg_color)}}">
                                                <div class="app-fields headerbarBgPosition" ng-if="vm.only_contorls.headerbar" control-id="{{vm.only_contorls.headerbar.control_id}}">
                                                    <div class="app-field clearfix" ng-class="{ 'editing':vm.only_contorls.headerbar.selected}" ng-click="vmFunc.selectRightControl(vm.only_contorls.headerbar)">
                                                        <div class="control-group">
                                                            <headerbar class="warpHeaderBar" ng-attr-confg="vm.only_contorls.headerbar.config"></headerbar>
                                                        </div>
                                                        <div class="actions">
                                                            <div class="actions-wrap">
                                                                <span class="action delete" ng-click="vmFunc.deleteControl(vm.only_contorls.headerbar)">删除</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="app-fields ui-sortable">
                                                    <div class="app-field clearfix" ng-class="{ 'editing':control.selected}" control-id="{{control.control_id}}"
                                                        ng-repeat="control in vm.right_controls" ng-click="vmFunc.selectRightControl(control)"
                                                        ng-if="control.control!='sidemenubox' && control.config">
                                                        <div class="control-group" ng-switch="control.control">
                                                            <search ng-switch-when="search"></search>
                                                            <notice key="{{control.control_id}}" ng-switch-when="notice"></notice>
                                                            <userinfo ng-switch-when="userinfo"></userinfo>
                                                            <slide key="{{control.control_id}}" ng-switch-when="slides" ng-if="control.data && control.data.length>0"></slide>
                                                            <navs key="{{control.control_id}}" ng-switch-when="navs" ng-if="control.data && control.data.length>0"></navs>
                                                            <tabs key="{{control.control_id}}" ng-switch-when="tabs" ng-if="control.data && control.data.length>0"></tabs>
                                                            <malls key="{{control.control_id}}" ng-switch-when="malls" ng-if="control.config.mall_list && control.config.mall_list.length>0"></malls>
                                                            <cardlist key="{{control.control_id}}" ng-switch-when="cardlist" ng-if="control.config.card_data && control.config.card_data.list && control.config.card_data.list.length>0"></cardlist>
                                                            <activitylist key="{{control.control_id}}" ng-switch-when="activitylist" ng-if="control.config.activity_list && control.config.activity_list.length>0"></activitylist>
                                                            <content key="{{control.control_id}}" ng-switch-when="content"></content>
                                                            <block key="{{control.control_id}}" ng-switch-when="block"></block>
                                                            <linetext key="{{control.control_id}}" ng-switch-when="linetext"></linetext>
                                                            <linebutton key="{{control.control_id}}" ng-switch-when="linebutton"></linebutton>
                                                            <linehead key="{{control.control_id}}" ng-switch-when="linehead"></linehead>
                                                        </div>
                                                        <div class="actions sortable">
                                                            <div class="actions-wrap">
                                                                <%--<span class="action handle" sv-handle>拖动</span>--%>
                                                                <span class="action delete" ng-click="vmFunc.deleteControl(control)">删除</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="app-fields toolBarBgPosition" control-id="{{vm.only_contorls.totop.control_id}}" ng-if="vm.only_contorls.totop">
                                                    <div class="app-field clearfix" ng-class="{ 'editing':vm.only_contorls.totop.selected}" ng-click="vmFunc.selectRightControl(vm.only_contorls.totop)">
                                                        <div class="control-group">
                                                            <totop></totop>
                                                        </div>
                                                        <div class="actions">
                                                            <div class="actions-wrap">
                                                                <span class="action delete" ng-click="vmFunc.deleteControl(vm.only_contorls.totop)">删除</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="app-fields toolBarBgPosition" control-id="{{vm.only_contorls.foottool.control_id}}" ng-if="vm.only_contorls.foottool">
                                                    <div class="app-field clearfix" ng-class="{ 'editing':vm.only_contorls.foottool.selected}" ng-click="vmFunc.selectRightControl(vm.only_contorls.foottool)">
                                                        <div class="control-group">
                                                            <foottool ng-if="vm.only_contorls.foottool.data && vm.only_contorls.foottool.data.length>0"></foottool>
                                                        </div>
                                                        <div class="actions">
                                                            <div class="actions-wrap">
                                                                <span class="action delete" ng-click="vmFunc.deleteControl(vm.only_contorls.foottool)">删除</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="notify-bar js-notify animated hinge hide">
                        </div>
                    </div>
                </td>
                <td class="right-col">
                    <div class="right-inner">
                        <div ng-if="vm.show_page_set">
                            <div class="form-div">
                                <%--<i class="icon iconfont icon-shixinjiantouzuo arrowleft"></i>--%>
                                <table class="form-table">
                                    <tr>
                                        <td class="form_td">
                                            <label class="shop_name">页面标题：</label>
                                            <div class="shop_choose">
                                                <input class="shop_text" type="text" ng-model="vm.only_contorls.pageinfo.title" placeholder="页面标题" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_td">
                                            <label class="shop_name">页面背景色：</label>
                                            <div class="shop_choose">
                                                <input class="shop_text color" type="text" ng-model="vm.only_contorls.pageinfo.bg_color" control-control="pageinfo" control-id="pageinfo" attr-key="bg_color" placeholder="页面背景色" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="picture">
                                            <div class="pic_choose imgnav-info">
                                                <div class="formitems">
                                                    <label class="fi-name">
                                                        点击上传背景图片：
                                                    </label>
                                                    <div class="form-controls">
                                                        <img alt="背景图片" ng-src="{{vm.only_contorls.pageinfo.bg_img}}" class="imgUpload imgnav pgBgImg" onerror="this.removeAttribute('src')" />
                                                        <input type="file" name="file1" class="file" style="display: none;" data-model="vm.only_contorls.pageinfo.bg_img" />
                                                        <button class="button button-assertive button-small" ng-click="vmFunc.clearBgImg()">删除背景图</button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="pic_choose imgnav-info">
                                                <div class="formitems">
                                                    <label class="fi-name">背景图样式：</label>
                                                    <div class="form-controls">
                                                        <label class="shop_label" for="rdoRightBottom">
                                                            <input id="rdoRightBottom" type="radio" ng-model="vm.only_contorls.pageinfo.bg_img_style" value="1" class="positionTop2">
                                                            <span>右下角填充</span>
                                                        </label>
                                                        <label class="shop_label" for="rdoCover">
                                                            <input id="rdoCover" type="radio" ng-model="vm.only_contorls.pageinfo.bg_img_style" value="2" class="positionTop2">
                                                            <span>撑满全屏</span>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div ng-show="vm.kindeditorControl != null">
                            <div class="form-div">
                                <%--<i class="icon iconfont icon-shixinjiantouzuo arrowleft"></i>--%>
                                <table class="form-table">
                                    <tr>
                                        <td class="form_td">
                                            <label class="shop_name" style="float: none;">内容</label>
                                            <div class="shop_choose" style="width: 400px; margin-left: 0px;">
                                                <div id="txtEditor">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="deleteDiv">
                                    <button class="button button-assertive" ng-click="vmFunc.deleteControl(vm.kindeditorControl)">删除</button>
                                </div>
                            </div>
                        </div>
                        <div ng-repeat="control in vm.cur_select_controls">
                            <div class="form-div" ng-class="{'m-top':$index>0}">
                                <%--<i class="icon iconfont icon-shixinjiantouzuo arrowleft" ng-if="control.control != 'sidemenubox'"></i>--%>
                                <table class="form-table" ng-if="control.control=='foottool' || control.control=='tabs'">
                                    <tr>
                                        <td class="form_td">
                                            <label class="shop_name" ng-bind="control.control_name+'：'"></label>
                                            <div class="shop_choose">
                                                <div class="radio-group" ng-if="vm.toolbars.length<=4">
                                                    <label class="shop_label" ng-repeat="item in vm.toolbars">
                                                        <input type="radio" name="toolbarone" ng-model="control.config.key_type" value="{{item.key_type}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.key_type"></span>
                                                    </label>
                                                </div>
                                                <select class="selectbox" ng-if="vm.toolbars.length>4" ng-model="control.config.key_type" ng-options="model.key_type as model.key_type for model in vm.toolbars" ng-change="vmFunc.editControlData(control)">
                                                </select>
                                                <label class="shop_label shop_a" ng-click="vmFunc.showAddType(control)">
                                                    <span>新增</span>
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr ng-if="control.control=='foottool' && vm.component.ComponentKey =='MallHome'">
                                        <td class="form_td">
                                            <label class="shop_name">首页显示：</label>
                                            <div class="shop_choose">
                                                <div class="radio-group" ng-repeat="item in [{text:'显示',value:'1'},{text:'隐藏',value:'0'}]">
                                                    <label class="shop_label">
                                                        <input type="radio" name="style" ng-model="vm.only_value_controls.foottool_home_show" value="{{item.value}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.text"></span>
                                                    </label>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <!--control.type:-1为导航数组；0为文本；1为选项-->
                                <table class="form-table" ng-if="control.type>=4 && control.type!=7 && control.type!=8 && control.control!='content'">
                                    <tr ng-repeat="attr in control.attrs" ng-if="!(control.control=='headerbar' && attr.key=='left_btn' && control.config.sidemenu_button == '1')">
                                        <td class="form_td">
                                            <label class="shop_name" ng-bind="attr.name+'：'"></label>
                                            <div class="shop_choose" ng-if="attr.type==-1">
                                                <div class="radio-group" ng-if="control.control =='slides' && vm.slides.length<=4">
                                                    <label class="shop_label" ng-repeat="item in vm.slides">
                                                        <input type="radio" name="slide" ng-model="control.config.slide_list" value="{{item}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item"></span>
                                                    </label>
                                                    <label class="shop_label shop_a" ng-click="vmFunc.showAddType(control)">
                                                        <span>新增</span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='slides' && vm.slides.length>4">
                                                    <select class="selectbox" ng-model="control.config.slide_list" ng-options="model for model in vm.slides" ng-change="vmFunc.editControlData(control)">
                                                    </select>
                                                    <label class="shop_label shop_a" ng-click="vmFunc.showAddType(control)">
                                                        <span>新增</span>
                                                    </label>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='navs'&&vm.toolbars.length<=4">
                                                    <label class="shop_label" ng-repeat="item in vm.toolbars">
                                                        <input type="radio" name="navs" ng-model="control.config.nav_list" value="{{item.key_type}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.key_type"></span>
                                                    </label>
                                                    <label class="shop_label shop_a" ng-click="vmFunc.showAddType(control)">
                                                        <span>新增</span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='navs'&&vm.toolbars.length>4">
                                                    <select class="selectbox" ng-model="control.config.nav_list" ng-options="model.key_type as model.key_type for model in vm.toolbars" ng-change="vmFunc.editControlData(control)">
                                                    </select>
                                                    <label class="shop_label shop_a" ng-click="vmFunc.showAddType(control)">
                                                        <span>新增</span>
                                                    </label>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='malls' && attr.key=='cate'">
                                                    <div class="checkLi" ng-class="{'checkLiTitle':item.pre_id==0}" ng-if="control.config.is_group_buy!=2" ng-repeat="item in vm.mall_cates">
                                                        <label class="shop_label">
                                                            <input type="checkbox" name="chkmallcate" ng-checked="vmFunc.isChecked(control.config.cate,item.cate_id)" value="{{item.cate_id}}" ng-click="vmFunc.checkboxControlData(control,'cate',$event)">
                                                            <span ng-bind="item.cate_name|FormatterCateName"></span>
                                                        </label>
                                                    </div>
                                                    <div class="checkLi" ng-class="{'checkLiTitle':item.pre_id==0}" ng-if="control.config.is_group_buy==2" ng-repeat="item in vm.course_cates">
                                                        <label class="shop_label">
                                                            <input type="checkbox" name="chkmallcate" ng-checked="vmFunc.isChecked(control.config.cate,item.cate_id)" value="{{item.cate_id}}" ng-click="vmFunc.checkboxControlData(control,'cate',$event)">
                                                            <span ng-bind="item.cate_name|FormatterCateName"></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='malls' && attr.key=='tag'">
                                                    <div class="checkLi" ng-repeat="item in vm.mall_tags">
                                                        <label class="shop_label">
                                                            <input type="checkbox" name="chkmalltag" ng-checked="vmFunc.isChecked(control.config.tag,item)" value="{{item}}" ng-click="vmFunc.checkboxControlData(control,'tag',$event)">
                                                            <span ng-bind="item"></span>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='malls' && attr.key=='sort_tag' && vm.mall_tags.length<=4">
                                                    <label class="shop_label">
                                                        <input type="radio" name="malltag" ng-model="control.config[attr.key]" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span>所有</span>
                                                    </label>
                                                    <label class="shop_label" ng-repeat="item in vm.mall_tags">
                                                        <input type="radio" name="malltag" ng-model="control.config[attr.key]" value="{{item}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item"></span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='malls' && attr.key=='sort_tag' && vm.mall_tags.length>4">
                                                    <select class="selectbox" ng-model="control.config[attr.key]" ng-options="model as model for model in vm.mall_tags" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">所有</option>
                                                    </select>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='cardlist' && attr.key=='cate_id' && vm.art_cates.length<=4">
                                                    <label class="shop_label">
                                                        <input type="radio" name="malltag" ng-model="control.config.cate_id" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span>所有</span>
                                                    </label>
                                                    <label class="shop_label" ng-repeat="item in vm.art_cates">
                                                        <input type="radio" name="malltag" ng-model="control.config.cate_id" value="{{item.cate_id}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.cate_name"></span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='cardlist' && attr.key=='cate_id' && vm.art_cates.length>4">
                                                    <select class="selectbox" ng-model="control.config.cate_id" ng-options="model.cate_id as model.cate_name for model in vm.art_cates" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">所有</option>
                                                    </select>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='activitylist' && attr.key=='cate_id' && vm.act_cates.length<=4">
                                                    <label class="shop_label">
                                                        <input type="radio" name="artcate" ng-model="control.config.cate_id" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span>所有</span>
                                                    </label>
                                                    <label class="shop_label" ng-repeat="item in vm.act_cates">
                                                        <input type="radio" name="artcate" ng-model="control.config.cate_id" value="{{item.cate_id}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.cate_name"></span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='activitylist' && attr.key=='cate_id' && vm.art_cates.length>4">
                                                    <select class="selectbox" ng-model="control.config.cate_id" ng-options="model.cate_id as model.cate_name for model in vm.act_cates" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">所有</option>
                                                    </select>
                                                </div>
                                                <div ng-if="control.control =='sidemenubox' && attr.key=='data_key'">
                                                    <div class="radio-group" ng-if="control.config.type=='1'&&vm.art_cates.length<=4">
                                                        <label class="shop_label">
                                                            <input type="radio" name="artcatetwo" ng-model="control.config.data_key" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span>最顶级</span>
                                                        </label>
                                                        <label class="shop_label" ng-repeat="item in vm.art_cates">
                                                            <input type="radio" name="artcatetwo" ng-model="control.config.data_key" value="{{item.cate_id}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span ng-bind="item.cate_name"></span>
                                                        </label>
                                                    </div>
                                                    <select class="selectbox" ng-model="control.config.data_key" ng-if="control.config.type=='1' && vm.art_cates.length > 4" ng-options="model.cate_id as model.cate_name for model in vm.art_cates" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">最顶级</option>
                                                    </select>
                                                    <div class="radio-group" ng-if="control.config.type=='2'&&vm.mall_cates.length<=4">
                                                        <label class="shop_label">
                                                            <input type="radio" name="mallcatetwo" ng-model="control.config.data_key" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span>最顶级</span>
                                                        </label>
                                                        <label class="shop_label" ng-repeat="item in vm.mall_cates">
                                                            <input type="radio" name="mallcatetwo" ng-model="control.config.data_key" value="{{item.cate_id}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span ng-bind="item.cate_name"></span>
                                                        </label>
                                                    </div>
                                                    <select class="selectbox" ng-model="control.config.data_key" ng-if="control.config.type=='2' && vm.mall_cates.length > 4" ng-options="model.cate_id as model.cate_name for model in vm.mall_cates" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">最顶级</option>
                                                    </select>
                                                    <div class="radio-group" ng-if="control.config.type=='3'&&vm.act_cates.length<=4">
                                                        <label class="shop_label">
                                                            <input type="radio" name="actcatetwo" ng-model="control.config.data_key" value="" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span>最顶级</span>
                                                        </label>
                                                        <label class="shop_label" ng-repeat="item in vm.act_cates">
                                                            <input type="radio" name="actcatetwo" ng-model="control.config.data_key" value="{{item.cate_id}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span ng-bind="item.cate_name"></span>
                                                        </label>
                                                    </div>
                                                    <select class="selectbox" ng-model="control.config.data_key" ng-if="control.config.type=='3' && vm.act_cates.length > 4" ng-options="model.cate_id as model.cate_name for model in vm.act_cates" ng-change="vmFunc.editControlData(control)">
                                                        <option value="">最顶级</option>
                                                    </select>
                                                    <div class="radio-group" ng-if="control.config.type=='4'&&vm.toolbars.length<=4">
                                                        <label class="shop_label" ng-repeat="item in vm.toolbars">
                                                            <input type="radio" name="toolbartwo" ng-model="control.config.data_key" value="{{item.key_type}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                            <span ng-bind="item.key_type"></span>
                                                        </label>
                                                    </div>
                                                    <select class="selectbox" ng-model="control.config.data_key" ng-if="control.config.type=='4' && vm.toolbars.length > 4" ng-options="model.key_type as model.key_type for model in vm.toolbars" ng-change="vmFunc.editControlData(control)">
                                                    </select>
                                                </div>
                                                <div class="radio-group" ng-if="control.control =='copyright'&&vm.toolbars.length<=4">
                                                    <label class="shop_label" ng-repeat="item in vm.toolbars">
                                                        <input type="radio" name="navs" ng-model="control.config.nav_list" value="{{item.key_type}}" checked="" ng-click="vmFunc.editControlConfig(control)">
                                                        <span ng-bind="item.key_type"></span>
                                                    </label>
                                                </div>
                                                <div ng-if="control.control =='copyright'&&vm.toolbars.length>4">
                                                    <select class="selectbox" ng-model="control.config.nav_list" ng-options="model.key_type as model.key_type for model in vm.toolbars" ng-change="vmFunc.editControlConfig(control)">
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==0">
                                                <input class="shop_text" type="text" ng-model="control.config[attr.key]" ng-if="!((control.control=='malls' && (attr.key=='count' || attr.key=='sort_tag'))||(control.control=='cardlist' && attr.key=='rows'))" placeholder="{{attr.rmk?attr.rmk:attr.name}}" ng-blur="vmFunc.editControlConfig(control)" />
                                                <input class="shop_text" type="text" ng-model="control.config[attr.key]" ng-if="(control.control=='malls' && attr.key=='count' )||(control.control=='cardlist' && attr.key=='rows')" placeholder="{{attr.rmk?attr.rmk:attr.name}}" ng-blur="vmFunc.editControlData(control)" />
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==1">
                                                <div class="radio-group" ng-repeat="item in attr.options" ng-if="attr.options.length<=4">
                                                    <label class="shop_label">
                                                        <input type="radio" name="{{'option'+control.control_id+attr.key}}" ng-if="(control.control =='search' || control.control =='headerbar') && attr.key=='sidemenu_button'" ng-model="control.config[attr.key]" value="{{item.value}}" checked="" ng-click="vmFunc.addSidemenuControl(control)">
                                                        <input type="radio" name="{{'option'+control.control_id+attr.key}}" ng-if="!((control.control =='search'|| control.control =='headerbar') && attr.key=='sidemenu_button')" ng-model="control.config[attr.key]" value="{{item.value}}" checked="" ng-click="vmFunc.editControlData(control)">
                                                        <span ng-bind="item.text"></span>
                                                    </label>
                                                </div>
                                                <select class="selectbox" ng-model="control.config[attr.key]" ng-if="(control.control =='search'|| control.control =='headerbar')  && attr.key=='sidemenu_button' && attr.options.length>4" ng-options="model.value as model.text for model in attr.options" ng-change="vmFunc.addSidemenuControl(control)">
                                                </select>
                                                <select class="selectbox" ng-model="control.config[attr.key]" ng-if="!((control.control =='search'|| control.control =='headerbar')  && attr.key=='sidemenu_button') && attr.options.length>4" ng-options="model.value as model.text for model in attr.options" ng-change="vmFunc.editControlData(control)">
                                                </select>
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==2">
                                                <img alt="图片" ng-src="{{control.config[attr.key]}}" width="80px" height="80px" class="imgUpload" onerror="this.removeAttribute('src')" />
                                                <input type="file" name="file1" class="file" style="display: none;" attr-key="{{attr.key}}" control-id="{{control.control_id}}" />
                                                <span class="font-small color_warning">点击上传或直接输入地址</span><br />
                                                <input type="text" class="txtFile shop_text" ng-model="control.config[attr.key]" ng-blur="vmFunc.editControlConfig(control)" />
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==3">
                                                <input type="text" class="color shop_text" ng-model="control.config[attr.key]" placeholder="{{attr.rmk?attr.rmk:attr.name}}" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-key="{{attr.key}}" ng-blur="vmFunc.editControlConfig(control)" />
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==4">
                                                <div class="imgDiv hand" ng-click="vmFunc.showSelectAttrClass(control,attr.key)">
                                                    <div class="toolbarIco" ng-bind-html="control.config[attr.key]|FormatterIcon">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==5">
                                                <zc-slider config="{{attr.slider}}" value="control.config[attr.key]"></zc-slider>
                                            </div>
                                            <div class="shop_choose" ng-if="attr.type==6">
                                                <div style="width: 90%;">
                                                    <zc-select-link s-type="control.config[attr.key+'_s_type']" s-value="control.config[attr.key+'_s_value']" s-text="control.config[attr.key+'_s_text']" s-link="control.config[attr.key]"></zc-select-link>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="deleteDiv" ng-if="control.control != 'sidemenubox'">
                                    <button class="button button-balanced" ng-if="vm.can_move_controls.indexOf(control.control)<0" ng-click="vmFunc.moveModelUp(vm.cur_select_controls[0])">向上移</button>
                                    <button class="button button-energized" ng-if="vm.can_move_controls.indexOf(control.control)<0" ng-click="vmFunc.moveModelDown(vm.cur_select_controls[0])">向下移</button>
                                    <button class="button button-assertive" ng-click="vmFunc.deleteControl(vm.cur_select_controls[0])">删除</button>
                                </div>
                            </div>
                            <div class="form-div m-top" ng-if="control.control=='slides'">
                                <div class="exDataDiv" ng-repeat="slide in control.data">
                                    <table class="data-table" ng-class="{'m-top':$index>0}">
                                        <tr>
                                            <td class="data-td imgTd">
                                                <img alt="图片" ng-src="{{slide.img}}" class="imgUpload imgDiv" onerror="this.removeAttribute('src')" />
                                                <input type="file" name="file1" class="file" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="img" control-right="data" />
                                            </td>
                                            <td class="data-td">
                                                <table class="c-table">
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>标题<span class="color_error">*</span>：</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <input type="text" class="data-txt" placeholder="标题" ng-model="slide.title" ng-blur="vmFunc.editExData(control,$index,'title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54" style="vertical-align: middle;">
                                                            <span>链接：</span>
                                                        </td>
                                                        <td colspan="3" style="vertical-align: middle;">
                                                            <div style="width: 90%;">
                                                                <zc-select-link s-type="slide.s_type" s-value="slide.s_value" s-text="slide.s_text" s-link="slide.link" s-edit="control.is_edit"></zc-select-link>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="deleteExData" ng-click="vmFunc.deleteExData(control,$index)">
                                        <img src="/img/delete.png" />
                                    </div>
                                    <div class="moveUpExData" ng-click="vmFunc.moveUpExData(control,$index)">
                                        <img src="/img/icons/up.png" />
                                    </div>
                                    <div class="moveDownExData" ng-click="vmFunc.moveDownExData(control,$index)">
                                        <img src="/img/icons/down.png" />
                                    </div>
                                </div>
                                <div class="deleteDiv">
                                    <button class="button button-calm" ng-click="vmFunc.addExData(control)">添加</button>
                                </div>
                            </div>
                            <div class="form-div m-top" ng-if="control.control=='foottool' || control.control=='tabs'">
                                <div class="exDataDiv" ng-repeat="toolbar in control.data">
                                    <table class="data-table" ng-class="{'m-top':$index>0}" ng-if="control.control =='foottool'">
                                        <tr>
                                            <td class="data-td imgTd pTop10">
                                                <div class="imgDiv toolbarImgDiv" ng-show="toolbar.ico == ''">
                                                    <img alt="图片" ng-src="{{toolbar.img}}" class="toolbarImg imgDiv img" onerror="this.removeAttribute('src')" />
                                                </div>
                                                <div class="imgDiv toolbarIcoDiv" ng-show="toolbar.ico != ''" ng-click="vmFunc.showSelectToolbarClass(control,$index)">
                                                    <div class="toolbarIco" ng-bind-html="toolbar.ico|FormatterIcon:toolbar.color">
                                                    </div>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <input type="file" name="file1" class="file toolbar" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="img" control-right="data" attr-toolbar-img="1" />
                                                    <button class="button button-calm toolbarImg">上传图片</button>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <button class="button button-calm" ng-click="vmFunc.showSelectToolbarClass(control,$index)">选择图标</button>
                                                </div>
                                            </td>
                                            <td class="data-td">
                                                <table class="c-table">
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>标题<span class="color_error">*</span>：</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <input type="text" class="data-txt" placeholder="标题" ng-model="toolbar.title" ng-blur="vmFunc.editExData(control,$index,'title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54" style="vertical-align: middle;">
                                                            <span>类型：</span>
                                                        </td>
                                                        <td colspan="3" style="vertical-align: middle;">
                                                            <div style="width: 90%;">
                                                                <zc-select-link s-type="toolbar.s_type" s-value="toolbar.s_value" s-text="toolbar.s_text" s-link="toolbar.url" s-l-type="toolbar.type" s-edit="control.is_edit"></zc-select-link>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="字色" ng-model="toolbar.color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="color" ng-blur="vmFunc.editExData(control,$index,'color')" />
                                                        </td>
                                                        <td class="label-td">
                                                            <span>选中字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="选中字色" ng-model="toolbar.active_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_color" ng-blur="vmFunc.editExData(control,$index,'active_color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>背景色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="背景色" ng-model="toolbar.bg_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="bg_color" ng-blur="vmFunc.editExData(control,$index,'bg_color')" />
                                                        </td>
                                                        <td class="label-td">
                                                            <span>选中背景色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="选中背景色" ng-model="toolbar.active_bg_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_bg_color" ng-blur="vmFunc.editExData(control,$index,'active_bg_color')" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="data-table" ng-class="{'m-top':$index>0}" ng-if="control.control =='tabs'">
                                        <tr>
                                            <td class="data-td">
                                                <table class="c-table">
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>标题<span class="color_error">*</span>：</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <input type="text" class="data-txt" placeholder="标题" ng-model="toolbar.title" ng-blur="vmFunc.editExData(control,$index,'title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54" style="vertical-align: middle;">
                                                            <span>类型：</span>
                                                        </td>
                                                        <td colspan="3" style="vertical-align: middle;">
                                                            <div style="width: 90%;">
                                                                <zc-select-link s-type="toolbar.s_type" s-value="toolbar.s_value" s-text="toolbar.s_text" s-link="toolbar.url" s-l-type="toolbar.type" s-edit="control.is_edit"></zc-select-link>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="字色" ng-model="toolbar.color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="color" ng-blur="vmFunc.editExData(control,$index,'color')" />
                                                        </td>
                                                        <td class="label-td">
                                                            <span>选中字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="选中字色" ng-model="toolbar.active_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_color" ng-blur="vmFunc.editExData(control,$index,'active_color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>背景色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="背景色" ng-model="toolbar.bg_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="bg_color" ng-blur="vmFunc.editExData(control,$index,'bg_color')" />
                                                        </td>
                                                        <td class="label-td">
                                                            <span>选中背景色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="选中背景色" ng-model="toolbar.active_bg_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_bg_color" ng-blur="vmFunc.editExData(control,$index,'active_bg_color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>背景图：</span>
                                                        </td>
                                                        <td ng-style="{'background-image':(!toolbar.bg_img?'':'url('+ toolbar.bg_img +')')}">
                                                            <a href="javascript:void(0)" class="imgUpload shop_a">上传背景图</a>
                                                            <input type="file" name="file1" class="file" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="bg_img" control-right="data" />
                                                            <a href="javascript:void(0)" class="imgUpload shop_a" style="margin-left: 15px;" ng-click="toolbar.bg_img='';vmFunc.editExData(control,$index,'bg_img')">清空</a>
                                                        </td>
                                                        <td class="label-td">
                                                            <span>选中背景图：</span>
                                                        </td>
                                                        <td ng-style="{'background-image':(!toolbar.active_bg_img?'':'url('+ toolbar.active_bg_img +')')}">
                                                            <a href="javascript:void(0)" class="imgUpload shop_a">上传背景图</a>
                                                            <input type="file" name="file1" class="file" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_bg_img" control-right="data" />
                                                            <a href="javascript:void(0)" class="imgUpload shop_a" style="margin-left: 15px;" ng-click="toolbar.active_bg_img='';vmFunc.editExData(control,$index,'active_bg_img')">清空</a>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="deleteExData" ng-click="vmFunc.deleteExData(control,$index)">
                                        <img src="/img/delete.png" />
                                    </div>
                                    <div class="moveUpExData" ng-click="vmFunc.moveUpExData(control,$index)">
                                        <img src="/img/icons/up.png" />
                                    </div>
                                    <div class="moveDownExData" ng-click="vmFunc.moveDownExData(control,$index)">
                                        <img src="/img/icons/down.png" />
                                    </div>
                                </div>
                                <div class="deleteDiv">
                                    <button class="button button-calm" ng-click="vmFunc.addExData(control)">添加</button>
                                </div>
                            </div>
                            <div class="form-div m-top" ng-if="control.type=='9'">
                                <div class="exDataDiv" ng-repeat="toolbar in control.data">
                                    <table class="data-table" ng-class="{'m-top':$index>0}">
                                        <tr>
                                            <td class="data-td imgTd pTop10">
                                                <div class="imgDiv toolbarImgDiv" ng-show="toolbar.ico == ''">
                                                    <img alt="图片" ng-src="{{toolbar.img}}" class="toolbarImg imgDiv img" onerror="this.removeAttribute('src')" />
                                                </div>
                                                <div class="imgDiv toolbarIcoDiv" ng-show="toolbar.ico != ''" ng-click="vmFunc.showSelectToolbarClass(control,$index)">
                                                    <div class="toolbarIco" ng-bind-html="toolbar.ico|FormatterIcon:toolbar.ico_color">
                                                    </div>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <input type="file" name="file1" class="file toolbar" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="img" control-right="data" attr-toolbar-img="1" />
                                                    <button class="button button-calm toolbarImg">上传图片</button>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <button class="button button-calm" ng-click="vmFunc.showSelectToolbarClass(control,$index)">选择图标</button>
                                                </div>
                                            </td>
                                            <td class="data-td">
                                                <table class="c-table">
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>标题<span class="color_error">*</span>：</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <input type="text" class="data-txt" placeholder="标题" ng-model="toolbar.title" ng-blur="vmFunc.editExData(control,$index,'title')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54" style="vertical-align: middle;">
                                                            <span>类型：</span>
                                                        </td>
                                                        <td style="vertical-align: middle;">
                                                            <div style="width: 90%;">
                                                                <zc-select-link s-type="toolbar.s_type" s-value="toolbar.s_value" s-text="toolbar.s_text" s-link="toolbar.url" s-l-type="toolbar.type" s-edit="control.is_edit"></zc-select-link>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="字色" ng-model="toolbar.color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="color" ng-blur="vmFunc.editExData(control,$index,'color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>背景色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="背景色" ng-model="toolbar.bg_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="bg_color" ng-blur="vmFunc.editExData(control,$index,'bg_color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>图标色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="图标色" ng-model="toolbar.ico_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="ico_color" ng-blur="vmFunc.editExData(control,$index,'ico_color')" />
                                                        </td>
                                                    </tr>
                                                    <tr ng-if="!!control.config.nav_right_style && control.config.nav_right_style==1">
                                                        <td class="label-td-54">
                                                            <span>右图：</span>
                                                        </td>
                                                        <td>
                                                            <div class="imgUpload upNavRightImg">
                                                                <img ng-src="{{toolbar.active_bg_img}}" />
                                                            </div>
                                                            <input type="file" name="file1" class="file" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="active_bg_img" control-right="data" />
                                                            <a href="javascript:void(0)" class="shop_a" style="margin-left: 15px;" ng-click="toolbar.active_bg_img='';vmFunc.editExData(control,$index,'active_bg_img')">清空</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="deleteExData" ng-click="vmFunc.deleteExData(control,$index)">
                                        <img src="/img/delete.png" />
                                    </div>
                                    <div class="moveUpExData" ng-click="vmFunc.moveUpExData(control,$index)">
                                        <img src="/img/icons/up.png" />
                                    </div>
                                    <div class="moveDownExData" ng-click="vmFunc.moveDownExData(control,$index)">
                                        <img src="/img/icons/down.png" />
                                    </div>
                                </div>
                                <div class="deleteDiv">
                                    <button class="button button-calm" ng-click="vmFunc.addExData(control)">添加</button>
                                </div>
                            </div>
                            <div class="form-div m-top" ng-if="control.control=='linehead'">
                                <div class="exDataDiv" ng-repeat="nav in control.data">
                                    <table class="data-table" ng-class="{'m-top':$index>0}">
                                        <tr>
                                            <td class="data-td imgTd pTop10">
                                                <div class="imgDiv toolbarImgDiv" ng-show="nav.ico == ''">
                                                    <img alt="图片" ng-src="{{nav.img}}" class="toolbarImg imgDiv img" onerror="this.removeAttribute('src')" />
                                                </div>
                                                <div class="imgDiv toolbarIcoDiv" ng-show="nav.ico != ''" ng-click="vmFunc.showSelectToolbarClass(control,$index)">
                                                    <div class="toolbarIco" ng-bind-html="nav.ico|FormatterIcon:nav.ico_color">
                                                    </div>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <input type="file" name="file1" class="file toolbar" style="display: none;" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="img" control-right="data" attr-toolbar-img="1" />
                                                    <button class="button button-calm toolbarImg">上传图片</button>
                                                </div>
                                                <div class="toolbarDiv">
                                                    <button class="button button-calm" ng-click="vmFunc.showSelectToolbarClass(control,$index)">选择图标</button>
                                                </div>
                                            </td>
                                            <td class="data-td">
                                                <table class="c-table">
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>名称<span class="color_error">*</span>：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt" placeholder="名称" ng-model="nav.text" ng-blur="vmFunc.editExData(control,$index,'text')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54" style="vertical-align: middle;">
                                                            <span>链接：</span>
                                                        </td>
                                                        <td style="vertical-align: middle;">
                                                            <div style="width: 90%;">
                                                                <zc-select-link s-type="nav.s_type" s-value="nav.s_value" s-text="nav.s_text" s-link="nav.link" s-l-type="nav.type" s-edit="control.is_edit"></zc-select-link>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>字色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="字色" ng-model="nav.color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="color" ng-blur="vmFunc.editExData(control,$index,'color')" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="label-td-54">
                                                            <span>图标色：</span>
                                                        </td>
                                                        <td>
                                                            <input type="text" class="data-txt color" placeholder="图标色" ng-model="nav.ico_color" control-control="{{control.control}}" control-id="{{control.control_id}}" attr-index="{{$index}}" attr-key="ico_color" ng-blur="vmFunc.editExData(control,$index,'ico_color')" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="deleteExData" ng-click="vmFunc.deleteExData(control,$index)">
                                        <img src="/img/delete.png" />
                                    </div>
                                    <div class="moveUpExData" ng-click="vmFunc.moveUpExData(control,$index)">
                                        <img src="/img/icons/up.png" />
                                    </div>
                                    <div class="moveDownExData" ng-click="vmFunc.moveDownExData(control,$index)">
                                        <img src="/img/icons/down.png" />
                                    </div>
                                </div>
                                <div class="deleteDiv">
                                    <button class="button button-calm" ng-click="vmFunc.addExData(control)">添加</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
<script type="text/javascript">
    var webInfo = {};
    var webFunc = {};
    var is_edit= true;
    webInfo.isQQBrowser = false;
    webInfo.bh = 568;
    webInfo.bw = 320;
</script>
<script type="text/javascript">
    var iconclasses = <%=iconclasses%>;
    var backlist = '<%=backlist%>';
    var component = <%=component%>;
    var template = <%=template%>;
    var component_model = <%=component_model%>;
    var slides = <%=slides%>;
    var toolbars = <%=toolbars%>;
    var mall_cates = <% =mall_cates%>;
    var course_cates = <% =course_cates%>;
    var mall_tags = <% =mall_tags%>;
    var art_cates = <% =art_cates%>;
    var act_cates = <% =act_cates%>;
    var login_info=<%=login_info%>;
    var mallConfig = <%= mallConfig %>;
    var strDomain = '<%=strDomain%>';
    var edit_template = <%=edit_template%>;
    var template_types = <%=template_types%>;
    var pageConfig={};
    var loginUserInfo=<% = loginUserInfo%>;
</script>
<script src="http://static-files.socialcrmyun.com/Scripts/jquery-1.12.4.min.js" type="text/javascript"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/jquery/lazyload/jquery.lazyload.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/jquery/slider/jquery-ui-slider.min.js"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016120101" type="text/javascript"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016120101" type="text/javascript"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/angularjs/1.3.15/angular.min.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/angularjs/1.3.15/angular-touch.min.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/angularjs/carousel/angular-carousel.min.js"></script>
<script type="text/javascript" src="/kindeditor-4.1.10/kindeditor.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/kindeditor-4.1.10/lang/zh_CN.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/kindeditor-4.1.10/kindeditor-plugins.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/jquery/sortable/jquery-ui-sortable.min.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/StringBuilder.Min.js" charset="utf-8"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/Plugins/ZeroClipboard/2.2.0/ZeroClipboard.min.js"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/global-m.js?v=2016120101" charset="utf-8"></script>
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/layer.mobile/layer.m.js"></script>
<%--<script src="/Scripts/color/jscolor.js?v=20160811"></script>--%>
<script src="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.js" type="text/javascript"></script>
<script type="text/javascript" src="/customize/comeoncloud/m2/dist/app.bundle.js?v=2017022501"></script>
<script type="text/javascript" src="/customize/comeoncloud/m2/dist/templateCache.js?v=2017022501"></script>
<script type="text/javascript" src="/Plugins/angular/zcSlider/zcSlider.js?v=2017022501"></script>
<script type="text/javascript" src="/Plugins/angular/zcSelectLink/zcSelectLink.js?v=2017022501"></script>
<script type="text/javascript" src="/Plugins/angular/zcShop/zcShop.js?v=2017030302"></script>
<script type="text/javascript" src="/admin/component/js/edit_page.js?v=2017030302"></script>

<script type="text/javascript">
    var layerIndex;
    window.alert = function (msg) {
        layerIndex = layer.open({
            type:3,
            content: msg,
            time: 2 //2秒后自动关闭
        });
    };
    window.progress = function () {
        layerIndex = layer.open({type: 2});
    };
</script>
<%= icoScript %>
