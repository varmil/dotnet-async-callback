using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace AsyncApp
{
    public class Peer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async void SendRequest<T>(T param, Action<ResponseObject> callback)
        {
            // コールバックをコンテナに突っ込む
            var callbackId = CallbackContainer.Instance.Add(callback);

            // ここで仮に Parallel.Invoke() を使うと、3秒待ってから「Amazing」出力される＝待つ
            // 対してタスクはawaitを書くことでコルーチンのyield returnのような扱いになる。
            // つまり、グローバルキューに積んだらメインスレッドに処理を返す
            await Task.Run(async () =>
            {
                //
                // 通信しているつもり（待機可能というより非同期実行が可能というイメージ）
                //
                await Task.Delay(3000);

                // 通信が終わったと仮定してレスポンスハンドラを呼び出す。
                ResponseObject response = new ResponseObject() { CallbackId = callbackId };
                this.OnResponse(response);
            });

            // awaitする場合はこれが後。しない場合はこれが先。
            log.Info("SendRequest after Task.Run()");
        }

        private void OnResponse(ResponseObject response)
        {
            // コンテナからdelegateを取り出す
            var callback = CallbackContainer.Instance.Get(response.CallbackId);

            // 委譲
            callback(response);
        }
    }
}
