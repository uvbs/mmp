using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.Project
{
    /// <summary>
    ///提交项目
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                ZentCloud.BLLJIMP.Model.Project model = bll.ConvertRequestToModel<ZentCloud.BLLJIMP.Model.Project>(new ZentCloud.BLLJIMP.Model.Project());
                model.UserId = CurrentUserInfo.UserID;
                model.WebsiteOwner = bll.WebsiteOwner;
                model.InsertDate = DateTime.Now;
                model.Status = "待审核";
                model.ProjectId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.ProjectType = "DistributionOffLine";

                string msg = "";
                if (!bll.CheckProjectInfo(model,out msg))
                {
                    apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    apiResp.msg = msg;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (bll.Add(model))
                {
                    apiResp.status = true;
                    apiResp.msg = "ok";
                    ZentCloud.BLLJIMP.Model.ProjectLog log = new ZentCloud.BLLJIMP.Model.ProjectLog();
                    log.InsertDate = DateTime.Now;
                    log.ProjectId = model.ProjectId;
                    log.ProjectName = model.ProjectName;
                    log.Remark = string.Format("提交了项目{0} 操作人:{1}", model.ProjectName,bllUser.GetUserDispalyName( CurrentUserInfo));
                    log.Status = model.Status = model.Status;
                    log.UserId = CurrentUserInfo.UserID;
                    log.WebsiteOwner = bll.WebsiteOwner;
                    bll.Add(log);

                    #region 给管理员发送消息
                    var currWebSiteUserInfo = bll.GetCurrWebSiteUserInfo();
                    if (!string.IsNullOrEmpty(currWebSiteUserInfo.WeiXinKeFuOpenId))
                    {
                        bllWeixin.SendTemplateMessageNotifyComm(currWebSiteUserInfo, string.Format("提交项目申请"), string.Format("用户:{0}\\n提交了项目{1}", CurrentUserInfo.TrueName, model.ProjectName));
                    } 
                    #endregion
                    
                }
                else
                {
                    apiResp.msg = "提交失败";
                }


            }
            catch (Exception ex)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = ex.Message;

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}