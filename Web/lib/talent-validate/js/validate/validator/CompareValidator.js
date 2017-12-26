tt.CV = tt.BV.ext({
	/**
	 * @param cmpType
	 *            比较类型 'n':数字比较; 'v':字符串比较；
	 * @param oper
	 *            比较符,可以为'<','<=','==','!=','>','>='
	 * @param fOrV
	 *            被比较的字段或值或表达式
	 * @param showCmpVal
	 *            是否显示被比较的值 举例: var field1 = new tt.Field("用户名", "loginName");
	 *            new tt.CV().set('n','>',field1);//要求添加此验证器的字段必须大于field1的值
	 */
	set : function(cmpType, oper, fOrV, showCmpVal) {
		this.cmpType = cmpType;
		this.oper = oper;
		this.cmpF = null; // comparedField
		this.cmpV = null; // comparedValue
		this.i18ps[0] = tt.operMap[this.oper];
		this.showCmpVal = true; // 默认为true
		if (arguments.length == 4) {
			this.showCmpVal = showCmpVal;
		}
		if (["string", 'number'].ttCons(typeof fOrV)) {
			this.cmpV = fOrV;
		} else {
			this.cmpF = fOrV;
			this.cmpF.initEs(fOrV.label, fOrV.name, fOrV.id, fOrV.e);
			this.i18ps[1] = fOrV.label;
		}

//		if (!this.showCmpVal) {
//			this.i18n = tt.i18Compare;
//		} else 
		
		if (cmpType == 'n' && this.cmpF) {
			this.i18n = tt.i18NumCompare;
		} else if (cmpType == 'v' && this.cmpF) {
			this.i18n = tt.i18StrCompare;
		} else if (cmpType == 'n' && this.cmpV) {
			this.i18n = tt.i18NumValueCompare;
		} else if (cmpType == 'v' && this.cmpV) {
			this.i18n = tt.i18StrValueCompare;
		} else {
			alert("error cmp type:" + cmpType);
		}
		return this;
	},
	v : function(str, index, es, f) {
		if (!str) {
			return true;
		}

		var _cmpV;
		if (this.cmpV) {
			try {
				if (this.cmpType == 'n') {
					_cmpV = eval(tt.getExp(this.cmpV, index));
				} else if (this.cmpType == 'v') {
					_cmpV = tt.getExp(this.cmpV, index);
				}
			} catch (e) {
				return false;
			}

			if (this.showCmpVal) {
				this.i18ps[1] = "<span class='t_v_ComparedValue'>" + _cmpV
						+ "</span>";
			} 
			else {
				this.i18ps[1] = "";
			}
		} else {
			var es = this.cmpF.es;
			if (es.length == 0) {
				return true;
			}

			_cmpV = (es[index]) ? es[index].value : es[es.length - 1].value;
			if (_cmpV == null || _cmpV == "") {
				return true;
			}

			if (this.showCmpVal) {
				this.i18ps[2] = "<span class='t_v_ComparedValue'>" + _cmpV
						+ "</span>";
			} 
			else {
				this.i18ps[2] = "";
			}
		}

		var s;
		if (this.cmpType == "n")// 是数字比较
		{
			var numV = tt.vf.num;
			if ((!numV.v(str)) || (!numV.v(_cmpV + "")))// 不是数字
			{
				return true; // 不是数字则留给数字验证器去验证
			}

			s = str + this.oper + _cmpV;
		} else if (this.cmpType == "v")// 是字符串比较
		{
			s = "\"" + str + "\"" + this.oper + "\"" + _cmpV + "\"";
		}
		return eval(s) == true;
	}
});