tt.vHandler = function(e, f) {
	var c = "t_v_ErrMsg_hover";
	this.h = function() {
		var es = f.errorStyle ? f.errorStyle : tt.Conf.errorStyle;
		if (es == 'alert') {
			return;
		}
		tt.validateElement(e);
		if (tt.Conf.highlight) {
			tt.addCls(tt.id(e[tt.Conf.pro4MsgId]), c);
		}
	}, this.rmCls = function() {
		tt.rmCls(tt.id(e[tt.Conf.pro4MsgId]), c);
	}, this.addCls = function() {
		tt.addCls(tt.id(e[tt.Conf.pro4MsgId]), c);
	}
};

/**
 * @author Tanyaowu
 * @version 2.0.0
 * @date 2011-8-13 BaseValidator
 */
tt.BV = tt.C.ext({
	init : function() {
		this.fs = [];
		this.i18ps = [];
		this.isInFactory = false;// 本验证器是否已经在验证器工厂中了.false:不在工厂中;true:已在工厂中.
		this.clrSpace = tt.Conf.clrSpace;
		this.vType = "l";
		this.setI18n = this.setI18;
	},
	/**
	 * 提示信息显示的地方
	 * 
	 * @param {}
	 *            msgId
	 */
	setMsgId : function(msgId) {
		this.msgId = msgId;
		return this;
	},
	getMsgId : function() {
		return this.msgId;
	},
	/**
	 * 用于代替input的容器id
	 * 
	 * @param {}
	 *            inputId
	 */
	setInputId : function(inputId) {
		this.inputId = inputId;
		return this;
	},
	getInputId : function() {
		return this.inputId;
	},
	getVType : function() {
		return this.vType;
	},
	setVType : function(vType) {
		this.vType = vType;
		return this;
	},
	initI18n : function(m) {
		!this.i18n ? this.i18n = m : null;
		return this;
	},
	setI18 : function(m) {
		this.i18n = m;
		return this;
	},
	setClrSpace : function(c) {
		this.clrSpace = c;
	},

	/**
	 * 
	 * @param {}
	 *            s 需要被验证的串，根据配置此串有可能清除了两边的空格
	 * @param {}
	 *            i index 当前元素序号，从0开始
	 * @param {}
	 *            es elements
	 * @param {}
	 *            f Field
	 * @return {Boolean} true:验证通过
	 */
	v : function(s, i, es, f) {
		return true;
	},
	/**
	 * 
	 * @param {}
	 *            f Field
	 */
	doAfterAdd : function(f) {
		for (var i = 0; i < f.es.length; i++) {
			seq = tt.vf.seq++;
			!f.es[i][tt.Conf.pro4MsgId]
					? f.es[i][tt.Conf.pro4MsgId] = tt.Conf.pro4MsgId + seq
					: null;
		}
	},
	/**
	 * 当移除后做些事情,子类视情况实现该函数,如Required验证器,需要去掉后面的红星号
	 */
	doBeforeRm : function(f) {
		this.clrFErr(f);
	},
	_onValid : function(sv, j, es, f) {
		this.handTip(es[j], f, sv, j);
	},
	_onInvalid : function(sv, j, es, f) {
		tt.vf.invalidEs.push(es[j]);
		this.handErr(es[j], f, sv, j);
	},
	_beforeV : function(sv, j, es, f) {
	},
	_afterV : function(sv, j, es, f) {
	},

	onValid : function(sv, j, es, f) {
	},
	onInvalid : function(sv, j, es, f) {
	},
	beforeV : function(sv, j, es, f) {
	},
	afterV : function(sv, j, es, f) {
	},
	/**
	 * 
	 * @param {}
	 *            fl filter
	 * @return {Boolean}
	 */
	vBf : function(fl) {
		var ret = true;
		if (this.vType == 'r') {
			if (['a', 'f'].ttCons(tt.vf.vFilter)) {
				this.v();
			}
			return;
		}

		for (var i = 0; i < this.fs.length; i++) {
			var es = this.fs[i].es;
			if (es) {
				for (j = 0; j < es.length; j++) {
					if (tt.vf.invalidEs.ttCons(es[j])) { // 没有通过前面的验证器
						continue;
					}

					/** 不需要验证或者验证通过则继续下一个元素的处理 */
					if (fl && !fl.f(es[j])) { // 被过滤了，不需要验证
						if (tt.Conf.clearOtherError) {
							this.clrEleErr(es[j], this.fs[i]);
						}
						continue;
					}

					var types = tt.inputType(es[j]);

					var sv;
					var tagname = es[j].tagName.toLowerCase();
					if (tagname != 'input' && tagname != 'select' && tagname != 'textarea'){
						sv = es[j].innerHTML;
						if (sv == '请选择' || sv == '请填写' || sv == '请选择市(先选择省)'){
							sv = '';//先这么写，有时间再完善
						}
					} else {
						sv = es[j].value;
					}
					sv = sv || "";

					if (this.clrSpace
							&& (!types['isSelect'] && !types['isCheckbox'] && !types['isRadio'])) {
						sv = tt.trim(sv);
						if (!["e"].ttCons(tt.vf.vFilter)) {
							try{
								es[j].value = sv;
							} catch(e) {
								
							}
						}
					}

					if (types['isRadio'] || types['isCheckbox']) {
						this.clrFErr(this.fs[i]);
					} else {
						this.clrEleErr(es[j], this.fs[i]);
					}

					this._beforeV(sv, j, es, this.fs[i]);
					this.beforeV(sv, j, es, this.fs[i]);
					if (this.v(sv, j, es, this.fs[i])) { // 验证通过
						this._onValid(sv, j, es, this.fs[i]);
						this.onValid(sv, j, es, this.fs[i]);
						continue;
					} else {
						this._onInvalid(sv, j, es, this.fs[i]);
						this.onInvalid(sv, j, es, this.fs[i]);
						ret = false;
					}
					this._afterV(sv, j, es, this.fs[i]);
					this.afterV(sv, j, es, this.fs[i]);
				}
			}
		}
		return ret;
	},

	/**
	 * 移除字段 用法:xx.rm("name1", "name2", "name3"...);
	 */
	rm : function() {
		return this._rm("name", arguments);
	},
	/**
	 * 移除字段 用法:xx.rmId("id1", "id2", "id3"...);
	 */
	rmId : function() {
		return this._rm("id", arguments);
	},
	_rm : function(type, args) {
		for (var i = 0; i < args.length; i++) {
			for (var j = 0; j < this.fs.length; j++) {
				var f = false;
				if (typeof args[i] != "string") {
					f = (this.fs[j] && this.fs[j] == args[i]);
				} else {
					f = (this.fs[j] && this.fs[j][type] == args[i]);
				}

				if (f) {
					this.doBeforeRm(this.fs[j]);
					this.fs[j].doAfterRm(this);
					this.fs[j] = null;
				}
			}
		}
		this.fs = tt.rmNull(this.fs);
		return this;
	},

	/**
	 * 移除所有字段 用法:xxValidator.rmAll();
	 */
	rmAll : function() {
		for (var i = 0; i < this.fs.length; i++) {
			this.rm(this.fs[i]);
		}
		this.fs = [];
	},
	/**
	 * 将要验证的字段加到验证器中 用法:xx.add("name1", "name2", "name3"...);
	 */
	add : function() {
		return this._addF('name', arguments);
	},
	/**
	 * 将要验证的字段加到验证器中 用法:xx.addId("id1", "id2", "id3"...);
	 */
	addId : function() {
		return this._addF('id', arguments);
	},

	/**
	 * 将要验证的字段加到验证器中 用法:xx.addObj(obj1, obj2, obj3...);
	 */
	addObj : function() {
		return this._addF('obj', arguments);
	},

	_addF : function(type, arg) {
		for (var i = 0; i < arg.length; i++) {
			var f = null;
			if (type == 'id') {
				var obj = tt.id(arg[i]);
				// alert(obj.id +" "+ arg[i]);
				if (obj && (obj.id == arg[i]) && this._c('name', obj.name)
						&& this._c('id', arg[i])) {
					f = new tt.Field("", null, arg[i]);
				}
			} else if (type == 'obj') {
				f = new tt.Field("", null, false, arg[i]);
			} else {
				isStr = (typeof arg[i] == 'string');
				var fg = false;
				if (isStr) {
					fg = this._c('name', arg[i]);
				} else {
					fg = (this._c('name', arg[i].name) && this._c('id',
							arg[i].id));
				}

				if (fg) {
					if (isStr) {
						f = new tt.Field("", arg[i]);
					} else {
						f = arg[i];
					}
				}
			}

			if (f != null) {
				f.doAfterAdd(f.label, f.name, f.id, f.e, this);

				this.fs[this.fs.length] = f;
				for (var j = 0; j < f.es.length; j++) {
					if (f.es[j].tt_addedEvent) {
						continue;
					}
					f.es[j].tt_addedEvent = true;
					this.attachE(f.es[j], f);
				}

				if (!this.isInFactory)// 必要时添加验证器到工厂中
				{
					tt.vf.add(this);
					this.isInFactory = true;
				}
				this.doAfterAdd(f);
			}
		}
		return this;
	},
	/**
	 * 
	 * @param {}
	 *            proName: id, name and so on
	 * @param {}
	 *            value
	 * @return {Boolean}
	 */
	_c : function(proName, value) {
		if (!value) {
			return true;
		}

		for (var i = 0; i < this.fs.length; i++) {
			if (this.fs[i][proName] == value) {
				return false;
			}
		}
		return true;
	},
	/**
	 * 处理没有验证通过的对象,例如对这个对象进行选中,将焦点转到该对象,修改该对象的样式等
	 */
	handErr : function(e, f, val, j) {
		var es = f.errorStyle ? f.errorStyle : tt.Conf.errorStyle;
		var h = tt.instanceByClass("tt." + es);
		h.setV(this).setE(e).setF(f).setVal(val).setIndex(j).h();
	},
	handTip : function(e, f, val, j) {
		if(tt.Conf.tipStyle){
			var h = tt.instanceByClass("tt." + tt.Conf.tipStyle);
			h.setV(this).setE(e).setF(f).setVal(val).setIndex(j).h();
		}
	},
	/**
	 * 验证不通过时，获取提示给用户的信息
	 * 
	 * @param label
	 */
	getI18 : function(label, e, f, index, val) {
		if (this.i18n) {
			ret = tt.getI18S(this.i18n, [label], 0);
			return tt.getI18S(ret, this.i18ps, 1);
		}
		return null;
	},
	setTip : function(tip) {
		this.tip = tip;
		return this;
	},

	/**
	 * 
	 * @param {}
	 *            e element
	 * @param {}
	 *            f field
	 * @param {}
	 *            v validator
	 * @param {}
	 *            val
	 * @param {}
	 *            index
	 * @return {}
	 */
	getTip : function(e, f, v, val, index) {
		return this.tip;
	},
	/**
	 * 清空field的错误
	 */
	clrFErr : function(f) {
		var es = f.es;
		for (i = 0; i < es.length; i++) {
			this.clrEleErr(es[i], f);
		}
	},
	clrAllErr : function() {
		if (this.fs) {
			for (var i = 0; i < this.fs.length; i++) {
				this.clrFErr(this.fs[i]);
			}
		}
	},
	clrEleErr : function(e, f) {
		if (e) {
			tt.rmCls(e, tt.Conf.errInputCls);
			tt.rmCls(e, "t_v_SucInput");
			tt.rmCls(e, tt.Conf.errInputCls + "_1");
			tt.rmCls(e, "t_v_SucInput_1");

			var inputForMsg = f.getInputId();
			if (inputForMsg) {
				tt.rmCls(inputForMsg, tt.Conf.errInputCls);
				tt.rmCls(inputForMsg, "t_v_SucInput");
			}
			tt.rmEle(tt.id(e[tt.Conf.pro4MsgId]));
		}

	},
	/**
	 * 获取本验证器所验证的所有element
	 */
	getEs : function() {
		es = [];
		for (i = 0; i < this.fs.length; i++) {
			for (var j = 0; j < this.fs[i].es.length; j++) {
				es = es.concat(this.fs[i].es[j]);
			}
		}
		return es;
	},
	getVWhen : function() {
		return this.vWhen;
	},
	setVWhen : function(vWhen) {
		this.vWhen = vWhen;
	},
	/**
	 * 对html element作些额外的处理，如加验证事件
	 * 
	 * @param {}
	 *            e html element
	 */
	attachE : function(e, f) {
		var es = f.errorStyle ? f.errorStyle : tt.Conf.errorStyle;
		if (es == 'alert') {
			return;
		}

		var types = tt.inputType(e);

		var handler = new tt.vHandler(e, f);
		var hd = handler.h;
		var validateOn = f.validateOn ? f.validateOn :  tt.Conf.validateOn;
		for (var x = 0; x < validateOn.length; x++) {
			var evt = validateOn[x];
//			if (types.isCheckbox || types.isRadio) {
//				if (evt == 'change') {
//					tt.addEH(e, evt, hd);
//					break;
//				}
//				continue;
//			}
//
//			if (types.isSelect && e.multiple != true) {
//				if (evt == 'change' || evt == 'blur') {
//					tt.addEH(e, 'blur', hd);
//					break;
//				}
//				continue;
//			}
			tt.addEH(e, evt, hd);
		}

		var ee = f.getInputId();
		if (!ee) {
			ee = e;
		}

		if (tt.Conf.highlight) {
			tt.addEH(ee, "mouseout", handler.rmCls);
			tt.addEH(ee, "mouseover", handler.addCls);
			tt.addEH(ee, "focus", handler.addCls);
			tt.addEH(ee, "blur", handler.rmCls);
		}

	}
});