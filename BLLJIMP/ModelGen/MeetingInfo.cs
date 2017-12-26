using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 无用
    /// </summary>
    [Serializable]
    public partial class MeetingInfo : ZCBLLEngine.ModelTable
    {
        public MeetingInfo()
        { }
        #region Model
        private string _meetingid;
        private string _userid;
        private string _meetingname;
        private DateTime? _meetingdate;
        private string _meetingaddress;
        private string _meetingurl;
        private string _meetingdescription;
        private int? _enableconfirmsms = 0;
        private string _confirmsmscontent;
        
        /// <summary>
        /// 
        /// </summary>
        public string MeetingId
        {
            set { _meetingid = value; }
            get { return _meetingid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MeetingName
        {
            set { _meetingname = value; }
            get { return _meetingname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? MeetingDate
        {
            set { _meetingdate = value; }
            get { return _meetingdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MeetingAddress
        {
            set { _meetingaddress = value; }
            get { return _meetingaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MeetingUrl
        {
            set { _meetingurl = value; }
            get { return _meetingurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MeetingDescription
        {
            set { _meetingdescription = value; }
            get { return _meetingdescription; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? EnableConfirmSMS
        {
            set { _enableconfirmsms = value; }
            get { return _enableconfirmsms; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ConfirmSMSContent
        {
            set { _confirmsmscontent = value; }
            get { return _confirmsmscontent; }
        }
        #endregion Model

    }
}

