using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.Models
{
    public class RoleViewModel
    {
        [Display(Name ="Ad")]
        [Required(ErrorMessage ="Ad alanı gereklidir")]
        public string Name { get; set; }
    }
}
