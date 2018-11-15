using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;


namespace CoreWebServicePOC.repo
{
    public class SqlQueryProvider : ISqlQueryProvider
    {
        private readonly ISqlQueryConnectionProvider _queryConnectionProvider;

        public SqlQueryProvider(ISqlQueryConnectionProvider queryConnectionProvider)
        {
            _queryConnectionProvider = queryConnectionProvider;
        }

        public IQueryReader Execute(string connectionName, string procedureName, object parameters)
        {
            var connection = _queryConnectionProvider.GetConnection(connectionName);

            SqlMapper.GridReader reader;

            try
            {
                connection.Open();
                reader = connection.QueryMultiple(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(reader, connection);
        }

        public async Task<IQueryReader> ExecuteAsync(string connectionName, string procedureName, object parameters)
        {
            var connection = _queryConnectionProvider.GetConnection(connectionName);

            Task<SqlMapper.GridReader> reader;

            try
            {
                connection.Open();
                reader = connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(await reader, connection);
        }

        public T ExecuteScalar<T>(string connectionName, string procedureName, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {

                try
                {
                    connection.Open();
                    return connection.ExecuteScalar<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        public async Task<IQueryReader> ExecuteSqlAsync(string connectionName, string sql, object parameters)
        {
            var connection = _queryConnectionProvider.GetConnection(connectionName);

            Task<SqlMapper.GridReader> reader;

            try
            {
                connection.Open();
                reader = connection.QueryMultipleAsync(sql, parameters);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(await reader, connection);
        }

        public async Task<IQueryReader> ExecuteSqlAsync(string connectionName, string sql)
        {
            //var connection = _queryConnectionProvider.GetConnection(connectionName);
            var connection = new SqlConnection(SQLHelper.ValuesSqlConnection);

            Task<SqlMapper.GridReader> reader;

            try
            {
                connection.Open();
                reader = connection.QueryMultipleAsync(sql);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(await reader, connection);
        }

        public async Task<T> ExecuteScalarAsync<T>(string connectionName, string procedureName, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {

                try
                {
                    connection.Open();
                    return await connection.ExecuteScalarAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScalarSqlAsync<T>(string connectionName, string sql, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {

                try
                {
                    connection.Open();
                    return await connection.ExecuteScalarAsync<T>(sql, parameters);
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        public IQueryReader Execute(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters)
        {
            var connection = _queryConnectionProvider.GetConnection(connectionName);
            var paramsList = new DbParams();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    paramsList.Add(p);
                }
            }

            SqlMapper.GridReader reader;

            try
            {
                connection.Open();
                reader = connection.QueryMultiple(procedureName, paramsList, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(reader, connection);

        }

        public async Task<IQueryReader> ExecuteAsync(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters)
        {
            var connection = _queryConnectionProvider.GetConnection(connectionName);
            var paramsList = new DbParams();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    paramsList.Add(p);
                }
            }

            SqlMapper.GridReader reader;

            try
            {
                connection.Open();
                reader = await connection.QueryMultipleAsync(procedureName, paramsList, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                connection.Close();
                throw;
            }
            return new QueryReader(reader, connection);

        }

        private class DbParams : SqlMapper.IDynamicParameters, IEnumerable<IDbDataParameter>
        {
            private readonly List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            public IEnumerator<IDbDataParameter> GetEnumerator() { return parameters.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public void Add(IDbDataParameter value)
            {
                parameters.Add(value);
            }
            void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command,
                SqlMapper.Identity identity)
            {
                foreach (IDbDataParameter parameter in parameters)
                    command.Parameters.Add(parameter);

            }
        }

        public void ExecuteNonQuery(string connectionName, string procedureName, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {
                try
                {
                    connection.Open();
                    connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public async Task ExecuteNonQueryAsync(string connectionName, string procedureName, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {
                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public void ExecuteNonQuery(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {
                var paramsList = new DbParams();
                foreach (var p in parameters)
                {
                    paramsList.Add(p);
                }

                try
                {
                    connection.Open();
                    connection.Execute(procedureName, paramsList, commandType: CommandType.StoredProcedure);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task ExecuteNonQueryAsync(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {
                var paramsList = new DbParams();
                foreach (var p in parameters)
                {
                    paramsList.Add(p);
                }

                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(procedureName, paramsList, commandType: CommandType.StoredProcedure);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task ExecuteNonQuerySqlAsync(string connectionName, string sql, object parameters)
        {
            using (var connection = _queryConnectionProvider.GetConnection(connectionName))
            {
                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(sql, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }

    public interface IQueryReader : IDisposable
    {
        IEnumerable<T> Read<T>();
        Task<IEnumerable<T>> ReadAsync<T>();
    }

    public class QueryReader : IQueryReader
    {
        private readonly SqlMapper.GridReader _reader;
        private readonly DbConnection _connection;

        public QueryReader(SqlMapper.GridReader reader, DbConnection connection)
        {
            _reader = reader;
            _connection = connection;
        }

        public IEnumerable<T> Read<T>()
        {
            return _reader.Read<T>();
        }

        public async Task<IEnumerable<T>> ReadAsync<T>()
        {
            return await _reader.ReadAsync<T>();
        }

        public void Dispose()
        {
            _reader.Dispose();
            _connection.Close();
        }
    }
}
