using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Roles;
using UserService.Entities;

namespace UserService.Profiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleCreatingDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<Role, RoleCreatingConfirmation>();
            CreateMap<RoleCreatingConfirmation, RoleCreatingConfirmationDto>();
            CreateMap<Role, Role>();
        }
    }
}
