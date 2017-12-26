
tt.Conf = {
	ver: '4.0.0',
	errorStyle : 'text',     //错误提示的风格，默认提供：['text', 'alert']
	tipStyle : null,//'tip',        //tip提示的风格，默认提供：['tip']。当此值为空时，则不会进行tip提示
	clearOtherError : false, // 当验证某一元素时，是否隐藏其它字段的错误提示。true 隐藏其它字段的错误提示
	validateOn : ['blur'/**'change'*/], // 触发验证的事件类型。  ['keyup', 'focus', 'change']
	clrSpace : true,  //验证时，是否清空输入值两端的空格
	animate: true,      //是否使用动画(此项配置只有你的页面中引入了jquery.js时才有效)
	highlight: true,    //当鼠标经过input元素时，是否要把信息突出显示

	//---------  下面的配置项不建议修改  ----------------
	pro4MsgId : 'ttTalentMsgId',
	pro4Star : 'ttTalentReqStarId',
	
	//eventId : 'talentEventId',
	
	aniCls:"animated swing",
	errCls: "t_v_ErrMsg",         //错误提示信息的样式
	tipCls: "t_v_TipMsg",         //验证通过时，信息的样式
	errInputCls: "t_v_ErrInput",  //验证不通过时，输入框的样式
	addStar4Req: true,            //是否为非空项添加星号
	reqStarCls: "t_v_ReqStar"     // 必须输入项后面紧跟着一个星号的样式
};