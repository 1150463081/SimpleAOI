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
                    Thread.Sleep(100);
                }
            });

            while (true)
            {
                Console.ReadLine();
            }

        }
    }
}
