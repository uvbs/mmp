using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Questionnaire
{
    public partial class QuestionnaireEdit : System.Web.UI.Page
    {
        ZentCloud.BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public ZentCloud.BLLJIMP.Model.Questionnaire model = new BLLJIMP.Model.Questionnaire();
        public bool haseRecord = false;
        protected string type;
        protected string typeName;
        protected void Page_Load(object sender, EventArgs e)
        {
            type = string.IsNullOrWhiteSpace(Request["type"]) ? "0" : Request["type"];
            typeName = type == "0" ? "题库" : "问卷";
            model = bll.Get<ZentCloud.BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}",Request["id"]));
            ZentCloud.BLLJIMP.Model.QuestionnaireRecord oldRecord = bll.GetByKey<ZentCloud.BLLJIMP.Model.QuestionnaireRecord>("QuestionnaireID", Request["id"]);
            if (oldRecord != null)
            {
                model.Questions = new List<BLLJIMP.Model.Question>();
                haseRecord = true; 
                return;
            }
            model.Questions = bll.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", Request["id"]), "Sort Asc");
            List<ZentCloud.BLLJIMP.Model.Answer> modelAnswerList = bll.GetList<ZentCloud.BLLJIMP.Model.Answer>(string.Format("QuestionnaireID={0}", Request["id"]));
            for (int i = 0; i < model.Questions.Count; i++)
            {
                model.Questions[i].Answers = modelAnswerList.Where(p => p.QuestionID == model.Questions[i].QuestionID).ToList();
            }



        }
    }
}