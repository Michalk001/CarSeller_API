using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
/*
        public override Guid RoleId { get; set; }
        public override Guid UserId { get; set; }*/
    }
}
