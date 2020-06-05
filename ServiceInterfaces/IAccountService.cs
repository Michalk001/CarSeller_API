using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ServiceInterfaces
{
    public interface IAccountService
    {
        Task<object> Login(LoginUserViewModel model);
        Task<object> Register(RegisterUserViewModel model);
        bool ValidateToken(string authToken);
        Task<object> ChangePassword(string token, string passwordOld, string passwordNew);
        Task<object> ChangeData(string token, UserChangeDataViewModel model);
    }
}
