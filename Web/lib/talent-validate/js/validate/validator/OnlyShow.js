tt.OnlyShow = tt.BV.ext({
	v : function(){
		return this.result;
	},
	getTip : function() {
		return this.result ? this.msg : null;
	},
	getI18 : function() {
		return !this.result ? this.msg : null;
	}
});