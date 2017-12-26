using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.MemberTag
{
    /// <summary>
    /// Add 的摘要说明  添加会员标签接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
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
                resp.errcode=(int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "json格式错误,请检查";
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
            model.AccessLevel = requestModel.access_level;
            model.TagName = requestModel.tag_name;
            model.WebsiteOwner = bllTag.WebsiteOwner;
            model.CreateTime = DateTime.Now;
            model.Creator = bllTag.GetCurrUserID();
            model.TagType = "Member";
            if (bllTag.AddTag(model))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "添加会员标签出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel 
        {
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