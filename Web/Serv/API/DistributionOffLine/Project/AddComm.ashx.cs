using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.Project
{
    /// <summary>
    /// Summary description for AddComm
    /// </summary>
    public class AddComm : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BLLJIMP.Model.Project model = bll.ConvertRequestToModel<BLLJIMP.Model.Project>(new BLLJIMP.Model.Project());
                model.UserId = CurrentUserInfo.UserID;
                model.WebsiteOwner = bll.WebsiteOwner;
                model.InsertDate = DateTime.Now;
                model.Status = "待审核";
                model.ProjectId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));

                string msg = "";

                if (bll.Add(model))
                {
                    apiResp.status = true;
                    apiResp.msg = "ok";
                    BLLJIMP.Model.ProjectLog log = new BLLJIMP.Model.ProjectLog();
                    log.InsertDate = DateTime.Now;
                    log.ProjectId = model.ProjectId;
                    log.ProjectName = model.ProjectName;
                    log.Remark = string.Format("提交了 {0} 操作人:{1}", model.ProjectName, bllUser.GetUserDispalyName(CurrentUserInfo));
                    log.Status = model.Status = model.Status;
                    log.UserId = CurrentUserInfo.UserID;
                    log.WebsiteOwner = bll.WebsiteOwner;
                    bll.Add(log);

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

            context.Response.Write(JsonConvert.SerializeObject(apiResp));

        }
    }
}