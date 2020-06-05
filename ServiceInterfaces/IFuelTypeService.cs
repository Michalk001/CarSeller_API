using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface IFuelTypeService
    {
        Task<object> Save(FuelTypeViewModel model);
        Task<object> Get();
        Task<FuelTypeViewModel> Get(string idString);
        Task<object> Remove(string idString);
        Task<object> Update(FuelTypeViewModel model);
    }
}
