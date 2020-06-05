using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ICarModelService
    {
        Task<object> Save(CarModelViewModel model);
        Task<object> Get();
        Task<CarModelViewModel> Get(string idString);
        Task<object> Remove(string idString, string idProducer);
        Task<object> Update(CarModelViewModel model);
    }
}
