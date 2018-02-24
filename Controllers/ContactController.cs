using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAddressBook.Models;

namespace OnlineAddressBook.Controllers
{
    [Authorize]
    public class Contract : Controller
    {
        public Contract()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddContract()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddContract(PeopleViewModel model)
        {
            if(!ModelState.IsValid){

            }else{
                return BadRequest();
            }
            return View();
        }

    }
}