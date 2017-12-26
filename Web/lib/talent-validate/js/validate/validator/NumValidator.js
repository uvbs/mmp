tt.NV = tt.BV.ext({
	v:function(s)
	{
		this.initI18n(tt.i18Num);
		return this.isNum(s);
	},
	isNum:function(s)
	{
		return (!s) || (!isNaN(s) && (s.indexOf('.') != (s.length - 1)) );
	}
});