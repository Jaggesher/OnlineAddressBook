using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineAddressBook.Models
{
    public class Phone
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Phone Number cannot be longer than 20 characters.")]
        public String Number { get; set; }

        public People People { get; set; }

        [Required]
        public Guid PeopleId { get; set; }
    }
}