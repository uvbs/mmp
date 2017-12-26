using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ZentCloud.BLLPermission;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.Util;
using System.Text;
using System.Drawing;


namespace ZentCloud.JubitIMP.Web
{


    public class DataLoadTool
    {
        //#if DEBUG  
        //        const string WEBSITEOWNER = "hf";
        //#endif
        /// <summary>
        /// 获取当前用户实体
        /// </summary>
        /// <returns></returns>
        public static ZentCloud.BLLJIMP.Model.UserInfo GetCurrUserModel()
        {
            //#if DEBUG

            //            return new BLLJIMP.Model.UserInfo()
            //            {
            //                AutoID = 262056,
            //                UserID = "jubit",
            //                UserType =1,
            //                TrueName = "杜鸿飞",
            //                Phone = "13636394008",
            //                WXAccessToken = "OezXcEiiBSKSxW0eoylIeK085OFFE57B9aMQr8tVwry3YSWHYeMCytIs8mSbQisb4VyqRhgRh1roLSk2xr7MCXUUaCqfamCQf4r5Sx3Nh9M-vX9FrJEApAJ74K3OEOxMnyLP2G6x0Ft7ddVsINurGQ",
            //                WXHeadimgurl = "http://wx.qlogo.cn/mmopen/0wRpPfN90ibChLwbS1tNZj6ib6EnTFEPA9X3b2GJ2iaIpcqNgJOzQW7m1Rb1zLP828aW2t64nVRxcYK6tCv577U3w/0",
            //                WXOpenId = "oTtgeuBURDQ3wr_1a4a7qTUj6ioc",
            //                WebsiteOwner = "hf",
            //                TotalScore=1000000


            //            };
            //#endif


            try
            {
                if (HttpContext.Current.Session[SessionKey.UserID] == null)
                    HttpContext.Current.Response.Redirect(Common.ConfigHelper.GetConfigString("logoutUrl"));

                ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL("");
                ZentCloud.BLLJIMP.Model.UserInfo userInfo = bll.GetCurrentUserInfo();
                return userInfo;
                //return bll.Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("UserID = '{0}'", HttpContext.Current.Session[SessionKey.UserID]));
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                throw new FormatException(ex.Message, ex);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw new Exception(ex.Message, ex);
            //}


        }

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrUserID()
        {
            try
            {
                return HttpContext.Current.Session[SessionKey.UserID].ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 检查当前用户权限
        /// </summary>
        /// <param name="pmsID"></param>
        /// <returns></returns>
        public static bool CheckCurrUserPms(long pmsID)
        {
            bool result = false;
            try
            {
                ZentCloud.BLLPermission.BLLMenuPermission bll = new ZentCloud.BLLPermission.BLLMenuPermission("");
                result = bll.CheckUserAndPms(GetCurrUserID(), pmsID);

            }
            catch { }

            return result;
        }

        ///// <summary>
        ///// 检查当前用户是否是鸿风管理员
        ///// </summary>
        ///// <returns></returns>
        //public static bool currIsHFAdmin()
        //{
        //    return DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Hongfeng_Admin);
        //}

        ///// <summary>
        ///// 检查当前用户是否是鸿风付费用户
        ///// </summary>
        ///// <returns></returns>
        //public static bool currIsHFVipUser()
        //{
        //    return DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Hongfeng_VIPUser);
        //}

        ///// <summary>
        ///// 检查当前用户是否是鸿风教师
        ///// </summary>
        ///// <returns></returns>
        //public static bool currIsHFTeacher()
        //{
        //    return DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Hongfeng_Teacher);
        //}


        //Pms_Hongfeng_VIPUser

        //public string CreateQrCode(string msg)
        //{
        //    QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();

        //    string imgName = Common.Rand.Str_char(7) + ".jpg";

        //    qrGenerator.CreateQrCode(msg, QRCoder.QRCodeGenerator.ECCLevel.Q).GetGraphic(20).Save(HttpContext.Current.Server.MapPath(@"\FileUpload\FShare\QRCode" + imgName));

        //    //imgTest.Src = "/FileUpload/" + imgName;

        //    return "/FileUpload/FShare/QRCode/" + imgName;
        //}

        /// <summary>
        /// 获取当前站点信息
        /// </summary>
        /// <returns></returns>
        public static BLLJIMP.Model.WebsiteInfo GetWebsiteInfoModel()
        {
            //#if DEBUG
            //            return new BLLJIMP.Model.WebsiteInfo()
            //            {
            //                WebsiteOwner = WEBSITEOWNER,
            //            };
            //#endif


            if (HttpContext.Current.Session["WebsiteInfoModel"] != null)
                return (BLLJIMP.Model.WebsiteInfo)HttpContext.Current.Session["WebsiteInfoModel"];
            return null;
        }

        //public static string GetJuActiviCateRName(int cateId)
        //{
        //    BLLJIMP.Model.WebsiteInfo currWebsiteInfo = GetWebsiteInfoModel();

        //    string result = "";

        //    switch (cateId)
        //    {
        //        case 11:
        //            //月度成果分享
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate1) ? "月度成果分享" : currWebsiteInfo.ArticleCate1;
        //            break;
        //        case 12:
        //            //老板点评分享
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate2) ? "老板点评分享" : currWebsiteInfo.ArticleCate2;
        //            break;
        //        case 13:
        //            //实用资源分享
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate3) ? "实用资源分享" : currWebsiteInfo.ArticleCate3;
        //            break;
        //        case 14:
        //            //成功案例分享
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate4) ? "成功案例分享" : currWebsiteInfo.ArticleCate4;
        //            break;
        //        case 15:
        //            //每周感悟分享
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.ArticleCate5) ? "每周感悟分享" : currWebsiteInfo.ArticleCate5;
        //            break;
        //        case 16:
        //            //精彩课程回放
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.CourseCate1) ? "精彩课程回放" : currWebsiteInfo.CourseCate1;
        //            break;
        //        case 17:
        //            //课程预告
        //            result = string.IsNullOrWhiteSpace(currWebsiteInfo.CourseCate2) ? "课程预告" : currWebsiteInfo.CourseCate2;
        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 将二进制流输出到浏览器
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        public static void RenderToBrowser(MemoryStream ms, string fileName)
        {
            if (HttpContext.Current.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        }
        /// <summary>
        /// Stream 转成
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;

        }
        /// <summary>
        /// 流转成文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public void StreamToFile(Stream stream, string fileName)
        {

            // 把 Stream 转换成 byte[] 

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 

            //FileStream fs = new FileStream(fileName, FileMode.Create);

            //BinaryWriter bw = new BinaryWriter(fs);

            //bw.Write(bytes);

            //bw.Close();

            //fs.Close();
            File.WriteAllBytes(fileName, bytes);

        }
        /// <summary>
        /// 将表格 生成二进制流
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                //using (IWorkbook workbook = new HSSFWorkbook())
                //{
                ISheet sheet = workbook.CreateSheet();
                //using (ISheet sheet = workbook.CreateSheet())
                //{
                IRow headerRow = sheet.CreateRow(0);

                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                
                ms.Flush();
                ms.Position = 0;
            }
            //}
            // }
            return ms;
        }

        /// <summary>
        /// DataTable导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportDataTable(DataTable dt, string fileName)
        {

            //MemoryStream ms = RenderToExcel(dt);
            //RenderToBrowser(ms, fileName);
            NPOIHelper.ExportByWeb(dt, "", fileName);

        }
        /// <summary>
        /// DataTable导出  多条sheet
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void ExportDataTable(DataTable[] dt, string fileName)
        {
            NPOIHelper.ExportExcel(dt, "", fileName);
        }
        /// <summary>
        /// 检查万邦是否登录 已登录返回true 未登录返回false
        /// </summary>
        /// <returns></returns>
        public static bool CheckWanBangLogin()
        {

            if (HttpContext.Current.Session[SessionKey.WanBangUserID] != null && HttpContext.Current.Session[SessionKey.WanBangUserType] != null)
            {
                if ((!string.IsNullOrEmpty(HttpContext.Current.Session[SessionKey.WanBangUserID].ToString())) && (!string.IsNullOrEmpty(HttpContext.Current.Session[SessionKey.WanBangUserType].ToString())))
                {

                    return true;
                }


            }

            return false;
        }




        public class NPOIHelper
        {
            /// <summary>
            /// DataTable导出到Excel文件
            /// </summary>
            /// <param name="dtSource">源DataTable</param>
            /// <param name="strHeaderText">表头文本</param>
            /// <param name="strFileName">保存位置</param>
            public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
            {
                using (MemoryStream ms = Export(dtSource, strHeaderText))
                {
                    using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }

            /// <summary>
            /// DataTable导出到Excel的MemoryStream
            /// </summary>
            /// <param name="dtSource">源DataTable</param>
            /// <param name="strHeaderText">表头文本</param>
            public static MemoryStream Export(DataTable dtSource, string strHeaderText)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

                #region 右击文件 属性信息
                {
                    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                    dsi.Company = "上海至云信息科技";
                    workbook.DocumentSummaryInformation = dsi;
                    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                    si.Author = "上海至云信息科技"; //填加xls文件作者信息
                    si.ApplicationName = "上海至云信息科技"; //填加xls文件创建程序信息
                    si.LastAuthor = "上海至云信息科技"; //填加xls文件最后保存者信息
                    si.Comments = "上海至云信息科技"; //填加xls文件作者信息
                    si.Title = "上海至云信息科技"; //填加xls文件标题信息
                    si.Subject = "上海至云信息科技";//填加文件主题信息
                    si.CreateDateTime = DateTime.Now;
                    workbook.SummaryInformation = si;
                }
                #endregion

                HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                dateStyle.WrapText = true;
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd HH:mm:ss");

                //取得列宽
                int[] arrColWidth = new int[dtSource.Columns.Count];
                foreach (DataColumn item in dtSource.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }
                int rowIndex = 0;
                foreach (DataRow row in dtSource.Rows)
                {
                    #region 新建表，填充表头，填充列头，样式
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }

                        #region 表头及样式
                        {
                            //HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            //headerRow.HeightInPoints = 25;
                            //headerRow.CreateCell(0).SetCellValue(strHeaderText);

                            //HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            //headStyle.Alignment = HorizontalAlignment.CENTER;
                            //HSSFFont font = workbook.CreateFont() as HSSFFont;
                            //font.FontHeightInPoints = 20;
                            //font.Boldweight = 700;
                            //headStyle.SetFont(font);
                            ////headerRow.GetCell(0).CellStyle = headStyle;
                            //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                            //headerRow.Dispose();
                        }
                        #endregion


                        #region 列头及样式
                        {
                            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                            HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                            headStyle.Alignment = HorizontalAlignment.LEFT;
                            HSSFFont font = workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            headStyle.SetFont(font);
                            foreach (DataColumn column in dtSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                                //设置列宽
                                try
                                {
                                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                                }
                                catch (Exception)
                                {
                                    sheet.SetColumnWidth(column.Ordinal, 20000);
                                    continue;
                                }

                            }
                            //headerRow.Dispose();
                        }
                        #endregion

                        rowIndex = 1;
                    }
                    #endregion


                    #region 填充内容
                    HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                        string drValue = row[column].ToString();

                        switch (column.DataType.ToString())
                        {
                            case "System.String"://字符串类型
                                newCell.SetCellValue(drValue);
                                newCell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.DateTime"://日期类型
                                if (drValue == "")
                                {
                                    newCell.SetCellValue("");
                                    break;
                                }
                                DateTime dateV;
                                DateTime.TryParse(drValue, out dateV);
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = dateStyle;//格式化显示
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(drValue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(drValue, out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(drValue, out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }

                    }
                    #endregion

                    rowIndex++;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;

                    // sheet.Dispose();
                    //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                    return ms;
                }
            }
            /// <summary>
            /// 导出EXCEL,可以导出多个sheet
            /// </summary>
            /// <param name="dtSources">原始数据数组类型</param>
            /// <param name="strFileName">路径</param>
            public static MemoryStream ExportEasy(DataTable[] dtSources)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                #region 右击文件 属性信息
                {
                    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                    dsi.Company = "上海至云信息科技";
                    workbook.DocumentSummaryInformation = dsi;
                    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                    si.Author = "上海至云信息科技"; //填加xls文件作者信息
                    si.ApplicationName = "上海至云信息科技"; //填加xls文件创建程序信息
                    si.LastAuthor = "上海至云信息科技"; //填加xls文件最后保存者信息
                    si.Comments = "上海至云信息科技"; //填加xls文件作者信息
                    si.Title = "上海至云信息科技"; //填加xls文件标题信息
                    si.Subject = "上海至云信息科技";//填加文件主题信息
                    si.CreateDateTime = DateTime.Now;
                    workbook.SummaryInformation = si;
                }
                #endregion

                for (int k = 0; k < dtSources.Length; k++)
                {
                    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(dtSources[k].TableName.ToString());

                    //填充表头
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
                    foreach (DataColumn column in dtSources[k].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    }

                    //填充内容
                    for (int i = 0; i < dtSources[k].Rows.Count; i++)
                    {
                        dataRow = (HSSFRow)sheet.CreateRow(i + 1);
                        for (int j = 0; j < dtSources[k].Columns.Count; j++)
                        {
                            dataRow.CreateCell(j).SetCellValue(dtSources[k].Rows[i][j].ToString());
                        }
                    }
                }

                //保存
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    // sheet.Dispose();
                    //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                    return ms;
                }
            }
            /// <summary>
            /// 用于Web导出
            /// </summary>
            /// <param name="dtSource">源DataTable</param>
            /// <param name="strHeaderText">表头文本</param>
            /// <param name="strFileName">文件名</param>
            public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
            {
                HttpContext curContext = HttpContext.Current;

                // 设置编码和附件格式
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = Encoding.UTF8;
                curContext.Response.Charset = "";
                curContext.Response.AddHeader("Content-Disposition", "attachment;fileName=" + strFileName);
                curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
                curContext.Response.End();
            }

            public static void ExportExcel(DataTable[] dtSource, string strHeaderText, string strFileName)
            {
                HttpContext curContext = HttpContext.Current;

                // 设置编码和附件格式
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = Encoding.UTF8;
                curContext.Response.Charset = "";
                curContext.Response.AddHeader("Content-Disposition", "attachment;fileName=" + strFileName);
                curContext.Response.BinaryWrite(ExportEasy(dtSource).GetBuffer());
                curContext.Response.End();
            }
            /// <summary>读取excel
            /// 默认第一行为标头
            /// </summary>
            /// <param name="strFileName">excel文档路径</param>
            /// <returns></returns>
            public static DataTable Import(string strFileName)
            {
                DataTable dt = new DataTable();

                HSSFWorkbook hssfworkbook;
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                NPOI.HSSF.UserModel.HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
                int cellCount = headerRow.LastCellNum;

                for (int j = 0; j < cellCount; j++)
                {
                    HSSFCell cell = headerRow.GetCell(j) as HSSFCell;
                    dt.Columns.Add(cell.ToString());
                }

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = sheet.GetRow(i) as HSSFRow;
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }

                    dt.Rows.Add(dataRow);
                }
                return dt;
            }

        }
    }
}
