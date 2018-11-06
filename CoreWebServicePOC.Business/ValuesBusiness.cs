using CoreWebServicePOC.core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreWebServicePOC.Business
{
    public class ValuesBusiness : IValuesBusiness
    {
        private readonly IValuesRepo _repo;
        
        public ValuesBusiness(IValuesRepo repo)
        {
            _repo = repo;
        }
        
        public async Task<IList<Value>> Get()
        {
            var list = await Task.Run(() => _repo.GetAllValues());

            return list;
        }
    }
}
