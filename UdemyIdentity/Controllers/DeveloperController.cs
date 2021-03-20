using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Controllers
{
    public class DeveloperController : Controller
    {
        [Authorize(Roles ="Admin,Developer")]//admin veye deveeloperR girebilir
        public IActionResult Index()
        {
            return View();
        }
    }
}
