using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface IEquipmentService
    {
        Task<Object> Save(EquipmentViewModel model);
        Task<Object> Get();
        Task<EquipmentViewModel> Get(string idString);
        Task<Object> Remove(string idString);
        Task<Object> Update(EquipmentViewModel model);
    }
}
