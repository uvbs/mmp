using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 运费模板
    /// </summary>
    public class FreightTemplate : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 键值数据
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();

        /// <summary>
        /// 运费模板列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            var sourceData = bllMall.GetFreightTemplateList();
            var list = from p in sourceData
                       select new
                       {
                           freight_template_id = p.TemplateId,
                           freight_template_name = p.TemplateName,
                           is_enable = p.IsEnable,
                           valuation_rules = GetFreightTemplateAreaObj(bllMall.GetFreightTemplateRuleList(p.TemplateId)),
                           last_modiffty_date = p.LastModifyDate != null ? bllMall.GetTimeStamp((DateTime)p.LastModifyDate) : 0,
                           calc_type=p.CalcType
                       };
            var data = new
            {
                totalcount = list.Count(),
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取运费模板详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string freightTemplateId = context.Request["freight_template_id"];
            if (string.IsNullOrEmpty(freightTemplateId))
            {
                resp.errcode = 1;
                resp.errmsg = "运费模板Id不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var sourceData = bllMall.GetFreightTemplate(int.Parse(freightTemplateId));
            if (sourceData == null)
            {
                resp.errcode = 1;
                resp.errmsg = "运费模板不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var data = new
            {
                freight_template_id = sourceData.TemplateId,
                freight_template_name = sourceData.TemplateName,
                is_enable = sourceData.IsEnable,
                valuation_rules = GetFreightTemplateAreaObj(bllMall.GetFreightTemplateRuleList(sourceData.TemplateId)),
                last_modiffty_date = sourceData.LastModifyDate != null ? bllMall.GetTimeStamp((DateTime)sourceData.LastModifyDate) : 0,
                calc_type=sourceData.CalcType,
                freight_limit_type=sourceData.FreightFreeLimitType,
                freight_free_limit=sourceData.FreightFreeLimitValue
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 增加运费模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string data = context.Request["data"];
            try
            {
                FreightTemplateModel requestModel = ZentCloud.Common.JSONHelper.JsonToModel<FreightTemplateModel>(data);

                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {

                    #region 数据有效性检查
                    if (string.IsNullOrEmpty(requestModel.freight_template_name))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "模板名称不能为空";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                    }
                    if (requestModel.valuation_rules.Count == 0)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "配送区域不能为空";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    foreach (var rule in requestModel.valuation_rules)
                    {
                        if (rule.area_code_list.Count == 0)
                        {
                            resp.errcode = 1;
                            resp.errmsg = "配送区域不能为空";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                    }
                    List<string> areaCodeList = new List<string>();
                    foreach (var rule in requestModel.valuation_rules)
                    {
                        foreach (string areaCode in rule.area_code_list)
                        {
                            areaCodeList.Add(areaCode);

                        }
                    }

                    if (areaCodeList.Count != areaCodeList.Distinct().Count())
                    {
                        resp.errcode = 1;
                        resp.errmsg = "配送区域重复,请检查";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                    }

                    #endregion

                    BLLJIMP.Model.FreightTemplate freightTemplateInfo = new BLLJIMP.Model.FreightTemplate();
                    freightTemplateInfo.InsertDate = DateTime.Now;
                    freightTemplateInfo.IsEnable = requestModel.is_enable;
                    freightTemplateInfo.TemplateId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddFreightTemplate));
                    freightTemplateInfo.TemplateName = requestModel.freight_template_name;
                    freightTemplateInfo.WebsiteOwner = bllMall.WebsiteOwner;
                    freightTemplateInfo.CalcType =!string.IsNullOrEmpty(requestModel.calc_type)?requestModel.calc_type:"count";//默认按件数计费
                    freightTemplateInfo.LastModifyDate = DateTime.Now;
                    freightTemplateInfo.FreightFreeLimitValue = requestModel.freight_free_limit;
                    freightTemplateInfo.FreightFreeLimitType = requestModel.freight_limit_type;
                    if (!bllMall.Add(freightTemplateInfo, tran))
                    {

                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "插入模板表失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                    foreach (var rule in requestModel.valuation_rules)
                    {
                        BLLJIMP.Model.FreightTemplateRule freightTemplateRule = new BLLJIMP.Model.FreightTemplateRule();
                        freightTemplateRule.InitialProductCount = rule.initial_product_count;
                        freightTemplateRule.InitialAmount = rule.initial_amount;
                        freightTemplateRule.AddProductCount = rule.add_product_count;
                        freightTemplateRule.AddAmount = rule.add_amount;
                        freightTemplateRule.AreaCodes = string.Join(",", rule.area_code_list);
                        freightTemplateRule.TemplateId = freightTemplateInfo.TemplateId;
                        freightTemplateRule.WebsiteOwner = bllMall.WebsiteOwner;
                        if (freightTemplateRule.AddProductCount==0)
                        {
                            freightTemplateRule.AddProductCount = 1;
                        }
                        if (!bllMall.Add(freightTemplateRule))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "插入模板规则表失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }




                    }



                    tran.Commit();
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = ex.Message;

                }


            }
            catch (Exception)
            {

                resp.errcode = 1;
                resp.errmsg = "JSON 格式错误,请检查";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 编辑运费模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string data = context.Request["data"];
            try
            {
                ///模板更新规则
                ///1更新模板表信息
                ///2删除旧的规则
                ///3创建新的规则到规则表

                FreightTemplateModel requestModel = ZentCloud.Common.JSONHelper.JsonToModel<FreightTemplateModel>(data);
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {
                    //检查
                    BLLJIMP.Model.FreightTemplate freightTemplateInfo = bllMall.GetFreightTemplate(requestModel.freight_template_id);
                    if (freightTemplateInfo == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "运费模板不存在,请检查";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                    #region 数据有效性检查

                    if (string.IsNullOrEmpty(requestModel.freight_template_name))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "模板名称不能为空";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                    }
                    if (requestModel.valuation_rules.Count == 0)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "配送区域不能为空";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    foreach (var rule in requestModel.valuation_rules)
                    {
                        if (rule.area_code_list.Count == 0)
                        {
                            resp.errcode = 1;
                            resp.errmsg = "配送区域不能为空";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                    }

                    List<string> areaCodeList = new List<string>();
                    foreach (var rule in requestModel.valuation_rules)
                    {
                        foreach (string areaCode in rule.area_code_list)
                        {
                            areaCodeList.Add(areaCode);

                        }
                    }
                    if (areaCodeList.Count != areaCodeList.Distinct().Count())
                    {
                        resp.errcode = 1;
                        resp.errmsg = "配送区域重复,请检查";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                    }

                    #endregion

                    freightTemplateInfo.IsEnable = requestModel.is_enable;
                    freightTemplateInfo.TemplateName = requestModel.freight_template_name;
                    freightTemplateInfo.LastModifyDate = DateTime.Now;
                    freightTemplateInfo.CalcType = !string.IsNullOrEmpty(requestModel.calc_type) ? requestModel.calc_type : "count";//默认按件数计费
                    freightTemplateInfo.FreightFreeLimitType = requestModel.freight_limit_type;
                    freightTemplateInfo.FreightFreeLimitValue = requestModel.freight_free_limit;
                    if (!bllMall.Update(freightTemplateInfo, tran))
                    {

                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "更新模板表失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                    //
                    //先删除旧的规则表
                    if (bllMall.Delete(new FreightTemplateRule(), string.Format(" TemplateId ={0}", freightTemplateInfo.TemplateId), tran) <= 0)
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "删除旧规则表失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    //先删除旧的规则表


                    //再创建新的规则表
                    foreach (var rule in requestModel.valuation_rules)
                    {
                        BLLJIMP.Model.FreightTemplateRule freightTemplateRule = new BLLJIMP.Model.FreightTemplateRule();
                        freightTemplateRule.InitialProductCount = rule.initial_product_count;
                        freightTemplateRule.InitialAmount = rule.initial_amount;
                        freightTemplateRule.AddProductCount = rule.add_product_count;
                        freightTemplateRule.AddAmount = rule.add_amount;
                        freightTemplateRule.AreaCodes = string.Join(",", rule.area_code_list);
                        freightTemplateRule.TemplateId = freightTemplateInfo.TemplateId;
                        freightTemplateRule.WebsiteOwner = bllMall.WebsiteOwner;
                        if (freightTemplateRule.AddProductCount == 0)
                        {
                            freightTemplateRule.AddProductCount = 1;
                        }
                        if (!bllMall.Add(freightTemplateRule))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "插入模板规则表失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }


                    }
                    tran.Commit();
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = ex.Message;

                }


            }
            catch (Exception)
            {

                resp.errcode = 1;
                resp.errmsg = "JSON 格式错误,请检查";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除运费模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            int freightTemplateId = int.Parse(context.Request["freight_template_id"]);
            if (bllMall.DeleteFreightTemplate(freightTemplateId))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 把 用逗号分隔的省市区代码转换成数组
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string[] ConvertAreaCodeToArry(string code)
        {

            return !string.IsNullOrEmpty(code) ? (code.Split(',')) : (new string[0]);

        }

        /// <summary>
        /// 可配送区域对象 用于接口返回
        /// </summary>
        /// <param name="areaList"></param>
        /// <returns></returns>
        public object GetFreightTemplateAreaObj(List<FreightTemplateRule> areaList)
        {

            var list = from p in areaList
                       select new
                       {
                           rule_id = p.AutoId,
                           area_code_list = ConvertAreaCodeToArry(p.AreaCodes),
                           area_name_list = ConvertToAreaNameArry(ConvertAreaCodeToArry(p.AreaCodes)),
                           initial_product_count = p.InitialProductCount,
                           initial_amount = p.InitialAmount,
                           add_product_count = p.AddProductCount,
                           add_amount = p.AddAmount

                       };
            return list;



        }

        /// <summary>
        /// 把用省市区代码数组转换成对应的名称
        /// </summary>
        /// <param name="area"></param>
        /// <param name="codeList"></param>
        /// <returns></returns>
        public List<string> ConvertToAreaNameArry(string[] codeList)
        {
            List<string> areaNameList = new List<string>();
            foreach (var item in codeList)
            {

                var keyValueData = bllKeyValue.Get<KeyVauleDataInfo>(string.Format(" DataKey='{0}' And DataType in('Province','City','District')", item));
                if (keyValueData != null)
                {
                    areaNameList.Add(keyValueData.DataValue);
                }




            }

            return areaNameList;

        }

        /// <summary>
        /// 运费模板模型
        /// </summary>
        public class FreightTemplateModel
        {

            /// <summary>
            /// 运费模板ID
            /// </summary>
            public int freight_template_id { get; set; }
            /// <summary>
            /// 运费模板名称
            /// </summary>
            public string freight_template_name { get; set; }
            /// <summary>
            /// 启用 禁用 1启用 0禁用
            /// </summary>
            public int is_enable { get; set; }
            /// <summary>
            /// 计算方式
            /// count  按数量
            /// weight 按重量
            /// volume 按体积
            /// </summary>
            public string calc_type { get; set; }

            /// <summary>
            /// 0 不开启   
            /// 1 满多少件（重量）  
            /// 2 满多少金额
            /// </summary>
            public int freight_limit_type { get; set; }

            /// <summary>
            /// 包邮类型值
            /// </summary>
            public decimal freight_free_limit { get; set; }
            /// <summary>
            /// 规则列表
            /// </summary>
            public List<ValuationRules> valuation_rules { get; set; }

        }

        /// <summary>
        /// 配送规则
        /// </summary>
        public class ValuationRules
        {

            /// <summary>
            ///规则ID
            /// </summary>
            public int rule_id { get; set; }
            /// <summary>
            /// 省市区代码列表
            /// </summary>
            public List<string> area_code_list { get; set; }
            /// <summary>
            /// 首件
            /// 首重
            /// </summary>
            public decimal initial_product_count { get; set; }
            /// <summary>
            ///首费
            /// </summary>
            public decimal initial_amount { get; set; }
            /// <summary>
            /// 续件
            /// 续重
            /// </summary>
            public decimal add_product_count { get; set; }
            /// <summary>
            /// 续费
            /// </summary>
            public decimal add_amount { get; set; }

        }


    }
}