tt.tip = tt.bh.ext({
	h:function()
	{
		var tipMsg = this.v.getTip(this.e,this.f,this.v,this.val,this.index);
		if (this.needHandle() && tipMsg) {
			this.render(tt.Conf.aniCls + " " + tt.Conf.tipCls, tipMsg, "t_v_Close t_v_CloseTip", "t_v_SucInput");
		}
	}
});