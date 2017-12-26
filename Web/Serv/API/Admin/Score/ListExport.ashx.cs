using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Score
{
    /// <summary>
    /// ListExport 的摘要说明
    /// </summary>
    public class ListExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUserScoreDetailsInfo bll = new BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            int rows = int.MaxValue;
            int page = 1;
            string score_type = context.Request["score_type"];
            string score_events = context.Request["score_events"];
            string member = context.Request["member"];
            string start = context.Request["start"];
            string end = context.Request["end"];
            string is_print = context.Request["is_print"];
            if (!string.IsNullOrWhiteSpace(end)) end = Convert.ToDateTime(end).ToString("yyyy-MM-dd 23:59:59.999");
            string websiteOwner = bll.WebsiteOwner;

            string memberUserIds = "";
            if (!string.IsNullOrWhiteSpace(member)) {
                memberUserIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            }
            List<UserScoreDetailsInfo> list = bll.GetScoreList(rows, page, bll.WebsiteOwner, score_type, userIDs: memberUserIds,
                colName: "AutoID,UserID,Score,AddNote,AddTime,ScoreEvent,EventScore,DeductScore,Ex1,Ex2,Ex3,Ex4,Ex5,RelationID,SerialNumber",
                scoreEvents: score_events, startTime: start, endTime: end, isPrint: is_print);

            List<UserInfo> users = new List<UserInfo>();

            DataTable dt = new DataTable();
            dt.Columns.Add("记录编号", typeof(int));
            dt.Columns.Add("会员编号", typeof(int));
            dt.Columns.Add("会员姓名", typeof(string));
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("余额变动", typeof(double));
            dt.Columns.Add("事件", typeof(string));
            if (is_print == "1")
            {
                dt.Columns.Add("充值渠道", typeof(string));
                dt.Columns.Add("商户单号", typeof(string));
                dt.Columns.Add("支付单号", typeof(string));
                dt.Columns.Add("开户银行", typeof(string));
                dt.Columns.Add("开户名", typeof(string));
                dt.Columns.Add("银行卡号", typeof(string));
                dt.Columns.Add("税费", typeof(double));
            }
            else
            {
                dt.Columns.Add("说明", typeof(string));
                dt.Columns.Add("公积金/扣税", typeof(double));
            }

            if (list.Count > 0)
            {
                list.Select(p => p.UserID).ToList();
                string userIds = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p => p.UserID).ToList(), "'", ",");
                users = bll.GetColMultListByKey<UserInfo>(rows, 1, "UserID", userIds, "AutoID,UserID,TrueName,WXNickname,Phone", websiteOwner: websiteOwner);

                foreach (UserScoreDetailsInfo item in list)
                {
                    DataRow dr = dt.NewRow();
                    UserInfo nu = users.FirstOrDefault(p => p.UserID == item.UserID);
                    string id = nu == null ? "　" : nu.AutoID.ToString();
                    string name = bllUser.GetUserDispalyName(nu);
                    string phone = nu == null || string.IsNullOrWhiteSpace(nu.Phone) ? " " : nu.Phone;
                    dr["记录编号"] = item.AutoID.Value;
                    dr["会员编号"] = id;
                    dr["会员姓名"] = name;
                    dr["会员手机"] = phone;
                    dr["时间"] = item.AddTime.ToString("yyyy/MM/dd HH:mm:ss");
                    dr["余额变动"] = Math.Round(item.Score, 2);
                    dr["事件"] = item.ScoreEvent;
                    if (is_print == "1")
                    {
                        if (item.ScoreEvent.Contains("提现"))
                        {
                            dr["开户银行"] = item.Ex1;
                            dr["开户名"] = item.Ex2;
                            dr["银行卡号"] = item.Ex3;
                            dr["税费"] = Math.Round(item.DeductScore, 2);
                        }
                        else
                        {
                            string ex1 = "";
                            if (item.Ex5 == "alipay") { ex1 = "支付宝"; }
                            else if (item.Ex5 == "weixin") { ex1 = "微信"; }
                            else {ex1 = item.Ex1;}
                            dr["充值渠道"] = ex1;
                            if (!item.ScoreEvent.Contains("线下")) dr["商户单号"] = item.RelationID;
                            dr["支付单号"] = item.SerialNumber;
                        }
                    }
                    else
                    {
                        if (nu != null)
                        {
                            dr["说明"] = item.AddNote.Replace("转账给您", string.Format("转账给{0}({1})", name, id, phone));
                        }
                        else
                        {
                            dr["说明"] = item.AddNote.Replace("转账给您", string.Format("转账给[{0}]", item.UserID));
                        }
                        dr["公积金/扣税"] = Math.Round(item.DeductScore, 2);
                    }
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
            }
            string fname = is_print == "1" ? "充值提现记录" : "财务明细";
            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, fname);
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("{1}{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm"), fname),
                Stream = ms
            };
            string cache = Guid.NewGuid().ToString("N").ToUpper();
            ZentCloud.Common.DataCache.SetCache(cache, exCache, slidingExpiration: TimeSpan.FromMinutes(5));

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "生成完成";
            apiResp.result = new
            {
                cache = cache
            };
            bllUser.ContextResponse(context, apiResp);
        }
    }
}