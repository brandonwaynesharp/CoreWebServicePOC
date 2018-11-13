using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace CoreWebServicePOC.repo
{
    public class ValuesRepo : IValuesRepo
    {
        private readonly ISqlDataContext _context;

        public ValuesRepo(ISqlDataContext context)
        {
            _context = context;
        }

        public async Task<IList<Value>> GetAllValues()
        {
            List<Value> retList = new List<Value>();

            using (IDbConnection conn = _context.CreateConnection())
            {
                conn.Open();
                return (await conn.QueryAsync<Value>(@"SELECT [id],[value] FROM [ValueDB].[dbo].[Value]")).ToList();
            }
        }
    }
}
