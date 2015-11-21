using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace AsyncApp
{
    public sealed class CallbackContainer
    {
        private ConcurrentDictionary<string, Action<ResponseObject>> Container = new ConcurrentDictionary<string, Action<ResponseObject>>();

        private static CallbackContainer instance = new CallbackContainer();

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

        public string Add(Action<ResponseObject> callback)
        {
            var id = Guid.NewGuid().ToString();

            Container.AddOrUpdate(id, callback, (key, oldValue) =>
            {
                throw new InvalidOperationException("keyが重複しました");
            });

            return id;
        }

        public Action<ResponseObject> Get(string id)
        {
            return Container.GetOrAdd(id, (key) =>
            {
                throw new InvalidOperationException("keyが存在しません");
            });
        }

        // TODO remove, remove timeout
    }
}
