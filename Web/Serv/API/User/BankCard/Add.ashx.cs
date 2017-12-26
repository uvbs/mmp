using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.BankCard
{
    /// <summary>
    /// Add 的摘要说明 绑定银行卡
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #region 验证
            if (string.IsNullOrEmpty(requestModel.bank_card_name))
            {
                resp.errmsg = "请输入您的姓名";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.identity_card))
            {
                resp.errmsg = "请输入身份证号";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.bank_card_number))
            {
                resp.errmsg = "请输入银行卡卡号";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.bank_name))
            {
                resp.errmsg = "请输入银行名称";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.phone))
            {
                resp.errmsg = "请输入预留手机号";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.ex1))
            {
                resp.errmsg = "请输入交易密码";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //if (string.IsNullOrEmpty(requestModel.trade_pwd))
            //{
            //    resp.errmsg = "请输入交易密码";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //if (string.IsNullOrEmpty(requestModel.config_pwd))
            //{
            //    resp.errmsg = "请再次输入交易密码";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            if ((!Regex.IsMatch(requestModel.identity_card, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase)))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                resp.errmsg = "身份证号格式错误";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (!ZentCloud.Common.ValidatorHelper.PhoneNumLogicJudge(requestModel.phone))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                resp.errmsg = "手机号码格式出错";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //if (requestModel.trade_pwd.Length<8||requestModel.config_pwd.Length<8)
            //{
            //    resp.errmsg = "密码不能少于8位";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //if (requestModel.trade_pwd != requestModel.config_pwd)
            //{
            //    resp.errmsg = "两次输入密码不一致";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //if (CurrentUserInfo.Password == requestModel.trade_pwd)
            //{
            //    resp.errmsg = "不能和登录密码一致";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            if (CurrentUserInfo.Ex1 != requestModel.ex1)
            {
                resp.errmsg = "交易密码错误";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.ModelGen.BankCard model = bllUser.Get<BLLJIMP.ModelGen.BankCard>(string.Format(" WebsiteOwner='{0}' AND BankCardNumber='{1}'", bllUser.WebsiteOwner, requestModel.bank_card_number));
            if (model != null)
            {
                resp.errmsg = "卡号已经添加";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            #endregion
            BLLJIMP.ModelGen.BankCard bankModel = new BLLJIMP.ModelGen.BankCard();
            bankModel.BankCardID = int.Parse(bllUser.GetGUID(BLLJIMP.TransacType.CommAdd));
            bankModel.WebsiteOwner = bllUser.WebsiteOwner;
            bankModel.BankName = requestModel.bank_name;
            bankModel.UserID = CurrentUserInfo.UserID;
            bankModel.BankCardName = requestModel.bank_card_name;
            bankModel.BankCardNumber = requestModel.bank_card_number;
            bankModel.IdentityCard = requestModel.identity_card;
            bankModel.Phone = requestModel.phone;
            bankModel.CreateDate = DateTime.Now;
            if (bllUser.Add(bankModel))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "操作失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        public class RequestModel
        {

            /// <summary>
            /// 银行名称
            /// </summary>
            public string bank_name { get; set; }
            /// <summary>
            /// 银行卡姓名
            /// </summary>
            public string bank_card_name { get; set; }

            /// <summary>
            /// 身份证号码
            /// </summary>
            public string identity_card { get; set; }

            /// <summary>
            /// 银行卡号码
            /// </summary>
            public string bank_card_number { get; set; }

            /// <summary>
            /// 手机号码
            /// </summary>
            public string phone { get; set; }

            /// <summary>
            /// 交易密码
            /// </summary>
            public string ex1 { get; set; }


        }
    }
}