using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;

namespace ZentCloud.BLLJIMP
{
    public class BLLMeeting : BLL
    {
        public BLLMeeting(string userID)
            : base(userID)
        {

        }
        public List<Model.MeetingDetails> GetMeetingDetails(string meetingId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
                    @"select Name, Mobile, Email, QQ, Company, Title, SerialNumber, IsInvited, IsEnrolled, IsSigned, GuestId from ZCJ_MeetingDetails T0
                     join ZCJ_MemberInfo T1 on T0.GuestId = T1.MemberId
                     where T0.MeetingId = '{0}'", meetingId);
            return Query<Model.MeetingDetails>(strSql.ToString());
        }

        //public static string GetMeeingUID()
        //{
        //    return GetZCUID("MeetingUID");
        //}

        public string GetSerialNumber(string meetingID)
        {
            return GetSingle(string.Format("select count(*) + 1000 from ZCJ_MeetingDetails where meetingId = '{0}' ", meetingID)).ToString();
        }
       

        public string GetSMSMobileNumbers(string strWhere )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                    @"select distinct Mobile from ZCJ_MemberInfo 
                        join ZCJ_MeetingDetails on ZCJ_MemberInfo.MemberId = ZCJ_MeetingDetails.GuestId ");

            if (strWhere != null && strWhere.Trim() != string.Empty)
            {
                strSql.AppendFormat(" where {0}", strWhere);
            }

            DataSet ds = Query(strSql.ToString());
            StringBuilder strMobiles = new StringBuilder();
            for (int i =0; i<ds.Tables[0].Rows.Count; ++i)
            {
                strMobiles.AppendFormat("{0}\n",ds.Tables[0].Rows[i]["Mobile"].ToString());
            }
            return strMobiles.ToString();
        }

        public int BatchSign(string meetingId, List<string> guestId)
        {
            if (guestId.Count == 0 || meetingId == null)
            {
                return 0;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"update ZCJ_MeetingDetails set IsSigned = 1 where GuestId in (");
            foreach (var item in guestId)
	        {
                strSql.AppendFormat(" '{0}',", item);
	        }
            strSql = strSql.Remove(strSql.Length - 2 , 1);
            strSql.AppendFormat(") and MeetingId = '{0}'", meetingId);
            return ZCDALEngine.DALEngine.ExecuteSql(strSql.ToString());


        }

        //public static List<Model.MeetingInfo> GetUserMeeting(string userId)
        //{
        //    return BLLBase.GetList<Model.MeetingInfo>(string.Format("UserId = '{0}'", userId));
        //}
    }
}
