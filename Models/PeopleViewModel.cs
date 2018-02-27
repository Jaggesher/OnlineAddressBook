using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAddressBook.Models
{
    public class PeopleViewModel
    {

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "Full Name")]
        public String FullName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Nick Name cannot be longer than 20 characters.")]
        [Display(Name = " Nick Name")]
        public String NickName { get; set; }
        public String WebSite { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        public DateTime BirthDate { get; set; }

        [StringLength(150, ErrorMessage = " Address cannot be longer than 150 characters.")]
        public String Address { get; set; }
        
        public List<PhoneViewModel> Phones { get; set; }

        public People SinglePeople { get; set; } // For edit purpose

    }
}