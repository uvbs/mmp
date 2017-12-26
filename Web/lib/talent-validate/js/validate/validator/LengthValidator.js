tt.LV = tt.NRV.ext({
	init:function(){
		this._super();
		this.needAddNumV = false;
	},
	getTip:function(e,f,v,val,index) {
		var len = tt.getStrLen(val);
		if(this.max != '++' && !this.exp && !['f','a'].ttCons(tt.vf.vFilter)) {
			return tt.getI18S(tt.i18LenTip, [len, this.max - len]);
		} else {
			return this._super(e,f,v,val,index);
		}
	},
	v:function(s)
	{
		var len = tt.getStrLen(s);
		var r = this._super(len+"");
		if(!this.exp){
			v = len - this.max;
			if (v > 0) {  //超长了
				this.i18n = tt.i18LenMin;
				this.i18ps[0] = this.max;
				this.i18ps[1] = v;
			} else if ((v = this.min - len) > 0){ //长度不够
				this.i18n = tt.i18Len;
				this.i18ps[0] = this.min;
				this.i18ps[1] = v;
			}
		} else {
			this.i18n = tt.i18LenExp;
			this.i18ps[0] = this.exp;
		}
		return r;
	}
});