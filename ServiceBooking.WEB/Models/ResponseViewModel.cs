using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class IndexResponseViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int OrderId { get; set; }

        public int PerformerId { get; set; }

        public string PerformerName { get; set; }

        public int PerformerRating { get; set; }
    }

    public class CreateResponseViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public int OrderId { get; set; }
    }
}