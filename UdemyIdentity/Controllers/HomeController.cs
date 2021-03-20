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
        //giriş yapmak için singInManagera da ihityacımız var;
        private readonly SignInManager<AppUser> _singInManager;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;

        }
        public IActionResult Index()
        {
            return View(new UserSignViewInModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>GirisYAp(UserSignViewInModel model)
        {
            if(ModelState.IsValid)
            {
                //isPersistent = kullanıcıyı hatırla hatırlama
                //lockoutOnFailure = kullanıcı belirlli sayıda şifresini yanlış girerse kullanıcıyı bloklayalım mı bloklamayalım mı ? süre olaraf default 5 dk gibi
              var identitResult =  await _singInManager.PasswordSignInAsync(model.UserName, model.Password, model.rememberMe, true /*true olunca kullanıcı yanlış girince hesabı bloklayaak databasede accessFailedCount gözlemlenmeli*/ );
                                                                                                            //kuallnıcıya bıraktık hatırlayı hatırlamayacağını
                
                
                //Eğer Hesabımız kitlendiyse
                if(identitResult.IsLockedOut)// databasede lockedout değeri kontrol edilmeli= true olucak
                {

                    //ne kadar süre bloklu kaldığını buluyoruz
                   var gelen = await _userManager.GetLockoutEndDateAsync(await _userManager.FindByNameAsync(model.UserName));
                    var ksıtlananSure = gelen.Value;
                    var kalandk =ksıtlananSure.Minute - DateTime.Now.Minute ;


                    ModelState.AddModelError("", $"5 kere yanlış giriş yaptığınız için hesabınız kitlenmiştir.{kalandk} dk kitlenmiştir ");
                    return View("Index", model);
                }
                if(identitResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Email adresinizi Lütfen doğrulayınız");
                    return View("Index", model);
                }
                
                if(identitResult.Succeeded)
                {
                    return RedirectToAction("Index", "Panel");
                }

                //kalan giriş hakkını çekip kullanıcıya yazıyoruz
                //default 5 hakkımız var
                var yanlisGirilmeSayisi = await _userManager.GetAccessFailedCountAsync( await _userManager.FindByNameAsync(model.UserName));
                ModelState.AddModelError("", $"Kullanıcı adı veya şifre hatalı{5- yanlisGirilmeSayisi} kadar yanlış girerseniz hesabınız bloklanacak");
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
