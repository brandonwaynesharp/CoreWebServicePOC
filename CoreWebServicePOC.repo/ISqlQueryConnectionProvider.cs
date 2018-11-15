using System.Data.Common;

namespace CoreWebServicePOC.repo
{
    public interface ISqlQueryConnectionProvider
    {
        DbConnection GetConnection(string connectionName);
    }
}
