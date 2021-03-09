using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentity.Context;
using UdemyIdentity.Models;

namespace UdemyIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View(new UserSignViewInModel());
        }
        [HttpPost]
        public IActionResult GirisYAp(UserSignViewInModel model)
        {
            if(ModelState.IsValid)
            {

            }
            return View("Index", model);
        }
        public IActionResult KayitOl()
        {
            return View(new UserSingUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> KayitOl(UserSingUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = model.Email,
                    Name = model.Name,
                    UserName = model.UserName,
                    SurName = model.SurName


                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }



            }
            
            return View(model);
        }
    }
}
