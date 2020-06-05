
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class CarOfferViewModel
    {
    

        public string ShortDescription { get; set; } = "";

        public string Description { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public List<string> Equipment { get; set; } = null;
       
        public string CarProducent { get; set; } = "";

        public string CarModel { get; set; } = "";

        public string Fuel { get; set; } = "";

        public string Title { get; set; } = "";

        public string Year { get; set; } = "";

        public string Price { get; set; } = "";

        public string VinNumber { get; set; } = "";

        public List<FileViewModel> FileViewModels { get; set; } = null;

        public string Mileage { get; set; } = "";

        public string Condition { get; set; } = "";

        public string Seat { get; set; } = "";

        public string Door { get; set; } = "";

        public string Country { get; set; } = "";

        public string HoursePower { get; set; } = "";

        public string Color { get; set; } = "";

        public string BusinessProfile { get; set; } = "";

        public string UserName { get; set; } = "";

    }
}
