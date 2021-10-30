using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    /// <summary>
    /// DTO klasa koja modeluje personalni nalog
    /// </summary>
    public class PersonalUserDto: UserDto
    {
        /// <summary>
        /// Ime i prezime
        /// </summary>
        public String FirstAndLastName { get; set; }
    }
}
