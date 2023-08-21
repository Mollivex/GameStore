using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public  class ShippingDetails
    {
        [Required(ErrorMessage = "What's your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Insert the first delivery address")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "What's your town")]
        public string City { get; set; }

        [Required(ErrorMessage = "What's your country")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
