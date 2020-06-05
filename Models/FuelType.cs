using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class FuelType
    {
   
        public Guid Id { get; set; }
        public string FuelName { get; set; }
        public string IdString { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
