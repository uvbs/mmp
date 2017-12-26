tt.text = tt.bh.ext({
	h:function()
	{
		var i18n = this.v.getI18(this.f.label, this.e, this.f, this.index, this.val);
		if (this.needHandle() && i18n) {
			this.render(tt.Conf.aniCls + " " + tt.Conf.errCls, i18n, "t_v_Close", tt.Conf.errInputCls);
		}
	}
});