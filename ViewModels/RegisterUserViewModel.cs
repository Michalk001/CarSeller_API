using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; } 
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string TypeSeller { get; set; } = "0";
    }
}
