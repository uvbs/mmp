/**
 * 
 * @param {}
 *            target
 * @param {}
 *            type such as "click"
 * @param {}
 *            handler
 */
tt.addEH = function(o, type, handler) {
	if (arguments.length == 4) {
		$(o).unbind(type, handler);
	} else {
        if (o.addEventListener) {
            o.addEventListener(type, handler, false);
        } else if (o.attachEvent) {
            o.attachEvent("on" + type, handler);
        } else {
            o["on" + type] = handler;
        }
	}
};

/**
 * 将htmlElement插入到srcElement元素后面
 * 
 * @param srcElement
 * @param htmlElement
 */
tt.insertAfter = function(src, e) {
	$(e).insertAfter(src);
};

/**
 * 为element添加className样式
 * 
 * @param element
 *            被操作的元素
 * @param cls
 *            样式名
 * @return
 */
tt.addCls = function(element, cls) {
	$(element).addClass(cls);
};
/**
 * 为element删除className样式
 * 
 * @param element
 *            被操作的元素
 * @param cls
 *            样式名
 * @return
 */
tt.rmCls = function(element, cls) {
	$(element).removeClass(cls);
};

/**
 * 删除某一个元素
 * 
 * @param element
 * @return
 */
tt.rmEle = function(e) {
	if (e && e.parentNode) {
		e.parentNode.removeChild(e);
	}
};

/**
 * 相当于string的trim
 * 
 * @param str
 * @return
 */
tt.trim = function(s) {
	return $.trim(s);
};

/**
 * 根据类名实例化js对象
 * 
 * @param {}
 *            clazz
 * @return {}
 */
tt.instanceByClass = function(c) {
	eval("var r = new " + c + "();");
	return r;
};

/**
 * 
 * @param {}
 *            v comparedValue
 * @param {}
 *            exp expression
 * @return {}
 */
tt.parRngExp = function(v, exp) {
	var map = {
		'(' : '>',
		'[' : '>='
	};
	var expArr = [];
	var m1 = {
		"{" : "(",
		"}" : ")",
		"|" : "||",
		"&" : "&&"
	};
	for (i = 0; i < exp.length; i++) {
		c = exp.charAt(i);

		if (c == '(' || c == '[') {
			compareOper1 = map[c];

			index1 = exp.indexOf(')');
			index2 = exp.indexOf(']');
			_index = index1;
			compareOper2 = '<';
			if (index1 == -1 && index2 == -1) {
				alert('expression is invalid, not found ] or )!');
				return null;
			} else if (index1 == -1 || (index1 > index2 && index2 != -1)) {
				_index = index2;
				compareOper2 = '<=';
			}
			var singleExp = exp.substring(i + 1, _index);

			var numArr = singleExp.split(',');
			numArr[0] = tt.trim(numArr[0]);

			if (numArr.length == 1) {
				numArr[1] = tt.trim(numArr[0]);
			} else if (numArr.length == 2) {
				numArr[1] = tt.trim(numArr[1]);
			} else {
				alert(singleExp + ' is error!');
				return null;
			}

			expArr.push("(");
			if (numArr[0] != '') {
				expArr.push(v);
				expArr.push(compareOper1);
				expArr.push(numArr[0]);
			}
			if (numArr[0] != '' && numArr[1] != '') {
				expArr.push(' && ');
			}
			if (numArr[1] != '') {
				expArr.push(v);
				expArr.push(compareOper2);
				expArr.push(numArr[1]);
			}

			expArr.push(")");

			exp = exp.substring(_index + 1, exp.length);
			i = 0;
			continue;
		} else if (m1[c]) {
			expArr.push(m1[c]);
		}
	}
	return expArr.join('');
};
/**
 * tt.getI18S("my name is {0}, your name is {1}",["kebo","smis"], 0);
 * tt.getI18S("my name is {1}, your name is {2}",["kebo","smis"], 1);
 */
tt.getI18S = function() {
	var ret = arguments[0];
	if (arguments.length > 1) {
		si = 0; // startIndex
		if (arguments.length == 3) {
			si = arguments[2];
		}
		tt.each(arguments[1], function(item) {
					ret = ret.replace("{" + si + "}", item);
					si++;
				}, this);
	}
	return ret;
};
Array.prototype.ttCons = function(e) {
	i = 0;
	for (; i < this.length && this[i] != e; i++);
	return !(i == this.length);
};

Array.prototype.ttIndexOf = function(obj) {
	for (var i = 0; i < this.length; i++) {
		if (this[i] == obj) {
			return i;
		}
	}
	return -1;
};
/**
 * 
 * @param {}
 *            e
 * @return {} true:包含
 */

/**
 * 删除指定序号的元素
 * 
 * @param {}
 *            index
 */
