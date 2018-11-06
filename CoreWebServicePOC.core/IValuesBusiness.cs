using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebServicePOC.core
{
    public interface IValuesBusiness
    {
        Task<IList<Value>> Get();
    }
}
