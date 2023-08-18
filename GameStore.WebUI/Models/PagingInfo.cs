using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class PagingInfo
    {
            /// <summary>
            /// Products total number
            /// </summary>
        public int TotalItems { get; set; }

            /// <summary>
            /// Products total number on the one page
            /// </summary>
        public int ItemPerPage { get; set; }

            /// <summary>
            /// Current page number
            /// </summary>
        public int CurrentPage { get; set; }

            /// <summary>
            /// Pages total number
            /// </summary>
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemPerPage); }
        }
    }
}