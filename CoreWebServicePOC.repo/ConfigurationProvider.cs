using System.Data;
using System.Data.SqlClient;

namespace CoreWebServicePOC.repo
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IDbConnection _connection;

        public ConfigurationProvider()
        {
            _connection = CreateConnection();
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(SQLHelper.ValuesSqlConnection);
        }
    }
}
