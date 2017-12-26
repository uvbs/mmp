using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class HelpCategory : ZCBLLEngine.ModelTable
    {

        public HelpCategory()
        { }

   
		#region Model
		private long _categoryid;
		private string _nodename;
		private string _url;
		private long _preid;
		private string _icocss;
		private int _sort;
		private int? _ishide;
		/// <summary>
		/// 
		/// </summary>
		public long CategoryID
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NodeName
		{
			set{ _nodename=value;}
			get{return _nodename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long PreID
		{
			set{ _preid=value;}
			get{return _preid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ICOCSS
		{
			set{ _icocss=value;}
			get{return _icocss;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsHide
		{
			set{ _ishide=value;}
			get{return _ishide;}
		}
		#endregion Model
    }
}
