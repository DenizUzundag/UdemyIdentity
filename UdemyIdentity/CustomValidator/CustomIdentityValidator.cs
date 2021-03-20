using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentity.CustomValidator
{//default olarak gelen hataları türkçeleştiricez.
    public class CustomIdentityValidator:IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PassworTooShort",
                Description = $"Paralo minimum {length} karakter olmalıdır"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "PArola bir alfanümerik karakter(!,~vs.) içermelidir"
            };

        }

        //username alınmış mı alınmamış mı aynı username ile kayıt olunamaz
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"ilgili kullanıcı adı({userName}) zaten sistemde kayıtlı"
            };

        }
    }
}
