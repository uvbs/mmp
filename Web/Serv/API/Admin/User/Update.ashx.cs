using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary> 
    /// 修改会员信息
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
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

                resp.errcode=(int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.autoid <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "autoid 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserInfo userInfo = bllUser.GetUserInfoByAutoID(requestModel.autoid,bllUser.WebsiteOwner);
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该条会员信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            System.Text.StringBuilder sbPar = new System.Text.StringBuilder();
            
            if (!string.IsNullOrEmpty(requestModel.wx_nick_name))
            {
                sbPar.AppendFormat(" WXNickname='{0}',", requestModel.wx_nick_name);
            }
            if (!string.IsNullOrEmpty(requestModel.true_name))
            {
                sbPar.AppendFormat(" TrueName='{0}',", requestModel.true_name);
            }
            if (!string.IsNullOrEmpty(requestModel.user_company))
            {
                sbPar.AppendFormat(" Company='{0}',", requestModel.user_company);
            }
            if (!string.IsNullOrEmpty(requestModel.user_position))
            {
                sbPar.AppendFormat(" Postion='{0}',", requestModel.user_position);
            }
            if (!string.IsNullOrEmpty(requestModel.user_phone))
            {
                sbPar.AppendFormat(" Phone='{0}',", requestModel.user_phone);
            }
            if (!string.IsNullOrEmpty(requestModel.user_email))
            {
                sbPar.AppendFormat(" Email='{0}',", requestModel.user_email);
            }
            if (requestModel.available_vote_count<=0)
            {
                sbPar.AppendFormat(" AvailableVoteCount={0} ", requestModel.available_vote_count);
            }
            if (bllUser.Update(userInfo,sbPar.ToString(),string.Format(" AutoID={0} ",requestModel.autoid))>0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "修改会员数据出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel 
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public int autoid { get; set; }

            /// <summary>
            /// 微信名称
            /// </summary>
            public string wx_nick_name { get; set; }

            /// <summary>
            /// 真是姓名
            /// </summary>
            public string true_name { get; set; }

            /// <summary>
            /// 手机号码
            /// </summary>
            public string user_phone { get; set; }

            /// <summary>
            /// 公司
            /// </summary>
            public string user_company { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            public string user_email { get; set; }

            /// <summary>
            /// 职位
            /// </summary>
            public string user_position { get; set; }

            /// <summary>
            /// 票数
            /// </summary>
            public int available_vote_count { get; set; }





        }
    }
}