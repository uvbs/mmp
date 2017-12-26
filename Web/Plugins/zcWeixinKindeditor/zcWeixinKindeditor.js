
(function ($) {
    //实例化插件
    $.fn.zcWeixinKindeditor = function (options) {
        var opts = $.extend({}, $.fn.zcWeixinKindeditor.defaults, options);
        var nWeixinKindeditor = this.each(function () {
            var $this = $(this);
            var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
            if (!o.keditor) { return; }
            o.$this = $this;
            $($this).html('');
            $($this).addClass('wx_editor').css('height', o.height + 'px');
            var str = new StringBuilder();
            str.AppendFormat('<table class="ed_table" cellpadding="0" cellspacing="0" style="height:' + o.height + 'px;">');
            str.AppendFormat('<tr>');
            str.AppendFormat('<td class="w1">');
            str.AppendFormat('<div class="n1 wxEditorTheme">');
            str.AppendFormat('<div class="wxEditorThemeDiv">');
            str.AppendFormat('<span class="wxEditorThemeRadius"><span class="wxEditorThemeRadiusInner" data-color=""></span></span>');
            str.AppendFormat('</div>');
            str.AppendFormat('<div class="wxEditorThemeTitle">主题色</div>');
            str.AppendFormat('<div class="wxEditorThemeTextColor"><input type="text" class="zcWeixinKindeditorColor" placeholder="默认色" value="" /></div>');
            str.AppendFormat('</div>');
            str.AppendFormat('<div class="cate" style="height:' + (o.height-65) + 'px;">');
            str.AppendFormat('<ul class="n1-1">');
            str.AppendFormat('</ul>');
            str.AppendFormat('</div>');
            str.AppendFormat('</td>');
            str.AppendFormat('<td class="w2">');
            str.AppendFormat('<div class="show">');
            str.AppendFormat('<div class="n2" style="display:none;">样式展示区</div>');
            str.AppendFormat('<div></div>');
            str.AppendFormat('<div class="item" style="height:' + o.height+ 'px;">');
            str.AppendFormat('<div class="tab-pane">');
            str.AppendFormat('<ul class="editor-template-list">');
            str.AppendFormat('</ul>');
            str.AppendFormat('</div>');
            str.AppendFormat('</div>');
            str.AppendFormat('</div>');
            str.AppendFormat('</td>');
            str.AppendFormat('</tr>');
            str.AppendFormat('</table>');

            $($this).append(str.ToString());

            var themeSet = $.extend({}, o.color_set);
            $($this).find('.zcWeixinKindeditorColor').spectrum(themeSet);
            $($this).find('.wxEditorTheme').bind('click', function () {
                o.cur_edit_theme = true;
                var tempThemeSet = $.extend({}, o.color_set);
                tempThemeSet.allowEmpty = true;
                tempThemeSet.hide = function (color) { $.fn.zcWeixinKindeditor.setTheme(color, $this, o); };
                tempThemeSet.move = function (color) { $.fn.zcWeixinKindeditor.moveTheme(color, $this, o); };
                tempThemeSet.change = function (color) { $.fn.zcWeixinKindeditor.setTheme(color, $this, o); };
                var tColor = $($this).find(".wxEditorThemeRadiusInner").attr('data-color');
                $($this).find('.zcWeixinKindeditorColor').val(tColor);
                $($this).find('.zcWeixinKindeditorColor').spectrum(tempThemeSet);
                $($this).find('.zcWeixinKindeditorColor').spectrum("toggle");

                var _t = $(this).offset().top;
                var _l = $(this).offset().left;
                var _st = 0;
                if ($(document.body).length > 0) {
                    _st = $(document.body).scrollTop();
                }
                var _zt = _t - 240;
                var _zl = _zl - 180;
                if (_l < 200) {
                    _zl = 20;
                }
                if (_t - _st < 240) {
                    _zt = _t + 80;
                }
                $('.sp-container').css('left', _zl + 'px');
                $('.sp-container').css('top', _zt + 'px');
                return false;
            });

            $($this).find(".show .item").mCustomScrollbar({ theme: "minimal-dark" });
            $.fn.zcWeixinKindeditor.getWeixinKindeditorCateList($this, o);

            $(o.keditor.edit.iframe[0].contentDocument).scroll(function () {
                $($this).find('.zcWeixinKindeditorColor').spectrum("hide");
            });
            $(o.keditor.edit.iframe[0].contentDocument.body).bind('click', function () {
                var _zcThis = $(o.keditor.cmd.range.startContainer);
                if (!$(_zcThis).hasClass('zcWxEditorView')) {
                    _zcThis = $(_zcThis).closest('.zcWxEditorView');
                }
                if ($(_zcThis).hasClass('zcWxEditorSelect')) return;

                $($this).find('.zcWeixinKindeditorColor').spectrum("hide");
                $(o.keditor.edit.iframe[0].contentDocument.body).find('.zcWxEditorSelect').removeClass('zcWxEditorSelect');
                $(o.keditor.edit.iframe[0].contentDocument.body).find('.zcWxEditorTools').remove();

                if (!$(_zcThis).hasClass('zcWxEditorView')) return;

                o.cur_select_value = $(_zcThis).html();
                $(_zcThis).addClass('zcWxEditorSelect');
                $.fn.zcWeixinKindeditor.showTools(_zcThis, o);
            });
        });
        nWeixinKindeditor.clearEditorSelect = function () {
            var $this = $(this);
            var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
            $(o.keditor.edit.iframe[0].contentDocument.body).find('.zcWxEditorSelect').removeClass('zcWxEditorSelect');
            $(o.keditor.edit.iframe[0].contentDocument.body).find('.zcWxEditorTools').remove();
        }
        return nWeixinKindeditor;
    };

    $.fn.zcWeixinKindeditor.showTools = function (_zcThis, o) {
        var editTools = $('<div class="zcWxEditorTools"></div>');
        var deleteTool = $('<span data-operate="delete">删除</span>');
        $(deleteTool).unbind().bind('click', function () {
            $(_zcThis).remove();
        });

        var beforeTool = $('<span data-operate="insert-before">前空行</span>');
        $(beforeTool).unbind().bind('click', function () {
            $(_zcThis).before('<p><br /></p>');
        });
        var afterTool = $('<span data-operate="insert-after">后空行</span>');
        $(afterTool).unbind().bind('click', function () {
            $(_zcThis).after('<p><br /></p>');
        });
        var upTool = $('<span data-operate="up">上移</span>');
        $(upTool).unbind().bind('click', function () {
            var _prev = $(_zcThis).prev();
            if (_prev.length > 0) {
                var _prevclone = $(_prev).clone();
                $(_zcThis).after(_prevclone);
                $(_prev).remove();
            }
        });
        var downTool = $('<span data-operate="down">下移</span>');
        $(downTool).unbind().bind('click', function () {
            var _next = $(_zcThis).next();
            if (_next.length > 0) {
                var _nextclone = $(_next).clone();
                $(_zcThis).before(_nextclone);
                $(_next).remove();
            }
        });
        $(editTools).append(deleteTool);
        $(editTools).append(beforeTool);
        $(editTools).append(afterTool);
        $(editTools).append(upTool);
        $(editTools).append(downTool);
        //$(editTools).append(noselectTool);
        //$(editTools).append(copyTool);
        //$(editTools).append(cutTool);
        var tool_w = 195;
        var hasThemeColor = false;
        if ($(_zcThis).find('[edit-bgcolor],[edit-bdcolor],[edit-color]').length > 0) {
            var colorTool = $('<span data-operate="color">主题色</span>');
            $(colorTool).unbind().bind('click', function () {
                var _dw = $(document).width();
                var _t = $(this).offset().top;
                var _l = $(this).offset().left;
                var _kw = $(o.keditor.container).width();
                var _kh = $(o.keditor.container).height();
                var _kth = $(o.keditor.container).find('.ke-toolbar').height();
                var _kt = $(o.keditor.container).offset().top;
                var _kl = $(o.keditor.container).offset().left;

                var _kst = 0;
                if ($(o.keditor.edit.iframe[0].contentDocument.body).length > 0) {
                    _kst = $(o.keditor.edit.iframe[0].contentDocument.body).scrollTop();
                }
                //console.log(_t);
                //console.log(_l);
                //console.log($(o.keditor.container));

                var _zt = _t + _kt + _kth + 24 - _kst;
                var _zl = _l + _kl;
                if (_dw < _l + _kl + 400) _zl = _l + _kl - 330;
                if (_kh - _kth - _t < 200) _zt = _zt - 267;

                o.cur_edit_theme = false;
                var colorSet = $.extend({}, o.color_set);
                colorSet.hide = function (color) { $.fn.zcWeixinKindeditor.setTheme(color, _zcThis, o); };
                colorSet.move = function (color) { $.fn.zcWeixinKindeditor.moveTheme(color, _zcThis, o); };
                colorSet.change = function (color) { $.fn.zcWeixinKindeditor.setTheme(color, _zcThis, o); };
                //colorSet.allowEmpty = false;
                $(o.$this).find('.zcWeixinKindeditorColor').val("");
                $(o.$this).find('.zcWeixinKindeditorColor').spectrum(colorSet);
                $(o.$this).find('.zcWeixinKindeditorColor').spectrum("toggle");
                $('.sp-container').css('left', _zl + 'px');
                $('.sp-container').css('top', _zt + 'px');
                return false;
            });
            $(editTools).append(colorTool);
            tool_w += 45;
        }
        $(editTools).css('width', tool_w + 'px');
        $(_zcThis).append(editTools);
    }
    $.fn.zcWeixinKindeditor.setTheme = function (color, _this, o) {
        var color_str = "";
        if (color) color_str = color.toRgbString();
        if (o.cur_edit_theme) {
            o.cur_theme_color = color_str;
            if (color_str == "") {
                $(_this).find(".wxEditorThemeRadiusInner").css('background', 'RGB(255, 255, 255)');
            }
            else {
                $(_this).find(".wxEditorThemeRadiusInner").css('background', color_str);
            }
            $(_this).find(".wxEditorThemeRadiusInner").attr('data-color', color_str);
            $.fn.zcWeixinKindeditor.selectCate(_this, o, o.cur_key_value_list, color_str);
        }
        else {
            if (color_str == "") {
                $(_this).html(o.cur_select_value);
                $.fn.zcWeixinKindeditor.showTools(_this,o);
            }
            else {
                $(_this).find('[edit-bgcolor],[edit-bdcolor],[edit-color]').each(function () {
                    $.fn.zcWeixinKindeditor.setLiColor(color_str, this, o);
                });
                o.cur_select_value = $(_this).clone().find('.zcWxEditorTools').remove();
            }
        }
    }
    $.fn.zcWeixinKindeditor.moveTheme = function (color, _this, o) {
        var color_str = "";
        if (color) color_str = color.toRgbString();
        if (o.cur_edit_theme) {
            if(color_str ==""){
                $.fn.zcWeixinKindeditor.selectCate(_this, o, o.cur_key_value_list, color_str);
            }
            else{
                $(_this).find('.w2 .show .item .editor-template-list').find('[edit-bgcolor],[edit-bdcolor],[edit-color]').each(function () {
                    $.fn.zcWeixinKindeditor.setLiColor(color_str, this, o);
                });
            }
        }
        else {
            if (color_str == "") {
                $(_this).html(o.cur_select_value);
                $.fn.zcWeixinKindeditor.showTools(_this, o);
            }
            else {
                $(_this).find('[edit-bgcolor],[edit-bdcolor],[edit-color]').each(function () {
                    $.fn.zcWeixinKindeditor.setLiColor(color_str, this, o);
                });
            }
        }
    }
    $.fn.zcWeixinKindeditor.setLiColor = function (color_str, _this, o) {
        var editBgcolor = $(_this).attr('edit-bgcolor');
        var editBdcolor = $(_this).attr('edit-bdcolor');
        var editColor = $(_this).attr('edit-color');
        if (typeof (editBgcolor) != 'undefined') {
            $(_this).css('background-color', color_str);
        }
        if (typeof (editBdcolor) != 'undefined') {
            var cblc = $(_this).css('border-left-color');
            if (cblc != 'transparent') $(_this).css('border-left-color', color_str);
            var cbtc = $(_this).css('border-top-color');
            if (cbtc != 'transparent') $(_this).css('border-top-color', color_str);
            var cbrc = $(_this).css('border-right-color');
            if (cbrc != 'transparent') $(_this).css('border-right-color', color_str);
            var cbbc = $(_this).css('border-bottom-color');
            if (cbbc != 'transparent') $(_this).css('border-bottom-color', color_str);
        }
        if (typeof (editColor) != 'undefined') {
            $(_this).css('color', color_str);
        }
    }
    //post提交
    $.fn.zcWeixinKindeditor.post = function (url, reqData, callBack, failCallBack) {
        $.ajax({
            type: 'POST',
            url: url,
            data: reqData,
            success: function (result) {   //function1()
                callBack(result);
            },
            failure: function (result) {
                failCallBack(result);
            }
        });
    };
    //获取数据
    $.fn.zcWeixinKindeditor.getWeixinKindeditorCateList = function (_this, o) {
        var getWeixinKindeditorCateUrl = '/Serv/API/Common/zcWeixinKindeditorCateList.ashx';
        $.fn.zcWeixinKindeditor.post(o.domain + getWeixinKindeditorCateUrl, {}, function (data) {
            if(data.status){
                $.fn.zcWeixinKindeditor.insertCateHtml(_this, o, data.result);
            }
        }, function (data) { })
    }
    //获取
    $.fn.zcWeixinKindeditor.insertCateHtml = function (_this, o, data) {
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                var li = $('<li class="dropdown"></li>');
                var a = $('<a class="filter dropdown-toggle" href="javascript:void(0);">'+data[i].cate_name+'</a>');
                $(li).append($(a));
                $(li).data('i', i);

                $(a).addClass('cursor_default');
                $(li).unbind().bind('mouseover', function () {
                    $(this).addClass('open');
                }).bind('mouseout', function () {
                    $(this).removeClass('open');
                });
                var cul = $('<ul class="dropdown-menu"></ul>');
                if(data[i].child_cate_list.length>0) {
                    for (var j = 0; j < data[i].child_cate_list.length; j++) {
                        var cli = $('<li></li>');
                        $(cli).data('i', i);
                        $(cli).data('j', j);
                        var ca = $('<a href="javascript:void(0);">'+data[i].child_cate_list[j].cate_name+'</a>');
                        $(cli).unbind().bind('mouseover',function(){
                            $(this).addClass('open');
                        }).bind('mouseout',function(){
                            $(this).removeClass('open');
                        }).bind('click', function () {
                            $(_this).find('.active').removeClass('active');
                            var pli = $(this).parents('.dropdown');
                            $(pli).addClass('active');
                            $(this).addClass('active');
                            var ci = parseInt($(this).data('i'));
                            var cj = parseInt($(this).data('j'));
                            o.cur_key_value_list = data[ci].child_cate_list[cj].key_value_list;
                            $.fn.zcWeixinKindeditor.selectCate(_this, o, data[ci].child_cate_list[cj].key_value_list, o.cur_theme_color);
                        });
                        $(cli).append($(ca));
                        $(cul).append($(cli));

                        if (data[i].child_cate_list[j].cate_id == o.def_cate) {
                            $(cli).addClass('active');
                            $(li).addClass('active');
                            o.cur_key_value_list = data[i].child_cate_list[j].key_value_list;
                            $.fn.zcWeixinKindeditor.selectCate(_this, o, data[i].child_cate_list[j].key_value_list, o.cur_theme_color);
                        }
                    }
                }
                else {
                    var cli = $('<li></li>');
                    $(cli).data('i', i);
                    $(cli).data('j', -1);
                    var ca = $('<a href="javascript:void(0);">' + data[i].cate_name + '</a>');
                    $(cli).unbind().bind('mouseover', function () {
                        $(this).addClass('open');
                    }).bind('mouseout', function () {
                        $(this).removeClass('open');
                    }).bind('click', function () {
                        $(_this).find('.active').removeClass('active');
                        var pli = $(this).parents('.dropdown');
                        $(pli).addClass('active');
                        $(this).addClass('active');
                        var ci = parseInt($(this).data('i'));
                        o.cur_key_value_list = data[ci].key_value_list;
                        $.fn.zcWeixinKindeditor.selectCate(_this, o, data[ci].key_value_list, o.cur_theme_color);
                    });
                    $(cli).append($(ca));
                    $(cul).append($(cli));

                    if (data[i].cate_id == o.def_cate) {
                        $(cli).addClass('active');
                        $(li).addClass('active');
                        o.cur_key_value_list = data[i].key_value_list;
                        $.fn.zcWeixinKindeditor.selectCate(_this, o, data[i].key_value_list, o.cur_theme_color);
                    }
                }
                $(li).append($(cul));
                $(_this).find('.n1-1').append($(li));
            }
        }
    }
    $.fn.zcWeixinKindeditor.selectCate = function (_this, o, key_value_list, color_str) {
        $(_this).find('.w2 .show .item .editor-template-list').html('');
        for (var i = 0; i < key_value_list.length; i++) {
            var sli = $('<li></li>');
            var sl = $('<section class="zcWxEditor"></section>');
            $(sli).append(key_value_list[i].data_value);
            $(sli).unbind().bind('mouseover', function () {
                $(this).addClass('open');
            }).bind('mouseout', function () {
                $(this).removeClass('open');
            }).bind('click', function () {
                var wxedit = $(this).children().get(0);
                if (!$(wxedit).hasClass('zcWxEditorView')) $(wxedit).addClass('zcWxEditorView')
                var innerhtml = $(wxedit).prop('outerHTML');
                innerhtml = '<p><br></p>' + innerhtml + '<section style="clear:both;"></section><p><br></p>';
                o.keditor.insertHtml(innerhtml);
                //console.log(o.keditor);
            });
            if (color_str != '') {
                $(sli).find('[edit-bgcolor],[edit-bdcolor],[edit-color]').each(function () {
                    $.fn.zcWeixinKindeditor.setLiColor(color_str, this, o);
                });
            }
            $(_this).find('.w2 .show .item .editor-template-list').append($(sli));
        }
    }
    //默认值
    $.fn.zcWeixinKindeditor.defaults = {
        height: 600,
        keditor: null,
        domain: '',
        def_cate: '1216',
        cur_theme_color:'',
        cur_edit_theme: false,
        cur_select_value: null,
        cur_key_value_list: [],
        color_set: {
            flat: false,//直接显示颜色选择器
            showInput: true,//颜色输入框
            allowEmpty: true,//能否为空
            showAlpha: true,//是否可透明
            className: "full-spectrum",
            showButtons: true,//显示确认取消按钮
            chooseText: "确认",
            cancelText: "取消",
            showInitial: true,
            clickoutFiresChange: true,//自动保存
            preferredFormat: "rgb",//输出格式
            showPalette: true,//推荐色
            showSelectionPalette: true,//推荐色选中
            localStorageKey: "spectrum.local",//选色历史记录
            maxSelectionSize: 8,//最大历史保存数
            showPaletteOnly: false,//仅显示推荐色
            togglePaletteOnly: false,//显示高级按钮
            togglePaletteMoreText: '高级',
            togglePaletteLessText: '收起',
            hideAfterPaletteSelect: true,//选择推荐色后隐藏
            //appendTo: '#sd',//颜色选择器父容器，默认body
            move: function (color) { },
            change: function (color) { },
            show: function (color) {

            },
            beforeShow: function (color) {

            },
            hide: function (color) {
            },
            palette: [
                ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
            ]
        }
    };
})(jQuery);