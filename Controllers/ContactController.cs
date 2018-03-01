using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAddressBook.Models;
using OnlineAddressBook.Services;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace OnlineAddressBook.Controllers
{
    [Authorize]
    public class Contact : Controller
    {
        private readonly IContactService _contactServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public Contact(IContactService contact, UserManager<ApplicationUser> userManager)
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _contactServices.AddContactAsync(model, currentUser.Id);

            //return Json(currentUser);
            if (successful) return Redirect("Index");

            return BadRequest(new { error = "Could not add new Contact." });

        }

        [HttpGet]

        public async Task<IActionResult> ViewContact(Guid peopleId)
        {
            var data = await _contactServices.GetPeople(peopleId);
            if (data == null) return BadRequest(new { error = "Opps! We can not find any thing" });
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            if (currentUser.Id != data.UserId) return BadRequest(new { error = "Opps! Access Denying" });
            var model = new PeopleViewModel()
            {
                SinglePeople = data
            };

            //return Json(model);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditContact(PeopleViewModel model, Guid peopleId, String UserId)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            if (currentUser.Id != UserId) return BadRequest(new { error = "Opps! Access Denying" });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var successful = await _contactServices.SaveChanges(model, peopleId);

            //return Json(currentUser);
            if (successful) return Redirect("Index");

            return BadRequest(new { error = "Could not add new Contact." });

        }

        public async Task<IActionResult> DeleteIt(Guid peopleId)
        {
            //return Json(peopleId);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _contactServices.DeleteIt(peopleId);

            //return Json(currentUser);
            if (successful) return Redirect("Index");

            return BadRequest(new { error = "Opps!! We can't find any thing." });

        }

        [HttpPost]
        public async Task<IActionResult> DownloadCSV()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            var data = await _contactServices.GetAllContactsAsync(currentUser.Id);

            var model = new AllContactViewModel()
            {
                MyContacts = data
            };

            var memory = new MemoryStream();

            memory = await _contactServices.BuildCSV(model);
            
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", @"ContactList.xlsx");
        }

    }
}