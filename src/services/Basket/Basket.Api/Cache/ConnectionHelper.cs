using StackExchange.Redis;
using System;

namespace Basket.Api.Cache
{
    public class ConnectionHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        static ConnectionHelper()
        {
            // object is created only when it is first accessed => for Thread Safety: The Lazy<T> type in C# provides built-in thread safety for lazy initialization.
            // It ensures that only a single instance of ConnectionMultiplexer is created even in a multi-threaded environment.
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
            });
        }
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }


    }
}
