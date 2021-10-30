using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;

namespace UserService.Services.Countries
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly ICorporationUserRepository _corporationUserRepository;
        public CountryService(ICountryRepository countryRepository, ICorporationUserRepository corporationUserRepository, IPersonalUserRepository personalUserRepository)
        {
            _countryRepository = countryRepository;
            _corporationUserRepository = corporationUserRepository;
            _personalUserRepository = personalUserRepository;
        }
        public CountryCreatingConfirmation CreateCountry(Country country)
        {
            CountryCreatingConfirmation createdCountry = _countryRepository.CreateCountry(country);
            _countryRepository.SaveChanges();
            return createdCountry;
        }

        public void DeleteCountry(Guid countryId)
        {
            List<PersonalUser> persUsers = _personalUserRepository.GetUsersWithCountry(countryId);
            if (persUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of country is referenced in another table");
            }
            List<CorporationUser> corpUsers = _corporationUserRepository.GetUsersWithCountry(countryId);
            if (corpUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of country is referenced in another table");
            }
            _countryRepository.DeleteCountry(countryId);
            _countryRepository.SaveChanges();
        }

        public List<Country> GetCountries(string countryName)
        {
            return _countryRepository.GetCountries(countryName);
        }

        public Country GetCountryByCountryId(Guid countryId)
        {
            return _countryRepository.GetCountryByCountryId(countryId);
        }

        public void UpdateCountry(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
