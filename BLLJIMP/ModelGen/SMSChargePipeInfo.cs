using System;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class SMSChargePipeInfo : ZCBLLEngine.ModelTable
    {
        public SMSChargePipeInfo()
        { }
        #region Model
        private string _pipeid;
        private string _otherdescription;
        /// <summary>
        /// 
        /// </summary>
        public string PipeID
        {
            set { _pipeid = value; }
            get { return _pipeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OtherDescription
        {
            set { _otherdescription = value; }
            get { return _otherdescription; }
        }
        #endregion Model
    }
}