Array.prototype.ttRmAt = function(index) {
	this.splice(index, 1);
	return this;
};
/**
 * 从数组中删除指定元素
 * 
 * @param {}
 *            obj
 */
Array.prototype.ttRm = function(obj) {
	var index = this.ttIndexOf(obj);
	if (index >= 0) {
		this.ttRmAt(index);
	}
	tt.rmNull(this);
	return this;
};

tt.getStrLen = function(s) {
	var len = 0;
	var c = -1;
	for (var i = 0; i < s.length; i++) {
		c = s.charCodeAt(i);
		if (c >= 0 && c <= 128)
			len += 1;
		else
			len += 2;
	}
	return len;
};
tt.id = function(id) {
	if (id) {
		return document.getElementById(id);
	}
	return null;

};

/**
 * 
 * @param {}
 *            arr 要迭代的数组
 * @param {}
 *            call
 * @param {}
 *            thisObj
 * @param {}
 *            eachType 1:数组方式；2:迭代方式 默认为1
 */
tt.each = function(arr, callback, thisObj, eachType) {
	if (arr) {
		if (!eachType || eachType != 1) {
			for (var i = 0; i < arr.length; i++) { // 数组方式
				callback.call(thisObj, arr[i]);
			}
		} else {
			for (var name in arr) {
				callback.call(thisObj, arr[name]);
			}
		}
	}
};

/**
 * 获取元素的位置信息
 * 
 * @param {}
 *            e
 * @return {} {"t":t,'l':l,"b":b,'r':r};
 */
tt.getPos = function(e) {
	var p = $(e).position();
	var rect = e.getBoundingClientRect();
	var width = rect.right - rect.left;
	var height = rect.bottom - rect.top;
	
	return {
		"t" : p.top,
		'l' : p.left,
		"b" : p.top + height,
		'r' : p.left + width
	};
};

tt.getRuntimeStyle = function(obj, attr) {
	if (window.getComputedStyle) {
		return getComputedStyle(obj, false)[attr];
	} else {
		return obj.currentStyle[attr];
	}
}

/**
 * 将srcE移到targetE后面
 * 
 * @param {}
 *            srcE
 * @param {}
 *            targetE
 */
tt.moveToPos = function(srcE, targetE) {
	var targetpostion = tt.getPos(targetE);
	srcE.style.zIndex = tt.getRuntimeStyle(targetE, "zIndex");
	srcE.style.position = "absolute";

	var top = targetpostion.t - 1;
	var left = targetpostion.r + 17;
	
	try {
		if (tt.Conf.animate) {
			srcE.style.display = "none";
			srcE.style.top = top + "px";
			srcE.style.left = left + "px";
			$(srcE).fadeIn("slow");
		} else {
			throw e1;
		}
	} catch (e1) {
		srcE.style.display = "";
		srcE.style.top = top + "px";
		srcE.style.left = left + "px";
	}
};

tt.getSelectedCount = function(j, es) {
	if (!es) {
		return 0;
	}

	var types = tt.inputType(es[j]);
	var c = 0;

	if (types.isSelect) {
		for (var i = 0; i < es[j].options.length; i++) {
			if (es[j].options[i].selected) {
				c++;
			}
		}
	} else {
		for (var i = 0; i < es.length; i++) {
			if (es[i].checked) {
				c++;
			}
		}
	}
	return c;
};

/**
 * 
 * @param {}
 *            e element
 * @return {}
 */
tt.inputType = function(e) {
	return {
		'isSelect' : e.tagName == "SELECT",
		'isCheckbox' : e.tagName == "INPUT" && e.type == 'checkbox',
		'isRadio' : e.tagName == "INPUT" && e.type == 'radio'
	};
};

/**
 * 获取表达式的值
 * 
 * @param {}
 *            exp 形如：(#{myid} / 34) * {myname} //
 * @param {}
 *            index used for "getElementsByName()[index]"
 * @return (44 / 34) * 22
 */
tt.getExp = function(exp, index) {
	var idRex = /\#{1}\{{1}([^}]+)\}{1}/gi;
	var nameRex = /\${1}\{{1}([^}]+)\}{1}/gi;

	var ret = exp;
	while ((result = idRex.exec(exp)) != null) {
		ret = ret.replace(result[0], tt.id(result[1]).value);
	}
	while ((result = nameRex.exec(exp)) != null) {
		var es = document.getElementsByName((result[1]));
		var v = (es[index]) ? es[index].value : es[es.length - 1].value;
		ret = ret.replace(result[0], v);
	}

	return ret;
};

tt.setVfP = function(clrSpace) {
	tt.vf.clrSpace = clrSpace;
};

tt.rmNull = function(arr) {
	var temp = [];
	for (var i = 0; i < arr.length; i++) {
		if (arr[i]) {
			temp.push(arr[i]);
		}
	}
	return temp;
};