using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.WEB.Filters
{
    public class FutureDateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value) 
            => (value is DateTime) && ((DateTime)value) > DateTime.Today && base.IsValid(value);
    }
}