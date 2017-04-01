using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.WEB.Models
{
    public class CategoryViewModel
    {
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}