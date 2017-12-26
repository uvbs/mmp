using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User.Score
{
    /// <summary>
    /// Move 的摘要说明
    /// </summary>
    public class Move :BaseHanderOpen
    {

        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "请用POST提交";
                bllScore.ContextResponse(context, resp);
                return;
            }
            string fromOpenId = context.Request["from_openid"];
            string toOpenId=context.Request["to_openid"];
            string serialNumber = context.Request["serial_number"];//流水号
            string remark=context.Request["remark"];

            if (string.IsNullOrEmpty(fromOpenId))
            {
                 resp.msg = " from_openid 参数必传";
                 resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                 bllScore.ContextResponse(context, resp);
                 return;
            }
            if (string.IsNullOrEmpty(toOpenId))
            {
                resp.msg = " to_openid 参数必传";
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllScore.ContextResponse(context, resp);
                return;
            }
            if (fromOpenId==toOpenId)
            {
                resp.msg = " openid不能相同";
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bllScore.ContextResponse(context, resp);
                return;
            }
            string msg = "";
            if (bllScore.Move(bllUser.WebsiteOwner,fromOpenId, toOpenId,out msg,serialNumber,remark))
            {

                resp.msg = "ok";
                resp.status = true;


            }
            else {

                resp.msg = msg;
                resp.code = (int)APIErrCode.OperateFail;
            }
            bllScore.ContextResponse(context, resp);



        }


        
    }
}