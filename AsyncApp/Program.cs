using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncApp
{
    class Program
    {
        static void Main(string[] args)
        {

            StartAsyncTask();

            // Taskは待たない（メインスレッドに処理を戻す）ので、こちらが先に実行される
            Debug.WriteLine("Amazing !");

            // コンソールを終了させない
            Console.ReadLine();
        }

        private static void StartAsyncTask()
        {
            var peer = new Peer();

            // 非同期処理の開始
            string param = "Hello World";

            // レスポンスまで3秒かかる
            peer.SendRequest<string>(param, (response) =>
            {
                // TODO log4netを使う
                Debug.WriteLine("Response got! callback id: {0}", response.CallbackId);
            });
        }
    }
}
