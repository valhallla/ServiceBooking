using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class IndexCommentViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int Rating { get; set; }

        public int PerformerId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public byte[] Image { get; set; }
    }

    public class CreateCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment can't be empty")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public string Rating { get; set; }

        public byte[] Image { get; set; }

        public int PerformerId { get; set; }
    }
}