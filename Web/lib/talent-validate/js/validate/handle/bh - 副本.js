/**
 * 信息提示框的关闭按钮动作处理类
 * 
 * @param {}
 *            obj
 * @param {}
 *            closeObj
 */
tt.closeHandler = function(obj, closeObj, e, eCls, closeCls) {
	var _closeCls = closeCls + "_hover";
	this.click = function() {
		tt.rmEle(obj);
		tt.rmCls(e, eCls + '_1');
	};
	this.mouseover = function() {
		tt.addCls(closeObj, _closeCls);
	};
	this.mouseout = function() {
		tt.rmCls(closeObj, _closeCls);
	};
};
tt.msgHandler = function(e, cls, msgCls, target) {
	var _cls = cls + "_1";
	var _msgcls = msgCls + "_hover";
	this.mouseover = function() {
		tt.addCls(e, _cls);
		tt.addCls(target, _msgcls);
	};
	this.mouseout = function() {
		tt.rmCls(e, _cls);
		tt.rmCls(target, _msgcls);
	};
	this.click = function() {
		try {
			e.focus(true);
		} catch (e1) {}
	};
};

/**
 * baseHandler
 */
tt.bh = tt.C.ext({
	setV:function(v)
	{
		this.v = v;
		return this;
	},
	setE:function(e)
	{
		this.e = e;
		return this;
	},
	setF:function(f)
	{
		this.f = f;
		return this;
	},
	setVal:function(val)
	{
		this.val = val;
		return this;
	},
	setIndex:function(index)
	{
		this.index = index;
		return this;
	},
	needHandle:function()
	{
		return !(this.e.style.display == 'none' || this.e.disabled)//对于不可见的元素,不处理
	},
	render:function(cls, msg, closeCls, inputCls) {
			var e = this.e;
			
			var _inputE = this.f.getInputId();
			_inputE = _inputE ? _inputE : tt.id(this.v.getInputId());
			var ee = _inputE ? _inputE :  e;
			
			var types = tt.inputType(e);
			var divWrap = document.createElement("div");
			var div = document.createElement("div");
			divWrap.appendChild(div);
			
			var msgId = this.f.getMsgId(e);
			msgId = msgId ? msgId : this.v.getMsgId();
			if (msgId) {
				tt.id(msgId).appendChild(divWrap);
			} else {
				if (types.isCheckbox || types.isRadio) {
					tt.moveToPos(divWrap, _inputE ? _inputE : this.f.es[this.f.es.length - 1]);
				} else {
					tt.moveToPos(divWrap, ee);
				}
				ee.parentNode.insertBefore(divWrap, ee);
				
				tt.vf.msgs.push({"msg":divWrap,"ele":ee});
			}
			
			divWrap.id = e[tt.Conf.pro4MsgId];
			divWrap.className = cls;
			div.innerHTML = msg;
			divWrap.title = msg;
			div.style.display = "inline";
			
			var close = document.createElement("div");
			divWrap.appendChild(close);
			
			if (msgId) {
				try {
					if (tt.vf.vFilter != "e" && tt.Conf.animate) {
						var $divWrap = $(divWrap);
						$divWrap.fadeOut("fast");
						$divWrap.fadeIn("slow");
					} else{}
				}catch(e1) {
				}
			}
			
			close.className = closeCls;
			close.innerHTML = "X";
			close.title = tt.close;
			
			tt.addCls(ee, inputCls);
			
			var _closeHandler = new tt.closeHandler(divWrap, close, ee, inputCls, closeCls);
			tt.addEH(close, "click", _closeHandler["click"]);
			
			
			var _msgHandler =  new tt.msgHandler(ee, inputCls, cls, divWrap);
			tt.addEH(div, "click", new tt.msgHandler(ee, inputCls, cls, div)["click"]);
			
			if (tt.Conf.highlight) {
//				tt.addEH(close, "mouseover", _closeHandler["mouseover"]);
//				tt.addEH(close, "mouseout", _closeHandler["mouseout"]);
				
				tt.addEH(divWrap, "mouseover", _msgHandler["mouseover"]);
				tt.addEH(divWrap, "mouseout", _msgHandler["mouseout"]);
			}
			
			
	}
});