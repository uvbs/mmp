using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Policy
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJuActivity bllJuActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            Add.PostModel requestModel = new Add.PostModel();//模型
            List<Add.FileModel> nFiles = new List<Add.FileModel>();
            try
            {
                requestModel = bllJuActivity.ConvertRequestToModel<Add.PostModel>(requestModel);
                nFiles = ZentCloud.Common.JSONHelper.JsonToModel<List<Add.FileModel>>(requestModel.file_list);
            }
            catch (Exception ex)
            {
                apiResp.msg = "提交格式错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            //数据检查
            if (string.IsNullOrEmpty(requestModel.policy_name))
            {
                apiResp.msg = "政策名称必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            JuActivityInfo pInfo = bllJuActivity.GetByKey<JuActivityInfo>("JuActivityID", requestModel.id.ToString());
            if (pInfo == null)
            {
                apiResp.msg = "原记录不存在";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            pInfo.ActivityName = requestModel.policy_name;
            pInfo.Summary = requestModel.summary;
            pInfo.K2 = requestModel.policy_object;
            pInfo.K3 = requestModel.subsidy_standard;
            pInfo.K4 = requestModel.subsidy_period;
            pInfo.K5 = requestModel.policy_number;
            pInfo.K6 = requestModel.policy_level;
            pInfo.K7 = requestModel.domicile_place;
            pInfo.K8 = requestModel.sex;
            pInfo.K9 = string.IsNullOrWhiteSpace(requestModel.male_age_min) ? null : requestModel.male_age_min;
            pInfo.K10 = string.IsNullOrWhiteSpace(requestModel.male_age_max) ? null : requestModel.male_age_max;
            pInfo.K11 = string.IsNullOrWhiteSpace(requestModel.famale_age_min) ? null : requestModel.famale_age_min;
            pInfo.K12 = string.IsNullOrWhiteSpace(requestModel.famale_age_max) ? null : requestModel.famale_age_max;
            pInfo.K13 = requestModel.education;
            pInfo.K14 = string.IsNullOrWhiteSpace(requestModel.graduation_year_min) ? null : requestModel.graduation_year_min;
            pInfo.K15 = string.IsNullOrWhiteSpace(requestModel.graduation_year_max) ? null : requestModel.graduation_year_max; 
            pInfo.K16 = requestModel.employment_status;
            pInfo.K17 = string.IsNullOrWhiteSpace(requestModel.current_job_life_min) ? null : requestModel.current_job_life_min; 
            pInfo.K18 = string.IsNullOrWhiteSpace(requestModel.current_job_life_max) ? null : requestModel.current_job_life_max;
            pInfo.K19 = string.IsNullOrWhiteSpace(requestModel.unemployment_period_min) ? null : requestModel.unemployment_period_min; 
            pInfo.K20 = string.IsNullOrWhiteSpace(requestModel.unemployment_period_max) ? null : requestModel.unemployment_period_max; 
            pInfo.K21 = requestModel.company_type;
            pInfo.K22 = string.IsNullOrWhiteSpace(requestModel.registered_capital_min) ? null : requestModel.registered_capital_min; 
            pInfo.K23 = string.IsNullOrWhiteSpace(requestModel.registered_capital_max) ? null : requestModel.registered_capital_max; 
            pInfo.K24 = string.IsNullOrWhiteSpace(requestModel.personnel_size_min) ? null : requestModel.personnel_size_min; 
            pInfo.K25 = string.IsNullOrWhiteSpace(requestModel.personnel_size_max) ? null : requestModel.personnel_size_max; 
            pInfo.K26 = requestModel.company_size;
            pInfo.K27 = requestModel.industry;
            pInfo.Sort = requestModel.sort;
            pInfo.LastUpdateDate = DateTime.Now;


            List<JuActivityFiles> files = new List<JuActivityFiles>();
            foreach (var item in nFiles.Where(p=>p.id==0))
            {
                files.Add(new JuActivityFiles()
                {
                    AddDate = DateTime.Now,
                    FileClass = item.file_class,
                    JuActivityID = pInfo.JuActivityID,
                    FileName = item.file_name,
                    FilePath = item.file_path,
                    UserID = currentUserInfo.UserID
                });
            }

            bool result = false;
            BLLTransaction tran = new BLLTransaction();
            result = bllJuActivity.Update(pInfo, tran);
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            string noDeleteFileIds = "0";
            if (nFiles.Where(p => p.id != 0).Count() > 0) noDeleteFileIds = ZentCloud.Common.MyStringHelper.ListToStr(nFiles.Where(p => p.id != 0).Select(p => p.id).ToList(), "", ",");
            result = bllJuActivity.Delete(new JuActivityFiles(), string.Format(" AutoID Not In ({0}) AND JuActivityID={1}", noDeleteFileIds, pInfo.JuActivityID.ToString()), tran) >= 0;
            
            foreach (var item in files)
            {
                if (!result) break;
                result = bllJuActivity.Add(item, tran);
            }
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            tran.Commit();
            apiResp.status = true;
            apiResp.msg = "提交完成";
            apiResp.code = (int)APIErrCode.IsSuccess;

            bllJuActivity.ContextResponse(context, apiResp);

        }

    }
}