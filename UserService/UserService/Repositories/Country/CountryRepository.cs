using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class CountryRepository : ICountryRepository
    {
        private readonly UserDbContext context;
        private readonly IMapper mapper;

        public CountryRepository(UserDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public CountryCreatingConfirmation CreateCountry(Country country)
        {
            var createdCountry = context.Add(country);
            return mapper.Map<CountryCreatingConfirmation>(createdCountry.Entity);
        }

        public void DeleteCountry(Guid countryId)
        {
            var country = GetCountryByCountryId(countryId);
            context.Remove(country);
        }

        public List<Country> GetCountries(string countryName)
        {
            return context.Country.Where(c => countryName == null || c.CountryName == countryName).ToList();
        }

        public Country GetCountryByCountryId(Guid countryId)
        {
            return context.Country.FirstOrDefault(e => e.CountryId == countryId);

        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateCountry(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
