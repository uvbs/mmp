<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.Model.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            font-family: 微软雅黑;
        }

        table td {
            height: 24px;
        }

        .title {
            font-size: 12px;
        }

        .field {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 800px;
        }

        .fieldsort {
            float: left;
            margin-left: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .field input[type=text] {
            width: 90%;
        }

        .width120 {
            width: 120px !important;
            margin-right: 2px;
        }

        .width60 {
            width: 60px !important;
            margin-right: 2px;
        }

        .width300 {
            width: 300px !important;
            margin-right: 2px;
        }

        .select {
            border-collapse: separate;
            border-spacing: 2px;
            border-color: grey;
            color: gray;
        }

        .red {
            color: #CCCCCC;
        }
        .uparray{
            margin-right:5px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <strong><%= Request["component_model_id"]=="0"?"添加":"编辑" %>组件库</strong>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div class="ActivityBox">
        <div style="font-size: 12px; width: 800px;">
            <table style="width: 800px;">
                <tr>
                    <td style="width: 120px;">组件库名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtModelName" value="" style="width: 450px;" placeholder="组件库名称(必填)" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">组件库分类：
                    </td>
                    <td width="*" align="left">
                        <select id="dllModelType" style="width: 260px;"></select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">页面访问地址：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtModelLinkUrl" value="" style="width: 450px;" placeholder="" />
                        <br />
                        {component_id}标识当前使用本组件库的页面id
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">页面相对路径：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtModelHtmlkUrl" value="" style="width: 450px;" placeholder="" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;">是否作废：
                    </td>
                    <td width="*" align="left">
                        <input id="chkIsDelete" type="checkbox" class="positionTop2" value="1" /><label for="chkIsDelete">作废</label>
                    </td>
                </tr>
            </table>
            <strong style="font-size: 18px;">组件列表:</strong><span style="color: red;">组件最少需要一个，组件key不能相同</span>
            <div class="fields">
                <div class="field" data-field-index="0" data-field-id="0">
                    <img src="/img/icons/up.png" class="upfield fieldsort" />
                    <img src="/img/icons/down.png" class="downfield fieldsort" />
                    <img src="/img/delete.png" style="float: right;" class="deletefield" />
                    <table style="width: 800px; margin-left: 10px;">
                        <tr>
                            <td style="width: 120px;">组件:</td>
                            <td style="width: 325px;">
                                <input type="text" name="fieldkey" class="width300" placeholder="组件Key(必填)" />
                            </td>
                            <td style="width: 325px;">
                                <input type="text" name="fieldname" class="width300" placeholder="组件名称(必填)" />
                            </td>
                            <td style="width: 30px;"></td>
                        </tr>
                        <tr>
                            <td>类型:</td>
                            <td colspan="3" data-oldtype="1">
                                <input name="rdtype0" type="radio" class="positionTop2 hand red" disabled="disabled" value="0" id="rd00" /><label for="rd00" class="hand red">输入</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand red" disabled="disabled" value="1" id="rd01" /><label for="rd01" class="hand red">选择</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand red" disabled="disabled" value="2" id="rd02" /><label for="rd02" class="hand red">图片</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand red" disabled="disabled" value="3" id="rd03" /><label for="rd03" class="hand red">颜色</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand" value="4" id="rd04" checked="checked" /><label for="rd04" class="hand">数组</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand" value="5" id="rd05" /><label for="rd05" class="hand">对象</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand slide-to" value="6" id="rd06" /><label for="rd06" class="hand slide-to">广告</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand tool-to red" disabled="disabled" value="7" id="rd07" /><label for="rd07" class="hand tool-to red">导航</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand component-to red" disabled="disabled" value="8" id="rd08" /><label for="rd08" class="hand component-to red">页面</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand navs-to" value="9" id="rd09" /><label for="rd09" class="hand navs-to">导航数组</label>
                                <input name="rdtype0" type="radio" class="positionTop2 hand malls-to" value="10" id="rd010" /><label for="rd010" class="hand malls-to">商品数组</label>
                            </td>
                        </tr>
                        <tr class="data-source" style="display: none;">
                            <td>数据:</td>
                            <td colspan="3"> <input type="radio" name="rddata0" value="0" checked="checked" id="rdd00" /><label for="rdd00">固定</label></td>
                        </tr>
                        <tr class="data-arrays">
                            <td class="array" style="vertical-align: top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td>
                            <td style="vertical-align: top;">
                                <input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" />
                                <select class="width60">
                                    <option value="0">输入</option>
                                    <option value="1">选择</option>
                                    <option value="2">图片</option>
                                    <option value="3">颜色</option>
                                    <option value="4">图标</option>
                                    <option value="5">数值</option>
                                    <option value="6">链接</option>
                                    <option value="7">商品</option>
                                </select>
                            </td>
                            <td>
                                <input type="text" class="width300" placeholder="默认值" /></td>
                            <td style="vertical-align: top;"></td>
                        </tr>
                        <tr class="data-arrays">
                            <td class="array" style="vertical-align: top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td>
                            <td style="vertical-align: top;">
                                <input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" />
                                <select class="width60">
                                    <option value="0">输入</option>
                                    <option value="1">选择</option>
                                    <option value="2">图片</option>
                                    <option value="3">颜色</option>
                                    <option value="4">图标</option>
                                    <option value="5">数值</option>
                                    <option value="6">链接</option>
                                    <option value="7">商品</option>
                                </select>
                            </td>
                            <td>
                                <input type="text" class="width300" placeholder="默认值" /></td>
                            <td style="vertical-align: top;"></td>
                        </tr>
                        <tr class="data-arrays">
                            <td></td>
                            <td colspan="3"><a class="button button-rounded button-primary addarray">添加属性</a></td>
                        </tr>
                    </table>
                </div>
                <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnaddfield">添加组件</a>
            </div>
            <div style="text-align: center;">
                <a href="javascript:void(0);" id="btnSave" class="button glow button-rounded button-flat-action" style="width: 160px;">提交</a>
                <a href="javascript:void(0);" id="btnPageBack" class="button glow " style="width: 160px;">返回</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/component/model/';
        var component_model_id = '<%= Request["component_model_id"] %>'; //问题数量
        var action = component_model_id == 0 ? "add.ashx" : "update.ashx";
        var components = [];
        var keyvalue_list = <% =keyvalue_list%>;
        var tool_list = <% =tool_list%>;
        $(function () {

            //加载分类
            loadTypeList();

            //选择属性类型
            $("#dllModelType").live("change", function () {
                hideComponents();
            });
            $(".upfield").live("click",function(){
                if($(this).closest("div").prev(".field").length>0){
                    $(this).closest("div").prev(".field").before($(this).closest("div").clone());
                    $(this).closest("div").remove();
                }
            });
            $(".downfield").live("click",function(){
                if($(this).closest("div").next(".field").length>0){
                    $(this).closest("div").next(".field").after($(this).closest("div").clone());
                    $(this).closest("div").remove();
                }
            });
            $(".uparray").live("click",function(){
                var npre = $(this).closest("tr").prev();
                if($(npre).hasClass('data-source')) return;
                $(npre).before($(this).closest("tr"));
            });
            //加载分类
            //添加组件
            $("#btnaddfield").click(function () {
                var objs = $(".fields .field");
                var _fieldindex = 0;
                if (objs.length > 0) {
                    _fieldindex = Number($(objs[objs.length - 1]).attr("data-field-index"));
                    _fieldindex++;
                }
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<div class="field" data-field-index="{0}" data-field-id="{1}">', _fieldindex, 0);
                appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort"/>');
                appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort"/>');
                appendhtml.AppendFormat('<img src="/img/delete.png" style="float:right;" class="deletefield"/>');
                appendhtml.AppendFormat('<table style="width:100%;margin-left:10px;">');
                appendhtml.AppendFormat('<tr><td style="width:120px;">组件:</td><td style="width: 325px;"><input type="text" name="fieldkey" class="width300" placeholder="组件Key(必填)" /></td><td style="width: 325px;"><input type="text" name="fieldname" class="width300" placeholder="组件名称(必填)" /></td><td style="width:30px;"></td></tr>');

                appendhtml.AppendFormat('<tr><td>类型:</td><td colspan="3" data-oldtype="1">');
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="0" id="rd{0}0"/><label for="rd{0}0" class="hand red">输入</label>', _fieldindex, '');
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="1" id="rd{0}1"/><label for="rd{0}1" class="hand red">选择</label>', _fieldindex, '');
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="2" id="rd{0}2"/><label for="rd{0}2" class="hand red">图片</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="3" id="rd{0}3"/><label for="rd{0}3" class="hand red">颜色</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand" {1} value="4" id="rd{0}4"/><label for="rd{0}4" class="hand">数组</label>', _fieldindex, 'checked="checked"');
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand" {1} value="5" id="rd{0}5"/><label for="rd{0}5" class="hand">对象</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand slide-to" {1} value="6" id="rd{0}6"/><label for="rd{0}6" class="hand slide-to">广告</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand tool-to red" disabled="disabled" {1} value="7" id="rd{0}7"/><label for="rd{0}7" class="hand tool-to red">导航</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand component-to red" disabled="disabled" {1} value="8" id="rd{0}8"/><label for="rd{0}8" class="hand component-to red">页面</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand navs-to" {1} value="9" id="rd{0}9"/><label for="rd{0}9" class="hand navs-to">导航数组</label>', _fieldindex, "");
                appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand malls-to" {1} value="10" id="rd{0}10"/><label for="rd{0}10" class="hand malls-to">商品数组</label>', _fieldindex, "");
                appendhtml.AppendFormat('</td></tr>');
                appendhtml.AppendFormat('<tr class="data-source" style="display:none;"><td>数据:</td><td colspan="3"><input type="radio" name="rddata{0}" value="0" checked="checked" id="rdd{0}0"/><label for="rdd{0}0">固定</label></td></tr>', _fieldindex);
                
                appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;"><input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" /><select class="width60"><option value="0">输入</option><option value="1">选择</option><option value="2">图片</option><option value="3">颜色</option><option value="4">图标</option><option value="5">数值</option><option value="6">链接</option><option value="7">商品</option></select></td><td><input type="text" class="width300" placeholder="默认值" /></td><td style="vertical-align:top;"></td></tr>');
                appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;"><input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" /><select class="width60"><option value="0">输入</option><option value="1">选择</option><option value="2">图片</option><option value="3">颜色</option><option value="4">图标</option><option value="5">数值</option><option value="6">链接</option><option value="7">商品</option></select></td><td><input type="text" class="width300" placeholder="默认值" /></td><td style="vertical-align:top;"></td></tr>');
                appendhtml.AppendFormat('<tr class="data-arrays"><td></td><td colspan="3"><a class="button button-rounded button-primary addarray">添加属性</a></td></tr>');

                appendhtml.AppendFormat('</table>');
                appendhtml.AppendFormat('</div>');
                $(this).before(appendhtml.ToString());
                hideComponents();
            });
            //添加组件
            //删除组件
            $('.deletefield').live("click", function () {
                var objs = $(".fields .field");
                if (objs.length <= 1) {
                    Alert("最少需要1个组件");
                    return;
                }
                $(this).closest("div").remove();
            });
            //删除组件
            //删除选项
            $('.deleteoption').live("click", function () {
                var objs = $(this).closest("tr").closest("table").find(".option");
                if (objs.length <= 2) {
                    Alert("最少需要2个选项");
                    return;
                }
                $(this).closest("tr").remove();
            });
            //删除选项
            //添加选项
            $(".addoption").live("click", function () {
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<tr class="data-select"><td class="option">选项:</td><td><input type="text" name="key" placeholder="选项名称" /></td><td><input type="text" name="value" placeholder="选项值" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption"/></td></tr>');
                $(this).closest("tr").before(appendhtml.ToString());
            });
            //添加选项
            //选择属性类型
            $(".data-arrays select").live("change", function () {
                var val = $(this).val();
                $(this).closest("td").next().html("");
                $(this).closest("td").next().next().html("");
                if (val == 0) {
                    var appendhtmlul = new StringBuilder();
                    appendhtmlul.AppendFormat('<input type="text" class="width300" placeholder="默认值" />');
                    $(this).closest("td").next().append(appendhtmlul.ToString());
                }
                else if (val == 1) {
                    var appendhtmlul = new StringBuilder();
                    appendhtmlul.AppendFormat('<ul>');
                    appendhtmlul.AppendFormat('<li>');
                    appendhtmlul.AppendFormat('<input type="text" name="likey" class="width120 floatL" placeholder="属性选项名称" />');
                    appendhtmlul.AppendFormat('<input type="text" name="livalue" class="width120 floatL" placeholder="属性选项值" />');
                    appendhtmlul.AppendFormat('<img src="/img/delete.png" width="20" height="20" alt="删除属性选项" class="deletearrayoption floatL hand" />');
                    appendhtmlul.AppendFormat('</li>');
                    appendhtmlul.AppendFormat('</ul>');
                    $(this).closest("td").next().append(appendhtmlul.ToString());
                    $(this).closest("td").next().next().append('<img src="/img/icons/add.png" width="20" height="20" alt="添加属性选项" class="addarrayoption hand" />');
                }
                else if (val == 3) {
                    var appendhtmlul = new StringBuilder();
                    appendhtmlul.AppendFormat('<input type="text" class="width300 color" />');
                    $(this).closest("td").next().append(appendhtmlul.ToString());
                    $(this).closest("td").next().find('.color').spectrum();
                }
                else if (val == 5) {
                    var appendhtmlul = new StringBuilder();
                    appendhtmlul.AppendFormat('<table>');
                    appendhtmlul.AppendFormat('<tr><td class="width60">最小值</td><td><input type="text" /></td><td class="width60">最大值</td><td><input type="text" /></td></tr>');
                    appendhtmlul.AppendFormat('<tr><td class="width60">每格值</td><td><input type="text" /></td><td class="width60">默认值</td><td><input type="text" /></td></tr>');
                    appendhtmlul.AppendFormat('</table>');
                    $(this).closest("td").next().append(appendhtmlul.ToString());
                }
            });
            //选择属性类型
            //删除属性选项
            $('.deletearrayoption').live("click", function () {
                var objs = $(this).closest("ul").find("li");
                //console.log(objs);
                if (objs.length < 2) {
                    Alert("最少需要1个属性选项");
                    return;
                }
                $(this).closest("li").remove();
            });
            //删除属性选项
            //添加属性选项
            $(".addarrayoption").live("click", function () {
                var obj = $(this).closest("td").prev().find("ul");
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<li>');
                appendhtml.AppendFormat('<input type="text" name="likey" class="width120 floatL" placeholder="属性选项名称" />');
                appendhtml.AppendFormat('<input type="text" name="livalue" class="width120 floatL" placeholder="属性选项值" />');
                appendhtml.AppendFormat('<img src="/img/delete.png" width="20" height="20" alt="删除属性选项" class="deletearrayoption floatL hand" />');
                appendhtml.AppendFormat('</li>');
                $(obj).append(appendhtml.ToString());
            });
            //添加属性选项
            //添加属性
            $(".addarray").live("click", function () {
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;"><input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" /><select class="width60"><option value="0">输入</option><option value="1">选择</option><option value="2">图片</option><option value="3">颜色</option><option value="4">图标</option><option value="5">数值</option><option value="6">链接</option><option value="7">商品</option></select></td><td><input type="text" class="width300" placeholder="默认值" /></td><td style="vertical-align:top;"></td></tr>');
                $(this).closest("tr").before(appendhtml.ToString());
            });
            //添加属性
            //删除属性
            $(".deletearray").live("click", function () {
                var objs = $(this).closest("tr").closest("table").find(".array");
                if (objs.length < 2) {
                    Alert("最少需要1个属性");
                    return;
                }
                $(this).closest("tr").remove();
            });
            //删除属性

            $("input[type='radio'][name^='rdtype']").live("click", function () {
                var ptable = $(this).closest("table");
                var oldtype = $(this).closest("td").attr("data-oldtype");

                var nowtype = $(this).val();
                if (oldtype == nowtype) return;
                var field_type = parseInt(nowtype);

                //if(field_type==6 && $("td[data-oldtype='"+field_type+"']").length>0){
                //    $(this).closest("td").find("input[type='radio'][name^='rdtype']").each(function () {
                //        $(this).attr("checked", $(this).val() == oldtype);
                //    })
                //    Alert("组件库中仅能添加一个广告");
                //    return;
                //}
                
                var fieldkeyObj = $(ptable).find("input[type='text'][name='fieldkey']");
                var fieldnameObj = $(ptable).find("input[type='text'][name='fieldname']");
                if(oldtype ==6 || oldtype==7 || oldtype==8){
                    $(fieldkeyObj).val('');
                    $(fieldnameObj).val('');
                }
                $(fieldkeyObj).attr("disabled",false);

                $(ptable).find(".data-text").remove();
                $(ptable).find(".data-select").remove();
                $(ptable).find(".data-arrays").remove();
                $(ptable).find(".data-component").remove();
                $(ptable).find(".data-toolbar").remove();
                if (field_type == 0) {
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<tr class="data-text"><td>默认值:</td><td colspan="3"><input type="text" class="width300" placeholder="默认值" /></td></tr>');
                    $(ptable).append(appendhtml.ToString());
                }
                else if (field_type == 1) {
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<tr class="data-select"><td class="option">选项:</td><td><input type="text" name="key" placeholder="选项名称" /></td><td><input type="text" name="value" placeholder="选项值" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption"/></td></tr>');
                    appendhtml.AppendFormat('<tr class="data-select"><td class="option">选项:</td><td><input type="text" name="key" placeholder="选项名称" /></td><td><input type="text" name="value" placeholder="选项值" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption"/></td></tr>');
                    appendhtml.AppendFormat('<tr class="data-select"><td></td><td  colspan="3"><a class="button button-rounded button-primary addoption">添加选项</a></td></tr>');
                    $(ptable).append(appendhtml.ToString());
                }
                else if (field_type == 3) {
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<tr class="data-text"><td>默认颜色:</td><td colspan="3"><input type="text" class="width300 color" /></td></tr>');
                    $(ptable).append(appendhtml.ToString());
                    $(ptable).find('.color').spectrum();
                }
                else if (field_type == 4 || field_type == 5 || field_type == 6 ||field_type == 9 || field_type == 10) {
                    
                    var appendhtml = new StringBuilder();
                    if(field_type == 9){
                        var old_t_navs = $("td[data-oldtype='9']");
                        if(old_t_navs.length==0){
                            $(fieldkeyObj).val('navs');
                            $(fieldnameObj).val("导航数组");
                        }
                        else{
                            $(fieldkeyObj).val('navs' + "_" + old_t_navs.length);
                            $(fieldnameObj).val("导航数组"+ old_t_navs.length);
                        }
                        $(fieldkeyObj).attr("disabled",true);
                    }
                    else if(field_type == 10){
                        var old_t_navs = $("td[data-oldtype='10']");
                        if(old_t_navs.length==0){
                            $(fieldkeyObj).val('malls');
                        }
                        else{
                            $(fieldkeyObj).val('malls' + "_" + old_t_navs.length);
                        }
                        $(fieldkeyObj).attr("disabled",true);
                    }
                    else if(field_type == 6){
                        var old_t_navs = $("td[data-oldtype='6']");
                        if(old_t_navs.length==0){
                            $(fieldkeyObj).val('slides');
                            $(fieldnameObj).val("幻灯片广告");
                        }
                        else{
                            $(fieldkeyObj).val('slides' + "_" + old_t_navs.length);
                            $(fieldnameObj).val("幻灯片广告"+ old_t_navs.length);
                        }
                        $(fieldkeyObj).attr("disabled",true);
                    }
                    appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;"><input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" /><select class="width60"><option value="0">输入</option><option value="1">选择</option><option value="2">图片</option><option value="3">颜色</option><option value="4">图标</option><option value="5">数值</option><option value="6">链接</option><option value="7">商品</option></select></td><td><input type="text" class="width300" placeholder="默认值" /></td><td style="vertical-align:top;"></td></tr>');
                    appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;"><input type="text" name="key" class="width120" placeholder="属性Key" /><input type="text" name="name" class="width120" placeholder="属性名称" /><select class="width60"><option value="0">输入</option><option value="1">选择</option><option value="2">图片</option><option value="3">颜色</option><option value="4">图标</option><option value="5">数值</option><option value="6">链接</option><option value="7">商品</option></select></td><td><input type="text" class="width300" placeholder="默认值" /></td><td style="vertical-align:top;"></td></tr>');
                    appendhtml.AppendFormat('<tr class="data-arrays"><td></td><td colspan="3"><a class="button button-rounded button-primary addarray">添加属性</a></td></tr>');
                    $(ptable).append(appendhtml.ToString());
                }
                    //else if(field_type ==6){
                    //    $(fieldkeyObj).val('slide_list');
                    //    $(fieldnameObj).val('幻灯片广告');
                    //    $(fieldkeyObj).attr("disabled",true);
                    //}
                else if(field_type ==7){
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<tr class="data-toolbar"><td>导航</td><td  colspan="3"><select class="select-toolbar">');
                    for (var i = 0; i < tool_list.length; i++) {
                        appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', tool_list[i].value, tool_list[i].name, i == 0 ? 'selected="selected"' : '');

                    }
                    appendhtml.AppendFormat('</select></td></tr>');
                    $(ptable).append(appendhtml.ToString());
                    
                    var old_t_toolbars = $("td[data-oldtype='7']").closest("table").find(".select-toolbar option:selected[value='"+ tool_list[0].value+"']");
                    if(old_t_toolbars.length==0){
                        $(fieldkeyObj).val(tool_list[0].value + '_list');
                        $(fieldnameObj).val(tool_list[0].name);
                    }
                    else{
                        $(fieldkeyObj).val(tool_list[0].value + '_list'+ "_" + old_t_toolbars.length);
                        $(fieldnameObj).val(tool_list[0].name+ old_t_toolbars.length);
                    }
                    $(fieldkeyObj).attr("disabled",true);
                }
                else if (field_type == 8) {
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<tr class="data-component"><td>页面类型</td><td  colspan="3"><select class="select-component">');
                    for (var i = 0; i < components.length; i++) {
                        appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', components[i].value, components[i].name, i == 0 ? 'selected="selected"' : '');
                    }
                    appendhtml.AppendFormat('</select></td></tr>');
                    $(ptable).append(appendhtml.ToString());

                    var old_t_components = $("td[data-oldtype='8']").closest("table").find(".select-component option:selected[value='"+ components[0].value+"']");
                    if(old_t_components.length==0){
                        $(fieldkeyObj).val('component_' + components[0].value);
                        $(fieldnameObj).val(components[0].name+"页面");
                    }
                    else{
                        $(fieldkeyObj).val('component_' + components[0].value + "_" + old_t_components.length);
                        $(fieldnameObj).val(components[0].name+"页面"+ old_t_components.length);
                    }
                    $(fieldkeyObj).attr("disabled",true);
                }
                $(this).closest("td").attr("data-oldtype", nowtype);
                hideComponents();
            });
            //选择属性类型
            $(".select-component").live("change", function () {
                var ncomponent_value = $(this).find("option:selected").val();
                var ncomponent_name = $(this).find("option:selected").text();
                var fieldkeyObj = $(this).closest("table").find("input[type='text'][name='fieldkey']");
                var fieldnameObj = $(this).closest("table").find("input[type='text'][name='fieldname']");
                var old_t_components = $("td[data-oldtype='8']").closest("table").find(".select-component option:selected[value='"+ ncomponent_value+"']");
                if(old_t_components.length==1){
                    $(fieldkeyObj).val('component_' + ncomponent_value);
                }
                else{
                    $(fieldkeyObj).val('component_' + ncomponent_value + "_" + (old_t_components.length-1));
                }
                $(fieldnameObj).val(ncomponent_name+"页面");
            });
            //导航
            $(".select-toolbar").live("change", function () {
                var ntoolbar_value = $(this).find("option:selected").val();
                var ntoolbar_name = $(this).find("option:selected").text();
                var fieldkeyObj = $(this).closest("table").find("input[type='text'][name='fieldkey']");
                var fieldnameObj = $(this).closest("table").find("input[type='text'][name='fieldname']");
                var old_t_toolbars = $("td[data-oldtype='7']").closest("table").find(".select-toolbar option:selected[value='"+ ntoolbar_value+"']");
                if(old_t_toolbars.length==1){
                    $(fieldkeyObj).val( ntoolbar_value + "_list");
                    $(fieldnameObj).val(ntoolbar_name);
                }
                else{
                    $(fieldkeyObj).val(ntoolbar_value + "_list_" + (old_t_toolbars.length-1));
                    $(fieldnameObj).val(ntoolbar_name+ (old_t_toolbars.length-1));
                }
            });

            //编辑时加载原数据
            if (component_model_id > 0) {
                //$.messager.progress({ text: '正在加载...' });
                $.ajax({
                    type: 'post',
                    url: handlerUrl + "get.ashx",
                    data: { component_model_id: component_model_id,edit_model:"1" },
                    dataType: "json",
                    success: function (resp) {
                        //$.messager.progress('close');
                        if (resp.status) {
                            $("#txtModelName").val(resp.result.component_model_name);
                            $("#dllModelType").val(resp.result.component_model_type);
                            $("#txtModelLinkUrl").val(resp.result.component_model_link_url);
                            $("#txtModelHtmlkUrl").val(resp.result.component_model_html_url);
                            if(resp.result.is_delete == 1) chkIsDelete.checked = true;
                            $(".fields .field").remove();
                            if (resp.result.component_model_fields.length > 0) {
                                var appendhtml = new StringBuilder();
                                for (var i = 0; i < resp.result.component_model_fields.length; i++) {
                                    var nfield = resp.result.component_model_fields[i];
                                    var field_type = parseInt(nfield.component_field_type);
                                    appendhtml.AppendFormat('<div class="field" data-field-index="{0}" data-field-id="{1}">', i, nfield.component_field_id);
                                    appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort"/>');
                                    appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort"/>');
                                    appendhtml.AppendFormat('<img src="/img/delete.png" style="float:right;" class="deletefield"/>');
                                    appendhtml.AppendFormat('<table style="width:100%;margin-left:10px;">');
                                    var disabledBool = field_type==6||field_type==7||field_type==8||field_type==9||field_type==10 ?true:false;
                                    appendhtml.AppendFormat('<tr><td style="width:120px;">组件:</td><td style="width: 325px;"><input type="text" name="fieldkey" class="width300" placeholder="组件Key(必填)" {2} value="{0}" /></td><td style="width: 325px;"><input type="text" name="fieldname" class="width300" placeholder="组件名称(必填)" value="{1}" /></td><td style="width:30px;"></td></tr>', 
                                        nfield.component_field, nfield.component_field_name,disabledBool?'disabled="disabled"':'');

                                    appendhtml.AppendFormat('<tr><td>类型:</td><td colspan="3" data-oldtype="{0}">', nfield.component_field_type);
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="0" id="rd{0}0"/><label for="rd{0}0" class="hand red">输入</label>', i, nfield.component_field_type == 0 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="1" id="rd{0}1"/><label for="rd{0}1" class="hand red">选择</label>', i, nfield.component_field_type == 1 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="2" id="rd{0}2"/><label for="rd{0}2" class="hand red">图片</label>', i, nfield.component_field_type == 2 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand red" disabled="disabled" {1} value="3" id="rd{0}3"/><label for="rd{0}3" class="hand red">颜色</label>', i, nfield.component_field_type == 3 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand" {1} value="4" id="rd{0}4"/><label for="rd{0}4" class="hand">数组</label>', i, nfield.component_field_type == 4 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand" {1} value="5" id="rd{0}5"/><label for="rd{0}5" class="hand">对象</label>', i, nfield.component_field_type == 5 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand slide-to" {1} value="6" id="rd{0}6"/><label for="rd{0}6" class="hand slide-to">广告</label>', i, nfield.component_field_type == 6 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand tool-to red" disabled="disabled" {1} value="7" id="rd{0}7"/><label for="rd{0}7" class="hand tool-to red">导航</label>', i, nfield.component_field_type == 7 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand component-to red" disabled="disabled" {1} value="8" id="rd{0}8"/><label for="rd{0}8" class="hand component-to red">页面</label>', i, nfield.component_field_type == 8 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand navs-to" {1} value="9" id="rd{0}9"/><label for="rd{0}9" class="hand navs-to">导航数组</label>', i, nfield.component_field_type == 9 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('<input name="rdtype{0}" type="radio" class="positionTop2 hand malls-to" {1} value="10" id="rd{0}10"/><label for="rd{0}10" class="hand malls-to">商品数组</label>', i, nfield.component_field_type == 10 ? 'checked="checked"' : "");
                                    appendhtml.AppendFormat('</td></tr>');
                                    appendhtml.AppendFormat('<tr class="data-source" style="display:none;"><td>数据:</td><td colspan="3"><input type="radio" name="rddata{0}" value="0" checked="checked" id="rdd{0}0"/><label for="rdd{0}0">固定</label></td></tr>', i);
                                    if (field_type == 0) {
                                        appendhtml.AppendFormat('<tr class="data-text"><td>默认值:</td><td colspan="3"><input type="text" class="width300" placeholder="默认值" value="{0}" /></td></tr>', nfield.component_field_data_value);
                                    }
                                    else if (field_type == 1) {
                                        if (nfield.component_field_data_value && nfield.component_field_data_value != "") {
                                            var data_options = nfield.component_field_data_value.split("@");
                                            for (var j = 0; j < data_options.length; j++) {
                                                var datakv = data_options[j].split("|");
                                                appendhtml.AppendFormat('<tr class="data-select"><td class="option">选项:</td><td><input type="text" name="key" placeholder="选项名称" value="{0}" /></td><td><input type="text" name="value" placeholder="选项值" value="{1}" /></td><td><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteoption"/></td></tr>', datakv[0], datakv[1]);
                                            }
                                        }
                                        appendhtml.AppendFormat('<tr class="data-select"><td></td><td colspan="3"><a class="button button-rounded button-primary addoption">添加选项</a></td></tr>');
                                    }
                                    else if (field_type == 3) {
                                        appendhtml.AppendFormat('<tr class="data-text"><td>默认颜色:</td><td colspan="3"><input type="text" class="width300 color"  value="{0}" /></td></tr>', nfield.component_field_data_value);
                                    }
                                    else if (field_type == 4 || field_type == 5 || field_type==6 || field_type==9 || field_type==10) {
                                        if (nfield.component_field_data_value && nfield.component_field_data_value != "") {
                                            var data_options = nfield.component_field_data_value.split("@");
                                            for (var j = 0; j < data_options.length; j++) {
                                                var datakv = data_options[j].split("|");
                                                appendhtml.AppendFormat('<tr class="data-arrays"><td class="array" style="vertical-align:top;">属性:<img src="/img/delete.png" width="20" height="20" alt="删除属性" class="deletearray floatR hand" /><img src="/img/icons/up.png" width="20" height="20" alt="排序" class="uparray floatR hand" /></td><td style="vertical-align:top;">');
                                                appendhtml.AppendFormat('<input type="text" name="key" class="width120" placeholder="属性Key" value="{0}"/><input type="text" name="name" class="width120" placeholder="属性名称" value="{1}"/><select class="width60">', datakv[0], datakv[1]);
                                                var datakt = parseInt(datakv[2]);
                                                appendhtml.AppendFormat('<option value="0" {0}>输入</option>', datakt == 0 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="1" {0}>选择</option>', datakt == 1 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="2" {0}>图片</option>', datakt == 2 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="3" {0}>颜色</option>', datakt == 3 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="4" {0}>图标</option>', datakt == 4 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="5" {0}>数值</option>', datakt == 5 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="6" {0}>链接</option>', datakt == 6 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('<option value="7" {0}>商品</option>', datakt == 7 ? 'selected="selected"' : '');
                                                appendhtml.AppendFormat('</select></td><td>');
                                                if (datakt == 0) {
                                                    appendhtml.AppendFormat('<input type="text" class="width300" placeholder="默认值" value="{0}" />', datakv[3]);
                                                }
                                                else if (datakt == 1) {
                                                    appendhtml.AppendFormat('<ul>');
                                                    var datakli = datakv[3].split("$");
                                                    for (var ij = 0; ij < datakli.length; ij++) {
                                                        var dataklv = datakli[ij].split("#");
                                                        appendhtml.AppendFormat('<li>');
                                                        appendhtml.AppendFormat('<input type="text" name="likey" class="width120 floatL" placeholder="属性选项名称" value="{0}" />', dataklv[0]);
                                                        appendhtml.AppendFormat('<input type="text" name="livalue" class="width120 floatL" placeholder="属性选项值" value="{0}" />', dataklv[1]);
                                                        appendhtml.AppendFormat('<img src="/img/delete.png" width="20" height="20" alt="删除属性选项" class="deletearrayoption floatL hand" />');
                                                        appendhtml.AppendFormat('</li>');
                                                    }
                                                    appendhtml.AppendFormat('</ul>');
                                                }
                                                else if (datakt == 3) {
                                                    appendhtml.AppendFormat('<input type="text" class="width300 color" value="{0}" />', datakv[3]);
                                                }
                                                else if (datakt == 5) {
                                                    var datakli = datakv[3].split("$");
                                                    appendhtml.AppendFormat('<table>');
                                                    appendhtml.AppendFormat('<tr><td class="width60">最小值</td><td><input type="text" {0} /></td>', (datakli.length>0?'value="'+datakli[0]+'"':''));
                                                    appendhtml.AppendFormat('<td class="width60">最大值</td><td><input type="text" {0} /></td></tr>', (datakli.length>1?'value="'+datakli[1]+'"':''));
                                                    appendhtml.AppendFormat('<tr><td class="width60">每格值</td><td><input type="text" {0} /></td>', (datakli.length>2?'value="'+datakli[2]+'"':''));
                                                    appendhtml.AppendFormat('<td class="width60">默认值</td><td><input type="text" {0} /></td></tr>', (datakli.length>3?'value="'+datakli[3]+'"':''));
                                                    appendhtml.AppendFormat('</table>');
                                                }
                                                appendhtml.AppendFormat('</td><td style="vertical-align:top;">');
                                                if (datakt == 1) {
                                                    appendhtml.AppendFormat('<img src="/img/icons/add.png" width="20" height="20" alt="添加属性选项" class="addarrayoption hand" />');
                                                }
                                                appendhtml.AppendFormat('</td></tr>');
                                            }
                                        }
                                        appendhtml.AppendFormat('<tr class="data-arrays"><td></td><td colspan="3"><a class="button button-rounded button-primary addarray">添加属性</a></td></tr>');
                                    }
                                    else if(field_type ==7){
                                        appendhtml.AppendFormat('<tr class="data-toolbar"><td>导航</td><td  colspan="3"><select class="select-toolbar">');
                                        for (var j = 0; j < tool_list.length; j++) {
                                            appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', tool_list[j].value, tool_list[j].name, tool_list[j].value == nfield.component_field_data_value ? 'selected="selected"' : '');
                                        }
                                        appendhtml.AppendFormat('</select></td></tr>');
                                    }
                                    else if (field_type == 8) {
                                        appendhtml.AppendFormat('<tr class="data-component"><td>页面类型</td><td  colspan="3"><select class="select-component">');
                                        for (var j = 0; j < components.length; j++) {
                                            appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', components[j].value, components[j].name, components[j].value == nfield.component_field_data_value ? 'selected="selected"' : '');
                                        }
                                        appendhtml.AppendFormat('</select></td></tr>');
                                    }
                                    appendhtml.AppendFormat('</table>');
                                    appendhtml.AppendFormat('</div>');
                                }
                                $("#btnaddfield").before(appendhtml.ToString());
                                hideComponents();
                                $('.color').spectrum();
                            }
                        }
                        else {
                            Alert(resp.msg);
                        }
                    }
                });
            }
            //编辑时加载原数据
            //提交
            $('#btnSave').click(function () {
                try {
                    var dv = $.trim($("#txtModelName").val());
                    if (dv == "") {
                        Alert("请输入组件库名称");
                        $("#txtModelName").focus();
                        return false;
                    }
                    var dt = $.trim($("#dllModelType").val());
                    if (dt == "") {
                        Alert("选择组件库分类");
                        $("#dllModelType").focus();
                        return false;
                    }
                    //模型
                    var postDataModel = {
                        component_model_id: component_model_id,
                        component_model_name: dv,
                        component_model_type: dt,
                        component_model_link_url: $.trim($("#txtModelLinkUrl").val()),
                        component_model_html_url: $.trim($("#txtModelHtmlkUrl").val()),
                        component_model_fields: [],
                        is_delete : chkIsDelete.checked == true?1:0
                    };
                    var checkresult = true;
                    var objs = $(".fields .field");
                    for (var i = 0; i < objs.length; i++) {
                        var nfield = {};
                        var nobj = objs[i];

                        var field_id = parseInt($(nobj).attr("data-field-id"));
                        nfield.component_field_id = field_id;
                        nfield.component_field_sort = i+1;

                        var fieldKeyObj = $(nobj).find("input[name='fieldkey']")[0];
                        var field_key = $.trim($(fieldKeyObj).val());
                        if (field_key == "") {
                            $(fieldKeyObj).focus();
                            Alert("请完成组件Key");
                            checkresult = false;
                            break;
                        }
                        var haveRepeatFieldKey = false;
                        for (var j = 0; j < postDataModel.component_model_fields.length; j++) {
                            if (postDataModel.component_model_fields[j].component_field == field_key) {
                                haveRepeatFieldKey = true;
                                break;
                            }
                        }
                        if (haveRepeatFieldKey) {
                            $(fieldKeyObj).focus();
                            Alert("组件Key已存在");
                            checkresult = false;
                            break;
                        }

                        nfield.component_field = field_key;

                        var fieldNameObj = $(nobj).find("input[name='fieldname']")[0];
                        var field_name = $.trim($(fieldNameObj).val());
                        if (field_name == "") {
                            $(fieldNameObj).focus();
                            Alert("请完成组件名称");
                            checkresult = false;
                            break;
                        }
                        nfield.component_field_name = field_name;

                        var field_type = parseInt($(nobj).find("input[type='radio'][name^='rdtype']:checked").val());
                        nfield.component_field_type = field_type;

                        var data_type = parseInt($(nobj).find("input[type='radio'][name^='rddata']:checked").val());
                        nfield.component_field_data_type = data_type;

                        if (field_type == 0) {
                            var nval = $.trim($(nobj).find(".data-text input[type='text']").val());
                            nfield.component_field_data_value = nval;
                        }
                            //选择
                        else if (field_type == 1) {
                            var opObjs = $(nobj).find(".option");
                            if (opObjs.length == 0) {
                                Alert("未找到组件[" + field_name + "]的选项");
                                checkresult = false;
                                break;
                            }
                            var data_value = "";
                            for (var j = 0; j < opObjs.length; j++) {
                                if (j > 0) data_value += "@";
                                var optionKeyObj = $(opObjs[j]).closest("tr").find("input[name='key']")[0];
                                var option_key = $.trim($(optionKeyObj).val());
                                if (option_key == "") {
                                    $(optionKeyObj).focus();
                                    Alert("请填写选项的名称");
                                    checkresult = false;
                                    break;
                                }
                                if (("@" + data_value).indexOf("@" + option_key + "|") >= 0) {
                                    $(optionKeyObj).focus();
                                    Alert("选项已存在");
                                    checkresult = false;
                                    break;
                                }
                                var optionValueObj = $(opObjs[j]).closest("tr").find("input[name='value']")[0];
                                var option_value = $.trim($(optionValueObj).val());
                                if (option_value == "") {
                                    $(optionValueObj).focus();
                                    Alert("请填写选项的值");
                                    checkresult = false;
                                    break;
                                }
                                data_value += option_key + "|" + option_value;
                            }
                            if (checkresult == false) {
                                break;
                            }
                            nfield.component_field_data_value = data_value;
                        }
                        else if (field_type == 3) {
                            var nval = $.trim($(nobj).find(".data-text input[type='text']").val());
                            nfield.component_field_data_value = nval;
                        }
                            //数组或对象
                        else if (field_type == 4 || field_type == 5 || field_type == 6 || field_type == 9 || field_type == 10) {
                            var opObjs = $(nobj).find(".array");
                            if (opObjs.length == 0) {
                                Alert("未找到组件[" + field_name + "]的属性");
                                checkresult = false;
                                break;
                            }
                            var data_value = "";
                            for (var j = 0; j < opObjs.length; j++) {
                                if (j > 0) data_value += "@";
                                var optionKeyObj = $(opObjs[j]).closest("tr").find("input[name='key']")[0];
                                var option_key = $.trim($(optionKeyObj).val());
                                if (option_key == "") {
                                    $(optionKeyObj).focus();
                                    Alert("请填写属性的Key");
                                    checkresult = false;
                                    break;
                                }
                                if (("@" + data_value).indexOf("@" + option_key + "|") >= 0) {
                                    $(optionKeyObj).focus();
                                    Alert("属性已存在");
                                    checkresult = false;
                                    break;
                                }
                                var optionValueObj = $(opObjs[j]).closest("tr").find("input[name='name']")[0];
                                var option_value = $.trim($(optionValueObj).val());
                                if (option_value == "") {
                                    $(optionValueObj).focus();
                                    Alert("请填写属性的名称");
                                    checkresult = false;
                                    break;
                                }
                                var optionSelectObj = $(opObjs[j]).closest("tr").find("select")[0];
                                var optionSelectVal = $(optionSelectObj).val();
                                optionSelectVal = parseInt(optionSelectVal);
                                data_value += option_key + "|" + option_value + "|" + optionSelectVal;
                                if (optionSelectVal == 0) {
                                    var liValue = $.trim($(optionSelectObj).closest("td").next().find("input[type='text']").val());
                                    data_value += "|" + liValue;
                                }
                                else if (optionSelectVal == 1) {
                                    var liObjs = $(optionSelectObj).closest("td").next().find("li");
                                    if (liObjs.length == 0) {
                                        Alert("属性为选择时，未设置选项");
                                        checkresult = false;
                                        break;
                                    }
                                    var liArrayValue = "";
                                    for (var ij = 0; ij < liObjs.length; ij++) {
                                        if (ij > 0) liArrayValue += "$";
                                        var likeyObj = $(liObjs[ij]).find("input[name='likey']")[0];
                                        var likey = $.trim($(likeyObj).val());
                                        if (likey == "") {
                                            $(likeyObj).focus();
                                            Alert("请填写属性[" + option_value + "]的选项名称");
                                            checkresult = false;
                                            break;
                                        }
                                        if (("@" + liArrayValue).indexOf("@" + likey + "|") >= 0) {
                                            $(likeyObj).focus();
                                            Alert("属性[" + option_value + "]的选项[" + likey + "]已存在");
                                            checkresult = false;
                                            break;
                                        }
                                        var liValueObj = $(liObjs[ij]).find("input[name='livalue']")[0];
                                        var livalue = $.trim($(liValueObj).val());
                                        if (livalue == "") {
                                            $(liValueObj).focus();
                                            Alert("请填写属性[" + option_value + "]的选项值");
                                            checkresult = false;
                                            break;
                                        }
                                        liArrayValue += likey + "#" + livalue;
                                    }
                                    data_value += "|" + liArrayValue;
                                }
                                else if (optionSelectVal == 3) {
                                    var liValue = $.trim($(optionSelectObj).closest("td").next().find("input[type='text']").val());
                                    data_value += "|" + liValue;
                                }
                                else if (optionSelectVal == 5) {
                                    var liObjs = $(optionSelectObj).closest("td").next().find("input[type='text']");
                                    var liArrayValue = "";
                                    for (var ij = 0; ij < liObjs.length; ij++) {
                                        if (ij > 0) liArrayValue += "$";
                                        liArrayValue += $.trim($(liObjs.get(ij)).val());
                                    }
                                    data_value += "|" + liArrayValue;
                                }
                            }
                            if (checkresult == false) {
                                break;
                            }
                            nfield.component_field_data_value = data_value;
                        }
                        else if (field_type == 7) {
                            var data_value = $(nobj).find("select option:selected").val();
                            nfield.component_field_data_value = data_value;
                        }
                        else if (field_type == 8) {
                            var data_value = $(nobj).find("select option:selected").val();
                            nfield.component_field_data_value = data_value;
                        }
                        postDataModel.component_model_fields.push(nfield);
                    }
                    if (checkresult == false) {
                        return;
                    }
                    $.messager.progress({ text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl + action,
                        data: { data: JSON.stringify(postDataModel) },
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status) {
                                window.location.href = "List.aspx";
                            }
                            else {
                                Alert(resp.msg);
                            }
                        }
                    });

                } catch (e) {
                    Alert(e);
                }
            });
            //提交
            //返回列表
            $('#btnPageBack').click(function () {
                window.location.href = "List.aspx";
            });
            //返回列表
        });
        //隐藏选择页面
        function hideComponents() {
            //if($("td[data-oldtype='6']").length>0){
            //    $(".slide-to").hide();
            //    $("td[data-oldtype='6']").find(".slide-to").show();
            //}
            //else{
            //    $(".slide-to").show();
            //}
            if(tool_list.length>0){
                $(".tool-to").show();
            }
            else{
                $(".tool-to").hide();
            }
            var val = $("#dllModelType").val();
            if (val == "page") {
                $(".component-to").show();
            }
            else {
                $(".component-to").hide();
            }
        }
        function loadTypeList(){
            var appendhtml = new StringBuilder();
            for (var i = 0; i < keyvalue_list.length; i++) {
                appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', keyvalue_list[i].value, keyvalue_list[i].name,i==0?'selected="selected"':'');
                if(keyvalue_list[i].value !="page") components.push(keyvalue_list[i]);
            }
            $("#dllModelType").html("");
            $("#dllModelType").append(appendhtml.ToString());
            hideComponents();
        }
    </script>
</asp:Content>
