using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UserChangeDataViewModel
    {

        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
