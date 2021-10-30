using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Models.Users
{
    /// <summary>
    /// DTO klasa koja modelu korisnicki nalog
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Jedinstveni kljuc korisnika
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Korisnicki email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Boolean vrednost koja ukazuje da li je nalog aktivna
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
        /// Korisnicka uloga koja se koristi za autorizaciju
        /// </summary>
        public String Role { get; set; }

        /// <summary>
        /// Drzava korisnika
        /// </summary>
        public String Country { get; set; }
    }
}
