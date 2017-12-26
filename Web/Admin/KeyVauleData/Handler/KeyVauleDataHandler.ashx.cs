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
    /// KeyVauleDataHandler 的摘要说明
    /// </summary>
    public class KeyVauleDataHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();

        UserInfo currentUserInfo;
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

                string action = context.Request["Action"];
                switch (action)
                {
                    case "Query":
                        result = Query(context);
                        break;
                    case "PutKeyValue":
                        result = PutKeyValue(context);
                        break;
                    case "DeleteKeyValue":
                        result = DeleteKeyValue(context);
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
        #region 数据管理
        /// <summary>
        /// 查询数据管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Query(HttpContext context)
        {
            string type = context.Request["type"];
            string websiteowner = context.Request["websiteowner"];
            string preKey = context.Request["preKey"];
            if (string.IsNullOrWhiteSpace(websiteowner)) websiteowner = bllKeyValueData.WebsiteOwner;
            List<KeyVauleDataInfo> dataList = bllKeyValueData.GetKeyVauleDataInfoList(type, preKey, websiteowner);
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = dataList.Count,
                    rows = dataList
                });
        }

        /// <summary>
        /// 提交数据字典
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string PutKeyValue(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string key = context.Request["key"];
            string dataName = context.Request["dataName"];
            string type = context.Request["type"];
            string preKey = context.Request["preKey"];
            string value = context.Request["value"];
            string websiteowner = context.Request["websiteowner"];

            KeyVauleDataInfo keyValue = new KeyVauleDataInfo();
            bool isAdd = false;
            if (string.IsNullOrWhiteSpace(autoId) || autoId == "0")
            {
                if (string.IsNullOrWhiteSpace(websiteowner))
                {
                    keyValue.WebsiteOwner = bllKeyValueData.WebsiteOwner;
                }
                else
                {
                    keyValue.WebsiteOwner = websiteowner;
                }
                keyValue.Creater = currentUserInfo.UserID;
                keyValue.CreateTime = DateTime.Now;
                isAdd = true;
                if (string.IsNullOrWhiteSpace(key)) keyValue.DataKey = Guid.NewGuid().ToString("N").ToUpper();
            }
            else
            {
                keyValue = bllKeyValueData.GetByKey<KeyVauleDataInfo>("AutoId", autoId);
                if (!string.IsNullOrWhiteSpace(key)) keyValue.DataKey = key;
            }

            keyValue.DataName = dataName;
            keyValue.DataType = type;
            keyValue.DataValue = value;
            if (!string.IsNullOrWhiteSpace(preKey)) keyValue.PreKey = preKey;
            bool r = false;

            if (isAdd)
            {
                r = bllKeyValueData.Add(keyValue);
            }
            else
            {
                r = bllKeyValueData.Update(keyValue);
            }
            if (r)
            {
                resp.Status = 1;
                if (keyValue.DataType == "WeixinKindeditor") Comm.StaticData.InitKeyValueData();
                resp.Msg = "提交完成";
            }
            else
            {
                resp.Msg = "提交失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteKeyValue(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllKeyValueData.DeleteDataVaule(ids))
            {
                resp.Status = 1;
                resp.Msg = "删除完成";
            }
            else
            {
                resp.Msg = "删除失败";
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