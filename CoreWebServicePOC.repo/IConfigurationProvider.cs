using System.Data;

namespace CoreWebServicePOC.repo
{
    public interface IConfigurationProvider
   {
        IDbConnection CreateConnection();
    }
}
