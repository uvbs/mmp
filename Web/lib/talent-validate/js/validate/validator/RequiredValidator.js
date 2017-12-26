tt.ReqV = tt.BV.ext({
v : function(s, i, es, f) {
	this.setI18(tt.i18Req);
	if (es[i].type == "file" || (es[i].tagName && (es[i].tagName == "SELECT"))) {
		this.setI18(tt.i18Req1);
	}
	return (s);
},
/**
 * 在字段后面加上星号
 */
doAfterAdd : function(f) {
	this._super(f);
	this.addStar(f);
},
/**
 * 当注销后做些事情,子类视情况实现该函数,如Required验证器,需要去掉后面的红星号
 */
doBeforeRm : function(f) {
	this._super(f);
	this.clearStar(f); // 清空星号
},
/**
 * 添加星号
 */
addStar : function(f) {
	if (tt.Conf.addStar4Req){
		for (var i = 0; i < f.es.length; i++) {
			seq = tt.vf.seq++;
			!f.es[i][tt.Conf.pro4Star] ? f.es[i][tt.Conf.pro4Star] = tt.Conf.pro4Star + seq : null;
			tt.insertAfter(f.es[i], tt.getI18S("<span id='{0}' class='{1}'>*</span>", [f.es[i][tt.Conf.pro4Star], tt.Conf.reqStarCls]));
		}
	}
},
/**
 * 清空星号
 */
clearStar : function(f) {
	if (tt.Conf.addStar4Req){
		for (i = 0; i< f.es.length; i++) {
			tt.rmEle(tt.id(f.es[i][tt.Conf.pro4Star]));
		}
	}
}
});