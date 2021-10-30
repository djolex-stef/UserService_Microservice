using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class CountryCreatingConfirmation
    {
        /// <summary>
        /// Jedinstveni kljuc za zemlju
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Naziv zemlje
        /// </summary>
        public String CountryName { get; set; }
    }
}
