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
        [Display(Name="First address")]
        public string Line1 { get; set; }

        [Display(Name="Second address")]
        public string Line2 { get; set; }

        [Display(Name = "Third address")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "What's your town")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "What's your country")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
