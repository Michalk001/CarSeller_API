using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
