using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP;

namespace MQConsumerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BLLMQ bllMQ = new BLLMQ();

            bllMQ.Subscribe();
        }
    }
}
