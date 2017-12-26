/**
 * 
 */
package tt;

/**
 * 验证结果
 * @author tanyaowu
 *
 */
public class TalentValidateResult {
	private boolean result = false;
	private String msg = null;
	private String msgId = null;

	/**
	 * 
	 */
	public TalentValidateResult() {
	}

	public TalentValidateResult(boolean result, String msg) {
		this.result = result;
		this.msg = msg;
	}
	public TalentValidateResult(boolean result, String msg, String msgId) {
		this.result = result;
		this.msg = msg;
		this.msgId = msgId;
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {

	}

	public void setResult(boolean result) {
		this.result = result;
	}

	public boolean isResult() {
		return result;
	}

	public void setMsg(String msg) {
		this.msg = msg;
	}

	public String getMsg() {
		return msg;
	}

	public void setMsgId(String msgId) {
		this.msgId = msgId;
	}

	public String getMsgId() {
		return msgId;
	}

}
