using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class PersonalUserCreatingConfirmation: UserCreatingConfirmation
    {
        /// <summary>
        /// Ime korisnika
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Prezime korisnika
        /// </summary>
        public String LastName { get; set; }
    }
}
