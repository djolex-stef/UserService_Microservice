using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    public class UserInfoDto
    {
        /// <summary>
        /// Jedinstveni kljuc korisnika
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Informacije o tome koji je tip profila: personal ili corporational
        /// </summary>
        public String AccountType { get; set; }

        /// <summary>
        /// Korisnicki email
        /// </summary>
        public String Email { get; set; }

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
        /// Uloga koja se koristi za autorizaciju
        /// </summary>
        public String Role { get; set; }

        /// <summary>
        /// Drzava korisnika
        /// </summary>
        public String Country { get; set; }
    }
}
