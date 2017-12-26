tt.GV = tt.BV.ext({
	/**
	 * 
	 * @param {} exp 形如："tt.vf.req{} && (tt.vf.email{} || tt.vf.idcard{})"
	 */
	set: function(exp){
		
		this.exp = exp;
		return this;
	},
	v : function(s, i, es, f)
	{
		return eval("(" + this.exp.replace(/\{\}{1}/g, ".v(s, i, es, f)") + ")");
	}
});