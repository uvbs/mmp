using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShList 的摘要说明
    /// </summary>
    public class ShList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bllDis = new BLLDistribution();
        BLLMember bll = new BLLMember();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);

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
            if(!string.IsNullOrWhiteSpace(up_member)) distributionOwners = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            string regUserIds = "";
            if(!string.IsNullOrWhiteSpace(reg_member)) regUserIds = bllUser.GetSpreadUserIds(reg_member, websiteOwner);
            string bill = "";
            if(empty_bill=="1" && true_bill!="1") bill="1";
            else if(empty_bill!="1" && true_bill=="1") bill="0";
            string apply = "";
            if(apply_pass=="1" && apply_other!="1") apply="1";
            else if(apply_pass!="1" && apply_other=="1") apply="0";
            string hasImg = "";
            if(has_img=="1" && no_img!="1") hasImg="1";
            else if(has_img!="1" && no_img=="1") hasImg="0";
            if(!string.IsNullOrWhiteSpace(end)) end = Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd");

            int total = bll.GetShMemberCount(websiteOwner, min_level, member, distributionOwners,
                regUserIds, bill, apply, hasImg, start, end, level_num, apply_cancel: apply_cancel, is_cancel: is_cancel);
            List<dynamic> rList = new List<dynamic>();
            if (total > 0)
            {
                List<UserInfo> list = bll.GetShMemberList(rows, page, websiteOwner, min_level, member,
                    distributionOwners, regUserIds, bill, apply, hasImg, start, end, level_num,
                    colName: "AutoID,UserID,Phone,TrueName,Regtime,DistributionOwner,RegUserID,RegisterWay,Stock,EmptyBill" +
                    ",MemberApplyStatus,MemberApplyTime,MemberLevel,Ex1,WebsiteOwner,IsDisable,IsLock", apply_cancel: apply_cancel, is_cancel: is_cancel);
                if (list.Count > 0)
                {
                    List<UserLevelConfig> lvlist = bllDis.QueryUserLevelList(websiteOwner, "DistributionOnLine", colName: "AutoId,LevelNumber,LevelString",showAll:true);
                    Dictionary<int, string> dicLevel = new Dictionary<int, string>();
                    foreach (UserLevelConfig item in lvlist) {
                        if (!dicLevel.ContainsKey(item.LevelNumber)){
                            dicLevel.Add(item.LevelNumber, item.LevelString);
                        }
                    }
                    List<UserInfo> uList = new List<UserInfo>();
                    List<string> uIdList = list.Select(p => p.DistributionOwner).Where(pi => !string.IsNullOrWhiteSpace(pi)).Distinct().ToList();
                    uIdList.AddRange(list.Select(p => p.RegUserID).Where(pi => !string.IsNullOrWhiteSpace(pi)).Distinct().ToList());
                    if (uIdList.Count > 0)
                    {
                        string uIdsString = ZentCloud.Common.MyStringHelper.ListToStr(uIdList,"'",",");
                        uList = bll.GetColList<UserInfo>(int.MaxValue, 1,
                            string.Format("UserID In ({0}) And WebsiteOwner='{1}'", uIdsString, websiteOwner),
                            "AutoID,UserID,Phone,TrueName,WebsiteOwner");
                    }

                    foreach (var item in list)
                    {
                        UserInfo regUser = uList.FirstOrDefault(p => p.UserID == item.RegUserID);
                        UserInfo upUser = uList.FirstOrDefault(p => p.UserID == item.DistributionOwner);
                        string lvString = dicLevel.ContainsKey(item.MemberLevel) ? dicLevel[item.MemberLevel] : "";
                        string memberStatus = item.EmptyBill == 1 ? "空单" : "实单";
                        memberStatus += (item.MemberApplyStatus == 9 ? "已审" : "未审");
                        if (item.IsDisable == 1 && item.MemberLevel ==0) { 
                            memberStatus = "已撤单";
                        }
                        else if (item.IsDisable == 1 && item.MemberLevel>0){
                            memberStatus = "申请撤单";
                        }
                        rList.Add(new
                        {
                            id = item.AutoID,
                            uid = item.UserID,
                            name = bllUser.GetUserDispalyName(item),
                            phone = item.Phone,
                            up_user = upUser == null ? null : new {
                                id = upUser.AutoID,
                                uid = upUser.UserID,
                                name = bllUser.GetUserDispalyName(upUser),
                                phone = upUser.Phone
                            },
                            reg_user = regUser == null ? null : new
                            {
                                id = regUser.AutoID,
                                uid = regUser.UserID,
                                name = bllUser.GetUserDispalyName(regUser),
                                phone = regUser.Phone
                            },
                            reg_time = item.MemberApplyTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            lv = lvString,
                            status = memberStatus,
                            way = item.RegisterWay,
                            stock = item.Stock,
                            ex1 = item.Ex1,
                            is_lock = item.IsLock
                        });
                    }
                }
            }
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "查询列表完成";
            apiResp.result = new
            {
                totalcount = total,
                list = rList
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}