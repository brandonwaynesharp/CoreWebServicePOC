using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CoreWebServicePOC.repo
{
    public class ValuesRepo : IValuesRepo
    {
        public async Task<IList<Value>> GetAllValues()
        {
            List<Value> retList = new List<Value>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
            {
                await conn.OpenAsync();
                var commandString = @"SELECT [id],[value] FROM [ValueDB].[dbo].[Value]";
                using (SqlCommand comm = new SqlCommand(commandString, conn))
                {

                    using (SqlDataReader dr = await comm.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Value v = new Value();
                            v.id = int.Parse(dr[0].ToString());
                            v.value = System.Convert.ToString(dr[1]);
                            retList.Add(v);
                        }
                    }

                }
            }
            return retList;
        }
    }
}
