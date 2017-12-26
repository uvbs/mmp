using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    /// 修改积分规则
    /// </summary>
    public class Put : BaseHandlerNeedLoginAdminNoAction
    {
        BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
        BLLKeyValueData bllkeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try 
	        {
                requestModel = bllScoreDefine.ConvertRequestToModel<RequestModel>(requestModel);
	        }
	        catch (Exception ex)
	        {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = ex.Message;
                bllScoreDefine.ContextResponse(context,resp);
                return;
	        }
            KeyVauleDataInfo ScoreDefineData = bllkeyValueData.GetKeyData("ScoreDefineType", requestModel.score_type, "Common");
            if (ScoreDefineData == null)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "规则类型暂不支持";
                bllScoreDefine.ContextResponse(context,resp);
                return;
            }
            ScoreDefineInfo scoreDefineInfo = new ScoreDefineInfo();
            scoreDefineInfo.Score = requestModel.score;
            scoreDefineInfo.DayLimit = requestModel.day_limit;
            scoreDefineInfo.Description = requestModel.description;
            scoreDefineInfo.WebsiteOwner = bllScoreDefine.WebsiteOwner;
            scoreDefineInfo.CreateUserId = currentUserInfo.UserID;
            scoreDefineInfo.IsHide = requestModel.ishide;
            scoreDefineInfo.InsertTime = DateTime.Now;
            scoreDefineInfo.OrderNum = requestModel.order_num;
            scoreDefineInfo.Name = ScoreDefineData.DataValue;
            scoreDefineInfo.ScoreType = ScoreDefineData.DataKey;
            if (bllScoreDefine.PutScoreDefine(scoreDefineInfo))
            {
                resp.errcode = (int)APIErrCode.IsSuccess;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "提交失败";
            }
            bllScoreDefine.ContextResponse(context,resp);
        }
        public class RequestModel
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string score_type { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 分值
            /// </summary>
            public double score { get; set; }
            /// <summary>
            /// 是否隐藏 0不隐藏 1隐藏
            /// </summary>
            public int ishide { get; set; }
            /// <summary>
            /// 每日上限 分值
            /// </summary>
            public double day_limit { get; set; }
            /// <summary>
            /// 问排序
            /// </summary>
            public int order_num { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string description { get; set; }
        }
    }
}