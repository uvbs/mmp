using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPlatform.Helper
{
    public class EnumDescriptionAttribute : Attribute
    {
        private string _text = "";
        public string Text
        {
            get { return this._text; }
        }
        public EnumDescriptionAttribute(string text)
        {
            _text = text;
        }

    }

    public class EnumStringHelper
    {
        public static string ToString(object o)
        {
            Type t = o.GetType();
            string s = o.ToString();
            EnumDescriptionAttribute[] os = (EnumDescriptionAttribute[])t.GetField(s).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (os != null && os.Length == 1)
            {
                return os[0].Text;
            }
            return s;
        }
    }
}
