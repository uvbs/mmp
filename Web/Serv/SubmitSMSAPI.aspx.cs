using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv
{
    public partial class SubmitSMSAPI : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            string userName = "", userPwd = "", mobile = "", content = "", pipeID = "", attime = "";

            userName = this.GetPostParm("userName");
            userPwd = this.GetPostParm("userPwd");
            mobile = this.GetPostParm("mobile");
            content = this.GetPostParm("content");
            pipeID = this.GetPostParm("pipeID");
            attime = this.GetPostParm("attime");

            Response.Write(SendSms(userName, userPwd, mobile, HttpUtility.UrlDecode(content, Encoding.GetEncoding("utf-8")), pipeID));
            //Response.Write();
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

        public int SendSms(string userName, string userPwd, string mobile, string content, string pipeID, string attime = "")
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

                if (pipeID.Equals("membermission"))
                {
                    return (int)smsBll.SubmitSMSMission(pipeID, mobile, content, userName);
                }
                else
                {
                    return (int)smsBll.SubmitSMS(pipeID, mobile, content);
                }

            }
            catch
            {
                //SMS_Exception
                return (int)BLLJIMP.ReturnCode.SMS_Exception;
            }
        }

    }
}