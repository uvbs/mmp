using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.MemberTag
{
    /// <summary>
    /// Update 的摘要说明  修改会员标签接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        /// <param name="context"></param>
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.tag_id<=0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "tag_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.tag_name))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "tag_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.Model.MemberTag model = new BLLJIMP.Model.MemberTag();
            model.AutoId = requestModel.tag_id;
            model.AccessLevel = requestModel.access_level;
            model.TagName = requestModel.tag_name;
            if (bllTag.Update(model))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "修改会员标签出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


        /// <summary>
        /// 会员标签
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 标签id
            /// </summary>
            public int tag_id { get; set; }

            /// <summary>
            /// 标签名称
            /// </summary>
            public string tag_name { get; set; }

            /// <summary>
            /// 访问等级
            /// </summary>
            public int access_level { get; set; }
        }
    }
}