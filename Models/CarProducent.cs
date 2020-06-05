using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CarProducent
    {
        public CarProducent()
        {
            this.CarModels = new HashSet<CarModel>();
        }
        public Guid Id { get; set; }
        
        public string ProducentName { get; set; }

        public string IdString { get; set; }
  
        public virtual ICollection<CarModel> CarModels  { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
