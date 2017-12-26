tt.f.Dft = tt.C.ext({
	init:function()
	{
		this.p = [];
		var a = arguments;
		if(a.length != 0){
			for (var i =0; i < a[0].length; i++){
				this.p.push(a[0][i]);
			}
		}
	},
	/**
	 * @return true:表示需要验证,false:表示被过滤了,不需要验证.
	 */
	f:function(e)
	{
		return true;//默认都需要验证
	}
});