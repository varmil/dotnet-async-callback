using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace AsyncApp
{
    public class Model
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // 非同期callbackをTaskに変換する
        public Task<ResponseObject> Fetch()
        {
            var tcs = new TaskCompletionSource<ResponseObject>();

            var peer = new Peer();

            string param = "Hello World";

            peer.SendRequest<string>(param, (response) =>
            {
                log.InfoFormat("Response got! callback id: {0}", response.CallbackId);

                var result = tcs.TrySetResult(response);
            });

            return tcs.Task;
        }
    }
}
