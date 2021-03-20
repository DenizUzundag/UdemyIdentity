using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentity.Context;

namespace UdemyIdentity.CustomValidator
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //parola kullanıcı adı içermemeli

            List<IdentityError> errors = new List<IdentityError>();
            if(password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code="PasswordContainUSerName",
                    Description="Parola kullanıcı adı içeremez"
                });;;
            }
            if(errors.Count>0)//0 dan büyükse bir hata olmuş
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            else
            {
                return Task.FromResult(IdentityResult.Success);
            }


            return Task.FromResult(IdentityResult.Failed());
        }
    }
}
