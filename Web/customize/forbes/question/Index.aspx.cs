using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    public partial class Index : ForbesQuestionBase
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
           


            int count = bll.GetCount<ForbesQuestionPersonal>(string.Format("UserId='{0}'",CurrentUserInfo.UserID));
            if (count==0)//用户第一次进来 给用户分配题目
            {
                List<ForbesQuestion> questionList = bll.GetList<ForbesQuestion>();
                var result = GenerateNumber();
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {
                    List<int> questionIdFirst = new List<int>();//第一道题目编号
                    for (int i = 0; i < result.Length; i++)
                    {

                        ForbesQuestionPersonal model = new ForbesQuestionPersonal();
                        model.Count = 1;
                        model.QuestionId = questionList[result[i]].AutoID;
                        model.UserId = CurrentUserInfo.UserID;
                        questionIdFirst.Add(model.QuestionId);
                        if (!bll.Add(model, tran))
                        {
                            tran.Rollback();
                            Response.Write("生成第一道题目失败");
                            Response.End();
                        }
                    }

                    string questionIdFirstStr = "";
                    foreach (var item in questionIdFirst)
                    {
                        questionIdFirstStr += item.ToString()+",";
                    }
                    questionIdFirstStr = questionIdFirstStr.TrimEnd(',');


                    List<ForbesQuestion> questionSecond = bll.GetList<ForbesQuestion>(string.Format(" AutoID not in ({0})", questionIdFirstStr));

                    foreach (var item in questionSecond)
                    {
                        ForbesQuestionPersonal model = new ForbesQuestionPersonal();
                        model.Count =2;
                        model.QuestionId = item.AutoID;
                        model.UserId = CurrentUserInfo.UserID;
                        if (!bll.Add(model, tran))
                        {
                            tran.Rollback();
                            Response.Write("生成第二道题目失败");
                            Response.End();
                        }

                    }
                    tran.Commit();

                    


                   
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Response.Write("生成题目失败");
                    Response.End();
                    

                }





              


                
            }

        }

        public int[] GenerateNumber()
        {
            //用于存放1到33这33个数     
            int[] container = new int[60];
            //用于保存返回结果     
            int[] result = new int[30];
            Random random = new Random();
            for (int i = 1; i <60; i++)
            {
                container[i - 1] = i;
            }
            int index = 0;
            int value = 0;
            for (int i = 0; i <30; i++)
            {
                //从[1,container.Count + 1)中取一个随机值，保证这个值不会超过container的元素个数     
                index = random.Next(1, container.Length - 1 - i);
                //以随机生成的值作为索引取container中的值     
                value = container[index];
                //将随机取得值的放到结果集合中     
                result[i] = value;
                //将刚刚使用到的从容器集合中移到末尾去     
                container[index] = container[container.Length - i - 1];
                //将队列对应的值移到队列中     
                container[container.Length - i - 1] = value;
            }
            //result.Sort();排序     
            return result;  
        } 

    }
}