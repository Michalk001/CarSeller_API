using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name) : base(name)
        {

        }
        public Role(Guid guid) : base()
        {

        }
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
