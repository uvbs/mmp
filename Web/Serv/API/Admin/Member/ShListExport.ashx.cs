using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShListExport 的摘要说明
    /// </summary>
    public class ShListExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bllDis = new BLLDistribution();
        BLLMember bll = new BLLMember();
        public void ProcessRequest(HttpContext context)
        {

            int rows = int.MaxValue;
            int page = 1;
            string member = context.Request["member"];
            string up_member = context.Request["up_member"];
            string reg_member = context.Request["reg_member"];
            string empty_bill = context.Request["empty_bill"];
            string true_bill = context.Request["true_bill"];
            string apply_pass = context.Request["apply_pass"];
            string apply_other = context.Request["apply_other"];
            string has_img = context.Request["has_img"];
            string no_img = context.Request["no_img"];
            string apply_cancel = context.Request["apply_cancel"];
            string is_cancel = context.Request["is_cancel"];
            string start = context.Request["start"];
            string end = context.Request["end"];
            string min_level = context.Request["min_level"];
            string level_num = context.Request["level_num"];

            string websiteOwner = bll.WebsiteOwner;

            string distributionOwners = "";
            if (!string.IsNullOrWhiteSpace(up_member)) distributionOwners = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            string regUserIds = "";
            if (!string.IsNullOrWhiteSpace(reg_member)) regUserIds = bllUser.GetSpreadUserIds(reg_member, websiteOwner);
            string bill = "";
            if (empty_bill == "1" && true_bill != "1") bill = "1";
            else if (empty_bill != "1" && true_bill == "1") bill = "0";
            string apply = "";
            if (apply_pass == "1" && apply_other != "1") apply = "1";
            else if (apply_pass != "1" && apply_other == "1") apply = "0";
            string hasImg = "";
            if (has_img == "1" && no_img != "1") hasImg = "1";
            else if (has_img != "1" && no_img == "1") hasImg = "0";
            if (!string.IsNullOrWhiteSpace(end)) end = Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd");

            List<UserInfo> list = bll.GetShMemberList(rows, page, websiteOwner, min_level, member,
                distributionOwners, regUserIds, bill, apply, hasImg, start, end, level_num,
                colName: "AutoID,UserID,Phone,TrueName,Regtime,DistributionOwner,RegUserID,RegisterWay,Stock,EmptyBill" +
                ",MemberApplyStatus,MemberApplyTime,MemberLevel,Ex1,WebsiteOwner,IsDisable,TotalAmount,AccountAmountEstimate,AccumulationFund" +
                ",Province,City,District,Town,Address,IsLock", apply_cancel: apply_cancel, is_cancel: is_cancel);

            List<UserInfo> otherList = new List<UserInfo>();

            DataTable dt = new DataTable();
            dt.Columns.Add("会员编号", typeof(int));
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("会员姓名", typeof(string));
            dt.Columns.Add("推荐人编号", typeof(string));
            dt.Columns.Add("推荐人手机", typeof(string));
            dt.Columns.Add("推荐人姓名", typeof(string));
            dt.Columns.Add("报单人编号", typeof(string));
            dt.Columns.Add("报单人手机", typeof(string));
            dt.Columns.Add("报单人姓名", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            dt.Columns.Add("当前级别", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("报单方式", typeof(string));
            dt.Columns.Add("账面余额", typeof(decimal));
            dt.Columns.Add("可用余额", typeof(decimal));
            dt.Columns.Add("账面公积金", typeof(decimal));
            dt.Columns.Add("所在地", typeof(string));
            dt.Columns.Add("地址", typeof(string));
            dt.Columns.Add("是否锁定", typeof(string));

            if (list.Count > 0)
            {
                List<UserLevelConfig> lvlist = bllDis.QueryUserLevelList(websiteOwner, "DistributionOnLine",
                    colName: "AutoId,LevelNumber,LevelString", showAll: true);
                Dictionary<int, string> dicLevel = new Dictionary<int, string>();
                foreach (UserLevelConfig item in lvlist)
                {
                    if (!dicLevel.ContainsKey(item.LevelNumber))
                    {
                        dicLevel.Add(item.LevelNumber, item.LevelString);
                    }
                }

                foreach (var item in list)
                {
                    UserInfo upUser = null;
                    if (!string.IsNullOrWhiteSpace(item.DistributionOwner))
                    {
                        upUser = list.FirstOrDefault(p => p.UserID == item.DistributionOwner);
                        if (upUser == null) otherList.FirstOrDefault(p => p.UserID == item.DistributionOwner);
                        if (upUser == null)
                        {
                            upUser = bll.GetColByKey<UserInfo>("UserID", item.DistributionOwner, "AutoID,UserID,Phone,TrueName,Regtime,DistributionOwner,RegUserID,RegisterWay,Stock,EmptyBill" +
                            ",MemberApplyStatus,MemberLevel,Ex1,WebsiteOwner,IsDisable,TotalAmount,AccountAmountEstimate,AccumulationFund" +
                            ",Province,City,District,Town,Address", websiteOwner: websiteOwner);
                            if (upUser != null) otherList.Add(upUser);
                        }
                    }
                    UserInfo regUser = null;
                    if (!string.IsNullOrWhiteSpace(item.RegUserID))
                    {
                        regUser = list.FirstOrDefault(p => p.UserID == item.RegUserID);
                        if (regUser == null) otherList.FirstOrDefault(p => p.UserID == item.RegUserID);
                        if (regUser == null)
                        {
                            regUser = bll.GetColByKey<UserInfo>("UserID", item.RegUserID, "AutoID,UserID,Phone,TrueName,Regtime,DistributionOwner,RegUserID,RegisterWay,Stock,EmptyBill" +
                            ",MemberApplyStatus,MemberLevel,Ex1,WebsiteOwner,IsDisable,TotalAmount,AccountAmountEstimate,AccumulationFund" +
                            ",Province,City,District,Town,Address", websiteOwner: websiteOwner);
                            if (regUser != null) otherList.Add(regUser);
                        }
                    }

                    string lvString = dicLevel.ContainsKey(item.MemberLevel) ? dicLevel[item.MemberLevel] : "";
                    string memberStatus = item.EmptyBill == 1 ? "空单" : "实单";
                    memberStatus += (item.MemberApplyStatus == 9 ? "已审" : "未审");
                    if (item.IsDisable == 1 && item.MemberLevel == 0)
                    {
                        memberStatus = "已撤单";
                    }
                    else if (item.IsDisable == 1 && item.MemberLevel > 0)
                    {
                        memberStatus = "申请撤单";
                    }

                    DataRow dr = dt.NewRow();
                    dr["会员编号"] = item.AutoID;
                    dr["会员手机"] = item.Phone;
                    dr["会员姓名"] = item.TrueName;

                    dr["推荐人编号"] = upUser == null ? "" : upUser.AutoID.ToString();
                    dr["推荐人手机"] = upUser == null ? "" : upUser.Phone;
                    dr["推荐人姓名"] = upUser == null ? "" : upUser.TrueName;

                    dr["报单人编号"] = regUser == null ? "" : regUser.AutoID.ToString();
                    dr["报单人手机"] = regUser == null ? "" : regUser.Phone;
                    dr["报单人姓名"] = regUser == null ? "" : regUser.TrueName;

                    dr["注册时间"] = item.MemberApplyTime.ToString("yyyy/MM/dd HH:mm:ss");
                    dr["当前级别"] = dicLevel.ContainsKey(item.MemberLevel) ? dicLevel[item.MemberLevel] : "";
                    dr["状态"] = memberStatus;
                    dr["报单方式"] = item.RegisterWay;

                    dr["账面余额"] = item.TotalAmount;
                    dr["可用余额"] = item.AccountAmountEstimate;
                    dr["账面公积金"] = item.AccumulationFund;

                    List<string> pcdt = new List<string>();
                    if (!string.IsNullOrWhiteSpace(item.Province)) pcdt.Add(item.Province);
                    if (!string.IsNullOrWhiteSpace(item.City)) pcdt.Add(item.City);
                    if (!string.IsNullOrWhiteSpace(item.District)) pcdt.Add(item.District);
                    if (!string.IsNullOrWhiteSpace(item.Town)) pcdt.Add(item.Town);
                    if (pcdt.Count > 0)
                    {
                        dr["所在地"] = ZentCloud.Common.MyStringHelper.ListToStr(pcdt, "", " ");
                    }
                    dr["地址"] = item.Address;
                    dr["是否锁定"] = item.IsLock == 1 ? "锁定" : "";
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
            }

            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, "会员信息");
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("会员信息{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")),
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