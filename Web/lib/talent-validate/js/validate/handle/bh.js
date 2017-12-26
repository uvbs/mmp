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
		tt.rmCls(target, tt.Conf.aniCls);
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
			var swrap = null;
			
			if (tt.Conf.msgId) {
				swrap = document.createElement("li");
				
			} else {
				swrap = document.createElement("span");
			}
			
			
			var s = document.createElement("span");  //存放提示信息
			swrap.appendChild(s);
			s.style.width = "auto";
			
			var msgId = this.f.getMsgId(e);
			msgId = msgId ? msgId : this.v.getMsgId();
			msgId = msgId ? msgId : tt.Conf.msgId;
			if (msgId) {
				tt.id(msgId).appendChild(swrap);
			} else {
				if (types.isCheckbox || types.isRadio) {
					tt.moveToPos(swrap, _inputE ? _inputE : this.f.es[this.f.es.length - 1]);
				} else {
					tt.moveToPos(swrap, ee);
				}
				ee.parentNode.insertBefore(swrap, ee);
				
				tt.vf.msgs.push({"msg":swrap,"ele":ee});
			}
			
			if (!e[tt.Conf.pro4MsgId]) {
				var seq = tt.vf.seq++;
				e[tt.Conf.pro4MsgId] = tt.Conf.pro4MsgId + seq;
			}
			
			swrap.id = e[tt.Conf.pro4MsgId];
			swrap.className = cls;
			s.innerHTML = msg;
			swrap.title = msg;
			s.style.display = "inline";
			
			var close = document.createElement("span");
			swrap.appendChild(close);
			
			if (msgId) {
				try {
					if (tt.vf.vFilter != "e" && tt.Conf.animate) {
						var $swrap = $(swrap);
						//$swrap.fadeOut("fast");
						$swrap.fadeIn("slow");
					} else{}
				}catch(e1) {
				}
			}
			
			close.className = closeCls;
			close.innerHTML = "x";
			close.title = tt.close;
			
			tt.addCls(ee, inputCls);
			
			var _closeHandler = new tt.closeHandler(swrap, close, ee, inputCls, closeCls);
			tt.addEH(close, "click", _closeHandler["click"]);
			
			
			var _msgHandler =  new tt.msgHandler(ee, inputCls, cls, swrap);
			tt.addEH(s, "click", new tt.msgHandler(ee, inputCls, cls, s)["click"]);
			
			if (tt.Conf.highlight) {
//				tt.addEH(close, "mouseover", _closeHandler["mouseover"]);
//				tt.addEH(close, "mouseout", _closeHandler["mouseout"]);
				
				tt.addEH(swrap, "mouseover", _msgHandler["mouseover"]);
				tt.addEH(swrap, "mouseout", _msgHandler["mouseout"]);
			}
			
			
	}
});