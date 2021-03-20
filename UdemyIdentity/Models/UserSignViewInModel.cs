using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class UserSignViewInModel
    {
        [Display(Name ="Kullanıcı Adı:")]
        [Required(ErrorMessage ="Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }
        [Display(Name = "Sifre:")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }

        public bool rememberMe { get; set; }//beni hatırla
    }
}
