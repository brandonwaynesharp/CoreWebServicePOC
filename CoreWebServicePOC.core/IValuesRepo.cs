using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebServicePOC.core
{
    public interface IValuesRepo
    {
        Task<IList<Value>> GetAllAsync();
    }
}
