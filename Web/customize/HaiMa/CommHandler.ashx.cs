using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.HaiMa
{
    /// <summary>
    /// 海马端公共处理文件 手机端
    /// </summary>
    public class Handler : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// 响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 海马业务逻辑
        /// </summary>
        BLLJIMP.BLLHaiMa bllHaiMa = new BLLJIMP.BLLHaiMa();
        /// <summary>
        /// 短信业务逻辑
        /// </summary>
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo CurrentUserInfo = new UserInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                if (bllHaiMa.IsLogin)
                {

                    CurrentUserInfo = bllHaiMa.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action不存在";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }
        outoff:
            context.Response.Write(result);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Reg(HttpContext context)
        {


            int regType = int.Parse(context.Request["RegType"]);//注册类型 2销售人员 3海马内部人员
            string phone = context.Request["Phone"];//手机号
            string smsVerCode = context.Request["VerCode"];//手机验证码
            string trueName = context.Request["TrueName"];


            string province = context.Request["Province"];//省份
            string storeName = context.Request["StoreName"];//门店名称
            string storeCode = context.Request["StoreCode"];//门店代码

            string position = context.Request["Position"];//岗位

            string ex1 = context.Request["Ex1"];//本部
            string ex2 = context.Request["Ex2"];//部门


            if (bllHaiMa.IsReg(CurrentUserInfo))
            {
                resp.errcode = 8;
                resp.errmsg = "已经注册过了";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if ((!Common.ValidatorHelper.PhoneNumLogicJudge(phone)))//验证手机号格式
            {
                resp.errcode = 1;
                resp.errmsg = "手机号码无效";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            var lastVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastVerCode == null)
            {
                resp.errcode = 2;
                resp.errmsg = "请先获取验证码";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(smsVerCode))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入手机验证码";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!smsVerCode.Equals(lastVerCode.VerificationCode))
            {
                resp.errcode = 4;
                resp.errmsg = "验证码不正确";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //检查门店
            if ((bllHaiMa.GetSingleStore(province, storeName, storeCode) == null) && (regType==2))
            {
                resp.errcode = 5;
                resp.errmsg = "店代码输入有误,请重新输入";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllUser.Get<UserInfo>(string.Format("Phone='{0}' And WebSiteOwner='{1}'", phone, bllUser.WebsiteOwner)) != null)
            {

                resp.errcode = 6;
                resp.errmsg = "此手机号已经被注册";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            
            CurrentUserInfo.Phone = phone;
            CurrentUserInfo.TrueName = trueName;
            CurrentUserInfo.Province = province;//省份
            CurrentUserInfo.Postion = position;
            CurrentUserInfo.UserType = regType;
            CurrentUserInfo.Ex1 = ex1;//本部
            CurrentUserInfo.Ex2 = ex2;//部门
            CurrentUserInfo.Ex3 = storeName;//销售店名称
            CurrentUserInfo.Ex4 = storeCode;//销售店代码

            string setPar=string.Format(" TrueName='{0}',Phone='{1}',Postion='{2}',UserType={3},Province='{4}',Ex1='{5}',Ex2='{6}',Ex3='{7}',Ex4='{8}'", CurrentUserInfo.TrueName, CurrentUserInfo.Phone, CurrentUserInfo.Postion, CurrentUserInfo.UserType, CurrentUserInfo.Province, CurrentUserInfo.Ex1, CurrentUserInfo.Ex2, CurrentUserInfo.Ex3, CurrentUserInfo.Ex4);

            if (bllUser.Update(CurrentUserInfo,setPar , string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) > 0)
            {
                resp.errmsg = "成功注册";
            }
            else
            {
                resp.errcode = 7;
                resp.errmsg = "注册失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSmsVercode(HttpContext context)
        {

            string phone = context.Request["Phone"];//手机号
            if ((!Common.ValidatorHelper.PhoneNumLogicJudge(phone)))//
            {
                resp.errcode = 1;
                resp.errmsg = "手机号码无效!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            var lastVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastVerCode != null)
            {
                if ((DateTime.Now - lastVerCode.InsertDate).TotalSeconds < 60)
                {
                    resp.errcode = 2;
                    resp.errmsg = "验证码限制每60秒发送一次";
                    return Common.JSONHelper.ObjectToJson(resp);

                }

            }
            bool isSuccess = false;
            string msg = "";
            string verCode = new Random().Next(111111, 999999).ToString();
            bllSms.SendSmsVerificationCode(phone, string.Format("您的验证码为{0}",verCode), "海马汽车",verCode, out isSuccess, out msg);
            if (isSuccess)
            {
                resp.errmsg = "验证码发送成功";
            }
            else
            {
                resp.errcode = 3;
                resp.errmsg = msg;
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 根据省份获取门店信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetStoreListByProvince(HttpContext context)
        {
            string province = context.Request["Province"];//省份
            var data = bllHaiMa.GetStoreList(province);
            return Common.JSONHelper.ObjectToJson(data);

        }


        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SignUp(HttpContext context)
        {
            string question1 = context.Request["Question1"];
            string question2 = context.Request["Question2"];
            string idCard = context.Request["IDCard"];
            string introduction = context.Request["Introduction"];//省份

            if (!CurrentUserInfo.UserType.Equals(2))
            {
                resp.errcode = 1;
                resp.errmsg = "只有销售店人员可以报名";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (CurrentUserInfo.Postion.Equals("其它"))
            {
                resp.errcode = 3;
                resp.errmsg = "您注册的岗位是其它,不可以报名";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(CurrentUserInfo.Ex5))
            {
                resp.errcode = 2;
                resp.errmsg = "您已经报过名了";
                return Common.JSONHelper.ObjectToJson(resp);

            }


            CurrentUserInfo.Ex5 = question1 ;
            CurrentUserInfo.Ex6 = question2;
            CurrentUserInfo.Ex7 = introduction;
            CurrentUserInfo.Ex8 = idCard;
            string setPar = string.Format(" Ex5='{0}',Ex6='{1}',Ex7='{2}',Ex8='{3}'", CurrentUserInfo.Ex5, CurrentUserInfo.Ex6, CurrentUserInfo.Ex7, CurrentUserInfo.Ex8);
            if (bllUser.Update(CurrentUserInfo, setPar, string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) > 0)
            {
                resp.errmsg = "报名成功";
            }
            else
            {
                resp.errcode = 3;
                resp.errmsg = "报名失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}