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
            return container;
        }

    }
}
