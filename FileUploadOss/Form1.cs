using AliOss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZentCloud.Common;
using ZentCloud.ZCDALEngine;

namespace FileUploadOss
{
    public partial class Form1 : Form
    {
        #region 变量
        private static int total = 0;
        private static string maxid = "0";
        private static int now = 0;
        private const string baseDir = "oldsite";

        private static string TopNum = "500";
        private static string WebsiteOwner = "";
        private static string txt1 = "";
        private static string txt2 = "";
        private static string txt3 = "";
        private static string txt4 = "";
        private static string txt5 = ConfigHelper.GetConfigString("WebDir");

        private static string[] txtLikeWebDomain = new string[]{ ".comeoncloud.net/"};
        
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        #region 上传并记录
        private void ToOss2(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string oldFile = dr[txt2].ToString();
                    if (string.IsNullOrWhiteSpace(oldFile))continue; 
                    string oldFile1 = "";

                    if (oldFile.StartsWith(OssHelper.GetDomain()))
                    {
                        SetTextErrorMessage("已经在Oss服务器：" + oldFile);
                        continue;
                    }
                    else if (oldFile.StartsWith("/"))
                    {
                        oldFile1 = oldFile;
                    }
                    else
                    {
                        bool haveLocalSrc = false;
                        string nowWebDomain = "";
                        for (int i = 0; i < txtLikeWebDomain.Length; i++)
                        {
                            if (oldFile.IndexOf(txtLikeWebDomain[i]) > 0)
                            {
                                haveLocalSrc = true;
                                nowWebDomain = txtLikeWebDomain[i];
                                break;
                            }
                        }
                        if (!haveLocalSrc)
                        {
                            SetTextErrorMessage("不是本站图片：" + oldFile);
                            continue;
                        }

                        oldFile1 = oldFile.Substring(oldFile.IndexOf(nowWebDomain) + nowWebDomain.Length - 1);
                    }
                    SetTextErrorMessage(oldFile1);
                    string url = "";
                    DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile1));
                    if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                    {
                        url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                    }
                    if (string.IsNullOrWhiteSpace(url))
                    { 
                        ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile));
                        if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                        }
                    }
                    if(string.IsNullOrWhiteSpace(url))
                    {
                        string LocalPath = txt5 + oldFile1.Replace("/", @"\");
                        string extension = Path.GetExtension(oldFile1).ToLower();
                        if (!File.Exists(LocalPath))
                        {
                            SetTextErrorMessage("文件不存在：" + LocalPath);
                            continue;
                        }
                        byte[] fileByte = File.ReadAllBytes(LocalPath);
                        if (fileByte.Length == 0)
                        {
                            SetTextErrorMessage("文件大小为空：" + LocalPath);
                            continue;
                        }
                        url = OssHelper.UploadFileFromByte(OssHelper.GetBucket(""), baseDir + oldFile1, fileByte, extension);
                    }

                    DataSet ds1 = DbHelperSQL.Query(
                        string.Format("SELECT 1 FROM {0} WHERE [TableKeyID]='{1}' AND [TableName]='{2}' AND [FieldName]='{3}' AND [OldPath]='{4}'"
                        , "FileToOssLog", dr[txt4].ToString(), txt1, txt2, oldFile));

                    if (ds1 != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count == 0)
                    {
                        sb.AppendFormat("INSERT INTO {0} ([TableKeyID],[TableName],[FieldName],[OldPath],[NewPath]) ", "FileToOssLog");
                        sb.AppendFormat("VALUES ('{0}','{1}','{2}','{3}','{4}') ;", dr[txt4].ToString(), txt1, txt2, oldFile, url);
                    }
                }
                catch (Exception ex)
                {
                    SetTextErrorMessage(ex.Message);
                }
                finally
                {
                    now++;
                    SetTextMessage();
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                    DbHelperSQL.ExecuteSql(sb.ToString());
            }
            catch (Exception ex)
            {
                SetTextErrorMessage(ex.Message);
            }
        }

        private void ToOss3(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string htmlField = dr[txt2].ToString();
                    if (string.IsNullOrWhiteSpace(htmlField)) continue;
                    List<string> srcList = MyRegex.GetPadImg(htmlField);
                    if (srcList.Count == 0)
                    {
                        continue;
                    }

                    foreach (var item in srcList)
                    {
                        string oldFile = MyRegex.GetPadSrcUrl(item);
                        string oldFile1 = "";
                        if (oldFile.StartsWith(OssHelper.GetDomain()))
                        {
                            SetTextErrorMessage("已经在Oss服务器：" + oldFile);
                            continue;
                        }
                        else if (oldFile.StartsWith("/"))
                        {
                            oldFile1 = oldFile;
                        }
                        else{
                            bool haveLocalSrc = false;
                            string nowWebDomain = "";
                            for (int i = 0; i < txtLikeWebDomain.Length; i++)
                            {
                                if (oldFile.IndexOf(txtLikeWebDomain[i]) > 0)
                                {
                                    haveLocalSrc = true;
                                    nowWebDomain = txtLikeWebDomain[i];
                                    break;
                                }
                            }
                            if (!haveLocalSrc)
                            {
                                SetTextErrorMessage("不是本站图片：" + oldFile);
                                continue; 
                            }
                            oldFile1 = oldFile.Substring(oldFile.IndexOf(nowWebDomain) + nowWebDomain.Length - 1);
                        }
                        SetTextErrorMessage(oldFile1);
                        string url = "";
                        DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile1));
                        if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                        {
                            url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                        }
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile));
                            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                            {
                                url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                            }
                        }
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            string LocalPath = txt5 + oldFile1.Replace("/", @"\");
                            string extension = Path.GetExtension(oldFile1).ToLower();
                            if (!File.Exists(LocalPath))
                            {
                                SetTextErrorMessage("文件不存在：" + LocalPath);
                                continue;
                            }
                            byte[] fileByte = File.ReadAllBytes(LocalPath);
                            if (fileByte.Length == 0)
                            {
                                SetTextErrorMessage("文件大小为空：" + LocalPath);
                                continue; 
                            }
                            url = OssHelper.UploadFileFromByte(OssHelper.GetBucket(""), baseDir + oldFile1, fileByte, extension);
                        }

                        DataSet ds1 = DbHelperSQL.Query(
                            string.Format("SELECT 1 FROM {0} WHERE [TableKeyID]='{1}' AND [TableName]='{2}' AND [FieldName]='{3}' AND [OldPath]='{4}'"
                            , "FileToOssLog", dr[txt4].ToString(), txt1, txt2, oldFile));

                        if (ds1 != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count == 0)
                        {
                            sb.AppendFormat("INSERT INTO {0} ([TableKeyID],[TableName],[FieldName],[OldPath],[NewPath]) ", "FileToOssLog");
                            sb.AppendFormat("VALUES ('{0}','{1}','{2}','{3}','{4}') ;", dr[txt4].ToString(), txt1, txt2, oldFile, url);
                        }
                    }

                }
                catch (Exception ex)
                {
                    SetTextErrorMessage(ex.Message);
                }
                finally
                {
                    now++;
                    SetTextMessage();
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                    DbHelperSQL.ExecuteSql(sb.ToString());
            }
            catch (Exception ex)
            {
                SetTextErrorMessage(ex.Message);
            }
        }
        private void ToOss4(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string[] txt2s = txt2.Split(',');
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    foreach (var txt2item in txt2s)
                    {
                        string htmlField = dr[txt2item].ToString();
                        if (string.IsNullOrWhiteSpace(htmlField)) continue;
                        List<string> srcList = htmlField.Split(',').ToList();
                        if (srcList.Count == 0)
                        {
                            continue;
                        }

                        foreach (var item in srcList)
                        {
                            if (string.IsNullOrWhiteSpace(htmlField)) continue;
                            string oldFile = item;
                            string oldFile1 = "";
                            if (oldFile.StartsWith(OssHelper.GetDomain()))
                            {
                                SetTextErrorMessage("已经在Oss服务器：" + oldFile);
                                continue;
                            }
                            else if (oldFile.StartsWith("/"))
                            {
                                oldFile1 = oldFile;
                            }
                            else
                            {
                                bool haveLocalSrc = false;
                                string nowWebDomain = "";
                                for (int i = 0; i < txtLikeWebDomain.Length; i++)
                                {
                                    if (oldFile.IndexOf(txtLikeWebDomain[i]) > 0)
                                    {
                                        haveLocalSrc = true;
                                        nowWebDomain = txtLikeWebDomain[i];
                                        break;
                                    }
                                }
                                if (!haveLocalSrc)
                                {
                                    SetTextErrorMessage("不是本站图片：" + oldFile);
                                    continue;
                                }
                                oldFile1 = oldFile.Substring(oldFile.IndexOf(nowWebDomain) + nowWebDomain.Length - 1);
                            }
                            SetTextErrorMessage(oldFile1);
                            string url = "";
                            DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile1));
                            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                            {
                                url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                            }
                            if (string.IsNullOrWhiteSpace(url))
                            {
                                ds = DbHelperSQL.Query(string.Format("SELECT TOP 1 OldPath,NewPath FROM {0} WHERE OldPath='{1}' ", "FileToOssLog", oldFile));
                                if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count == 1)
                                {
                                    url = ds.Tables[0].Rows[0]["NewPath"].ToString();
                                }
                            }
                            if (string.IsNullOrWhiteSpace(url))
                            {
                                string LocalPath = txt5 + oldFile1.Replace("/", @"\");
                                string extension = Path.GetExtension(oldFile1).ToLower();
                                if (!File.Exists(LocalPath))
                                {
                                    SetTextErrorMessage("文件不存在：" + LocalPath);
                                    continue;
                                }
                                byte[] fileByte = File.ReadAllBytes(LocalPath);
                                if (fileByte.Length == 0)
                                {
                                    SetTextErrorMessage("文件大小为空：" + LocalPath);
                                    continue;
                                }
                                url = OssHelper.UploadFileFromByte(OssHelper.GetBucket(""), baseDir + oldFile1, fileByte, extension);
                            }

                            DataSet ds1 = DbHelperSQL.Query(
                                string.Format("SELECT 1 FROM {0} WHERE [TableKeyID]='{1}' AND [TableName]='{2}' AND [FieldName]='{3}' AND [OldPath]='{4}'"
                                , "FileToOssLog", dr[txt4].ToString(), txt1, txt2item, oldFile));

                            if (ds1 != null && ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count == 0)
                            {
                                sb.AppendFormat("INSERT INTO {0} ([TableKeyID],[TableName],[FieldName],[OldPath],[NewPath]) ", "FileToOssLog");
                                sb.AppendFormat("VALUES ('{0}','{1}','{2}','{3}','{4}') ;", dr[txt4].ToString(), txt1, txt2item, oldFile, url);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    SetTextErrorMessage(ex.Message);
                }
                finally
                {
                    now++;
                    SetTextMessage();
                }
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                    DbHelperSQL.ExecuteSql(sb.ToString());
            }
            catch (Exception ex)
            {
                SetTextErrorMessage(ex.Message);
            }
        }
        #endregion

        #region 委托 更新进度 输出错误消息
        private delegate void SetPos();
        private void SetTextMessage()
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMessage);
                this.Invoke(setpos, new object[] { });
            }
            else
            {
                this.label3.Text = "进度：" + now.ToString() + "/" + total.ToString();
                if (now > this.progressBar1.Maximum) now = this.progressBar1.Maximum;
                    this.progressBar1.Value = now;
            }
        }

        private delegate void SetTextError(string log);
        private void SetTextErrorMessage(string log)
        {
            if (this.InvokeRequired)
            {
                SetTextError writelog = new SetTextError(SetTextErrorMessage);
                this.Invoke(writelog, new object[] { log });
            }
            else
            {
                textBox6.Text = log+"\r\n"+ textBox6.Text;
            }
        }
        #endregion

        #region 文章缩略图
        //文章缩略图
        private void button1_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_JuActivityInfo";
            txt2 = "ThumbnailsPath";
            txt3 = "0";
            txt4 = "JuActivityID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}'", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND WebsiteOwner='{4}' ORDER BY {0}", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss2(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion
        #region 商城缩略图
        //商城缩略图
        private void button3_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_WXMallProductInfo";
            txt2 = "RecommendImg";
            txt3 = "0";
            txt4 = "PID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}'", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND WebsiteOwner='{4}' ORDER BY {0}", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss2(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion
        #region 用户头像缩略图
        //用户头像缩略图
        private void button2_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_UserInfo";
            txt2 = "WXHeadimgurl";
            txt3 = "0";
            txt4 = "AutoID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}' ", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND WebsiteOwner='{4}' ORDER BY {0} ", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss2(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion
        #region 文章内容中的图片
        /// <summary>
        /// 文章内容中的图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_JuActivityInfo";
            txt2 = "ActivityDescription";
            txt3 = "0";
            txt4 = "JuActivityID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}' ", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND  WebsiteOwner='{4}' ORDER BY {0} ", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss3(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion
        #region 商城描述
        /// <summary>
        /// 商城描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_WXMallProductInfo";
            txt2 = "PDescription";
            txt3 = "0";
            txt4 = "PID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}'", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND WebsiteOwner='{4}' ORDER BY {0} ", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss3(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion

        #region 商城ShowImage
        private void button4_Click(object sender, EventArgs e)
        {
            txt1 = "ZCJ_WXMallProductInfo";
            txt2 = "ShowImage,ShowImage1,ShowImage2,ShowImage3,ShowImage4,ShowImage5";
            txt3 = "0";
            txt4 = "PID";
            WebsiteOwner = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(WebsiteOwner))
            {
                MessageBox.Show("请设置WebsiteOwner");
                return;
            }
            now = 0;
            total = Convert.ToInt32(DbHelperSQL.GetSingle(string.Format("SELECT COUNT(*) FROM {0} WHERE WebsiteOwner='{1}'", txt1, WebsiteOwner)));
            this.progressBar1.Maximum = total;
            maxid = txt3;
            SetTextMessage();

            bool MoreData = true;
            while (MoreData)
            {
                DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP {5} {0},{1} FROM {2} WHERE CONVERT(INT,{0})>{3} AND WebsiteOwner='{4}' ORDER BY {0} ", txt4, txt2, txt1, maxid, WebsiteOwner, TopNum));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MoreData = false;
                    break;
                }
                maxid = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][txt4].ToString();
                Thread th1 = new Thread(delegate()
                {
                    ToOss4(ds.Tables[0]);
                });
                th1.Start();
            }
        }
        #endregion
    }
}
