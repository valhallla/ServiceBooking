using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServiceBooking.BLL.DTO;

namespace ServiceBooking.WEB.Models
{
    public class IndexPerformerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public int? Rating { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        public string Category { get; set; }
    }

    public class DetailsPerformerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        [DataType(DataType.MultilineText)]
        public string Info { get; set; }

        public int? Rating { get; set; }

        public bool AdminStatus { get; set; }

        public IEnumerable<int> CustomersId { get; set; }

        public IEnumerable<IndexCommentViewModel> Comments { get; set; }
    }
}