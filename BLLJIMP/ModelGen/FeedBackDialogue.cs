using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class FeedBackDialogue : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _dialogueid;
        private int _feedbackid;
        private string _userid;
        private string _dialoguecontent;
        private DateTime _insertdate;
        /// <summary>
        /// ID
        /// </summary>
        public int DialogueID
        {
            set { _dialogueid = value; }
            get { return _dialogueid; }
        }
        /// <summary>
        /// 问题ID 外键ZCJ_FeedBack FeedBackID
        /// </summary>
        public int FeedBackID
        {
            set { _feedbackid = value; }
            get { return _feedbackid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 对话内容
        /// </summary>
        public string DialogueContent
        {
            set { _dialoguecontent = value; }
            get { return _dialoguecontent; }
        }
        /// <summary>
        /// 对话时间
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        #endregion Model


    }
}
