using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAddressBook.Models
{
    public class PhoneViewModel
    {
        [MaxLength(20, ErrorMessage = "Phone Number cannot be longer than 20 characters.")]
        public String Phone { get; set; }
    }
}