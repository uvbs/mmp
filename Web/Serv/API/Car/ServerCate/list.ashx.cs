using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerCate
{
    /// <summary>
    /// 获取服务分类：根据车型查询有没有服务，再查询所有所属的服务分类
    /// </summary>
    public class list : CarBaseHandler
    {

        BLLJIMP.BLLArticleCategory bllCate = new BLLJIMP.BLLArticleCategory();

        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                var pageSize = Convert.ToInt32(context.Request["pageSize"]);
                var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
                var carModelId = Convert.ToInt32(context.Request["carModelId"]);
                var serverType = context.Request["serverType"];

                int totalCount = 0;

                pageSize = pageSize == 0 ? 20000 : pageSize;
                pageIndex = pageIndex == 0 ? 1 : pageIndex;

                var list = bll.GetServerList(out totalCount, bll.WebsiteOwner, pageSize, pageIndex, 0, 0, 0, 0, carModelId,serverType);

                var serverCateRootId = bll.GetPureCarServerRootId();

                var cateList = list.Where(p => p.CateId > 0).Select(l => bllCate.GetArticleCategory(l.CateId)).Where(q => q != null).ToList();

                if (cateList != null)
                {
                    cateList = cateList.DistinctBy(p => p.AutoID).ToList();
                }

                //直接一级
                var level1 = cateList.Where(p => p.PreID.Equals(serverCateRootId)).ToList();

                var tmpLevel1 = new List<BLLJIMP.Model.ArticleCategory>();
                //查询一级
                foreach (var item in cateList.Where(p => !p.PreID.Equals(serverCateRootId) && !p.PreID.Equals(0)))
                {
                    if (tmpLevel1.Count(l => l.AutoID.Equals(item.PreID)) > 0) continue;
                    
                    var tmpCate = bllCate.GetArticleCategory(item.PreID);

                    if (tmpCate != null) tmpLevel1.Add(tmpCate);                    
                }

                foreach (var item in tmpLevel1)
                {
                    if (level1.Count(p => p.AutoID.Equals(item.AutoID)) > 0) continue;

                    level1.Add(item);
                }

                List<ReturnCate> resultList = new List<ReturnCate>();
                
                //填充二级 child
                foreach (var item in level1)
                {
                    ReturnCate cate = new ReturnCate()
                    {
                        id = item.AutoID,
                        pre_id = item.PreID,
                        name = item.CategoryName,
                        img = item.ImgSrc,
                        child = new List<ReturnCate>()
                    };

                    foreach (var child in cateList.Where(p => p.PreID.Equals(cate.id)))
                    {
                        cate.child.Add(new ReturnCate()
                        {
                             id = child.AutoID,
                             pre_id = child.PreID,
                             name = child.CategoryName,
                             img = child.ImgSrc
                        });
                    }

                    resultList.Add(cate);
                }
                
                resp.isSuccess = true;
                
                resp.returnObj = new
                {
                    list = resultList
                };
            }
            catch (Exception ex)                                                                                                                                                                                                                                     
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

        /// <summary>
        /// 返回分类
        /// </summary>
        private struct ReturnCate
        {
            public int id { get; set; }
            public string name { get; set; }
            public int pre_id { get; set; }
            public string img { get; set; }

            public List<ReturnCate> child { get; set; }

        }
    }
}