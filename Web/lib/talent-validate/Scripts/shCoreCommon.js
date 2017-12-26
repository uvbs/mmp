var dp = {
	sh : {
		Toolbar : {},
		Utils : {},
		RegexLib : {},
		Brushes : {},
		Version : "1.5.1"
	}
};
dp.SyntaxHighlighter = dp.sh;
dp.sh.Toolbar.CopyToClipboard = function($) {
	var _ = $;
	while (_ != null && _.className.indexOf("dp-highlighter") == -1)
		_ = _.parentNode;
	var A = _.highlighter, B = A.originalCode.replace(/&lt;/g, "<").replace(
			/&gt;/g, ">").replace(/&amp;/g, "&");
	window.clipboardData.setData("text", B);
	alert("\u4ee3\u7801\u5df2\u88ab\u590d\u5236\u5230\u526a\u8d34\u677f")
};
dp.sh.Toolbar.Create = function(_) {
	var $ = document.createElement("DIV");
	$.className = "tools";
	$.innerHTML = _.language.capitalize() + "\u4ee3\u7801";
	if (window.clipboardData)
	{
		//$.innerHTML += " <a href=\"#\" onclick=\"dp.sh.Toolbar.CopyToClipboard(this);return false;\" title=\"\u590d\u5236\u4ee3\u7801\"><img src=\"/images/icon_copy.gif\" alt=\"\u590d\u5236\u4ee3\u7801\"/></a>";
	}
	else {
		var A = _.originalCode.replace(/&lt;/g, "<").replace(/&gt;/g, ">")
				.replace(/&amp;/g, "&");
		$.innerHTML += " <embed src=\"/javascripts/syntaxhighlighter/clipboard_new.swf\" width=\"14\" height=\"15\" flashvars=\"clipboard="
				+ encodeURIComponent(A)
				+ "\" quality=\"high\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\"/>"
	}
	return $
};
dp.sh.RegexLib = {
	MultiLineCComments : new RegExp("/\\*[\\s\\S]*?\\*/", "gm"),
	SingleLineCComments : new RegExp("//.*$", "gm"),
	SingleLinePerlComments : new RegExp("#.*$", "gm"),
	DoubleQuotedString : new RegExp("\"(?:\\.|(\\\\\\\")|[^\\\"\"\\n])*\"", "g"),
	SingleQuotedString : new RegExp("'(?:\\.|(\\\\\\')|[^\\''\\n])*'", "g")
};
dp.sh.Match = function(_, $, A) {
	this.value = _;
	this.index = $;
	this.length = _.length;
	this.css = A
};
dp.sh.Highlighter = function() {
	this.noGutter = false;
	this.addControls = true;
	this.collapse = false;
	this.tabsToSpaces = true;
	this.wrapColumn = 80;
	this.showColumns = true
};
dp.sh.Highlighter.SortCallback = function($, _) {
	if ($.index < _.index)
		return -1;
	else if ($.index > _.index)
		return 1;
	else if ($.length < _.length)
		return -1;
	else if ($.length > _.length)
		return 1;
	return 0
};
dp.sh.Highlighter.prototype.CreateElement = function(_) {
	var $ = document.createElement(_);
	$.highlighter = this;
	return $
};
dp.sh.Highlighter.prototype.GetMatches = function(_, B) {
	var $ = 0, A = null;
	while ((A = _.exec(this.code)) != null)
		this.matches[this.matches.length] = new dp.sh.Match(A[0], A.index, B)
};
dp.sh.Highlighter.prototype.AddBit = function($, A) {
	if ($ == null || $.length == 0)
		return;
	var C = this.CreateElement("SPAN");
	$ = $.replace(/ /g, "&nbsp;");
	$ = $.replace(/</g, "&lt;");
	$ = $.replace(/\n/gm, "&nbsp;<br>");
	if (A != null) {
		if ((/br/gi).test($)) {
			var _ = $.split("&nbsp;<br>");
			for (var B = 0; B < _.length; B++) {
				C = this.CreateElement("SPAN");
				C.className = A;
				C.innerHTML = _[B];
				this.div.appendChild(C);
				if (B + 1 < _.length)
					this.div.appendChild(this.CreateElement("BR"))
			}
		} else {
			C.className = A;
			C.innerHTML = $;
			this.div.appendChild(C)
		}
	} else {
		C.innerHTML = $;
		this.div.appendChild(C)
	}
};
dp.sh.Highlighter.prototype.IsInside = function(_) {
	if (_ == null || _.length == 0)
		return false;
	for (var A = 0; A < this.matches.length; A++) {
		var $ = this.matches[A];
		if ($ == null)
			continue;
		if ((_.index > $.index) && (_.index < $.index + $.length))
			return true
	}
	return false
};
dp.sh.Highlighter.prototype.ProcessRegexList = function() {
	for (var $ = 0; $ < this.regexList.length; $++)
		this.GetMatches(this.regexList[$].regex, this.regexList[$].css)
};
dp.sh.Highlighter.prototype.ProcessSmartTabs = function(E) {
	var B = E.split("\n"), $ = "", D = 4, A = "\t";
	function _(A, E, _) {
		var B = A.substr(0, E), C = A.substr(E + 1, A.length), $ = "";
		for (var D = 0; D < _; D++)
			$ += " ";
		return B + $ + C
	}
	function C(B, C) {
		if (B.indexOf(A) == -1)
			return B;
		var D = 0;
		while ((D = B.indexOf(A)) != -1) {
			var $ = C - D % C;
			B = _(B, D, $)
		}
		return B
	}
	for (var F = 0; F < B.length; F++)
		$ += C(B[F], D) + "\n";
	return $
};
dp.sh.Highlighter.prototype.SwitchToList = function() {
	var C = this.div.innerHTML.replace(/<(br)\/?>/gi, "\n"), B = C.split("\n");
	if (this.addControls == true)
		this.bar.appendChild(dp.sh.Toolbar.Create(this));
	if (this.showColumns) {
		var A = this.CreateElement("div"), _ = this.CreateElement("div"), E = 10, G = 1;
		while (G <= 150)
			if (G % E == 0) {
				A.innerHTML += G;
				G += (G + "").length
			} else {
				A.innerHTML += "&middot;";
				G++
			}
		_.className = "columns";
		_.appendChild(A);
		this.bar.appendChild(_)
	}
	for (var G = 0, D = this.firstLine; G < B.length - 1; G++, D++) {
		var $ = this.CreateElement("LI"), F = this.CreateElement("SPAN");
		F.innerHTML = B[G] + "&nbsp;";
		$.appendChild(F);
		this.ol.appendChild($)
	}
	this.div.innerHTML = ""
};
dp.sh.Highlighter.prototype.Highlight = function(C) {
	function A($) {
		return $.replace(/^\s*(.*?)[\s\n]*$/g, "$1")
	}
	function $($) {
		return $.replace(/\n*$/, "").replace(/^\n*/, "")
	}
	function _(B) {
		var E = B.split("\n"), C = new Array(), D = new RegExp("^\\s*", "g"), $ = 1000;
		for (var F = 0; F < E.length && $ > 0; F++) {
			if (A(E[F]).length == 0)
				continue;
			var _ = D.exec(E[F]);
			if (_ != null && _.length > 0)
				$ = Math.min(_[0].length, $)
		}
		if ($ > 0)
			for (F = 0; F < E.length; F++)
				E[F] = E[F].substr($);
		return E.join("\n")
	}
	function D(A, $, _) {
		return A.substr($, _ - $)
	}
	var F = 0;
	if (C == null)
		C = "";
	this.originalCode = C;
	this.code = $(_(C));
	this.div = this.CreateElement("DIV");
	this.bar = this.CreateElement("DIV");
	this.ol = this.CreateElement("OL");
	this.matches = new Array();
	this.div.className = "dp-highlighter";
	this.div.highlighter = this;
	this.bar.className = "bar";
	this.ol.start = this.firstLine;
	if (this.CssClass != null)
		this.ol.className = this.CssClass;
	if (this.collapse)
		this.div.className += " collapsed";
	if (this.noGutter)
		this.div.className += " nogutter";
	if (this.tabsToSpaces == true)
		this.code = this.ProcessSmartTabs(this.code);
	this.ProcessRegexList();
	if (this.matches.length == 0) {
		this.AddBit(this.code, null);
		this.SwitchToList();
		this.div.appendChild(this.bar);
		this.div.appendChild(this.ol);
		return
	}
	this.matches = this.matches.sort(dp.sh.Highlighter.SortCallback);
	for (var E = 0; E < this.matches.length; E++)
		if (this.IsInside(this.matches[E]))
			this.matches[E] = null;
	for (E = 0; E < this.matches.length; E++) {
		var B = this.matches[E];
		if (B == null || B.length == 0)
			continue;
		this.AddBit(D(this.code, F, B.index), null);
		this.AddBit(B.value, B.css);
		F = B.index + B.length
	}
	this.AddBit(this.code.substr(F), null);
	this.SwitchToList();
	this.div.appendChild(this.bar);
	this.div.appendChild(this.ol)
};
dp.sh.Highlighter.prototype.GetKeywords = function($) {
	return "\\b" + $.replace(/ /g, "\\b|\\b") + "\\b"
};
dp.sh.HighlightAll = function(N, B, K, I, O, E) {
	function A() {
		var $ = arguments;
		for (var _ = 0; _ < $.length; _++) {
			if ($[_] == null)
				continue;
			if (typeof($[_]) == "string" && $[_] != "")
				return $[_] + "";
			if (typeof($[_]) == "object" && $[_].value != "")
				return $[_].value + ""
		}
		return null
	}
	function J($, _) {
		for (var A = 0; A < _.length; A++)
			if (_[A] == $)
				return true;
		return false
	}
	function L(A, B, C) {
		var _ = new RegExp("^" + A + "\\[(\\w+)\\]$", "gi"), $ = null;
		for (var D = 0; D < B.length; D++)
			if (($ = _.exec(B[D])) != null)
				return $[1];
		return C
	}
	function C(B, A, _) {
		var $ = document.getElementsByTagName(_);
		for (var C = 0; C < $.length; C++)
			if ($[C].getAttribute("name") == A)
				B.push($[C])
	}
	var T = [], P = null, M = {}, $ = "innerHTML";
	C(T, N, "pre");
	C(T, N, "textarea");
	if (T.length == 0)
		return;
	for (var R in dp.sh.Brushes) {
		var F = dp.sh.Brushes[R].Aliases;
		if (F == null)
			continue;
		for (var G = 0; G < F.length; G++)
			M[F[G]] = R
	}
	for (G = 0; G < T.length; G++) {
		var _ = T[G], U = A(_.attributes["class"], _.className,
				_.attributes["language"], _.language), Q = "";
		if (U == null)
			continue;
		U = U.split(":");
		Q = U[0].toLowerCase();
		if (M[Q] == null)
			M[Q] = M["default"];
		P = new dp.sh.Brushes[M[Q]]();
		P.language = Q;
		_.style.display = "none";
		P.noGutter = (B == null) ? J("nogutter", U) : !B;
		P.addControls = (K == null) ? !J("nocontrols", U) : K;
		P.collapse = (I == null) ? J("collapse", U) : I;
		P.showColumns = (E == null) ? J("showcolumns", U) : E;
		var D = document.getElementsByTagName("head")[0];
		if (P.Style && D) {
			var S = document.createElement("style");
			S.setAttribute("type", "text/css");
			if (S.styleSheet)
				S.styleSheet.cssText = P.Style;
			else {
				var H = document.createTextNode(P.Style);
				S.appendChild(H)
			}
			D.appendChild(S)
		}
		P.firstLine = (O == null) ? parseInt(L("firstline", U, 1)) : O;
		P.Highlight(_[$]);
		P.source = _;
		_.parentNode.insertBefore(P.div, _)
	}
};
dp.sh.Brushes.JScript = function() {
	var $ = "abstract boolean break byte case catch char class const continue debugger "
			+ "default delete do double else enum export extends false final finally float "
			+ "for function goto if implements import in instanceof int interface long native "
			+ "new null package private protected public return short static super switch "
			+ "synchronized this throw throws transient true try typeof var void volatile while with";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.MultiLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("^\\s*#.*", "gm"),
				css : "preprocessor"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}];
	this.CssClass = "dp-c"
};
dp.sh.Brushes.JScript.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.JScript.Aliases = ["js", "jscript", "javascript"];
dp.sh.Brushes.Java = function() {
	var $ = "abstract assert boolean break byte case catch char class const "
			+ "continue default do double else enum extends "
			+ "false final finally float for goto if implements import "
			+ "instanceof int interface long native new null "
			+ "package private protected public return "
			+ "short static strictfp super switch synchronized this throw throws true "
			+ "transient try void volatile while";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.MultiLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("\\b([\\d]+(\\.[\\d]+)?|0x[a-f0-9]+)\\b",
						"gi"),
				css : "number"
			}, {
				regex : new RegExp("(?!\\@interface\\b)\\@[\\$\\w]+\\b", "g"),
				css : "annotation"
			}, {
				regex : new RegExp("\\@interface\\b", "g"),
				css : "keyword"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}];
	this.CssClass = "dp-j";
	this.Style = ".dp-j .annotation { color: #646464; }"
			+ ".dp-j .number { color: #C00000; }"
};
dp.sh.Brushes.Java.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Java.Aliases = ["java"];
dp.sh.Brushes.Ruby = function() {
	var $ = "alias and BEGIN begin break case class def define_method defined do each else elsif "
			+ "END end ensure false for if in module new next nil not or raise redo rescue retry return "
			+ "self super then throw true undef unless until when while yield", _ = "Array Bignum Binding Class Continuation Dir Exception FalseClass File::Stat File Fixnum Fload "
			+ "Hash Integer IO MatchData Method Module NilClass Numeric Object Proc Range Regexp String Struct::TMS Symbol "
			+ "ThreadGroup Thread Time TrueClass";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLinePerlComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp(":[a-z][A-Za-z0-9_]*", "g"),
				css : "symbol"
			}, {
				regex : new RegExp("(\\$|@@|@)\\w+", "g"),
				css : "variable"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}, {
				regex : new RegExp(this.GetKeywords(_), "gm"),
				css : "builtin"
			}];
	this.CssClass = "dp-rb";
	this.Style = ".dp-rb .symbol { color: #a70; }"
			+ ".dp-rb .variable { color: #a70; font-weight: bold; }"
};
dp.sh.Brushes.Ruby.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Ruby.Aliases = ["ruby", "rails", "ror"];
dp.sh.Brushes.Xml = function() {
	this.CssClass = "dp-xml";
	this.Style = ".dp-xml .cdata { color: #ff1493; }"
			+ ".dp-xml .tag, .dp-xml .tag-name { color: #069; font-weight: bold; }"
			+ ".dp-xml .attribute { color: red; }"
			+ ".dp-xml .attribute-value { color: blue; }"
};
dp.sh.Brushes.Xml.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Xml.Aliases = ["xml", "xhtml", "xslt", "html", "xhtml"];
dp.sh.Brushes.Xml.prototype.ProcessRegexList = function() {
	function B(_, $) {
		_[_.length] = $
	}
	var $ = 0, A = null, _ = null;
	this.GetMatches(
			new RegExp("(&lt;|<)\\!\\[[\\w\\s]*?\\[(.|\\s)*?\\]\\](&gt;|>)",
					"gm"), "cdata");
	this.GetMatches(new RegExp("(&lt;|<)!--\\s*.*?\\s*--(&gt;|>)", "gm"),
			"comments");
	_ = new RegExp("([:\\w-.]+)\\s*=\\s*(\".*?\"|'.*?'|\\w+)*|(\\w+)", "gm");
	while ((A = _.exec(this.code)) != null) {
		if (A[1] == null)
			continue;
		B(this.matches, new dp.sh.Match(A[1], A.index, "attribute"));
		if (A[2] != undefined)
			B(this.matches, new dp.sh.Match(A[2], A.index + A[0].indexOf(A[2]),
							"attribute-value"))
	}
	this.GetMatches(new RegExp("(&lt;|<)/*\\?*(?!\\!)|/*\\?*(&gt;|>)", "gm"),
			"tag");
	_ = new RegExp("(?:&lt;|<)/*\\?*\\s*([:\\w-.]+)", "gm");
	while ((A = _.exec(this.code)) != null)
		B(this.matches, new dp.sh.Match(A[1], A.index + A[0].indexOf(A[1]),
						"tag-name"))
};
dp.sh.Brushes.CSharp = function() {
	var $ = "abstract as base bool break byte case catch char checked class const "
			+ "continue decimal default delegate do double else enum event explicit "
			+ "extern false finally fixed float for foreach get goto if implicit in int "
			+ "interface internal is lock long namespace new null object operator out "
			+ "override params private protected public readonly ref return sbyte sealed set "
			+ "short sizeof stackalloc static string struct switch this throw true try "
			+ "typeof uint ulong unchecked unsafe ushort using virtual void while";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.MultiLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("^\\s*#.*", "gm"),
				css : "preprocessor"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}];
	this.CssClass = "dp-c";
	this.Style = ".dp-c .vars { color: #d00; }"
};
dp.sh.Brushes.CSharp.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.CSharp.Aliases = ["c#", "c-sharp", "csharp"];
dp.sh.Brushes.Cpp = function() {
	var _ = "ATOM BOOL BOOLEAN BYTE CHAR COLORREF DWORD DWORDLONG DWORD_PTR "
			+ "DWORD32 DWORD64 FLOAT HACCEL HALF_PTR HANDLE HBITMAP HBRUSH "
			+ "HCOLORSPACE HCONV HCONVLIST HCURSOR HDC HDDEDATA HDESK HDROP HDWP "
			+ "HENHMETAFILE HFILE HFONT HGDIOBJ HGLOBAL HHOOK HICON HINSTANCE HKEY "
			+ "HKL HLOCAL HMENU HMETAFILE HMODULE HMONITOR HPALETTE HPEN HRESULT "
			+ "HRGN HRSRC HSZ HWINSTA HWND INT INT_PTR INT32 INT64 LANGID LCID LCTYPE "
			+ "LGRPID LONG LONGLONG LONG_PTR LONG32 LONG64 LPARAM LPBOOL LPBYTE LPCOLORREF "
			+ "LPCSTR LPCTSTR LPCVOID LPCWSTR LPDWORD LPHANDLE LPINT LPLONG LPSTR LPTSTR "
			+ "LPVOID LPWORD LPWSTR LRESULT PBOOL PBOOLEAN PBYTE PCHAR PCSTR PCTSTR PCWSTR "
			+ "PDWORDLONG PDWORD_PTR PDWORD32 PDWORD64 PFLOAT PHALF_PTR PHANDLE PHKEY PINT "
			+ "PINT_PTR PINT32 PINT64 PLCID PLONG PLONGLONG PLONG_PTR PLONG32 PLONG64 POINTER_32 "
			+ "POINTER_64 PSHORT PSIZE_T PSSIZE_T PSTR PTBYTE PTCHAR PTSTR PUCHAR PUHALF_PTR "
			+ "PUINT PUINT_PTR PUINT32 PUINT64 PULONG PULONGLONG PULONG_PTR PULONG32 PULONG64 "
			+ "PUSHORT PVOID PWCHAR PWORD PWSTR SC_HANDLE SC_LOCK SERVICE_STATUS_HANDLE SHORT "
			+ "SIZE_T SSIZE_T TBYTE TCHAR UCHAR UHALF_PTR UINT UINT_PTR UINT32 UINT64 ULONG "
			+ "ULONGLONG ULONG_PTR ULONG32 ULONG64 USHORT USN VOID WCHAR WORD WPARAM WPARAM WPARAM "
			+ "char bool short int __int32 __int64 __int8 __int16 long float double __wchar_t "
			+ "clock_t _complex _dev_t _diskfree_t div_t ldiv_t _exception _EXCEPTION_POINTERS "
			+ "FILE _finddata_t _finddatai64_t _wfinddata_t _wfinddatai64_t __finddata64_t "
			+ "__wfinddata64_t _FPIEEE_RECORD fpos_t _HEAPINFO _HFILE lconv intptr_t "
			+ "jmp_buf mbstate_t _off_t _onexit_t _PNH ptrdiff_t _purecall_handler "
			+ "sig_atomic_t size_t _stat __stat64 _stati64 terminate_function "
			+ "time_t __time64_t _timeb __timeb64 tm uintptr_t _utimbuf "
			+ "va_list wchar_t wctrans_t wctype_t wint_t signed", $ = "break case catch class const __finally __exception __try "
			+ "const_cast continue private public protected __declspec "
			+ "default delete deprecated dllexport dllimport do dynamic_cast "
			+ "else enum explicit extern if for friend goto inline "
			+ "mutable naked namespace new noinline noreturn nothrow "
			+ "register reinterpret_cast return selectany "
			+ "sizeof static static_cast struct switch template this "
			+ "thread throw true false try typedef typeid typename union "
			+ "using uuid virtual void volatile whcar_t while";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.MultiLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("^ *#.*", "gm"),
				css : "preprocessor"
			}, {
				regex : new RegExp(this.GetKeywords(_), "gm"),
				css : "datatypes"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}];
	this.CssClass = "dp-cpp";
	this.Style = ".dp-cpp .datatypes { color: #2E8B57; font-weight: bold; }"
};
dp.sh.Brushes.Cpp.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Cpp.Aliases = ["cpp", "c", "c++"];
dp.sh.Brushes.Python = function() {
	var $ = "and assert break class continue def del elif else "
			+ "except exec finally for from global if import in is "
			+ "lambda not or pass print raise return try yield while", _ = "None True False self cls class_";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLinePerlComments,
				css : "comment"
			}, {
				regex : new RegExp("^\\s*@\\w+", "gm"),
				css : "decorator"
			}, {
				regex : new RegExp("(['\"]{3})([^\\1])*?\\1", "gm"),
				css : "comment"
			}, {
				regex : new RegExp(
						"\"(?!\")(?:\\.|\\\\\\\"|[^\\\"\"\\n\\r])*\"", "gm"),
				css : "string"
			}, {
				regex : new RegExp("'(?!')*(?:\\.|(\\\\\\')|[^\\''\\n\\r])*'",
						"gm"),
				css : "string"
			}, {
				regex : new RegExp("\\b\\d+\\.?\\w*", "g"),
				css : "number"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}, {
				regex : new RegExp(this.GetKeywords(_), "gm"),
				css : "special"
			}];
	this.CssClass = "dp-py";
	this.Style = ".dp-py .builtins { color: #ff1493; }"
			+ ".dp-py .magicmethods { color: #808080; }"
			+ ".dp-py .exceptions { color: brown; }"
			+ ".dp-py .types { color: brown; font-style: italic; }"
			+ ".dp-py .commonlibs { color: #8A2BE2; font-style: italic; }"
};
dp.sh.Brushes.Python.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Python.Aliases = ["py", "python"];
dp.sh.Brushes.Sql = function() {
	var _ = "abs avg case cast coalesce convert count current_timestamp "
			+ "current_user day isnull left lower month nullif replace right "
			+ "session_user space substring sum system_user upper user year", $ = "absolute action add after alter as asc at authorization begin bigint "
			+ "binary bit by cascade char character check checkpoint close collate "
			+ "column commit committed connect connection constraint contains continue "
			+ "create cube current current_date current_time cursor database date "
			+ "deallocate dec decimal declare default delete desc distinct double drop "
			+ "dynamic else end end-exec escape except exec execute false fetch first "
			+ "float for force foreign forward free from full function global goto grant "
			+ "group grouping having hour ignore index inner insensitive insert instead "
			+ "int integer intersect into is isolation key last level load local max min "
			+ "minute modify move name national nchar next no numeric of off on only "
			+ "open option order out output partial password precision prepare primary "
			+ "prior privileges procedure public read real references relative repeatable "
			+ "restrict return returns revoke rollback rollup rows rule schema scroll "
			+ "second section select sequence serializable set size smallint static "
			+ "statistics table temp temporary then time timestamp to top transaction "
			+ "translation trigger true truncate uncommitted union unique update values "
			+ "varchar varying view when where with work", A = "all and any between cross in join like not null or outer some";
	this.regexList = [{
				regex : new RegExp("--(.*)$", "gm"),
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp(this.GetKeywords(_), "gmi"),
				css : "func"
			}, {
				regex : new RegExp(this.GetKeywords(A), "gmi"),
				css : "op"
			}, {
				regex : new RegExp(this.GetKeywords($), "gmi"),
				css : "keyword"
			}];
	this.CssClass = "dp-sql";
	this.Style = ".dp-sql .func { color: #ff1493; }"
			+ ".dp-sql .op { color: #808080; }"
};
dp.sh.Brushes.Sql.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Sql.Aliases = ["sql"];
dp.sh.Brushes.Php = function() {
	var _ = "abs acos acosh addcslashes addslashes "
			+ "array_change_key_case array_chunk array_combine array_count_values array_diff "
			+ "array_diff_assoc array_diff_key array_diff_uassoc array_diff_ukey array_fill "
			+ "array_filter array_flip array_intersect array_intersect_assoc array_intersect_key "
			+ "array_intersect_uassoc array_intersect_ukey array_key_exists array_keys array_map "
			+ "array_merge array_merge_recursive array_multisort array_pad array_pop array_product "
			+ "array_push array_rand array_reduce array_reverse array_search array_shift "
			+ "array_slice array_splice array_sum array_udiff array_udiff_assoc "
			+ "array_udiff_uassoc array_uintersect array_uintersect_assoc "
			+ "array_uintersect_uassoc array_unique array_unshift array_values array_walk "
			+ "array_walk_recursive atan atan2 atanh base64_decode base64_encode base_convert "
			+ "basename bcadd bccomp bcdiv bcmod bcmul bindec bindtextdomain bzclose bzcompress "
			+ "bzdecompress bzerrno bzerror bzerrstr bzflush bzopen bzread bzwrite ceil chdir "
			+ "checkdate checkdnsrr chgrp chmod chop chown chr chroot chunk_split class_exists "
			+ "closedir closelog copy cos cosh count count_chars date decbin dechex decoct "
			+ "deg2rad delete ebcdic2ascii echo empty end ereg ereg_replace eregi eregi_replace error_log "
			+ "error_reporting escapeshellarg escapeshellcmd eval exec exit exp explode extension_loaded "
			+ "feof fflush fgetc fgetcsv fgets fgetss file_exists file_get_contents file_put_contents "
			+ "fileatime filectime filegroup fileinode filemtime fileowner fileperms filesize filetype "
			+ "floatval flock floor flush fmod fnmatch fopen fpassthru fprintf fputcsv fputs fread fscanf "
			+ "fseek fsockopen fstat ftell ftok getallheaders getcwd getdate getenv gethostbyaddr gethostbyname "
			+ "gethostbynamel getimagesize getlastmod getmxrr getmygid getmyinode getmypid getmyuid getopt "
			+ "getprotobyname getprotobynumber getrandmax getrusage getservbyname getservbyport gettext "
			+ "gettimeofday gettype glob gmdate gmmktime ini_alter ini_get ini_get_all ini_restore ini_set "
			+ "interface_exists intval ip2long is_a is_array is_bool is_callable is_dir is_double "
			+ "is_executable is_file is_finite is_float is_infinite is_int is_integer is_link is_long "
			+ "is_nan is_null is_numeric is_object is_readable is_real is_resource is_scalar is_soap_fault "
			+ "is_string is_subclass_of is_uploaded_file is_writable is_writeable mkdir mktime nl2br "
			+ "parse_ini_file parse_str parse_url passthru pathinfo readlink realpath rewind rewinddir rmdir "
			+ "round str_ireplace str_pad str_repeat str_replace str_rot13 str_shuffle str_split "
			+ "str_word_count strcasecmp strchr strcmp strcoll strcspn strftime strip_tags stripcslashes "
			+ "stripos stripslashes stristr strlen strnatcasecmp strnatcmp strncasecmp strncmp strpbrk "
			+ "strpos strptime strrchr strrev strripos strrpos strspn strstr strtok strtolower strtotime "
			+ "strtoupper strtr strval substr substr_compare", $ = "and or xor __FILE__ __LINE__ array as break case "
			+ "cfunction class const continue declare default die do else "
			+ "elseif empty enddeclare endfor endforeach endif endswitch endwhile "
			+ "extends for foreach function include include_once global if "
			+ "new old_function return static switch use require require_once "
			+ "var while __FUNCTION__ __CLASS__ "
			+ "__METHOD__ abstract interface public implements extends private protected throw";
	this.regexList = [{
				regex : dp.sh.RegexLib.SingleLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.MultiLineCComments,
				css : "comment"
			}, {
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("\\$\\w+", "g"),
				css : "vars"
			}, {
				regex : new RegExp(this.GetKeywords(_), "gmi"),
				css : "func"
			}, {
				regex : new RegExp(this.GetKeywords($), "gm"),
				css : "keyword"
			}];
	this.CssClass = "dp-c"
};
dp.sh.Brushes.Php.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Php.Aliases = ["php"];
dp.sh.Brushes.Default = function() {
	this.regexList = [{
				regex : dp.sh.RegexLib.DoubleQuotedString,
				css : "string"
			}, {
				regex : dp.sh.RegexLib.SingleQuotedString,
				css : "string"
			}, {
				regex : new RegExp("\\b([\\d]+(\\.[\\d]+)?|0x[a-f0-9]+)\\b",
						"gi"),
				css : "number"
			}];
	this.CssClass = "dp-default";
	this.Style = ".dp-default .number { color: #C00000; }"
};
dp.sh.Brushes.Default.prototype = new dp.sh.Highlighter();
dp.sh.Brushes.Default.Aliases = ["default"]