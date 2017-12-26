using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.Common;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP
{
    public class BLLUserDistributionMember : BLL
    {
        
        /// <summary>
        /// 批量设置用户的上级到分销会员表，前提是这些会员关系都是允许设置的
        /// </summary>
        /// <param name="userIdList"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int SetUserDistributionOwnerInMember(List<string> userIdList, string distributionOwner, string websiteOwner)
        {
            if (userIdList == null)
            {
                return 0;
            }
            if (userIdList.Count == 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(distributionOwner) || string.IsNullOrWhiteSpace(websiteOwner))
            {
                return 0;
            }

            int result = 0;
           
            //再把新关系加上
            foreach (var item in userIdList)
            {
                //判断关系是否本身就存在，存在则不修改
                if (GetCount<UserDistributionMemberInfo>(string.Format(" MemberId = '{0}' AND UserId = '{1}' AND WebsiteOwner = '{2}' ",
                     item,
                     distributionOwner,
                     websiteOwner
                    )) > 0)
                {
                    continue;
                }

                Delete(new UserDistributionMemberInfo(), string.Format(" MemberId = '{0}' AND WebsiteOwner = '{1}' ",
                  item,
                  websiteOwner
                ));

                var member = new UserDistributionMemberInfo()
                {
                    InsertDate = DateTime.Now,
                    MemberId = item,
                    UserId = distributionOwner,
                    WebsiteOwner = websiteOwner
                };

                if (Add(member))
                {
                    result++;
                }
            }

            return result;
        }

        public int SetUserDistributionOwnerInMember(List<string> userIdList, string distributionOwner, string websiteOwner, BLLTransaction tran)
        {
            if (userIdList == null)
            {
                return 0;
            }
            if (userIdList.Count == 0)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(distributionOwner) || string.IsNullOrWhiteSpace(websiteOwner))
            {
                return 0;
            }

            int result = 0;

            //再把新关系加上
            foreach (var item in userIdList)
            {
                //判断关系是否本身就存在，存在则不修改
                if (GetCount<UserDistributionMemberInfo>(string.Format(" MemberId = '{0}' AND UserId = '{1}' AND WebsiteOwner = '{2}' ",
                     item,
                     distributionOwner,
                     websiteOwner
                    )) > 0)
                {
                    continue;
                }

                Delete(new UserDistributionMemberInfo(), string.Format(" MemberId = '{0}' AND WebsiteOwner = '{1}' ",
                  item,
                  websiteOwner
                ), tran);

                var member = new UserDistributionMemberInfo()
                {
                    InsertDate = DateTime.Now,
                    MemberId = item,
                    UserId = distributionOwner,
                    WebsiteOwner = websiteOwner
                };

                if (Add(member, tran))
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取会员是分销员的第几位会员
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetMemberRowCount(string memberId, string distributionOwner, string websiteOwner)
        {
            int result = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" select row_number() over(order by autoid asc) as RowNumber,* ");
            strSql.AppendFormat(" into #tmpDIsMember ");
            strSql.AppendFormat(" from ZCJ_UserDistributionMemberInfo ");
            strSql.AppendFormat(" where userid = '{0}' and websiteowner = '{1}'; ", distributionOwner, websiteOwner);
            strSql.AppendFormat(" select * from #tmpDIsMember ");
            strSql.AppendFormat(" where memberid = '{0}' ", memberId);

            var member = Query<UserDistributionMemberInfo>(strSql.ToString());

            if (member.Count > 0)
            {
                result = (int)member[0].RowNumber;
            }

            return result;
        }


    }
}
