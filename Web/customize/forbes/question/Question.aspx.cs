using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    public partial class Question : ForbesQuestionBase
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
          

            List<ForbesQuestion> questionList = bll.GetList<ForbesQuestion>();


        }
    }
}