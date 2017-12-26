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
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();

        public void ProcessRequest(HttpContext context)
        {

            PostModel requestModel = new PostModel();//模型
            List<FileModel> nFiles = new List<FileModel>();
            try
            {
                requestModel = bllJuActivity.ConvertRequestToModel<PostModel>(requestModel);
                nFiles = ZentCloud.Common.JSONHelper.JsonToModel<List<FileModel>>(requestModel.file_list);
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
            JuActivityInfo pInfo = new JuActivityInfo();
            pInfo.JuActivityID = int.Parse(bllJuActivity.GetGUID(BLLJIMP.TransacType.AddPolicy));
            pInfo.ActivityName = requestModel.policy_name;
            pInfo.Summary = requestModel.summary;
            pInfo.ArticleType = "Policy";
            pInfo.CategoryId = "0";
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
            pInfo.IsHide = 0;
            pInfo.CreateDate = DateTime.Now;
            pInfo.LastUpdateDate = DateTime.Now;
            pInfo.WebsiteOwner = bllJuActivity.WebsiteOwner;
            pInfo.UserID = currentUserInfo.UserID;

            List<JuActivityFiles> files = new List<JuActivityFiles>();
            foreach (var item in nFiles)
            {
                files.Add(new JuActivityFiles()
                {
                    AddDate = DateTime.Now,
                    FileClass = item.file_class,
                    JuActivityID = pInfo.JuActivityID,
                    FileName = item.file_name,
                    FilePath = item.file_path,
                    UserID = pInfo.UserID
                });
            }

            bool result = false;
            BLLTransaction tran = new BLLTransaction();
            result = bllJuActivity.Add(pInfo, tran);
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
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
        /// <summary>
        /// 模型
        /// </summary>
        public class PostModel
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 政策名称
            /// </summary>
            public string policy_name { get; set; }
            /// <summary>
            /// 政策对象
            /// </summary>
            public string policy_object { get; set; }
            /// <summary>
            /// 户籍所在地
            /// </summary>
            public string domicile_place { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public string sex { get; set; }
            /// <summary>
            /// 男性年龄最小
            /// </summary>
            public string male_age_min { get; set; }
            /// <summary>
            /// 男性年龄最大
            /// </summary>
            public string male_age_max { get; set; }
            /// <summary>
            /// 女性年龄最小
            /// </summary>
            public string famale_age_min { get; set; }
            /// <summary>
            /// 女性年龄最大
            /// </summary>
            public string famale_age_max { get; set; }
            /// <summary>
            /// 学历
            /// </summary>
            public string education { get; set; }
            /// <summary>
            /// 毕业年限最小
            /// </summary>
            public string graduation_year_min { get; set; }
            /// <summary>
            /// 毕业年限最大
            /// </summary>
            public string graduation_year_max { get; set; }
            /// <summary>
            /// 就业状态
            /// </summary>
            public string employment_status { get; set; }
            /// <summary>
            /// 目前岗位工作年限最小
            /// </summary>
            public string current_job_life_min { get; set; }
            /// <summary>
            /// 目前岗位工作年限最大
            /// </summary>
            public string current_job_life_max { get; set; }
            /// <summary>
            /// 失业期限最小
            /// </summary>
            public string unemployment_period_min { get; set; }
            /// <summary>
            /// 失业期限最大
            /// </summary>
            public string unemployment_period_max { get; set; }
            /// <summary>
            /// 单位类型
            /// </summary>
            public string company_type { get; set; }
            /// <summary>
            /// 注册资金最小
            /// </summary>
            public string registered_capital_min { get; set; }
            /// <summary>
            /// 注册资金最大
            /// </summary>
            public string registered_capital_max{ get; set; }
            /// <summary>
            /// 人员规模最小
            /// </summary>
            public string personnel_size_min { get; set; }
            /// <summary>
            /// 人员规模最大
            /// </summary>
            public string personnel_size_max { get; set; }
            /// <summary>
            /// 单位规模
            /// </summary>
            public string company_size { get; set; }
            /// <summary>
            /// 所属行业
            /// </summary>
            public string industry { get; set; }
            /// <summary>
            /// 补贴标准
            /// </summary>
            public string subsidy_standard { get; set; }
            /// <summary>
            /// 补贴期限
            /// </summary>
            public string subsidy_period { get; set; }
            /// <summary>
            /// 政策文号
            /// </summary>
            public string policy_number { get; set; }
            /// <summary>
            /// 政策级别
            /// </summary>
            public string policy_level { get; set; }
            /// <summary>
            /// 政策大致描述
            /// </summary>
            public string summary { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            public int sort { get; set; }
            /// <summary>
            /// 附件
            /// </summary>
            public string file_list { get; set; }
        }

        public class FileModel
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 文件名称
            /// </summary>
            public string file_name { get; set; }
            /// <summary>
            /// 文件地址
            /// </summary>
            public string file_path { get; set; }
            /// <summary>
            /// 文件分类 1政策原文附件 2办事指南原文附件
            /// </summary>
            public int file_class { get; set; }
        }
    }
}