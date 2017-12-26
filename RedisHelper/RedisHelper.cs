using System;
using StackExchange.Redis;
using ZentCloud.Common;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using RedisHelper.Enums;

namespace RedisHelper
{
    /// <summary>
    /// Redis
    /// </summary>
    public class RedisHelper
    {
        /// <summary>
        /// Redis 连接字符串 
        /// </summary>
        private static readonly string Coonstr = ConfigHelper.GetConfigString("RedisExchangeHosts");
        /// <summary>
        /// RedisKey前缀
        /// </summary>
        private static readonly string RedisPreKey = ConfigHelper.GetConfigString("RedisPreKey");
        /// <summary>
        /// 锁定对象
        /// </summary>
        private static object _locker = new Object();
        /// <summary>
        /// 连接实例
        /// </summary>
        private static ConnectionMultiplexer _instance = null;
        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = ConnectionMultiplexer.Connect(Coonstr);
                            
                        }
                    }
                }
                //注册如下事件
                _instance.ConnectionFailed += MuxerConnectionFailed;
                _instance.ConnectionRestored += MuxerConnectionRestored;
                _instance.ErrorMessage += MuxerErrorMessage;
                _instance.ConfigurationChanged += MuxerConfigurationChanged;
                _instance.HashSlotMoved += MuxerHashSlotMoved;
                _instance.InternalError += MuxerInternalError;
                return _instance;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        static RedisHelper()
        {
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        /// <summary>
        /// 这里的 MergeKey 用来拼接 Key 的前缀，具体不同的业务模块使用不同的前缀。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string MergeKey(string key)
        {            
            var mergeKey = RedisPreKey + key;

            mergeKey = mergeKey.ToLower();//全部转换成小写，控制大小写不作区分，否则redis 的key是区分大小写的

            return mergeKey;
        }

        /// <summary>
        /// 检查指定key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyExists(key);  //可直接调用
        }
        /// <summary>
        /// 检查指定key是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyExists(RedisKeyEnum redisKey)
        {
            return KeyExists(EnumToString(redisKey));

        }
        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDelete(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyDelete(key);
        }
        /// <summary>
        /// 模糊删除key
        /// </summary>
        /// <param name="pattern"></param>
        public static RedisResult KeyBatchDelete(string pattern)
        {
            pattern = MergeKey(pattern);
            return GetDatabase().ScriptEvaluate(LuaScript.Prepare(
                //Redis的keys模糊查询：
                " local ks = redis.call('KEYS', @keypattern) " + //local ks为定义一个局部变量，其中用于存储获取到的keys
                " for i=1,#ks,5000 do " +    //#ks为ks集合的个数, 语句的意思： for(int i = 1; i <= ks.Count; i+=5000)
                "     redis.call('del', unpack(ks, i, math.min(i+4999, #ks))) " + //Lua集合索引值从1为起始，unpack为解包，获取ks集合中的数据，每次5000，然后执行删除
                " end " +
                " return true "
                ),
                new { keypattern = pattern });//mykey*
        }
        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyDelete(RedisKeyEnum redisKey)
        {
            return KeyDelete(EnumToString(redisKey));
        }

        #region 字符串 String

        /// <summary>
        /// 获取String
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string StringGet(string key)
        {
            key = MergeKey(key);
            return GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 根据key获取缓存对象 (反序列化后)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key)
        {
            key = MergeKey(key);
            var value = GetDatabase().StringGet(key);
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return Deserialize<T>(value);
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(RedisKeyEnum redisKey)
        {
            string key = EnumToString(redisKey);
            return StringGet<T>(key);
        }
 

        /// <summary>
        /// 设置缓存 序列化后存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间间隔</param>
        public static void StringSetSerialize(string key, object value,TimeSpan? expiry=null)
        {
            key = MergeKey(key);
            GetDatabase().StringSet(key, Serialize(value),expiry);
            
        }
        /// <summary>
        /// 设置缓存 序列化后存储
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间间隔</param>
        public static void StringSetSerialize(RedisKeyEnum redisKey, object value,TimeSpan? expiry=null)
        {
            StringSetSerialize(EnumToString(redisKey), value,expiry);
        }
        /// <summary>
        /// 设置String
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间间隔</param>
        public static bool StringSet(string key, string value, TimeSpan? expiry = null)
        {
            key = MergeKey(key);
            return GetDatabase().StringSet(key, value, expiry);
        }

        #endregion
         
        #region 哈希 Hash
        /// <summary>
        /// 哈希设置 直接存储字符串
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">哈希字段值</param>
        /// <returns>true false</returns>
        private static bool HashSet(string key, string field, string value)
        {
            key = MergeKey(key);
            return GetDatabase().HashSet(key, field, value);
        }
        /// <summary>
        /// 哈希设置 直接存储字符串
        /// </summary>
        /// <param name="key">RedisKey 枚举</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">哈希字段值</param>
        /// <returns>true false</returns>
        public static bool HashSet(RedisKeyEnum enumKey, string field, string value)
        {
            string key =EnumToString(enumKey);
            return HashSet(key,field,value);
        }

        /// <summary>
        /// 哈希设置 序列化存储
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">哈希字段值</param>
        /// <returns>true false</returns>
        private static bool HashSetSerialize(string key, string field, object value)
        {
            key = MergeKey(key);
            return GetDatabase().HashSet(key, field, Serialize(value));
        }
        /// <summary>
        /// 哈希设置 序列化存储
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">哈希字段值</param>
        /// <returns>true false</returns>
        public static bool HashSetSerialize(RedisKeyEnum enumKey, string field, object value)
        {
            string key = EnumToString(enumKey);
            return HashSetSerialize(key, field, value);
        }

        /// <summary>
        /// 哈希读取 读取反序列化后的对象
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>反序列化后的对象</returns>
        private static T HashGet<T>(string key, string field)
        {
            key = MergeKey(key);
            return Deserialize<T>(GetDatabase().HashGet(key, field));
        }
        /// <summary>
        /// 哈希读取 读取反序列化后的对象
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>反序列化后的对象</returns>
        public static T HashGet<T>(RedisKeyEnum enumKey, string field)
        {
            string key = EnumToString(enumKey);
            return HashGet<T>(key,field);
        }
        /// <summary>
        /// 哈希读取 读取存储的字符串
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>存储的字符串</returns>
        private static string HashGet(string key, string field)
        {
            key = MergeKey(key);
            return GetDatabase().HashGet(key, field);
        }
        /// <summary>
        /// 哈希读取 读取存储的字符串
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>存储的字符串</returns>
        public static string HashGet(RedisKeyEnum enumKey, string field)
        {
            string key = EnumToString(enumKey);
            return HashGet(key, field);
        }
        /// <summary>
        /// 检查哈希是否存在
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>true false</returns>
        private static bool HashExists(string key, string field)
        {
            key = MergeKey(key);
            return GetDatabase().HashExists(key, field);
        }
        /// <summary>
        /// 检查哈希是否存在
        /// </summary>
        /// <param name="key">RedisKey 枚举</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>true false</returns>
        public static bool HashExists(RedisKeyEnum enumKey, string field)
        {
            string key = EnumToString(enumKey);
            return HashExists(key, field);
        }
        /// <summary>
        /// 删除哈希
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>true false</returns>
        public static bool HashDelete(string key, string field)
        {
            key = MergeKey(key);
            return GetDatabase().HashDelete(key, field);
        }
        /// <summary>
        /// 删除哈希
        /// </summary>
        /// <param name="key">RedisKey 枚举</param>
        /// <param name="field">哈希字段名称</param>
        /// <returns>true false</returns>
        public static bool HashDelete(RedisKeyEnum enumKey, string field)
        {
            string key = EnumToString(enumKey);
           return HashDelete(key, field);

        }
        /// <summary>
        /// 获取指定key的所有哈希
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static HashEntry[] HashGetAll(string key)
        {
            key = MergeKey(key);
            return GetDatabase().HashGetAll(key);
        }

        /// <summary>
        /// 获取指定哈希的所有字段
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static RedisValue[] HashKeys(string key)
        {
            key = MergeKey(key);
            return GetDatabase().HashKeys(key);
        }
        /// <summary>
        /// 获取指定指定哈希的所有值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static RedisValue[] HashValues(string key)
        {
            key = MergeKey(key);
            return GetDatabase().HashValues(key);
        }
        /// <summary>
        /// 减去指定哈希的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">减去的值</param>
        /// <returns></returns>
        public static long HashDecrement(string key, string field, long value = 1)
        {
            key = MergeKey(key);
            return GetDatabase().HashDecrement(key, field, value);
        }
        ///// <summary>
        ///// 实现递减
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static long HashDecrement(string key, string value)
        //{
        //    key = MergeKey(key);
        //    return GetDatabase().HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        //}
        /// <summary>
        /// 增加指定哈希的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="field">哈希字段名称</param>
        /// <param name="value">增加的值</param>
        /// <returns></returns>
        public static long HashIncrement(string key, string field, long value = 1)
        {
            key = MergeKey(key);
            return GetDatabase().HashIncrement(key, field, value);
        }
        /// <summary>
        /// 获取指定哈希的数量
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static long HashLength(string key)
        {
            key = MergeKey(key);
            return GetDatabase().HashLength(key);
        }
        /// <summary>
        /// 哈希搜索
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="pattern">匹配规则</param>
        /// <param name="pageSize">每页返回条数</param>
        /// <param name="cursor"></param>
        /// <param name="pageOffset"></param>
        /// <returns></returns>
        public static IEnumerable<HashEntry> HashScan(string key, string pattern = null, int pageSize = 10, long cursor = 0, int pageOffset = 0)
        {
            key = MergeKey(key);
            return GetDatabase().HashScan(key, pattern, pageSize, cursor, pageOffset);
        }
        /// <summary>
        /// 哈希搜索
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="pattern">匹配规则</param>
        /// <param name="pageSize">每页返回条数</param>
        /// <returns></returns>
        public static IEnumerable<HashEntry> HashScan(string key, string pattern = null, int pageSize = 10)
        {
            key = MergeKey(key);
            return GetDatabase().HashScan(key, pattern, pageSize);
        }

        #endregion

        #region 列表 List

        /// <summary>
        /// 根据索引获取元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引 从0开始</param>
        /// <returns></returns>
        public static string ListGetByIndex(string key, long index)
        {
            key = MergeKey(key);
            return GetDatabase().ListGetByIndex(key, index);
        }
        /// <summary>
        /// 在某个值之后插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListInsertAfter(string key, string pivot, string value)
        {
            key = MergeKey(key);
            return GetDatabase().ListInsertAfter(key, pivot, value);
        }
        /// <summary>
        /// 在某个值之前插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListInsertBefore(string key, string pivot, string value)
        {
            key = MergeKey(key);
            return GetDatabase().ListInsertBefore(key, pivot, value);
        }
        /// <summary>
        /// 删除并返回第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ListLeftPop(string key)
        {
            key = MergeKey(key);
            return GetDatabase().ListLeftPop(key);
        }
        /// <summary>
        /// 左侧插入元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListLeftPush(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().ListLeftPush(key,value);
        }
        /// <summary>
        /// 获取列表元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListLength(string key)
        {
            key = MergeKey(key);
            return GetDatabase().ListLength(key);
        }
        /// <summary>
        /// 获取指定索引范围内的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static RedisValue[] ListRange(string key, long start=0, long stop=-1)
        {
            key = MergeKey(key);
            return GetDatabase().ListRange(key,start,stop);
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListRemove(string key, string value,long count=0)
        {
            key = MergeKey(key);
            return GetDatabase().ListRemove(key,value,count);
        }
        /// <summary>
        /// 删除并返回最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ListRightPop(string key)
        {
            key = MergeKey(key);
            return GetDatabase().ListRightPop(key);
        }
        /// <summary>
        /// 删除原集合最后一个元素 并把元素放到新key
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <param name="destinationKey"></param>
        /// <returns></returns>
        public static string ListRightPopLeftPush(string sourceKey, string destinationKey)
        {
            sourceKey = MergeKey(sourceKey);
            destinationKey = MergeKey(destinationKey);
            return GetDatabase().ListRightPopLeftPush(sourceKey,destinationKey);
        }
        /// <summary>
        /// 在右侧插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListRightPush(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().ListRightPush(key,value);
        }
        /// <summary>
        /// 根据索引位置设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void ListSetByIndex(string key, long index, string value)
        {
            key = MergeKey(key);
            GetDatabase().ListSetByIndex(key, index,value);
        }
        /// <summary>
        /// 删除指定区间的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        public static void ListTrim(string key, long start, long stop)
        {
            key = MergeKey(key);
            GetDatabase().ListTrim(key, start, stop);
        }

        #endregion

        #region 集合 Set
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetAdd(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SetAdd(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static RedisValue[] SetCombine(SetOperation operation, string firstKey, string secondKey)
        {
            firstKey = MergeKey(firstKey);
            secondKey = MergeKey(secondKey);
            return GetDatabase().SetCombine(operation,firstKey, secondKey);
        }
        /// <summary>
        /// 比较集合并把结果放到新key中
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="destinationKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="secondKey"></param>
        /// <returns></returns>
        public static long SetCombineAndStore(SetOperation operation, string destinationKey, string firstKey, string secondKey)
        {
            firstKey = MergeKey(firstKey);
            secondKey = MergeKey(secondKey);
            return GetDatabase().SetCombineAndStore(operation, destinationKey, firstKey, secondKey);
        }
        /// <summary>
        /// 集合是否包含指定值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetContains(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SetContains(key, value);
        }
        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SetLength(string key)
        {
            key = MergeKey(key);
            return GetDatabase().SetLength(key);
        }
        /// <summary>
        /// 获取集合的所有元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RedisValue[] SetMembers(string key)
        {
            key = MergeKey(key);
            return GetDatabase().SetMembers(key);
        }
        /// <summary>
        /// 移动元素到目标key
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <param name="destinationKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetMove(string sourceKey, string destinationKey, string value)
        {
            sourceKey = MergeKey(sourceKey);
            destinationKey = MergeKey(destinationKey);
            return GetDatabase().SetMove(sourceKey,destinationKey,value);
        }
        /// <summary>
        /// 随机删除一个元素并返回
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SetPop(string key)
        {
            key = MergeKey(key);
            return GetDatabase().SetPop(key);
        }
        /// <summary>
        /// 随机返回一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SetRandomMember(string key)
        {
            key = MergeKey(key);
            return GetDatabase().SetRandomMember(key);
        }
        /// <summary>
        /// 随机返回指定数量的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static RedisValue[] SetRandomMembers(string key, long count)
        {
            key = MergeKey(key);
            return GetDatabase().SetRandomMembers(key,count);
        }
        /// <summary>
        ///根据元素值删除元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetRemove(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SetRemove(key,value);
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> SetScan(string key, string pattern,int pageSize)
        {
            key = MergeKey(key);
            return GetDatabase().SetScan(key, pattern, pageSize);
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <param name="cursor"></param>
        /// <param name="pageOffSet"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> SetScan(string key, string pattern, int pageSize=10,long cursor=0,int pageOffSet=0)
        {
            key = MergeKey(key);
            return GetDatabase().SetScan(key, pattern, pageSize, cursor,pageOffSet);
        }

        #endregion

        #region 有序集合 SortSet
        /// <summary>
        ///添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(string key, string value, double score)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetAdd(key, value, score);
        }
        /// <summary>
        /// 比较两个集合并把比较结果存储到目标key
        /// </summary>
        /// <param name="operation">比较操作</param>
        /// <param name="destinationKey">目标key</param>
        /// <param name="firstKey">第一个</param>
        /// <param name="secondKey">第二个</param>
        /// <returns></returns>
        public static long SortedSetCombineAndStore(SetOperation operation, string destinationKey, string firstKey, string secondKey)
        {
            destinationKey = MergeKey(destinationKey);
            firstKey = MergeKey(firstKey);
            secondKey = MergeKey(secondKey);
            return GetDatabase().SortedSetCombineAndStore(operation, destinationKey, firstKey, secondKey);
        }
        /// <summary>
        /// 减掉分值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static double SortedSetDecrement(string key, string value, double score)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetDecrement(key, value, score);
        }
        /// <summary>
        /// 增加分值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static double SortedSetIncrement(string key, string value, double score)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetIncrement(key, value, score);
        }
        /// <summary>
        /// 获取集合元素数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static double SortedSetLength(string key)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetLength(key);
        }
        /// <summary>
        /// 获取指定区间分数元素的数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static long SortedSetLengthByValue(string key, int minValue, int maxValue)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetLengthByValue(key,minValue,maxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static RedisValue[] SortedSetRangeByRank(string key, long start=0, long stop=-1, Order order=Order.Ascending)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRangeByRank(key, start, stop, order);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static SortedSetEntry[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRangeByRankWithScores(key, start, stop, order);
        }
        /// <summary>
        /// 根据分类区间返回元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static RedisValue[] SortedSetRangeByScore(string key, double start = 0, double stop = 0)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRangeByScore(key, start, stop);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static SortedSetEntry[] SortedSetRangeByScoreWithScores(string key, double start = 0, double stop = 0)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRangeByScoreWithScores(key, start, stop);
        }
        /// <summary>
        /// 根据元素值最大最小值获取元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static RedisValue[] SortedSetRangeByValue(string key, double minValue, double maxValue)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRangeByValue(key, minValue, maxValue);
        }
        /// <summary>
        /// 获取指定元素的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? SortedSetRank(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRank(key, value);
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SortedSetRemove(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRemove(key, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRemoveRangeByRank(key, start, stop);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static long SortedSetRemoveRangeByScore(string key, long start, long stop)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRemoveRangeByScore(key, start, stop);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static long SortedSetRemoveRangeByValue(string key, long minValue, long maxValue)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetRemoveRangeByValue(key, minValue, maxValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<SortedSetEntry> SortedSetScan(string key, string pattern, int pageSize = 10)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetScan(key, pattern, pageSize);
        }
        /// <summary>
        /// 获取指定元素的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double? SortedSetScore(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().SortedSetScore(key, value);
        }
        #endregion


        /// <summary>
        /// 异步设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task SetAsync(string key, object value)
        {
            key = MergeKey(key);
            await GetDatabase().StringSetAsync(key, Serialize(value));
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<object> GetAsync(string key)
        {
            key = MergeKey(key);
            object value = await GetDatabase().StringGetAsync(key);
            return value;
        }

        /// <summary>
        /// 实现递增
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long StringIncrement(string key)
        {
            key = MergeKey(key);
            //三种命令模式
            //Sync,同步模式会直接阻塞调用者，但是显然不会阻塞其他线程。
            //Async,异步模式直接走的是Task模型。
            //Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现：
            return GetDatabase().StringIncrement(key, flags: CommandFlags.FireAndForget);
        }



        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            WriteInfoLog("Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            WriteInfoLog("ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            var logStr = "ConnectionRestored: " + e.EndPoint;
            if (e.Exception != null)
            {
                logStr += e.Exception.Message;
            }
            WriteInfoLog(logStr);
        }
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            WriteInfoLog("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            WriteInfoLog("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }
        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            WriteInfoLog("InternalError:Message" + e.Exception.Message);
        }

        //场景不一样，选择的模式便会不一样，大家可以按照自己系统架构情况合理选择长连接还是Lazy。
        //建立连接后，通过调用ConnectionMultiplexer.GetDatabase 方法返回对 Redis Cache 数据库的引用。从 GetDatabase 方法返回的对象是一个轻量级直通对象，不需要进行存储。

        /// <summary>
        /// 使用的是Lazy，在真正需要连接时创建连接。
        /// 延迟加载技术
        /// 微软azure中的配置 连接模板
        /// </summary>
        //private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    //var options = ConfigurationOptions.Parse(constr);
        //    ////options.ClientName = GetAppName(); // only known at runtime
        //    //options.AllowAdmin = true;
        //    //return ConnectionMultiplexer.Connect(options);
        //    ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Coonstr);
        //    muxer.ConnectionFailed += MuxerConnectionFailed;
        //    muxer.ConnectionRestored += MuxerConnectionRestored;
        //    muxer.ErrorMessage += MuxerErrorMessage;
        //    muxer.ConfigurationChanged += MuxerConfigurationChanged;
        //    muxer.HashSlotMoved += MuxerHashSlotMoved;
        //    muxer.InternalError += MuxerInternalError;
        //    return muxer;
        //});


        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景
        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(string channel, string message)
        {
            ISubscriber sub = Instance.GetSubscriber();
            //return sub.Publish("messages", "hello");
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 在消费者端得到该消息并输出
        /// </summary>
        /// <param name="channelFrom"></param>
        /// <returns></returns>
        public static void Subscribe(string channelFrom)
        {
            ISubscriber sub = Instance.GetSubscriber();
            sub.Subscribe(channelFrom, (channel, message) =>
            {
                Console.WriteLine((string)message);
            });
        }
        #endregion

        /// <summary>
        /// GetServer方法会接收一个EndPoint类或者一个唯一标识一台服务器的键值对
        /// 有时候需要为单个服务器指定特定的命令
        /// 使用IServer可以使用所有的shell命令，比如：
        /// DateTime lastSave = server.LastSave();
        /// ClientInfo[] clients = server.ClientList();
        /// 如果报错在连接字符串后加 ,allowAdmin=true;
        /// </summary>
        /// <returns></returns>
        public static IServer GetServer(string host, int port)
        {
            IServer server = Instance.GetServer(host, port);
            return server;
        }

        /// <summary>
        /// 获取全部终结点
        /// </summary>
        /// <returns></returns>
        public static EndPoint[] GetEndPoints()
        {
            EndPoint[] endpoints = Instance.GetEndPoints();
            return endpoints;
        }

        private static void WriteInfoLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\redisexchangelog.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }
        /// <summary>
        /// 枚举转成String
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string EnumToString(object o)
        {
            Type t = o.GetType();
            string s = o.ToString();
            EnumDescriptionAttribute[] os = (EnumDescriptionAttribute[])t.GetField(s).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (os != null && os.Length == 1)
            {
                return os[0].Text;
            }
            return s;
        }
        private class EnumDescriptionAttribute : Attribute
        {
            private string _text = "";
            public string Text
            {
                get { return this._text; }
            }
            public EnumDescriptionAttribute(string text)
            {
                _text = text;
            }

        }


    }


}
