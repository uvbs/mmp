using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.BLLJIMP
{
    public class BLLSignIn:BLL
    {
        public List<SignInAddress> GetSignInAddressList(int rows, int page, string keyword, string websiteOwner
            , out int total, bool showDelete = true,string type="")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" and WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" and Address like '%{0}%' ", keyword);
            if (!showDelete) sbSql.AppendFormat(" and IsDelete =0 ");
            if (!string.IsNullOrWhiteSpace(type))
            {
                sbSql.AppendFormat(" And Type='{0}' ", type);
            }
            else
            {
                sbSql.AppendFormat(" And (Type is null or Type='') ");
            }
            total = GetCount<SignInAddress>(sbSql.ToString());
            return GetLit<SignInAddress>(rows, page, sbSql.ToString());
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public bool AddSignInLog(int autoId, string userId, double longitude, double latitude, string websiteOwner, out string errmsg)
        {
            errmsg = "";
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" AutoID={0} and IsDelete=0 ", autoId);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" and WebsiteOwner='{0}' ", websiteOwner);
            SignInAddress signInAddress = Get<SignInAddress>(sbSql.ToString());
            if (signInAddress == null)
            {
                errmsg = "签到地点未找到";
                return false;
            }
            signInAddress.Distance = GeolocationHelper.ComputeDistance(longitude, latitude
                    , Convert.ToDouble(signInAddress.Longitude), Convert.ToDouble(signInAddress.Latitude));
            SignInLog signInLog = new SignInLog();
            signInLog.UserID = userId;
            signInLog.WebsiteOwner = websiteOwner;
            signInLog.IP = ZentCloud.Common.MySpider.GetClientIP();
            signInLog.CreateDate = DateTime.Now;
            signInLog.Longitude = longitude.ToString();
            signInLog.Latitude = latitude.ToString();
            signInLog.AddressId = signInAddress.AutoID;
            signInLog.Address = signInAddress.Address;
            signInLog.Distance = signInAddress.Distance;
            string DistanceString = string.Empty;
            if (signInAddress.Distance > signInAddress.Range)
            {
                signInLog.Status = 0;
                signInLog.Remark = "签到失败，超出有效距离";
            }
            else
            {
                signInLog.Status = 1;
                signInLog.Remark = "签到成功";
            }
            if (!Add(signInLog))
            {
                errmsg = "签到出错";
                return false;
            }
            errmsg = signInLog.Remark;
            return signInLog.Status == 1;
        }
        /// <summary>
        /// 智能签到
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public bool AddSignInLogAuto(string userId, double longitude, double latitude, string websiteOwner,out string errmsg)
        {
            errmsg = "";
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" and WebsiteOwner='{0}' ", websiteOwner);
            List<SignInAddress> list = GetList<SignInAddress>(sbSql.ToString());
            if (list.Count == 0) {
                errmsg = "请设置签到地点";
                return false; 
            }
            for (int i = 0; i < list.Count; i++)
			{
                list[i].Distance = GeolocationHelper.ComputeDistance(longitude,latitude
                    ,Convert.ToDouble(list[i].Longitude),Convert.ToDouble(list[i].Latitude));
			}
            List<SignInAddress> tempList = list.Where(p => p.Distance<=p.Range).ToList();

            SignInLog signInLog = new SignInLog();
            SignInAddress nearestAddress = new SignInAddress();
            signInLog.UserID = userId;
            signInLog.WebsiteOwner = websiteOwner;
            signInLog.IP = ZentCloud.Common.MySpider.GetClientIP();
            signInLog.CreateDate = DateTime.Now;
            signInLog.Longitude = longitude.ToString();
            signInLog.Latitude = latitude.ToString();
            string distanceString = string.Empty;
            if(tempList.Count>0){
                nearestAddress = tempList.OrderBy(p=>p.Distance).FirstOrDefault();
                signInLog.AddressId = nearestAddress.AutoID;
                signInLog.Address = nearestAddress.Address;
                signInLog.Distance = nearestAddress.Distance;
                signInLog.Status = 1;
                distanceString = nearestAddress.Distance > 1000 ? (nearestAddress.Distance / 1000).ToString("f2") + "千米" 
                    : nearestAddress.Distance.ToString("f2") + "米";
                signInLog.Remark = "签到成功，距离[" + nearestAddress.Address + "]" + distanceString;
            }
            else{
                nearestAddress = list.OrderBy(p => p.Distance).FirstOrDefault();
                signInLog.AddressId = nearestAddress.AutoID;
                signInLog.Address = nearestAddress.Address;
                signInLog.Distance = nearestAddress.Distance;
                signInLog.Status = 0;
                distanceString = nearestAddress.Distance > 1000 ? (nearestAddress.Distance / 1000).ToString("f2") + "千米"
                    : nearestAddress.Distance.ToString("f2") + "米";

                double moreDistance = signInLog.Distance - nearestAddress.Range;
                string moreDistanceString = string.Empty;
                moreDistanceString = moreDistance > 1000 ? (moreDistance / 1000).ToString("f2") + "千米" : moreDistance.ToString("f2") + "米";

                signInLog.Remark = "签到失败，距离最近签到地址["+nearestAddress.Address+"]"
                    + distanceString + "，超出有效距离"
                    + moreDistanceString;
            }
            if (!Add(signInLog))
            {
                errmsg = "签到出错";
                return false; 
            }
            errmsg = signInLog.Remark;
            return signInLog.Status ==1;
        }

        /// <summary>
        /// 获取签到地点详情
        /// </summary>
        /// <param name="webSiteOwner"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.SignInAddress GetSignInAddress(string webSiteOwner,string id)
        {
            return Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND AutoID={1} ",webSiteOwner,id));
        }


        /// <summary>
        /// 获取签到日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<SignInLog> GetSignLogList(int pageIndex, int pageSize, string keyWord,string addressId, out int totalCount,string startTime,string stopTime,string userId,string status)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(addressId))
            {
                sbWhere.AppendFormat(" AND AddressId={0} ",addressId);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND Address like '%{0}%'  ", keyWord);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" AND UserID like '%{0}%' ", userId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" AND Status={0} ", int.Parse(status));
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                sbWhere.AppendFormat(" AND CreateDate>='{0}'  ", Convert.ToDateTime(startTime));
            }

            if (!string.IsNullOrEmpty(stopTime))
            {
                sbWhere.AppendFormat(" AND CreateDate<'{0}' ", Convert.ToDateTime(stopTime));
            }
            
            totalCount = GetCount<SignInLog>(sbWhere.ToString());
            return GetLit<SignInLog>(pageSize,pageIndex,sbWhere.ToString()," AutoID Desc");


        }
    }
}
