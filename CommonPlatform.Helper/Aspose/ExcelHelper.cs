using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aspose.Cells;
using System.IO;
using System.Data;

namespace CommonPlatform.Helper.Aspose
{
    public class ExcelHelper
    {
        public static MemoryStream OutModelFileToStream(string templateFileName, Dictionary<String,Object> dictSource, DataTable dt = null)
        {
            Crack();//调用Hot Patch
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(templateFileName);
            if (dt != null) designer.SetDataSource(dt);
            if (dictSource != null && dictSource.Count > 0)
            {
                foreach (var keyValuePair in dictSource)
                {
                    designer.SetDataSource(keyValuePair.Key, keyValuePair.Value);
                }
            }  
            designer.Process();
            return designer.Workbook.SaveToStream();
        }
        private static void Crack()//使用前调用一次即可
        {
            string[] stModule = new string[8]
            {
                "\u000E\u2008\u2001\u2000",
                "\u000F\u2008\u2001\u2000",
                "\u0002\u2008\u200B\u2001",
                "\u000F",
                "\u0006",
                "\u000E",
                "\u0003",
                "\u0002"
            };
            Assembly assembly = Assembly.GetAssembly(typeof(License));


            Type typeLic = null, typeIsTrial = null, typeHelper = null;

            foreach (Type type in assembly.GetTypes())
            {
                if ((typeLic == null) && (type.Name == stModule[0]))
                {
                    typeLic = type;
                }
                else if ((typeIsTrial == null) && (type.Name == stModule[1]))
                {
                    typeIsTrial = type;
                }
                else if ((typeHelper == null) && (type.Name == stModule[2]))
                {
                    typeHelper = type;
                }
            }
            if (typeLic == null || typeIsTrial == null || typeHelper == null)
            {
                throw new Exception();
            }
            object lic = Activator.CreateInstance(typeLic);
            int findCount = 0;

            foreach (FieldInfo field in typeLic.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (field.FieldType == typeLic && field.Name == stModule[3])
                {
                    field.SetValue(null, lic);
                    ++findCount;
                }
                else if (field.FieldType == typeof(DateTime) && field.Name == stModule[4])
                {
                    field.SetValue(lic, DateTime.MaxValue);
                    ++findCount;
                }
                else if (field.FieldType == typeIsTrial && field.Name == stModule[5])
                {
                    field.SetValue(lic, 1);
                    ++findCount;
                }

            }
            foreach (FieldInfo field in typeHelper.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(bool) && field.Name == stModule[6])
                {
                    field.SetValue(null, false);
                    ++findCount;
                }
                if (field.FieldType == typeof(int) && field.Name == stModule[7])
                {
                    field.SetValue(null, 128);
                    ++findCount;
                }
            }
            if (findCount < 5)
            {
                throw new NotSupportedException("无效的版本");
            }
        }
    }
}
