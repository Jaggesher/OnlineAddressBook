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

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            var data = await _contactServices.GetAllContactsAsync(currentUser.Id); 
            
            var model = new AllContactViewModel()
            {
                MyContacts = data 
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddContact() //Adding new contact view
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(PeopleViewModel model) // Adding new contact Save
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

        [HttpPost]

        public  IActionResult ViewPeople(Guid peopleId){
            return Json(peopleId);
        }

    }
}