using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// AdInfoHander 的摘要说明
    /// </summary>
    public class AdInfoHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        UserInfo currentUserInfo;
        BLLAdInfo bllAdInfo = new BLLAdInfo();
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = (int)APIErrCode.UserIsNotLogin;
                    resp.Msg = "用户未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string Action = context.Request["Action"];
                switch (Action)
                {
                    case "QueryList":
                        result = QueryList(context);
                        break;
                    case "AddAdInfo":
                        result = AddAdInfo(context);
                        break;
                    case "UpdateAdInfo":
                        result = UpdateAdInfo(context);
                        break;
                    case "DeleteAdInfo":
                        result = DeleteAdInfo(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }
            context.Response.Write(result);
        }

        #region 用户标签管理
        /// <summary>
        /// 设计标签
        /// </summary>
        ///  #region 用户标签管理
        /// <summary>
        /// 查询用户标签管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            int adType = int.Parse(context.Request["Type"]);
            int totalCount = 0;
            List<ZentCloud.BLLJIMP.Model.AdInfo> dataList = bllAdInfo.GetAdInfoList(pageSize, pageIndex, adType,bllAdInfo.WebsiteOwner, out totalCount);
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddAdInfo(HttpContext context)
        {
            ZentCloud.BLLJIMP.Model.AdInfo model = new BLLJIMP.Model.AdInfo();
            model = bllAdInfo.ConvertRequestToModel<ZentCloud.BLLJIMP.Model.AdInfo>(model);
            model.AutoId = 0;
            model.CreateDate = DateTime.Now;
            model.UserId = currentUserInfo.UserID;
            model.WebsiteOwner = bllAdInfo.WebsiteOwner;
            if (bllAdInfo.PutAdInfo(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateAdInfo(HttpContext context)
        {
            int autoId = int.Parse( context.Request["AutoId"]);
            ZentCloud.BLLJIMP.Model.AdInfo model = bllAdInfo.GetAdInfo(autoId);
            model = bllAdInfo.ConvertRequestToModel<ZentCloud.BLLJIMP.Model.AdInfo>(model);
            if (bllAdInfo.PutAdInfo(model))
            {
                resp.Status = 1;
                resp.Msg="修改成功";
            }
            else
            {
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteAdInfo(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (bllAdInfo.DeleteAdInfos(ids))
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}