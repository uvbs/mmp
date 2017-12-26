using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 储值卡
    /// </summary>
    public class BLLStoredValueCard : BLL
    {
        #region 储值卡主表
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string GetWhereString(string websiteOwner, string status, string keyword)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(status)) sbWhere.AppendFormat(" And Status={0}", status);
            if (!string.IsNullOrWhiteSpace(keyword)) sbWhere.AppendFormat(" And Name Like '%{0}%'", keyword);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 查询储值卡列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<StoredValueCard> GetList(int rows, int page, string websiteOwner, string status, string keyword)
        {
            return GetLit<StoredValueCard>(rows, page, GetWhereString(websiteOwner, status, keyword), "AutoId Desc");
        }
        /// <summary>
        /// 查询储值卡数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="status"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int GetCount(string websiteOwner, string status, string keyword)
        {
            return GetCount<StoredValueCard>(GetWhereString(websiteOwner, status, keyword));
        }

        public StoredValueCard GetStoredValueCard(int cardId)
        {
            return Get<StoredValueCard>(string.Format(" WebsiteOwner='{0}' AND AutoId={1} ", WebsiteOwner, cardId));
        }

        public StoredValueCardRecord GetStoredValueCardRecord(int autoid)
        {
            return Get<StoredValueCardRecord>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", WebsiteOwner, autoid));
        }
        /// <summary>
        /// 新建储值卡
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <param name="maxCount"></param>
        /// <param name="validTo"></param>
        /// <param name="bgImg"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="createUserId"></param>
        /// <returns></returns>
        public bool AddCard(string name, decimal amount, int maxCount, DateTime? validTo, string bgImg, string websiteOwner, string createUserId)
        {
            StoredValueCard card = new StoredValueCard();
            card.Name = name;
            card.Amount = amount;
            card.MaxCount = maxCount;
            if (validTo.HasValue) card.ValidTo = validTo;
            card.BgImage = bgImg;
            card.WebsiteOwner = websiteOwner;
            card.CreateUserId = createUserId;
            card.CreateDate = DateTime.Now;
            card.ModifyUserId = createUserId;
            card.ModifyDate = card.CreateDate;

            return Add(card);
        }
        /// <summary>
        /// 修改储值卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <param name="maxCount"></param>
        /// <param name="validTo"></param>
        /// <param name="bgImg"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="createUserId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateCard(string id, string name, decimal amount, int maxCount, DateTime? validTo,
            string bgImg, string createUserId, string websiteOwner, out string msg)
        {
            msg = "";
            StoredValueCard card = GetByKey<StoredValueCard>("AutoId", id, websiteOwner: websiteOwner);
            if (card == null)
            {
                msg = "储值卡未找到";
                return false;
            }
            card.Name = name;
            if (card.Amount != amount)
            {
                if (GetCountByKey<StoredValueCardRecord>("CardId", id, websiteOwner: websiteOwner) > 0)
                {
                    msg = "储值卡存在发放记录，禁止修改金额";
                    return false;
                }
                card.Amount = amount;
            }
            card.MaxCount = maxCount;
            if (validTo.HasValue)
            {
                card.ValidTo = validTo;
            }
            else if (card.ValidTo.HasValue)
            {
                card.ValidTo = null;
            }
            card.BgImage = bgImg;
            card.ModifyUserId = createUserId;
            card.ModifyDate = DateTime.Now;
            if (!Update(card))
            {
                msg = "修改储值卡失败";
                return false;
            }
            return true;
        }

        #endregion 储值卡主表

        #region 储值卡发放表

        /// <summary>
        /// 发放查询条件
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="status">
        /// 当userId为空时（做后台查询）0未使用 1已使用 2已过期 3初发放 4已转赠
        /// 当userId不为空时（做前台查询）0未使用 1已使用 2已转赠 3已过期
        /// </param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GetRecordWhereString(string cardId, string websiteOwner, string status, string userId)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(cardId)) sbWhere.AppendFormat(" And CardId ={0}", cardId);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                if (status == "0")
                {  //未使用（含转赠未过期）
                    sbWhere.AppendFormat(" And ( ");
                    sbWhere.AppendFormat(" (Status=0 And UserId='{0}' And (ValidTo Is Null OR ValidTo >= GetDate())) ", userId);
                    sbWhere.AppendFormat(" Or (Status=1 And ToUserId='{0}' And (ValidTo Is Null OR ValidTo >= GetDate())) ", userId);
                    sbWhere.AppendFormat(" ) ");
                }
                else if (status == "1")
                { //已使用
                    sbWhere.AppendFormat(" And Status=9 And ((UserId='{0}' And ToUserId Is Null) Or (ToUserId Is Not Null And ToUserId='{0}')) ", userId);
                }
                else if (status == "2")
                { //已转赠
                    sbWhere.AppendFormat(" And ( ");
                    sbWhere.AppendFormat(" (Status=1 And UserId='{0}') ", userId);
                    sbWhere.AppendFormat(" Or (Status=9 And ToUserId Is Not Null And UserId='{0}') ", userId);
                    sbWhere.AppendFormat(" ) ");
                }
                else if (status == "3")
                { //已过期
                    sbWhere.AppendFormat(" And ( ");
                    sbWhere.AppendFormat(" (Status=0 And UserId='{0}' And ValidTo Is Not Null And ValidTo < GetDate()) ", userId);
                    sbWhere.AppendFormat(" Or (Status=1 And ToUserId='{0}' And ValidTo Is Not Null And ValidTo < GetDate()) ", userId);
                    sbWhere.AppendFormat(" ) ");
                }
                else //查询相关数据
                {
                    sbWhere.AppendFormat(" And (UserId='{0}' Or ToUserId='{0}') ");
                }
            }
            else
            {
                if (status == "0")
                { //未使用（含转赠未过期）
                    sbWhere.AppendFormat(" And Status In (0,1) And (ValidTo Is Null OR ValidTo >= GetDate())  ");
                }
                else if (status == "1")
                {
                    sbWhere.AppendFormat(" And Status = 9 ");
                }
                else if (status == "2")
                { // 已过期 （含转赠）
                    sbWhere.AppendFormat(" And Status In (0,1) And ValidTo Is Not Null And ValidTo < GetDate() ");
                }
                else if (status == "3")
                {
                    sbWhere.AppendFormat(" And Status = 0 ");
                }
                else if (status == "4")
                {
                    sbWhere.AppendFormat(" And Status = 1 ");
                }
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 发放列表查询
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="cardId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<StoredValueCardRecord> GetRecordList(int rows, int page, string cardId, string websiteOwner, string status, string userId)
        {
            return GetLit<StoredValueCardRecord>(rows, page, GetRecordWhereString(cardId, websiteOwner, status, userId), "AutoId Desc");
        }
        /// <summary>
        /// 发放记录数
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetRecordCount(string cardId, string websiteOwner, string status, string userId)
        {
            return GetCount<StoredValueCardRecord>(GetRecordWhereString(cardId, websiteOwner, status, userId));
        }
        /// <summary>
        /// 储值卡发放
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="cardId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="msg"></param>
        /// <param name="authority">域名加端口</param>
        /// <returns></returns>
        public bool SendRecord(string card_id, string type, string user_ids, string tags, string websiteOwner, string createUserId, out string msg, string authority)
        {
            msg = "";

            if (type == "2" && string.IsNullOrWhiteSpace(tags))
            {
                msg = "请选择接受储值卡的用户标签组";
                return false;
            }
            else if (type == "1" && string.IsNullOrWhiteSpace(user_ids))
            {
                msg = "请选择接受储值卡的用户";
                return false;
            }

            StoredValueCard card = GetByKey<StoredValueCard>("AutoId", card_id, websiteOwner: websiteOwner);
            if (card == null)
            {
                msg = "储值卡不存在";
                return false;
            }
            if (card.Status != 0)
            {
                msg = "储值卡已停用";
                return false;
            }

            DateTime curDate = DateTime.Now;
            string dateString = curDate.ToString("yyyyMMdd");
            if (card.ValidType == 0 && card.ValidTo.HasValue && card.ValidTo.Value < curDate)
            {
                msg = string.Format("储值卡已过有效期");
                return false;
            }
            List<string> userIdList = new List<string>();

            if (type == "2")
            {
                StringBuilder sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebSiteOwner='{0}' And ( 1=2 ", websiteOwner);
                foreach (var tag in tags.Split(','))
                {
                    sbWhere.AppendFormat(" Or ',' + TagName + ',' Like '%,{0},%' ", tag);
                }
                sbWhere.AppendFormat(" )");
                List<UserInfo> userList = GetColList<UserInfo>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,UserID");
                if (userList.Count > 0)
                {
                    userIdList = userList.Select(p => p.UserID).Distinct().ToList();
                }
            }
            else if (type == "1")
            {
                userIdList = user_ids.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().ToList();
            }

            if (userIdList.Count == 0)
            {
                msg = "接受卡券的用户未找到";
                return false;
            }

            int cCount = userIdList.Count - (card.MaxCount - card.SendCount);
            if (cCount > 0)
            {
                msg = string.Format("储值卡可发放数量不足，少{0}张", cCount);
                return false;
            }

            int startNum = 0;
            StoredValueCardRecord oldRecord = Get<StoredValueCardRecord>(string.Format("{0}{1}", GetRecordWhereString(card.AutoId.ToString(), websiteOwner, null, null), " Order By AutoId Desc"));
            if (oldRecord != null)
            {
                startNum = Convert.ToInt32(oldRecord.CardNumber.Substring(16));
            }

            StoredValueCardRecord baseRecord = new StoredValueCardRecord();
            baseRecord.CardId = card.AutoId;
            baseRecord.WebsiteOwner = websiteOwner;
            baseRecord.CreateUserId = createUserId;
            baseRecord.CreateDate = curDate;
            if (card.ValidType == 0)
            {
                baseRecord.ValidTo = card.ValidTo;
            }
            else if (card.ValidType == 1 && card.ValidDay.HasValue)
            {
                baseRecord.ValidTo = curDate.AddDays(card.ValidDay.Value);
            }
            baseRecord.Amount = card.Amount;
            Random ran = new Random();
            string cardString = card_id.PadLeft(3, '0');
            if (card_id.Length > 3) cardString = cardString.Substring(card_id.Length - 3);
            List<StoredValueCardRecord> sendRecordList = new List<StoredValueCardRecord>();
            List<UserInfo> usList = new List<UserInfo>();
            for (int i = 0; i < userIdList.Count; i++)
            {
                UserInfo cru = usList.FirstOrDefault(p => p.UserID == userIdList[i]);
                if (cru == null)
                {
                    cru = GetColByKey<UserInfo>("UserID", userIdList[i], "AutoID,UserID,WXOpenId,WebsiteOwner", websiteOwner: websiteOwner);
                    if (cru == null) continue;
                    usList.Add(cru);
                }
                else
                {
                    usList.Add(cru);
                }
                startNum++;
                StoredValueCardRecord rRecord = (StoredValueCardRecord)baseRecord.Clone();
                rRecord.UserId = userIdList[i];
                string numString = startNum.ToString();
                string ranString = ran.Next(99).ToString();
                rRecord.CardNumber = "No." + dateString + cardString + ranString.PadLeft(2, '0') + numString.PadLeft(3, '0');
                sendRecordList.Add(rRecord);
            }
            if (sendRecordList.Count == 0)
            {
                msg = "接收用户未找到";
                return false;
            }
            BLLWeixin bllWeixin = new BLLWeixin();
            int suCount = 0;
            string redicturl = string.Format("http://{0}/App/SVCard/Wap/List.aspx", authority);
            for (int i = 0; i < sendRecordList.Count; i++)
            {
                if (Add(sendRecordList[i]))
                {
                    suCount++;

                    string content = string.Format("{0}\\n金额：{1}", card.Name, card.Amount);
                    if (sendRecordList[i].ValidTo.HasValue)
                    {
                        content += string.Format("\\n有效期：{0}", sendRecordList[i].ValidTo.Value.ToString("yyyy-MM-dd HH:mm"));
                    }
                    bllWeixin.SendTemplateMessageNotifyComm(usList[i], "您收到一张储值卡", content, redicturl);
                }
            }
            if (Update(card, string.Format("SendCount=SendCount+{0}", suCount),
                string.Format("AutoId={0} And WebsiteOwner='{1}'", card.AutoId, websiteOwner)) <= 0)
            {
                if (suCount < sendRecordList.Count)
                {
                    msg = "发送成功" + suCount + "张，但更新发放数量出错";
                }
                else
                {
                    msg = "发送成功，但更新发放数量出错";
                }
                return true;
            }
            if (suCount < sendRecordList.Count)
            {
                msg = "发送成功" + suCount + "张";
            }
            else
            {
                msg = "发放储值卡成功";
            }
            return true;
        }

        /// <summary>
        /// 后台发放的储值卡可用状态
        /// </summary>
        /// <param name="record">发放的储值卡</param>
        /// <returns>
        /// 0可用
        /// 1已使用
        /// 2已过期
        /// </returns>
        public int GetUseStatusByAdmin(StoredValueCardRecord record)
        {
            if (record.Status == 9)
            {
                return 1;
            }
            else if (record.ValidTo.HasValue && record.ValidTo.Value < DateTime.Now)
            {
                return 2;
            }
            return 0;
        }
        /// <summary>
        /// 前台发放的储值卡可用状态
        /// </summary>
        /// <param name="card">储值卡主卡</param>
        /// <param name="record">发放的储值卡</param>
        /// <param name="curUser">当前登录用户</param>
        /// <param name="fromUser">发放用户</param>
        /// <param name="toUser">转赠用户</param>
        /// <param name="isGive">是否转赠</param>
        /// <returns>
        /// 可用状态
        /// 0可用
        /// 1已使用
        /// 2已过期
        /// 10待接收转赠 （接口判断查看人构造）
        /// 11已转赠 （接口判断查看人构造）
        /// 12已转赠他人（接口判断查看人构造）
        /// 9已停用 （接口判断查看人构造）
        /// 99非法状态 （未知）
        /// </returns>
        public int GetUseStatus(StoredValueCard card, StoredValueCardRecord record, UserInfo curUser, UserInfo fromUser, UserInfo toUser, bool isGive)
        {
            BLLMall bllMall = new BLLMall();
            int useStatus = 99; //正常
            if (card.Status == 1)
            {
                useStatus = 9; //已停用
            }
            else if (record.Status == 0)
            {
                if (curUser.AutoID == fromUser.AutoID)
                {
                    useStatus = 0; //已转赠
                }
                else if (isGive)
                {
                    useStatus = 10; //待接收转赠
                }
            }
            else if (record.Status == 1)
            {
                if (curUser.AutoID == toUser.AutoID)
                {
                    useStatus = 0; //正常
                }
                else if (curUser.AutoID == fromUser.AutoID)
                {
                    useStatus = 11; //已转赠
                }
                else
                {
                    useStatus = 12; //已转赠它人
                }
            }
            else if (record.Status == 9)
            {
                if (curUser.AutoID == fromUser.AutoID && toUser != null)
                {
                    useStatus = 11; //已转赠
                }
                else
                {
                    useStatus = 1; //已使用
                }

                // 储值卡有余额还可转赠
                decimal canUseAmount = string.IsNullOrEmpty(record.ToUserId) ? bllMall.GetStoreValueCardCanUseAmount(record.AutoId.ToString(), record.UserId) : bllMall.GetStoreValueCardCanUseAmount(record.AutoId.ToString(), record.UserId);
                if (canUseAmount > 0)
                {
                    useStatus = 0;
                    if (isGive)
                    {
                        useStatus = 10;
                    }
                }
                
                // 储值卡有余额还可转赠

            }

            if (useStatus == 0 && record.ValidTo.HasValue && record.ValidTo.Value < DateTime.Now)
            {
                useStatus = 2;
            }
            return useStatus;
        }


        /// <summary>
        /// 获取储值卡使用记录列表
        /// </summary>
        /// <param name="myCardId">主卡Id</param>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public List<StoredValueCardUseRecord> GetUseRecordList(int myCardId, string userId)
        {

            return GetList<StoredValueCardUseRecord>(string.Format("WebsiteOwner='{0}' And MyCardId={1} And UserId='{2}'", WebsiteOwner, myCardId, userId));


        }

        /// <summary>
        /// 获取可用的储值卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<StoredValueCardRecord> GetCanUseStoredValueCardList(string userId)
        {
            List<StoredValueCardRecord> canUseList = new List<StoredValueCardRecord>();//可以使用的储值卡=从未使用+已使用但未使用完的。
            List<StoredValueCardRecord> unUseList = GetRecordList(int.MaxValue, 1, null, WebsiteOwner, "0", userId);//从未使用的
            List<StoredValueCardRecord> useList = GetRecordList(int.MaxValue, 1, null, WebsiteOwner, "1", userId);//使用过的
            foreach (var item in useList)
            {

                if (item.ValidTo.HasValue && (DateTime)(item.ValidTo.Value) > DateTime.Now)//未过期
                {
                    if (GetUseRecordList(item.AutoId, userId).Sum(p => p.UseAmount) < item.Amount)
                    {
                        canUseList.Add(item);
                    }

                }
            }
            canUseList.AddRange(unUseList);


            return canUseList;


        }





        #endregion 储值卡发放表
    }
}
