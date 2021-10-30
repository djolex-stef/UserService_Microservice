using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Klasa koje modelu potvrdu o kreiranju korisnickog naloga
    /// </summary>
    public class UserCreatingConfirmation
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
