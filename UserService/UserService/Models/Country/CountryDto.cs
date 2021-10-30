using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Country
{
    /// <summary>
    /// DTO klasa koja modelu drzavu
    /// </summary>
    public class CountryDto
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
