using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue = false)]
        public int GameId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please, enter game name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Description")]
        [Required(ErrorMessage = "Please, enter game description")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please, enter game category")]
        public string Category { get; set; }

        [Display(Name = "Price (mdl)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please, enter price positive value")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
