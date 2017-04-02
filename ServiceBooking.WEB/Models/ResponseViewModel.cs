using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.WEB.Models
{
    public class IndexResponseViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public int OrderId { get; set; }

        public int PerformerId { get; set; }

        public string PerformerName { get; set; }
    }
}