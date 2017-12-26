/**
 * validate all
 */
tt.validate = function() {
	tt.setVfP(true);
	tt.vf.vFilter = "a";
	return tt.vf.vBf();
};
/**
 * validate form eg:tt.validateForm('formname1', 'formname2', ... );
 */
tt.validateForm = function () {
	tt.setVfP(true);
	tt.vf.vFilter = "f";
	return tt.vf.vBf(new tt.f.Form(arguments));
};
/**
 * validate element eg:tt.validateElement(element1, element2, ... );
 */
tt.validateElement = function() {
	tt.setVfP(false);
	tt.vf.vFilter = "e";
	return tt.vf.vBf(new tt.f.Ele(arguments));
};
/**
 * validate id eg:tt.validateId('id1', 'id2', ... );
 */
tt.validateId = function() {
	tt.setVfP(true);
	tt.vf.vFilter = "i";
	return tt.vf.vBf(new tt.f.Id(arguments));
};
/**
 * validate name eg:tt.validateName('name1', 'name2', ... );
 */
tt.validateName = function() {
	tt.setVfP(true);
	tt.vf.vFilter = "n";
	return tt.vf.vBf(new tt.f.Name(arguments));
};
/**
 * remove all validator from validatorFactory
 */
tt.removeAll = function() {
	tt.vf.rmAll();
};
/**
 * 删除所有服务器端验证的
 */
tt.rmAllRemote = function(){
	tt.vf.rmAllRemote();
};
tt.clearMsg = function(){
	tt.vf.clrAllMsg();
};