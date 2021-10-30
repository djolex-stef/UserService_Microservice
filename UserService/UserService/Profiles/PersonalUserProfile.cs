using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Users;
using UserService.Entities;

namespace UserService.Profiles
{
    public class PersonalUserProfile: Profile
    {
        public PersonalUserProfile()
        {
            CreateMap<PersonalUser, PersonalUserDto>().ForMember(
                dest => dest.FirstAndLastName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                dest => dest.Country,
                opt => opt.MapFrom(src => $"{src.Country.CountryName}"))
                .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => $"{src.Role.RoleName}")
                );


                CreateMap<PersonalUser, UserInfoDto>().ForMember(
                dest => dest.AccountType,
                opt => opt.MapFrom(src => "personalUser")).ForMember(
                dest => dest.Country,
                opt => opt.MapFrom(src => $"{src.Country.CountryName}"))
                .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => $"{src.Role.RoleName}")
                );

            CreateMap<PersonalUserCreatingDto, PersonalUser>().ForMember(
                dest => dest.CountryId,
                opt => opt.MapFrom(src => src.CountryId));
            CreateMap<PersonalUser, PersonalUserCreatingConfirmation>();
            CreateMap<PersonalUserCreatingConfirmation, PersonalUserCreatingConfirmationDto>().ForMember(
                dest => dest.FirstAndLastName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<PersonalUserUpdateDto, PersonalUser>();
            CreateMap<PersonalUser, PersonalUser>();
        }
    }
}
