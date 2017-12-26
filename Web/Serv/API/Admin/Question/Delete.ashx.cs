using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// 删除调查问卷
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string forceDelete = context.Request["force_delete"];
            //force_delete等于1时，则不管是否存在答题记录，进行强制删除
            if (forceDelete != "1")
            {
                if (bllQuestion.GetCountByKey<QuestionnaireRecord>("QuestionnaireID", id) > 0)
                {
                    resp.errcode = (int)APIErrCode.LotteryHaveRecord;
                    resp.errmsg = string.Format("已经有人答题,不能删除");
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
            }
            BLLTransaction tran = new BLLTransaction();
            try
            {

                if (bllQuestion.UpdateByKey<Questionnaire>("QuestionnaireID", id, "IsDelete", "1", tran) == -1)
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除问卷失败";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }

                //if (bllQuestion.DeleteByKey<Questionnaire>("QuestionnaireID", id, tran) == -1)
                //{
                //    tran.Rollback();
                //    resp.errcode = (int)APIErrCode.OperateFail;
                //    resp.errmsg = "删除问卷失败";
                //    bllQuestion.ContextResponse(context, resp);
                //    return;
                //}

                //if (bllQuestion.DeleteByKey<BLLJIMP.Model.Question>("QuestionnaireID", id, tran) == -1)
                //{
                //    tran.Rollback();
                //    resp.errcode = (int)APIErrCode.OperateFail;
                //    resp.errmsg = "删除问题失败";
                //    bllQuestion.ContextResponse(context, resp);
                //    return;
                //}
                //if (bllQuestion.DeleteByKey<Answer>("QuestionnaireID", id, tran) == -1)
                //{
                //    tran.Rollback();
                //    resp.errcode = (int)APIErrCode.OperateFail;
                //    resp.errmsg = "删除选项失败";
                //    bllQuestion.ContextResponse(context, resp);
                //    return;
                //}
                //if (bllQuestion.DeleteByKey<QuestionnaireRecord>("QuestionnaireID", id, tran) == -1)
                //{
                //    tran.Rollback();
                //    resp.errcode = (int)APIErrCode.OperateFail;
                //    resp.errmsg = "删除答题失败";
                //    bllQuestion.ContextResponse(context, resp);
                //    return;
                //}
                //if (bllQuestion.DeleteByKey<QuestionnaireRecordDetail>("QuestionnaireID", id, tran) == -1)
                //{
                //    tran.Rollback();
                //    resp.errcode = (int)APIErrCode.OperateFail;
                //    resp.errmsg = "删除答题详情失败";
                //    bllQuestion.ContextResponse(context, resp);
                //    return;
                //}
                tran.Commit();
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            bllQuestion.ContextResponse(context, resp);
        }

    }
}