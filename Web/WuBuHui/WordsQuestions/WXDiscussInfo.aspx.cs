using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions
{
    public partial class WXDiscussInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 话题ID
        /// </summary>
        public string autoId;
        /// <summary>
        /// 是否已经赞过
        /// </summary>
        public bool isPraise = false;
        /// <summary>
        /// BLL基类
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 提问用户的信息
        /// </summary>
       // public UserInfo TiWenUserInfo=new UserInfo();
        protected ReviewInfo reviewInfo; 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                    autoId = Request["AutoId"];
                    if (!string.IsNullOrEmpty(autoId))
                    {
                        reviewInfo = bll.Get<BLLJIMP.Model.ReviewInfo>(string.Format(" AutoId={0}", autoId));
                        if (reviewInfo != null)
                        {
                            reviewInfo.Pv++;
                            bll.Update(reviewInfo);
                            //txtStepNum.Text = rInfo.StepNum.ToString();
                            //txtPraiseNum.Text = reviewInfo.PraiseNum.ToString();
                            //TiWenUserInfo=bllUser.GetUserInfo(rInfo.UserId);
                            //var tutorInfo=bll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", TiWenUserInfo.UserID));
                            //if (tutorInfo != null)
                            //{
                            //    TiWenUserInfo.TrueName = tutorInfo.TutorName;
                            //    TiWenUserInfo.WXHeadimgurl = tutorInfo.TutorImg;
                            //}
                            //else
                            //{
                            //    TiWenUserInfo.WXHeadimgurl = TiWenUserInfo.WXHeadimgurlLocal;
                            //}


                        }
                    }

                        ForwardingRecord record = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND WebsiteOwner='{2}' AND TypeName='话题赞'", bll.GetCurrentUserInfo().UserID, autoId, bll.WebsiteOwner));
                        if (record != null)
                        {
                            isPraise = true;
                        }


                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }

        }
        ///// <summary>
        ///// 获取当前用户是否为导师
        ///// </summary>
        //private void GetUser()
        //{
        //    BLLJIMP.Model.UserInfo userInfo = DataLoadTool.GetCurrUserModel();

        //    if (bll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", userInfo.UserID)) == null)
        //    {
        //        //jointhisdiscuss.Visible = false;
        //    }
        //}


    }
}
