using System;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class ActivityFieldMappingInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        private int _fieldIsDefauld = 0;
        private string _fieldname;

        /// <summary>
        /// 是否默认字段
        /// </summary>
        public int FieldIsDefauld { get { return _fieldIsDefauld; } set { _fieldIsDefauld = value; } }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName
        {
            get
            {
                if (FieldIsDefauld == 0)
                    return "K" + _exfieldindex;
                else
                    return _fieldname;
            }
            set { _fieldname = value; }
        }
    }
}
