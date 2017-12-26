using System;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// CarServerInfo
	/// </summary>
	[Serializable]
	public partial class CarServerInfo : ZCBLLEngine.ModelTable
	{
		public CarServerInfo()
		{}

        #region Model

        public int ServerId { get; set; }
        public int CarBrandId { get; set; }
        public int CarSeriesCateId { get; set; }
        public int CarSeriesId { get; set; }
        public int CarModelId { get; set; }
        /// <summary>
        /// 门店类型：4s店、专修店
        /// </summary>
        public string ShopType { get; set; }
        /// <summary>
        /// 服务类型：门店服务、上门服务
        /// </summary>
        public string ServerType { get; set; }
        public int CateId { get; set; }
        public string ServerName { get; set; }
        public int WorkHours { get; set; }
        public string WebsiteOwner { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }

        #endregion Model

        #region ModelEx

        public string ShowCarModel { get; set; } 

        public string ShowCate { get; set; }

        /// <summary>
        /// 展示的服务名称：一级分类-二级分类-服务名称
        /// </summary>
        public string ShowName
        {
            get
            {
                string result = string.Empty;

                BLLArticleCategory bllCate = new BLLArticleCategory();

                ArticleCategory cateLV1 = null, cateLV2 = null;

                //二级分类
                cateLV2 = bllCate.GetArticleCategory(CateId);

                //一级分类
                if (cateLV2 != null)
                {
                    cateLV1 = bllCate.GetArticleCategory(cateLV2.PreID);
                }

                result = string.Format("{0}{1}{2}",
                        cateLV1 == null? "":cateLV1.CategoryName + "/",
                        cateLV2 == null? "":cateLV2.CategoryName + "/",
                        ServerName
                    );

                return result;
            }

        }

        #endregion

    }
}

