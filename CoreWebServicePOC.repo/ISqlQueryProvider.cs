using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CoreWebServicePOC.repo
{
    public interface ISqlQueryProvider
    {
        IQueryReader Execute(string connectionName, string procedureName, object parameters);
        IQueryReader Execute(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters);
        T ExecuteScalar<T>(string connectionName, string procedureName, object parameters);
        void ExecuteNonQuery(string connectionName, string procedureName, object parameters);
        void ExecuteNonQuery(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters);
        //Async methods
        Task<IQueryReader> ExecuteAsync(string connectionName, string procedureName, object parameters);
        Task<IQueryReader> ExecuteAsync(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters);
        Task<IQueryReader> ExecuteSqlAsync(string connectionName, string sql, object parameters);
        Task<IQueryReader> ExecuteSqlAsync(string connectionName, string sql);
        Task<T> ExecuteScalarAsync<T>(string connectionName, string procedureName, object parameters);
        Task<T> ExecuteScalarSqlAsync<T>(string connectionName, string sql, object parameters);
        Task ExecuteNonQueryAsync(string connectionName, string procedureName, object parameters);
        Task ExecuteNonQueryAsync(string connectionName, string procedureName, IEnumerable<IDbDataParameter> parameters);
        Task ExecuteNonQuerySqlAsync(string connectionName, string sql, object parameters);
    }
}
