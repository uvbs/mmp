tt.VF = tt.C.ext({
	init : function() {
		this.localVArr = [];
		this.remoteVArr = [];
		this.extVArr = [];
		
		var c = "(\\d{1,2}|0{2}\\d{1}|1\\d\\d|2[0-4]\\d|25[0-5])";
		var cs = c + "\\.";
		var ipexp = "^" + cs + cs + cs + c + "$";
		this.ip = new tt.RV().set(new RegExp(ipexp)).setI18(tt.i18Ip);
		this.ip.onValid = function (sv, j, es, f) {
			var types = tt.inputType(es[j]);
			if ((types.isSelect || types.isCheckbox || types.isRadio) || !sv) {
				return;
			}
			
			if (tt.vf.vFilter != "e") {
				var arr = sv.split(".");
				var newArr = [];
				for (var i = 0; i < arr.length; i++) {
					var trimV = arr[i].replace(/(^0*)/g, "");
					newArr.push(trimV == "" ? "0" : trimV);
				}
				es[j].value = newArr.join(".");
			}
		};
		
		this.email = new tt.RV().set(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).setI18(tt.i18Email);
		this.postcode = new tt.RV().set(/^[1-9]\d{5}(?!\d)$/).setI18(tt.i18Postcode);
		this.tel = new tt.RV().set(/^\d{3}-\d{8}$|^\d{4}-\d{7,8}$/).setI18(tt.i18Tel);
		this.idcard = new tt.RV().set(/^\d{15}$|^\d{17}[\d,x,X]{1}$/).setI18(tt.i18Idcard);
		this.req = new tt.ReqV();
		this['int'] = new tt.IV();  
		this.num = new tt.NV();
		
		this.msgs = [];
		this.seq = 0;            //序列号
	},
	rm: function(validatorArr){
		if(validatorArr) {
			var varr;
			for (var j = 0; j < validatorArr.length; j++){
				if (validatorArr[j].getVType() == "l") {
					varr = this.localVArr;
				} else if (validatorArr[j].getVType() == "r"){
					varr = this.remoteVArr;
				} else {
					varr = this.extVArr;
				}
				for (var i = 0; i < varr.length; i++){
					if(validatorArr[j] == varr[i]){
						varr[i].rmAll();
						varr[i] = null;
					}
				}
			}
		}
		this.localVArr = tt.rmNull(this.localVArr);
		this.remoteVArr = tt.rmNull(this.remoteVArr);
		this.extVArr = tt.rmNull(this.extVArr);
	},
	/**
	 * 移除所有验证器
	 */
	rmAll : function() {
		this.rm(this.remoteVArr);
		this.rm(this.localVArr);
		this.rm(this.extVArr);
	},
	rmAllRemote : function() {
		this.rm(this.remoteVArr);
		this.rm(this.extVArr);
	},
	clrAllMsg: function(){
		this.clrMsg(this.remoteVArr);
		this.clrMsg(this.localVArr);
		this.clrMsg(this.extVArr);
	},
	clrMsg: function(validatorArr){
		if(validatorArr) {
			var varr;
			for (var j = 0; j < validatorArr.length; j++){
				if (validatorArr[j].getVType() == "l") {
					varr = this.localVArr;
				} else if (validatorArr[j].getVType() == "r"){
					varr = this.remoteVArr;
				} else {
					varr = this.extVArr;
				}
				for (var i = 0; i < varr.length; i++){
					if(validatorArr[j] == varr[i]){
						varr[i].clrAllErr();
					}
				}
			}
		}
	},
	/**
	 * 用法:tt.vf.add(validator1, validator2, validator3... ...
	 * validatorx);
	 */
	add : function() {
		for ( var i = 0; i < arguments.length; i++) {
			if(arguments[i].getVType() == "l") {
				this.localVArr.push(arguments[i]); 
			} else if(arguments[i].getVType() == "r"){
				this.remoteVArr.push(arguments[i]); 
			} else {
				this.extVArr.push(arguments[i]); 
			}
		}
	},
	vBf : function(f) {
		this.invalidEs = [];
		this.msgs = [];
		
		this.rm(this.extVArr);
		
		var ret = this._vBf(f, this.localVArr);

		if (ret) {
			ret = this._vBf(f, this.remoteVArr);
			ret = this._vBf(f, this.extVArr);
		}
		return ret;
	},
	_vBf: function(f, vArr) {
		var ret = true;
		for (var i = 0; i < vArr.length; i++) {
			var vwhen = vArr[i].getVWhen();
			if (vwhen && !vwhen.ttCons(tt.vf.vFilter)){
				continue;
			}
			if (!vArr[i].vBf(f)) {
				ret = false;
			}
		}
		return ret;
	},
	resizeWindow : function() {
		for (var i = 0; i < tt.vf.msgs.length; i++) {
			tt.moveToPos(tt.vf.msgs[i].msg, tt.vf.msgs[i].ele);
		}
	}
});


tt.vf = new tt.VF();
tt.addEH(window, "resize", tt.vf.resizeWindow);