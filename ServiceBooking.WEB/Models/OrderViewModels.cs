using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class IndexOrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public bool AdminStatus { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class DetailsOrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        public bool AdminStatus { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        [Required]
        public decimal Price { get; set; }  
        
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public IEnumerable<IndexResponseViewModel> Responses { get; set; }
    }

    public class CreateOrderViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Category { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class DeleteOrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        public string CustomerName { get; set; }
    }
}