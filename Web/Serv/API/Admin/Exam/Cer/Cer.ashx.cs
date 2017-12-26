using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Exam.Cer
{
    /// <summary>
    /// 证书管理
    /// </summary>
    public class Cer : BaseHandlerNeedLoginAdmin
    {


        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {


            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            string keyWord = context.Request["keyword"];
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (BarCode='{0}' Or CodeName='{0}' Or ModelCode='{0}')", keyWord);
            }
            var totalCount = bll.GetCount<BarCodeInfo>(sbWhere.ToString());
            var sourceData = bll.GetLit<BarCodeInfo>(rows, page, sbWhere.ToString(), "AutoId DESC");

            var data = new
            {
                total = totalCount,
                rows = sourceData//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            List<BarCodeInfo> dataList = new List<BarCodeInfo>();
            BarCodeInfo model = new BarCodeInfo();
            if (!string.IsNullOrEmpty(context.Request["img1"]))
            {
                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img1"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img2"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img2"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img3"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img3"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img4"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img4"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img5"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img5"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img6"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img6"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img7"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img7"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img8"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img8"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img9"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img9"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img10"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img10"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img11"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img11"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }
            if (!string.IsNullOrEmpty(context.Request["img12"]))
            {

                model = new BarCodeInfo();
                model.ImageUrl = context.Request["img12"];
                model.BarCode = GetCodeByFileName(model.ImageUrl);
                model.websiteOwner = bll.WebsiteOwner;
                model.InsetData = DateTime.Now.ToString();
                dataList.Add(model);
            }


            //var model = bll.ConvertRequestToModel<BarCodeInfo>(new BarCodeInfo());
            //if (string.IsNullOrEmpty(model.BarCode))
            //{
            //    model.BarCode = GetCodeByFileName(model.ImageUrl);
            //}
            //model.websiteOwner = bll.WebsiteOwner;
            //model.InsetData = DateTime.Now.ToString();
            if (bll.AddList(dataList))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "上传失败";
            }


            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        ///编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            var requestModel = bll.ConvertRequestToModel<BarCodeInfo>(new BarCodeInfo());
            var model = bll.Get<BarCodeInfo>(string.Format("AutoId={0}", requestModel.AutoId));
            model.BarCode = requestModel.BarCode;
            if (string.IsNullOrEmpty(model.BarCode))
            {
                model.BarCode = GetCodeByFileName(model.ImageUrl);
            }
            model.ImageUrl = requestModel.ImageUrl;
            model.CodeName = requestModel.CodeName;
            model.ModelCode = requestModel.ModelCode;
            if (bll.Update(model))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "编辑失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            var ids = context.Request["ids"];

            if (bll.Delete(new BarCodeInfo(),string.Format(" AutoId In({0})",ids))>0)
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "删除失败";
            }


            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }


        /// <summary>
        /// 根据文件名获取证书编号
        /// </summary>
        /// <returns></returns>
        private string GetCodeByFileName(string fileName) {
            string code = "";
            try
            {
                //string fileEx=System.IO.Path.GetExtension(fileName);
                //code = fileName.Replace(fileEx,null);
                code=System.IO.Path.GetFileNameWithoutExtension(fileName);

            }
            catch (Exception)
            {
                
              
            }
            return code;
        
        
        }

    }
}