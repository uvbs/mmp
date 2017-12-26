using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class MeetingDetails : ZentCloud.ZCBLLEngine.ModelTable
    {
        private string _mobile;
        private string _name;
        private string _company;
        private string _title;

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public string Mobile
        {
            get {return _mobile;}
            set {_mobile = value;}
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email { get; set; }

        public string QQ { get; set; }
    }
}
