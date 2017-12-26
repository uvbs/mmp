//作者：杜恒；
//版本：3.0；
//调用方式       $("input[type='text'],input[type='password'],textarea").defFieldLabels();
//在相应输入框中写   plg_defval = '要输入的内容'
// 备注:动态添加的时候需保证在节点已经添加到也页面的时候调用 ,如有不需要行高的情况设置.plg_lb_val .plg_lb_cls{line-height:0;}
(function($) {
    $.fn.defFieldLabels = function(options) {
        var base = this;
        var opts = $.extend({},$.fn.defFieldLabels.defaults, options);
		var bcss = ".plg_lb_val{\
						position:relative;\
						display:inline-block\
					}\
					.plg_lb_cls{\
						position:absolute;\
						left:0px;top:0px;\
						color:#999999;\
						font-weight:normal;\
						cursor:text !important;\
						font-size:12px;\
					}";
        function bindInputChange(obj_v, obj_l) {
            '' == $.trim($(obj_v).val()) ? $(obj_l).show() : $(obj_l).hide();
            return false
        }
        if (this.selector && 0==$('#def_plu_tmp').length) $("head").append("<style type='text/css' id='def_plu_tmp'>"+bcss+"</style>");
        return this.each(function(i, obj) {
			var _self_     = this;
			//判断是否有绑定过
			if($(_self_).attr('plg_defval') == $(_self_).parent().children('label.plg_lb_cls').text()){
				return true;
			}
			$(_self_).attr('autocomplete','off');
            var plg_defval = $(this).attr('plg_defval');
            var plg_defcls = eval('(' + ($(this).attr('plg_defcls') || "{}") + ')');
            if (!plg_defval) return;
            var obj_p = $("<span class='plg_lb_val'></span>");
            $(obj_p).appendTo($(this).parent()).insertBefore($(this));
            $(this).appendTo($(obj_p));
            var obj_l = $("<label class='plg_lb_cls' for='" + ($(this).attr('id') || '') + "'>" + plg_defval + "</label>");
            $(obj_l).appendTo($(obj_p));
            $(obj_l).css($.extend({},
            {
                "left": "7px",
                "line-height":$(this).prev().height() + 'px'
            },
            opts.css||{},
            plg_defcls));
            $(this).bind('propertychange',
            function(e) {
                bindInputChange($(this), $(obj_l));
            }).bind('input',
            function(e) {
                bindInputChange($(this), $(obj_l));
            }).bind('keyup',
            function(e) {
                8 == e.keyCode && $.browser.msie && ($.browser.version == "9.0") && (bindInputChange($(this), $(obj_l)))
            });
            if(!$.browser.msie){
            	$(this).bind('focus',function(){
                	bindInputChange($(this), $(obj_l));
                })
            }
			$(obj_l).click(function(e){
				$(_self_).focus();
			})
			'' != $.trim($(_self_).val()) && $(obj_l).hide();
        })
    };
    $.fn.defFieldLabels.defaults = {}
})(jQuery);
