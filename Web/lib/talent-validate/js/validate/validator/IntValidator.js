tt.IV = tt.NV.ext({
	v:function(s)
	{
		this.initI18n(tt.i18Int);
		return this.isInt(s);
	},
	isInt:function(s)
	{
		if (this.isNum(s))
		{
			var t = (s % 10) + "";
			return (!s) || t.indexOf(".") == -1;
		}
		return false;
	}
});