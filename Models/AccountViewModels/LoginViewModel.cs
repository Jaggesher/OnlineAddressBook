using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAddressBook.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [Display(Name = "User Name")]
        public String Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
