using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model.Weixin;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// 宽桥处理文件
    /// </summary>
    public class KuanqiaoHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {

                this.currentUserInfo = bllUser.GetCurrentUserInfo();
                string action = context.Request["Action"];
                switch (action)
                {


                    case "QuerySignUpData":
                        result = QuerySignUpData(context);
                        break;
                    case "UpdateApplyStatus"://修改申请结果
                        result = UpdateApplyStatus(context);
                        break;
                    case "UpdateApplyResult"://修改
                        result = UpdateApplyResult(context);
                        break;

                    case "DeleteSignUpData"://删除
                        result = DeleteSignUpData(context);
                        break;

                    case "GetApplyImageList":
                        result = GetApplyImageList(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = "异常:" + ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }

            context.Response.Write(result);
        }

        /// <summary>
        /// 查询企业核名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string QuerySignUpData(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string applyStatus = context.Request["ApplyStatus"];
            string processStatus = context.Request["ProcessStatus"];
            string companyName = context.Request["companyName"];
            StringBuilder sbWhere = new StringBuilder("ActivityID='130725'");
            if (!string.IsNullOrEmpty(companyName))
            {
                sbWhere.AppendLine(string.Format("And K2 like '%{0}%'", companyName));
            }
            if (!string.IsNullOrEmpty(applyStatus))
            {

                if (applyStatus.Equals("审核成功,审核失败"))
                {
                    sbWhere.AppendLine("And K15='审核通过' Or K15='审核失败'");
                }
                else
                {
                    sbWhere.AppendLine(string.Format("And K15='{0}'", applyStatus));
                }

            }
            if (!string.IsNullOrEmpty(processStatus))
            {

                sbWhere.AppendLine(string.Format("And K16='{0}'", processStatus));
            }
            int totalCount = this.bllJuactivity.GetCount<ActivityDataInfo>(sbWhere.ToString());
            List<ActivityDataInfo> dataList = this.bllJuactivity.GetLit<ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");

            return Common.JSONHelper.ObjectToJson(new { 
            total=totalCount,
            rows=dataList
            
            });
           
        }

        /// <summary>
        ///更新企业核名申请结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateApplyStatus(HttpContext context)
        {
            string uid = context.Request["UID"];
            string applyStauts = context.Request["ApplyStauts"];
            string failReason = context.Request["FailReason"];
            int count = bllJuactivity.Update(new ActivityDataInfo(), string.Format("K15='{0}',K17='{1}'", applyStauts, failReason), string.Format("ActivityID='130725' And UID='{0}'", uid));
            if (count >= 1)
            {
                resp.Status = 1;
                resp.Msg = "修改申请结果成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改失败，请重试!";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        ///更新企业核名处理结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateApplyResult(HttpContext context)
        {
            string uid = context.Request["UID"];
            string result = context.Request["Result"];
            int count = bllJuactivity.Update(new ActivityDataInfo(), string.Format("K16='{0}'", result), string.Format("ActivityID='130725' And UID='{0}'", uid));
            if (count >= 1)
            {
                resp.Status = 1;
                resp.Msg = "修改状态成功!";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "修改状态失败!";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除核名信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DeleteSignUpData(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = bllJuactivity.Delete(new ActivityDataInfo(), string.Format("ActivityID='130725' And UID in({0})", ids));

            resp.Status = 1;
            resp.Msg = string.Format("删除了{0}条数据", count);

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取用户向公众平台发送的图片列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetApplyImageList(HttpContext context)
        {
            var openId = context.Request["oid"];
            var data = bllJuactivity.GetList<WXFileReceive>(string.Format(" FileType='image' And WeixinOpenID='{0}' And FilePath!='' And WebsiteOwner='{1}'", openId, bllUser.WebsiteOwner));
            //if (data.Count > 0)
            //{
                return Common.JSONHelper.ObjectToJson(data);
                //return string.Format("[{0}]", ZentCloud.Common.JSONHelper.ListToJson<WXFileReceive>(data, ","));

            //}
            //return "";
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