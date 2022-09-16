using System;
using System.Threading;
using System.Threading.Tasks;
using PEUtils;
namespace AOIServer
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            PELog.InitSettings();

            Task.Run(() =>
            {
                ServerRoot.Instance.Init();
                while (true)
                {
                    ServerRoot.Instance.Tick();
                    Thread.Sleep(10);
                }
            });

            while (true)
            {
                var str= Console.ReadLine();
                if (int.TryParse(str,out var num))
                {
                    for (int i = 0; i < num; i++)
                    {
                        ServerRoot.Instance.CreateRobotRole();
                    }
                }
            }

        }
    }
}
