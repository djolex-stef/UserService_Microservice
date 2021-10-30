using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Users;

namespace UserService.Validators
{
    public class CorporationUserUpdateDtoValidator: AbstractValidator<CorporationUserUpdateDto>
    {
        public CorporationUserUpdateDtoValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Telephone).NotNull().NotEmpty().MaximumLength(30).MinimumLength(7);
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.CountryId).NotNull().NotEmpty();
            RuleFor(x => x.CorporationName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Pib).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.CorporationCity).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.CorporationAddress).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
        }        
    }
}
