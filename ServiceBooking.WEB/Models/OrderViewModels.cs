using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public IEnumerable<IndexResponseViewModel> Responses { get; set; }
    }

    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Completion date is required")]
        [Display(Name = "Completion date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CompletionDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
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