using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// 会员列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
                int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
                string keyWord = context.Request["keyword"];//关键字
                string tagNames = context.Request["tag_names"];//标签
                string distriBution = context.Request["distribution"];//只显示分销会员
                string nameOrWxName = context.Request["name_wxname"];//只显示有姓名和微信名称的记录
                string userName = context.Request["user_name"];//只显示有姓名的记录
                string keyWordType = context.Request["key_word_type"];//根据key查找 

                int totalCount = 0;

                var userList = bllUser.GetUserList(pageIndex, pageSize, keyWord, tagNames, distriBution, nameOrWxName, userName, out totalCount, "", "", "", "", keyWordType);

                resp.isSuccess = true;

                List<dynamic> returnList = new List<dynamic>();

                foreach (var item in userList)
                {
                    returnList.Add(new 
                    { 
                        autoid=item.AutoID,
                        user_id=item.UserID,
                        wx_nick_name=item.WXNickname,
                        wx_head_img_url=item.WXHeadimgurl,
                        true_name=item.TrueName,
                        user_phone=item.Phone,
                        user_company = item.Company,
                        user_email = item.Email,
                        tag_name=item.TagName,
                        total_score=item.TotalScore,
                        user_position = item.Postion,
                        distributionowner=item.DistributionOwner,
                        access_level=item.AccessLevel,
                        available_vote_count=item.AvailableVoteCount,
                        #region 你我有信 无用
                        credit_acount =item.CreditAcount,
                        avatar=item.Avatar,
                        salary=item.Salary,
                        ex5=item.Ex5,
                        ex4=item.Ex4,
                        ex3=item.Ex3,
                        ex2=item.Ex2,
                        gender=item.Gender,
                        district=item.District,
                        birthday=bllUser.GetTimeStamp(item.Birthday)
                        #endregion
                    });
                }
                resp.returnObj = new
                {
                    totalcount=totalCount,
                    list = returnList
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}