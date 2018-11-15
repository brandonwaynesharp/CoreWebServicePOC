using SimpleInjector;
using CoreWebServicePOC.core;
using CoreWebServicePOC.Business;
using CoreWebServicePOC.repo;

namespace CoreWebServicePOC
{
    public class SimpleInjectorWrapper
    {
        public Container AddApplicationDependencies(Container container)
        {
            container.Register<IValuesBusiness, ValuesBusiness>(Lifestyle.Scoped);

            container.Register<IValuesRepo, ValuesRepo>(Lifestyle.Scoped);

            container.Register<IConfigurationProvider, ConfigurationProvider>(Lifestyle.Scoped);

            container.Register<ISqlQueryConnectionProvider, SqlQueryConnectionProvider>(Lifestyle.Scoped);

            container.Register<ISqlQueryProvider, SqlQueryProvider>(Lifestyle.Scoped);

            container.Register<IQueryReader, QueryReader>(Lifestyle.Scoped);

            return container;
        }

    }
}
