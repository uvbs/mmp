using System;
using System.Collections.Generic;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户表扩展
    /// </summary>
    public partial class UserInfo : ZCBLLEngine.ModelTable
    {
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 获取本月发送短信总数
        /// </summary>
        /// <returns></returns>
        public int GetCurrMonthSendSmsTotalCount()
        {
            ////任务总数+触发总数(按计费条数来计算)

            //int i = 0, j = 0;

            //try
            //{

            //    object tmp = ZCDALEngine.DbHelperSQL.GetSingle(string.Format(" select SUM(ChargeCount) from dbo.ZCJ_SMSPlanInfo where month(SubmitDate) = {0} and YEAR(SubmitDate) = {1} and SenderID = '{2}'  ",
            //                DateTime.Now.Month,
            //                DateTime.Now.Year,
            //                _userid
            //            ));
            //    int.TryParse(tmp.ToString(), out i);
            //}
            //catch { }

            //try
            //{
            //    int.TryParse(ZCDALEngine.DbHelperSQL.GetSingle(string.Format(" select SUM(ChargeCount) from dbo.ZCJ_SMSTriggerDetails where month(SubmitDate) = {0} and YEAR(SubmitDate) = {1} and UserID = '{2}' ",
            //               DateTime.Now.Month,
            //               DateTime.Now.Year,
            //               _userid
            //           )).ToString(), out j);
            //}
            //catch { }

            //return i + j;
            return 0;
        }
        /// <summary>
        /// 获取本月发送邮件总数
        /// </summary>
        /// <returns></returns>
        public int GetCurrMonthSendEdmTotalCount()
        {
            //获取本月发送所有的邮件

            //try
            //{
            //    BLL bll = new BLL("");

            //    List<EmailInfo> list = bll.GetList<EmailInfo>(string.Format("  month(SubmitDate) = {0} and YEAR(SubmitDate) = {1} and UserID = '{2}' ",
            //            DateTime.Now.Month,
            //            DateTime.Now.Year,
            //            _userid
            //        ));

            //    int result = 0;

            //    foreach (var item in list)
            //    {
            //        result += item.OSendTotalCount;
            //    }

            //    return result;
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return 0;
        }

        /// <summary>
        /// 获取AutoID16位字符
        /// </summary>
        /// <returns></returns>
        public string GetUserAutoIDHex()
        {
            return Convert.ToString(AutoID, 16);
        }

        private string _wXHeadimgurlLocal;

        /// <summary>
        /// 针对会员头像QQ防盗链的情况，将头像图片下载到本地地址
        /// </summary>
        public string WXHeadimgurlLocal
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WXHeadimgurl))
                {
                    return "http://open-files.comeoncloud.net/www/guicai/jubit/image/20160325/A12476F2D0CD4B8FA037267F393BF69E.png";
                }
                return WXHeadimgurl;

                //string result = "";

                //if (string.IsNullOrWhiteSpace(WXHeadimgurl))
                //{
                //    result = "";
                //}
                //else if (!WXHeadimgurl.ToLower().StartsWith("http"))//后台上传的
                //{
                //    result = WXHeadimgurl;
                //}
                //else
                //{
                //    result = new BLLJuActivity().DownLoadRemoteImage(WXHeadimgurl);
                //}

                //if (result != "")
                //{
                //    result = Common.ConfigHelper.GetConfigString("serverImgPath") + result;
                //}

                //return result;
            }

            set { _wXHeadimgurlLocal = value; }
        }

        /// <summary>
        /// 用户是否是导师
        /// </summary>
        public bool IsTutor { get; set; }
        /// <summary>
        /// 鸿风用户组
        /// </summary>
        public string HFUserPmsGroup
        {

            get
            {

                return "";
                ////                       <%--130273 a 1fce1
                ////130334 g 1fd1e
                ////130335 f 1fd1f--%>

                //List<long> hfPmsIdList = new List<long>() { 130273, 130334, 130335, 130388 };
                //long hfPmsId = 0;
                //List<UserPmsGroupRelationInfo> pmsGroup = new BLL().GetList<UserPmsGroupRelationInfo>(string.Format(" UserID = '{0}' ",UserID));

                //foreach (var item in pmsGroup)
                //{
                //    if (hfPmsIdList.Contains(item.GroupID))
                //    {
                //        hfPmsId = item.GroupID;
                //        break;
                //    }
                //}

                //switch (hfPmsId)
                //{
                //    case 130273:
                //        return "管理员";
                //    case 130334:
                //        return "游客";
                //    case 130335:
                //        return "正式学员";
                //    case 130388:
                //        return "教师";
                //    default:
                //        return "";
                //}

            }
        }

        /// <summary>
        /// 距离
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 线下分销推荐人信息
        /// </summary>
        public UserInfo DistributionOffLineRecomendUserInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(DistributionOffLinePreUserId))
                {
                    if (DistributionOffLinePreUserId == UserID)
                    {
                        return null;
                    }
                    return bllUser.GetUserInfo(DistributionOffLinePreUserId);
                }
                return null;
            }
        }
        /// <summary>
        /// 商城分销推荐人信息 无用,不在这里查了
        /// </summary>
        public UserInfo DistributionOnLineRecomendUserInfo
        {
            get
            {
                return null;
                //UserInfo result = new UserInfo();
                //if (!string.IsNullOrEmpty(DistributionOwner))
                //{
                //    if (DistributionOwner == UserID)
                //    {
                //        return null;
                //    }

                //    result = bllUser.GetUserInfo(DistributionOwner);

                //    if (result != null)
                //    {
                //        result.TrueName = bllUser.GetUserDispalyName(result);
                //    }

                //    return result;
                //}
                //return null;
            }
        }

        //年龄
        public string Age
        {
            get
            {
                if (Birthday < new DateTime(1901, 1, 1)) return "";
                int ageNum = DateTime.Today.Year - Birthday.Year;
                if (ageNum < 1) return "";
                return ageNum.ToString();
            }
        }

        /// <summary>
        /// 会员归属地，由省市区县拼接
        /// </summary>
        public string MemberAttribution
        {
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrWhiteSpace(Province))
                {
                    result = Province;

                    if (!string.IsNullOrWhiteSpace(City)) result += " " + City;
                    
                    if (!string.IsNullOrWhiteSpace(District)) result += " " + District;

                    if (!string.IsNullOrWhiteSpace(Town)) result += " " + Town;

                }

                return result;
            }
        }

    }
}
