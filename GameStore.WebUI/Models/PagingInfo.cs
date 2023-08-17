using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class PagingInfo
    {
            // Products total number
        public int TotalItems { get; set; }

            // Products total number on the one page
        public int ItemPerPage { get; set; }

            // Current page number
        public int CurrentPage { get; set; }

            // Pages total number
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemPerPage); }
        }
    }
}