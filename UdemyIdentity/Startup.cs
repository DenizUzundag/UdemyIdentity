using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentity.Context;
using UdemyIdentity.CustomValidator;

namespace UdemyIdentity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //identity ayaða kaldýrmak için
            //Identity nerede hangi database ile çalýþacak
            services.AddDbContext<UdemyContext>();

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false; //bir sayý olmasý zorunluluðunu kaldýrdýk
                opt.Password.RequireLowercase = false;//küçük harf zorunluluðu kaldýrdýk
                opt.Password.RequiredLength = 1;//6 karakter zorunluluðunu 1 yaptýk
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;


                ////lockout deðerimizi deðiþtirelim
                opt.Lockout.DefaultLockoutTimeSpan = /*TimeSpan.FromDays();birgün  */ TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 3; //3 kere yanlýþ girerse
                                                         //eposta yada iþte telefon onaylandýmý?
               // opt.SignIn.RequireConfirmedEmail = true;

                //özel validasyon
            }).AddErrorDescriber<CustomIdentityValidator>().AddPasswordValidator<CustomPasswordValidator>().AddEntityFrameworkStores<UdemyContext>();
            services.AddControllersWithViews();


            //Default olarak ayaða kalkan cookie konf. deðitiriyoruz
            services.ConfigureApplicationCookie(opt =>
            {
                //default u Account/loging
                opt.LoginPath = new PathString("/Home/Index");//yetkim olmayan bir yere gitmek istediðimde beni giriþ yapa yönlendirecek
                //giriþ baþarýlýysa bizim belirlediðimiz þekilde cookie oluþacak
                //expiration = Cookinin ne kadar ayakta kalacaðýný belirtiyor
                //httponly= ilgili cookie javascript tarafýndan çekilebiliyr mu


                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Name = "UdemyCookie";
                //SameSite= lax ise cookie paylaþabiliriz strict subdomeinlerde dahil olmak üzere bu cookie eriþemeyiz.
                opt.Cookie.SameSite = SameSiteMode.Strict;
                //güvelik protokolü always dersek bu cookie her zaman https üzerinden çalýþýr, sameAsRequest ne ile çalýþýrsak onun üzerindençalýþýr
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(20); //cookie 20 gün ayakta kalýcak default 14



            });

            services.AddAuthorization(Opt =>
            {
                Opt.AddPolicy("FemalePolicy", cnf => { cnf.RequireClaim("gender", "female"); });
                ;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();//wwwroot dýþarý açýldý
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
