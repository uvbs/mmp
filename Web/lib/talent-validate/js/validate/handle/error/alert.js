tt.alert = tt.bh.ext({
	h:function()
	{
		var i18n = this.v.getI18(this.f.label);
		if (this.needHandle() && i18n) {
			tt.addCls(this.e, tt.Conf.errInputCls);
	        alert(i18n);
		}
	}
});