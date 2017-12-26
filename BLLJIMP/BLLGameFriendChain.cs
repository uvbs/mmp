using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 五步会 HRLOVE BLL
    /// </summary>
    public class BLLGameFriendChain : BLL
    {
        ////获取count 个 Friend 随机列表,
        //public List<GameFriendChain>  GetRandomFriendList(int count)
        //{


        //    return GetList<GameFriendChain>(40, "", "AutoId asc");

        //    int totalCount = this.GetCount<GameFriendChain>("");
        //    List<int> idList = new List<int>();

        //    //获取count个随机id
        //    for (int i = 0; idList.Count < count && idList.Count < totalCount; ++i)
        //    {
        //        int id = new Random(i*i).Next(totalCount);
        //        if (!idList.Contains(id))
        //        {
        //            idList.Add(id);
        //        }
        //    }

        //    string ids = string.Join(",", idList.ToArray());
        //    List<GameFriendChain> listFriend = this.GetList<GameFriendChain>(string.Format("AutoId in ({0})", ids));

        //    //如果取到的数量不够（比如某些autoid的记录被删掉），继续一次取一个
        //    for (int j = 0; listFriend.Count < count && listFriend.Count < totalCount; ++j)
        //    {
        //        int id = new Random(j*j).Next(totalCount);
        //        if (!idList.Contains(id))
        //        {
        //            idList.Add(id);
        //            GameFriendChain item = this.Get<GameFriendChain>(string.Format("AutoId = {0}", id));
        //            if (item != null)
        //            {
        //                listFriend.Add(item);
        //            }
        //        }
        //    }

        //    return listFriend;
        //}

        //获取count 个 Friend 随机列表, 以后可以加到DLLEngine里面
        public List<GameFriendChain> GetRandomFriendList(int count)
        {


            BLLGameFriendChain bllFriendChain = new BLLGameFriendChain();
            int totalCount = bllFriendChain.GetCount<GameFriendChain>("");
            if (totalCount < count)
            {
                count = totalCount;
            }
            List<int> idList = new List<int>();

            //获取count个随机id
            for (int i = 0; idList.Count < count && idList.Count < totalCount; ++i)
            {
                int id = new Random().Next(totalCount) + 1; //随机数从0开始，行数从1开始，所以加1
                if (!idList.Contains(id))
                {
                    idList.Add(id);
                }
            }
            string ids = string.Join(",", idList.ToArray());

            string strQuery = string.Format(@"select * from 
                   (select row_number() over(order by AutoId) as COL_ROWNUMBER, * from ZCJ_GameFriendChain) TABLE_ORDERDATA
                    where COL_ROWNUMBER in ({0})", ids);
            System.Data.DataSet ds = BLL.Query(strQuery);
            return BLL.DataSetToModelList<GameFriendChain>(ds);
        }
    }
}
