using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class WeixinMemberInfo : ZCBLLEngine.ModelTable
    {
        public List<string> FlowIDHistoryList
        {
            get
            {
                List<string> result = new List<string>();
                if (string.IsNullOrWhiteSpace(FlowIDHistory))
                    return result;
                result = FlowIDHistory.Split(',').ToList();
                return result;
            }
        }

        public void AddFlowIDHistory(int id)
        {
            List<string> flowIDHistory = new List<string>();
            flowIDHistory.Add(id.ToString());
            this.FlowIDHistory = Common.StringHelper.ListToStr<string>(flowIDHistory, "", ",");
        }

        /// <summary>
        /// 检查是否已执行过某流程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isExitFlowIDHistory(int id)
        {
            if (this.FlowIDHistoryList.Contains(id.ToString()))
                return true;
            return false;
        }

    }
}
