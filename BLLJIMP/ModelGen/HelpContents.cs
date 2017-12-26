using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model 
{
     [Serializable]
    public class HelpContents : ZCBLLEngine.ModelTable
    {
        #region Model
        private int _autoid;
        private string _title;
        private string _helpcontent;
        private DateTime _adddate;
        private int _categoryid;
        private int _sort;
        private int _status;
        private string _adduserid;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HelpContent
        {
            set { _helpcontent = value; }
            get { return _helpcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CategoryID
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sort
        {
            set { _sort = value; }
            get { return _sort; }
        }
        /// <summary>
        /// 0未发布 1已发布
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AddUserID
        {
            set { _adduserid = value; }
            get { return _adduserid; }
        }
        #endregion Model

    }
}
