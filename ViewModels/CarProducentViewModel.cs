using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class CarProducentViewModel
    {
        public string ProducentName { get; set; }
        public string IdString { get; set; }
        public List<CarModelViewModel> CarModelViewModels { get; set; }
    }
}
