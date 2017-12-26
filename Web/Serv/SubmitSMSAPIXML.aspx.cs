using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.Serv
{
    public partial class SubmitSMSAPIXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string userName = "", userPwd = "", mobile = "", content = "", pipeID = "", attime = "";

                userName = this.GetPostParm("userName");
                userPwd = this.GetPostParm("userPwd");
                mobile = this.GetPostParm("mobile");
                content = this.GetPostParm("content");
                pipeID = this.GetPostParm("pipeID");
                attime = this.GetPostParm("attime");

                int result = 0;

                if(attime != null)
                    result = SendSms(userName, userPwd, mobile, HttpUtility.UrlDecode(content, Encoding.GetEncoding("gb2312")), pipeID,attime);
                else
                    result = SendSms(userName, userPwd, mobile, HttpUtility.UrlDecode(content, Encoding.GetEncoding("gb2312")), pipeID);

                if (result.Equals(0))
                {
                    this.ReturnResult(result, "提交发送成功");
                }
                else
                {
                    this.ReturnResult(-1, "提交发送失败：" + result.ToString());
                }
            }
            catch (Exception ex)
            {
                this.ReturnResult(-1, ex.Message);
            }
        }

        private string GetPostParm(string parm)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parm))
                    parm = Request[parm];
            }
            catch { }
            return parm;
        }

        private void ReturnResult(int code, string description)
        {
            Response.Write(string.Format("<?xml version=\"1.0\" encoding=\"gb2312\"?><root><Result>{0}</Result><Descript>{1}</Descript></root>", code.ToString(), description));
        }

        public int SendSms(string userName, string userPwd, string mobile, string content, string pipeID)
        {
            return SendSms(userName, userPwd, mobile, content, pipeID, null);
        }

        public int SendSms(string userName, string userPwd, string mobile, string content, string pipeID, string attime)
        {
            try
            {
                //用户名密码不能为空
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userPwd))
                    return (int)BLLJIMP.ReturnCode.SMS_LoginError;

                BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser(userName);

                //用户登录
                if (!userBll.Login(userName, userPwd))
                    return (int)BLLJIMP.ReturnCode.SMS_LoginError;

                //通道不能为空
                if (string.IsNullOrWhiteSpace(pipeID))
                    return (int)BLLJIMP.ReturnCode.SMS_PipeError;

                //手机号码不能为空
                if (string.IsNullOrWhiteSpace(mobile))
                    return (int)BLLJIMP.ReturnCode.SMS_MobileEmpty;

                //发送内容不能为空
                if (string.IsNullOrWhiteSpace(content))
                    return (int)BLLJIMP.ReturnCode.SMS_ContentEmpty;

                pipeID = pipeID.ToLower().Trim();

                BLLJIMP.BLLSMS smsBll = new BLLJIMP.BLLSMS(userName);

                DateTime planTime = DateTime.Now;
                if (attime != null)
                {
                    if (!DateTime.TryParse(attime, out planTime))
                    {
                        return (int)BLLJIMP.ReturnCode.SMS_AddSMSPlanTimeError;
                    }
                    else
                    {
                        return (int)smsBll.SubmitSMSMission(pipeID, mobile, content, userName, planTime);
                    }
                }
                else
                    return (int)smsBll.SubmitSMSMission(pipeID, mobile, content, userName);

            }
            catch
            {
                //SMS_Exception
                return (int)BLLJIMP.ReturnCode.SMS_Exception;
            }
        }

    }
}