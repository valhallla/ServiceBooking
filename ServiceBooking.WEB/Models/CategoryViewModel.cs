﻿using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ItemsAmount { get; set; }
    }
}