using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CoreWebServicePOC.repo
{
    public class SqlDataContext : ISqlDataContext
    {

        private readonly SqlConnection _connection;

        public SqlDataContext()
        {
            _connection = CreateConnection();
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(SQLHelper.ConnectionString);
        }
    }

    public interface ISqlDataContext
    {
        SqlConnection CreateConnection();
    }
}
