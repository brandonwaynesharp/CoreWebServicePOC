using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace CoreWebServicePOC.repo
{
    public class SqlQueryConnectionProvider : ISqlQueryConnectionProvider
    {
        public SqlQueryConnectionProvider()
        {
        }

        public DbConnection GetConnection(string connectionName)
        {
            throw new NotImplementedException();
        }
    }
}
