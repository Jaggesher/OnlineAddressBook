using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineAddressBook.Models
{
    public class People
    {
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }

        public String UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public String FullName { get; set; }

        [Required]
        [StringLength(20)]
        public String NickName { get; set; }
        public String WebSite { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(150)]
        public String Address { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public String DisplayName(){
            return FullName+ "("+NickName+")";
        }

    }
}