using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    public class UserUpdateDto
    {
        /// <summary>
        /// Boolean vrednost koja ukazuje na to da li je nalog aktivan
        /// </summary>
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Korisnicki username
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Telefon korisnika
        /// </summary>
        public String Telephone { get; set; }

        /// <summary>
        /// Strani kljuc CountryId
        /// </summary>
        public Guid CountryId { get; set; }
    }
}
