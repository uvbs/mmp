tt.Field = tt.C.ext({
	init : function(label, name, id, e) {
		this.label = label ? label : "";
		this.name = name ? name : null;
		this.id = id ? id : null;
		this.e = e ? e : null;
		this.vArr = [];
		this.validateOn = null;
	},
	initEs: function(label, name, id, e){
		es = [];
		
		if(e){
			es.push(e);
		} 
		if (name) {
			es = document.getElementsByName(name);
		}
		if (id) {
			es = [];
			this.id = id;
			e = tt.id(id);
			if (e) {
				es.push(e);
			}
		}
		this.es = es;
	},
	
	/**
	 * 设置什么事件能触发验证
	 * @param {} validateOn 形如：['keyup', 'focus', 'change']
	 */
	setValidateOn : function(validateOn) {
		this.validateOn = validateOn ? validateOn : [];
		return this;
	},
	/**
	 * 
	 * @param {} errorStyle 'text'或'alert'
	 * @return {}
	 */
	setErrorStyle : function(errorStyle) {
		this.errorStyle = errorStyle;
		return this;
	},
	
	doAfterAdd : function(label, name, id, e, validator){
		this.vArr.push(validator);
		this.initEs(label, name, id, e);
		return this;
	},
	doAfterRm: function(validator) {
		this.vArr.ttRm(validator);
	},
	setMsgId:function(id) {
		this.msgId = id;
		return this;
	},
	getMsgId:function(e) {
		return this.msgId;
	},
	setInputId: function(id){
		this.inputForErr = tt.id(id);
		return this;
	},
	getInputId:function(e) {
		return this.inputForErr;
	},
	
	add: function()
	{
		tt.each (arguments, function(item) {
			item.add(this);
		}, this);
		return this;
	},
	
	rm: function()
	{
		tt.each (arguments, function(item) {
			item.rm(this);
		}, this);
		return this;
	},
	
	rmAll: function() {
		for (var i = this.vArr.length - 1; i >=0; i--) {
			this.vArr[i].rm(this);
		}
		return this;
	}
});