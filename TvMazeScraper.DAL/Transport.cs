using StackExchange.Redis;

namespace TvMazeScraper.DAL
{
    public class Transport
    {
        private ConnectionMultiplexer _connectionMultiplexer;

        public Transport(StoreConfiguration config)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(config.ConnectionString);
        }

        public IDatabase GetChannel()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}