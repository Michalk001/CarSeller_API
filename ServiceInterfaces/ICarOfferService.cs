using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ICarOfferService
    {
        Task<object> Save(CarOfferViewModel model, string token);
        Task<object> Get(string title);
        Task<object> GetRaw(string title);
        Task<List<CarOfferViewModel>> Get();
        Task<List<string>> GetAllOnlyTitle();
        Task<object> Remove(string title,string token);
        Task<object> Update(CarOfferViewModel model, string token);

    }
}
