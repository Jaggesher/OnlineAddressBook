using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAddressBook.Models;
using OnlineAddressBook.Services;
using Microsoft.AspNetCore.Identity;

namespace OnlineAddressBook.Controllers
{
    [Authorize]
    public class Contact : Controller
    {
        private readonly IContactService _contactServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public Contact( IContactService contact , UserManager<ApplicationUser> userManager)
        {
            _contactServices = contact;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(PeopleViewModel model)
        {
            
            //return Json(model);

            if(!ModelState.IsValid){
               return BadRequest(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            
            var successful = await _contactServices.AddContactAsync(model,currentUser.Id);

            //return Json(currentUser);
            if(successful) return  Redirect ("AddContact");

            return BadRequest(new { error = "Could not add new Contact." });

        }

    }
}