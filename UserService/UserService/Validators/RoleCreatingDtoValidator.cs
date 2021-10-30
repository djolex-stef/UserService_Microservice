using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Roles;

namespace UserService.Validators
{
    public class RoleCreatingDtoValidator: AbstractValidator<RoleCreatingDto>
    {
        public RoleCreatingDtoValidator()
        {
            RuleFor(x => x.RoleName).NotNull().NotEmpty().MaximumLength(50).MinimumLength(3);
        }
    }
}
