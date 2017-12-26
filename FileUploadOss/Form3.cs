using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZentCloud.BLLJIMP;
using ZentCloud.ZCBLLEngine;

namespace FileUploadOss
{
    public partial class Form3 : Form
    {
        ZentCloud.BLLJIMP.BLLComponent bll = new ZentCloud.BLLJIMP.BLLComponent();
        ZentCloud.BLLJIMP.BLLSlide bllSlide = new ZentCloud.BLLJIMP.BLLSlide();
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ZentCloud.BLLJIMP.Model.Component> list = bll.GetColList<ZentCloud.BLLJIMP.Model.Component>(int.MaxValue, 1, "ComponentModelId>=10", "WebsiteOwner,AutoId desc", "AutoId,WebsiteOwner,ComponentConfig");
            foreach (ZentCloud.BLLJIMP.Model.Component item in list)
            {
                UpdateConfig(item);
            }
        }


        private void UpdateConfig(ZentCloud.BLLJIMP.Model.Component item)
        {
            //item.ComponentConfig = "{\"pageinfo\":{\"title\":\"MixBlu\",\"bg_img\":\"\",\"bg_color\":\"#FFFFFF\"},\"shareinfo\":{\"title\":\"首页\",\"desc\":\"首页——MixBlu服装\",\"img_url\":\"http://open-files.comeoncloud.net/www/xikaiye/jubit/image/20160706/A30528B3DA394312B8E8A4DAE8C3D797.png\",\"link\":\"\"},\"head_bar\":{\"show\":\"1\",\"title\":\"首页\",\"left_btn\":\"javascript:history.go(-1);\",\"right_btn\":\"http://m.mixblu.com/customize/mixblu/index.aspx?ngroute=/shoppingBasket#/shoppingBasket\",\"right_btn_ico\":\"iconfont icon-shangchengicon25\",\"left_btn_ico\":\"iconfont icon-shangchengicon26\",\"sidemenu_button\":\"1\",\"left_btn_size\":\"40\",\"right_btn_num\":\"1\",\"right_btn_size\":\"23\",\"color\":\"#FFFFFF\",\"bg_color\":\"#0A0A0A\",\"bd_color\":\"#0A0A0A\"},\"totop\":{\"style\":\"1\"},\"sidemenubox\":{\"show\":\"0\",\"type\":\"4\",\"data_key\":\"商城左侧导航\",\"head_bgcolor\":\"#6BD3FF\",\"item_bgcolor\":\"#F563FF\",\"style\":\"2\",\"search_url\":\"http://m.mixblu.com/customize/mixblu/index.aspx?ngroute=/productList///{search_key}///time_desc/#/productList///{search_key}///time_desc/\",\"search_placeholder\":\"输入货号或关键词,如:连衣裙\"},\"notice_1\":[{\"text\":\"7.18秋款上新 全场满减\",\"color\":\"#FFFFFF\",\"bg_color\":\"#FF5C48\",\"link\":\"\",\"line_height\":\"28\",\"mg_top\":\"0\",\"mg_bottom\":\"0\",\"font_size\":\"12\",\"letter_spacing\":\"2\",\"scroll_amount\":\"20\"}],\"slides_1\":[{\"slide_list\":\"首页banner\",\"show_type\":\"3\",\"size_height\":\"0\",\"size_width\":\"0\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"0\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"slides_2\":[{\"slide_list\":\"首页满减\",\"show_type\":\"3\",\"size_height\":\"0\",\"size_width\":\"0\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FF5C48\",\"bd_width\":\"7\",\"mg_top\":\"5\",\"mg_bottom\":\"5\"}],\"slides_3\":[{\"slide_list\":\"包包\",\"show_type\":\"3\",\"size_height\":\"354\",\"size_width\":\"581\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"slides_4\":[{\"slide_list\":\"首页banner2\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"slides_5\":[{\"slide_list\":\"首页当季爆款\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"5\",\"mg_bottom\":\"0\"}],\"malls_1\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"sort_tag\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"\",\"tag\":\"hot\",\"style_child_1\":\"2\",\"sort_tag\":\"hot\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\"}],\"slides_6\":[{\"slide_list\":\"首页特卖爆款\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_2\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"sales_volume\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"sale\",\"tag\":\"sale\"}],\"slides_7\":[{\"slide_list\":\"首页分类-T恤\",\"show_type\":\"1\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_3\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"def\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"190\",\"tag\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"\"}],\"slides_8\":[{\"slide_list\":\"首页分类-连衣裙\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_4\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"def\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"192\",\"tag\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"\"}],\"slides_9\":[{\"slide_list\":\"首页分类-衬衫\",\"show_type\":\"1\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_5\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"def\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"191\",\"tag\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"\"}],\"slides_10\":[{\"slide_list\":\"首页分类-半裙\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_6\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"def\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"193\",\"tag\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"\"}],\"slides_11\":[{\"slide_list\":\"首页分类-裤装\",\"show_type\":\"2\",\"size_height\":\"300\",\"size_width\":\"640\",\"show_title\":\"0\",\"switch_class\":\"1\",\"bd_color\":\"#FFFFFF\",\"bd_width\":\"0\",\"mg_top\":\"0\",\"mg_bottom\":\"0\"}],\"malls_7\":[{\"count\":\"6\",\"style\":\"1\",\"is_group_buy\":\"0\",\"sort\":\"def\",\"auto_load\":\"0\",\"title_show\":\"0\",\"cate\":\"189\",\"tag\":\"\",\"style_child_1\":\"2\",\"link\":\"/customize/mixblu/index.aspx?ngroute=/productDetail/{id}#/productDetail/{id}\",\"top_sort\":\"\",\"sort_tag\":\"\"}],\"content_1\":[{\"thtml\":\"<div style=\\\"width:100%;height:100px;text-align:center;color:#646464;background:#010101;\\\">\\n\\t<div style=\\\"width:100%;height:45px;line-height:45px;\\\">\\n\\t\\t<a class=\\\"iconfont icon-xinlang\\\" style=\\\"margin:0 13px;color:#646464!important;text-decoration:none!important;font-size:24px;\\\" href=\\\"http://weibo.com/u/5592492372\\\"></a> <a class=\\\"iconfont icon-weixin\\\" style=\\\"margin:0 13px;color:#646464!important;text-decoration:none!important;font-size:24px;\\\" href=\\\"http://mp.weixin.qq.com/s?__biz=MjM5NzQ4MTc1Nw==&amp;mid=208770866&amp;idx=1&amp;sn=d38dddf4a7b9e17c8ca3222036740616&amp;scene=1&amp;srcid=GhQQlEdHwdr7fDrIan4d&amp;key=dffc561732c226510df4349b28f24ea5bc612bf796f35abf333275024093bed75b8e9f37347c5c79f57adbd6dea2c678&amp;ascene=0&amp;uin=MjE0NjUwNjE1&amp;devicetype=iMac+MacBookAir6%2C2+OSX+OSX+10.10.5+build(14F27)&amp;version=11020201&amp;pass_ticket=6XZxybJhAq%2F7BtoA3zNntViYiwqNLOP8WoUJ0cRbKeI%3D\\\"></a> <a class=\\\"iconfont icon-taobao\\\" style=\\\"margin:0 13px;color:#646464!important;text-decoration:none!important;font-size:24px;\\\" href=\\\"http://msbfs.m.tmall.com\\\"></a> \\n\\t</div>\\n\\t<div class=\\\"font14\\\">\\n\\t\\t&copy; 2015 Mixblu 沪ICP备14040036号 版权所有&nbsp;&nbsp;<a href=\\\"https://www.sgs.gov.cn/lz/licenseLink.do?method=licenceView&amp;entyId=dov73ne26zbqq0iswy128wh32k4nas6qky\\\"><img src=\\\"http://open-files.comeoncloud.net/www/mixblu/mixblu/image/20151117/2BBC9B9818764CE582FCF1F65DACE4F4.gif\\\" alt=\\\"\\\" style=\\\"width:14px;top:2px;position:relative;\\\" /></a><br />\\n<a href=\\\"#/articleList/572\\\" style=\\\"color:#646464;border-right:1px solid #646464;padding:0 8px;\\\">隐私政策</a> <a href=\\\"#/articleList/574\\\" style=\\\"color:#646464;border-right:1px solid #646464;padding:0 8px;\\\">买家条款与条件</a> <a href=\\\"#/articleList/573\\\" style=\\\"color:#646464;padding:0 8px;\\\">帮助中心</a> \\n\\t</div>\\n</div>\"}]}";
            if (string.IsNullOrWhiteSpace(item.ComponentConfig)) return;

            //加载分类数据和文章首页数据
            JObject jobject = JObject.Parse(item.ComponentConfig);
            //List<string> liT = new List<string>{"slide_list","slides","navs","tab_list","cards","malls","activitys"};
            Dictionary<string, int> dicT = new Dictionary<string, int>();
            dicT.Add("slides", 1);
            dicT.Add("navs", 1);
            dicT.Add("cards", 1);
            dicT.Add("malls", 1);
            dicT.Add("activitys", 1);

            //Dictionary<string, int> dicTn = new Dictionary<string, int>();
            //dicTn.Add("slides", "slides");
            //dicTn.Add("navs", "navs");
            //dicTn.Add("cards", "cards");
            //dicTn.Add("malls", "malls");
            //dicTn.Add("activitys", "activitys");

            List<string> liT = new List<string>{"slides","navs","cards","malls","activitys"};
            List<JProperty> listProperty = jobject.Properties().ToList();
            if (listProperty.Count == 0) return;

            JObject result = new JObject();
            bool haArr = false;
            foreach (JProperty pro in listProperty)
            {
                string pName = pro.Name;
                string lio = liT.FirstOrDefault(p => pName.StartsWith(p));
                if (jobject[pro.Name].Type == JTokenType.Array && !string.IsNullOrWhiteSpace(lio))
                {
                    haArr = true;
                    JArray oChildJArray = JArray.FromObject(pro.Value);

                    int n = dicT[lio];
                    if (oChildJArray.Count > 1)
                    {
                        foreach (JToken cio in oChildJArray)
                        {
                            JArray childJArray = new JArray();
                            childJArray.Add(cio);

                            result[lio + "_" + n] = childJArray;
                            n++;
                            dicT[lio] = n;
                        }
                    }
                    else
                    {
                        result[lio + "_" + n] = oChildJArray;
                        n++;
                        dicT[lio] = n;
                    }
                }
                else
                {
                    result[pro.Name] = pro.Value;
                }
            }
            item.ComponentConfig = JsonConvert.SerializeObject(result);
            if (haArr)
            {
                if (bll.UpdateByKey<ZentCloud.BLLJIMP.Model.Component>("AutoId", item.AutoId.ToString(), "ComponentConfig", item.ComponentConfig) > 0)
                {
                    SetTextErrorMessage(item.WebsiteOwner + ":" + item.AutoId);
                }
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
                textBox6.Text = log + "\r\n" + textBox6.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<ZentCloud.BLLJIMP.Model.Slide> list = bllSlide.GetColList<ZentCloud.BLLJIMP.Model.Slide>(int.MaxValue, 1, "[Width] is null or [Width]=0", "AutoID,ImageUrl,Width,Height");
            foreach (ZentCloud.BLLJIMP.Model.Slide item in list)
            {
                GetImgWidth(item);
            }
        }
        private void GetImgWidth(ZentCloud.BLLJIMP.Model.Slide item)
        {
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    string imgUrl = item.ImageUrl;
                    if (!imgUrl.ToLower().StartsWith("http")) imgUrl = "http://comeoncloud.comeoncloud.net" + imgUrl;
                    System.Uri uri = new Uri(imgUrl);
                    byte[] b = webClient.DownloadData(uri);
                    MemoryStream mes_keleyi_com = new MemoryStream(b);
                    Image image = Image.FromStream(mes_keleyi_com);
                    item.Width = image.Width;
                    item.Height = image.Height;

                    if (bll.Update(new ZentCloud.BLLJIMP.Model.Slide(),
                        string.Format("Width='{0}',Height='{1}'", item.Width, item.Height),
                        string.Format("AutoID={0}", item.AutoID)) > 0)
                    {
                        SetTextErrorMessage(item.Width + " " + item.Height + " " + item.AutoID);
                    }
                    else
                    {
                        SetTextErrorMessage("出错" + item.AutoID);
                    }
                }
            }
            catch (Exception ex)
            {
                SetTextErrorMessage("出错" + item.AutoID);
            }
        }
    }
}
