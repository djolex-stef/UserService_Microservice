using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Country
{
    public class CountryCreatingConfirmationDto
    {
        /// <summary>
        /// Jedinstveni kljuc za drzavu
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Naziv drzave 
        /// </summary>
        public String CountryName { get; set; }
    }
}
