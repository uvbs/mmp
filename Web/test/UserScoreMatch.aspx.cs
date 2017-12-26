using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class UserScoreMatch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bll = new BLLJIMP.BLLUser();

            var oldUserList = bll.GetList<BLLJIMP.Model.OldWeimengUser>(" ToUserId IS NULL ");

            string result = string.Empty;
            
            int totalCount = 0;

            foreach (var item in oldUserList)
            {

                var phone = item.Phone.Replace("\t", "").Replace("\"", "");
                var name = item.Name.Replace("\t", "").Replace("\"", "");
                var score = item.Score.Replace("\t", "").Replace("\"", "");

                int addScore = 0;

                int.TryParse(score, out addScore);

                if (addScore > 0)
                {
                    var userInfo = bll.GetUserInfoByPhone(phone);

                    if (userInfo != null)
                    {
                        //更新本库积分

                        //更新原数据已导入的用户信息

                        totalCount++;
                        result += "<br>" + phone + ":" + name + ":" + score;
                        
                    }
                }

            }
            Response.Write(totalCount + "/" + oldUserList.Count);
            Response.Write(result);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bll = new BLLJIMP.BLLUser();

            var oldUserList = bll.GetList<BLLJIMP.Model.OldWeimengUser>(" ToUserId IS NULL ");

            string result = string.Empty;
            int totalCount = 0;

            foreach (var item in oldUserList)
            {

                var phone = item.Phone.Replace("\t", "").Replace("\"", "");
                var name = item.Name.Replace("\t", "").Replace("\"", "");
                var score = item.Score.Replace("\t", "").Replace("\"", "");

                int addScore = 0;

                int.TryParse(score, out addScore);

                if (addScore > 0)
                {
                    var userInfo = bll.GetUserInfoByPhone(phone);

                    if (userInfo != null)
                    {
                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                        try
                        {
                            //更新用户头像和积分
                            if (string.IsNullOrWhiteSpace(userInfo.TrueName) && !string.IsNullOrWhiteSpace(name))
                            {
                                bll.Update(userInfo, string.Format(" TrueName='{0}' ", name), string.Format(" AutoID={0} ", userInfo.AutoID), tran);
                            }

                            bll.Update(userInfo, " TotalScore=TotalScore+" + addScore, string.Format(" AutoID={0} ", userInfo.AutoID), tran);

                            //插入积分详情记录
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.UserID = userInfo.UserID;
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.TotalScore = userInfo.TotalScore;
                            scoreRecord.Score = addScore;
                            scoreRecord.ScoreType = "老系统积分同步";
                            scoreRecord.AddNote = "老系统积分同步";
                            scoreRecord.RelationID = bll.GetCurrUserID();
                            scoreRecord.WebSiteOwner = bll.WebsiteOwner;
                            bll.Add(scoreRecord, tran);

                            //更新原数据已导入的用户信息
                            bll.Update(item, 
                                string.Format(" ToUserId='{0}',ToUserAutoId='{1}',ToTime='{2}' ",userInfo.UserID,userInfo.AutoID,DateTime.Now.ToString()),
                                string.Format(" AutoId = {0} ",item.AutoId), tran);
                            
                            tran.Commit();
                            
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                        totalCount++;
                        //result += "<br>" + phone + ":" + name + ":" + score;
                        
                    }
                }
                
            }
            Response.Write(totalCount + "/" + oldUserList.Count);
            //Response.Write(result);

        }
    }
}