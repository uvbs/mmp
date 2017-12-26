using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery.admin
{
    public partial class TEST : System.Web.UI.Page
    {
        
        public System.Text.StringBuilder sb = new System.Text.StringBuilder();
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        public WXLotteryV1 model;
        protected void Page_Load(object sender, EventArgs e)
        {
            model= bll.Get<WXLotteryV1>(string.Format("LotteryID={0}", int.Parse(Request["id"])));
            //中奖算法
            //先声明 list用于从中间取随机数
            List<AwardModel> AwardModelList = new List<AwardModel>();
            List<WXAwardsV1> AwardsList = bll.GetAwardsListV1(model.LotteryID);
            foreach (var item in AwardsList)
            {

                for (int i = 1; i <= item.Probability; i++)
                {
                    AwardModel m = new AwardModel();
                    m.AwardID = item.AutoID;
                    m.PrizeName = item.PrizeName;
                    AwardModelList.Add(m);
                }

            }
            if (AwardsList.Sum(p => p.Probability) < 100)//总中奖概率小于100
            {
                for (int i = 1; i <= (100 - (AwardsList.Sum(p => p.Probability))); i++)
                {
                    AwardModel m = new AwardModel();
                    m.AwardID = 0;
                    m.PrizeName = "谢谢参与";
                    AwardModelList.Add(m);

                }
            }
            List<WXLotteryWinningDataV1> WinData = bll.GetList<WXLotteryWinningDataV1>(string.Format("LotteryId={0}", model.LotteryID));


            List<AwardModel> win=new List<AwardModel>();
            for (int i = 1; i <=int.Parse(Request["count"]); i++)
            {
                
                sb.AppendLine(i.ToString() + "<br/>");
                AwardModelList = GetRandomList<AwardModel>(AwardModelList);
            start:
                Random rand = new Random();
                int index = rand.Next(0, AwardModelList.Count);
                if (AwardModelList[index].AwardID > 0)//随机数中奖
                {

                    if (win.Where(p => p.AwardID == AwardModelList[index].AwardID).Count() < (AwardsList.Single(p => p.AutoID.Equals(AwardModelList[index].AwardID)).PrizeCount))
                    {
                        AwardModel m = new AwardModel();
                        m.AwardID = AwardModelList[index].AwardID;
                        m.PrizeName = AwardModelList[index].PrizeName;
                        win.Add(m);
                        sb.AppendLine(string.Format(AwardModelList[index].PrizeName) + "<br/>");

                    }
                    else
                    {

                        goto start;
                    }


                }
                else
                {
                    //随机数未中奖
                    sb.AppendLine("谢谢参与" + "<br/>");

                }
                

            }





        }
        //打乱数组顺序
        int GenerateDigit(Random rng)
        {
            // Assume there'd be more logic here really  
            return rng.Next(10);
        }
        private class ScratchJsonModel
        {
            /// <summary>
            /// 抽奖编号
            /// </summary>
            public int awardId { get; set; }
            /// <summary>
            /// 奖项名称
            /// </summary>
            public string awardName { get; set; }
            /// <summary>
            /// 是否中奖
            /// </summary>
            public bool isAward { get; set; }
            /// <summary>
            /// 是否已经开始
            /// </summary>
            public bool isStart { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string startTime { get; set; }

        }
        /// <summary>
        /// 检查是否领过奖品
        /// </summary>
        /// <param name="UserId">当前用户的id</param>
        /// <param name="autoId">活动编号</param>
        /// <returns>返回是否领奖成功</returns>
        private bool IsUserGetPrizeV1(string UserId, int autoId)
        {
            WXLotteryRecordV1 wxlRecord = bll.Get<WXLotteryRecordV1>(" UserId='" + UserId + "' And LotteryId=" + autoId);
            if (wxlRecord != null)
            {
                if (wxlRecord.IsGetPrize == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private class AwardModel
        {
            /// <summary>
            /// 奖品ID
            /// </summary>
            public int AwardID { get; set; }
            /// <summary>
            /// 奖品名称
            /// </summary>
            public string PrizeName { get; set; }

        }



        //随机数组排序
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

    }
}