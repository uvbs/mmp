using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui
{
    public partial class TongJi : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public TongJiModel model = new TongJiModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            string WebsiteOwner = "wubuhui";
            model.ToDay =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            model.PreDate =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-1);
            string ToDayTotalGuestCountSql = string.Format("WebsiteOwner='{0}' And (TrueName =''  Or  TrueName is null ) And (Company =''  Or  Company is null ) And (Phone =''  Or  Phone is null )",WebsiteOwner);
            string ToDayTotalRegCountSql = string.Format("WebsiteOwner='{0}' And TrueName !=''  And Company!='' And Phone !='' ",WebsiteOwner);


            string PreTotalGuestCountSql = string.Format("WebsiteOwner='{0}'  And Regtime<'{1}' And (TrueName =''  Or  TrueName is null ) And (Company =''  Or  Company is null ) And (Phone =''  Or  Phone is null )", WebsiteOwner, model.ToDay.ToString("yyyy-MM-dd"));
            string PreTotalRegCountSql = string.Format("WebsiteOwner='{0}' And Regtime<'{1}' And TrueName !=''  And Company!='' And Phone !='' ", WebsiteOwner, model.ToDay.ToString("yyyy-MM-dd"));



            string PreTwoDayTotalGuestCountSql = string.Format("WebsiteOwner='{0}'  And Regtime<'{1}' And (TrueName =''  Or  TrueName is null ) And (Company =''  Or  Company is null ) And (Phone =''  Or  Phone is null )", WebsiteOwner, model.PreDate.ToString("yyyy-MM-dd"));//前两天访客数量

            string PreTwoDayTotalRegCountSql = string.Format("WebsiteOwner='{0}'  And Regtime<'{1}' And TrueName !=''  And Company!='' And Phone !='' ", WebsiteOwner,model.PreDate.ToString("yyyy-MM-dd"));//前两天注册用户数量


            int ToDayTotalGuestCount = bllUser.GetCount<UserInfo>(ToDayTotalGuestCountSql);//今天访客数量
            int ToDayTotalRegCount = bllUser.GetCount<UserInfo>(ToDayTotalRegCountSql);//今天注册数量

            int PreTotalGuestCount = bllUser.GetCount<UserInfo>(PreTotalGuestCountSql);//昨天访客数量
            int PreTotalRegCount = bllUser.GetCount<UserInfo>(PreTotalRegCountSql);//昨天注册数量

            int PreTwoDayTotalGuestCount = bllUser.GetCount<UserInfo>(PreTwoDayTotalGuestCountSql);//前两天访客数量
            int PreTwoDayTotalRegCount = bllUser.GetCount<UserInfo>(PreTwoDayTotalRegCountSql);//前两天注册数量




            model.PreTotalGuestCount = PreTotalGuestCount;
            model.PreTotalRegCount = PreTotalRegCount;
            model.ToDayTotalGuestCount = ToDayTotalGuestCount;
            model.ToDayTotalRegCount = ToDayTotalRegCount;

            model.PreAddGuestTount = PreTotalGuestCount - PreTwoDayTotalGuestCount;
            model.PreAddGuestTount = model.PreAddGuestTount > 0 ? model.PreAddGuestTount : 0;

            model.PreAddRegTount = PreTotalRegCount - PreTwoDayTotalRegCount;
            model.PreAddRegTount = model.PreAddRegTount > 0 ? model.PreAddRegTount : 0;


            model.ToDayAddGuestTount = ToDayTotalGuestCount - PreTotalGuestCount;
            model.ToDayAddGuestTount = model.ToDayAddGuestTount > 0 ? model.ToDayAddGuestTount : 0;

            model.ToDayAddRegTount = ToDayTotalRegCount - PreTotalRegCount;
            model.ToDayAddRegTount= model.ToDayAddRegTount>0? model.ToDayAddRegTount:0;


        }



        public class TongJiModel {

            /// <summary>
            /// 昨天日期
            /// </summary>
            public DateTime PreDate { get; set; }
            /// <summary>
            /// 昨天访客数量
            /// </summary>
            public int PreTotalGuestCount { get; set; }
            /// <summary>
            /// 昨天访客增长
            /// </summary>
            public int PreAddGuestTount { get; set; }

            /// <summary>
            /// 昨天注册数量
            /// </summary>
            public int PreTotalRegCount { get; set; }
            /// <summary>
            /// 昨天注册增长
            /// </summary>
            public int PreAddRegTount { get; set; }

            /// <summary>
            /// 今天日期
            /// </summary>
            public DateTime ToDay { get; set; }
            /// <summary>
            /// 今天访客数量
            /// </summary>
            public int ToDayTotalGuestCount { get; set; }
            /// <summary>
            /// 今天访客增长
            /// </summary>
            public int ToDayAddGuestTount { get; set; }

            /// <summary>
            ///今天注册数量
            /// </summary>
            public int ToDayTotalRegCount { get; set; }
            /// <summary>
            /// 今天注册增长
            /// </summary>
            public int ToDayAddRegTount { get; set; }

        
        
        }

    }

    



}