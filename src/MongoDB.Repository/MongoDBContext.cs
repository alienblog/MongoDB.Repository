using System.Configuration;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public abstract class MongoDBContext : IMongoDBContext
    {
        readonly string _connectionStringName;
        public string ConnectionStringName { get { return _connectionStringName; } }
        public MongoDBContext(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public abstract void OnRegisterModel(ITypeRegistration registration);

        IConfigurationRegistration _configuration;
        public IConfigurationRegistration BuildConfiguration()
        {
            if (_configuration != null) return _configuration;

            var setting = ConfigurationManager.ConnectionStrings[_connectionStringName];
            if (setting == null) throw new MongoConnectionException("Wrong ConnectionString Name");
            var url = new MongoUrl(setting.ConnectionString);
            _configuration = new ConfigurationRegistration();
            _configuration.Add(GetType(), url);
            return _configuration;
        }
    }
}
