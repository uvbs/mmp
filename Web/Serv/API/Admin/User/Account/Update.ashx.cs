using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// Update 的摘要说明   编辑子账户
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
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
            #region 检查
            if (requestModel.autoid <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "autoid 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.user_pwd))
            {
                resp.errmsg = "user_pwd 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion
            UserInfo userInfo = this.bllUser.GetUserInfoByAutoID(requestModel.autoid);
            if (userInfo == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "不存在此子账户";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            userInfo.TrueName = requestModel.true_name;
            userInfo.Password = requestModel.user_pwd;
            userInfo.Company = requestModel.user_company;
            userInfo.Phone = requestModel.user_phone;
            userInfo.Postion = requestModel.user_postion;
            userInfo.WXHeadimgurl = requestModel.wx_head_img_url;
            userInfo.VoteCount = requestModel.user_vote_count;
            if (bllUser.Update(userInfo, string.Format(" TrueName='{0}',Company='{1}',Phone='{2}',Postion='{3}',VoteCount='{4}',WXHeadimgurl='{5}',Password='{6}'", userInfo.TrueName, userInfo.Company, userInfo.Phone, userInfo.Postion, userInfo.VoteCount, userInfo.WXHeadimgurl,userInfo.Password), string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "编辑子账户出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }
        public class RequestModel
        {
            /// <summary>
            /// 用户id
            /// </summary>
            public int autoid { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string user_pwd { get; set; }

            /// <summary>
            /// 公司
            /// </summary>
            public string user_company { get; set; }

            /// <summary>
            /// 职位
            /// </summary>
            public string user_postion { get; set; }

            /// <summary>
            /// 头像
            /// </summary>
            public string wx_head_img_url { get; set; }

            /// <summary>
            /// 手机
            /// </summary>
            public string user_phone { get; set; }

            /// <summary>
            /// 真实姓名
            /// </summary>
            public string true_name { get; set; }

            /// <summary>
            /// 票数
            /// </summary>
            public int user_vote_count { get; set; }

            ///// <summary>
            ///// 是否允许删除
            ///// </summary>
            //public string isdelete { get; set; }


            ///// <summary>
            ///// 是否允许导出
            ///// </summary>
            //public string isexport { get; set; }
        }
    }
}