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

        public SqlDataContext(string connectionString)
        {
            _connection = CreateConnection(connectionString);
        }

        public SqlDataContext()
        {
            _connection = CreateConnection(SQLHelper.ConnectionString);
        }

        public IDataReader ExecuteReader(string storedProcedureName, ICollection<SqlParameter> parameters)
        {
            _connection.Open();

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parm in parameters)
            {
                command.Parameters.Add(parm);
            }            
            command.CommandText = storedProcedureName;

            return command.ExecuteReader();
        }

        public async Task<DbDataReader> ExecuteReaderAsync(string commandString)
        {
            await _connection.OpenAsync();
            SqlCommand command = new SqlCommand(commandString, _connection);
            return await command.ExecuteReaderAsync();
        }

        private SqlConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            return new SqlConnection(connectionString);
        }
    }

    public interface ISqlDataContext
    {
        IDataReader ExecuteReader(string storedProcedureName, ICollection<SqlParameter> parameters);

        Task<DbDataReader> ExecuteReaderAsync(string commandString);
    }
}
