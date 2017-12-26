using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    /// RefuseWithdrawCash 的摘要说明
    /// </summary>
    public class RefuseWithdrawCash : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string moduleName = "积分";
            if (!string.IsNullOrWhiteSpace(context.Request["module_name"])) moduleName = context.Request["module_name"];
            int total = 0;
            if (string.IsNullOrWhiteSpace(ids))
            {
                apiResp.msg = "请选中申请";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            var sourceList = bll.QueryWithdrawCashList(1, int.MaxValue, null, out total, "0", context.Request["type"], ids);
            string websiteOwner = bll.WebsiteOwner;
            List<int> idList = new List<int>();
            foreach (var p in sourceList)
            {
                UserInfo pu = bllUser.GetUserInfo(p.UserId, websiteOwner);

                //积分明细
                UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
                if (pu != null)
                {
                    scoreModel.AddNote = string.Format("申请提现未通过审核，返还{0}{1}，编号：{2}", p.Score, moduleName, p.AutoID);
                    scoreModel.AddTime = DateTime.Now;
                    scoreModel.Score = p.Score;
                    scoreModel.UserID = p.UserId;
                    scoreModel.ScoreType = "RefuseWithdrawCash";
                    scoreModel.TotalScore = pu.TotalScore + p.Score;
                    scoreModel.WebSiteOwner = websiteOwner;
                }

                //通知
                BLLJIMP.Model.SystemNotice notice = new BLLJIMP.Model.SystemNotice();
                if (pu != null)
                {
                    notice.Ncontent = scoreModel.AddNote;
                    notice.UserId = currentUserInfo.UserID;
                    notice.Receivers = pu.UserID;
                    notice.SendType = 2;
                    notice.Title = "申请提现未通过审核";
                    notice.NoticeType = 1;
                    notice.WebsiteOwner = websiteOwner;
                    notice.InsertTime = DateTime.Now;
                }

                BLLTransaction tran = new BLLTransaction();
                bool result = bll.Update(p, string.Format("Status=3,LastUpdateDate=getdate()"), string.Format("AutoID={0}", p.AutoID), tran) > 0;
                if (!result)
                {
                    idList.Add(p.AutoID);
                    tran.Rollback();
                    continue;
                }
                if (pu != null)
                {
                    result = bll.Update(pu, string.Format("TotalScore=TotalScore+{0}", p.Score), string.Format("UserID='{0}' And WebsiteOwner='{1}'", pu.UserID, websiteOwner), tran) > 0;
                    if (!result)
                    {
                        idList.Add(p.AutoID);
                        tran.Rollback();
                        continue;
                    }

                    result = bll.Add(scoreModel, tran);
                    if (!result)
                    {
                        idList.Add(p.AutoID);
                        tran.Rollback();
                        continue;
                    }
                    notice.SerialNum = bllUser.GetGUID(TransacType.SendSystemNotice);
                    result = bll.Add(notice, tran);
                    if (!result)
                    {
                        idList.Add(p.AutoID);
                        tran.Rollback();
                        continue;
                    }
                }
                tran.Commit();
            }

            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            if (idList.Count == 0)
            {
                apiResp.msg = "处理完成";
            }
            else
            {
                apiResp.msg = string.Format("处理完成，编号[{0}]处理失败", ZentCloud.Common.MyStringHelper.ListToStr(idList, "", ","));
            }
            bll.ContextResponse(context, apiResp);
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