using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 车型库
    /// </summary>
    public class BLLCarLibrary : BLL
    {
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser userBll = new BLLUser();


        #region 车型库管理
        /// <summary>
        /// 更新汽车品牌数据库
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int UpdateBrandInfo(List<CarBrandInfo> dataList)
        {
            int result = 0;

            foreach (var item in dataList)
            {
                if (GetCount<CarBrandInfo>(string.Format("CarBrandId = {0} ", item.CarBrandId)) > 0)
                {
                    if (Update(item)) result++;
                }//更新
                else
                {
                    if (Add(item)) result++;
                }//新增
            }

            return result;
        }

        /// <summary>
        /// 更新汽车车系分类
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int UpdateSeriesCateInfo(List<CarSeriesCateInfo> dataList)
        {
            int result = 0;

            foreach (var item in dataList)
            {
                if (GetCount<CarSeriesCateInfo>(string.Format("CarSeriesCateId = {0} AND CarBrandId = {1} ", item.CarSeriesCateId, item.CarBrandId)) > 0)
                {
                    if (Update(new CarSeriesCateInfo(), string.Format(" CarSeriesCateName='{0}' ", item.CarSeriesCateName), string.Format("CarSeriesCateId = {0} AND CarBrandId = {1} ", item.CarSeriesCateId, item.CarBrandId)) > 0) result++;
                }//更新
                else
                {
                    if (Add(item)) result++;
                }//新增
            }

            return result;
        }

        /// <summary>
        /// 更新车系数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int UpdateSeriesInfo(List<CarSeriesInfo> dataList)
        {
            int result = 0;

            foreach (var item in dataList)
            {
                if (GetCount<CarSeriesInfo>(string.Format("CarSeriesId = {0} AND CarSeriesCateId = {1} AND CarBrandId = {2} ", item.CarSeriesId, item.CarSeriesCateId, item.CarBrandId)) > 0)
                {
                    if (Update(new CarSeriesInfo(), string.Format(" CarSeriesName='{0}' ", item.CarSeriesName), string.Format("CarSeriesId = {0} AND CarSeriesCateId = {1} AND CarBrandId = {2}  ", item.CarSeriesId, item.CarSeriesCateId, item.CarBrandId)) > 0) result++;
                }//更新
                else
                {
                    if (Add(item)) result++;
                }//新增
            }

            return result;
        }

        /// <summary>
        /// 更新车型类别数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int UpdateModelCateInfo(List<CarModelCateInfo> dataList)
        {
            int result = 0;

            foreach (var item in dataList)
            {
                if (GetCount<CarModelCateInfo>(string.Format("CarModelCateId = {0} AND CarSeriesId = {1} ", item.CarModelCateId, item.CarSeriesId)) > 0)
                {
                    if (Update(new CarModelCateInfo(), string.Format(" CarModelCateName='{0}' ", item.CarModelCateName), string.Format("CarModelCateId = {0} AND CarSeriesId = {1} ", item.CarModelCateId, item.CarSeriesId)) > 0) result++;
                }//更新
                else
                {
                    if (Add(item)) result++;
                }//新增
            }

            return result;
        }

        /// <summary>
        /// 更新车型数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public int UpdateModelInfo(List<CarModelInfo> dataList)
        {
            int result = 0;

            foreach (var item in dataList)
            {
                if (GetCount<CarModelInfo>(string.Format("CarModelId = {0} ", item.CarModelId)) > 0)
                {
                    if (Update(item)) result++;
                }//更新
                else
                {
                    if (Add(item)) result++;
                }//新增
            }

            return result;
        }

        /// <summary>
        /// 查询品牌
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="firstLetter"></param>
        /// <returns></returns>
        public List<CarBrandInfo> QueryBrand(out int totalCount, string websiteOwner, int pageSize, int pageIndex, string firstLetter = "", bool isMustInWebsite = false)
        {
            List<CarBrandInfo> result = new List<CarBrandInfo>();
            totalCount = 0;

            List<int> buyBrandIdList = new List<int>();
            List<int> serviceBrandList = new List<int>();
            List<int> allBrandIdList = new List<int>();

            GetWebSiteBrandIdList(websiteOwner, out buyBrandIdList, out serviceBrandList, out allBrandIdList);

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(firstLetter))
            {
                strWhere.AppendFormat("AND FirstLetter='{0}' ", firstLetter);
            }

            if (isMustInWebsite)
            {
                if (allBrandIdList.Count > 0)
                {
                    strWhere.AppendFormat("AND CarBrandId IN ({0}) ", MySpider.MyStringHelper.ListToStr<int>(allBrandIdList, "", ","));
                }
            }

            result = GetLit<CarBrandInfo>(pageSize, pageIndex, strWhere.ToString(), " FirstLetter ASC ");
            totalCount = GetCount<CarBrandInfo>(strWhere.ToString());


            for (int i = 0; i < result.Count; i++)
            {
                result[i].IsCurrBuyCarBrand = buyBrandIdList.Contains(result[i].CarBrandId);
                result[i].IsCurrServiceCarBrand = serviceBrandList.Contains(result[i].CarBrandId);
            }

            return result;
        }

        /// <summary>
        /// 查询车系分类
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public List<CarSeriesCateInfo> GetSeriesCateList(int brandId)
        {
            return GetList<CarSeriesCateInfo>(string.Format(" CarBrandId = {0} ", brandId));
        }

        //查询车系
        public List<CarSeriesInfo> GetSeriesList(int cateId, int brandId)
        {
            return GetList<CarSeriesInfo>(string.Format(" CarSeriesCateId = {0} AND CarBrandId = {1} ", cateId, brandId));
        }

        //查询车型分类
        public List<CarModelCateInfo> GetModelCateList(int seriesId)
        {
            return GetList<CarModelCateInfo>(string.Format(" CarSeriesId = {0} ", seriesId));
        }

        //查询车型
        public List<CarModelInfo> GetModelList(out int totalCount, int pageSize, int pageIndex, int modelCateId, int seriesId)
        {
            List<CarModelInfo> result = new List<CarModelInfo>();

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            if (modelCateId != 0)
                strWhere.AppendFormat(" AND CarModelCateId = {0} ", modelCateId);

            if (seriesId != 0)
                strWhere.AppendFormat(" AND CarSeriesId = {0} ", seriesId);


            result = GetLit<CarModelInfo>(pageSize, pageIndex, strWhere.ToString(), " Year DESC ");

            totalCount = GetCount<CarModelInfo>(strWhere.ToString());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="buyBrandIdList"></param>
        /// <param name="serviceBrandList"></param>
        /// <param name="allList"></param>
        public void GetWebSiteBrandIdList(string websiteOwner, out List<int> buyBrandIdList, out List<int> serviceBrandList, out List<int> allList)
        {
            buyBrandIdList = GetWebSiteBrandIdList(websiteOwner, Enums.CommRelationType.BuyCarBrand);
            serviceBrandList = GetWebSiteBrandIdList(websiteOwner, Enums.CommRelationType.ServiceCarBrand);

            allList = buyBrandIdList;
            allList.AddRange(serviceBrandList);
            allList = allList.Distinct().ToList();

        }

        /// <summary>
        /// 获取站点设置的购车库、养车库品牌
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<int> GetWebSiteBrandIdList(string websiteOwner, Enums.CommRelationType type)
        {
            List<int> result = new List<int>();

            var relationData = bllCommRelation.GetRelationList(type, websiteOwner, "", 1, int.MaxValue);

            if (relationData != null && relationData.Count > 0)
            {
                result = relationData.Select(p => int.Parse(p.RelationId)).ToList();
            }

            return result;
        }


        /// <summary>
        /// 设置购车、养车品牌
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="brandId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SetWebSiteBrand(string websiteOwner, int brandId, Enums.CommRelationType type)
        {
            return bllCommRelation.AddCommRelation(type, websiteOwner, brandId.ToString());
        }

        /// <summary>
        /// 取消设置购车、养车品牌
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="brandId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool RemoveWebSiteBrand(string websiteOwner, int brandId, Enums.CommRelationType type)
        {
            return bllCommRelation.DelCommRelation(type, websiteOwner, brandId.ToString());
        }

        /// <summary>
        /// 判断是否是当前汽车品牌
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="brandId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ExistWebSiteBrand(string websiteOwner, int brandId, Enums.CommRelationType type)
        {
            return bllCommRelation.ExistRelation(type, websiteOwner, brandId.ToString());
        }

        /// <summary>
        /// 更新品牌配图
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="imgSrc"></param>
        /// <returns></returns>
        public bool UpdateBrandImg(int brandId, string imgSrc)
        {
            return Update(
                    new CarBrandInfo(),
                    string.Format(" BrandImg = '{0}' ", imgSrc),
                    string.Format(" CarBrandId = {0} ", brandId)
                ) > 0;
        }

        /// <summary>
        /// 获取品牌实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.CarBrandInfo GetBrand(int id)
        {
            return Get<Model.CarBrandInfo>(string.Format(" CarBrandId = {0} ", id));
        }

        /// <summary>
        /// 获取车系分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.CarSeriesCateInfo GetSeriesCateInfo(int id)
        {
            return Get<Model.CarSeriesCateInfo>(string.Format(" CarSeriesCateId = {0} ", id));
        }

        /// <summary>
        /// 获取车系实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.CarSeriesInfo GetSeriesInfo(int id)
        {
            return Get<Model.CarSeriesInfo>(string.Format(" CarSeriesId = {0} ", id));
        }

        /// <summary>
        /// 获取车型实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.CarModelInfo GetCarModelInfo(int id)
        {
            var result = Get<Model.CarModelInfo>(string.Format(" CarModelId = {0} ", id));

            return result;
        }

        /// <summary>
        /// 获取车型完整名称
        /// </summary>
        /// <param name="carModelId"></param>
        /// <returns></returns>
        public string GetAllCarModelName(int carModelId)
        {
            string result = string.Empty;
            Model.CarModelInfo carModel = GetCarModelInfo(carModelId);
            Model.CarBrandInfo brand = GetBrand(carModel.CarBrandId);
            Model.CarSeriesCateInfo seriesCate = GetSeriesCateInfo(carModel.CarSeriesCateId);
            Model.CarSeriesInfo series = GetSeriesInfo(carModel.CarSeriesId);

            result = string.Format("{0}/{1}/{2}", brand.CarBrandName, series.CarSeriesName, carModel.ShowName);

            return result;
        }

        #endregion



        #region 订单管理
        //订单列表查询
        //添加订单
        //订单评价
        //更改订单状态

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<CarServerOrderInfo> GetCarServerOrderList(
                out int totalCount,
                int pageSize,
                int pageIndex,
                string websiteOwner = "",
                string sallerId = "",
                string userId = "",
                string commentStatus = "",//评论状态  0为未点评  1为已点评
                string status = ""//订单状态

            )
        {
            List<CarServerOrderInfo> result = new List<CarServerOrderInfo>();

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(status))
            {
                strWhere.AppendFormat(" AND Status in ({0}) ", status);
            }

            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            }

            if (!string.IsNullOrWhiteSpace(sallerId))
            {
                strWhere.AppendFormat(" AND SallerId = '{0}' ", sallerId);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                strWhere.AppendFormat(" AND UserId = '{0}' ", userId);
            }

            if (!string.IsNullOrWhiteSpace(commentStatus))
            {
                strWhere.AppendFormat(" AND CommentStatus = {0} ", commentStatus);
            }

            result = GetLit<CarServerOrderInfo>(pageSize, pageIndex, strWhere.ToString(), " Status ASC,CreateTime DESC ");

            totalCount = GetCount<CarServerOrderInfo>(strWhere.ToString());

            return result;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public CarServerOrderInfo GetCarServerOrderDetail(int orderId)
        {
            return Get<CarServerOrderInfo>(string.Format(" OrderId = {0} ", orderId));
        }

        /// <summary>
        /// 评价订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="score"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool RateCarServerOrder(string userId, int orderId, int score, string comment)
        {
            var key = Enums.CommRelationType.CarServerOrderRateScore;

            var result = bllCommRelation.AddCommRelation(key, userId, orderId.ToString(), "", score.ToString(), comment, "", "", "", WebsiteOwner);

            return result;
        }
        
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool CancelCarServerOrder(int orderId)
        {
            return Update(
                    new CarServerOrderInfo(),
                    string.Format(" Status = 3 "),
                    string.Format(" OrderId = {0} ", orderId)
                ) > 0;
        }

        /// <summary>
        /// 编辑服务订单状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">0受理中，1已确认，2已完成，3已取消</param>
        /// <returns></returns>
        public int EditCarServerOrderStatus(string ids, int status)
        {
            StringBuilder sqlUpdate = new StringBuilder();

            sqlUpdate.AppendFormat(" Status = {0} ", status);

            if (status == 2)
            {
                sqlUpdate.AppendFormat(" , UseTime = '{0}' ", DateTime.Now.ToString());
            }

            var result = Update(
                    new CarServerOrderInfo(),
                    sqlUpdate.ToString(),
                    string.Format(" OrderId in ({0}) ", ids)
                );

            //TODO：订单状态变更服务通知

            return result;
        }

        /// <summary>
        /// 评价商家
        /// </summary>
        /// <param name="sallerId">商家id</param>
        /// <param name="doRateUserId">评价者id</param>
        /// <param name="reputationScore">信誉分数</param>
        /// <param name="serviceAttitudeScore">服务态度分数</param>
        /// <param name="comment">评价内容</param>
        /// <returns></returns>
        //public bool RateSaller(string sallerId, string doRateUserId, double reputationScore, double serviceAttitudeScore, string comment)
        //{
        //    return bllCommRelation.AddCommRelation(Enums.CommRelationType.SallerRateScore, doRateUserId, sallerId, "", reputationScore.ToString(), serviceAttitudeScore.ToString(), comment);
        //}

        ///// <summary>
        ///// 添加订单
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="userId"></param>
        ///// <param name="serverIds"></param>
        ///// <param name="sallerId"></param>
        ///// <param name="arrvieTime"></param>
        ///// <param name="carModelId"></param>
        ///// <param name="carOwnerName"></param>
        ///// <param name="carOwnerPhone"></param>
        ///// <param name="status"></param>
        ///// <param name="sallerType"></param>
        ///// <param name="score"></param>
        ///// <param name="review"></param>
        ///// <param name="couponId"></param>
        ///// <param name="websiteOwner"></param>
        ///// <returns></returns>
        //public bool AddCarServerOrder(
        //    out CarServerOrderInfo model,
        //    string userId,
        //    string serverIds,
        //    int sallerId,
        //    DateTime arrvieTime,
        //    int carModelId,
        //    string carOwnerName,
        //    string carOwnerPhone,
        //    int status,
        //    int sallerType,
        //    int score,
        //    string review,
        //    int couponId,
        //    string websiteOwner
        //    )
        //{
        //    bool result = false;
        //    //计算  TotalPrice OrderId
        //    double totalPrice = 0;

        //    if (sallerType == 2)//pureCar
        //    {
        //        //TODO:根据ids获取服务总价
        //        List<int> serverIdList = serverIds.Split(',').Select(p => Convert.ToInt32(p)).Distinct().ToList(); 


        //    }

        //    model = new CarServerOrderInfo()
        //    {
        //        ArrvieTime = arrvieTime,
        //        CarModelId = carModelId,
        //        CarOwnerName = carOwnerName,
        //        CarOwnerPhone = carOwnerPhone,
        //        CouponId = couponId,
        //        CreateTime = DateTime.Now,
        //        OrderId = int.Parse(GetGUID(TransacType.CommAdd)),
        //        SallerId = sallerId,
        //        SallerType = sallerType,
        //        ServerIds = serverIds,
        //        Status = status,
        //        TotalPrice = totalPrice,
        //        UserId = userId,
        //        WebsiteOwner = websiteOwner
        //    };

        //    result = Add(model);

        //    return result;
        //}

        ////订单评价
        //public bool UpdateCarServerOrderReview(int orderId, int score, string review, string updater)
        //{
        //    //只有完成状态的订单才可评价
        //    bool result = false;

        //    var model = Get<CarServerOrderInfo>(string.Format(" OrderId = {0} ", orderId));

        //    if (model == null) return false;

        //    if (model.Status != 3) return false;

        //    result = Update(
        //                new CarServerOrderInfo(),
        //                string.Format(" Score={0},Review='{1}',Updater='{2}',UpdateTime='{3}' ", score, review, updater, DateTime.Now),
        //                string.Format(" OrderId = {0} ", orderId)
        //            ) > 0;

        //    return result;
        //}

        ////更改订单状态
        //public bool UpdateCarServerOrderStatus(int orderId,int status,string updater)
        //{
        //    bool result = false;

        //    result = Update(
        //                new CarServerOrderInfo(),
        //                string.Format(" Status={0},Updater='{1}',UpdateTime='{2}' ", status, updater, DateTime.Now),
        //                string.Format(" OrderId = {0} ", orderId)
        //            ) > 0;

        //    return result;
        //}

        #endregion


        #region 商户管理

        #endregion


        #region 配件管理
        //配件列表查询
        //配件添加
        //配件编辑        
        //配件复制（一次复制多个）

        /// <summary>
        /// 配件列表查询
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="carModelId"></param>
        /// <returns></returns>
        public List<CarPartsInfo> GetPartsList(out int totalCount, int pageSize, int pageIndex, string websiteOwner, int? carModelId, int brandId = 0, int seriesCateId = 0, int seriesId = 0, int cateId = 0, int partsBrandId = 0)
        {
            List<CarPartsInfo> result = new List<CarPartsInfo>();

            StringBuilder strWhere = new StringBuilder(" IsDelete = 0 ");

            strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);

            if (carModelId != null && carModelId > 0)
            {
                strWhere.AppendFormat(" AND CarModelId = {0} ", carModelId);
            }

            if (brandId > 0)
            {
                strWhere.AppendFormat(" AND CarBrandId = {0} ", brandId);
            }

            if (seriesCateId > 0)
            {
                strWhere.AppendFormat(" AND CarSeriesCateId = {0} ", seriesCateId);
            }

            if (seriesId > 0)
            {
                strWhere.AppendFormat(" AND CarSeriesId = {0} ", seriesId);
            }

            if (cateId > 0)
            {
                strWhere.AppendFormat(" AND PartsCateId = {0} ", cateId);
            }

            if (partsBrandId > 0)
            {
                strWhere.AppendFormat(" AND PartsBrandId = {0} ", partsBrandId);
            }



            totalCount = GetCount<CarPartsInfo>(strWhere.ToString());
            result = GetLit<CarPartsInfo>(pageSize, pageIndex, strWhere.ToString(), " PartId DESC ");

            return result;
        }

        /// <summary>
        /// 添加部件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="partName"></param>
        /// <param name="price"></param>
        /// <param name="carBrandId"></param>
        /// <param name="carSeriesCateId"></param>
        /// <param name="carSeriesId"></param>
        /// <param name="carModelId"></param>
        /// <param name="status"></param>
        /// <param name="count"></param>
        /// <param name="updater"></param>
        /// <returns></returns>
        public bool AddPars(
                string websiteOwner,
                string partName,
                double price,
                int carBrandId,
                int carSeriesCateId,
                int carSeriesId,
                int carModelId,
                int count,
                string updater,
                int partsCateId,
                string partsCateName,
                int partsBrandId,
                string partsBrandName,
                string partsSpecs
            )
        {
            bool result = false;

            CarPartsInfo model = new CarPartsInfo()
            {
                PartId = int.Parse(GetGUID(TransacType.CommAdd)),
                CarModelId = carModelId,
                PartName = partName,
                Price = price,
                Updater = updater,
                UpdateTime = DateTime.Now,
                WebsiteOwner = websiteOwner,
                Count = count,
                CarBrandId = carBrandId,
                CarSeriesCateId = carSeriesCateId,
                CarSeriesId = carSeriesId,
                PartsBrandId = partsBrandId,
                PartsBrandName = partsBrandName,
                PartsCateId = partsCateId,
                PartsCateName = partsCateName,
                PartsSpecs = partsSpecs
            };

            result = Add(model);

            return result;
        }

        /// <summary>
        /// 编辑部件
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="partName"></param>
        /// <param name="price"></param>
        /// <param name="carBrandId"></param>
        /// <param name="carSeriesCateId"></param>
        /// <param name="carSeriesId"></param>
        /// <param name="carModelId"></param>
        /// <param name="count"></param>
        /// <param name="updater"></param>
        /// <returns></returns>
        public bool EditPart(
                int partId,
                string partName,
                double price,
                int carBrandId,
                int carSeriesCateId,
                int carSeriesId,
                int carModelId,
                int count,
                string updater,
                int partsCateId,
                string partsCateName,
                int partsBrandId,
                string partsBrandName,
                string partsSpecs
            )
        {
            bool result = false;

            CarPartsInfo model =
                Get<CarPartsInfo>(string.Format(" PartId = {0} ", partId));

            if (model == null) return false;

            model.PartName = partName;
            model.Price = price;
            model.CarBrandId = carBrandId;
            model.CarSeriesCateId = carSeriesCateId;
            model.CarSeriesId = carSeriesId;
            model.CarModelId = carModelId;
            model.Count = count;
            model.Updater = updater;
            model.UpdateTime = DateTime.Now;

            model.PartsBrandId = partsBrandId;
            model.PartsBrandName = partsBrandName;
            model.PartsCateId = partsCateId;
            model.PartsCateName = partsCateName;
            model.PartsSpecs = partsSpecs;

            result = Update(model);

            return result;
        }

        //public bool EditPart(
        //        int partId,
        //        string partName,
        //        double price,
        //        int carModelId,
        //        int status,
        //        string updater
        //    )
        //{
        //    bool result = false;

        //    CarPartsInfo model =
        //        Get<CarPartsInfo>(string.Format(" PartId = {0} ",partId));

        //    if (model == null) return false;

        //    model.PartName = partName;
        //    model.Price = price;
        //    model.CarModelId = carModelId;
        //    model.Status = status;
        //    model.Updater = updater;
        //    model.UpdateTime = DateTime.Now;

        //    result = Update(model);

        //    return result;
        //}

        //public bool EditPart(
        //        int partId,
        //        int status,
        //        string updater
        //    )
        //{
        //    bool result = false;

        //    CarPartsInfo model =
        //        Get<CarPartsInfo>(string.Format(" PartId = {0} ", partId));

        //    if (model == null) return false;

        //    model.Status = status;
        //    model.Updater = updater;
        //    model.UpdateTime = DateTime.Now;

        //    result = Update(model);

        //    return result;
        //}

        //public int CopyParts(int partId, int count, string updater)
        //{
        //    int result = 0;

        //    CarPartsInfo model =
        //        Get<CarPartsInfo>(string.Format(" PartId = {0} ", partId));

        //    for (int i = 0; i < count; i++)
        //    {
        //        CarPartsInfo newModel = new CarPartsInfo()
        //        {
        //            CarModelId = model.CarModelId,
        //            PartId = int.Parse(GetGUID(TransacType.CommAdd)),
        //            PartName = model.PartName,
        //            Price = model.Price,
        //            Updater = updater,
        //            UpdateTime = DateTime.Now,
        //            WebsiteOwner = model.WebsiteOwner
        //        };

        //        if (Add(newModel))
        //        {
        //            result++;
        //        }

        //    }

        //    return result;
        //}

        public CarPartsInfo GetPartById(int partId)
        {
            return Get<CarPartsInfo>(string.Format(" PartId = {0} ", partId));
        }

        public List<CarPartsInfo> GetPartsByIds(List<int> ids)
        {
            if (ids.Count == 0) return null;

            return GetList<CarPartsInfo>(string.Format(" PartId IN ({0}) ",
                    MySpider.MyStringHelper.ListToStr<int>(ids, "", ",")
                ));
        }

        /// <summary>
        /// 删除部件
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public bool DeletePartsById(int partId)
        {
            var result = Update(new Model.CarPartsInfo(), " IsDelete = 1 ", string.Format(" PartId = {0} ", partId));

            return result > 0;
        }
        public int DeletePartsById(string ids)
        {
            var result = Update(new Model.CarPartsInfo(), " IsDelete = 1 ", string.Format(" PartId in ({0}) ", ids)); //Delete(new Model.CarPartsInfo(), string.Format(" PartId in ({0}) ", ids));

            return result;
        }


        #endregion


        #region 服务管理

        //服务管理  服务列表查询  服务添加 服务删除  服务修改

        //pureCarServerRootId

        /// <summary>
        /// 获取pureCar服务分类根节点
        /// </summary>
        /// <returns></returns>
        public int GetPureCarServerRootId()
        {
            return Common.ConfigHelper.GetConfigInt("pureCarServerRootId");
        }

        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="cateId"></param>
        /// <param name="carBrandId"></param>
        /// <param name="carSeriesCateId"></param>
        /// <param name="carSeriesId"></param>
        /// <param name="carModelId"></param>
        /// <returns></returns>
        public List<CarServerInfo> GetServerList(
                out int totalCount,
                string websiteOwner,
                int pageSize,
                int pageIndex,
                int cateId = 0,
                int carBrandId = 0,
                int carSeriesCateId = 0,
                int carSeriesId = 0,
                int carModelId = 0,
                string serverType = "",
                string shopType = ""
            )
        {
            List<CarServerInfo> result = new List<CarServerInfo>();

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);

            if (!string.IsNullOrWhiteSpace(serverType))
            {
                strWhere.AppendFormat(" AND ServerType = '{0}' ", serverType);
            }

            if (!string.IsNullOrWhiteSpace(shopType))
            {
                strWhere.AppendFormat("  AND  ShopType = '{0}' ", shopType);
            }

            if (carBrandId > 0)
            {
                strWhere.AppendFormat(" AND  CarBrandId = {0} ", carBrandId);
            }
            if (carSeriesCateId > 0)
            {
                strWhere.AppendFormat(" AND  CarSeriesCateId = {0} ", carSeriesCateId);
            }
            if (carSeriesId > 0)
            {
                strWhere.AppendFormat(" AND  CarSeriesId = {0} ", carSeriesId);
            }
            if (carModelId > 0)
            {
                strWhere.AppendFormat(" AND  CarModelId = {0} ", carModelId);
            }
            if (cateId > 0)
            {
                strWhere.AppendFormat(" AND  CateId = {0} ", cateId);
            }


            totalCount = GetCount<CarServerInfo>(strWhere.ToString());
            result = GetLit<CarServerInfo>(pageSize, pageIndex, strWhere.ToString(), " ServerId DESC ");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public CarServerInfo GetServer(int serverId)
        {
            CarServerInfo result = Get<CarServerInfo>(" ServerId = " + serverId.ToString());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<Model.CarServerParts> GetServerPartsList(int serverId, string sallerId)
        {
            List<Model.CarServerParts> result = new List<CarServerParts>();

            //查出所有默认及指定商户的配件

            //var defList = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);
            //var sallerList = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            strWhere.AppendFormat(" AND RelationType IN ('{0}','{1}') AND  MainId = '{2}'  ",
                        CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.CarServerPartsDef),
                        CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.CarServerPartsSaller),
                        serverId
                    );

            var relatioDataList = GetList<Model.CommRelationInfo>(strWhere.ToString());

            var partsIds = relatioDataList.Select(p => p.RelationId).Distinct().ToList();

            var partsList = GetList<Model.CarPartsInfo>(string.Format(" PartId IN ({0}) ", Common.StringHelper.ListToStr<string>(partsIds, "", ",")));

            foreach (var item in relatioDataList)
            {
                Model.CarServerParts data = Model.CarServerParts.GetObjByCommRelation(item);

                //data.Parts = GetPartById(Convert.ToInt32(data.RelationId));

                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<Model.CarServerParts> GetServerAllRelation(int serverId)
        {
            List<Model.CarServerParts> result = new List<CarServerParts>();

            StringBuilder strWhere = new StringBuilder(" 1=1 ");

            strWhere.AppendFormat(" AND RelationType IN ('{0}','{1}','{3}') AND  MainId = '{2}'  ",
                        CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.CarServerPartsDef),
                        CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.CarServerPartsSaller),
                        serverId,
                        CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.CarServerSaller)
                    );

            var relatioDataList = GetList<Model.CommRelationInfo>(strWhere.ToString());

            var partsIds = relatioDataList.Where(p => p.RelationType == "CarServerPartsDef" || p.RelationType == "CarServerPartsSaller").Select(s => s.RelationId).Distinct().ToList();

            //List<Model.CarPartsInfo> partsList = new List<Model.CarPartsInfo>();

            //if (partsIds.Count > 0)
            //{
            //    partsList = GetList<Model.CarPartsInfo>(string.Format(" PartId IN ({0}) ", Common.StringHelper.ListToStr<string>(partsIds, "", ",")));
            //}

            foreach (var item in relatioDataList)
            {
                Model.CarServerParts data = Model.CarServerParts.GetObjByCommRelation(item);

                //if (item.RelationType == "CarServerPartsDef" || item.RelationType == "CarServerPartsSaller")
                //{
                //    data.Parts = partsList.FirstOrDefault(p => p.PartId == Convert.ToInt32(data.RelationId));//GetPartById(Convert.ToInt32(data.RelationId));
                //}

                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// 获取商户服务配件列表
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<Model.CarServerParts> GetServerSallerParts(int serverId, string sallerId)
        {
            List<Model.CarServerParts> result = new List<CarServerParts>();

            List<Model.CommRelationInfo> parts = new List<CommRelationInfo>();
            //var defList = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);
            //var sallerList = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);

            parts = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsSaller, serverId.ToString(), sallerId, 1, 1000);

            //if (parts == null || parts.Count == 0)
            //{
            //    parts = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);
            //}

            //if (parts != null && parts.Count > 0)
            //{
            //    //TODO:
            //}

            foreach (var item in parts)
            {
                result.Add(Model.CarServerParts.GetObjByCommRelation(item));
            }

            return result;
        }

        /// <summary>
        /// 获取商户服务配件列表
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public List<Model.CarServerParts> GetServerDefParts(int serverId)
        {
            List<Model.CarServerParts> result = new List<CarServerParts>();

            List<Model.CommRelationInfo> parts = new List<CommRelationInfo>();

            parts = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerPartsDef, serverId.ToString(), "", 1, 1000);

            foreach (var item in parts)
            {
                result.Add(Model.CarServerParts.GetObjByCommRelation(item));
            }

            return result;
        }

        /// <summary>
        /// 获取服务关联的商户列表
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetServerSallerList(int serverId)
        {
            List<Model.UserInfo> result = new List<UserInfo>();

            var relationData = bllCommRelation.GetRelationList(Enums.CommRelationType.CarServerSaller, serverId.ToString(), "", 1, 1000);

            foreach (var item in relationData)
            {
                var data = userBll.GetUserInfo(item.RelationId);
                if (data != null)
                {
                    result.Add(data);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定商户指定服务下七天所有价格
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<Model.CarServerDayPrice> GetCarServerNext7DayPrice(int serverId,string sallerId)
        {
            //某个时间段价格组成：服务工时*（店铺默认工时单价/指定车型工时单价）* 工时折扣率 + 配件价格（服务配件默认配件总和/制定商户配件总和）* 配件折扣率

            List<Model.CarServerDayPrice> result = new List<CarServerDayPrice>();

            var server = GetServer(serverId);
            var saller = userBll.GetUserInfo(sallerId);//工时价  Ex1

            if (server == null || saller == null)
            {
                return null;
            }

            DateTime dt = DateTime.Now.AddDays(1);
            
            for (int i = 0; i < 7; i++)
            {
                var day = dt.ToString("yyyy-MM-dd");
                
                //TODO:计算每天三个时间段价格
                List<string> timeInterval = new List<string>() { "8:30-11:30", "11:30-14:30", "14:30-17:30" };
                for (int j = 0; j < timeInterval.Count; j++)
                {
                    Model.CarServerDayPrice dayData = new CarServerDayPrice()
                    {
                        Date = day,
                        StartTime = timeInterval[j].Split('-')[0],
                        EndTime = timeInterval[j].Split('-')[1],
                        PartsRate = 1,
                        WorkhoursRate = 1,
                        Workhours = server.WorkHours
                    };

                    double workhoursPriceBySaller = 0;

                    if (double.TryParse(saller.Ex1, out workhoursPriceBySaller))
                    {
                        dayData.WorkhoursPrice = workhoursPriceBySaller;
                    }

                    //获取工时折扣和配件折扣
                    CarDiscountRateInfo discountRateInfo = Get<CarDiscountRateInfo>(string.Format(" SallerId = '{0}' AND Week = {1} AND StartTime = '{2}' AND EndTime = '{3}' ",
                            sallerId,
                            dayData.Week,
                            dayData.StartTime,
                            dayData.EndTime
                        ));
                    if (discountRateInfo != null)
                    {
                        dayData.WorkhoursRate = discountRateInfo.WorkhoursRate;
                        dayData.PartsRate = discountRateInfo.PartsRate;
                    }

                    //查询该服务的车型，是否有店铺指定车型工时价
                    CarWorkhoursPriceInfo workhoursPriceInfo = Get<CarWorkhoursPriceInfo>(string.Format(" SallerId = '{0}' AND CarModelId = {1} ", sallerId, server.CarModelId));
                    if (workhoursPriceInfo != null)
                    {
                        dayData.WorkhoursPrice = workhoursPriceInfo.Price;
                    }

                    //查询配件总价格
                    //查询服务配件
                    var parts = GetServerDefParts(serverId);

                    //如何没有服务配件则查询默认配件
                    if (parts.Count == 0)
                    {
                        parts = GetServerSallerParts(serverId, sallerId);
                    }

                    dayData.PartsDetail = new List<CarServerDayPartsDetail>();

                    foreach (var item in parts)
                    {
                        int count = 0;

                        if (int.TryParse(item.Ex1, out count))
                        {

                        }

                        if (item.Parts != null)
                        {
                            dayData.PartsDetail.Add(new CarServerDayPartsDetail()
                            {
                                PartsId = item.Parts.PartId,
                                Count = count,
                                PartsName = item.Parts.PartName,
                                Price = item.Parts.Price.Value
                            }
                            );
                            dayData.PartsTotalPrice += item.Parts.Price.Value * count;
                        }

                    }

                    dayData.Price = dayData.Workhours * dayData.WorkhoursPrice * dayData.WorkhoursRate + dayData.PartsTotalPrice * dayData.PartsRate;

                    result.Add(dayData);
                }

                dt = dt.AddDays(1);
            }
            
            return result;
        }


        #endregion



        #region 工时表优惠信息管理
        //工时表优惠信息查询：列表、指定时段
        //工时表优惠信息添加
        //公司表优惠信息删除

        /// <summary>
        /// 查询工时表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<CarWorkhoursPriceInfo> QueryCarWorkhoursPrice(out int totalCount,int pageIndex,int pageSize,string sallerId)
        {
            List<CarWorkhoursPriceInfo> result = new List<CarWorkhoursPriceInfo>();
            
            StringBuilder strWhere = new StringBuilder(" 1=1 ");
            strWhere.AppendFormat(" And WebsiteOwer = '{0}' ",WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(sallerId))
            {
                strWhere.AppendFormat(" And SallerId ", sallerId);
            }
            
            result = GetLit<CarWorkhoursPriceInfo>(pageSize,pageIndex,strWhere.ToString(), " AutoId Desc ");

            totalCount = GetCount<CarWorkhoursPriceInfo>(strWhere.ToString());

            return result;
        }

        public CarWorkhoursPriceInfo GetCarWorkhoursPriceInfo(int id)
        {
            return Get<CarWorkhoursPriceInfo>(string.Format(" AutoId = {0} ", id));
        }

        public int DeleteCarWorkhoursPrice(string ids)
        {
            return Delete(new CarWorkhoursPriceInfo(), string.Format(" AutoId in ({0}) And WebsiteOwer = '{1}' ", ids, WebsiteOwner));
        }

        /// <summary>
        /// 查询折扣率
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public List<CarDiscountRateInfo> QueryCarDiscountRate(out int totalCount, int pageIndex, int pageSize, string sallerId)
        {
            List<CarDiscountRateInfo> result = new List<CarDiscountRateInfo>();

            StringBuilder strWhere = new StringBuilder(" 1=1 ");
            strWhere.AppendFormat(" And WebsiteOwer = '{0}' ", WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(sallerId))
            {
                strWhere.AppendFormat(" And SallerId ", sallerId);
            }

            result = GetLit<CarDiscountRateInfo>(pageSize, pageIndex, strWhere.ToString(), " AutoId Desc ");

            totalCount = GetCount<CarDiscountRateInfo>(strWhere.ToString());

            return result;
        }

        public CarDiscountRateInfo GetCarDiscountRateInfo(int id)
        {
            return Get<CarDiscountRateInfo>(string.Format(" AutoId = {0} ", id));
        }

        public int DeleteCarDiscountRate(string ids)
        {
            return Delete(new CarDiscountRateInfo(), string.Format(" AutoId in ({0}) ", ids));
        }

        #endregion


        #region 用户管理

        //车主认证信息添加

        //车主认证信息编辑

        //我的评价列表查询

        //待评价订单查询

        /// <summary>
        /// 更改用户汽车服务类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ChangeUserCarServerType(int type)
        {
            var userId = GetCurrUserID();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var result = Update(
                    new UserInfo(),
                    string.Format(" CarServerType = {0} ", type),
                    string.Format(" UserId = '{0}' ", userId)
                );

            return result > 0;
        }

        #endregion


        public List<Model.CarQuotationInfo> QueryCarQuotationInfo(
            out int totalCount,
            int pageSize,
            int pageIndex,
            string userId,
            string status,
            int carBrandId = 0,
            int carSeriesCateId = 0,
            int carSeriesId = 0,
            int crModelId = 0
            )
        {
            List<Model.CarQuotationInfo> result = new List<CarQuotationInfo>();

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WebSiteOwner = '{0}' ", WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                strWhere.AppendFormat(" AND  UserId = '{0}' ", userId);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                strWhere.AppendFormat(" AND Status IN ({0}) ", status);
            }

            totalCount = GetCount<CarQuotationInfo>(strWhere.ToString());
            result = GetLit<CarQuotationInfo>(pageSize, pageIndex, strWhere.ToString(), " QuotationId DESC ");

            return result;
        }

        /// <summary>
        /// 取消报价单
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public bool CancelCarQuotation(int quotationId)
        {
            return Update(
                    new CarQuotationInfo(),
                    string.Format(" CancelUserId = '{0}',CancelTime = '{1}',Status = 3 ", GetCurrUserID(), DateTime.Now),
                    string.Format(" QuotationId = '{0}' ", quotationId)
                ) > 0;
        }

        /// <summary>
        /// 获取购车报价单详情
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public CarQuotationInfo GetCarQuotation(int quotationId)
        {
            return Get<CarQuotationInfo>(string.Format(" QuotationId = {0} ", quotationId));
        }

    }
}
