/**
 * 
 */
package tt;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.HashMap;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.alibaba.fastjson.JSON;

/**
 * @author Administrator
 *
 */
public class RemoteValidatorDemo extends HttpServlet {

	/**
	 * 
	 */
	public RemoteValidatorDemo() {
	}
	
	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		doPost(req, resp);
	}
	
	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		String name = req.getParameter("username");
		String pwd = req.getParameter("password");
		
		resp.setCharacterEncoding("utf-8");
		PrintWriter out = resp.getWriter();
		Map<String, TalentValidateResult> resultMap = new HashMap<String, TalentValidateResult>();
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
		System.out.println(JSON.toJSONString(resultMap));
		out.print(JSON.toJSONString(resultMap));
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub

	}

}
