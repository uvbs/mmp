using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// SmsServer 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SmsServer : System.Web.Services.WebService
    {
        [WebMethod]
        public string Hi(string str)
        {
            return str + "123";
        }

        [WebMethod]
        public int SendSms(string userName, string userPwd, string mobile, string content ,string pipeID)
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


                ////判断通余额是否充足
                //int smsCntLeg = content.Length;//内容长度
                //int smsCntCount = 1;//内容拆分条数
                //int smsCount = 1;//短信扣点数
                //int userPoints = userBll.GetPoints();

                //if (smsCntLeg > 65)
                //{
                //    smsCntCount = (int)Math.Ceiling((double)smsCntLeg / 65);
                //}

                //smsCount = smsCount * smsCntCount;


                //if( userPoints.Equals(0) || userPoints < smsCount)
                //    return (int)BLLJIMP.ReturnCode.SMS_PointNotEnough;

                if (pipeID.Equals("membermission"))
                {
                    return (int)smsBll.SubmitSMSMission("membermission", mobile, content, userName);
                }
                else
                {
                    return (int)smsBll.SubmitSMS(pipeID, mobile, content);
                }
                //switch (channel)
                //{
                //    case "membertrigger"://触发通道
                        
                        

                //        break;
                //    case "mission"://任务发送


                //        break;
                //    default:
                //        return (int)BLLJIMP.ReturnCode.SMS_PipeError;
                //}

            }
            catch
            {
                //SMS_Exception
                return (int)BLLJIMP.ReturnCode.SMS_Exception;
            }
        }

        

    }
}
