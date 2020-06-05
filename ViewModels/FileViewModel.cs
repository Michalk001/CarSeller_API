using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class FileViewModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Base64 { get; set; }
        public string Path { get; set; }
    }
}
