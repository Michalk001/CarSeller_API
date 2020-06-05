using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface ISearchOfferService
    {
        Task<object> Get(string page, string producer, string model, string fuel/*, string yearMin, string yearMax*/);
        Task<object> GetByUser(string page, string user);
    }
}
