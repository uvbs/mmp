using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User.Score
{
    /// <summary>
    /// 更新用户积分
    /// </summary>
    public class Update : BaseHanderOpen
    {
        /// <summary>
        /// 积分Bll
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "请用POST提交";
                bllScore.ContextResponse(context, resp);
                return;
            }
            string openId = context.Request["openid"];//openId
            string  score = context.Request["score"];//积分变动值
            string remark=context.Request["remark"];//备注
            string serialNumber = context.Request["serial_number"];//流水号

            remark = HttpUtility.UrlDecode(remark);
           
            if (string.IsNullOrEmpty(openId))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "openid 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(score))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "score 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }
           if (string.IsNullOrEmpty(remark))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "remark 参数必传";
                bllScore.ContextResponse(context, resp);
                return;
            }
           double scoreD = 0;
           if (!double.TryParse(score,out scoreD))
           {
               resp.code = (int)APIErrCode.OperateFail;
               resp.msg = "score 参数错误";
               bllScore.ContextResponse(context, resp);
               return;
           }
           if (scoreD==0)
           {
               resp.code = (int)APIErrCode.OperateFail;
               resp.msg = "score 参数不能等于0";
               bllScore.ContextResponse(context, resp);
               return;
           }
           string msg = "";
           if (bllScore.Update(bllScore.WebsiteOwner,openId, scoreD, remark,out msg,serialNumber))
            {
                resp.msg = "ok";
                resp.status = true;
            }
            else
            {
                resp.msg = msg;
                resp.code = (int)APIErrCode.OperateFail;
            }
            bllScore.ContextResponse(context, resp);

           


             


        }

        
    }
}