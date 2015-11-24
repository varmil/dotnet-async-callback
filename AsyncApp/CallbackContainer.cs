using System;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;

namespace AsyncApp
{
    public sealed class CallbackContainer
    {
        private ConcurrentDictionary<int, Action<ResponseObject>> Container = new ConcurrentDictionary<int, Action<ResponseObject>>();

        private static CallbackContainer instance = new CallbackContainer();

        private int idConter = 0;

        public static CallbackContainer Instance
        {
            get
            {
                return instance;
            }
        }

        private CallbackContainer()
        {

        }

        public int Add(Action<ResponseObject> callback)
        {
            var id = Interlocked.Increment(ref idConter);

            Container.AddOrUpdate(id, callback, (key, oldValue) =>
            {
                throw new InvalidOperationException("keyが重複しました");
            });

            return id;
        }

        public Action<ResponseObject> Get(int id)
        {
            return Container.GetOrAdd(id, (key) =>
            {
                throw new InvalidOperationException("keyが存在しません");
            });
        }

        // TODO remove, remove timeout
    }
}
