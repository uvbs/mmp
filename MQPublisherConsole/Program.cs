using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP;

namespace MQPublisherConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = "";
            Console.WriteLine("Enter a message. 'q' to quit.");

            while ((input = Console.ReadLine()) != "q")
            {
                if (input == "csh")
                {
                    new BLLDistribution().InitUserDistributionMember();
                }
                else
                {
                    new BLLMQ().Publish(new ZentCloud.BLLJIMP.Model.MQ.MessageInfo()
                    {
                        MsgType = "test",
                        Msg = input
                    });
                }

                
            }

        }
    }
}
