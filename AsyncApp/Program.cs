using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace AsyncApp
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly int loopNum = 3;

        static void Main(string[] args)
        {
            log.InfoFormat("this is main thread");

            // --------------------------------------
            // Task（非同期） と Parallel（データ並列） の違いを検証

            LoopByFor();

            //LoopByParallelFor();

            // --------------------------------------

            // Taskはスレッド開放する（メインスレッドに処理を戻す）ので、こちらが先に実行される
            log.InfoFormat("Amazing !");

            // コンソールを終了させない
            Console.ReadLine();
        }

        // StartAsyncTaskで Thread.Sleep() を使った場合：
        // メインスレッドは空けながら、大体CPUと同じ分だけのスレッド（8つずつ）処理が進んでいく。
        // Task.Delay() の場合両者に違いはない。
        private static async void LoopByFor()
        {
            for (int i = 0; i < loopNum; i++)
            {
                await StartAsyncTask();
            }
        }

        // StartAsyncTaskで Thread.Sleep() を使った場合：
        // 一気に100スレッド立ったりはしないが、8つずつ処理が進んで100ループ終わるまでメインスレッドには戻らない。
        // Task.Delay() の場合両者に違いはない。
        private static void LoopByParallelFor()
        {
            Parallel.For(0, loopNum, async (i) =>
            {
                await StartAsyncTask();
            });
        }

        private static async Task StartAsyncTask()
        {
            ResponseObject response;
            response = await new Model().Fetch();
            log.InfoFormat("CALLBACK ID: {0}", response.CallbackId);
        }
    }
}
