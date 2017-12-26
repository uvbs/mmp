using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Forbes;
using ZCJson.Linq;

namespace ZentCloud.JubitIMP.Web.customize.SportsShow
{
    public partial class question : System.Web.UI.Page
    {
        BLLForbesQuestion bllQuestion = new BLLForbesQuestion();

        protected List<ForbesQuestion> ListQuestion = new List<ForbesQuestion>();
        protected List<ForbesQuestionPersonal> ListQPersonal = new List<ForbesQuestionPersonal>();
        private string ActivityName = "2015上海体博会体商测试";
        protected int NeedCount = 5;
        private string[] Categorys = new string[] { "马拉松题库", "", "", "赛事文化区知识", "电子竞技方面" };
        private string secret = "k4cs4cwc00c4o8ok8g84cccskgkkgc4s";
        private string mark = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo user = bllQuestion.GetCurrentUserInfo();
            string QuestionListKey = "QuestionList" + user.AutoID.ToString() + Session.SessionID;
            if (IsPostBack)
            {
                ListQuestion = (List<ForbesQuestion>)Common.DataCache.GetCache(QuestionListKey);//写入缓存
                return;
            }
            if (user == null)
            {
                this.Response.Redirect(Common.ConfigHelper.GetConfigString("noPmsUrl").ToLower(), true);
                return;
            }
            int ResultCount = bllQuestion.GetQuestionResultCount(user.UserID, bllQuestion.WebsiteOwner, ActivityName);
            if (ResultCount >=2)
            {
                this.Response.Redirect("beforCanceled.aspx", true);
                return;
            }

