using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo
{
    public class WeiboAPI
    {
        private string urlBase = "https://api.weibo.com/2/";
        Common.HttpInterFace m = new Common.HttpInterFace();
        Encoding encodingBase = Encoding.GetEncoding("utf-8");

        private string accessTokenBase;

        public string AccessTokenBase
        {
            get { return accessTokenBase; }
        }

        public WeiboAPI(string accessToken)
        {
            this.accessTokenBase = accessToken;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public T GetCommand<T>(string data, string url, string method = "get")
        {
            data += "&access_token=" + accessTokenBase;

            string result = string.Empty;

            try
            {
                if (method.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                    result = m.GetWebRequest(data, url, encodingBase);
                else if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                    result = m.PostWebRequest(data, url, encodingBase);

                if (result == "[]" || string.IsNullOrWhiteSpace(result))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                return default(T);
                //throw ex;
            }

        }

        #region Statuses
        /// <summary>
        /// 返回最新的200条公共微博，返回结果非完全实时
        /// </summary>
        /// <param name="count">单页返回的记录条数，最大不超过200，默认为20。</param>
        /// <returns></returns>
        public Statuses.PublicTimeline Statuses_PublicTimeline(int count = 20)
        {
            Statuses.PublicTimeline result = new Statuses.PublicTimeline();
            string url = string.Format("{0}{1}", urlBase, "statuses/public_timeline.json");
            StringBuilder data = new StringBuilder();
            data.AppendFormat("count={0}", count);

            try
            {
                result = GetCommand<Statuses.PublicTimeline>(data.ToString(), url);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Friendships
        /// <summary>
        /// 获取用户的关注列表
        /// </summary>
        /// <param name="uid">需要查询的用户UID。</param>
        /// <param name="count">单页返回的记录条数，默认为50，最大不超过200。</param>
        /// <param name="cursor">返回结果的游标，下一页用返回值里的next_cursor，上一页用previous_cursor，默认为0。</param>
        /// <param name="trim_status">返回值中user字段中的status字段开关，0：返回完整status字段、1：status字段仅返回status_id，默认为1。</param>
        /// <returns></returns>
        public Friendships.Friends Friendships_Friends(string uid = "", int count = 50, int cursor = 0, int trim_status = 1)
        {
            Friendships.Friends result = new Friendships.Friends();

            string url = string.Format("{0}{1}", urlBase, "friendships/friends.json");
            string data = string.Format("uid={0}&count={1}&cursor={2}&trim_status={3}",
                    uid,
                    count,
                    cursor,
                    trim_status
                );

            try
            {
                result = GetCommand<Friendships.Friends>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取用户的关注列表
        /// </summary>
        /// <param name="uid">需要查询的用户UID。</param>
        /// <param name="count">单页返回的记录条数，默认为50，最大不超过200。</param>
        /// <param name="cursor">返回结果的游标，下一页用返回值里的next_cursor，上一页用previous_cursor，默认为0。</param>
        /// <param name="trim_status">返回值中user字段中的status字段开关，0：返回完整status字段、1：status字段仅返回status_id，默认为1。</param>
        /// <returns></returns>
        public Friendships.Followers Friendships_Followers(string uid = "", int count = 50, int cursor = 0, int trim_status = 1)
        {
            Friendships.Followers result = new Friendships.Followers();

            string url = string.Format("{0}{1}", urlBase, "friendships/followers.json");
            string data = string.Format("uid={0}&count={1}&cursor={2}&trim_status={3}",
                    uid,
                    count,
                    cursor,
                    trim_status
                );

            try
            {
                result = GetCommand<Friendships.Followers>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取用户的双向关注列表，即互粉列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="count">单页返回的记录条数，默认为50。</param>
        /// <param name="page">返回结果的页码，默认为1。</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public Friendships.Bilateral Friendships_Bilateral(string uid = "", int count = 50, int page = 0, int sort = 0)
        {
            Friendships.Bilateral result = new Friendships.Bilateral();

            string url = string.Format("{0}{1}", urlBase, "friendships/friends/bilateral.json");
            string data = string.Format("uid={0}&count={1}&page={2}&sort={3}",
                    uid,
                    count,
                    page,
                    sort
                );

            try
            {
                result = GetCommand<Friendships.Bilateral>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 获取用户的双向关注ID列表，即互粉ID列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public Friendships.BilateralIds Friendships_BilateralIds(string uid = "", int count = 50, int page = 0, int sort = 0)
        {
            Friendships.BilateralIds result = new Friendships.BilateralIds();

            string url = string.Format("{0}{1}", urlBase, "friendships/friends/bilateral/ids.json");
            string data = string.Format("uid={0}&count={1}&page={2}&sort={3}",
                    uid,
                    count,
                    page,
                    sort
                );

            try
            {
                result = GetCommand<Friendships.BilateralIds>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public bool Friendships_Destroy(string uid)
        {
            string url = string.Format("{0}{1}", urlBase, "friendships/friends/bilateral/ids.json");
            string data = string.Format("uid={0}",
                    uid
                );

            return false;
        }
        #endregion

        #region Users
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public Users.Show Users_Show(string uid)
        {
            Users.Show result = new Users.Show();

            string url = string.Format("{0}{1}", urlBase, "users/show.json");
            string data = string.Format("uid={0}",
                    uid
                );

            try
            {
                result = GetCommand<Users.Show>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }


        /// <summary>
        /// 批量获取用户的粉丝数、关注数、微博数
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public Users.Count[] Users_Counts(string uids)
        {
            Users.Count[] result;

            string url = string.Format("{0}{1}", urlBase, "users/counts.json");
            string data = string.Format("uids={0}",
                    uids
                );

            try
            {
                result = GetCommand<Users.Count[]>(data, url);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }
        #endregion

        #region Search

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="query">查询关键字(必须做URLencoding)</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Search.User> Search_Suggestions_Users(string query, int count = 200)
        {
            List<Search.User> result = new List<Search.User>();

            //先获取查询结果列表
            string url = string.Format("{0}{1}", urlBase, "search/suggestions/users.json");
            string data = string.Format("q={0}&count={1}",
                    query,
                    count.ToString()
                );
            try
            {
                result = GetCommand<Search.User[]>(data, url).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }


        #endregion

        #region Place
        /// <summary>
        /// 获取附近发位置微博的人
        /// </summary>
        /// <param name="lat">动态发生的纬度，有效范围：-90.0到+90.0，+表示北纬，默认为0.0。</param>
        /// <param name="log">动态发生的经度，有效范围：-180.0到+180.0，+表示东经，默认为0.0。</param>
        /// <param name="count">单页返回的记录条数，最大为50，默认为20。</param>
        /// <param name="page">返回结果的页码，默认为1。</param>
        /// <param name="range">查询范围半径，默认为2000，最小500，最大为11132。</param>
        /// <param name="sort">排序方式，0：按时间、1：按距离、2：按社会化关系，默认为2。</param>
        /// <param name="gender">性别过滤，0：全部、1：男、2：女，默认为0。</param>
        /// <param name="level">用户级别过滤，0：全部、1：普通用户、2：VIP用户、7：达人，默认为0。</param>
        /// <param name="offset">传入的经纬度是否是纠偏过，0：没纠偏、1：纠偏过，默认为1。</param>
        /// <returns></returns>
        public Place.NearbyUsers Place_NearbyUsers(
            float lat,
            float log,
            int page = 1,
            int count = 20,
            int range = 2000,
            int sort = 1,
            int gender = 0,
            int level = 0,
            int offset = 1)
        {
            Place.NearbyUsers result = new Place.NearbyUsers();

            //先获取查询结果列表
            string url = string.Format("{0}{1}", urlBase, "place/nearby_users/list.json");

            StringBuilder data = new StringBuilder();
            data.AppendFormat("lat={0}", lat);
            data.AppendFormat("&long={0}", log);
            data.AppendFormat("&count={0}", count);
            data.AppendFormat("&page={0}", page);
            data.AppendFormat("&range={0}", range);
            data.AppendFormat("&sort={0}", sort);
            data.AppendFormat("&gender={0}", gender);
            data.AppendFormat("&level={0}", level);
            data.AppendFormat("&offset={0}", offset);

            try
            {
                result = GetCommand<Place.NearbyUsers>(data.ToString(), url);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="total">总数</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public int GetTotalPage(decimal total, decimal pageSize)
        {
            try
            {
                return (int)Math.Ceiling(total / pageSize);
            }
            catch { }
            return 0;
        }

        /// <summary>
        /// 获取缓存列表分页数据
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">从1开始</param>
        /// <param name="list"></param>
        /// <returns>分页数据</returns>
        public List<T> GetPageList<T>(int pageSize, int pageIndex, List<T> list)
        {
            if (pageIndex > 0)
                --pageIndex;
            int starIndex = pageIndex * pageSize;

            List<T> result = new List<T>();

            for (int i = 0; i < pageSize; i++)
            {
                try
                {
                    int j = starIndex + i;
                    if (j > list.Count - 1)
                        break;
                    result.Add(list[j]);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return result;
        }

        /// <summary>
        /// 计算页码游标值
        /// </summary>
        /// <param name="total">总数量</param>
        /// <param name="pageSize">一页的数量</param>
        /// <returns></returns>
        public List<int> GetPageCursorList(decimal total, decimal pageSize)
        {
            List<int> result = new List<int>();

            //求出总页数
            int totalPage = GetTotalPage(total, pageSize);

            int j = 0;

            //求出Cursor游标
            for (int i = 0; i <= totalPage; i++)
            {
                result.Add(j);
                j += (int)pageSize;
            }

            return result;
        }

    }
}
