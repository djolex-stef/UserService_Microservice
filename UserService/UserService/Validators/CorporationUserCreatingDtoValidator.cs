using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Users;

namespace UserService.Validators
{
    public class CorporationUserCreatingDtoValidator: AbstractValidator<CorporationUserCreatingDto>
    {
        public CorporationUserCreatingDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Telephone).NotNull().NotEmpty().MaximumLength(30).MinimumLength(7);
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.CountryId).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.CorporationName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Pib).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.CorporationCity).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.CorporationAddress).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
        }
    }
}
