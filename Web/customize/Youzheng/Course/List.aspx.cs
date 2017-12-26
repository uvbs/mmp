using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.Youzheng.Course
{
    public partial class List : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 分类列表
        /// </summary>
        public List<WXMallCategory> categoryList = new List<WXMallCategory>();

        /// <summary>
        /// 查询参数
        /// </summary>
        protected string keyword = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryList = bllMall.GetCategoryList().Where(p=>p.Type=="Course").ToList();
            foreach (var item in categoryList)
            {
                if (string.IsNullOrEmpty(item.CategoryImg))
                {
                    item.CategoryImg = "../images/category1.png";
                }
            }
            if (!string.IsNullOrEmpty(Request["keyword"]))
            {
                keyword = Request["keyword"];
            }
            

        }
    }
}