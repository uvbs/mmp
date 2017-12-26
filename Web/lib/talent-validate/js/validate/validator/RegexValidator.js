tt.RV = tt.BV.ext({
set : function(regex, i18n) {
	this.regex = regex;
	this.i18ps[0] = i18n;
	return this;
},

v : function(s) {
	this.initI18n(tt.i18Regex);
	return (!s) || this.regex.test(s);
}
});