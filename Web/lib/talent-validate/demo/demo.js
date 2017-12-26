function showCode() {
	var talent_script = document.getElementById('talent_script');
	if (!talent_script) {
		return;
	}

	var _html = talent_script.innerHTML;

	var containerDiv = document.createElement("div");
	var codeDiv = document.createElement("div");
	var codeDivHeader = document.createElement("div");
	var codeAera = document.createElement("pre");

	containerDiv.className = 'container-fluid';
	
	codeDiv.className = 'codeDiv';
	codeDivHeader.className = 'codeDivHeader';
	codeAera.name = "code";
	codeAera.setAttribute('name', 'code'); // ff下须如此

	codeAera.className = 'javascript';
	codeAera.id = 'script_div';

	codeDivHeader.innerHTML = "对应的js代码";

	document.body.appendChild(containerDiv);
	containerDiv.appendChild(codeDiv);
	codeDiv.appendChild(codeDivHeader);
	codeDiv.appendChild(codeAera);

	// _html = _html.replace(";",";/r/n");
	codeAera.innerHTML = _html;
}

function showBodyCode() {
	var _html = document.body.innerHTML + "";

	var codeDiv = document.createElement("div");
	var codeDivHeader = document.createElement("div");
	var codeAera = document.createElement("textarea");

	codeDiv.className = 'codeDiv';
	codeDivHeader.className = 'codeDivHeader';
	codeAera.name = "code";
	codeAera.setAttribute('name', 'code'); // ff下须如此

	codeAera.className = 'javascript';
	codeAera.id = 'body_div';

	codeDivHeader.innerHTML = "html code";

	document.body.appendChild(codeDiv);
	codeDiv.appendChild(codeDivHeader);
	codeDiv.appendChild(codeAera);

	// _html = _html.replace(";",";/r/n");
	document.getElementById('body_div').innerHTML = _html;
}
function wrapForm() {
	var codeDiv = document.createElement("div");
	var codeDivHeader = document.createElement("div");

	codeDiv.className = 'codeDiv';
	codeDivHeader.className = 'codeDivHeader';

	codeDivHeader.innerHTML = "显示效果";

	// document.body.appendChild(codeDiv);

	codeDiv.appendChild(codeDivHeader);

	var forms = document.forms;
	for (var i = 0; i < forms.length; i++) {
		if (i == 0) {
			forms[i].parentNode.insertBefore(codeDiv, forms[i]);
		}
		codeDiv.appendChild(forms[i]);
	}

}
function loadjscssfile(filename, filetype) {
	if (filetype == "js") { // if filename is a external JavaScript file
		var fileref = document.createElement('script')
		fileref.setAttribute("type", "text/javascript")
		fileref.setAttribute("src", filename)
	} else if (filetype == "css") { // if filename is an external CSS file
		var fileref = document.createElement("link")
		fileref.setAttribute("rel", "stylesheet")
		fileref.setAttribute("type", "text/css")
		fileref.setAttribute("href", filename)
	}
	if (typeof fileref != "undefined")
		document.getElementsByTagName("head")[0].appendChild(fileref)
}

window.onload = function() {
	//loadjscssfile("../css/animate.min.css", "css");
//	loadjscssfile("../Styles/SyntaxHighlighter.css", "css");
//	loadjscssfile("../Scripts/shCore.js", "js");
//	loadjscssfile("../Scripts/shBrushJScript.js", "js");
//	loadjscssfile("../Scripts/shBrushJava.js", "js");
//	loadjscssfile("./demo.css", "css");
	
	setTimeout('timeoutExe()', 1);
}

function timeoutExe() {
	// showBodyCode();
	wrapForm();
	showCode();
	try {
		// tt.validate();
	} catch (e) {

	}

	dp.SyntaxHighlighter.ClipboardSwf = './Scripts/clipboard.swf';
	dp.SyntaxHighlighter.HighlightAll('code');
}

// ----------------- static method start -------------------------//
/**
 * @param name标签的name属性值
 * @param tagName标签的名字
 *            根据tagName和name属性获取对象 获取不到返回null,否则返回数组
 */
function getByNameAndTagName(name, tagName) {
	var s = document.getElementsByTagName(tagName);
	if (!s) {
		return null;
	}
	var j = 0;
	var ret = [];
	for (var i = 0; i < s.length; i++) {
		if (s[i].name == name) {
			ret[j++] = s[i];
		}
	}
	return j == 0 ? null : ret;
}

/**
 * 返回element后面的的第index个元素
 * 
 * @param element
 *            被操作的元素
 * @param index
 *            从0开始
 * @return
 */
nextEle = function(e, index) {
	if (!e) {
		return;
	}
	var ret = e;
	for (var i = 0; i <= index; i++) {
		ret = ret.nextSibling;
		if (ret == null) {
			break;
		}
	}
	return ret;
};
/**
 * 返回element前面的的第index个元素
 * 
 * @param element
 *            被操作的元素
 * @param index
 *            从0开始
 * @return
 */
preEle = function(e, index) {
	var ret = e;
	for (var i = 0; i <= index; i++) {
		ret = ret.previousSibling;
		if (ret == null) {
			break;
		}
	}
	return ret;
};
