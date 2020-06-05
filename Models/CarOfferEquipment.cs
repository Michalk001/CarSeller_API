using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CarOfferEquipment
    {
       // public Guid Id { get; set; }
        public CarOffer CarOffer { get; set; }
        public Equipment Equipment { get; set; }

        public Guid CarOfferId { get; set; }
        public Guid EquipmentId { get; set; }

    }
}
