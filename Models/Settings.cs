using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireMinutes { get; set; }
    }
}
