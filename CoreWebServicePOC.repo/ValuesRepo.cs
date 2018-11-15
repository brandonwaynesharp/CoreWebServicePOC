using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Collections.Specialized;
using System.Configuration;

namespace CoreWebServicePOC.repo
{
    public class ValuesRepo : IValuesRepo
    {
        private readonly ISqlQueryProvider _sqlQueryProvider;

        public ValuesRepo(ISqlQueryProvider sqlQueryProvider)
        {
            _sqlQueryProvider = sqlQueryProvider;
        }

        public async Task<IList<Value>> GetAllAsync()
        {
            using (var reader = await _sqlQueryProvider.ExecuteSqlAsync(RepositorySettings.ValuesConnectionName,
                    "SELECT [id],[value] FROM [ValueDB].[dbo].[Value];"))
            {
                var records = reader.Read<Value>();

                return records.Select(m => new Value
                {
                    id = m.id,
                    value = m.value
                }).ToList();
            }
        }
    }
}
