using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAddressBook.Models;

namespace OnlineAddressBook.Controllers
{
    [Authorize]
    public class Contact : Controller
    {
        public Contact()
        {
            
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
        public IActionResult AddContact(PeopleViewModel model)
        {
            
            return Json(model);

            if(!ModelState.IsValid){
               return BadRequest(ModelState);
            }

           return  Redirect ("AddContact");

        }

    }
}