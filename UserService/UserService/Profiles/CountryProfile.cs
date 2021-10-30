using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Country;
using UserService.Entities;

namespace UserService.Profiles
{
    public class CountryProfile: Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<CountryCreatingDto, Country>();
            CreateMap<Country, CountryCreatingConfirmation>();
            CreateMap<CountryCreatingConfirmation, CountryCreatingConfirmationDto>();
            CreateMap<CountryUpdateDto, Country>();
            CreateMap<Country, Country>();
        }
    }
}
