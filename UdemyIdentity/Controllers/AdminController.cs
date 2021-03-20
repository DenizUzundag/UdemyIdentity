using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Controllers
{
    [Authorize(Roles ="Admin")]  //sadece adminler
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
