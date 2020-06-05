using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<CarOffer> CarOffers { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public bool BusinessProfile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public virtual List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }   
}
