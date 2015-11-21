using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncApp
{
    public class Peer
    {
        public async void SendRequest<T>(T param, Action<ResponseObject> callback)
        {
            // コールバックをコンテナに突っ込む
            var callbackId = CallbackContainer.Instance.Add(callback);

            await Task.Run(() =>
            {
                //
                // 通信しているつもり
                //
                System.Threading.Thread.Sleep(3000);

                // 通信が終わったと仮定してレスポンスハンドラを呼び出す。
                ResponseObject response = new ResponseObject() { CallbackId = callbackId };
                this.OnResponse(response);
            });
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
