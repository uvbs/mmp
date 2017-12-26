using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// ConfrimFormExport 的摘要说明
    /// </summary>
    public class ConfrimFormExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bll.WebsiteOwner;
            TeamPerformance myPerformance = bll.GetByKey<TeamPerformance>("AutoID", id, websiteOwner: websiteOwner);
            UserInfo user = bllUser.GetUserInfo(myPerformance.UserId,websiteOwner);
            List<UserLevelConfig> userLvlist = bll.QueryUserLevelList(websiteOwner, "DistributionOnLine",levelNumber:user.MemberLevel.ToString(), colName: "AutoId,LevelNumber,LevelString",showAll:true);

            string templatePath = context.Server.MapPath("/JsonConfig/ExcelTemplate/业绩确认表模板.xlsx");

            Dictionary<String,Object> dictSource = new Dictionary<string,object>();
            if(userLvlist.Count>0){
                dictSource.Add("lvname",userLvlist[0].LevelString);
            }
            dictSource.Add("name",user.TrueName);
            dictSource.Add("performance", Convert.ToDouble(myPerformance.Performance).ToString());
            dictSource.Add("rate",Convert.ToDouble(myPerformance.Rate)+"%");
            dictSource.Add("totalreward", Convert.ToDouble(myPerformance.TotalReward).ToString());
            dictSource.Add("childreward", Convert.ToDouble(myPerformance.ChildReward).ToString());

            dictSource.Add("fund", Convert.ToDouble((myPerformance.Reward * 20 / 100)).ToString());
            decimal trueamount = (myPerformance.Reward * 80 / 100);
            dictSource.Add("amount",Convert.ToDouble(trueamount).ToString());

            string ym = myPerformance.YearMonth.ToString();
            string dicYearmonth = ym.Substring(0,4)+"年"+ym.Substring(4)+"月";
            string dicMinAmount = trueamount.ToString("N");
            dictSource.Add("lower",string.Format("{0}应结算总金额  人民币小写：{1}",dicYearmonth,dicMinAmount));
            string cnAmount = CommonPlatform.Helper.MoneyHelper.GetCnString(trueamount.ToString());
            dictSource.Add("upper", string.Format("                       人民币大写：{0}", cnAmount));
            List<TeamPerformanceDetails> performanceList = bll.GetPerformanceDetailList(int.MaxValue, 1, "", "", websiteOwner, myPerformance.YearMonth, "", myPerformance.DetailIds);
            DataTable dt = null;
            if(performanceList.Count>0){
                List<lvGroup> lvList = performanceList.Where(p=>p.AddType=="注册").GroupBy(g=> new{
                    up = g.Performance,
                    cate = g.AddNote.Replace("注册", "").Replace("实单审核", "")
                }).Select(l=>new lvGroup{
                    cate = l.Key.cate,
                    count = l.Count(),
                    up=l.Key.up,
                    amount = l.Sum(li=> li.Performance)
                }).OrderBy(a=>a.amount).ToList();
                List<lvGroup> upList =performanceList.Where(p=>p.AddType=="升级").GroupBy(g=> new{
                    cate = "升级金额"
                }).Select(l=>new lvGroup{
                    cate = l.Key.cate,
                    count = l.Count(),
                    amount = l.Sum(li=> li.Performance)
                }).OrderBy(a => a.amount).ToList();

                List<lvGroup> ceList = performanceList.Where(p => p.AddType == "撤单").GroupBy(g => new
                {
                    cate = "撤单扣除"
                }).Select(l => new lvGroup
                {
                    cate = l.Key.cate,
                    count = l.Count(),
                    amount = l.Sum(li => li.Performance)
                }).OrderBy(a => a.amount).ToList();

                List<lvGroup> upuList = performanceList.Where(p => p.AddType == "变更").GroupBy(g => new
                {
                    cate = "变更上级"
                }).Select(l => new lvGroup
                {
                    cate = l.Key.cate,
                    count = l.Count(),
                    amount = l.Sum(li => li.Performance)
                }).OrderBy(a => a.amount).ToList();

                lvList.AddRange(upList);
                lvList.AddRange(ceList);
                lvList.AddRange(upuList);

                if(lvList.Count>0){
                    dt = new DataTable();
                    dt.Columns.Add("num", typeof(string));
                    dt.Columns.Add("cate", typeof(string));
                    dt.Columns.Add("count", typeof(int));
                    dt.Columns.Add("up", typeof(string));
                    dt.Columns.Add("amount", typeof(string));
                    int num = 0;
                    foreach (var item in lvList)
	                {
		                num ++;
                        DataRow dr = dt.NewRow();
                        dr["num"] = num.ToString().PadLeft(3,'0');
                        dr["cate"] = item.cate;
                        dr["count"] = item.count;
                        dr["up"] = item.up == 0 ? "" : Convert.ToDouble(item.up).ToString();
                        dr["amount"] = Convert.ToDouble(item.amount).ToString();
                        dt.Rows.Add(dr);
	                }
                    dictSource.Add("sumcount",lvList.Sum(p=>p.count));
                    decimal sumamount = lvList.Sum(p => p.amount);
                    dictSource.Add("sumamount", Convert.ToDouble(sumamount).ToString());
                }
            }
            if(dt!=null){ 
                dt.TableName = "detail";
                dt.AcceptChanges();
            }
            MemoryStream ms =CommonPlatform.Helper.Aspose.ExcelHelper.OutModelFileToStream(templatePath,dictSource,dt);
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("{1}业绩确认表_{0}.xls", user.TrueName, dicYearmonth),
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class lvGroup
        {
            public string cate { get; set; }
            public int count { get; set; }
            public decimal up { get; set; } 
            public decimal amount { get; set; } 
        }
    }
}