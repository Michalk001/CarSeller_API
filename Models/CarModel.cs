using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CarModel
    {


        public Guid Id { get; set; }

        public string ModelName { get; set; }

        public string IdString { get; set; }
        public virtual CarProducent CarProducent { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
