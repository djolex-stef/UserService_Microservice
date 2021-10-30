using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    /// <summary>
    /// DTO klasa koja modeluje potvrdu o kreiranju korisnika
    /// </summary>
    public class UserCreatingConfirmationDto
    {
        /// <summary>
        /// Jedinstveni kljuc naloga
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Korisnicki email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Korisnicki username
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Telefon korisnika
        /// </summary>
        public String Telephone { get; set; }
    }
}
