using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Models.Users;
using UserService.Entities;

namespace UserService.Profiles
{
    public class CorporationUserProfile: Profile
    {
        public CorporationUserProfile()
        {
            CreateMap<CorporationUser, CorporationUserDto>().ForMember(
               dest => dest.Country,
               opt => opt.MapFrom(src => $"{src.Country.CountryName}"))
               .ForMember(
               dest => dest.Role,
               opt => opt.MapFrom(src => $"{src.Role.RoleName}")
               );

            CreateMap<CorporationUser, UserInfoDto>().ForMember(
                dest => dest.AccountType,
                opt => opt.MapFrom(src => "corporationUser")).ForMember(
                dest => dest.Country,
                opt => opt.MapFrom(src => $"{src.Country.CountryName}"))
                .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => $"{src.Role.RoleName}")
                );

            CreateMap<CorporationUserCreatingDto, CorporationUser>().ForMember(
                dest => dest.CountryId,
                opt => opt.MapFrom(src => src.CountryId));
            CreateMap<CorporationUser, CorporationUserCreatingConfirmation>();
            CreateMap<CorporationUserCreatingConfirmation, CorporationUserCreatingConfirmationDto>();
            CreateMap<CorporationUserUpdateDto, CorporationUser>();
            CreateMap<CorporationUser, CorporationUser>();
        }
       
    }
}
