using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ICarProducentService
    {

        Task<object> Save(CarProducentViewModel model);
        Task<object> Get();
        Task<object> Get(string idString);
        Task<object> Remove(string idString);
        Task<object> Update(CarProducentViewModel model);
    }
}
