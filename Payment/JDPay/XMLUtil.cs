using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Payment.JDPay.Model;

namespace Payment.JDPay
{
    public class XMLUtil
    {
        public static String encryptReqXml(String rsaPrivateKey, String strDesKey, SortedDictionary<String, String> dic)
        {
            XmlDocument xmldoc = sortedDictionary2AllXml(dic);
            String smlStr = ConvertXmlToString(xmldoc);
            
            String sha256SourceSignString = SHAUtil.encryptSHA256(smlStr);
            System.Diagnostics.Debug.WriteLine("xml摘要:" + sha256SourceSignString);
            byte[] encyptBytes = RSACoder.encryptByPrivateKey(sha256SourceSignString, rsaPrivateKey);
            String sign = Convert.ToBase64String(encyptBytes, Base64FormattingOptions.InsertLineBreaks);
            String data = smlStr.Replace("</jdpay>", "<sign>" + sign + "</sign></jdpay>");
            System.Diagnostics.Debug.WriteLine("封装后:" + data);
            byte[] key = Convert.FromBase64String(strDesKey);
            String encrypt = Des3.Des3EncryptECB(key, data);
            System.Diagnostics.Debug.WriteLine("3DES后:" + encrypt);
            encrypt = Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypt));
            System.Diagnostics.Debug.WriteLine("base64后:" + encrypt);
            SortedDictionary<String, String> reqdic = new SortedDictionary<string, string>();
            reqdic.AddOrReplace("version", dic.getVaule("version"));
            reqdic.AddOrReplace("merchant", dic.getVaule("merchant"));
            reqdic.AddOrReplace("encrypt", encrypt);

            XmlDocument reqXml = new XmlDocument();
            sortedDictionary2Xml(reqXml, reqdic);
            String reqXmlStr = ConvertXmlToString(reqXml);
            System.Diagnostics.Debug.WriteLine("请求xml:" + reqXmlStr);
            return reqXmlStr;
        }


        public static T decryptResXml<T>(String rsaPubKey, String strDesKey, String xmlResp)
        {
            Type type = typeof(T);
            JdPayResponse jdPayRes = Deserialize<JdPayResponse>(typeof(JdPayResponse),xmlResp);
            object entity = type.Assembly.CreateInstance(type.FullName);

            String encryptStr = jdPayRes.encrypt;
            if (!"".Equals(encryptStr))
            {
                byte[] key = Convert.FromBase64String(strDesKey);
                String base64EncryptStr = Encoding.UTF8.GetString(Convert.FromBase64String(encryptStr));
                String reqBody = Des3.Des3DecryptECB(key, base64EncryptStr);
                //System.Diagnostics.Debug.WriteLine("请求返回encrypt Des解密后:" + reqBody);

                XmlDocument reqBodyDoc = new XmlDocument();
                reqBodyDoc.LoadXml(reqBody);
                
                String inputSign = getValue(reqBodyDoc, "sign");
                //System.Diagnostics.Debug.WriteLine("请求返回sign:" + inputSign);
                XmlNode jdpayRoot = reqBodyDoc.SelectSingleNode("jdpay");
                XmlNode signNode = jdpayRoot.SelectSingleNode("sign");
                jdpayRoot.RemoveChild(signNode);

                //XmlNodeList nodelist = jdpayRoot.ChildNodes;

                String reqBodyStr = ConvertXmlToString(reqBodyDoc);
                String xmlh = xmlResp.Substring(0, xmlResp.IndexOf("<jdpay>"));
                if(xmlh!=null && !"".Equals(xmlh))
                {
                    reqBodyStr = reqBodyStr.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", xmlh);
                }
                
                //System.Diagnostics.Debug.WriteLine("本地摘要原串:" + reqBodyStr);
                String sha256SourceSignString = SHAUtil.encryptSHA256(reqBodyStr);
                //System.Diagnostics.Debug.WriteLine("本地摘要:" + sha256SourceSignString);


                //var res = RSACoder.DecryptByPublibKey(inputSign, rsaPubKey);




                byte[] decryptByte = RSACoder.decryptByPublicKey(inputSign, rsaPubKey);
                String decryptStr = Des3.bytesToString(decryptByte);



                //System.Diagnostics.Debug.WriteLine("解密后摘要:" + decryptStr);
                if (sha256SourceSignString.Equals(decryptStr))
                {
                    //System.Diagnostics.Debug.WriteLine("验签通过");
                    entity = Deserialize<T>(type, reqBody);
                }
                else
                {
                     
                    //System.Diagnostics.Debug.WriteLine("验签失败");
                    //throw new Exception("验签失败");
                }
       
            }
            setProperValue(entity, "version",jdPayRes.version);
            setProperValue(entity, "merchant", jdPayRes.merchant);
            setProperValue(entity, "result", jdPayRes.result);
           
            return (T)entity;
        }

        public static XmlDocument sortedDictionary2AllXml(SortedDictionary<String, String> dic)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmldecl);
            sortedDictionary2Xml(xmldoc,dic);
            System.Diagnostics.Debug.WriteLine("xml:" + ConvertXmlToString(xmldoc));
            return xmldoc;
        }

        private static void sortedDictionary2Xml(XmlDocument xmldoc, SortedDictionary<String, String> dic)
        {
            XmlElement xmlelem = xmldoc.CreateElement("", "jdpay", "");
            xmldoc.AppendChild(xmlelem);
            foreach (KeyValuePair<string, string> kv in dic)
            {
                XmlElement xe = xmldoc.CreateElement(kv.Key);
                xe.InnerText = kv.Value;
                xmlelem.AppendChild(xe);
            }
        }

        /// <summary>  
        /// 将XmlDocument转化为string  
        /// </summary>  
        /// <param name="xmlDoc"></param>  
        /// <returns></returns>  
        public static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
            writer.Formatting = System.Xml.Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            xmlString = xmlString.Replace("\r", "");
            xmlString = xmlString.Replace("\n", "");
            xmlString = xmlString.Replace("\t", "");
            xmlString = Regex.Replace(xmlString, @">\s+<", "><");
            xmlString = Regex.Replace(xmlString, @"\s+/>", "/>");
            xmlString = xmlString.Replace("encoding=\"utf-8\"", "encoding=\"UTF-8\"");
            return xmlString;
        }

        private static String getValue(XmlDocument doc,String name)
        {
            XmlNodeList nodeList = doc.GetElementsByTagName(name);
            if(nodeList!=null && nodeList.Count > 0)
            {
                return nodeList[0].InnerText;
            }
            return "";
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(Type type, string xml)
        {
            //JsonConvert.DeserializeObjectAsync
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR "+e.StackTrace);
                return default(T);
            }
        }

        private static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        private static void setProperValue(object entity, String field, Object value)
        {
            Type t = entity.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (field.Equals(prop.Name))
                {
                    prop.SetValue(entity, value, null);
                }
            }

        }
    }
}