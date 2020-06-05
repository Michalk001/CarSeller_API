using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Equipment
    {
      
        public Guid Id { get; set; }
     
        public string Name { get; set; }
       
        public string IdString { get; set; }
        public List<CarOfferEquipment> CarOffers { get; set; } = new List<CarOfferEquipment>();
        public bool Deleted { get; set; } = false;
    }
}
