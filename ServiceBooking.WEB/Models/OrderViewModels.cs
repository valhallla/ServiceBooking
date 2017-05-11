using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServiceBooking.WEB.Filters;

namespace ServiceBooking.WEB.Models
{
    public class IndexOrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool AdminStatus { get; set; }

        public int CustomerId { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }
    }

    public class DetailsOrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Category { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public bool AdminStatus { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        public decimal Price { get; set; }  
        
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public byte[] Image { get; set; }

        public IEnumerable<IndexResponseViewModel> Responses { get; set; }
    }

    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [StringLength(60, ErrorMessage = "The {0} must be from 2 to 60 characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000, ErrorMessage = "The {0} must be from 10 to 1000 characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Completion date is required")]
        [Display(Name = "Completion date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "Completion date must refere to future")]
        public DateTime CompletionDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [Range(0, 10000000000, ErrorMessage = "The {0} must be from 0 to 10000000000 $")]
        public decimal Price { get; set; }

        [Display(Name = "Photo")]
        public byte[] Image { get; set; }
    }

    public class DeleteOrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        public string CustomerName { get; set; }
    }
}