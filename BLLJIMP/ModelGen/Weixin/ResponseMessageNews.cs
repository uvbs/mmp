using System.Collections.Generic;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public class ResponseMessageNews : ResponseMessageBase, IResponseMessageBase
    {
        public int ArticleCount
        {
            get { return (Articles ?? new List<Article>()).Count; }
        }

        public List<Article> Articles { get; set; }

        public ResponseMessageNews()
        {
            Articles = new List<Article>();
        }
    }
}
