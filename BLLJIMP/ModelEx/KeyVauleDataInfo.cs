using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class KeyVauleDataInfo
    {
        public KeyVauleDataInfo() { }
        /// <summary>
        /// 构造新对象
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="dataType"></param>
        /// <param name="dataValue"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="preKey"></param>
        /// <param name="creater"></param>
        /// <param name="orderBy"></param>
        public KeyVauleDataInfo(string dataType, string dataKey, string dataValue, string websiteOwner, string preKey = null
            , string creater = null, string orderBy = null, string dataName = null)
        {
            if (!string.IsNullOrWhiteSpace(dataType)) DataType = dataType;
            if (!string.IsNullOrWhiteSpace(dataKey)) DataKey = dataKey;
            if (!string.IsNullOrWhiteSpace(dataValue)) DataValue = dataValue;
            if (!string.IsNullOrWhiteSpace(websiteOwner)) WebsiteOwner = websiteOwner;
            if (!string.IsNullOrWhiteSpace(preKey)) PreKey = preKey;
            if (!string.IsNullOrWhiteSpace(creater)) Creater = creater;
            CreateTime = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(orderBy)) this.OrderBy = Convert.ToInt32(orderBy);
            if (!string.IsNullOrWhiteSpace(dataName)) this.DataName = dataName;
        }
    }
}
