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
            //identity aya�a kald�rmak i�in
            //Identity nerede hangi database ile �al��acak
            services.AddDbContext<UdemyContext>();

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false; //bir say� olmas� zorunlulu�unu kald�rd�k
                opt.Password.RequireLowercase = false;//k���k harf zorunlulu�u kald�rd�k
                opt.Password.RequiredLength = 1;//6 karakter zorunlulu�unu 1 yapt�k
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;


                ////lockout de�erimizi de�i�tirelim
                opt.Lockout.DefaultLockoutTimeSpan = /*TimeSpan.FromDays();birg�n  */ TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 3; //3 kere yanl�� girerse
                                                         //eposta yada i�te telefon onayland�m�?
               // opt.SignIn.RequireConfirmedEmail = true;

                //�zel validasyon
            }).AddErrorDescriber<CustomIdentityValidator>().AddPasswordValidator<CustomPasswordValidator>().AddEntityFrameworkStores<UdemyContext>();
            services.AddControllersWithViews();


            //Default olarak aya�a kalkan cookie konf. de�itiriyoruz
            services.ConfigureApplicationCookie(opt =>
            {
                //default u Account/loging
                opt.LoginPath = new PathString("/Home/Index");//yetkim olmayan bir yere gitmek istedi�imde beni giri� yapa y�nlendirecek
                //giri� ba�ar�l�ysa bizim belirledi�imiz �ekilde cookie olu�acak
                //expiration = Cookinin ne kadar ayakta kalaca��n� belirtiyor
                //httponly= ilgili cookie javascript taraf�ndan �ekilebiliyr mu


                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Name = "UdemyCookie";
                //SameSite= lax ise cookie payla�abiliriz strict subdomeinlerde dahil olmak �zere bu cookie eri�emeyiz.
                opt.Cookie.SameSite = SameSiteMode.Strict;
                //g�velik protokol� always dersek bu cookie her zaman https �zerinden �al���r, sameAsRequest ne ile �al���rsak onun �zerinden�al���r
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(20); //cookie 20 g�n ayakta kal�cak default 14



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
            app.UseStaticFiles();//wwwroot d��ar� a��ld�
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
