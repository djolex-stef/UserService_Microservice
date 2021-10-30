using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    public class PersonalUserCreatingConfirmationDto: UserCreatingConfirmationDto
    {
        /// <summary>
        /// Ime i prezime korisnika
        /// </summary>
        public String FirstAndLastName { get; set; }
    }
}