            ForbesQuestion question = bllQuestion.GetRandomQuestion(Categorys[0], null, bllQuestion.WebsiteOwner);
            if (question != null)
            {
                ListQuestion.Add(question);
            }
            Common.DataCache.SetCache(QuestionListKey, ListQuestion, System.Web.Caching.Cache.NoAbsoluteExpiration,TimeSpan.FromHours(1));//写入缓存
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hdnValue.Value)) return;
            UserInfo user = bllQuestion.GetCurrentUserInfo();
            string QuestionListKey = "QuestionList" + user.AutoID.ToString() + Session.SessionID;
            string QuestionPersonalListKey = "QuestionPersonalList" + user.AutoID.ToString() + Session.SessionID; 

            object  ObjPersonalList = Common.DataCache.GetCache(QuestionPersonalListKey);//写入缓存
            if (ObjPersonalList != null) ListQPersonal = (List<ForbesQuestionPersonal>)ObjPersonalList;

            ForbesQuestionPersonal QPersonal = new ForbesQuestionPersonal();
            QPersonal.UserId = bllQuestion.GetCurrentUserInfo().UserID;
            QPersonal.WebsiteOwner = bllQuestion.WebsiteOwner;
            QPersonal.Status = 1;
            QPersonal.QuestionId = ListQuestion[ListQuestion.Count - 1].AutoID;
            QPersonal.Answer = hdnValue.Value;
            if(ListQuestion[ListQuestion.Count - 1].CorrectAnswerCode == QPersonal.Answer){
                QPersonal.IsCorrect = 1;
                QPersonal.Score = ListQuestion[ListQuestion.Count - 1].Score;
            }
            else{
                QPersonal.IsCorrect = 0;
                QPersonal.Score = 0;
            }
            ListQPersonal.Add(QPersonal);

            hdnValue.Value = "";
            if (ListQuestion.Count < NeedCount)
            {
                string nCategory = ListQuestion.Count < Categorys.Length ? Categorys[ListQuestion.Count] : null;
                string LimitID = Common.MyStringHelper.ListToStr(ListQuestion.Select(p => p.AutoID.ToString()).ToList(), "", ",");
                ForbesQuestion question = bllQuestion.GetRandomQuestion(nCategory, LimitID, bllQuestion.WebsiteOwner);
                ListQuestion.Add(question);
                Common.DataCache.SetCache(QuestionListKey, ListQuestion, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));//写入缓存
                Common.DataCache.SetCache(QuestionPersonalListKey, ListQPersonal, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));//写入缓存
                return;
            }
            Common.DataCache.ClearCache(QuestionListKey);//写入缓存
            Common.DataCache.ClearCache(QuestionPersonalListKey);//写入缓存

            int OldResultCount = bllQuestion.GetQuestionResultCount(user.UserID, bllQuestion.WebsiteOwner, ActivityName);
            if (OldResultCount >= 2)
            {
                Common.WebMessageBox.Show(this, "仅能参与2次调查");
                return;
            }
            string errmsg = "";
            int totalScore = 0;
            int resultNum = 0;

            if (ListQPersonal.Count > 5)
            {
                ListQPersonal = ListQPersonal.Take(5).ToList();
            }

            if (bllQuestion.PostQuestionResult(ListQPersonal, NeedCount, user.UserID, bllQuestion.WebsiteOwner
                , ActivityName, out errmsg, out totalScore, out resultNum))
            {
                if(totalScore == 100){
                    int ResultCount = bllQuestion.GetQuestionResultCount(user.UserID, bllQuestion.WebsiteOwner, ActivityName);
                    if (ResultCount > 2)
                    {
                        this.Response.Redirect("beforCanceled.aspx", true);
                        return;
                    }
                    string event_id = "6ac7bb6476ab8fd0";
                    if (ResultCount ==2)
                    {
                        event_id = "9cf118d317d3f756";
                    }
                    BLLUser bllUser = new BLLUser();

                    Dictionary<string, string> Dic1 = new Dictionary<string, string>();
                    Dic1.Add("event_id", event_id);
                    Dic1.Add("openid", user.WXOpenId);
                    string nonce = Guid.NewGuid().ToString("N");
                    Dic1.Add("nonce", nonce);
                    if (!string.IsNullOrWhiteSpace(user.WXNickname)) Dic1.Add("nickname", user.WXNickname);
                    if (!string.IsNullOrWhiteSpace(user.WXSex.ToString())) Dic1.Add("sex", user.WXSex.ToString());
                    if (!string.IsNullOrWhiteSpace(mark)) Dic1.Add("mark", mark);

                    string strtemp = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(Dic1, false);
                    strtemp = strtemp + "&secret=" + secret;
                    string sign = Payment.WeiXin.MD5Util.GetMD5(strtemp, "UTF-8").ToUpper();

                    Dictionary<string, string> Dic2 = new Dictionary<string, string>();
                    Dic2.Add("event_id", event_id);
                    Dic2.Add("openid", user.WXOpenId);
                    Dic2.Add("nonce", nonce);
                    Dic2.Add("nickname", user.WXNickname);
                    Dic2.Add("sex", user.WXSex.ToString());
                    if (!string.IsNullOrWhiteSpace(mark)) Dic2.Add("mark", mark);
                    Dic2.Add("sign", sign);

                    string urlParams = Payment.WeiXin.CommonUtil.FormatBizQueryParaMap(Dic2, false);

                    string url = "https://admin.coffice.so/index.php/event_api/make_code?" + urlParams;
                    string rjson = "";
                    try
                    {
                        using (HttpWebResponse response = ZentCloud.Common.HttpInterFace.CreateGetHttpResponse(url, null, null, null))
                        {
                            Stream stream = response.GetResponseStream();   //获取响应的字符串流
                            StreamReader sr = new StreamReader(stream); //创建一个stream读取流
                            rjson = sr.ReadToEnd();   //从头读到尾，放到字符串html李米
                        }
                    }
                    catch (Exception ex)
                    {
                        ToLog("GetResponseStream", QPersonal.UserId, resultNum.ToString(), strtemp, sign, url, rjson, ex.Message);
                    }
                    try 
	                {	    
                        JToken Jtoken = JToken.Parse(rjson);
                        if (Jtoken["msg"] != null && Jtoken["msg"].ToString() == "success")
                        {
                            bllQuestion.UpdateResultGiftCode(QPersonal.UserId, resultNum, QPersonal.WebsiteOwner, Jtoken["code"].ToString());
                        }
                        else
                        {
                            ToLog("make_code", QPersonal.UserId, resultNum.ToString(), strtemp, sign, url, rjson,"");
                        }
	                }
                    catch (Exception ex)
                    {
                        ToLog("UpdateResultGiftCode", QPersonal.UserId, resultNum.ToString(), strtemp, sign, url, rjson,ex.Message);
	                }
                }
                this.Response.Redirect("beforCanceled.aspx", true);
            }
            else
            {
                Common.WebMessageBox.Show(this, errmsg);
            }
        }

        private void ToLog(string type, string UserId, string ResultNum, string strtemp, string sign, string url,string rjson, string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\tbhtest.txt", true, Encoding.GetEncoding("gb2312")))
                {

                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), type));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "UserId", UserId));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "ResultNum", ResultNum));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "strtemp", strtemp));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "sign", sign));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "url", url));
                    if (!string.IsNullOrWhiteSpace(strtemp)) sw.WriteLine(string.Format("{0}\t{1}", "rjson", rjson));
                    if (!string.IsNullOrWhiteSpace(msg)) sw.WriteLine(string.Format("{0}\t{1}", "msg", msg));
                }
            }
            catch { }
        }
    }
}