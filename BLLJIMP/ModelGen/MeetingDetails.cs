using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 无用
    /// </summary>
    [Serializable]
    public partial class MeetingDetails : ZCBLLEngine.ModelTable
    {
        public MeetingDetails()
        { }
        #region Model
        private int _autoid;
        private string _meetingid;
        private string _guestid;
        private string _serialnumber;
        private int? _isinvited;
        private int? _isenrolled;
        private int? _issigned;
        /// <summary>
        /// 自增ID
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 会议ID
        /// </summary>
        public string MeetingId
        {
            set { _meetingid = value; }
            get { return _meetingid; }
        }
        /// <summary>
        /// 参会者ID
        /// </summary>
        public string GuestId
        {
            set { _guestid = value; }
            get { return _guestid; }
        }
        /// <summary>
        /// 报名编号
        /// </summary>
        public string SerialNumber
        {
            set { _serialnumber = value; }
            get { return _serialnumber; }
        }
        /// <summary>
        /// 标志参会者是否被邀请
        /// </summary>
        public int? IsInvited
        {
            set { _isinvited = value; }
            get { return _isinvited; }
        }
        /// <summary>
        /// 标志参会者是否报名
        /// </summary>
        public int? IsEnrolled
        {
            set { _isenrolled = value; }
            get { return _isenrolled; }
        }
        /// <summary>
        /// 标志参会者是否签到
        /// </summary>
        public int? IsSigned
        {
            set { _issigned = value; }
            get { return _issigned; }
        }
        #endregion Model

       

    }
}

