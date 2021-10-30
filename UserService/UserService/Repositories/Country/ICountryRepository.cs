using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface ICountryRepository
    {
        List<Country> GetCountries(string countryName);
        Country GetCountryByCountryId(Guid countryId);
        CountryCreatingConfirmation CreateCountry(Country country);
        void UpdateCountry(Country country);
        void DeleteCountry(Guid countryId);
        bool SaveChanges();
    }
}
