﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceBooking.DAL.Entities
{
    public class Response
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }
    }
}