using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using System.Web.SessionState;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXBarCodeHandler 的摘要说明
    /// </summary>
    public class WXBarCodeHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// BLL基类
        /// </summary>
        BLL bll = new BLL();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser userBll = new BLLUser();  //用户数据
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;  //站点拥有者，也就是当前用户的父类或同一个人
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {
                this.currentUserInfo = bll.GetCurrentUserInfo();
                this.currWebSiteUserInfo = this.userBll.GetUserInfo(bll.WebsiteOwner);
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "请联系管理员";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);

        }


        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadCodeInfoData(HttpContext context)
        {
            HttpPostedFile postFile = context.Request.Files["pictPath"];
            if (string.IsNullOrEmpty(postFile.FileName))
            {
                resp.Status = -1;
                resp.Msg = "请选择文件";
            }
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(postFile.FileName).ToLower();
            string newFilePath = context.Server.MapPath("~/FileUpload/") + newFileName;
            postFile.SaveAs(newFilePath);
            if (!File.Exists(newFilePath))
            {
                resp.Status = -1;
                resp.Msg = "文件不存在";
            }
            int dataCount = 0;
            int totalCount = 0;
            int repeatCount = 0;
            List<BLLJIMP.Model.BarCodeInfo> data = DatabaleToList(ExeclToDatabale(newFilePath));
            totalCount = data.Count;
            foreach (BLLJIMP.Model.BarCodeInfo item in data)
            {
                int num = bll.GetCount<BLLJIMP.Model.BarCodeInfo>(string.Format(" BarCode='{0}'", item.BarCode));
                if (num > 0)
                {
                    repeatCount = repeatCount + 1;
                }
                else
                {
                    dataCount = dataCount + 1;
                    bll.Add(item);
                }
            }
            resp.Status = 0;
            resp.Msg = string.Format("上传{0}条数据，录入{1}条，{2}条重复", totalCount, dataCount, repeatCount);

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// execl转换dataBale
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private DataTable ExeclToDatabale(string filePath)
        {
            List<string> sheetNameList = new List<string>();
            string strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'", filePath);
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string sheetName = "";
            System.Data.DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sheetName = dt.Rows[i]["TABLE_NAME"].ToString();

                sheetNameList.Add(sheetName);
            }
            string currSheet = sheetNameList[0].ToString();
            OleDbDataAdapter oda = new OleDbDataAdapter("select * from[" + currSheet + "]", conn);
            DataSet ds = new DataSet();
            oda.Fill(ds, "tmp");
            conn.Close();
            return ds.Tables[0];
        }

        /// <summary>
        /// DataTable转换 List<BLLJIMP.Model.BarCodeInfo>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BLLJIMP.Model.BarCodeInfo> DatabaleToList(DataTable dt)
        {
            List<BLLJIMP.Model.BarCodeInfo> data = new List<BLLJIMP.Model.BarCodeInfo>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        data.Add(new BLLJIMP.Model.BarCodeInfo()
                        {
                            CodeName = item[0].ToString(),
                            BarCode = item[1].ToString(),
                            ModelCode = item[2].ToString(),
                            Agency = item[3].ToString(),
                            InsetData = item[4].ToString(),
                            websiteOwner = bll.WebsiteOwner

                        });
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 更新或添加数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AUBarCodeInfoData(HttpContext context)
        {
            string codeId = context.Request["AutoId"];
            string codeName = context.Request["CodeName"];
            string barCode = context.Request["BarCode"];
            string modelCode = context.Request["ModelCode"];
            string agency = context.Request["Agency"];

            if (string.IsNullOrEmpty(codeName) || string.IsNullOrEmpty(barCode) || string.IsNullOrEmpty(modelCode) || string.IsNullOrEmpty(agency))
            {
                resp.Status = -1;
                resp.Msg = "请填写完整信息";
                goto OutF;
            }
            BLLJIMP.Model.BarCodeInfo model = bll.Get<BLLJIMP.Model.BarCodeInfo>(string.Format(" bll.WebSiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, codeId));
            if (model != null)
            {
                model.CodeName = codeName;
                model.BarCode = barCode;
                model.ModelCode = modelCode;
                model.Agency = agency;
                model.websiteOwner = bll.WebsiteOwner;
                bool IsTrue = bll.Update(model);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "更新成功";
                    goto OutF;
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "更新失败";
                    goto OutF;
                }
            }
            else
            {
                model = new BLLJIMP.Model.BarCodeInfo()
                {
                    CodeName = codeName,
                    BarCode = barCode,
                    ModelCode = modelCode,
                    Agency = agency,
                    InsetData = DateTime.Now.ToString("yyyy-MM-dd"),
                    websiteOwner = bll.WebsiteOwner
                };
                bool IsTrue = bll.Add(model);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "添加成功";
                    goto OutF;
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败";
                    goto OutF;
                }
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取产品真伪信息
        /// </summary>
        /// <returns></returns>
        private string GetBarCodeInfoData(HttpContext context)
        {
            string codeId = context.Request["codeId"];
            BLLJIMP.Model.BarCodeInfo model = bll.Get<BLLJIMP.Model.BarCodeInfo>(string.Format(" CodeId={0}", codeId));
            if (model != null)
            {
                resp.Status = 0;
                resp.ExObj = model;
            }
            else
            {
                resp.Status = -1;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取信息配置信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetConfigureConfigInfo(HttpContext context)
        {

            BLLJIMP.Model.ConfigBarCodeInfo model = bll.Get<BLLJIMP.Model.ConfigBarCodeInfo>(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (model != null)
            {
                resp.Status = 0;
                resp.ExObj = model;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 配置返回结果
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ConfigureConfigInfo(HttpContext context)
        {
            string popupInfo = context.Request["PopupInfo"];
            string queryNum = context.Request["QueryNum"];

            if (string.IsNullOrEmpty(popupInfo))
            {
                resp.Status = -1;
                resp.Msg = "请配置返回结果";
                goto OutF;
            }
            if (string.IsNullOrEmpty(queryNum))
            {
                resp.Status = -1;
                resp.Msg = "请输入查询";
                goto OutF;
            }
            BLLJIMP.Model.ConfigBarCodeInfo model = bll.Get<BLLJIMP.Model.ConfigBarCodeInfo>(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (model != null)
            {
                model.PopupInfo = popupInfo;
                model.QueryNum = Convert.ToInt32(queryNum);
                model.websiteOwner = bll.WebsiteOwner;
                bool IsTrue = bll.Update(model);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "更新成功";
                    goto OutF;
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "更新失败";
                    goto OutF;
                }
            }
            else
            {
                model = new BLLJIMP.Model.ConfigBarCodeInfo()
                {
                    PopupInfo = popupInfo,
                    QueryNum = Convert.ToInt32(queryNum),
                    websiteOwner = bll.WebsiteOwner
                };
                bool IsTrue = bll.Add(model);
                if (IsTrue)
                {
                    resp.Status = 0;
                    resp.Msg = "添加成功";
                    goto OutF;
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败";
                    goto OutF;
                }
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除信息\
        /// 根据id删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteBarCodeInfoId(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条记录进行操作！";
                goto OutF;
            }
            BLLJIMP.Model.BarCodeInfo model = new BLLJIMP.Model.BarCodeInfo();
            int count = bll.Delete(model, string.Format(" AutoId in ({0})", ids));
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询当前站点的所有真伪产品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetBarCodeInfoList(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.BarCodeInfo> dataList;
            string barCode = context.Request["BarCode"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(barCode))
            {
                sbWhere.AppendFormat(" AND BarCode lIKE '%{0}%'", barCode);
            }
            totalCount = this.bll.GetCount<BLLJIMP.Model.BarCodeInfo>(sbWhere.ToString());
            dataList = this.bll.GetLit<BLLJIMP.Model.BarCodeInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsetData desc");
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 清空查询次数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EmptyQuerys(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条记录进行操作！";
                goto OutF;
            }

            List<BLLJIMP.Model.BarCodeInfo> data = bll.GetList<BLLJIMP.Model.BarCodeInfo>(string.Format(" AutoId in ({0})", ids));
            if (data.Count > 0)
            {
                foreach (BLLJIMP.Model.BarCodeInfo item in data)
                {
                    item.TimeOne = "";
                    item.TimeTwo = "";
                    item.TimeThree = "";
                    bll.Update(item);
                }
                resp.Status = -1;
                resp.Msg = "查询次数已清空！！";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条记录进行操作！";
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}