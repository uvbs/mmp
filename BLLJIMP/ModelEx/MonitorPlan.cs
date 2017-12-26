using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class MonitorPlan : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 点击人次
        /// </summary>
        public int ClickCount
        {
            get
            {
                int count = 0;
                //count = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(string.Format("MonitorPlanID={0} and EventType=1", _monitorplanid));
                return count;
               
                

            }



        }


        /// <summary>
        /// 点击人数
        /// </summary>
        public int DistinctClickCount
        {
            get
            {
                int count = 0;
                //count = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("MonitorPlanID={0} and EventType=1", _monitorplanid));
                return count;



            }



        }


        /// <summary>
        /// 打开人次
        /// </summary>
        public int OpenCount
        {
            get
            {
                int count = 0;
                //count = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(string.Format("MonitorPlanID={0} and EventType=0", _monitorplanid));
                //if (count>0)
                //{
                //    return count;
                //}
                //else
                //{
                //    return new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>(string.Format("MonitorPlanID={0} and EventType=1", _monitorplanid));
                //}
                return count;
                
                }



        }



        


        /// <summary>
        ///打开人数
        /// </summary>
        public int DistinctOpenCount
        {
            get
            {

                int count = 0;
                //count = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("MonitorPlanID={0} and EventType=0", _monitorplanid));
                //if (count>0)
                //{
                //     return count;
                //}
                //else
                //{
                // return   new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("MonitorPlanID={0} and EventType=1", _monitorplanid));
                //}

                return count;


            }



        }


       

        /// <summary>
        /// 任务下的链接数
        /// </summary>
        public int LinkCount {

            get
            {
                int count = 0;
                //count = new BLL("").GetCount<ZentCloud.BLLJIMP.Model.MonitorLinkInfo>(string.Format("MonitorPlanID={0}", _monitorplanid));
                return count;

                

            }
        
        }

    }
}
