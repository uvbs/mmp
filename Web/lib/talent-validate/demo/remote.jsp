<!DOCTYPE html>
<html>
<%@ page contentType="text/html;charset=UTF-8"%>
<head>
<title>validate验证框架演示</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta http-equiv="Page-Enter"
	content="revealTrans(duration=1, transition=2)" />
<meta http-equiv="Page-Exit"
	content="revealTrans(duration=1, transition=2)" />
<script src="./jquery-1.6.4.js" language="javascript"></script>
<link type="text/css" rel="stylesheet"
	href="../js/validate/css/validate.css" />
<script src="../js/validate/talent-validate-all.js"
	language="javascript"></script>
<script src="./demo.js" language="javascript"></script>

<link type="text/css" rel="stylesheet" href="../Styles/SyntaxHighlighter.css" />
	<link type="text/css" rel="stylesheet" href="./demo.css" />
	<script src="../Scripts/shCore.js" type="text/javascript"></script>
	<script src="../Scripts/shBrushJScript.js" type="text/javascript"></script>
	<script src="../Scripts/shBrushJava.js" type="text/javascript"></script>
<!-- 第一步 -->

</head>
<body>
	<form name="form1" id='form1'>
		用户名和密码都是talent时通过，否则不通过<span style='color: red; font-size: 14pt'>(本页面需要服务器支持)</span>
		<br /> <br /> <span id='usernameId'>用户名</span><input type="text"
			value="talent" id='username' name="username" /> <br /> 密码<input
			type="text" value="2" id='password' name="password" /> <br /> <input
			type="button" class='button' value="验证" onclick="tt.validate();" />
		<input type="button" class='button' value="取消验证"
			onclick="remoteV.rm();" /> <input type="button" class='button'
			value="添加验证" onclick="remoteV.add();" />

		<div id="passwordMsgid"></div>
	</form>

	<pre name='code' class='java'>
	/**
	  * 服务端代码，输出格式形如：
	  {
		  'username':{'result':true,'msg':'用户[talent]存在!'},
		  'password':{'result':false,'msg':'密码不正确!'}
	  }
	  **/
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String name = req.getParameter("username");
		String pwd = req.getParameter("password");
		
		resp.setCharacterEncoding("utf-8");
		PrintWriter out = resp.getWriter();
		Map&lt;String, TalentValidateResult> resultMap = new HashMap&lt;String, TalentValidateResult>();
		if ("talent".equals(name) ) {
			resultMap.put("username", new TalentValidateResult(true, "用户["+name+"]存在!"));
			if ("talent".equals(pwd)) {
				resultMap.put("password", new TalentValidateResult(true, "密码正确!"));
			} else {
				resultMap.put("password", new TalentValidateResult(false, "密码不正确!(在指定的位置显示信息)", "passwordMsgid"));
			}
		} else {
			/**
			 * "id:usernameId"：在id指定的位置提示信息
			 */
			resultMap.put("id:usernameId", new TalentValidateResult(false, "用户["+name+"]不存在!(和指定的id绑定)"));
		}
		out.print(JSON.toJSONString(resultMap));
	}
	</pre>
</body>

<!-- 第二步 -->
<script id="talent_script">
			tt.vf.req.add("username");
			/**
			 * 自带了一个ajax框架
			 */
			var ajaxConfig = {
				formId: 'form1',      //要提交的form
				url : "<%=request.getContextPath()%>/RemoteValidatorDemo"
			};
			
			var remoteV = new tt.RemoteV().set(ajaxConfig).add();       //服务端验证勿需调用add()方法；取消验证：remoteV.rm(); 添加验证：remoteV.add();
		</script>
</html>
