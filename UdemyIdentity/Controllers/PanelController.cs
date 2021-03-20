using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentity.Context;
using UdemyIdentity.Models;

namespace UdemyIdentity.Controllers
{
    //Giriş yapmış kullanıcının göreceği (oturum açmış ) 

    [Authorize] //giriş yapıp yapmadığını anlıyoruz
    //startupa app.USeAuthentication(); ve app.USeAuthorization();
    //burada ne kadar action varsa hepsine girebilmek için giriş yapmış olması gerekir
    public class PanelController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public PanelController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> UpdateUSer()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            UserUpdateViewModel model = new UserUpdateViewModel
            {
                Email = user.Email,
                Name = user.Name,
                SurName = user.SurName,
                PhoneNumber = user.PhoneNumber,
                PictureUrl = user.PictureUrl
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUSer(UserUpdateViewModel model)
        {
   
            if (ModelState.IsValid)
            {
                //giriş yapmış kullanıcın ismini aldık
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                //bir dosya seçtimi seçmedi mi 
                if(model.Picture !=null)
                {
                    //özünde yaptığımız şey ilgili dosyayı bir yerden alıp başka bir yere kopyalamak
                    //path : bana gelecek olan dosyayı nereye kaydediceğim. benzersiz bir isimle gerçekleşsin.
                    //uyg/wwwroot/img/resimad.uzantı gibi bir yol vermeliyiz
                   var uygulamaninCalistigiYer= Directory.GetCurrentDirectory();//uygulamanın çalıştığı yere ulaştık
                    var uzanti = Path.GetExtension(model.Picture.FileName); //verdiğimiz yoldan uzantıyı getirir
                    var resimAd = Guid.NewGuid() + uzanti; //benzersiz olması için guid
                    var kaydedilecekYer = uygulamaninCalistigiYer + "/wwwroot/img/" + resimAd;
                    //string path= 
                   using var stream = new FileStream(kaydedilecekYer,FileMode.Create);
                    //neden asenkronik eğer bizim yaptığımız işte basitlik daha öndeyse senkronik fakat veri alma çekme gibi işlemler yani uzun süren işlerse asenkronik
                    await model.Picture.CopyToAsync(stream);
                    user.PictureUrl = resimAd;
                }
                user.Name = model.Name;
                user.SurName = model.SurName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

               var result= await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(user);
        }
        public async Task<IActionResult> LogOut()
        {
           await  _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
