using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WeiboSpiderPlanInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class WeiboSpiderPlanInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 采集总数
        /// </summary>
        public int GatherResultCount { get; set; }

        public string CreateDateStr
        {

            get
            {
                return _createdate.ToString();
            }
        }
    }
}

