using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebServicePOC.repo
{
    public class ValuesRepo : IValuesRepo
    {
        private readonly ISqlDataContext _context;

        public async Task<IList<Value>> GetAllValues()
        {           

            List<Value> retList = new List<Value>();
            var commandString = @"SELECT [id],[value] FROM [ValueDB].[dbo].[Value]";
            using (var dr = await _context.ExecuteReaderAsync(commandString))
            {
                while (await dr.ReadAsync())
                {
                    Value v = new Value();
                    v.id = int.Parse(dr[0].ToString());
                    v.value = System.Convert.ToString(dr[1]);
                    retList.Add(v);
                }
            }
            return retList;
        }
    }
}
