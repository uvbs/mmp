tt.NRV = tt.NV.ext({
	init:function(){
		this._super();
		this.needAddNumV = true;
	},
	/**
	 * 设置最小值,"--"表示无穷小
	 */
	setMin:function(min)
	{
		this.min = min;
		if (min == '--')//无穷小
		{
			this.i18n = tt.i18NumRangeMin;
			this.i18ps[0] = this.max;
			
		}else
		{
			this.i18ps[0] = min;
		}
	},
	/**
	 * 设置最大值,"++"表示无穷大
	 */
	setMax:function(max)
	{
		this.max = max;
		if (max == '++')
		{
			this.i18n = tt.i18NumRangeMax;
			this.i18ps[0] = this.min;
		}
		else
		{
			if (this.min == "--")
			{
				this.i18ps[0] = max;
			}
			else
			{
				this.i18ps[1] = max;
			}
		}
	},
	set:function(min, max)
	{
		if(arguments.length == 2) {
			this.setMin(min);
			this.setMax(max);
		} else {
			this.exp = min;
			this.i18n = tt.i18NumRangeExp;
			this.i18ps[0] = this.exp;
		}
		return this;
	},
	_rm:function(type,args){
		this.numV ? this.numV._rm(type,args) : null;
		this.numV = null;
		return this._super(type,args);
	},
	v:function(s)
	{
		this.initI18n(tt.i18NumRange);
		if (!this.numV && this.needAddNumV){
			this.numV = new tt.NV();
			for (i=0; i < this.fs.length; i++) {
				this.numV.add(this.fs[i]);
			}
		}
		if (!s || !this.isNum(s))//如果不是数字，就让数字验证器来验证
		{
			return true;
		}
		
		if (s != 0) {
			s = s.replace(/(^0*)/g, "");  //去掉前面的0，否则有可能被当成8进制
		}
		
		if (this.exp) {
			return eval(tt.parRngExp(s, this.exp));
		}
		
		try 
		{
			if (this.max == "++" && this.min == "--")
			{
				return true;
			}
			
			if (this.max == "++")
			{
				if (eval(s) < eval(this.min))
				{
					return false;
				}else 
				{
					return true;
				}
			}
			
			if (this.min == "--")
			{
				if (eval(s) > eval(this.max))
				{
					return false;
				}else 
				{
					return true;
				}
			}
			
			if (eval(s) > eval(this.max) || eval(s) < eval(this.min))
			{
				return false;
			}
			return true;
		}
		catch (e)
		{
			return false;
		}
	}
});