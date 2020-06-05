using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class Pagination
    {
        public int TotalItems { get; set; }
        public int ItemsOnPage { get; set; }
        public int CurrentyPage { get; set; }
    }
}
