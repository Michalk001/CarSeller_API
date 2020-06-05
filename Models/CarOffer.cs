using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CarOffer
    {


        public Guid Id { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; } 
        public virtual List<CarOfferEquipment> Equipments { get; set; } = new List<CarOfferEquipment>();
        public virtual ICollection<File> Files { get; set; }
        public virtual CarProducent CarProducent { get; set; }
        public virtual CarModel CarModel { get; set; }
        public virtual FuelType Fuel { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string VinNumber { get; set; }
        public string Year { get; set; }
        public string Mileage { get; set; }
        public DateTime AddDate { get; set; }
        public string Condition { get; set; }
        public string Seat { get; set; }
        public string Door { get; set; }
        public string Country { get; set; }
        public string HoursePower { get; set; }
        public string Color { get; set; }
        public bool Deleted { get; set; } = false;
        public bool Hidden { get; set; } = false;
        public virtual User User { get; set; }
    }
}
