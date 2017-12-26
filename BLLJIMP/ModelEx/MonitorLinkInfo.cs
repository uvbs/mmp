using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class MonitorLinkInfo : ZCBLLEngine.ModelTable
    {
        ///// <summary>
        ///// 打开代码
        ///// </summary>
        //public string LinkOpenCode
        //{

        //    get
        //    {

        //        //if (!string.IsNullOrEmpty(_reallink))
        //        //{
        //        //    string host = System.Web.HttpContext.Current.Request.Url.Host;
        //        //    string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();

        //        //    return string.Format("<img src=\"{0}\" style=\"display:none;\" >", string.Format("http://{0}:{1}/Monitor/transurl.aspx?s={2}", host, port, EncryptParameter));

        //        //}
        //        return "";

        //    }



        //}



        ///// <summary>
        ///// 转换后的点击链接
        ///// </summary>
        //public string ConvertUrl
        //{

        //    get
        //    {

        //        //if (!string.IsNullOrEmpty(_reallink))
        //        //{
        //        //    string host = System.Web.HttpContext.Current.Request.Url.Host;
        //        //    string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();
        //        //    return string.Format("http://{0}:{1}/Monitor/transurl.aspx?s={2}teq1", host, port, EncryptParameter);


        //        //}
        //        return "";

        //    }



        //}

        ///// <summary>
        ///// 打开人次
        ///// </summary>
        //public int OpenCount1
        //{
        //    get
        //    {
        //        int clickcount = 0;
        //        //clickcount = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(string.Format("LinkID={0} and EventType=0", _linkid));
        //        //if (clickcount > 0)
        //        //{
        //        //    return clickcount;
        //        //}
        //        //else
        //        //{
        //        //    return new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(string.Format("LinkID={0} and EventType=1", _linkid));
        //        //}
        //        return clickcount;



        //    }



        //}


        ///// <summary>
        /////打开人数
        ///// </summary>
        //public int DistinctOpenCount1
        //{
        //    get
        //    {
        //        int clickcount = 0;
        //        //clickcount = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("LinkID={0} and EventType=1", _linkid));
        //        //if (clickcount > 0)
        //        //{
        //        //    return clickcount;
        //        //}
        //        //else
        //        //{
        //        //    return new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("LinkID={0} and EventType=0", _linkid));
        //        //}
        //        return clickcount;



        //    }



        //}
        /// <summary>
        /// 报名人数
        /// </summary>
        //public int SignUpCount
        //{
        //    get
        //    {

        //        int count = 0;
        //        try
        //        {
        //            count = new BLL("").GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", _monitorplanid, _linkname));
        //        }
        //        catch
        //        {


        //        }
        //        return count;
        //    }
        //}

        ///// <summary>
        ///// 吸粉数量
        ///// </summary>
        //public int FansCount
        //{
        //    get
        //    {
        //        int count = 0;
        //        try
        //        {
        //            count = new BLL("").GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND  DistributionOwner='{2}'", WebsiteOwner, ActivityId,new BLL().GetCurrUserID()));
        //        }
        //        catch (Exception)
        //        {
                    
        //            throw;
        //        }
        //        return count;
        //    }
        //}


        public UserInfo userInfo
        {
            get
            {
                return new BLLUser("").GetUserInfo(LinkName);
            }
        }

        public string TureName
        {
            get
            {
                if (userInfo != null)
                {
                    if (!string.IsNullOrEmpty(userInfo.TrueName))
                    {
                        return userInfo.TrueName;
                    }
                    else
                    {
                        return userInfo.UserID;
                    }
                    
                }
                else
                    return "";
            }

        }

        public string Phone { get { if (userInfo != null) { return userInfo.Phone; } else { return ""; } } }


        /// <summary>
        /// 活动id
        /// </summary>
        public int JuActivityID { get; set; }

        /// <summary>
        /// PV
        /// </summary>
        public int PV { get; set; }

    }
}
