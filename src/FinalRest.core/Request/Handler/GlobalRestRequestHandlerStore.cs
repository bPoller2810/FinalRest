using System;
using System.Collections.Generic;

namespace FinalRest.core
{
    internal sealed class GlobalRestRequestHandlerStore
    {

        #region singleton
        private static GlobalRestRequestHandlerStore _instance;
        private GlobalRestRequestHandlerStore() { }
        public static GlobalRestRequestHandlerStore Store
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new GlobalRestRequestHandlerStore();
                }
                return _instance;
            }
        }
        #endregion

        private readonly Dictionary<Type, object> _handlers = new Dictionary<Type, object>();

        public THandler GetHandlerInstance<THandler>(Type handlerType)
        {
            if (_handlers.ContainsKey(handlerType) && _handlers[handlerType] is THandler handler)
            {
                return handler;
            }
            var instance = Activator.CreateInstance(handlerType);
            if (instance is THandler newHandler)
            {
                _handlers.Add(handlerType, newHandler);
                return newHandler;
            }
            throw new InvalidOperationException($"{handlerType.Name} is not {typeof(THandler).Name}");
        }

    }
}
