using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class CarOfferAdverViewModel
    {
        public string Title { get; set; }
        public string ProducentName { get; set; }
        public string ModelName { get; set; }
        public string ShortDescription { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string Fuel { get; set; }
        public string Mileage { get; set; }
        public DateTime AddDate { get; set; }
        public FileViewModel FileViewModel { get; set; }
    }
}
